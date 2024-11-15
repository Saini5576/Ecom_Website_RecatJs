using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configuration;
public class JwtKeyConfiguration
{    
    public string SecretKey { get; init; } = null!;
    public int ExpiryMinutes { get; init; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
}
