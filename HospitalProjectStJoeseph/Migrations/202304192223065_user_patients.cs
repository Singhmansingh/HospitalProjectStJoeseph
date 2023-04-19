namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_patients : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserPatients",
                c => new
                    {
                        UserPatientId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        PatientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserPatientId)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPatients", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.UserPatients", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserPatients", new[] { "PatientId" });
            DropIndex("dbo.UserPatients", new[] { "UserId" });
            DropTable("dbo.UserPatients");
        }
    }
}
