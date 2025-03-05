using HomeCafeteriaPOS.Data;
using HomeCafeteriaPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeCafeteriaPOS.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly HomeCafeteriaDbContext _context;

        public SalesController(HomeCafeteriaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            return await _context.Sales.Include(s => s.Items).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> CreateSale(Sale sale)
        {
            foreach (var item in sale.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null || product.Quantity < item.Quantity)
                {
                    return BadRequest($"Not enough stock for product ID {item.ProductId}");
                }
                product.Quantity -= item.Quantity;
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSales), new { id = sale.Id }, sale);
        }
    }
}
