using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using DatingApp.API.DTOS;
using System.Security.Claims;
using System;
using DatingApp.API.Helpers;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDTO>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize,
                users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        { 
             var user = await _repo.GetUser(id);
             var usertoreturn = _mapper.Map<UserForDetailedDTO>(user);
             return Ok(usertoreturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserforUpdateDTO userforUpdateDTO)
        {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userFromRepo = await _repo.GetUser(id);

            _mapper.Map(userforUpdateDTO, userFromRepo);

            if(await _repo.SaveAll())
                return NoContent();
            
            throw new Exception($"Updating user {id} failed on save");
        }
    }
}