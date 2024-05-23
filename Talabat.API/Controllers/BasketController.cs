using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository 
            ,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet] // Get : /api/basket?id=
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id) 
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket?? new CustomerBasket(id));
        }

        [HttpPost]  //Post : /api/basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto customerBasket)
        {
            var mappedBasket = _mapper.Map<CustomerBasket>(customerBasket);
            
            var createdorUpdated = await _basketRepository.UdpateBasketAsync(mappedBasket);
            if (createdorUpdated is null)
                return BadRequest(new ApiResponse(400));
            return Ok(createdorUpdated);
        }

        [HttpDelete]  // delete : /api/basket?id=
        public async Task DeleteBasket(string id)
        {
           await _basketRepository.DeleteBasketAsync(id);
          
        }
    }
}
