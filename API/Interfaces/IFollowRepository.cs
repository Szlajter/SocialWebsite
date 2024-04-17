using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IFollowRepository
    {
        Task<UserFollow> GetFollow(int sourceUserId, int targetUserId);
        Task<PaginatedList<FollowDto>> GetFollowers(int userId, PaginationParams paginationParams);
        Task<PaginatedList<FollowDto>> GetFollowing(int userId, PaginationParams paginationParams);
        Task<User> GetUserWithFollows(int userId);
    }
}