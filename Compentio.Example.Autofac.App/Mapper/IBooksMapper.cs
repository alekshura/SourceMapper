using Compentio.Example.Autofac.App.Entities;
using Compentio.SourceMapper.Attributes;

namespace Compentio.Example.Autofac.App.Mapper
{
    [Mapper]
    public partial interface IBooksMapper
    {
        [Mapping(Source = nameof(BookDao.Description), Target = nameof(BookDto.BookDescription))]
        [Mapping(Source = nameof(BookDao.Id), Target = nameof(BookDto.BookId))]
        //[InverseMapping(InverseMethodName = "MapBookToDao")]
        BookDto MapBookToDto(BookDao source);

        [Mapping(Source = nameof(AddressDao.PostCode), Target = nameof(AddressDto.PostalCode))]
        [Mapping(Source = nameof(AddressDao.HomeNumber), Target = nameof(AddressDto.Home))]
        [Mapping(Source = nameof(AddressDao.Id), Target = nameof(AddressDto.AddressId))]
        //[InverseMapping(InverseMethodName = "MapAddressToDao")]
        AddressDto MapAddressToDto(AddressDao source);
    }
}