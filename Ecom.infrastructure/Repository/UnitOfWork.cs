using AutoMapper;
using Ecom.Core.Entites;
using Ecom.Core.Entites.Identity;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.ServicesContract;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
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
        private readonly IProductRepository _productRepository;
        private readonly IAuth _auth;
        //public  IProductRepository productRepository { get; set; }
        public IAuth Auth => _auth;        
        public IProductRepository productRepository  => _productRepository ;

        private Hashtable _repository;

        public UnitOfWork(AppDbContext dbcontext,
                           IMapper mapper,
                           IProductRepository productRepository,
                           IAuth auth
                           )
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
            _productRepository = productRepository;
            _auth = auth;
            _repository = new Hashtable();
            //productRepository = new ProductRepository(_dbcontext, _mapper, _imageMangement);
            //Auth = new AuthRepository(_userManager, _signInManager, _emailService , _Token);
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
                var repository = new GenericRepository<TEntity>(_dbcontext , _mapper);
                _repository.Add(key, repository);
            }
            return _repository[key] as IGenericRepository<TEntity>;
        }
    }
}
