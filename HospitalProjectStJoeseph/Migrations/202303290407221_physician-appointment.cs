namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class physicianappointment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "appointment_start", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointments", "appointment_end", c => c.DateTime(nullable: false));
            AddColumn("dbo.Physicians", "first_name", c => c.String());
            AddColumn("dbo.Physicians", "last_name", c => c.String());
            DropColumn("dbo.Appointments", "appointment_date");
            DropColumn("dbo.Appointments", "duration");
            DropColumn("dbo.Physicians", "physician_name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Physicians", "physician_name", c => c.String());
            AddColumn("dbo.Appointments", "duration", c => c.Int(nullable: false));
            AddColumn("dbo.Appointments", "appointment_date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Physicians", "last_name");
            DropColumn("dbo.Physicians", "first_name");
            DropColumn("dbo.Appointments", "appointment_end");
            DropColumn("dbo.Appointments", "appointment_start");
        }
    }
}
