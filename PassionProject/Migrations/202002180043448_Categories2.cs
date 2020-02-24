namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categories2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Brands_BrandId", c => c.Int());
            CreateIndex("dbo.Categories", "Brands_BrandId");
            AddForeignKey("dbo.Categories", "Brands_BrandId", "dbo.Brands", "BrandId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "Brands_BrandId", "dbo.Brands");
            DropIndex("dbo.Categories", new[] { "Brands_BrandId" });
            DropColumn("dbo.Categories", "Brands_BrandId");
        }
    }
}
