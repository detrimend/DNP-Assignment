using CLI.UI.ManageComments;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly ICommentRepository commentRepository;
    private readonly Task<Post> post;
    private readonly ListPostsView listPostsView;

    private ListCommentsView listCommentsView;

    public SinglePostView(ICommentRepository commentRepository, Task<Post> post,
        ListPostsView listPostsView)
    {
        this.commentRepository = commentRepository;
        this.post = post;
        this.listPostsView = listPostsView;
    }

    public async Task ShowPost()
    {
        Console.WriteLine(
            $"'{post.Result.Title}'[{post.Result.Id}] by User [{post.Result.UserId}]" +
            $"\n{post.Result.Body}\n");
        Console.WriteLine("(1) Show comments \n" + // not implemented
                          "(2) Edit post \n" +
                          "(3) Delete post \n" +
                          "(0) Go back");
        int? selection = int.Parse(Console.ReadLine());
        switch (selection)
        {
            case 1:
                if (listCommentsView is null)
                {
                    listCommentsView =
                        new ListCommentsView(commentRepository, post, this);
                }

                await listCommentsView.ListComments();
                break;
            case 2: throw new NotImplementedException(); break;
            case 3: throw new NotImplementedException(); break;
            default: await listPostsView.ListPosts(); break;
        }
    }
}