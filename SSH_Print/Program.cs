using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Renci.SshNet;
using Newtonsoft.Json;
using DataUtility;

namespace SSH_Print
{
    class Program
    {
        const string CONFIGRATION_FILE_NAME = "server_config.xml";
        const string DEPT_PRINTER_MAP_ADDRESS = "http://web.cse.ohio-state.edu/~zhante/printer_map.json";
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                System.Console.WriteLine("Please don't open this file directly.");
                System.Console.ReadLine();
                return;
            }
            string filePath = args[0];
            string printerName = args[1];

            ConfigLoader loader = new ConfigLoader(CONFIGRATION_FILE_NAME, DEPT_PRINTER_MAP_ADDRESS);
            if (!loader.LoadConfig())
            {
                return;
            }
            string[] config = loader.GetConfig(printerName);

            if (config == null)
            {
                Console.WriteLine("Cannot find the correspoding department of printer: " + printerName);
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Uploading file, please wait...");
            NetworkHandler handler = new NetworkHandler(config[0], config[1], config[2]);
            if (!handler.uploadFile(filePath))
            {
                Console.WriteLine("Fail to upload file to the server");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Printing file, please wait...");
            string fileName = Path.GetFileName(filePath);
            handler.printFile(fileName, printerName);
        }
    }
}
