using Entities;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly Task<Post> post;
    private readonly ListPostsView listPostsView;

    public SinglePostView(Task<Post> post, ListPostsView listPostsView)
    {
        this.post = post;
        this.listPostsView = listPostsView;
    }

    public async Task ShowPost()
    {
        throw new NotImplementedException();
    }
}