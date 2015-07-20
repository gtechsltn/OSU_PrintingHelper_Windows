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
    class NetworkHandler
    {
        string address;
        string username;
        string password;

        IList<string> CommandsList;

        public NetworkHandler(string address, string username, string password)
        {
            this.address = address;
            this.username = username;
            this.password = password;
            CommandsList = new List<string>();
        }
        public bool UploadFile(string filePath)
        {
            ConnectionInfo connectionInfo = new PasswordConnectionInfo(address, ConstFields.SFTP_PORT, username, password);
            try
            {
                using (var sftp = new SftpClient(connectionInfo))
                {
                    sftp.Connect();
                    using (var file = File.OpenRead(filePath))
                    {
                        if (!sftp.Exists(ConstFields.TEMP_PRINT_DIRECTORY))
                        {
                            sftp.CreateDirectory(ConstFields.TEMP_PRINT_DIRECTORY);
                        }
                        sftp.ChangeDirectory(ConstFields.TEMP_PRINT_DIRECTORY);
                        string filename = Path.GetFileName(filePath);
                        sftp.UploadFile(file, filename);
                    }
                    sftp.Disconnect();
                }
            }
            catch (Renci.SshNet.Common.SshConnectionException)
            {
                Console.WriteLine("Cannot connect to the server.");
                return false;
            }
            catch (System.Net.Sockets.SocketException)
            {
                Console.WriteLine("Unable to establish the socket.");
                return false;
            }
            catch (Renci.SshNet.Common.SshAuthenticationException)
            {
                Console.WriteLine("Authentication of SSH session failed.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
        }
        public void PrintFile(string FileName, string PrinterName)
        {
            string changeDirectoryCommand = @"cd " + ConstFields.TEMP_PRINT_DIRECTORY;
            CommandsList.Add(changeDirectoryCommand);

            IList<string> ConvertCommandList = FileFormatConverter.GetChangeFileFormatCommand(FileName);
            foreach (string command in ConvertCommandList)
            {
                CommandsList.Add(command);
            }
            string GeneralName = FileName;
            if (ConvertCommandList.Count > 0)
            {
                GeneralName = FileFormatConverter.GetFileNameAsPdf(FileName);
            }
            string PrintingCommand = String.Format(@"lp -d {0} ""{1}""", PrinterName, GeneralName);
            ConfigManager Loader = new ConfigManager(ConstFields.CONFIGRATION_FILE_NAME);
            PrintingCommand += (" " + string.Join(" ", Loader.GetEnabledPrintingOptions()));
            CommandsList.Add(PrintingCommand);

            string removeFileCommand = String.Format(@"rm -f ""{0}""", FileName);
            CommandsList.Add(removeFileCommand);

            try
            {
                using (var client = new SshClient(address, username, password))
                {
                    client.Connect();
                    string commmand = string.Join("; ", CommandsList);
                    SshCommand result = client.RunCommand(commmand);
                    CommandsList.Clear();
                    Console.WriteLine(commmand);
                    Console.WriteLine(result.Result);
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
