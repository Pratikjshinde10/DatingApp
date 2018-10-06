using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
      [HttpGet()]
      public string GetHomeValues()
      { 
          return "Home1,Home2";
      }
        
    }
}