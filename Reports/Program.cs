using System;
using System.Windows.Forms;

namespace Reports
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var language = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["language"]) ? "def" : System.Configuration.ConfigurationManager.AppSettings["language"];
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(language);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
