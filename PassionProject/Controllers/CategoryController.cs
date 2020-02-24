using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Net;
using PassionProject.Data;
using PassionProject.Models;
using System.Diagnostics;

namespace PassionProject.Controllers
{
    public class CategoryController : Controller
    {
        //create object of database
        private CosmeticsContext db = new CosmeticsContext();

        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        //List of Category
        public ActionResult List(string categorySearchKey)
        {
            //debug line to test whether we get the input from search key
            Debug.WriteLine("Search keyword is " + categorySearchKey);

            //sql query to select all the Categories from the database
            string query = "Select * from Categories";

            //if search key store some value 
            //then add that keyword to the sql query
            if (categorySearchKey != "")
            {
                query = query + " where CategoryId like '%" + categorySearchKey + "%'";

                //debug line to test the sql query
                Debug.WriteLine("Query is " + query);
            }

            //get list of Categories
            List<Category> categories = db.Categories.SqlQuery(query).ToList();

            return View(categories);
        }

        //ADD: Category (this method is used to push data from database in the fields)
        //But here we do not need any data from database to add a new Brand
        //That's why there is nothing in the Add() method
        public ActionResult Add()
        {
            return View();
        }

        //ADD: Category (Pull data from form and store in the database)
        //This block of code execute when we click on submit button to add a new Category on the URL: /Category/Add
        [HttpPost]
        public ActionResult Add(string CategoryName)
        {
            //STEP 1: Pull data from the arguments of Add Method 

            //Debug line to know whether we are accessing correct data from Add Method 
            Debug.WriteLine("New Category name is" + CategoryName);

            //STEP 2: Write SQL Query to insert data in the database
            string query = "Insert into Categories (CategoryName) values (@CategoryName)";
            var parameter = new SqlParameter("@CategoryName", CategoryName);

            //db.Database.ExecuteSqlCommand will execute "Insert" statement
            db.Database.ExecuteSqlCommand(query, parameter);

            //again execute the List mehod to display new list of Categories
            return RedirectToAction("List");
        }

        // GET: Details of individual Category
        public ActionResult Show(int id)
        {
            //query to fetch a selected Category
            string query = "Select * from Categories where CategoryId = @id";
            var parameter = new SqlParameter("@id", id);
            Category selectedCategory = db.Categories.SqlQuery(query, parameter).FirstOrDefault();

            return View(selectedCategory);
        }

        //Update: get information of a selected Category
        public ActionResult Update(int id)
        {
            //query to get information of selected Category from database
            string query = "Select * from Categories where CategoryId = @id";
            var parameter = new SqlParameter("@id", id);
            Category selectedCategory = db.Categories.SqlQuery(query, parameter).FirstOrDefault();

            //give information of Categoty to us
            return View(selectedCategory);
        }

        //Update Category in database
        //This block of code execute when we click on submit button to update a Category on the URL: /Category/Update
        [HttpPost]
        public ActionResult Update(int id, string CategoryName)
        {
            //query to update Category in database
            string query = "Update Categories set CategoryName = @CategoryName where CategoryId = @id";

            //check the update query
            Debug.WriteLine(query);

            SqlParameter[] sqlparams = new SqlParameter[2];//0,1 pieces of information to add
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@id", id);
            sqlparams[1] = new SqlParameter("@CategoryName", CategoryName);

            //db.Database.ExecuteSqlCommand will execute "Update" statement
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //this statement will execute the "List" method and provide updated List of Categories
            return RedirectToAction("List");
        }

        //Delete Category
        //GET Data of selected Category
        public ActionResult Delete(int id)
        {
            //query to get the selected Category
            string query = "Select * from Categories where CategoryId=@id";
            SqlParameter param = new SqlParameter("@id", id);
            Category selectedCategory = db.Categories.SqlQuery(query, param).FirstOrDefault();

            //display the selected Category
            return View(selectedCategory);
        }

        //Delete: Category (Delete data from the database)
        //This block of code execute when we click on submit button to delete a Category on the URL: /Category/Delete
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            //query to delete Category from database
            string query = "delete from Categories where CategoryId=@id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);


            //after deleting a Category
            //unset the Categories for those products whose brand got deleted above
            //so that product does not contain that brand which does not exist in the database now
            //this will help in maintaining the consistency and accuracy
           // string refQuery = "Update Products set BrandId = '' where BrandId=@id";
           // db.Database.ExecuteSqlCommand(refQuery, param); //same param as before

            //redirect to List method to view updated list of Category
            return RedirectToAction("List");
        }
    }
}