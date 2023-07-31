using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace API.Controllers
{
    [Authorize]
    public class UsersController: BaseApiController
    { 
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(ApplicationDbContext context, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _context = context;
            _mapper = mapper;
        }        
        
         //ProjectTo() is used to make query shorter (only MemberDto columns)
         //returns users expect logged one
        [HttpGet]
        public async Task<ActionResult<PaginatedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var user = await GetFullUserByUsername(User.GetUsername());
            userParams.currentUsername = user.UserName;
            
            var query = _context.Users.AsQueryable();
            query = query.Where(u => u.UserName != userParams.currentUsername);

            var users = await PaginatedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider),
                        userParams.PageIndex, 
                        userParams.PageSize);
                
            Response.AddPaginationHeader(new PaginationHeader(users.PageIndex, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDto>> GetUser(int id)
        {
            // var user = await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.Id == id);
            // return _mapper.Map<MemberDto>(user);

            return await _context.Users
                .Where(x => x.Id == id)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            // var user = await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
            // return _mapper.Map<MemberDto>(user);

            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        private async Task<User> GetFullUserByUsername(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var user = await GetFullUserByUsername(User.GetUsername());

            if (user == null) return NotFound();

            //updates by overriding
            _mapper.Map(memberUpdateDto, user);

            if (await _context.SaveChangesAsync() > 0) return NoContent();

            return BadRequest("User update failed");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await GetFullUserByUsername(User.GetUsername());

            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            user.Photos.Add(photo);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetUserByUsername), new {username = user.UserName}, _mapper.Map<PhotoDto>(photo)); 
            }

            return BadRequest("Something went wrong while adding a new photo");
        }

        [HttpPost("add-profile-picture")]
        public async Task<ActionResult<PhotoDto>> AddProfilePicture(IFormFile file)
        {
            var user = await GetFullUserByUsername(User.GetUsername());

            if (user == null) return NotFound();

            var currentProfilePicture = user.Photos.FirstOrDefault(x => x.isProfilePicture == true);

            if(currentProfilePicture != null)
                currentProfilePicture.isProfilePicture = false;

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                isProfilePicture = true,
                PublicId = result.PublicId
            };

            user.Photos.Add(photo);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtAction(nameof(GetUserByUsername), new {username = user.UserName}, _mapper.Map<PhotoDto>(photo)); 
            }

            return BadRequest("Something went wrong while adding a profile picture");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePicture(int photoId)
        {
            var user = await GetFullUserByUsername(User.GetUsername());

            if (user == null) return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();

            //removing only photos that are in cloudinary
            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest("Something went wrong while deleting a picture");
        }

        [HttpPost("follow/{username}")]
        public async Task<ActionResult> AddFollower(string username)
        {
            var sourceUserId = User.GetUserId();
            var userToFollow = await GetFullUserByUsername(username);
            var sourceUser = await _context.Users
                .Include(x => x.Followers)
                .FirstOrDefaultAsync(x => x.Id == sourceUserId);

            if(userToFollow == null) return NotFound();
            if(sourceUser.UserName == username) return BadRequest("Impossible to follow own profile");            

            var userFollow = await _context.Follows.FindAsync(sourceUserId, userToFollow.Id);
            if(userFollow != null) return BadRequest("User is already followed");

            userFollow = new UserFollow
            {
                SourceUserId = sourceUserId,
                TargetUserId = userToFollow.Id
            };

            sourceUser.Following.Add(userFollow);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest("Failed to follow");
        }

        [HttpGet("following")]
        public async Task<ActionResult<PaginatedList<FollowDto>>> GetFollowing([FromQuery]UserParams userParams)
        {
            var users = _context.Users
                .OrderBy(u => u.UserName)
                .AsQueryable();

            var currentUserId = User.GetUserId();
            var follows = _context.Follows
                .Where(follow => follow.SourceUserId == currentUserId)
                .AsQueryable();

            users = follows.Select(follow => follow.TargetUser);

            var followedUsers = users.Select(user => new FollowDto
            {
                Id = user.Id,
                UserName = user.UserName,
                NickName = user.NickName,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.isProfilePicture).Url
            });

            var paginatedFollowedUsers = await PaginatedList<FollowDto>.CreateAsync(followedUsers, userParams.PageIndex, userParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(paginatedFollowedUsers.PageIndex, 
                                                              paginatedFollowedUsers.PageSize, 
                                                              paginatedFollowedUsers.TotalCount, 
                                                              paginatedFollowedUsers.TotalPages));

            return  Ok(paginatedFollowedUsers);
        }

        [HttpGet("followers")]
        public async Task<ActionResult<PaginatedList<FollowDto>>> GetFollowers([FromQuery]UserParams userParams)
        {
            var users = _context.Users
                .OrderBy(u => u.UserName)
                .AsQueryable();

            var currentUserId = User.GetUserId();
            var follows = _context.Follows
                .Where(follow => follow.TargetUserId == currentUserId)
                .AsQueryable();

            users = follows.Select(follow => follow.SourceUser);

            var followingUsers = users.Select(user => new FollowDto
            {
                Id = user.Id,
                UserName = user.UserName,
                NickName = user.NickName,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.isProfilePicture).Url
            });

            var paginatedFollowingUsers = await PaginatedList<FollowDto>.CreateAsync(followingUsers, userParams.PageIndex, userParams.PageSize);

            Response.AddPaginationHeader(new PaginationHeader(paginatedFollowingUsers.PageIndex, 
                                                              paginatedFollowingUsers.PageSize, 
                                                              paginatedFollowingUsers.TotalCount, 
                                                              paginatedFollowingUsers.TotalPages));
            return Ok(paginatedFollowingUsers);
        }

        [HttpDelete("unfollow/{username}")]
        public async Task<ActionResult> DeleteFollow(string username)
        {
            var sourceUserId = User.GetUserId();
            var userToUnfollow = await GetFullUserByUsername(username);
            var sourceUser = await _context.Users
                .Include(x => x.Followers)
                .FirstOrDefaultAsync(x => x.Id == sourceUserId);

            if(userToUnfollow == null) return NotFound();
            if(sourceUser.UserName == username) return BadRequest("Impossible to unfollow own profile");            

            var userFollow = await _context.Follows.FindAsync(sourceUserId, userToUnfollow.Id);
            if(userFollow == null) return BadRequest("User is already unfollowed");

            sourceUser.Following.Remove(userFollow);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest("Failed to unfollow");
        }
    }
}