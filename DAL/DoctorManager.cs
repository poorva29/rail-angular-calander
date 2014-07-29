using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Model;

namespace DAL
{
    public class DoctorManager
    {
        private static DoctorManager _instance = new DoctorManager();
        private DoctorManager() { }
        public static DoctorManager getInstance() { return _instance; }
        SqlConnection conn = null;

        public DataTable registerdoctor(User doctor)
        {
            SqlConnection conn = null;
            DataTable dtDoctor = null;
            DataSet dsDoctor = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                SqlParameter[] parm = new SqlParameter[18];
                parm[0] = new SqlParameter("@firstname", doctor.FirstName);
                parm[1] = new SqlParameter("@lastname", doctor.LastName);
                parm[2] = new SqlParameter("@gender", doctor.Gender);
                if (doctor.DateOfBirth != null)
                    parm[3] = new SqlParameter("@dateofbirth", doctor.DateOfBirth);
                else
                    parm[3] = new SqlParameter("@dateofbirth", System.DBNull.Value);
                parm[4] = new SqlParameter("@email", doctor.Email);
                parm[5] = new SqlParameter("@mobileno", doctor.MobileNo);
                parm[6] = new SqlParameter("@password", doctor.Password);
                if (!String.IsNullOrEmpty(doctor.Image))
                    parm[7] = new SqlParameter("@photopath", doctor.Image);
                else
                    parm[7] = new SqlParameter("@photopath", System.DBNull.Value);

                parm[8] = new SqlParameter("@status", doctor.Status);
                parm[9] = new SqlParameter("@userType", doctor.UserType);
                parm[10] = new SqlParameter("@registrationdate", DateTime.Now);
                DataTable specialitiesTable = CreateDataTable(doctor.specialities, "speciality_id");
                DataTable locationTable = CreateDataTable(doctor.locations);
                parm[11] = new SqlParameter("@specialities", specialitiesTable);
                parm[11].SqlDbType = SqlDbType.Structured;
                if (doctor.CountryId != 0)
                {
                    parm[12] = new SqlParameter("@countryId", doctor.CountryId);
                }
                else
                {
                    parm[12] = new SqlParameter("@countryId", System.DBNull.Value);
                }
                if (!String.IsNullOrEmpty(doctor.RegistrationNumber))
                    parm[13] = new SqlParameter("@registrationnumber", doctor.RegistrationNumber);
                else
                    parm[13] = new SqlParameter("@registrationnumber", System.DBNull.Value);
                if (doctor.RegistrationCouncil != 0)
                    parm[14] = new SqlParameter("@registrationcouncil", doctor.RegistrationCouncil);
                else
                    parm[14] = new SqlParameter("@registrationcouncil", System.DBNull.Value);
                if (!String.IsNullOrEmpty(doctor.AboutMe))
                    parm[15] = new SqlParameter("@aboutme", doctor.AboutMe);
                else
                    parm[15] = new SqlParameter("@aboutme", System.DBNull.Value);
                parm[16] = new SqlParameter("@username", doctor.Email);
                if (!String.IsNullOrEmpty(doctor.PhotoUrl))
                    parm[17] = new SqlParameter("@photourl", doctor.PhotoUrl);
                else
                    parm[17] = new SqlParameter("@photourl", System.DBNull.Value);
                dsDoctor = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "askmirai_doctor_Insert", parm);
                if (dsDoctor != null && dsDoctor.Tables.Count > 0)
                {
                    dtDoctor = dsDoctor.Tables[0];
                }
                return dtDoctor;
            }
        }

        private static DataTable CreateDataTable(IList<DoctorSpecialities> specialityIds, string columnName)
        {
            DataTable table = new DataTable();
            table.Columns.Add(columnName, typeof(long));
            foreach (DoctorSpecialities ds in specialityIds)
            {
                table.Rows.Add(ds.SpecialityId);
            }
            return table;
        }
        private static DataTable CreateDataTable(IList<DoctorLocations> locationIds)
        {
            DataTable table = new DataTable();
            table.Columns.Add("country_id", typeof(long));
            table.Columns.Add("state_id", typeof(long));
            table.Columns.Add("city_id", typeof(long));
            table.Columns.Add("loction_id", typeof(long));
            foreach (DoctorLocations doclocation in locationIds)
            {
                table.Rows.Add(doclocation.CountryId, doclocation.StateId, doclocation.CityId, doclocation.LocationId);
            }
            return table;
        }
        public DataTable UpdatedoctordetailById(User doctor)
        {
            DataTable dtDoctor = null;
            DataSet dsDoctor = null;
            SqlConnection conn = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                SqlParameter[] parm = new SqlParameter[16];
                parm[0] = new SqlParameter("@doctorid", doctor.UserId);
                parm[1] = new SqlParameter("@firstname", doctor.FirstName);
                parm[2] = new SqlParameter("@lastname", doctor.LastName);
                parm[3] = new SqlParameter("@email", doctor.Email);
                parm[4] = new SqlParameter("@mobileno", doctor.MobileNo);
                parm[5] = new SqlParameter("@gender", doctor.Gender);
                if (doctor.DateOfBirth != null)
                    parm[6] = new SqlParameter("@dateofbirth", doctor.DateOfBirth);
                else
                    parm[6] = new SqlParameter("@dateofbirth", System.DBNull.Value);
                if (!String.IsNullOrEmpty(doctor.Image))
                    parm[7] = new SqlParameter("@photopath", doctor.Image);
                else
                    parm[7] = new SqlParameter("@photopath", System.DBNull.Value);
                if (!String.IsNullOrEmpty(doctor.RegistrationNumber))
                    parm[8] = new SqlParameter("@registrationnumber", doctor.RegistrationNumber);
                else
                    parm[8] = new SqlParameter("@registrationnumber", System.DBNull.Value);
                if (doctor.RegistrationCouncil != 0)
                    parm[9] = new SqlParameter("@registrationcouncil", doctor.RegistrationCouncil);
                else
                    parm[9] = new SqlParameter("@registrationcouncil", System.DBNull.Value);
                if (!String.IsNullOrEmpty(doctor.AboutMe))
                    parm[10] = new SqlParameter("@aboutme", doctor.AboutMe);
                else
                    parm[10] = new SqlParameter("@aboutme", System.DBNull.Value);
                parm[11] = new SqlParameter("@username", doctor.UserName);
                DataTable specialitiesTable = CreateDataTable(doctor.specialities, "speciality_id");
                parm[12] = new SqlParameter("@specialities", specialitiesTable);
                parm[12].SqlDbType = SqlDbType.Structured;
                parm[13] = new SqlParameter("@isemailverified", doctor.IsEmailVerified);
                if(doctor.CountryId != 0)
                    parm[14] = new SqlParameter("@countryId", doctor.CountryId);
                else
                    parm[14] = new SqlParameter("@countryId", System.DBNull.Value);
                if (!String.IsNullOrEmpty(doctor.PhotoUrl))
                    parm[15] = new SqlParameter("@photourl", doctor.PhotoUrl);
                else
                    parm[15] = new SqlParameter("@photourl", System.DBNull.Value);
                dsDoctor = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "askmirai_doctor_Update", parm);
                if (dsDoctor != null && dsDoctor.Tables.Count > 0)
                {
                    dtDoctor = dsDoctor.Tables[0];
                }
                return dtDoctor;
            }
        }
        public User getDoctorDetailsById(int DocId)
        {
            IList<User> lstdoctors = new List<User>();
            DataSet dsDoctorDetails = null;
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@DOCID", DocId);
            using (conn = SqlHelper.GetSQLConnection())
            {
                dsDoctorDetails = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "askmirai_get_alldoctorsdetails", param);
            }
            lstdoctors = populateDoctorDetails(dsDoctorDetails);

            if (lstdoctors != null && lstdoctors.Count != 0)
                return lstdoctors[0];
            else
                return null;
        }
        private static IList<User> populateDoctorDetails(DataSet dsDoctorDetails)
        {
            IList<User> lstdoctors = new List<User>();
            User doctor;
            int doctorid;
            DataView dvdocspecialities, dvdoctorlocations, dvdoctorqualifications, dvdoctorsdetails;
            DataRow[] datarows;
            if (dsDoctorDetails != null && dsDoctorDetails.Tables.Count > 0 && dsDoctorDetails.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsDoctorDetails.Tables[0].Rows)
                {
                    doctor = new User();

                    doctorid = Convert.ToInt32(dr["userid"]);
                    doctor.UserId = doctorid;
                    doctor.FirstName = Convert.ToString(dr["firstname"]);
                    doctor.LastName = Convert.ToString(dr["lastname"]);
                    doctor.Email = Convert.ToString(dr["email"]);
                    doctor.MobileNo = Convert.ToString(dr["mobileno"]);
                    doctor.Gender = Convert.ToInt32(dr["gender"]);
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["dateofbirth"])))
                    {
                        doctor.DateOfBirth = Convert.ToDateTime(dr["dateofbirth"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["photopath"])))
                    {
                        doctor.Image = Convert.ToString(dr["photopath"]);
                    }
                    doctor.UserName = Convert.ToString(dr["username"]);
                    doctor.Password = Convert.ToString(dr["password"]);
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["isemailverified"])))
                    {
                        doctor.IsEmailVerified = Convert.ToBoolean(dr["isemailverified"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["status"])))
                    {
                        doctor.Status = Convert.ToInt32(dr["status"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["registrationdate"])))
                    {
                        doctor.RegistrationDate = Convert.ToDateTime(dr["registrationdate"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["registrationnumber"])))
                    {
                        doctor.RegistrationNumber = Convert.ToString(dr["registrationnumber"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["registrationcouncil"])))
                    {
                        doctor.RegistrationCouncil = Convert.ToInt32(dr["registrationcouncil"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["aboutme"])))
                    {
                        doctor.AboutMe = Convert.ToString(dr["aboutme"]);
                    }
                    if(!String.IsNullOrEmpty(Convert.ToString(dr["countryid"])))
                    {
                        doctor.CountryId = Convert.ToInt32(dr["countryid"]);
                    }
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["photourl"])))
                    {
                        doctor.PhotoUrl = Convert.ToString(dr["photourl"]);
                    }
                    if (dsDoctorDetails.Tables.Count == 5)
                    {
                        dvdocspecialities = new DataView(dsDoctorDetails.Tables[1]);
                        dvdoctorlocations = new DataView(dsDoctorDetails.Tables[2]);
                        dvdoctorqualifications = new DataView(dsDoctorDetails.Tables[3]);
                        dvdoctorsdetails = new DataView(dsDoctorDetails.Tables[4]);
                        string expression = "userid =" + doctorid;
                        string sortOrder = "";
                        datarows = dvdocspecialities.Table.Select(expression, sortOrder);
                        //foreach (DataRow dr1 in datarows)
                        //{
                        //    DoctorSpecialities doctorspeciality = new DoctorSpecialities();
                        //    if (!String.IsNullOrEmpty(Convert.ToString(dr1["specialityid"])))
                        //    {
                        //        doctorspeciality.SpecialityId = Convert.ToInt32(dr1["specialityid"]);
                        //    }
                        //    if (!String.IsNullOrEmpty(Convert.ToString(dr1["speciality_name"])))
                        //    {
                        //        doctorspeciality.Speciality = Convert.ToString(dr1["speciality_name"]);

                        //    }
                        //    doctor.specialities.Add(doctorspeciality);
                        //}
                        datarows = dvdoctorlocations.Table.Select(expression, sortOrder);
                        bool isloc_already_added = false;

                        foreach (DataRow drlocation in datarows)
                        {
                            DoctorLocations doctorlocation = new DoctorLocations();
                            isloc_already_added = doctor.locations.Any(e => e.DoctorLocationId == Convert.ToInt32(drlocation["doctorlocationid"]));
                            if (!isloc_already_added)
                            {
                                doctorlocation.DoctorLocationId = Convert.ToInt32(drlocation["doctorlocationid"]);
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["countryid"])))
                                {
                                    doctorlocation.CountryId = Convert.ToInt32(drlocation["countryid"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["country_name"])))
                                {
                                    doctorlocation.Country = Convert.ToString(drlocation["country_name"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["stateid"])))
                                {
                                    doctorlocation.StateId = Convert.ToInt32(drlocation["stateid"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["state_name"])))
                                {
                                    doctorlocation.State = Convert.ToString(drlocation["state_name"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["cityid"])))
                                {
                                    doctorlocation.CityId = Convert.ToInt32(drlocation["cityid"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["city_name"])))
                                {
                                    doctorlocation.City = Convert.ToString(drlocation["city_name"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["locationid"])))
                                {
                                    doctorlocation.LocationId = Convert.ToInt32(drlocation["locationid"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["clinicname"])))
                                {
                                    doctorlocation.ClinicName = Convert.ToString(drlocation["clinicname"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["location_name"])))
                                {
                                    doctorlocation.Location = Convert.ToString(drlocation["location_name"]);
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(drlocation["telephone"])))
                                {
                                    doctorlocation.Telephone = Convert.ToString(drlocation["telephone"]);
                                }
                                if (!String.IsNullOrEmpty(drlocation["address"].ToString()))
                                {
                                    doctorlocation.Address = drlocation["address"].ToString();
                                }
                                doctor.locations.Add(doctorlocation);
                            }
                        }
                        datarows = dvdoctorqualifications.Table.Select(expression, sortOrder);
                        foreach (DataRow dr1 in datarows)
                        {
                            doctorqualification doctorqualification = new doctorqualification();
                            if (!String.IsNullOrEmpty(Convert.ToString(dr1["degreeid"])))
                            {
                                doctorqualification.DegreeId = Convert.ToInt32(dr1["degreeid"]);
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(dr1["degree_name"])))
                            {
                                doctorqualification.Degree = Convert.ToString(dr1["degree_name"]);
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(dr1["university"])))
                            {
                                doctorqualification.University = Convert.ToString(dr1["university"]);

                            }
                            doctor.qualification.Add(doctorqualification);
                        }
                        datarows = dvdoctorsdetails.Table.Select(expression, sortOrder);
                        foreach (DataRow drdoctorsdetails in datarows)
                        {
                            doctordetails doctordetails = new doctordetails();
                            if (!String.IsNullOrEmpty(Convert.ToString(drdoctorsdetails["docdetailsid"])))
                            {
                                doctordetails.DocDetailsId = Convert.ToInt32(drdoctorsdetails["docdetailsid"]);
                            }

                            if (!String.IsNullOrEmpty(Convert.ToString(drdoctorsdetails["userid"])))
                            {
                                doctordetails.UserId = Convert.ToInt32(drdoctorsdetails["userid"]);
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(drdoctorsdetails["certification"])))
                            {
                                doctordetails.Certification = Convert.ToString(drdoctorsdetails["certification"]);
                            }
                            if (!String.IsNullOrEmpty(Convert.ToString(drdoctorsdetails["society"])))
                            {
                                doctordetails.Society = Convert.ToString(drdoctorsdetails["society"]);
                            }
                            doctor.details.Add(doctordetails);
                        }
                        lstdoctors.Add(doctor);
                    }
                }
            }
            return lstdoctors;
        }
        public int UpdateDegreeUniversityByDoctorAndDegrreId(string doctorId, string LastSelectedDegreeID, string SelectedDegreeId, string university)
        {
            SqlConnection conn = null;
            int result = 0;
            using (conn = SqlHelper.GetSQLConnection())
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@doctorid", Convert.ToInt32(doctorId));
                if (!String.IsNullOrEmpty(LastSelectedDegreeID))
                {
                    param[1] = new SqlParameter("@lastdegreeiD", Convert.ToInt32(LastSelectedDegreeID));
                }
                else
                {
                    param[1] = new SqlParameter("@lastdegreeiD", System.DBNull.Value);
                }
                param[2] = new SqlParameter("@selecteddegreeid", Convert.ToInt32(SelectedDegreeId));
                if (!String.IsNullOrEmpty(university))
                {
                    param[3] = new SqlParameter("@University", university);
                }
                else
                {
                    param[3] = new SqlParameter("@University", System.DBNull.Value);
                }
                param[4] = new SqlParameter("@otherdegree", System.DBNull.Value);
                result = Convert.ToInt32(SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, "doctorqualification_Update", param));
            }
            return result;
        }
        public int DeleteDegreeUniversityByDoctorAndDegrreId(string doctorId, string LastSelectedDegreeID, string university)
        {
            int result = 0;
            SqlParameter[] param = new SqlParameter[3];
            using (conn = SqlHelper.GetSQLConnection())
            {
                if (!String.IsNullOrEmpty(doctorId))
                {
                    param[0] = new SqlParameter("@userid", Convert.ToInt32(doctorId));
                }
                else
                {
                    param[0] = new SqlParameter("@userid", System.DBNull.Value);
                }
                if (!String.IsNullOrEmpty(LastSelectedDegreeID))
                {
                    param[1] = new SqlParameter("@lastdegreeiD", Convert.ToInt32(LastSelectedDegreeID));
                }
                else
                {
                    param[1] = new SqlParameter("@lastdegreeiD", System.DBNull.Value);
                }
                if (!String.IsNullOrEmpty(university))
                {
                    param[2] = new SqlParameter("@University", university);
                }
                else
                {
                    param[2] = new SqlParameter("@University", System.DBNull.Value);
                }
                string strquery;
                if (!String.IsNullOrEmpty(university))
                {
                     strquery = "DELETE FROM doctorqualification WHERE doctorid = @userid and degreeid = @lastdegreeiD and university=@University";
                }
                else
                {
                    strquery = "DELETE FROM doctorqualification WHERE doctorid = @userid and degreeid = @lastdegreeiD and university IS NULL";
                }
                result = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strquery, param);
            }
            return result;
        }
        public int UpdateDoctorDetailsByDoctorDetailsId(string doctorId, string doctordetailsid, string certification, string society)
        {
            SqlConnection conn = null;
            int result = 0;

            using (conn = SqlHelper.GetSQLConnection())
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@userid", Convert.ToInt32(doctorId));
                if (!String.IsNullOrEmpty(doctordetailsid))
                {
                    param[1] = new SqlParameter("@doctordetailsid", Convert.ToInt32(doctordetailsid));
                }
                else
                {
                    param[1] = new SqlParameter("@doctordetailsid", System.DBNull.Value);
                }
                param[2] = new SqlParameter("@certification", Convert.ToString(certification));
                if (!String.IsNullOrEmpty(society))
                {
                    param[3] = new SqlParameter("@society", society);
                }
                else
                {
                    param[3] = new SqlParameter("@society", System.DBNull.Value);
                }

                result = Convert.ToInt32(SqlHelper.ExecuteScalar(conn, CommandType.StoredProcedure, "askmirai_doctorsdetails_Update", param));

            }
            return result;
        }
        public int DeleteDoctorDetailsByDoctorDetailsId(string doctordetailsid)
        {
            SqlConnection conn = null;
            int result = 0;
            SqlParameter param;
            using (conn = SqlHelper.GetSQLConnection())
            {
                param = new SqlParameter("@Doctordetailsid", Convert.ToInt32(doctordetailsid));
                string strquery = "DELETE FROM dbo.doctorsdetails WHERE doctordetailsid = @Doctordetailsid";
                result = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strquery, param);
            }
            return result;
        }
        public int DeleteDoctorLocattonByDoctorLocationId(string doctorloctionid)
        {
            SqlConnection conn = null;
            int result = 0;
            SqlParameter[] param = new SqlParameter[1];
            using (conn = SqlHelper.GetSQLConnection())
            {
                param[0] = new SqlParameter("@Doctorloctionid", Convert.ToInt32(doctorloctionid));
                string strquery = "DELETE FROM dbo.doctorlocations WHERE doctorlocationid = @Doctorloctionid";
                result = SqlHelper.ExecuteNonQuery(conn, CommandType.Text, strquery, param);
            }
            return result;
        }
        public DataSet getDoctorLocationbylocid(int locationID)
        {
            SqlConnection conn = null;
            DataSet dsDoctorloaction = new DataSet();
            SqlParameter param;
            using (conn = SqlHelper.GetSQLConnection())
            {
                param = new SqlParameter("@locationid", Convert.ToInt32(locationID));
                string strquery = "select * from doctorlocations where doctorlocationid=@locationid";
                dsDoctorloaction = SqlHelper.ExecuteDataset(conn, CommandType.Text, strquery, param);
            }
            return dsDoctorloaction;
        }
        public DataSet AddUpdateDoctorClinic(int doctorid, DoctorLocations docloaction)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                SqlParameter[] param = new SqlParameter[9];
                param[0] = new SqlParameter("@countryid", Convert.ToInt32(docloaction.CountryId));
                param[1] = new SqlParameter("@stateid", Convert.ToInt32(docloaction.StateId));
                param[2] = new SqlParameter("@cityid", Convert.ToInt32(docloaction.CityId));
                param[3] = new SqlParameter("@locationid", Convert.ToInt32(docloaction.LocationId));
                if (!String.IsNullOrEmpty(docloaction.Address))
                {
                    param[4] = new SqlParameter("@address", Convert.ToString(docloaction.Address));
                }
                else
                {
                    param[4] = new SqlParameter("@address", System.DBNull.Value);
                }
                if (!String.IsNullOrEmpty(docloaction.Telephone))
                {
                    param[5] = new SqlParameter("@telephone", Convert.ToString(docloaction.Telephone));
                }
                else
                {
                    param[5] = new SqlParameter("@telephone", System.DBNull.Value);
                }
                if (!String.IsNullOrEmpty(docloaction.ClinicName))
                {
                    param[6] = new SqlParameter("@clinicname", Convert.ToString(docloaction.ClinicName));
                }
                else
                {
                    param[6] = new SqlParameter("@clinicname", System.DBNull.Value);
                }
                param[7] = new SqlParameter("@userid ", Convert.ToInt32(doctorid));
                param[8] = new SqlParameter("@doclocid ", Convert.ToInt32(docloaction.DoctorLocationId));
                ds = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "askmirai_doctorlocations_Update_and_Insert", param);
            }
            return ds;
        }

        /// Get all data of doctors  details
        /// </summary>
        /// <returns> ilist of doctor</returns>
        public IList<User> getAllDoctorDetails()
        {
            IList<User> lstdoctors = new List<User>();
            DataSet dsDoctorDetails = null;
            using (conn = SqlHelper.GetSQLConnection())
            {
                dsDoctorDetails = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "askmirai_get_alldoctorsdetails");
            }
            lstdoctors = populateDoctorDetails(dsDoctorDetails);
            return lstdoctors;
        }

        public DataSet get_AuthenticateData(string username, string password)
        {
            DataSet dsUserDetails = null;
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@username", username);
            param[1] = new SqlParameter("@password", password);
            using (conn = SqlHelper.GetSQLConnection())
            {
                dsUserDetails = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "get_AuthenticateData", param);
            }

            if (dsUserDetails != null && dsUserDetails.Tables.Count != 0)
                return dsUserDetails;
            else
                return null;
        }

        public DataSet UpdateProfile(int userId, string firstName, string LastName, string mobile, string Email, string IsNewEmail)
        {
            SqlParameter[] questionParam = new SqlParameter[6];
            DataSet result = new DataSet();

            using (SqlConnection conn = SqlHelper.GetSQLConnection())
            {
                questionParam[0] = new SqlParameter("@userId", userId);
                questionParam[1] = new SqlParameter("@firstName", firstName);
                questionParam[2] = new SqlParameter("@LastName", LastName);
                questionParam[3] = new SqlParameter("@mobile", mobile);
                questionParam[4] = new SqlParameter("@Email", Email);
                questionParam[5] = new SqlParameter("@IsNewEmail", IsNewEmail);
                result = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "UpdateProfile", questionParam);
            }
            return result;
        }

        public int insertQuestion(int userId, int status, string questionText)
        {
            SqlParameter[] questionParam = new SqlParameter[4];
            int result = 0;
            using (SqlConnection conn = SqlHelper.GetSQLConnection())
            {
                questionParam[0] = new SqlParameter("@userId", userId);
                questionParam[1] = new SqlParameter("@status", status);
                questionParam[2] = new SqlParameter("@createDate", System.DateTime.Now);
                questionParam[3] = new SqlParameter("@questionText", questionText);
                result = SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "question_Insert", questionParam);
            }
            return result;
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

    }
}