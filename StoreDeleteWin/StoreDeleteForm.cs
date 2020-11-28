using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.StoreDelete
{
	public partial class StoreDeleteForm : Form
	{
		private bool _changePending = false;
		private bool _formLoaded = false;
		private bool _analysisOnly = false;
		private string _connectionString;
		string eventLogID = "MIDStoreDelete";
		ArrayList _syncList = new ArrayList();

		public StoreDeleteForm()
		{
			if (!EventLog.SourceExists(eventLogID))
			{
				EventLog.CreateEventSource(eventLogID, null);
			}

			try
			{
				InitializeComponent();
			}
			catch (Exception ex)
			{
				//MessageBox.Show("Cannot start application..." + ex.ToString());
				EventLog.WriteEntry(eventLogID, "Cannot start application..." + ex.ToString(), EventLogEntryType.Error);
			}

		}

		private void StoreDeleteForm_Load(object sender, EventArgs e)
		{
			try
			{
				_formLoaded = false;
				MyListBox.SetListBox(this.lbAnalysis);
				_connectionString = MIDConfigurationManager.AppSettings["ConnectionString"];
                MIDConnectionString.ConnectionString = _connectionString;  // TT#2131-MD - JSmith - Halo Integration
				GetConfiguration();
				CheckAnalysisDates();
				CheckResetButton();
				SetText();
				EnableSaveButton(false);
				_formLoaded = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading application..." + ex.ToString());
				EventLog.WriteEntry(eventLogID, "Error loading application..." + ex.ToString(), EventLogEntryType.Error);

			}
		}

		private void GetConfiguration()
		{
			try
			{
				string strParm = ConfigurationManager.AppSettings["BatchSize"];
				if (strParm != null)
				{
					try
					{
						tbBatchSize.Text = strParm;
					}
					catch
					{
					}
				}

				strParm = ConfigurationManager.AppSettings["CopyConcurrentProcesses"];
				if (strParm != null)
				{
					try
					{
						tbConProcessCopy.Text = strParm;
					}
					catch
					{
					}
				}

				strParm = ConfigurationManager.AppSettings["DeleteConcurrentProcesses"];
				if (strParm != null)
				{
					try
					{
						tbConProcessDelete.Text = strParm;
					}
					catch
					{
					}
				}

				//strParm = ConfigurationManager.AppSettings["SkipAnalysis"];
				//if (strParm != null)
				//{
				//    try
				//    {
				//        _skipAnalysis = Include.ConvertStringToBool(strParm);
				//    }
				//    catch
				//    {
				//    }
				//}

				//strParm = ConfigurationManager.AppSettings["AnalysisOnly"];
				//if (strParm != null)
				//{
				//    try
				//    {
				//        _analysisOnly = Include.ConvertStringToBool(strParm);
				//    }
				//    catch
				//    {
				//    }
				//}

				strParm = ConfigurationManager.AppSettings["RowPercentageThreshold"];
				if (strParm != null)
				{
					try
					{
						tbRowPct.Text = strParm;
					}
					catch
					{
					}
				}

				strParm = ConfigurationManager.AppSettings["MinimumRowCount"];
				if (strParm != null)
				{
					try
					{
						tbMinRowCount.Text = strParm;
					}
					catch
					{
					}
				}

				strParm = ConfigurationManager.AppSettings["MaximumRowCount"];
				if (strParm != null)
				{
					try
					{
						tbMaxRowCount.Text = strParm;
					}
					catch
					{
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void SaveConfiguration()
		{
			// Open App.Config of executable
			System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			// Modify an Application Setting.
			config.AppSettings.Settings.Remove("CopyConcurrentProcesses"); config.AppSettings.Settings.Add("CopyConcurrentProcesses", tbConProcessCopy.Text);
			config.AppSettings.Settings.Remove("DeleteConcurrentProcesses"); config.AppSettings.Settings.Add("DeleteConcurrentProcesses", tbConProcessDelete.Text);
			config.AppSettings.Settings.Remove("BatchSize"); config.AppSettings.Settings.Add("BatchSize", tbBatchSize.Text);
			config.AppSettings.Settings.Remove("MinimumRowCount"); config.AppSettings.Settings.Add("MinimumRowCount", tbMinRowCount.Text);
			config.AppSettings.Settings.Remove("MaximumRowCount"); config.AppSettings.Settings.Add("MaximumRowCount", tbMaxRowCount.Text);
			config.AppSettings.Settings.Remove("RowPercentageThreshold"); config.AppSettings.Settings.Add("RowPercentageThreshold", tbRowPct.Text);
			config.AppSettings.Settings.Remove("AnalysisOnly"); config.AppSettings.Settings.Add("AnalysisOnly", _analysisOnly.ToString());

			// Save the changes in App.config file.
			config.Save(ConfigurationSaveMode.Modified);  // Save changes
			// Force a reload of a changed section.
			ConfigurationManager.RefreshSection("appSettings");

			config.AppSettings.Settings.Remove("MIDEnvironment");
			config.AppSettings.Settings.Remove("ControlServer");
			config.AppSettings.Settings.Remove("LocalHierarchyServer");
			config.AppSettings.Settings.Remove("LocalStoreServer");
			config.AppSettings.Settings.Remove("LocalApplicationServer");
			config.AppSettings.Settings.Remove("HeaderReleaseFilePath");
			config.AppSettings.Settings.Remove("AutoUpgradePath");
			config.AppSettings.Settings.Remove("User");
			config.AppSettings.Settings.Remove("Password");
			config.AppSettings.Settings.Remove("ReportPath");
			config.AppSettings.Settings.Remove("Export_FileFormat");
			config.AppSettings.Settings.Remove("Export_CSVDelimeter");
			config.AppSettings.Settings.Remove("Export_CSVFileSuffix");
			config.AppSettings.Settings.Remove("Export_DateType");
			config.AppSettings.Settings.Remove("Export_PreinitValues");
			config.AppSettings.Settings.Remove("Export_ExcludeZeroValues");
			config.AppSettings.Settings.Remove("Export_FilePath");
			config.AppSettings.Settings.Remove("Export_AddDateStampToFileName");
			config.AppSettings.Settings.Remove("Export_AddTimeStampToFileName");
			config.AppSettings.Settings.Remove("Export_SplitFiles");
			config.AppSettings.Settings.Remove("Export_SplitNumberOfEntries");
			config.AppSettings.Settings.Remove("Export_ConcurrentProcesses");
			config.AppSettings.Settings.Remove("Export_CreateFlagFile");
			config.AppSettings.Settings.Remove("Export_FlagFileSuffix");
			config.AppSettings.Settings.Remove("Export_CreateEndFile");
			config.AppSettings.Settings.Remove("Export_EndFileSuffix");
			config.AppSettings.Settings.Remove("ConnectionString");


			// Save the changes in App.config file.
			config.Save(ConfigurationSaveMode.Modified);  // Save changes
			// Force a reload of a changed section.
			ConfigurationManager.RefreshSection("appSettings");


		}

		private void SetText()
		{
			this.toolTip1.SetToolTip(this.tbRowPct, MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteToolTipRowPct));
			this.toolTip1.SetToolTip(this.tbMaxRowCount, MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteToolTipRowMax));
			this.toolTip1.SetToolTip(this.tbMinRowCount, MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteToolTipRowMin));
			this.toolTip1.SetToolTip(this.tbBatchSize, MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteToolTipBatchSize));
			this.toolTip1.SetToolTip(this.tbConProcessDelete, MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteToolTipConcurrentDeletes));
			this.toolTip1.SetToolTip(this.tbConProcessCopy, MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteToolTipConcurrentCopies));
			this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreDelete);
			this.gbConfig.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreDeleteSettings);
			this.label1.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_NumConcurrentForCopy);
			this.label3.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_NumConcurrentForDelete);
			this.label7.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_BatchSize);
			this.label2.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MinimumRowCount);
			this.label4.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MaximumRowCount);
			this.label6.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_RowPctMaximum);
			this.gbAnalysis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Analysis);
			this.btProcessA.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ProcessAnalysis);
			this.label5.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ProcessAnalysis);
			this.btProcessSD.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ProcessStoreDelete);
			this.btReset.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ResetForRestart);
			this.btSave.Text = MIDText.GetTextOnly(eMIDTextCode.menu_File_Save);
			this.lbSave.Text = MIDText.GetTextOnly(eMIDTextCode.msg_SavingForcesSave);
		}

		private void btProcessSD_Click(object sender, EventArgs e)
		{
			btReset.Visible = false;
			Cursor.Current = Cursors.WaitCursor;
			_analysisOnly = false;
			SaveConfiguration();
			//msgDelete.Text = string.Empty;

			if (ReadyForStoreDelete(true))
			{
				StoreDeleteMain storeDelete = new StoreDeleteMain();
				int rc = storeDelete.Process(false, true, new StoreDeleteCommon.SendMessageDelegate(HandleMessage));
				if (rc == 0)
				{
					//msgDelete.Text = "Store Delete completed successfully";
					AddMessage("Store Delete completed successfully");
				}
				else
				{
					//msgDelete.Text = "Store Delete completed with errors";
					AddMessage("Store Delete completed with errors");

				}
			}
			else
			{
				//msgDelete.Text = "Store Delete did not start due to issues found.";
				AddMessage("Store Delete did not start due to issues found.");
			}

			Cursor.Current = Cursors.Default;
		}

		private void AddMessage(string msg)
		{
			lbAnalysis.Items.Add(msg);
			lbAnalysis.SelectedIndex = lbAnalysis.Items.Count - 1;
		}

		private void HandleException(string msg, Exception ex)
		{
			EventLog.WriteEntry(eventLogID, msg + " " + ex.ToString(), EventLogEntryType.Error);
            MessageBox.Show(msg + " " + ex.Message + Environment.NewLine + " (Check the Windows Event Viewer for more information.)"); //TT#4636-VStuart-Incorrectly checking if services are down-MID
        }

		private void HandleMessage(string msg)
		{
			try
			{
				if (lbAnalysis.InvokeRequired)
				{
					lbAnalysis.Invoke((MethodInvoker)delegate
					{
						lbAnalysis.Items.Add(msg);
						lbAnalysis.SelectedIndex = lbAnalysis.Items.Count - 1;
					});
				}
				else
				{
					lbAnalysis.Items.Add(msg);
					lbAnalysis.SelectedIndex = lbAnalysis.Items.Count - 1;
				}
			}
			catch (Exception exe)
			{
			}
		}

		private void btProcessA_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			_analysisOnly = true;
			SaveConfiguration();
			lbAnalysis.Items.Clear();

            //BEGIN TT#4636-VStuart-Incorrectly checking if services are down-MID
            if (ReadyForProcessAnalysis())
			{
				StoreDeleteMain storeDelete = new StoreDeleteMain();
				int rc = storeDelete.Process(true, false, new StoreDeleteCommon.SendMessageDelegate(HandleMessage));
				if (rc == 0)
				{
					EnableProcessButton(true);
                    lbAnalysis.Items.Add("Analysis has completed successfully.");   //TT#4636-VStuart-Incorrectly checking if services are down-MID
					lbAnalysis.Items.Add("The information below is also available in the StoreDeleteActivity.log");
					LogAnalysis();
					CheckAnalysisDates();
					if (AreConstraintesDropped())
					{
						btProcessSD.Enabled = true;
					}
					else
					{
						btProcessSD.Enabled = false;
						string msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteDropConstraints);
                        //lbAnalysis.Items.Add(" *ERROR * " + msg);
                        lbAnalysis.Items.Add(" *INFO * " + msg);
					}
				}
				else
				{
					AddMessage("There were problems with the analysis run. Please check the StoreDeleteActivity.log");
				}
			}
            //END TT#4636-VStuart-Incorrectly checking if services are down-MID
            else
			{
				//msgDelete.Text = "Analysis did not start due to issues found.";
				AddMessage("Analysis did not start due to issues found.");
			}
			Cursor.Current = Cursors.Default;
		}

		private bool ReadyForStoreDelete(bool isStoreDelete)
		{
			string msg = string.Empty;
			bool ready = true;
			if (!AreServicesStopped() && isStoreDelete)
			{
				//lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteServicesMustBeDown);
				lbAnalysis.Items.Add(" *ERROR * " + msg);
				//lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				ready = false;
			}
			else if (IsIsStoreDeleteInProgress())
			{
				//lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteInProgress);
				lbAnalysis.Items.Add(" *ERROR * " + msg);
				//lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				ready = false;
				CheckResetButton();
			}
			else if (!AreStoresMarkedForDeletion())
			{
				//lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				msg = MIDText.GetTextOnly(eMIDTextCode.msg_NoStoresMarkedForDeletion);
				lbAnalysis.Items.Add(" *ERROR * " + msg);
				//lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				ready = false;
			}
			else if (!AreConstraintesDropped() && isStoreDelete)
			{
				//lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteDropConstraints);
				lbAnalysis.Items.Add(" *ERROR * " + msg);
				//lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				ready = false;
			}

			return ready;
		}

        //BEGIN TT#4636-VStuart-Incorrectly checking if services are down-MID
        private bool ReadyForProcessAnalysis()
        {
            string msg = string.Empty;
            bool ready = true;
            if (IsIsStoreDeleteInProgress())
            {
                //lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteInProgress);
                lbAnalysis.Items.Add(" *ERROR * " + msg);
                //lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                ready = false;
                CheckResetButton();
            }
            else if (!AreStoresMarkedForDeletion())
            {
                //lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                msg = MIDText.GetTextOnly(eMIDTextCode.msg_NoStoresMarkedForDeletion);
                lbAnalysis.Items.Add(" *ERROR * " + msg);
                //lbAnalysis.Items.Add("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                ready = false;
            }
            return ready;
        }
        //END TT#4636-VStuart-Incorrectly checking if services are down-MID

		private bool AreStoresMarkedForDeletion()
		{
			bool marked = true;
			StoreData sd = new StoreData(_connectionString);
			DataTable storeTable = sd.StoreProfile_ReadForStoreDelete();
			if (storeTable.Rows.Count == 0)
			{
				marked = false;
			}
			return marked;
		}

		private bool IsIsStoreDeleteInProgress()
		{
			GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
			gop.LoadOptions();
			return gop.IsStoreDeleteInProgress;
		}

		private bool AreConstraintesDropped()
		{
			GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
			gop.LoadOptions();
			return gop.ConstraintsDropped;
		}

		private void ResetStoreDeleteInProgress()
		{
			GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
			gop.LoadOptions();
			GlobalOptions go = new GlobalOptions();
			//============================================================
			//Update Global Options Store Delete In progress to false;
			//============================================================
			go.OpenUpdateConnection();
			go.UpdateStoreDeleteInProgress(gop.Key, false);
			go.CommitData();
			go.CloseUpdateConnection();
		}

        //BEGIN TT#4636-VStuart-Incorrectly checking if services are down-MID
        private bool AreServicesStopped() 
        {
			try
			{
                bool stopped = false;
                //BEGIN TT#4636-VStuart-Incorrectly checking if services are down-MID
                string appControlServer = MIDConfigurationManager.AppSettings["ControlServiceNames"];
                string appStoreServer = MIDConfigurationManager.AppSettings["StoreServiceNames"];
                string appHierarchyServer = MIDConfigurationManager.AppSettings["MerchandiseServiceNames"];
                string appSchedulerServer = MIDConfigurationManager.AppSettings["SchedulerServiceNames"];
                //string appApplicationServer = MIDConfigurationManager.AppSettings["ApplicationServiceNames"];

                bool controlServerStopped = IsServiceStopped(appControlServer);
                bool storeServerStopped = IsServiceStopped(appStoreServer);
                bool hierarchyServerStopped = IsServiceStopped(appHierarchyServer);
                bool schedulerServerStopped = IsServiceStopped(appSchedulerServer);
                //bool applicationServerStopped = IsServiceStopped(appApplicationServer);

                //return (controlServerStopped && storeServerStopped && hierarchyServerStopped && schedulerServerStopped && applicationServerStopped);
                //return (controlServerStopped && storeServerStopped && hierarchyServerStopped && schedulerServerStopped);
                if (controlServerStopped && storeServerStopped && hierarchyServerStopped && schedulerServerStopped)
                { 
                    stopped = true; 
                }
                return stopped;
                //END TT#4636-VStuart-Incorrectly checking if services are down-MID
            }
			catch (Exception exc)
			{
				HandleException("", exc);
				return false;
			}
		}
        //END TT#4636-VStuart-Incorrectly checking if services are down-MID

		private bool IsServiceStopped(string serviceName)
		{
            //BEGIN TT#4636-VStuart-Incorrectly checking if services are down-MID
            bool isStopped = false;
            // return is stopped if no name for service
            if (serviceName == null || serviceName.Trim().Length == 0)
            {
                isStopped = true;
            }
			System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController(serviceName);

			try
			{
				if (sc.Status == ServiceControllerStatus.Stopped)
				{
					isStopped = true;
				}
			}
            catch (System.InvalidOperationException ex)
            {
                HandleException("", ex);
                isStopped = false;
            }
            catch (Exception ex)
            {
                HandleException("", ex);
            }

            //catch (System.InvalidOperationException ex)
            //{
            //    // Exception thrown when service doesn't exist...which is OK.
            //    isStopped = true;
            //}
            //catch (Exception exc)
            //{
            //    HandleException("", exc);
            //}
            //END TT#4636-VStuart-Incorrectly checking if services are down-MID
			return isStopped;
		}

		private void btSave_Click(object sender, EventArgs e)
		{
			SaveConfiguration();
			TruncateAnalysisResults();
			EnableAnalysisButton(true);
			EnableSaveButton(false);
		}

		private void btReset_Click(object sender, EventArgs e)
		{
			ResetStoreDeleteInProgress();
			EnableAnalysisButton(true);
			EnableProcessButton(true);
			EnableSaveButton(true);
			btReset.Visible = false;
			AddMessage(MIDText.GetTextOnly(eMIDTextCode.msg_InProgressReset));
		}

		private void tbConProcessCopy_TextChanged(object sender, EventArgs e)
		{
			if (_formLoaded)
			{
				EnableButtons(false);
				EnableSaveButton(true);
			}
		}

		private void EnableButtons(bool enable)
		{
			_changePending = enable;
			EnableAnalysisButton(enable);
			EnableProcessButton(enable);
		}

		private void EnableAnalysisButton(bool enable)
		{
			btProcessA.Enabled = enable;
		}

		private void EnableProcessButton(bool enable)
		{
			btProcessSD.Enabled = enable;
		}

		private void EnableSaveButton(bool enable)
		{
			btSave.Enabled = enable;
			lbSave.Enabled = enable;
		}

		private void TruncateAnalysisResults()
		{
			StoreData sd = new StoreData(_connectionString);
			try
			{
				if (!sd.ConnectionIsOpen)
				{
					sd.OpenUpdateConnection();
				}
				sd.TruncateTable("STORE_REMOVAL_ANALYSIS");
			}
			catch (Exception exc)
			{
				HandleException("", exc);
			}
			finally
			{
				if (sd.ConnectionIsOpen)
				{
					sd.CommitData();
					sd.CloseUpdateConnection();
				}
			}
		}

		private void tbConProcessDelete_TextChanged(object sender, EventArgs e)
		{
			if (_formLoaded)
			{
				EnableButtons(false);
				EnableSaveButton(true);
			}
		}

		private void tbBatchSize_TextChanged(object sender, EventArgs e)
		{
			if (_formLoaded)
			{
				EnableButtons(false);
				EnableSaveButton(true);
			}
		}

		private void tbMinRowCount_TextChanged(object sender, EventArgs e)
		{
			if (_formLoaded)
			{
				EnableButtons(false);
				EnableSaveButton(true);
			}
		}

		private void tbMaxRowCount_TextChanged(object sender, EventArgs e)
		{
			if (_formLoaded)
			{
				EnableButtons(false);
				EnableSaveButton(true);
			}
		}

		private void tbRowPct_TextChanged(object sender, EventArgs e)
		{
			if (_formLoaded)
			{
				EnableButtons(false);
				EnableSaveButton(true);
			}
		}

		private bool CheckAnalysisDates()
		{
			bool errorFound = false;
			//=====================
			// get ANALYSIS table
			//=====================
			StoreData sd = new StoreData(_connectionString);
			DataTable analysisTable = sd.StoreProfile_ReadRemovalAnalysis();
			DateTime sevenDaysAgo = DateTime.Now.AddDays(-7);
			if (analysisTable.Rows.Count == 0)
			{
				msgAnalysis.Text = "To date, no Analysis has been run.";
				EnableProcessButton(false);
				errorFound = true;
				return errorFound;
			}
			foreach (DataRow row in analysisTable.Rows)
			{
				if (row["ANALYSIS_DATE"] != DBNull.Value)
				{
					DateTime AnalysisDate = Convert.ToDateTime(row["ANALYSIS_DATE"]);
					msgAnalysis.Text = "Last Analysis Date: " + AnalysisDate;

					if (AnalysisDate < sevenDaysAgo)
					{
						string msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteAnalysisOutdated);
						lbAnalysis.Items.Add(msg);
						EnableProcessButton(false);
						errorFound = true;
						break;
					}
				}
			}
			return errorFound;
		}

		private void CheckResetButton()
		{
			bool show = false;
			int completeCnt = 0;
			int uncompleteCnt = 0;
			if (IsIsStoreDeleteInProgress())
			{
				//=====================
				// get ANALYSIS table
				//=====================
				StoreData sd = new StoreData(_connectionString);
				DataTable analysisTable = sd.StoreProfile_ReadRemovalAnalysis();
				foreach (DataRow row in analysisTable.Rows)
				{
					if (row["COMPLETED"] != DBNull.Value)
					{
						bool completed = Include.ConvertStringToBool(row["COMPLETED"].ToString());
						if (completed)
						{
							completeCnt++;
						}
						else
						{
							uncompleteCnt++;
						}

						if (completeCnt > 0 && uncompleteCnt > 0)
						{
							show = true;
							break;
						}
					}
				}
			}

			if (show)
			{
				btReset.Visible = true;
				EnableAnalysisButton(false);
				EnableProcessButton(false);
				EnableSaveButton(false);
				string msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteInProgress);
				lbAnalysis.Items.Add(" *ERROR * " + msg);
			}
			else
			{
				btReset.Visible = false;
				ResetStoreDeleteInProgress();
			}
		}

		private void LogAnalysis()
		{
			List<string> deleteList = new List<string>();
			List<string> copyList = new List<string>();
			List<string> bypassList = new List<string>();


			string results = string.Empty;
			lbAnalysis.Items.Add("===============================");
			lbAnalysis.Items.Add(" ANALYSIS RESULTS");
			lbAnalysis.Items.Add("===============================");

			//=====================
			// get ANALYSIS table
			//=====================
			StoreData sd = new StoreData(_connectionString);
			DataTable analysisTable = sd.StoreProfile_ReadRemovalAnalysis();
			int maxTableNameLength = analysisTable.AsEnumerable().Select(row => row["TABLE_NAME"]).OfType<string>().Max(val => val.Length);
			maxTableNameLength = maxTableNameLength + 9;

			foreach (DataRow row in analysisTable.Rows)
			{
				string tableString = string.Empty;
				string tableName = row["TABLE_NAME"].ToString();
				int rowsToDelCount = int.Parse(row["ROWS_TO_DELETE_COUNT"].ToString());
				if (Include.ConvertStringToBool(row["STORE_SET_IND"].ToString()))
				{
					tableName = tableName + " (sets)";
				}

				if (rowsToDelCount == 0)
				{
					results = "Bypassed  ";
					bypassList.Add(tableName + "," + rowsToDelCount);
				}
				else
				{
					results = "Copy/Load ";
					if (row["DO_DELETE_PROCESS"] != DBNull.Value && Include.ConvertStringToBool(row["DO_DELETE_PROCESS"].ToString()))
					{
						results = "Delete    ";
					}
					if (results == "Copy/Load ")
					{
						copyList.Add(tableName + "," + rowsToDelCount);
					}
					else
					{
						deleteList.Add(tableName + "," + rowsToDelCount);
					}
				}
			}

			foreach (string line in copyList)
			{
				string[] items = MIDstringTools.Split(line, ',', true);
				string tableName = items[0];
				string rows = items[1];

				String rec = new String(' ', 150);
				rec = rec.Insert(0, tableName);
				rec = rec.Insert(maxTableNameLength, "Results: Copy/Load Number of Rows: " + rows);
				lbAnalysis.Items.Add(rec);

			}
			lbAnalysis.Items.Add("");
			foreach (string line in deleteList)
			{
				string[] items = MIDstringTools.Split(line, ',', true);
				string tableName = items[0];
				string rows = items[1];

				String rec = new String(' ', 150);
				rec = rec.Insert(0, tableName);
				rec = rec.Insert(maxTableNameLength, "Results: Delete    Number of Rows: " + rows);
				lbAnalysis.Items.Add(rec);
			}
			lbAnalysis.Items.Add("");
			foreach (string line in bypassList)
			{
				string[] items = MIDstringTools.Split(line, ',', true);
				string tableName = items[0];
				string rows = items[1];

				String rec = new String(' ', 150);
				rec = rec.Insert(0, tableName);
				rec = rec.Insert(maxTableNameLength, "Results: Bypass    Number of Rows: " + rows);
				lbAnalysis.Items.Add(rec);
			}
		}

	

	}
}
