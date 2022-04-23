using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Asha.Service;
using Asha.Model;
using System.Web.Security;
using Asha.Util;
using System.DirectoryServices;

namespace AshaAdmin.Controllers
{
    public class LoginController : Controller
    {
        ManagerService managerService = new ManagerService();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (SysConstants.IsTestMode == "Y")
                    {
                        LogUtil.Info(MethodBase.GetCurrentMethod(), "UID:" + form.Cid);

                        #region DB

                        try
                        {
                            //CheckDB

                            bool ThisUserRoles = managerService.CheckUser(form.Cid,form.Auth);
                            MANAGER ThisUser = managerService.GetMenager(form.Cid, form.Auth);
                            if (ThisUser == null)
                            {
                                LogUtil.Info(MethodBase.GetCurrentMethod(), "NoUID:" + form.Cid);
                                TempData["Message"] = MessageConstants.DATA_NOT_FOUND;
                            }
                            else
                            {
                                //Login
                                bool isPersistent = false;//如果票證將存放於持續性 Cookie 中 (跨瀏覽器工作階段儲存)，則為 true，否則為 false。 如果票證是存放於 URL 中，則忽略這個值。
                                string userData = string.Format("{0}_{1}", ThisUser.CID, ThisUser.NAME);//可放使用者自訂的內容
                                LogUtil.Info(MethodBase.GetCurrentMethod(), "UID:" + ThisUser.CID + " 登入成功!!!");
                                //寫cookie
                                //使用 Cookie 名稱、版本、到期日、核發日期、永續性和使用者特定的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。 此 Cookie 路徑設定為在應用程式的組態檔中建立的預設值。
                                //使用 Cookie 名稱、版本、目錄路徑、核發日期、到期日期、永續性和使用者定義的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                    ThisUser.CID,//使用者名稱(@User.Identity.Name)
                                    DateTime.Now,//核發日期
                                    DateTime.Now.AddMinutes(SysConstants.LoginTimeout),//到期日期 60分鐘 
                                    isPersistent,//永續性
                                    userData,//使用者定義的資料
                                    FormsAuthentication.FormsCookiePath);

                                string encTicket = FormsAuthentication.Encrypt(ticket);
                                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                                Session[SysConstants.USER_ROLE] = ThisUser.ROLE;
                                return RedirectToAction("Index", "Index");

                            }
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error(MethodBase.GetCurrentMethod(), ex);
                        }

                        #endregion
                    }
                    else
                    {
                        LogUtil.Info(MethodBase.GetCurrentMethod(), "Uid:" + form.Cid);

                        #region LDAP
                        DirectoryEntry entry = new DirectoryEntry("LDAP://" + SysConstants.LDAPDomainName, form.Cid, form.Auth);
                        DirectorySearcher mySearcher = new DirectorySearcher(entry);

                        try
                        {
                            SearchResult result = mySearcher.FindOne();

                            //CheckDB
                            //List<FSITC_MANAGER_ROLE> ThisUserRoles = managerService.GetRoleList(form.Uid);
                            MANAGER ThisUser = managerService.GetMenager(form.Cid, form.Auth);
                            //List<FSITC_MANAGER_ROLE> UserRole = managerService.GetRoleList(ThisUser.ID);
                            if (ThisUser == null)
                            {
                                LogUtil.Info(MethodBase.GetCurrentMethod(), "NoUID:" + form.Cid);
                                TempData["Message"] = MessageConstants.DATA_NOT_FOUND;
                            }
                            else if (ThisUser.STATUS == SysConstants.DISABLE_STATUS)
                            {
                                LogUtil.Info(MethodBase.GetCurrentMethod(), "UID:" + form.Cid + "Status:0");
                                TempData["Message"] = MessageConstants.INCORRECT;
                            }
                            else
                            {
                                bool isPersistent = false;//如果票證將存放於持續性 Cookie 中 (跨瀏覽器工作階段儲存)，則為 true，否則為 false。 如果票證是存放於 URL 中，則忽略這個值。
                                string userData = string.Format("{0}_{1}", ThisUser.ID, ThisUser.CID);//可放使用者自訂的內容
                                LogUtil.Info(MethodBase.GetCurrentMethod(), "UID:" + ThisUser.CID + " 登入成功!!!");
                                //寫cookie
                                //使用 Cookie 名稱、版本、到期日、核發日期、永續性和使用者特定的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。 此 Cookie 路徑設定為在應用程式的組態檔中建立的預設值。
                                //使用 Cookie 名稱、版本、目錄路徑、核發日期、到期日期、永續性和使用者定義的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                    ThisUser.NAME,//使用者名稱(@User.Identity.Name)
                                    DateTime.Now,//核發日期
                                    DateTime.Now.AddMinutes(SysConstants.LoginTimeout),//到期日期 60分鐘 
                                    isPersistent,//永續性
                                    userData,//使用者定義的資料
                                    FormsAuthentication.FormsCookiePath);

                                string encTicket = FormsAuthentication.Encrypt(ticket);
                                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                                Session[SysConstants.USER_ROLE] = ThisUser.ROLE;
                                return RedirectToAction("Index", "Index");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error(MethodBase.GetCurrentMethod(), "LoginIndex", ex);
                            // TempData["Message"] = MessageConstants.LOGIN_FAILURE;
                        }
                        finally
                        {
                            mySearcher.Dispose();
                            entry.Close();
                            entry.Dispose();
                        }
                        #endregion
                    }
                }
            }
            catch (Exception e)
            {
                //  TempData["Message"] = MessageConstants.FAILURE;
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
    }
}