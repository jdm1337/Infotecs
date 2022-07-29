using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Task1;

public class Program
{
    public static void Main(string [] args)
    {
        try 
        {
            var host = CreateDefaultBuilder().Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var copierInstance = provider.GetRequiredService<CopyMaster>();
            copierInstance.Execute();

            host.Run();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static IHostBuilder CreateDefaultBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json");
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<CopyMaster>();
            });
    }
}