using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

namespace MIDRetail.DataCommon
{
    public static class FilterMonitor
    {
        public static DataTable dtMonitor = null;
        public static bool doMonitor = false;



        public static void ClearMonitorEntries()
        {
            dtMonitor.Rows.Clear();
            List<string> colNames = new List<string>();


            foreach (DataColumn col in dtMonitor.Columns)
            {
                colNames.Add(col.ColumnName);
            }


            foreach (string colName in colNames)
            {
                if (colName == "Filter Name" || colName == "Condition" || colName == "Store" || colName == "Outer Week")
                {
                  
                }
                else
                {
                    dtMonitor.Columns.Remove(dtMonitor.Columns[colName]);
                }
            }
            
        }

      
        public class FilterMonitorEntry
        {
            public string filterName;
            public string conditionText;
            public string storeID;
            public string outerWeek;
            public void AddEntryToMonitor()
            {
                DataRow dr = dtMonitor.NewRow();
                dr["Filter Name"] = filterName;
                dr["Condition"] = conditionText;
                dr["Store"] = storeID;
                dr["Outer Week"] = outerWeek;

                foreach (string colName in dateAndFieldColumns)
                {
                    dateAndFieldEntry dfe = dateAndFieldVals.Find(x => x.colName == colName);
                    if (dfe != null)
                    {
                        dr[colName] = dfe.val;
                    }
                }

                dtMonitor.Rows.Add(dr);
            }

            List<string> dateAndFieldColumns = new List<string>();
            public void AddDateAndFieldColumn(string dateID, string fieldID)
            {
                string colName = dateID + " - " + fieldID;
                if (dateAndFieldColumns.Contains(colName) == false)
                {
                    dateAndFieldColumns.Add(colName);
                }
                if (dtMonitor.Columns.Contains(colName) == false)
                {
                    dtMonitor.Columns.Add(colName);
                }
            }

            public class dateAndFieldEntry
            {
                public string colName;
                public string val;
            }

            List<dateAndFieldEntry> dateAndFieldVals = new List<dateAndFieldEntry>();
            public void AddDateAndFieldValue(string dateID, string fieldID, string val)
            {
                dateAndFieldEntry dfe = new dateAndFieldEntry();
                dfe.colName = dateID + " - " + fieldID;
                dfe.val = val;
                dateAndFieldVals.Add(dfe);
            }
            public void RemoveDateAndFieldColunns()
            {
                foreach (string colName in dateAndFieldColumns)
                {
                    dtMonitor.Columns.Remove(dtMonitor.Columns[colName]);
                }
            }
        }

        public static void AddRow(FilterMonitorEntry fme)
        {
            if (doMonitor)
            {
                fme.AddEntryToMonitor();
            }
        }

        public static void PrepareDataTableForBinding()
        {
            dtMonitor = new DataTable();
            dtMonitor.Columns.Add("Filter Name");
            dtMonitor.Columns.Add("Condition");
            dtMonitor.Columns.Add("Store");
            dtMonitor.Columns.Add("Outer Week");
        }


    }
}
