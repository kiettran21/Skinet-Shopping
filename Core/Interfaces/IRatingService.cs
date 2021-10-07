using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Params;

namespace Core.Interfaces
{
    public interface IRatingService
    {
        Task<Rating> AddRating(string comment, string userId, int ratingNumber, int productId);

        Task<Rating> GetRating(int ratingId);
        Task<IReadOnlyList<Rating>> GetRatings(string userId);
        Task<IReadOnlyList<Rating>> GetRatingsWithSpec(RatingParams param);
        Task<int> CountRatingsWithSpec(RatingParams param);

        Task<Rating> UpdateRating(int ratingId, string comment, int ratingNumber);
        Task<Rating> DeleteRating(int ratingId);

        // Utilities
        Task<bool> HasExistedRating(string userId, int? productId, int? ratingId);
        decimal GetAverageRating(IReadOnlyList<Rating> ratings);
    }
}