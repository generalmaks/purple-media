using Microsoft.EntityFrameworkCore;
using purple_media_rest.Models;

namespace purple_media_rest.Repositories;

public class FileRepository(ApplicationDbContext context)
{
    public Task<List<int>> ListImageIdAsync()
    {
        var results = context.FileAttachment
            .Select(f => f.FileId).ToList();

        return Task.FromResult(results);
    }

    public async Task<FileAttachment> GetFileAsync(int id)
    {
        var result = await context.FileAttachment
            .FindAsync(id);

        return result ?? throw new Exception("File was not .");
    }

    public async Task AddFileAsync(IFormFile file, int? postId, string ownerUserName)
    {
        if (file == null || file.Length == 0)
            throw new Exception("No file provided.");

        var owner = await context.Users.FindAsync(ownerUserName);
        if (owner is null)
            throw new Exception("Owner not found.");

        byte[] fileData;

        await using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            fileData = ms.ToArray();
        }

        var attachment = new FileAttachment()
        {
            Name = file.FileName,
            DataFormat = file.ContentType,
            Data = fileData,
            PostId = postId,
            OwnerId = owner.Username,
            Owner = owner
        };
        
        await context.FileAttachment.AddAsync(attachment);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var file = await context.FileAttachment.FindAsync(id);
        if (file is null)
        {
            throw new Exception("File was not found.");
        }
        context.FileAttachment.Remove(file);
        await context.SaveChangesAsync();
    }
}