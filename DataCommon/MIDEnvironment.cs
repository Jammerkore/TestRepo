using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;

using MIDRetail.Encryption;

namespace MIDRetail.DataCommon
{
    /// <summary>
    /// Summary description for Include.
    /// </summary>
    /// 
    public class MIDEnvironment
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public MIDEnvironment()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //===========
        // PROPERTIES
        //===========

        //========
        // METHODS
        //========

        public static bool ValidEnvironment(out string aMessage)
        {
            try
            {
                aMessage = string.Empty;
                // check first two positions of framework
                string strFrameworkVersion = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
                int index = strFrameworkVersion.IndexOf(".");
                index = strFrameworkVersion.IndexOf(".", index + 1);
                double frameworkVersion = Convert.ToDouble(strFrameworkVersion.Substring(1, index - 1));

                //if (System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion().StartsWith("v2.0"))
                if (frameworkVersion >= 2.0)
                {
                    return true;
                }
                aMessage = "Application requires Microsoft .NET Framework 2.0 or higher."
                        + Environment.NewLine
                        + "Please install before starting the application";
                return false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public static void SetDataTableGlobalization(DataTable aTable)
        {
            try
            {
                aTable.CaseSensitive = true;
                aTable.Locale = CultureInfo.InvariantCulture;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public static void SetDataSetGlobalization(DataSet aDataSet)
        {
            try
            {
                aDataSet.CaseSensitive = true;
                aDataSet.Locale = CultureInfo.InvariantCulture;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public static DataTable CreateDataTable()
        {
            try
            {
                DataTable dt = new DataTable();
                SetDataTableGlobalization(dt);
                return dt;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public static DataTable CreateDataTable(string tableName)
        {
            try
            {
                DataTable dt = new DataTable(tableName);
                SetDataTableGlobalization(dt);
                return dt;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public static DataTable CreateDataTable(string tableName, string tableNamespace)
        {
            try
            {
                DataTable dt = new DataTable(tableName, tableNamespace);
                SetDataTableGlobalization(dt);
                return dt;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public static DataSet CreateDataSet()
        {
            try
            {
                DataSet ds = new DataSet();
                SetDataSetGlobalization(ds);
                return ds;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public static DataSet CreateDataSet(string dataSetName)
        {
            try
            {
                DataSet ds = new DataSet(dataSetName);
                SetDataSetGlobalization(ds);
                return ds;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

    }

    public class MIDConfigurationManager
    {
        private static AppSettings _appSettings;
        //=============
        // CONSTRUCTORS
        //=============

        static MIDConfigurationManager()
        {
            _appSettings = new AppSettings();
        }

        //===========
        // PROPERTIES
        //===========

        public static AppSettings AppSettings
        {
            get
            {
                return _appSettings;
            }
        }
        //========
        // METHODS
        //========
    }

    public class AppSettings
    {
        private MIDEncryption _encryption;
        //=============
        // CONSTRUCTORS
        //=============

        public AppSettings()
        {
            _encryption = new MIDEncryption();
            //
            // TODO: Add constructor logic here
            //
        }

        //===========
        // PROPERTIES
        //===========

        public string this[string aName]
        {
            get
            {
                try
                {
                    string value = System.Configuration.ConfigurationManager.AppSettings[aName];
                    if (!String.IsNullOrEmpty(value))
                    {
                        value = _encryption.Decrypt(value);
                    }
                    else
                    {
                        // set default values
                        switch (aName)
                        {
                            case "ReaderLockTimeOut":
                                value = "10000";
                                break;
                            case "WriterLockTimeOut":
                                value = "10000";
                                break;
                            case "Delimiter":
                                value = "~";
                                break;
                        }
                    }
                    return value;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

        public string this[int aIndex]
        {
            get
            {
                try
                {
                    string value = System.Configuration.ConfigurationManager.AppSettings[aIndex];
                    if (!String.IsNullOrEmpty(value))
                    {
                        value = _encryption.Decrypt(value);
                    }
                    return value;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

        //========
        // METHODS
        //========
    }
}
