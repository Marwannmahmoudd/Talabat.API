using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specification.ProductSpec;

namespace Talabat.Services
{
    public class ProductService : IProductService
    {
        private readonly IUniteOfWork _uniteOfWork;

        public ProductService(IUniteOfWork uniteOfWork)
        {
            _uniteOfWork = uniteOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParam spec)
        {
            var specParam = new ProductWithBrandAndCategorySpecification(spec);
            var products = await _uniteOfWork.Repository<Product>().GetAllWithSpecAsync(specParam);
            return products;
        }
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var spec = new ProductWithBrandAndCategorySpecification(productId);
            var product = await _uniteOfWork.Repository<Product>().GetByIdSpecAsync(spec);
            return product;
        }
        public async Task<int> GetCountAsync(ProductSpecParam spec)
        {
            var CountSpec = new ProductWithFiltrationCountAsync(spec);
            var Count = await _uniteOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Count;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
      => await _uniteOfWork.Repository<ProductBrand>().GetAllAsync();
        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
         => await _uniteOfWork.Repository<ProductCategory>().GetAllAsync();
    }
}
