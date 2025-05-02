namespace Ecommerce.Blazor.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
    }

    public class SearchProductsResponse
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public int TotalCount { get; set; }
    }
}