using AutoMapper;
using DesignAutomation.API.DTOs.Users;
using DesignAutomation.API.Models;

namespace DesignAutomation.API.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(d => d.Roles, o => o.Ignore());

        CreateMap<RegisterDto, ApplicationUser>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName ?? s.Email));
    }
}
