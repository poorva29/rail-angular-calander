using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class QuestionManager
    {
        private static QuestionManager _instance = new QuestionManager();
        private QuestionManager() { }
        public static QuestionManager getInstance() { return _instance; }
        SqlConnection conn = null;

        public int insertQuestion(Question question)
        {
            SqlParameter[] questionParam = new SqlParameter[4];
            int result = 0;
            using (conn = SqlHelper.GetSQLConnection())
            {
                questionParam[0] = new SqlParameter("@userId", question.UserId);
                questionParam[1] = new SqlParameter("@status", question.Status);
                questionParam[2] = new SqlParameter("@createDate", System.DateTime.Now);
                questionParam[3] = new SqlParameter("@questionText", question.QuestionText);
                result = SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "question_Insert", questionParam);
            }
            return result;
        }

        public IList<Question> getAllQuestionByText(string question, int questionStatus)
        {
            SqlParameter[] questionParam = new SqlParameter[2];
            IList<Question> lstQuestions = new List<Question>();
            DataSet dsQuestion = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                questionParam[0] = new SqlParameter("@questionText", question);
                questionParam[1] = new SqlParameter("@questionStatus", questionStatus);
                dsQuestion = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_AllQuestionsByTag", questionParam);
            }
            Question questions;
            if (dsQuestion != null && dsQuestion.Tables.Count > 0 && dsQuestion.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsQuestion.Tables[0].Rows)
                {
                    questions = new Question();
                    questions.QuestionId = Convert.ToInt32(dr["questionid"]);
                    questions.QuestionText = Convert.ToString(dr["questiontext"]);
                    questions.Counts = Convert.ToString(dr["counts"]);
                    lstQuestions.Add(questions);
                }
            }
            return lstQuestions;
        }

        public DataSet getQuestionDetailsbyId(int questionid, int userid, int questionStatus)
        {
            SqlConnection conn = null;
            DataSet feedList = null;
            SqlParameter[] param = new SqlParameter[3];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@questionid", questionid);
                param[1] = new SqlParameter("@userid", userid);
                param[2] = new SqlParameter("@questionStatus", questionStatus);
                feedList = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_questiondetailsbyId", param);
            }
            if ((feedList != null) && (feedList.Tables.Count > 0))
            {
                return feedList;
            }
            return null;
        }
        public DataSet getQuestionDetailsbyId(int questionid, int userid, int assignQuestion, int questionStatus)
        {
            SqlConnection conn = null;
            DataSet feedList = null;
            SqlParameter[] param = new SqlParameter[4];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@questionid", questionid);
                param[1] = new SqlParameter("@userid", userid);
                param[2] = new SqlParameter("@assignQuestion", assignQuestion);
                param[3] = new SqlParameter("@questionStatus", questionStatus);
                feedList = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_questiondetailsbyId", param);
            }
            if ((feedList != null) && (feedList.Tables.Count > 0))
            {
                return feedList;
            }
            return null;
        }
        public IList<Question> getQuestionsbyUserId(int userID)
        {
            SqlConnection conn = null;
            DataSet dsQuestion = null;
            IList<Question> lstQuestions = new List<Question>();
            SqlParameter[] param = new SqlParameter[1];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@Userid", userID);
                dsQuestion = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_AllQuestionsByUserId", param);
            }
            Question questions;
            if (dsQuestion != null && dsQuestion.Tables.Count > 0 && dsQuestion.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsQuestion.Tables[0].Rows)
                {
                    questions = new Question();
                    questions.QuestionId = Convert.ToInt32(dr["questionid"]);
                    questions.QuestionText = Convert.ToString(dr["questiontext"]);
                    questions.Counts = Convert.ToString(dr["counts"]);
                    questions.CreateDate = Convert.ToDateTime(dr["createdate"]);
                    lstQuestions.Add(questions);
                }
            }
            return lstQuestions;
        }

        public DataSet getQuestionDetailsbyQuestionText(string questionText, int questionStatus)
        {
            SqlConnection conn = null;
            DataSet dsQuestionDetails = null;
            SqlParameter[] param = new SqlParameter[2];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@questionText", '%' + questionText + '%');
                param[1] = new SqlParameter("@questionStatus", questionStatus);
                dsQuestionDetails = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "getsimilarQuestionbyText", param);
            }
            if ((dsQuestionDetails != null) && (dsQuestionDetails.Tables.Count > 0))
            {
                return dsQuestionDetails;
            }
            return null;
        }

        public DataTable getQuestionList(int RecordSize, int flag, int lastQuestionNo)
        {
            SqlConnection conn = null;
            DataSet dsQuestionDetails = null;
            SqlParameter[] param = new SqlParameter[3];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@recordSize", RecordSize);
                param[1] = new SqlParameter("@flag", flag);
                param[2] = new SqlParameter("@lastQuestionNo", lastQuestionNo);
              
                dsQuestionDetails = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_AllQuestions", param);
            }
            if ((dsQuestionDetails != null) && (dsQuestionDetails.Tables.Count > 0))
            {
                return dsQuestionDetails.Tables[0];
            }
            return null;
        }

        public DataSet searchQuestion(string searchstr, int questionStatus)
        {
            SqlConnection conn = null;
            DataSet questionList = null;
            SqlParameter[] param = new SqlParameter[2];
            string query = @"select top(10) * from questions where questions.status = @questionStatus and questiontext like @searchstr order by createdate desc";
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@searchstr", "%" + searchstr + "%");
                param[1] = new SqlParameter("@questionStatus", questionStatus);
                questionList = SqlHelper.ExecuteDataset(conn, CommandType.Text, query, param);
            }
            if ((questionList != null) && (questionList.Tables.Count > 0))
            {
                return questionList;
            }
            return null;
        }
        public IList<Question> getAllAssignQuestionsByDoctorid(int userID)
        {
            IList<Question> lstQuestions = new List<Question>();
            SqlConnection conn = null;
            DataSet dsQuestion = null;
            SqlParameter[] param = new SqlParameter[1];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@userID", userID);
                dsQuestion = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "getQuestionListByDoctorid", param);
            }
            Question questions;
            Answer answer;
            if (dsQuestion != null && dsQuestion.Tables.Count > 0 && dsQuestion.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsQuestion.Tables[0].Rows)
                {
                    questions = new Question();
                    answer = new Answer();
                    questions.QuestionId = Convert.ToInt32(dr["questionid"]);
                    questions.QuestionText = Convert.ToString(dr["questiontext"]);
                    questions.CreateDate = Convert.ToDateTime(dr["createdate"]);
                    if (dr["answeredby"] != System.DBNull.Value)
                    {
                        questions.AnsweredBy = Convert.ToInt32(dr["answeredby"]);
                    }
                    if (dr["answertext"] != System.DBNull.Value)
                    {
                        answer.AnswerText = Convert.ToString(dr["answertext"]);
                        questions.answers.Add(answer);
                    }
                    lstQuestions.Add(questions);
                }
            }
            return lstQuestions;
        }
        public int SaveDoctorAnswer(int questionId, int userId, string title, string answer, string filename)
        {
            SqlConnection conn = null;
            int result;
            SqlParameter[] answerParam = new SqlParameter[6];
            using (conn = SqlHelper.GetSQLConnection())
            {
                answerParam[0] = new SqlParameter("@questionId", questionId);
                answerParam[1] = new SqlParameter("@userId", userId);
                answerParam[2] = new SqlParameter("@createDate", System.DateTime.Now);
                answerParam[3] = new SqlParameter("@answer", answer);
                if (!String.IsNullOrEmpty(title))
                {
                    answerParam[4] = new SqlParameter("@title", title);
                }
                else
                {
                    answerParam[4] = new SqlParameter("@title", System.DBNull.Value);
                }
                if (!string.IsNullOrEmpty(filename))
                {
                    answerParam[5] = new SqlParameter("@filename", filename);
                }
                else
                {
                    answerParam[5] = new SqlParameter("@filename", System.DBNull.Value);
                }
                result = SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "answer_Insert", answerParam);
            }
            return result;
        }

        public int addNewTags(string newtag, int questionid)
        {
            SqlParameter[] param = new SqlParameter[2];
            int result = 0;
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@newtag", newtag);
                param[1] = new SqlParameter("@questionid", questionid);
               result = Convert.ToInt32(SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, "add_tag", param));
            }
            return result;
        }

        public int removeTags(string removtag)
        {
            SqlParameter[] param = new SqlParameter[1];
            int result = 0;
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@removtag", removtag);
                result = Convert.ToInt32(SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, "delete_tag", param));
            }
            return result;
        }

        public int assignTagsToQuestion(IList<Tag> ToAddlstTags,IList<Tag> ToDeletelstTags, int questionId)
        {
            DataTable ToAddTagTable = new DataTable();
            DataTable ToDeletetagTable = new DataTable();
            SqlConnection conn = null;
            int result;
            SqlParameter[] parm = new SqlParameter[3];
            using (conn = SqlHelper.GetSQLConnection())
            {

                ToAddTagTable.Columns.Add("tagid", typeof(long));
                foreach (Tag ds in ToAddlstTags)
                {
                    ToAddTagTable.Rows.Add(ds.TagId);
                }
                ToDeletetagTable.Columns.Add("tagid", typeof(long));
                foreach (Tag ds in ToDeletelstTags)
                {
                    ToDeletetagTable.Rows.Add(ds.TagId);
                }
                parm[0] = new SqlParameter("@addtags", ToAddTagTable);
                parm[1] = new SqlParameter("@deletetags", ToDeletetagTable);
                parm[2] = new SqlParameter("@questionId", questionId);
                result = Convert.ToInt32(SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, "assignTagsToQuestion", parm));
            }
            return result;
        }
        public DataSet assignDoctorToQuestion(int questionid, string doctorIds)
        {
            DataSet doctorList = new DataSet();
            SqlConnection conn = null;
            int result;
            SqlParameter[] parm = new SqlParameter[2];
            using (conn = SqlHelper.GetSQLConnection())
            {
                parm[0] = new SqlParameter("@questionid", questionid);
                parm[1] = new SqlParameter("@doctorIds", doctorIds);
                doctorList = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "assignDoctorToQuestion", parm);
            }
            if ((doctorList != null) && (doctorList.Tables.Count > 0))
            {
                return doctorList;
            }
            return null;
        }
        public int RemoveAssignDoctorbyUserID(int userid, int questionid)
        {
            int result;
            SqlConnection conn = null;
            SqlParameter[] parm = new SqlParameter[2];
            using (conn = SqlHelper.GetSQLConnection())
            {
                parm[0] = new SqlParameter("@userid", userid);
                parm[1] = new SqlParameter("@questionid", questionid);
                result = Convert.ToInt32(SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "removeAssignDoctorbyUserID", parm));
            }
            return result;
        }
        public int getUnansweredQuestionCount(int UserId)
        {
            int result;
            SqlConnection conn = null;
            SqlParameter[] parm = new SqlParameter[1];
            using (conn = SqlHelper.GetSQLConnection())
            {
                parm[0] = new SqlParameter("@userid", UserId);
                result = Convert.ToInt32(SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, "getunansweredQuestionCount", parm));
            }
            return result;
        }
        public DataTable RejectQuestionFromQuestionList(int QusetionID, int statusRejected)
        {
            SqlConnection conn = null;
            DataSet dsUserDetails = null;
            DataTable dtUserDetails = null;
            SqlParameter[] param = new SqlParameter[2];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@questionid", QusetionID);
                param[1] = new SqlParameter("@statusid", statusRejected);
                dsUserDetails = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "RejectQuestionFromQuestionList", param);
            }
            if ((dsUserDetails != null) && (dsUserDetails.Tables.Count > 0))
            {
                return dsUserDetails.Tables[0];

            }
            return dtUserDetails;
        }
    }
}