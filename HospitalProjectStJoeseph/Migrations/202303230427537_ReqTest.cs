namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReqTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requisitions",
                c => new
                    {
                        RequisitionID = c.Int(nullable: false, identity: true),
                        patient_id = c.Int(nullable: false),
                        physician_id = c.Int(nullable: false),
                        clinic_id = c.Int(nullable: false),
                        test_id = c.Int(nullable: false),
                        record_date = c.DateTime(nullable: false),
                        test_result = c.String(),
                    })
                .PrimaryKey(t => t.RequisitionID);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        TestID = c.Int(nullable: false, identity: true),
                        test_category = c.String(),
                        test_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TestID);
            
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tests");
            DropTable("dbo.Requisitions");
        }
    }
}
