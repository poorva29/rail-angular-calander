namespace MiraiConsultMVC.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AllowNullsInPrepayColumns : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.appointments", "prepay_by", c => c.DateTime());
            AlterColumn("dbo.appointments", "cca_paid_on", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.appointments", "cca_paid_on", c => c.DateTime(nullable: false));
            AlterColumn("dbo.appointments", "prepay_by", c => c.DateTime(nullable: false));
        }
    }
}
