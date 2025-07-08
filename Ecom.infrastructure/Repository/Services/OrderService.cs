using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.order;
using Ecom.Core.Interfaces;
using Ecom.Core.ServicesContract;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _dbContext;
        private readonly ICustomerBasketRepository _basketRepository;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOf ,IMapper mapper ,AppDbContext dbContext, ICustomerBasketRepository basketRepository , IPaymentService paymentService)
        {
            _unitOf = unitOf;
            _mapper = mapper;
            _dbContext = dbContext;
            _basketRepository = basketRepository;
            _paymentService = paymentService;
        }
        public async Task<orders> CreateOrdersAsync(orderDTO orderDTO, string buyerEmail)
        {
            //1. Get Basket From basket repo
            var basket = await _basketRepository.GetBasketAsync(orderDTO.basketId);
            if (basket is null)
            {
                return null;
            }
            //2. Get Selected Items at Basket from product repo and add it to orderitems 
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.basketItems)
            {
                var product = await _unitOf.productRepository.GetByIdAsync( item.Id);
                var orderItem = new OrderItem(product.Id, item.ImageURL , product.Name, product.NewPrice, item.Quantity,product.Description);
                orderItems.Add(orderItem);
            }

            //3. Get DeliveryMethod from DeliveryMethod repo
             var deliveryMethod = await _unitOf.Repository<DeliveryMethod>().GetByIdAsync(orderDTO.deliveryMethodID);

            //4 Calculate Suptotal
            var supTotal = orderItems.Sum(m => m.Price * m.Quntity);

            //Additional if any order has the same PaymentIntentId

            var ExistOrder = await _dbContext.Orders
                .Where(m=>m.PaymentIntentId == basket.PaymentIntentId)
                .FirstOrDefaultAsync();
            if (ExistOrder != null)
            {
                _dbContext.Orders.Remove(ExistOrder);
                await _paymentService.CreateOrUpdatePaymentAsync(basket.Id, deliveryMethod.Id);
            }

                //5. Create Order
                var shipAddress = _mapper.Map<ShippingAddress>(orderDTO.ShipAddress);
            var order = new orders(buyerEmail, supTotal, shipAddress , deliveryMethod, orderItems,basket.PaymentIntentId);
            await _unitOf.Repository<orders>().AddAsync(order);

            //6. Save To DataBase
            var result = await _unitOf.CompleteAsync();
            if (result <= 0) return null;
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOf.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var order = await _dbContext.Orders.Where(m => m.Id == id && m.BuyerEmail == buyerEmail)
                                                .Include(o=>o.OrderItems)
                                                .Include(o=>o.deliveryMethod)
                                                .FirstOrDefaultAsync();
            var result = _mapper.Map<OrderToReturnDTO>(order);
            return result;
        }

        public async Task<IReadOnlyList<OrderToReturnDTO>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orders = await _dbContext.Orders.Where(m => m.BuyerEmail == buyerEmail)
                                                .Include(o => o.OrderItems)
                                                .Include(o => o.deliveryMethod)
                                                .ToListAsync();
            var result = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);
            return result;
        }
    }
}
