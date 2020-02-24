using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PassionProject.Data
{
    public class CosmeticsContext : DbContext
    {
        public CosmeticsContext() : base("name=CosmeticsContext")
        {
        }

        public System.Data.Entity.DbSet<PassionProject.Models.Brand> Brands { get; set; }
        public System.Data.Entity.DbSet<PassionProject.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<PassionProject.Models.Product> Products { get; set; }
        
    }
}