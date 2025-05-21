
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

    namespace Bookstore
    {
        // Interface to support EntityFramework without upgrading to Core
        namespace EntityFramework
        {
            public interface IDbContextFactory
            {
                System.Data.Entity.DbContext Create();
            }
        }
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add configuration from appsettings.json and environment variables
                builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();

                // Add configuration values from Web.config's appSettings
                builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Environment", builder.Environment.EnvironmentName },
                    { "Services/Authentication", "local" },
                    { "Services/Database", "local" },
                    { "Services/FileService", "local" },
                    { "Services/ImageValidationService", "local" },
                    { "Services/LoggingService", "local" },
                    { "ClientValidationEnabled", "true" }
                });

                // Configure connection strings from Web.config
                builder.Services.AddSingleton<System.Data.Entity.DbContext>(serviceProvider =>
                {
                    // Note: Real implementation would provide actual DbContext creation
                    return null; // Replace with actual DbContext instance
                });

                // Add connection string from Web.config
                builder.Services.AddSingleton<System.Data.Entity.DbContext>(serviceProvider =>
                {
                    // Configure with the connection string from Web.config
                    var connectionString = builder.Configuration.GetConnectionString("BookstoreDatabaseConnection");
return null; // Replace with actual DbContext instance using the connection string
                });

                // Store configuration in static ConfigurationManager
                ConfigurationManager.Configuration = builder.Configuration;

                // Add services to the container (formerly ConfigureServices)
                builder.Services.AddControllersWithViews()
                    .AddViewOptions(options =>
                    {
                        options.HtmlHelperOptions.ClientValidationEnabled =
                            builder.Configuration.GetValue<bool>("ClientValidationEnabled", true);
                    });

                // Configure Entity Framework (not upgrading to EF Core per requirements)
                builder.Services.AddSingleton<Bookstore.EntityFramework.IDbContextFactory>((serviceProvider) =>
                {
                    return new DefaultDbContextFactory(builder.Configuration.GetConnectionString("BookstoreDatabaseConnection"));
                });

                // Register areas
                builder.Services.AddMvc()
                    .AddMvcOptions(options =>
                    {
                        // Register global filters here (equivalent to FilterConfig.RegisterGlobalFilters)
                    });

                // Bundle configuration would have been handled by WebOptimizer in .NET Core
                builder.Services.AddWebOptimizer(pipeline =>
                {
                    // Configure bundles here, similar to BundleConfig.RegisterBundles
                });

                //Added Services
                
                // Apply additional configuration settings from Web.config
                builder.Configuration.AddEntityFrameworkConfiguration();

                var app = builder.Build();
                
                // Configure the HTTP request pipeline (formerly Configure method)
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                
                //Added Middleware
                
                app.UseRouting();

                // Configure error handling middleware to replace Application_Error
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError(exception, "An unhandled exception occurred");

                        await context.Response.WriteAsync("An error occurred. Please try again later.");
                    });
                });

                app.UseAuthorization();
                
                // Route configuration (equivalent to RouteConfig.RegisterRoutes)
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Add area routes if needed
                app.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                
                app.Run();
            }
        }
        
        public class ConfigurationManager
        {
            public static IConfiguration Configuration { get; set; }
        }

        // Helper class for EntityFramework configuration
        public static class EntityFrameworkConfigurationExtensions
        {
            public static IConfigurationBuilder AddEntityFrameworkConfiguration(this IConfigurationBuilder builder)
            {
                // Add Entity Framework configuration settings from web.config
                return builder;
            }
        }

        // Default DbContext Factory for EntityFramework
        public class DefaultDbContextFactory : Bookstore.EntityFramework.IDbContextFactory
        {
            private readonly string _connectionString;

            public DefaultDbContextFactory(string connectionString)
            {
                _connectionString = connectionString;
            }

            public System.Data.Entity.DbContext Create()
            {
                // Create DbContext with connection string
                return null; // Replace with actual DbContext creation
            }
        }
    }