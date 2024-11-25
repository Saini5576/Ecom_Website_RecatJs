using Application.Features.Account.Command;
using Application.Features.Account.Query;
using Azure;
using Domain.BaseResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authentication.Controllers.v1
{
    //[Authorize(Roles = "test")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseApiController
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("add-role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRole([FromBody] CreateRoleCommand createRoleCommand)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(createRoleCommand));
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        }

        [HttpPost("assign-role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleCommand assignRoleCommand)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(assignRoleCommand));
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        }
        [HttpGet("get-role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetRole()
        {
            if (ModelState.IsValid)
            {
                GetAllRolesQuery getAllRoles = new GetAllRolesQuery();   
                return Ok(await _mediator.Send(getAllRoles));
            }
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        }
        [HttpDelete("delete-role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteRole(DeleteRoleQuery deleteRoleQuery)
        {
            if (ModelState.IsValid)
                return Ok(await _mediator.Send(deleteRoleQuery));
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
