using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Renci.SshNet;
using Newtonsoft.Json;

namespace SSH_Print
{
    class NetworkHandler
    {
        const string TEMP_PRINT_DIRECTORY = "temp_print";
        const int SFTP_PORT = 22;
        string[] SUPPORTED_CONVERTING_TO_PDF_EXTENSION = {".doc", ".docx", ".xls", ".xlsx"};

        string address;
        string username;
        string password;
        public NetworkHandler(string address, string username, string password)
        {
            this.address = address;
            this.username = username;
            this.password = password;
        }
        public bool uploadFile(string filePath)
        {
            ConnectionInfo connectionInfo = new PasswordConnectionInfo(address, SFTP_PORT, username, password);
            try
            {
                using (var sftp = new SftpClient(connectionInfo))
                {
                    sftp.Connect();
                    using (var file = File.OpenRead(filePath))
                    {
                        if (!sftp.Exists(TEMP_PRINT_DIRECTORY))
                        {
                            sftp.CreateDirectory(TEMP_PRINT_DIRECTORY);
                        }
                        sftp.ChangeDirectory(TEMP_PRINT_DIRECTORY);
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
        string getFileNameAsPdf(string fileName)
        {
            string nameWithoutExtentsion = Path.GetFileNameWithoutExtension(fileName);
            return nameWithoutExtentsion + ".pdf";           
        }
        Tuple<string, string> getChangeFileFormatCommand(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            string convertCommand =  @"soffice --headless --convert-to pdf ""{0}""; ";
            string removeCommand = @"rm -f ""{0}""";

            if (SUPPORTED_CONVERTING_TO_PDF_EXTENSION.Contains(extension))
            {
                convertCommand = String.Format(convertCommand, fileName);
                removeCommand = String.Format(removeCommand, getFileNameAsPdf(fileName));
                return new Tuple<string, string>(convertCommand, removeCommand);
            }
            return new Tuple<string, string>("", "");
        }
        public void printFile(string fileName, string printerName)
        {
            string changeDirectoryCommand = @"cd temp_print; ";
            string removeFileCommand = String.Format(@"rm -f ""{0}""; ", fileName);
            Tuple<string, string> tuple = getChangeFileFormatCommand(fileName);
            string convertPdfCommand = tuple.Item1;
            string removePdfCommand = tuple.Item2;
            string generalNanme = fileName;

            if (convertPdfCommand != "")
            {
                generalNanme = getFileNameAsPdf(fileName);
            }
            string printingCommand = String.Format(@"lp -d {0} ""{1}""; ", printerName, generalNanme);

            try
            {
                using (var client = new SshClient(address, username, password))
                {
                    client.Connect();
                    string commmand = changeDirectoryCommand + convertPdfCommand
                        + printingCommand + removeFileCommand + removePdfCommand;

                    SshCommand result = client.RunCommand(commmand);
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
