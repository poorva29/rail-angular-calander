using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiraiConsultMVC.Models
{

    public class QuestionDtlModel
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public int UserId { get; set; }

        public int Gender { get; set; }

        public string PatientLastName { get; set; }

        public string PatientEmail { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string MobileNo { get; set; }
        public int status { get; set; }
        public DateTime CreateDate { get; set; }
        public string seoQuestionText { get; set; }
        public string QuestionText { get; set; }


        public int AnswerId { get; set; }
        public string DoctorImg { get; set; }
        public string Title { get; set; }
        public string AnswerText { get; set; }
        public string AnswerImg { get; set; }
        public DateTime AnswerDate { get; set; }
        public int DocId { get; set; }
        public string Doctor { get; set; }
        public bool IsPatientThank { get; set; }
        public bool IsEndorse { get; set; }
        public int ThanxCount { get; set; }
        public int EndorseCount { get; set; }
        public bool IsDocconnectUser { get; set; }
        public string DocconnectDoctorId { get; set; }
        public string Name_seo { get; set; }
        
    }
    public class QuestionModel
    {
        private int questionid;
        private int userid;
        private int status;
        private DateTime createdate;
        private string questiontext;
        private string counts;
        private int answeredby;
        private string _docImg;
        private string _title;
        private string textSeo;
        private bool _isdocconnectuser;
        public IList<AnswerModel> answers;


        public int QuestionId { get { return questionid; } set { questionid = value; } }
        public int UserId { get { return userid; } set { userid = value; } }
        public int Status { get { return status; } set { status = value; } }
        public DateTime CreateDate { get { return createdate; } set { createdate = value; } }
        public string QuestionText { get { return questiontext; } set { questiontext = value; } }
        public string Counts { get { return counts; } set { counts = value; } }
        public int AnsweredBy { get { return answeredby; } set { answeredby = value; } }
        public string DocImg { get { return _docImg; } set { _docImg = value; } }
        public string Title { get { return _title; } set { _title = value; } }
        public bool isdocconnectuser { get { return _isdocconnectuser; } set { _isdocconnectuser = value; } }
        public bool Filter { get; set; }
        public string answerreplyedby { get; set; }
        public string name_seo { get; set; }
        public string QuestionTextSeo { get { return textSeo; } set { textSeo = value; } }
        public QuestionModel()
        {
            answers = new List<AnswerModel>();
        }
        public void AddAnswer(AnswerModel answer)
        {
            answers.Add(answer);
        }
        public void RemoveAnswer(AnswerModel answer)
        {
            this.answers.Remove(answer);
        }
    }
}