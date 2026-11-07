using System;
using Microsoft.EntityFrameworkCore;
using shopforge_backend.Infrastructure.config;
using shopforge_backend.Application.DTO;
using shopforge_backend.generics;
using shopforge_backend.Application.iservices;
using shopforge_backend.Domain.models;

namespace shopforge_backend.services;

public class CouponService(appdbcontext _context) : iCouponService
{
    public async Task<Coupon?> FindValidCoupon(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return null;

        var normalized = code.Trim().ToUpper();
        var now = DateTime.UtcNow;

        return await _context.Coupons
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Code == normalized
                                      && c.IsActive
                                      && (c.ExpiresAt == null || c.ExpiresAt > now));
    }

    public async Task<response_model<coupondto>> Validate(string code)
    {
        var coupon = await FindValidCoupon(code);
        if (coupon == null)
            return response_model<coupondto>.fail(null, "Invalid coupon code.", 404);

        return response_model<coupondto>.success(
            new coupondto { Code = coupon.Code, DiscountPercent = coupon.DiscountPercent },
            $"{coupon.DiscountPercent}% discount applied successfully!");
    }
}
