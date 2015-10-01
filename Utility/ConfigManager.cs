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
    public class ConfigManager : Interfaces.IConfigManager
    {
        public static readonly ISet<string> VAILD_PRINTER_ATTRIBUTES = new HashSet<string>(
            new List<string> { "Name", "Location", "Type", "Department" });

        private const int NUMBER_OF_INFO_FIELDS = 3;
        private const string BASE_CONFIG_LOCATION = "//configuration";
        private const string XML_SERVER_INFO_LOCATION = BASE_CONFIG_LOCATION + "/server_info";
        private const string PRINTING_OPTION_LOCATION = BASE_CONFIG_LOCATION + "/printing_options";
        private const string LOADED_PRINTER_LOCATION = BASE_CONFIG_LOCATION + "/loaded_printers";

        private readonly Dictionary<string, int> SERVER_FIELD_MAP =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            {"address", 0}, {"username", 1}, {"password", 2}
        };

        private string _configPath;
        XmlDocument _doc;

        public ConfigManager(string filename)
        {
            this._configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            this._doc = new XmlDocument();
            if (!File.Exists(this._configPath))
            {
                DownloadConfig(ConstFields.CONFIGRATION_FILE_URL, filename);
            }
        }

        private static void DownloadConfig(string url, string path)
        {
            try
            {
                string xmlString = new WebClient().DownloadString(url);
                System.IO.File.WriteAllText(path, xmlString);
            }
            catch (IOException)
            {
                Debug.WriteLine("Error writing file.");
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
            IDictionary<string, string[]> ServerInfo =
                new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            try
            {
                _doc.Load(_configPath);
                XmlNodeList userNodes = _doc.SelectNodes(XML_SERVER_INFO_LOCATION);
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
            IDictionary<string, string> DepartmentMap =
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                _doc.Load(_configPath);
                XmlNode userNodes = _doc.SelectNodes(LOADED_PRINTER_LOCATION)[0];
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
            var commands = new List<Tuple<string, bool>>();
            try
            {
                _doc.Load(_configPath);
                XmlNode printingOptions = _doc.SelectNodes(PRINTING_OPTION_LOCATION)[0];
                foreach (XmlNode option in printingOptions.ChildNodes)
                {
                    var optionTuple = new Tuple<string, bool>(
                        option.Attributes["Name"].Value, Boolean.Parse(option.Attributes["Enabled"].Value));
                    commands.Add(optionTuple);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return commands;
        }
        public IList<string> GetEnabledPrintingOptions()
        {
            IList<string> commands = new List<string>();
            try
            {
                _doc.Load(_configPath);
                XmlNode printingOptions = _doc.SelectNodes(PRINTING_OPTION_LOCATION)[0];

                foreach (XmlNode option in printingOptions.ChildNodes)
                {
                    if (Boolean.Parse(option.Attributes["Enabled"].Value))
                    {
                        commands.Add(option.InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return commands;
        }
        public bool SaveEnabledPrintingOptions(ISet<string> optionName)
        {
            try
            {
                _doc.Load(_configPath);
                XmlNode PrintingOptions = _doc.SelectNodes(PRINTING_OPTION_LOCATION)[0];
                foreach (XmlNode option in PrintingOptions.ChildNodes)
                {
                    bool val = optionName.Contains(option.Attributes["Name"].Value);
                    option.Attributes["Enabled"].Value = val.ToString();
                }
                _doc.Save(this._configPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        public IList<Dictionary<string, string>> GetAllLoadedPrinters()
        {
            IList<Dictionary<string, string>> ret = new List<Dictionary<string, string>>();
            try
            {
                _doc.Load(_configPath);
                XmlNode loadedPrinters = _doc.SelectNodes(LOADED_PRINTER_LOCATION)[0];
                foreach (XmlNode XmlPrinter in loadedPrinters.ChildNodes)
                {
                    Dictionary<string, string> printer =
                        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
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
        public bool SaveAllLoadedPrinters(IList<Dictionary<string, string>> printers)
        {
            try
            {
                _doc.Load(_configPath);
                XmlNode allPrinters = _doc.SelectNodes(LOADED_PRINTER_LOCATION)[0];
                allPrinters.RemoveAll();

                foreach (IDictionary<string, string> p in printers)
                {
                    XmlElement element = _doc.CreateElement("printer", null);
                    if (p.ContainsKey("Name"))
                    {
                        element.InnerText = p["Name"];
                    }
                    foreach (var pair in p)
                    {
                        element.SetAttribute(pair.Key, null, pair.Value);
                    }
                    allPrinters.AppendChild(element);
                }
                _doc.Save(this._configPath);
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
                _doc.Load(_configPath);
                XmlNode AllPrinters = _doc.SelectNodes(LOADED_PRINTER_LOCATION)[0];
                XmlNode removed = AllPrinters.ChildNodes[PrinterPosition];
                removed = AllPrinters.RemoveChild(removed);
                AllPrinters.PrependChild(removed);

                _doc.Save(this._configPath);
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
        public IDictionary<string, Tuple<string, string>> GetUserCredentials()
        {
            IDictionary<string, Tuple<string, string>> userCredentials =
                new Dictionary<string, Tuple<string, string>>(StringComparer.OrdinalIgnoreCase);
            try
            {
                _doc.Load(_configPath);
                XmlNodeList userNodes = _doc.SelectNodes(XML_SERVER_INFO_LOCATION);
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
            IDictionary<string, Tuple<string, string>> userCredentials = GetUserCredentials();
            if (userCredentials.ContainsKey(department))
            {
                return userCredentials[department].Item1;
            }
            return "";
        }

        public string GetPassword(string department)
        {
            IDictionary<string, Tuple<string, string>> userCredentials = GetUserCredentials();
            if (userCredentials.ContainsKey(department))
            {
                string password = userCredentials[department].Item2;
                if (password.Length > 0)
                {
                    password = CredentialsManager.DecryptText(password);
                }
                return password;
            }
            return "";
        }

        public bool SaveCredentials(string department, string username, string password)
        {
            if (password.Length == 0)
            {
                return false;                
            }
            password = CredentialsManager.EncryptText(password);
            try
            {
                _doc.Load(_configPath);
                string select = XML_SERVER_INFO_LOCATION + "[@Department='" + department + "']";
                XmlNodeList userNodes = _doc.SelectNodes(select);

                XmlNode userNode = userNodes[0];

                userNode.SelectNodes("username").Item(0).InnerText = username;
                userNode.SelectNodes("password").Item(0).InnerText = password;

                _doc.Save(this._configPath);
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
