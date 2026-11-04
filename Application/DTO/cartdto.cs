using System;
using System.ComponentModel.DataAnnotations;

namespace shopforge_backend.Application.DTO
{
    // POST /api/cart/items
    public class addcartitemdto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
    }

    // PUT /api/cart/items/{productId}
    public class updatecartitemdto
    {
        [Range(1, 100)]
        public int Quantity { get; set; }
    }

    public class cartitemresponsedto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        // so the frontend can stop the "+" button at the available stock
        public int StockQuantity { get; set; }
    }

    // What the cart page needs: items + every number it used to calculate itself
    public class cartsummarydto
    {
        public List<cartitemresponsedto> Items { get; set; } = new();
        public int ItemCount { get; set; }
        public decimal Subtotal { get; set; }
        public string? CouponCode { get; set; }
        public int DiscountPercent { get; set; }
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}
