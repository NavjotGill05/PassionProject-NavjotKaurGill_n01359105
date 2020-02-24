using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PassionProject.Data;
using PassionProject.Models;
using PassionProject.Models.ViewModels;
using System.Diagnostics;
using System.IO;

namespace PassionProject.Controllers
{
    public class ProductController : Controller
    {
        //create object of database
        private CosmeticsContext db = new CosmeticsContext();

        // GET: Product
        public ActionResult List(string productSearchKey)
        {
            //debug line to test whether we get the input from search key
            Debug.WriteLine("Search keyword is " + productSearchKey);

            //sql query to select all the products from the database
            string query = "Select * from Products";

            //if search key store some value 
            //then add that keyword to the sql query
            if (productSearchKey != "") 
            {
                query = query + " where ProductName like '%" + productSearchKey + "%'";

                //debug line to print the sql query
                Debug.WriteLine("Query is " + query);
            }
            List<Product> products = db.Products.SqlQuery(query).ToList();
            return View(products);
        }

        //ADD: Product (Pull data from form and store in the database)
        //This block of code execute when we click on submit button to add a new product on the URL: /Product/Add
        [HttpPost]
        public ActionResult Add(string ProductName, string ProductDescription, int ProductPrice, int BrandId)
        {
            //STEP 1: Pull data from the arguments of Add Method 

            //Debug line to know whether we are accessing correct data from Add Method 
            Debug.WriteLine("New product name is" + ProductName) ;

            //STEP 2: Write SQL Query to insert data in the database
            string query = "insert into Products (ProductName, ProductDescription, ProductPrice, BrandId) values (@ProductName,@ProductDescription,@ProductPrice,@BrandId)";
            SqlParameter[] sqlparams = new SqlParameter[4]; //0,1,2,3 pieces of information to add
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@ProductName", ProductName);
            sqlparams[1] = new SqlParameter("@ProductDescription", ProductDescription);
            sqlparams[2] = new SqlParameter("@ProductPrice", ProductPrice);
            sqlparams[3] = new SqlParameter("@BrandId", BrandId);

            //db.Database.ExecuteSqlCommand will execute "Insert" statement
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //this statement will execute the "List" method and provide us a List of Products
            //The new product will also be displayed in the list
            return RedirectToAction("List");
        }

        //ADD: Product (this method is used to push data from database in the fields)
        public ActionResult Add()
        {
            //STEP 1: Push Data
            //get a list of brands for productC:\Users\navjot\source\repos\PassionProject\PassionProject\Models\ViewModels\ShowCategory.cs

            List<Brand> brands = db.Brands.SqlQuery("Select * from Brands").ToList();

            return View(brands);
        }

        // GET: Details of individual Product
        public ActionResult Show(int? id)
        {
            //if id is equal to NULL then, return BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Pet pet = db.Pets.Find(id); //EF 6 technique
            Product product = db.Products.SqlQuery("select * from Products where ProductId=@ProductId", new SqlParameter("@ProductId", id)).FirstOrDefault();
           
            //if there is no product then, return HttpNotFound
            if (product == null)
            {
                return HttpNotFound();
            }

            //get information about the list of categories associated with that product
            string query = "select * from Categories inner join CategoryProducts on Categories.CategoryId = CategoryProducts.Category_CategoryId where Product_ProductId = @id";
            SqlParameter param = new SqlParameter("@id", id);
            List<Category> CategoryProducts = db.Categories.SqlQuery(query, param).ToList();

            List<Category> all_catgories = db.Categories.SqlQuery("select * from Categories").ToList();


            ShowProduct viewmodel = new ShowProduct();
            viewmodel.product = product;
            viewmodel.categories = CategoryProducts;
            viewmodel.all_categories = all_catgories;

            return View(viewmodel);
        }

        //GET: details of a particular product
        public ActionResult Update(int id)
        {
           
            Product selectedProduct = db.Products.SqlQuery("Select * from Products where ProductId = @id", new SqlParameter("@id", id)).FirstOrDefault();

            //fetch all the brands from Brands table
            List<Brand> brands = db.Brands.SqlQuery("Select * from Brands").ToList();

            //create an object of UpdateProduct ViewModel
            UpdateProduct UpdateProductViewModel = new UpdateProduct();

            //use the properties of UpdateProduct ViewModel to populate the fields 
            UpdateProductViewModel.Product = selectedProduct;
            UpdateProductViewModel.Brands = brands;

            return View(UpdateProductViewModel);
        }

        //Update Product in database
        //This block of code execute when we click on submit button to update a product on the URL: /Product/Update
        [HttpPost]
        public ActionResult Update(int id, string ProductName, string ProductDescription, int ProductPrice, int BrandId, HttpPostedFileBase ProductPic)
        {
            //assume that Product have no picture
            //let the extension of the file be empty

            int hasPic = 0;
            string productPicExtension = "";
            //checking to see if some information is there
            if (ProductPic != null)
            {
                //Debug line to check that ProductPic have some value
                Debug.WriteLine("There is a product pic.");

                //ccondition to check if the file size is more than 0 (bytes)
                if (ProductPic.ContentLength > 0)
                {
                    //Debug line to check if file size is greater than 0
                    Debug.WriteLine("Image identified");

                    //file extensioncheck taken from https://www.c-sharpcorner.com/article/file-upload-extension-validation-in-asp-net-mvc-and-javascript/
                    var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                    //get the extension of the pic
                    var extension = Path.GetExtension(ProductPic.FileName).Substring(1);

                    //check if valtypes aaray contain extension from the given values
                    if (valtypes.Contains(extension))
                    {
                        try
                        {
                            //file name is the id of the image
                            string fileName = id + "." + extension;

                            //get a direct file path to ~/Content/Products/{id}.{extension}
                            string path = Path.Combine(Server.MapPath("~/Content/Products/"), fileName);

                            //save file
                            ProductPic.SaveAs(path);
                            //if the file saves successfully then in the database hasPic willl contain 1 and productPicExtension will contain the extension
                            hasPic = 1;
                            productPicExtension = extension;

                        }
                        catch (Exception exception)
                        {
                            //if there is some problem in saving the file 
                            //code will enter in this block

                            //debug line to check the exception occured
                            Debug.WriteLine("Exception:" + exception);
                        }
                    }
                }
            }
            //query to update the Product
            string query = "Update Products set ProductName=@ProductName, ProductDescription=@ProductDescription, ProductPrice=@ProductPrice, BrandId=@BrandId, HasPic=@hasPic, PicExtension=@productPicExtension where ProductId=@id";
            SqlParameter[] sqlparams = new SqlParameter[7];//0,1,2,3,4,5,6 pieces of information to add
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@ProductName", ProductName);
            sqlparams[1] = new SqlParameter("@ProductDescription", ProductDescription);
            sqlparams[2] = new SqlParameter("@ProductPrice", ProductPrice);
            sqlparams[3] = new SqlParameter("@BrandId", BrandId);
            sqlparams[4] = new SqlParameter("@id", id);
            sqlparams[5] = new SqlParameter("@HasPic", hasPic);
            sqlparams[6] = new SqlParameter("@productPicExtension", productPicExtension);

            //db.Database.ExecuteSqlCommand will execute "Update" statement
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //this statement will execute the "List" method and provide us a List of Products
            return RedirectToAction("List");
        }

        //Delete Product
        //GET Data of selected Product
        public ActionResult Delete(int id)
        {
            //query to get the selected product
            string query = "Select * from Products where ProductId = @id";
            SqlParameter param = new SqlParameter("@id", id);
            Product selectedProduct = db.Products.SqlQuery(query, param).FirstOrDefault();

            //display the selected Product
            return View(selectedProduct);
        }

        //Delete: Product (Delete data from the database)
        //This block of code execute when we click on submit button to delete a product on the URL: /Product/Delete
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            //query to delete the Product
            string query = "Delete from Products where ProductId = @id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);

            //redirect to the List Method
            return RedirectToAction("List");
        }


        //we can add the ProductId and CategoryId into the bridging table
        //first we fetch the data from categories and bridging table
        //then we can add categories to products that product does not have previously
        [HttpPost]
        public ActionResult AttachCategory(int id, int CategoryId)
        {
            //debug line to check the id 
            Debug.WriteLine("ProductId is" + id + " and CategoryId is " + CategoryId);

            //fetch the data from categories and bridging table then execute the if statement on that to add new category that previously not added
            //first, check if that pet is already owned by that owner
            string check_query = "select * from Categories inner join CategoryProducts on CategoryProducts.Category_CategoryId = Categories.CategoryId where Product_ProductId=@id and Category_CategoryId=@CategoryId";
            SqlParameter[] check_params = new SqlParameter[2];
            check_params[0] = new SqlParameter("@id", id);
            check_params[1] = new SqlParameter("@CategoryId", CategoryId);
            List<Category> categories = db.Categories.SqlQuery(check_query, check_params).ToList();

            //if product does not have the category then we can add category to the product
            if (categories.Count <= 0) 
            {


                //insert the ids in bridging table
                string query = "insert into CategoryProducts (Product_ProductId, Category_CategoryId) values (@id, @CategoryId)";
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@id", id);
                sqlparams[1] = new SqlParameter("@CategoryId", CategoryId);


                db.Database.ExecuteSqlCommand(query, sqlparams);
            }

            //execute the show method with same id
            return RedirectToAction("Show/" + id);

        }


        //delete the ids from bridging table
        //pass productid and categoryid to compare with the bridging table's ids

       [HttpGet]
        public ActionResult DetachCategory(int id, int CategoryId)
        {
            //checkwhether we get the correct ids
            Debug.WriteLine("productid is" + id + " and categoryid is " + CategoryId);

            //query to delete from bridging table
            string query = "delete from CategoryProducts where Category_CategoryId=@CategoryId and Product_ProductId=@id";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@CategoryId", CategoryId);
            sqlparams[1] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            //execute show method by passing same product id
            return RedirectToAction("Show/" + id);
        }
    }
}