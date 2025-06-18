using Ecom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        public IAuth Auth { get; }
        public  IProductRepository productRepository { get; }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        public Task<int> CompleteAsync();
    }
}
