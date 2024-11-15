using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BaseResponse;
public record TokenResponse(
     string Token = default!,
     DateTime Expiration = default
    );
