using System;

namespace Compentio.SourceMapper.Tests.Entities
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Address Address { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class UserInfoWithArray
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Address[] Addresses { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public enum Sex { M, W }
}
