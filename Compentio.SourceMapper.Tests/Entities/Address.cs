namespace Compentio.SourceMapper.Tests.Entities
{
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public Region Region { get; set; }
    }

    public class Region
    {
        public string State { get; set; }
        public string District { get; set; }
    }
}
