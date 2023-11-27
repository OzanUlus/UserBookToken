using Microsoft.AspNetCore.Mvc;
using UserBookToken.Context;
using UserBookToken.Entities;

namespace UserBookToken.Controllers
{
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
        public async Task<IActionResult> ListBook([FromQuery]) 
        {
        
        }
    }
}
