using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Models;

namespace UrlShortener.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<ShortUrlModel> ShortUrlModels { get; set; }
    }
}
