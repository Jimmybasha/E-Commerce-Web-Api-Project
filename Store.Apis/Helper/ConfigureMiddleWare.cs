using Store.Apis.MiddleWares;
using Store.Repository.Data.Contexts;
using Store.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Store.Repository.Data.Identity.Contexts;
using Store.Repository.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;

namespace Store.Apis.Helper
{
    static public class ConfigureMiddleWare
    {

        public static async Task<WebApplication> ConfigureMiddlewaresAsync(this WebApplication app )
         {

            //To Create Dependency injection bcz the storeDbContext needs an option and i can't provide it here
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<StoreDbContext>(); // Ask Clr to create object StoreDbContext
            var identityDbContext = services.GetRequiredService<StoreIdentityDbContext>(); // Ask Clr to create object StoreIdentityDbContext
            //Service to use logger
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            try
            {
                await context.Database.MigrateAsync();
                //Add After Updating Database , and take the same context 
                await StoreDbContextSeed.SeedAsync(context);
                await identityDbContext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);

            }
            catch (Exception ex)
            {
                // To Log the exceptions or error
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "There are problesm during migrations");
            }

            //Configure The User Defined [ExceptionMiddleware] Middleware i've created to use the
            //Configuration i've created
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Change Default Page Error :
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            //To Allow using the static files sent in the wwwroot
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }


    }
}
