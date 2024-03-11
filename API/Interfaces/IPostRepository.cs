using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPostRepository
    {
        void CreatePost(Post post);
        void DeletePost(Post post);
        Task<Post> GetPost(int id);
        Task<PaginatedList<PostDto>> GetPosts(UserParams userParams);
    }
}