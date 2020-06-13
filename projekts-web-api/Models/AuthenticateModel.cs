using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class AuthenticateModel
    {
        [Required(ErrorMessage = "Wrong username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Wrong password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}