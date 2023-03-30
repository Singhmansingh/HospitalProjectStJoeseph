namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scorchedEarth : DbMigration
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
                        appointment_start = c.DateTime(nullable: false),
                        appointment_end = c.DateTime(nullable: false),
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
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        PatientName = c.String(),
                        PatientPhoneNumber = c.String(),
                        PatientPhysicalAddress = c.String(),
                        PatientEmailAddress = c.String(),
                    })
                .PrimaryKey(t => t.PatientId);
            
            CreateTable(
                "dbo.Physicians",
                c => new
                    {
                        physician_id = c.Int(nullable: false, identity: true),
                        first_name = c.String(),
                        last_name = c.String(),
                        specialty = c.String(),
                        phone = c.String(),
                        email = c.String(),
                    })
                .PrimaryKey(t => t.physician_id);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        ServiceTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ServiceId);
            
            CreateTable(
                "dbo.Clinics",
                c => new
                    {
                        ClinicId = c.Int(nullable: false, identity: true),
                        ClinicName = c.String(),
                        ClinicDescription = c.String(),
                        ClinicTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ClinicId);
            
            CreateTable(
                "dbo.BestWishes",
                c => new
                    {
                        BestWishId = c.Int(nullable: false, identity: true),
                        BestWishSender = c.String(),
                        BestWishMessage = c.String(),
                        BestWishIsRead = c.Boolean(nullable: false),
                        BestWishSendDate = c.DateTime(nullable: false),
                        BestWishSenderEmail = c.String(),
                        BestWishSenderPhone = c.String(),
                        PatientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BestWishId)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Requisitions",
                c => new
                    {
                        RequisitionID = c.Int(nullable: false, identity: true),
                        test_result = c.String(),
                        record_date = c.DateTime(nullable: false),
                        PatientId = c.Int(nullable: false),
                        physician_id = c.Int(nullable: false),
                        ClinicId = c.Int(nullable: false),
                        TestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequisitionID)
                .ForeignKey("dbo.Clinics", t => t.ClinicId, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .ForeignKey("dbo.Physicians", t => t.physician_id, cascadeDelete: true)
                .ForeignKey("dbo.Tests", t => t.TestID, cascadeDelete: true)
                .Index(t => t.PatientId)
                .Index(t => t.physician_id)
                .Index(t => t.ClinicId)
                .Index(t => t.TestID);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        TestID = c.Int(nullable: false, identity: true),
                        test_category = c.String(),
                        test_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TestID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Requisitions", "TestID", "dbo.Tests");
            DropForeignKey("dbo.Requisitions", "physician_id", "dbo.Physicians");
            DropForeignKey("dbo.Requisitions", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Requisitions", "ClinicId", "dbo.Clinics");
            DropForeignKey("dbo.BestWishes", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Appointments", "servicesId", "dbo.Services");
            DropForeignKey("dbo.ClinicServices", "Service_ServiceId", "dbo.Services");
            DropForeignKey("dbo.ClinicServices", "Clinic_ClinicId", "dbo.Clinics");
            DropForeignKey("dbo.Appointments", "physicianId", "dbo.Physicians");
            DropForeignKey("dbo.Appointments", "patientId", "dbo.Patients");
            DropIndex("dbo.ClinicServices", new[] { "Service_ServiceId" });
            DropIndex("dbo.ClinicServices", new[] { "Clinic_ClinicId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Requisitions", new[] { "TestID" });
            DropIndex("dbo.Requisitions", new[] { "ClinicId" });
            DropIndex("dbo.Requisitions", new[] { "physician_id" });
            DropIndex("dbo.Requisitions", new[] { "PatientId" });
            DropIndex("dbo.BestWishes", new[] { "PatientId" });
            DropIndex("dbo.Appointments", new[] { "servicesId" });
            DropIndex("dbo.Appointments", new[] { "physicianId" });
            DropIndex("dbo.Appointments", new[] { "patientId" });
            DropTable("dbo.ClinicServices");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Tests");
            DropTable("dbo.Requisitions");
            DropTable("dbo.BestWishes");
            DropTable("dbo.Clinics");
            DropTable("dbo.Services");
            DropTable("dbo.Physicians");
            DropTable("dbo.Patients");
            DropTable("dbo.Appointments");
        }
    }
}
