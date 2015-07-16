namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class enforce_usertype_in_users : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.users", "usertype", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.users", "usertype", c => c.Int());
        }
    }
}
