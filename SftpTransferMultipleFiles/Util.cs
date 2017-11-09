using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SftpSamples
{
    public static class Util
    {
        public static void SaveProperty(string keyName, object value)
        {
            try
            {
                RegistryKey Key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ComponentPro\\Samples\\SftpDownloadFiles");
                Key.SetValue(keyName, value);
            }
            catch
            {
                return;
            }
        }

        public static object GetProperty(string keyName, object defaultValue)
        {
            try
            {
                RegistryKey Key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ComponentPro\\Samples\\SftpDownloadFiles");
                return Key.GetValue(keyName, defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static object GetProperty(string keyName)
        {
            return GetProperty(keyName, null);
        }

        public static int GetIntProperty(string keyName, int defaultValue)
        {
            try
            {
                RegistryKey Key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ComponentPro\\Samples\\SftpDownloadFiles");
                return int.Parse(Key.GetValue(keyName, defaultValue).ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long GetLongProperty(string keyName, long defaultValue)
        {
            try
            {
                RegistryKey Key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ComponentPro\\Samples\\SftpDownloadFiles");
                return long.Parse(Key.GetValue(keyName, defaultValue).ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static void ShowError(Exception exc)
        {
            string str;

            if (exc.InnerException != null)
                str = exc.InnerException.Message;
            else
                str = exc.Message;

            if (str.IndexOf("Bad message") != -1)
                str = "Item already exists or no permission";

            MessageBox.Show(string.Format(null, "An error occurred: {0}", str), "Error");
        }

        public static void ShowError(Exception exc, string msg)
        {
            string str;

            if (exc.InnerException != null)
                str = exc.InnerException.Message;
            else
                str = exc.Message;

            if (str.IndexOf("Bad message") != -1)
                str = "Item already exists or no permission";

            MessageBox.Show(string.Format(null, "{0}. An error occurred: {1}", msg, str), "Error");
        }

        const int MF_BYCOMMAND = 0;
        const int MF_ENABLED = 0x00000000;
        const int MF_GRAYED = 0x00000001;

        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("User32")]
        private static extern bool EnableMenuItem(IntPtr hMenu, IntPtr hMenuItem, int nEnable);

        [DllImport("User32")]
        private static extern IntPtr GetMenuItemID(IntPtr hMenu, int nPos);

        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

        static readonly Dictionary<string, bool> _map = new Dictionary<string, bool>();

        /// <summary>
        /// Disables Close(X) button.
        /// </summary>
        /// <param name="form">Form object.</param>
        /// <param name="enable">Indicates whether the close button is enabled.</param>
        static void EnableCloseButtonInt(Form form, bool enable)
        {
            IntPtr hMenu = GetSystemMenu(form.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            IntPtr hItem = GetMenuItemID(hMenu, menuItemCount - 1);
            EnableMenuItem(hMenu, hItem, MF_BYCOMMAND | (enable ? MF_ENABLED : MF_GRAYED));
        }

        /// <summary>
        /// Disables Close(X) button.
        /// </summary>
        /// <param name="form">Form object.</param>
        /// <param name="enable">Indicates whether the close button is enabled.</param>
        public static void EnableCloseButton(Form form, bool enable)
        {
            EnableCloseButtonInt(form, enable);

            if (!_map.ContainsKey(form.Name))
            {
                lock (_map)
                {
                    _map.Add(form.Name, enable);
                    form.Resize += form_Resize;
                }
            }
            else
                _map[form.Name] = enable;
        }

        static void form_Resize(object sender, EventArgs e)
        {
            Form form = (Form)sender;

            if (!_map[form.Name])
                EnableCloseButtonInt(form, false);
        }
    }
}