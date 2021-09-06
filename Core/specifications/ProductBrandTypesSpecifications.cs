using Core.Entities;
using Core.Params;

namespace Core.Specifications
{
    public class ProductBrandTypesSpecifications : BaseSpecification<Product>
    {
        public ProductBrandTypesSpecifications(ProductParams productParams) : base(
            x => (!productParams.TypeId.HasValue || x.ProductTypeId.Equals(productParams.TypeId)) &&
            (!productParams.BrandId.HasValue || x.ProductBrandId.Equals(productParams.BrandId)) &&
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Trim().Contains(productParams.Search.Trim().ToLower()))
        )
        {
            base.AddInclude(x => x.ProductType);
            base.AddInclude(x => x.ProductBrand);
            base.ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1),
                    productParams.PageSize);

            // Default ordering
            switch (productParams.Sort)
            {
                case "nameAsc":
                    base.AddOrderBy(x => x.Name);
                    break;
                case "nameDesc":
                    base.AddOrderByDescending(x => x.Name);
                    break;
            }
        }

        public ProductBrandTypesSpecifications(int id) : base(x => x.Id == id)
        {
            base.AddInclude(x => x.ProductType);
            base.AddInclude(x => x.ProductBrand);
        }
    }
}