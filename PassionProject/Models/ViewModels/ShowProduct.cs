using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class ShowProduct
    {
        //details of an individual Product
        public virtual Product product { get; set; }

        //details of multiple Categories
        public List<Category> categories { get; set; }

        //display a separate list to ADD a Category to the Product 
        //display a dropdown list of all Catgories with a button "Add Category"
        public List<Category> all_categories { get; set; }
    }
}