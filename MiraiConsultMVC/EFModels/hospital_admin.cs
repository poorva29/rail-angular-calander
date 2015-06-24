namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class hospital_admin
    {
        public int id { get; set; }

        public int hospital_id { get; set; }

        public int assistant_id { get; set; }

        public virtual hospital hospital { get; set; }

        public virtual user user { get; set; }
    }
}
