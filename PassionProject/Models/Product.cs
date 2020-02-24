using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        //price is in canadian dollars

        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brands { get; set; }

        public int HasPic { get; set; }
        //can have extension .jpg, .gif, .png, .jpeg
        public string PicExtension { get; set; }

        //Representing "Many to Many" relation(Many Products to Many Categories)
        public ICollection<Category> Categories { get; set; }
    }
}