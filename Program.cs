using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ComponentPro.Net;
using ComponentPro.IO;

namespace SftpSamples
{
    static class ConsoleUtil
    {
        public const ConsoleColor TextColorResponse = ConsoleColor.DarkGray; // Text color for response logs.
        public const ConsoleColor TextColorError = ConsoleColor.Red; // Text color for error logs.
        public const ConsoleColor TextColorInfo = ConsoleColor.Green; // Text color for information logs.

        /// <summary>
        /// Writes line a string to the console screen with the specified color.
        /// </summary>
        /// <param name="color">The text color.</param>
        /// <param name="msg">The string.</param>
        public static void WriteLine(ConsoleColor color, string msg)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes line a string to the console with the specified color.
        /// </summary>
        /// <param name="color">The text color.</param>
        /// <param name="msg">The string format.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void WriteLine(ConsoleColor color, string msg, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg, args);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes line an error message to the console.
        /// </summary>
        /// <param name="msg">The error message.</param>
        public static void WriteError(string msg)
        {
            Console.ForegroundColor = TextColorError;
            Console.WriteLine("ERROR: " + msg);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes line an information to the console.
        /// </summary>
        /// <param name="msg">The information to write.</param>
        public static void WriteInfo(string msg)
        {
            Console.ForegroundColor = TextColorInfo;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes line a string to the console with the specified color.
        /// </summary>
        /// <param name="msg">The string format.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void WriteInfo(string msg, params object[] args)
        {
            Console.ForegroundColor = TextColorInfo;
            Console.WriteLine(msg, args);
            Console.ResetColor();
        }

        /// <summary>
        /// Parses a string into an array of parameters.
        /// </summary>
        /// <param name="param">The string to parse.</param>
        /// <returns>Parsed parameters.</returns>
        public static string[] ParseParams(string param)
        {
            List<string> arr = new List<string>();

            int start = 0;
            bool instr = false;
            char pc = ' ';

            for (int i = 0; i < param.Length; i++)
            {
                char c = param[i];

                // Start a string?
                if (c == '"')
                    instr = !instr;
                else if (c == ' ' && pc != ' ' && !instr)
                {
                    // Removes all leading and trailing occurrences of a set of "space, tab and qoute".
                    string p = param.Substring(start, i - start).Trim(new char[] { '\t', ' ', '"' });
                    // Add to the parameters list.
                    arr.Add(p);
                    start = i + 1;
                }
                pc = c;
            }

            string s = param.Substring(start).Trim(new char[] { '\t', ' ', '"' });
            arr.Add(s);

            return arr.ToArray();
        }
    }

    internal class SftpConsoleClient
    {
        private readonly Sftp _client; // FtpClient object.
        private DateTime _operationTime = DateTime.MinValue; // Start time of an operation.

        public SftpConsoleClient()
        {
            // Create Sftp object and set event handlers
            _client = new Sftp();
            _client.Timeout = 25000;
            _client.CommandResponse += CommandResponse;
            _client.Progress += Progress;
        }

        [STAThread]
        private static void Main(string[] args)
        {
            ComponentPro.Licensing.Common.LicenseManager.SetLicenseKey(ComponentPro.TrialLicenseKey.Key);

            // Print out copyright.
            Console.ForegroundColor = ConsoleUtil.TextColorInfo;
            Console.WriteLine("UltimateSftp Console Client Demo");
            Console.WriteLine("UltimateSftp is available at http://www.componentpro.com");
            Console.WriteLine();
            Console.ResetColor();

            SftpConsoleClient client = new SftpConsoleClient();

            // If the program started with parameters.
            if (args.Length > 0)
            {
                try
                {
                    // Open a new connection and authenticate with the specified parameters.
                    if (client.OpenConnection(args))
                        client.AuthenticateUser(null);
                }
                catch (Exception e)
                {
                    ConsoleUtil.WriteError(e.Message);
                }
            }

            // Go to the main loop.
            client.MainLoop();
        }

        /// <summary>
        /// Processes the parameter list and return a list of options and params. An option is recognized when it has '-' at the beginning.
        /// </summary>
        /// <param name="parameters">The list of parameters.</param>
        /// <param name="min">The minimum number of items in the input param list (for validation).</param>
        /// <param name="max">The maximum number of items in the input param list (for validation)</param>
        /// <param name="minopts">The minimum number of options (for validation)</param>
        /// <param name="maxopts">The maximum number of options (for validation)</param>
        /// <param name="outOptions">The output option list.</param>
        /// <param name="minparams">The minimum number of parameters (for validation)</param>
        /// <param name="maxparams">The maximum number of parameters (for validation)</param>
        /// <param name="outParams">The output parameter list.</param>
        /// <returns>true if the given input param list is valid; otherwise is false.</returns>
        static bool GetOptionsAndParams(string[] parameters, int min, int max, int minopts, int maxopts, out string[] outOptions, int minparams, int maxparams, out string[] outParams)
        {
            // If the input parameter is not specified.
            if (parameters == null)
            {
                outOptions = null;
                outParams = null;
                return min == 0;
            }

            if (parameters.Length < min || parameters.Length > max)
            {
                outOptions = null;
                outParams = null;
                return false;
            }

            List<string> opts = new List<string>();
            List<string> prs = new List<string>();

            foreach (string s in parameters)
            {
                if (s[0] == '-')
                    opts.Add(s);
                else
                {
                    prs.Add(s);
                }
            }

            bool correct = opts.Count >= minopts && opts.Count <= maxopts && 
                           prs.Count >= minparams && prs.Count <= maxparams;

            if (correct)
            {
                outOptions = opts.Count == 0 ? null : opts.ToArray();
                outParams = prs.Count == 0 ? null : prs.ToArray();
            }
            else
            {
                outOptions = null;
                outParams = null;
            }

            return correct;
        }

        /// <summary>
        /// Reads password line.
        /// </summary>
        /// <returns>The read password.</returns>
        public static string ReadLine()
        {
            StringBuilder line = new StringBuilder();
            bool complete = false;

            while (!complete)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        complete = true;
                        break;

                    case ConsoleKey.Backspace:
                        if (line.Length > 0)
                        {
                            line = line.Remove(line.Length - 1, 1);
                            Console.Write(key.KeyChar);
                            Console.Write(" ");
                            Console.Write(key.KeyChar);
                        }
                        break;

                    default:
                        if ((key.KeyChar >= 0x20) || (key.Key == ConsoleKey.Tab))
                        {
                            line = line.Append(key.KeyChar);
                            Console.Write("*");
                        }
                        break;
                }
            }

            Console.WriteLine();
            return line.ToString();
        }

        /// <summary>
        /// Handles the client's CommandResponse event.
        /// </summary>
        /// <param name="sender">The client object.</param>
        /// <param name="e">The event arguments.</param>
        public void CommandResponse(object sender, CommandResponseEventArgs e)
        {
            if (e.Response != null)
                ConsoleUtil.WriteLine(ConsoleUtil.TextColorResponse, e.Response);
        }

        /// <summary>
        /// Handles the client's Progress event.
        /// </summary>
        /// <param name="sender">The client object.</param>
        /// <param name="e">The event arguments.</param>
        public void Progress(object sender, FileSystemProgressEventArgs e)
        {
            switch (e.State)
            {
                case TransferState.Uploading:
                case TransferState.Downloading:
                    if ((DateTime.Now - _operationTime).TotalSeconds <= 1) return;

                    ConsoleUtil.WriteInfo(string.Format("{0:00.0}% completed.", e.Percentage));
                    _operationTime = DateTime.Now;
                    break;

                case TransferState.FileUploaded:
                case TransferState.FileDownloaded:
                    ConsoleUtil.WriteInfo(string.Format("{0:00.0}% completed.", 100.0f));
                    break;
            }
        }

        /// <summary>
        /// Shows all supported commands.
        /// </summary>
        private static void ShowHelp()
        {
            Console.ForegroundColor = ConsoleUtil.TextColorInfo;
            Console.WriteLine("!         disconnect   rmdir       timedif");
            Console.WriteLine("?         exit         user        cls");
            Console.WriteLine("ascii     help         connect     del");
            Console.WriteLine("ls        get          put         chmod");
            Console.WriteLine("binary    mkdir        rget");
            Console.WriteLine("bye       open         ren");
            Console.WriteLine("cd        rput         putdir");
            Console.WriteLine("close     pwd          getdir");
            Console.WriteLine("dir       quit         calc");
            Console.ResetColor();
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        private void CloseConnection()
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            ConsoleUtil.WriteInfo("Disconnecting...");
            _client.Disconnect();
        }

        /// <summary>
        /// Opens a new connection.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <returns>true if successful; otherwise is false.</returns>
        private bool OpenConnection(string[] parameters)
        {
            try
            {
                string host;

                if (_client.State != RemoteFileSystemState.Disconnected)
                {
                    ConsoleUtil.WriteError("Already connected. Disconnect first.");
                    return false;
                }

                if (parameters == null || parameters.Length == 0)
                {
                    ConsoleUtil.WriteInfo("Usage: hostname [port]\r\n   port       The server port (eg. 22).");
                    while (true)
                    {
                        Console.Write("Connect to: ");
                        host = Console.ReadLine();
                        if (host.Trim().Length == 0)
                            ConsoleUtil.WriteError("Host name cannot be empty");
                        else
                        {
                            break;
                        }
                    }

                    parameters = ConsoleUtil.ParseParams(host);
                    if (parameters.Length == 0)
                    {
                        ConsoleUtil.WriteError("Host cannot be empty");
                        return false;
                    }
                }

                if (parameters.Length > 3)
                {
                    ConsoleUtil.WriteInfo("Usage: hostname [port]\r\n   port       The server port (eg. 22).");
                    return false;
                }

                host = parameters[0];
                int port = 0;
                switch (parameters.Length)
                {
                    case 1: // only server name is specified.
                        port = 22;
                        break;
                    case 2: // server name and port are specified
                        try
                        {
                            port = int.Parse(parameters[1]);
                        }
                        catch
                        {
                            port = -1;
                        }
                        if (port <= 0 || port > 65535)
                        {
                            ConsoleUtil.WriteError("Invalid port number.");
                            return false;
                        }
                        break;
                }

                _client.Connect(host, port);
                return true;
            }
            catch (Exception e)
            {
                ConsoleUtil.WriteError(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Authenticates user. If the specified authentication parameter is not specified, it will prompt user for username and password.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        private void AuthenticateUser(string[] parameters)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            string user = null;

            if (parameters == null || parameters.Length == 0)
            {
                while (true)
                {
                    Console.Write("User: ");
                    user = Console.ReadLine();
                    if (user.Trim().Length == 0)
                    {
                        ConsoleUtil.WriteError("User name cannot be empty");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (parameters.Length != 1)
            {
                ConsoleUtil.WriteInfo("Usage: auth [username]");
            }
            else
            {
                user = parameters[0];
            }

            string pass;
            while (true)
            {
                Console.Write("Password: ");
                pass = ReadLine();
                if (pass.Trim().Length == 0)
                {
                    ConsoleUtil.WriteError("Password cannot be empty");
                }
                else
                {
                    break;
                }
            }

            // Login to the server.
            _client.Authenticate(user, pass);
        }

        /// <summary>
        /// Shows the current working folder on the remote server.
        /// </summary>
        private void ShowCurrentDirectory()
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            string cdir = _client.GetCurrentDirectory();
            ConsoleUtil.WriteInfo("Current directory: \"" + cdir + "\"");
        }

        /// <summary>
        /// Sets file permissions.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        private void SetFilePermissions(string[] parameters)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            string[] prs;
            string[] opts;
            // Validate and process parameters.
            if (!GetOptionsAndParams(parameters, 2, 3, 0, 1, out opts, 2, 2, out prs))
            {
                ConsoleUtil.WriteInfo("Usage: chmod [-r] mode names\r\n   -r           Set permissions of specified files from all subdirectories.\r\n   mode         The desired numeric permissions (e.g 644 - This gives the file\r\n                read/write by the owner and only read by everyone else\r\n                (-rw-r--r--)).\r\n   names        Specifies a list of one or more files or directories.\r\n                Wildcards may be used to set multiple files. If a directory\r\n                is specified, all files within the directory will be set.");
                return;
            }

            bool r = opts != null && opts[0] == "-r";
            string mode = prs[0];
            string target = prs[1];

            string path = FileSystemPath.GetRemoteDirectoryName(target);
            string file = FileSystemPath.GetRemoteFileName(target);

            if (r && file.IndexOfAny(new char[] { '*', '?' }) == -1)
            {
                path = target;
                file = "*.*";
            }

            SftpFileAttributes attr = new SftpFileAttributes();
            uint p;
            try
            {
                p = Convert.ToUInt16(mode, 16);
                if (p > 0xFFF)
                {
                    ConsoleUtil.WriteError("Invalid permissions");
                    return;
                }
            }
            catch (Exception)
            {
                ConsoleUtil.WriteError("Invalid permissions");
                return;
            }
            attr.Permissions = (SftpFilePermissions)p;
            if (file.IndexOfAny(new char[] { '*', '?' }) != -1)
            {
                // Perform on multiple files.
                _client.SetMultipleFilesAttributes(path, attr, r, new NameSearchCondition(file));
            }
            else
            {
                // Perform on a single file.
                _client.SetFileAttributes(target, attr);
            }
        }

        /// <summary>
        /// Changes the current working folder.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <param name="rawParams">The raw input parameters string.</param>
        private void SetCurrentDirectory(string[] parameters, string rawParams)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            if (parameters == null || parameters.Length >= 1)
            {
                _client.SetCurrentDirectory(rawParams);
            }

            // Show the current working remote path.
            ShowCurrentDirectory();
        }

        /// <summary>
        /// Moves up one level.
        /// </summary>
        private void SetCurrentDirectoryUp()
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            _client.SetCurrentDirectory("..");
            ShowCurrentDirectory();
        }

        /// <summary>
        /// Deletes remote files.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        private void DeleteDirectory(string[] parameters)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            string[] prs;
            string[] opts;
            if (!GetOptionsAndParams(parameters, 1, 2, 0, 1, out opts, 1, 1, out prs))
            {
                ConsoleUtil.WriteInfo("Usage: del [-r] names\r\n   -r           Delete specified files from all subdirectories.\r\n   names        Specifies a list of one or more files or directories.\r\n                Wildcards may be used to delete multiple files. If a directory\r\n                is specified, all files within the directory will be deleted.");
                return;
            }

            bool r = opts != null && opts[0] == "-r";
            string target = prs[0];

            string path = FileSystemPath.GetRemoteDirectoryName(target);
            string file = FileSystemPath.GetRemoteFileName(target);

            if (r && file.IndexOfAny(new char[] { '*', '?' }) == -1)
            {
                path = target;
                file = "*.*";
            }

            if (file.IndexOfAny(new char[] {'*', '?'}) != -1)
            {
                if (file == "*.*")
                {
                    string s;
                    do
                    {
                        Console.Write("Are you sure you want to delete all files [y/n]?");
                        s = Console.ReadLine();
                        if (s == "n")
                            return;
                    } while (s != "y");
                }
                _client.Delete(path, true, null, false, r, true, file);
            }
            else
            {
                _client.DeleteFile(target);
            }
        }

        /// <summary>
        /// Calculates total size.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        private void CalculateFiles(string[] parameters)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            string[] prs;
            string[] opts;
            if (!GetOptionsAndParams(parameters, 1, 2, 0, 1, out opts, 1, 1, out prs))
            {
                ConsoleUtil.WriteInfo("Usage: calc [-r] names\r\n   -r           Calculate total size of the specified files from all\r\n                subdirectories.\r\n   names        Specifies a list of one or more files or directories.\r\n                Wildcards may be used to delete multiple files. If a directory\r\n                is specified, all files within the directory will be calculates.");
                return;
            }

            bool r = opts != null && opts[0] == "-r";
            string target = prs[0];

            string path = FileSystemPath.GetRemoteDirectoryName(target);
            string file = FileSystemPath.GetRemoteFileName(target);

            if (r && file.IndexOfAny(new char[] { '*', '?' }) == -1)
            {
                path = target;
                file = "*.*";
            }

            long total = file.IndexOfAny(new char[] { '*', '?' }) != -1 ? _client.GetDirectorySize(path, r, file) : _client.GetDirectorySize(target, r, null);

            ConsoleUtil.WriteInfo("Total size: " + FormatSize(total));
        }

        /// <summary>
        /// Closes the connection, if connected, and exit the program.
        /// </summary>
        private void Quit()
        {
            if (_client.State != RemoteFileSystemState.Disconnected)
                CloseConnection();

            ConsoleUtil.WriteInfo("Thanks for using UltimateSftp .NET");
        }

        /// <summary>
        /// Switches to the ASCII transfer mode.
        /// </summary>
        private void SwitchToAsciiTransMode()
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }
            ConsoleUtil.WriteInfo("Transfer mode is switched to ASCII.");
            _client.TransferType = FileTransferType.Ascii;
        }

        /// <summary>
        /// Switches to the BINARY transfer mode.
        /// </summary>
        private void SwitchToBinaryTransMode()
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            ConsoleUtil.WriteInfo("Transfer mode is switched to BINARY.");
            _client.TransferType = FileTransferType.Binary;
        }

        /// <summary>
        /// Makes a new directory
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <param name="rawParams">The raw parameter string.</param>
        private void MakeDirectory(string[] parameters, string rawParams)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            if (parameters.Length == 0)
            {
                ConsoleUtil.WriteInfo("Usage: mkdir dirname");
                return;
            }

            _client.CreateDirectory(rawParams);
        }

        /// <summary>
        /// Removes a remote directory.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        private void RemoveDirectory(string[] parameters)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected."); 
                return;
            }

            string[] prs;
            string[] opts;
            if (!GetOptionsAndParams(parameters, 1, 2, 0, 1, out opts, 1, 1, out prs))
            {
                ConsoleUtil.WriteInfo("Usage: rmdir [-r] dirname\r\n   -r      Removes all directories and files in the specified directory\r\n           in addition to the directory itself. Used to remove a directory tree.");
                return;
            }

            bool r = opts != null && opts[0] == "-r";
            string target = prs[0];

            if (opts != null)
                _client.DeleteDirectory(target, r);
            else
            {
                _client.DeleteDirectory(target);
            }
        }

        /// <summary>
        /// Lists all files.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <param name="rawParams">Additional parameters.</param>
        private void ShowFiles(string[] parameters, string rawParams)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            string cdir = null;
            _operationTime = DateTime.Now;
            if (!string.IsNullOrEmpty(rawParams))
            {
                cdir = _client.GetCurrentDirectory();
                _client.SetCurrentDirectory(rawParams);
            }

            try
            {
                SftpFileInfoCollection list = (SftpFileInfoCollection)_client.ListDirectory();
                long total = 0;
                int files = 0;

                for (int i = 0; i < list.Count; i++)
                {
                    SftpFileInfo item = list[i];

                    Console.Write(Convert.ToString((int) item.Permissions, 16).PadLeft(3, '0') + " ");

                    Console.Write(item.LastWriteTime.ToString("u").Substring(0, 16));
                    Console.Write(FormatSize(item.Length).PadLeft(10, ' '));
                    Console.Write(" {0}", item.Name);
                    Console.WriteLine();
                    total += item.Length;
                    if (item.Name != "." && item.Name != "..")
                        files++;
                }
                ConsoleUtil.WriteInfo("{0} sftp file(s) found - {1}", files, FormatSize(total));
            }
            finally
            {
                if (cdir != null)
                    _client.SetCurrentDirectory(cdir);
            }
        }

        /// <summary>
        /// Lists all file names.
        /// </summary>
        private void ListNameOnly()
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            _operationTime = DateTime.Now;
            SftpFileInfoCollection list = (SftpFileInfoCollection)_client.ListDirectory();

            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i].Name);
            }
        }

        /// <summary>
        /// Downloads a remote file.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <param name="rawParams">The raw parameter string.</param>
        private void GetFile(string[] parameters, string rawParams)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            if (parameters.Length == 0)
            {
                ConsoleUtil.WriteInfo("Usage: get remotePath");
                return;
            }

            try
            {
                _operationTime = DateTime.Now;
                string remotePath = rawParams;
                _client.DownloadFile(remotePath, Path.GetFileName(remotePath));
            }
            catch (Exception e)
            {
                if (e is SftpException)
                    throw;

                ConsoleUtil.WriteError(e.Message);
            }
        }

        /// <summary>
        /// Resumes download a remote file.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <param name="rawParams">The raw parameter string.</param>
        private void ResumeDownloadFile(string[] parameters, string rawParams)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            if (parameters.Length == 0)
            {
                ConsoleUtil.WriteInfo("Usage: rget remotePath");
                return;
            }

            string remotePath = rawParams;
            long result = _client.ResumeDownloadFile(remotePath, Path.GetFileName(remotePath));
            switch (result)
            {
                case -1:
                    ConsoleUtil.WriteInfo("Local file is longer than remote file");
                    break;
                case 0:
                    ConsoleUtil.WriteInfo("File has been already downloaded - local file size is equal to remote file size");
                    break;
                default:
                    ConsoleUtil.WriteInfo("{0} byte(s) downloaded", result);
                    break;
            }
        }

        /// <summary>
        /// Uploads an entire directory tree.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <param name="rawParams">The raw parameter string.</param>
        private void Upload(string[] parameters, string rawParams)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            if (parameters.Length == 0)
            {
                ConsoleUtil.WriteInfo("Usage: putdir localPath");
                return;
            }

            string localPath = rawParams;

            if (!Directory.Exists(localPath))
            {
                ConsoleUtil.WriteError("Path does not exist.");
                return;
            }

            _operationTime = DateTime.Now;
            string remotePath = Path.GetFileName(localPath);
            _client.Upload(localPath, remotePath);
        }

        /// <summary>
        /// Downloads an entire directory tree.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <param name="rawParams">The raw parameter string.</param>
        private void Download(string[] parameters, string rawParams)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            if (parameters.Length == 0)
            {
                ConsoleUtil.WriteInfo("Usage: getdir remotePath");
                return;
            }

            string remotePath = rawParams;

            _operationTime = DateTime.Now;
            string localPath = Path.GetFileName(remotePath);
            _client.Download(remotePath, localPath);
        }

        /// <summary>
        /// Shows the time difference between the client and the server.
        /// </summary>
        public void GetTimeDif()
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            TimeSpan ts = _client.GetServerTimeDifference();
            ConsoleUtil.WriteInfo("Time difference between the client PC and the FTP server: " + ts.ToString());
        }

        /// <summary>
        /// Uploads a single file.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <param name="rawParams">The raw parameter string.</param>
        private void UploadFile(string[] parameters, string rawParams)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            if (parameters.Length == 0)
            {
                ConsoleUtil.WriteInfo("Usage: put localPath");
                return;
            }

            string localPath = rawParams;

            try
            {
                _operationTime = DateTime.Now;
                _client.UploadFile(localPath, Path.GetFileName(localPath));
            }
            catch (Exception e)
            {
                if (e is SftpException)
                    throw;

                ConsoleUtil.WriteError(e.Message);
            }
        }

        /// <summary>
        /// Resumes upload a file.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        /// <param name="rawParams">The raw parameter string.</param>
        private void ResumePutFile(string[] parameters, string rawParams)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            if (parameters.Length == 0)
            {
                ConsoleUtil.WriteInfo("Usage: rput localPath");
                return;
            }

            _operationTime = DateTime.Now;
            string localPath = rawParams;
            long result = _client.ResumeUploadFile(localPath, Path.GetFileName(localPath));

            switch (result)
            {
                case -1:
                    ConsoleUtil.WriteInfo("Remote file is longer than local file");
                    break;
                case 0:
                    ConsoleUtil.WriteInfo("File has been already uploaded - remote file size is equal to local file size");
                    break;
                default:
                    ConsoleUtil.WriteInfo("{0} byte(s) uploaded", result);
                    break;
            }
        }

        /// <summary>
        /// Renames a remote file.
        /// </summary>
        /// <param name="parameters">The input parameters.</param>
        private void RenameFile(string[] parameters)
        {
            if (_client.State == RemoteFileSystemState.Disconnected)
            {
                ConsoleUtil.WriteError("Not connected.");
                return;
            }

            if (parameters.Length != 2)
            {
                ConsoleUtil.WriteInfo("Usage: ren oldPath newPath");
                return;
            }

            _client.Rename(parameters[0], parameters[1]);
        }

        /// <summary>
        /// Processes user input commands.
        /// </summary>
        public void MainLoop()
        {
            while (true)
            {
                Console.Write("Sftp> ");
                string command = Console.ReadLine().Trim();
                string[] arr = ConsoleUtil.ParseParams(command);

                string cmd = arr[0];
                if (cmd.Length == 0)
                    continue;

                string[] parameters = new string[arr.Length - 1];
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = arr[i + 1];
                }

                int n = command.IndexOfAny(new char[] { '\t', ' ' });
                string rawParams;
                if (n != -1)
                {
                    rawParams = command.Substring(n + 1).Trim(new char[] { '\t', ' ', '"' });
                }
                else
                    rawParams = string.Empty;

                try
                {
                    switch (cmd)
                    {
                        case "bye":
                            goto case "quit";
                        case "!":
                            goto case "quit";
                        case "exit":
                            goto case "quit";
                        case "quit":
                            Quit();
                            return;
                        case "disconnect":
                            goto case "close";
                        case "close":
                            CloseConnection();
                            break;
                        case "connect":
                            goto case "open";
                        case "open":
                            if (!OpenConnection(parameters)) break;
                            parameters = null;
                            goto case "user";
                        case "user":
                            AuthenticateUser(parameters);
                            break;
                        case "?":
                            goto case "help";
                        case "help":
                            ShowHelp();
                            break;
                        case "del":
                            DeleteDirectory(parameters);
                            break;
                        case "ascii":
                            SwitchToAsciiTransMode();
                            break;
                        case "binary":
                            SwitchToBinaryTransMode();
                            break;
                        case "cd":
                            SetCurrentDirectory(parameters, rawParams);
                            break;
                        case "cd..":
                            SetCurrentDirectoryUp();
                            break;
                        case "cd\\":
                            SetCurrentDirectory(null, "/");
                            break;
                        case "cd/":
                            SetCurrentDirectory(null, "/");
                            break;
                        case "pwd":
                            ShowCurrentDirectory();
                            break;
                        case "mkdir":
                            MakeDirectory(parameters, rawParams);
                            break;
                        case "rmdir":
                            RemoveDirectory(parameters);
                            break;
                        case "get":
                            GetFile(parameters, rawParams);
                            break;
                        case "put":
                            UploadFile(parameters, rawParams);
                            break;
                        case "rget":
                            ResumeDownloadFile(parameters, rawParams);
                            break;
                        case "rput":
                            ResumePutFile(parameters, rawParams);
                            break;
                        case "putdir":
                            Upload(parameters, rawParams);
                            break;
                        case "getdir":
                            Download(parameters, rawParams);
                            break;
                        case "timedif":
                            GetTimeDif();
                            break;
                        case "dir":
                            ShowFiles(parameters, rawParams);
                            break;
                        case "ls":
                            if (parameters.Length == 1 && parameters[0] == "-l")
                                ShowFiles(null, null);
                            else
                                ListNameOnly();
                            break;
                        case "cls":
                            Console.Clear();
                            break;
                        case "ren":
                            RenameFile(parameters);
                            break;
                        case "calc":
                            CalculateFiles(parameters);
                            break;
                        case "chmod":
                            SetFilePermissions(parameters);
                            break;
                        default:
                            ConsoleUtil.WriteError(string.Format("'{0}' is not recognized as a supported command.", cmd));
                            break;
                    }
                }
                catch (Exception exc)
                {
                    SftpException e = exc as SftpException;
                    ConsoleUtil.WriteError(exc.Message);
                    if (e != null && e.Status != SftpExceptionStatus.ProtocolError && e.Status != SftpExceptionStatus.ConnectionClosed && e.Message.IndexOf("not supported") == 0)
                    {
                        _client.Disconnect();
                    }
                }
            }
        }

        /// <summary>
        /// Returns a formatted file size in bytes, kbytes, or mbytes.
        /// </summary>
        /// <param name="size">The input file size.</param>
        /// <returns>The formatted file size.</returns>
        public static string FormatSize(long size)
        {
            if (size < 1024)
                return size + " B";
            if (size < 1024 * 1024)
                return string.Format("{0:#.#} KB", size / 1024.0f);
            return size < 1024 * 1024 * 1024 ? string.Format("{0:#.#} MB", size / 1024.0f / 1024.0f) : string.Format("{0:#.#} GB", size / 1024.0f / 1024.0f / 1024.0f);
        }
    }
}