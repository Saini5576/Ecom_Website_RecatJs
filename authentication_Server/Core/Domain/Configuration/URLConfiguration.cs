using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configuration;
public record class URLConfiguration
{
    public string? FrontEndBaseURL { get; set; }
}
