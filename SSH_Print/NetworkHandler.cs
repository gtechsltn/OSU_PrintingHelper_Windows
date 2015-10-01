using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Renci.SshNet;
using Utility;
using System.Diagnostics;
using Renci.SshNet.Sftp;
using System.Net;

namespace SSH_Print
{
    public class NetworkHandler
    {
        private string _address;
        private string _username;
        private string _password;

        private IList<string> _commandsList;
        private const string PRINTING_COMMAND_TEMPLATE = @"lp -d {0} ""{1}""";
        private const string REMOVE_COMMAND = @"rm -f ""{0}""";
        private const string CHANGE_DIR_COMMMAND_TEMPLATE = @"cd ""{0}""";

        public NetworkHandler(string address, string username, string password)
        {
            this._address = address;
            this._username = username;
            this._password = password;
            _commandsList = new List<string>();
        }
        public bool UploadFile(string filePath)
        {
            ConnectionInfo connectionInfo = new PasswordConnectionInfo(_address, ConstFields.SFTP_PORT, _username, _password);
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
        public void PrintFile(string fileName, string printerName)
        {
            string changeDirectoryCommand =
                String.Format(CHANGE_DIR_COMMMAND_TEMPLATE, ConstFields.TEMP_PRINT_DIRECTORY);
            _commandsList.Add(changeDirectoryCommand);

            IList<string> ConvertCommandList = FileFormatConverter.GetChangeFileFormatCommand(fileName);
            foreach (string command in ConvertCommandList)
            {
                _commandsList.Add(command);
            }
            string NameAsPdf = fileName;
            if (ConvertCommandList.Count > 0)
            {
                NameAsPdf = FileFormatConverter.GetFileNameAsPdf(fileName);
            }
            string PrintingCommand = String.Format(PRINTING_COMMAND_TEMPLATE, printerName, NameAsPdf);
            ConfigManager manager = new ConfigManager(ConstFields.CONFIGRATION_FILE_NAME);

            PrintingCommand += (" " + string.Join(" ", manager.GetEnabledPrintingOptions()));
            _commandsList.Add(PrintingCommand);

            string RemoveFileCommand = String.Format(REMOVE_COMMAND, fileName);
            _commandsList.Add(RemoveFileCommand);

            try
            {
                using (var client = new SshClient(_address, _username, _password))
                {
                    client.Connect();
                    string commmand = string.Join("; ", _commandsList);
                    Console.WriteLine("Executing printing command, waiting for response...");
                    SshCommand result = client.RunCommand(commmand);
                    _commandsList.Clear();
                    Console.WriteLine("Response message is: " + result.Result);
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }

        public async Task<bool> CheckConnectionAsync()
        {
            return await Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    using (var client = new SshClient(_address, _username, _password))
                    {
                        client.Connect();
                        client.Disconnect();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
                return true;
            });
        }
    }
}
