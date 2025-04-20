using AutoMapper;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly IImageMangementService _imageMangement;
        private Hashtable _repository;
        public IProductRepository ProductRepository { get; }

        public UnitOfWork(AppDbContext dbcontext , IMapper mapper , IImageMangementService imageMangement)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _imageMangement = imageMangement;

            _repository = new Hashtable();
             ProductRepository = new ProductRepository(_dbcontext ,_mapper ,_imageMangement);
        } 

        public async Task<int> CompleteAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbcontext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;
            if (!_repository.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_dbcontext);
                _repository.Add(key, repository);
            }
            return _repository[key] as IGenericRepository<TEntity>;
        }
    }
}
