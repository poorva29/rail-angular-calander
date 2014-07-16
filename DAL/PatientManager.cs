using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Model;

namespace DAL
{
    public class PatientManager 
    {
        private static PatientManager _instance = new PatientManager();
        private PatientManager() { }
        public static PatientManager getInstance() { return _instance; }
        SqlConnection conn = null;

        public User getPatientDetailsByPatientId(int userid)
        {
            DataSet dsPatientDetails = null;
            DataRow drPatient = null;
            User patient = new User();
            SqlParameter[] patientParam = new SqlParameter[1];
            patientParam[0] = new SqlParameter("@userid", userid);
            using (conn = SqlHelper.GetSQLConnection())
            {
                string strquery = "select * from users where userid=@userid";
                dsPatientDetails = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery, patientParam);
                if (dsPatientDetails != null && dsPatientDetails.Tables.Count > 0 && dsPatientDetails.Tables[0].Rows.Count > 0)
                {
                    drPatient = dsPatientDetails.Tables[0].Rows[0];
                }
            }
            if (drPatient != null)
            {
                patient.UserId = Convert.ToInt32(drPatient["userid"]);
                patient.FirstName = Convert.ToString(drPatient["firstname"]);
                patient.LastName = Convert.ToString(drPatient["lastname"]);
                patient.Email = Convert.ToString(drPatient["email"]);
                patient.MobileNo = Convert.ToString(drPatient["mobileno"]);
                patient.Gender = Convert.ToInt32(drPatient["gender"]);
                patient.Address = Convert.ToString(drPatient["address"]);
                if (drPatient["height"] != System.DBNull.Value)
                {
                    patient.Height = Convert.ToInt32(drPatient["height"]);
                }
                if (drPatient["weight"] != System.DBNull.Value)
                {
                    patient.Weight = Convert.ToDecimal(drPatient["weight"]);
                }
                if (drPatient["pincode"] != System.DBNull.Value)
                {
                    patient.Pincode = Convert.ToInt32(drPatient["pincode"]);
                }
                if (drPatient["dateofbirth"] != System.DBNull.Value)
                {
                    patient.DateOfBirth = Convert.ToDateTime(drPatient["dateofbirth"]);
                }
                if (!String.IsNullOrEmpty(Convert.ToString(drPatient["countryid"])))
                {
                    patient.CountryId = Convert.ToInt32(drPatient["countryid"]);
                }
                if (!String.IsNullOrEmpty(Convert.ToString(drPatient["stateid"])))
                {
                    patient.StateId = Convert.ToInt32(drPatient["stateid"]);
                }
                if (!String.IsNullOrEmpty(Convert.ToString(drPatient["cityid"])))
                {
                    patient.CityId = Convert.ToInt32(drPatient["cityid"]);
                }
                if (!String.IsNullOrEmpty(Convert.ToString(drPatient["locationid"])))
                {
                    patient.LocationId = Convert.ToInt32(drPatient["locationid"]);
                }
                patient.UserName = Convert.ToString(drPatient["username"]);
                if (!String.IsNullOrEmpty(Convert.ToString(drPatient["registrationdate"])))
                {
                    patient.RegistrationDate = Convert.ToDateTime(drPatient["registrationdate"]);
                }
                if (!String.IsNullOrEmpty(Convert.ToString(drPatient["status"])))
                {
                    patient.Status = Convert.ToInt32(drPatient["status"]);
                }
                if (!String.IsNullOrEmpty(Convert.ToString(drPatient["isemailverified"])))
                {
                    patient.IsEmailVerified = Convert.ToBoolean(drPatient["isemailverified"]);
                }
            }
            return patient;
        }
        public DataTable updatePatientDetails(User patient)
        {
            SqlParameter[] param = new SqlParameter[21];
            DataSet dsPatient = null;
            DataTable dtPatient = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@firstname", patient.FirstName);
                param[1] = new SqlParameter("@lastname", patient.LastName);
                param[2] = new SqlParameter("@email", patient.Email);
                param[3] = new SqlParameter("@mobileno", patient.MobileNo);
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

                if (Convert.ToString(patient.Height) != null)
                {
                    param[11] = new SqlParameter("@height", patient.Height);
                }
                else
                {
                    param[11] = new SqlParameter("@height", System.DBNull.Value);
                }

                if (Convert.ToString(patient.Weight) != null)
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
                if (Convert.ToString(patient.Pincode) != null)
                {
                    param[14] = new SqlParameter("@pincode", patient.Pincode);
                }
                else
                {
                    param[14] = new SqlParameter("@pincode", System.DBNull.Value);
                }
                param[15] = new SqlParameter("@registrationdate", DateTime.Now);
                param[16] = new SqlParameter("@status", patient.Status);
                param[17] = new SqlParameter("@Usertype", 2);
                param[18] = new SqlParameter("@username", patient.Email);
                param[19] = new SqlParameter("@userid", patient.UserId);
                param[20] = new SqlParameter("@emailverified", patient.IsEmailVerified);
                dsPatient = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "askmirai_patient_Insert_Update", param);
                if (dsPatient != null && dsPatient.Tables.Count > 0)
                {
                    dtPatient = dsPatient.Tables[0];
                }
                return dtPatient;
            }
        }
    }
}
