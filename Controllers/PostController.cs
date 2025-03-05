using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using purple_media_rest.Models;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController(ApplicationDbContext context) : ControllerBase
{
    // GET: api/Posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetPosts([FromQuery] bool sortByDate = true)
    {
        var posts = await context.Posts
        .Include(p => p.User)  // Joins the User table
        .OrderByDescending(p => p.CreatedAt)
        .Select(p => new
        {
            p.PostId,
            p.Title,
            p.Content,
            p.CreatedAt,
            Username = p.User.Username  // Get the username from the User table
        })
        .ToListAsync();

        return Ok(posts);
    }

    // GET: api/Posts/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        return await context.Posts
        .Include(p => p.User)
        .Where(p => p.PostId == id)
        .Select(p => new Post
        {
            PostId = p.PostId,
            Title = p.Title,
            Content = p.Content,
            User = new User
            {
                UserId = p.User.UserId,
                Username = p.User.Username
            }
        })
        .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }

    [HttpGet("GetByUsername/{username}")]
    public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUsername(string username)
    {
        var posts = await context.Posts
        .Include(p => p.User)
        .Where(p => p.User.Username == username)
        .Select(p => new
        {
            p.PostId,
            p.Title,
            p.Content,
            p.CreatedAt,
            Username = p.User.Username
        })
        .ToListAsync();

        return Ok(posts);
    }

    // POST: api/Posts
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost(Post post)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await context.Users.FindAsync(post.UserId);
        if (user == null)
            return BadRequest("User not found.");

        context.Posts.Add(post);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPost), new { id = post.PostId }, post);
    }

    // PUT: api/Posts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPost(int id, Post post)
    {
        if (id != post.PostId)
        {
            return BadRequest();
        }

        context.Entry(post).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Posts.Any(e => e.PostId == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Posts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        context.Posts.Remove(post);
        await context.SaveChangesAsync();

        return NoContent();
    }
}