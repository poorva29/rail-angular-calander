namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointmentDocLocationLink : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.appointments", "doclocationid");
            AddForeignKey("dbo.appointments", "doclocationid", "dbo.doctorlocations", "doctorlocationid", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.appointments", "doclocationid", "dbo.doctorlocations");
            DropIndex("dbo.appointments", new[] { "doclocationid" });
        }
    }
}
