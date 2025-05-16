using Ecom.Core.DTO;
using Ecom.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] Includes );
        Task<T> GetByIdAsync(int Id);
        Task<T> GetByIdAsync(int Id , params Expression<Func<T, object>>[] Includes);
        Task AddAsync(T entity);
        void UpdateAsync(T entity);
        Task DeleteAsync(int Id);
        Task<int> CountAsync();

    }
}
