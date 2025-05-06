using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Sharing
{
    public class ProductParam
    {
        //string? sort , int? categoryId , int PageSize, int pageNumber
        public string? sort { get; set; }
        public int? categoryId { get; set; }
        private int MaxPageSize { get; set; } = 6;
        private int _pageSize = 3;
        public int PageSize 
        {
            get { return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }  // 7 => t + f = t   --- 5 => f + f = f  -- 0 => f + t = t 
        }
        public int pageNumber { get; set; } = 1;

    }
}
