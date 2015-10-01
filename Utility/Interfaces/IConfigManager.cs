using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Interfaces
{
    interface IConfigManager
    {
        IDictionary<string, string[]> GetServerInfo();
        string[] GetServerConfig(string printerName);
        IDictionary<string, string> GetDepartmentMap();
        IList<Tuple<string, bool>> GetAllPrintingOptions();
        IList<string> GetEnabledPrintingOptions();
        bool SaveEnabledPrintingOptions(ISet<string> optionName);
        IList<Dictionary<string, string>> GetAllLoadedPrinters();
        bool SaveAllLoadedPrinters(IList<Dictionary<string, string>> printers);
        bool MovePrinterToFront(int PrinterPosition);
        IDictionary<string, Tuple<string, string>> GetUserCredentials();
        string GetUserName(string department);
        string GetPassword(string department);
        bool SaveCredentials(string department, string username, string password);
    }
}
