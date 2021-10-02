using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IRatingService
    {
        Task<Rating> AddRating(string comment, string userId, int ratingNumber, int productId);

        Task<Rating> GetRating(int ratingId);
        Task<IReadOnlyList<Rating>> GetRatings(string userId);

        Task<Rating> UpdateRating(int ratingId, string comment, int ratingNumber);
        Task<Rating> DeleteRating(int ratingId);

        Task<bool> HasExistedRating(string userId, int? productId, int? ratingId);
    }
}