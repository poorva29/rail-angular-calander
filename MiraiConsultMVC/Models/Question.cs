﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class Question
    {
        private int questionid;
        private int userid;
        private int status;
        private DateTime createdate;
        private string questiontext;
        private string counts;
        private int answeredby;
        public IList<Answer> answers;

        public int QuestionId { get { return questionid; } set { questionid = value; } }
        public int UserId { get { return userid; } set { userid = value; } }
        public int Status { get { return status; } set { status = value; } }
        public DateTime CreateDate { get { return createdate; } set { createdate = value; } }
        public string QuestionText { get { return questiontext; } set { questiontext = value; } }
        public string Counts { get { return counts; } set { counts = value; } }
        public int AnsweredBy { get { return answeredby; } set { answeredby = value; } }

        public Question()
        {
            answers = new List<Answer>();
        }
        public void AddAnswer(Answer answer)
        {
            answers.Add(answer);
        }
        public void RemoveAnswer(Answer answer)
        {
            this.answers.Remove(answer);
        }
    }
}