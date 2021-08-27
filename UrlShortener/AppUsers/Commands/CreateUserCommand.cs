using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Models;

namespace UrlShortener.AppUsers.Commands
{
    public class CreateUserCommand : IRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(UserManager<UserModel> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UserModel>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Something went wrong when creating a user...");
            }
            return Unit.Value;
        }
    }
}
