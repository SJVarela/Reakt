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
            CreateMap<Application.Persistence.Models.Post, Domain.Models.Post>()
                .ReverseMap();
            CreateMap<Domain.Models.Post, Models.Post>()
                .ReverseMap();
        }
    }
}