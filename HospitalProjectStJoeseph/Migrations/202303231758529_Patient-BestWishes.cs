namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientBestWishes : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Clinics",
                c => new
                    {
                        ClinicId = c.Int(nullable: false, identity: true),
                        ClinicName = c.String(),
                        ClinicDescription = c.String(),
                        ClinicTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ClinicId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        ServiceTime = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ServiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BestWishes", "PatientId", "dbo.Patients");
            DropIndex("dbo.BestWishes", new[] { "PatientId" });
            DropTable("dbo.Services");
            DropTable("dbo.Clinics");
            DropTable("dbo.Patients");
            DropTable("dbo.BestWishes");
        }
    }
}
