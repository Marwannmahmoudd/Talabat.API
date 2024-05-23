using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specification;
using Talabat.Core.Specification.ProductSpec;

namespace Talabat.API.Controllers
{
  
    public class ProductsController : BaseApiController
    {
        //private readonly IGenericRepository<ProductBrand> _productBrandrepo;
        //private readonly IGenericRepository<ProductCategory> _productCategoryrepo;
        //private readonly IGenericRepository<Product> _Productrepo;
        private readonly IProductService _productService;

       
        public IMapper _Mapper { get; }

        public ProductsController(IProductService productService
            //IGenericRepository<Product> productrepo,
        //    IGenericRepository<ProductBrand> productBrandrepo,
        //    IGenericRepository<ProductCategory> productCategoryrepo
            , IMapper mapper)
        {
            _productService = productService;
            //_Productrepo = productrepo;
            //_productBrandrepo = productBrandrepo;
            //_productCategoryrepo = productCategoryrepo;
            _Mapper = mapper;
        }
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParam specParam)
        {
      
            var products = await _productService.GetProductsAsync(specParam);
            var productdto = _Mapper.Map<IReadOnlyList< ProductToReturnDto>>(products);
            var CountSpec = new ProductWithFiltrationCountAsync(specParam);
            var count = await _productService.GetCountAsync(specParam);
            return Ok(new Pagination<ProductToReturnDto>(specParam.PageIndex,specParam.PageSize,productdto , count));
        }
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product is null)
                return NotFound(new ApiResponse(404));
            var productdto = _Mapper.Map<Product,ProductToReturnDto>(product);
            return Ok(productdto);
        }

        [HttpGet("brands")] // api/product/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> Getbrands()
        {
        
            var brands = await _productService.GetBrandsAsync();
           
            return Ok(brands);
        }
        [HttpGet("categories")] // api/product/categories
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> Getcategories()
        {

            var categories = await _productService.GetCategoriesAsync();

            return Ok(categories);
        }
    }
}
