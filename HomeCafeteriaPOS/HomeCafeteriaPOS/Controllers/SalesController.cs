using HomeCafeteriaPOS.Data;
using HomeCafeteriaPOS.Hubs;
using HomeCafeteriaPOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HomeCafeteriaPOS.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly HomeCafeteriaDbContext _context;
        private readonly IHubContext<SaleHub> _hubContext;

        public SalesController(HomeCafeteriaDbContext context, IHubContext<SaleHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
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
            await _hubContext.Clients.All.SendAsync("ReceiveNewSale", sale);
            return CreatedAtAction(nameof(GetSales), new { id = sale.Id }, sale);
        }
    }
}
