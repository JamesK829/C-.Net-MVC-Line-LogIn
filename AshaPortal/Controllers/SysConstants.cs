using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AshaPortal
{
    public class SysConstants
    {
        public static string IsTestMode = (string)ConfigurationManager.AppSettings["IsTestMode"];
        public static string LDAPDomainName = ConfigurationManager.AppSettings["LDAPDomainName"];
        public static string Redirect_Uri_Test = ConfigurationManager.AppSettings["Redirect_Uri_Test"];
        public static string Redirect_Uri = ConfigurationManager.AppSettings["Redirect_Uri"];
        public static string Client_ID = ConfigurationManager.AppSettings["Client_ID"];
        public static string Client_Auth = ConfigurationManager.AppSettings["Client_Auth"];

        public const string USER_ROLE = "UserRole";
        public const int LoginTimeout = 60;

        /**狀態選項**/
        public const string ENABLE_STATUS = "1";
        public const string DISABLE_STATUS = "0";
        public static Dictionary<string, string> StatusOption = new Dictionary<string, string> {
            { ENABLE_STATUS, "啟用" },
            { DISABLE_STATUS, "停用" }
        };

        /// <summary>alter訊息用 TempData Key</summary>
        public static readonly string ALTER_MESSAGE = "ALTER_MESSAGE";


    }
}