using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;
    private readonly ManageUsersView manageUsersView;
    private bool running;

    private SingleUserView singleUserView;

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
                Console.WriteLine($"* '{user.username}' [{user.Id}]");
            }

            Console.WriteLine("Enter [ID] to select a user \n" +
                              "Enter 'exit' to return");
            string selection = Console.ReadLine();

            if (selection == "exit")
            {
                running = false;
                break;
            }

            Task<User> selectedUser =
                userRepository.GetSingleAsync(int.Parse(selection));
            if (singleUserView is null)
            {
               singleUserView = new SingleUserView(selectedUser, this); 
            }

            await singleUserView.ShowUser();
        }

        manageUsersView.ShowOptions();
    }
}