namespace Library.Models
{
    public class BookDto
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public bool AvailableToDelete { get; set; }
    }
}
