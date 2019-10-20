using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.API.Models;

namespace Users.API.Controllers
{
    [Route("user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: users
        /// 
        /// Technically not part of the spec, but added it to make it easier to verify the result
        /// </summary>
        [HttpGet]
        [Route("~/users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers() {
            return await _context.Users.ToListAsync();
        }

        // GET: user/William
        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUser(string username) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) {
                return NotFound();
            }

            return user;
        }

        // PUT: user/William
        [HttpPut("{username}")]
        public async Task<IActionResult> PutUser(string username, User user)
        {
            if (username != user.Username)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(username))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: user
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user) {
            //Db should probably be set up so that the username is unique,
            //but since that's not supported by EF in-memory-db, I just handle/hack it by deleting existing user.
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser != null) {
                _context.Users.Remove(existingUser);
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { username = user.Username }, user);
        }

        // POST: user/createWithArray or user/createWithList
        [HttpPost]
        [Route("createWithArray")]
        [Route("createWithList")]
        public async Task<ActionResult<List<User>>> PostUsers(List<User> users) {
            //Db should probably be set up so that the username is unique,
            //but since that's not supported by EF in-memory-db, I just handle/hack it by deleting existing user.
            foreach (var user in users) {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                if (existingUser != null) {
                    _context.Users.Remove(existingUser);
                }
            }
            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            //TODO: There might be a better way of handling this instead of directly returning the statuscode
            return StatusCode(201, users);
        }

        // DELETE: user/William
        [HttpDelete("{username}")]
        public async Task<ActionResult<User>> DeleteUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(string userName) {
            return _context.Users.Any(e => e.Username == userName);
        }
    }
}
