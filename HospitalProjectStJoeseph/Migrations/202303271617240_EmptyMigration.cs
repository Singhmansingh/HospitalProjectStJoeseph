namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmptyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        appointmentId = c.Int(nullable: false, identity: true),
                        patientId = c.Int(nullable: false),
                        physicianId = c.Int(nullable: false),
                        appointment_date = c.DateTime(nullable: false),
                        duration = c.Int(nullable: false),
                        servicesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.appointmentId)
                .ForeignKey("dbo.Patients", t => t.patientId, cascadeDelete: true)
                .ForeignKey("dbo.Physicians", t => t.physicianId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.servicesId, cascadeDelete: true)
                .Index(t => t.patientId)
                .Index(t => t.physicianId)
                .Index(t => t.servicesId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "servicesId", "dbo.Services");
            DropForeignKey("dbo.Appointments", "physicianId", "dbo.Physicians");
            DropForeignKey("dbo.Appointments", "patientId", "dbo.Patients");
            DropIndex("dbo.Appointments", new[] { "servicesId" });
            DropIndex("dbo.Appointments", new[] { "physicianId" });
            DropIndex("dbo.Appointments", new[] { "patientId" });
            DropTable("dbo.Appointments");
        }
    }
}
