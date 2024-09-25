using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using api.src.Data;
using api.src.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.Users.Include(u => u.Role).ToList();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            if(role == null)
            {
                return BadRequest("Role NOT found");
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }
        [HttpPut("{id}")]
        public IActionResult PutId([FromRoute] int id, [FromBody] User user)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            if(role == null)
            {
                return BadRequest("Role NOT found");
            }
            var userToUpdate = _context.Users.FirstOrDefault(u => u.Id == id);
            if(userToUpdate == null)
            {
                return NotFound();
            }
            userToUpdate.Rut = user.Rut;
            userToUpdate.Name = user.Name;
            userToUpdate.Email = user.Email;
            userToUpdate.RoleId = user.RoleId;

            _context.SaveChanges();
            return Ok(userToUpdate);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteId([FromRoute] int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPost("withCookie")]
        public IActionResult PostByCookie([FromBody] User user)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);
            if(role == null)
            {
                return BadRequest("Role NOT found");
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions{
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(5)
            });
            return Ok(user);
        }
        [HttpGet("Me")]
        public IActionResult GetCurrentUser()
        {
            if(Request.Cookies.TryGetValue("UserId", out var userId))
            {
                var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == int.Parse(userId));
                if(user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            return NotFound("UserId cookie NOT found");
        }
    }    
    //Si se agrega o elimina un usuario a la base de datos, SIEMPRE hay que SaveChanges()
}