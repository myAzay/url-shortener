using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Models
{
    public class ShortUrlModel
    {
        public int Id { get; set; }

        public string OriginalUrl { get; set; }

        public string ShortUrl { get; set; }
    }
}
