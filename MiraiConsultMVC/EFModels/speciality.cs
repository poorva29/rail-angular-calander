namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class speciality
    {
        public int specialityid { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        public int? created_by { get; set; }

        public DateTime? created_on { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_on { get; set; }
    }
}
