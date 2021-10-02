using Core.Entities;

namespace Core.Specifications
{
    public class RatingWithUserAndProductSpecification : BaseSpecification<Rating>
    {
        public RatingWithUserAndProductSpecification()
        {
            //AddInclude(r => r.AppUser);
            AddInclude(r => r.Product);

            AddOrderByDescending(r => r.PublishedDate);
        }

        public RatingWithUserAndProductSpecification(int? ratingId, string userId, int? productId) : base(
            x => (!ratingId.HasValue || x.Id.Equals(ratingId)) && (string.IsNullOrEmpty(userId) || x.AppUserId.Equals(userId))
                && (!productId.HasValue || x.ProductId.Equals(productId)))
        {
            //AddInclude(r => r.AppUser);
            AddInclude(r => r.Product);

            AddOrderByDescending(r => r.PublishedDate);
        }
    }
}