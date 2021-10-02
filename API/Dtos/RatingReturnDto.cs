using System;

namespace API.Dtos
{
    public class RatingReturnDto
    {
        public string Comment { get; set; }
        public int RatingNumber { get; set; }
        public DateTime PublishedDate { get; set; }

        public string AppUserId { get; set; }

        public int ProductId { get; set; }
    }
}