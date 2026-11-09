using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace shopforge_backend.API.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(iOrderService orderservice) : ControllerBase
    {
        // POST api/orders   -> turns the logged-in user's cart into an order
        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] placeorderdto dto)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();

            var result = await orderservice.PlaceOrder(userId.Value, dto);
            return result.ToActionResult();
        }

        // GET api/orders   -> my orders
        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();

            var result = await orderservice.GetMyOrders(userId.Value);
            return result.ToActionResult();
        }

        // GET api/orders/{id}   -> one of my orders (admins can open any order)
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var userId = User.GetUserId();
            if (userId is null) return Unauthorized();

            var result = await orderservice.GetOrder(userId.Value, id, User.IsInRole("Admin"));
            return result.ToActionResult();
        }
        
    }

    
}
