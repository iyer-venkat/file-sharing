//#define SHOWSPEED

using System;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using ComponentPro;
using ComponentPro.Net;
using ComponentPro.IO;
using ComponentPro.Diagnostics;

namespace SftpSamples
{
    public partial class SftpMulFiles : Form
    {
        private readonly bool _exception;
        private bool _connected;

        private LoginInfo _loginSettings;

        private ComponentPro.Samples.FileOperation _fileOpForm;

        public SftpMulFiles()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error");
                _exception = true;
            }

            XTrace.Default.Listeners.Add(new RichTextBoxTraceListener(txtLog));
            XTrace.Default.Level = TraceEventType.Information;
        }

        private void TransferFiles(bool download)        
        {
            if (!UpdateLoginInfo(true))
                return;

            EnableDialog(false);

            txtLog.Clear();

            try
            {
                toolStripStatusLabel.Text = string.Format("Connecting to {0}:{1}...", _loginSettings.SftpServer, _loginSettings.SftpPort);
                Application.DoEvents();

                WebProxyEx proxy = new WebProxyEx();
                client.Proxy = proxy;
                if (_loginSettings.ProxyServer.Length > 0 && _loginSettings.ProxyPort > 0)
                {
                    proxy.Server = _loginSettings.ProxyServer;
                    proxy.Port = _loginSettings.ProxyPort;
                    proxy.UserName = _loginSettings.ProxyUser;
                    proxy.Password = _loginSettings.ProxyPassword;
                    proxy.Domain = _loginSettings.ProxyDomain;
                    proxy.ProxyType = _loginSettings.ProxyType;
                    proxy.AuthenticationMethod = _loginSettings.ProxyMethod;
                }

                if (chkUtf8Encoding.Checked)
                    client.Encoding = Encoding.UTF8;
                else
                    client.Encoding = Encoding.Default;

                // Connect to the server.
                client.Connect(_loginSettings.SftpServer, _loginSettings.SftpPort);
                _connected = true;
            }
            catch (Exception exc)
            {
                Util.ShowError(exc);
                goto Finish;
            }

            try
            {
                toolStripStatusLabel.Text = string.Format("Logging in as {0}...", _loginSettings.UserName);
                Application.DoEvents();

                // Login in with the provided user name and password.
                if (string.IsNullOrEmpty(_loginSettings.PrivateKey))
                    client.Authenticate(_loginSettings.UserName, _loginSettings.Password);
                else
                {
                    ComponentPro.Samples.PasswordPrompt dlg = new ComponentPro.Samples.PasswordPrompt();
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (_loginSettings.Password.Length == 0)
                                _loginSettings.Password = null;
                            client.Authenticate(_loginSettings.UserName, _loginSettings.Password, new SecureShellPrivateKey(_loginSettings.PrivateKey, dlg.Password));
                        }
                        catch (Exception exc)
                        {
                            Util.ShowError(exc);
                            goto Finish;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password has not been provided\r\nThe connection will be closed", "Login");
                        goto Finish;
                    }
                }
            }
            catch (Exception exc)
            {
                Util.ShowError(exc);
                goto Finish;
            }

            try
            {
                Application.DoEvents();

                TransferOptions opt = new TransferOptions(
                    true, // Build directory structure before transferring - show total progress.
                    true, // Scan all files and subdirectories.
                    OptionValue.Auto, // Allow creating empty directories.
                    new NameSearchCondition(_loginSettings.SearchPattern), // Search for files that match the specified pattern in names.
                    FileOverwriteMode.Confirm, // Show the confirmation dialog when overwritting an existing file.
                    SymlinksResolveAction.Confirm // Show the confirmation dialog when a link is found.
                    );

                if (download)
                    client.Download(_loginSettings.RemoteDir, _loginSettings.LocalDir, opt);
                else
                    client.Upload(_loginSettings.LocalDir, _loginSettings.RemoteDir, opt);
            }
            catch (Exception exc)
            {
                Util.ShowError(exc);
                goto Finish;
            }

        Finish:
            client.Disconnect();
            _connected = false;
            EnableDialog(true);

            toolStripStatusLabel.Text = "Ready";
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            TransferFiles(true);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            TransferFiles(false);
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            client.Cancel();
        }

        #region Form

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_exception)
                this.Close();

            UpdateLoginForm();

            _fileOpForm = new ComponentPro.Samples.FileOperation();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_connected)
            {
                client.Cancel();
            }

            UpdateLoginInfo(false);
            _loginSettings.SaveConfig();

            base.OnClosing(e);
        }

        private void EnableDialog(bool enable)
        {
            // Enable/Disable controls.
            tabControlExt.Enabled = enable;
            btnDownload.Enabled = enable;
            btnUpload.Enabled = enable;
            btnAbort.Enabled = !enable;

            progressBarFile.Value = 0;
            progressBarTotal.Value = 0;

            _fileOpForm.Init();

            Util.EnableCloseButton(this, enable);
        }

        private void UpdateLoginForm()
        {
            _loginSettings = LoginInfo.LoadConfig();

            #region Login Info

            txtServer.Text = _loginSettings.SftpServer;
            txtPort.Text = _loginSettings.SftpPort.ToString();
            txtUserName.Text = _loginSettings.UserName;
            txtPassword.Text = _loginSettings.Password;
            txtRemoteDir.Text = _loginSettings.RemoteDir;
            txtLocalDir.Text = _loginSettings.LocalDir;

            chkUtf8Encoding.Checked = _loginSettings.Utf8Encoding;

            txtProxyHost.Text = _loginSettings.ProxyServer;
            txtProxyPort.Text = _loginSettings.ProxyPort.ToString();
            txtProxyUser.Text = _loginSettings.ProxyUser;
            txtProxyPassword.Text = _loginSettings.ProxyPassword;
            txtProxyDomain.Text = _loginSettings.ProxyDomain;
            cbxProxyMethod.SelectedIndex = (int)_loginSettings.ProxyMethod;
            cbxProxyType.SelectedIndex = (int)_loginSettings.ProxyType;

            txtPrivateKey.Text = _loginSettings.PrivateKey;

            txtSearchPattern.Text = _loginSettings.SearchPattern;

            #endregion
        }

        private bool UpdateLoginInfo(bool showError)
        {
            int port;
            try
            {
                port = int.Parse(txtPort.Text);
            }
            catch (Exception exc)
            {
                if (showError)
                    Util.ShowError(exc, "Invalid Port");
                return false;
            }
            if (port < 0 || port > 65535)
            {
                if (showError)
                    MessageBox.Show("Invalid port number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtServer.Text.Length == 0)
            {
                if (showError)
                    MessageBox.Show("Host name cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            int proxyport;
            try
            {
                proxyport = int.Parse(txtProxyPort.Text);
            }
            catch (Exception exc)
            {
                if (showError)
                    Util.ShowError(exc, "Invalid Proxy Port");
                return false;
            }
            if (proxyport < 0 || proxyport > 65535)
            {
                if (showError)
                    MessageBox.Show("Invalid port number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtLocalDir.Text))
            {
                if (showError)
                    MessageBox.Show("Local path cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                if (showError)
                    MessageBox.Show("SFTP User Name cannot be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text) && string.IsNullOrEmpty(txtPrivateKey.Text))
            {
                if (showError)
                    MessageBox.Show("You must specify either password or private key file or both of them", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            _loginSettings.SftpServer = txtServer.Text;
            _loginSettings.SftpPort = int.Parse(txtPort.Text);
            _loginSettings.UserName = txtUserName.Text;
            _loginSettings.Password = txtPassword.Text;
            _loginSettings.RemoteDir = txtRemoteDir.Text;
            _loginSettings.LocalDir = txtLocalDir.Text;

            _loginSettings.Utf8Encoding = chkUtf8Encoding.Checked;

            _loginSettings.ProxyServer = txtProxyHost.Text;
            _loginSettings.ProxyPort = int.Parse(txtProxyPort.Text);
            _loginSettings.ProxyUser = txtProxyUser.Text;
            _loginSettings.ProxyPassword = txtProxyPassword.Text;
            _loginSettings.ProxyDomain = txtProxyDomain.Text;
            _loginSettings.ProxyType = (ProxyType)cbxProxyType.SelectedIndex;
            _loginSettings.ProxyMethod = (ProxyHttpConnectAuthMethod)cbxProxyMethod.SelectedIndex;

            _loginSettings.PrivateKey = txtPrivateKey.Text;

            _loginSettings.SearchPattern = txtSearchPattern.Text;
            if (_loginSettings.SearchPattern.Length == 0)
                _loginSettings.SearchPattern = null;

            return true;
        }

        #endregion

        #region Login Form

        private void btnLocalDirBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "Select local folder to store file";
                dlg.SelectedPath = txtLocalDir.Text;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtLocalDir.Text = dlg.SelectedPath;
                }
            }
            catch (Exception exc)
            {
                Util.ShowError(exc);
            }
        }

        /// <summary>
        /// Handles the proxy type combobox's SelectedIndexChanged event.
        /// </summary>
        /// <param name="sender">The combobox</param>
        /// <param name="e">The event arguments.</param>
        private void cbxProxy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enable = cbxProxyType.SelectedIndex > 0;

            cbxProxyMethod.Enabled = cbxProxyType.SelectedIndex == (int)ProxyType.HttpConnect; // Authentication method is available for HTTP Connect only.
            txtProxyDomain.Enabled = cbxProxyMethod.Enabled && cbxProxyMethod.SelectedIndex == (int)ProxyHttpConnectAuthMethod.Ntlm; // Domain is available for NTLM authentication method only.
            txtProxyUser.Enabled = enable;
            txtProxyPassword.Enabled = enable;
            txtProxyHost.Enabled = enable; // Proxy host and port are not available in NoProxy type.
            txtProxyPort.Enabled = enable;
        }

        private void btnCertBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select a private key file";
            dlg.FileName = txtPrivateKey.Text;
            dlg.Filter = "All files|*.*";
            dlg.FilterIndex = 1;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPrivateKey.Text = dlg.FileName;
            }
        }

        #endregion

        private void client_MultipleFilesTransferActionConfirm(object sender, TransferConfirmEventArgs e)
        {
            _fileOpForm.Show(this, e, client);
        }

        private void client_Progress(object sender, FileSystemProgressEventArgs e)
        {
            progressBarTotal.Value = (int)e.TotalPercentage;
            progressBarFile.Value = (int)e.Percentage;
            
            switch (e.State)
            {
                case TransferState.BuildingDirectoryStructure:
                    toolStripStatusLabel.Text = "Building directory structure...";
                    break;

#if SHOWSPEED
                case TransferState.Uploading:
                    if (e.BytesPerSecond > 0)
                            toolStripStatusLabel.Text =
                                string.Format("Uploading file {0}... {1:#.#} KB/sec {2} remaining",
                                              System.IO.Path.GetFileName(e.SourcePath), e.KbytesPerSecond,
                                              e.RemainingTime);
                    break;
#endif

                case TransferState.StartUploadingFile:
                    toolStripStatusLabel.Text = string.Format("Uploading file {0}...", System.IO.Path.GetFileName(e.SourcePath));
                    break;

#if SHOWSPEED
                case TransferState.Downloading:
                    if (e.BytesPerSecond > 0)
                            toolStripStatusLabel.Text =
                                string.Format("Downloading file {0}... {1:#.#} KB/sec {2} remaining",
                                              System.IO.Path.GetFileName(e.SourcePath), e.KbytesPerSecond,
                                              e.RemainingTime);
                    break;
#endif

                case TransferState.StartDownloadingFile:
                    toolStripStatusLabel.Text = string.Format("Downloading file {0}...", System.IO.Path.GetFileName(e.SourcePath));
                    break;
            }

            Application.DoEvents();
        }
    }
}