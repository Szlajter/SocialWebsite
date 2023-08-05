using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IFollowRepository
    {
        Task<UserFollow> GetFollow(int sourceUserId, int targetUserId);
        Task<PaginatedList<FollowDto>> GetFollowers(UserParams userParams);
        Task<PaginatedList<FollowDto>> GetFollowing(UserParams userParams);
        Task<User> GetUserWithFollows(int userId);
    }
}