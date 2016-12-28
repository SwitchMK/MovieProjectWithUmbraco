using System;
using System.ComponentModel.DataAnnotations;

namespace MovieProjectWithUmbraco.Models
{
    public class Feedback
    {
        public string Publisher { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [MaxLength(400, ErrorMessage = "The comment message must not exceed 400 characters.")]
        public string Content { get; set; }
        public DateTime DateOfPublication { get; set; }
    }
}