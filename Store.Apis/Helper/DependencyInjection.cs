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
using Store.Service.Services.Basket;
using Store.Service.Services.Caches;
using Store.Repository.Data.Identity.Contexts;
using Store.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Store.Service.Services.User;
using Store.Service.Services.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.Core.Mapping.Auth;
using Store.Core.Mapping.Orders;
using Store.Service.Services.Orders;
using Store.Service.Services.Payments;

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
            services.AddIdentityServices();
            services.AddAuthenticationService(configuration);
            services.ConfigureCors();


            return services;

        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {

            services.AddControllers();

            return services;
        }

        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            //Inject All Identity Built in Services
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdentityDbContext>();

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

            services.AddDbContext<StoreIdentityDbContext>(option =>
            {
                //Get the Connection String from the AppSettings in => DefaultConnection Key
                option.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            return services;

            return services;
        } 
        private static IServiceCollection AddUserDefinedServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheServices, CacheService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBasketServices, BasketService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        private static IServiceCollection AddAutoMapperServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            services.AddAutoMapper(M => M.AddProfile(new AuthenticationProfile()));
            services.AddAutoMapper(M => M.AddProfile(new OrderProfile(configuration)));
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


        private static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            //Auth for the JWT I Created .
            services.AddAuthentication(options =>
            {

                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }
            ).AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ValidateSignatureLast = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            });


            return services;

        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


        }



    }
}
