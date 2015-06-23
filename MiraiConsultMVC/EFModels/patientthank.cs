namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class patientthank
    {
        [Key]
        public int patientthanksid { get; set; }

        public int? userid { get; set; }

        public int? answerid { get; set; }
    }
}
