using System;
using Microsoft.EntityFrameworkCore;
using shopforge_backend.Infrastructure.config;
using shopforge_backend.Application.DTO;
using shopforge_backend.generics;
using shopforge_backend.Application.iservices;
using shopforge_backend.Domain.models;

namespace shopforge_backend.Application.services;

public class OrderService(appdbcontext _context, iCouponService couponService) : iOrderService
{
     public async Task<response_model<orderresponsedto>> PlaceOrder(Guid userId, placeorderdto dto)
     {
          // Products are loaded WITH tracking on purpose: we reduce their stock below.
          var cartRows = await _context.Cart
               .Include(c => c.Product)
               .Where(c => c.UserId == userId)
               .ToListAsync();

          if (cartRows.Count == 0)
               return response_model<orderresponsedto>.fail(null, "Your cart is empty.");

          foreach (var row in cartRows)
          {
               var product = row.Product;
               if (product == null || !product.IsAvailable)
                    return response_model<orderresponsedto>.fail(null, "A product in your cart is no longer available.");

               if (row.Quantity > product.StockQuantity)
                    return response_model<orderresponsedto>.fail(null,
                         $"{product.Name}: only {product.StockQuantity} left in stock.");
          }

          string? couponCode = null;
          var discountPercent = 0;
          if (!string.IsNullOrWhiteSpace(dto.CouponCode))
          {
               var coupon = await couponService.FindValidCoupon(dto.CouponCode);
               if (coupon == null)
                    return response_model<orderresponsedto>.fail(null, "Invalid or expired coupon code.");

               couponCode = coupon.Code;
               discountPercent = coupon.DiscountPercent;
          }

          var subtotal = cartRows.Sum(r => r.Product!.Price * r.Quantity);
          var price = PricingCalculator.Calculate(subtotal, discountPercent, dto.ShippingMethod);

          var order = new Order
          {
               OrderNumber = await GenerateOrderNumber(),
               UserId = userId,
               Status = OrderStatus.Pending,
               ShippingMethod = dto.ShippingMethod,
               PaymentMethod = dto.PaymentMethod,
               Subtotal = price.Subtotal,
               Discount = price.Discount,
               ShippingCost = price.Shipping,
               Tax = price.Tax,
               Total = price.Total,
               CouponCode = couponCode,
               Email = dto.Email,
               Phone = dto.Phone,
               FirstName = dto.FirstName,
               LastName = dto.LastName,
               Address = dto.Address,
               City = dto.City,
               State = dto.State,
               PostalCode = dto.PostalCode,
               Items = cartRows.Select(r => new OrderItem
               {
                    ProductId = r.ProductId,
                    ProductName = r.Product!.Name,
                    ProductImage = r.Product.Image1,
                    UnitPrice = r.Product.Price,
                    Quantity = r.Quantity
               }).ToList()
          };

          foreach (var row in cartRows)
               row.Product!.StockQuantity -= row.Quantity;

          _context.Orders.Add(order);
          _context.Cart.RemoveRange(cartRows);

          // One SaveChanges = one database transaction: order + stock + cart clear
          // either all happen or none happen.
          await _context.SaveChangesAsync();

          return response_model<orderresponsedto>.success(ToDto(order), "Order placed successfully!");
     }

     public async Task<response_model<List<orderresponsedto>>> GetMyOrders(Guid userId)
     {
          var orders = await _context.Orders
               .AsNoTracking()
               .Include(o => o.Items)
               .Where(o => o.UserId == userId)
               .OrderByDescending(o => o.CreatedAt)
               .ToListAsync();

          return response_model<List<orderresponsedto>>.success(orders.Select(ToDto).ToList(), "orders loaded");
     }  

     public async Task<response_model<orderresponsedto>> GetOrder(Guid userId, Guid orderId, bool isAdmin)
     {
          var order = await _context.Orders
               .AsNoTracking()
               .Include(o => o.Items)
               .FirstOrDefaultAsync(o => o.Id == orderId);

          // Same message for "doesn't exist" and "not yours" so ids can't be probed
          if (order == null || (!isAdmin && order.UserId != userId))
               return response_model<orderresponsedto>.fail(null, "Order not found.", 404);

          return response_model<orderresponsedto>.success(ToDto(order), "order loaded");
     }
     
     public async Task<response_model<List<orderresponsedto>>> GetAllOrders()
     {
          var orders = await _context.Orders
               .AsNoTracking()
               .Include(o => o.Items)
               .OrderByDescending(o => o.CreatedAt)
               .ToListAsync();

          return response_model<List<orderresponsedto>>.success(orders.Select(ToDto).ToList(), "orders loaded");
     }

     public async Task<response_model<orderresponsedto>> UpdateStatus(Guid orderId, updateorderstatusdto dto)
     {
          var order = await _context.Orders
               .Include(o => o.Items)
               .FirstOrDefaultAsync(o => o.Id == orderId);

          if (order == null)
               return response_model<orderresponsedto>.fail(null, "Order not found.", 404);

          order.Status = dto.Status;
          await _context.SaveChangesAsync();

          return response_model<orderresponsedto>.success(ToDto(order), "order status updated");
     }

     private async Task<string> GenerateOrderNumber()
     {
          for (var attempt = 0; attempt < 5; attempt++)
          {
               var number = $"SF-{Random.Shared.Next(100000, 999999)}";
               if (!await _context.Orders.AnyAsync(o => o.OrderNumber == number))
                    return number;
          }
          return $"SF-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
     }
}