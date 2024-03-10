using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PostRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
        }

        public void DeletePost(Post post)
        {
            _context.Posts.Remove(post);
        }

        public async Task<Post> GetPost(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<PaginatedList<PostDto>> GetPosts(UserParams userParams)
        {
            var query = _context.Posts
                .OrderByDescending(x => x.DatePosted)
                .AsQueryable();

            // remember to change mapping profile
            var posts = query.ProjectTo<PostDto>(_mapper.ConfigurationProvider);

            return await PaginatedList<PostDto>.CreateAsync(posts, userParams.PageIndex, userParams.PageSize);
        }
    }
}