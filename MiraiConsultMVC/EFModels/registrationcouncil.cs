namespace MiraiConsultMVC.EFModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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
