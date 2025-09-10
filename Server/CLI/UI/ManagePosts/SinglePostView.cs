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
        Console.WriteLine($"'{post.Result.Title}'[{post.Result.Id}] by User [{post.Result.UserId}]" +
                          $"\n{post.Result.Body}\n");
        Console.WriteLine("(1) Show comments \n" + // not implemented
                          "(2) Edit post \n" +
                          "(3) Delete post \n" +
                          "(0) Go back");
        int? selection = int.Parse(Console.ReadLine());
        switch (selection)
        {
            case 1: throw new NotImplementedException();
            case 2: throw new NotImplementedException();
            case 3: throw new NotImplementedException();
            default: await listPostsView.ListPosts(); break;
        }
    }
}