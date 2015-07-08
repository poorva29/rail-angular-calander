using System;
using System.Web;
using System.Configuration;

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

            msg += "<p align=left><font size=2 face=verdana>Thank you and welcome to Mirai Health (" + ConfigurationManager.AppSettings["website"] + "). ";

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
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
                                         "<br>" + "<br>" + "<br>" +
                                        "<img src='cid:logoImage' ></img>" +
                                        "</form></body></html>";
            return strBodyContent;
        }

        public static string GetTemplateOfApprovalNotificationEmailToDoc(string DoctorName)
        {
            string msg = "";
            msg += "<p align=left><font size=2 face=verdana>Dear Dr. " + DoctorName + ",</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Congratulations! Your Mirai Health account has been approved. Now you can login to the system with your credentials, set up your profile and start answering questions from users.</font></p>";
            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                        msg +
                                        "<br>" +
                                        "<font size=2 face=verdana> Regards,</font>" +
                                        "<br>" +
                                         "<font size=2 face=verdana>Mirai Health Team</font>" +
                                         "<br>" +
                                        "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                         "<br>" +
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
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
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
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
            msg += "<p align=left><font size=2 face=verdana>Thank you and welcome to Mirai Health (" + ConfigurationManager.AppSettings["website"] + "). ";
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
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
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
                                             "<b><font size=2 face=verdana> Mirai Health </font></b>" +

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
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
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
            EmailBody += "<p align=left><font size=2 face=verdana>Please sign-into Mirai Health (" + ConfigurationManager.AppSettings["website"] + ") to answer the question. </font></p>";

            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                        EmailBody +
                                        "<br>" +
                                        "<font size=2 face=verdana> Best wishes,</font>" +
                                        "<br>" +
                                        "<font size=2 face=verdana>Mirai Health Team</font>" +
                                         "<br>" +
                                        "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                         "<br>" +
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
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
                                      "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
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
                                "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
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
                                "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
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


            EmailBody += "<p align=left><font size=2 face=verdana><a href=" + ConfigurationManager.AppSettings["website"] + "> " + ConfigurationManager.AppSettings["website"] + " </a></font></p>";

            string strBodyContent = "<html><body> <form name=frmMessage method=post>" +
                                        EmailBody +
                                        "<br>" +
                                        "<font size=2 face=verdana> Regards,</font>" +
                                        "<br>" +
                                        "<font size=2 face=verdana>Mirai Health Team</font>" +
                                        "<br>" +
                                        "<b><font size=2 face=verdana color=#69728B !important > " + ConfigurationManager.AppSettings["FromEmail"].ToString() + "</font></b>" +
                                        "<br>" +
                                        "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
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
            
            EmailBody += "<p align=left><font size=2 face=verdana>Please sign-into Mirai Health (" + ConfigurationManager.AppSettings["website"] + ") to view the answer.  </font></p>";
            
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
                                      "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
                                      "<br>" + "<br>" + "<br>" +
                                      "<img src='cid:logoImage' ></img>" +
                                      "</form></body></html>";
            return strBodyContent;
        }

        public static string GetEmailTemplateToSendWelcomeMessage(string doctorName, string emailId,string password)
        {
            string EmailBody = "<p align=left><font size=2 face=verdana>Dear Dr. " + doctorName + ",</font></p>";

            EmailBody += "<p align=left><font size=2 face=verdana>Congratulations! Your Mirai Health account has been created. Now you can login to the system with following credentials, set up your profile and continue answering questions from patients.</font></p>";

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
                                      "<b><font  face=Verdana size=2  color='#69728B' !important>" + ConfigurationManager.AppSettings["website"].ToString() + "</font></b>" +
                                      "<br>" + "<br>" + "<br>" +
                                      "<img src='cid:logoImage' ></img>" +
                                      "</form></body></html>";
            return strBodyContent;
        }

        public static string SendEmailNotificationToPatientForPaidAppointments(string date, string time, string docFullName, DateTime prepayBy, string token)
        {
            string msg = "";
            string[] prepayByDateTime = Convert.ToString(prepayBy).Split(' ');
            msg += "<p align=left><font size=2 face=verdana>Hello</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Your appointment with Dr. " + docFullName + " on " + Utilities.GetDisplayDate(Convert.ToDateTime(date)) + " at " + Utilities.GetDisplayTime(time) + " has been blocked. Your doctor requests a pre-pay to confirm the appointment.</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Click on this link to pay and confirm the appointment: <a href='" + ConfigurationManager.AppSettings["prePayUrl"].ToString() + token + "'>" + ConfigurationManager.AppSettings["prePayUrl"].ToString() + token + "</a>.</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Please pay by " + Utilities.GetDisplayDate(Convert.ToDateTime(prepayByDateTime[0])) + " " + Utilities.GetDisplayTime(prepayByDateTime[1]) + " to avoid cancellation.</font></p>";
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
                                    "<img src=cid:logoImage>" +
                                    "</form></body></html>";
            return strBodyContent;
        }

        public static string SendCancellationNotificationForPaidAppointments(string date, string time, string docFullName)
        {
            string msg = "";
            msg += "<p align=left><font size=2 face=verdana>Hello</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Your appointment with Dr. " + docFullName + " on " + Utilities.GetDisplayDate(Convert.ToDateTime(date)) + " at " + Utilities.GetDisplayTime(time) + " has been cancelled as payment was not received in time.</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Please call (not SMS) on " + ConfigurationManager.AppSettings["phoneNumber"].ToString() + " to book another appointment. Mirai Health (service@miraihealth.com)</font></p>";
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
                                    "<img src=cid:logoImage>" +
                                    "</form></body></html>";
            return strBodyContent;
        }

        public static string SendSuccessfulNotificationToDoctorForPrepaidAppt (string docFullName, string patientName, DateTime date, string time, string clinicName, string city, decimal ReceivedAmount)
        {
            string msg = "";
            msg += "<p align=left><font size=2 face=verdana>Dear Dr. " + docFullName + ", </font></p>";
            msg += "<p align=left><font size=2 face=verdana>We have received payment for your appointment with patient " + patientName + " has been received. </font></p>";
            msg += "<p align=left><font size=2 face=verdana>The appointment details are</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Patient Name: " + patientName + "</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Appointment Date,Time: " + Utilities.GetDisplayDate(date) + " ," + Utilities.GetDisplayTime(time) + "</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Clinic: " + clinicName + "</font></p>";
            msg += "<p align=left><font size=2 face=verdana>City: " + city + "</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Payment Received: Rs. " + ReceivedAmount + "</font></p>";
            msg += "<p align=left><font size=2 face=verdana>Please feel free to contact us for any questions.</font></p>";
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
                                    "<img src=cid:logoImage>" +
                                    "</form></body></html>";
            return strBodyContent;
        }
    }
}
