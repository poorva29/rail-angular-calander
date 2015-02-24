using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using DAL;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace MiraiConsultMVC
{
    public class Utilities
    {
        #region Encrypted part
        private static string strKey = "3Fl9#esO#3NJ0hzj4fz$KnAsfl3W";
        private static string timeStamp = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);
        private static Random random = new Random();

        private static string Encrypt(string input, string key)
        {
            //.Net on produciton server does not like empty strings to encrypt. 
            //This causes the whole worker thread to crash.
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }
            TripleDESCryptoServiceProvider des = null;
            MD5CryptoServiceProvider hashmd5;
            byte[] pwdhash, buff;
            string result;
            try
            {
                hashmd5 = new MD5CryptoServiceProvider();
                pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
                hashmd5 = null;
                des = new TripleDESCryptoServiceProvider();
                des.Key = pwdhash;
                des.Mode = CipherMode.ECB; //CBC, CFB
                buff = ASCIIEncoding.ASCII.GetBytes(input);
                result = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (des != null)
                {
                    des.Clear();
                    des = null;
                }
            }
            return result;
        }

        private static string Decrypt(string input, string key)
        {
            //.Net on produciton server does not like empty strings to decrypt. 
            //This causes the whole worker thread to crash.
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }
            TripleDESCryptoServiceProvider des = null;
            MD5CryptoServiceProvider hashmd5;
            byte[] pwdhash, buff;
            string result;
            try
            {
                hashmd5 = new MD5CryptoServiceProvider();
                pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
                hashmd5 = null;
                des = new TripleDESCryptoServiceProvider();
                des.Key = pwdhash;
                des.Mode = CipherMode.ECB; //CBC, CFB
                buff = Convert.FromBase64String(input);
                result = ASCIIEncoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));
            }
            catch (Exception ex)
            {
                result = null;
            }
            finally
            {
                if (des != null)
                {
                    des.Clear();
                    des = null;
                }
            }
            return result;
        }

        public static string Encrypt(string input)
        {
            return Encrypt(input, strKey);
        }

        public static string Decrypt(string input)
        {
            return Decrypt(input, strKey);
        }

        #endregion

        public static void FillListBox(DataTable dtSource, ListBox listbox, string selectFlag)
        {
            listbox.Items.Clear();
            //The DataSet should have its first column as the ID and the second column as the Text.
            listbox.DataSource = dtSource;
            if (dtSource != null)
            {
                if (dtSource.Rows.Count > 0)
                {
                    listbox.DataValueField = dtSource.Columns[0].ToString();
                    listbox.DataTextField = dtSource.Columns[1].ToString();
                    listbox.DataBind();
                }
            }
            if (!selectFlag.Equals("N"))
            {
                System.Web.UI.WebControls.ListItem cmbItem;
                cmbItem = new System.Web.UI.WebControls.ListItem(selectFlag, "0");
                listbox.Items.Insert(0, cmbItem);
                listbox.SelectedIndex = 0;
            }
        }
        public static void FillDropDownList(DataTable dtSource, DropDownList dropdown, string selectFlag)
        {
            dropdown.Items.Clear();
            //The DataSet should have its first column as the ID and the second column as the Text.
            dropdown.DataSource = dtSource;
            if (dtSource != null)
            {
                if (dtSource.Rows.Count > 0)
                {
                    dropdown.DataTextField = dtSource.Columns[1].ToString();
                    dropdown.DataValueField = dtSource.Columns[0].ToString();
                    dropdown.DataBind();
                }
            }
            if (!selectFlag.Equals("N"))
            {
                System.Web.UI.WebControls.ListItem cmbItem;
                cmbItem = new System.Web.UI.WebControls.ListItem(selectFlag, "0");
                dropdown.Items.Insert(0, cmbItem);
                dropdown.SelectedIndex = 0;
            }
        }
        public static void terminateSession()
        {
            terminateSession("../home/home.aspx");
        }
        public static void terminateSession(string redirectURL)
        {
            FormsAuthentication.SignOut();
            if (HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddYears(-30);
            }
            if (HttpContext.Current.Request.Cookies["ASP.NET_SessionId"] != null)
            {
                HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-30);
            }
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Redirect(redirectURL);
        }
        public static void checkSessionExpired()
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] == null)
            {
                HttpContext.Current.Response.Redirect("/login");
            }
        }
        public static Boolean isAuthorisedandSessionExpired(int privilege)
        {
            IList<int> privileges = (IList<int>)HttpContext.Current.Session["privileges"];
            bool isAccessible = false;
            if (privileges!=null && privileges.Count > 0)
            {
                isAccessible = privileges.Contains(privilege);
            }
            return isAccessible;
        }

        public static string GenerateSessionURl()
        {
            string nonse = GenerateNonce(timeStamp.Length);
            string signature = CreateSignature("application_id=" + ConfigurationManager.AppSettings["QuickBloxApplicationId"].ToString() + "&auth_key=" + ConfigurationManager.AppSettings["QuickBloxAuthKey"].ToString() + "&nonce=" + nonse + "&timestamp=" + timeStamp, ConfigurationManager.AppSettings["QuickBloxAuthSecret"].ToString());
            string url = "https://api.quickblox.com/session.json?application_id=" + ConfigurationManager.AppSettings["QuickBloxApplicationId"].ToString() + "&auth_key=" + ConfigurationManager.AppSettings["QuickBloxAuthKey"].ToString() + "&nonce=" + nonse + "&timestamp=" + timeStamp + "&signature=" + signature;
            return url;
        }

        public static string CreateSignature(string message, string secret)
        {
            var enc = Encoding.ASCII;
            HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(secret));
            hmac.Initialize();

            byte[] buffer = enc.GetBytes(message);
            return BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();
        }

        public static string GenerateNonce(int length)
        {
            var nonceString = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                nonceString.Append(timeStamp[random.Next(0, timeStamp.Length - 1)]);
            }
            return nonceString.ToString();
        }

        public static int signUpOnQuickblox(int userId, string email)
        {
            int result = 0;
            string sessioUrl = GenerateSessionURl();
            var sessionhttp = (HttpWebRequest)WebRequest.Create(new Uri(sessioUrl));
            sessionhttp.Accept = "application/json";
            sessionhttp.ContentType = "application/json";
            sessionhttp.Method = "POST";
            var response = sessionhttp.GetResponse();
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            var sessionDetail = new
            {
                session = new
                {
                    _id = "",
                    application_id = "",
                    created_at = "",
                    device_id = "",
                    nonce = "",
                    token = "",
                    ts = "",
                    updated_at = "",
                    user_id = "",
                    id = ""
                }
            };
            if (!string.IsNullOrEmpty(content))
            {
                sessionDetail = JsonConvert.DeserializeAnonymousType(content, sessionDetail);
                string token = sessionDetail.session.token;
                if (!string.IsNullOrEmpty(token))
                {
                    string signUpUrl = "http://api.quickblox.com/users.json";

                    var signUphttp = (HttpWebRequest)WebRequest.Create(new Uri(signUpUrl));
                    signUphttp.Accept = "application/json";
                    signUphttp.ContentType = "application/json";
                    signUphttp.Method = "POST";
                    signUphttp.Headers["QB-Token"] = token;
                    var StreamWriter = new StreamWriter(signUphttp.GetRequestStream());
                    string login = email.Split('@')[0];

                    string postJson = "{\"user\": {\"login\": \"" + login + "\", \"password\": \"" + Convert.ToString(ConfigurationManager.AppSettings["QuickbloxUserPassword"]) + "\", \"email\": \"" + email + "\"}}";
                    StreamWriter.Write(Convert.ToString(postJson));
                    StreamWriter.Flush();
                    StreamWriter.Close();

                    var signUpResponse = signUphttp.GetResponse();
                    var signUpStream = signUpResponse.GetResponseStream();
                    var signUpsr = new StreamReader(signUpStream);
                    var signUpJson = signUpsr.ReadToEnd();

                    var userDetail = new
                    {
                        user = new
                        {
                            id = "",
                            login = "",
                            email = ""
                        }
                    };
                    if (!string.IsNullOrEmpty(signUpJson))
                    {
                        userDetail = JsonConvert.DeserializeAnonymousType(signUpJson, userDetail);
                        if (userDetail != null)
                        {
                            result = UserManager.getInstance().Insert_QuickbloxUser(userId, Convert.ToInt32(UserType.Doctor), userDetail.user.id, userDetail.user.login, Convert.ToString(ConfigurationManager.AppSettings["QuickbloxUserPassword"]), userDetail.user.email);
                        }
                    }
                }
            }
            return result;
        }
    }
}