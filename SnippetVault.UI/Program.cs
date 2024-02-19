using Microsoft.Data.SqlClient;
using SnippetVault.UI.Middlewares;

namespace SnippetVault.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Allow access in LAN
            //builder.WebHost.UseUrls("http://IP:PORT");

            builder.Services.ConfigureServices(builder.Configuration, builder.Environment);

            var app = builder.Build();

            // Enables Https
            app.UseHsts();
            app.UseHttpsRedirection();

            // Allows clients to reach static files
            app.UseStaticFiles();

            // Identifies action method based on route
            app.UseRouting();

            // Reading Identity cookie
            app.UseAuthentication();

            // Validates access permissions of the user
            app.UseAuthorization();

            // Session for managing user state
            app.UseSession();

            // Executes the filter pipeline
            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                // If exception occurs, returns a exception 'View'
                //app.UseExceptionHandlingMiddleware();
            }

            app.Run();
        }
    }
}