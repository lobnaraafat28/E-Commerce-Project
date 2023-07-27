using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTO;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Specefications;
using Talabat.Core.Specifications;

namespace Talabat.API.Controllers
{
	
	public class ProductsController : BaseAPIController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		

		public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		[CashedAttribute(600)]
		[HttpGet]
		public async Task<ActionResult<Pagenations<ProductToReturnDTO>>> GetProducts([FromQuery]ProductSpecParams productSpec)
		{
			//1. Build the object of the specification of the query (to get products)
			var spec = new ProductBrandAndTypeSpectification(productSpec);
			
			var productRepo = _unitOfWork.Repository<Product>();
			if(productRepo != null) {
				//create the linq query with spec and send it to EF and EF convert it to sql query to get the data from database
				var products = await productRepo.GetAllSpecAsync(spec);
				//mapping the data (getting from DB) into DTO data to display it to frontend user
				var mappedData = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);
				//build the object of the specification of the query (to get the count of products)
				var countSpec = new ProductWithFiltrationForCountSpecfication(productSpec);
				var count = await _unitOfWork.Repository<Product>().GetCountWithSpec(countSpec);
				return Ok(new Pagenations<ProductToReturnDTO>(productSpec.PageIndex, productSpec.PageSize, count, mappedData));
			}
			return null;
		}
		[ProducesResponseType(typeof(ProductToReturnDTO),200)]
		[ProducesResponseType(typeof(ApiResponse),404)]
		[CashedAttribute(600)]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetProduct(int id)
		{
			var spec = new ProductBrandAndTypeSpectification(id);
			var productRepo = _unitOfWork.Repository<Product>();
			if (productRepo != null)
			{
				var product = await productRepo.GetByIdSpecAsync(spec);
				if (product == null)
				{
					return NotFound(new ApiResponse(404));
				}
				return Ok(_mapper.Map<Product, ProductToReturnDTO>(product));

			}
			return null;


		}
		[CashedAttribute(600)]
		[HttpGet("brands")]

		public async Task<IActionResult> GetBrands()
		{
			var brandRepo = _unitOfWork.Repository<ProductBrand>();
			if (brandRepo != null)
			{
				var brands = await brandRepo.GetAllAsync();
				return Ok(brands);
			}
			return null;
		}
		[CashedAttribute(600)]
		[HttpGet("types")]

		public async Task<IActionResult> GetTypes()
		{
			var typeRepo = _unitOfWork.Repository<ProductType>();
			if(typeRepo != null)
			{
				var types = await typeRepo.GetAllAsync();
				return Ok(types);

			}
			return null;
		}

	}
}
