using System;
using System.ComponentModel.DataAnnotations;
using shopforge_backend.Domain.models;

namespace shopforge_backend.Application.DTO
{
    // POST /api/orders  (the items come from the user's server-side cart)
    public class placeorderdto
    {
        // contact
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Phone { get; set; } = string.Empty;

        // shipping address
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string State { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;

        // "standard" | "express"
        public ShippingMethod ShippingMethod { get; set; } = ShippingMethod.Standard;

        // "card" | "paypal" | "cod"   (only the METHOD is stored, never card details)
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cod;

        public string? CouponCode { get; set; }
    }

    public class orderitemdto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class orderresponsedto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string? CouponCode { get; set; }

        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public List<orderitemdto> Items { get; set; } = new();
    }

    // PATCH /api/orders/{id}/status  (admin)
    public class updateorderstatusdto
    {
        public OrderStatus Status { get; set; }
    }
}
