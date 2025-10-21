using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepo;

    public PostsController(IPostRepository postRepo)
    {
        this.postRepo = postRepo;
    }

    [HttpPost]
    public async Task<ActionResult<Post>> AddPost([FromBody] Post request)
    {
        try
        {
            Post post = new()
            {
                Title = request.Title,
                Body = request.Body,
                UserId = request.UserId
            };
            Post created = await postRepo.AddAsync(post);
            return Created($"/Posts/{created.Id}", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Post>> UpdatePost(int id, [FromBody] Post request)
    {
        try
        {
            Post post = new()
            {
                Id = id,
                Title = request.Title,
                Body = request.Body,
                UserId = request.UserId
            };
            await postRepo.UpdateAsync(post);
            Post updated = await postRepo.GetSingleAsync(id);
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
    public async Task<ActionResult<Post>> GetPost(int id, [FromQuery] bool includeComments = true)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            if (post == null)
                return NotFound();
            return Ok(post);
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
    public ActionResult<IEnumerable<Post>> GetPosts(
        [FromQuery] string titleContains = null,
        [FromQuery] int? userId = null)
    {
        try
        {
            var posts = postRepo.GetMany();

            if (!string.IsNullOrEmpty(titleContains))
                posts = posts.Where(p => p.Title != null && p.Title.Contains(titleContains));

            if (userId.HasValue)
                posts = posts.Where(p => p.UserId == userId.Value);

            return Ok(posts.ToList());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            await postRepo.DeleteAsync(id);
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