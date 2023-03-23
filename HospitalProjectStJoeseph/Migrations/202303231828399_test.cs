namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clinics", "ServiceId", c => c.Int(nullable: false));
            AddColumn("dbo.Clinics", "ServiceName", c => c.String());
            CreateIndex("dbo.Clinics", "ServiceId");
            AddForeignKey("dbo.Clinics", "ServiceId", "dbo.Services", "ServiceId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Clinics", "ServiceId", "dbo.Services");
            DropIndex("dbo.Clinics", new[] { "ServiceId" });
            DropColumn("dbo.Clinics", "ServiceName");
            DropColumn("dbo.Clinics", "ServiceId");
        }
    }
}
