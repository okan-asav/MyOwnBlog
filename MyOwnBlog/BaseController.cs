using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace MyBlog;

public abstract class BaseController : Controller
{
    public Guid? UserId => User.Identity?.IsAuthenticated == true ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value) : default;
}
