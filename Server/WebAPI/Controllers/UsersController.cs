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
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
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
            return Created($"/Users/{dto.Id}", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}