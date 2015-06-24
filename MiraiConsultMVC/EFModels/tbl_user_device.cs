namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_user_device
    {
        public int id { get; set; }

        [MaxLength(500)]
        public byte[] user_id { get; set; }

        [MaxLength(500)]
        public byte[] device_id { get; set; }

        public byte[] push_token { get; set; }

        public byte[] os { get; set; }

        public byte[] os_version { get; set; }

        public byte[] app_version { get; set; }

        public byte[] device_name { get; set; }

        [MaxLength(1000)]
        public byte[] create_date { get; set; }

        [MaxLength(1000)]
        public byte[] created_by { get; set; }

        [MaxLength(1000)]
        public byte[] modified_date { get; set; }

        [MaxLength(1000)]
        public byte[] modified_by { get; set; }
    }
}
