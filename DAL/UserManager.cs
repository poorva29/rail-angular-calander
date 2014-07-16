using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Model;

namespace DAL
{
    public class UserManager
    {
        private static UserManager _instance = new UserManager();
        private UserManager() { }
        public static UserManager getInstance() { return _instance; }
        SqlConnection conn = null;
        public User GetActiveUserByEmailId(string emailId)
        {
            User user = new User();
            DataSet dsUser = null;
            DataRow drUser = null;
            SqlParameter[] param = new SqlParameter[1];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@emailId", emailId);
                dsUser = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_UserbyEmailId", param);
                if (dsUser != null && dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count > 0)
                {
                    drUser = dsUser.Tables[0].Rows[0];
                    user.UserId = drUser["userid"] != DBNull.Value ? Convert.ToInt32(drUser["userid"]) : 0;
                    user.FirstName = drUser["firstname"] != DBNull.Value ? Convert.ToString(drUser["firstname"]) : "";
                    user.LastName = drUser["lastname"] != DBNull.Value ? Convert.ToString(drUser["lastname"]) : "";
                    user.UserName = drUser["username"] != DBNull.Value ? Convert.ToString(drUser["username"]) : "";
                    user.Password = drUser["password"] != DBNull.Value ? Convert.ToString(drUser["password"]) : "";
                    user.Email = drUser["email"] != DBNull.Value ? Convert.ToString(drUser["email"]) : "";
                    if (drUser["status"] !=DBNull.Value) user.Status = Convert.ToInt32(drUser["status"]);
                    user.IsEmailVerified=drUser["isemailverified"] != DBNull.Value && Convert.ToBoolean(drUser["isemailverified"]) ? true : false;
                    user.LocationId = drUser["locationid"] != DBNull.Value ? Convert.ToInt32(drUser["locationid"]) : 0;
                    user.CityId = drUser["cityid"] != DBNull.Value ? Convert.ToInt32(drUser["cityid"]) : 0;
                    if (drUser["userType"] != DBNull.Value) user.UserType = Convert.ToInt32(drUser["userType"]);
                    user.IsDocConnectUser = drUser["isdocconnectuser"] != DBNull.Value && Convert.ToBoolean(drUser["isdocconnectuser"]) ? true : false;
                }
            }
            return user;
        }
        public DataTable registerPatient(User patient)
        {
            SqlParameter[] param = new SqlParameter[19];
            DataSet dsPatient = null;
            DataTable dtPatient = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@firstname", patient.FirstName);
                param[1] = new SqlParameter("@lastname", patient.LastName);
                param[2] = new SqlParameter("@email", patient.Email);
                if (patient.MobileNo != null)
                {
                    param[3] = new SqlParameter("@mobileno", patient.MobileNo);
                }
                else 
                {
                    param[3] = new SqlParameter("@mobileno", System.DBNull.Value);
                }
                param[4] = new SqlParameter("@Gender", patient.Gender);
                if (patient.DateOfBirth != null)
                    param[5] = new SqlParameter("@dateofbirth", patient.DateOfBirth);
                else
                    param[5] = new SqlParameter("@dateofbirth", System.DBNull.Value);

                if (patient.CountryId != 0)
                {
                    param[6] = new SqlParameter("@countryid", patient.CountryId);
                }
                else
                {
                    param[6] = new SqlParameter("@countryid", System.DBNull.Value);
                }
                if (patient.StateId != 0)
                {
                    param[7] = new SqlParameter("@stateid", patient.StateId);
                }
                else
                {
                    param[7] = new SqlParameter("@stateid", System.DBNull.Value);
                }
                if (patient.LocationId != 0)
                {
                    param[8] = new SqlParameter("@locationid", patient.LocationId);
                }
                else
                {
                    param[8] = new SqlParameter("@locationid", System.DBNull.Value);
                }
                if (patient.CityId != 0)
                {
                    param[9] = new SqlParameter("@cityid", patient.CityId);
                }
                else
                {
                    param[9] = new SqlParameter("@cityid", System.DBNull.Value);
                }
                param[10] = new SqlParameter("@password", patient.Password);

                if (patient.Height != null)
                {
                    param[11] = new SqlParameter("@height", patient.Height);
                }
                else
                {
                    param[11] = new SqlParameter("@height", System.DBNull.Value);
                }

                if (patient.Weight != null)
                {
                    param[12] = new SqlParameter("@weight", patient.Weight);
                }
                else
                {
                    param[12] = new SqlParameter("@weight", System.DBNull.Value);
                }
                if (!String.IsNullOrEmpty(patient.Address))
                {
                    param[13] = new SqlParameter("@address", patient.Address);
                }
                else
                {
                    param[13] = new SqlParameter("@address", System.DBNull.Value);
                }
                if (patient.Pincode != null)
                {
                    param[14] = new SqlParameter("@pincode", patient.Pincode);
                }
                else
                {
                    param[14] = new SqlParameter("@pincode", System.DBNull.Value);
                }
                param[15] = new SqlParameter("@registrationdate", DateTime.Now);
                param[16] = new SqlParameter("@status", patient.Status);
                param[17] = new SqlParameter("@Usertype", patient.UserType);
                param[18] = new SqlParameter("@username", patient.Email);
                dsPatient = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "askmirai_patient_Insert_Update", param);
                if (dsPatient != null && dsPatient.Tables.Count > 0)
                {
                    dtPatient = dsPatient.Tables[0];
                }
                return dtPatient;
            }
        }

        public int getUserIdAndTypeByEmail(string email, out string firstName)
        {
            SqlParameter[] param = new SqlParameter[2];
            int userid = 0;
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@emailId", email);
                SqlParameter paramusername = new SqlParameter();
                paramusername.ParameterName = "@userFirstName";
                paramusername.DbType = DbType.String;
                paramusername.Size = 50;
                paramusername.Direction = ParameterDirection.Output;
                param[1] = paramusername;
                userid = Convert.ToInt32(SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, "askmirai_get_UserIdandTypebyEmail", param));
                firstName = Convert.ToString(param[1].Value);
            }
            return userid;
        }

        public string getUserRecord(string userid)
        {
            SqlParameter[] param = new SqlParameter[1];
            string currentPassword = "";
            string strQuery = "SELECT password from users where userid = @userId";
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@userId", userid);
                currentPassword = Convert.ToString(SqlHelper.ExecuteScalar(conn, CommandType.Text, strQuery, param));
            }
            return currentPassword;
        }

        public int setNewPassword(string userid, string newpassword)
        {
            SqlParameter[] param = new SqlParameter[2];
            int result;
            string strQuery = "UPDATE  users SET password = @newPassword WHERE userid= @userId";
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@userId", userid);
                param[1] = new SqlParameter("@newPassword", newpassword);
                result = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strQuery, param);
            }
            return result;
        }

        public DataTable getDoctorListByCriteria(string cityid, string locationid, string specialityOrName, int userStatus)
        {
            DataSet doctorList = null;
            SqlParameter[] param = new SqlParameter[4];
            using (conn = SqlHelper.GetSQLConnection())
            {
                if (!String.IsNullOrEmpty(cityid))
                {
                    param[0] = new SqlParameter("@cityid", Convert.ToInt32(cityid));
                }
                else
                {
                    param[0] = new SqlParameter("@cityid", System.DBNull.Value);
                }
                if (!String.IsNullOrEmpty(locationid))
                {
                    param[1] = new SqlParameter("@locationid", Convert.ToInt32(locationid));
                }
                else
                {
                    param[1] = new SqlParameter("@locationid", System.DBNull.Value);
                }
                if (!string.IsNullOrEmpty(specialityOrName))
                {
                    param[2] = new SqlParameter("@specialityOrName", specialityOrName);
                }
                else
                {
                    param[2] = new SqlParameter("@specialityOrName", System.DBNull.Value);
                }
                param[3] = new SqlParameter("@userStatus", userStatus);
                doctorList = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "getDoctorListByLoactionNameCitySpeciality", param);
            }
            if ((doctorList != null) && (doctorList.Tables.Count > 0))
            {
                return doctorList.Tables[0];
            }
            return null;
        }


        public int insertDocconnectDoctor(User user)
        {
            SqlParameter[] param = new SqlParameter[9];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@userid", user.UserId);
                param[1] = new SqlParameter("@username", user.UserName);
                param[2] = new SqlParameter("@password", user.Password);
                param[3] = new SqlParameter("@firstname", user.FirstName);
                param[4] = new SqlParameter("@lastname", user.LastName);
                param[5] = new SqlParameter("@emailid", user.Email);
                param[6] = new SqlParameter("@mobileno", user.MobileNo);
                param[7] = new SqlParameter("@gender", user.Gender);
                param[8] = new SqlParameter("@docconnectdoctorid", user.DocConnectDoctorId);
                Object obj = SqlHelper.ExecuteScalar(conn, "Docconnect_doctor_InsertUpdate", param);
                return Convert.ToInt32(obj);
            }
        }

        public int updateAppointmentclickedcnt(int userid)
        {
            int retVal = 0;
            SqlParameter[] param = new SqlParameter[1];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@userid", userid);
                retVal = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, "update users set users.appointmentbuttonhits = isnull(users.appointmentbuttonhits,0) + 1 where users.userid = @userid", param);
            }
            return retVal;
        }
    }
}
   
