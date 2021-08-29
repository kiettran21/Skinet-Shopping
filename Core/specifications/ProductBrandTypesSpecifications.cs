using Core.Entities;

namespace Core.Specifications
{
    public class ProductBrandTypesSpecifications : BaseSpecification<Product>
    {
        public ProductBrandTypesSpecifications() : base()
        {
            base.AddInclude(x => x.ProductType);
            base.AddInclude(x => x.ProductBrand);
        }

        public ProductBrandTypesSpecifications(int id) : base(x => x.Id == id)
        {
            base.AddInclude(x => x.ProductType);
            base.AddInclude(x => x.ProductBrand);
        }
    }
}