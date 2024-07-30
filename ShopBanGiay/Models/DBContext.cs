using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ShopBanGiay.Models
{
    public class DBContext:DbContext
    {
        public DBContext() : base("MyConnectionString") { }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> User { get; set; }
    }
}