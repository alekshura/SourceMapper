using Compentio.Example.Autofac.App.Entities;
using Compentio.Example.Autofac.App.Mapper;
using Compentio.Example.Autofac.App.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Compentio.Example.Autofac.App.Services
{
    [ExcludeFromCodeCoverage]
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IBooksMapper _booksMapper;

        public BooksService(IBooksRepository booksRepository, IBooksMapper booksMapper)
        {
            _booksRepository = booksRepository;
            _booksMapper = booksMapper;
        }

        public async Task<BookDto> CreateBook(BookDto book)
        {
            //var bookDao = _booksMapper.MapBookToDao(book);
            //var bookResult = await _booksRepository.CreateBook(bookDao);
            //return _booksMapper.MapBookToDto(bookResult);
            return null;
        }

        public async Task<BookDto> GetBook(Guid bookId)
        {
            var bookResult = await _booksRepository.GetBook(bookId);
            return _booksMapper.MapBookToDto(bookResult);
        }

        public async Task<IEnumerable<BookDto>> GetBooks()
        {
            var booksResult = await _booksRepository.GetBooks();
            return booksResult.Select(bookDao => _booksMapper.MapBookToDto(bookDao));
        }

        public async Task<BookDto> UpdateBook(BookDto book)
        {
            //var bookDao = _booksMapper.MapBookToDao(book);
            //var updateResult = await _booksRepository.UpdateBook(bookDao);
            //return _booksMapper.MapBookToDto(updateResult);
            return null;
        }
    }
}