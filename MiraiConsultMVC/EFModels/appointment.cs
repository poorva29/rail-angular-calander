namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class appointment
    {
        public int appointmentid { get; set; }

        [StringLength(200)]
        public string subject { get; set; }

        public int? patientid { get; set; }

        public int doctorid { get; set; }

        public int? docassistantid { get; set; }

        public int doclocationid { get; set; }

        [Required]
        [StringLength(255)]
        public string description { get; set; }

        public DateTime starttime { get; set; }

        public DateTime endtime { get; set; }

        public short? isalldayevent { get; set; }

        public int? appointmenttypeid { get; set; }

        public int status { get; set; }

        public DateTime createdat { get; set; }

        public int createdby { get; set; }

        public DateTime? lastmodifiedat { get; set; }

        public int? lastmodifiedby { get; set; }

        [StringLength(100)]
        public string patientname { get; set; }

        [StringLength(100)]
        public string patientemail { get; set; }

        [StringLength(15)]
        public string patientmobile { get; set; }

        [StringLength(500)]
        public string comments { get; set; }

        [StringLength(500)]
        public string feedback { get; set; }

        public int? rating { get; set; }

        public int? unregpatientid { get; set; }

        [StringLength(1000)]
        public string note { get; set; }

        [StringLength(1000)]
        public string instruction { get; set; }

        [StringLength(200)]
        public string deviceid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? prepayamount { get; set; }

        public DateTime? prepay_by { get; set; }

        [StringLength(10)]
        public string txncode { get; set; }

        public bool? ispaid { get; set; }

        [StringLength(20)]
        public string cca_order { get; set; }

        public int cca_status { get; set; }

        public DateTime? cca_paid_on { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? cca_amount { get; set; }
    }
}
