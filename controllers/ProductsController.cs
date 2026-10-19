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
        [HttpGet("GetProducts")]
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

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProducts(productdto dto)
        {
            try
            {
                var addedproduct = await productservice.addproducts(dto);
                if(addedproduct.Item1 == 0)
                {
                    return Ok(response_model<string>.success(addedproduct.Item2 , "quantity updated of given product!"));  
                }
                return Ok(response_model<string>.success(addedproduct.Item2 , "product added successfully!"));  
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPatch("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(productdto dto)
        {
            try
            {
                var updatedproduct = await productservice.UpdateProduct(dto);
                if (updatedproduct.Item1 == 0)
                {
                    return Ok(response_model<string>.success(updatedproduct.Item2 , "Product details Updated!"));  
                }
                return BadRequest();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

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
