namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentDescriptionOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.appointments", "description", c => c.String(maxLength: 255, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.appointments", "description", c => c.String(nullable: false, maxLength: 255, unicode: false));
        }
    }
}
