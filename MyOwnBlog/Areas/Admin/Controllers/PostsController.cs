using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;

namespace MyBlog.Areas.Admin.Controllers;

[Authorize(Roles = "Administrators")]
[Area("Admin")]
public class PostsController : BaseController
{
    private readonly AppDbContext _context;

    public PostsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Posts
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Posts.Include(p => p.Category).Include(p => p.Tags).Include(p => p.User);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Admin/Posts/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Posts == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Category)
            .Include(p => p.User)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // GET: Admin/Posts/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        ViewData["Tags"] = new SelectList(_context.Tags, "Id", "Name");
        return View();
    }

    // POST: Admin/Posts/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePostViewModel post)
    {
        var postToCreate = new Post
        {
            Title = post.Title,
            Content = post.Content,
            CategoryId = post.CategoryId,
            Tags = _context.Tags.Where(p => post.TagIds.Any(q => q == p.Id)).ToList(),
            UserId = UserId!.Value,
        };

        _context.Add(postToCreate);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/Posts/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Posts == null)
        {
            return NotFound();
        }

        Post? post = await _context.Posts.Include(post => post.Tags).FirstOrDefaultAsync(post => post.Id == id);
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
        ViewData["Tags"] = new MultiSelectList(_context.Tags, "Id", "Name", post.Tags.Select(tag => tag.Id).ToArray());
        return View(new EditPostViewModel
        {
            Title = post.Title,
            Content = post.Content,
            Tags = post.Tags.ToList(),
            CategoryId = post.CategoryId
        });
    }

    // POST: Admin/Posts/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditPostViewModel post)
    {

        if (id != post.Id)
        {
            return NotFound();
        }
        try
        {
            Post? postToUpdate = await _context.Posts.Include(post => post.Tags).FirstOrDefaultAsync(post => post.Id == id);

            foreach (var tag in postToUpdate.Tags.ToList())
            {
                post.Tags.Remove(tag);
            }

            postToUpdate.CategoryId = post.CategoryId;
            postToUpdate.Title = post.Title;
            postToUpdate.Content = post.Content;
            postToUpdate.Tags.Clear();
            postToUpdate.Tags = _context.Tags.Where(p => post.TagIds.Any(q => q == p.Id)).ToList();
            _context.Update(postToUpdate);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PostExists(post.Id))
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

    // GET: Admin/Posts/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Posts == null)
        {
            return NotFound();
        }

        var post = await _context.Posts
            .Include(p => p.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // POST: Admin/Posts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Posts == null)
        {
            return Problem("Entity set 'AppDbContext.Posts'  is null.");
        }
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PostExists(Guid id)
    {
        return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}