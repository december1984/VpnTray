using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using VpnTray.Domain;
using VpnTray.Driver.WinApi;
using VpnTray.Properties;
using VpnTray.SystemDataAccess.FileSimulator;
using VpnTray.ViewModels;

namespace VpnTray
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var startup = new Startup();
            startup.ConfigureServices();

            var form = startup.Services.GetInstance<VpnTrayForm>();

            Application.Run(form);
        }
    }
}
