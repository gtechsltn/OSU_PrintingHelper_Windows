using Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SSH_Print
{    
    public static class FileFormatConverter
    {
        public static string GetFileNameAsPdf(string fileName)
        {
            string nameWithoutExtentsion = Path.GetFileNameWithoutExtension(fileName);
            return nameWithoutExtentsion + ".pdf";
        }
        public static IList<string> GetChangeFileFormatCommand(string fileName)
        {
            IList<string> ret = new List<string>();

            string extension = Path.GetExtension(fileName).ToLower();

            string convertCommand = @"oowriter -convert-to pdf:writer_pdf_Export ""{0}""; ";
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
