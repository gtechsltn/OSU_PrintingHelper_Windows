using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Renci.SshNet;
using Utility;

namespace SSH_Print
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                System.Console.WriteLine("Please don't open this file directly.");
                System.Console.ReadLine();
                return;
            }
            string FilePath = args[0];
            string PrinterName = args[1];

            ConfigLoader Loader = new ConfigLoader(ConstFields.CONFIGRATION_FILE_NAME,
                ConstFields.PRINTER_MAP_URL);
            if (!Loader.LoadConfig())
            {
                return;
            }
            string[] Config = Loader.GetServerConfig(PrinterName);

            if (Config == null)
            {
                Console.WriteLine("Cannot find the correspoding department of printer: " + PrinterName);
                Console.ReadLine();
                return;
            }
            if (Config[1].Length == 0 || Config[2].Length == 0)
            {
                Console.WriteLine("Username or password not found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Uploading file, please wait...");
            NetworkHandler Handler = new NetworkHandler(Config[0], Config[1], Config[2]);

            if (!Handler.UploadFile(FilePath))
            {
                Console.WriteLine("Fail to upload file to the server");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Printing file, please wait...");
            string FileName = Path.GetFileName(FilePath);
            Handler.PrintFile(FileName, PrinterName);
        }
    }
}
