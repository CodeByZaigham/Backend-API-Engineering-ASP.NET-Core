using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shopforge_backend.generics;
using shopforge_backend.Application.DTO;
using shopforge_backend.Application.iservices;
using shopforge_backend.Domain.models;

namespace shopforge_backend.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(iProductsService productservice) : ControllerBase
    {
        // GET api/Products/GetProducts
        // Optional filters: ?category=shoes&search=leather&maxPrice=150&sort=price-low&isOffer=true
        // category can be: all | shoes | shirts | pants | offers
        // sort can be: featured | price-low | price-high | rating
        [HttpGet("GetProducts")]
        public async Task<IActionResult> get_all_products([FromQuery] productqueryparams query)
        {
            var products = await productservice.Getallproducts(query);

            // An empty list is a normal answer, not an error
            var message = products.Item2.Any() ? "products found!" : "no products found";
            return Ok(response_model<List<productdto>>.success(products.Item2, message));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await productservice.GetProductById(id);

            if (product == null)
                return NotFound(response_model<productdto>.fail(null, "product not found."));

            return Ok(response_model<productdto>.success(product, "product found."));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProducts(productdto dto)
        {
            var addedproduct = await productservice.addproducts(dto);
            if (addedproduct.Item1 == 0)
            {
                return Ok(response_model<string>.success(addedproduct.Item2, "quantity updated of given product!"));
            }
            return Ok(response_model<string>.success(addedproduct.Item2, "product added successfully!"));
        }

         [Authorize(Roles = "Admin")]
        [HttpPatch("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(productdto dto)
        {
            var updatedproduct = await productservice.UpdateProduct(dto);
            if (updatedproduct.Item1 == 1)
            {
                return Ok(response_model<string>.success(updatedproduct.Item2, "Product details Updated!"));
            }
            return NotFound(response_model<string>.fail(null, updatedproduct.Item2));
        }
        
        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(Guid ProductId)
        {
            try
            {
                var deletedproduct = await productservice.DeleteProduct(ProductId);
                if(deletedproduct.Item1 == 0)
                {
                    return NotFound(response_model<string>.fail(deletedproduct.Item2 , "Product not found!"));
                }
                return Ok(response_model<string>.success(deletedproduct.Item2 , "Product deleted!"));
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }
}
