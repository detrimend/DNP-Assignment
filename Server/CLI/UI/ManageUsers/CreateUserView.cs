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
    
    public async Task CreateUser()
    {
        throw new NotImplementedException();
    }
}