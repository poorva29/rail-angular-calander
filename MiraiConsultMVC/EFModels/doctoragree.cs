namespace MiraiConsultMVC.EFModels
{
    using System.ComponentModel.DataAnnotations;

    public partial class doctoragree
    {
        [Key]
        public int docagreeid { get; set; }

        public int? doctorid { get; set; }

        public int? answerid { get; set; }
    }
}
