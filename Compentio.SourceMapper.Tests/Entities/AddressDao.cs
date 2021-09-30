namespace Compentio.SourceMapper.Tests.Entities
{
    public class AddressDao
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public RegionDao Region { get; set; }
    }

    public class RegionDao
    {
        public int Id { get; set; }
        public string State { get; set; }
        public string District { get; set; }
    }
}
