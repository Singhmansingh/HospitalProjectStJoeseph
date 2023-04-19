namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class patient_registered_check : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "PatientIsRegistered", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "PatientIsRegistered");
        }
    }
}
