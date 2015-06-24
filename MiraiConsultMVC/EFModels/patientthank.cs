namespace MiraiConsultMVC.EFModels
{
    using System.ComponentModel.DataAnnotations;

    public partial class patientthank
    {
        [Key]
        public int patientthanksid { get; set; }

        public int? userid { get; set; }

        public int? answerid { get; set; }
    }
}
