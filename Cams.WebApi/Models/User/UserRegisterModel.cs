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
    public partial class UserRegisterModel
    {       
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất {2} và nhiều nhất {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Birthday { get; set; }
        public int UserType { get; set; }
        public string Email { get; set; }
        public string SsoIdentity { get; set; }
        public string OTP { get; set; }
    }
}

