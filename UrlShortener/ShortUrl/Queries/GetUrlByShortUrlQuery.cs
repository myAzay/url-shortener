using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrlShortener.Data;
using UrlShortener.Extractions;
using UrlShortener.Models;

namespace UrlShortener.ShortUrl.Commands
{
    public class GetUrlByShortUrlQuery : IRequest<string>
    {
        public string ShortUrl { get; set; }
    }
    public class GetUrlByShortUrlQueryHandler : IRequestHandler<GetUrlByShortUrlQuery, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        public GetUrlByShortUrlQueryHandler(IApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<string> Handle(GetUrlByShortUrlQuery request, CancellationToken cancellationToken)
        {
            var recordKey = "UrlShortener_" + request.ShortUrl;

            var cacheData = await _cache.GetRecordAsync<ShortUrlModel>(recordKey);

            if(cacheData is not null)
            {
                return cacheData.OriginalUrl;
            }

            var entity = await _context.ShortUrlModels
                .Where(x => x.ShortUrl == request.ShortUrl)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            _ = entity ?? throw new Exception("Address is no exists");

            await _cache.SetRecordAsync(recordKey, entity);

            return entity.OriginalUrl;
        }
    }
}
