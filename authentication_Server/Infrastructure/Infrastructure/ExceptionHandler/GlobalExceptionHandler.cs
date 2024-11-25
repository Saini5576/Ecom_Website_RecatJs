using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.ExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";
            ErrorResponse errorDetails = new() { Message = exception.Message ?? exception?.InnerException?.Message };

            (httpContext.Response.StatusCode) = (errorDetails.StatusCode) = exception switch
            {
                OperationCanceledException => StatusCodes.Status408RequestTimeout,
                NotImplementedException => StatusCodes.Status501NotImplemented,
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                Exception => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException(),
            };
            await httpContext.Response.WriteAsJsonAsync(errorDetails);
            return true;
        }
    }
}
