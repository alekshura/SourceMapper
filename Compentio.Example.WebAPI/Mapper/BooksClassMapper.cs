using Compentio.Example.WebAPI.Entities;
using Compentio.SourceMapper.Attributes;

namespace Compentio.Example.WebAPI.Mapper
{
    [Mapper(ClassName = "BooksMappings")]
    public abstract class BooksClassMapper : IBooksMapper
    {
        [Mapping(Source = nameof(AddressDto.PostalCode), Target = nameof(AddressDao.PostCode))]
        public abstract AddressDao MapAddressToDao(AddressDto source);

        [Mapping(Source = nameof(AddressDao.PostCode), Target = nameof(AddressDto.PostalCode))]
        public abstract AddressDto MapAddressToDto(AddressDao source);

        [Mapping(Source = nameof(BookDto.BookDescription), Target = nameof(BookDao.Description))]
        public abstract BookDao MapBookToDao(BookDto source);

        [Mapping(Source = nameof(BookDao.Description), Target = nameof(BookDto.BookDescription))]
        public abstract BookDto MapBookToDto(BookDao source);
    }
}