using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Controllers;
using Talabat.API.DTO;
using Talabat.API.Helpers;
using Talabat.Core.Entities;
using Talabat.Repository;

namespace Talabat.APIs.Controllers
{
    
    public class BasketController : BaseAPIController
	{
        private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository,IMapper mapper) {
            _basketRepository = basketRepository;
			_mapper = mapper;
		}
		[CashedAttribute(600)]
		[HttpGet("{id}")]

        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id) { 
        
            var basket = await _basketRepository.GetBasketAsync(id);
            return basket == null ? new CustomerBasket(id):basket;
        }
        [HttpPost]

        public async Task<IActionResult> UpdateBasketAsyns(CustomerBasketDTO basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDTO, CustomerBasket>(basket);
          var updatedOrCreatedBasket=  await _basketRepository.UpdateBasketAsync(mappedBasket);
            if (updatedOrCreatedBasket == null) return BadRequest();
            return Ok(updatedOrCreatedBasket);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await _basketRepository.DeleteBasket(id); 
        }

    }
}
