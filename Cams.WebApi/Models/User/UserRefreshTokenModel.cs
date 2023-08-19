using System.ComponentModel.DataAnnotations;

namespace XichLip.WebApi.Models.Account.User
{
    public class UserRefreshTokenModel
    {
        [Required(ErrorMessage = "required_field")]
        public string Token { get; set; }
        [Required(ErrorMessage = "required_field")]
        public string RefreshToken { get; set; }
    }
}
