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
    public async Task<ActionResult<Comment>> AddComment([FromBody] Comment request)
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
            return Created($"/Comments/{created.Id}", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Comment>> UpdateComment(int id, [FromBody] Comment request)
    {
        try
        {
            Comment comment = new()
            {
                Id = id,
                Body = request.Body,
                PostId = request.PostId,
                UserId = request.UserId
            };
            await commentRepo.UpdateAsync(comment);
            Comment updated = await commentRepo.GetSingleAsync(id);
            return Ok(updated);
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
    public async Task<ActionResult<Comment>> GetComment(int id)
    {
        try
        {
            Comment comment = await commentRepo.GetSingleAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
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
    public ActionResult<IEnumerable<Comment>> GetComments(
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
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
}