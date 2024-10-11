using Store.Core.Dtos.Products;
using Store.Core.Entities;
using Store.Core.Helper;
using Store.Core.Specifications.ProductSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface IProductService
    {
       Task<PaginationResponse<ProductDto>> getAllProductsAsync(ProductSpecParams productSpec);
       Task<ProductDto>getProductByIdAsync(int id);
       Task<IEnumerable<TypeBrandDto>>getAllBrandsAsync();
       Task<IEnumerable<TypeBrandDto>>getAllTypesAsync();





    }
}
