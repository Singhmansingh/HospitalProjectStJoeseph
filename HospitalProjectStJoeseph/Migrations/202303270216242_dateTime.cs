namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Services", "ServiceTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Clinics", "ClinicTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clinics", "ClinicTime", c => c.Time(nullable: false, precision: 7));
            AlterColumn("dbo.Services", "ServiceTime", c => c.Time(nullable: false, precision: 7));
        }
    }
}
