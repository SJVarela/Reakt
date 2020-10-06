using AutoMapper;

namespace Reakt.Server.MapperConfig
{
    /// <summary>
    /// </summary>
    public class PostProfile : Profile
    {
        /// <summary>
        /// </summary>
        public PostProfile()
        {
            CreateMap<Reakt.Application.Persistence.Models.Post, Reakt.Domain.Models.Post>().ReverseMap();
            CreateMap<Reakt.Domain.Models.Post, Reakt.Server.Models.Post>().ReverseMap();
        }
    }
}