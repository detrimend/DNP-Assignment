using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task UpdateUserAsync(int id, UpdateUserDto request);
    Task DeleteAsync(int id);
    Task<UserDto> GetSingleAsync(int id);
    IQueryable<UserDto> GetMany();
}