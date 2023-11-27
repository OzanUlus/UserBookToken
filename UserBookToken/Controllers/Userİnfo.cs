using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Authorize(Roles = "user")]
        [HttpGet("AddBookToFavList")]
        public IActionResult AddBookToFavList(string userId,string bookId) 
        {
            try
            {
                var user = _appDbContext.Users.Find(userId);
                if (user== null) 
                {
                    return NotFound("Kullanıcı bulunamadı");                
                }
                var book = _appDbContext.Books.Find(bookId);
                if (book != null) 
                {
                    user.UserFavBooks.Add(book);
                    _appDbContext.SaveChanges();
                }
                return NotFound("Kitap bulunamadı");






            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
            
           
        
        }

    }
}
