using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class EmailConfirmedDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter valid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; }
    }
}
