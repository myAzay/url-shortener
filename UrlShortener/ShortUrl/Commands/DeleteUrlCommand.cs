using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Data;

namespace UrlShortener.ShortUrl.Commands
{
    public class DeleteUrlCommand : IRequest
    {
        public int Id { get; set; }
    }
    public class DeleteUrlCommandHandler : IRequestHandler<DeleteUrlCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUrlCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUrlCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShortUrlModels.FindAsync(request.Id);

            if (entity is not null) _context.ShortUrlModels.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
