using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SnippetVault.Core.Domain.IdentityEntities;
using SnippetVault.Core.Domain.RepositoryContracts;
using SnippetVault.Core.ServiceContracts;
using SnippetVault.Core.Services;
using SnippetVault.Infrastructure.DbContext;
using SnippetVault.Infrastructure.Repositories;


namespace SnippetVault.UI
{
    public static class ConfigureServicesExtension
    {
        // Extension method for configuring service with intent of reducing complexity of Program.cs
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfigurationManager configuration, IWebHostEnvironment env)
        {
            // Adding global filter
            services.AddControllersWithViews(options =>
            {
                // Add global antiforgery token validation for post requests
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            // Add services related to identity
            services.AddScoped<IApplicationUserStore, ApplicationUserStore>();
            services.AddScoped<IApplicationUserManager, ApplicationUserManager>();
            services.AddScoped<ApplicationUserManager>();

            // Add repository services
            services.AddScoped<ISnippetRepository, SnippetRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IStarRepository, StarRepository>();
            services.AddScoped<ICommentLikeRepository, CommentLikeRepository>();

            // Add our services
            services.AddScoped<ISnippetService, SnippetService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IStarService, StarService>();
            services.AddScoped<ICommentLikeService, CommentLikeService>();

            string sqlServerConnectionString;
            var envRemoteDb = Environment.GetEnvironmentVariable("REMOTE_DB");
            if (envRemoteDb != null && envRemoteDb == "TRUE")
            {
                sqlServerConnectionString = configuration["SV_AZURE_DB"];
            }
            else
            {
                sqlServerConnectionString = configuration.GetConnectionString("Default");
            }

            // Add Database to application
            services.AddDbContext<ApplicationDbContext>(
                   options => options.UseSqlServer(sqlServerConnectionString
                   ));

            // Enable Identity in this project
            // This configration for application layer
            services.AddIdentityCore<ApplicationUser>((options) =>
            {
                // Configure password complexity
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 3;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";

                if (configuration.GetValue<bool>("EmailConfirmRequired"))
                {
                    options.SignIn.RequireConfirmedEmail = true;
                }
            })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                // These configurations for repository layer
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
                .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddIdentityCore<ModeratorUser>(options =>
            {
                // Configure password complexity
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 3;
            })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ModeratorUser, ApplicationRole, ApplicationDbContext, Guid>>();



            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddIdentityCookies();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/";
                options.LogoutPath = "/account/logout";
            });

            services.AddAuthorization(options =>
            {
                // Fallback policy if not specified another authorization policy all actions required an authenticated user
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            // Configure generated URLs for aesthetics
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseQueryStrings = true;
                options.LowercaseUrls = true;
            });

            // Add Session related services
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true; // Client side can't access this
                options.Cookie.IsEssential = true;
            });

            // Add email services
            services.AddTransient<IEmailService>((provider) =>
            {
                //provider.GetRequiredService<IExampleService>();
                var azureCommConnectionString = configuration["COMMUNICATION_SERVICES_CONNECTION_STRING"];
                var sender = configuration["COMMUNICATION_SERVICES_SENDER"];
                return new AzureCommEmail(azureCommConnectionString, sender);
            });

            services.Configure<DataProtectionTokenProviderOptions>(o =>
               o.TokenLifespan = TimeSpan.FromHours(1));

            return services;
        }
    }
}