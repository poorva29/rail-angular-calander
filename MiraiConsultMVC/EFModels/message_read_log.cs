namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class message_read_log
    {
        public int id { get; set; }

        [MaxLength(1000)]
        public byte[] chat_message_id { get; set; }

        [MaxLength(1000)]
        public byte[] read_at { get; set; }
    }
}
