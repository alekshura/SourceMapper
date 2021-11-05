using Compentio.Example.Autofac.App.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compentio.Example.Autofac.App.Repository
{
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
            PublishingYear = 1961
        };
    }
}