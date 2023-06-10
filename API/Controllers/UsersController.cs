using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

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
        
        //using ProjectTo() to send shorter select clauses only for memberDto columns instead of whole user entity
        [HttpGet]
        public async Task<IEnumerable<MemberDto>> GetUsers()
        {
            // var users = await _context.Users.Include(p => p.Photos).ToListAsync();
            // return _mapper.Map<IEnumerable<MemberDto>>(users);

            return await _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
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

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            //not using getUserByUsername because i need User entity, not memberDto
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == User.GetUsername());

            if (user == null) return NotFound();

            //updates by overriding
            _mapper.Map(memberUpdateDto, user);

            if (await _context.SaveChangesAsync() > 0) return NoContent();

            return BadRequest("User update failed");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == User.GetUsername());

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
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == User.GetUsername());

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
    }
}