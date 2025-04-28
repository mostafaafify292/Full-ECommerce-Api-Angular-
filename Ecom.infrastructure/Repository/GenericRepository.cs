using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Ecom.infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public GenericRepository(AppDbContext dbContext , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
            
        public async Task AddAsync(T entity)
        {
           await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int Id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(Id);
            _dbContext.Remove(entity);
            // await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] Includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            foreach (var item in Includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync(string sort)
        {
            var query = _dbContext.Products.Include(m => m.Category)
                                           .Include(m => m.Photos)
                                           .AsNoTracking();
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "PriceAsn":
                        query = query.OrderBy(m => m.NewPrice);
                        break;
                    case "PriceDes":
                        query = query.OrderByDescending(m => m.NewPrice);
                        break;
                    default:
                        query = query.OrderBy(m => m.Name);
                        break;
                }
            }
            var result = _mapper.Map<List<ProductDTO>>(query);
            return (result);
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            return await _dbContext.Set<T>().FindAsync(Id);
        }

        public async Task<T> GetByIdAsync(int Id, params Expression<Func<T, object>>[] Includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            foreach (var item in Includes)
            {
                query = query.Include(item);
            }
            var entity = await query.FirstOrDefaultAsync(x => x.Id == Id);
            return entity;
        }

        public  void UpdateAsync(T entity)
        {
             _dbContext.Update(entity);
        }
    }
}
