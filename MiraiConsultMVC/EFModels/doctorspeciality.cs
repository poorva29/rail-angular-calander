namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class doctorspeciality
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int doctorid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int specialityid { get; set; }

        public int? created_by { get; set; }

        public DateTime? created_on { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_on { get; set; }
    }
}
