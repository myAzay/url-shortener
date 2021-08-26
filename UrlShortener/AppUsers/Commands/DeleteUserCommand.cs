using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Models;

namespace UrlShortener.AppUsers.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public int UserId { get; set; }
    }
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly UserManager<UserModel> _userManager;

        public DeleteUserCommandHandler(UserManager<UserModel> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == request.UserId);

            if (user != null)
            {
                await DeleteUserAsync(user);
            }

            return Unit.Value;
        }
        public async Task<bool> DeleteUserAsync(UserModel user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}
