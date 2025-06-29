namespace Ecom.Core.Entites.order
{
    public class DeliveryMethod : BaseEntity
    {
        public DeliveryMethod()
        {
            
        }
        public DeliveryMethod(string name, string description, decimal price, string deliveryTime)
        {
            Name = name;
            Description = description;
            Price = price;
            DeliveryTime = deliveryTime;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string DeliveryTime { get; set; }
    }
}