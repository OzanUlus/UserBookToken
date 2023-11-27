namespace UserBookToken.Entities
{
    public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string AuthourName { get; set; }
        public string Category { get; set; }
        public int PageCount { get; set; }
        public string Color { get; set; }
        public List<UserFavBook> userFavBooks { get; set; }
    }
}
