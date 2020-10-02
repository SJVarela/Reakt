using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Reakt.Server.MapperConfig
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Reakt.Application.Persistence.Models.Post, Reakt.Domain.Models.Post>();
            CreateMap<Reakt.Domain.Models.Post, Reakt.Server.Models.Post>();
        }
    }
}
