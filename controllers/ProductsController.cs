using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopforge_backend.DTO;
using shopforge_backend.generics;
using shopforge_backend.iservices;

namespace shopforge_backend.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(iProductsService productservice) : ControllerBase
    {
        public async Task<IActionResult> get_all_products()
        {
            try
            {
                var products=await productservice.Getallproducts();
                if (!products.Item2.Any())
                {
                    return NotFound(response_model<List<productdto>>.fail(null,"no products found!"));
                }
                return Ok(response_model<List<productdto>>.success(products.Item2,"products found!"));
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }
}
