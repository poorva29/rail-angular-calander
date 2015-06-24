namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("patientfollowuphistory")]
    public partial class patientfollowuphistory
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        public int? doctorid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int patientid { get; set; }

        public int? appointmentid { get; set; }

        [StringLength(50)]
        public string patienttype { get; set; }

        [StringLength(1000)]
        public string note { get; set; }

        [StringLength(1000)]
        public string instruction { get; set; }

        public int? created_by { get; set; }

        public int? modified_by { get; set; }

        public DateTime? created_on { get; set; }

        public DateTime? modified_on { get; set; }

        public DateTime? reminderdate { get; set; }

        public bool? isemailsent { get; set; }

        [StringLength(200)]
        public string deviceid { get; set; }
    }
}
