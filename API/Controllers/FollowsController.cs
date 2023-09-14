using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class FollowsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("follow/{username}")]
        public async Task<ActionResult> AddFollower(string username)
        {
            var sourceUserId = User.GetUserId();
            var userToFollow = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _unitOfWork.FollowRepository.GetUserWithFollows(sourceUserId);

            if(userToFollow == null) return NotFound();
            if(sourceUser.UserName == username) return BadRequest("Impossible to follow own profile");            

            var userFollow = await _unitOfWork.FollowRepository.GetFollow(sourceUserId, userToFollow.Id);
            if(userFollow != null) return BadRequest("User is already followed");

            userFollow = new UserFollow
            {
                SourceUserId = sourceUserId,
                TargetUserId = userToFollow.Id
            };

            sourceUser.Following.Add(userFollow);

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Failed to follow");
        }

        [HttpGet("following")]
        public async Task<ActionResult<PaginatedList<FollowDto>>> GetFollowing([FromQuery]UserParams userParams)
        {
            userParams.UserId = User.GetUserId();

            var users = await _unitOfWork.FollowRepository.GetFollowing(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.PageIndex, 
                users.PageSize, 
                users.TotalCount, 
                users.TotalPages));

            return  Ok(users);
        }

        [HttpGet("followers")]
        public async Task<ActionResult<PaginatedList<FollowDto>>> GetFollowers([FromQuery]UserParams userParams)
        {
            var users = await _unitOfWork.FollowRepository.GetFollowers(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.PageIndex, 
                users.PageSize, 
                users.TotalCount, 
                users.TotalPages));

            return  Ok(users);
        }

        [HttpDelete("unfollow/{username}")]
        public async Task<ActionResult> DeleteFollow(string username)
        {
            var sourceUserId = User.GetUserId();
            var userToUnfollow = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _unitOfWork.FollowRepository.GetUserWithFollows(sourceUserId);

            if(userToUnfollow == null) return NotFound();
            if(sourceUser.UserName == username) return BadRequest("Impossible to unfollow own profile");            

            var userFollow = await _unitOfWork.FollowRepository.GetFollow(sourceUserId, userToUnfollow.Id);
            if(userFollow == null) return BadRequest("User is already unfollowed");

            sourceUser.Following.Remove(userFollow);

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Failed to unfollow");
        }

    }
}