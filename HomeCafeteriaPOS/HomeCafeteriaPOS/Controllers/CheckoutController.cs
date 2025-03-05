using HomeCafeteriaPOS.Data;
using HomeCafeteriaPOS.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeCafeteriaPOS.Controllers
{
    [Route("api/checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly HomeCafeteriaDbContext _context;

        public CheckoutController(HomeCafeteriaDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<object>> ProcessCheckout(CheckoutRequest request)
        {
            decimal totalAmount = 0;
            List<SaleItem> saleItems = new();

            foreach (var item in request.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null || product.Quantity < item.Quantity)
                {
                    return BadRequest($"Not enough stock for product ID {item.ProductId}");
                }

                product.Quantity -= item.Quantity;
                decimal itemTotal = item.Quantity * product.Price;
                totalAmount += itemTotal;

                saleItems.Add(new SaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                });
            }

            if (request.PaidAmount < totalAmount)
            {
                return BadRequest("Paid amount is less than total price.");
            }

            decimal change = request.PaidAmount - totalAmount;
            var changeBreakdown = CalculateChange(change);

            var sale = new Sale
            {
                Items = saleItems,
                TotalAmount = totalAmount
            };
            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                TotalAmount = totalAmount,
                PaidAmount = request.PaidAmount,
                Change = change,
                ChangeBreakdown = changeBreakdown
            });
        }

        private Dictionary<decimal, int> CalculateChange(decimal change)
        {
            decimal[] denominations = { 50.00m, 20.00m, 10.00m, 5.00m, 2.00m, 1.00m, 0.50m, 0.20m, 0.10m, 0.05m };
            Dictionary<decimal, int> changeBreakdown = new();

            foreach (var denom in denominations)
            {
                int count = (int)(change / denom);
                if (count > 0)
                {
                    changeBreakdown[denom] = count;
                    change -= count * denom;
                }
            }

            return changeBreakdown;
        }
    }

    public class CheckoutRequest
    {
        public List<CheckoutItem> Items { get; set; } = new();
        public decimal PaidAmount { get; set; }
    }

    public class CheckoutItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
