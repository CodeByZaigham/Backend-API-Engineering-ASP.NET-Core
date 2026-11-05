using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace shopforge_backend.Domain.models
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled
    }

    // Matches the buttons on the checkout page: card / paypal / cod
    public enum PaymentMethod
    {
        Card,
        Paypal,
        Cod
    }

    // Matches the radio buttons on the checkout page: standard / express
    public enum ShippingMethod
    {
        Standard,
        Express
    }

    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Human readable, e.g. SF-849204
        [Required]
        [MaxLength(30)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        public Guid UserId { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public ShippingMethod ShippingMethod { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        [Precision(18, 2)]
        public decimal Subtotal { get; set; }
        [Precision(18, 2)]
        public decimal Discount { get; set; }
        [Precision(18, 2)]
        public decimal ShippingCost { get; set; }
        [Precision(18, 2)]
        public decimal Tax { get; set; }
        [Precision(18, 2)]
        public decimal Total { get; set; }

        [MaxLength(50)]
        public string? CouponCode { get; set; }

        // Contact + shipping address (copied from the checkout form)
        [Required, MaxLength(200)]
        public string Email { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Phone { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required, MaxLength(300)]
        public string Address { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string City { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string State { get; set; } = string.Empty;
        [Required, MaxLength(20)]
        public string PostalCode { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public Users? User { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderId { get; set; }

        // Deliberately NOT a foreign key: if an admin deletes a product later,
        // old orders must still exist.
        public Guid ProductId { get; set; }

        // Snapshot taken at purchase time
        [Required, MaxLength(200)]
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }

        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
    }
}
