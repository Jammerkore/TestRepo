using System;
using System.Collections;
using System.IO;

namespace MIDRetail.DatabaseUpdate
{
	class Startup
	{
		public Startup()
		{
		}

        [STAThread]
        static int Main(string[] args)
        {
            frmDatabaseUpdate frmDBLoad = null;
            Queue errorQueue;
            System.IO.StreamWriter outFile;
            // Begin TT#1668 - JSmith - Install Log
            StreamWriter installLog = null;
            // End TT#1668
            int returnCode = 0;

            try
            {
    			// Begin TT#1668 - JSmith - Install Log
                string logFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\MIDRetail";
                DirectoryInfo dir = new System.IO.DirectoryInfo(logFolder);
                if (!dir.Exists)
                {
                    dir.Create();
                }
                //installLog = new StreamWriter(logFolder + @"\MIDRetailInstall.log", true);
				// End TT#1668

                if (args.Length == 0)
                {
                    installLog = new StreamWriter(logFolder + @"\MIDRetailInstall_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".log", true);
				    // Begin TT#1668 - JSmith - Install Log
					//frmDBLoad = new frmDatabaseUpdate();
                    frmDBLoad = new frmDatabaseUpdate(installLog);
					// End TT#1668
                    System.Windows.Forms.Application.Run(frmDBLoad);
                }
                // Begin TT#74 MD - JSmith - One-button Upgrade
                else if (args.Length == 2)
                {
                    installLog = new StreamWriter(Convert.ToString(args[1]), true);
                    frmDBLoad = new frmDatabaseUpdate(args[0], installLog);
                }
                // End TT#74 MD
				// Begin TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
                else if (args.Length == 3)
                {
                    installLog = new StreamWriter(Convert.ToString(args[1]), true);
                    bool oneClick = bool.Parse(args[2]);
                    frmDBLoad = new frmDatabaseUpdate(args[0], oneClick, installLog);
                    if (!oneClick)
                    {                    
                        System.Windows.Forms.Application.Run(frmDBLoad);
                    }

                }
				// End TT#4080 - stodd - Pre-populate database connection information in the upgrade dialogue window 
                else if (args.Length == 4)
                {
                    errorQueue = new Queue();
                    outFile = new System.IO.StreamWriter("BatchComputationLoad.out", false);

                    try
                    {
                        //					if (UpdateRoutines.ProcessScripts(errorQueue, args[0], args[1], args[2], args[3]))
                        //					{
                        //						outFile.WriteLine("The database has been updated successfully.");
                        //					}
                        //					else
                        //					{
                        //						outFile.WriteLine("The database has not been updated successfully.");
                        //					}
                        //
                        //					if (LoadRoutines.LoadComputations(errorQueue, args[0], args[1], args[2], args[3]))
                        //					{
                        //						outFile.WriteLine("The computations have been updated successfully.");
                        //					}
                        //					else
                        //					{
                        //						outFile.WriteLine("The computations have not been updated successfully.");
                        //					}
                    }
                    catch (Exception exc)
                    {
                        returnCode = 1;
                        string message = exc.ToString();
                        outFile.WriteLine("Errors encountered:");

                        while (errorQueue.Count > 0)
                        {
                            outFile.WriteLine((string)errorQueue.Dequeue());
                        }
                    }
                    finally
                    {
                        outFile.Close();
                    }
                }
            }
			// Begin TT#1668 - JSmith - Install Log
            finally
            {
                if (installLog != null)
                {
                    installLog.Close();
                }
            }
			// End TT#1668

            if (frmDBLoad == null ||
                !frmDBLoad.LoadSuccessful)
            {
                returnCode = 1;
            }

            return returnCode;
        }
	}
}