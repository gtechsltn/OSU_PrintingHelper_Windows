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

        public NetworkHandler(string address, string username, string password)
        {
            this._address = address;
            this._username = username;
            this._password = password;
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
            return true;
        }

        string CreatePrintFileCommand(string fileName, string printerName)
        {
            ConfigManager manager = new ConfigManager(ConstFields.CONFIGRATION_FILE_NAME);
            IList<string>[] commandsArray = new List<string>[]
            {
                CommandsFactory.CreateChangeDirectoryCommand(ConstFields.TEMP_PRINT_DIRECTORY),
                CommandsFactory.CreateChangeFileFormatCommand(fileName),
                CommandsFactory.CreatePrintingCommand(printerName, fileName, manager.GetEnabledPrintingOptions()),
                CommandsFactory.CreateRemoveFileCommand(fileName),
            };

            string command = "";
            foreach (IList<string> strList in commandsArray)
            {
                if (strList.Count > 0)
                {
                    command += string.Join("; ", strList);
                    command += "; ";
                }
            }
            return command;
        }

        public void PrintFile(string fileName, string printerName)
        {
            string commands = CreatePrintFileCommand(fileName, printerName);
            try
            {
                using (var client = new SshClient(_address, _username, _password))
                {
                    client.Connect();
                    Console.WriteLine("Executing printing command, waiting for response...");
                    SshCommand result = client.RunCommand(commands);
                    string resultString = result.Result.Trim('\n', '\r', ' ');
                    Console.WriteLine("Response message is: " + resultString);

                    if (resultString.Contains("request id"))
                    {
                        Console.WriteLine("Your documents printing job is SUCCESSFULLY requested.");
                    }
                    else
                    {
                        Console.WriteLine("FAILED to executing printing command.");
                    }
                    client.Disconnect();
                }
            }
            catch (Renci.SshNet.Common.SshConnectionException)
            {
                Console.WriteLine("Cannot connect to the server.");
            }
            catch (System.Net.Sockets.SocketException)
            {
                Console.WriteLine("Unable to establish the socket.");
            }
            catch (Renci.SshNet.Common.SshAuthenticationException)
            {
                Console.WriteLine("Authentication of SSH session failed.");
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
