using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddAsync(LoginDto request);
    Task UpdateAsync(int id, UpdateUserDto request);
    Task DeleteAsync(int id);
    Task<UserDto> GetSingleAsync(int id);
    IQueryable<UserDto> GetMany();
}