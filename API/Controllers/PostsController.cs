using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
        public async Task<ActionResult<PostDto>> CreatePost(PostCreateDto postCreate)
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

            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<PostDto>(post));

            return BadRequest("Failed to create a post");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetPost(int id)
        {
            var post = await _unitOfWork.PostRepository.GetPost(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostDto>(post));
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
    }
}