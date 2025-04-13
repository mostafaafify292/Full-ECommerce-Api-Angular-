using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites.Product
{
    public class Photo : BaseEntity
    {
        public string ImageName { get; set; }
        public int ProductId { get; set; }
        //[ForeignKey(nameof(ProductId))]
        //public Product Product { get; set; }
    }
}
