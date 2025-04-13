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
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        public Task<int> CompleteAsync();
    }
}
