using System;
using shopforge_backend.Application.DTO;
using shopforge_backend.generics;
using shopforge_backend.Domain.models;

namespace shopforge_backend.Application.iservices;

public interface iCartService
{
    Task<response_model<cartsummarydto>> GetCart(Guid userId, string? couponCode, ShippingMethod shippingMethod);
    Task<response_model<cartsummarydto>> AddItem(Guid userId, addcartitemdto dto, string? couponCode);
    Task<response_model<cartsummarydto>> UpdateItem(Guid userId, Guid productId, updatecartitemdto dto, string? couponCode);
    Task<response_model<cartsummarydto>> RemoveItem(Guid userId, Guid productId, string? couponCode);
    Task<response_model<cartsummarydto>> Clear(Guid userId);
}
