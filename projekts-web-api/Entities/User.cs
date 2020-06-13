using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities

{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}