using System;
using Microsoft.EntityFrameworkCore;
using shopforge_backend.config;
using shopforge_backend.Application.DTO;
using shopforge_backend.Application.iservices;
using shopforge_backend.Domain.models;

namespace shopforge_backend.Application.services;

public class ProductsService(appdbcontext _context):iProductsService
{
    public async Task<Tuple<int,List<productdto>>> Getallproducts(productqueryparams query)
    {
        IQueryable<Product> q = _context.Products.AsNoTracking();

        // category: "all" or empty = no filter, "offers" = discounted items
        if (!string.IsNullOrWhiteSpace(query.Category)
            && !query.Category.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            if (query.Category.Equals("offers", StringComparison.OrdinalIgnoreCase))
            {
                q = q.Where(p => p.IsOffer);
            }
            else
            {
                var category = query.Category.Trim().ToLower();
                q = q.Where(p => p.Category != null && p.Category.ToLower() == category);
            }
        }

        if (query.IsOffer == true)
            q = q.Where(p => p.IsOffer);

        if (query.MaxPrice.HasValue)
            q = q.Where(p => p.Price <= query.MaxPrice.Value);

        // same fields the frontend search looked at: name, description, category
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim().ToLower();
            q = q.Where(p => p.Name.ToLower().Contains(search)
                          || (p.Description != null && p.Description.ToLower().Contains(search))
                          || (p.Category != null && p.Category.ToLower().Contains(search)));
        }

        q = (query.Sort?.ToLower()) switch
        {
            "price-low"  => q.OrderBy(p => p.Price),
            "price-high" => q.OrderByDescending(p => p.Price),
            "rating"     => q.OrderByDescending(p => p.Rating),
            _            => q.OrderBy(p => p.CreatedAt).ThenBy(p => p.Name)
        };

        var products = await q.Select(x => new productdto
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
            Rating=x.Rating,
            IsOffer=x.IsOffer,
            CreatedAt=x.CreatedAt,
            UpdatedAt=x.UpdatedAt
        }).ToListAsync();

        return new Tuple<int,List<productdto>>(1, products);
    }

    public async Task<productdto?> GetProductById(Guid id)
    {
        var x = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (x == null)
            return null;

        return new productdto
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
            Rating=x.Rating,
            IsOffer=x.IsOffer,
            CreatedAt=x.CreatedAt,
            UpdatedAt=x.UpdatedAt
        };
    }

    public async Task<Tuple<int,string>> addproducts(productdto dto)
    {
        var existingproduct=await _context.Products.FirstOrDefaultAsync(x=>x.Name==dto.Name && x.Category==dto.Category);
        if (existingproduct != null)
        {
            existingproduct.StockQuantity+=dto.StockQuantity;
            existingproduct.UpdatedAt=DateTime.UtcNow;
            //this will only update quatity, if you want to update product details you have another endpoint
            await _context.SaveChangesAsync();
            return new Tuple<int, string>(0,$"{System.Text.Json.JsonSerializer.Serialize(existingproduct)}");
        }
        var newproduct=new Product
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
            Rating=dto.Rating,
            IsOffer=dto.IsOffer,
            CreatedAt=DateTime.UtcNow
        };
        _context.Products.Add(newproduct);
        await _context.SaveChangesAsync();
        return new Tuple<int, string>(
            1,
            System.Text.Json.JsonSerializer.Serialize(newproduct)
        );
    }

    public async Task<Tuple<int,string>> UpdateProduct(productdto dto)
    {
        var product = await _context.Products
        .FirstOrDefaultAsync(p => p.Id == dto.Id);

        if (product == null)
            return new Tuple<int, string>(0, "Product not found");

        product.Name = string.IsNullOrWhiteSpace(dto.Name) ? product.Name : dto.Name;
        product.Description = string.IsNullOrWhiteSpace(dto.Description) ? product.Description : dto.Description;
        product.Category = string.IsNullOrWhiteSpace(dto.Category) ? product.Category : dto.Category;   // NEW: was never updated before
        product.Image1 = string.IsNullOrWhiteSpace(dto.Image1) ? product.Image1 : dto.Image1;
        product.Image2 = string.IsNullOrWhiteSpace(dto.Image2) ? product.Image2 : dto.Image2;
        product.Image3 = string.IsNullOrWhiteSpace(dto.Image3) ? product.Image3 : dto.Image3;

        if (dto.Price > 0)
            product.Price = dto.Price;

        if (dto.StockQuantity >= 0)
            product.StockQuantity = dto.StockQuantity;

        if (dto.Rating > 0)
            product.Rating = dto.Rating;

        product.IsAvailable = dto.IsAvailable;
        product.IsOffer = dto.IsOffer;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new Tuple<int, string>(
            1,
            System.Text.Json.JsonSerializer.Serialize(product)
        );
    }

    public async Task<Tuple<int,string>> DeleteProduct(Guid Productid)
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
}
