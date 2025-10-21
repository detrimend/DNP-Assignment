using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;
    private readonly ManageUsersView manageUsersView;

    public CreateUserView(IUserRepository userRepository,
        ManageUsersView manageUsersView)
    {
        this.userRepository = userRepository;
        this.manageUsersView = manageUsersView;
    }

    // SingleUserView needs ListUsersView to go back to list after viewing a single user
    // For that reason, after creating a user, we go back to the manage menu
    // Rather than to a single view of the newly created user
    // to be continued
    public async Task CreateUser()
    {
        Console.WriteLine("Creating a new user...");
        Console.WriteLine("Enter a username:");
        string username = Console.ReadLine();
        Console.WriteLine("Enter a password:");
        string password = Console.ReadLine();
        await userRepository.AddAsync(new User
        {
            UserName = username,
            Password = password
        });
        Console.WriteLine(
            $"User created successfully with ID: {userRepository.GetMany().Last().Id}");
        await manageUsersView.ShowOptions();
    }
}