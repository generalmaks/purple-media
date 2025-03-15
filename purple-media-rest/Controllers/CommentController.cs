using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using purple_media_rest;
using purple_media_rest.Models;

[ApiController]
[Route("api/[controller]")]

public class CommentController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/Comment
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments()
    {
        var comments = await _context.Comments
            .Include(c => c.Author)
            .Select(c => c.ToDTO())
            .ToListAsync();

        return Ok(comments);
    }

    // GET: api/Comment/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDTO>> GetComment(int id)
    {
        var comment = await _context.Comments
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.CommentId == id);

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment.ToDTO());
    }

    // GET: api/Comment/ByPost/5
    [HttpGet("ByPost/{postId}")]
    public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsByPost(int postId)
    {
        var comments = await _context.Comments
            .Include(c => c.Author)
            .Where(c => c.PostId == postId)
            .Select(c => c.ToDTO())
            .ToListAsync();

        return Ok(comments);
    }

    // POST: api/Comment
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CommentDTO>> CreateComment(CreateCommentDTO createCommentDTO)
    {
        // Verify the post exists
        var post = await _context.Posts.FindAsync(createCommentDTO.PostId);
        if (post == null)
        {
            return BadRequest("Post does not exist");
        }

        // Verify the user exists
        var user = await _context.Users.FindAsync(createCommentDTO.AuthorId);
        if (user == null)
        {
            return BadRequest("User does not exist");
        }

        var comment = new Comment
        {
            PostId = createCommentDTO.PostId,
            Content = createCommentDTO.Content,
            AuthorId = createCommentDTO.AuthorId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        // Reload the comment with Author included
        await _context.Entry(comment).Reference(c => c.Author).LoadAsync();

        return CreatedAtAction(
            nameof(GetComment),
            new { id = comment.CommentId },
            comment.ToDTO());
    }

    // PUT: api/Comment/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, UpdateCommentDTO updateCommentDTO)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        // Optional: Check if the user has permission to update this comment
        // if (comment.AuthorId != currentUserId) return Forbid();

        // Only update the content, not other properties
        comment.Content = updateCommentDTO.Content;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CommentExists(id))
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

    // DELETE: api/Comment/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        // Optional: Check if the user has permission to delete this comment
        // if (comment.AuthorId != currentUserId) return Forbid();

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CommentExists(int id)
    {
        return _context.Comments.Any(e => e.CommentId == id);
    }
}

// DTO for creating a new comment
public class CreateCommentDTO
{
    public int PostId { get; set; }
    public string Content { get; set; }
    public int AuthorId { get; set; }
}

// DTO for updating an existing comment
public class UpdateCommentDTO
{
    public string Content { get; set; }
}

// Assuming this is the CommentDTO class based on the ToDTO method in Comment
public class CommentDTO
{
    public int CommentId { get; set; }
    public int PostId { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public DateTime CreatedAt { get; set; }
}