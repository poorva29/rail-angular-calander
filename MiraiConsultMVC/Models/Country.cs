namespace MiraiConsultMVC.Models
{
    public class Country:_dbAskMiraiDataContext
    {
        public int countryid { get; set; }
        public string name { get; set; }
        public string countrycode { get; set; }
    }
}