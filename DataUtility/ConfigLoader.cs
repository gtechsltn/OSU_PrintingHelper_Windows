using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace DataUtility
{
    public class ConfigLoader
    {
        private string filename;
        private string jsonUrl;
        /*
         *  Key: department
         *  Value: [adddress, username, password]
         */
        private Dictionary<string, string[]> serverInfo;
        private const int NUMBER_OF_INFO_FIELDS = 3;
        private const string XML_SERVER_INFO_LOCATION = "//configuration/server_info";

        private Dictionary<string, int> serverFieldMap = new Dictionary<string, int>() {
            {"address", 0}, {"username", 1}, {"password", 2}
        };

        private Dictionary<string, string[]> departmentPrinterMap = new Dictionary<string, string[]>();

        public ConfigLoader(string filename, string jsonUrl)
        {
            this.filename = filename;
            this.jsonUrl = jsonUrl;
            serverInfo = new Dictionary<string, string[]>();
        }
        public bool LoadConfig()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            return processXmlDocument(configPath) && processJSON(jsonUrl);
        }
        private bool processXmlDocument(string filepath)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filepath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Configuration file not found!");
                Console.ReadLine();
                return false;
            }

            try
            {
                XmlNodeList userNodes = doc.SelectNodes(XML_SERVER_INFO_LOCATION);
                foreach (XmlNode userNode in userNodes)
                {
                    string department = userNode.Attributes["department"].Value;
                    string[] arr = new string[NUMBER_OF_INFO_FIELDS];

                    XmlNodeList info = userNode.ChildNodes;
                    foreach (XmlNode x in info)
                    {
                        arr[serverFieldMap[x.Name]] = x.InnerText;
                    }
                    serverInfo.Add(department, arr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
                return false;
            }
            return true;
        }
        private bool processJSON(string url)
        {
            try
            {
                var jsonString = new WebClient().DownloadString(url);
                departmentPrinterMap = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
                return false;
            }
            return true;
        }
        public string[] GetConfig(string printerName)
        {
            foreach (var pair in departmentPrinterMap)
            {
                if (pair.Value.Contains(printerName))
                {
                    string dept = pair.Key;
                    if (serverInfo.ContainsKey(dept))
                    {
                        string[] ret = new string[NUMBER_OF_INFO_FIELDS];
                        serverInfo.TryGetValue(dept, out ret);
                        ret[2] = CredentialsManager.DecryptText(ret[2]);
                        return ret;
                    }
                    return null;
                }
            }
            return null;
        }
    }

}
