using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Services
{
    public interface IJwtService
    {
        Task<string> CreateToken(string username, string userId);
    }
}
