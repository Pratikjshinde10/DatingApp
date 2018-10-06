using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOS;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthRepository repo, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            userForRegisterDTO.Username = userForRegisterDTO.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDTO.Username))
                return BadRequest("Username already exists");

            var userTocreate = new User
            {
                UserName = userForRegisterDTO.Username
            };

            var createduser = await _repo.Register(userTocreate, userForRegisterDTO.Password);

            return StatusCode(201);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginForDTO userLoginForDTO)
        {
            var userfromRepo = await _repo.Login(userLoginForDTO.UserName, userLoginForDTO.Password);

            if (userfromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
              new Claim(ClaimTypes.NameIdentifier,userfromRepo.Id.ToString()),
              new Claim(ClaimTypes.Name, userfromRepo.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(_configuration.GetSection("AppSettings:Token").Value));
                
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
              Subject = new ClaimsIdentity(claims),
              Expires = DateTime.Now.AddDays(1),
              SigningCredentials = creds
            };
            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokenDescriptor);
            return Ok(new {
                token = tokenhandler.WriteToken(token)
            });


              
        }
    }
}