using System;

namespace Compentio.SourceMapper.Tests.Entities
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; }
        public DateTime BirthDate { get; set; }

    }

    public enum Gender { Male, Female }
}
