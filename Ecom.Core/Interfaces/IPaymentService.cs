﻿using Ecom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId, int? deliveryId);
    }
}
