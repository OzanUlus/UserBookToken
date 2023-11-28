using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserBookToken.Context;
using UserBookToken.DTO;
using UserBookToken.Entities;

namespace UserBookToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Userİnfo : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public Userİnfo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

       
        [Authorize(Roles = "user")]
        [HttpGet("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo(AppUser user) 
        {
            var item = await _appDbContext.Users.FindAsync(user.Id);
            if (item != null) 
            {
              user.Name = item.Name;
                user.SurName = item.SurName;
                user.BirthDate = item.BirthDate;
                return Ok(user);

            }
            return BadRequest("Kullanıcı bulunamadı");
        }
        [Authorize(Roles = "user")]
        [HttpGet("GetUserFavBook")]
        public async Task<IActionResult> GetUserFavbook(string id)
        {
          var user = await _appDbContext.Users.FindAsync(id);
            if (user != null) 
            {
                var  userfavbook = await _appDbContext.userFavBooks.Where(x => x.AppUserId == user.Id).ToListAsync();
                return Ok(userfavbook);

            }
            return BadRequest("Kullanıcı bulunamadı");

        }
        
        [HttpGet("AddBookToFavList/{id}")]
        public async Task<IActionResult> AddBookToFavList(string id) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var exists = await _appDbContext.Books.AnyAsync(x => x.Id == id) && await _appDbContext.Users.AnyAsync(x => x.Id == userId);

            if (exists)
            {
                var userFavBook = new UserFavBook
                {
                    AppUserId = userId,
                     BookID= id
                };

                _appDbContext.userFavBooks .Add(userFavBook);
                await _appDbContext.SaveChangesAsync();

                return Ok();
            }

            return NotFound();




        }

    }
}
