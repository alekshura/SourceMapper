using System;

namespace Compentio.Example.Autofac.App.Entities
{
    public class AddressDao
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }
        public string HomeNumber { get; set; }
    }
}