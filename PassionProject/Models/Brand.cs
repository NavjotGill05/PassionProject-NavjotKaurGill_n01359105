using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class Brand 
    {
        [Key]
        public int BrandId { get; set; }
        public string BrandName { get; set; }

        //Representing "Many to One" relation (One Brand to many Products)   
        public ICollection<Product> Products { get; set; }
    }
}