using System.Web.Mvc;

namespace MiraiConsultMVC
{
    public class BasePage:Controller
    {    /// <summary>
        /// Check for session expired if session is expired redirect to login page
        /// </summary>
        public void checkSessionExpired()
        {
            Utilities.checkSessionExpired();
        }        
        /// <summary>
        /// If any error then redirect to Custom error page. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        //void Page_Error(Object sender, EventArgs args)
        //{
        //    Exception error = Server.GetLastError();
        //    Session["unHandledErrors"] = error;
        //    Response.Redirect("../admin/CustomError.aspx");
        //}
        public int isAuthorisedandSessionExpired(int privilege)
        {
            Utilities.checkSessionExpired();
            if (!Utilities.isAuthorisedandSessionExpired(privilege))
            {
                return 1;
            }
            else
            return 0;
        }
    }
}