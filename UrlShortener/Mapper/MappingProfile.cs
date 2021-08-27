using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.AppUsers.Commands;
using UrlShortener.Models;

namespace UrlShortener.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserCommand, UserModel>();
        }
    }
}
