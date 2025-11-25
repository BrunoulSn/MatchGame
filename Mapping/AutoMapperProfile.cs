using AutoMapper;
using BFF_GameMatch.Services.Dtos.Team;
using MyBffProject.Models;
using MyBffProject.Services.Results;
using BFF_GameMatch.Services.Dtos.User;


namespace MyBffProject.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Team, TeamDto>()
                .ForMember(d => d.MemberCount, opt => opt.MapFrom(s => s.Members == null ? 0 : s.Members.Count))
                .ForMember(d => d.Owner, opt => opt.MapFrom(s => s.Owner));

            CreateMap<TeamCreateDto, Team>();
            CreateMap<TeamUpdateDto, Team>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id));

            CreateMap<User, UserDto>();

            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
        }
    }
}