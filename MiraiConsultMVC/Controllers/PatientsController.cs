using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;
using MiraiConsultMVC;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using DAL;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Model;
using System.Data.SqlClient;
namespace MiraiConsultMVC.Controllers
{
    public class PatientsController : Controller
    {
        //
        // GET: /Patients/

        _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
        public int questionId;
        private int userId;
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PatientProfile()
        {
            return View(getPatientDetailsByPatientId(Convert.ToInt32(Session["UserId"])));
        }

        public ActionResult Myactivity()
        {
             SqlConnection conn = null;
            DataSet dsQuestion = null;
            IList<QuestionModel> lstQuestions = new List<QuestionModel>();
            SqlParameter[] param = new SqlParameter[1];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@Userid", Convert.ToInt32(Session["UserId"]));
                dsQuestion = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_AllQuestionsByUserId", param);
            }
            QuestionModel questions;
            if (dsQuestion != null && dsQuestion.Tables.Count > 0 && dsQuestion.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsQuestion.Tables[0].Rows)
                {
                    questions = new QuestionModel();
                    questions.QuestionId = Convert.ToInt32(dr["questionid"]);
                    questions.QuestionText = Convert.ToString(dr["questiontext"]);
                    questions.Counts = Convert.ToString(dr["counts"]);
                    questions.CreateDate = Convert.ToDateTime(dr["createdate"]);
                    lstQuestions.Add(questions);
                }
            }
            return View(lstQuestions);
        }
        [HttpPost]
        public ActionResult PatientProfile(Profile profile)
        {
            if (ModelState.IsValid)
            {
                profile.RegistrationDate = DateTime.Now;
                profile.UserType = Convert.ToInt32(UserType.Patient);
                profile.UserId = Convert.ToInt32(Session["UserId"]);
                profile.Status = Convert.ToInt32(UserStatus.Pending);
                if (!TempData["Email"].Equals(profile.Email))
                {
                    profile.IsEmailVerified = false;
                }
                else
                {
                    profile.IsEmailVerified = true;
                }
                profile.DateOfBirth = DateTime.Parse(Convert.ToString(profile.DateOfBirth));
                var result = (db.askmirai_patient_Insert_Update(profile.FirstName, profile.FirstName, profile.Email, profile.MobileNo, profile.Gender, profile.DateOfBirth, profile.CountryId, profile.StateId, profile.LocationId, profile.CityId, profile.Password, profile.Height, profile.Weight, profile.Address, profile.Pincode, profile.UserId, profile.RegistrationDate, profile.Status, profile.UserType, profile.UserName, profile.IsEmailVerified)).ToList();
                if (result != null && result.Count() > 0)
                {
                    var res = result.FirstOrDefault();
                    if (Convert.ToBoolean(res.EmailAvailable))
                     {
                         if (!TempData["Email"].Equals(profile.Email))
                         {
                             string patientid = Convert.ToString(res.UserId);
                             string emailVerficationURL = ConfigurationManager.AppSettings["EmailVerificationLink"].ToString();
                             string emailBody = EmailTemplates.SendNotificationEmailtoUser(profile.FirstName, patientid, emailVerficationURL, "Patient");
                             string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                             string Logoimage = Server.MapPath("..\\Resources\\image\\LogoForMail.png");
                             Mail.SendHTMLMailWithImage(fromEmail, profile.Email, "Mirai Consult - Verify your email", emailBody, Logoimage);
                             Session["UserFullName"] = profile.FirstName + ' ' + profile.LastName;                            
                             ViewBag.message = "Account has been Updated successfully and you will receive verification email shortly. Please check spam/junk incase you don't find an email in your inbox.";
                         }
                         else
                         {
                             Session["UserFullName"] = profile.FirstName + ' ' + profile.LastName;
                             ViewBag.message = "Account has been updated successfully.";
                         }
                         Session["locationid"] = profile.LocationId;
                         Session["cityid"] = profile.CityId;
                     }

                    else if (!Convert.ToBoolean(res.EmailAvailable))
                    {
                        ViewBag.message = "This username is not available. Please select a different username.";
                    }
                }
            }
            var countryList = poupulateCountry();
            profile.Countries = new SelectList(countryList, "countryid", "name");
            profile.CountryId = Convert.ToInt32(TempData["CountryId"]);

            var stateList = poupulateState(Convert.ToInt32(TempData["CountryId"]));
            profile.States = new SelectList(stateList, "stateId", "name");
            profile.StateId = Convert.ToInt32(TempData["stateId"]);

            var cityList = poupulateCity(Convert.ToInt32(TempData["stateId"]));
            profile.Cities = new SelectList(cityList, "cityId", "name");
            profile.CityId = Convert.ToInt32(TempData["cityId"]);

            var locationList = poupulateLocation(Convert.ToInt32(TempData["cityId"]));
            profile.Locations = new SelectList(locationList, "locationId", "name");
            profile.LocationId = Convert.ToInt32(TempData["locationId"]);

            return View(profile);
        }
       
        public IList<Country> poupulateCountry()
        {
           IList<Country> countryLst = new List<Country>();
            var countrylist = db.countries.ToList().OrderBy(c=>c.name);
            if(countrylist != null && countrylist.Count() > 0)
            {
                foreach(var country in countrylist)
                {
                    Country country1 = new Country();
                    country1.countryid = Convert.ToInt32(country.countryid);
                    country1.name = Convert.ToString(country.name);
                    country1.countrycode = Convert.ToString(country.countrycode);
                    countryLst.Add(country1);
                }
            }
            return countryLst;
        }
       
        public IList<State> poupulateState(int countryId)
        {
            IList<State> stateLst = new List<State>();
            var stateList = db.states.Where(s => s.countryid.Equals(countryId)).ToList().OrderBy(c => c.name);
            if (stateList != null && stateList.Count() > 0)
            {
                foreach (var state in stateList)
                {
                    State state1 = new State();
                    state1.stateid = Convert.ToInt32(state.stateid);
                    state1.countryid = Convert.ToInt32(state.countryid);
                    state1.name = Convert.ToString(state.name);
                    stateLst.Add(state1);
                }
            }
            return stateLst;
        }
        
        public IList<City> poupulateCity(int stateId)
        {
            IList<City> cityLst = new List<City>();
            var cityList = db.cities.Where(c => c.stateid.Equals(stateId)).ToList().OrderBy(c => c.name);
            if (cityList != null && cityList.Count() > 0)
            {
                foreach (var city in cityList)
                {
                    City city1 = new City();
                    city1.cityid = Convert.ToInt32(city.cityid);
                    city1.stateid = Convert.ToInt32(city.stateid);
                    city1.name = Convert.ToString(city.name);
                    cityLst.Add(city1);
                }
            }
            return cityLst;
        }
       
        public IList<Location> poupulateLocation(int cityId)
        {
            IList<Location> locationLst = new List<Location>();
            var locationList = db.locations.Where(c => c.cityid.Equals(cityId)).ToList().OrderBy(c => c.name);
            if (locationList != null && locationList.Count() > 0)
            {
                foreach (var location in locationList)
                {
                    Location location1 = new Location();
                    location1.locationid = Convert.ToInt32(location.locationid);
                    location1.cityid = Convert.ToInt32(location.cityid);
                    location1.name = Convert.ToString(location.name);
                    locationLst.Add(location1);
                }
            }
            return locationLst;
        }

        [HttpGet]
        public JsonResult poupulateStateByCountry(int countryId)
        {
            IList<State> stateLst = new List<State>();
            var stateList = db.states.Where(s => s.countryid.Equals(countryId)).ToList().OrderBy(c => c.name);
            if (stateList != null && stateList.Count() > 0)
            {
                foreach (var state in stateList)
                {
                    State state1 = new State();
                    state1.stateid = Convert.ToInt32(state.stateid);
                    state1.countryid = Convert.ToInt32(state.countryid);
                    state1.name = Convert.ToString(state.name);
                    stateLst.Add(state1);
                }
            }
            return Json(stateLst,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult poupulateCityByState(int stateId)
        {
            IList<City> cityLst = new List<City>();
            var cityList = db.cities.Where(s => s.stateid.Equals(stateId)).ToList().OrderBy(c => c.name);
            if (cityList != null && cityList.Count() > 0)
            {
                foreach (var city in cityList)
                {
                    City city1 = new City();
                    city1.cityid = Convert.ToInt32(city.cityid);
                    city1.stateid = Convert.ToInt32(city.stateid);
                    city1.name = Convert.ToString(city.name);
                    cityLst.Add(city1);
                }
            }
            return Json(cityLst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult poupulateLocationByCity(int cityId)
        {
            IList<Location> locationLst = new List<Location>();
            var locationList = db.locations.Where(c => c.cityid.Equals(cityId)).ToList().OrderBy(c => c.name);
            if (locationList != null && locationList.Count() > 0)
            {
                foreach (var location in locationList)
                {
                    Location location1 = new Location();
                    location1.locationid = Convert.ToInt32(location.locationid);
                    location1.cityid = Convert.ToInt32(location.cityid);
                    location1.name = Convert.ToString(location.name);
                    locationLst.Add(location1);
                }
            }
            return Json(locationLst, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public Profile getPatientDetailsByPatientId(int userid)
        {
            Profile patientDetail = new Profile();
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
            var patient = db.users.FirstOrDefault(p => p.userid.Equals(userid));          
            if (patient != null)
            {
                patientDetail.UserId = Convert.ToInt32(patient.userid);
                patientDetail.FirstName = Convert.ToString(patient.firstname);
                patientDetail.LastName = Convert.ToString(patient.lastname);
                patientDetail.Email = Convert.ToString(patient.email);
                TempData["Email"] = Convert.ToString(patient.email);
                if (patient.mobileno != null)
                    patientDetail.MobileNo = Convert.ToString(patient.mobileno);
                if (patient.gender != null)
                    patientDetail.Gender = Convert.ToInt32(patient.gender);
                if (patient.address != null)
                    patientDetail.Address = Convert.ToString(patient.address);
                if (patient.height != null)
                    patientDetail.Height = Convert.ToInt32(patient.height);
                if (patient.weight != null)
                    patientDetail.Weight = Convert.ToDecimal(patient.weight);
                if (patient.pincode != null)
                    patientDetail.Pincode = Convert.ToInt32(patient.pincode);
                if (patient.dateofbirth != null)
                    patientDetail.DateOfBirth = Convert.ToDateTime(patient.dateofbirth);
                if (patient.countryid != null && patient.countryid != 0)
                {
                    var countryList = poupulateCountry();    
                    patientDetail.Countries = new SelectList(countryList, "countryid", "name");
                    patientDetail.CountryId = Convert.ToInt32(patient.countryid);
                    TempData["CountryId"] = patient.countryid;
                }
                if (patient.stateid != null && patient.stateid != 0)
                {
                    var stateList = poupulateState(Convert.ToInt32(patient.countryid));
                    patientDetail.States = new SelectList(stateList, "stateId", "name");
                    patientDetail.StateId = Convert.ToInt32(patient.stateid);
                    TempData["stateId"] = patientDetail.StateId;
                }
                if (patient.cityid != null && patient.cityid != 0)
                {
                    var cityList = poupulateCity(Convert.ToInt32(patient.stateid));
                    patientDetail.Cities = new SelectList(cityList, "cityId", "name");
                    patientDetail.CityId = Convert.ToInt32(patient.cityid);
                    TempData["cityId"] = patientDetail.CityId;
                }
                if (patient.locationid != null && patient.locationid != 0)
                {
                    var locationList = poupulateLocation(Convert.ToInt32(patient.cityid));
                    patientDetail.Locations = new SelectList(locationList, "locationId", "name", "cityid");
                    patientDetail.LocationId = Convert.ToInt32(patient.locationid);
                    TempData["locationId"] = patientDetail.LocationId;
                }
                if (patient.username != null)
                    patientDetail.UserName = Convert.ToString(patient.username);
                if (patient.registrationdate != null)
                    patientDetail.RegistrationDate = Convert.ToDateTime(patient.registrationdate);
                if (patient.status != null)
                    patientDetail.Status = Convert.ToInt32(patient.status);
                if (patient.isemailverified != null)
                {
                    patientDetail.IsEmailVerified = Convert.ToBoolean(patient.isemailverified);
                }
            }
            return patientDetail;
        }
        public string AutoComplete(string term)
        {
            DataSet dsQuestions = QuestionManager.getInstance().searchQuestion(term, Convert.ToInt32(QuestionStatus.Approved));
            return JsonConvert.SerializeObject(dsQuestions.Tables[0]);
        }

        [HttpGet]
        public ActionResult PatientQuestionDetails()        
        {
            try
            {
                if (Request.QueryString["questionid"] != null)
                {
                    questionId = Convert.ToInt32(Request.QueryString["questionid"]);
                }
                if (Session["UserId"] != null)
                {
                    userId = Convert.ToInt32(Session["UserId"]);
                }
                IList<QuestionDtlModel> QDModel = new List<QuestionDtlModel>();
                QuestionDtlModel qm;
                System.Data.Linq.ISingleResult<get_questiondetailsbyIdResult> ModelQuestion = db.get_questiondetailsbyId(questionId, 63, 0, Convert.ToInt32(QuestionStatus.Approved));
                @ViewBag.questionid = questionId;
                foreach (var item in ModelQuestion)
                {
                    qm = new QuestionDtlModel();
                    qm.AnswerDate = Convert.ToDateTime(item.answerdate);
                    qm.AnswerId = Convert.ToInt32(item.answerid);
                    qm.AnswerImg = item.answerimg;
                    qm.AnswerText = item.answertext;
                    qm.CreateDate = Convert.ToDateTime(item.createdate);
                    qm.DocconnectDoctorId = item.docconnectdoctorid;
                    qm.DocId = Convert.ToInt32(item.Docid);
                    qm.Doctor = item.doctor;
                    qm.DoctorImg = item.doctorimg;
                    qm.Email = item.Email;
                    qm.EndorseCount = Convert.ToInt32(item.endorsecount);
                    qm.Gender = Convert.ToInt32(item.gender);
                    qm.Id = item.id;
                    qm.IsDocconnectUser = Convert.ToBoolean(item.isdocconnectuser);
                    qm.IsEndorse = Convert.ToBoolean(item.isendorse);
                    qm.IsPatientThank = Convert.ToBoolean(item.ispatientthank);
                    qm.LastName = item.lastname;
                    qm.MobileNo = item.mobileno;
                    qm.PatientEmail = item.patientemail;
                    qm.PatientLastName = item.patientlastname;
                    qm.QuestionId = Convert.ToInt32(item.questionid);
                    qm.QuestionText = item.questiontext;
                    qm.status = Convert.ToInt32(item.status);
                    qm.ThanxCount = Convert.ToInt32(item.thanxcount);
                    qm.Title = item.title;
                    qm.UserId = Convert.ToInt32(item.userid);
                    QDModel.Add(qm);
                }
                ViewBag.AskmiraiUrl = Convert.ToString(ConfigurationSettings.AppSettings["askMiraiLink"]);
                ViewBag.FacebookAppKey = Convert.ToString(ConfigurationSettings.AppSettings["FacebookAppKey"]);

                DataTable dtTags = UtilityManager.getInstance().getAlltags();

                List<tag> tags = new List<tag>();

                var selectdTags = db.questiontags.Where(x => x.questionid.Equals(questionId)).ToList();

                tags = dtTags.AsEnumerable().Select(dataRow => new tag
                {
                    tagid = dataRow.Field<int>("tagid"),
                    tagname = dataRow.Field<string>("tagname"),
                }).ToList();

                List<tag> seletedTagslist = new List<tag>();
                int[] values = new int[selectdTags.Count];
                int count = 0;
                foreach (var item in selectdTags)
	            {
                    values[count++]=Convert.ToInt32(item.tagid);
	            }
                MultiSelectList makeSelected = new MultiSelectList(tags, "tagid", "tagname", values);
                ViewBag.tags = makeSelected;
                return View(QDModel);
            }
            catch(Exception e)
            {
                return View();
            }
        }

        public ActionResult AskDoctor()
        {
            return View(new AskDoctor());
        }

        [HttpGet]
        public ActionResult GetSimilarQuestion(string questionText)
        {            
            IList<AskDoctor> lstQuestions = new List<AskDoctor>();
            var questionList = db.get_AllQuestionsByTag(questionText, Convert.ToInt32(QuestionStatus.Approved)).ToList();
            if (questionList != null && questionList.Count() > 0)
            {
                foreach (var item in questionList)
                {
                    AskDoctor qModel = new AskDoctor();
                    qModel.QuestionId = Convert.ToInt32(item.questionid);
                    qModel.QuestionText = item.questiontext;
                    qModel.Counts = item.counts;
                    lstQuestions.Add(qModel);
                }
            }
            return Json(lstQuestions, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult question_Insert(string questionText)
        {
            int userId = 0;
            int result = 0;
            if (Session["UserId"] != null && Session["UserType"] != null)
            {
                userId = Convert.ToInt32(Session["UserId"]);
            }
            Question question = new Question();
            question.UserId = userId;
            question.Status = Convert.ToInt32(QuestionStatus.Pending);
            question.QuestionText = questionText;
            result = QuestionManager.getInstance().insertQuestion(question);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InviteFriend()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InviteFriend(MiraiConsultMVC.Models.Patients.InviteFriend inviteFriend)
        {
            ViewBag.Message = "";
            if(ModelState.IsValid)
            {
                string from_address = null;
                bool sent_mail = false;
                string emailsIds = inviteFriend.email;
                string msgBody = inviteFriend.message;
                msgBody = "<html><body> <form name=frmMessage method=post>" +
                            msgBody + "<br><br>" +
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
                string subject = "I want to invite you to use Mirai Consult";
                if (Session["UserId"] != null && Session["UserEmail"] != null)
                {
                    from_address = Convert.ToString(Session["UserEmail"]);
                }
                string Logoimage = Server.MapPath("..\\Content\\image\\LogoForMail.png");
                sent_mail = Mail.SendHTMLMailWithImage(from_address, emailsIds.Split(','), subject, msgBody, Logoimage);
                if (!sent_mail)
                {
                    ViewBag.Message = "Failed to send email.";
                }
                else
                {
                    ViewBag.Message = "Your email sent successfully.";
                    ModelState.Clear();
                }
            }
            return View();
        }

        public ActionResult similarQuestions(string questionText)
        {
            DataSet QuestionDetails = QuestionManager.getInstance().getQuestionDetailsbyQuestionText(questionText, Convert.ToInt32(QuestionStatus.Approved));

            List<QuestionModel> viewmodel = new List<QuestionModel>();

            //viewmodel = QuestionDetails.Tables[0].AsEnumerable().Select(dataRow => new QuestionModel
            //{
            //    UserId = dataRow.Field<int>("UserID"),
            //    QuestionId = dataRow.Field<string>("questionid"),
            //    QuestionText = dataRow.Field<string>("cities"),
            //    isdocconnectuser = dataRow.Field<string>("specialities"),
            //    userid = dataRow.Field<int>("userid"),
            //    locations = dataRow.Field<string>("locations"),
            //    questiontext = QuestionDetails.Tables[0].Rows[0]["questiontext"].ToString()

            //}).ToList();

            return View();
        }
    }
}
