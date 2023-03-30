namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReqForeignKey : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Requisitions", "physician_id");
            RenameColumn(table: "dbo.Requisitions", name: "PhysicianId", newName: "physician_id");
            RenameIndex(table: "dbo.Requisitions", name: "IX_PhysicianId", newName: "IX_physician_id");
            DropColumn("dbo.Requisitions", "patient_id");
            DropColumn("dbo.Requisitions", "clinic_id");
            DropColumn("dbo.Requisitions", "test_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requisitions", "test_id", c => c.Int(nullable: false));
            AddColumn("dbo.Requisitions", "clinic_id", c => c.Int(nullable: false));
            AddColumn("dbo.Requisitions", "patient_id", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Requisitions", name: "IX_physician_id", newName: "IX_PhysicianId");
            RenameColumn(table: "dbo.Requisitions", name: "physician_id", newName: "PhysicianId");
            AddColumn("dbo.Requisitions", "physician_id", c => c.Int(nullable: false));
        }
    }
}
