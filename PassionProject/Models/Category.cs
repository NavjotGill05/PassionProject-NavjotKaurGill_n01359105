using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } 
        public string CategoryName { get; set; }

        //public virtual Brand Brands { get; set; } 

        //Representing "Many to Many" relation (Many Categories to Many Products)
        public ICollection<Product> Products { get; set; }
    }
}