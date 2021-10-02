using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork unitOfWork;

        public RatingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Rating> AddRating(string comment, string userId, int ratingNumber, int productId)
        {
            // Check product is exsits
            var product = await unitOfWork.Repository<Product>().GetByIdAsync(productId);

            if (product == null) throw new System.Exception("Prouct is not found with id " + productId);

            var rating = new Rating()
            {
                Comment = comment,
                RatingNumber = ratingNumber,
                AppUserId = userId,
                ProductId = product.Id
            };

            unitOfWork.Repository<Rating>().Add(rating);

            await unitOfWork.Complete();

            return rating;
        }

        public async Task<Rating> DeleteRating(int ratingId)
        {

            var rating = await unitOfWork.Repository<Rating>().GetByIdAsync(ratingId);

            if (rating == null) return null;

            unitOfWork.Repository<Rating>().Delete(rating);

            await unitOfWork.Complete();

            return rating;
        }

        public async Task<Rating> GetRating(int ratingId)
        {
            var specification = new RatingWithUserAndProductSpecification(ratingId, null, null);

            var rating = await unitOfWork.Repository<Rating>().GetEntityWithSpec(specification);

            return rating;
        }

        public async Task<IReadOnlyList<Rating>> GetRatings(string userId)
        {
            var specification = new RatingWithUserAndProductSpecification(null, userId, null);
            var ratings = await unitOfWork.Repository<Rating>().GetAllWithSpec(specification);

            return ratings;
        }

        public async Task<Rating> UpdateRating(int ratingId, string comment, int ratingNumber)
        {
            var rating = await unitOfWork.Repository<Rating>().GetByIdAsync(ratingId);

            if (rating == null) return null;

            rating.Comment = comment;
            rating.RatingNumber = ratingNumber;

            unitOfWork.Repository<Rating>().Update(rating);

            await unitOfWork.Complete();

            return rating;
        }

         public async Task<bool> HasExistedRating(string userId, int? productId, int? ratingId)
         {
            // 1. Check user has rated yet
            var specification = new RatingWithUserAndProductSpecification(ratingId, userId, productId);
            var rating = await unitOfWork.Repository<Rating>().GetEntityWithSpec(specification);

            return rating != null;
         }
    }
}