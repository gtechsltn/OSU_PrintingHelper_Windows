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
            string NameWithoutExtentsion = Path.GetFileNameWithoutExtension(fileName);
            return NameWithoutExtentsion + ".pdf";
        }
        public static List<string> GetChangeFileFormatCommand(string fileName)
        {
            List<string> ret = new List<string>();

            string extension = Path.GetExtension(fileName).ToLower();
            string ConvertCommand = @"soffice --headless --convert-to pdf ""{0}""; ";
            string RemoveCommand = @"rm -f ""{0}""";

            if (ConstFields.SUPPORTED_CONVERTING_TO_PDF_EXTENSION.Contains(extension))
            {
                ConvertCommand = String.Format(ConvertCommand, fileName);
                RemoveCommand = String.Format(RemoveCommand, GetFileNameAsPdf(fileName));
                ret.Add(ConvertCommand);
                ret.Add(RemoveCommand);
            }
            return ret;
        }

    }
}
