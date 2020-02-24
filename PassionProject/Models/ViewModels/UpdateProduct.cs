using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class UpdateProduct

    {
        //To update a Product we need a details of that Product as well as list of Brands in the database
        public Product Product { get; set; }
        public List<Brand> Brands { get; set; }
    }
}