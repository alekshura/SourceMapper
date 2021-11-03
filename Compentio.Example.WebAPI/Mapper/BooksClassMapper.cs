using Compentio.Example.WebAPI.Entities;
using Compentio.SourceMapper.Attributes;

namespace Compentio.Example.WebAPI.Mapper
{
    [Mapper(ClassName = "BooksMappings")]
    public abstract class BooksClassMapper : IBooksMapper
    {
        [Mapping(Source = nameof(AddressDto.PostalCode), Target = nameof(AddressDao.PostCode))]
        [Mapping(Source = nameof(AddressDto.Home), Target = nameof(AddressDao.HomeNumber))]
        [Mapping(Source = nameof(AddressDto.AddressId), Target = nameof(AddressDao.Id))]
        public abstract AddressDao MapAddressToDao(AddressDto source);

        [Mapping(Source = nameof(AddressDao.PostCode), Target = nameof(AddressDto.PostalCode))]
        [Mapping(Source = nameof(AddressDao.HomeNumber), Target = nameof(AddressDto.Home))]
        [Mapping(Source = nameof(AddressDao.Id), Target = nameof(AddressDto.AddressId))]
        public abstract AddressDto MapAddressToDto(AddressDao source);

        [Mapping(Source = nameof(BookDto.BookDescription), Target = nameof(BookDao.Description))]
        [Mapping(Source = nameof(BookDto.BookId), Target = nameof(BookDao.Id))]
        public abstract BookDao MapBookToDao(BookDto source);

        [Mapping(Source = nameof(BookDao.Description), Target = nameof(BookDto.BookDescription))]
        [Mapping(Source = nameof(BookDao.Id), Target = nameof(BookDto.BookId))]
        public abstract BookDto MapBookToDto(BookDao source);
    }
}