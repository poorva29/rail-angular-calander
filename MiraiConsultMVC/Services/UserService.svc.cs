using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Collections;
using Newtonsoft.Json;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using DAL;
using Model;
using System.Web;
using System.Configuration;
using log4net;
using MiraiConsultMVC;



namespace Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserService.svc or UserService.svc.cs at the Solution Explorer and start debugging.
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Required)]
    public class UserService : IUserService
    {
        private static readonly ILog logfile = LogManager.GetLogger(typeof(UserService));

        public String getFeedList(string lastQuestionNo, string RecordSize, int UserId, int myFeed, Boolean isEncrypt = true)
        {
            DataTable feedlist = null;
            if (!string.IsNullOrEmpty(lastQuestionNo))
            {
                feedlist = FeedManager.getInstance().getFeedListByLastQuestionNo(Convert.ToInt32(lastQuestionNo), Convert.ToInt32(RecordSize), Convert.ToInt32(UserId), Convert.ToInt32(myFeed), Convert.ToInt32(QuestionStatus.Approved));
                if (feedlist != null && isEncrypt)
                {
                    for (int cnt = 0; cnt < feedlist.Rows.Count; cnt++)
                    {
                        feedlist.Rows[cnt]["docid"] = Utilities.Encrypt(Convert.ToString(feedlist.Rows[cnt]["docid"]));
                    }
                }
            }
            return JsonConvert.SerializeObject(feedlist);
        }
        public String getDoctorListByCriteria(string cityid, string locationid, string specialityOrName, Boolean isEncrypt = true)
        {
            DataTable feedlist = null;
            if(specialityOrName.Contains(' '))
            {
               specialityOrName = specialityOrName.Replace(' ', '%');
            }
            feedlist = UserManager.getInstance().getDoctorListByCriteria(cityid, locationid, specialityOrName, Convert.ToInt32(UserStatus.Approved));
            if (feedlist != null &&  isEncrypt)
            {
                for (int cnt = 0; cnt < feedlist.Rows.Count; cnt++)
                {
                    feedlist.Rows[cnt]["userid"] = Utilities.Encrypt(Convert.ToString(feedlist.Rows[cnt]["userid"]));
                }
            }
            return JsonConvert.SerializeObject(feedlist);
        }
        public String thankdoctor(string userid, string answerid, string lastname, string emailid, string mobileno, string questiontext, string thanxcount)
        {
            questiontext = HttpUtility.UrlDecode(questiontext);
            int feedlist = 0;
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(answerid))
            {
                feedlist = FeedManager.getInstance().insertPatientThankToAnswer(Convert.ToInt32(userid), Convert.ToInt32(answerid));
            }
            string msgText = ConfigurationManager.AppSettings["OnPatientThankSendEmail"].ToString();
            if (!string.IsNullOrEmpty(msgText) && !string.IsNullOrEmpty(lastname) && !string.IsNullOrEmpty(questiontext) && !string.IsNullOrEmpty(thanxcount))
            {
                string emailBody = EmailTemplates.GetEmailTemplateOnPatientThanked(msgText, lastname, questiontext, thanxcount);
                string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                string Logoimage = HttpContext.Current.Server.MapPath("..\\Content\\image\\LogoForMail.png");
                Mail.SendHTMLMailWithImage(fromEmail, emailid, "Mirai Health - Patient thanked your answer", emailBody, Logoimage);
            }
            if (!string.IsNullOrEmpty(mobileno))
            {
                string SmsText = ConfigurationManager.AppSettings["OnPatientThankSendSMS"].ToString();
                SMS.SendSMS(mobileno, SmsText);
            }
            return JsonConvert.SerializeObject(feedlist);
        }
        public String endorsedoctoranswer(string userid, string answerid, string lastname, string Email, string answerreplyedby, string mobileno, string questiontext, string endorsecount)
        {
            questiontext = HttpUtility.UrlDecode(questiontext);
            int feedlist = 0;
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(answerid))
            {
                feedlist = FeedManager.getInstance().insertDoctorEndorseToAnswer(Convert.ToInt32(userid), Convert.ToInt32(answerid));
            }
            if (!string.IsNullOrEmpty(answerreplyedby) && !string.IsNullOrEmpty(lastname) && !string.IsNullOrEmpty(questiontext) && !string.IsNullOrEmpty(endorsecount))
            {
                string msgText = "We are pleased to inform you that Dr." + answerreplyedby + " has endorsed your answer";
                string emailBody = EmailTemplates.GetEmailTemplateOnDoctorEndorsed(msgText, lastname, questiontext, endorsecount);
                string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                string Logoimage = HttpContext.Current.Server.MapPath("..\\Content\\image\\LogoForMail.png");
                Mail.SendHTMLMailWithImage(fromEmail, Email, "Mirai Health - Endorsements Notification", emailBody, Logoimage);
            }
            string SmsText = ConfigurationManager.AppSettings["OnDoctorEndoresedSendSMS"].ToString();
            if (!string.IsNullOrEmpty(mobileno))
            {
                SMS.SendSMS(mobileno, SmsText);
            }
            return JsonConvert.SerializeObject(feedlist);
        }
        public String fillDropdowns()
        {
            DataTable dtCities = UtilityManager.getInstance().getAllCities();

            return JsonConvert.SerializeObject(dtCities);
        }

        public String getAllTags()
        {
            DataTable dtTags = UtilityManager.getInstance().getAllTags();

            return JsonConvert.SerializeObject(dtTags);
        }
        public String get_allTagsWithCountOfAnsweredQuestions()
        {
            DataTable dtTags = UtilityManager.getInstance().get_allTagsWithCountOfAnsweredQuestions().Tables[0];

            return JsonConvert.SerializeObject(dtTags);
        }
        
        public String addNewTags(string tag, string questionid)
        {
            int result = QuestionManager.getInstance().addNewTags(tag, Convert.ToInt32(questionid));
            return JsonConvert.SerializeObject(result);
        }
        public String removeTags(string tag)
        {
            int result = QuestionManager.getInstance().removeTags(tag);
            return JsonConvert.SerializeObject(result);
        }
        public String getLocationsbyCity(string cityId)
        {
            DataTable dtSpecialities = UtilityManager.getInstance().getAllLocationByCityId(Convert.ToInt32(cityId));

            return JsonConvert.SerializeObject(dtSpecialities);
        }
        public String searchQuestion(string seacrhstr)
        {
            DataSet dsQuestions = QuestionManager.getInstance().searchQuestion(seacrhstr, Convert.ToInt32(QuestionStatus.Approved));
            return JsonConvert.SerializeObject(dsQuestions.Tables[0]);
        }
        public String fillDegreeDropdowns()
        {
            DataSet dtDegrees = UtilityManager.getInstance().getAllDegree();

            return JsonConvert.SerializeObject(dtDegrees);
        }
        public String getStatebyCountry(string countryid)
        {
            DataTable dtStates = UtilityManager.getInstance().gelAllStatesbyCountry(Convert.ToInt32(countryid));
            return JsonConvert.SerializeObject(dtStates);
        }
        public String getAllCountryList()
        {
            DataTable countryList = null;
            countryList = UtilityManager.getInstance().getAllCountries();
            return JsonConvert.SerializeObject(countryList);
        }
        public String getCitybyState(int stateId)
        {
            DataTable dtCity = UtilityManager.getInstance().getAllCitiesByStateId(stateId);
            return JsonConvert.SerializeObject(dtCity);
        }
        public String getCountryStateCityData(int countryId, int stateId, int cityId, int locationId)
        {
            DataSet dsData = UtilityManager.getInstance().getCountryStateCityLocation(countryId, stateId, cityId, locationId);
            if (dsData != null)
            {
                dsData.Tables[0].TableName = "Country";
                dsData.Tables[1].TableName = "State";
                dsData.Tables[2].TableName = "City";
                dsData.Tables[3].TableName = "Location";
            }
            return JsonConvert.SerializeObject(dsData);
        }

        public String getQuestionList(string RecordSize, int Flag, int lastQuestionNo)
        {
            DataTable doclist = null;
            if (Flag != null)
            {
                doclist = QuestionManager.getInstance().getQuestionList(Convert.ToInt32(RecordSize), Flag, lastQuestionNo);
            }
            return JsonConvert.SerializeObject(doclist);
        }

        public String IncrementAppointmentHitCnt(string userid)
        {
            int docid = Convert.ToInt32(Utilities.Decrypt(HttpUtility.UrlDecode(Convert.ToString(userid)).Replace(" ", "+")));
            int retVal = UserManager.getInstance().updateAppointmentclickedcnt(docid);
            if (retVal != 0)
                return JsonConvert.SerializeObject("{'Success' : true}");
            else
                return JsonConvert.SerializeObject("{'Success' : false}");
        }

        public void get_AuthenticateData(string username, string password)
        {
            DataSet dsUserDetails = null;
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            try
            {
                dsUserDetails = DoctorManager.getInstance().get_AuthenticateData(username, Utilities.Encrypt(password));
                if (dsUserDetails != null && dsUserDetails.Tables.Count == 2)
                {
                    dsUserDetails.Tables[0].TableName = "UserInfo";
                    dsUserDetails.Tables[1].TableName = "DoctorQuestion";
                    
                    
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(dsUserDetails));
                }
                else if (dsUserDetails != null && dsUserDetails.Tables.Count == 3)
                {
                    dsUserDetails.Tables[0].TableName = "UserInfo";
                    dsUserDetails.Tables[1].TableName = "PatientQuestions";
                    dsUserDetails.Tables[2].TableName = "PatientAnswers";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(dsUserDetails));
                }
                else if (dsUserDetails != null && dsUserDetails.Tables.Count == 1)
                {
                    HttpContext.Current.Response.Write("{'Error':'true','Msg':'" + dsUserDetails.Tables[0].Rows[0]["MSG"].ToString() + "'}");
                }
                else
                {
                    logfile.Error("\n\nMessage: Dataset is null\n");
                    HttpContext.Current.Response.Write("{'Error':'true','Msg':'UserName & Password do not match'}");
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("{'Error':'true','Msg':'Server Error'}");
                // Log Error details & Send Crash mail to support
                string message = "Exception type: " + e.GetType() + Environment.NewLine + "Exception message: " + e.Message + Environment.NewLine +
                 "Stack trace: " + e.StackTrace + Environment.NewLine;
                logfile.Error("Web Service >>> App Crash >>> \n" + message);
            }
        }

        public void insertQuestion(int userId, string questionText)
        {
            try
            {
               int result = DoctorManager.getInstance().insertQuestion(userId, 1, questionText);
                //Need to check for time being I had put if condition
               if (result == 1)
               {
                   HttpContext.Current.Response.Write("{'Msg':'Question Inserted'}");
               }
               else
               {
                   HttpContext.Current.Response.Write("{'Error':'true','Msg':'Server Error'}");
               }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("{'Error':'true','Msg':'Server Error'}");
                // Log Error details & Send Crash mail to support
                string message = "Exception type: " + e.GetType() + Environment.NewLine + "Exception message: " + e.Message + Environment.NewLine +
                 "Stack trace: " + e.StackTrace + Environment.NewLine;
                logfile.Error("Web Service >>> App Crash >>> \n" + message);
            }
            
        }
        
        public void SaveDoctorAnswer(int questionId, int userId, string title, string answer)
        {
            try
            {
                DataSet dsResult = DoctorManager.getInstance().SaveDoctorAnswer(questionId, userId, title, answer, string.Empty);
                //Need to check for time being I had put if condition
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    DataSet QuestionDetails = QuestionManager.getInstance().getQuestionDetailsbyId(questionId, userId, Convert.ToInt32(QuestionStatus.Approved));
                    for (int i = 0; i < QuestionDetails.Tables[0].Rows.Count; i++)
                    {
                        if (QuestionDetails.Tables[0].Rows[i]["DocId"].ToString() == Convert.ToString(userId))
                        {
                            string msgText = ConfigurationManager.AppSettings["OnDocAnswerAssignQuestionSendEmail"].ToString();
                            string emailBody = EmailTemplates.GetEmailTemplateOnQuestionAnswer(msgText, QuestionDetails.Tables[0].Rows[i]["lastname"].ToString(), QuestionDetails.Tables[0].Rows[i]["questiontext"].ToString(), QuestionDetails.Tables[0].Rows[i]["answertext"].ToString());
                            string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                            string Logoimage = AppDomain.CurrentDomain.BaseDirectory + ("..\\Content\\image\\LogoForMail.png");
                            Mail.SendHTMLMailWithImage(fromEmail, QuestionDetails.Tables[0].Rows[i]["Email"].ToString(), "Mirai Health - Answer Notification", emailBody, Logoimage);
                        }
                    }
                    HttpContext.Current.Response.Write("{'Msg':'Answer Updated'}");
                }
                else
                {
                    HttpContext.Current.Response.Write("{'Error':'true','Msg':'Server Error'}");
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("{'Error':'true','Msg':'Server Error'}");
                // Log Error details & Send Crash mail to support
                string message = "Exception type: " + e.GetType() + Environment.NewLine + "Exception message: " + e.Message + Environment.NewLine +
                 "Stack trace: " + e.StackTrace + Environment.NewLine;
                logfile.Error("Web Service >>> App Crash >>> \n" + message);
            }
        }

        public void UpdateProfile(int userId, string firstName, string LastName, string mobile, string Email, string IsNewEmail)
        {
            try
            {
                DataSet dsresult = new DataSet();
                dsresult = DoctorManager.getInstance().UpdateProfile(userId, firstName, LastName, mobile, Email, IsNewEmail);
                if (dsresult.Tables.Count == 0)
                {
                    HttpContext.Current.Response.Write("{'Msg':'Profile Updated'}");
                }
                else if (dsresult.Tables.Count != 0 && dsresult.Tables[0].Rows.Count != 0)
                {
                    if (dsresult.Tables[0].Rows[0].ItemArray.Contains("Email Already Exist"))
                    {
                        HttpContext.Current.Response.Write("{'Msg':'Email Already Exist'}");
                    }
                    else
                    {
                        DataRow dr;
                        dr = dsresult.Tables[0].Rows[0];
                        if (dr["email"].ToString() != string.Empty)
                        {
                            string patientid = Convert.ToString(dr["UserId"]);
                            string emailVerficationURL = ConfigurationManager.AppSettings["EmailVerificationLink"].ToString();
                            string emailBody = EmailTemplates.SendNotificationEmailtoUser(dr["firstname"].ToString(), patientid, emailVerficationURL, "");
                            string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                            string Logoimage = AppDomain.CurrentDomain.BaseDirectory + ("Content\\image\\LogoForMail.png");
                            Mail.SendHTMLMailWithImage(fromEmail, dr["email"].ToString(), "Mirai Health - Verify your email", emailBody, Logoimage);

                            HttpContext.Current.Response.Write("{'Msg':'verification email shortly'}");
                        }
                    }
                    
                }
                
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("{'Error':'true','Msg':'Server Error'}");
                // Log Error details & Send Crash mail to support
                string message = "Exception type: " + e.GetType() + Environment.NewLine + "Exception message: " + e.Message + Environment.NewLine +
                 "Stack trace: " + e.StackTrace + Environment.NewLine;
                logfile.Error("Web Service >>> App Crash >>> \n" + message);
            }
            
        }

        public void UpdateQuestionTags(string Addedtags, string DeletedTags, string QuestionID)
        {
            string[] AddedtagsArr = Addedtags.Split(',');
            string[] DeletedTagsArr = DeletedTags.Split(',');
            IList<Tag> ToAddlstTags = new List<Tag>();
            IList<Tag> ToDeletelstTags = new List<Tag>();
            Tag tags;
            if (!string.IsNullOrEmpty(Addedtags))
            {
                foreach (string item in AddedtagsArr)
                {
                     tags = new Tag();
                    tags.TagId = Convert.ToInt32(item);
                    ToAddlstTags.Add(tags);
                }
            }
            if (!string.IsNullOrEmpty(DeletedTags))
            {
                foreach (string item in DeletedTagsArr)
                {
                    tags = new Tag();
                    tags.TagId = Convert.ToInt32(item);
                    ToDeletelstTags.Add(tags);
                }

            }
            QuestionManager.getInstance().assignTagsToQuestion(ToAddlstTags, ToDeletelstTags, Convert.ToInt32(QuestionID));
        }
    }
}
