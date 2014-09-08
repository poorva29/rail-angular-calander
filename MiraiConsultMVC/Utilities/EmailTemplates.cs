using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MiraiConsultMVC;

namespace MiraiConsultMVC
{
    public class EmailTemplates
    {
        public static string SendNotificationEmailtoUser(string FirstName, string UserID, string emailVerificationURL,string userType)
        {
            string encryptedUserId = Utilities.Encrypt(UserID);
            String[] values = new String[1];
            values[0] = emailVerificationURL + "?id=" + HttpUtility.UrlEncode(encryptedUserId) + "&userType=" + userType;
            string msg = "";
            msg += "<p align=left><font size=2 face=verdana>Dear " + FirstName + ",</font></p>";

            msg += "<p align=left><font size=2 face=verdana>Thank you and welcome to Mirai Consult (" + ConfigurationManager.AppSettings["website"] + "). ";

            msg += "<p align=left><font size=2 face=verdana>Please click on the link below to verify your email.</font></p>";

            msg += "<p align=left><font size=2 face=verdana><a href=" + values[0] + ">" + values[0] + "</a></font></p>";

            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                        msg +
                                        "<br>" +
                                        "<font size=2 face=verdana> Regards,</font>" +
                                        "<br>" +
                                        "<font size=2 face=verdana>Mirai Health Team</font>" +
                                         "<br>" +
                                        "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                         "<br>" +
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                         "<br>" + "<br>" + "<br>" +
                                        "<img src='cid:logoImage' ></img>" +
                                        "</form></body></html>";
            return strBodyContent;
        }

        public static string GetTemplateOfApprovalNotificationEmailToDoc(string DoctorName)
        {
            string msg = "";
            msg += "<p align=left><font size=2 face=verdana>Dear Dr. " + DoctorName + ",</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Congratulations! Your Mirai Consult account has been approved. Now you can login to the system with your credentials, set up your profile and start answering questions from users.</font></p>";
            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                        msg +
                                        "<br>" +
                                        "<font size=2 face=verdana> Regards,</font>" +
                                        "<br>" +
                                         "<font size=2 face=verdana>Mirai Health Team</font>" +
                                         "<br>" +
                                        "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                         "<br>" +
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                         "<br>" + "<br>" + "<br>" +
                                        "<img src='cid:logoImage' ></img>" +
                                        "</form></body></html>";
            return strBodyContent;
        }

        public static string EmailNotificationTempleteForRejectedDoctor()
        {
            string msg = "";
            msg += "<p align=left><font size=2 face=verdana>Your registration request has been rejected by the administrator.</font></p>";
            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                        msg +
                                        "<br>" +
                                        "<font size=2 face=verdana> Regards,</font>" +
                                        "<br>" +
                                        "<font size=2 face=verdana>Mirai Health Team</font>" +
                                         "<br>" +
                                        "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                         "<br>" +
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                         "<br>" + "<br>" + "<br>" +
                                        "<img src='cid:logoImage' ></img>" +
                                        "</form></body></html>";
            return strBodyContent;
        }

        public static string SendResetPasswordNotificationEmail(string UserId, string firstName, string emailRestPasswordURL)
        {
            string encryptedUserId = Utilities.Encrypt(UserId);
            String[] values = new String[1];
            values[0] = emailRestPasswordURL + "?id=" + HttpUtility.UrlEncode(encryptedUserId);
            string msg = "";
            msg += "<p align=left><font size=2 face=verdana>Dear " + firstName + ",</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Thank you and welcome to Mirai Consult (" + ConfigurationManager.AppSettings["website"] + "). ";
            msg += "<p align=left><font size=2 face=verdana>Please click on the link below, to reset password.</font></p>";
            msg += "<p align=left><font size=2 face=verdana><a href=" + values[0] + ">" + values[0] + "</a></font></p>";
            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                        msg +
                                        "<br>" +
                                        "<font size=2 face=verdana> Regards,</font>" +
                                        "<br>" +
                                        "<font size=2 face=verdana>Mirai Health Team</font>" +
                                         "<br>" +
                                        "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                         "<br>" +
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                         "<br>" + "<br>" + "<br>" +
                                        "<img src='cid:logoImage' ></img>" +
                                        "</form></body></html>";
            return strBodyContent;
        }
        /// <summary>
        /// returns the Email Template for Default Error Exception 
        /// </summary>
        /// <param name="ErrorDes"> error description </param>
        /// <param name="exceptionDetails">exception detail </param>
        /// <returns> </returns>
        public static string GetEmailTemplateToSendError(string ErrorDes, string exceptionDetails)
        {
            string msg = "<br>";
            msg += "<p align=left><font size=2 face=verdana>" + ErrorDes + " </font></p>";
            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                             msg +
                                             "<br>" +
                                             exceptionDetails +
                                             "<br>" +
                                             "<font size=2 face=verdana> Regards,</font>" +
                                             "<br>" +
                                             "<br>" +
                                             "<b><font size=2 face=verdana> Mirai Consult </font></b>" +

                                             "</form></body></html>";
            return strBodyContent;
        }
        public static string SendEmailVerifcationtoUser(string LastName, string UserID, string emailVerificationURL, String UserType, string emailid, bool isemailverfiy)
        {
            string encryptedUserId = Utilities.Encrypt(UserID);
            string encryptedUserType = Utilities.Encrypt(UserType);
            string encryptedEmailId = Utilities.Encrypt(emailid);
            String[] values = new String[1];
            if (isemailverfiy == false)
                values[0] = emailVerificationURL + "?id=" + HttpUtility.UrlEncode(encryptedUserId) + "&type=" + HttpUtility.UrlEncode(encryptedUserType);
            else
                values[0] = emailVerificationURL + "?id=" + HttpUtility.UrlEncode(encryptedUserId) + "&type=" + HttpUtility.UrlEncode(encryptedUserType) + "&email=" + HttpUtility.UrlEncode(encryptedEmailId) + "&isemailverify=" + isemailverfiy;
            string msg = "";
            msg += "<p align=left><font size=2 face=verdana>Dear Dr." + LastName + ",</font></p>";

            msg += "<p align=left><font size=2 face=verdana>Please click on below given link, to verify your email.</font></p>";

            msg += "<p align=left><font size=2 face=verdana><a href=" + values[0] + ">" + values[0] + "</a></font></p>";

            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                        msg +
                                        "<br>" +
                                        "<font size=2 face=verdana> Regards,</font>" +
                                        "<br>" +
                                        "<font size=2 face=verdana>Mirai Health Team</font>" +
                                         "<br>" +
                                        "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                         "<br>" +
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                         "<br>" + "<br>" + "<br>" +
                                        "<img src='cid:logoImage' ></img>" +
                                        "</form></body></html>";
            return strBodyContent;
        }
        public static string GetEmailTemplateOnQuestionAssign(string msg, string lastname, string questiontext)
        {
            string EmailBody = "<p align=left><font size=2 face=verdana>Dear " + lastname + ",</font></p>";
            EmailBody += "<p align=left><font size=2 face=verdana>" + msg + "</font></p>";
            EmailBody +="<p align=left> <font size=2 face=verdana>" +"\"" + "<font size=2 face=verdana>" + questiontext + "</font>" + "\"" +"</font></p>";
            EmailBody += "<p align=left><font size=2 face=verdana>Please sign-into Mirai Consult (" + ConfigurationManager.AppSettings["website"] + ") to answer the question. </font></p>";

            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                        EmailBody +
                                        "<br>" +
                                        "<font size=2 face=verdana> Best wishes,</font>" +
                                        "<br>" +
                                        "<font size=2 face=verdana>Mirai Health Team</font>" +
                                         "<br>" +
                                        "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                         "<br>" +
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                         "<br>" + "<br>" + "<br>" +
                                        "<img src='cid:logoImage' ></img>" +
                                        "</form></body></html>";
            return strBodyContent;
       }
       public static string GetEmailTemplateOnQuestionAnswer(string msg, string lastname, string questiontext, string answertext)
       {
           string EmailBody = "<p align=left><font size=2 face=verdana>Dear Dr. " + lastname + ",</font></p>";
           EmailBody += "<p align=left><font size=2 face=verdana>" + msg + "</font></p>";
           EmailBody += "<p align=left> <font size=2 face=verdana>" + "\"" + "<font size=2 face=verdana>" + questiontext + "</font>" + "\"" + "</font></p>";
           EmailBody += "<p align=left> <font size=2 face=verdana>" + "\"" + "<font size=2 face=verdana>" + answertext + "</font>" + "\"" + "</font></p>";
           string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                      EmailBody +
                                      "<br>" +
                                      "<font size=2 face=verdana> Best wishes,</font>" +
                                      "<br>" +
                                      "<font size=2 face=verdana>Mirai Health Team</font>" +
                                      "<br>" +
                                      "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                      "<br>" +
                                      "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                      "<br>" + "<br>" + "<br>" +
                                      "<img src='cid:logoImage' ></img>" +
                                      "</form></body></html>";
           return strBodyContent;
       }
       public static string GetEmailTemplateOnPatientThanked(string msgText, string lastname, string questiontext, string thanxcount)
       {
           string EmailBody = "<p align=left><font size=2 face=verdana>Dear Dr. " + lastname + ",</font></p>";
           EmailBody += "<p align=left><font size=2 face=verdana>" + msgText + "</font></p>";
           EmailBody += "<p align=left> <font size=2 face=verdana>" + "\"" + "<font size=2 face=verdana>" + questiontext + "</font>" + "\"" + "</font></p>";
           EmailBody += "<p align=left><font size=2 face=verdana>You have received " + thanxcount + " Thank you’s.</font></p>";
           string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                EmailBody +
                                "<br>" +
                                "<font size=2 face=verdana> Best wishes,</font>" +
                                "<br>" +
                                "<font size=2 face=verdana>Mirai Health Team</font>" +
                                "<br>" +
                                "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                "<br>" +
                                "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                "<br>" + "<br>" + "<br>" +
                                "<img src='cid:logoImage' ></img>" +
                                "</form></body></html>";
           return strBodyContent;
       }
       public static string GetEmailTemplateOnDoctorEndorsed(string msgText, string lastname, string questiontext, string endorsecount)
       {
           string EmailBody = "<p align=left><font size=2 face=verdana>Dear Dr. " + lastname + ",</font></p>";
           EmailBody += "<p align=left><font size=2 face=verdana>" + msgText + "</font></p>";
           EmailBody += "<p align=left> <font size=2 face=verdana>" + "\"" + "<font size=2 face=verdana>" + questiontext + "</font>" + "\"" + "</font></p>";
           EmailBody += "<p align=left><font size=2 face=verdana>You have received " + endorsecount + " endorsement(s).</font></p>";
           string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                EmailBody +
                                "<br>" +
                                "<font size=2 face=verdana> Best wishes,</font>" +
                                "<br>" +
                                "<font size=2 face=verdana>Mirai Health Team</font>" +
                                "<br>" +
                                "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                "<br>" +
                                "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                "<br>" + "<br>" + "<br>" +
                                "<img src='cid:logoImage' ></img>" +
                                "</form></body></html>";
           return strBodyContent;
        }
        public static string GetEmailTemplateOnRejectQuestionFromQuestionList(string msg, string patientname)
        {
            string EmailBody = "<p align=left><font size=2 face=verdana>Dear " + patientname + ",</font></p>";

            EmailBody += "<p align=left><font size=2 face=verdana>" + msg + "</font></p>";

            EmailBody += "<p align=left><font size=2 face=verdana>Please click on the link below to ask new question.</font></p>";


            EmailBody += "<p align=left><font size=2 face=verdana><a href=" + ConfigurationManager.AppSettings["websiteUrl"] + "> " + ConfigurationManager.AppSettings["websiteUrl"] + " </a></font></p>";

            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                        EmailBody +
                                        "<br>" +
                                        "<font size=2 face=verdana> Regards,</font>" +
                                        "<br>" +
                                        "<font size=2 face=verdana>Mirai Health Team</font>" +
                                        "<br>" +
                                        "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                        "<br>" +
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                        "<br>" + "<br>" + "<br>" +
                                        "<img src='cid:logoImage' ></img>" +
                                        "</form></body></html>";
            return strBodyContent;
        }
        public static string GetEmailTemplateOnQuestionAnswerToPatient(string patientname, string doctorname, string questiontext, string bookAppointmentUrl, string doctorId)
        {
            string encrypteddoctorId = Utilities.Encrypt(doctorId);
            String[] values = new String[1];
            values[0] = bookAppointmentUrl + "?doc=" + HttpUtility.UrlEncode(encrypteddoctorId);
            string EmailBody = "<p align=left><font size=2 face=verdana>Dear " + patientname + ",</font></p>";
            EmailBody += "<p align=left><font size=2 face=verdana>Dr. " + doctorname + " has addressed your question-</font></p>";
           
            EmailBody += "<p align=left> <font size=2 face=verdana>" + "\"" + "<font size=2 face=verdana>" + questiontext + "</font>" + "\"" + "</font></p>";
            
            EmailBody += "<p align=left><font size=2 face=verdana>Please sign-into Mirai Consult (" + ConfigurationManager.AppSettings["website"] + ") to view the answer.  </font></p>";
            
            EmailBody += "<p align=left><font size=2 face=verdana>You can also book an appointment with the doctor by clicking on the following link:</font></p>";
            
            EmailBody += "<p align=left><font size=2 face=verdana><a href=" + values[0] + ">" + values[0] + "</a></font></p>";

            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                      EmailBody +
                                      "<br>" +
                                      "<font size=2 face=verdana> Best wishes,</font>" +
                                      "<br>" +
                                      "<font size=2 face=verdana>Mirai Health Team</font>" +
                                      "<br>" +
                                      "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                      "<br>" +
                                      "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                      "<br>" + "<br>" + "<br>" +
                                      "<img src='cid:logoImage' ></img>" +
                                      "</form></body></html>";
            return strBodyContent;
        }

        public static string GetEmailTemplateToSendWelcomeMessage(string doctorName, string emailId,string password)
        {
            string EmailBody = "<p align=left><font size=2 face=verdana>Dear Dr. " + doctorName + ",</font></p>";

            EmailBody += "<p align=left><font size=2 face=verdana>Congratulations! Your MiraiConsult account has been created. Now you can login to the system with following credentials, set up your profile and continue answering questions from patients.</font></p>";

            EmailBody += "<p align=left> <font size=2 face=verdana>Credentials:</font></p>";

            EmailBody += "<p align=left><font size=2 face=verdana>Email ID: " + emailId +"</font></p>";

            EmailBody += "<p align=left><font size=2 face=verdana>Password: " + password +"</font></p>";

            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                      EmailBody +
                                      "<br>" +
                                      "<font size=2 face=verdana> Best wishes,</font>" +
                                      "<br>" +
                                      "<font size=2 face=verdana>Mirai Health Team</font>" +
                                      "<br>" +
                                      "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                      "<br>" +
                                      "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["WebsiteUrl"].ToString() + "</font></b>" +
                                      "<br>" + "<br>" + "<br>" +
                                      "<img src='cid:logoImage' ></img>" +
                                      "</form></body></html>";
            return strBodyContent;
        }
    }
}
