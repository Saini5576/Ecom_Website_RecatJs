using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "Refresh token is required.")]
        [StringLength(1000, MinimumLength = 32, ErrorMessage = "Refresh token must be between 32 and 1000 characters.")]
        public string RefreshToken { get; set; }
    }
}
