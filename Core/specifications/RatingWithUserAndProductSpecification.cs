using Core.Entities;
using Core.Params;

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

        public RatingWithUserAndProductSpecification(RatingParams param) : base(
            x => (string.IsNullOrEmpty(param.UserId) || x.AppUserId == param.UserId)
            && (!param.ProductId.HasValue || x.ProductId == param.ProductId)
        )
        {
            //AddInclude(r => r.AppUser);
            AddInclude(r => r.Product);

            AddOrderByDescending(r => r.PublishedDate);
            ApplyPaging(param.PageSize * (param.PageIndex - 1), param.PageSize);
        }

        public RatingWithUserAndProductSpecification(int? ratingId, string userId, int? productId) : base(
            x => (!ratingId.HasValue || x.Id == ratingId) && (string.IsNullOrEmpty(userId) || x.AppUserId == userId)
                && (!productId.HasValue || x.ProductId == productId))
        {
            //AddInclude(r => r.AppUser);
            AddInclude(r => r.Product);

            AddOrderByDescending(r => r.PublishedDate);
        }
    }
}