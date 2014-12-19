using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MiraiConsultMVC.Models
{
    public class AskDoctor
    {
        private int questionid;
        private int userid;
        private int status;
        private DateTime createdate;
        private string questiontext;
        private string seoQuestionText;
        private string counts;

        public int QuestionId { get { return questionid; } set { questionid = value; } }
        public int UserId { get { return userid; } set { userid = value; } }
        public int Status { get { return status; } set { status = value; } }
        public DateTime CreateDate { get { return createdate; } set { createdate = value; } }

        [Required(ErrorMessage = "Please Enter Question")]
        public string QuestionText { get { return questiontext; } set { questiontext = value; } }
        public string SeoQuestionText { get { return seoQuestionText; } set { seoQuestionText = value; } }
        public string Counts { get { return counts; } set { counts = value; } }
    }
}