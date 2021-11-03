using Compentio.Example.WebAPI.Entities;
using Compentio.Example.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Compentio.Example.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksServices;

        public BooksController(IBooksService booksService)
        {
            _booksServices = booksService;
        }

        [HttpGet]
        public IEnumerable<BookDto> Get()
        {
            var books = _booksServices.GetBooks();

            return books.Result;
        }
    }
}