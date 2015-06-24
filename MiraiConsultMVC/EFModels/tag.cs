namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tag")]
    public partial class tag
    {
        public int tagid { get; set; }

        [StringLength(50)]
        public string tagname { get; set; }

        [StringLength(100)]
        public string tag_seo { get; set; }
    }
}
