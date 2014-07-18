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

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DoctorQuestionList(int userId = 6,bool filter = false)
        {
            IList<QuestionModel> Questions = new List<QuestionModel>();
            _dbAskMiraiDataContext db = new _dbAskMiraiDataContext();
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
        
        public ActionResult DoctorQuestionDetails()
        {
            return View();
        }
    }
}
