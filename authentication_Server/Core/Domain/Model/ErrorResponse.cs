using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ErrorResponse
    {
        public string? Title { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
