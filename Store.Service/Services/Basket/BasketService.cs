using AutoMapper;
using Store.Core.Dtos.Basket;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Basket
{
    public class BasketService : IBasketServices
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }


        public async Task<CustomerBasketDto?> GetBasketAsync(string basketId) 
        {
            var basket = await basketRepository.GetBasketAsync(basketId);

            if (basket == null)
            {
                return mapper.Map<CustomerBasketDto>(new CustomerBasket() { Id = basketId });
            }
            return mapper.Map<CustomerBasketDto>(basket);

        }

        public async Task<CustomerBasketDto?> UpdateBasketAsync(CustomerBasketDto model)
        {
            var basket = await basketRepository.UpdateBasketAsync(mapper.Map<CustomerBasket>(model));

            if (basket == null) return null;

            return mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId) 
        {
                return await basketRepository.DeleteBasketAsync(basketId);
        }
         
            


       

        
    }
}
