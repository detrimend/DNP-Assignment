using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDto>> AddAsync([FromBody] CreateUserDto request)
    {
        try
        {
            await userRepo.VerifyUserNameIsAvailableAsync(request.UserName);

            User user = new ()
            {
                UserName = request.UserName,
                Password = request.Password
            };
            User created = await userRepo.AddAsync(user);
            UserDto dto = new()
            {
                Id = created.Id,
                UserName = created.UserName
            };
            return Created($"/Users/{dto.Id}", dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateAsync([FromBody] UpdateUserDto request)
    {
        try
        {
            User user = new()
            {
                Id = request.Id,
                UserName = request.UserName,
                Password = request.Password
            };
            await userRepo.UpdateAsync(user);
            User updated = await userRepo.GetSingleAsync(request.Id);
            UserDto dto = new()
            {
                Id = updated.Id,
                UserName = updated.UserName
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
            await userRepo.DeleteAsync(id);
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
    public async Task<ActionResult<UserDto>> GetSingleAsync(int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            if (user == null)
                return NotFound();
            UserDto dto = new()
            {
                Id = user.Id,
                UserName = user.UserName
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
    public ActionResult<IEnumerable<UserDto>> GetMany([FromQuery] string userName = null)
    {
        try
        {
            var users = userRepo.GetMany();
            if (!string.IsNullOrEmpty(userName))
            {
                users = users.Where(u => u.UserName == userName);
            }
            var dtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName
            });
            return Ok(dtos);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}