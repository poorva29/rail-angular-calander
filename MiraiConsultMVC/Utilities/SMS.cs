using System;
using System.Net;
using System.IO;
using System.Configuration;
using log4net;

namespace MiraiConsultMVC
{
    public class SMS
    {
        private static readonly ILog logfile = LogManager.GetLogger(typeof(SMS));

        /// <summary>
        /// Return a SMS Gateway URL with username and password and senderid
        /// </summary>
        /// <returns></returns>
        public static string getSMSGatewayURL()
        {
            string SMSGatewayURL = "";
            string SMSGatewayUserName = "";
            string SMSGatewayPassword = "";
            string SMSGatewaySenderID = "";

            SMSGatewayURL = Convert.ToString(ConfigurationManager.AppSettings["SMSGatewayURL"]);
            SMSGatewayUserName = Convert.ToString(ConfigurationManager.AppSettings["SMSGatewayUserName"]); 
            SMSGatewayPassword = Convert.ToString(ConfigurationManager.AppSettings["SMSGatewayPassword"]);
            SMSGatewaySenderID = Convert.ToString(ConfigurationManager.AppSettings["SMSGatewaySenderID"]);

            // http:///api.mVaayoo.com/mvaayooapi/MessageCompose?user=Username:Password&senderID=mVaayoo
            if (!String.IsNullOrEmpty(SMSGatewayURL) && !String.IsNullOrEmpty(SMSGatewayUserName) && !String.IsNullOrEmpty(SMSGatewayPassword) && !String.IsNullOrEmpty(SMSGatewaySenderID))
                SMSGatewayURL = SMSGatewayURL + "?user=" + SMSGatewayUserName + ":" + SMSGatewayPassword + "&senderID=" + SMSGatewaySenderID;
 
            return SMSGatewayURL;
        }

        /// <summary>
        /// Send a SMS to a desired receipient
        /// </summary>
        /// <param name="receipientNo">Receipient's Mobile No.</param>
        /// <param name="msgText">Message Text</param>
        /// <returns>true - if message sent OR false if message is not sent or if there is any exception while sending sms</returns>
        public static bool SendSMS(string receipientNo, string msgText)
        {
            bool isSuccess = false;
            try
            {
                string strUrl = getSMSGatewayURL() +  "&receipientno="+ receipientNo + "&msgtxt=" + msgText + "&state=4";
                WebRequest request = HttpWebRequest.Create(strUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = (Stream)response.GetResponseStream();
                StreamReader readStream = new StreamReader(s);
                string dataString = readStream.ReadToEnd();
                response.Close();
                s.Close();
                readStream.Close();
                isSuccess =  true;
            }
            catch(Exception ex)
            {
                logfile.Error(ex);
                isSuccess = false;
            }
            return isSuccess;
        }

        public static string GenerateToken(int length)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            // Wait to force new Seed
            System.Threading.Thread.Sleep(20);
            Random rnd = new Random();
            char ch;
            int i = 0;
            for (i = 1; i <= length; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(25 * rnd.NextDouble() + 65));
                sb.Append(ch);
            }
            return sb.ToString();
        } 
    }
}