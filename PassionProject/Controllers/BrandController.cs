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
    public class BrandController : Controller
    {
        //create object of database
        private CosmeticsContext db = new CosmeticsContext();

        // GET: Brand
        public ActionResult Index()
        {
            return View();
        }

          
        //List of Brands
        public ActionResult List(string brandSearchKey)
        {
            //debug line to test whether we get the input from search key
            Debug.WriteLine("Search keyword is " + brandSearchKey);

            //sql query to select all the brands from the database
            string query = "Select * from Brands";

            //if search key store some value 
            //then add that keyword to the sql query
            if (brandSearchKey != "")
            {
                query = query + " where BrandName like '%" + brandSearchKey + "%'";

                //debug line to test the sql query
                Debug.WriteLine("Query is " + query);
            }

            //get list of brands
            List<Brand> brands = db.Brands.SqlQuery(query).ToList();

            return View(brands);
        }

        //ADD: Brand (this method is used to push data from database in the fields)
        //But here we do not need any data from database to add a new Brand
        //That's why there is nothing in the Add() method
        public ActionResult Add()
        {
            return View();
        }

        //ADD: Brand (Pull data from form and store in the database)
        //This block of code execute when we click on submit button to add a new Brand on the URL: /Brand/Add
        [HttpPost]
        public ActionResult Add(string BrandName)
        {
            //STEP 1: Pull data from the arguments of Add Method 

            //Debug line to know whether we are accessing correct data from Add Method 
            Debug.WriteLine("New Brand name is" + BrandName);

            //STEP 2: Write SQL Query to insert data in the database
            string query = "Insert into Brands (BrandName) values (@BrandName)";
            var parameter = new SqlParameter("@BrandName", BrandName);

            //db.Database.ExecuteSqlCommand will execute "Insert" statement
            db.Database.ExecuteSqlCommand(query, parameter);

            //again execute the List mehod to display new list of Brands
            return RedirectToAction("List");
        }

        // GET: Details of individual Brand
        public ActionResult Show(int id)
        {
            //query to fetch a selected Brand
            string query = "Select * from Brands where BrandId = @id";
            var parameter = new SqlParameter("@id", id);
            Brand selectedBrand = db.Brands.SqlQuery(query, parameter).FirstOrDefault();

            return View(selectedBrand);
        }

        //Update: get information of a selected Brand
        public ActionResult Update(int id)
        {
            //query to get information of selected Brand from database
            string query = "Select * from Brands where BrandId = @id";
            var parameter = new SqlParameter("@id", id);
            Brand selectedBrand = db.Brands.SqlQuery(query, parameter).FirstOrDefault();

            //give information of brand to us
            return View(selectedBrand);
        }

        //Update Brand in database
        //This block of code execute when we click on submit button to update a Brand on the URL: /Brand/Update
        [HttpPost]
        public ActionResult Update(int id, string BrandName)
        {
            //query to update brand in database
            string query = "Update Brands set BrandName = @BrandName where BrandId = @id";

            //check the update query
            Debug.WriteLine(query);

            SqlParameter[] sqlparams = new SqlParameter[2];//0,1 pieces of information to add
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@id", id);
            sqlparams[1] = new SqlParameter("@BrandName", BrandName);

            //db.Database.ExecuteSqlCommand will execute "Update" statement
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //this statement will execute the "List" method and provide updated List of Brands
            return RedirectToAction("List");
        }

        //Delete Brand
        //GET Data of selected Brand
        public ActionResult Delete(int id)
        {
            //query to get the selected brand
            string query = "Select * from Brands where BrandId=@id";
            SqlParameter param = new SqlParameter("@id", id);
            Brand selectedBrand = db.Brands.SqlQuery(query, param).FirstOrDefault();

            //display the selected brand
            return View(selectedBrand);
        }

        //Delete: Brand (Delete data from the database)
        //This block of code execute when we click on submit button to delete a Brand on the URL: /Brand/Delete
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            //query to delete brand from database
            string query = "delete from Brands where BrandId=@id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);


            //after deleting a brand
            //unset the brands for those products whose brand got deleted above
            //so that product does not contain that brand which does not exist in the database now
            //this will help in maintaining the consistency and accuracy
            string refQuery = "Update Products set BrandId = '' where BrandId=@id";
            db.Database.ExecuteSqlCommand(refQuery, param); //same param as before

            //redirect to List method to view updated list of brands
            return RedirectToAction("List");
        }
    }
}        