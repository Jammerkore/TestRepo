using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;

using MIDRetail.Data;
using MIDRetail.DataCommon;


namespace MIDRetail.HeaderExpand
{
    class HeaderExpand
    {
        static int Main(string[] args)
        {
            ExpandWorker expand = new ExpandWorker();
            return expand.Expand(args);
        }

        public class ExpandWorker
        {
            public int Expand(string[] args)
            {
                string eventLogID = "HeaderExpand";
                string message = null;
                int errorCode = 0;
                string parm;
                string[] headers = null;
                string connectionString;
                Hashtable htHeaders = null;

                try
                {

                    if (!EventLog.SourceExists(eventLogID))
                    {
                        EventLog.CreateEventSource(eventLogID, null);
                    }

                    // Begin TT#1054 - JSmith - Relieve Intransit not working.
                    //connectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];
                    connectionString = MIDConfigurationManager.AppSettings["ConnectionString"];
                    // End TT#1054

                    // Begin TT#1054 - JSmith - Relieve Intransit not working.
                    //parm = System.Configuration.ConfigurationSettings.AppSettings["HeadersToExpand"];
                    parm = MIDConfigurationManager.AppSettings["HeadersToExpand"];
                    // End TT#1054
                    if (parm != null)
                    {
                        htHeaders = new Hashtable();
                        headers = parm.Split(';');
                        foreach (string header in headers)
                        {
                            htHeaders.Add(header, null);
                        }
                    }

                    errorCode = ExpandHeaders(eventLogID, connectionString, htHeaders);
                }

                catch (Exception ex)
                {
                    errorCode = 1;
                    message = "";
                    while (ex != null)
                    {
                        message += " -- " + ex.Message;
                        ex = ex.InnerException;
                    }
                    EventLog.WriteEntry(eventLogID, message);
                }


                return errorCode;
            }

            public int ExpandHeaders(string eventLogID, string connectionString, Hashtable htHeaders)
            {
                int errorCode = 0;
                Header headerData;
                StoreData storeData;
                int[] storeRIDList;
                DataTable dt = null;
                int i = 0;
                string message = null;
                ConsoleStatusBar cs = new ConsoleStatusBar();
                double headerTotal;

                try
                {
                    storeData = new StoreData(connectionString);
                    dt = storeData.StoreProfile_Read();
                    storeRIDList = new int[dt.Rows.Count];

                    foreach (DataRow dr in dt.Rows)
                    {
                        storeRIDList[i] = Convert.ToInt32(dr["ST_RID"]);
                        i++;
                    }

                    Console.Clear();
                    Console.WriteLine("Converting Headers...");
                    Console.WriteLine();
                    Console.WriteLine();

                    headerData = new Header(connectionString);
                    dt = headerData.GetCompressedHeaderList();
                    headerTotal = dt.Rows.Count;
                    DateTime startTime = DateTime.Now;
                    int headerCount = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string headerID = string.Empty;
                        int headerRID;

                        if (dr["HDR_ID"] != DBNull.Value && dr["HDR_RID"] != DBNull.Value)
                        {
                            headerID = dr["HDR_ID"].ToString();
                            headerRID = Convert.ToInt32(dr["HDR_RID"]);

                            if (htHeaders == null ||
                                htHeaders.ContainsKey(headerID))
                            {
                                headerData.GetHeaderAllocation(headerRID);
                                headerData.CopyStoreAllocationFromBinToExpand(storeRIDList, headerRID, true);
                                headerCount++;

                                if (headerCount % 5 == 0)
                                {
                                    cs.RenderConsoleProgress(Convert.ToInt32((headerCount / headerTotal) * 100), '\u2592', ConsoleColor.Green, Convert.ToInt32((headerCount / headerTotal) * 100).ToString() + "% Complete");
                                }
                            }
                        }

                    }
                    TimeSpan duration = DateTime.Now.Subtract(startTime);
                    string strDuration = Convert.ToString(duration, CultureInfo.CurrentUICulture);
                }
                catch (Exception ex)
                {
                    message = "";
                    while (ex != null)
                    {
                        message += " -- " + ex.Message;
                        ex = ex.InnerException;
                    }
                    EventLog.WriteEntry(eventLogID, message);
                    errorCode = 1;
                    cs.RenderConsoleProgress(100, '\u2591', ConsoleColor.Red, "");
                    Console.WriteLine(" Error during header conversion. Review Event Viewer. Press ENTER key to continue...");
                    Console.ReadLine();

                    throw;
                }

                cs.RenderConsoleProgress(100, '\u2592', ConsoleColor.Green, "100% Complete");
                Console.WriteLine(" Header conversion was Successful. Press ENTER key to continue...");
                Console.ReadLine();

                return errorCode;
            }
        }
    }
}
