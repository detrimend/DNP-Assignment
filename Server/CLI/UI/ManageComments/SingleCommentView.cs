using Entities;

namespace CLI.UI.ManageComments;

public class SingleCommentView
{
    private readonly Task<Comment> comment;
    private readonly ListCommentsView listCommentsView;

    public SingleCommentView(Task<Comment> comment,
        ListCommentsView listCommentsView)
    {
        this.comment = comment;
        this.listCommentsView = listCommentsView;
    }

    public async Task ShowComment()
    {
        Console.WriteLine($"Comment made by User [{comment.Result.UserId}]: " +
                          $"\n{comment.Result.Body}\n");
        Console.WriteLine("(1) Edit comment \n" +
                          "(2) Delete comment \n" +
                          "(0) Go back");
        int? selection = int.Parse(Console.ReadLine());

        switch (selection)
        {
            case 1: throw new NotImplementedException(); break;
            case 2: throw new NotImplementedException(); break;
            default: await listCommentsView.ListComments(); break;
        }
    }
}