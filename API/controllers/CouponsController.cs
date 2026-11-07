using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shopforge_backend.Application.DTO;
using shopforge_backend.Application.iservices;

namespace shopforge_backend.controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController(iCouponService couponservice) : ControllerBase
    {
        // POST api/coupons/validate   body: { "code": "SHOPFORGE25" }
        // Used by the "Apply" button on the cart page.
        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] validatecoupondto dto)
        {
            var result = await couponservice.Validate(dto.Code);
            return result.ToActionResult();
        }
    }
}
