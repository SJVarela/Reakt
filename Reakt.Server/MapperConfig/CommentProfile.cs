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
            CreateMap<Domain.Models.Comment, Models.Comment>();
            CreateMap<Models.Comment, Domain.Models.Comment>();
            CreateMap<Application.Persistence.Models.Comment, Domain.Models.Comment>();
            CreateMap<Domain.Models.Comment, Application.Persistence.Models.Comment>();
        }
    }
}
