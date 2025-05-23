﻿using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;

namespace Ecom.API.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<UpdateCategoryDTO, Category>().ReverseMap();

            //Product 
            CreateMap<Product, ProductDTO>().ForMember(p => p.CategoryName, D => D.MapFrom(d => d.Category.Name))                                     
                                            .ForMember(p=>p.photos , D=>D.MapFrom(d=>d.Photos))
                                            .ReverseMap();
            CreateMap<AddProductDTO, Product>()
             .ForMember(p => p.Photos, d => d.Ignore());
            CreateMap<UpdateProductDTO, Product>()
             .ForMember(p => p.Photos, d => d.Ignore());

            //photos
            CreateMap<Photo, PhotoDTO>().ReverseMap();
    

        }
    }
}
