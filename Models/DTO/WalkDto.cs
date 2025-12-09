using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class WalkDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(100)]

        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        [Range(3, 100)]
        public double LengthInkm { get; set; }

        public string? WalkImageUrl { get; set; }

        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
