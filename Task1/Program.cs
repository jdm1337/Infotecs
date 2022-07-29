using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Task1;

public class Program
{
    public static void Main(string [] args)
    {
        try 
        {
            var logfileName = $"log{DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd_HH_mm")}.log";
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var logLevelParam = config["LogLevel"];
            var logLevelValue = DefineLogLevel(logLevelParam);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(logLevelValue)
                .WriteTo.Console()
                .WriteTo.File(logfileName,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Application started");

            var host = CreateDefaultBuilder().Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var copierInstance = provider.GetRequiredService<CopyMaster>();
            copierInstance.Execute();

            host.Run();
        }
        catch(Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    static IHostBuilder CreateDefaultBuilder()
    {
        return Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json");
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<CopyMaster>();
            });
    }
    public static Serilog.Events.LogEventLevel DefineLogLevel(string logLevelParam)
    {
        var logLevelValue = Serilog.Events.LogEventLevel.Information;

        if (logLevelParam.Equals("Error"))
        {
            logLevelValue = Serilog.Events.LogEventLevel.Error;
        }
        else if (logLevelParam.Equals("Debug"))
        {
            logLevelValue = Serilog.Events.LogEventLevel.Debug;
        }

        return logLevelValue;
    }
}