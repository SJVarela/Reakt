using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Reakt.Server.MapperConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class PostProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public PostProfile()
        {
            CreateMap<Reakt.Application.Persistence.Models.Post, Reakt.Domain.Models.Post>();
            CreateMap<Reakt.Domain.Models.Post, Reakt.Server.Models.Post>();

            CreateMap<Reakt.Domain.Models.Post, Reakt.Application.Persistence.Models.Post>();
            CreateMap<Reakt.Server.Models.Post, Reakt.Domain.Models.Post>();
        }
    }
}
