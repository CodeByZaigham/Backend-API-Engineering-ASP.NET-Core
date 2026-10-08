using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shopforge_backend.models
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        // Price at the time the item was added to the cart
        [Required]
        public decimal UnitPrice { get; set; }

        // Navigation property
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}
