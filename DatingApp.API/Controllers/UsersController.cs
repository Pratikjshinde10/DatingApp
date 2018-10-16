using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.Collections.Generic;
using DatingApp.API.DTOS;

namespace DatingApp.API.Controllers
{
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
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var userstoreturn = _mapper.Map<IEnumerable<UserForDetailedDTO>>(users);
            return Ok(userstoreturn);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        { 
             var user = await _repo.GetUser(id);
             var usertoreturn = _mapper.Map<UserForDetailedDTO>(user);
             return Ok(usertoreturn);
        }
    }
}