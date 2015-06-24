namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class appointmenttype
    {
        public int appointmenttypeid { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }
    }
}
