using Ecom.Core.DTO;
using Ecom.Core.Entites;
using Ecom.Core.Entites.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<bool> AddAsync(AddProductDTO productDTO);
    }
}
