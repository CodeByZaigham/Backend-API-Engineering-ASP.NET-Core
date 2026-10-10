using System;
using System.ComponentModel.DataAnnotations;

namespace shopforge_backend.DTO
{
    public class productdto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(200)]
        required public string Name { get; set; } = string.Empty;

        public string? Image { get; set; }

        public string? Description { get; set; }
        required public decimal Price { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        public int StockQuantity { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }   
}
