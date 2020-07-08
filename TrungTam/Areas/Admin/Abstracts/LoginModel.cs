using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrungTam.Areas.Admin.Abstracts
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Bạn phải nhập tên đăng nhập!!!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu!!!")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}