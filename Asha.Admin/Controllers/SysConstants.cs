using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AshaAdmin
{
    public class SysConstants
    {
        public static string IsTestMode = (string)ConfigurationManager.AppSettings["IsTestMode"];
        public static string ImageUrl = (string)ConfigurationManager.AppSettings["Image.Url"];
        public static string UploadPath = (string)ConfigurationManager.AppSettings["Upload.Path"];
        public static string LDAPDomainName = ConfigurationManager.AppSettings["LDAPDomainName"];

        /** 預設頁碼 **/
        public const int DAFAULT_PAGEITEM = 1;
        /** 預設筆數 **/
        public const int DAFAULT_PAGESIZE = 20;
        public static int[] PageSizeList = { 50, 100, 150, 200 };
        /** Login User Session Key **/
        public const string LOGIN_USER = "LoginUser";
        /** Login User Menu Session Key **/
        public const string USER_MENU = "UserMenu";
        public const string LOGIN_UNITS = "AdminLoginUnits";
        public const string USER_ROLE = "UserRole";
        public const int LoginTimeout = 60;

        /**上傳檔案類型限制**/
        public static string[] SupportedTypeImage = { "jpg", "jpeg", "png" };
        public static string[] SupportedTypeFile = { "xls", "xlsx", "xlsm" };
        public static string SupportedPropImage = "上傳的檔案必須是格式(jpg、jpeg、png)";
        public static string SupportedPropFile = "上傳的檔案必須是格式(excel)";

        /**狀態選項**/
        public const string ENABLE_STATUS = "1";
        public const string DISABLE_STATUS = "0";
        public static Dictionary<string, string> StatusOption = new Dictionary<string, string> {
            { ENABLE_STATUS, "啟用" },
            { DISABLE_STATUS, "停用" }
        };
    }
}