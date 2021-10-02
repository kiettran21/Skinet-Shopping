using System;
using Core.Entities.Identity;

namespace Core.Entities
{
    public class Rating : BaseEntity
    {
        public string Comment { get; set; }
        public int RatingNumber { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Now;

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}