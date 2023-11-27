namespace UserBookToken.Filter
{
    public class BookFilter
    {
        public int? MinPageCount { get; set; }
        public int? MaxPageCount { get; set; }
        public int? PageCount { get; set; }
        public string? Category { get; set; }
        public string? Color { get; set; }
        public string? AuthourName { get; set; }
    }
}
