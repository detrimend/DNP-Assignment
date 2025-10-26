using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;

    public CommentsController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddAsync([FromBody] CreateCommentDto request)
    {
        try
        {
            Comment comment = new()
            {
                Body = request.Body,
                PostId = request.PostId,
                UserId = request.UserId
            };
            Comment created = await commentRepo.AddAsync(comment);
            CommentDto dto = new()
            {
                Id = created.Id,
                Body = created.Body,
                PostId = created.PostId,
                UserId = created.UserId
            };
            return Created($"/Comments/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> UpdateAsync([FromBody] CommentDto request)
    {
        try
        {
            Comment comment = new()
            {
                Id = request.Id,
                Body = request.Body,
                PostId = request.PostId,
                UserId = request.UserId
            };
            await commentRepo.UpdateAsync(comment);
            Comment updated = await commentRepo.GetSingleAsync(request.Id);
            CommentDto dto = new()
            {
                Id = updated.Id,
                Body = updated.Body,
                PostId = updated.PostId,
                UserId = updated.UserId
            };
            return Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetSingleAsync(int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            if (comment == null)
                return NotFound();
            CommentDto dto = new()
            {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                UserId = comment.UserId
            };
            return Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetMany(
        [FromQuery] int? userId = null,
        [FromQuery] int? postId = null)
    {
        try
        {
            var comments = commentRepo.GetMany();

            if (userId.HasValue)
                comments = comments.Where(c => c.UserId == userId.Value);

            if (postId.HasValue)
                comments = comments.Where(c => c.PostId == postId.Value);

            return Ok(comments.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}