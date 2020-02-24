namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Products : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "BrandId", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "HasPic", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "PicExtension");
            DropColumn("dbo.Products", "HasPic");
            DropColumn("dbo.Products", "BrandId");
        }
    }
}
