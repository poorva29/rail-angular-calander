namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class packagesubscriptionmonth
    {
        [Key]
        public int packagesubscriptionmonthsid { get; set; }

        public int packagesubscriptionmonths { get; set; }
    }
}
