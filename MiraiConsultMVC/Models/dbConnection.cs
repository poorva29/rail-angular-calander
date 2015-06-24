namespace MiraiConsultMVC.Models
{
    public class dbConnection
    {
        public dbConnection()
        {
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
        }
    }   
}