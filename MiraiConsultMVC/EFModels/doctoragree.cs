namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class doctoragree
    {
        [Key]
        public int docagreeid { get; set; }

        public int? doctorid { get; set; }

        public int? answerid { get; set; }
    }
}
