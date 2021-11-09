using System;

namespace Compentio.Example.Autofac.App.Entities
{
    public class BookDto
    {
        public Guid BookId { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string BookDescription { get; set; }
        public int PublishingYear { get; set; }
        public AddressDto LibraryAddress { get; set; }
    }
}