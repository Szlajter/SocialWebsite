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
            string currentUserName = null;

            CreateMap<User, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, 
                           opt => opt.MapFrom(src => src.ProfilePicture.Url))
                .ForMember(dest => dest.Age,
                           opt => opt.MapFrom(src => src.Birthdate.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<ProfilePicture, PhotoDto>();
            CreateMap<MemberUpdateDto, User>();
            CreateMap<RegisterDto, User>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => src.Sender.ProfilePicture.Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => src.Recipient.ProfilePicture.Url));
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.AuthorNickname, opt => opt.MapFrom(src => src.Author.NickName))
                .ForMember(dest => dest.AuthorUsername, opt => opt.MapFrom(src => src.Author.UserName))
                .ForMember(dest => dest.AuthorPhotoUrl, opt => opt.MapFrom(src => src.Author.ProfilePicture.Url))
                .ForMember(dest => dest.LikedByCount, opt => opt.MapFrom(src => src.LikedBy.Count))
                .ForMember(dest => dest.DislikedByCount, opt => opt.MapFrom(src => src.DislikedBy.Count))
                .ForMember(dest => dest.hasLiked, opt => opt.MapFrom(u => u.LikedBy.Any(u => u.UserName == currentUserName)))
                .ForMember(dest => dest.hasDisliked, opt => opt.MapFrom(u => u.DislikedBy.Any(u => u.UserName == currentUserName)))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
            //entity framework refuses to return dates as utc so mapping is required.
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
        }
    }
}