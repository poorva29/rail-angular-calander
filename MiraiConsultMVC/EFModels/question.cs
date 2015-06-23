namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class question
    {
        public int questionid { get; set; }

        public int? userid { get; set; }

        public int status { get; set; }

        public DateTime createdate { get; set; }

        [StringLength(200)]
        public string questiontext { get; set; }

        [StringLength(200)]
        public string question_seo { get; set; }

        public int? question_type { get; set; }

        public int? chat_status { get; set; }

        public int? created_by { get; set; }

        [StringLength(200)]
        public string deviceid { get; set; }

        [StringLength(50)]
        public string dialogueid { get; set; }
    }
}
