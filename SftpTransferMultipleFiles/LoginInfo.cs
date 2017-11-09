using System;
using ComponentPro.Net;

namespace SftpSamples
{
    public class LoginInfo
    {
        #region General Info

        string _server;
        public string SftpServer
        {
            get { return _server; }
            set { _server = value; }
        }

        int _port;
        public int SftpPort
        {
            get { return _port; }
            set { _port = value; }
        }

        string _username;
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        string _remoteDir;
        public string RemoteDir
        {
            get { return _remoteDir; }
            set { _remoteDir = value; }
        }

        string _localDir;
        public string LocalDir
        {
            get { return _localDir; }
            set { _localDir = value; }
        }

        string _searchPattern;
        public string SearchPattern
        {
            get { return _searchPattern; }
            set { _searchPattern = value; }
        }

        bool _encoding;
        public bool Utf8Encoding
        {
            get { return _encoding; }
            set { _encoding = value; }
        }

        #endregion

        #region Proxy

        string _proxyServer;
        public string ProxyServer
        {
            get { return _proxyServer; }
            set { _proxyServer = value; }
        }

        int _proxyPort;
        public int ProxyPort
        {
            get { return _proxyPort; }
            set { _proxyPort = value; }
        }

        string _proxyUser;
        public string ProxyUser
        {
            get { return _proxyUser; }
            set { _proxyUser = value; }
        }

        string _proxyPassword;
        public string ProxyPassword
        {
            get { return _proxyPassword; }
            set { _proxyPassword = value; }
        }

        string _proxyDomain;
        public string ProxyDomain
        {
            get { return _proxyDomain; }
            set { _proxyDomain = value; }
        }

        ProxyType _proxyType;
        public ProxyType ProxyType
        {
            get { return _proxyType; }
            set { _proxyType = value; }
        }

        ProxyHttpConnectAuthMethod _proxyMethod;
        public ProxyHttpConnectAuthMethod ProxyMethod
        {
            get { return _proxyMethod; }
            set { _proxyMethod = value; }
        }

        #endregion

        #region Security

        string _privateKey;
        public string PrivateKey
        {
            get { return _privateKey; }
            set { _privateKey = value; }
        }

        #endregion

        #region Methods

        public void SaveConfig()
        {
            // Save Login information.
            Util.SaveProperty("SftpServer", SftpServer);
            Util.SaveProperty("SftpPort", SftpPort);
            Util.SaveProperty("UserName", UserName);
            Util.SaveProperty("Password", Password);
            Util.SaveProperty("RemoteDir", RemoteDir);
            Util.SaveProperty("LocalDir", LocalDir);

            Util.SaveProperty("Encoding", Utf8Encoding);

            // Proxy Info.
            Util.SaveProperty("ProxyServer", ProxyServer);
            Util.SaveProperty("ProxyPort", ProxyPort);
            Util.SaveProperty("ProxyUser", ProxyUser);
            Util.SaveProperty("ProxyPassword", ProxyPassword);
            Util.SaveProperty("ProxyDomain", ProxyDomain);
            Util.SaveProperty("ProxyType", (int)ProxyType);
            Util.SaveProperty("ProxyMethod", (int)ProxyMethod);

            // Security Info.
            Util.SaveProperty("PrivateKey", PrivateKey);

            Util.SaveProperty("SearchPattern", SearchPattern);
        }

        public static LoginInfo LoadConfig()
        {
            // Load Login information.
            LoginInfo s = new LoginInfo();

            // Server and authentication info.
            s.SftpServer = (string)Util.GetProperty("SftpServer", string.Empty);
            s.SftpPort = Util.GetIntProperty("SftpPort", 22);
            s.UserName = (string)Util.GetProperty("UserName", string.Empty);
            s.Password = (string)Util.GetProperty("Password", string.Empty);
            s.RemoteDir = (string)Util.GetProperty("RemoteDir", "/");
            s.LocalDir = (string)Util.GetProperty("LocalDir", AppDomain.CurrentDomain.BaseDirectory);

            s.Utf8Encoding = (string)Util.GetProperty("Encoding", "False") == "True";

            // Proxy info.
            s.ProxyServer = (string)Util.GetProperty("ProxyServer", string.Empty);
            s.ProxyPort = Util.GetIntProperty("ProxyPort", 1080);
            s.ProxyUser = (string)Util.GetProperty("ProxyUser", string.Empty);
            s.ProxyPassword = (string)Util.GetProperty("ProxyPassword", string.Empty);
            s.ProxyDomain = (string)Util.GetProperty("ProxyDomain", string.Empty);
            s.ProxyType = (ProxyType)Util.GetIntProperty("ProxyType", 0);
            s.ProxyMethod = (ProxyHttpConnectAuthMethod)Util.GetIntProperty("ProxyMethod", 0);

            // Security info.
            s.PrivateKey = (string)Util.GetProperty("PrivateKey", string.Empty);

            s.SearchPattern = (string)Util.GetProperty("SearchPattern", "*.*");

            return s;
        }

        #endregion
    }    
}