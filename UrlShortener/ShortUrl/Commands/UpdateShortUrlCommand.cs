using MediatR;
using shortid;
using shortid.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.ShortUrl.Commands
{
    public class UpdateShortUrlCommand : IRequest<ShortUrlModel>
    {
        public int UrlId { get; set; }
    }
    public class UpdateShortUrlCommandHandler : IRequestHandler<UpdateShortUrlCommand, ShortUrlModel>
    {
        private readonly IApplicationDbContext _context;
        private const string ServiceUrl = "http://localhost:5001";

        public UpdateShortUrlCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ShortUrlModel> Handle(UpdateShortUrlCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShortUrlModels.FindAsync(request.UrlId);

            _ = entity ?? throw new Exception("Entity is not exist");

            var options = new GenerationOptions
            {
                Length = 8
            };

            entity.ShortUrl = $"{ServiceUrl}/{ShortId.Generate(options)}";

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
