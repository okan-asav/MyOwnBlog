
namespace MyBlog.Data;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
}
