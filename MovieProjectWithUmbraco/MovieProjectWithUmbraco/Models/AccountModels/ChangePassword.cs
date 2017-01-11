using System.ComponentModel.DataAnnotations;

namespace MovieProjectWithUmbraco.Models
{
    public class ChangePassword
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match!")]
        public string ConfirmPassword { get; set; }
    }
}