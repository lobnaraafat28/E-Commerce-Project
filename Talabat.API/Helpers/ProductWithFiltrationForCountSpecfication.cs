using Talabat.Core.Entities;
using Talabat.Core.Specefications;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Helpers
{
    public class ProductWithFiltrationForCountSpecfication : BaseSpecification<Product>
    {
        public ProductWithFiltrationForCountSpecfication(ProductSpecParams specparams)
        : base(P =>
                    (string.IsNullOrEmpty(specparams.Search) || (P.Name.ToLower().Contains(specparams.Search))) &&
                    (!specparams.BrandId.HasValue || P.ProductBrandId == specparams.BrandId.Value) &&  
                    (!specparams.TypeId.HasValue || P.ProductTypeId == specparams.TypeId.Value))

        {

        }
    }
}
