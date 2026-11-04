using System;
using shopforge_backend.Application.DTO;
using shopforge_backend.models;

namespace shopforge_backend.Application.iservices;

public interface iProductsService
{
    Task<Tuple<int,List<productdto>>> Getallproducts();
    Task<Product?> GetProductById(Guid id);
    Task<Tuple<int,string>> addproducts(productdto dto);
    Task<Tuple<int,string>> UpdateProduct(productdto dto);
    Task<Tuple<int,string>> DeleteProduct(Guid Productid);
}
