using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Utility
{
    public class ConfigLoader
    {
        private const int NUMBER_OF_INFO_FIELDS = 3;
        private const string XML_SERVER_INFO_LOCATION = "//configuration/server_info";
        private const string PRINTING_OPTION_LOCATION = "//configuration/printing_options";
        private readonly Dictionary<string, int> serverFieldMap = new Dictionary<string, int>() {
            {"address", 0}, {"username", 1}, {"password", 2}
        };

        /*
         *  Key: department
         *  Value: [adddress, username, password]
         */
        private Dictionary<string, string[]> serverInfo;
        private string filename;
        private string jsonUrl;
        private string configPath;
        XmlDocument doc;

        private Dictionary<string, string[]> departmentPrinterMap = new Dictionary<string, string[]>();

        public ConfigLoader(string filename, string jsonUrl = "")
        {
            this.filename = filename;
            this.jsonUrl = jsonUrl;
            this.configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            this.serverInfo = new Dictionary<string, string[]>();
            this.doc = new XmlDocument();
        }
        public bool LoadConfig()
        {
            return ProcessXmlDocument(configPath) && ProcessJSON(jsonUrl);
        }
        private bool ProcessXmlDocument(string file)
        {
            try
            {
                doc.Load(file);
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
                return false;
            }
            return true;
        }
        private bool ProcessJSON(string url)
        {
            try
            {
                var jsonString = new WebClient().DownloadString(url);
                departmentPrinterMap = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Get the configuration of the server by printer name.
        /// </summary>
        /// <param name="printerName">The name of the printer, eg: lj_cl_112a </param>
        /// <returns>A array of size 3 containing the information of the server</returns>
        public string[] GetServerConfig(string printerName)
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
        public IList<Tuple<string, bool>> GetAllPrintingOptions()
        {
            var Commands = new List<Tuple<string, bool>>();
            try
            {
                doc.Load(configPath);
                XmlNode PrintingOptions = doc.SelectNodes(PRINTING_OPTION_LOCATION)[0];
                foreach (XmlNode option in PrintingOptions.ChildNodes)
                {
                    var OptionTuple = new Tuple<string, bool>(
                        option.Attributes["name"].Value, Boolean.Parse(option.Attributes["enabled"].Value));
                    Commands.Add(OptionTuple);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Commands;
        }
        public IList<string> GetEnabledPrintingOptions()
        {
            IList<string> Commands = new List<string>();
            try
            {
                doc.Load(configPath);
                XmlNode PrintingOptions = doc.SelectNodes(PRINTING_OPTION_LOCATION)[0];

                foreach (XmlNode option in PrintingOptions.ChildNodes)
                {
                    if (Boolean.Parse(option.Attributes["enabled"].Value))
                    {
                        Commands.Add(option.InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Commands;
        }
        private bool SaveXmlDocument(string XmlFilePath)
        {
            try
            {
                doc.Save(XmlFilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }
        public bool SetEnabledPrintingOptions(ISet<string> OptionName)
        {
            try
            {
                doc.Load(configPath);
                XmlNode PrintingOptions = doc.SelectNodes(PRINTING_OPTION_LOCATION)[0];
                foreach (XmlNode option in PrintingOptions.ChildNodes)
                {
                    if (OptionName.Contains(option.Attributes["name"].Value))
                    {
                        option.Attributes["enabled"].Value = "true";
                    }
                    else
                    {
                        option.Attributes["enabled"].Value = "false";
                    }
                }
                if (!this.SaveXmlDocument(configPath))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }
    }

}
