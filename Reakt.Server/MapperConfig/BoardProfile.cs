using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reakt.Server.MapperConfig
{
    public class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<Reakt.Domain.Models.Board, Reakt.Server.Models.Board>();
            CreateMap<Reakt.Application.Persistence.Models.Board, Reakt.Domain.Models.Board>();
        }
    }
}
