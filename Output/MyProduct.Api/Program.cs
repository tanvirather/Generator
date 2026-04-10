using System.Reflection;
using Zuhid.Base;

namespace MyCompany.MyProduct.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplicationExtension.AddServices(args);
        Assembly.GetAssembly(typeof(MyProductContext))!.GetTypes().Where(s =>
            s.Name.EndsWith("Repository")
            || s.Name.EndsWith("Mapper")
            || s.Name.EndsWith("Validator")
          )
          .ToList()
          .ForEach(item => builder.Services.AddScoped(item));
        var appSetting = new AppSetting(builder.Configuration);
        builder.Services.AddSingleton(appSetting);

        builder.AddDatabase<MyProductContext, MyProductContext>(appSetting.ConnectionStrings.MyProduct);
        var app = builder.BuildServices();
        app.Run();
    }
}
