using Compentio.SourceMapper.Attributes;
using System;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Tests.Entities
{
    public class UserInfo
    {
        public static string UserCodeStatic = "UserCodeStaticInfo";
        public string UserCode = "UserCodeInfo";
        public Address AddressField;
        public int Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Address Address { get; set; }
        public DateTime BirthDate { get; set; }
        [IgnoreMapping]
        public object PropertyToBeIgnored { get; set; }
    }

    public class UserInfoWithArray
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Address[] Addresses { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class UserInfoWithList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public IList<Address> Addresses { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public enum Sex { M, W }
}
