﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Answer
    {
        private int answerid;
        private int userid;
        private int status;
        private DateTime createdate;
        private string answertext;

        public int AnswerId { get { return answerid; } set { answerid = value; } }
        public int UserId { get { return userid; } set { userid = value; } }
        public int Status { get { return status; } set { status = value; } }
        public DateTime CreateDate { get { return createdate; } set { createdate = value; } }
        public string AnswerText { get { return answertext; } set { answertext = value; } }
    }
}