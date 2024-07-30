using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShopBanGiay.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}