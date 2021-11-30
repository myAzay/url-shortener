using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Data;
using UrlShortener.Models;

namespace Tests
{
    public static class ApplicationDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);

            context.Database.EnsureCreated();
            SeedSampleData(context);

            return context;
        }

        public static void SeedSampleData(ApplicationDbContext context)
        {
            context.ShortUrlModels.AddRange(
                new ShortUrlModel
                {
                    Id = 1,
                    ShortUrl = "http://localhost:5001/qtdJwBun",
                    OriginalUrl = "https://www.google.ru/"
                },
                new ShortUrlModel
                {
                    Id = 2,
                    ShortUrl = "http://localhost:5001/RxoZRbNZ",
                    OriginalUrl = "https://www.yandex.ru/"
                }
            );

            context.SaveChanges();
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
