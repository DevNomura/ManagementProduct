namespace ManagementProduct.Models
{
    public class Product
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
