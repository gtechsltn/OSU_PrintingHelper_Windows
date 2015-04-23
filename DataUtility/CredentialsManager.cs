using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace DataUtility
{
    public class CredentialsManager
    {
        /*
         * Const fields.
         */
        private const string c_key = "SEVMTE9fV09STEQ=";
        private const string c_iv = "V0hBVF9ET19ZT1VfV0FOVA==";
        private const string XML_SERVER_INFO_LOCATION = "//configuration/server_info";

        private const string WEBSITE_DIRECTORY = "http://web.cse.ohio-state.edu/~zhante/";
        private const string CONFIGRATION_FILE_NAME = "server_config.xml";
        private const string DEPT_PRINTER_MAP = "printer_map.json";

        private string DEPT_PRINTER_MAP_URL = Path.Combine(WEBSITE_DIRECTORY, DEPT_PRINTER_MAP);
        private string CONFIGRATION_TEMPLATE_URL = Path.Combine(WEBSITE_DIRECTORY, CONFIGRATION_FILE_NAME);
        /*
         * Key:     department
         * Value:   (username, password) tuple
         */
        private Dictionary<string, Tuple<string, string>> userCredentials =
            new Dictionary<string, Tuple<string, string>>();
        private string credentialFilePath;

        public static string EncryptText(string openText)
        {
            RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
            ICryptoTransform encryptor = rc2CSP.CreateEncryptor(Convert.FromBase64String(c_key), Convert.FromBase64String(c_iv));
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    byte[] toEncrypt = Encoding.Unicode.GetBytes(openText);

                    csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                    csEncrypt.FlushFinalBlock();

                    byte[] encrypted = msEncrypt.ToArray();

                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        public static string DecryptText(string encryptedText)
        {
            try
            {
                RC2CryptoServiceProvider rc2CSP = new RC2CryptoServiceProvider();
                ICryptoTransform decryptor = rc2CSP.CreateDecryptor(Convert.FromBase64String(c_key), Convert.FromBase64String(c_iv));
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        List<Byte> bytes = new List<byte>();
                        int b;
                        do
                        {
                            b = csDecrypt.ReadByte();
                            if (b != -1)
                            {
                                bytes.Add(Convert.ToByte(b));
                            }

                        }
                        while (b != -1);

                        return Encoding.Unicode.GetString(bytes.ToArray());
                    }
                }
            } 
            catch (Exception)
            {
                return "";
            }
        }
        public CredentialsManager(string credentialFilePath)
        {
            this.credentialFilePath = credentialFilePath;
        }
        public bool LoadUserCredentials()
        {
            XmlDocument doc = LoadXmlDocument();
            if (doc == null)
            {
                return false;
            }
            try
            {
                XmlNodeList userNodes = doc.SelectNodes(XML_SERVER_INFO_LOCATION);
                foreach (XmlNode userNode in userNodes)
                {
                    string department = userNode.Attributes["department"].Value;
                    string username = userNode.SelectNodes("username").Item(0).InnerText;
                    string password = userNode.SelectNodes("password").Item(0).InnerText;
                    userCredentials.Add(department, new Tuple<string, string>(username, password));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }
        private void DownloadXmlDocument(string url, string path)
        {
            Debug.WriteLine(url + " " + path);
            try
            {
                string xmlString = new WebClient().DownloadString(url);
                System.IO.File.WriteAllText(path, xmlString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private XmlDocument LoadXmlDocument()
        {
            Debug.WriteLine("loading..");
            XmlDocument doc = new XmlDocument();
            try
            {
                if (File.Exists(this.credentialFilePath))
                {
                    doc.Load(this.credentialFilePath);
                }
                else
                {
                    DownloadXmlDocument(CONFIGRATION_TEMPLATE_URL, this.credentialFilePath);
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine(this.credentialFilePath + " not found.");
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            return doc;
        }
        public bool StoreCredentials(string department, string username, string password)
        {
            if (!password.Equals(""))
            {
                password = CredentialsManager.EncryptText(password);
            }
            XmlDocument doc = LoadXmlDocument();
            if (doc == null)
            {
                return false;
            }
            try
            {
                string select = XML_SERVER_INFO_LOCATION + "[@department='" + department + "']";
                XmlNodeList userNodes = doc.SelectNodes(select);

                XmlNode userNode = userNodes[0];
                XmlAttribute departmentAttribute = userNode.Attributes["department"];
                userNode.SelectNodes("username").Item(0).InnerText = username;
                userNode.SelectNodes("password").Item(0).InnerText = password;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            try
            {
                doc.Save(this.credentialFilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        public string GetUserName(string department)
        {
            if (userCredentials.ContainsKey(department))
            {
                return userCredentials[department].Item1;
            }
            return "";
        }

        public string GetPassword(string department)
        {
            if (userCredentials.ContainsKey(department))
            {
                string password = userCredentials[department].Item2;
                if (!password.Equals(""))
                {
                    password = DecryptText(password);
                }
                return password;
            }
            return "";
        }
    }
}
