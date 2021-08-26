using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Data;

namespace UrlShortener.ShortUrl.Commands
{
    public class GetUrlByShortUrlQuery : IRequest<string>
    {
        public string ShortUrl { get; set; }
    }
    public class GetUrlByShortUrlQueryHandler : IRequestHandler<GetUrlByShortUrlQuery, string>
    {
        private readonly IApplicationDbContext _context;

        public GetUrlByShortUrlQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetUrlByShortUrlQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShortUrlModels
                .Where(x => x.ShortUrl == request.ShortUrl)
                .SingleOrDefaultAsync();

            _ = entity ?? throw new Exception("Address is no exists");

            return entity.OriginalUrl;
        }
    }
}
