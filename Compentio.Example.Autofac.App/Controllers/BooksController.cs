using Compentio.Example.Autofac.App.Entities;
using Compentio.Example.Autofac.App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Compentio.Example.Autofac.App.Controllers
{
    [ExcludeFromCodeCoverage]
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