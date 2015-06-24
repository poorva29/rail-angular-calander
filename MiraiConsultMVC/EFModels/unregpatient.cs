namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("unregpatient")]
    public partial class unregpatient
    {
        public int id { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(15)]
        public string mobileno { get; set; }

        public int? created_by { get; set; }

        public DateTime? created_on { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_on { get; set; }

        [StringLength(200)]
        public string deviceid { get; set; }
    }
}
