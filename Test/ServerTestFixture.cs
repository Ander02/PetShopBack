using Core;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Nudes.SeedMaster.Interfaces;
using Api;
using Api.Data;
using Api.Infrastructure.Notifications;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace Test
{
    public class ServerTestFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly TestServer _testServer;

        public IServiceProvider Services { get; }
        public HttpClient HttpClient { get; set; }
        public IMediator Mediator { get; }
        public ApiDbContext DbContext { get; }
        public NotificationContext NotificationContext { get; }
        public IConfiguration Configuration { get; }
        public ISeeder Seeder { get; set; }
        public UserManager<User> UserManager { get; }

        public ServerTestFixture() : this("Api") { }

        protected ServerTestFixture(string relativeProjectDir)
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;

            var contentRoot = GetProjectPath(relativeProjectDir, startupAssembly);
            var hostingEnv = new HostingEnvironment()
            {
                ApplicationName = "PetShop.Test",
                EnvironmentName = "Test",
                ContentRootPath = contentRoot
            };

            var builder = new WebHostBuilder()
                            .UseStartup<TStartup>()
                            .ConfigureTestServices(this.InitializeServices)
                            .UseContentRoot(contentRoot)
                            .UseEnvironment(hostingEnv.EnvironmentName)
                            .UseSetting(WebHostDefaults.ApplicationKey, startupAssembly.FullName);

            _testServer = new TestServer(builder);

            Services = _testServer.Host.Services;
            HttpClient = _testServer.CreateClient();
            Mediator = Services.GetService<IMediator>();
            DbContext = Services.GetService<ApiDbContext>();
            NotificationContext = Services.GetService<NotificationContext>();
            Configuration = Services.GetService<IConfiguration>();
            Seeder = Services.GetService<ISeeder>();
            UserManager = Services.GetService<UserManager<User>>();
        }

        protected static string GetProjectPath(string projectRelativePath, Assembly assembly)
        {
            var projectName = assembly.GetName().Name;
            var applicationBasePath = AppContext.BaseDirectory;
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));

                if (projectDirectoryInfo.Exists)
                {
                    var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, $"{projectName}.csproj"));

                    if (projectFileInfo.Exists) return Path.Combine(projectDirectoryInfo.FullName);
                }
            } while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }

        protected virtual void InitializeServices(IServiceCollection services)
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;

            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(startupAssembly));
            manager.FeatureProviders.Add(new ControllerFeatureProvider());

            services.AddSingleton(manager);

            string inMemoryName = $"TestDB_{Guid.NewGuid()}";

            services.AddScoped<DbContextOptions>(sp => new DbContextOptionsBuilder().UseInMemoryDatabase(inMemoryName).EnableSensitiveDataLogging().Options);
            services.AddScoped<DbContextOptions<ApiDbContext>>(sp => new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase(inMemoryName).EnableSensitiveDataLogging().Options);
        }

        public void Dispose()
        {
            _testServer?.Dispose();
            DbContext?.Dispose();
            HttpClient?.Dispose();
            Seeder?.Dispose();
            NotificationContext?.Reset();
        }
    }
}
