using AppCitas.Service.Controllers;
using AppCitas.Service.Data; //Takes the DataContext
using AppCitas.Service.Entities; //Gets the AppUsers
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace AppCitas.Service.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]   //GET api/users/{id}
        [AllowAnonymous] //tiene permiso
        public  async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() //IEnumerable es un listado sin capacidades. AppUser into IEnumerable.
        { //async hace que el método sea asíncrono
            return await _context.Users.ToListAsync();
        }
        [HttpGet("{id}")] //GET api/users/id
        [Authorize] //necesita autorización
        public ActionResult<AppUser> GetUserById([FromRoute] int id)
        {
            return _context.Users.Find(id);
        }
    }
}