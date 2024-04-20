using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPostRepository
    {
        void CreatePost(Post post);
        void DeletePost(Post post);
        Task UpdatePostContent(int id, string content);
        Task<Post> GetPost(int id);
        Task<PaginatedList<PostDto>> GetPosts(string username, PaginationParams paginationParams);
        void addLike(User user, Post post);
        void addDislike(User user, Post post);
    }
}