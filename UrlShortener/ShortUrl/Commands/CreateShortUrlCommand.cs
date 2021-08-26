using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class CreateShortUrlCommand : IRequest<ShortUrlModel>
    {
        public string LongUrl { get; set; }
    }
    public class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, ShortUrlModel>
    {
        private readonly IApplicationDbContext _context;
        private const string ServiceUrl = "http://localhost:5001";

        public CreateShortUrlCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShortUrlModel> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShortUrlModels
                .Where(x => x.OriginalUrl == request.LongUrl)
                .SingleOrDefaultAsync();

            if (entity is not null) return entity;

            var options = new GenerationOptions
            {
                Length = 8
            };

            var url = new ShortUrlModel
            {
                OriginalUrl = request.LongUrl,
                ShortUrl = $"{ServiceUrl}/{ShortId.Generate(options)}"
            };
             _context.ShortUrlModels.Add(url);

            await _context.SaveChangesAsync(cancellationToken);

            return url;
        }
    }
}
