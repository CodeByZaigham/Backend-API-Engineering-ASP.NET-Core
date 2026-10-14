using System;
using Microsoft.EntityFrameworkCore;
using shopforge_backend.config;
using shopforge_backend.DTO;
using shopforge_backend.iservices;

namespace shopforge_backend.services;

public class ProductsService(appdbcontext _context):iProductsService
{
    public async Task<Tuple<int,List<productdto>>> Getallproducts()
    {
        return new Tuple<int,List<productdto>>(1, await _context.Products.AsNoTracking().Select(x=>new productdto
        {
            Name=x.Name,
            Image=x.Image,
            Description=x.Description,
            Price=x.Price,
            Category=x.Category,
            StockQuantity=x.StockQuantity,
            IsAvailable=x.IsAvailable,
            CreatedAt=x.CreatedAt,
            UpdatedAt=x.UpdatedAt
        }).ToListAsync());
    }
}
