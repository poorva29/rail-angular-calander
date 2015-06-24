namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class setting
    {
        public int settingid { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [StringLength(4000)]
        public string value { get; set; }
    }
}
