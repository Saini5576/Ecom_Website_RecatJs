﻿using Domain.BaseResponse;
using Domain.DTO;
using Domain.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Command
{
    public record LoginCommand (LoginDto Login) : IRequest<Response<LoginResponse>>;
}