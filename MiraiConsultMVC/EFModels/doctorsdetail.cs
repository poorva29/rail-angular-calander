namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class doctorsdetail
    {
        [Key]
        public int doctordetailsid { get; set; }

        public int doctorid { get; set; }

        public int? type { get; set; }

        [StringLength(1000)]
        public string details { get; set; }

        [StringLength(150)]
        public string certification { get; set; }

        [StringLength(150)]
        public string society { get; set; }

        public int? created_by { get; set; }

        public DateTime? created_on { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_on { get; set; }
    }
}
