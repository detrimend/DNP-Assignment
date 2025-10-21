using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        List<User> users = await GetUsersFromFile();
        int maxId = users.Count > 0 ? users.Max(u => u.Id) : 1;
        user.Id = maxId + 1;
        users.Add(user);
        await WriteUsersToFile(users);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        List<User> users = await GetUsersFromFile();
        User? exisitingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (exisitingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        users.Remove(exisitingUser);
        users.Add(user);
        await WriteUsersToFile(users);
    }

    public async Task DeleteAsync(int id)
    {
        List<User> users = await GetUsersFromFile();
        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        users.Remove(userToRemove);
        await WriteUsersToFile(users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        List<User> users = await GetUsersFromFile();
        User? user = users.SingleOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        await WriteUsersToFile(users);
        return user;
    }

    public IQueryable<User> GetMany()
    {
        string usersAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User> users =
            JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }

    private async Task<List<User>> GetUsersFromFile()
    {
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
    }

    private async Task WriteUsersToFile(List<User> users)
    {
        string usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
    }
    
    public async Task VerifyUserNameIsAvailableAsync(string userName)
    {
        List<User> users = await GetUsersFromFile();
        bool userNameExists = users.Any(u => u.UserName == userName);
        if (userNameExists)
        {
            throw new InvalidOperationException(
                $"UserName '{userName}' is already taken.");
        }
    }
}