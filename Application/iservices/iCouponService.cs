using System;
using shopforge_backend.Application.DTO;
using shopforge_backend.generics;
using shopforge_backend.Domain.models;

namespace shopforge_backend.Application.iservices;

public interface iCouponService
{
    // Returns the coupon only if it exists, is active and not expired. Otherwise null.
    Task<Coupon?> FindValidCoupon(string? code);
    Task<response_model<coupondto>> Validate(string code);
}
