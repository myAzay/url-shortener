using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Data;

namespace Tests
{
    public class TestBase : IDisposable
    {
        public ApplicationDbContext Context { get; }
        public IDistributedCache Cache { get; }
        public TestBase()
        {
            Context = ApplicationDbContextFactory.Create();
            Cache = new Mock<IDistributedCache>().Object;
        }

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy(Context);
        }
    }
}
