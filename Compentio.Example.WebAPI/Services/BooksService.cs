using Compentio.Example.WebAPI.Entities;
using Compentio.Example.WebAPI.Mapper;
using Compentio.Example.WebAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compentio.Example.WebAPI.Services
{
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
            var bookDao = _booksMapper.MapBookToDao(book);
            var bookResult = await _booksRepository.CreateBook(bookDao);
            return _booksMapper.MapBookToDto(bookResult);
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
            var bookDao = _booksMapper.MapBookToDao(book);
            var updateResult = await _booksRepository.UpdateBook(bookDao);
            return _booksMapper.MapBookToDto(updateResult);
        }
    }
}