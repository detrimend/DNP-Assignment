using ApiContracts;
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
    public async Task<ActionResult<PostDto>> AddAsync([FromBody] CreatePostDto request)
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
            PostDto dto = new()
            {
                Id = created.Id,
                Title = created.Title,
                Body = created.Body,
                UserId = created.UserId
            };
            return Created($"/Posts/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PostDto>> UpdateAsync([FromBody] PostDto request)
    {
        try
        {
            Post post = new()
            {
                Id = request.Id,
                Title = request.Title,
                Body = request.Body,
                UserId = request.UserId
            };
            await postRepo.UpdateAsync(post);
            Post updated = await postRepo.GetSingleAsync(request.Id);
            PostDto dto = new()
            {
                Id = updated.Id,
                Title = updated.Title,
                Body = updated.Body,
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

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetSingleAsync(int id, [FromQuery] bool includeComments = true)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            if (post == null)
                return NotFound();
            PostDto dto = new()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId
            };
            // Mangler kommentarer... Mangler at parse dem fra repository, ved 
            // ikke om der findes en bedre løsning
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
    public ActionResult<IEnumerable<PostDto>> GetMany(
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
}