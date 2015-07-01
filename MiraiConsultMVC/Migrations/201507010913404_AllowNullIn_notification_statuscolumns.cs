namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllowNullIn_notification_statuscolumns : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.appointments", "notification_status", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.appointments", "notification_status", c => c.Int(nullable: false));
        }
    }
}
