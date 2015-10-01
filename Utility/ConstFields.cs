using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utility
{
    public static class ConstFields
    {
        public const string WEBSITE_DIRECTORY = "http://web.cse.ohio-state.edu/~zhante/";
        public const string LEGACY_REGISTRY_ROOT = "*\\shell\\OSU Printer";
        public const string TEMP_PRINT_DIRECTORY = "temp_print";

        public const int SFTP_PORT = 22;

        public const string PRINTER_LIST_FILE_NAME = "OSU_printers.json";
        public const string CONFIGRATION_FILE_NAME = "config.xml";
        public const string SSH_PRINT_FILE_NAME = "SSH_Print.exe";
        public const string SHELL_REG_MANAGER_FILE_NAME = "srm.exe";
        public const string PRINTER_SHELL_EXT_FILE_NAME = "PrinterShellExtension.dll";

        public const string SHELL_EXT_GUID = "278a069a-65aa-36f7-a4f3-80b97eb30838";

        public static readonly string PRINTER_LIST_URL = Path.Combine(WEBSITE_DIRECTORY, PRINTER_LIST_FILE_NAME);
        public static readonly string CONFIGRATION_FILE_URL = Path.Combine(WEBSITE_DIRECTORY, CONFIGRATION_FILE_NAME);

        public static readonly string[] SUPPORTED_CONVERTING_TO_PDF_EXTENSION = { ".doc", ".docx", ".xls", ".xlsx" };

        public static readonly string REGASM_PATH_32BIT =
            Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"),
                @"Microsoft.NET\Framework\v4.0.30319\regasm.exe");
        public static readonly string REGASM_PATH_64BIT =
            Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"),
                @"Microsoft.NET\Framework64\v4.0.30319\regasm.exe");

        public const string PRINTER_IDENTIFIER_FILED = "Name";
    }
}
