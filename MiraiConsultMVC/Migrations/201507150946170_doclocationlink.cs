namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doclocationlink : DbMigration
    {
        public override void Up()
        {
            Sql("delete from appointments where doclocationid in(select doctorlocationid from doctorlocations where doctorid not in (select userid from users))");
            Sql("delete from doctorlocations where doctorid not in (select userid from users)");
            Sql("delete from doctorlocations where locationid = 0");
            CreateIndex("dbo.doctorlocations", "doctorid");
            AddForeignKey("dbo.doctorlocations", "doctorid", "dbo.users", "userid", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.doctorlocations", "doctorid", "dbo.users");
            DropIndex("dbo.doctorlocations", new[] { "doctorid" });
        }
    }
}
