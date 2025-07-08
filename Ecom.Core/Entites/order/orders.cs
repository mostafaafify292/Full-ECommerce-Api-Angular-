using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites.order
{
    public class orders : BaseEntity
    {
        public orders() { }
        
        public orders(string buyerEmail, 
                      decimal supTotal,
                      ShippingAddress shippingAddress, 
                      DeliveryMethod deliveryMethod, 
                      IReadOnlyList<OrderItem> orderItems,
                      string PaymentIntentId
                      )
        {
            BuyerEmail = buyerEmail;
            SupTotal = supTotal;
            this.shippingAddress = shippingAddress;
            this.deliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            this.PaymentIntentId = PaymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public decimal SupTotal { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public ShippingAddress shippingAddress { get; set; }
        public string PaymentIntentId { get; set; }
        public DeliveryMethod deliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Status status { get; set; } = Status.Pending;

        public decimal GetTotal()
        {
            return SupTotal + deliveryMethod.Price;
        }

    }
}
