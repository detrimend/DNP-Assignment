using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly ManagePostsView managePostsView;

    public CreatePostView(IPostRepository postRepository,
        ManagePostsView managePostsView)
    {
        this.postRepository = postRepository;
        this.managePostsView = managePostsView;
    }

    public async Task CreatePost()
    {
        throw new NotImplementedException();
    }
}