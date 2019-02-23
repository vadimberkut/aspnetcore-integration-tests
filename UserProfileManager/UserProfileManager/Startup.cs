using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserProfileManager.Data.DbContexts;
using UserProfileManager.Data.Repositories;
using UserProfileManager.Data.Repositories.Base;
using Newtonsoft.Json;
using UserProfileManager.Filters;
using UserProfileManager.Entity.Entities;
using UserProfileManager.Entity.Enums;
using UserProfileManager.Data.Extensions;
using System.Data.SqlClient;

namespace UserProfileManager
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IHostingEnvironment env
        )
        {
            this.Configuration = configuration;
            this.Env = env;

            // This step can be skipped - injected IConfiguration already contain built configuration
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            //builder.AddEnvironmentVariables();
            //var builtConfiguration = builder.Build();
            //this.Configuration = builtConfiguration;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // DB configuration
            string connectionString = Configuration.GetConnectionString("DataAccessMicrosoftSqlProvider");
            string dbContextAssemblyName = typeof(ApplicationDbContext).Assembly.GetName().Name;
            services.AddDbContext<ApplicationDbContext>(options =>
                {
//                    options.UseSqlServer(connectionString,
//                        sqlOptions => sqlOptions.MigrationsAssembly(dbContextAssemblyName));
                    // options.UseInMemoryDatabase("dev.UserProfileManagerInMemoryDb");
                    options.UseSqlite("Filename=dev.UserProfileManager.db", dbOptions => dbOptions.MigrationsAssembly(dbContextAssemblyName));
                }
            );

            // Register servies
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserProfileRepository, UserProfileRepository>();

            // Cors
            services.AddCors();

            // Mvc
            services.AddMvc(options => {
                // Regiter custom filters
                options.Filters.Add(typeof(ValidateActionInputFilterAttribute));
                options.Filters.Add(typeof(CustomExceptionFilterAttribute));
            })
            .AddJsonOptions(options => {
                // Config serialization
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        { 
            if (env.IsDevelopment())
            {
                this.InitializeDatabase(app, env);

                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Cors
            if (env.IsDevelopment())
            {
                app.UseCors(options =>
                {
                    options.WithOrigins("http://localhost:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            }
            else
            {
                // TODO: add if needed
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeDatabase(IApplicationBuilder app, IHostingEnvironment env)
        {
            using(var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var appDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                // Ensure DB is created (do not apply migrations if exists)
                if (appDbContext.Database.IsSqlite())
                {
                    appDbContext.Database.EnsureCreated();
                }

                if (!appDbContext.IsAllMigrationsApplied())
                {
//                    try
//                    {
//                        appDbContext.Database.Migrate();
//                    }
//                    catch (SqlException ex)
//                    {
//                        Console.WriteLine(ex.Message);
//                    }
                }

                try
                {
                    // Seed predefined Roles
                    List<UserProfileRoleEntity> roles = new List<UserProfileRoleEntity>
                    {
                        new UserProfileRoleEntity()
                        {
                            Name = RoleType.User.ToString(),
                            Type = RoleType.User
                        },
                        new UserProfileRoleEntity()
                        {
                            Name = RoleType.Manager.ToString(),
                            Type = RoleType.Manager
                        },
                        new UserProfileRoleEntity()
                        {
                            Name = RoleType.Admin.ToString(),
                            Type = RoleType.Admin
                        },
                        new UserProfileRoleEntity()
                        {
                            Name = RoleType.Support.ToString(),
                            Type = RoleType.Support
                        }
                    };

                    var existing = appDbContext.UserProfileRoles.ToList();
                    var shouldBeCreated = roles.Where(x => !existing.Any(y => y.Type == x.Type));
                    appDbContext.UserProfileRoles.AddRange(shouldBeCreated);
                    appDbContext.SaveChanges();

                    // Seed Users
                    if (!appDbContext.UserProfiles.Any())
                    {
                        Random random = new Random();
                        Func<int, string> randomString = ((length) =>
                        {
                            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                            return new string(Enumerable.Repeat(chars, length)
                              .Select(s => s[random.Next(s.Length)]).ToArray());
                        });

                        var role = roles.First();
                        List<UserProfileEntity> users = new List<UserProfileEntity>();
                        for (int i = 0; i <= 1000; i++)
                        {
                            users.Add(new UserProfileEntity
                            {
                                Name = $"Test User {randomString(3)}",
                                Email = $"testuser{i+1}@test.com",
                                SkypeLogin = $"skypetestuser{0}",
                                Signature = $"signature {i+1}",
                                ImageUrl = null,
                                Role = role,
                                Settings = new UserProfileSettingsEntity
                                {
                                    Enabled = i % 2 == 0 ? true : false
                                }
                            });
                        }
                        appDbContext.UserProfiles.AddRange(users);
                        appDbContext.SaveChanges();
                    }
                   
                }
                catch(SqlException ex)
                {
                    // Ignore - maybe migrations haven't been applied yet
                    throw ex;
                }
            }
        }
    }
}
