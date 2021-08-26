using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.AppUsers.Commands;
using UrlShortener.Dtos;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    public class AccountController : ApiControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(AuthorizeUserCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(CreateUserCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        [HttpDelete("delete-user")]
        public async Task<ActionResult> DeleteUser(DeleteUserCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}
