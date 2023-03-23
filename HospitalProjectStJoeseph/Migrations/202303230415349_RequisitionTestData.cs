namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionTestData : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requisitions", "TestID", "dbo.Tests");
            DropIndex("dbo.Requisitions", new[] { "TestID" });
            DropColumn("dbo.Requisitions", "TestID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requisitions", "TestID", c => c.Int(nullable: false));
            CreateIndex("dbo.Requisitions", "TestID");
            AddForeignKey("dbo.Requisitions", "TestID", "dbo.Tests", "TestID", cascadeDelete: true);
        }
    }
}
