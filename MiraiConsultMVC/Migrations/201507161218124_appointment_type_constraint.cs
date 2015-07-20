namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class appointment_type_constraint : DbMigration
    {
        public override void Up()
        {
            Sql("set identity_insert appointmenttypes ON");
            Sql("insert appointmenttypes(appointmenttypeid, name) values(0, 'Patient Appointment')");
            Sql("set identity_insert appointmenttypes OFF");
            Sql("update appointments set appointmenttypeid = 0 where appointmenttypeid = -1");
            CreateIndex("dbo.appointments", "appointmenttypeid");
            AddForeignKey("dbo.appointments", "appointmenttypeid", "dbo.appointmenttypes", "appointmenttypeid");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.appointments", "appointmenttypeid", "dbo.appointmenttypes");
            DropIndex("dbo.appointments", new[] { "appointmenttypeid" });
        }
    }
}
