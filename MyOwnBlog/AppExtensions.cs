using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using System.Security.Claims;

namespace MyBlog;

public static class AppExtensions
{

    public static IServiceCollection AddQuasarShop(this IServiceCollection services)
    {

        //services
        //    .AddScoped<ICatalogsService, CatalogsService>();
        return services;
    }
    public static IApplicationBuilder UseQuasarShop(this IApplicationBuilder builder)
    {
        //CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol = "€";
        //CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("en_GB");
        //CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture("en_GB");

        using var scope = builder.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        context.Database.Migrate();

        roleManager.CreateAsync(new Role { Name = "Administrators" }).Wait();
        roleManager.CreateAsync(new Role { Name = "Members" }).Wait();

        var user = new User
        {
            UserName = configuration.GetValue<string>("Security:DefaultUser:UserName"),
            Email = configuration.GetValue<string>("Security:DefaultUser:UserName"),
            Name = configuration.GetValue<string>("Security:DefaultUser:Name"),
            EmailConfirmed = true
        };

        userManager.CreateAsync(user, configuration.GetValue<string>("Security:DefaultUser:Password")).Wait(); ;
        userManager.AddToRoleAsync(user, "Administrators").Wait();
        var claimResult =  userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, configuration.GetValue<string>("Security:DefaultUser:Name"))).Result;


        return builder;
    }

    //public static PagedList<T> ToPagedList<T>(this IQueryable<T> list, int page, int pageSize = 10)
    //{
    //    return new PagedList<T> { AbsolutePage = page, PageSize = pageSize, Items = list.Skip((page - 1) * pageSize).Take(pageSize) };
    //}
}

//public class PagedList<T>
//{
//    public int PageSize { get; set; }
//    public int AbsolutePage { get; set; }
//    public int PageCount => (int)Math.Ceiling(Items.Count() / (double)PageSize);
//    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

//}
