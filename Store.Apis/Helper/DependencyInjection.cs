using Microsoft.EntityFrameworkCore;
using Store.Core.Services.Contract;
using Store.Core;
using Store.Repository;
using Store.Repository.Data.Contexts;
using Store.Service.Services.Products;
using Store.Core.Mapping.Products;
using Microsoft.AspNetCore.Mvc;
using Store.Apis.Errors;
using Store.Core.Repositories.Contract;
using Store.Repository.Repository;
using StackExchange.Redis;
using Store.Core.Mapping.Basket;

namespace Store.Apis.Helper
{
    static public class DependencyInjection
    {

        public static IServiceCollection AddDependency(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddRedisService(configuration);
            services.AddBuiltInServices();
            services.AddSwaggerService();
            services.AddDbContextService(configuration);
            services.AddUserDefinedServices();
            services.AddAutoMapperServices(configuration);
            services.ConfigureInvalidModelStateResponseService();


            return services;

        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {

            services.AddControllers();

            return services;
        }
        private static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        private static IServiceCollection AddDbContextService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(option =>
            {
                //Get the Connection String from the AppSettings in => DefaultConnection Key
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        } 
        private static IServiceCollection AddUserDefinedServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        private static IServiceCollection AddAutoMapperServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            return services;
        }
        
        private static IServiceCollection ConfigureInvalidModelStateResponseService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>((options) =>
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
            return services;
        }


        private static IServiceCollection AddRedisService(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {

                //In Appsettings.Json define new ConnectionString for Redis db
                var connection = configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(connection);

            });


            return services;
        }





    }
}
