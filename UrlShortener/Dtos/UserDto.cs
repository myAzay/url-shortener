using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Dtos
{
    public class UserDto
    {
        public string UserName { get; set; }
        public int Id { get; set; }
        public string Token { get; set; }
    }
}
