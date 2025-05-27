using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public ProductRepository(AppDbContext dbContext ,IMapper mapper , IImageMangementService imageMangement) : base(dbContext , mapper)
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

        public async Task DeleteAsync(Product product)
        {
            var photos =await _dbContext.Photos.Where(m => m.ProductId == product.Id).ToListAsync();
            foreach (var item in photos)
            {
                _imageMangement.RemoveImageAsync(item.ImageName);
            }
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();


        }
        public async Task<ReturnProductDTO> GetAllAsync(ProductParam productParam)
        {
            var query = _dbContext.Products.Include(m => m.Category)
                                           .Include(m => m.Photos)
                                           .AsNoTracking();
            //Searching By Name of product 

            if (!string.IsNullOrEmpty(productParam.Search))
            {
                var stringToSearch = productParam.Search.ToLower().Split(' ');
                query = query.Where(p => stringToSearch.All(word =>
                p.Name.ToLower().Contains(word)
                ||
                p.Description.ToLower().Contains(word)
                ));
            }
            //filtering By Catrgory Id 
            if (productParam.categoryId.HasValue)
            {
                query = query.Where(m => m.CategoryId == productParam.categoryId);   
            }
            //Sort By Price
            if (!string.IsNullOrEmpty(productParam.sort))
            {
                query = productParam.sort switch
                {
                    "PriceAsn" => query.OrderBy(m => m.NewPrice),
                    "PriceDes" => query.OrderByDescending(m => m.NewPrice),
                    _ => query.OrderBy(m => m.Name),
                };
            }
            ReturnProductDTO returnProductDTO = new ReturnProductDTO();
            returnProductDTO.TotalCount = query.Count();

            //pagination

            query = query.Skip((productParam.PageSize) * (productParam.pageNumber - 1)).Take(productParam.PageSize);
            returnProductDTO.products = _mapper.Map<List<ProductDTO>>(query);
            return (returnProductDTO);
        }

        public async Task<bool> UpdateAsync(UpdateProductDTO productDTO)
        {
            if (productDTO == null) return false;
            var Product =await _dbContext.Products.Include(m => m.Category).Include(m => m.Photos)
                                                      .FirstOrDefaultAsync(p => p.Id == productDTO.Id);
            if (Product is null)
            {
                return false;
            }
            _mapper.Map(productDTO , Product);
            var findPhoto = await _dbContext.Photos.Where(ph => ph.ProductId == productDTO.Id).ToListAsync();
            foreach (var item in findPhoto)
            {
                _imageMangement.RemoveImageAsync(item.ImageName);
            }
            _dbContext.Photos.RemoveRange(findPhoto);
            var ImagePath = await _imageMangement.AddImageAsync(productDTO.Photos, productDTO.Name);
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = productDTO.Id
            }).ToList();
            await _dbContext.AddRangeAsync(photo);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
