using System.Drawing;
using System.Windows.Forms;

namespace SftpSamples
{
    partial class SftpMulFiles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SftpMulFiles));
            this.txtSearchPattern = new System.Windows.Forms.TextBox();
            this.lblSearchPattern = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.ToolStripButton();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnAbort = new System.Windows.Forms.ToolStripButton();
            this.client = new ComponentPro.Net.Sftp(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControlExt = new System.Windows.Forms.TabControl();
            this.loginPage = new System.Windows.Forms.TabPage();
            this.chkUtf8Encoding = new System.Windows.Forms.CheckBox();
            this.btnKeyBrowse = new System.Windows.Forms.Button();
            this.txtPrivateKey = new System.Windows.Forms.TextBox();
            this.lblKey = new System.Windows.Forms.Label();
            this.btnLocalDirBrowse = new System.Windows.Forms.Button();
            this.txtLocalDir = new System.Windows.Forms.TextBox();
            this.txtRemoteDir = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblLocalDir = new System.Windows.Forms.Label();
            this.lblRemote = new System.Windows.Forms.Label();
            this.lblSftpPassword = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblSftpUserName = new System.Windows.Forms.Label();
            this.proxyPage = new System.Windows.Forms.TabPage();
            this.txtProxyDomain = new System.Windows.Forms.TextBox();
            this.lblDomain = new System.Windows.Forms.Label();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cbxProxyMethod = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.txtProxyPort = new System.Windows.Forms.TextBox();
            this.txtProxyHost = new System.Windows.Forms.TextBox();
            this.txtProxyPassword = new System.Windows.Forms.TextBox();
            this.txtProxyUser = new System.Windows.Forms.TextBox();
            this.lblProxyPort = new System.Windows.Forms.Label();
            this.lblProxyServer = new System.Windows.Forms.Label();
            this.cbxProxyType = new System.Windows.Forms.ComboBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.ToolStripButton();
            this.progressBarTotal = new System.Windows.Forms.ProgressBar();
            this.progressBarFile = new System.Windows.Forms.ProgressBar();
            this.toolbarMain = new System.Windows.Forms.ToolStrip();
            this.statusStrip.SuspendLayout();
            this.tabControlExt.SuspendLayout();
            this.loginPage.SuspendLayout();
            this.proxyPage.SuspendLayout();
            this.toolbarMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearchPattern
            // 
            this.txtSearchPattern.Location = new System.Drawing.Point(74, 131);
            this.txtSearchPattern.Name = "txtSearchPattern";
            this.txtSearchPattern.Size = new System.Drawing.Size(118, 20);
            this.txtSearchPattern.TabIndex = 8;
            this.txtSearchPattern.Text = "*.*";
            // 
            // lblSearchPattern
            // 
            this.lblSearchPattern.Location = new System.Drawing.Point(7, 127);
            this.lblSearchPattern.Name = "lblSearchPattern";
            this.lblSearchPattern.Size = new System.Drawing.Size(44, 26);
            this.lblSearchPattern.TabIndex = 17;
            this.lblSearchPattern.Text = "Search \r\nPattern:";
            // 
            // btnDownload
            // 
            this.btnDownload.Image = ((System.Drawing.Image)(resources.GetObject("btnDownload.Image")));
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(114, 36);
            this.btnDownload.Text = "Download Files";
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.ForeColor = System.Drawing.Color.Silver;
            this.txtLog.Location = new System.Drawing.Point(6, 234);
            this.txtLog.MaxLength = 0;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(557, 218);
            this.txtLog.TabIndex = 11;
            this.txtLog.Text = "";
            // 
            // btnAbort
            // 
            this.btnAbort.Enabled = false;
            this.btnAbort.Image = ((System.Drawing.Image)(resources.GetObject("btnAbort.Image")));
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(36, 36);
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // client
            // 
            this.client.TransferConfirm += new System.EventHandler<ComponentPro.IO.TransferConfirmEventArgs>(this.client_MultipleFilesTransferActionConfirm);
            this.client.Progress += new System.EventHandler<ComponentPro.IO.FileSystemProgressEventArgs>(this.client_Progress);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 498);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(569, 22);
            this.statusStrip.TabIndex = 31;
            this.statusStrip.Text = "Ready";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(38, 17);
            this.toolStripStatusLabel.Text = "Ready";
            // 
            // tabControlExt
            // 
            this.tabControlExt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlExt.Controls.Add(this.loginPage);
            this.tabControlExt.Controls.Add(this.proxyPage);
            this.tabControlExt.Location = new System.Drawing.Point(6, 46);
            this.tabControlExt.Name = "tabControlExt";
            this.tabControlExt.SelectedIndex = 0;
            this.tabControlExt.Size = new System.Drawing.Size(557, 184);
            this.tabControlExt.TabIndex = 115;
            // 
            // loginPage
            // 
            this.loginPage.Controls.Add(this.chkUtf8Encoding);
            this.loginPage.Controls.Add(this.btnKeyBrowse);
            this.loginPage.Controls.Add(this.txtPrivateKey);
            this.loginPage.Controls.Add(this.lblKey);
            this.loginPage.Controls.Add(this.btnLocalDirBrowse);
            this.loginPage.Controls.Add(this.txtSearchPattern);
            this.loginPage.Controls.Add(this.lblSearchPattern);
            this.loginPage.Controls.Add(this.txtLocalDir);
            this.loginPage.Controls.Add(this.txtRemoteDir);
            this.loginPage.Controls.Add(this.txtUserName);
            this.loginPage.Controls.Add(this.txtPassword);
            this.loginPage.Controls.Add(this.txtPort);
            this.loginPage.Controls.Add(this.txtServer);
            this.loginPage.Controls.Add(this.lblLocalDir);
            this.loginPage.Controls.Add(this.lblRemote);
            this.loginPage.Controls.Add(this.lblSftpPassword);
            this.loginPage.Controls.Add(this.lblPort);
            this.loginPage.Controls.Add(this.lblServer);
            this.loginPage.Controls.Add(this.lblSftpUserName);
            this.loginPage.Location = new System.Drawing.Point(4, 22);
            this.loginPage.Name = "loginPage";
            this.loginPage.Padding = new System.Windows.Forms.Padding(3);
            this.loginPage.Size = new System.Drawing.Size(549, 158);
            this.loginPage.TabIndex = 0;
            this.loginPage.Text = "Connection Settings";
            // 
            // chkUtf8Encoding
            // 
            this.chkUtf8Encoding.Location = new System.Drawing.Point(203, 31);
            this.chkUtf8Encoding.Name = "chkUtf8Encoding";
            this.chkUtf8Encoding.Size = new System.Drawing.Size(111, 23);
            this.chkUtf8Encoding.TabIndex = 120;
            this.chkUtf8Encoding.Text = "UTF8 Encoding";
            // 
            // btnKeyBrowse
            // 
            this.btnKeyBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKeyBrowse.Location = new System.Drawing.Point(516, 56);
            this.btnKeyBrowse.Name = "btnKeyBrowse";
            this.btnKeyBrowse.Size = new System.Drawing.Size(26, 20);
            this.btnKeyBrowse.TabIndex = 118;
            this.btnKeyBrowse.Text = "...";
            this.btnKeyBrowse.Click += new System.EventHandler(this.btnCertBrowse_Click);
            // 
            // txtPrivateKey
            // 
            this.txtPrivateKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrivateKey.Location = new System.Drawing.Point(269, 57);
            this.txtPrivateKey.Name = "txtPrivateKey";
            this.txtPrivateKey.Size = new System.Drawing.Size(241, 20);
            this.txtPrivateKey.TabIndex = 117;
            // 
            // lblKey
            // 
            this.lblKey.Location = new System.Drawing.Point(200, 59);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(64, 13);
            this.lblKey.TabIndex = 119;
            this.lblKey.Text = "Private Key:";
            // 
            // btnLocalDirBrowse
            // 
            this.btnLocalDirBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocalDirBrowse.Location = new System.Drawing.Point(516, 107);
            this.btnLocalDirBrowse.Name = "btnLocalDirBrowse";
            this.btnLocalDirBrowse.Size = new System.Drawing.Size(26, 20);
            this.btnLocalDirBrowse.TabIndex = 7;
            this.btnLocalDirBrowse.Text = "...";
            this.btnLocalDirBrowse.Click += new System.EventHandler(this.btnLocalDirBrowse_Click);
            // 
            // txtLocalDir
            // 
            this.txtLocalDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalDir.Location = new System.Drawing.Point(74, 107);
            this.txtLocalDir.Name = "txtLocalDir";
            this.txtLocalDir.Size = new System.Drawing.Size(436, 20);
            this.txtLocalDir.TabIndex = 6;
            // 
            // txtRemoteDir
            // 
            this.txtRemoteDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemoteDir.Location = new System.Drawing.Point(74, 82);
            this.txtRemoteDir.Name = "txtRemoteDir";
            this.txtRemoteDir.Size = new System.Drawing.Size(468, 20);
            this.txtRemoteDir.TabIndex = 5;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(74, 32);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(118, 20);
            this.txtUserName.TabIndex = 3;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(74, 57);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(118, 20);
            this.txtPassword.TabIndex = 4;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(269, 7);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(56, 20);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "22";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(74, 8);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(118, 20);
            this.txtServer.TabIndex = 1;
            // 
            // lblLocalDir
            // 
            this.lblLocalDir.Location = new System.Drawing.Point(7, 109);
            this.lblLocalDir.Name = "lblLocalDir";
            this.lblLocalDir.Size = new System.Drawing.Size(52, 13);
            this.lblLocalDir.TabIndex = 52;
            this.lblLocalDir.Text = "Local Dir:";
            // 
            // lblRemote
            // 
            this.lblRemote.Location = new System.Drawing.Point(7, 84);
            this.lblRemote.Name = "lblRemote";
            this.lblRemote.Size = new System.Drawing.Size(63, 13);
            this.lblRemote.TabIndex = 49;
            this.lblRemote.Text = "Remote Dir:";
            // 
            // lblSftpPassword
            // 
            this.lblSftpPassword.Location = new System.Drawing.Point(7, 58);
            this.lblSftpPassword.Name = "lblSftpPassword";
            this.lblSftpPassword.Size = new System.Drawing.Size(56, 13);
            this.lblSftpPassword.TabIndex = 47;
            this.lblSftpPassword.Text = "Password:";
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(200, 10);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(51, 13);
            this.lblPort.TabIndex = 45;
            this.lblPort.Text = "Sftp Port:";
            // 
            // lblServer
            // 
            this.lblServer.Location = new System.Drawing.Point(7, 10);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(63, 13);
            this.lblServer.TabIndex = 44;
            this.lblServer.Text = "Sftp Server:";
            // 
            // lblSftpUserName
            // 
            this.lblSftpUserName.Location = new System.Drawing.Point(7, 34);
            this.lblSftpUserName.Name = "lblSftpUserName";
            this.lblSftpUserName.Size = new System.Drawing.Size(63, 13);
            this.lblSftpUserName.TabIndex = 46;
            this.lblSftpUserName.Text = "User Name:";
            // 
            // proxyPage
            // 
            this.proxyPage.Controls.Add(this.txtProxyDomain);
            this.proxyPage.Controls.Add(this.lblDomain);
            this.proxyPage.Controls.Add(this.lblMethod);
            this.proxyPage.Controls.Add(this.cbxProxyMethod);
            this.proxyPage.Controls.Add(this.lblType);
            this.proxyPage.Controls.Add(this.txtProxyPort);
            this.proxyPage.Controls.Add(this.txtProxyHost);
            this.proxyPage.Controls.Add(this.txtProxyPassword);
            this.proxyPage.Controls.Add(this.txtProxyUser);
            this.proxyPage.Controls.Add(this.lblProxyPort);
            this.proxyPage.Controls.Add(this.lblProxyServer);
            this.proxyPage.Controls.Add(this.cbxProxyType);
            this.proxyPage.Controls.Add(this.lblPassword);
            this.proxyPage.Controls.Add(this.lblUserName);
            this.proxyPage.Location = new System.Drawing.Point(4, 22);
            this.proxyPage.Name = "proxyPage";
            this.proxyPage.Padding = new System.Windows.Forms.Padding(3);
            this.proxyPage.Size = new System.Drawing.Size(549, 158);
            this.proxyPage.TabIndex = 1;
            this.proxyPage.Text = "Proxy Settings";
            // 
            // txtProxyDomain
            // 
            this.txtProxyDomain.Location = new System.Drawing.Point(74, 80);
            this.txtProxyDomain.Name = "txtProxyDomain";
            this.txtProxyDomain.Size = new System.Drawing.Size(126, 20);
            this.txtProxyDomain.TabIndex = 7;
            // 
            // lblDomain
            // 
            this.lblDomain.Location = new System.Drawing.Point(6, 82);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(46, 13);
            this.lblDomain.TabIndex = 33;
            this.lblDomain.Text = "Domain:";
            // 
            // lblMethod
            // 
            this.lblMethod.Location = new System.Drawing.Point(211, 58);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(46, 13);
            this.lblMethod.TabIndex = 29;
            this.lblMethod.Text = "Method:";
            // 
            // cbxProxyMethod
            // 
            this.cbxProxyMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProxyMethod.Items.AddRange(new object[] {
            "Basic",
            "Ntlm"});
            this.cbxProxyMethod.Location = new System.Drawing.Point(285, 56);
            this.cbxProxyMethod.Name = "cbxProxyMethod";
            this.cbxProxyMethod.Size = new System.Drawing.Size(92, 21);
            this.cbxProxyMethod.TabIndex = 6;
            this.cbxProxyMethod.SelectedIndexChanged += new System.EventHandler(this.cbxProxy_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(211, 34);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(63, 13);
            this.lblType.TabIndex = 28;
            this.lblType.Text = "Proxy Type:";
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.Location = new System.Drawing.Point(285, 8);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.Size = new System.Drawing.Size(92, 20);
            this.txtProxyPort.TabIndex = 2;
            // 
            // txtProxyHost
            // 
            this.txtProxyHost.Location = new System.Drawing.Point(74, 8);
            this.txtProxyHost.Name = "txtProxyHost";
            this.txtProxyHost.Size = new System.Drawing.Size(126, 20);
            this.txtProxyHost.TabIndex = 1;
            // 
            // txtProxyPassword
            // 
            this.txtProxyPassword.Location = new System.Drawing.Point(74, 56);
            this.txtProxyPassword.Name = "txtProxyPassword";
            this.txtProxyPassword.PasswordChar = '*';
            this.txtProxyPassword.Size = new System.Drawing.Size(126, 20);
            this.txtProxyPassword.TabIndex = 5;
            // 
            // txtProxyUser
            // 
            this.txtProxyUser.Location = new System.Drawing.Point(74, 32);
            this.txtProxyUser.Name = "txtProxyUser";
            this.txtProxyUser.Size = new System.Drawing.Size(126, 20);
            this.txtProxyUser.TabIndex = 3;
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.Location = new System.Drawing.Point(211, 10);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(58, 13);
            this.lblProxyPort.TabIndex = 27;
            this.lblProxyPort.Text = "Proxy Port:";
            // 
            // lblProxyServer
            // 
            this.lblProxyServer.Location = new System.Drawing.Point(6, 10);
            this.lblProxyServer.Name = "lblProxyServer";
            this.lblProxyServer.Size = new System.Drawing.Size(70, 13);
            this.lblProxyServer.TabIndex = 26;
            this.lblProxyServer.Text = "Proxy Server:";
            // 
            // cbxProxyType
            // 
            this.cbxProxyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProxyType.Items.AddRange(new object[] {
            "Never",
            "Socks4",
            "Socks4A",
            "Socks5",
            "HttpConnect"});
            this.cbxProxyType.Location = new System.Drawing.Point(285, 32);
            this.cbxProxyType.Name = "cbxProxyType";
            this.cbxProxyType.Size = new System.Drawing.Size(92, 21);
            this.cbxProxyType.TabIndex = 4;
            this.cbxProxyType.SelectedIndexChanged += new System.EventHandler(this.cbxProxy_SelectedIndexChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(6, 58);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 23;
            this.lblPassword.Text = "Password:";
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(6, 35);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(63, 13);
            this.lblUserName.TabIndex = 21;
            this.lblUserName.Text = "User Name:";
            // 
            // btnUpload
            // 
            this.btnUpload.Image = ((System.Drawing.Image)(resources.GetObject("btnUpload.Image")));
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(100, 36);
            this.btnUpload.Text = "Upload Files";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // progressBarTotal
            // 
            this.progressBarTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarTotal.Location = new System.Drawing.Point(6, 476);
            this.progressBarTotal.Name = "progressBarTotal";
            this.progressBarTotal.Size = new System.Drawing.Size(557, 16);
            this.progressBarTotal.TabIndex = 118;
            // 
            // progressBarFile
            // 
            this.progressBarFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarFile.Location = new System.Drawing.Point(6, 458);
            this.progressBarFile.Name = "progressBarFile";
            this.progressBarFile.Size = new System.Drawing.Size(557, 16);
            this.progressBarFile.TabIndex = 117;
            // 
            // toolbarMain
            // 
            this.toolbarMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolbarMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUpload,
            this.btnDownload,
            this.btnAbort});
            this.toolbarMain.Location = new System.Drawing.Point(0, 0);
            this.toolbarMain.Name = "toolbarMain";
            this.toolbarMain.Size = new System.Drawing.Size(569, 39);
            this.toolbarMain.TabIndex = 119;
            // 
            // SftpMulFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 520);
            this.Controls.Add(this.progressBarTotal);
            this.Controls.Add(this.progressBarFile);
            this.Controls.Add(this.tabControlExt);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolbarMain);
            this.MinimumSize = new System.Drawing.Size(577, 528);
            this.Name = "SftpMulFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ultimate Sftp Download Multiple Files";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControlExt.ResumeLayout(false);
            this.loginPage.ResumeLayout(false);
            this.loginPage.PerformLayout();
            this.proxyPage.ResumeLayout(false);
            this.proxyPage.PerformLayout();
            this.toolbarMain.ResumeLayout(false);
            this.toolbarMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.ToolStripButton btnAbort;
        private System.Windows.Forms.ToolStripButton btnDownload;
        private ComponentPro.Net.Sftp client;
        private System.Windows.Forms.TextBox txtSearchPattern;
        private System.Windows.Forms.Label lblSearchPattern;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.TabControl tabControlExt;
        private System.Windows.Forms.TabPage loginPage;
        private System.Windows.Forms.Button btnLocalDirBrowse;
        private System.Windows.Forms.TextBox txtLocalDir;
        private System.Windows.Forms.TextBox txtRemoteDir;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblLocalDir;
        private System.Windows.Forms.Label lblRemote;
        private System.Windows.Forms.Label lblSftpPassword;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblSftpUserName;
        private System.Windows.Forms.TabPage proxyPage;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox cbxProxyMethod;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.TextBox txtProxyPort;
        private System.Windows.Forms.TextBox txtProxyHost;
        private System.Windows.Forms.TextBox txtProxyPassword;
        private System.Windows.Forms.TextBox txtProxyUser;
        private System.Windows.Forms.Label lblProxyPort;
        private System.Windows.Forms.Label lblProxyServer;
        private System.Windows.Forms.ComboBox cbxProxyType;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.ToolStripButton btnUpload;
        private System.Windows.Forms.ProgressBar progressBarTotal;
        private System.Windows.Forms.ProgressBar progressBarFile;
        private System.Windows.Forms.TextBox txtProxyDomain;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.ToolStrip toolbarMain;
        private System.Windows.Forms.Button btnKeyBrowse;
        private System.Windows.Forms.TextBox txtPrivateKey;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.CheckBox chkUtf8Encoding;
    }
}

