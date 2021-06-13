using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RetroPieRomUploader.Data;
using RetroPieRomUploader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RetroPieRomUploader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    if (args.Contains("--dbmigrate"))
                        DoDBMigrations(services);

                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        webBuilder.ConfigureAppConfiguration((configBuilder) =>
                            configBuilder.AddJsonFile($"appsettings.linux.json", true));
                    }
                    webBuilder.UseStartup<Startup>();
                });

        private static void DoDBMigrations(IServiceProvider services)
        {
            var service = services.GetRequiredService<DbContextOptions<RetroPieRomUploaderContext>>();
            using (var context = new RetroPieRomUploaderContext(service))
            {
                context.Database.Migrate();
                Console.WriteLine("DB Migration completed");
                Environment.Exit(0);
            }
        }
    }
}
