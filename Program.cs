using BookStore.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            //RunMigration(webHost);

            webHost.Run();
        }

        /// <summary>
        /// Run Migration when app start and create database if it's not exist
        /// </summary>
        /// <param name="webHost"></param>
        private static void RunMigration(IHost webHost)
        {
            using var scope = webHost.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<StoreContext>();
            db.Database.Migrate();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
