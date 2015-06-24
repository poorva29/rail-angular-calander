namespace MiraiConsultMVC.EFModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mobilelog")]
    public partial class mobilelog
    {
        [Key]
        public int logid { get; set; }

        [StringLength(500)]
        public string filename { get; set; }

        [StringLength(50)]
        public string filextension { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        [StringLength(200)]
        public string username { get; set; }

        public int? userid { get; set; }

        [StringLength(100)]
        public string logtype { get; set; }

        public DateTime? logdate { get; set; }
    }
}
