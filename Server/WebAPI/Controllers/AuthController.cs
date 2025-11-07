using Microsoft.AspNetCore.Mvc;
using ApiContracts;
using RepositoryContracts;
using Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        // Find user by username
        var user = userRepository.GetMany().FirstOrDefault(u => u.UserName == loginDto.UserName);
        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        // Check password
        if (user.Password != loginDto.Password)
        {
            return Unauthorized("Invalid username or password.");
        }

        // Map to UserDto
        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };

        return Ok(userDto);
    }
}