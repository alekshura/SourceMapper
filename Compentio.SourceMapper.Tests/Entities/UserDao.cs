using System;
using System.Collections.Generic;

namespace Compentio.SourceMapper.Tests.Entities
{
    public class UserDao
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserGender UserGender { get; set; }
        public string City { get; set; }
        public string House { get; set; }
        public string ZipCode { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class UserDataDao
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserGender UserGender { get; set; }
        public AddressDao UserAddress { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class UserWithArrayDao
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressDao[] UserAddresses { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class UserWithListDao
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<AddressDao> UserAddresses { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class UserDataWithListDao
    {
        public long UserId { get; set; }
        public IList<AddressDao> UserAddresses { get; set; }
    }

    public enum UserGender { Female, Male }
}
