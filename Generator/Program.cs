using Zuhid.Generator.Generate;
using Zuhid.Generator.Models;
using Zuhid.Generator.Tools;

class Program
{
    static void Main(string[] args)
    {
        var setting = new SettingModel(args[0], args[1], args[2], args[3]);
        var fileService = new FileService();
        new TemplateGenerate(setting, fileService).Generate();
        new PostgresGenerate(setting, fileService).Generate();
        new CsharpGenerate(setting, fileService).Generate();
        new CsharpListGenerate(setting, fileService).Generate();
        new CsharpConfigGenerate(setting, fileService).Generate();
    }
}
