
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Store.Apis.Errors;
using Store.Apis.MiddleWares;
using Store.Core;
using Store.Core.Mapping.Products;
using Store.Core.Services.Contract;
using Store.Repository;
using Store.Repository.Data;
using Store.Repository.Data.Contexts;
using Store.Service.Services.Products;

namespace Store.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
          

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddDbContext<StoreDbContext>(option =>
            {
                //Get the Connection String from the AppSettings in => DefaultConnection Key
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        

            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(M => M.AddProfile(new ProductProfile(builder.Configuration)));
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.Configure<ApiBehaviorOptions>((options) =>
            {




                options.InvalidModelStateResponseFactory = (actionContext) =>
                {

                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count > 0)
                                                        .SelectMany(p => p.Value.Errors)
                                                        .Select(E => E.ErrorMessage)
                                                        .ToArray();


                    var res = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(res);

                };


            });


            var app = builder.Build();

            //To Create Dependency injection bcz the storeDbContext needs an option and i can't provide it here
            using var scope = app.Services.CreateScope();
            
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<StoreDbContext>();
            //Service to use logger
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
            await context.Database.MigrateAsync();
                //Add After Updating Database , and take the same context 
             await StoreDbContextSeed.SeedAsync(context);
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

            //To Allow using the static files sent in the wwwroot
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
