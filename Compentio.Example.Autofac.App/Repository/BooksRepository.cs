using Compentio.Example.Autofac.App.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Compentio.Example.Autofac.App.Repository
{
    [ExcludeFromCodeCoverage]
    public class BooksRepository : IBooksRepository
    {
        public async Task<BookDao> CreateBook(BookDao book)
        {
            return await Task.FromResult(_bookDao);
        }

        public async Task<BookDao> GetBook(Guid bookId)
        {
            return await Task.FromResult(_bookDao);
        }

        public async Task<IEnumerable<BookDao>> GetBooks()
        {
            var booksList = new List<BookDao>();
            booksList.Add(_bookDao);

            return await Task.FromResult(booksList);
        }

        public async Task<BookDao> UpdateBook(BookDao book)
        {
            return await Task.FromResult(_bookDao);
        }

        private readonly BookDao _bookDao = new BookDao()
        {
            Id = Guid.NewGuid(),
            Author = "Lem, S",
            Title = "Solaris",
            PublishingYear = 1961,
            LibraryAddressesDao = new List<AddressDao>{ _addressDao, _addressDao, _addressDao }
        };

        private static readonly AddressDao _addressDao = new AddressDao()
        {
            City = "City",
            Country = "Country",
            HomeNumber = "HomeNumber",
            Id = Guid.NewGuid(),
            PostCode = "PostCode",
            Region = " Region",
            Street = "Street"
        };
    }
}