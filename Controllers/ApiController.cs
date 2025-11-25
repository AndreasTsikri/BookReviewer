
namespace BookReviewer.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/books")]
public class ApiController : Controller
{
    ILogger<ApiController> _logger;
    public ApiController(ILogger<ApiController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetBooks()
    {
        // Return a filtered list of books (stub data for now)
        return Ok(new List<string> { "Book A", "Book B" });
    }

    // GET /api/books/{id} - λεπτομέρειες βιβλίου
    [HttpGet("{id}")]
    public IActionResult GetBookDetails(int id)
    {
        // Return book details by id
        return Ok($"Details of Book {id}");
    }

    // POST /api/books - δημιουργία βιβλίου
    [HttpPost]
    public IActionResult CreateBook([FromBody] string book)
    {
        // Add a new book
        return CreatedAtAction(nameof(GetBookDetails), new { id = 1 }, book); // assuming ID = 1 for example
    }

    // GET /api/books/{id}/reviews - κριτικές του βιβλίου
    [HttpGet("{id}/reviews")]
    public IActionResult GetBookReviews(int id)
    {
        return Ok(new List<string> { "Review 1", "Review 2" });
    }
}
