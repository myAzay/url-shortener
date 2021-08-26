using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Dtos;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.AppUsers.Commands
{
    public class AuthorizeUserCommand : IRequest<UserDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class AuthorizeUserCommandHandler : IRequestHandler<AuthorizeUserCommand, UserDto>
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthorizeUserCommandHandler(UserManager<UserModel> userManager, 
            SignInManager<UserModel> signInManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _signInManager = signInManager;
        }
        public async Task<UserDto> Handle(AuthorizeUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = request.UserName,
                    Token = await _jwtService.CreateToken(request.UserName, user.Id.ToString())
                };


                return userDto;
            }

            throw new UnauthorizedAccessException();
        }
    }
}
