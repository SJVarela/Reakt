using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.MapperConfig
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Reakt.Domain.Models.Comment, Reakt.Server.Models.Comment>();
            CreateMap<Reakt.Application.Persistence.Models.Comment, Reakt.Domain.Models.Comment>();
        }
    }
}
