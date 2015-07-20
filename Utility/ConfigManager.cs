using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Utility
{
    public class ConfigManager
    {
        public static readonly ISet<string> VAILD_PRINTER_ATTRIBUTES = new HashSet<string>(
            new List<string> { "Name", "Location", "Type", "Department" });

        private const int NUMBER_OF_INFO_FIELDS = 3;
        private const string BASE_CONFIG_LOCATION = "//configuration";
        private const string XML_SERVER_INFO_LOCATION = BASE_CONFIG_LOCATION + "/server_info";
        private const string PRINTING_OPTION_LOCATION = BASE_CONFIG_LOCATION + "/printing_options";
        private const string LOADED_PRINTER_LOCATION = BASE_CONFIG_LOCATION + "/loaded_printers";

        private readonly Dictionary<string, int> SERVER_FIELD_MAP = new Dictionary<string, int>() {
            {"address", 0}, {"username", 1}, {"password", 2}
        };

        private string ConfigPath;
        XmlDocument doc;

        public ConfigManager(string filename)
        {
            this.ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            this.doc = new XmlDocument();
            if (!File.Exists(filename))
            {
                DownloadConfig(ConstFields.CONFIGRATION_FILE_URL, filename);
            }
        }

        private void DownloadConfig(string url, string path)
        {
            try
            {
                string xmlString = new WebClient().DownloadString(url);
                System.IO.File.WriteAllText(path, xmlString);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// Get the all the servers info.
        /// </summary>
        /// <returns>A map mapping from department to array of ["address", "username", "password"]</returns>
        public IDictionary<string, string[]> GetServerInfo()
        {
            IDictionary<string, string[]> ServerInfo = new Dictionary<string, string[]>();
            try
            {
                doc.Load(ConfigPath);
                XmlNodeList userNodes = doc.SelectNodes(XML_SERVER_INFO_LOCATION);
                foreach (XmlNode userNode in userNodes)
                {
                    string department = userNode.Attributes["Department"].Value;
                    string[] arr = new string[NUMBER_OF_INFO_FIELDS];

                    XmlNodeList info = userNode.ChildNodes;
                    foreach (XmlNode x in info)
                    {
                        arr[SERVER_FIELD_MAP[x.Name]] = x.InnerText;
                    }
                    ServerInfo.Add(department, arr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            return ServerInfo;
        }
        /// <summary>
        ///     <para>Get the configuration of the server by printer name.</para>
        ///     <para>
        ///         Note: If the information of the server is not found,
        ///         the function will return null.
        ///     </para>
        /// </summary>
        /// <param name="printerName">The name of the printer, eg: lj_cl_112a </param>
        /// <returns>
        /// A array of size 3 containing the information of the server
        /// </returns>
        public string[] GetServerConfig(string printerName)
        {
            IDictionary<string, string> DepartmentPrinterMap = GetDepartmentMap();
            IDictionary<string, string[]> ServerInfo = GetServerInfo();
            if (DepartmentPrinterMap != null && ServerInfo != null
                && DepartmentPrinterMap.ContainsKey(printerName))
            {
                string department = DepartmentPrinterMap[printerName];
                if (ServerInfo.ContainsKey(department))
                {
                    string[] ret = new string[NUMBER_OF_INFO_FIELDS];
                    ServerInfo.TryGetValue(department, out ret);
                    ret[2] = CredentialsManager.DecryptText(ret[2]);
                    return ret;
                }
            }
            return null;
        }
        public IDictionary<string, string> GetDepartmentMap()
        {
            IDictionary<string, string> DepartmentMap = new Dictionary<string, string>();
            try
            {
                doc.Load(ConfigPath);
                XmlNode userNodes = doc.SelectNodes(LOADED_PRINTER_LOCATION)[0];
                foreach (XmlNode userNode in userNodes.ChildNodes)
                {
                    string department = userNode.Attributes["Department"].Value;
                    string name = userNode.InnerText;
                    DepartmentMap[name] = department;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            return DepartmentMap;
        }

        public IList<Tuple<string, bool>> GetAllPrintingOptions()
        {
            var Commands = new List<Tuple<string, bool>>();
            try
            {
                doc.Load(ConfigPath);
                XmlNode PrintingOptions = doc.SelectNodes(PRINTING_OPTION_LOCATION)[0];
                foreach (XmlNode option in PrintingOptions.ChildNodes)
                {
                    var OptionTuple = new Tuple<string, bool>(
                        option.Attributes["Name"].Value, Boolean.Parse(option.Attributes["Enabled"].Value));
                    Commands.Add(OptionTuple);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return Commands;
        }
        public IList<string> GetEnabledPrintingOptions()
        {
            IList<string> Commands = new List<string>();
            try
            {
                doc.Load(ConfigPath);
                XmlNode PrintingOptions = doc.SelectNodes(PRINTING_OPTION_LOCATION)[0];

                foreach (XmlNode option in PrintingOptions.ChildNodes)
                {
                    if (Boolean.Parse(option.Attributes["Enabled"].Value))
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
        public bool SaveEnabledPrintingOptions(ISet<string> OptionName)
        {
            try
            {
                doc.Load(ConfigPath);
                XmlNode PrintingOptions = doc.SelectNodes(PRINTING_OPTION_LOCATION)[0];
                foreach (XmlNode option in PrintingOptions.ChildNodes)
                {
                    bool val = OptionName.Contains(option.Attributes["Name"].Value);
                    option.Attributes["Enabled"].Value = val.ToString();
                }
                doc.Save(this.ConfigPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        public IList<IDictionary<string, string>> GetAllLoadedPrinters()
        {
            IList<IDictionary<string, string>> ret = new List<IDictionary<string, string>>();
            try
            {
                doc.Load(ConfigPath);
                XmlNode LoadedPrinters = doc.SelectNodes(LOADED_PRINTER_LOCATION)[0];
                foreach (XmlNode XmlPrinter in LoadedPrinters.ChildNodes)
                {
                    IDictionary<string, string> printer = new Dictionary<string, string>();
                    for (int i = 0; i < XmlPrinter.Attributes.Count; ++i)
                    {
                        var item = XmlPrinter.Attributes[i];
                        printer[item.Name] = item.Value;
                    }
                    printer["Name"] = XmlPrinter.InnerText;
                    ret.Add(printer);
                }
            }
            catch (Exception ex)
            {
                ret.Clear();
                Debug.WriteLine(ex);
            }
            return ret;
        }
        public bool SaveAllLoadedPrinters(IList<IDictionary<string, string>> Printers)
        {
            try
            {
                doc.Load(ConfigPath);
                XmlNode AllPrinters = doc.SelectNodes(LOADED_PRINTER_LOCATION)[0];
                AllPrinters.RemoveAll();

                foreach (IDictionary<string, string> p in Printers)
                {
                    XmlElement element = doc.CreateElement("printer", null);
                    if (p.ContainsKey("Name"))
                    {
                        element.InnerText = p["Name"];
                    }
                    foreach (var pair in p)
                    {
                        element.SetAttribute(pair.Key, null, pair.Value);
                    }
                    AllPrinters.AppendChild(element);
                }
                doc.Save(this.ConfigPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        public bool MovePrinterToFront(int PrinterPosition)
        {
            try
            {
                doc.Load(ConfigPath);
                XmlNode AllPrinters = doc.SelectNodes(LOADED_PRINTER_LOCATION)[0];
                XmlNode Removed = AllPrinters.ChildNodes[PrinterPosition];
                Removed = AllPrinters.RemoveChild(Removed);
                AllPrinters.PrependChild(Removed);

                doc.Save(this.ConfigPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        ///     <para>Get the user credentials</para>
        ///     <para>
        ///         Note: If the information of the server is not found,
        ///         the function will return null.
        ///     </para>
        /// </summary>
        /// <returns>
        /// A map mapping from department to a Tuple which has the format (username, password)
        /// </returns>
        public IDictionary<string, Tuple<string, string>> LoadUserCredentials()
        {
            IDictionary<string, Tuple<string, string>> userCredentials =
                new Dictionary<string, Tuple<string, string>>();
            try
            {
                doc.Load(ConfigPath);
                XmlNodeList userNodes = doc.SelectNodes(XML_SERVER_INFO_LOCATION);
                foreach (XmlNode userNode in userNodes)
                {
                    string department = userNode.Attributes["Department"].Value;
                    string username = userNode.SelectNodes("username").Item(0).InnerText;
                    string password = userNode.SelectNodes("password").Item(0).InnerText;
                    userCredentials.Add(department, new Tuple<string, string>(username, password));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
            return userCredentials;
        }

        public string GetUserName(string department)
        {
            IDictionary<string, Tuple<string, string>> userCredentials = LoadUserCredentials();
            if (userCredentials.ContainsKey(department))
            {
                return userCredentials[department].Item1;
            }
            return "";
        }

        public string GetPassword(string department)
        {
            IDictionary<string, Tuple<string, string>> userCredentials = LoadUserCredentials();
            if (userCredentials.ContainsKey(department))
            {
                string password = userCredentials[department].Item2;
                if (!password.Equals(""))
                {
                    password = CredentialsManager.DecryptText(password);
                }
                return password;
            }
            return "";
        }

        public bool SaveCredentials(string department, string username, string password)
        {
            if (password.Equals(""))
            {
                return false;                
            }
            password = CredentialsManager.EncryptText(password);
            try
            {
                doc.Load(ConfigPath);
                string select = XML_SERVER_INFO_LOCATION + "[@Department='" + department + "']";
                XmlNodeList userNodes = doc.SelectNodes(select);

                XmlNode userNode = userNodes[0];
                XmlAttribute departmentAttribute = userNode.Attributes["Department"];
                userNode.SelectNodes("username").Item(0).InnerText = username;
                userNode.SelectNodes("password").Item(0).InnerText = password;

                doc.Save(this.ConfigPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }
    }

}
