using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nudes.SeedMaster.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PetShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            //Is Seed
            if (args.Any(d => d.Equals("seed", StringComparison.InvariantCultureIgnoreCase)))
            {
                using var scope = webHost.Services.CreateScope();
                var seeder = scope.ServiceProvider.GetService<ISeeder>();
                await seeder.Run();
            }

            await webHost.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
