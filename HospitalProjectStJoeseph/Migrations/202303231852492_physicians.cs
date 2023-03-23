namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class physicians : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Physicians",
                c => new
                    {
                        physician_id = c.Int(nullable: false, identity: true),
                        physician_name = c.String(),
                        specialty = c.String(),
                        phone = c.String(),
                        email = c.String(),
                    })
                .PrimaryKey(t => t.physician_id);
            
            AddColumn("dbo.Requisitions", "PatientId", c => c.Int(nullable: false));
            AddColumn("dbo.Requisitions", "PhysicianId", c => c.Int(nullable: false));
            AddColumn("dbo.Requisitions", "ClinicId", c => c.Int(nullable: false));
            AddColumn("dbo.Requisitions", "TestID", c => c.Int(nullable: false));
            CreateIndex("dbo.Requisitions", "PatientId");
            CreateIndex("dbo.Requisitions", "PhysicianId");
            CreateIndex("dbo.Requisitions", "ClinicId");
            CreateIndex("dbo.Requisitions", "TestID");
            AddForeignKey("dbo.Requisitions", "ClinicId", "dbo.Clinics", "ClinicId", cascadeDelete: true);
            AddForeignKey("dbo.Requisitions", "PatientId", "dbo.Patients", "PatientId", cascadeDelete: true);
            AddForeignKey("dbo.Requisitions", "PhysicianId", "dbo.Physicians", "physician_id", cascadeDelete: true);
            AddForeignKey("dbo.Requisitions", "TestID", "dbo.Tests", "TestID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requisitions", "TestID", "dbo.Tests");
            DropForeignKey("dbo.Requisitions", "PhysicianId", "dbo.Physicians");
            DropForeignKey("dbo.Requisitions", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Requisitions", "ClinicId", "dbo.Clinics");
            DropIndex("dbo.Requisitions", new[] { "TestID" });
            DropIndex("dbo.Requisitions", new[] { "ClinicId" });
            DropIndex("dbo.Requisitions", new[] { "PhysicianId" });
            DropIndex("dbo.Requisitions", new[] { "PatientId" });
            DropColumn("dbo.Requisitions", "TestID");
            DropColumn("dbo.Requisitions", "ClinicId");
            DropColumn("dbo.Requisitions", "PhysicianId");
            DropColumn("dbo.Requisitions", "PatientId");
            DropTable("dbo.Physicians");
        }
    }
}
