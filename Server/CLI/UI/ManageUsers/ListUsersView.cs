using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;
    private readonly ManageUsersView manageUsersView;
    private bool running;

    public ListUsersView(IUserRepository userRepository,
        ManageUsersView manageUsersView)
    {
        this.userRepository = userRepository;
        this.manageUsersView = manageUsersView;
    }

    public async Task ListUsers()
    {
        running = true;
        while (running)
        {
            Console.WriteLine("Listing users... ('username' [ID])");
            foreach (var user in userRepository.GetMany())
            {
                Console.WriteLine($"* '{user.UserName}' [{user.Id}]");
            }

            Console.WriteLine(
                "\n Please enter a number corresponding with your selection:");
            Console.WriteLine("(1) Select user \n" +
                              "(0) Go back");
            int? selection = int.Parse(Console.ReadLine());

            switch (selection)
            {
                case 1:
                    Console.WriteLine("Enter [ID] to select a user");
                    string userSelection = Console.ReadLine();
                    Task<User> selectedUser =
                        userRepository.GetSingleAsync(int.Parse(userSelection));
                    if (selectedUser is null)
                    {
                        Console.WriteLine("User not found");
                        break;
                    }

                    SingleUserView singleUserView =
                        new SingleUserView(selectedUser, this);
                    
                    await singleUserView.ShowUser();
                    break;

                default: running = false; break;
            }
        }

        manageUsersView.ShowOptions();
    }
}