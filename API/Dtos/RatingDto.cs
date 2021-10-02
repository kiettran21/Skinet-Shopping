using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RatingDto
    {
        [Required]
        [MaxLength(200)]
        public string Comment { get; set; }

        [Required]
        [Range(0, 5)]
        public int RatingNumber { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}