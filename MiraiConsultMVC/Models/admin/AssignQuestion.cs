namespace MiraiConsultMVC.Models.admin
{
    public class AssignQuestion
    {
        public int id { get; set; }
        public int Questionid { get; set; }
        public int userid { get; set; }
        public string name { get; set; }
        public string specialities { get; set; }
        public string cities { get; set; }
        public string locations { get; set; }
        public string questiontext { get; set; }
    }
}