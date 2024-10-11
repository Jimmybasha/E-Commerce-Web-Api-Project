using AutoMapper;
using Store.Core;
using Store.Core.Dtos.Products;
using Store.Core.Entities;
using Store.Core.Helper;
using Store.Core.Services.Contract;
using Store.Core.Specifications.ProductSpecs;
using Store.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
         
        //Injection
        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }



        public async Task<PaginationResponse<ProductDto>> getAllProductsAsync(ProductSpecParams productSpec) 
        {
          var productspec = new ProductSpecification(productSpec);
          var products = await unitOfWork.Repository<Product,int>().GetAllWithSpecAsync(productspec);
          var productCount = new ProductWithCountSpecification(productSpec);

          var mappedProduct = mapper.Map<IEnumerable<ProductDto>>(products);

            var count = await unitOfWork.Repository<Product, int>().getCountAsync(productCount);


          return new PaginationResponse<ProductDto>(productSpec.PageIndex,productSpec.PageSize, count, mappedProduct);
        }


        public async Task<ProductDto> getProductByIdAsync(int id)
        {
         var productspec = new ProductSpecification(id);
         return   mapper.Map<ProductDto>(await unitOfWork.Repository<Product, int>().GetWithSpecAsync(productspec));
        }

        public async Task<IEnumerable<TypeBrandDto>> getAllTypesAsync()
            =>
             mapper.Map<IEnumerable<TypeBrandDto>>(await unitOfWork.Repository<ProductBrand, int>().GetAllAsync());
        


        public async Task<IEnumerable<TypeBrandDto>> getAllBrandsAsync() 
            =>
            mapper.Map<IEnumerable<TypeBrandDto>>(await unitOfWork.Repository<ProductBrand,int>().GetAllAsync());

     

    }
}
