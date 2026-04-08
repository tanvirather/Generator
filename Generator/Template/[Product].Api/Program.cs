using System.Reflection;
using Zuhid.Base;

namespace [Company].[Product].Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplicationExtension.AddServices(args);
        Assembly.GetAssembly(typeof([Product]Context))!.GetTypes().Where(s =>
            s.Name.EndsWith("Repository")
            || s.Name.EndsWith("Mapper")
            || s.Name.EndsWith("Validator")
          )
          .ToList()
          .ForEach(item => builder.Services.AddScoped(item));
        var appSetting = new AppSetting(builder.Configuration);
        builder.Services.AddSingleton(appSetting);

        builder.AddDatabase<[Product]Context, [Product]Context>(appSetting.ConnectionStrings.[Product]);
        var app = builder.BuildServices();
        app.Run();
    }
}
