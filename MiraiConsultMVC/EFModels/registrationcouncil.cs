namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("registrationcouncil")]
    public partial class registrationcouncil
    {
        [Key]
        public int regcouncilid { get; set; }

        public int countryid { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }
    }
}
