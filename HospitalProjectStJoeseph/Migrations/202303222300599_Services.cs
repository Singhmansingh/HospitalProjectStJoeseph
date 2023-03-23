namespace HospitalProjectStJoeseph.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Services : DbMigration
    {
        public override void Up()
        {
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
            DropTable("dbo.Services");
        }
    }
}
