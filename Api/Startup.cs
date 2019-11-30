using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nudes.SeedMaster;
using Nudes.SeedMaster.Interfaces;
using Nudes.SeedMaster.Seeder;
using PetShop.Data;
using PetShop.Infrastructure.Behaviors;
using PetShop.Infrastructure.Notifications;
using PetShop.Infrastructure.Swagger;
using System.Reflection;

namespace PetShop
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(environment.ContentRootPath)
                                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                                    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                                    .AddEnvironmentVariables();

            Configuration = builder.Build();
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var apiAssembly = Assembly.GetExecutingAssembly();

            #region Database

            //DbContext
            services.AddDbContext<ApiDbContext>(options => options
                .EnableDetailedErrors(Environment.IsDevelopment())
                .EnableSensitiveDataLogging(Environment.IsDevelopment())
                .UseSqlServer(Configuration.GetConnectionString("Default")));

            //Seeder
            services.AddScoped<DbContext>(provider => provider.GetService<ApiDbContext>());

            SeedScanner.FindSeedersInAssembly(apiAssembly)
                            .ForEach(d => services.AddScoped(d.InterfaceType, d.ImplementationType));

            services.AddScoped<ISeeder, EfCoreSeeder>();

            #endregion

            #region MVC

            services.AddControllers(config =>
            {
                
                config.Filters.Add(typeof(NotificationFilter));

            }).AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.IgnoreNullValues = true;
                config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            #endregion

            #region Infrastructure

            services.AddScoped<NotificationContext>();

            services.AddMediatR(apiAssembly);

            AssemblyScanner.FindValidatorsInAssembly(apiAssembly)
                .ForEach(validator => services.AddScoped(validator.InterfaceType, validator.ValidatorType));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(NotificationValidationBehavior<,>));


            #endregion

            #region Swagger

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Franca API",
                    Version = "v1",
                });

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                });

                config.DocumentFilter<LowercaseDocumentFilter>();
            });
            services.ConfigureSwaggerGen(c => c.CustomSchemaIds(x => x.FullName));

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseCors((option) =>
            {
                option.AllowAnyMethod();
                option.AllowAnyOrigin();
                option.AllowAnyHeader();
            });

            app.UseSwagger();
            app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "Franca API"));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(options => options.MapControllers());
        }
    }
}
