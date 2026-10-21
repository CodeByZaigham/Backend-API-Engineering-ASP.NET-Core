using System;
using Microsoft.EntityFrameworkCore;
using shopforge_backend.config;
using shopforge_backend.DTO;
using shopforge_backend.iservices;
using shopforge_backend.Migrations;
using shopforge_backend.models;

namespace shopforge_backend.services;

public class ProductsService(appdbcontext _context):iProductsService
{
    public async Task<Tuple<int,List<productdto>>> Getallproducts()
    {
        return new Tuple<int,List<productdto>>(1, await _context.Products.AsNoTracking().Select(x=>new productdto
        {
            Id=x.Id,
            Name=x.Name,
            Image1=x.Image1,
            Image2=x.Image2,
            Image3=x.Image3,
            Description=x.Description,
            Price=x.Price,
            Category=x.Category,
            StockQuantity=x.StockQuantity,
            IsAvailable=x.IsAvailable,
            CreatedAt=x.CreatedAt,
            UpdatedAt=x.UpdatedAt
        }).ToListAsync());
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
    } // one commit

    public async Task<Tuple<int,string>> addproducts(productdto dto)
    {
        try
        {
            var existingproduct=await _context.Products.FirstOrDefaultAsync(x=>x.Name==dto.Name && x.Category==dto.Category);
            if (existingproduct != null)
            {
                existingproduct.StockQuantity+=dto.StockQuantity;
                existingproduct.UpdatedAt=DateTime.UtcNow;
                _context.Products.Update(existingproduct); //this will only update quatity, if you want to update product details you have another endpoint
                await _context.SaveChangesAsync();
                return new Tuple<int, string>(0,$"{System.Text.Json.JsonSerializer.Serialize(existingproduct)}");
            }
            var newproduct=new models.Product
            {
                Id=Guid.NewGuid(),
                Name=dto.Name,
                Description=dto.Description,
                Image1=dto.Image1,
                Image2=dto.Image2,
                Image3=dto.Image3,
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
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<Tuple<int,string>> UpdateProduct(productdto dto)
    {
      
    }  

    public async Task<Tuple<int,string>> DeleteProduct(Guid Productid)
    {
        try
        {
            var existingproduct=await _context.Products.FirstOrDefaultAsync(x=>x.Id==Productid);
            if (existingproduct == null)
            {
                return new Tuple<int, string>(0, "Product not found");
            }

            _context.Products.Remove(existingproduct);
            await _context.SaveChangesAsync();

            return new Tuple<int, string>(1, "Product deleted successfully");
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
