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
            context.ShortUrlModels.Add(
                new ShortUrlModel
                {
                    Id = 1,
                    ShortUrl = "http://localhost:5001/qtdJwBun",
                    OriginalUrl = "https://www.google.com/search?q=bitly&rlz=1C1SQJL_ruUA906UA906&oq=bitl&aqs=chrome.0.69i59j69i57j46j0i433j69i60l4.1023j0j4&sourceid=chrome&ie=UTF-8"
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
