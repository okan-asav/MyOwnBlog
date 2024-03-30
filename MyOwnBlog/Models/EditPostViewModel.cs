using MyBlog.Data;

namespace MyBlog.Models;

public class EditPostViewModel : PostViewModel
{
    public Guid Id { get; set; }
    public List<Tag> Tags = new List<Tag>();
}
