using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utility
{
    public class ConstFields
    {
        public const string WEBSITE_DIRECTORY = "http://web.cse.ohio-state.edu/~zhante/";
        public const string REGISTRY_ROOT = "*\\shell\\OSU Printer";
        public const string TEMP_PRINT_DIRECTORY = "temp_print";

        public const int SFTP_PORT = 22;

        public const string PRINTER_LIST_FILE_NAME = "OSU_printers.json";
        public const string PRINTER_MAP_FILE_NAME = "printer_map.json";
        public const string CONFIGRATION_FILE_NAME = "config.xml";
        public const string SSH_PRINT_FILE_NAME = "SSH_Print.exe";

        public static readonly string PRINTER_MAP_URL = Path.Combine(WEBSITE_DIRECTORY, PRINTER_MAP_FILE_NAME);
        public static readonly string PRINTER_LIST_URL = Path.Combine(WEBSITE_DIRECTORY, PRINTER_LIST_FILE_NAME);
        public static readonly string CONFIGRATION_FILE_URL = Path.Combine(WEBSITE_DIRECTORY, CONFIGRATION_FILE_NAME);

        public static readonly string[] SUPPORTED_CONVERTING_TO_PDF_EXTENSION = { ".doc", ".docx", ".xls", ".xlsx" };
    }
}
