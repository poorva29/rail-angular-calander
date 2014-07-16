using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Model;
namespace DAL
{
    public class FeedManager
    {
        private static FeedManager _instance = new FeedManager();
        private FeedManager() { }
        public static FeedManager getInstance() { return _instance; }
        SqlConnection conn = null;
        public DataTable getFeedListByLastQuestionNo(int lastQuestionNo, int RecordSize, int UserId, int myFeed, int questionStatus)
        {
            DataSet feedList = null;
            SqlParameter[] param = new SqlParameter[5];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@lastQuestionNo", lastQuestionNo);
                param[1] = new SqlParameter("@RecordSize", RecordSize);
                param[2] = new SqlParameter("@UserId", UserId);
                param[3] = new SqlParameter("@myFeed", myFeed);
                param[4] = new SqlParameter("@questionStatus", questionStatus);
                feedList = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "getFeedListByLastQuestionNo", param);
            }
            if ((feedList != null) && (feedList.Tables.Count > 0))
            {
                return feedList.Tables[0];
            }
            return null;
        }
        public int insertPatientThankToAnswer(int userid, int answerid)
        {
            SqlParameter[] param = new SqlParameter[2];
            int result = 0;
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@userid", userid);
                param[1] = new SqlParameter("@answerid", answerid);
                string strquery = @"insert into patientthanks(userid, answerid) values(@userid, @answerid)";
                result = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strquery, param);
            }
            return result;
        }
        public int insertDoctorEndorseToAnswer(int userid, int answerid)
        {
            SqlParameter[] param = new SqlParameter[2];
            int result = 0;
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@userid", userid);
                param[1] = new SqlParameter("@answerid", answerid);
                string strquery = @"insert into doctoragrees(doctorid, answerid) values(@userid, @answerid)";
                result = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strquery, param);
            }
            return result;
        }
    }
}
