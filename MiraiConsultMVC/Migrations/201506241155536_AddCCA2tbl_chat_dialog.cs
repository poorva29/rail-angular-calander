namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCCA2tbl_chat_dialog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_chat_dialogs", "cca_order", c => c.Binary(maxLength: 500));
            AddColumn("dbo.tbl_chat_dialogs", "cca_status", c => c.Binary());
            AddColumn("dbo.tbl_chat_dialogs", "cca_paid_on", c => c.Binary());
            AddColumn("dbo.tbl_chat_dialogs", "cca_amount", c => c.Binary());
            AddColumn("dbo.tbl_chat_dialogs", "is_waived", c => c.Binary());
            AddColumn("dbo.tbl_chat_dialogs", "dialog_fee", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_chat_dialogs", "dialog_fee");
            DropColumn("dbo.tbl_chat_dialogs", "is_waived");
            DropColumn("dbo.tbl_chat_dialogs", "cca_amount");
            DropColumn("dbo.tbl_chat_dialogs", "cca_paid_on");
            DropColumn("dbo.tbl_chat_dialogs", "cca_status");
            DropColumn("dbo.tbl_chat_dialogs", "cca_order");
        }
    }
}
