using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.JSInterop.Infrastructure;

namespace API.Helpers
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, 
                           opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.isProfilePicture).Url))
                .ForMember(dest => dest.Age,
                           opt => opt.MapFrom(src => src.Birthdate.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, User>();
        }
    }
}