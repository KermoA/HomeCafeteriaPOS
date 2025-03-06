namespace HomeCafeteriaPOS.Client.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int SelectedQuantity { get; set; } = 0;
    }
}
