namespace UserBookToken.Entities
{
    public class UserFavBook
    {
        public string AppUserId { get; set; }
        public string BookID { get; set; }

        public Book Book { get; set; }
        public AppUser AppUser { get; set; } 
    }
}
