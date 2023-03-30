namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalConfigs : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clinics", "ServiceId", "dbo.Services");
            DropIndex("dbo.Clinics", new[] { "ServiceId" });
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
            
            AddColumn("dbo.Appointments", "appointment_start", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointments", "appointment_end", c => c.DateTime(nullable: false));
            AddColumn("dbo.Physicians", "first_name", c => c.String());
            AddColumn("dbo.Physicians", "last_name", c => c.String());
            AlterColumn("dbo.Services", "ServiceTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Clinics", "ClinicTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Appointments", "appointment_date");
            DropColumn("dbo.Appointments", "duration");
            DropColumn("dbo.Physicians", "physician_name");
            DropColumn("dbo.Clinics", "ServiceId");
            DropColumn("dbo.Clinics", "ServiceName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clinics", "ServiceName", c => c.String());
            AddColumn("dbo.Clinics", "ServiceId", c => c.Int(nullable: false));
            AddColumn("dbo.Physicians", "physician_name", c => c.String());
            AddColumn("dbo.Appointments", "duration", c => c.Int(nullable: false));
            AddColumn("dbo.Appointments", "appointment_date", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.ClinicServices", "Service_ServiceId", "dbo.Services");
            DropForeignKey("dbo.ClinicServices", "Clinic_ClinicId", "dbo.Clinics");
            DropIndex("dbo.ClinicServices", new[] { "Service_ServiceId" });
            DropIndex("dbo.ClinicServices", new[] { "Clinic_ClinicId" });
            AlterColumn("dbo.Clinics", "ClinicTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Services", "ServiceTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Physicians", "last_name");
            DropColumn("dbo.Physicians", "first_name");
            DropColumn("dbo.Appointments", "appointment_end");
            DropColumn("dbo.Appointments", "appointment_start");
            DropTable("dbo.ClinicServices");
            CreateIndex("dbo.Clinics", "ServiceId");
            AddForeignKey("dbo.Clinics", "ServiceId", "dbo.Services", "ServiceId", cascadeDelete: true);
        }
    }
}
