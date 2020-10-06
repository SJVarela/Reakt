using AutoMapper;

namespace Reakt.Server.MapperConfig
{
    /// <summary>
    /// </summary>
    public class BoardProfile : Profile
    {
        /// <summary>
        /// </summary>
        public BoardProfile()
        {
            CreateMap<Reakt.Domain.Models.Board, Reakt.Server.Models.Board>()
                .ReverseMap();
            CreateMap<Reakt.Application.Persistence.Models.Board, Reakt.Domain.Models.Board>()
                .ReverseMap();
        }
    }
}