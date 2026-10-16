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

    public async Task<Tuple<int,string>> addproducts(productdto dto)
    {
        var existingproduct=await _context.Products.FirstOrDefaultAsync(x=>x.Name==dto.Name && x.Category==dto.Category);
        if (existingproduct != null)
        {
            existingproduct.StockQuantity+=1;
            existingproduct.UpdatedAt=DateTime.UtcNow;
            _context.Products.Update(existingproduct);
            _context.SaveChanges();
            return new Tuple<int, string>(0,$"{existingproduct}");
        }
        var newproduct=new models.Product
        {
            Id=Guid.NewGuid(),
            Name=dto.Name,
            Description=dto.Description,
            Image=dto.Image,
            Price=dto.Price,
            Category=dto.Category,
            StockQuantity=dto.StockQuantity,
            IsAvailable=dto.IsAvailable,
            CreatedAt=dto.CreatedAt
        };
        _context.Products.Add(newproduct);
        await _context.SaveChangesAsync();
        return new Tuple<int, string>(
            1,
            System.Text.Json.JsonSerializer.Serialize(newproduct)
        );

    }
}
