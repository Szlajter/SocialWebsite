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
                           opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsProfilePicture).Url))
                .ForMember(dest => dest.Age,
                           opt => opt.MapFrom(src => src.Birthdate.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, User>();
            CreateMap<RegisterDto, User>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsProfilePicture).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(x => x.IsProfilePicture).Url));
            //entity framework refuses to return dates as utc so mapping is required.
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}