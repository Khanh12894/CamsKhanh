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
    public partial class UserUpdateModel
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        public string UserName { get; set; }
        [Display(Name = "Số điện thoại")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Số điện thoại không chính xác")]
        public string PhoneNumber { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp, vùi lòng kiểm tra lại.")]
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; }
    }
}

