using StackExchange.Redis;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository.Repository
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDatabase database;

        //Inject the Reddis Database from the Package downloaded in the Core
        public BasketRepository(IConnectionMultiplexer reddis)
        {
            database = reddis.GetDatabase();
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
             
            var basket =  await database.StringGetAsync(basketId);

            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            //Json Serialized used here bcz the value return as jsonString
            var createdOrUpdatedBasket =  await database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(20));

            if (createdOrUpdatedBasket is false) return null;

            return await GetBasketAsync(basket.Id);

        }
    }
}
