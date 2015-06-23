namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class answer
    {
        public int answerid { get; set; }

        public int questionid { get; set; }

        public int? userid { get; set; }

        [StringLength(50)]
        public string title { get; set; }

        [StringLength(1500)]
        public string answertext { get; set; }

        public DateTime? createdate { get; set; }

        [StringLength(100)]
        public string image { get; set; }

        [StringLength(200)]
        public string deviceid { get; set; }
    }
}
