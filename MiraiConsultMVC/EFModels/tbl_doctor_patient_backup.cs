namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tbl_doctor_patient_backup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int docPatientId { get; set; }

        public int? docId { get; set; }

        public int? patientId { get; set; }

        public int? isRegistered { get; set; }

        public int? isEconsult { get; set; }

        public int? created_by { get; set; }

        public DateTime? created_on { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_on { get; set; }

        [StringLength(200)]
        public string deviceid { get; set; }
    }
}
