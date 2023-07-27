using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specefications;

namespace Talabat.Core.Specifications
{
	public class ProductBrandAndTypeSpectification : BaseSpecification<Product>
	{
		
		public ProductBrandAndTypeSpectification(ProductSpecParams specParams) :base(P=> (string.IsNullOrEmpty(specParams.Search) || (P.Name.ToLower().Contains(specParams.Search))) &&
		(!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId.Value)&&
		(!specParams.TypeId.HasValue || P.ProductTypeId == specParams.TypeId.Value))
		{
			Includes.Add(P => P.ProductBrand);
			Includes.Add(P => P.ProductType);
			if (!string.IsNullOrEmpty(specParams.Sort))
			{
				switch (specParams.Sort)
				{
					case "PriceAsc":
						AddOrderBy(P => P.Price);
						break;
					case "PriceDesc":
						AddOrderByDesc(P => P.Price);
						break;
					default:
						AddOrderBy(P => P.Name);
						break;
				}
			}
			Applypagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);   

		}
		public ProductBrandAndTypeSpectification(int id): base(P => P.Id == id)
		{
			Includes.Add(P => P.ProductBrand);
			Includes.Add(P => P.ProductType);
		}
	}
}
