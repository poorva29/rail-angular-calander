namespace MiraiConsultMVC.EFModels
{
    using System.ComponentModel.DataAnnotations;

    public partial class packagesubscriptionmonth
    {
        [Key]
        public int packagesubscriptionmonthsid { get; set; }

        public int packagesubscriptionmonths { get; set; }
    }
}
