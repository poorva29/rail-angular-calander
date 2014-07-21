using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiraiConsultMVC.Models;

namespace MiraiConsultMVC.Controllers
{
    public class QuestionsController : Controller
    {
        //
        // GET: /Questions/
        _dbAskMiraiDataContext db;
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DoctorQuestionList(int userId = 6,bool filter = false)
        {
            IList<QuestionModel> Questions = new List<QuestionModel>();
            db = new _dbAskMiraiDataContext();
            var QuestionsById = db.getQuestionListByDoctorid(userId).ToList();
            QuestionModel QModel;
            AnswerModel AModel;

            
            if (QuestionsById != null && QuestionsById.Count > 0)
            {
                foreach (var question in QuestionsById)
                {
                    QModel = new QuestionModel();
                    AModel = new AnswerModel();
                    QModel.QuestionId = Convert.ToInt32(question.questionid);
                    QModel.QuestionText = Convert.ToString(question.questiontext);
                    QModel.CreateDate = Convert.ToDateTime(question.createdate);
                    if (question.answeredby != null)
                    {
                        QModel.AnsweredBy = Convert.ToInt32(question.answeredby);
                    }
                    if (question.answertext!=null)
                    {
                        AModel.AnswerText = Convert.ToString(question.answertext);
                        QModel.answers.Add(AModel);
                    }
                    QModel.UserId = userId;
                    QModel.Filter = filter;
                    Questions.Add(QModel);

                }
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_DoctorQuestionList", Questions);
            }
            return View(Questions);
        }
        [HttpGet]
        public ActionResult DoctorQuestionDetails(int QuestionId = 0)
        {
            try
            {
                IList<QuestionDtlModel> QDModel = new List<QuestionDtlModel>();
                QuestionDtlModel qm;
                db = new _dbAskMiraiDataContext();
                System.Data.Linq.ISingleResult<get_questiondetailsbyIdResult> ModelQuestion = db.get_questiondetailsbyId(QuestionId, userId, 0, 1);
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
                return View(QDModel);
            }
            catch
            {
                return View();
            }
        }
    }
}
