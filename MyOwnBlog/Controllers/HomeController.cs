using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;
using System.Diagnostics;

namespace MyBlog.Controllers;
public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Posts.Include(p => p.User).Include(p => p.Category).Include(p => p.Tags);
        return View(await appDbContext.ToListAsync());
    }
    public async Task<IActionResult> Post(Guid? id)
    {
        if (id == null || _context.Posts == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Category)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
