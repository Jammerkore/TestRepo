using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.Data;

namespace UnitTesting
{
    public static class Shared_UtilityFunctions
    {


        public static string GetCurrentProjectPath()
        {
            string sCurrentProjectDir = System.AppDomain.CurrentDomain.BaseDirectory;
            if (sCurrentProjectDir.Length > 0)
            {
                int lenUnitTest = sCurrentProjectDir.IndexOf("UnitTesting");
                if (lenUnitTest > 0)
                {
                    sCurrentProjectDir = sCurrentProjectDir.Substring(0, lenUnitTest);
                }
            }
            return sCurrentProjectDir;
        }
        public static void DataRowCopy(DataRow drOld, DataRow drNew)
        {
            foreach (DataColumn col in drOld.Table.Columns)
            {
                drNew[col.ColumnName] = drOld[col.ColumnName];
            }
        }
        public static string DataRowReadField(DataRow dr, string field)
        {
            if (dr.Table.Columns.Contains(field) && dr[field] != DBNull.Value)
            {
                return (string)dr[field];
            }
            else
            {
                return string.Empty;
            }
        }

        public static string DetermineSQLItemName(string[] sSplit)
        {
            string itemName = string.Empty;
            bool usesQuotes = false;
            bool foundAdd = false;
            foreach (string s in sSplit)
            {
                if (s.StartsWith("/*") == true || s.StartsWith("--") == true)
                {
                    continue;
                }
                if (s.ToUpper().Contains("CREATE PROCEDURE") || s.ToUpper().Contains("CREATE FUNCTION"))
                {
                    int iLast;
                    iLast = s.LastIndexOf("]");
                    if (iLast != -1)
                    {
                        itemName = s.Substring(0, iLast).Trim();
                    }
                    else
                    {
                        itemName = s;
                    }

                    iLast = itemName.LastIndexOf('[');
                    itemName = itemName.Substring(iLast + 1);
                    itemName = itemName.Replace("CREATE PROCEDURE", "");
                    itemName = itemName.Replace("CREATE FUNCTION", "");
                    return itemName.Trim();
                }
                else if (s.ToUpper().Contains("CREATE TYPE"))
                {
                    int iLast;
                    iLast = s.LastIndexOf(" AS");
                    if (iLast != -1)
                    {
                        itemName = s.Substring(0, iLast).Trim();
                    }
                    else
                    {
                        itemName = s;
                    }

                    iLast = itemName.LastIndexOf('[');
                    itemName = (itemName.Substring(iLast + 1)).Replace("]", "");

                    return itemName;
                }
                else if (s.ToUpper().Contains("CREATE TABLE"))
                {
                    int iLast;
                    iLast = s.LastIndexOf("]");
                    if (iLast != -1)
                    {
                        itemName = s.Substring(0, iLast).Trim();
                    }
                    else
                    {
                        iLast = s.LastIndexOf(@"""");
                        if (iLast != -1)
                        {
                            itemName = s.Substring(0, iLast).Trim();
                            usesQuotes = true;
                        }
                        else
                        {
                            itemName = s;
                        }
                    }
                    if (usesQuotes)
                    {
                        iLast = itemName.IndexOf(@"""");
                    }
                    else
                    {
                        iLast = itemName.LastIndexOf('[');
                    }
                    itemName = itemName.Substring(iLast + 1);
                    itemName = itemName.ToUpper().Replace("CREATE TABLE", "");
                    return itemName.Trim();
                }
                else if (s.ToUpper().Contains("CREATE VIEW"))
                {
                    int iLast;
                    iLast = s.LastIndexOf("]");
                    if (iLast != -1)
                    {
                        itemName = s.Substring(0, iLast).Trim();
                    }
                    else
                    {
                        iLast = s.LastIndexOf(@"""");
                        if (iLast != -1)
                        {
                            itemName = s.Substring(0, iLast).Trim();
                            usesQuotes = true;
                        }
                        else
                        {
                            itemName = s;
                        }
                    }
                    if (usesQuotes)
                    {
                        iLast = itemName.IndexOf(@"""");
                    }
                    else
                    {
                        iLast = itemName.LastIndexOf('[');
                    }
                    itemName = itemName.Substring(iLast + 1);
                    itemName = itemName.ToUpper().Replace("CREATE VIEW", "");
                    return itemName.Trim();
                }
                else if (s.ToUpper().Contains("CREATE TRIGGER"))
                {
                    int iLast;
                    iLast = s.LastIndexOf("]");
                    if (iLast != -1)
                    {
                        itemName = s.Substring(0, iLast).Trim();
                    }
                    else
                    {
                        iLast = s.LastIndexOf(@"""");
                        if (iLast != -1)
                        {
                            itemName = s.Substring(0, iLast).Trim();
                            usesQuotes = true;
                        }
                        else
                        {
                            itemName = s;
                        }
                    }
                    if (usesQuotes)
                    {
                        iLast = itemName.IndexOf(@"""");
                    }
                    else
                    {
                        iLast = itemName.LastIndexOf('[');
                    }
                    itemName = itemName.Substring(iLast + 1);
                    itemName = itemName.ToUpper().Replace("CREATE TRIGGER", "");
                    return itemName.Trim();
                }
                else if ((foundAdd && s.ToUpper().Contains("CONSTRAINT")) ||
                    (s.ToUpper().Contains("ADD") && s.ToUpper().Contains("CONSTRAINT")) ||
                    (s.ToUpper().Contains("CHECK") && s.ToUpper().Contains("CONSTRAINT")))
                {
                    int iLast;
                    int iFirst;
                    iLast = s.LastIndexOf("]");
                    if (iLast != -1)
                    {
                        iFirst = s.ToUpper().IndexOf("CONSTRAINT");
                        iFirst = s.IndexOf("[", iFirst + 1);
                        iLast = s.IndexOf("]", iFirst + 1);
                        itemName = s.Substring(iFirst + 1, iLast - iFirst - 1).Trim();
                    }
                    else
                    {
                        iFirst = s.IndexOf(@"""");
                        if (iFirst != -1)
                        {
                            iLast = s.IndexOf(@"""", iFirst + 1);
                            itemName = s.Substring(iFirst + 1, iLast - iFirst - 1).Trim();
                            usesQuotes = true;
                        }
                        else
                        {
                            iFirst = s.ToUpper().IndexOf("CONSTRAINT");
                            iFirst = s.IndexOf(" ", iFirst + 5);
                            iLast = s.IndexOf(" ", iFirst + 1);
                            itemName = s.Substring(iFirst + 1, iLast - iFirst - 1).Trim();
                        }
                    }
                    return itemName.Trim();
                }
                else if (s.ToUpper().Contains(" ADD"))
                {
                    foundAdd = true;
                }
                else if (s.ToUpper().Contains("CREATE INDEX") || s.ToUpper().Contains("CREATE UNIQUE INDEX"))
                {
                    int iLast;
                    int iFirst;
                    iLast = s.LastIndexOf("]");
                    if (iLast != -1)
                    {
                        iFirst = s.ToUpper().IndexOf("INDEX");
                        iFirst = s.IndexOf("[", iFirst + 1);
                        iLast = s.IndexOf("]", iFirst + 1);
                        itemName = s.Substring(iFirst + 1, iLast - iFirst - 1).Trim();
                    }
                    else
                    {
                        iFirst = s.IndexOf(@"""");
                        if (iFirst != -1)
                        {
                            iLast = s.IndexOf(@"""", iFirst + 1);
                            itemName = s.Substring(iFirst + 1, iLast - iFirst - 1).Trim();
                            usesQuotes = true;
                        }
                        else
                        {
                            iFirst = s.ToUpper().IndexOf("INDEX");
                            iFirst = s.IndexOf(" ", iFirst + 1);
                            iLast = s.IndexOf(" ", iFirst + 1);
                            itemName = s.Substring(iFirst + 1, iLast - iFirst - 1).Trim();
                        }
                    }
                    return itemName.Trim();
                }

            }
            return itemName;
        }
    }

}
