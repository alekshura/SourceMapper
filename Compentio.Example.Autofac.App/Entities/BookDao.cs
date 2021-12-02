using System;
using System.Collections.Generic;

namespace Compentio.Example.Autofac.App.Entities
{
    public class BookDao
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PublishingYear { get; set; }
        public IList<AddressDao> LibraryAddressesDao { get; set; }
    }
}