using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Currently im getting the user using claims. Passing id by query might be a better idea but i'm not sure yet.
    [Authorize]
    public class UsersController: BaseApiController
    { 
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }        
        
        [HttpGet]
        public async Task<ActionResult<PaginatedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            userParams.CurrentUsername = User.GetUsername();

            var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);
                
            Response.AddPaginationHeader(new PaginationHeader(users.PageIndex, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _unitOfWork.UserRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            //updates by overriding
            _mapper.Map(memberUpdateDto, user);

            _unitOfWork.UserRepository.Update(user);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("User update failed");
        }

        [HttpPost("update-profile-picture")]
        public async Task<ActionResult<PhotoDto>> UpdateProfilePicture(IFormFile file)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            _unitOfWork.UserRepository.UpdateProfilePicture(user, result.SecureUrl.AbsoluteUri, result.PublicId);

            if (await _unitOfWork.Complete())
            {
                return CreatedAtAction("GetUser", new {username = user.UserName}, _mapper.Map<PhotoDto>(user.ProfilePicture)); 
            }

            return BadRequest("Something went wrong while adding a profile picture");
        }

        [HttpDelete("delete-profile-picture")]
        public async Task<ActionResult> DeletePicture()
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return NotFound();

            var photo = user.ProfilePicture;

            if(photo == null) return NotFound();

            //removing only photos that are in cloudinary
            if(photo.PublicId != null)
            {
                var result = await _photoService.DeleteMediaAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            user.ProfilePicture = null;

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("Something went wrong while deleting a picture");
        }
    }
}