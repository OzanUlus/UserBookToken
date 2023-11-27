using Microsoft.AspNetCore.Identity;

namespace UserBookToken.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<UserFavBook> UserFavBooks { get; set; } = new();
    }
}
