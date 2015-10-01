using Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SSH_Print
{    
    public class FileFormatConverter
    {
        public static string GetFileNameAsPdf(string fileName)
        {
            string nameWithoutExtentsion = Path.GetFileNameWithoutExtension(fileName);
            return nameWithoutExtentsion + ".pdf";
        }
        public static List<string> GetChangeFileFormatCommand(string fileName)
        {
            List<string> ret = new List<string>();

            string extension = Path.GetExtension(fileName).ToLower();

            //TODO: Contacting ECE for bug fixes.
            string convertCommand = @"soffice --headless --convert-to pdf ""{0}""; ";
            string removeCommand = @"rm -f ""{0}""";

            if (ConstFields.SUPPORTED_CONVERTING_TO_PDF_EXTENSION.Contains(extension))
            {
                convertCommand = String.Format(convertCommand, fileName);
                removeCommand = String.Format(removeCommand, GetFileNameAsPdf(fileName));
                ret.Add(convertCommand);
                ret.Add(removeCommand);
            }
            return ret;
        }

    }
}
