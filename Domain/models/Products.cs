using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace shopforge_backend.Domain.models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Image1 { get; set; } = string.Empty;
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        public int StockQuantity { get; set; }

        public bool IsAvailable { get; set; } = true;

        // NEW: shown on the product cards in the frontend (0 - 5)
        [Precision(3, 1)]
        public decimal Rating { get; set; }

        // NEW: used by the "Offers" page
        public bool IsOffer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}