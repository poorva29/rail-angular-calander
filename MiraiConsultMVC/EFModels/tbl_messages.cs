namespace MiraiConsultMVC.EFModels
{
    using System.ComponentModel.DataAnnotations;

    public partial class tbl_messages
    {
        [Key]
        public int chat_message_id { get; set; }

        public byte[] message { get; set; }

        [MaxLength(1000)]
        public byte[] chat_dialog_id { get; set; }

        [MaxLength(1000)]
        public byte[] sender_id { get; set; }

        [MaxLength(1000)]
        public byte[] receipient_id { get; set; }

        public byte[] thumbnail_file_location { get; set; }

        public byte[] file_location_of_server { get; set; }

        [MaxLength(500)]
        public byte[] read_by_receipient { get; set; }

        [MaxLength(1000)]
        public byte[] date_sent { get; set; }

        [MaxLength(1000)]
        public byte[] created_at { get; set; }

        [MaxLength(1000)]
        public byte[] updated_at { get; set; }
    }
}
