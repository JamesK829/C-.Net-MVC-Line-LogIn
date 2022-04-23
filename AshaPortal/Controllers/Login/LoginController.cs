using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using PortalService;
using Asha.Model;
using System.Web.Security;
using Asha.Util;
using System.DirectoryServices;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using Newtonsoft.Json;

namespace AshaPortal
{
    public class LoginController : Controller
    {
        CustomerService service = new CustomerService();

        public ActionResult Index()
        {
            var model = new CheckPhone();
            return View(model);

        }

        public ActionResult PhoneLogIn()
        {
            var model = new CheckPhone();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PhoneLogIn(CheckPhone form)
        {
            try
            {
                var checkData = new CUSTOMER();
                bool checkPhone = false;
                checkData = service.CheckData(form.phone, form.auth);
                checkPhone = service.CheckPhone(form.phone);
                if (checkPhone == true)
                {
                    LogUtil.Info(MethodBase.GetCurrentMethod(), "PhoneNum :" + form.phone);

                    if (checkData != null)
                    {
                        bool isPersistent = false;//如果票證將存放於持續性 Cookie 中 (跨瀏覽器工作階段儲存)，則為 true，否則為 false。 如果票證是存放於 URL 中，則忽略這個值。
                        string userData = string.Format("{0}_{1}", checkData.ID, checkData.NAME);//可放使用者自訂的內容
                        LogUtil.Info(MethodBase.GetCurrentMethod(), "UID:" + checkData.MOBILE + " 登入成功!!!");
                        //寫cookie
                        //使用 Cookie 名稱、版本、到期日、核發日期、永續性和使用者特定的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。 此 Cookie 路徑設定為在應用程式的組態檔中建立的預設值。
                        //使用 Cookie 名稱、版本、目錄路徑、核發日期、到期日期、永續性和使用者定義的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                            checkData.NAME,//使用者名稱(@User.Identity.Name)
                            DateTime.Now,//核發日期
                            DateTime.Now.AddMinutes(SysConstants.LoginTimeout),//到期日期 60分鐘 
                            isPersistent,//永續性
                            userData,//使用者定義的資料
                            FormsAuthentication.FormsCookiePath);

                        string encTicket = FormsAuthentication.Encrypt(ticket);
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                        return RedirectToAction("Home", "Reserve");
                    }
                    else
                    {
                        TempData["Message"] = "密碼輸入不正確";

                        return RedirectToAction("LoginViaPhone", "Reserve");
                    }
                }
                else
                {
                    TempData["Message"] = "此手機尚未註冊為會員";
                    return RedirectToAction("SignUp", "Login");
                }

            }
            catch (Exception e)
            {
                TempData["Message"] = e.ToString();

                LogUtil.Error(MethodBase.GetCurrentMethod(), e);
            }
            return View(form);
        }

        public ActionResult Logout(bool timeout = false)
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Abandon();

                // clear authentication cookie
                HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                cookie1.Expires = DateTime.Now.AddYears(-1);
                cookie1.HttpOnly = true;
                Response.Cookies.Add(cookie1);

                // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
                cookie2.HttpOnly = true;
                cookie2.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(cookie2);

                //FormsAuthentication.RedirectToLoginPage();
                TempData["Message"] = (timeout ? "操作已逾時" : "帳號已登出") + "，請重新登入！";
                //return PartialView("LoginFail");
                return RedirectToAction("Index", "Login", new { t = timeout });
            }
            catch (Exception e)
            {
                LogUtil.Error(MethodBase.GetCurrentMethod(), "LogOut", e);
            }
            return RedirectToAction("Index", "Login", new { t = timeout });

        }

        public ActionResult Timeout()
        {
            return Logout(true);
        }

        public ActionResult SignUp(string phone, string id, string name, string pic)
        {
            var model = new CustomerForm();
            if (!string.IsNullOrWhiteSpace(phone))
            {
                model.Mobile = phone;
            }
            if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(name))
            {
                model.LineId = id;
                model.Line_Title = name;
                model.Avatar = pic;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(CustomerForm form)
        {
            var model = new CUSTOMER();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(form);
                }
                model.LINE_ID = form.LineId;
                model.NAME = form.Name;
                model.MOBILE = form.Mobile;
                model.BIRTHDAY = form.BDay;
                model.CREATOR = form.Name;
                model.MODIFIER = form.Name;
                model.AUTH = form.Auth;
                model.CREATE_TIME = DateTime.Now;
                model.MODIFY_TIME = DateTime.Now;
                model.STATUS = SysConstants.ENABLE_STATUS;
                model.LINE_NAME = form.Line_Title;

                model.ID = service.CreateCustomerData(model);
                if (!string.IsNullOrEmpty(model.ID))
                {
                    bool isPersistent = false;//如果票證將存放於持續性 Cookie 中 (跨瀏覽器工作階段儲存)，則為 true，否則為 false。 如果票證是存放於 URL 中，則忽略這個值。
                    string userData = string.Format("{0}_{1}", model.ID, model.NAME);//可放使用者自訂的內容
                    LogUtil.Info(MethodBase.GetCurrentMethod(), "UID:" + model.MOBILE + " 登入成功!!!");
                    //寫cookie
                    //使用 Cookie 名稱、版本、到期日、核發日期、永續性和使用者特定的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。 此 Cookie 路徑設定為在應用程式的組態檔中建立的預設值。
                    //使用 Cookie 名稱、版本、目錄路徑、核發日期、到期日期、永續性和使用者定義的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        model.NAME,//使用者名稱(@User.Identity.Name)
                        DateTime.Now,//核發日期
                        DateTime.Now.AddMinutes(SysConstants.LoginTimeout),//到期日期 60分鐘 
                        isPersistent,//永續性
                        userData,//使用者定義的資料
                        FormsAuthentication.FormsCookiePath);

                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                }
            }
            catch (Exception e)
            {
                LogUtil.Error(MethodBase.GetCurrentMethod(), e);
            }
            return RedirectToAction("Home", "Reserve");
        }

    }
}