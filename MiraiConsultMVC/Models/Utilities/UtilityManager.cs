using System;
using System.Security.Cryptography;
using System.Text;

namespace MiraiConsultMVC.Models.Utilities
{
    public class UtilityManager
    {
        private static string strKey = "3Fl9#esO#3NJ0hzj4fz$KnAsfl3W";
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
    }
}