namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class delayedappointmentsrecord
    {
        public int id { get; set; }

        public int locationid { get; set; }

        public DateTime date { get; set; }

        [Required]
        [StringLength(100)]
        public string delayedby { get; set; }

        [Required]
        [StringLength(4)]
        public string starttime { get; set; }

        [Required]
        [StringLength(4)]
        public string endtime { get; set; }

        public int doctorid { get; set; }

        [Required]
        [StringLength(4)]
        public string delay { get; set; }
    }
}
