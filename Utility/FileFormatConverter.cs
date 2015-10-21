using Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SSH_Print
{    
    public static class CommandsFactory
    {
        private const string PRINTING_COMMAND_TEMPLATE = @"lp -d {0} ""{1}""";
        private const string REMOVE_COMMAND_TEMPLATE = @"rm -f ""{0}""";
        private const string CHANGE_DIR_COMMMAND_TEMPLATE = @"cd ""{0}""";
        private const string CONVERT_COMMAND_TEMPLATE = @"oowriter -convert-to pdf:writer_pdf_Export ""{0}""";

        private static string GetFileNameAsPdf(string fileName)
        {
            string nameWithoutExtentsion = Path.GetFileNameWithoutExtension(fileName);
            return nameWithoutExtentsion + ".pdf";
        }

        public static List<string> CreateChangeFileFormatCommand(string fileName)
        {
            List<string> ret = new List<string>();
            string extension = Path.GetExtension(fileName).ToLower();
            if (ConstFields.SUPPORTED_CONVERTING_TO_PDF_EXTENSION.Contains(extension))
            {
                ret.Add(String.Format(CONVERT_COMMAND_TEMPLATE, fileName));
            }
            return ret;
        }
        public static List<string> CreateRemoveFileCommand(string fileName)
        {
            List<string> ret = new List<string>();
            ret.Add(String.Format(REMOVE_COMMAND_TEMPLATE, fileName));
            if (CreateChangeFileFormatCommand(fileName).Count > 0)
            {
                ret.Add(String.Format(REMOVE_COMMAND_TEMPLATE, GetFileNameAsPdf(fileName)));
            }
            return ret;
        }
        public static List<string> CreateChangeDirectoryCommand(string directory)
        {
            return new List<string>()
            {
                String.Format(CHANGE_DIR_COMMMAND_TEMPLATE, directory)
            };
        }
        public static List<string> CreatePrintingCommand(string printerName,
            string fileName, IList<string> options)
        {
            if (CreateChangeFileFormatCommand(fileName).Count > 0)
            {
                return new List<string>()
                {
                    String.Format(PRINTING_COMMAND_TEMPLATE, printerName, GetFileNameAsPdf(fileName))
                        + " " + string.Join(" ", options)
                };
            }
            return new List<string>()
            {
                String.Format(PRINTING_COMMAND_TEMPLATE, printerName, fileName)
                    + " " + string.Join(" ", options)
            };
        }
    }
}
