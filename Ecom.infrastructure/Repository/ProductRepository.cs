using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repository
{
    public class ProductRepository :  IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
