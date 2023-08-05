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
        private readonly IFollowRepository _followRepository;
        private readonly IUserRepository _userRepository;

        public FollowsController(IFollowRepository followRepository, IUserRepository userRepository)
        {
            _followRepository = followRepository;
            _userRepository = userRepository;
        }

        [HttpPost("follow/{username}")]
        public async Task<ActionResult> AddFollower(string username)
        {
            var sourceUserId = User.GetUserId();
            var userToFollow = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _followRepository.GetUserWithFollows(sourceUserId);

            if(userToFollow == null) return NotFound();
            if(sourceUser.UserName == username) return BadRequest("Impossible to follow own profile");            

            var userFollow = await _followRepository.GetFollow(sourceUserId, userToFollow.Id);
            if(userFollow != null) return BadRequest("User is already followed");

            userFollow = new UserFollow
            {
                SourceUserId = sourceUserId,
                TargetUserId = userToFollow.Id
            };

            sourceUser.Following.Add(userFollow);

            if (await _userRepository.SaveAllAsync())
            {
                return Ok();
            }
            return BadRequest("Failed to follow");
        }

        [HttpGet("following")]
        public async Task<ActionResult<PaginatedList<FollowDto>>> GetFollowing([FromQuery]UserParams userParams)
        {
            userParams.UserId = User.GetUserId();

            var users = await _followRepository.GetFollowing(userParams);

            Response.AddPaginationHeader(new PaginationHeader(users.PageIndex, 
                users.PageSize, 
                users.TotalCount, 
                users.TotalPages));

            return  Ok(users);
        }

        [HttpGet("followers")]
        public async Task<ActionResult<PaginatedList<FollowDto>>> GetFollowers([FromQuery]UserParams userParams)
        {
            var users = await _followRepository.GetFollowers(userParams);

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
            var userToUnfollow = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _followRepository.GetUserWithFollows(sourceUserId);

            if(userToUnfollow == null) return NotFound();
            if(sourceUser.UserName == username) return BadRequest("Impossible to unfollow own profile");            

            var userFollow = await _followRepository.GetFollow(sourceUserId, userToUnfollow.Id);
            if(userFollow == null) return BadRequest("User is already unfollowed");

            sourceUser.Following.Remove(userFollow);

            if (await _userRepository.SaveAllAsync())
            {
                return Ok();
            }
            return BadRequest("Failed to unfollow");
        }

    }
}