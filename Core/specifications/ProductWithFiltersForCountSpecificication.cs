using Core.Entities;
using Core.Params;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecificication : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecificication(ProductParams productParams) : base(
            x => (!productParams.TypeId.HasValue || x.ProductTypeId.Equals(productParams.TypeId)) &&
            (!productParams.BrandId.HasValue || x.ProductBrandId.Equals(productParams.BrandId)) &&
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Trim().Contains(productParams.Search.Trim().ToLower()))
        )
        {
        }
    }
}