using Compentio.Example.WebAPI.Entities;
using Compentio.SourceMapper.Attributes;

namespace Compentio.Example.WebAPI.Mapper
{
    [Mapper]
    public interface IBooksMapper
    {
        [Mapping(Source = nameof(BookDao.Description), Target = nameof(BookDto.BookDescription))]
        BookDto MapBookToDto(BookDao source);

        [Mapping(Source = nameof(BookDto.BookDescription), Target = nameof(BookDao.Description))]
        BookDao MapBookToDao(BookDto source);

        [Mapping(Source = nameof(AddressDao.PostCode), Target = nameof(AddressDto.PostalCode))]
        AddressDto MapAddressToDto(AddressDao source);

        [Mapping(Source = nameof(AddressDto.PostalCode), Target = nameof(AddressDao.PostCode))]
        AddressDao MapAddressToDao(AddressDto source);
    }
}