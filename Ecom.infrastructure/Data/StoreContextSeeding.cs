using Ecom.Core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data
{
    public class StoreContextSeeding
    {
        public static async Task SeedAsync (AppDbContext _dbContext)
        {
            var CategoryData = File.ReadAllText("../Ecom.infrastructure/Data/Data Seeding/categories.json");
            var Data = JsonSerializer.Deserialize<List<Category>>(CategoryData);
            if (Data.Count() > 0)
            {
                if (_dbContext.Categories.Count() == 0)
                {
                    foreach (var category in Data)
                    {
                        _dbContext.Set<Category>().Add(category);
                    }
                    await _dbContext.SaveChangesAsync();
                }

            }

            var ProductData = File.ReadAllText("../Ecom.infrastructure/Data/Data Seeding/products.json");
            var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
            if (Products.Count() > 0)
            {
                if (_dbContext.Products.Count() == 0)
                {
                    foreach (var product in Products)
                    {
                        _dbContext.Set<Product>().Add(product);
                    }
                    await _dbContext.SaveChangesAsync();
                }

            }

            var PhotoData = File.ReadAllText("../Ecom.infrastructure/Data/Data Seeding/Photos.json");
            var photos = JsonSerializer.Deserialize<List<Photo>>(PhotoData);
            if (photos.Count() > 0)
            {
                if (_dbContext.Photos.Count() == 0)
                {
                    foreach (var photo in photos)
                    {
                        _dbContext.Set<Photo>().Add(photo);
                    }
                    await _dbContext.SaveChangesAsync();
                }

            }

        }


    }
}
