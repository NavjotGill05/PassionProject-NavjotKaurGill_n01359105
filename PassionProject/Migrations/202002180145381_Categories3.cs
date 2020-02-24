namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categories3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "Brands_BrandId", "dbo.Brands");
            DropIndex("dbo.Categories", new[] { "Brands_BrandId" });
            DropColumn("dbo.Categories", "Brands_BrandId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "Brands_BrandId", c => c.Int());
            CreateIndex("dbo.Categories", "Brands_BrandId");
            AddForeignKey("dbo.Categories", "Brands_BrandId", "dbo.Brands", "BrandId");
        }
    }
}
