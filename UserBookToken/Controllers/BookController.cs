using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserBookToken.Context;
using UserBookToken.Entities;
using UserBookToken.Filter;
using UserBookToken.Paging;

namespace UserBookToken.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet("Search")]
        public async Task<IActionResult> Search(Book book) 
        {
          var result = await _context.Books.FindAsync(book.Id);
            if (result == null) 
            {
              return NotFound();
            }
            else return Ok(result);
        
        
        }
        [HttpGet("ListBook")]
        public async Task<IActionResult> ListBook([FromQuery] PagingParameter p, [FromQuery] BookFilter bf) 
        {
            var query = _context.Books.AsQueryable();

            if (bf.AuthourName != null) 
            {
                query = query.Where(x => x.AuthourName == bf.AuthourName);
            
            }
            if (bf.Category != null) 
            {
                query = query.Where(x => x.Category == bf.Category);
            
            }
            if (bf.PageCount != null) 
            {
                query = query.Where(x => x.PageCount == bf.PageCount);
            }
            if (bf.Color != null) 
            {
                query = query.Where(x => x.Color == bf.Color);
            }
            query = query.Skip(p.PageSize.Value*(p.PageCount.Value-1)).Take(p.PageSize.Value);
            return Ok(query.ToList());

          

        }
        [Authorize(Roles ="admin")]
        [HttpPost("AddBook")]
        public IActionResult AddBook(Book book) 
        {
          _context.Books.Add(book);
            _context.SaveChanges();
            return Ok();
        
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteBook")]
        public IActionResult DeleteBook(Book book) 
        {
          var item = _context.Books.Find(book.Id);
            _context.Books.Remove(item);
            _context.SaveChanges();
            return Ok();
        
        }
        [HttpGet("ListUserFavBook")]
        public IActionResult ListUserFavBook(string userId,string bookId)
        {
         var result = _context.userFavBooks.Where(x=>x.AppUserId==userId && x.BookID==bookId).ToList();
            return Ok(result);
        
        }
    }
}
