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
    public async Task<ActionResult<IEnumerable<object>>> GetPosts([FromQuery] bool sortByDate = true)
    {
        var query = context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Include(p => p.LikedBy)
            .Select(p => new
            {
                p.PostId,
                p.Content,
                p.CreatedAt,
                p.User.Username,
                p.User.ProfilePicturePath,
                p.CommentsCount,
                p.Likes
            });

        if (sortByDate)
            query = query.OrderByDescending(p => p.CreatedAt);

        var posts = await query.ToListAsync();
        return Ok(posts);
    }

    // GET: api/Posts/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetPost(int id)
    {
        var post = await context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Include(p => p.LikedBy)
            .Where(p => p.PostId == id)
            .Select(p => new
            {
                p.PostId,
                p.Content,
                p.CreatedAt,
                p.User.Username,
                p.CommentsCount,
                p.Likes
            })
            .FirstOrDefaultAsync();

        if (post == null)
            return NotFound();

        return Ok(post);
    }

    [HttpGet("GetByUsername/{username}")]
    public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUsername(string username)
    {
        var posts = await context.Posts
        .Include(p => p.User)
        .Include(p => p.Comments)
        .Include(p => p.LikedBy)
        .Where(p => p.User.Username == username)
        .Select(p => new
        {
            p.PostId,
            p.Content,
            p.CreatedAt,
            p.User.Username,
            p.CommentsCount,
            p.Likes
        })
        .OrderByDescending(p => p.CreatedAt)
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