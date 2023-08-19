using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace XichLip.WebApi.Models.User
{
    /// <summary>
    /// User register model
    /// </summary>
    /// Created By: KietNQ
    /// Created Time: 2019/09/04
    /// Updated By: 
    /// Updated Time:
    [XmlRoot("User")]
    public partial class UserChangePasswordModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} và nhiều nhất {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} và nhiều nhất {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp, vui lòng kiểm tra lại")]
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
    }

    public class UserForgotPasswordModel
    {
        public long UserId { set; get; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} và nhiều nhất {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp, vui lòng kiểm tra lại")]
        public string ConfirmPassword { get; set; }

        public string Code { set; get; }
    }

    public class ForgotPasswordModel
    {
        public string Email { set; get; }
        public string Url { set; get; }
    }

    public class UserResetPasswordModel
    {
        public long UserId { set; get; }
        public string Password { set; get; }
    }

    public partial class UserChangePasswordByUserNameModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} và nhiều nhất {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} và nhiều nhất {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp, vui lòng kiểm tra lại")]
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
    }
}

