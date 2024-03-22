
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PaginatedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();
            query = query.Where(u => u.UserName != userParams.CurrentUsername);

            return await PaginatedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider),
                userParams.PageIndex, 
                userParams.PageSize);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        // Rethink including profile picture table
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.ProfilePicture)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.ProfilePicture)
                .ToListAsync();
        }
 
        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public void UpdateProfilePicture(User user, string url, string publicId)
        {
            if (user.ProfilePicture != null)
            {
                user.ProfilePicture.Url = url;
                user.ProfilePicture.PublicId = publicId;
            }
            else
            {
                var newProfilePicture = new ProfilePicture
                {
                    Url = url,
                    PublicId = publicId,
                    UserId = user.Id
                };
                user.ProfilePicture = newProfilePicture;
            }
        }
    }
}