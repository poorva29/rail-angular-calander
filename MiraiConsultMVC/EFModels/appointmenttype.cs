namespace MiraiConsultMVC.EFModels
{
    using System.ComponentModel.DataAnnotations;

    public partial class appointmenttype
    {
        public int appointmenttypeid { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }
    }
}
