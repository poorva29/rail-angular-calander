namespace MiraiConsultMVC.EFModels
{
    using System.ComponentModel.DataAnnotations;

    public partial class tbl_chat_dialogs
    {
        public int id { get; set; }

        public byte[] chat_dialog_id { get; set; }

        public byte[] patient_id { get; set; }

        public byte[] doctor_id { get; set; }

        public byte[] dialog_text { get; set; }

        [MaxLength(500)]
        public byte[] dialog_status { get; set; }

        public byte[] order_id { get; set; }

        public byte[] create_date { get; set; }

        public byte[] created_by { get; set; }

        public byte[] modified_date { get; set; }

        public byte[] modified_by { get; set; }

        [StringLength(500)]
        public byte[] cca_order { get; set; }

        public byte[] cca_status { get; set; }

        public byte[] cca_paid_on { get; set; }

        public byte[] cca_amount { get; set; }

        public byte[] is_waived { get; set; }

        public byte[] dialog_fee { get; set; }
    }
}