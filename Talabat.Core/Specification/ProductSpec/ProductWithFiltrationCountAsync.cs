using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification.ProductSpec
{
    public class ProductWithFiltrationCountAsync : BaseSpecifications<Product>
    {
        public ProductWithFiltrationCountAsync(ProductSpecParam specParam) : base(P =>
         (string.IsNullOrEmpty(specParam.Search) || P.Name.ToLower().Contains(specParam.Search)) &&
            (!specParam.BrandId.HasValue || P.BrandId == specParam.BrandId.Value) &&
            (!specParam.CategoryId.HasValue || P.CategoryId == specParam.CategoryId.Value)
            )
        {
            
        }
    }
}
