using System;
using shopforge_backend.Application.DTO;

namespace shopforge_backend.Application.iservices;

public interface iProductsService
{
    Task<Tuple<int,List<productdto>>> Getallproducts(productqueryparams query);
    Task<productdto?> GetProductById(Guid id);
    Task<Tuple<int,string>> addproducts(productdto dto);
    Task<Tuple<int,string>> UpdateProduct(productdto dto);
    Task<Tuple<int,string>> DeleteProduct(Guid Productid);
}
