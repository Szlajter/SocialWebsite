using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PostsController: BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost(PostCreateDto postCreate)
        {
            var author = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (author == null)
                return NotFound();

            var post = new Post
            {
                Content = postCreate.Content,
                Author = author,
                AuthorId = author.Id
            };

            // when the post is a comment
            if (postCreate.ParentPostId !=  null)
            {
                var ParentPostId = postCreate.ParentPostId.Value;
                var ParentPost = await _unitOfWork.PostRepository.GetPost(ParentPostId);

                post.ParentPostId = ParentPostId;
                post.ParentPost = ParentPost;

                ParentPost.Comments.Add(post);
            }

            _unitOfWork.PostRepository.CreatePost(post);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to create a post");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostWithCommentsDto>> GetPost(int id)
        {
            var post = await _unitOfWork.PostRepository.GetPostWithComments(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedList<PostDto>>> GetPosts([FromQuery]PaginationParams paginationParams)
        {
            var username = User.GetUsername();

            var posts = await _unitOfWork.PostRepository.GetPosts(username, paginationParams);

            Response.AddPaginationHeader(new PaginationHeader(posts.PageIndex, posts.PageSize, posts.TotalCount, posts.TotalPages));

            return Ok(posts);
        }

        // TODO: add some checks for content
        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdatePost(int id, [FromBody] string content)
        {
            await _unitOfWork.PostRepository.UpdatePostContent(id, content);
            
            return Ok();
        }

        // [HttpDelete("{id}")] 
        // public async Task<ActionResult> DeletePost(int id)
        // {

        // }

        // todo: make getters smaller and faster.
        [HttpPost("{id}/like")]
        public async Task<ActionResult> likePost(int id) 
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) 
            {
                return NotFound();
            }

            var post = await _unitOfWork.PostRepository.GetPost(id);

            if (post == null) 
            {
               return NotFound();
            }

            _unitOfWork.PostRepository.addLike(user, post);

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Failed to like");
        }

        [HttpPost("{id}/dislike")]
        public async Task<ActionResult<int>> dislikePost(int id) 
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) 
            {
                return NotFound();
            }

            var post = await _unitOfWork.PostRepository.GetPost(id);

            if (post == null) 
            {
               return NotFound();
            }

            _unitOfWork.PostRepository.addDislike(user, post);

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Failed to like");
        }
    }
}