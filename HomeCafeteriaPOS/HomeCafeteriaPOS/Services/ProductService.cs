using HomeCafeteriaPOS.Data;
using HomeCafeteriaPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeCafeteriaPOS.Services
{
    public class ProductService
    {
        private readonly HomeCafeteriaDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductService(HomeCafeteriaDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task LoadProductsAsync()
        {
            var products = _configuration.GetSection("Products").Get<List<Product>>();

            foreach (var product in products)
            {
                // Check if the product already exists
                if (!await _context.Products.AnyAsync(p => p.Id == product.Id))
                {
                    _context.Products.Add(product);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
