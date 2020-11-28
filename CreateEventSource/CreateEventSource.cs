using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;

namespace CreateEventSource
{
    class Program
    {
        static int Main(string[] args)
        {
            CreateWorker worker = new CreateWorker();
            return worker.CreateEventSources(args);
        }

        public class CreateWorker
        {
            string[] sourceNames = null;
            bool bShowConfirmation = true;

            public int CreateEventSources(string[] args)
            {
                try
                {
                    if (args.Length > 0)
                    {
                        if (args[0] == "##INSTALLER##")
                        {
                            bShowConfirmation = false;
                        }
                    }

                    string strParm = MIDConfigurationManager.AppSettings["EventSourceNames"];
                    if (strParm != null)
                    {
                        try
                        {
                            sourceNames = strParm.Split(';');
                        }
                        catch
                        {
                        }
                    }

                    if (sourceNames != null)
                    {
                        foreach (string sourceName in sourceNames)
                        {
                            if (sourceName.Trim().Length > 0 &&
                                !EventLog.SourceExists(sourceName.Trim()))
                            {
                                EventLog.CreateEventSource(sourceName.Trim(), null);
                            }
                        }
                    }

                    if (bShowConfirmation)
                    {
                        Console.WriteLine("All Event Source Names successfully created.  Hit any key to continue.");
                        Console.ReadKey();
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error encountered. " + ex.Message + "  Hit any key to continue.");
                    Console.ReadKey();
                    return 1;
                }
            }
        }
    }
}
