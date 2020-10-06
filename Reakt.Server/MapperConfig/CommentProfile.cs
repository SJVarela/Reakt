using AutoMapper;

namespace Reakt.Server.MapperConfig
{
    /// <summary>
    /// This should probably move to a startup project
    /// </summary>
    public class CommentProfile : Profile
    {
        /// <summary>
        /// </summary>
        public CommentProfile()
        {
            CreateMap<Domain.Models.Comment, Models.Comment>()
                .ReverseMap();
            CreateMap<Application.Persistence.Models.Comment, Domain.Models.Comment>()
                .ReverseMap();
        }
    }
}