namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class cca_payment
    {
        public int id { get; set; }

        public float amount { get; set; }

        public string order_id { get; set; }

        public string currency { get; set; }

        public string card_name { get; set; }

        public string payment_mode { get; set; }

        public string bank_ref_no { get; set; }

        public string tracking_id { get; set; }

        public string order_status { get; set; }

        public string failure_message { get; set; }

        public string status_message { get; set; }

        public string billing_name { get; set; }

        public string billing_address { get; set; }

        public string billing_city { get; set; }

        public string billing_state { get; set; }

        public string billing_zip { get; set; }

        public string billing_country { get; set; }

        public string billing_tel { get; set; }

        public string billing_email { get; set; }

        public string delivery_name { get; set; }

        public string delivery_address { get; set; }

        public string delivery_city { get; set; }

        public string delivery_state { get; set; }

        public string delivery_zip { get; set; }

        public string delivery_country { get; set; }

        public string delivery_tel { get; set; }

        public string status_code { get; set; }

    }
}
