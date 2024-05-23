using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification.ProductSpec
{
    public class ProductWithBrandAndCategorySpecification : BaseSpecifications<Product>
    {
        public ProductWithBrandAndCategorySpecification(ProductSpecParam specParam)
            :
            
            base( P =>
            (string.IsNullOrEmpty(specParam.Search) || P.Name.ToLower().Contains(specParam.Search)) &&
            (!specParam.BrandId.HasValue ||  P.BrandId == specParam.BrandId.Value) &&
            (!specParam.CategoryId.HasValue || P.CategoryId == specParam.CategoryId.Value)
            )
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);

            if (!string.IsNullOrEmpty(specParam.Sort))
            {
                switch(specParam.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else 
                AddOrderBy(p => p.Name);

            ApplyPagination((specParam.PageIndex - 1) * specParam.PageSize, specParam.PageSize);
        }
        public ProductWithBrandAndCategorySpecification(int id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Category);
        }
    }
}
