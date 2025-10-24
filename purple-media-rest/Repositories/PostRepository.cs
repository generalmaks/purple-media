using Microsoft.EntityFrameworkCore;
using purple_media_rest.DTO;
using purple_media_rest.Extensions;
using purple_media_rest.Models;

namespace purple_media_rest.Repositories;

public class PostRepository(ApplicationDbContext context)
{
    public async Task<IEnumerable<GetPostDto>> GetAllPosts(bool sortByDate=true)
    {
        var posts = await context.Posts
            .Include(p => p.LikedBy)
            .ToListAsync();

        if (sortByDate)
            posts = posts.OrderByDescending(p => p.CreatedAt).ToList();

        var postsDto = posts.Select(p => p.ToGetPostDto()).ToList();
        return postsDto;
    }

    public async Task<IEnumerable<GetPostDto>> GetAllPostsWithoutParents(bool sortByDate = true)
    {
        var posts = await context.Posts
            .Where(p => p.ParentPost == null)
            .Include(p => p.LikedBy)
            .ToListAsync();

        if (sortByDate)
            posts = posts.OrderByDescending(p => p.CreatedAt).ToList();

        var postsDto = posts.Select(p => p.ToGetPostDto()).ToList();
        return postsDto;
    }

    public async Task<GetPostDto> GetPost(int id)
    {
        var post = await context.Posts
            .FirstOrDefaultAsync(p => p.PostId == id);

        if (post == null)
            throw new Exception("Post not found.");
        
        var postDto = post.ToGetPostDto();

        return postDto;
    }

    public async Task<IEnumerable<GetPostDto>> GetPostsByAuthorUsername(string username)
    {
        var posts = await context.Posts
            .Where(p => p.AuthorId == username)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var postsDto = posts
            .Select(p => p.ToGetPostDto())
            .ToList();

        return postsDto;
    }

    public async Task CreatePost(PostPostDTO postPostDto)
    {
        var user = await context.Users
            .FindAsync(postPostDto.AuthorId);
        if (user == null)
            throw new Exception($"User {postPostDto.AuthorId} was not found");

        var post = new Post
        {
            AuthorId = postPostDto.AuthorId,
            Content = postPostDto.Content,
            ParentPostId = postPostDto.ParentPostId
        };

        context.Posts.Add(post);
        await context.SaveChangesAsync();
    }
    
    public async Task DeletePost(int id)
    {
        var post = await context.Posts.FindAsync(id);
        if (post == null)
            throw new Exception($"Post {id} not found.");

        context.Posts.Remove(post);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetLikedBy(int id)
    {
        var post = await context.Posts
            .Include(post => post.LikedBy)
            .FirstOrDefaultAsync(p => p.PostId == id);
        if (post == null)
            throw new Exception($"Post {id} not found.");

        var likedBy = post.LikedBy.Select(p => p.Username);
        return likedBy;
    }

    public async Task LikePost(int id, string username)
    {
        var post = await context.Posts
            .Include(p => p.LikedBy)
            .FirstOrDefaultAsync(p => p.PostId == id);
        if(post == null)
            throw new Exception($"Post {id} not found.");

        var user = await context.Users.FindAsync(username);
        if (user == null)
            throw new Exception($"User {username} not found.");

        var alreadyLiked = post.LikedBy.Any(u => u.Username == username);
        if (alreadyLiked)
            post.LikedBy.Remove(user);
        else
            post.LikedBy.Add(user);

        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<GetPostDto>> GetResponses(int id)
    {
        var post = await context.Posts
            .Include(post => post.ChildPosts)
            .FirstOrDefaultAsync(p => p.PostId == id);
        
        if (post == null)
            throw new Exception($"Post {id} not found.");

        var respones = post.ChildPosts.Select(p => p.ToGetPostDto());
        return respones;
    }

    public async Task<IEnumerable<PostSearchResultDto>> FindBySnippet(string snippet)
    {
        const int minimalSnippetLength = 3;
        if (snippet.Length < minimalSnippetLength)
            throw new Exception(
                $"Snippet is too short. " +
                $"Minimal snippet length is {minimalSnippetLength}. " +
                $"Actualy is {snippet.Length}.");
        
        var snippedArray = snippet.Split(' ', 
            StringSplitOptions.TrimEntries |
            StringSplitOptions.RemoveEmptyEntries);
        
        var posts = await context.Posts
            .Where(p => 
                snippedArray.Any(s => p.Content.Contains(s)))
            .ToListAsync();

        var results = posts
            .Select(p => new PostSearchResultDto(){
                Post = p.ToGetPostDto(),
                Indices = p.Content.AllIndices(snippedArray).ToArray()
            })
            .Where(x => x.Indices.Length > 0)
            .ToList();
        return results;
    }
}