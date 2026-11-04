using System.ComponentModel.DataAnnotations;

namespace shopforge_backend.DTO
{
    // POST /api/coupons/validate
    public class validatecoupondto
    {
        [Required]
        public string Code { get; set; } = string.Empty;
    }

    public class coupondto
    {
        public string Code { get; set; } = string.Empty;
        public int DiscountPercent { get; set; }
    }
}
