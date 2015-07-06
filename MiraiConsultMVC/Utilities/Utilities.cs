using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace MiraiConsultMVC
{
    public class Utilities
    {
        #region Encrypted part
        private static string strKey = "3Fl9#esO#3NJ0hzj4fz$KnAsfl3W";
        private static string timeStamp = Convert.ToString((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);

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
        public static string GetDisplayDate(DateTime dateTime)
        {
            //dateTime = GetClientDateTime(dateTime);
            string[] months = { "dummyMonth", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            string displayDate = dateTime.Day.ToString() + " " +
            months[dateTime.Month] + " " +
            dateTime.Year.ToString();

            return displayDate;
        }

        public static string GetDisplayDate(Nullable<DateTime> date)
        {
            string displayDate = "";
            if (date.HasValue)
            {
                DateTime dateTime = date.Value;

                //dateTime = GetClientDateTime(dateTime);
                string[] months = { "dummyMonth", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

                displayDate = dateTime.Day.ToString() + " " +
                months[dateTime.Month] + " " +
                dateTime.Year.ToString();
            }
            return displayDate;
        }

        public static string GetDisplayTime(string time)
        {
            string displayTime = "";
            if (!string.IsNullOrEmpty(time))
                displayTime = String.Format("{0:h:mm tt}", Convert.ToDateTime(time));

            return displayTime;
        }
    }
}