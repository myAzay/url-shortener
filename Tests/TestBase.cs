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
        public TestBase()
        {
            Context = ApplicationDbContextFactory.Create();
        }

        public void Dispose()
        {
            ApplicationDbContextFactory.Destroy(Context);
        }
    }
}
