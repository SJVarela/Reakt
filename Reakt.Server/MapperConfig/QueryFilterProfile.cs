using AutoMapper;
using DM = Reakt.Application.Contracts.Common;
using SM = Reakt.Server.Models.Filters;

namespace Reakt.Server.MapperConfig
{
    public class QueryFilterProfile : Profile
    {
        public QueryFilterProfile()
        {
            CreateMap<SM.QueryFilter, DM.QueryFilter>();
        }
    }
}