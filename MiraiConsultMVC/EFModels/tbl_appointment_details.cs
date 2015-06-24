namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_appointment_details
    {
        [Key]
        public int followupid { get; set; }

        public int apptid { get; set; }

        public DateTime followupdate { get; set; }

        public DateTime followuptime { get; set; }

        [StringLength(1000)]
        public string notes { get; set; }

        [StringLength(1000)]
        public string instruction { get; set; }
    }
}
