using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private bool running;
    
    private ManagePostsView managePostsView;
    private ManageUsersView manageUsersView;

    public CliApp(IUserRepository userRepository,
        ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        running = true;
        while (running)
        {
            Console.WriteLine(
                "Please enter a number corresponding with your selection:");
            Console.WriteLine("(1) Manage posts \n" +
                              "(2) Manage users \n" +
                              "(0) Exit");
            int? selection = int.Parse(Console.ReadLine());
            switch (selection)
            {
                case 1:
                    if(managePostsView is null)
                    {
                        managePostsView = new ManagePostsView(postRepository, this);
                    }
                    await managePostsView.ShowOptions();
                    break;
                case 2:
                    if (manageUsersView is null)
                    {
                        manageUsersView = new ManageUsersView(userRepository, this);
                    }
                    await manageUsersView.ShowOptions();
                    break;
                default:
                    running = false;
                    break;
            }
        }
    }
}