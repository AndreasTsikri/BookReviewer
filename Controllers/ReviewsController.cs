using Microsoft.AspNetCore.Mvc;
using BookReviewer.Models;
using BookReviewer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using SQLitePCL;
using Microsoft.AspNetCore.Identity;

namespace BookReviewer.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly ILogger<ReviewsController> _logger;
    readonly ApplicationDbContext _ctx;
    UserManager<IdentityUser> _userManager;

    public ReviewsController(ILogger<ReviewsController> logger, ApplicationDbContext ctx, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _ctx = ctx;
        _userManager = userManager;
    }
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get(int? id)
    {
        if (id == null)
            return BadRequest();        
        var b = await _ctx.Books.Include(book => book.Reviews).FirstOrDefaultAsync(b=> b.Id == id);
        if(b == null)
            return NotFound();
        var vm = new ReviewsViewModel
        {
            BookId = (int) id,
            BookTitle = b.Title,
            BookAuthor= b.Author,
            BookPublishedYear = b.PublishedYear,
            Reviews  = b.Reviews
        };

        return View(nameof(Index), vm);
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create(int? id)
    {
         if (id == null)
            return BadRequest();
        
        // var b = await _ctx.Books.AsNoTracking().Where(b => b.Id == id).FirstOrDefaultAsync();

        // if(b == null)
        //     return NotFound();

        ViewBag.Success = false;
        ViewBag.Faillure = false;
        

        var r = new ReviewViewModel
        {
            BookId = (int)id,
            //UserId = "1" //_userManager.GetUserIdAsync()...
            //Book = b
        };
        return View(r);
    }   

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] ReviewViewModel rvm)//[Bind("Rating, Comment, BookId")] Review rvm */)
    {
         ViewBag.Success = false;
         ViewBag.Faillure = false;
        if (!ModelState.IsValid)
        {
            ViewBag.Faillure = true;
            return View(rvm);   
        }
        var r = new Review
        {
            Rating = rvm.Rating,
            Comment = rvm.Comment,
            BookId = rvm.BookId,
        };
        await _ctx.Reviews.AddAsync(r);
        await _ctx.SaveChangesAsync();
        ViewBag.Success = true;
        return View(rvm);
    }
    
    private async Task<int> simulateInsert()
    {
        await Task.Delay(5000);
        return 5;
    }
    // public async Task<IActionResult> Edit(int? id, [Bind("Id, Rating, Comment")] Review rev)
    // {
    //     if (id == null)
    //         return BadRequest();
    //     if (id != rev.Id)
    //         return NotFound();
    //     int test = await simulateInsert();
    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             rev.CreatedAt = DateTime.UtcNow;
    //             _ctx.Update(rev);
    //             await _ctx.SaveChangesAsync();
    //             ViewBag.Success = true;
    //         }
    //         catch (DbUpdateConcurrencyException)
    //         {
    //             var exists = await _ctx.Reviews.AnyAsync(b => b.Id == rev.Id);
    //             if (!exists)
    //             {
    //                 return NotFound();
    //             }
    //             else
    //             {
    //                 throw;
    //             }
    //         }
    //         return RedirectToAction(nameof(Index));
    //     }
    //     return View(rev);

    // }

}
