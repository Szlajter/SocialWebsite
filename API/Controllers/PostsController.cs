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
            var username = User.GetUsername();

            var author = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if (author == null)
                return NotFound();

            var post = new Post
            {
                Title = postCreate.Title,
                Content = postCreate.Content,
                Author = author,
                AuthorId = author.Id
            };

            _unitOfWork.PostRepository.CreatePost(post);

            if (await _unitOfWork.Complete()) return Ok(_mapper.Map<PostDto>(post));

            return BadRequest("Faield to create a post");
        }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult> DeletePost(int id)
        // {

        // }
    }
}