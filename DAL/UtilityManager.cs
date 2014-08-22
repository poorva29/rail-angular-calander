using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class UtilityManager
    {
        private static UtilityManager _instance = new UtilityManager();
        private UtilityManager() { }
        public static UtilityManager getInstance() { return _instance; }
        SqlConnection conn = null;

        public DataTable getAllCountries()
        {
            DataTable dtCountry = null;
            DataSet dsCountry = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                string strquery = "select countryid, name from getAllCountries()";
                dsCountry = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery);
            }
            if ((dsCountry != null) && (dsCountry.Tables.Count > 0))
                dtCountry = dsCountry.Tables[0];
            return dtCountry;
        }
        public DataTable getAllCountriescode()
        {
            DataTable dtCountrycode = null;
            DataSet dsCountrycode = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                string strquery = "SELECT  DISTINCT countrycode from Country order by countrycode";
                dsCountrycode = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery);
            }
            if ((dsCountrycode != null) && (dsCountrycode.Tables.Count > 0))
                dtCountrycode = dsCountrycode.Tables[0];
            return dtCountrycode;
        }
        public DataTable getAllCitiesByStateId(int stateid)
        {
            DataTable dtCity = null;
            DataSet dsCity = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                SqlParameter parm = new SqlParameter("@stateId", stateid);
                string strquery = "select cityid, name from city where stateid=@stateId order by name";
                dsCity = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery, parm);
            }
            if ((dsCity != null) && (dsCity.Tables.Count > 0))
                dtCity = dsCity.Tables[0];
            return dtCity;
        }
        public DataTable getAllLocationByCityId(int cityid)
        {
            DataTable dtLocation = null;
            DataSet dsLocation = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                SqlParameter parm = new SqlParameter("@cityId", cityid);
                string strquery = "select locationid ,name from location where cityid=@cityId order by name";
                dsLocation = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery, parm);
            }
            if ((dsLocation != null) && (dsLocation.Tables.Count > 0))
                dtLocation = dsLocation.Tables[0];
            return dtLocation;
        }
        public DataTable getAllStatesByCountryId(int countryid)
        {
            DataTable dtState = null;
            DataSet dsState = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                SqlParameter parm = new SqlParameter("@countryId", countryid);
                string strquery = "select stateid ,name from state where countryid=@countryId order by name";
                dsState = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery, parm);
            }
            if ((dsState != null) && (dsState.Tables.Count > 0))
                dtState = dsState.Tables[0];
            return dtState;
        }
        public int ActivateEmail(int userID, bool isEmailVerify, string email, out int isLinkActivate)
        {
            SqlParameter[] param = new SqlParameter[4];
            int result = 0;
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@userID", userID);
                SqlParameter paramisLinkActivate = new SqlParameter();
                paramisLinkActivate.ParameterName = "@isLinkActivate";
                paramisLinkActivate.DbType = DbType.String;
                paramisLinkActivate.Size = 10;
                paramisLinkActivate.Direction = ParameterDirection.Output;
                param[1] = paramisLinkActivate;
                param[2] = new SqlParameter("@isemailverify", isEmailVerify);
                param[3] = new SqlParameter("@email", email);
                result = Convert.ToInt32(SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "ActivateAccount", param));
                isLinkActivate = Convert.ToInt32(param[1].Value);
            }
            return isLinkActivate;
        }
        public DataTable getAllCities()
        {
            DataSet dsCities = null;
            DataTable dtCities = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                string strquery = "select distinct * from city order by name";
                dsCities = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery);
            }
            if ((dsCities != null) && (dsCities.Tables.Count > 0))
                dtCities = dsCities.Tables[0];
            return dtCities;
        }
        public DataTable getAllTags()
        {
            DataSet dsTags = null;
            DataTable dtTags = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                string strquery = "select distinct tagname from tag order by tagname";
                dsTags = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery);
            }
            if ((dsTags != null) && (dsTags.Tables.Count > 0))
                dtTags = dsTags.Tables[0];
            return dtTags;
        }

        public DataSet get_allTagsWithCountOfAnsweredQuestions()
        {
            DataSet dsData = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                dsData = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_allTagsWithCountOfAnsweredQuestions");
            }
            if ((dsData != null) && (dsData.Tables.Count > 0))
            {
                return dsData;
            }
            return null;
        }
        public DataTable getAllSpecialities()
        {
            DataTable dtSpecialities = null;
            DataSet dsSpecialities = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                string strquery = "SELECT Specialityid, name from specialities order by name";
                dsSpecialities = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery);
            }
            if ((dsSpecialities != null) && (dsSpecialities.Tables.Count > 0))
                dtSpecialities = dsSpecialities.Tables[0];
            return dtSpecialities;
        }
        public DataTable getAlltags()
        {
            DataTable dtTags = null;
            DataSet dsTags = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                string strquery = "SELECT tagid, tagname from tag order by tagname";
                dsTags = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery);
            }
            if ((dsTags != null) && (dsTags.Tables.Count > 0))
                dtTags = dsTags.Tables[0];
            return dtTags;
        }
        public DataTable getAllRegistrationCouncilbyCountry(int countryId)
        {
            DataTable dtRegCouncil = null;
            DataSet dsRegCouncil = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                string strquery = "SELECT regcouncilid, name from registrationcouncil where countryid = @countryId order by name";
                SqlParameter regCouncilParam = new SqlParameter("@countryId", countryId);
                dsRegCouncil = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery, regCouncilParam);
            }
            if ((dsRegCouncil != null) && (dsRegCouncil.Tables.Count > 0))
                dtRegCouncil = dsRegCouncil.Tables[0];
            return dtRegCouncil;
        }
        public DataSet getAllDegree()
        {
            DataSet dsdegree = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                string strquery = "SELECT degreeid, name from degree order by name";
                dsdegree = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery);
            }
            return dsdegree;
        }
        public DataTable gelAllStatesbyCountry(int countryId)
        {
            DataSet dsStates = null;

            using (conn = SqlHelper.GetSQLConnection())
            {
                string strquery = "select distinct stateid, name from state where countryid = @countryId order by name";
                SqlParameter cityParam = new SqlParameter("@countryId", countryId);
                dsStates = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery, cityParam);
            }
            if ((dsStates != null) && (dsStates.Tables.Count > 0))
            {
                return dsStates.Tables[0];
            }
            return null;
        }
        public DataSet getCountryStateCityLocation(int countryId, int stateId, int cityId, int locationId)
        {
            DataSet dsData = null;
            SqlParameter[] param = new SqlParameter[4];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@countryId", countryId);
                param[1] = new SqlParameter("@stateId", stateId);
                param[2] = new SqlParameter("@cityId", cityId);
                param[3] = new SqlParameter("@locationId", locationId);
                dsData = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "getCountryStateCityLocation", param);
            }
            if ((dsData != null) && (dsData.Tables.Count > 0))
            {
                return dsData;
            }
            return null;
        }

        public int insertHomePageImages(string image, string homePageImage)
        {
            SqlParameter[] param = new SqlParameter[2];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@image", image);
                param[1] = new SqlParameter("@homePageImage", homePageImage);

                int result = SqlHelper.ExecuteNonQuery(conn, "insert_HompePageImage", param);
                return result;
            }
        }

        public DataTable getHomePageImages()
        {
            DataTable dtImg = null;
            DataSet dsImg = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                dsImg = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_HomePageImagePath");
            }
            if ((dsImg != null) && (dsImg.Tables.Count > 0))
                dtImg = dsImg.Tables[0];
            return dtImg;
        }
    }
}
