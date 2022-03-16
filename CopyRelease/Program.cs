using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MIDRetail.CopyRelease
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form1 form1 = new Form1();
            if (args.Length == 0)
            {
                Application.Run(form1);
            }
            else
            {
                form1.Form1_Load(sender: null, e: new EventArgs());
                form1.CopyToQA(
                    quietMode: true,
                    folderSuffix: args[0]
                    );
            }
        }
    }
}
