using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product> , IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IImageMangementService _imageMangement;

        public ProductRepository(AppDbContext dbContext ,IMapper mapper , IImageMangementService imageMangement) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _imageMangement = imageMangement;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null) return false;
            var product = _mapper.Map<Product>(productDTO);
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            var imagePath = await _imageMangement.AddImageAsync(productDTO.Photos, productDTO.Name);
            var photo = imagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = product.Id
            }).ToList();
            await _dbContext.AddRangeAsync(photo);
            await _dbContext.SaveChangesAsync();
            return true;

                
            
        }
    }
}
