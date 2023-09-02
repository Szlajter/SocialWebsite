using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.Username)) 
            {
                return BadRequest("Username is already taken.");
            }

            var user = _mapper.Map<User>(registerDto);
            user.UserName = registerDto.Username.ToLower();
            var registrationResult = await _userManager.CreateAsync(user, registerDto.Password);

            if(!registrationResult.Succeeded)
            {
                return BadRequest(registrationResult.Errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if(!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(p => p.UserName == loginDto.Username.ToLower());

            if(user == null)
            {
                return Unauthorized("Invalid Username"); 
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if(!result)
            {
                return Unauthorized("Invalid Password");
            }
            
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsProfilePicture)?.Url,
                Nickname = user.NickName
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(p => p.UserName == username.ToLower());
        }
    }
}   