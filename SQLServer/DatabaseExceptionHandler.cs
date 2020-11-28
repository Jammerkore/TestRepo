using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    internal class DatabaseExceptionHandler
    {
        internal DatabaseExceptionHandler()
		{

		}

        internal void HandleDatabaseException(SqlException aSqlException, string aMIDCommand, SqlCommand aSqlCommand)
        {
            try
            {
                string sErrorMessage = "";
                if (aSqlException != null)
                {
                    for (int i = 0; i < aSqlException.Errors.Count; i++)
                        sErrorMessage += aSqlException.Errors[i].Number.ToString()
                            + ":" + aSqlException.Errors[i].Message + "\n";
                }

                string sParameters = "";
                string sParametersShort = "";
                string sValue;
                // Begin TT#3302 - JSmith - Size Curves Failures
                string sDirection = "None";
                string sDbType = "None";
                // End TT#3302 - JSmith - Size Curves Failures
                if (aSqlCommand != null &&
                    aSqlCommand.Parameters != null)
                {
                    foreach (SqlParameter sp in aSqlCommand.Parameters)
                    {
                        // Begin TT#3302 - JSmith - Size Curves Failures
                        //if (sp.Value == null)
                        if (sp.Value == null ||
                            sp.Value == System.DBNull.Value)
                        // End TT#3302 - JSmith - Size Curves Failures
                        {
                            sValue = " ";
                        }
                        // Begin TT#3302 - JSmith - Size Curves Failures
                        else if (sp.SqlDbType == SqlDbType.Structured)
                        {
                            if (sp.TypeName == null ||
                                sp.TypeName.Trim().Length == 0)
                            {
                                sValue = "Structured Data";
                            }
                            else
                            {
                                sValue = sp.TypeName;
                            }
                        }
                        // End TT#3302 - JSmith - Size Curves Failures
                        else
                        {
                            sValue = sp.Value.ToString();
                        }
                        // Begin TT#3302 - JSmith - Size Curves Failures
                        //sParameters += "Name=" + sp.ParameterName + ":"
                        //            + "Direction=" + sp.Direction.ToString() + ":"
                        //            + "Type=" + sp.DbType.ToString() + ":"
                        //            + "Value=" + sValue + Environment.NewLine;

                        ////sValue = sp.Value.ToString();
                        //if (sValue.Length > 100)
                        //{
                        //    sValue = sValue.Substring(0, 99);
                        //}
                        //sParametersShort += "Name=" + sp.ParameterName + ":"
                        //            + "Direction=" + sp.Direction.ToString() + ":"
                        //            + "Type=" + sp.DbType.ToString() + ":"
                        //            + "Value=" + sValue + Environment.NewLine;

                        //Begin TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
                        //if (sp.Direction == null)
                        //{
                        //    sDirection = "None";
                        //}
                        //else
                        //{
                            sDirection = sp.Direction.ToString();
                        //}
                        
                        //if (sp.DbType == null)
                        //{
                        //    sDbType = "None";
                        //}
                        //else
                        //{
                            sDbType = sp.DbType.ToString();
                        //}
                        //End TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
                        if (sValue.Length > 5000)
                        {
                            sValue = sValue.Substring(0, 4999);
                        }
                        sParameters += "Name=" + sp.ParameterName + ":"
                                    + "Direction=" + sDirection + ":"
                                    + "Type=" + sDbType + ":"
                                    + "Value=" + sValue + Environment.NewLine;

                        //sValue = sp.Value.ToString();
                        if (sValue.Length > 100)
                        {
                            sValue = sValue.Substring(0, 99);
                        }
                        sParametersShort += "Name=" + sp.ParameterName + ":"
                                    + "Direction=" + sDirection + ":"
                                    + "Type=" + sDbType + ":"
                                    + "Value=" + sValue + Environment.NewLine;
                        // End TT#3302 - JSmith - Size Curves Failures
                    }
                }

                // Begin TT#3302 - JSmith - Size Curves Failures
                //System.Data.SqlClient.SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(aSqlCommand.Connection.ConnectionString);
                //builder.Password = "*******";
                string sConnectionString = null;
                if (aSqlCommand != null &&
                    aSqlCommand.Connection != null &&
                    aSqlCommand.Connection.ConnectionString != null)
                {
				    try
                    {
                        System.Data.SqlClient.SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(aSqlCommand.Connection.ConnectionString);
                        builder.Password = "*******";
                        sConnectionString = builder.ToString();
					}
                    catch
                    {
                        sConnectionString = null;
                    }
                }
                // End TT#3302 - JSmith - Size Curves Failures

                Exception err = null;

                // Begin RO-2722 - AGallagher - Database Password Logged in Plain Text
                if (aMIDCommand.Contains("Password="))
                {
                    string parse1 = aMIDCommand;
                    int parsefirstStringPosition = parse1.IndexOf("Password=");
                    parsefirstStringPosition = parsefirstStringPosition + 8;
                    int parsesecondStringPosition = parse1.IndexOf("Error=");
                    string result1 = parse1.Substring(1, parsefirstStringPosition);
                    string result2 = parse1.Substring(parsesecondStringPosition, parsefirstStringPosition);
                    string result3 = (result1 + "******** " + result2);
                    aMIDCommand = result3;
                }
                // End RO-2722 - AGallagher - Database Password Logged in Plain Text

                switch (aSqlException.Number)
                {
                    case (int)eDatabaseError.Timeout:
                    case (int)eDatabaseError.Blocking:
                    case (int)eDatabaseError.ScanErrorWithNolock:
                    case (int)eDatabaseError.DeadLock:
                    case (int)eDatabaseError.DeadLock2:
                    case (int)eDatabaseError.GeneralNetworkError:
                    // Begin TT#3771 - JSmith - Add Database Retry for Network Timeout
                    case (int)eDatabaseError.TCPSemaphoreTimeout:  
                    case (int)eDatabaseError.SQLNotExistOrAccessDenied:  
                    case (int)eDatabaseError.SQLNotExistOrAccessDenied2:  
                    case (int)eDatabaseError.UnableToReadLoginPacket:  
                    case (int)eDatabaseError.UnableToCloseServerSideConnection:  
                    case (int)eDatabaseError.UnableToWriteToServerSideConnection:  
                    case (int)eDatabaseError.CannotSendAfterSocketShutdown:  
                    case (int)eDatabaseError.ConnectionResetByPeer:  
                    case (int)eDatabaseError.SoftwareCausedConnectionAbort:  
                    case (int)eDatabaseError.NetworkErrorWasEncounteredWhileSendingResults:  
                    case (int)eDatabaseError.ThePipeIsBeingClosed:  
                    case (int)eDatabaseError.ThePipeHasBeenEnded:  
                    case (int)eDatabaseError.BadTokenFromSQLServer:  
                    case (int)eDatabaseError.ReadFromSQLServerFailed:  
                    case (int)eDatabaseError.ErrorClosingNetworkConnection:  
                    case (int)eDatabaseError.WriteToSQLServerFailed:  
                    // End TT#3771 - JSmith - Add Database Retry for Network Timeout
                        // Begin TT#3302 - JSmith - Size Curves Failures
                        //err = new DatabaseRetryableViolation(FormatMessage(Include.ErrorDatabase, builder.ToString(), aMIDCommand, sParameters, sErrorMessage),
                        //    FormatMessage(Include.ErrorDatabase, null, aMIDCommand, sParametersShort, sErrorMessage));
                        err = new DatabaseRetryableViolation(FormatMessage(Include.ErrorDatabase, sConnectionString, aMIDCommand, sParameters, sErrorMessage),
                            FormatMessage(Include.ErrorDatabase, null, aMIDCommand, sParametersShort, sErrorMessage));
                        // End TT#3302 - JSmith - Size Curves Failures
                        break;
                    case (int)eDatabaseError.ForeignKeyViolation:
                        // Begin TT#3302 - JSmith - Size Curves Failures
                        //EventLog.WriteEntry("MIDRetail", FormatMessage("ForeignKeyViolation Database error", builder.ToString(), aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        EventLog.WriteEntry("MIDRetail", FormatMessage("ForeignKeyViolation Database error", sConnectionString, aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        // End TT#3302 - JSmith - Size Curves Failures
                        err = new DatabaseForeignKeyViolation(FormatMessage(Include.ErrorDatabase, null, null, null, sErrorMessage));
                        break;
                    case (int)eDatabaseError.InvalidDatabase:
                        // Begin TT#3302 - JSmith - Size Curves Failures
                        //EventLog.WriteEntry("MIDRetail", FormatMessage("InvalidDatabase Database error", builder.ToString(), aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        EventLog.WriteEntry("MIDRetail", FormatMessage("InvalidDatabase Database error", sConnectionString, aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        // End TT#3302 - JSmith - Size Curves Failures
                        err = new MIDDatabaseUnavailableException(FormatMessage(Include.ErrorDatabase, null, null, null, sErrorMessage));
                        break;
                    case (int)eDatabaseError.LoginFailed:
                        // Begin TT#3302 - JSmith - Size Curves Failures
                        //EventLog.WriteEntry("MIDRetail", FormatMessage("LoginFailed Database error", builder.ToString(), aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        EventLog.WriteEntry("MIDRetail", FormatMessage("LoginFailed Database error", sConnectionString, aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        // End TT#3302 - JSmith - Size Curves Failures
                        err = new DatabaseLoginFailed(FormatMessage(Include.ErrorDatabase, null, null, null, sErrorMessage));
                        break;
                    case (int)eDatabaseError.UniqueIndexConstriantViolation:
                        // Begin TT#3302 - JSmith - Size Curves Failures
                        //EventLog.WriteEntry("MIDRetail", FormatMessage("UniqueIndexConstriantViolation Database error", builder.ToString(), aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        EventLog.WriteEntry("MIDRetail", FormatMessage("UniqueIndexConstriantViolation Database error", sConnectionString, aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        // End TT#3302 - JSmith - Size Curves Failures
                        err = new DatabaseUniqueIndexConstriantViolation(FormatMessage(Include.ErrorDatabase, null, null, null, sErrorMessage));
                        break;
                    case (int)eDatabaseError.UniqueIndexConstriantViolation2:
                        // Begin TT#3302 - JSmith - Size Curves Failures
                        //EventLog.WriteEntry("MIDRetail", FormatMessage("UniqueIndexConstriantViolation2 Database error", builder.ToString(), aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        EventLog.WriteEntry("MIDRetail", FormatMessage("UniqueIndexConstriantViolation2 Database error", sConnectionString, aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        // End TT#3302 - JSmith - Size Curves Failures
                        err = new DatabaseUniqueIndexConstriantViolation2(FormatMessage(Include.ErrorDatabase, null, null, null, sErrorMessage));
                        break;
                    case (int)eDatabaseError.NotInCatalog:
                        err = new DatabaseNotInCatalog(FormatMessage(Include.ErrorDatabase, null, null, null, sErrorMessage));
                        break;
                    case (int)eDatabaseError.ParallelQueryThreadError:
                        err = new ParallelQueryThreadError(FormatMessage(Include.ErrorDatabase, null, null, null, sErrorMessage));
                        break;
                    case 50000:
                        System.Diagnostics.StackTrace t = new System.Diagnostics.StackTrace();
                        if (t.ToString().Length > 32700)
                        {
                            EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString().Substring(0, 32700), EventLogEntryType.Error);
                        }
                        else
                        {
                            EventLog.WriteEntry("MIDRetail", "Call Stack" + Environment.NewLine + t.ToString(), EventLogEntryType.Error);
                        }
                        EventLog.WriteEntry("MIDRetail", FormatMessage("Database error=" + aSqlException.Number.ToString(), sConnectionString, aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        err = new Exception(Include.ErrorDatabase + sErrorMessage);
                        break;
                    default:
                        // Begin TT#3302 - JSmith - Size Curves Failures
                        //EventLog.WriteEntry("MIDRetail", FormatMessage("Database error=" + aSqlException.Number.ToString(), builder.ToString(), aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        EventLog.WriteEntry("MIDRetail", FormatMessage("Database error=" + aSqlException.Number.ToString(), sConnectionString, aMIDCommand, sParameters, sErrorMessage), EventLogEntryType.Error);
                        // End TT#3302 - JSmith - Size Curves Failures
                        err = new Exception(Include.ErrorDatabase + sErrorMessage);
                        break;
                    }

                throw err;
            }
            catch (Exception error)
            {
                string message = error.ToString();
                throw;
            }
        }

        // Begin TT#3325 - JSmith - Event Log String Too Long - Error Message
        //private string FormatMessage(string aMessage, string aConnectionString, string aCommand, string aParameters, string aError)
        //{
        //    string sMessage = aMessage;
        //    if (aConnectionString != null)
        //    {
        //        sMessage += Environment.NewLine + "ConnectionString=" + aConnectionString;
        //    }
        //    if (aCommand != null)
        //    {
        //        sMessage += Environment.NewLine + "Command=" + aCommand;
        //    }
        //    if (aParameters != null)
        //    {
        //        sMessage += Environment.NewLine + "Parameters" + Environment.NewLine + aParameters;
        //    }
        //    if (aError != null)
        //    {
        //        sMessage += Environment.NewLine + "Error=" + aError;
        //    }

        //    Debug.WriteLine(sMessage.ToString());

        //    return sMessage.ToString();
        //}
        private string FormatMessage(string aMessage, string aConnectionString, string aCommand, string aParameters, string aError)
        {
            string sMessage = aMessage;
            if (aConnectionString != null &&
                aConnectionString.Trim().Length > 0)
            {
                sMessage += Environment.NewLine + "ConnectionString=" + aConnectionString.Trim();
            }

            if (aCommand != null &&
                aCommand.Trim().Length > 0)
            {
                sMessage += Environment.NewLine + "Command=" + aCommand.Trim();
            }

            if (aParameters != null &&
                aParameters.Trim().Length > 0)
            {
                sMessage += Environment.NewLine + "Parameters" + Environment.NewLine + aParameters.Trim();
            }

            if (aError != null &&
                aError.Trim().Length > 0)
            {
                sMessage += Environment.NewLine + "Error=" + aError.Trim();  
            }

            Debug.WriteLine(sMessage.ToString());

            if (sMessage.Length > 32700)
            {
                return sMessage.ToString().Substring(0, 32700);
            }
            else
            {
                return sMessage.ToString();
            }
        }
        // End TT#3325 - JSmith - Event Log String Too Long - Error Message
    }

}
