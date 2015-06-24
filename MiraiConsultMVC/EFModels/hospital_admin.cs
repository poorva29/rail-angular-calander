namespace MiraiConsultMVC.EFModels
{
    public partial class hospital_admin
    {
        public int id { get; set; }

        public int hospital_id { get; set; }

        public int assistant_id { get; set; }

        public virtual hospital hospital { get; set; }

        public virtual user user { get; set; }
    }
}
