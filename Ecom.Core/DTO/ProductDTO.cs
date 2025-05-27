using Ecom.Core.Entites.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTO
{
    public record ProductDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }

        //photo
        public List<PhotoDTO> photos { get; set; }

        //Category
        public string CategoryName { get; set; }
       // public string CategoryDescription { get; set; }

    }

    // i do that to can calculate count on pagination with filter
    public record ReturnProductDTO
    {
        public List<ProductDTO> products { get; set; }
        public int TotalCount { get; set; }
    }

    public record PhotoDTO
    {
        public string ImageName { get; set; }
        public int ProductId { get; set; }
    }

    public record AddProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public int CategoryId { get; set; }
        public IFormFileCollection Photos { get; set; }
    }
    public record UpdateProductDTO : AddProductDTO
    {
        public int Id { get; set; }
    }
}
