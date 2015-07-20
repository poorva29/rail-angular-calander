namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_url_in_doctorlocation : DbMigration
    {
        public override void Up()
        {
           // AddColumn("dbo.doctorlocations", "url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.doctorlocations", "url");
        }
    }
}
