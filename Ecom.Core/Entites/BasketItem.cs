namespace Ecom.Core.Entites
{
    public class BasketItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}