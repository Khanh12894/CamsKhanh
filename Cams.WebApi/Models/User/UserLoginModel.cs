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
    public partial class UserLoginModel
    {
        [Required]
        //[EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int? ClientId { get; set; }
    }
}

