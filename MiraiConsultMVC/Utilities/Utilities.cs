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
        private static string secret = "Miraihealthcare";

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

        public static string GenerateTokenForQuickBlox()
        {
            string token = "";
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
                token = sessionDetail.session.token;
            }
            return token;
        }

        public static int signUpOnQuickblox(int userId, string email, int userType)
        {
            int result = 0;
            string token = GenerateTokenForQuickBlox();
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
                if (login.Length < 5)
                {
                    login = login + "Mirai";
                }
                string quickbloxPassword = login + Convert.ToString(userId);
                string qBloxpassword = generatePasswordForQuickBlox(secret, quickbloxPassword);
                qBloxpassword = qBloxpassword.Substring(0, 15);

                string postJson = "{\"user\": {\"login\": \"" + login + "\", \"password\": \"" + qBloxpassword + "\", \"email\": \"" + email + "\"}}";
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
                        email = "",
                        created_at = "",
                        updated_at = ""
                    }
                };
                if (!string.IsNullOrEmpty(signUpJson))
                {
                    userDetail = JsonConvert.DeserializeAnonymousType(signUpJson, userDetail);
                    if (userDetail != null)
                    {
                        result = UserManager.getInstance().Insert_update_QuickbloxUser(0, userId, userType, userDetail.user.id, userDetail.user.login, qBloxpassword, userDetail.user.email, userDetail.user.created_at, userDetail.user.updated_at);
                    }
                }
            }
            return result;
        }

        public static int UpdateEmailByQuickBloxId(string oldEmail, string updatedEmail)
        {
            int result = 0;
            string token = GenerateTokenForQuickBlox();
            string LoginUrl = "http://api.quickblox.com/login.json?email=" + Convert.ToString(ConfigurationManager.AppSettings["QuickBloxUserLoginId"]) + "&password=" + Convert.ToString(ConfigurationManager.AppSettings["QuickBloxUserLoginPassword"]);

            var Loginhttp = (HttpWebRequest)WebRequest.Create(new Uri(LoginUrl));
            Loginhttp.Accept = "application/json";
            Loginhttp.ContentType = "application/json";
            Loginhttp.Method = "POST";
            Loginhttp.Headers["QB-Token"] = token;
            var LoginResponse = Loginhttp.GetResponse();
            var LoginStream = LoginResponse.GetResponseStream();
            var Loginsr = new StreamReader(LoginStream);
            var LoginJson = Loginsr.ReadToEnd();
            var loginDetail = new
            {
                user = new
                {
                    id = "",
                    owner_id = "",
                    full_name = "",
                    email = "",
                    login = "",
                    phone = "",
                    website = "",
                    created_at = "",
                    updated_at = "",
                    last_request_at = "",
                    external_user_id = "",
                    facebook_id = "",
                    twitter_id = "",
                    blob_id = "",
                    custom_data = "",
                    user_tags = ""
                }
            };
            if (!string.IsNullOrEmpty("LoginJson"))
            {
                loginDetail = JsonConvert.DeserializeAnonymousType(LoginJson, loginDetail);
            }
            if (loginDetail != null)
            {
                DataTable quickbloxDetail = UserManager.getInstance().CheckQuickBloxAccountAvailableOrNot(oldEmail);
                if (quickbloxDetail != null && quickbloxDetail.Rows.Count > 0)
                {
                    string login = updatedEmail.Split('@')[0];
                    if (login.Length < 5)
                    {
                        login = login + "Mirai";
                    }
                    string quickbloxPassword = login + Convert.ToString(quickbloxDetail.Rows[0]["userid"]);
                    string qBloxpassword = generatePasswordForQuickBlox(secret, quickbloxPassword);
                    qBloxpassword = qBloxpassword.Substring(0, 15);

                    string updateUserUrl = "http://api.quickblox.com/users/" + quickbloxDetail.Rows[0]["quickblox_userid"] + ".json";
                    var updateUserhttp = (HttpWebRequest)WebRequest.Create(new Uri(updateUserUrl));
                    updateUserhttp.Accept = "application/json";
                    updateUserhttp.ContentType = "application/json";
                    updateUserhttp.Method = "PUT";
                    updateUserhttp.Headers["QB-Token"] = token;

                    var StreamWriter = new StreamWriter(updateUserhttp.GetRequestStream());
                    string postJson = "{\"user\": {\"login\": \"" + login + "\",\"old_password\": \"" + Convert.ToString(quickbloxDetail.Rows[0]["quickblox_password"]) + "\",\"password\": \"" + qBloxpassword + "\",\"email\": \"" + updatedEmail + "\"}}";
                    StreamWriter.Write(Convert.ToString(postJson));
                    StreamWriter.Flush();
                    StreamWriter.Close();

                    var upadteUserResponse = updateUserhttp.GetResponse();
                    var updateUserStream = upadteUserResponse.GetResponseStream();
                    var upadteUsersr = new StreamReader(updateUserStream);
                    var UpadetUserJson = upadteUsersr.ReadToEnd();
                    if (!string.IsNullOrEmpty(UpadetUserJson))
                    {
                        var userDetail = new
                        {
                            user = new
                            {
                                id = "",
                                login = "",
                                email = "",
                                created_at = "",
                                updated_at = ""
                            }
                        };
                        if (!string.IsNullOrEmpty(UpadetUserJson))
                        {
                            userDetail = JsonConvert.DeserializeAnonymousType(UpadetUserJson, userDetail);
                            if (userDetail != null)
                            {
                                result = UserManager.getInstance().Insert_update_QuickbloxUser(Convert.ToInt32(quickbloxDetail.Rows[0]["id"]), Convert.ToInt32(quickbloxDetail.Rows[0]["userid"]), Convert.ToInt32(quickbloxDetail.Rows[0]["usertype"]), userDetail.user.id, userDetail.user.login, qBloxpassword, userDetail.user.email, userDetail.user.created_at, userDetail.user.updated_at);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static String generatePasswordForQuickBlox(String secretKey, String message)
        {
            var StringToSign = secretKey + message;
            var sha256 = new SHA256Managed();
            byte[] digest = sha256.ComputeHash(Encoding.ASCII.GetBytes(StringToSign));
            String signedInput = Convert.ToBase64String(digest);
            //Removing the trailing = signs
            var lastEqualsSignIndex = signedInput.Length - 1;
            while (signedInput[lastEqualsSignIndex] == '=')
            {
                lastEqualsSignIndex--;
            }
            signedInput = signedInput.Substring(0, lastEqualsSignIndex + 1);
            return HttpUtility.UrlEncode(signedInput.Substring(0, 10));
        }
    }
}