namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clinicxservice : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clinics", "ServiceId", "dbo.Services");
            DropIndex("dbo.Clinics", new[] { "ServiceId" });
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
            
            CreateTable(
                "dbo.ClinicServices",
                c => new
                    {
                        Clinic_ClinicId = c.Int(nullable: false),
                        Service_ServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Clinic_ClinicId, t.Service_ServiceId })
                .ForeignKey("dbo.Clinics", t => t.Clinic_ClinicId, cascadeDelete: true)
                .ForeignKey("dbo.Services", t => t.Service_ServiceId, cascadeDelete: true)
                .Index(t => t.Clinic_ClinicId)
                .Index(t => t.Service_ServiceId);
            
            DropColumn("dbo.Clinics", "ServiceId");
            DropColumn("dbo.Clinics", "ServiceName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clinics", "ServiceName", c => c.String());
            AddColumn("dbo.Clinics", "ServiceId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Appointments", "servicesId", "dbo.Services");
            DropForeignKey("dbo.ClinicServices", "Service_ServiceId", "dbo.Services");
            DropForeignKey("dbo.ClinicServices", "Clinic_ClinicId", "dbo.Clinics");
            DropForeignKey("dbo.Appointments", "physicianId", "dbo.Physicians");
            DropForeignKey("dbo.Appointments", "patientId", "dbo.Patients");
            DropIndex("dbo.ClinicServices", new[] { "Service_ServiceId" });
            DropIndex("dbo.ClinicServices", new[] { "Clinic_ClinicId" });
            DropIndex("dbo.Appointments", new[] { "servicesId" });
            DropIndex("dbo.Appointments", new[] { "physicianId" });
            DropIndex("dbo.Appointments", new[] { "patientId" });
            DropTable("dbo.ClinicServices");
            DropTable("dbo.Appointments");
            CreateIndex("dbo.Clinics", "ServiceId");
            AddForeignKey("dbo.Clinics", "ServiceId", "dbo.Services", "ServiceId", cascadeDelete: true);
        }
    }
}
