using System.ComponentModel.DataAnnotations;

namespace MovieProjectWithUmbraco.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}