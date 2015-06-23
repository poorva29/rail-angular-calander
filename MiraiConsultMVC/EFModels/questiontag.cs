namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class questiontag
    {
        public int questiontagid { get; set; }

        public int? questionid { get; set; }

        public int? tagid { get; set; }
    }
}
