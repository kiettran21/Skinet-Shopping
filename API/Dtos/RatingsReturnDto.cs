using System.Collections.Generic;
using Core.Entities;

namespace API.Dtos
{
    public class RatingsReturnDto
    {
        public IReadOnlyList<Rating> Ratings { get; set; }
        public int Count { get; set; }
        public decimal AverageRating { get; set; }
    }
}