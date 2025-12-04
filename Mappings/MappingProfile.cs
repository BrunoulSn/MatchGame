using AutoMapper;
using BFF_GameMatch.Models;
using BFF_GameMatch.Services.Dtos.User;
using BFF_GameMatch.Services.Dtos.Team;
using BFF_GameMatch.Services.Dtos.Group;

namespace BFF_GameMatch.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings - usando apenas modelos do BFF
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone ?? ""))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => "")); // CPF não existe no backend

            CreateMap<UserCreateDto, User>();
            CreateMap<UserDto, User>();

            // Team mappings - usando apenas modelos do BFF
            CreateMap<Team, TeamDto>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? ""))
                .ForMember(dest => dest.SportType, opt => opt.MapFrom(src => src.SportType ?? ""));

            CreateMap<TeamCreateDto, Team>();
            CreateMap<TeamDto, Team>();

            // Group mappings - usando apenas modelos do BFF
            CreateMap<Group, GroupResponseDto>();
            CreateMap<GroupCreateDto, Group>();
            CreateMap<GroupUpdateDto, Group>();
            CreateMap<GroupResponseDto, Group>();

            // Mapeamento para CreateGroupRequest (se necessário)
            CreateMap<CreateGroupRequest, Group>()
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => 1)); // Default, ajustar conforme autenticação
        }
    }
}