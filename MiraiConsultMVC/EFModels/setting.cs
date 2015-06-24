namespace MiraiConsultMVC.EFModels
{
    using System.ComponentModel.DataAnnotations;

    public partial class setting
    {
        public int settingid { get; set; }

        [StringLength(100)]
        public string name { get; set; }

        [StringLength(4000)]
        public string value { get; set; }
    }
}
