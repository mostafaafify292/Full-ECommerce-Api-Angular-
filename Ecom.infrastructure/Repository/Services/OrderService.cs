using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.order;
using Ecom.Core.Interfaces;
using Ecom.Core.ServicesContract;
using Ecom.infrastructure.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repository.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOf;
        private readonly IMapper _mapper;
        private readonly ICustomerBasketRepository _basketRepository;

        public OrderService(IUnitOfWork unitOf ,IMapper mapper , ICustomerBasketRepository basketRepository)
        {
            _unitOf = unitOf;
            _mapper = mapper;
            _basketRepository = basketRepository;
        }
        public async Task<orders> CreateOrdersAsync(orderDTO orderDTO, string buyerEmail)
        {
            //1. Get Basket From basket repo
            var basket = await _basketRepository.GetBasketAsync(orderDTO.basketId);

            //2. Get Selected Items at Basket from product repo and add it to orderitems 
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.basketItems)
            {
                var product = await _unitOf.productRepository.GetByIdAsync(item.Id);
                var orderItem = new OrderItem(product.Id, item.ImageURL , product.Name, product.NewPrice, item.Quantity);
                orderItems.Add(orderItem);
            }

            //3. Get DeliveryMethod from DeliveryMethod repo
            var deliveryMethod = await _unitOf.Repository<DeliveryMethod>().GetByIdAsync(orderDTO.deliveryMethodID);

            //4 Calculate Suptotal
            var supTotal = orderItems.Sum(m => m.Price * m.Quntity);

            //Additional if any order has the same PaymentIntentId

            //5. Create Order
            var shipAddress = _mapper.Map<ShippingAddress>(orderDTO.ShipAddress);
            var order = new orders(buyerEmail, supTotal, shipAddress , deliveryMethod, orderItems);
            await _unitOf.Repository<orders>().AddAsync(order);

            //6. Save To DataBase
            var result = await _unitOf.CompleteAsync();
            if (result <= 0) return null;
            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<orders> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<orders>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
