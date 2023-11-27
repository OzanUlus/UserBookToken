using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserBookToken.Entities;

namespace UserBookToken.Context
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserFavBook> userFavBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserFavBook>().HasOne(x => x.Book).WithMany(x => x.userFavBooks).HasForeignKey(x => x.BookID);
            builder.Entity<UserFavBook>().HasOne(x => x.AppUser).WithMany(x => x.UserFavBooks).HasForeignKey(x => x.AppUserId);
            builder.Entity<UserFavBook>().HasKey(x => new { x.AppUserId, x.BookID });
            base.OnModelCreating(builder);
        }

        internal Task FindAsync(string ıd)
        {
            throw new NotImplementedException();
        }
    }
}
