using System;
using shopforge_backend.models;

namespace shopforge_backend.DTO
{
    public class cartdto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

            required public Guid UserId { get; set; }
            required public Guid ProductId { get; set; }
            required public int Quantity { get; set; } = 1;
            // Price at the time the item was added to the cart
            required public decimal UnitPrice { get; set; }
            public Product? Product { get; set; }
    }   
}
