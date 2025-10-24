using Microsoft.AspNetCore.Mvc;
using purple_media_rest.DTO;
using purple_media_rest.Models;
using purple_media_rest.Repositories;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly FileRepository _repo;
    private readonly ApplicationDbContext _context;

    public FileController(ApplicationDbContext context, FileRepository repo)
    {
        _context = context;
        this._repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult> ListFileId()
    {
        try
        {
            var filesId = await _repo
                .ListImageIdAsync();
            return Ok(filesId);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetFile(int id)
    {
        try
        {
            var file = await _repo.GetFileAsync(id);
            return File(file.Data, file.DataFormat, file.Name);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteFile(int id)
    {
        try
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostFile(
        [FromForm] FileUploadDTO file
            )
    {
        try
        {
            await _repo.AddFileAsync(file.File, file.PostId, file.OwnerUserName);

            return Ok("File was created.");            
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}