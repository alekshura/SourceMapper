using Compentio.Example.Autofac.App.Entities;
using Compentio.SourceMapper.Attributes;
using System.Linq;

namespace Compentio.Example.Autofac.App.Mapper
{
    [Mapper]
    public partial interface IBooksMapper
    {
        [Mapping(Source = nameof(BookDao.Description), Target = nameof(BookDto.BookDescription))]
        [Mapping(Source = nameof(BookDao.Id), Target = nameof(BookDto.BookId))]
        [InverseMapping(InverseMethodName = "MapBookToDao")]
        BookDto MapBookToDto(BookDao source);

        [Mapping(Source = nameof(AddressDao.PostCode), Target = nameof(AddressDto.PostalCode))]
        [Mapping(Source = nameof(AddressDao.HomeNumber), Target = nameof(AddressDto.Home))]
        [Mapping(Source = nameof(AddressDao.Id), Target = nameof(AddressDto.AddressId))]
        [InverseMapping(InverseMethodName = "MapAddressToDao")]
        AddressDto MapAddressToDto(AddressDao source);
    }

    public class CustomBooksMapper : BooksMapper
    {
        public override BookDao MapBookToDao(BookDto source)
        {
            var result = base.MapBookToDao(source);
            result.LibraryAddressesDao = source.LibraryAddressesDto.Select(a => MapAddressToDao(a)).ToList();
            return result;
        }

        public override BookDto MapBookToDto(BookDao source)
        {
            var result = base.MapBookToDto(source);
            result.LibraryAddressesDto = source.LibraryAddressesDao.Select(a => MapAddressToDto(a)).ToList();
            return result;
        }
    }
}