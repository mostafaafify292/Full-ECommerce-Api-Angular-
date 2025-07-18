﻿using Ecom.Core.DTO;
using Ecom.Core.Entites.order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.ServicesContract
{
    public interface IOrderService
    {
        Task<orders> CreateOrdersAsync(orderDTO orderDTO , string buyerEmail);
        Task<IReadOnlyList<OrderToReturnDTO>> GetOrdersForUserAsync(string buyerEmail);
        Task<OrderToReturnDTO> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();

    }
}
