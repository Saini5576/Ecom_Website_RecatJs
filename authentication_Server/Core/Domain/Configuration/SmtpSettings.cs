using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Configuration;
public class SmtpSettings
{
    public string PrimaryDomain { get; init; } = null!;
    public int PrimaryPort { get; init; } = 0;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string DefaultFromEmail { get; init; } = null!;
}
