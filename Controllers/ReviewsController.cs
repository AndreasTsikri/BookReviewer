using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookReviewer.Models;
using BookReviewer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BookReviewer.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly ILogger<ReviewsController> _logger;
    readonly ApplicationDbContext _ctx;

    public ReviewsController(ILogger<ReviewsController> logger, ApplicationDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Get(int? id)
    {
        if (id == null)
            return BadRequest();
        //var reviews = await _ctx.Reviews.AsNoTracking().ToListAsync();
        //return Views(reviews);
    }
    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id == null)
            return BadRequest();
        //var rev = await _ctx.Reviews.AsNoTracking().FindAsync(id);
        //if(rev == null)
        //NotFound();
        //return Views(rev);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int? id, [Bind("Id, Rating, Comment")] Review rev)
    {
        if (id == null)
            return BadRequest();
        if (id != rev.Id)
            return NotFound();
         if (ModelState.IsValid)
        {
            try
            {
                rev.CreatedAt = DateTime.UtcNow;
                _ctx.Update(rev);
                await _ctx.SaveChangesAsync();
                ViewBag.Success = true;        
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _ctx.Reviews.AnyAsync(b => b.Id == rev.Id);
                if (!exists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        return RedirectToAction(nameof(Index));
    }
    return View(rev);
        

    } 

}
