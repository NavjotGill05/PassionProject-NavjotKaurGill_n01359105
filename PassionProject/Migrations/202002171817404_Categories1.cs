namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categories1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductPrice", c => c.String());
        }
    }
}
