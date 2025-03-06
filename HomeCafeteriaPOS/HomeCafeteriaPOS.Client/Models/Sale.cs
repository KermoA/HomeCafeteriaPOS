namespace HomeCafeteriaPOS.Client.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public List<SaleItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Change { get; set; }
    }

    public class SaleItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}