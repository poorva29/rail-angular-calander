namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("healthinformation")]
    public partial class healthinformation
    {
        public int healthinformationid { get; set; }

        public int doctorid { get; set; }

        [StringLength(200)]
        public string healthmessagesubject { get; set; }

        public string healthmessagedetails { get; set; }

        public DateTime createddate { get; set; }

        [Required]
        [StringLength(50)]
        public string createdby { get; set; }

        public DateTime? modifieddate { get; set; }

        [StringLength(50)]
        public string modiifiedby { get; set; }

        public DateTime? publishdate { get; set; }
    }
}
