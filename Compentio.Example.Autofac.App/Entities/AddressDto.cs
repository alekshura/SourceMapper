using System;

namespace Compentio.Example.Autofac.App.Entities
{
    public class AddressDto
    {
        public Guid AddressId { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
    }
}