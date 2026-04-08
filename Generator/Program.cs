using Zuhid.Generator.Generate;
using Zuhid.Generator.Models;

class Program
{
    static void Main(string[] args)
    {
        var setting = new SettingModel(args[0], args[1], args[2], args[3]);
        new TemplateGenerate(setting).Generate();
        new PostgresGenerate(setting).Generate();
        new CsharpListGenerate(setting).Generate();
        new CsharpConfigGenerate(setting).Generate();
    }
}
