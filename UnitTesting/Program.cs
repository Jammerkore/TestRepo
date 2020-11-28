using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UnitTesting
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args = null)
        {
            if (args != null && args.Length > 0)
            {
                
                string sPerformAutoRun = args[0].ToUpper(); //first argument must be autorun
                if (sPerformAutoRun == "AUTORUN")
                {
                    //2nd argument is optional - it will override the default autorun folder in the .config file
                    string autoRunPath = System.Configuration.ConfigurationManager.AppSettings["DefaultAutoRunFilePath"];
                    if (args.Length > 1)
                    {
                        autoRunPath = args[1];
                    }

                    //autoUpgradeLog = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MIDRetail\AutoUpgrade.log"; 
                    Plan.Run(autoRunPath);
                }
            }
            else
            {
                //Start the app normally
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }

        }
    }
}
