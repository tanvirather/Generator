using Zuhid.Generator.Generate;
using Zuhid.Generator.Models;

class Program
{
    static void Main(string[] args)
    {
        var setting = new SettingModel("OneFlight", "FlightOps", "../OneFlight/GeneratorInput", "../OneFlight");
        new TemplateGenerate(setting).Generate();
        // new PostgresGenerate(setting).Generate();
        new CsharpGenerate(setting).Generate();
    }
}
