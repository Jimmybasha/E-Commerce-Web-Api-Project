
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Store.Apis.Errors;
using Store.Apis.Helper;
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

            //Add Services To The Container

            builder.Services.AddDependency(builder.Configuration);


            var app = builder.Build();

            await app.ConfigureMiddlewaresAsync();
            
            app.Run();
        }
    }
}
