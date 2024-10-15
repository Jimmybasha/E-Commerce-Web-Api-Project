using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Data
{
    public class StoreDbContextSeed
    {

        public async static Task SeedAsync(StoreDbContext context)
        {

            //Brands Seeding
            if (context.Brands.Count() == 0)
            {
                //Brand
                //1.Read The data from the json File
                var BrandsData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\brands.json");

                //2. Convert Json String To List<T>
               var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);

                //Add to Database
                if (Brands.Count() > 0 && Brands is not null)
                {
                    await context.Brands.AddRangeAsync(Brands);
                    await context.SaveChangesAsync();
                }


            }



            if (context.Types.Count() == 0)
            {
                //1) Get Data From json file
                var TypesData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\types.json");

                //2)Convert Json string to List 

                var types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);

                //3) Add Data to Database
                if(types.Count() > 0 && types is not null)
                {
                    await context.Types.AddRangeAsync(types);
                    await context.SaveChangesAsync();

                }
            }




            if (context.Products.Count() == 0)
            {
                //1) Get Data From json file
                var ProductData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\products.json");

                //2) Convert Json string to List 
                var products = JsonSerializer.Deserialize<List<Product>>(ProductData);

                //3) Check and add data to Database
                if (products != null && products.Count > 0)
                {
                    foreach (var product in products)
                    {
                        // Check if the product already exists by a unique identifier, such as Name
                        if (!await context.Products.AnyAsync(p => p.Name == product.Name))
                        {
                            await context.Products.AddAsync(product);
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }



            if (context.DeliveryMethods.Count() == 0)
            {
                //1) Get Data From json file
                var DeliveryData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\delivery.json");

                //2) Convert Json string to List 
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);

                //3) Check and add data to Database
            
                if(deliveryMethods is not null)
                {
                    await context.DeliveryMethods.AddRangeAsync(deliveryMethods);
                    await context.SaveChangesAsync();
                }

                }
            }

        }

    }
