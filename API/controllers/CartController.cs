using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopforge_backend.Application.DTO;
using shopforge_backend.Application.iservices;
using shopforge_backend.Domain.models;
using shopforge_backend.generics;

namespace shopforge_backend.API.controllers
{
    // Every endpoint needs the Bearer token. The user id comes from the token,
    // never from the request body, so nobody can touch someone else's cart.
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(iCartService cartservice) : ControllerBase
    {
        // GET api/cart?couponCode=SHOPFORGE25&shippingMethod=express
        [HttpGet]
        public async Task<IActionResult> GetCart([FromQuery] string? couponCode, [FromQuery] ShippingMethod shippingMethod = ShippingMethod.Standard)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();

            var result = await cartservice.GetCart(userId.Value, couponCode, shippingMethod);
            return result.ToActionResult();
        }

        // POST api/cart/items   body: { "productId": "...", "quantity": 1 }
        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] addcartitemdto dto, [FromQuery] string? couponCode)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();

            var result = await cartservice.AddItem(userId.Value, dto, couponCode);
            return result.ToActionResult();
        }
    }
}
