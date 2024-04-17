using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class FollowRepository : IFollowRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserFollow> GetFollow(int sourceUserId, int targetUserId)
        {
            return await  _context.Follows.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PaginatedList<FollowDto>> GetFollowers(int userId, PaginationParams paginationParams)
        {
            var users =_context.Users
                .OrderBy(u => u.UserName).AsQueryable();
            var follows = _context.Follows.
                Where(follow => follow.TargetUserId == userId).AsQueryable();

            users = follows.Select(follow => follow.SourceUser);

            var followingUsers = users.Select(user => new FollowDto
            {
                Id = user.Id,
                UserName = user.UserName,
                NickName = user.NickName,
                PhotoUrl = user.ProfilePicture.Url
            });

            return await PaginatedList<FollowDto>.CreateAsync(followingUsers, paginationParams.PageIndex, paginationParams.PageSize);
        }

        public async Task<PaginatedList<FollowDto>> GetFollowing(int userId, PaginationParams paginationParams)
        {
            var users =_context.Users
                .OrderBy(u => u.UserName).AsQueryable();
            var follows = _context.Follows
                 .Where(follow => follow.SourceUserId == userId).AsQueryable();

            users = follows.Select(follow => follow.TargetUser);

            var followedUsers =  users.Select(user => new FollowDto
            {
                Id = user.Id,
                UserName = user.UserName,
                NickName = user.NickName,
                PhotoUrl = user.ProfilePicture.Url
            });

            return await PaginatedList<FollowDto>.CreateAsync(followedUsers, paginationParams.PageIndex, paginationParams.PageSize);
        }

        public async Task<User> GetUserWithFollows(int userId)
        {
            return await _context.Users
                .Include(x => x.Following)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}