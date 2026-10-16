using System;
using shopforge_backend.DTO;

namespace shopforge_backend.iservices;

public interface iProductsService
{
    Task<Tuple<int,List<productdto>>> Getallproducts();
    Task<Tuple<int,string>> addproducts(productdto dto);
    Task<Tuple<int,string>> UpdateProduct(productdto dto);
}
