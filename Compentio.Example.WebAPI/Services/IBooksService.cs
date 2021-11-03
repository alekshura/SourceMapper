using Compentio.Example.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compentio.Example.WebAPI.Services
{
    public interface IBooksService
    {
        Task<IEnumerable<BookDto>> GetBooks();

        Task<BookDto> GetBook(Guid bookId);

        Task<BookDto> CreateBook(BookDto book);

        Task<BookDto> UpdateBook(BookDto book);
    }
}