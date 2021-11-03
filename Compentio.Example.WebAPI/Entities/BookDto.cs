namespace Compentio.Example.WebAPI.Entities
{
    public class BookDto
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string BookDescription { get; set; }
        public int PublishingYear { get; set; }
        public AddressDto LibraryAddress { get; set; }
    }
}