using System;
using System.Windows.Forms;

namespace SftpSamples
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ComponentPro.Licensing.Common.LicenseManager.SetLicenseKey(ComponentPro.TrialLicenseKey.Key);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SftpMulFiles());
        }
    }
}