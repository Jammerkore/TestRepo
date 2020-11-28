using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace UnitTesting
{
    public static class Shared_SetParameter
    {

        public static void SetParametersForProcedure(MIDRetail.Data.baseStoredProcedure sp, DataSet dsParameters, string connectionString)
        {
            if (sp == null)
            {
                return;
            }

            foreach (baseParameter param in sp.inputParameterList)
            {
                DataRow[] drParmValue = dsParameters.Tables[0].Select("parameterName='" + param.parameterName + "'");
                if (drParmValue.Length > 0)
                {
                    if (param.DBType == MIDRetail.DataCommon.eDbType.Int)
                    {
                        intParameter p = (MIDRetail.Data.intParameter)param;
                        int? testValue = ConvertToIntNullable((string)drParmValue[0]["parameterValue"]);
                        p.SetValue(testValue);
                    }
                    else if (param.DBType == MIDRetail.DataCommon.eDbType.Int64)
                    {
                        longParameter p = (MIDRetail.Data.longParameter)param;
                        long? testValue = ConvertToLongNullable((string)drParmValue[0]["parameterValue"]);
                        p.SetValue(testValue);
                    }
                    else if (param.DBType == MIDRetail.DataCommon.eDbType.Float)
                    {
                        floatParameter p = (MIDRetail.Data.floatParameter)param;
                        double? testValue = ConvertToDoubleNullable((string)drParmValue[0]["parameterValue"]);
                        p.SetValue(testValue);
                    }
                    else if (param.DBType == MIDRetail.DataCommon.eDbType.Decimal)
                    {
                        decimalParameter p = (MIDRetail.Data.decimalParameter)param;
                        decimal? testValue = ConvertToDecimalNullable((string)drParmValue[0]["parameterValue"]);
                        p.SetValue(testValue);
                    }
                    else if (param.DBType == MIDRetail.DataCommon.eDbType.Image)
                    {
                        byteArrayParameter p = (MIDRetail.Data.byteArrayParameter)param;
                        byte[] testValue = (byte[])drParmValue[0]["parameterValue"];
                        p.SetValue(testValue);
                    }
                    else if (param.DBType == MIDRetail.DataCommon.eDbType.VarChar)
                    {
                        stringParameter p = (MIDRetail.Data.stringParameter)param;
                        string testValue = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                        if (drParmValue[0]["parameterValue"] != DBNull.Value)
                        {
                            testValue = ConvertToStringNullable((string)drParmValue[0]["parameterValue"]);
                        }
                        p.SetValue(testValue);
                    }
                    else if (param.DBType == MIDRetail.DataCommon.eDbType.Text)
                    {
                        textParameter p = (MIDRetail.Data.textParameter)param;
                        string testValue = ConvertToStringNullable((string)drParmValue[0]["parameterValue"]);
                        p.SetValue(testValue);
                    }
                    else if (param.DBType == MIDRetail.DataCommon.eDbType.Char)
                    {
                        charParameter p = (MIDRetail.Data.charParameter)param;
                        char? testValue = ConvertToCharNullable((string)drParmValue[0]["parameterValue"]);
                        p.SetValue(testValue);
                    }
                    else if (param.DBType == MIDRetail.DataCommon.eDbType.DateTime)
                    {
                        datetimeParameter p = (MIDRetail.Data.datetimeParameter)param;
                        DateTime? testValue = ConvertToDateTimeNullable((string)drParmValue[0]["parameterValue"]);
                        p.SetValue(testValue);
                    }
                    else if (param.DBType == MIDRetail.DataCommon.eDbType.Structured)
                    {
                        //parameterValue example for single column datatable: 5000;5002;5003
                        //parameterValue example for two column datatable: 5000,101;5002,201;5003,301

                        tableParameter p = (MIDRetail.Data.tableParameter)param;

                        string temptableParamValues = (string)drParmValue[0]["parameterValue"];
                        string[] tableParamValues = temptableParamValues.Split(';');

                        string selectCmd = "DECLARE @T " + p.typeName + "; SELECT * FROM @T;";
                        bool hasError = false;
                        string failureMsg = string.Empty;
                        DataTable testValues = Shared_GenericExecution.GetGenericDataTable(connectionString, selectCmd, out hasError, out failureMsg);

                        //string[] columnNames = p.columnNames.Split(';');
                        //string[] columnTypes = p.columnTypes.Split(';');

                        //DataTable testValues = new DataTable();
                        //int iColumnCounter = 0;
                        //foreach (string colName in columnNames)
                        //{
                        //    if (columnTypes[iColumnCounter] == "int")
                        //    {
                        //        testValues.Columns.Add(colName, typeof(int));
                        //    }
                        //    else
                        //    {
                        //        testValues.Columns.Add(colName, typeof(string)); //varchar
                        //    }
                        //    iColumnCounter++;
                        //}


                        foreach (string tableParamValue in tableParamValues)
                        {
                            if (tableParamValue != string.Empty)
                            {
                                string[] fieldParamValues = tableParamValue.Split(',');
                                DataRow dr = testValues.NewRow();

                                int iFieldCounter = 0;
                                foreach (DataColumn dc in testValues.Columns)
                                {
                                    //if (columnTypes[iColumnCounter] == "int")
                                    //{
                                    //    int tempVal;
                                    //    if (int.TryParse(fieldParamValues[iFieldCounter], out tempVal) == true)
                                    //    {
                                    //        dr[colName] = tempVal;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    dr[colName] = fieldParamValues[iFieldCounter];
                                    //}

                                    if (dc.DataType == typeof(int))
                                    {
                                        int tempVal;
                                        if (int.TryParse(fieldParamValues[iFieldCounter], out tempVal) == true)
                                        {
                                            dr[dc] = tempVal;
                                        }
                                    }
                                    else if (dc.DataType == typeof(float))
                                    {
                                        float tempVal;
                                        if (float.TryParse(fieldParamValues[iFieldCounter], out tempVal) == true)
                                        {
                                            dr[dc] = tempVal;
                                        }
                                    }
                                    else if (dc.DataType == typeof(DateTime))
                                    {
                                        DateTime tempVal;
                                        if (DateTime.TryParse(fieldParamValues[iFieldCounter], out tempVal) == true)
                                        {
                                            dr[dc] = tempVal;
                                        }
                                    }
                                    else
                                    {
                                        dr[dc] = fieldParamValues[iFieldCounter];
                                    }

                                    iFieldCounter++;
                                }

                                testValues.Rows.Add(dr);
                            }
                        }
                        p.SetValue(testValues);
                    }
                }
            }
        }




        private static int? ConvertToIntNullable(string sVal)
        {
            int? valNullable = null;
            int val;
            if (sVal == null || sVal == string.Empty)
            {
                valNullable = null;
            }
            else
            {
                if (int.TryParse(sVal, out val))
                {
                    valNullable = val;
                }
            }
            return valNullable;
        }
        private static long? ConvertToLongNullable(string sVal)
        {
            long? valNullable = null;
            long val;
            if (sVal == null || sVal == string.Empty)
            {
                valNullable = null;
            }
            else
            {
                if (long.TryParse(sVal, out val))
                {
                    valNullable = val;
                }
            }
            return valNullable;
        }
        private static double? ConvertToDoubleNullable(string sVal)
        {
            double? valNullable = null;
            double val;
            if (sVal == null || sVal == string.Empty)
            {
                valNullable = null;
            }
            else
            {
                if (double.TryParse(sVal, out val))
                {
                    valNullable = val;
                }
            }
            return valNullable;
        }
        private static decimal? ConvertToDecimalNullable(string sVal)
        {
            decimal? valNullable = null;
            decimal val;
            if (sVal == null || sVal == string.Empty)
            {
                valNullable = null;
            }
            else
            {
                if (decimal.TryParse(sVal, out val))
                {
                    valNullable = val;
                }
            }
            return valNullable;
        }
        private static string ConvertToStringNullable(string sVal)
        {
            string val;
            if (sVal == null || sVal == string.Empty || sVal.ToUpper() == Include.NullForStringValue)  //TT#1310-MD -jsobek -Error when adding a new Store)
            {
                val = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
            }
            else
            {
                val = sVal;
            }
            return val;
        }

        private static char? ConvertToCharNullable(string sVal)
        {
            char? valNullable = null;
            char val;
            if (sVal == null || sVal == string.Empty)
            {
                valNullable = null;
            }
            else
            {
                if (char.TryParse(sVal, out val))
                {
                    valNullable = val;
                }
            }
            return valNullable;
        }
        private static DateTime? ConvertToDateTimeNullable(string sVal)
        {
            DateTime? valNullable = null;
            DateTime val;
            if (sVal == null || sVal == string.Empty)
            {
                valNullable = null;
            }
            else
            {
                if (DateTime.TryParse(sVal, out val))
                {
                    valNullable = val;
                }
            }
            return valNullable;
        }
    }

}
