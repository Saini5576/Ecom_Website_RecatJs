using Domain.BaseResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Account.Query
{
    public record DeleteRoleQuery([Required]string deleteRoleId) : IRequest<Response>;
}
