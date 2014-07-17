// ----------------------------------------------------------------------
// <copyright file="Mail.cs" company="Vertis Infotech Limited, UK">
//     Copyright © 2011 Vertis Infotech Ltd. All Rights Reserved.
// </copyright>
//
// ------------------------------------------------------------------------
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using log4net;

namespace MiraiConsultMVC
{
    /// <summary>
    /// send mail methods.
    /// </summary>
    public class Mail
    {
        private static readonly ILog logfile = LogManager.GetLogger(typeof(Mail));

        /// <summary>
        /// configures smpt server.
        /// </summary>
        /// <returns></returns>
        public SmtpClient configureSMTP()
        {
            SmtpClient mailClient = new SmtpClient();
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["isSMTPAuthentication"]))
            {
                mailClient.Host = ConfigurationManager.AppSettings["SMTPServer"].ToString();
                //For developer testing just uncomment these lines to send actual emails.
                mailClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                mailClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["isEnableSSl"]);
                mailClient.UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["isUseDefaultCredentials"].ToString());
                mailClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SmtpServerUsername"], ConfigurationManager.AppSettings["SmtpServerPassword"]);
            }
            else
            {
                mailClient.Host = ConfigurationManager.AppSettings["UNAUTHENTICATEDSMTPSERVER"].ToString();
            }
            return mailClient;
        }

        public static bool SendHTMLMail(string FromAddress, string ToAddress, string Subject, string Body)
        {
            try
            {
                Mail new_mail = new Mail();
                SmtpClient mail_client = new SmtpClient();
                mail_client = new_mail.configureSMTP();
                MailMessage Message = new MailMessage();
                FromAddress = ConfigurationManager.AppSettings["FromEmail"].ToString();
                if (FromAddress == "" || ToAddress == "")
                {
                    //dont send mail , info invalid
                    return false;
                }
                Message.To.Add(new MailAddress(ToAddress));
                Message.From = new MailAddress(FromAddress);
                Message.IsBodyHtml = true;
                Message.Subject = Subject;
                Message.Body = Body;
                mail_client.Send(Message);
                return true;
            }
            catch (System.Exception ex)
            {
                logfile.Error(ex);
                return false;
            }
        }

        public static bool SendHTMLMail(string FromAddress, string ToAddress, string Subject, string Body, string[] EmailParameters)
        {
            try
            {
                MailMessage Message = new MailMessage();
                SmtpClient mail_client = new SmtpClient();
                Mail new_mail = new Mail();
                mail_client = new_mail.configureSMTP();
                if (FromAddress == "" || ToAddress == "")
                {
                    //dont send mail , info invalid
                    return false;
                }
                /* replacing any from mail id with one which in weg.config */
                FromAddress = ConfigurationManager.AppSettings["FromEmail"].ToString();
                Message.To.Add(new MailAddress(ToAddress));
                Message.From = new MailAddress(FromAddress);
                Message.IsBodyHtml = true;
                Message.Subject = Subject;
                Message.Body = EmailParameters != null ? String.Format(Body, EmailParameters) : Body;
                mail_client.Send(Message);
                return true;
            }
            catch (System.Exception ex)
            {
                logfile.Error(ex);
                return false;
            }
        }

        public static bool SendHTMLMail(string FromAddress, string[] ToAddresses, string Subject, string Body, string[] EmailParameters)
        {
            try
            {
                MailMessage Message = new MailMessage();
                SmtpClient mail_client = new SmtpClient();
                Mail new_mail = new Mail();
                mail_client = new_mail.configureSMTP();
                if (FromAddress == "" || ToAddresses.Length == 0)
                {
                    //dont send mail , info invalid
                    return false;
                }
                foreach (string address in ToAddresses)
                {
                    if (address != null && address != "")
                        Message.To.Add(new MailAddress(address));
                }
                /* replacing any from mail id with one which in weg.config */
                FromAddress = ConfigurationManager.AppSettings["FromEmail"].ToString();
                Message.From = new MailAddress(FromAddress);
                Message.IsBodyHtml = true;
                Message.Subject = Subject;
                Message.Body = EmailParameters != null ? String.Format(Body, EmailParameters) : Body;
                mail_client.Send(Message);
                return true;
            }
            catch (System.Exception ex)
            {
                logfile.Error(ex);
                return false;
            }
        }
        public static bool SendHTMLMailWithImage(string FromAddress, string ToAddress, string Subject, string Body, string imagePath)
        {
            try
            {
                MailMessage Message = new MailMessage();
                SmtpClient mail_client = new SmtpClient();
                Mail new_mail = new Mail();
                mail_client = new_mail.configureSMTP();
                if (FromAddress == "" || ToAddress == "")
                {
                    //dont send mail , info invalid
                    return false;
                }
                /* replacing any from mail id with one which in weg.config */
                FromAddress = ConfigurationManager.AppSettings["FromEmail"].ToString();

                Message.To.Add(new MailAddress(ToAddress));
                Message.From = new MailAddress(FromAddress);
                Message.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

                //Add Image
                LinkedResource Image = new LinkedResource(imagePath);
                Image.ContentId = "logoImage";

                //Add the Image to the Alternate view
                htmlView.LinkedResources.Add(Image);

                //Add view to the Email Message
                Message.AlternateViews.Add(htmlView);
                Message.Body = Body;
                Message.Subject = Subject;
                mail_client.Send(Message);
                return true;
            }
            catch (System.Exception ex)
            {
                logfile.Error(ex);
                return false;
            }
        }
        public static bool SendHTMLMailWithImage(string FromAddress, string[] ToAddress, string Subject, string Body, string imagePath)
        {
            try
            {
                MailMessage Message = new MailMessage();
                SmtpClient mail_client = new SmtpClient();
                Mail new_mail = new Mail();
                mail_client = new_mail.configureSMTP();
                if (FromAddress == "" || ToAddress.Length == 0)
                {
                    //dont send mail , info invalid
                    return false;
                }
                /* replacing any from mail id with one which in weg.config */
                FromAddress = ConfigurationManager.AppSettings["FromEmail"].ToString();
                foreach (string address in ToAddress)
                {
                    if (address != null && address != "")
                        Message.To.Add(new MailAddress(address));
                }
                Message.From = new MailAddress(FromAddress);
                Message.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

                //Add Image
                LinkedResource Image = new LinkedResource(imagePath);
                Image.ContentId = "logoImage";

                //Add the Image to the Alternate view
                htmlView.LinkedResources.Add(Image);

                //Add view to the Email Message
                Message.AlternateViews.Add(htmlView);
                Message.Body = Body;
                Message.Subject = Subject;
                mail_client.Send(Message);
                return true;
            }
            catch (System.Exception ex)
            {
                logfile.Error(ex);
                return false;
            }
        }
    }
}
