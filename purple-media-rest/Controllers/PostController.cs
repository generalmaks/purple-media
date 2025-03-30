using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using purple_media_rest.DTO;
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
        IQueryable<Post> query = context.Posts
            .Include(p => p.Author)
            .Include(p => p.LikedBy)
            .Include(p => p.ChildPosts);

        if (sortByDate)
            query = query.OrderByDescending(p => p.CreatedAt);

        var posts = await query.ToListAsync();
        var postDtos = posts.Select(p => p.ToGetPostDto());
        return Ok(postDtos);
    }

    // GET: api/Posts/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetPost(int id)
    {
        var post = await context.Posts
            .Include(p => p.Author)
            .Include(p => p.LikedBy)
            .Where(p => p.PostId == id)
            .FirstOrDefaultAsync();

        if (post == null)
            return NotFound();

        var postDto = post.ToGetPostDto();
        return Ok(postDto);
    }

    [HttpGet("GetByUsername/{username}")]
    public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUsername(string username)
    {
        var posts = await context.Posts
        .Include(p => p.Author)
        .Include(p => p.LikedBy)
        .Where(p => p.Author.Username == username)
        .OrderByDescending(p => p.CreatedAt)
        .ToListAsync();

        var postDtos = posts.Select(p => p.ToGetPostDto());

        return Ok(postDtos);
    }

    // POST: api/Posts
    [HttpPost]
    public async Task<ActionResult<PostPostDTO>> CreatePost(PostPostDTO postDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await context.Users.FindAsync(postDto.AuthorId);
        if (user == null)
            return BadRequest("Author not found.");

        var post = new Post
        {
            AuthorId = postDto.AuthorId,
            Content = postDto.Content,
            ParentPostId = postDto.ParentPostId
        };


        context.Posts.Add(post);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPost), new { id = post.PostId }, post.ToGetPostDto());
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

    // GET: api/likedBy
    [HttpGet("likedBy/{id}")]
    public async Task<ActionResult<IEnumerable<string>>> GetLikedBy(int id)
    {
        var post = await context.Posts
            .Include(p => p.LikedBy)
            .FirstOrDefaultAsync(p => p.PostId == id);
        if (post == null)
        {
            return NotFound("Post was not found");
        }
        var likedBy = post.LikedBy.Select(p => p.Username);
        return Ok(likedBy);
    }

    // Put: api/likedBy/{id}
    [HttpPut("likedBy/{id}/{username}")]
    public async Task<ActionResult> LikePost(int id, string username)
    {
        var post = await context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound("Post was not found");
        }
        var user = await context.Users.FindAsync(username);
        if (user == null)
        {
            return NotFound("User was not found");
        }
        post.LikedBy.Add(user);

        await context.SaveChangesAsync();
        return Ok();
    }

    // GET: api/responses/{id}
    [HttpGet("responses/{id}")]
    public async Task<ActionResult<Post>> GetResponses(int id)
    {
        var post = await context.Posts
            .Include(p => p.ChildPosts)
            .ThenInclude(cp => cp.Author)
            .Include(p => p.ChildPosts)
            .ThenInclude(cp => cp.LikedBy)
            .FirstOrDefaultAsync(p => p.PostId == id);
        if (post == null)
        {
            return NotFound("Post was not found");
        }

        var responses = post.ChildPosts.Select(p => p.ToGetPostDto());
        return Ok(responses);
    }
}