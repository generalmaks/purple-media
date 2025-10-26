using Microsoft.AspNetCore.Mvc;
using purple_media_rest.DTO;
using purple_media_rest.Models;
using purple_media_rest.Repositories;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController(PostRepository postRepository) : ControllerBase
{
    // GET: api/Posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPostDto>>> GetPosts([FromQuery] bool sortByDate = true)
    {
        try
        {
            var posts = await postRepository.GetAllPosts(sortByDate);
            return Ok(posts);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    // GET: api/PostsWithoutParents
    [HttpGet("PostsWithoutParents/{id:int}")]
    public async Task<ActionResult<IEnumerable<GetPostDto>>> GetPostsWithoutParents([FromQuery] bool sortByDate = true)
    {
        try
        {
            var posts = await postRepository.GetAllPostsWithoutParents(sortByDate);
            return Ok(posts);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    // GET: api/Posts/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPostDto>> GetPost(int id)
    {
        try
        {
            var post = await postRepository.GetPost(id);
            return Ok(post);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetByUsername/{username}")]
    public async Task<ActionResult<IEnumerable<GetPostDto>>> GetPostsByUsername(string username)
    {
        try
        {
            var posts = await postRepository.GetPostsByAuthorUsername(username);
            return Ok(posts);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/Posts
    [HttpPost]
    public async Task<ActionResult<PostPostDTO>> CreatePost(PostPostDTO postPostDto)
    {
        try
        {
            await postRepository.CreatePost(postPostDto);
            return Ok(new { message = "Post created"});
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // DELETE: api/Posts/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            await postRepository.DeletePost(id);
            return Ok( new { message = "Post has been deleted"});
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/likedBy
    [HttpGet("likedBy/{id:int}")]
    public async Task<ActionResult<IEnumerable<string>>> GetLikedBy(int id)
    {
        try
        {
            var likedBy = await postRepository.GetLikedBy(id);
            return Ok(likedBy);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // Put: api/Post/likedBy/{id}
    [HttpPut("LikePost/{id:int}/{username}")]
    public async Task<ActionResult> LikePost(int id, string username)
    {
        try
        {
            await postRepository.LikePost(id, username);
            return Ok(new { message = "Post has been liked/unliked"});
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/Post/responses/{id}
    [HttpGet("responses/{id:int}")]
    public async Task<ActionResult<Post>> GetResponses(int id)
    {
        try
        {
            var responses = await postRepository.GetResponses(id);
            return Ok(responses);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    // GET: api/Post/findBySnippet
    [HttpGet("search/{snippet}")]
    public async Task<ActionResult<List<PostSearchResultDto>>> FindBySnippet(string snippet)
    {
        try
        {
            var results = await postRepository.FindBySnippet(snippet);
            return Ok(results);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}