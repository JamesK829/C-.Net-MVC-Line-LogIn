using Asha.Util;
using AshaPortal;
using Newtonsoft.Json;
using PortalService;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Asha.Admin.Controllers.Login
{
    public class LineLoginController : Controller
    {
        CustomerService service = new CustomerService();

        string client_id = SysConstants.Client_ID;
        string client_auth = SysConstants.Client_Auth;

        /// <summary>
        /// 產生新的LineLoginUrl
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLineLoginUrl()
        {
            if (Request.IsAjaxRequest() == false)
            {
                return Content("");
            }
            //只讓本機Ajax讀取LineLoginUrl

            //state使用隨機字串比較安全
            //每次Ajax Request都產生不同的state字串，避免駭客拿固定的state字串將網址掛載自己的釣魚網站獲取用戶的Line個資授權(CSRF攻擊)
            string state = Guid.NewGuid().ToString();
            TempData["state"] = state;//利用TempData被取出資料後即消失的特性，來防禦CSRF攻擊
                                      //如果是ASP.net Form，就改成放入Session或Cookie，之後取出資料時再把Session或Cookie設為null刪除資料
            string redirect_uri = SysConstants.Redirect_Uri;
            if (SysConstants.IsTestMode.Equals("Y"))
            {
                redirect_uri = SysConstants.Redirect_Uri_Test;
            }
            string LineLoginUrl =
             $@"https://access.line.me/oauth2/v2.1/authorize?response_type=code&client_id={client_id}&redirect_uri={redirect_uri}&state={state}&scope={HttpUtility.UrlEncode("openid profile email")}";
            //scope給openid是程式為了抓id_token用，設email則為了id_token的Payload裡才會有用戶的email資訊
            return Content(LineLoginUrl);

        }

        /// <summary>
        /// 使用者在Line網頁登入後的處理，接收Line傳遞過來的參數
        /// </summary>
        /// <param name="state"></param>
        /// <param name="code"></param>
        /// <param name="error"></param>
        /// <param name="error_description"></param>
        /// <returns></returns>
        public ActionResult AfterLineLogin(string state, string code, string error, string error_description)
        {
            try
            {
                string redirect_uri = SysConstants.Redirect_Uri;//此URL為line 官方設定的Callback URL,本機測試時請用自己的localhost,發布後測試用正式URL,line官方CallbackURL也要改
                if (SysConstants.IsTestMode.Equals("Y"))
                {
                    redirect_uri = SysConstants.Redirect_Uri_Test;
                }
                if (!string.IsNullOrEmpty(error))
                {//用戶沒授權你的LineApp
                    ViewBag.error = error;
                    ViewBag.error_description = error_description;
                    return View();
                }

                if (TempData["state"] == null)
                {//可能使用者停留Line登入頁面太久

                    return Content("頁面逾期");
                }

                if (Convert.ToString(TempData["state"]) != state)
                {//使用者原先Request QueryString的TempData["state"]和Line導頁回來夾帶的state Querystring不一樣，可能是parameter tampering或CSRF攻擊

                    return Content("state驗證失敗");
                }

                if (Convert.ToString(TempData["state"]) == state)
                {//state字串驗證通過

                    //取得id_token和access_token:https://developers.line.biz/en/docs/line-login/web/integrate-line-login/#spy-getting-an-access-token
                    string issue_token_url = "https://api.line.me/oauth2/v2.1/token";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(issue_token_url);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    //必須透過ParseQueryString()來建立NameValueCollection物件，之後.ToString()才能轉換成queryString
                    NameValueCollection postParams = HttpUtility.ParseQueryString(string.Empty);
                    postParams.Add("grant_type", "authorization_code");
                    postParams.Add("code", code);
                    postParams.Add("redirect_uri", redirect_uri);
                    postParams.Add("client_id", this.client_id);
                    postParams.Add("client_secret", this.client_auth);

                    //要發送的字串轉為byte[] 
                    byte[] byteArray = Encoding.UTF8.GetBytes(postParams.ToString());
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(byteArray, 0, byteArray.Length);
                    }//end using

                    //API回傳的字串
                    string responseStr = "";
                    //發出Request
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            responseStr = sr.ReadToEnd();
                        }//end using  
                    }

                    LineLoginToken tokenObj = JsonConvert.DeserializeObject<LineLoginToken>(responseStr);
                    string id_token = tokenObj.id_token;

                    //方案總管>參考>右鍵>管理Nuget套件 搜尋 System.IdentityModel.Tokens.Jwt 來安裝
                    var jst = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(id_token);
                    LineUserProfile user = new LineUserProfile();
                    //↓自行決定要從id_token的Payload中抓什麼user資料
                    user.userId = jst.Payload.Sub;
                    user.displayName = jst.Payload["name"].ToString();
                    user.pictureUrl = jst.Payload["picture"].ToString();
                    //if (jst.Payload.ContainsKey("email") && !string.IsNullOrEmpty(Convert.ToString(jst.Payload["email"])))
                    //{//有包含email，使用者有授權email個資存取，並且用戶的email有值
                    //    user.email = jst.Payload["email"].ToString();
                    //}


                    string access_token = tokenObj.access_token;
                    ViewBag.access_token = access_token;
                    //#region 接下來是為了抓用戶的statusMessage狀態消息，如果你不想要可以省略不發出下面的Request

                    ////Social API v2.1 Getting user profiles
                    ////https://developers.line.biz/en/docs/social-api/getting-user-profiles/
                    ////取回User Profile
                    //string profile_url = "https://api.line.me/v2/profile";


                    //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(profile_url);
                    //req.Headers.Add("Authorization", "Bearer " + access_token);
                    //req.Method = "GET";
                    ////API回傳的字串
                    //string resStr = "";
                    ////發出Request
                    //using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                    //{
                    //    using (StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
                    //    {
                    //        resStr = sr.ReadToEnd();
                    //    }//end using  
                    //}

                    //LineUserProfile userProfile = JsonConvert.DeserializeObject<LineUserProfile>(resStr);
                    //user.statusMessage = userProfile.statusMessage;//補上狀態訊息

                    //#endregion

                    ViewBag.user = JsonConvert.SerializeObject(user, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        Formatting = Formatting.Indented
                    });

                    var name = user.displayName;
                    var id = user.userId;


                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(id))
                    {
                        var isCustomer = service.CheckLineId(id, name);
                        if (isCustomer != null)
                        {

                            bool isPersistent = false;//如果票證將存放於持續性 Cookie 中 (跨瀏覽器工作階段儲存)，則為 true，否則為 false。 如果票證是存放於 URL 中，則忽略這個值。
                            string userData = string.Format("{0}_{1}", isCustomer.ID, isCustomer.NAME);//可放使用者自訂的內容
                            LogUtil.Info(MethodBase.GetCurrentMethod(), "UID:" + isCustomer.MOBILE + " 登入成功!!!");
                            //寫cookie
                            //使用 Cookie 名稱、版本、到期日、核發日期、永續性和使用者特定的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。 此 Cookie 路徑設定為在應用程式的組態檔中建立的預設值。
                            //使用 Cookie 名稱、版本、目錄路徑、核發日期、到期日期、永續性和使用者定義的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。
                            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                isCustomer.NAME,//使用者名稱(@User.Identity.Name)
                                DateTime.Now,//核發日期
                                DateTime.Now.AddMinutes(SysConstants.LoginTimeout),//到期日期 60分鐘 
                                isPersistent,//永續性
                                userData,//使用者定義的資料
                                FormsAuthentication.FormsCookiePath);

                            string encTicket = FormsAuthentication.Encrypt(ticket);
                            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                            return RedirectToAction("Home", "Reserve");

                        }
                        else if (isCustomer == null)
                        {
                            return RedirectToAction("SignUp", "Login", new { id = id, name = name, pic = user.pictureUrl });

                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(MethodBase.GetCurrentMethod(), e);
            }
            return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// 徹銷Line Login，目前感覺不出差別在哪= =a，等待API改版
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RevokeLineLoginUrl(string access_token)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://api.line.me/oauth2/v2.1/revoke");
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            //必須透過ParseQueryString()來建立NameValueCollection物件，之後.ToString()才能轉換成queryString
            NameValueCollection postParams = HttpUtility.ParseQueryString(string.Empty);
            postParams.Add("access_token", access_token);
            postParams.Add("client_id", this.client_id);
            postParams.Add("client_secret", this.client_auth);


            //要發送的字串轉為byte[] 
            byte[] byteArray = Encoding.UTF8.GetBytes(postParams.ToString());
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
            }//end using

            //API回傳的字串
            string responseStr = "";
            //發出Request
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseStr = sr.ReadToEnd();
                }//end using  
            }
            return Content(responseStr);
        }

    }
}