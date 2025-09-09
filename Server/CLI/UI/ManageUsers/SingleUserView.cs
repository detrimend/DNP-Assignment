using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class SingleUserView
{
    private readonly Task<User> user;
    private readonly ListUsersView listUsersView;

    public SingleUserView(Task<User> user,
        ListUsersView listUsersView)
    {
        this.user = user;
        this.listUsersView = listUsersView;
    }

    public async Task ShowUser()
    {
        throw new NotImplementedException();
    }
}