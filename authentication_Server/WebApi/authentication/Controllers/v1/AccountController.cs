using Application.Features.Account.Command;
using Application.Features.Authentication.Command;
using Domain.BaseResponse;
using Domain.DTO;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
             _mediator = mediator;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(Response))]
        public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(registerCommand));
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

        }
        
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(Response))]
        public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(loginCommand));
            return BadRequest(ModelState.Values.SelectMany(v=>v.Errors.Select(e=> e.ErrorMessage)));
        }

        [HttpPost("generate-accessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(Response))]
        public async Task<IActionResult> GenerateAccessToken([FromBody] GenerateAccessTokenCommand refreshTokenCommand)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(refreshTokenCommand));
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        }
        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(Response))]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand forgotPasswordCommand)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(forgotPasswordCommand));
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
            }
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(Response))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand resetPassword)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(resetPassword));
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        }

        [HttpPost("email-confirmation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(Response))]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmedCommand emailConfirmed)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(emailConfirmed));
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
