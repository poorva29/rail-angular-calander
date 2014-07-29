using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiraiConsultMVC.Models
{
    public class AnswerModel
    {
        private int answerid;
        private int userid;
        private int status;
        private DateTime createdate;
        private string answertext;
        private string _answerImage;

        public int AnswerId { get { return answerid; } set { answerid = value; } }
        public int UserId { get { return userid; } set { userid = value; } }
        public int Status { get { return status; } set { status = value; } }
        public DateTime CreateDate { get { return createdate; } set { createdate = value; } }
        public string AnswerText { get { return answertext; } set { answertext = value; } }
        public string AnswerImage { get { return _answerImage; } set { _answerImage = value; } }
    }
}