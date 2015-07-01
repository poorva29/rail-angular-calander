namespace MiraiConsultMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_cca_payments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.cca_payment",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        amount = c.Single(nullable: false),
                        order_id = c.String(unicode: false),
                        currency = c.String(unicode: false),
                        card_name = c.String(unicode: false),
                        payment_mode = c.String(unicode: false),
                        bank_ref_no = c.String(unicode: false),
                        tracking_id = c.String(unicode: false),
                        order_status = c.String(unicode: false),
                        failure_message = c.String(unicode: false),
                        status_message = c.String(unicode: false),
                        billing_name = c.String(unicode: false),
                        billing_address = c.String(unicode: false),
                        billing_city = c.String(unicode: false),
                        billing_state = c.String(unicode: false),
                        billing_zip = c.String(unicode: false),
                        billing_country = c.String(unicode: false),
                        billing_tel = c.String(unicode: false),
                        billing_email = c.String(unicode: false),
                        delivery_name = c.String(unicode: false),
                        delivery_address = c.String(unicode: false),
                        delivery_city = c.String(unicode: false),
                        delivery_state = c.String(unicode: false),
                        delivery_zip = c.String(unicode: false),
                        delivery_country = c.String(unicode: false),
                        delivery_tel = c.String(unicode: false),
                        status_code = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            AlterColumn("dbo.answers", "title", c => c.String(maxLength: 50));
            AlterColumn("dbo.answers", "answertext", c => c.String(maxLength: 1500));
            AlterColumn("dbo.answers", "image", c => c.String(maxLength: 100));
            AlterColumn("dbo.answers", "deviceid", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.answers", "deviceid", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.answers", "image", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.answers", "answertext", c => c.String(maxLength: 1500, unicode: false));
            AlterColumn("dbo.answers", "title", c => c.String(maxLength: 50, unicode: false));
            DropTable("dbo.cca_payment");
        }
    }
}
