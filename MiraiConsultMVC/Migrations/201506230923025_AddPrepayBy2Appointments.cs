namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrepayBy2Appointments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.appointments", "prepay_by", c => c.DateTime(nullable: false));
            DropColumn("dbo.appointments", "prepayhours");
        }
        
        public override void Down()
        {
            AddColumn("dbo.appointments", "prepayhours", c => c.Int(nullable: false));
            DropColumn("dbo.appointments", "prepay_by");
        }
    }
}
