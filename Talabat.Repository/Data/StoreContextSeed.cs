using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
       public static async Task SeedAsync(StoreContext context)
        {

            if (context.ProductBrands.Count() == 0)
            {
                var brandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        context.Set<ProductBrand>().Add(brand);
                    }
                    await context.SaveChangesAsync();
                }  
            }

            if (context.ProductCategories.Count() == 0)
            {
                var categoryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoryData);
                if (categories?.Count > 0)
                {
                    foreach (var category in categories)
                    {
                        context.Set<ProductCategory>().Add(category);
                    }
                    await context.SaveChangesAsync();
                }
            }

            if (context.Products.Count() == 0)
            {
                var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                    {
                        
                            context.Set<Product>().Add(product);
                      
                    }
                    await context.SaveChangesAsync();
                }
            }
            if (context.DeliveryMethods.Count() == 0)
            {
                var deliverymethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var deliverymethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverymethodsData);
                if (deliverymethods?.Count > 0)
                {
                    foreach (var deliverymethod in deliverymethods)
                    {

                        context.Set<DeliveryMethod>().Add(deliverymethod);

                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
