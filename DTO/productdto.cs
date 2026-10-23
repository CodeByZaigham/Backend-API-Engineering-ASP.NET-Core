using System;
using System.ComponentModel.DataAnnotations;

namespace shopforge_backend.DTO
{
    public class productdto
    {
        public Guid Id { get; set; } 

        [MaxLength(200)]
        required public string Name { get; set; } = string.Empty;

        required public string Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }

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
