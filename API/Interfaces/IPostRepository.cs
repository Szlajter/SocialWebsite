using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPostRepository
    {
        void CreatePost(Post post);
        void DeletePost(Post post);
        void DeletePosts(List<Post> post);
        Task UpdatePostContent(int id, string content);
        Task<Post> GetPost(int id);
        Task<PostWithCommentsDto> GetPostWithComments(int id);
        Task<List<Post>> GetComments(int id);
        Task<PaginatedList<PostDto>> GetPosts(string username, PaginationParams paginationParams);
        void AddLike(User user, Post post);
        void AddDislike(User user, Post post);
    }
}