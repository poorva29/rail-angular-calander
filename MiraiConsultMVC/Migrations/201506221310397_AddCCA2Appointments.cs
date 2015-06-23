namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCCA2Appointments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.appointments", "cca_order", c => c.String(maxLength: 20));
            AddColumn("dbo.appointments", "cca_status", c => c.Int(nullable: false));
            AddColumn("dbo.appointments", "cca_paid_on", c => c.DateTime(nullable: false));
            AddColumn("dbo.appointments", "cca_amount", c => c.Decimal(precision: 18, scale: 2, storeType: "numeric"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.appointments", "cca_amount");
            DropColumn("dbo.appointments", "cca_paid_on");
            DropColumn("dbo.appointments", "cca_status");
            DropColumn("dbo.appointments", "cca_order");
        }
    }
}
