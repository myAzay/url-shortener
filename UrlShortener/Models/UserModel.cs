using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShortener.Models
{
    public class UserModel : IdentityUser<int>
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }
    }
}
