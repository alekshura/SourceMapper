using Compentio.Example.Autofac.App.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compentio.Example.Autofac.App.Repository
{
    public interface IBooksRepository
    {
        Task<IEnumerable<BookDao>> GetBooks();

        Task<BookDao> GetBook(Guid bookId);

        Task<BookDao> CreateBook(BookDao book);

        Task<BookDao> UpdateBook(BookDao book);
    }
}