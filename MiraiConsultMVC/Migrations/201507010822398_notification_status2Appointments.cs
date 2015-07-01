namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notification_status2Appointments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.appointments", "notification_status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.appointments", "notification_status");
        }
    }
}
