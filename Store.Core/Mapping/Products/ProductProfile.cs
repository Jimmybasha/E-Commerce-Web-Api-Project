using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Store.Core.Dtos.Products;
using Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {

        public ProductProfile(IConfiguration configuration)
        {

            CreateMap<Product, ProductDto>()
                .ForMember(P => P.BrandName, options => options.MapFrom(S => S.Brand.Name))
                .ForMember(P => P.TypeName, options => options.MapFrom(S => S.Type.Name))
                .ForMember(P => P.PictureUrl, options => options.MapFrom(S => $"{configuration["BaseUrl"]}/{S.PictureUrl}"));


            
            CreateMap<ProductBrand, TypeBrandDto>();
            
            CreateMap<ProductType, TypeBrandDto>();



        }

    }
}
