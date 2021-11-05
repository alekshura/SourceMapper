using Compentio.Example.Autofac.App.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compentio.Example.Autofac.App.Services
{
    public interface IBooksService
    {
        Task<IEnumerable<BookDto>> GetBooks();

        Task<BookDto> GetBook(Guid bookId);

        Task<BookDto> CreateBook(BookDto book);

        Task<BookDto> UpdateBook(BookDto book);
    }
}