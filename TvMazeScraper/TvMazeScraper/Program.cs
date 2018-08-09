using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace TvMazeScraper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog(ConfigureSerilog)
                .Build();
        
        private static void ConfigureSerilog(WebHostBuilderContext ctx, LoggerConfiguration loggerConfiguration)
        {
            loggerConfiguration
                .WriteTo.Console();
        }
    }
}