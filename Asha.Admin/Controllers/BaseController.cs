using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Asha.Model;
using Asha.Service;
using Asha.Util;

namespace AshaAdmin.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        ManagerService service = new ManagerService();

        public string GetLoginUserUID()
        {
            try
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket orgTicket = id.Ticket;
                var userData = orgTicket.UserData.Split('_');
                var Uid = userData[1];

                return Uid;
            }
            catch (Exception e)
            {
                LogUtil.Error(System.Reflection.MethodBase.GetCurrentMethod(), "GetLoginUserUID", e);
            }
            return "";

        }

        public string GetLoginUserID()
        {
            try
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                FormsAuthenticationTicket orgTicket = id.Ticket;
                var userData = orgTicket.UserData.Split('_');
                var Id = userData[0];

                return Id;
            }
            catch (Exception e)
            {
                LogUtil.Error(System.Reflection.MethodBase.GetCurrentMethod(), "GetLoginUserID", e);
            }
            return "";

        }


        public ActionResult Menu()
        {
            try
            {
                var userId = GetLoginUserID();
                if (userId != null)
                {
                    return PartialView("Menu");
                }
                return new EmptyResult();
            }
            catch (Exception e)
            {
                LogUtil.Error(MethodBase.GetCurrentMethod(), "Menu", e);
            }
            return new EmptyResult();
        }
    }
}