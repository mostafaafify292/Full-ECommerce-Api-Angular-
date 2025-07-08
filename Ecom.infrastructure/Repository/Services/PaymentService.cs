using Ecom.Core.Entites;
using Ecom.Core.Entites.order;
using Ecom.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repository.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, ICustomerBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId , int? deliveryId)
        {
            //Secret Key
            StripeConfiguration.ApiKey = _configuration["StripeSetting:secretKey"];
            //Get Basket
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            //Get DeliveryMethod
            var shipingPrice = 0M;   //Decimal
            if (deliveryId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryId.Value);
                shipingPrice = DeliveryMethod.Price;
            }

            //Total = supTotal + Delivery.cost
            if (basket.basketItems.Count > 0)
            {
                foreach (var item in basket.basketItems)
                {
                    var product = await _unitOfWork.productRepository.GetByIdAsync(item.Id);
                    if (item.Price != product.NewPrice)
                        item.Price = product.NewPrice;
                }
            }
            var subTotal = basket.basketItems.Sum(item => item.Price * item.Quantity);

            //Create Payment Intent

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) //Create
            {
                var optionsForCreate = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotal * 100) + (long)(shipingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await service.CreateAsync(optionsForCreate);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else  // Update
            {
                var optionsForUpdate = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100) + (long)(shipingPrice * 100)

                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, optionsForUpdate);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            //update basket on Database
            await _basketRepository.UpdateBasketAsync(basket);
            return (basket);
        }
    }
    
}
