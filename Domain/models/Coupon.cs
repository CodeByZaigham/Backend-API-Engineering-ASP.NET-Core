using System.ComponentModel.DataAnnotations;

namespace shopforge_backend.models
{
    public class Coupon
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Stored in UPPER CASE, e.g. SHOPFORGE25
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Range(1, 100)]
        public int DiscountPercent { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? ExpiresAt { get; set; }
    }
}
