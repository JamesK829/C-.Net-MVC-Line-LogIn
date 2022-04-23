using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AshaPortal
{
    public class CheckPhone
    {
        [RegularExpression("^[0-9]+$", ErrorMessage = "僅能有數字")]
        [StringLength(10)]
        public string phone { get; set; }
        [StringLength(20, ErrorMessage = "密碼最多20個字")]
        [RegularExpression("^[A-Za-z0-9]+$", ErrorMessage = "密碼僅能有英文跟數字")]
        [DataType(DataType.Password)]
        public string auth { get; set; }
    }
    public class LoginViewModel
    {
        [DisplayName("登入帳號")]
        [Required(ErrorMessage = "請輸入登入帳號")]
        [StringLength(40, ErrorMessage = "登入帳號最多20個字")]
        public string Cid { get; set; }

        [DisplayName("登入密碼")]
        [Required(ErrorMessage = "請輸入登入密碼")]
        [StringLength(20, ErrorMessage = "密碼最多20個字")]
        [RegularExpression("^[A-Za-z0-9]+$",ErrorMessage="密碼僅能有英文跟數字")]
        [DataType(DataType.Password)]
        public string Auth { get; set; }
    }
    public class LineLoginToken
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
    }

    public class LineProfile
    {
        public string userId { get; set; }
        public string displayName { get; set; }
        public string pictureUrl { get; set; }
        public string statusMessage { get; set; }
    }

    public class LineUserProfile
    {
        public string userId { get; set; }
        public string displayName { get; set; }
        public string pictureUrl { get; set; }
        public string statusMessage { get; set; }
        public string email { get; set; }
    }

    public class CustomerForm
    {
        [Required(ErrorMessage = "請輸入姓名")]
        [StringLength(16)]
        public string Name { get; set; }
        public string Auth { get; set; }
        public string ConfirmAuth { get; set; }
        [Required(ErrorMessage = "請輸入生日")]
        public DateTime BDay { get; set; }
        [Required(ErrorMessage = "請輸入手機號碼")]
        [RegularExpression("^09[0-9]{8}$", ErrorMessage = "手機號格式不正確")]
        [StringLength(10)]
        public string Mobile { get; set; }
        public string LineId { get; set; }
        public string Line_Title { get; set; }
        public string Avatar { get; set; }
    }

    public class HomeVIewModel
    {
        public string CId { get; set; }
    }

}