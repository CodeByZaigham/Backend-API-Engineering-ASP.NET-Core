using System;
using shopforge_backend.Domain.models;

namespace shopforge_backend.Application.services;

public record PriceBreakdown(decimal Subtotal, decimal Discount, decimal Shipping, decimal Tax, decimal Total);

// ONE place for every pricing rule. The cart and the order both use this,
// so the number the user sees in the cart is the number they are charged.
public static class PricingCalculator
{
     public const decimal TaxRate = 0.08m;             // 8% (cart page and checkout page both use 8%)
     public const decimal StandardShippingCost = 0m;   // checkout page: standard = FREE
     public const decimal ExpressShippingCost = 18m;   // checkout page: express = $18

     public static PriceBreakdown Calculate(decimal subtotal, int discountPercent, ShippingMethod method)
     {
          var discount = Math.Round(subtotal * discountPercent / 100m, 2);

          var shipping = subtotal <= 0
               ? 0m
               : method == ShippingMethod.Express ? ExpressShippingCost : StandardShippingCost;

          var tax = Math.Round((subtotal - discount) * TaxRate, 2);
          var total = subtotal - discount + shipping + tax;

          return new PriceBreakdown(subtotal, discount, shipping, tax, total);
     }
}
