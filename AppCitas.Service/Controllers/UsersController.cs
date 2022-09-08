using AppCitas.Service.Data; //Takes the DataContext
using AppCitas.Service.Entities; //Gets the AppUsers
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace AppCitas.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]   //GET api/users/{id}
        public  async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() //IEnumerable es un listado sin capacidades. AppUser into IEnumerable.
        { //async hace que el método sea asíncrono
            return await _context.Users.ToListAsync();
        }
        [HttpGet("{id}")] //GET api/users/id
        public ActionResult<AppUser> GetUserById([FromRoute] int id)
        {
            return _context.Users.Find(id);
        }
    }
}