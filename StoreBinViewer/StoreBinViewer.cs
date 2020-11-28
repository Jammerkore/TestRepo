using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.IO;



using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;


namespace StoreBinViewer
{
	public partial class StoreBinViewer : Form
	{
		string sourceModule = "StoreBinViewer.cs";
		string eventLogID = "StoreBinViewer";
		SessionAddressBlock _SAB;
		SessionSponsor _sponsor;
		IMessageCallback _messageCallback;
		ScheduleData _scheduleData;
		eMIDMessageLevel highestMessage;
		string tasklistName;
		string message = null;
		bool errorFound = false;
		System.Runtime.Remoting.Channels.IChannel channel;
		int _processId = Include.NoRID;
		string _accumAllDays = string.Empty;
		int _nodeRid = Include.NoRID;
		StoreVariableHistoryBin _dlStoreVarHist = null;
		Hashtable _storeIdHash; // Contains store IDs for logging. key is store IDX.
		Hashtable _storeRidHash; // Contains store RIDs for logging. key is store IDX.
		Hashtable _sizeCodeHash; // Contains Size Code IDs for logging.
		string _styleId = string.Empty;
		string _colorCodeId = string.Empty;
		string _ConfigVariable;

		private string SALES_VAR_NAME = string.Empty;
		private string SALES_REG_VAR_NAME = string.Empty;
		private string SALES_PROMO_VAR_NAME = string.Empty;
		private string SALES_MKDN_VAR_NAME = string.Empty;
		private string STOCK_VAR_NAME = string.Empty;
		private string STOCK_REG_VAR_NAME = string.Empty;
		private string STOCK_MKDN_VAR_NAME = string.Empty;
		private string IN_STOCK_SALES_VAR_NAME = string.Empty;
		private string IN_STOCK_SALES_REG_VAR_NAME = string.Empty;
		private string IN_STOCK_SALES_PROMO_VAR_NAME = string.Empty;
		private string IN_STOCK_SALES_MKDN_VAR_NAME = string.Empty;
		private string ACCUM_SELL_THRU_SALES_VAR_NAME = string.Empty;
		private string ACCUM_SELL_THRU_STOCK_VAR_NAME = string.Empty;
		private string DAYS_IN_STOCK_VAR_NAME = string.Empty;
		private string RECEIVED_STOCK_VAR_NAME = string.Empty;

		string _sDate = string.Empty;
		string _sNode = string.Empty;
		DataTable dtGrid = null;
		bool _loaded;
		DataView _dvGrid;
		bool _columnsLoaded = false;

		int _offset = 11;

		public StoreBinViewer()
		{
			InitializeComponent();
			StartServices();

		}

		private void StoreBinViewer_Load(object sender, EventArgs e)
		{

			try
			{
				txtDate.Text = MIDConfigurationManager.AppSettings["DisplayDate"];
				txtNode.Text = MIDConfigurationManager.AppSettings["DisplayNode"];
				txtColorFilter.Text = MIDConfigurationManager.AppSettings["ColorFilter"];
				txtSizeFilter.Text = MIDConfigurationManager.AppSettings["SizeFilter"];
				txtStoreFilter.Text = MIDConfigurationManager.AppSettings["StoreFilter"];
				_ConfigVariable = MIDConfigurationManager.AppSettings["VariableFilter"];
				DataTable dtVar = MIDText.GetLabels((int)eForecastBaseDatabaseStoreVariables.SalesTotal, (int)eForecastBaseDatabaseStoreVariables.ReceivedStock);
				cboVariable.Items.Add(string.Empty);
				foreach (DataRow aRow in dtVar.Rows)
				{
					string textValue = aRow["TEXT_VALUE"].ToString();
					cboVariable.Items.Add(textValue);
				}
				if (_ConfigVariable != string.Empty)
				{
					cboVariable.SelectedItem = _ConfigVariable;
				}
				_loaded = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			finally
			{

			}
		}

		public int StartServices()
		{
			try
			{
				_messageCallback = new BatchMessageCallback();
				_sponsor = new SessionSponsor();
				_SAB = new SessionAddressBlock(_messageCallback, _sponsor);

				if (!EventLog.SourceExists(eventLogID))
				{
					EventLog.CreateEventSource(eventLogID, null);
				}

				// Register callback channel

				try
				{
					channel = _SAB.OpenCallbackChannel();
				}
				catch (Exception exception)
				{
					errorFound = true;
					EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + exception.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				// Create Sessions

				try
				{
					_SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Hierarchy | (int)eServerType.Store | (int)eServerType.Application);
				}
				catch (Exception exception)
				{
					errorFound = true;
					Exception innerE = exception;
					while (innerE.InnerException != null)
					{
						innerE = innerE.InnerException;
					}
					EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				_scheduleData = new ScheduleData();
				tasklistName = string.Empty;

				eSecurityAuthenticate authentication = _SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
				MIDConfigurationManager.AppSettings["Password"], eProcesses.StoreBinViewer);
				if (authentication != eSecurityAuthenticate.UserAuthenticated)
				{
					errorFound = true;
					EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
					System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
					return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
				}

				if (_processId != Include.NoRID)
				{
					_SAB.ClientServerSession.Initialize(_processId);
				}
				else
				{
					_SAB.ClientServerSession.Initialize();
				}

				_SAB.HierarchyServerSession.Initialize();
				_SAB.ApplicationServerSession.Initialize();
				_SAB.StoreServerSession.Initialize();
			}

			catch (Exception exception)
			{
				errorFound = true;
				message = "";
				while (exception != null)
				{
					message += " -- " + exception.Message;
					exception = exception.InnerException;
				}
				_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
				_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
			}
			finally
			{
                //if (!errorFound)
                //{
                //    if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
                //    {
                //        _SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
                //    }
                //}
                //else
                //{
                //    if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
                //    {
                //        _SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                //    }
                //}

                //highestMessage = _SAB.CloseSessions();

			}

			return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
		}

		public DataTable LoadData()
		{
			gridData.DataSource = null;
			_dvGrid = null;

			MIDTimer timer = new MIDTimer();
			timer.Start();

			string sDate = txtDate.Text;
			string sNodeId = txtNode.Text;
			int drpRid = Include.NoRID;

			int styleRid = -1;
			int colorCodeRid = -1;
			int sizeCodeRid = -1;

			string style = string.Empty;
			string colorCode = string.Empty;
			string sizeCode = string.Empty;
			string varName = string.Empty;

			int timeId = -1;

			MIDTimer readTimer = new MIDTimer();
			ArrayList varKeyList;
			ArrayList timeKeyList;
			MerchandiseHierarchyData nodeData = new MerchandiseHierarchyData();
			HierarchyLevelProfile styleLevelProf = null;
			HierarchyLevelProfile colorLevelProf = null;
			_dlStoreVarHist = new StoreVariableHistoryBin(false, 0);

			SALES_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.SalesTotal);
			SALES_REG_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.SalesRegular);
			SALES_PROMO_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.SalesPromo);
			SALES_MKDN_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.SalesMarkdown);
			STOCK_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.StockTotal);
			STOCK_REG_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.StockRegular);
			STOCK_MKDN_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.StockMarkdown);

			IN_STOCK_SALES_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSales);
			IN_STOCK_SALES_REG_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSalesReg);
			IN_STOCK_SALES_PROMO_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSalesPromo);
			IN_STOCK_SALES_MKDN_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSalesMkdn);
			ACCUM_SELL_THRU_SALES_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.AccumSellThruSales);
			ACCUM_SELL_THRU_STOCK_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.AccumSellThruStock);
			DAYS_IN_STOCK_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.DaysInStock);
			RECEIVED_STOCK_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.ReceivedStock);

			try
			{
				readTimer.Start();

				//=======================================================================================
				// Get Main Hierarchy information and specifically info for the Style and COlor levels.
				//=======================================================================================
				HierarchyProfile mainHier = _SAB.HierarchyServerSession.GetMainHierarchyData();
				for (int levelIndex = 1; levelIndex <= mainHier.HierarchyLevels.Count; levelIndex++)
				{
					HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHier.HierarchyLevels[levelIndex];
					//hlp.LevelID is level name 
					//hlp.Level is level number 
					//hlp.LevelType is level type 
					if (hlp.LevelType == eHierarchyLevelType.Style)
					{
						styleLevelProf = hlp;
					}
					if (hlp.LevelType == eHierarchyLevelType.Color)
					{
						colorLevelProf = hlp;
					}
				}

				//=========
				// Stores
				//=========
				_storeIdHash = new Hashtable();
				_storeRidHash = new Hashtable();
				ProfileList storeList = _SAB.StoreServerSession.GetAllStoresList();
				int[] storeRIDs = new int[MIDStorageTypeInfo.GetStoreMaxRID(0)];
				for (int i = 0; i < storeRIDs.Length; i++)
				{
					StoreProfile sp = (StoreProfile)storeList.FindKey(i + 1);
					// Begin TT#646 - stodd - inactive stores causing viewer to abend.
					// This catches inactive stores that have not been retured from the store service.
					if (sp != null)
					{
						_storeIdHash.Add(i, sp.StoreId);
					}
					else
					{
						_storeIdHash.Add(i, "Inactive");
					}
					// End TT#646 - stodd - inactive stores causing viewer to abend.
					_storeRidHash.Add(i, i + 1);
					storeRIDs[i] = i + 1;
				}

				dtGrid = BuildGridDataTable(storeRIDs, _storeRidHash, _storeIdHash);

				//==========================
				// gather up weeklist from 
				//==========================
				DateRangeProfile overrideDateRange = new DateRangeProfile(Include.NoRID);
				if (sDate.Length == 13)
					{
						try
						{
							string[] sep = new string[] { "-" };
							string[] fromTo = sDate.Split(sep, StringSplitOptions.None);
							int from = int.Parse(fromTo[0]);
							int to = int.Parse(fromTo[1]);
							//cdrRid = _SAB.ApplicationServerSession.Calendar.AddDateRange(from, to, eCalendarRangeType.Static, eCalendarDateType.Week, eDateRangeRelativeTo.None, "", false, 0);
							overrideDateRange.StartDateKey = from;
							overrideDateRange.EndDateKey = to;
							overrideDateRange.DateRangeType = eCalendarRangeType.Static;
							overrideDateRange.SelectedDateType = eCalendarDateType.Week;
							overrideDateRange.RelativeTo = eDateRangeRelativeTo.None;
						}
						catch
						{
							throw;
						}
				}
				//================================================
				// Date format "YYYYWW" for a static week
				//================================================
				if (sDate.Length == 6)
				{
					try
					{
						int fromTo = int.Parse(sDate);
						overrideDateRange.StartDateKey = fromTo;
						overrideDateRange.EndDateKey = fromTo;
						overrideDateRange.DateRangeType = eCalendarRangeType.Static;
						overrideDateRange.SelectedDateType = eCalendarDateType.Week;
						overrideDateRange.RelativeTo = eDateRangeRelativeTo.None;

					}
					catch
					{
						throw;
					}

				}
				drpRid = _SAB.ApplicationServerSession.Calendar.AddDateRange(overrideDateRange);

				DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(drpRid);
				ProfileList weekList = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(drp, null);
				
				//==============
				// Get Styles
				//==============
				DataTable dtAllStyles = null;
				if (txtNode.Text.Trim() == string.Empty)
				{
					dtAllStyles = nodeData.GetAllStyles();
				}
				else
				{
					_nodeRid = _SAB.HierarchyServerSession.GetNodeRID(txtNode.Text);
					HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(_nodeRid);
					dtAllStyles = BuildAllStylesTable();

					if (hnp.LevelType == eHierarchyLevelType.Style)
					{
						DataRow aRow = dtAllStyles.NewRow();
						aRow[0] = _nodeRid;
						dtAllStyles.Rows.Add(aRow);

					}
					else
					{
						NodeDescendantList nodeDescList = _SAB.HierarchyServerSession.GetNodeDescendantList(_nodeRid, eHierarchyLevelType.Style, eNodeSelectType.All);
						foreach (NodeDescendantProfile ndp in nodeDescList.ArrayList)
						{
							object[] objs = new object[] { ndp.Key.ToString() };

							dtAllStyles.LoadDataRow(objs, false);
						}
					}
				}
				dtAllStyles.AcceptChanges();
				
				int styleCnt = 0, colorCnt = 0, sizeCnt = 0, totReads = 0, valCnt = 0;
				//===========
				// by STYLE
				//===========
				foreach (DataRow row in dtAllStyles.Rows)
				{
					styleCnt++;
					styleRid = int.Parse(row["HN_RID"].ToString());
					style = _SAB.HierarchyServerSession.GetNodeID(styleRid);

					HierarchyNodeList colorNodeList = _SAB.HierarchyServerSession.GetHierarchyChildren(styleLevelProf.Level, mainHier.Key, mainHier.Key, styleRid, false, eNodeSelectType.NoVirtual);
					if (colorNodeList.Count > 0)
						colorCnt++;
					//===========
					// by COLOR
					//===========
					//dlStoreVarHist.FlushAll();
					foreach (HierarchyNodeProfile colorNode in colorNodeList.ArrayList)
					{
						_sizeCodeHash = new Hashtable();
						colorCodeRid = colorNode.ColorOrSizeCodeRID;
						colorCode = colorNode.NodeID;

						//===================
						// TIME (each week)
						//===================
						foreach (WeekProfile weekProf in weekList)
						{
							timeKeyList = new ArrayList();
							//==========================
							// Build day list for week
							//==========================
							if (rbDay.Checked)
							{
								foreach (DayProfile dayProf in weekProf.Days.ArrayList)
								{
									timeKeyList.Add(new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, dayProf.Key));
								}
							}
							//==========================
							// Build week list for weeks
							//==========================
							else
							{
								timeKeyList.Add(new SQL_TimeID(eSQLTimeIdType.TimeIdIsWeekly, weekProf.Key));
							}

							HierarchyNodeList sizeNodeList = _SAB.HierarchyServerSession.GetHierarchyChildren(colorLevelProf.Level, mainHier.Key, mainHier.Key, colorNode.Key, false, eNodeSelectType.NoVirtual);
							if (sizeNodeList.Count > 0)
								sizeCnt++;
							Hashtable storeHash = new Hashtable();

							_sizeCodeHash.Clear();
							foreach (HierarchyNodeProfile sizeNode in sizeNodeList.ArrayList)
							{
								SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeNode.ColorOrSizeCodeRID);
								_sizeCodeHash.Add(scp.Key, scp.SizeCodeID + " (" + scp.SizeCodeName + " )");
							}

							//===========
							// by SIZES
							//===========

							foreach (HierarchyNodeProfile sizeNode in sizeNodeList.ArrayList)
							{
								varKeyList = new ArrayList();
								DataTable dtVar = MIDText.GetLabels((int)eForecastBaseDatabaseStoreVariables.SalesTotal, (int)eForecastBaseDatabaseStoreVariables.ReceivedStock);
								foreach (DataRow aRow in dtVar.Rows)
								{
									string textValue = aRow["TEXT_VALUE"].ToString();
									varKeyList.Add(textValue);
								}							

								sizeCodeRid = sizeNode.ColorOrSizeCodeRID;
								sizeCode = sizeNode.NodeID;
								double[] valueList = null;

								//=============
								// by VARIABLE
								//=============
								for (int v = 0; v < varKeyList.Count; v++)
								{
									varName = varKeyList[v].ToString();
									//=============
									// by TIME
									//=============
									for (int t = 0; t < timeKeyList.Count; t++)
									{
										SQL_TimeID timeID = (SQL_TimeID)timeKeyList[t];
										timeId = timeID.SqlTimeID;
										WeekProfile wp = null;
										DayProfile dp = null;
										totReads++;

										//=====================
										// get values by STORE
										//=====================
										if (rbDay.Checked)
										{
											dp = _SAB.ApplicationServerSession.Calendar.GetDay(timeID.TimeID);
											valueList = _dlStoreVarHist.GetStoreVariableDayValue(varName, styleRid, timeID, colorCodeRid, sizeCodeRid, storeRIDs);
										}
										else
										{
											wp = _SAB.ApplicationServerSession.Calendar.GetWeek(timeID.TimeID);
											valueList = _dlStoreVarHist.GetStoreVariableWeekValue(varName, styleRid, timeID, colorCodeRid, sizeCodeRid, storeRIDs);
										}

										DataRow newRow = dtGrid.NewRow();

										newRow["VARIABLE"] = varName;
										if (rbDay.Checked)
										{
											string dayOfWeek = dp.DayOfWeek.ToString().Substring(0, 3);
											string dayText = dp.Date.ToShortDateString();
											newRow["DATE"] = dayText + " " + dayOfWeek;
											newRow["SORTDATE"] = dp.YearDay;
										}
										else
										{
											newRow["DATE"] = wp.Text();
											newRow["SORTDATE"] = wp.YearWeek;
										}
										newRow["JULIAN"] = timeID.TimeID;
										newRow["TIME_ID"] = timeID.SqlTimeID;
										newRow["STYLE"] = style;
										newRow["STYLE_RID"] = styleRid;
										newRow["COLOR_CODE"] = colorCode;
										newRow["COLOR_CODE_RID"] = colorCodeRid;
										newRow["SIZE_CODE"] = _sizeCodeHash[sizeCodeRid];
										newRow["SIZE_CODE_RID"] = sizeCodeRid;

										//=============================================================================================
										// WARNING: IF YOU ADD COLUMNS ABOVE, ADJUST THE _offset NUMBER ABOVE IN THE GLOBAL VARIABLES
										//=============================================================================================
										int	storeCount = storeRIDs.Length;
										bool nonZeros = false;
										for (int i = 0; i < storeCount; i++)
										{
											if (cbZeroFilter.Checked)
											{
												double value = (double)valueList[i];
												if (value != 0)
													nonZeros = true;
											}

											newRow[i + _offset] = valueList[i];
										}

										if (cbZeroFilter.Checked)
										{
											if (nonZeros)
												dtGrid.Rows.Add(newRow);
										}
										else
										{
											dtGrid.Rows.Add(newRow);
										}
									}
								}
							}
						}
					}
				}
				dtGrid.AcceptChanges();
			}
			catch (Exception ex)
			{
				_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ToString(), this.ToString());
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch
			{
				throw;
			}
			timer.Stop("Load Data");
			Debug.WriteLine("Total Rows: " + dtGrid.Rows.Count);
			return dtGrid;
		}

		private void btFill_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				gridData.AutoGenerateColumns = false;
				DataTable dt = LoadData();

				_dvGrid = new DataView(dt);
				_dvGrid.Sort = "SORTDATE asc, STYLE asc, COLOR_CODE asc, VARIABLE asc , SIZE_CODE asc";

				BindDataToGrid(_dvGrid);

				UnfilterStoreColumns();

				gridData.Columns["SORTDATE"].Visible = false;
				if (cbHideRids.Checked)
				{
					gridData.Columns["JULIAN"].Visible = false;
					gridData.Columns["TIME_ID"].Visible = false;
					gridData.Columns["STYLE_RID"].Visible = false;
					gridData.Columns["COLOR_CODE_RID"].Visible = false;
					gridData.Columns["SIZE_CODE_RID"].Visible = false;
				}
				else
				{
					gridData.Columns["JULIAN"].Visible = true;
					gridData.Columns["TIME_ID"].Visible = true;
					gridData.Columns["STYLE_RID"].Visible = true;
					gridData.Columns["COLOR_CODE_RID"].Visible = true;
					gridData.Columns["SIZE_CODE_RID"].Visible = true;
				}
			}
			catch (Exception ex)
			{
				_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ToString(), this.ToString());
				MessageBox.Show(ex.ToString(), "Error Loading Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
				_loaded = true;
			}
		}
		private void BindDataToGrid(DataView dvGrid)
		{
			MIDTimer timer = new MIDTimer();
			timer.Start();
			gridData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			gridData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			gridData.EnableHeadersVisualStyles = false;

			gridData.DataSource = null;
			gridData.DataSource = dvGrid;
			if (!_columnsLoaded)
			{
				foreach (DataColumn dc in dvGrid.Table.Columns)
				{
					string name = dc.ColumnName;
					DataGridViewColumn gridCol = new DataGridViewColumn();
					gridCol.DataPropertyName = name;
					gridCol.Name = name;
					gridCol.FillWeight = 10;
					DataGridViewCell cell = new DataGridViewTextBoxCell();
					gridCol.CellTemplate = cell;
					gridData.Columns.Add(gridCol);
				}
			}
			_columnsLoaded = true;
			timer.Stop("Bind to Grid");
		}

		private DataTable BuildAllStylesTable()
		{
			DataTable dtAllStyles = MIDEnvironment.CreateDataTable();

			DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "HN_RID";
			dtAllStyles.Columns.Add(dataColumn);

			return dtAllStyles;
		}

		private DataTable BuildGridDataTable(int[] storeRIDs, Hashtable storeRidHash, Hashtable storeIdHash)
		{
			DataTable dtStoreSizeWeek = MIDEnvironment.CreateDataTable();

			DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SORTDATE";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "DATE";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "JULIAN";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "TIME_ID";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "STYLE";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "STYLE_RID";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "COLOR_CODE";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "COLOR_CODE_RID";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SIZE_CODE";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "SIZE_CODE_RID";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "VARIABLE";
			dtStoreSizeWeek.Columns.Add(dataColumn);

			int	storeCount = storeRIDs.Length;
			for (int i = 0; i < storeCount; i++)
			{
				string textValue = storeIdHash[i].ToString() + " (" + storeRidHash[i] + ")";
				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = textValue;
				dataColumn.DefaultValue = 0;
				dtStoreSizeWeek.Columns.Add(dataColumn);
			}

			//DataColumn[] PrimaryKeyColumn;
			//PrimaryKeyColumn = new DataColumn[2];
			//PrimaryKeyColumn[0] = dtStoreSizeWeek.Columns["STORE_IDX"];
			//PrimaryKeyColumn[1] = dtStoreSizeWeek.Columns["SIZE_CODE_RID"];
			//dtStoreSizeWeek.PrimaryKey = PrimaryKeyColumn;

			return dtStoreSizeWeek;
		}

		private void txtDate_TextChanged(object sender, EventArgs e)
		{
			_sDate = txtDate.Text;
		}

		private void txtNode_TextChanged(object sender, EventArgs e)
		{
			_sNode = txtNode.Text;

		}

		private void cbHideRids_CheckedChanged(object sender, EventArgs e)
		{
			if (cbHideRids.Checked)
			{
				gridData.Columns["JULIAN"].Visible = false;
				gridData.Columns["TIME_ID"].Visible = false;
				gridData.Columns["STYLE_RID"].Visible = false;
				gridData.Columns["COLOR_CODE_RID"].Visible = false;
				gridData.Columns["SIZE_CODE_RID"].Visible = false;
			}
			else
			{
				gridData.Columns["JULIAN"].Visible = true;
				gridData.Columns["TIME_ID"].Visible = true;
				gridData.Columns["STYLE_RID"].Visible = true;
				gridData.Columns["COLOR_CODE_RID"].Visible = true;
				gridData.Columns["SIZE_CODE_RID"].Visible = true;
			}
		}

		private void btFilter_Click(object sender, EventArgs e)
		{
			if (!_loaded)
			{
				MessageBox.Show("The data must first be loaded before you can apply a filter. Press the 'Load Data' button first.", "Filter Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			string filterText = string.Empty;
			try
			{
				Cursor.Current = Cursors.WaitCursor;

				DataView view = new DataView(dtGrid);
				BindingSource gridSource = new BindingSource();
				gridSource.DataSource = view;

				//====================
				// Color Filter Text
				//====================
				string colorFilter = txtColorFilter.Text.Trim();
				StringBuilder colorFilterText = new StringBuilder("");
				if (colorFilter != string.Empty)
				{
					//The Filter string can include Boolean expressions.
					colorFilterText.Append("COLOR_CODE = '" + colorFilter + "'");
				}

				//==================
				// Size Filter Text
				//==================
				string sizeFilter = txtSizeFilter.Text.Trim();
				StringBuilder sizeFilterText = new StringBuilder("");
				if (sizeFilter != string.Empty)
				{
					//The Filter string can include Boolean expressions.
					sizeFilterText.Append("SIZE_CODE = '" + sizeFilter + "'");
				}

				//======================
				// Variable Filter Text
				//======================
				string variableFilter = string.Empty;
				StringBuilder variableFilterText = new StringBuilder("");
				if (cboVariable.SelectedItem != null)
				{
					variableFilter = cboVariable.SelectedItem.ToString();
					if (variableFilter != string.Empty)
					{
						variableFilterText.Append("VARIABLE = '" + variableFilter + "'");
					}
				}

				//=================================
				// Apply all Grid Source filters
				//=================================
				if (colorFilterText.ToString() != string.Empty)
				{
					filterText = colorFilterText.ToString();
				}

				if (sizeFilterText.ToString() != string.Empty)
				{
					if (filterText == string.Empty)
					{
						filterText = sizeFilterText.ToString();
					}
					else
					{
						filterText += " and ";
						filterText += sizeFilterText.ToString();
					}
				}

				if (variableFilterText.ToString() != string.Empty)
				{
					if (filterText == string.Empty)
					{
						filterText = variableFilterText.ToString();
					}
					else
					{
						filterText += " and ";
						filterText += variableFilterText.ToString();
					}
				}

				//====================
				// Apply Row FILTER
				//====================
				gridSource.Filter = filterText;

				//====================
				// Apply Grid SORT
				//====================
				gridSource.Sort = "SORTDATE asc, STYLE asc, COLOR_CODE asc, VARIABLE asc , SIZE_CODE asc";

				//=================================
				// Apply column filters
				//=================================
				FilterStoreColumns();

				gridData.DataSource = gridSource;
			}
			catch (Exception ex)
			{
				_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ToString(), this.ToString());
				MessageBox.Show(ex.ToString(), "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void FilterStoreColumns()
		{
			string storeFilter = txtStoreFilter.Text.Trim();

			if (storeFilter != string.Empty)
			{
				string stores = txtStoreFilter.Text;
				stores = stores.Replace(" ", "");
				string[] sep = new string[] { "," };
				string[] storeList = stores.Split(sep, StringSplitOptions.None);

				for (int i = _offset; i < gridData.Columns.Count; i++)
				{
					DataGridViewColumn col = gridData.Columns[i];
					string[] space = new string[] { " " };
					string[] storeId = col.Name.Split(space, StringSplitOptions.None);
					if (storeList.Contains(storeId[0]))
					{
						col.Visible = true;
					}
					else
					{
						col.Visible = false;
					}
				}
			}
			else
			{
				for (int i = _offset - 1; i < gridData.Columns.Count; i++)
				{
					DataGridViewColumn col = gridData.Columns[i];
					col.Visible = true;
				}
			}
		}

		private void UnfilterStoreColumns()
		{
			for (int i = _offset - 1; i < gridData.Columns.Count; i++)
			{
				DataGridViewColumn col = gridData.Columns[i];
				col.Visible = true;
			}
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CopyClipboard();
		}

		private void CopyClipboard()
		{
			DataObject d = gridData.GetClipboardContent();
			Clipboard.SetDataObject(d);
		}

		private void gridData_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.Control && e.KeyCode == Keys.Delete) || (e.Shift && e.KeyCode == Keys.Delete))
			{
				CopyClipboard();
			}
		}

		private void btExport_Click(object sender, EventArgs e)
		{
			try
			{
				Stream myStream;
				Cursor.Current = Cursors.WaitCursor;

				saveFileDialog = new SaveFileDialog();

				saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
				saveFileDialog.FilterIndex = 2;
				saveFileDialog.RestoreDirectory = true;
				string dayWeek = "Weekly";
				if (rbDay.Checked)
					dayWeek = "Daily";
				string fileName = txtNode.Text + "_" + txtDate.Text + "_" + dayWeek + ".txt";

				if (fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
				{
					char[] chars = System.IO.Path.GetInvalidFileNameChars();
					foreach (char ch in chars)
					{
						fileName = fileName.Replace(ch, '_');
					}
				}

				saveFileDialog.FileName = fileName;

				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					if ((myStream = saveFileDialog.OpenFile()) != null)
					{
						StreamWriter sw = new StreamWriter(myStream);
						int iColCount = gridData.Columns.Count;
						for (int i = 0; i < iColCount; i++)
						{
							sw.Write(gridData.Columns[i].HeaderText);
							if (i < iColCount - 1)
							{
								sw.Write(",");
							}
						}
						sw.Write(sw.NewLine);
						// Now write all the rows.
						foreach (DataGridViewRow dr in gridData.Rows)
						{
							for (int i = 0; i < iColCount; i++)
							{
								if (dr.Cells[i].Value == null)
								{
									sw.Write("");
								}
								else
								{
									sw.Write(dr.Cells[i].Value.ToString());
								}
								if (i < iColCount - 1)
								{
									sw.Write(",");
								}
							}
							sw.Write(sw.NewLine);
						}
						sw.Close();
					}
				} 
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());

			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}

		}

        private void StoreBinViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!errorFound)
            {
                if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
                {
                    _SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
                }
            }
            else
            {
                if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
                {
                    _SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                }
            }

            highestMessage = _SAB.CloseSessions();
        }


	}
}
