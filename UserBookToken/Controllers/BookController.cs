using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserBookToken.Context;
using UserBookToken.Entities;
using UserBookToken.Filter;
using UserBookToken.Paging;

namespace UserBookToken.Controllers
{
    [Authorize]
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
        
        [HttpPost("AddBook")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddBook(BookDto bookDto) 
        {
            Book book = new Book()
            {
                Name = bookDto.BookName,
                Category = bookDto.CategoryName,
                AuthourName = bookDto.AuthorName,
                PageCount = bookDto.Page,
                Color = bookDto.Color
            };
             _context.Books.AddAsync(book);
             _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);

        }
        
        [HttpDelete("DeleteBook")]
        public IActionResult DeleteBook(Book book) 
        {
          var item = _context.Books.Find(book.Id);
            _context.Books.Remove(item);
            _context.SaveChanges();
            return Ok();
        
        }
        [HttpGet("ListUserFavBook")]
        public async Task<IActionResult> ListUserFavBook()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var books = await _context.userFavBooks
                .Include(x=>x.Book).Where(x=>x.AppUserId == userId)
                .Select(x=>x.Book)
                .ToListAsync();
            return Ok(books);
         
            
        
        }
        public class BookDto
    {
        public string BookName { get; set; }
        public string CategoryName { get; set; }
        public string AuthorName { get; set; }
        public int Page { get; set; }
        public string Color { get; set; }
    }
    }
}
