using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Globalization;
using System.Data;

using MID.MRS.Business;
using MID.MRS.Common;
using MID.MRS.DataCommon;
using MID.MRS.Data;

namespace MID.MRS.PlanForecastBalance
{
	/// <summary>
	/// Summary description for PlanForecastBalance.
	/// </summary>
	class PlanForecastBalance
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// <remarks>
		/// First Arg = Method_RID
		/// Second Arg (optional) = Proccess_RID
		/// </remarks>
		[STAThread]
		static int Main(string[] args)
		{
			string sourceModule = "PlanForecastBalance.cs";
			string eventLogID = "PlanForecastBalance";
			SessionAddressBlock SAB;
			SessionSponsor sponsor;
			IMessageCallback messageCallback;
			DateTime start = DateTime.Now;
			string message = null;
			messageCallback = new BatchMessageCallback();
			sponsor = new SessionSponsor();
			SAB = new SessionAddressBlock(messageCallback, sponsor);
			BinaryServerFormatterSinkProvider provider; 
			Hashtable port;
			System.Runtime.Remoting.Channels.IChannel channel;
			int _taskListRid = Include.NoRID;
			int _taskSeq = 0;
			ScheduleData _scheduleData;
			bool errorFound = false;
			eMIDMessageLevel highestMessage;
			
			try
			{
				if (!EventLog.SourceExists(eventLogID))
				{
					EventLog.CreateEventSource(eventLogID, null);
				}				

				//==========================
				// Register callback channel
				//==========================
				try
				{
					channel = SAB.OpenCallbackChannel();
				}
				catch (Exception e)
				{
					EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + e.Message, EventLogEntryType.Error);
					throw;
				}

				//===========================
				// edit for arguments
				//===========================
				if (args.Length > 0)
				{
					_taskListRid = Convert.ToInt32(args[0]);
					if (args.Length > 1)
					{
						_taskSeq = Convert.ToInt32(args[1]);
					}
					else
					{
						EventLog.WriteEntry(eventLogID, "Missing Argument: Task Sequence", EventLogEntryType.Error);
					}
				}
				else
				{
					EventLog.WriteEntry(eventLogID, "Missing Argument: Task List RID", EventLogEntryType.Error);
				}

				//==================
				// Create Sessions
				//==================
				try
				{
					SAB.CreateSessions((int)eServerType.Client|(int)eServerType.Application|(int)eServerType.Store|(int)eServerType.Hierarchy);
				}
				catch (Exception ex)
				{
					errorFound = true;
					Exception innerE = ex;
					while (innerE.InnerException != null) 
					{
						innerE = innerE.InnerException;
					}
					EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe,CultureInfo.CurrentUICulture);
				}

				//=================================
				// get user rid from tasklist table
				//=================================
				_scheduleData = new ScheduleData();
				DataRow taskListRow = _scheduleData.TaskList_Read(_taskListRid);
				int userRid = Include.UndefinedUserRID;
				if (taskListRow != null)
				{
					userRid = Convert.ToInt32(taskListRow["USER_RID"], CultureInfo.CurrentUICulture);
				}
				else
				{
					// ERROR - tasklist not found

				}

				eSecurityAuthenticate authentication = 
					SAB.ClientServerSession.UserLogin(userRid, eProcesses.forecastBalancing);

				if (authentication != eSecurityAuthenticate.ActiveUser)
				{
					EventLog.WriteEntry(eventLogID, "Unable to log in with user RID:" + userRid.ToString(), EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user RID:" + userRid.ToString());
					errorFound = true;
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}
				
				SAB.ClientServerSession.Initialize();
				SAB.ApplicationServerSession.Initialize();
				SAB.StoreServerSession.Initialize();
				SAB.HierarchyServerSession.Initialize();

				//=========================================
				// get the forecast balance tasks for this request
				//=========================================

				DataTable dtTaskForecastBalance = _scheduleData.TaskForecastBalance_ReadByTaskList(_taskListRid, _taskSeq);
				int rowCount = dtTaskForecastBalance.Rows.Count;
				int r=0;
				int forecastBalanceSeq = 0, nodeRid = 0, versionRid = 0;
				bool dateInRange = false;
				//================================================
				// gather up all the forecast balances for this Task List
				//================================================
				for (r=0;r<rowCount;r++)
				{
					DataRow aRow = dtTaskForecastBalance.Rows[r];
					forecastBalanceSeq = Convert.ToInt32(aRow["FORECAST_BALANCE_SEQUENCE"],CultureInfo.CurrentUICulture);
					if (aRow["HN_RID"] == DBNull.Value)
						nodeRid = Include.NoRID;
					else
						nodeRid = Convert.ToInt32(aRow["HN_RID"],CultureInfo.CurrentUICulture);
					if (aRow["FV_RID"] == DBNull.Value)
						versionRid = Include.NoRID;
					else
						versionRid = Convert.ToInt32(aRow["FV_RID"],CultureInfo.CurrentUICulture);
				
					DataTable dtTaskForecastBalanceDetail = _scheduleData.TaskForecastBalanceDetail_ReadByTaskList(_taskListRid, _taskSeq, forecastBalanceSeq);
					int DetailRowCount = dtTaskForecastBalanceDetail.Rows.Count;
					int r2=0;
					//=================================================================================
					// For each forecast balance detail, check to see if current date is within date range.
					// if it is, run the forecast.
					//=================================================================================
					for (r2=0;r2<DetailRowCount;r2++)
					{
						DataRow aDetailRow = dtTaskForecastBalanceDetail.Rows[r2];
						int detailSeq = Convert.ToInt32(aDetailRow["DETAIL_SEQUENCE"],CultureInfo.CurrentUICulture);
						int methodRid = Convert.ToInt32(aDetailRow["METHOD_RID"],CultureInfo.CurrentUICulture); 
						if (aDetailRow["EXECUTE_CDR_RID"] == System.DBNull.Value)
							dateInRange = true;
						else
						{
							int cdrRid = Convert.ToInt32(aDetailRow["EXECUTE_CDR_RID"],CultureInfo.CurrentUICulture);
							dateInRange = SAB.ApplicationServerSession.Calendar.IsCurrentDateWithinRange(cdrRid);
						}

						//==============================
						//  Process the Forecast Balance
						//==============================
						if (dateInRange)
						{
							ApplicationSessionTransaction _transaction = SAB.ApplicationServerSession.CreateTransaction();

							//================================================================================
							// this adds the node and version to the forecast balance override list.
							// the override list is used during forecasting to override the node and version
							// specified on the method.
							//================================================================================
							_transaction.ForecastingBalanceOverride_ClearAll();
							_transaction.ForecastingBalanceOverride_Add(nodeRid, versionRid);

							_transaction.ProcessMethod(eMethodType.ForecastBalance, methodRid);

							if (_transaction != null)
							{
								_transaction.Dispose();
							}
						}
					}
				}

			}
			catch ( Exception err )
			{
				message = err.Message;
				SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, err.Message, sourceModule);
				errorFound = true;
			}
			finally
			{ 
				if (!errorFound)
				{
					if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
					}
				}
				else
				{
					if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
					{
						SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
					}
				}

				highestMessage = SAB.CloseSessions();
			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}
	}
}
