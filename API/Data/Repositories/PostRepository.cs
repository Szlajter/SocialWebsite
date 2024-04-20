using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.LikedBy)
                .Include(p => p.DislikedBy)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PostWithCommentsDto> GetPostWithComments(int id)
        {
            var query = _context.Posts.Where(u => u.Id == id).AsQueryable();

            var post = await query.ProjectTo<PostWithCommentsDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            
            return post;
        }

        public async Task<PaginatedList<PostDto>> GetPosts(string username, PaginationParams postParams)
        {
            var query = _context.Posts
                .Where(x => x.ParentPostId == null)
                .OrderByDescending(x => x.DatePosted)
                .AsQueryable();

            var posts = query.ProjectTo<PostDto>(_mapper.ConfigurationProvider, new { currentUserName = username});
            return await PaginatedList<PostDto>.CreateAsync(posts, postParams.PageIndex, postParams.PageSize);
        }

        public async Task UpdatePostContent(int id, string content)
        {
            await _context.Posts
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(b => b
                    .SetProperty(u => u.Content, content)
                    .SetProperty(u => u.IsEdited, true));
        }

        public void addLike(User user, Post post)
        {
            if (post.LikedBy.Contains(user))
            {
                post.LikedBy.Remove(user);
            } 
            else if (post.DislikedBy.Contains(user))
            {
                post.DislikedBy.Remove(user);
                post.LikedBy.Add(user);
            }
            else
            {
                post.LikedBy.Add(user);
            }
        }

        public void addDislike(User user, Post post)
        {
            if (post.DislikedBy.Contains(user))
            {
                post.DislikedBy.Remove(user);
            } 
            else if (post.LikedBy.Contains(user))
            {
                post.LikedBy.Remove(user);
                post.DislikedBy.Add(user);
            }
            else
            {
                post.DislikedBy.Add(user);
            }
        }
    }
}   