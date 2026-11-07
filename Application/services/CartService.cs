using System;
using Microsoft.EntityFrameworkCore;
using shopforge_backend.Infrastructure.config;
using shopforge_backend.Application.DTO;
using shopforge_backend.generics;
using shopforge_backend.Application.iservices;
using shopforge_backend.Domain.models;

namespace shopforge_backend.Application.services;

public class CartService(appdbcontext _context, iCouponService couponService) : iCartService
{
     public async Task<response_model<cartsummarydto>> GetCart(Guid userId, string? couponCode, ShippingMethod shippingMethod)
     {
          var summary = await BuildSummary(userId, couponCode, shippingMethod);
          return response_model<cartsummarydto>.success(summary, "cart loaded");
     }

     public async Task<response_model<cartsummarydto>> AddItem(Guid userId, addcartitemdto dto, string? couponCode)
     {
          var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);
          if (product == null)
               return response_model<cartsummarydto>.fail(null, "Product not found.", 404);

          if (!product.IsAvailable || product.StockQuantity <= 0)
               return response_model<cartsummarydto>.fail(null, "This product is out of stock.");

          var existing = await _context.Cart
               .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == dto.ProductId);

          var newQuantity = (existing?.Quantity ?? 0) + dto.Quantity;
          if (newQuantity > product.StockQuantity)
               return response_model<cartsummarydto>.fail(null, $"Only {product.StockQuantity} left in stock.");

          if (existing != null)
          {
               existing.Quantity = newQuantity;
          }
          else
          {
               _context.Cart.Add(new CartItem
               {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
               });
          }

          await _context.SaveChangesAsync();

          var summary = await BuildSummary(userId, couponCode, ShippingMethod.Standard);
          return response_model<cartsummarydto>.success(summary, "item added to cart");
     }

     public async Task<response_model<cartsummarydto>> UpdateItem(Guid userId, Guid productId, updatecartitemdto dto, string? couponCode)
     {
          var item = await _context.Cart
               .Include(c => c.Product)
               .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

          if (item == null)
               return response_model<cartsummarydto>.fail(null, "Item is not in your cart.", 404);

          if (item.Product != null && dto.Quantity > item.Product.StockQuantity)
               return response_model<cartsummarydto>.fail(null, $"Only {item.Product.StockQuantity} left in stock.");

          item.Quantity = dto.Quantity;
          await _context.SaveChangesAsync();

          var summary = await BuildSummary(userId, couponCode, ShippingMethod.Standard);
          return response_model<cartsummarydto>.success(summary, "cart updated");
     }

     public async Task<response_model<cartsummarydto>> RemoveItem(Guid userId, Guid productId, string? couponCode)
     {
          var item = await _context.Cart
               .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

          if (item == null)
               return response_model<cartsummarydto>.fail(null, "Item is not in your cart.", 404);

          _context.Cart.Remove(item);
          await _context.SaveChangesAsync();

          var summary = await BuildSummary(userId, couponCode, ShippingMethod.Standard);
          return response_model<cartsummarydto>.success(summary, "item removed");
     }
     public async Task<response_model<cartsummarydto>> Clear(Guid userId)
     {
          var items = await _context.Cart.Where(c => c.UserId == userId).ToListAsync();
          _context.Cart.RemoveRange(items);
          await _context.SaveChangesAsync();

          var summary = await BuildSummary(userId, null, ShippingMethod.Standard);
          return response_model<cartsummarydto>.success(summary, "cart cleared");
     }
}