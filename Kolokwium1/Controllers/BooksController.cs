using System.Transactions;
using Kolokwium1.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium1.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("{id}/authors")]
        public async Task<IActionResult> GetBookById(int id)
        {
            if (!await _bookRepository.DoesBookExist(id))
                return NotFound();
            var book = await _bookRepository.GetBookById(id);
            return Ok(book);
        }
    }
}