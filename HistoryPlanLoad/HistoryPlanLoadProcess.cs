using System;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business;



namespace MIDRetail.HistoryPlanLoad
{
	/// <summary>
	/// Summary description for HistoryPlanLoadProcess.
	/// </summary>
	public class HistoryPlanLoadProcess
	{
		private string sourceModule = "HistoryPlanLoadProcess.cs";
		SessionAddressBlock _SAB;
		protected string _transStoreID = null;
		protected int _transStoreRID = Include.NoRID;
		protected HierarchyMaintenance _hm = null;
		protected string _transNodeID = null;
		protected int _transNodeRID = Include.NoRID;
		protected string _transDate = null;
		protected string _transVariable = null;
		protected VariableProfile _variableProfile = null;
		protected DateTime _date;
		protected int _transDateID = -1;
		protected int _transFiscalYearWeek = -1;
		protected int _transCurrentDateID = -1;
		protected int _transNextDateID = -1;
		protected int _transCurrentFiscalYearWeek = -1;
		protected int _transNextFiscalYearWeek = -1;
		protected int _intransitDateID = -1;
		protected int _postingDateID = -1;
		protected bool _rollStock;
		protected HierarchyNodeProfile _parent = new HierarchyNodeProfile(Include.NoRID);
		protected MRSCalendar _cal = new MRSCalendar();
		protected IPlanComputationTimeTotalVariables _timeTotalVariables;
		protected IPlanComputationVariables _variables;
		protected IPlanComputationQuantityVariables _quantityVariables;
		protected HierarchyProfile _hp = null;
		protected Audit _audit = null;
		protected MerchandiseHierarchyData _mhd = null;
		protected Intransit _intransitData = null;
		protected int _processRID = Include.NoRID;
		protected int _currDate = -1;
		protected int _currFiscalYearWeek = -1;
		protected int _currIntransitDate = -1;
		protected int _currVersionRID = Include.NoRID;
		protected int _currStoreRID = Include.NoRID;
		protected int _currNodeRID = Include.NoRID;
		protected string _currNodeID = null;
		protected int _currDayTimeID = 0;
		protected DayProfile _currDayProfile = null;
		protected int _numberDatabaseColumns;
		protected int _numberVariableTypes;
		protected HistoryPlanValue[] _values;
		protected object[] _locks;
		protected HistoryPlanValue[] _intransitValues;
		protected bool _foundValues = false;
		protected bool _foundIntransitValues = false;
		protected Hashtable _forecastVersions = new Hashtable();
		protected Hashtable _stores = null;
		protected Hashtable _nodes = null;
		protected Hashtable _lookupNodes = null;
		protected ArrayList _lookupAncestors = null;
		protected int _recordsRead = 0;
		protected int _recordsWithErrors = 0;
		protected int _recordsNotCommitted = 0;
		protected int _commitLimit;
		protected char _levelDelimiter;
		protected int _hitCounts = 0;
		protected int _nodesAdded = 0;
		protected bool _nodesNeedCommitted = false;
        // Begin TT#739-MD - JSmith - Delete Stores
        //// Begin TT#155 - JSmith - Size Curve Method
        //protected Dictionary<string, int> _colorCodes = null;
        //protected Dictionary<string, int> _sizeCodes = null;
        //protected bool _useOldTables = true;
        //protected bool _useNewTables = true;
        //// End TT#155
        // End TT#739-MD - JSmith - Delete Stores

		protected eVariableDataType _currProcessingData;
		protected int _hierarchyRID = Include.NoRID;
		protected int _chainDailyHistoryRecs = 0;
		protected int _chainWeeklyHistoryRecs = 0;
		protected int _chainWeeklyForecastRecs = 0;
		protected int _storeDailyHistoryRecs = 0;
		protected int _storeWeeklyHistoryRecs = 0;
		protected int _storeWeeklyForecastRecs = 0;
		protected int _intransitRecs = 0;

		// The _tableManager consists of an entry for each possible database table
		// The key to the table is the integer value of the eVariableDataType enumeration plus the 
		// table number where the data will reside.  The chain data, even though not partitioned 
		// on the database, are spread across multiple entries so that the update can be threaded
		// Each table entry contains a cube of values to consolidate "like" key values.  All cubes
		// all contain the following layout and are keyed by the database value for the field:
		//   Version Hashtable
		//      Time Hashtable
		//         Nodes Hashtable
		//            Stores Hashtable
		// All values are found in the stores hash.  Cubes that do not have a particular dimension,
		// like stores for chain data, use a default value for the dimension.
		protected Hashtable _tableManager;
		protected Hashtable _storeDailyHistoryRecords;
		protected Hashtable _storeWeeklyHistoryRecords;
		protected Hashtable _storeWeeklyForecastRecords;
		protected Hashtable _storeIntransitRecords;
		protected Hashtable _chainWeeklyHistoryRecords;
		protected Hashtable _chainWeeklyForecastRecords;
		protected Hashtable _chainDailyHistoryRecords;
		protected int _storeDailyHistoryTableNumber = -1;
		protected int _storeWeeklyHistoryTableNumber = -1;
		protected int _storeWeeklyForecastTableNumber = -1;
		protected int _storeIntransitTableNumber = -1;
		protected int _chainWeeklyHistoryTableNumber = -1;
		protected int _chainWeeklyForecastTableNumber = -1;
		protected int _chainDailyHistoryTableNumber = -1;

		// The _rollupManager ultimately consists of an entry for all data that needs rolled. 
		// The key to the table is the version RID. Each version contains a Hashtable of type of data
		// being posted stored by the eVariableDataType enumeration. The type Hashtable contains a 
		// Hashtable of time IDs containing all times being posted for the version and type.  The time
		// time ID contains a list of the nodes.
		// The table contains the following layout:
		// _rollupManager 
		//   type Hashtable
		//      Time Hashtable
		//         Nodes Arraylist
		
		protected Hashtable _rollupManager;

		protected bool _alternateAPIRolllupExists = false;
		protected bool _allowAutoAdds;
		protected bool _rollStoreDailyToWeekly;
		protected bool _rollStoreDailyUpHierarchy;
		protected bool _rollStoreWeeklyUpHierarchy;
		protected bool _rollStoreToChain;
		protected bool _rollChainUpHierarchy;
		protected bool _rollIntransit;
		protected string _storeDailyHistoryFileName = null;
		protected string _storeWeeklyHistoryFileName = null;
		protected string _storeWeeklyForecastFileName = null;
		protected string _storeIntransitFileName = null;
		protected string _chainDailyHistoryFileName = null;
		protected string _chainWeeklyHistoryFileName = null;
		protected string _chainWeeklyForecastFileName = null;
		protected StreamWriter _storeDailyHistoryWriter;
		protected StreamWriter _storeWeeklyHistoryWriter;
		protected StreamWriter _storeWeeklyForecastWriter;
		protected StreamWriter _storeIntransitWriter;
		protected StreamWriter _chainDailyHistoryWriter = null;
		protected StreamWriter _chainWeeklyHistoryWriter = null;
		protected StreamWriter _chainWeeklyForecastWriter = null;

		protected int _computationGroupRID = Include.NoRID;
		protected ComputationModel _computationModel = null;
		protected Hashtable _computationManager;
		// The _computationManager ultimately consists of an entry for all data that needs computed. 
		// The key to the table is the version RID. Each version contains a Hashtable of type of data
		// being posted stored by the eComputationType enumeration. The type Hashtable contains a 
		// Hashtable of time IDs containing all times being posted for the version and type.  The time
		// time ID contains a list of the nodes.
		// The table contains the following layout:
		// _computationManager 
		//   type Hashtable
		//      YearWeek Hashtable
		//         Nodes Arraylist

		protected enum eTransactionType
		{
			none,
			chain,
			store
		}

		public HistoryPlanLoadProcess(SessionAddressBlock SAB, int commitLimit, ref bool errorFound, char levelDelimiter,
			bool allowAutoAdds, bool rollStoreDailyToWeekly, bool rollStoreDailyUpHierarchy, bool rollStoreWeeklyUpHierarchy, 
			bool rollStoreToChain, bool rollChainUpHierarchy, bool rollIntransit, bool aRollStock, string aComputationModel)
		{
			try
			{
				_SAB = SAB;
				_commitLimit = commitLimit;
				_allowAutoAdds = allowAutoAdds;
				_rollStoreDailyToWeekly = rollStoreDailyToWeekly;
				_rollStoreDailyUpHierarchy = rollStoreDailyUpHierarchy;
				_rollStoreWeeklyUpHierarchy = rollStoreWeeklyUpHierarchy;
				_rollStoreToChain = rollStoreToChain;
				_rollChainUpHierarchy = rollChainUpHierarchy;
				_rollIntransit = rollIntransit;
				_levelDelimiter = levelDelimiter;
				_rollStock = aRollStock;
				_hm = new HierarchyMaintenance(SAB);
				_timeTotalVariables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanTimeTotalVariables;
				_variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables;
				_quantityVariables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanQuantityVariables;
				_currProcessingData = eVariableDataType.none;
				_audit = _SAB.ClientServerSession.Audit;
				_mhd = new MerchandiseHierarchyData();
				_intransitData = new Intransit();
				GetForecastVersions();
                // Begin TT#1902-MD - JSmith - Store Services - VSW API Error
                //_stores = StoreMgmt.GetStoreIDHash(); // _SAB.StoreServerSession.GetStoreIDHash();
                _stores = _SAB.StoreServerSession.GetStoreIDHash();
                // End TT#1902-MD - JSmith - Store Services - VSW API Error
                _nodes = _SAB.HierarchyServerSession.GetNodeIDHash();
                _lookupNodes = new Hashtable();
				_lookupAncestors = new ArrayList();
                // Begin TT#739-MD - JSmith - Delete Stores
                //// Begin TT#155 - JSmith - Size Curve Method
                //string useSizeStoreProcedureRead = MIDConfigurationManager.AppSettings["UseSizeStoreProcedureRead"];
                //if (useSizeStoreProcedureRead != null)
                //{
                //    useSizeStoreProcedureRead = useSizeStoreProcedureRead.ToUpper();
                //    if (useSizeStoreProcedureRead == "TRUE" ||
                //        useSizeStoreProcedureRead == "BOTH")
                //    {
                //        _useNewTables = true;
                //    }
                //    else
                //    {
                //        _useNewTables = false;
                //    }

                //    if (useSizeStoreProcedureRead == "FALSE" ||
                //        useSizeStoreProcedureRead == "BOTH")
                //    {
                //        _useOldTables = true;
                //    }
                //    else
                //    {
                //        _useOldTables = false;
                //    }
                //}

                //if (_useNewTables)
                //{
                //    _colorCodes = _SAB.HierarchyServerSession.GetColorCodeListByID();
                //    _sizeCodes = _SAB.HierarchyServerSession.GetSizeCodeListByID();
                //}
                //// End TT#155
                // End TT#739-MD - JSmith - Delete Stores
				
				_numberDatabaseColumns = _variables.GetMaximumDatabaseColumnPosition() + 1;
				_numberVariableTypes = _SAB.ClientServerSession.GlobalOptions.NumberOfStoreDataTables;
				// initialize cache area to hold values
				_tableManager = new Hashtable();
				InitializeTableManager();

				_alternateAPIRolllupExists = _SAB.HierarchyServerSession.AlternateAPIRollupExists();
				_rollupManager = new Hashtable();
				_computationManager = new Hashtable();
				LookupComputationModel(aComputationModel);
				
				// allocate containers to hold values and locks
				_values = new HistoryPlanValue[_numberDatabaseColumns];
				_locks = new object[_numberDatabaseColumns];
				_intransitValues = new HistoryPlanValue[1];
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);

				if (debugWriter != null)
				{
					debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber() + " - " + ex.ToString());
					debugWriter.Flush();
				
					string reportingModule;
					Exception innerE = ex;
					int stack = 1;
					while (innerE.InnerException != null) 
					{
						if (innerE.TargetSite == null)
						{
							reportingModule = "unknown";
						}
						else
						{
							reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
						}
						debugWriter.WriteLine("exception " + reportingModule + " " + new System.Diagnostics.StackFrame(stack,true).GetFileLineNumber() + " - " + innerE.ToString());
						debugWriter.Flush();
						++stack;
						innerE = innerE.InnerException;
					}
				}
				debugWriter.Close();
#endif
				errorFound = true;
                // Begin TT#4274 - stodd - History Load not displaying Call Stack when an error is encountered
				//_audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
                _audit.Log_Exception(ex, sourceModule);
                // End TT#4274 - stodd - History Load not displaying Call Stack when an error is encountered
				_audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
				throw;
			}
		}

        // Begin TT#916 - JSmith - Daily Size Records with style = 0
        public void InitializeNodeHash()
        {
            if (_nodesAdded > 0)
            {
                _nodes = _SAB.HierarchyServerSession.GetNodeIDHash();
            }
        }
        // End TT#916

		public void InitializeTableManager()
		{
			try
			{
				_tableManager.Clear();
				foreach(int variableDataType in Enum.GetValues(typeof( eVariableDataType)))
				{
					switch ((eVariableDataType)variableDataType)
					{
						default:
							if (variableDataType > 0)
							{
								for (int t=0; t<_numberVariableTypes; t++)
								{
									Hashtable ht = new Hashtable();
									_tableManager.Add(GetTableKeyValue((eVariableDataType)variableDataType, t), ht);
								}
							}
							break;
					}
				}
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
                // Begin TT#4274 - stodd - History Load not displaying Call Stack when an error is encountered
                //_audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
                _audit.Log_Exception(ex, sourceModule);
                // End TT#4274 - stodd - History Load not displaying Call Stack when an error is encountered
                _audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
				throw;
			}
		}

		private void LookupComputationModel(string aComputationModelID)
		{
			ComputationData cd = null;
            // Begin TT#1652 - JSmith - Computation model not found
            //if (aComputationModelID == null)
            if (aComputationModelID == null ||
                aComputationModelID.Trim().Length == 0)
			{
            // End TT#1652
				_computationModel = new ComputationModel();
			}
			else
			{
				_computationModel = new ComputationModel(aComputationModelID);
				if (!_computationModel.ComputationModelFound)
				{
					_audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_ComputationModelNotFound, aComputationModelID, sourceModule);
				}
				else
				{
					cd = new ComputationData();
					try
					{
						cd.OpenUpdateConnection();
						_computationGroupRID = cd.ComputationGroup_Add(eProcesses.historyPlanLoad, _computationModel.ComputationModelRID);
						cd.CommitData();
					}
					catch
					{
						throw;
					}
					finally
					{
						cd.CloseUpdateConnection();
					}
				}
			}
		}

		public void GetForecastVersions()
		{
			string description = null;
			int RID;
			ForecastVersion fv = new ForecastVersion();
			DataTable dt = fv.GetForecastVersions(false);
			foreach(DataRow dr in dt.Rows)
			{
				RID			= Convert.ToInt32(dr["FV_RID"]);
				description	= (string)dr["DESCRIPTION"];
				_forecastVersions.Add(description.ToLower(),RID);
			}
		}

		public eReturnCode ProcessVariableFile(string fileLocation, char[] delimiter, DateTime postingDate, bool aProcessingNodeLookup, ref bool errorFound)
		{
			StreamReader reader = null;
			string line = null;
			string message = null;
			eReturnCode returnCode = eReturnCode.successful;
			bool setPostingDate = false;
			
			try
			{
				if (!aProcessingNodeLookup)
				{
					// update the posting date in internal calendar
					if (postingDate != DateTime.MinValue)
					{
						_cal.SetPostingDate(postingDate);
						setPostingDate = true;
					}
					else
					{
						postingDate = _cal.PostDate.Date;
					}
					DayProfile postingDay = _cal.GetDay(postingDate);
					_postingDateID = postingDay.YearDay;
				}

				reader = new StreamReader(fileLocation);  //opens the file
					
				while ((line = reader.ReadLine()) != null)
				{
					string[] fields = MIDstringTools.Split(line,delimiter[0],true);
					if (fields.Length == 1 && (fields[0] == null || fields[0] == ""))  // skip blank line 
					{
						continue;
					}
					if (!aProcessingNodeLookup)
					{
						++_recordsRead;
					}

					if (fields.Length < 8)
					{
						if (fields[0].ToUpper() == "OPTIONS")
						{
							// this is a valid record.
						}
						else
						{
							++_recordsWithErrors;
							string msgDetails = "Delimiter defined as " + delimiter[0].ToString(CultureInfo.CurrentUICulture) + " in CONFIG file.";
							_audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_MismatchedDelimiter, msgDetails, sourceModule);
							continue;
						}
					}

					if (fields[0].ToUpper() == "OPTIONS")
					{
						if (aProcessingNodeLookup)
						{
							eHierarchyLevelType typeHint = eHierarchyLevelType.Undefined;
							// look for transaction hint
							if (fields.Length > 9 && fields[9] != null && fields[9].Length > 0)
							{
								switch (fields[9])
								{
									case "Color":
										typeHint = eHierarchyLevelType.Color;
										break;
									case "Size":
										typeHint = eHierarchyLevelType.Size;
										break;
									default:
										typeHint = eHierarchyLevelType.Undefined;
										break;
								}
							}
							_SAB.HierarchyServerSession.BuildColorSizeIDs(typeHint);
						}
						else
						{
							returnCode = ProcessOptions(line, fields, ref setPostingDate, ref postingDate);
						}
					}
					else
					{
						returnCode = SeparateDelimitedData(line, fields, aProcessingNodeLookup);
					}


					if (!aProcessingNodeLookup)
					{
						if (returnCode != eReturnCode.successful)
						{
							++_recordsWithErrors;
						}
					}
				}
			}
			catch ( FileNotFoundException fileNotFound_error )
			{
				string exceptionMessage = fileNotFound_error.Message;
				errorFound = true;
				message = " : " + fileLocation;
				_audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_InputFileNotFound, message, sourceModule);
				_audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
			}
			catch ( Exception ex )
			{
				errorFound = true;
                // Begin TT#4274 - stodd - History Load not displaying Call Stack when an error is encountered
                //_audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
                _audit.Log_Exception(ex, sourceModule);
                // End TT#4274 - stodd - History Load not displaying Call Stack when an error is encountered
                _audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
				if (_recordsNotCommitted > 0)
				{
					WriteValues();
					_recordsNotCommitted = 0;
				}
				else
				{
					CommitAutoAddedNodes();
				}

				throw;
			}
			finally
			{

				if (!aProcessingNodeLookup)
				{
					if (_currProcessingData != eVariableDataType.none)
					{
						returnCode = AddCurrentData();
					}
					if (_recordsNotCommitted > 0)
					{
						WriteValues();
					}
					// update the posting date in all services
					if (setPostingDate)
					{
						UpdatePostingDate(postingDate);
					}
				
					_audit.PostingAuditInfo_Add(_chainDailyHistoryRecs, _chainWeeklyHistoryRecs,
						_chainWeeklyForecastRecs, _storeDailyHistoryRecs, _storeWeeklyHistoryRecs, _storeWeeklyForecastRecs,
						_intransitRecs, _recordsWithErrors, _nodesAdded);
				
					if (_rollupManager != null &&
						_rollupManager.Count > 0)
					{
						WriteRollupItems();
					}

					if (_computationManager != null &&
						_computationManager.Count > 0)
					{
						WriteComputationItems();
					}
				}

				if (reader != null)
				{
					reader.Close(); 
				}
			}
			return returnCode;
		}

		public eReturnCode ProcessVariableFile(string fileLocation, DateTime postingDate, bool aProcessingNodeLookup, ref bool errorFound)
		{
			XmlTextReader reader = null;
			string message = null;
			eReturnCode returnCode = eReturnCode.successful;
			eTransactionType transactionType = eTransactionType.chain;
			ProductAmountsProductAmountPeriod period = ProductAmountsProductAmountPeriod.Week;
			string product = null;
			string parent = null;
			string version = "Actual";
			string store = "Chain";
			ProductAmountsProductAmountDateType dateType = ProductAmountsProductAmountDateType.Calendar;
			string date = DateTime.Now.ToShortDateString();
			string variable = null;
			string sizeProductCategory = null;
			string sizePrimary = null;
			string sizeSecondary = null;
			string productDescription = null;
			string productName = null;
			double amount = 0;
			string strAmount = null;
			bool setPostingDate = false;
			
			try
			{
				if (!aProcessingNodeLookup)
				{
					// update the posting date in internal calendar
					if (postingDate != DateTime.MinValue)
					{
						_cal.SetPostingDate(postingDate);
						setPostingDate = true;
					}
					else
					{
						postingDate = _cal.PostDate.Date;
					}
					DayProfile postingDay = _cal.GetDay(postingDate);
					_postingDateID = postingDay.YearDay;
				}

				reader = new XmlTextReader(fileLocation);  //opens the file
					
				while(reader.Read())
				{
					switch (reader.NodeType)
					{
						case XmlNodeType.Element:
						{
							switch (reader.Name.ToUpper(CultureInfo.CurrentUICulture))
							{
								case "OPTIONS":
								{
									eHierarchyLevelType typeHint = eHierarchyLevelType.Undefined;
									if (reader.HasAttributes)
									{
										// enumerate all attributes
										while (reader.MoveToNextAttribute())
										{
											switch (reader.Name.ToUpper(CultureInfo.CurrentUICulture))
											{
												case "SALESENDINGDATE":
												{
													try
													{
														postingDate = Convert.ToDateTime(reader.Value);
														setPostingDate = true;
													}
													catch ( Exception ex )
													{
														message = ex.Message;
														_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
													}
													break;
												}
												case "ROLLSTOREDAILYTOWEEKLY":
												{
													try
													{
														_rollStoreDailyToWeekly = Convert.ToBoolean(reader.Value);
													}
													catch ( Exception ex )
													{
														message = ex.Message;
														_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
													}
													break;
												}
												case "ROLLSTOREDAILYUPHIERARCHY":
												{
													try
													{
														_rollStoreDailyUpHierarchy = Convert.ToBoolean(reader.Value);
													}
													catch ( Exception ex )
													{
														message = ex.Message;
														_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
													}
													break;
												}
												case "ROLLSTOREWEEKLYUPHIERARCHY":
												{
													try
													{
														_rollStoreWeeklyUpHierarchy = Convert.ToBoolean(reader.Value);
													}
													catch ( Exception ex )
													{
														message = ex.Message;
														_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
													}
													break;
												}
												case "ROLLSTORETOCHAIN":
												{
													try
													{
														_rollStoreToChain = Convert.ToBoolean(reader.Value);
													}
													catch ( Exception ex )
													{
														message = ex.Message;
														_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
													}
													break;
												}
												case "ROLLCHAINUPHIERARCHY":
												{
													try
													{
														_rollChainUpHierarchy = Convert.ToBoolean(reader.Value);
													}
													catch ( Exception ex )
													{
														message = ex.Message;
														_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
													}
													break;
												}
												case "ROLLSTOCKFORWARD":
												{
													try
													{
														_rollStock = Convert.ToBoolean(reader.Value);
													}
													catch ( Exception ex )
													{
														message = ex.Message;
														_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
													}
													break;
												}

												case "ROLLINTRANSIT":
												{
													try
													{
														_rollIntransit = Convert.ToBoolean(reader.Value);
													}
													catch ( Exception ex )
													{
														message = ex.Message;
														_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
													}
													break;
												}
												case "TRANSACTIONHINT":
												{
													try
													{
														// look for transaction hint
														switch (reader.Value)
														{
															case "Color":
																typeHint = eHierarchyLevelType.Color;
																break;
															case "Size":
																typeHint = eHierarchyLevelType.Size;
																break;
															default:
																typeHint = eHierarchyLevelType.Undefined;
																break;
														}
													}
													catch ( Exception ex )
													{
														message = ex.Message;
														_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
													}
													break;
												}
												case "COMPUTATIONMODEL":
												{
													try
													{
														LookupComputationModel(reader.Value);
													}
													catch ( Exception ex )
													{
														message = ex.Message;
														_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
													}
													break;
												}
											}
										}
										// move back to the element node that contains
										// the attributes we just traversed
										reader.MoveToElement();
									}
									
									_SAB.HierarchyServerSession.BuildColorSizeIDs(typeHint);
									break;
								}
								case "PRODUCTAMOUNT":
								{
									if (reader.HasAttributes)
									{
										// enumerate all attributes
										while (reader.MoveToNextAttribute())
										{

											switch (reader.Name.ToUpper(CultureInfo.CurrentUICulture))
											{
												case "PERIOD":
												{
													if (reader.Value.ToUpper(CultureInfo.CurrentUICulture) == "DAY")
													{
														period = ProductAmountsProductAmountPeriod.Day;
													}
													else
													{
														period = ProductAmountsProductAmountPeriod.Week;
													}
													break;
												}
												case "PRODUCT":
												{
													product = reader.Value;
													break;
												}
												case "VERSION":
												{
													version = reader.Value;
													break;
												}
												case "DATETYPE":
												{
													if (reader.Value.ToUpper(CultureInfo.CurrentUICulture) == "FISCAL")
													{
														dateType = ProductAmountsProductAmountDateType.Fiscal;
													}
													else
													{
														dateType = ProductAmountsProductAmountDateType.Calendar;
													}
													break;
												}
												case "DATE":
												{
													date = reader.Value;
													break;
												}
												case "STORE":
												{
													store = reader.Value;
													switch (store.ToUpper(CultureInfo.CurrentUICulture))
													{
														case "CHAIN":
															transactionType = eTransactionType.chain;
															break;
														default:
															transactionType = eTransactionType.store;
															break;
													}
													break;
												}
													
												case "PARENT":
												{
													parent = reader.Value;
													break;
												}
												case "SIZECODEPRODUCTCATEGORY":
												{
													sizeProductCategory = reader.Value;
													break;
												}
												case "SIZECODEPRIMARY":
												{
													sizePrimary = reader.Value;
													break;
												}
												case "SIZECODESECONDARY":
												{
													sizeSecondary = reader.Value;
													break;
												}
												case "PRODUCTDESCRIPTION":
												{
													productDescription = reader.Value;
													break;
												}
												case "PRODUCTNAME":
												{
													productName = reader.Value;
													break;
												}
												default:
												{
													message = _audit.GetText(eMIDTextCode.msg_InvalidAttributeForElement, false);
													message = message.Replace("{0}", "PRODUCTAMOUNT");
													message = message.Replace("{1}", reader.Name);
													_audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
													break;
												}
											}
										}

										// move back to the element node that contains
										// the attributes we just traversed
										reader.MoveToElement();
									}
									else
									{
										// transaction error
									}
									break;
								}
								case "VARIABLEAMOUNT":
								{
									if (reader.HasAttributes)
									{
										// enumerate all attributes
										while (reader.MoveToNextAttribute())
										{

											switch (reader.Name.ToUpper(CultureInfo.CurrentUICulture))
											{
												case "VARIABLE":
												{
													variable = reader.Value;
													break;
												}
												case "AMOUNT":
												{
													strAmount = reader.Value;
													try
													{
														amount = Convert.ToDouble(reader.Value);
													}
													catch
													{
													}
													break;
												}
												default:
												{
													message = _audit.GetText(eMIDTextCode.msg_InvalidAttributeForElement, false);
													message = message.Replace("{0}", "VARIABLEAMOUNT");
													message = message.Replace("{1}", reader.Name);
													_audit.Add_Msg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InputInvalid, message, sourceModule);
													break;
												}
											}
										}

										if (aProcessingNodeLookup)
										{
											returnCode = AddLookupRecord(product, parent,
												sizeProductCategory, sizePrimary, sizeSecondary,
												productDescription, productName);
										}
										else
										{
											++_recordsRead;
											switch (transactionType)
											{
												case eTransactionType.chain:  
													returnCode = ProcessChainData(period, product, parent,
														//Begin BonTon Calcs - JScott - Add Display Precision
														//version, dateType, date, variable,
														//amount, strAmount,
														version, dateType, date, variable, strAmount,
														//End BonTon Calcs - JScott - Add Display Precision
														sizeProductCategory, sizePrimary, sizeSecondary,
														productDescription, productName);
													break;
												case eTransactionType.store:  
													returnCode = ProcessStoreData(period, product, parent,
														//Begin BonTon Calcs - JScott - Add Display Precision
														//version, store, dateType, date, variable, 
														//amount, strAmount,
														version, store, dateType, date, variable, strAmount,
														//End BonTon Calcs - JScott - Add Display Precision
														sizeProductCategory, sizePrimary, sizeSecondary, 
														productDescription, productName);
													break;

												default:  
													break;
											}
						
											if (returnCode != eReturnCode.successful)
											{
												++_recordsWithErrors;
											}
											variable = null;
											amount = 0;
											strAmount = null;
										}

										// move back to the element node that contains
										// the attributes we just traversed
										reader.MoveToElement();
									}
									else
									{
										// transaction error
									}
									break;
								}
							}
							break;
						}
						case XmlNodeType.EndElement:
						{
							switch (reader.Name.ToUpper(CultureInfo.CurrentUICulture))
							{
								case "VARIABLEAMOUNT":
									break;
								case "PRODUCTAMOUNT":
									transactionType = eTransactionType.chain;
									period = ProductAmountsProductAmountPeriod.Week;
									product = null;
									parent = null;
									version = "Actual";
									store = "Chain";
									date = DateTime.Now.ToShortDateString();
									break;
							}
							break;
						}
					}
				}
			}
			catch ( FileNotFoundException fileNotFound_error )
			{
				string exceptionMessage = fileNotFound_error.Message;
				errorFound = true;
				message = " : " + fileLocation;
				_audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_InputFileNotFound, message, sourceModule);
				_audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
			}
			catch ( Exception ex )
			{
				errorFound = true;
				_audit.Log_Exception(ex, sourceModule);
				try
				{
					_audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
				}
				catch
				{
					_audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", eMIDMessageLevel.Severe);
				}
				if (!aProcessingNodeLookup && _recordsNotCommitted > 0)
				{
					WriteValues();
					_recordsNotCommitted = 0;
				}
				throw;
			}
			finally
			{
				if (!aProcessingNodeLookup)
				{
					if (_currProcessingData != eVariableDataType.none)
					{
						returnCode = AddCurrentData();
					}
					if (_recordsNotCommitted > 0)
					{
						WriteValues();
					}
					else
					{
						CommitAutoAddedNodes();
					}
					// update the posting date in all services
					if (setPostingDate)
					{
						UpdatePostingDate(postingDate);
					}
								
					_audit.PostingAuditInfo_Add(_chainDailyHistoryRecs, _chainWeeklyHistoryRecs,
						_chainWeeklyForecastRecs, _storeDailyHistoryRecs, _storeWeeklyHistoryRecs, _storeWeeklyForecastRecs,
						_intransitRecs, _recordsWithErrors, _nodesAdded);
					
					if (_rollupManager != null &&
						_rollupManager.Count > 0)
					{
						WriteRollupItems();
					}

					if (_computationManager != null &&
						_computationManager.Count > 0)
					{
						WriteComputationItems();
					}
				}

				if (reader != null)
				{
					reader.Close(); 
				}
			}
			return returnCode;
		}

		public eReturnCode SerializeVariableFile(string fileLocation, DateTime postingDate, eHierarchyLevelType aTransactionHint, ref bool errorFound)
		{
			ProductAmounts productAmounts = null;
			string message = null;
			eReturnCode returnCode = eReturnCode.successful;
			DateTime startTime = DateTime.MinValue;
			DateTime stopTime = DateTime.MinValue;
			TimeSpan duration = TimeSpan.MinValue;
			string stringduration = string.Empty;
			bool setPostingDate = false;
			
			try
			{
				if(!File.Exists(fileLocation))	// Make sure our file exists before attempting to deserialize
				{
					errorFound = true;
					message = " : " + fileLocation;
					_audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_InputFileNotFound, message, sourceModule);
					_audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
				}
				else
				{
					TextReader r = null;
					try 
					{
						/* I created MIDRetail.HistoryPlanLoad.HistoryPlanLoadSchema.xsd to define and validate
								what a HistoryPlanLoad XML file should look like. From the Visual Studio command prompt I
								run xsd /c HistoryPlanLoadSchema.xsd to generate a class file that is a strongly typed
								represenation of that schema. The end result is I don't have to parse a loaded XML
								document node by node which can result in errors if the Xml document is not formed
								perfectly prior to reciept by this function.
							*/
						startTime = DateTime.Now;
						XmlSerializer s = new XmlSerializer(typeof(ProductAmounts));		// Create a Serializer
						r = new StreamReader(fileLocation);					// Load the Xml File
						productAmounts = (ProductAmounts)s.Deserialize(r);					// Deserialize the Xml File to a strongly typed object

						stopTime = DateTime.Now;
						duration = stopTime.Subtract(startTime);
						stringduration = Convert.ToString(duration,CultureInfo.CurrentUICulture);
						_audit.Add_Msg(eMIDMessageLevel.Information, "Serialization time=" + stringduration, this.ToString());
					}
					catch(Exception ex)
					{
						errorFound = true;
						_audit.Log_Exception(ex, sourceModule);
						returnCode = eReturnCode.severe;
						return returnCode;
					}
					finally
					{
						r.Close();														// Close the input file.
					}

					if (productAmounts.Options != null)
					{
						switch (productAmounts.Options.TransactionHint)
						{
							case ProductAmountsOptionsTransactionHint.Color:
								aTransactionHint = eHierarchyLevelType.Color;
								break;
							case ProductAmountsOptionsTransactionHint.Size:
								aTransactionHint = eHierarchyLevelType.Size;
								break;
							case ProductAmountsOptionsTransactionHint.StyleAndAbove:
								aTransactionHint = eHierarchyLevelType.Style;
								break;
							case ProductAmountsOptionsTransactionHint.All:
								aTransactionHint = eHierarchyLevelType.Undefined;
								break;
							default:
								break;
						}
					}
					if (aTransactionHint == eHierarchyLevelType.Undefined || 
						aTransactionHint == eHierarchyLevelType.Color ||
						aTransactionHint == eHierarchyLevelType.Size)
					{
						_SAB.HierarchyServerSession.BuildColorSizeIDs(aTransactionHint);
						// lookup or autoadd nodes before processing transactions 
						foreach(ProductAmountsProductAmount pa in productAmounts.ProductAmount)
						{
							AddLookupRecord(pa.Product, pa.Parent, 
								pa.SizeCodeProductCategory, pa.SizeCodePrimary,
								pa.SizeCodeSecondary, pa.ProductDescription, pa.ProductName);
						
						}
						LookupNodes();
						ClearWorkingVariables();
					}

					try
					{
						// check for override options in the file
						if (productAmounts.Options != null)
						{
							if (productAmounts.Options.SalesEndingDate != null)
							{
								try
								{
									postingDate = Convert.ToDateTime(productAmounts.Options.SalesEndingDate);
								}
								catch ( Exception ex )
								{
									message = ex.Message;
									_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
								}
							}

							if (productAmounts.Options.RollStoreDailyToWeeklySpecified)
							{
								_rollStoreDailyToWeekly = productAmounts.Options.RollStoreDailyToWeekly;
							}
							if (productAmounts.Options.RollStoreDailyUpHierarchySpecified)
							{
								_rollStoreDailyUpHierarchy = productAmounts.Options.RollStoreDailyUpHierarchy;
							}
							if (productAmounts.Options.RollStoreWeeklyUpHierarchySpecified)
							{
								_rollStoreWeeklyUpHierarchy = productAmounts.Options.RollStoreWeeklyUpHierarchy;
							}
							if (productAmounts.Options.RollStoreToChainSpecified)
							{
								_rollStoreToChain = productAmounts.Options.RollStoreToChain;
							}
							if (productAmounts.Options.RollChainUpHierarchySpecified)
							{
								_rollChainUpHierarchy = productAmounts.Options.RollChainUpHierarchy;
							}
							if (productAmounts.Options.RollIntransitSpecified)
							{
								_rollIntransit = productAmounts.Options.RollIntransit;
							}
							if (productAmounts.Options.RollStockForwardSpecified)
							{
								_rollStock = productAmounts.Options.RollStockForward;
							}

							// Computation Model
							if (productAmounts.Options.ComputationModel != null)
							{
								LookupComputationModel(productAmounts.Options.ComputationModel);
							}
						}

						// update the posting date in internal calendar
						if (postingDate != DateTime.MinValue)
						{
							_cal.SetPostingDate(postingDate);
							setPostingDate = true;
						}
						else
						{
							postingDate = _cal.PostDate.Date;
						}
						DayProfile postingDay = _cal.GetDay(postingDate);
						_postingDateID = postingDay.YearDay;

						startTime = DateTime.Now;
						if (productAmounts != null &&
							productAmounts.ProductAmount != null)
						{
							foreach(ProductAmountsProductAmount pa in productAmounts.ProductAmount)
							{
								switch (pa.Store.ToUpper(CultureInfo.CurrentUICulture))
								{
									case "CHAIN":
										if (pa.VariableAmount != null)
										{
											foreach(ProductAmountsProductAmountVariableAmount pava in pa.VariableAmount)
											{
												++_recordsRead;
												returnCode = ProcessChainData(pa.Period, pa.Product, pa.Parent,
													pa.Version, pa.DateType, pa.Date,
													//Begin BonTon Calcs - JScott - Add Display Precision
													//pava.Variable, pava.Amount, null,
													pava.Variable, Convert.ToString(pava.Amount),
													//End BonTon Calcs - JScott - Add Display Precision
													pa.SizeCodeProductCategory, pa.SizeCodePrimary,
													pa.SizeCodeSecondary, pa.ProductDescription, pa.ProductName);
												if (returnCode != eReturnCode.successful)
												{
													++_recordsWithErrors;
												}
											}
										}
										break;
									default:
										if (pa.VariableAmount != null)
										{
											foreach(ProductAmountsProductAmountVariableAmount pava in pa.VariableAmount)
											{
												++_recordsRead;
												returnCode = ProcessStoreData(pa.Period, pa.Product, pa.Parent,
													pa.Version, pa.Store, pa.DateType, pa.Date,
													//Begin BonTon Calcs - JScott - Add Display Precision
													//pava.Variable, pava.Amount, null,
													pava.Variable, Convert.ToString(pava.Amount),
													//End BonTon Calcs - JScott - Add Display Precision
													pa.SizeCodeProductCategory, pa.SizeCodePrimary,
													pa.SizeCodeSecondary, pa.ProductDescription, pa.ProductName);
												if (returnCode != eReturnCode.successful)
												{
													++_recordsWithErrors;
												}
											}
										}
										break;
								}
							}
						}
						stopTime = DateTime.Now;
						duration = stopTime.Subtract(startTime);
						stringduration = Convert.ToString(duration,CultureInfo.CurrentUICulture);
						_audit.Add_Msg(eMIDMessageLevel.Information, "Parse transaction time=" + stringduration, this.ToString());
					}
					catch(Exception ex)
					{
						errorFound = true;
						_audit.Log_Exception(ex, sourceModule);
					}
				}
			}
			catch ( Exception ex )
			{
				errorFound = true;
				_audit.Log_Exception(ex, sourceModule);
				_audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
				if (_recordsNotCommitted > 0)
				{
					WriteValues();
					_recordsNotCommitted = 0;
				}
				throw;
			}
			finally
			{
				if (returnCode != eReturnCode.severe &&
					returnCode != eReturnCode.fatal)
				{
					if (_currProcessingData != eVariableDataType.none)
					{
						returnCode = AddCurrentData();
					}
					if (_recordsNotCommitted > 0)
					{
						WriteValues();
					}
					else
					{
						CommitAutoAddedNodes();
					}
					// update the posting date in all services
					if (setPostingDate)
					{
						UpdatePostingDate(postingDate);
					}
								
					_audit.PostingAuditInfo_Add(_chainDailyHistoryRecs, _chainWeeklyHistoryRecs,
						_chainWeeklyForecastRecs, _storeDailyHistoryRecs, _storeWeeklyHistoryRecs, _storeWeeklyForecastRecs,
						_intransitRecs, _recordsWithErrors, _nodesAdded);

					if (_rollupManager != null &&
						_rollupManager.Count > 0)
					{
						WriteRollupItems();
					}

					if (_computationManager != null &&
						_computationManager.Count > 0)
					{
						WriteComputationItems();
					}
				}
			}
			return returnCode;
		}

		private void UpdatePostingDate(DateTime aPostingDate)
		{
			try
			{
				int retryCount = 0;
				int retryMaximum = 4;
				bool keepTrying = true;
				DateTime currentDate = DateTime.MinValue;
				while (keepTrying)
				{
					try
					{
						currentDate = _SAB.HierarchyServerSession.GetPostingDate();
						keepTrying = false;
					}
					catch (Exception ex)
					{
						++retryCount;
						if (retryCount == retryMaximum)
						{
							_audit.Add_Msg(eMIDMessageLevel.Severe, "HierarchyServerSession.GetPostingDate failed.  " + ex.ToString(), this.ToString());
							keepTrying = false;
							throw;
						}
						else
						{
							_audit.Add_Msg(eMIDMessageLevel.Warning, "HierarchyServerSession.GetPostingDate failed.  Retry count=" + retryCount.ToString(), this.ToString());
							System.Threading.Thread.Sleep(2000);
						}
					}
				}

				if (aPostingDate != currentDate)
				{
					retryCount = 0;
					keepTrying = true;
					while (keepTrying)
					{
						try
						{
							_SAB.HierarchyServerSession.PostingDateUpdate(aPostingDate);
							keepTrying = false;
						}
						catch (Exception ex)
						{
							++retryCount;
							if (retryCount == retryMaximum)
							{
								_audit.Add_Msg(eMIDMessageLevel.Severe, "HierarchyServerSession.PostingDateUpdate failed.  " + ex.ToString(), this.ToString());
								keepTrying = false;
								throw;
							}
							else
							{
								_audit.Add_Msg(eMIDMessageLevel.Warning, "HierarchyServerSession.PostingDateUpdate failed.  Retry count=" + retryCount.ToString(), this.ToString());
								System.Threading.Thread.Sleep(2000);
							}
						}
					}

					retryCount = 0;
					keepTrying = true;
					while (keepTrying)
					{
						try
						{
							_SAB.StoreServerSession.PostingDateUpdate(aPostingDate);
							keepTrying = false;
						}
						catch (Exception ex)
						{
							++retryCount;
							if (retryCount == retryMaximum)
							{
								_audit.Add_Msg(eMIDMessageLevel.Severe, "StoreServerSession.PostingDateUpdate failed.  " + ex.ToString(), this.ToString());
								keepTrying = false;
								throw;
							}
							else
							{
								_audit.Add_Msg(eMIDMessageLevel.Warning, "StoreServerSession.PostingDateUpdate failed.  Retry count=" + retryCount.ToString(), this.ToString());
								System.Threading.Thread.Sleep(2000);
							}
						}
					}
					retryCount = 0;
					keepTrying = true;
					while (keepTrying)
					{
						try
						{
							_SAB.ApplicationServerSession.PostingDateUpdate(aPostingDate);
							keepTrying = false;
						}
						catch (Exception ex)
						{
							++retryCount;
							if (retryCount == retryMaximum)
							{
								_audit.Add_Msg(eMIDMessageLevel.Severe, "ApplicationServerSession.PostingDateUpdate failed.  " + ex.ToString(), this.ToString());
								keepTrying = false;
								throw;
							}
							else
							{
								_audit.Add_Msg(eMIDMessageLevel.Warning, "ApplicationServerSession.PostingDateUpdate failed.  Retry count=" + retryCount.ToString(), this.ToString());
								System.Threading.Thread.Sleep(2000);
							}
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private eReturnCode ProcessOptions(string line, string[] fields,  ref bool setPostingDate, ref DateTime postingDate)
		{
			eReturnCode returnCode = eReturnCode.successful;
			try
			{
				// SalesEndingDate
				if (fields.Length > 1 && fields[1] != null && fields[1].Length > 0)
				{
					try
					{
						postingDate = Convert.ToDateTime(fields[1]);
						setPostingDate = true;
					}
					catch ( Exception ex )
					{
						returnCode = eReturnCode.severe;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
					}
				}

				// Roll store day to week
				if (fields.Length > 2 && fields[2] != null && fields[2].Length > 0)
				{
					try
					{
						_rollStoreDailyToWeekly = Convert.ToBoolean(fields[2]);
					}
					catch ( Exception ex )
					{
						returnCode = eReturnCode.severe;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
					}
				}

				// roll store day up hierarchy
				if (fields.Length > 3 && fields[3] != null && fields[3].Length > 0)
				{
					try
					{
						_rollStoreDailyUpHierarchy = Convert.ToBoolean(fields[3]);
					}
					catch ( Exception ex )
					{
						returnCode = eReturnCode.severe;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
					}
				}

				// roll store week up hierarchy
				if (fields.Length > 4 && fields[4] != null && fields[4].Length > 0)
				{
					try
					{
						_rollStoreWeeklyUpHierarchy = Convert.ToBoolean(fields[4]);
					}
					catch ( Exception ex )
					{
						returnCode = eReturnCode.severe;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
					}
				}

				// rollStoreToChain
				if (fields.Length > 5 && fields[5] != null && fields[5].Length > 0)
				{
					try
					{
						_rollStoreToChain = Convert.ToBoolean(fields[5]);
					}
					catch ( Exception ex )
					{
						returnCode = eReturnCode.severe;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
					}
				}

				// roll chain up hierarchy
				if (fields.Length > 6 && fields[6] != null && fields[6].Length > 0)
				{
					try
					{
						_rollChainUpHierarchy = Convert.ToBoolean(fields[6]);
					}
					catch ( Exception ex )
					{
						returnCode = eReturnCode.severe;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
					}
				}

				// RollStockForward
				if (fields.Length > 7 && fields[7] != null && fields[7].Length > 0)
				{
					try
					{
						_rollStock = Convert.ToBoolean(fields[7]);
					}
					catch ( Exception ex )
					{
						returnCode = eReturnCode.severe;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
					}
				}

				// RollIntransit
				if (fields.Length > 8 && fields[8] != null && fields[8].Length > 0)
				{
					try
					{
						_rollIntransit = Convert.ToBoolean(fields[8]);
					}
					catch ( Exception ex )
					{
						returnCode = eReturnCode.severe;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
					}
				}

				// Computation Model
				if (fields.Length > 10 && fields[10] != null && fields[10].Length > 0)
				{
					try
					{
						LookupComputationModel(Convert.ToString(fields[9]));
					}
					catch ( Exception ex )
					{
						returnCode = eReturnCode.severe;
						_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.Message, sourceModule);
					}
				}

				return returnCode;
			}
			catch
			{
				throw;
			}
		}

		private eReturnCode SeparateDelimitedData(string line, string[] fields, bool aProcessingNodeLookup)
		{
			eReturnCode returnCode = eReturnCode.successful;
			eTransactionType transactionType = eTransactionType.none;
			ProductAmountsProductAmountPeriod period = ProductAmountsProductAmountPeriod.Week;
			string product = null;
			string parent = null;
			string version = "Actual";
			string store = "Chain";
			ProductAmountsProductAmountDateType dateType = ProductAmountsProductAmountDateType.Calendar;
			string date = DateTime.Now.ToShortDateString();
			string variable = null;
			string sizeProductCategory = null;
			string sizePrimary = null;
			string sizeSecondary = null;
			string productDescription = null;
			string productName = null;
			int amount = 0;
			string strAmount = null;

			try
			{
				if (fields.Length < 1 || fields[0] == null || fields[0].Trim().Length == 0)   
				{
					period = ProductAmountsProductAmountPeriod.Week;
				}
				else
				{
					if (fields[0].ToUpper(CultureInfo.CurrentUICulture) == "D" || 
						fields[0].ToUpper(CultureInfo.CurrentUICulture) == "DAY")
					{
						period = ProductAmountsProductAmountPeriod.Day;
					}
					else
					{
						period = ProductAmountsProductAmountPeriod.Week;
					}
				}

				if (fields.Length < 2 || fields[1] == null || fields[1].Trim().Length == 0)   
				{
					version = "Actual";
				}
				else
				{
					version = fields[1];
				}
			

				if (fields.Length < 3 || fields[2] == null || fields[2].Trim().Length == 0)   
				{
					product = null;
				}
				else
				{
					product = fields[2];
				}

				if (fields.Length < 4 || fields[3] == null || fields[3].Trim().Length == 0)   
				{
					store = "Chain";
					transactionType = eTransactionType.chain;
				}
				else
				{
					store = fields[3];
					switch (store.ToUpper(CultureInfo.CurrentUICulture))
					{
						case "CHAIN":
							transactionType = eTransactionType.chain;
							break;
						default:
							transactionType = eTransactionType.store;
							break;
					}
				}

				if (fields.Length < 5 || fields[4] == null || fields[4].Trim().Length == 0)   
				{
					dateType = ProductAmountsProductAmountDateType.Calendar;
				}
				else
				{
					if (fields[4].ToUpper(CultureInfo.CurrentUICulture) == "F" ||
						fields[4].ToUpper(CultureInfo.CurrentUICulture) == "FISCAL")
					{
						dateType = ProductAmountsProductAmountDateType.Fiscal;
					}
					else
					{
						dateType = ProductAmountsProductAmountDateType.Calendar;
					}
				}

				if (fields.Length < 6 || fields[5] == null || fields[5].Trim().Length == 0)   
				{
					date = DateTime.Now.ToShortDateString();
				}
				else
				{
					date = fields[5];
				}

				if (fields.Length < 7 || fields[6] == null || fields[6].Trim().Length == 0)   
				{
					variable = null;
				}
				else
				{
					variable = fields[6];
				}

				if (fields.Length < 8 || fields[7] == null || fields[7].Trim().Length == 0)   
				{
					amount = 0;
				}
				else
				{
					strAmount = fields[7];
				}

				if (fields.Length < 9 || fields[8] == null || fields[8].Trim().Length == 0)   
				{
					parent = null;
				}
				else
				{
					parent = fields[8];
				}

				if (fields.Length < 10 || fields[9] == null || fields[9].Trim().Length == 0)   
				{
					sizeProductCategory = null;
				}
				else
				{
					sizeProductCategory = fields[9];
				}

				if (fields.Length < 11 || fields[10] == null || fields[10].Trim().Length == 0)   
				{
					sizePrimary = null;
				}
				else
				{
					sizePrimary = fields[10];
				}

				if (fields.Length < 12 || fields[11] == null || fields[11].Trim().Length == 0)   
				{
					sizeSecondary = null;
				}
				else
				{
					sizeSecondary = fields[11];
				}

				if (fields.Length < 13 || fields[12] == null || fields[12].Trim().Length == 0)   
				{
					productDescription = null;
				}
				else
				{
					productDescription = fields[12];
				}

				if (fields.Length < 14 || fields[13] == null || fields[13].Trim().Length == 0)   
				{
					productName = null;
				}
				else
				{
					productName = fields[13];
				}

				if (aProcessingNodeLookup)
				{
					returnCode = AddLookupRecord(product, parent,
						sizeProductCategory, sizePrimary, sizeSecondary,
						productDescription, productName);
				}
				else
				{
					switch (transactionType)
					{
						case eTransactionType.chain:  
							returnCode = ProcessChainData(period, product, parent,
								version, dateType, date, variable,
								//Begin BonTon Calcs - JScott - Add Display Precision
								//amount, strAmount, sizeProductCategory, sizePrimary, sizeSecondary,
								strAmount, sizeProductCategory, sizePrimary, sizeSecondary,
								//End BonTon Calcs - JScott - Add Display Precision
								productDescription, productName);
							break;
						case eTransactionType.store:  
							returnCode = ProcessStoreData(period, product, parent,
								version, store, dateType, date, variable,
								//Begin BonTon Calcs - JScott - Add Display Precision
								//amount, strAmount, sizeProductCategory, sizePrimary, sizeSecondary,
								strAmount, sizeProductCategory, sizePrimary, sizeSecondary,
								//End BonTon Calcs - JScott - Add Display Precision
								productDescription, productName);
							break;

						default:  
							break;
					}
				}
			}
			catch
			{
				throw;
			}

			return returnCode;
			
		}

		protected eReturnCode ProcessChainData(ProductAmountsProductAmountPeriod aPeriod, string aProductID, 
			string aParent, string aVersion, ProductAmountsProductAmountDateType aDateType, string aDate,
			//Begin BonTon Calcs - JScott - Add Display Precision
			//string aVariable, double aAmount, string aStrAmount,
			string aVariable, string aStrAmount,
			//End BonTon Calcs - JScott - Add Display Precision
			string aSizeProductCategory, string aSizePrimary, string aSizeSecondary, string aProductDescription,
			string aProductName)
		{
			eReturnCode returnCode = eReturnCode.successful;
			string message = null;
			EditMsgs em = new EditMsgs();
			eVariableDataType processingData = eVariableDataType.none;
			string version = "Actual";
			int versionRID = Include.NoRID;
			string productID = null;
			double amount = 0;
			EditMsgs tempem = null;

			try
			{
				message =  "Period: " + aPeriod + "; Product: " + aProductID + "; Version: " + aVersion + "; DateType: " + aDateType + "; Date: " + aDate + "; Variable: " + aVariable + "; Amount: " + aStrAmount + "; Parent: " + aParent + "; ProductDescription: " + aProductDescription+ "; ProductName: " + aProductName;
				if (aVersion == null)   
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VersionRequired, message, sourceModule);
				}
				else
				{
					version = aVersion.ToLower();
					if (_forecastVersions.Contains(version))
					{
						versionRID = (int)_forecastVersions[version];
					}
					else
					{
						em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VersionNotFound, message, sourceModule);
					}
				}

				if (aPeriod == ProductAmountsProductAmountPeriod.Day)
				{
					if (versionRID == 1)
					{
						processingData = eVariableDataType.chainDailyHistory;
					}
					else
					{
						em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NoChainDailyForcast, message, sourceModule);
					}
				}
				else
				{
					if (versionRID == 1)
					{
						processingData = eVariableDataType.chainWeeklyHistory;
					}
					else
					{
						processingData = eVariableDataType.chainWeeklyForecast;
					}
				}

				if (aProductID == null)   
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductRequired, message, sourceModule);
				}
				else
				{
					if (aProductID != _transNodeID)
					{
						object nodeRID = _nodes[aProductID];
						if (nodeRID != null)
						{
							_transNodeRID = Convert.ToInt32(nodeRID, CultureInfo.CurrentCulture);
						}
						else
						{
							_transNodeRID = Include.NoRID;
						}

						if (_transNodeRID == Include.NoRID) // not defined check for compound or autoadd
						{
							string oParentID = null;
							tempem = new EditMsgs();
							HierarchyNodeProfile hnp = _hm.NodeLookup(ref  tempem, aProductID, out oParentID, _allowAutoAdds, false);
							if (tempem.ErrorFound)
							{
								// transfer errors to main error class
								for (int e=0; e<tempem.EditMessages.Count; e++)
								{
									returnCode = eReturnCode.editErrors;
									EditMsgs.Message emm = (EditMsgs.Message) tempem.EditMessages[e];
									em.AddMsg(emm.messageLevel, emm.code, message, emm.module);
								}
							}
							if (hnp.Key == Include.NoRID) // not defined check for autoadd
							{
								if (_allowAutoAdds)
								{
									// replace parent with what was looked up if parent not provided
									if (aParent == null &&
										oParentID != null)
									{
										aParent = oParentID;
									}
									if (aParent != null)
									{
										tempem = new EditMsgs();
										returnCode = DetermineNode(ref  tempem, hnp, aParent, aProductID,
											aSizeProductCategory, aSizePrimary, aSizeSecondary, aProductDescription,
											aProductName);
										if (returnCode == eReturnCode.successful)
										{
											productID = aProductID;
											_nodes[aProductID] = _transNodeRID;
										}
										else
										{
											// transfer errors to main error class
											for (int e=0; e<tempem.EditMessages.Count; e++)
											{
												returnCode = eReturnCode.editErrors;
												EditMsgs.Message emm = (EditMsgs.Message) tempem.EditMessages[e];
												em.AddMsg(emm.messageLevel, emm.code, message, emm.module);
											}
											_transNodeID = null;
										}

									}
									else
									{
										_transNodeID = null;
										em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentRequiredAutoadd, message, sourceModule);
									}
								}
								else
								{
									_transNodeID = null;
									em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NodeNotFoundAutoaddDisabled, message, sourceModule);
								}
							}
							else
							{
								productID = aProductID;
								_transNodeID = aProductID;
								_transNodeRID = hnp.Key;
								if (!_nodes.ContainsKey(productID))
								{
									_nodes.Add(aProductID, _transNodeRID);
								}
							}
						}
						else
						{
							productID = aProductID;
							_transNodeID = aProductID;
						}
					}
				}

				if (aVariable == null)   
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VariableRequired, message, sourceModule);
				}
				else
				{
					if (aVariable != _transVariable)
					{
						_variableProfile = _variables.GetVariableProfileByName(aVariable);
						if (_variableProfile == null)
						{
							em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VariableNotFound, message, sourceModule);
						}
						// Begin MID Track #4637 - JSmith - Split variables by type
						else if ((processingData == eVariableDataType.chainWeeklyHistory && _variableProfile.ChainHistoryModelType == eVariableDatabaseModelType.None) ||
							(processingData == eVariableDataType.chainWeeklyForecast && _variableProfile.ChainForecastModelType == eVariableDatabaseModelType.None))
						{
							em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidForTypeOfData, message, sourceModule);
							_transVariable = string.Empty;
						}
						else
						{
							if (processingData == eVariableDataType.chainWeeklyHistory &&
								(_variableProfile.VariableType != eVariableType.Intransit) &&
								(_variableProfile.DatabaseColumnName == _variables.SalesTotalUnitsVariable.DatabaseColumnName ||
								_variableProfile.DatabaseColumnName == _variables.InventoryTotalUnitsVariable.DatabaseColumnName))
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HistoryNotToTotal, message, sourceModule);
							}
							else
								if (_variableProfile.VariableType != eVariableType.Intransit &&
								_variableProfile.DatabaseColumnName == null)
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VariableNotSavedOnDatabase, message, sourceModule);
							}
							else
							{
								_transVariable = aVariable;
							}
						}
					}
				}

				if (aDate == null)   
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DateRequired, message, sourceModule);
				}
				else
				{
					if (aDate != _transDate ||
						processingData != _currProcessingData)
					{
						try
						{
							if (processingData == eVariableDataType.chainDailyHistory)
							{
								DayProfile mrsDay = null;
								if (aDateType == ProductAmountsProductAmountDateType.Fiscal)
								{
									mrsDay = _cal.GetFiscalDay(Convert.ToInt32(aDate));
								}
								else
								{
									_date = Convert.ToDateTime(aDate);
									mrsDay = _cal.GetDay(_date);
								}
								_transCurrentDateID = mrsDay.Key;
								_transCurrentFiscalYearWeek = mrsDay.Week.YearWeek;
								mrsDay = _cal.Add(mrsDay, 1);
								_transNextDateID = mrsDay.Key;
								_transNextFiscalYearWeek = mrsDay.Week.YearWeek;
							}
							else
							{
								WeekProfile mrsWeek = null;
								if (aDateType == ProductAmountsProductAmountDateType.Fiscal)
								{
									mrsWeek = _cal.GetWeek(Convert.ToInt32(aDate));
								}
								else
								{
									_date = Convert.ToDateTime(aDate);
									mrsWeek = _cal.GetWeek(_date);
								}
								_transCurrentDateID = mrsWeek.Key;
								_transCurrentFiscalYearWeek = mrsWeek.YearWeek;
								mrsWeek = _cal.Add(mrsWeek, 1);
								_transNextDateID = mrsWeek.Key;
								_transNextFiscalYearWeek = mrsWeek.YearWeek;
							}
							_transDate = aDate;
						}
						catch(System.Exception e)
						{
							string exceptionMessage = e.Message;
							em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DateNotFound, message, sourceModule);
						}
					}
				}

				if (_variableProfile != null)
				{
					if (_variableProfile.VariableType == eVariableType.BegStock &&
						_rollStock)
					{
						_transDateID = _transNextDateID;
						_transFiscalYearWeek = _transNextFiscalYearWeek;
					}
					else
					{
						_transDateID = _transCurrentDateID;
						_transFiscalYearWeek = _transCurrentFiscalYearWeek;
					}
				}

				if (aStrAmount != null)
				{
					if (_variableProfile != null)
					{
						if (_variableProfile.ChainDatabaseVariableType == eVariableDatabaseType.Real ||
							_variableProfile.ChainDatabaseVariableType == eVariableDatabaseType.Float)
						{
							try
							{
								amount = Convert.ToDouble(aStrAmount);
							}
							catch
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeNumeric, message, sourceModule);
							}
						}
						else if (_variableProfile.ChainDatabaseVariableType == eVariableDatabaseType.Integer)
						{
							try
							{
								amount = Convert.ToInt32(aStrAmount);
							}
							catch
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeInteger, message, sourceModule);
							}
						}
						else if (_variableProfile.ChainDatabaseVariableType == eVariableDatabaseType.BigInteger)
						{
							try
							{
								amount = Convert.ToInt64(aStrAmount);
							}
							catch
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeInteger, message, sourceModule);
							}
						}
						else
						{
							try
							{
								amount = Convert.ToInt32(aStrAmount);
							}
							catch
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeNumeric, message, sourceModule);
							}
						}
					}
					else
					{
						try
						{
							amount = Convert.ToInt32(aStrAmount);
						}
						catch
						{
							em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeNumeric, message, sourceModule);
						}
					}
				}

				switch (processingData)
				{
					case eVariableDataType.chainDailyHistory:
						++_chainDailyHistoryRecs;
						break;
					case eVariableDataType.chainWeeklyForecast:
						++_chainWeeklyForecastRecs;
						break;
					case eVariableDataType.chainWeeklyHistory:
						++_chainWeeklyHistoryRecs;
						break;
				}

				for (int e=0; e<em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[e];
					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.lineNumber, emm.module);
				}
				if (em.ErrorFound)
				{
					returnCode = eReturnCode.editErrors;

				}
				else
				{
					if (_currProcessingData != eVariableDataType.none
						&& (processingData != _currProcessingData
						|| versionRID != _currVersionRID
						|| _transNodeRID != _currNodeRID  
						|| _transDateID != _currDate
						|| Include.NoRID != _currStoreRID))
					{
						ProcessKeyBreak();
						AddCurrentData();
					}
					HistoryPlanValue hpv = new HistoryPlanValue();
					hpv.Value = amount;
					hpv.VariableProfile = _variableProfile;
					_values[_variableProfile.DatabaseColumnPosition] = hpv;
					_foundValues = true;
							
					_currProcessingData = processingData;
					_currVersionRID = versionRID;
					_currDate = _transDateID;
					_currFiscalYearWeek = _transFiscalYearWeek;
					_currIntransitDate = _intransitDateID;
					_currStoreRID = Include.NoRID;
					_currNodeRID = _transNodeRID;
					_currNodeID = _transNodeID;
				}
			}
			catch
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				throw;
			}
			
			return returnCode;
		}

				
		protected eReturnCode ProcessStoreData(ProductAmountsProductAmountPeriod aPeriod, string aProductID, 
			string aParent, string aVersion, string aStore, ProductAmountsProductAmountDateType aDateType,
			//Begin BonTon Calcs - JScott - Add Display Precision
			//string aDate, string aVariable, double aAmount, string aStrAmount,
			string aDate, string aVariable, string aStrAmount,
			//End BonTon Calcs - JScott - Add Display Precision
			string aSizeProductCategory, string aSizePrimary, string aSizeSecondary, string aProductDescription,
			string aProductName)
		{
			eReturnCode returnCode = eReturnCode.successful;
			string message = null;
			EditMsgs em = new EditMsgs();
			eVariableDataType processingData = eVariableDataType.none;
			string version = "Actual";
			int versionRID = Include.NoRID;
			string productID = null;
			string storeID = "Chain";
			double amount = 0;
			EditMsgs tempem = null;
			
			try
			{
				message =  "Period: " + aPeriod + "; Product: " + aProductID + "; Version: " + aVersion + "; Store: " + aStore + "; DateType: " + aDateType + "; Date: " + aDate + "; Variable: " + aVariable + "; Amount: " + aStrAmount + "; Parent: " + aParent+ "; ProductDescription: " + aProductDescription+ "; ProductName: " + aProductName;
				
				if (aVersion == null)   
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VersionRequired, message, sourceModule);
				}
				else
				{
					version = aVersion.ToLower();
					if (_forecastVersions.Contains(version))
					{
						versionRID = (int)_forecastVersions[version];
					}
					else
					{
						em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VersionNotFound, message, sourceModule);
					}
				}

				if (aPeriod == ProductAmountsProductAmountPeriod.Day)
				{
					if (versionRID == 1)
					{
						processingData = eVariableDataType.storeDailyHistory;
					}
					else
					{
						em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NoStoreDailyForcast, message, sourceModule);
					}
				}
				else
				{
					if (versionRID == 1)
					{
						processingData = eVariableDataType.storeWeeklyHistory;
					}
					else
					{
						processingData = eVariableDataType.storeWeeklyForecast;
					}
				}

				if (aProductID == null)   
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductRequired, message, sourceModule);
				}
				else
				{
					if (aProductID != _transNodeID)
					{
						object nodeRID = _nodes[aProductID];
						if (nodeRID != null)
						{
							_transNodeRID = Convert.ToInt32(nodeRID, CultureInfo.CurrentCulture);
						}
						else
						{
							_transNodeRID = Include.NoRID;
						}

						if (_transNodeRID == Include.NoRID) // not defined check for compound or autoadd
						{
							string oParentID = null;
							tempem = new EditMsgs();
							HierarchyNodeProfile hnp = _hm.NodeLookup(ref  tempem, aProductID, out oParentID, _allowAutoAdds, false);
							if (tempem.ErrorFound)
							{
								// transfer errors to main error class
								for (int e=0; e<tempem.EditMessages.Count; e++)
								{
									returnCode = eReturnCode.editErrors;
									EditMsgs.Message emm = (EditMsgs.Message) tempem.EditMessages[e];
									em.AddMsg(emm.messageLevel, emm.code, message, emm.module);
								}
							}
							if (hnp.Key == Include.NoRID) // not defined check for autoadd
							{
								if (_allowAutoAdds)
								{
									// replace parent with what was looked up if parent not provided
									if (aParent == null &&
										oParentID != null)
									{
										aParent = oParentID;
									}
									if (aParent != null)
									{
										tempem = new EditMsgs();
										returnCode = DetermineNode(ref  tempem, hnp, aParent, aProductID,
											aSizeProductCategory, aSizePrimary, aSizeSecondary, aProductDescription,
											aProductName);
										if (returnCode == eReturnCode.successful)
										{
											productID = aProductID;
											_transNodeID = aProductID;
											_nodes[aProductID] = _transNodeRID;
										}
										else
										{
											// transfer errors to main error class
											for (int e=0; e<tempem.EditMessages.Count; e++)
											{
												returnCode = eReturnCode.editErrors;
												EditMsgs.Message emm = (EditMsgs.Message) tempem.EditMessages[e];
												em.AddMsg(emm.messageLevel, emm.code, message, emm.module);
											}
											_transNodeID = null;
										}
									}
									else
									{
										_transNodeID = null;
										em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentRequiredAutoadd, message, sourceModule);
									}
								}
								else
								{
									_transNodeID = null;
									em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_NodeNotFoundAutoaddDisabled, message, sourceModule);
								}
							}
							else
							{
								productID = aProductID;
								_transNodeID = aProductID;
								_transNodeRID = hnp.Key;
								if (!_nodes.ContainsKey(productID))
								{
									_nodes.Add(aProductID, _transNodeRID);
								}
							}
						}
						else
						{
							productID = aProductID;
							_transNodeID = aProductID;
						}
					}
				}

				if (aStore == null)   
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreRequired, message, sourceModule);
				}
				else
				{
					storeID = aStore;
					if (storeID != _transStoreID)
					{
						object storeRID = _stores[storeID];
						if (storeRID != null)
						{
							_transStoreRID = Convert.ToInt32(storeRID, CultureInfo.CurrentCulture);
							_transStoreID = aStore;
						}
						else
						{
							_transStoreRID = Include.UndefinedStoreRID;
							em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreNotFound, message, sourceModule);
							_transStoreID = null;
						}
					}
				}

				if (aVariable == null)   
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VariableRequired, message, sourceModule);
				}
				else
				{
					if (aVariable != _transVariable)
					{
						_variableProfile = _variables.GetVariableProfileByName(aVariable);
						if (_variableProfile == null)
						{
							em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VariableNotFound, message, sourceModule);
							_transVariable = string.Empty;
						}
						// Begin MID Track #4637 - JSmith - Split variables by type
                        // Begin Track #5786 - JSmith - Load fails for intransit
                        //if ((processingData == eVariableDataType.storeDailyHistory && _variableProfile.StoreDailyHistoryModelType == eVariableDatabaseModelType.None) ||
                        //    (processingData == eVariableDataType.storeWeeklyHistory && _variableProfile.StoreWeeklyHistoryModelType == eVariableDatabaseModelType.None) ||
                        //    (processingData == eVariableDataType.storeWeeklyForecast && _variableProfile.StoreForecastModelType == eVariableDatabaseModelType.None))
                        else if ((processingData == eVariableDataType.storeDailyHistory && _variableProfile.VariableType != eVariableType.Intransit && _variableProfile.StoreDailyHistoryModelType == eVariableDatabaseModelType.None) ||
                            (processingData == eVariableDataType.storeWeeklyHistory && _variableProfile.VariableType != eVariableType.Intransit && _variableProfile.StoreWeeklyHistoryModelType == eVariableDatabaseModelType.None) ||
                            (processingData == eVariableDataType.storeWeeklyForecast && _variableProfile.StoreForecastModelType == eVariableDatabaseModelType.None))
                        // End Track #5786
						{
							em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InvalidForTypeOfData, message, sourceModule);
							_transVariable = string.Empty;
						}
						// End MID Track #4637
						else
						{
							if ((processingData == eVariableDataType.storeDailyHistory ||
								processingData == eVariableDataType.storeWeeklyHistory) &&
								(_variableProfile.VariableType != eVariableType.Intransit) &&
								(_variableProfile.DatabaseColumnName == _variables.SalesTotalUnitsVariable.DatabaseColumnName ||
								_variableProfile.DatabaseColumnName == _variables.InventoryTotalUnitsVariable.DatabaseColumnName))
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_HistoryNotToTotal, message, sourceModule);
							}
							else
								if (_variableProfile.VariableType != eVariableType.Intransit &&
								_variableProfile.DatabaseColumnName == null)
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VariableNotSavedOnDatabase, message, sourceModule);
							}
							else
							{
								_transVariable = aVariable;
							}
						}
					}
				}

				if (aDate == null)   
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DateRequired, message, sourceModule);
				}
				else
				{
					if (aDate != _transDate ||
						processingData != _currProcessingData)
					{
						try
						{
							if (processingData == eVariableDataType.storeDailyHistory)
							{
								DayProfile mrsDay = null;
								if (aDateType == ProductAmountsProductAmountDateType.Fiscal)
								{
									mrsDay = _cal.GetFiscalDay(Convert.ToInt32(aDate));
								}
								else
								{
									_date = Convert.ToDateTime(aDate);
									mrsDay = _cal.GetDay(_date);
								}
								
								_transCurrentDateID = mrsDay.Key;
								_transCurrentFiscalYearWeek = mrsDay.Week.YearWeek;
								mrsDay = _cal.Add(mrsDay, 1);
								_transNextDateID = mrsDay.Key;
								_transNextFiscalYearWeek = mrsDay.Week.YearWeek;
							}
							else	// if store weekly
							{
								WeekProfile mrsWeek = null;
								if (aDateType == ProductAmountsProductAmountDateType.Fiscal)
								{
									mrsWeek = _cal.GetWeek(Convert.ToInt32(aDate));
								}
								else
								{
									_date = Convert.ToDateTime(aDate);
									mrsWeek = _cal.GetWeek(_date);
								}
								_transCurrentDateID = mrsWeek.Key;
								_transCurrentFiscalYearWeek = mrsWeek.YearWeek;

								mrsWeek = _cal.Add(mrsWeek, 1);
								_transNextDateID = mrsWeek.Key;
								_transNextFiscalYearWeek = mrsWeek.YearWeek;
							}
							_transDate = aDate;
						}
						catch(System.Exception e)
						{
							string exceptionMessage = e.Message;
							em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DateNotFound, message, sourceModule);
						}
					}
				}

				if (_variableProfile != null)
				{
					if (_variableProfile.VariableType == eVariableType.Intransit)
					{
						if (processingData != eVariableDataType.storeDailyHistory)
						{
							em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_IntransitMustBeByDay, message, sourceModule);
						}
						else
						{
							// adjust intransit date
							if (_transCurrentDateID == _postingDateID)
							{
								_intransitDateID = _transNextDateID;
							}
							else
							{
								_intransitDateID = _transCurrentDateID;
							}

							if (_intransitDateID < _postingDateID)
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_IntransitMustFuture, message, sourceModule);
							}
						}
					}
					else if (_variableProfile.VariableType == eVariableType.BegStock &&
						_rollStock)
					{
						_transDateID = _transNextDateID;
					}
					else
					{
						_transDateID = _transCurrentDateID;
					}
				}

                // Begin TT#155 - JSmith - Size Curve Method
                if (_variableProfile != null)
                {
                    if (_variableProfile.VariableType == eVariableType.BegStock &&
                        _rollStock)
                    {
                        _transDateID = _transNextDateID;
                        _transFiscalYearWeek = _transNextFiscalYearWeek;
                    }
                    else
                    {
                        _transDateID = _transCurrentDateID;
                        _transFiscalYearWeek = _transCurrentFiscalYearWeek;
                    }
                }
                // End TT#155

				if (aStrAmount != null)
				{
					if (_variableProfile != null)
					{
						if (_variableProfile.StoreDatabaseVariableType == eVariableDatabaseType.Real ||
							_variableProfile.StoreDatabaseVariableType == eVariableDatabaseType.Float)
						{
							try
							{
								amount = Convert.ToDouble(aStrAmount);
							}
							catch
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeNumeric, message, sourceModule);
							}
						}
						else if (_variableProfile.StoreDatabaseVariableType == eVariableDatabaseType.Integer)
						{
							try
							{
								amount = Convert.ToInt32(aStrAmount);
							}
							catch
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeInteger, message, sourceModule);
							}
						}
						else if (_variableProfile.StoreDatabaseVariableType == eVariableDatabaseType.BigInteger)
						{
							try
							{
								amount = Convert.ToInt64(aStrAmount);
							}
							catch
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeInteger, message, sourceModule);
							}
						}
						else
						{
							try
							{
								amount = Convert.ToInt32(aStrAmount);
							}
							catch
							{
								em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeNumeric, message, sourceModule);
							}
						}
					}
					else
					{
						try
						{
							amount = Convert.ToInt32(aStrAmount);
						}
						catch
						{
							em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_MustBeNumeric, message, sourceModule);
						}
					}
				}
		
				if (_variableProfile != null &&
					_variableProfile.VariableType == eVariableType.Intransit)
				{
					++_intransitRecs;
					if (amount < 0)
					{
						em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_InTransitCannotBeNeg, message, sourceModule);
					}
				}
				else
				{
					switch (processingData)
					{
						case eVariableDataType.storeDailyHistory:
							++_storeDailyHistoryRecs;
							break;
						case eVariableDataType.storeWeeklyForecast:
							++_storeWeeklyForecastRecs;
							break;
						case eVariableDataType.storeWeeklyHistory:
							++_storeWeeklyHistoryRecs;
							break;
					}
				}

				for (int e=0; e<em.EditMessages.Count; e++)
				{
					EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[e];
					_audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.lineNumber, emm.module);
				}

				if (em.ErrorFound)
				{
					returnCode = eReturnCode.editErrors;
				}
				else
				{
					if (_transNodeRID < 1)
					{
						_audit.Add_Msg(eMIDMessageLevel.Severe, "Bad node rid with " + message, sourceModule);
					}
					if (_transStoreRID < 1)
					{
						_audit.Add_Msg(eMIDMessageLevel.Severe, "Bad store rid with " + message, sourceModule);
					}
					if (_currProcessingData != eVariableDataType.none
						&& (processingData != _currProcessingData
						|| versionRID != _currVersionRID
						|| _transNodeRID != _currNodeRID  
						|| _transDateID != _currDate
						|| _intransitDateID != _currIntransitDate
						|| _transStoreRID != _currStoreRID))
					{
						ProcessKeyBreak();
						returnCode = AddCurrentData();
					}

					HistoryPlanValue hpv = new HistoryPlanValue();
					hpv.Value = amount;
					hpv.VariableProfile = _variableProfile;
					switch (hpv.VariableProfile.VariableType)
					{
						case eVariableType.Intransit:
							_intransitValues[0] = hpv;
							_foundIntransitValues = true;
							break;
						default:
							_values[_variableProfile.DatabaseColumnPosition] = hpv;
							_foundValues = true;
							break;
					}
					_currProcessingData = processingData;
					_currVersionRID = versionRID;
					_currDate = _transDateID;
					_currFiscalYearWeek = _transFiscalYearWeek;
					_currIntransitDate = _intransitDateID;
					_currStoreRID = _transStoreRID;
					_currNodeRID = _transNodeRID;
					_currNodeID = _transNodeID;
				}
			}
			catch (Exception ex)
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber() + " - " + ex.ToString());
				debugWriter.Flush();
			
				string reportingModule;
				Exception innerE = ex;
				int stack = 1;
				while (innerE.InnerException != null) 
				{
					if (innerE.TargetSite == null)
					{
						reportingModule = "unknown";
					}
					else
					{
						reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
					}
					debugWriter.WriteLine("exception " + reportingModule + " " + new System.Diagnostics.StackFrame(stack,true).GetFileLineNumber() + " - " + innerE.ToString());
					debugWriter.Flush();
					++stack;
					innerE = innerE.InnerException;
				}
				debugWriter.Close();
#endif
				throw;
			}
			
			return returnCode;
		}

		protected void ProcessKeyBreak()
		{
			try
			{
				if (_recordsNotCommitted >= _commitLimit)
				{
					WriteValues();
					_recordsNotCommitted = 0;
				}
			}
			catch
			{
				throw;
			}
		}
		protected eReturnCode DetermineNode(ref EditMsgs em, HierarchyNodeProfile hnp, string aParentID, string aProductID,
			string aSizeProductCategory, string aSizePrimary, string aSizeSecondary, string aProductDescription, 
			string aProductName)
		{
			eReturnCode returnCode = eReturnCode.successful;
			try
			{
				string parentID = aParentID;
				int parentRID = Include.NoRID;
				int startLevel = 0;
				string[] fields = MIDstringTools.Split(aProductID,_levelDelimiter,true);
				string description = null;
				string[] descriptions = null;
				if (aProductDescription != null)
				{
					descriptions = MIDstringTools.Split(aProductDescription,_levelDelimiter,true);
				}
				string name = null;
				string[] names = null;
				if (aProductName != null)
				{
					names = MIDstringTools.Split(aProductName,_levelDelimiter,true);
				}
				// make sure parent is valid
				HierarchyNodeProfile parent_hnp = _hm.NodeLookup(ref  em, parentID, true, false);
				parentRID = parent_hnp.Key;
				if (parentRID != Include.NoRID)
				{
					try
					{
						
						if (!_SAB.HierarchyServerSession.UpdateConnectionIsOpen())
						{
							_SAB.HierarchyServerSession.OpenUpdateConnection();
						}
						// autoadd levels
						for (int i=startLevel; i < fields.Length; i++)
						{
							if (aProductDescription != null &&
								i < descriptions.Length)
							{
								description = descriptions[i];
							}
							else
							{
								description = null;
							}
							if (aProductName != null &&
								i < names.Length)
							{
								name = names[i];
							}
							else
							{
								name = null;
							}
							returnCode = AutoAddMerchandise(ref  em, parentRID, fields[i],
								aSizeProductCategory, aSizePrimary, aSizeSecondary, description, name);
							parentRID = _transNodeRID;

							// When no RID is returned, there was a problem auto adding the node. we
							// need to exit the field loop.
							if (parentRID == Include.NoRID)
								break;
						}
					}
					catch
					{
						throw;
					}
				}
				else
				{
					em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ParentNotFound, parentID, sourceModule);
					returnCode = eReturnCode.editErrors;
				}
			}
			catch (System.Net.Sockets.SocketException ex)
			{
				_audit.Log_Exception(ex, sourceModule);
				throw;
			}
			catch (Exception ex)
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber() + " - " + ex.ToString());
				debugWriter.Flush();
			
				string reportingModule;
				Exception innerE = ex;
				int stack = 1;
				while (innerE.InnerException != null) 
				{
					if (innerE.TargetSite == null)
					{
						reportingModule = "unknown";
					}
					else
					{
						reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
					}
					debugWriter.WriteLine("exception " + reportingModule + " " + new System.Diagnostics.StackFrame(stack,true).GetFileLineNumber() + " - " + innerE.ToString());
					debugWriter.Flush();
					++stack;
					innerE = innerE.InnerException;
				}
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				returnCode = eReturnCode.severe;
			}
			finally
			{
			}
			return returnCode;
		}

		protected eReturnCode CommitAutoAddedNodes()
		{
			eReturnCode returnCode = eReturnCode.successful;
			if (!_nodesNeedCommitted)
			{
				return returnCode;
			}
			try
			{
				if (!_SAB.HierarchyServerSession.UpdateConnectionIsOpen())
				{
					return eReturnCode.severe;
				}
				_SAB.HierarchyServerSession.CommitData();
				_nodesNeedCommitted = false;
			}
			catch (System.Net.Sockets.SocketException ex)
			{
				_audit.Log_Exception(ex, sourceModule);
				throw;
			}
			catch (Exception ex)
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber() + " - " + ex.ToString());
				debugWriter.Flush();
			
				string reportingModule;
				Exception innerE = ex;
				int stack = 1;
				while (innerE.InnerException != null) 
				{
					if (innerE.TargetSite == null)
					{
						reportingModule = "unknown";
					}
					else
					{
						reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
					}
					debugWriter.WriteLine("exception " + reportingModule + " " + new System.Diagnostics.StackFrame(stack,true).GetFileLineNumber() + " - " + innerE.ToString());
					debugWriter.Flush();
					++stack;
					innerE = innerE.InnerException;
				}
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				returnCode = eReturnCode.severe;
			}
			finally
			{
				// close connection
				_SAB.HierarchyServerSession.CloseUpdateConnection();
			}
			return returnCode;
		}

		protected eReturnCode AutoAddMerchandise(ref EditMsgs em, int parentRID, string productID,
			string aSizeProductCategory, string aSizePrimary, string aSizeSecondary, string aProductDescription,
			string aProductName)
		{
			eReturnCode returnCode = eReturnCode.successful;
			try
			{

				EditMsgs quickAddem = new EditMsgs();
				bool nodeAdded = true;
                // data is now locked in the hierarchy session.
				// open connection to lock node for updating

				_transNodeRID = _hm.QuickAdd(ref quickAddem, parentRID, productID, aProductDescription,
					aProductName, aSizeProductCategory, aSizePrimary, aSizeSecondary, ref nodeAdded);
				_nodesNeedCommitted = true;
				if (quickAddem.ErrorFound)
				{
					// transfer quick add error to main error class
					for (int e=0; e<quickAddem.EditMessages.Count; e++)
					{
						returnCode = eReturnCode.editErrors;
						EditMsgs.Message emm = (EditMsgs.Message) quickAddem.EditMessages[e];
						em.AddMsg(emm.messageLevel, emm.code, emm.msg, emm.module);
					}
				}
				else
				{
					if (nodeAdded)
					{
						++_nodesAdded;
					}
				}
			}
			catch (System.Net.Sockets.SocketException ex)
			{
				_audit.Log_Exception(ex, sourceModule);
				throw;
			}
			catch (Exception ex)
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber() + " - " + ex.ToString());
				debugWriter.Flush();
			
				string reportingModule;
				Exception innerE = ex;
				int stack = 1;
				while (innerE.InnerException != null) 
				{
					if (innerE.TargetSite == null)
					{
						reportingModule = "unknown";
					}
					else
					{
						reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
					}
					debugWriter.WriteLine("exception " + reportingModule + " " + new System.Diagnostics.StackFrame(stack,true).GetFileLineNumber() + " - " + innerE.ToString());
					debugWriter.Flush();
					++stack;
					innerE = innerE.InnerException;
				}
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				returnCode = eReturnCode.severe;
			}
			finally
			{
			}

			return returnCode;
		}

		protected eReturnCode AddCurrentData()
		{
			eReturnCode returnCode = eReturnCode.successful;
			try
			{
				if (_currNodeRID != Include.NoRID)
				{
					if (_currVersionRID != Include.NoRID)
					{
						if (_foundValues)
						{
							returnCode = AddCurrentData(eVariableType.Other, _currNodeRID, _currDate, _currFiscalYearWeek, _currVersionRID, _currStoreRID, _values, _locks);
							_values = new HistoryPlanValue[_numberDatabaseColumns];
							_locks = new object[_numberDatabaseColumns];
						}
					}
					if (_foundIntransitValues)
					{
						returnCode = AddCurrentData(eVariableType.Intransit, _currNodeRID, _currIntransitDate, _currFiscalYearWeek, _currVersionRID, _currStoreRID, _intransitValues, null);
						_intransitValues = new HistoryPlanValue[1];
					}
				}
			}
			catch (Exception ex)
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber() + " - " + ex.ToString());
				debugWriter.Flush();
			
				string reportingModule;
				Exception innerE = ex;
				int stack = 1;
				while (innerE.InnerException != null) 
				{
					if (innerE.TargetSite == null)
					{
						reportingModule = "unknown";
					}
					else
					{
						reportingModule = innerE.TargetSite.DeclaringType.FullName + ":" + innerE.TargetSite.Name;
					}
					debugWriter.WriteLine("exception " + reportingModule + " " + new System.Diagnostics.StackFrame(stack,true).GetFileLineNumber() + " - " + innerE.ToString());
					debugWriter.Flush();
					++stack;
					innerE = innerE.InnerException;
				}
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				returnCode = eReturnCode.severe;
			}
			_foundValues = false;
			_foundIntransitValues = false;
			return returnCode;
		}
		
		protected eReturnCode AddCurrentData(eVariableType aVariableType, int currNodeRID, int currDate, int aFiscalYearWeek, int currVersionRID, int currStoreRID, 
			HistoryPlanValue[] values, object[] locks)
		{
			eReturnCode returnCode = eReturnCode.successful;
			switch (_currProcessingData)
			{
				case eVariableDataType.chainDailyHistory:
					returnCode = AddChainDailyHistory(currNodeRID, currDate, aFiscalYearWeek, values);
					break;
				case eVariableDataType.chainWeeklyHistory:
					returnCode = AddChainWeeklyHistory(currNodeRID, currDate, aFiscalYearWeek, currVersionRID, values, locks);
					break;
				case eVariableDataType.chainWeeklyForecast:
					returnCode = AddChainWeeklyForecast(currNodeRID, currDate, aFiscalYearWeek, currVersionRID, values, locks);
					break;
				case eVariableDataType.storeDailyHistory:
					if (aVariableType == eVariableType.Intransit)
					{
						returnCode = AddStoreIntransit(currNodeRID, currDate, aFiscalYearWeek, currStoreRID, values);
					}
					else
					{
						returnCode = AddStoreDailyHistory(currNodeRID, currDate, aFiscalYearWeek, currStoreRID, values);
					}
					break;
				case eVariableDataType.storeWeeklyHistory:
					returnCode = AddStoreWeeklyHistory(currNodeRID, currDate, aFiscalYearWeek, currVersionRID, currStoreRID, values, locks);
					break;
				case eVariableDataType.storeWeeklyForecast:
					returnCode = AddStoreWeeklyForecast(currNodeRID, currDate, aFiscalYearWeek, currVersionRID, currStoreRID, values, locks);
					break;
			}
			++_recordsNotCommitted;
			return returnCode;
		}

		protected eReturnCode AddChainDailyHistory(int nodeRID, int date, int aFiscalYearWeek, HistoryPlanValue[] values)
		{
			eReturnCode returnCode = eReturnCode.successful;
			int tableNumber = nodeRID%_numberVariableTypes;
			if (tableNumber != _chainDailyHistoryTableNumber)
			{
				if (_chainDailyHistoryTableNumber != -1)
				{
					_tableManager[GetTableKeyValue(eVariableDataType.chainDailyHistory,_chainDailyHistoryTableNumber)] = _chainDailyHistoryRecords;
				}
				_chainDailyHistoryRecords = (Hashtable)_tableManager[GetTableKeyValue(eVariableDataType.chainDailyHistory,tableNumber)];
				_chainDailyHistoryTableNumber = tableNumber;
			}
			HistoryPlanRecord hpr = new HistoryPlanRecord();
			hpr.NodeRID = nodeRID;
			hpr.TimeID = date;
			hpr.FiscalYearWeek = aFiscalYearWeek;
			hpr.VersionRID = Include.FV_ActualRID;
			hpr.StoreRID = Include.NoRID;
			hpr.Values = values;
			hpr.Locks = null;
			AddHistoryPlanRecord(_chainDailyHistoryRecords, hpr);
			return returnCode;
		}

		protected eReturnCode AddChainWeeklyHistory(int nodeRID, int date, int aFiscalYearWeek, int versionRID, HistoryPlanValue[] values, object[] locks)
		{
			eReturnCode returnCode = eReturnCode.successful;
			int tableNumber = nodeRID%_numberVariableTypes;
			if (tableNumber != _chainWeeklyHistoryTableNumber)
			{
				if (_chainWeeklyHistoryTableNumber != -1)
				{
					_tableManager[GetTableKeyValue(eVariableDataType.chainWeeklyHistory, _chainWeeklyHistoryTableNumber)] = _chainWeeklyHistoryRecords;
				}
				_chainWeeklyHistoryRecords = (Hashtable)_tableManager[GetTableKeyValue(eVariableDataType.chainWeeklyHistory,tableNumber)];
				_chainWeeklyHistoryTableNumber = tableNumber;
			}
			HistoryPlanRecord hpr = new HistoryPlanRecord();
			hpr.NodeRID = nodeRID;
			hpr.TimeID = date;
			hpr.FiscalYearWeek = aFiscalYearWeek;
			hpr.VersionRID = versionRID;
			hpr.StoreRID = Include.NoRID;
			hpr.Values = values;
			hpr.Locks = locks;
			AddHistoryPlanRecord(_chainWeeklyHistoryRecords, hpr);
			return returnCode;
		}

		protected eReturnCode AddChainWeeklyForecast(int nodeRID, int date, int aFiscalYearWeek, int versionRID, HistoryPlanValue[] values, object[] locks)
		{
			eReturnCode returnCode = eReturnCode.successful;
			int tableNumber = nodeRID%_numberVariableTypes;
			if (tableNumber != _chainWeeklyForecastTableNumber)
			{
				if (_chainWeeklyForecastTableNumber != -1)
				{
					_tableManager[GetTableKeyValue(eVariableDataType.chainWeeklyForecast, _chainWeeklyForecastTableNumber)] = _chainWeeklyForecastRecords;
				}
				_chainWeeklyForecastRecords = (Hashtable)_tableManager[GetTableKeyValue(eVariableDataType.chainWeeklyForecast,tableNumber)];
				_chainWeeklyForecastTableNumber = tableNumber;
			}
			HistoryPlanRecord hpr = new HistoryPlanRecord();
			hpr.NodeRID = nodeRID;
			hpr.TimeID = date;
			hpr.FiscalYearWeek = aFiscalYearWeek;
			hpr.VersionRID = versionRID;
			hpr.StoreRID = Include.NoRID;
			hpr.Values = values;
			hpr.Locks = locks;
			AddHistoryPlanRecord(_chainWeeklyForecastRecords, hpr);
			return returnCode;
		}

		protected eReturnCode AddStoreDailyHistory(int nodeRID,	int date, int aFiscalYearWeek, int storeRID, HistoryPlanValue[] values)
		{
			eReturnCode returnCode = eReturnCode.successful;
			int tableNumber = nodeRID%_numberVariableTypes;
			if (tableNumber != _storeDailyHistoryTableNumber)
			{
				if (_storeDailyHistoryTableNumber != -1)
				{
					_tableManager[GetTableKeyValue(eVariableDataType.storeDailyHistory, _storeDailyHistoryTableNumber)] = _storeDailyHistoryRecords;
				}
				_storeDailyHistoryRecords = (Hashtable)_tableManager[GetTableKeyValue(eVariableDataType.storeDailyHistory, tableNumber)];
				_storeDailyHistoryTableNumber = tableNumber;
			}
			HistoryPlanRecord hpr = new HistoryPlanRecord();
			hpr.NodeRID = nodeRID;
			hpr.TimeID = date;
			hpr.FiscalYearWeek = aFiscalYearWeek;
			hpr.VersionRID = Include.FV_ActualRID;
			hpr.StoreRID = storeRID;
			hpr.Values = values;
			hpr.Locks = null;
            // Begin TT#155 - JSmith - Size Curve Method 
            // Begin TT#739-MD - JSmith - Delete Stores
            //string[] fields = MIDstringTools.Split(_currNodeID, _levelDelimiter, true);
            //if (_useNewTables &&
            //    fields.Length == 3)
            //{
            //    hpr.StyleRID = Convert.ToInt32(_nodes[fields[0]]);
            //    hpr.IsSizeHistory = true;
            //    // look up color code RID
            //    //object key = _colorCodes[fields[1]];
            //    int key;
            //    // color code not found so retrieve it and add to the list
            //    //if (key == null)
            //    if (!_colorCodes.TryGetValue(fields[1], out key))
            //    {
            //        ColorCodeProfile ccp = _SAB.HierarchyServerSession.GetColorCodeProfile(fields[1]);
            //        key = ccp.Key;
            //        if (ccp.Key != Include.NoRID)
            //        {
            //            _colorCodes.Add(fields[1], ccp.Key);
            //        }
            //    }
            //    hpr.ColorCodeRID = Convert.ToInt32(key);
            //    // look up size code RID
            //    //key = _sizeCodes[fields[2]];
            //    // size code not found so retrieve it and add to the list
            //    //if (key == null)
            //    if (!_sizeCodes.TryGetValue(fields[2], out key))
            //    {
            //        SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(fields[2]);
            //        key = scp.Key;
            //        if (scp.Key != Include.NoRID)
            //        {
            //            _colorCodes.Add(fields[2], scp.Key);
            //        }
            //    }
            //    hpr.SizeCodeRID = Convert.ToInt32(key);
            //}
            // End TT#739-MD - JSmith - Delete Stores
            // End TT#155
			AddHistoryPlanRecord(_storeDailyHistoryRecords, hpr);
			return returnCode;
		}

		protected eReturnCode AddStoreWeeklyHistory(int nodeRID, int date, int aFiscalYearWeek, int versionRID, int storeRID, HistoryPlanValue[] values, object[] locks)
		{
			eReturnCode returnCode = eReturnCode.successful;
			int tableNumber = nodeRID%_numberVariableTypes;
			if (tableNumber != _storeWeeklyHistoryTableNumber)
			{
				if (_storeWeeklyHistoryTableNumber != -1)
				{
					_tableManager[GetTableKeyValue(eVariableDataType.storeWeeklyHistory,_storeWeeklyHistoryTableNumber)] = _storeWeeklyHistoryRecords;
				}
				_storeWeeklyHistoryRecords = (Hashtable)_tableManager[GetTableKeyValue(eVariableDataType.storeWeeklyHistory,tableNumber)];
				_storeWeeklyHistoryTableNumber = tableNumber;
			}
			HistoryPlanRecord hpr = new HistoryPlanRecord();
			hpr.NodeRID = nodeRID;
			hpr.TimeID = date;
			hpr.FiscalYearWeek = aFiscalYearWeek;
			hpr.VersionRID = versionRID;
			hpr.StoreRID = storeRID;
			hpr.Values = values;
			hpr.Locks = locks;
            // Begin TT#155 - JSmith - Size Curve Method
            // Begin TT#739-MD - JSmith - Delete Stores
            //string[] fields = MIDstringTools.Split(_currNodeID, _levelDelimiter, true);
            //if (_useNewTables && 
            //    fields.Length == 3)
            //{
            //    hpr.StyleRID = Convert.ToInt32(_nodes[fields[0]]);
            //    hpr.IsSizeHistory = true;
            //    // look up color code RID
            //    object key = _colorCodes[fields[1]];
            //    // color code not found so retrieve it and add to the list
            //    if (key == null)
            //    {
            //        ColorCodeProfile ccp = _SAB.HierarchyServerSession.GetColorCodeProfile(fields[1]);
            //        key = ccp.Key;
            //        if (ccp.Key != Include.NoRID)
            //        {
            //            _colorCodes.Add(fields[1], ccp.Key);
            //        }
            //    }
            //    hpr.ColorCodeRID = Convert.ToInt32(key);
            //    // look up size code RID
            //    key = _sizeCodes[fields[2]];
            //    // size code not found so retrieve it and add to the list
            //    if (key == null)
            //    {
            //        SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(fields[1]);
            //        key = scp.Key;
            //        if (scp.Key != Include.NoRID)
            //        {
            //            _colorCodes.Add(fields[2], scp.Key);
            //        }
            //    }
            //    hpr.SizeCodeRID = Convert.ToInt32(key);
            //}
            // End TT#739-MD - JSmith - Delete Stores
            // End TT#155
			AddHistoryPlanRecord(_storeWeeklyHistoryRecords, hpr);
			return returnCode;
		}

		protected eReturnCode AddStoreWeeklyForecast(int nodeRID, int date, int aFiscalYearWeek, int versionRID, int storeRID, HistoryPlanValue[] values, object[] locks)
		{
			eReturnCode returnCode = eReturnCode.successful;
			int tableNumber = nodeRID%_numberVariableTypes;
			if (tableNumber != _storeWeeklyForecastTableNumber)
			{
				if (_storeWeeklyForecastTableNumber != -1)
				{
					_tableManager[GetTableKeyValue(eVariableDataType.storeWeeklyForecast,_storeWeeklyForecastTableNumber)] = _storeWeeklyForecastRecords;
				}
				_storeWeeklyForecastRecords = (Hashtable)_tableManager[GetTableKeyValue(eVariableDataType.storeWeeklyForecast,tableNumber)];
				_storeWeeklyForecastTableNumber = tableNumber;
			}
			HistoryPlanRecord hpr = new HistoryPlanRecord();
			hpr.NodeRID = nodeRID;
			hpr.TimeID = date;
			hpr.FiscalYearWeek = aFiscalYearWeek;
			hpr.VersionRID = versionRID;
			hpr.StoreRID = storeRID;
			hpr.Values = values;
			hpr.Locks = locks;
			AddHistoryPlanRecord(_storeWeeklyForecastRecords, hpr);
			return returnCode;
		}

		protected eReturnCode AddStoreIntransit(int nodeRID, int date, int aFiscalYearWeek, int storeRID, HistoryPlanValue[] values)
		{
			eReturnCode returnCode = eReturnCode.successful;
			int tableNumber = nodeRID%_numberVariableTypes;
			if (tableNumber != _storeIntransitTableNumber)
			{
				if (_storeIntransitTableNumber != -1)
				{
					_tableManager[GetTableKeyValue(eVariableDataType.storeExternalIntransit,_storeIntransitTableNumber)] = _storeIntransitRecords;
				}
				_storeIntransitRecords = (Hashtable)_tableManager[GetTableKeyValue(eVariableDataType.storeExternalIntransit,tableNumber)];
				_storeIntransitTableNumber = tableNumber;
			}
			HistoryPlanRecord hpr = new HistoryPlanRecord();
			hpr.NodeRID = nodeRID;
			hpr.TimeID = date;
			hpr.FiscalYearWeek = aFiscalYearWeek;
			hpr.VersionRID = Include.FV_ActualRID;
			hpr.StoreRID = storeRID;
			hpr.Values = values;
			hpr.Locks = null;
			string[] fields = MIDstringTools.Split(_currNodeID,_levelDelimiter,true);
			if (fields.Length == 3)
			{
				hpr.IsSizeIntransit = true;
			}
			AddHistoryPlanRecord(_storeIntransitRecords, hpr);
			return returnCode;
		}
		protected int GetTableKeyValue(eVariableDataType aVariableDataType, int aTableNumber)
		{
			return (int)aVariableDataType + aTableNumber;
		}

		protected int AddHistoryPlanRecord(Hashtable aVersionHash, HistoryPlanRecord aHistoryPlanRecord)
		{
			try
			{
				// the organization of the Hashtables are:
				// version
				//   time
				//     node
				//       store - values
				int returnCode = 0;
				HistoryPlanRecord values;
				Hashtable timeHash;
				Hashtable nodeHash;
				Hashtable storeHash;
				timeHash = (Hashtable)aVersionHash[aHistoryPlanRecord.VersionRID];
				if (timeHash == null)
				{
					timeHash = new Hashtable();
					nodeHash = new Hashtable();
					storeHash = new Hashtable();

					storeHash.Add(aHistoryPlanRecord.StoreRID,aHistoryPlanRecord);
					nodeHash.Add(aHistoryPlanRecord.NodeRID,storeHash);
					timeHash.Add(aHistoryPlanRecord.TimeID,nodeHash);
					aVersionHash.Add(aHistoryPlanRecord.VersionRID, timeHash);
				}
				else
				{
					nodeHash = (Hashtable)timeHash[aHistoryPlanRecord.TimeID];
					if (nodeHash == null)
					{
						nodeHash = new Hashtable();
						storeHash = new Hashtable();

						storeHash.Add(aHistoryPlanRecord.StoreRID,aHistoryPlanRecord);
						nodeHash.Add(aHistoryPlanRecord.NodeRID,storeHash);
						timeHash.Add(aHistoryPlanRecord.TimeID,nodeHash);
					}
					else
					{
						storeHash = (Hashtable)nodeHash[aHistoryPlanRecord.NodeRID];
						if (storeHash == null)
						{
							storeHash = new Hashtable();

							storeHash.Add(aHistoryPlanRecord.StoreRID,aHistoryPlanRecord);
							nodeHash.Add(aHistoryPlanRecord.NodeRID,storeHash);
						}
						else
						{
							values = (HistoryPlanRecord)storeHash[aHistoryPlanRecord.StoreRID];
							if (values == null)
							{
								storeHash.Add(aHistoryPlanRecord.StoreRID,aHistoryPlanRecord);
							}
							else
							{
								for (int i=0; i<aHistoryPlanRecord.Values.Length; i++)
								{
									HistoryPlanValue hpv = (HistoryPlanValue)aHistoryPlanRecord.Values.GetValue(i);
									if (hpv == null)
									{
										HistoryPlanValue newhpv = (HistoryPlanValue)values.Values.GetValue(i);
										if (newhpv != null)
										{
											aHistoryPlanRecord.Values.SetValue(newhpv,i);
										}
									}
								}
								storeHash[aHistoryPlanRecord.StoreRID] = aHistoryPlanRecord;
							}
						}
					}
				}

				return returnCode;
			}
			catch
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				throw;
			}

		}

		protected void WriteValues()
		{
			try
			{
				CommitAutoAddedNodes();
				foreach (DictionaryEntry table in _tableManager) 
				{
					eVariableDataType variableDataType = eVariableDataType.none;
					int tableID = (int)table.Key;
					Hashtable ht = (Hashtable)table.Value;
					if (ht.Count > 0)
					{
						// determine the type of table by comparing the table id to the enumeration
						foreach(int vdt in Enum.GetValues(typeof( eVariableDataType)))
						{
							if (tableID >= vdt)
							{
								variableDataType = (eVariableDataType)vdt;
							}
						}
				
						int tableNumber = tableID - (int)variableDataType;
						switch (variableDataType)
						{
							case eVariableDataType.chainWeeklyHistory:
								WriteChainWeeklyHistory(ht); 
								ht.Clear();
								break;
							case eVariableDataType.chainWeeklyForecast:
								WriteChainWeeklyForecast(ht);
								ht.Clear();
								break;
							case eVariableDataType.storeDailyHistory:
								WriteStoreDailyHistory(ht, tableNumber);
								ht.Clear();
								break;
							case eVariableDataType.storeWeeklyHistory:
								WriteStoreWeeklyHistory(ht, tableNumber);
								ht.Clear();
								break;
							case eVariableDataType.storeWeeklyForecast:
								WriteStoreWeeklyForecast(ht, tableNumber);
								ht.Clear();
								break;
							case eVariableDataType.storeExternalIntransit:
								WriteStoreIntransit(ht, tableNumber);
								ht.Clear();
								break;
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}

		
		protected eReturnCode WriteChainWeeklyHistory(Hashtable aVersionHash)
		{
			eReturnCode returnCode = eReturnCode.successful;
			try
			{
                // Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
                //VariablesData varData = new VariablesData();
                VariablesData varData = new VariablesData(_numberVariableTypes);
                // End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
				try
				{
					Hashtable newValues = new Hashtable();
					Hashtable newLocks = new Hashtable();
					// initialize XML document to send one week at a time
					varData.Variable_XMLInit();
					foreach (Hashtable timeHash in aVersionHash.Values)
					{
						foreach (Hashtable nodeHash in timeHash.Values)
						{
							bool computationItemLogged = false;
							foreach (Hashtable storeHash in nodeHash.Values)
							{
								foreach (HistoryPlanRecord hpr in storeHash.Values)
								{
									bool stockValueFound = false;
									bool nonStockValueFound = false;
									newValues.Clear();
									foreach (HistoryPlanValue hpv in hpr.Values)
									{
										if (hpv != null)
										{
											newValues.Add(hpv.VariableProfile, hpv.Value);
											if (hpv.VariableProfile.VariableType == eVariableType.BegStock ||
												hpv.VariableProfile.VariableType == eVariableType.EndStock)
											{
												stockValueFound = true;
											}
											else
											{
												nonStockValueFound = true;
											}
											if (_computationModel.ComputationModelFound &&
												!computationItemLogged)
											{
												BuildComputationItems(eComputationType.Chain, hpr.NodeRID, 
													hpr.FiscalYearWeek, hpr.VersionRID, hpv.VariableProfile.Key, 
													ref computationItemLogged);
											}
										}
									}
									varData.ChainWeek_Update_Insert(hpr.NodeRID, hpr.TimeID, hpr.VersionRID, newValues, newLocks, false);
									if (_rollChainUpHierarchy)
									{
										BuildRollupItems(eVariableDataType.chainWeeklyHistory, hpr.NodeRID, hpr.TimeID, Include.FV_ActualRID, nonStockValueFound, stockValueFound, false);
									}
								}
							}
						}
					}
					varData.OpenUpdateConnection();
					varData.ChainWeek_XMLUpdate(Include.FV_ActualRID, false);
					varData.CommitData();			
				}
				catch ( Exception ex )
				{
#if(DEBUG)
					StreamWriter debugWriter = new StreamWriter("error.txt", true);
					debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
					debugWriter.Flush();
					debugWriter.Close();
#endif
					_audit.Log_Exception(ex, sourceModule);
					returnCode = eReturnCode.severe;
				}
				finally
				{
					varData.CloseUpdateConnection();
				}
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				returnCode = eReturnCode.severe;
			}
			return returnCode;
		}

		protected eReturnCode WriteChainWeeklyForecast(Hashtable aVersionHash)
		{
			eReturnCode returnCode = eReturnCode.successful;
			try
			{
                // Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
                //VariablesData varData = new VariablesData();
				VariablesData varData = new VariablesData(_numberVariableTypes);
                // End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
				try
				{
					Hashtable newValues = new Hashtable();
					Hashtable newLocks = new Hashtable();
					// initialize XML document to send one week at a time
					varData.Variable_XMLInit();
					foreach (Hashtable timeHash in aVersionHash.Values)
					{
						foreach (Hashtable nodeHash in timeHash.Values)
						{
							bool computationItemLogged = false;
							foreach (Hashtable storeHash in nodeHash.Values)
							{
								foreach (HistoryPlanRecord hpr in storeHash.Values)
								{
									bool stockValueFound = false;
									bool nonStockValueFound = false;
									newValues.Clear();
									foreach (HistoryPlanValue hpv in hpr.Values)
									{
										if (hpv != null)
										{
											newValues.Add(hpv.VariableProfile, hpv.Value);
											if (hpv.VariableProfile.VariableType == eVariableType.BegStock ||
												hpv.VariableProfile.VariableType == eVariableType.EndStock)
											{
												stockValueFound = true;
											}
											else
											{
												nonStockValueFound = true;
											}
											if (_computationModel.ComputationModelFound &&
												!computationItemLogged)
											{
												BuildComputationItems(eComputationType.Chain, hpr.NodeRID, 
													hpr.FiscalYearWeek, hpr.VersionRID, hpv.VariableProfile.Key, 
													ref computationItemLogged);
											}
										}
									}
									varData.ChainWeek_Update_Insert(hpr.NodeRID, hpr.TimeID, hpr.VersionRID, newValues, newLocks, false);
									if (_rollChainUpHierarchy)
									{
										BuildRollupItems(eVariableDataType.chainWeeklyForecast, hpr.NodeRID, hpr.TimeID, hpr.VersionRID, nonStockValueFound, stockValueFound, false);
									}
								}
							}
						}
					}
					// use any version other than actual to perform forcast update
					varData.OpenUpdateConnection();
					varData.ChainWeek_XMLUpdate(Include.FV_ActionRID, false);
					varData.CommitData();
				}
				catch ( Exception ex )
				{
#if(DEBUG)
					StreamWriter debugWriter = new StreamWriter("error.txt", true);
					debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
					debugWriter.Flush();
					debugWriter.Close();
#endif
					_audit.Log_Exception(ex, sourceModule);
					returnCode = eReturnCode.severe;
				}
				finally
				{
					varData.CloseUpdateConnection();
				}
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				returnCode = eReturnCode.severe;
			}
			return returnCode;
		}

		protected eReturnCode WriteStoreDailyHistory(Hashtable aVersionHash, int aTableNumber)
		{
			eReturnCode returnCode = eReturnCode.successful;
            // Begin TT#155 - JSmith - Size Curve Method
            // begin TT#173 - JEllis - Provide database container for large data collections
            //Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>> sizeVariableHistoryDictionary;
            Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>> sizeStyleHistoryDictionary;
            // end TT#173 - JEllist - Provide database container for large data collections
            // End TT#155
			try
			{
                // Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
                //VariablesData varData = new VariablesData();
				VariablesData varData = new VariablesData(_numberVariableTypes);
                // End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
                // Begin TT#155 - JSmith - Size Curve Method
                // begin TT#173 - JEllis - Provide database container for holding large data collections
                //sizeVariableHistoryDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>>();
                sizeStyleHistoryDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>>();
                // End TT#155
				try
				{
					Hashtable newValues = new Hashtable();
					Hashtable newLocks = new Hashtable();
					// initialize XML document to send one week at a time
					varData.Variable_XMLInit();
					foreach (Hashtable timeHash in aVersionHash.Values)
					{
						foreach (Hashtable nodeHash in timeHash.Values)
						{
							bool computationItemLogged = false;
							foreach (Hashtable storeHash in nodeHash.Values)
							{
								foreach (HistoryPlanRecord hpr in storeHash.Values)
								{
									bool stockValueFound = false;
									bool nonStockValueFound = false;
									newValues.Clear();
									foreach (HistoryPlanValue hpv in hpr.Values)
									{
										if (hpv != null)
										{
                                            // Begin TT#739-MD - JSmith - Delete Stores
                                            //// Begin TT#155 - JSmith - Size Curve Method
                                            //// Daily stock must be written to both
                                            //if (_useNewTables &&
                                            //    hpr.IsSizeHistory)
                                            //{
                                            //    // begin TT#173 - JEllis -  Provide database container to hold large data collections
                                            //    //AddSizeHistoryValue(sizeVariableHistoryDictionary, hpv.VariableProfile.Key, hpr.StyleRID, hpr.TimeID,
                                            //    //    hpr.ColorCodeRID, hpr.SizeCodeRID, hpr.StoreRID, hpv.Value);
                                            //    //Begin TT#467 - JSmith - EnqueueConflictException during size history load
                                            //    //AddSizeHistoryValue(sizeStyleHistoryDictionary, hpr.StyleRID, hpv.VariableProfile.Key, hpr.TimeID,
                                            //    //    hpr.ColorCodeRID, hpr.SizeCodeRID, hpr.StoreRID, hpv.Value);
                                            //    AddSizeHistoryValue(sizeStyleHistoryDictionary, hpr.StyleRID, hpv.VariableProfile.Key, hpr.TimeID,
                                            //        hpr.ColorCodeRID, hpr.SizeCodeRID, hpr.StoreRID, hpv.Value, hpr.NodeRID);
                                            //    //End TT#467
                                            //    // end TT#173 - JEllis - Provide database container to hold large data collections
                                            //    if (hpv.VariableProfile.VariableType != eVariableType.BegStock &&
                                            //        hpv.VariableProfile.VariableType != eVariableType.EndStock)
                                            //    {
                                            //        continue;
                                            //    }
                                            //}
                                            //// End TT#155
                                            // End TT#739-MD - JSmith - Delete Stores

											//Begin TT#575 - JSmith - HistoryPlanLoad does not save Store Weekly Values when "UseSizeStoreProcedureRead" config setting is true
											//if (_useOldTables)
                                            // Begin TT#739-MD - JSmith - Delete Stores
                                            //if (_useOldTables ||
                                            //    !hpr.IsSizeHistory)
                                            // End TT#739-MD - JSmith - Delete Stores
											//End TT#575 - JSmith - HistoryPlanLoad does not save Store Weekly Values when "UseSizeStoreProcedureRead" config setting is true
											{
                                                newValues.Add(hpv.VariableProfile, hpv.Value);
                                                if (hpv.VariableProfile.VariableType == eVariableType.BegStock ||
                                                    hpv.VariableProfile.VariableType == eVariableType.EndStock)
                                                {
                                                    stockValueFound = true;
                                                }
                                                else
                                                {
                                                    nonStockValueFound = true;
                                                }
                                                if (_computationModel.ComputationModelFound &&
                                                    !computationItemLogged)
                                                {
                                                    BuildComputationItems(eComputationType.Store, hpr.NodeRID,
                                                        hpr.FiscalYearWeek, hpr.VersionRID, hpv.VariableProfile.Key,
                                                        ref computationItemLogged);
                                                }
                                            }
										}
									}
                                    // Begin TT#155 - JSmith - Size Curve Method
                                    if (newValues.Count > 0)
                                    {
                                    // End TT#155
                                        varData.StoreDailyHistory_Update_Insert(hpr.NodeRID, hpr.TimeID, hpr.StoreRID, newValues);
                                        if (_rollStoreDailyToWeekly || _rollStoreDailyUpHierarchy)
                                        {
                                            BuildRollupItems(eVariableDataType.storeDailyHistory, hpr.NodeRID, hpr.TimeID, Include.FV_ActualRID, nonStockValueFound, stockValueFound, false);
                                        }
                                    // Begin TT#155 - JSmith - Size Curve Method
                                    }
                                    // End TT#155
								}
							}
						}
					}
					varData.OpenUpdateConnection();
					varData.StoreDaily_XMLUpdate(aTableNumber);
					varData.CommitData();

                    // Begin TT#739-MD - JSmith - Delete Stores
                    //// Begin TT#155 - JSmith - Size Curve Method
                    //// begin TT#173 - JEllis - Provide database container for large data collections
                    ////if (sizeVariableHistoryDictionary.Count > 0)
                    ////{
                    ////    WriteSizeHistoryValues(sizeVariableHistoryDictionary, eSQLTimeIdType.TimeIdIsDaily);
                    ////}
                    //if (sizeStyleHistoryDictionary.Count > 0)
                    //{
                    //    WriteSizeHistoryValues(sizeStyleHistoryDictionary, eSQLTimeIdType.TimeIdIsDaily);
                    //}
                    //// end TT#173 - JEllis - Provide database container for large data collections
                    //// End TT#155
                    // End TT#739-MD - JSmith - Delete Stores
				}
				catch ( Exception ex )
				{
#if(DEBUG)
					StreamWriter debugWriter = new StreamWriter("error.txt", true);
					debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
					debugWriter.Flush();
					debugWriter.Close();
#endif
					_audit.Log_Exception(ex, sourceModule);
					returnCode = eReturnCode.severe;
				}
				finally
				{
					varData.CloseUpdateConnection();
				}
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				returnCode = eReturnCode.severe;
			}
			return returnCode;
		}

		protected eReturnCode WriteStoreWeeklyHistory(Hashtable aVersionHash, int aTableNumber)
		{
			eReturnCode returnCode = eReturnCode.successful;
            // Begin TT#155 - JSmith - Size Curve Method
            // begin TT#173 - JEllis - Provide database container for large data collections
            //Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int ,Dictionary<int, ArrayList>>>>> sizeVariableHistoryDictionary;
            Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>> sizeStyleHistoryDictionary;
            // end TT#173 - JEllis - Provide database container for large data collections
            // End TT#155
			try
			{
                // Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
                //VariablesData varData = new VariablesData();
				VariablesData varData = new VariablesData(_numberVariableTypes);
                // End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
                // Begin TT#155 - JSmith - Size Curve Method
                // begin TT#173 - JEllis - Provide database container to hold large data collections
                // sizeVariableHistoryDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>>();
                sizeStyleHistoryDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>>();
                // end TT#173 - JEllis - Provide database container to hold large data collections
                // End TT#155
				try
				{
					Hashtable newValues = new Hashtable();
					Hashtable newLocks = new Hashtable();
					// initialize XML document to send one week at a time
					varData.Variable_XMLInit();
					foreach (Hashtable timeHash in aVersionHash.Values)
					{
						foreach (Hashtable nodeHash in timeHash.Values)
						{
							bool computationItemLogged = false;
							foreach (Hashtable storeHash in nodeHash.Values)
							{
								foreach (HistoryPlanRecord hpr in storeHash.Values)
								{
									bool stockValueFound = false;
									bool nonStockValueFound = false;
									newValues.Clear();
									foreach (HistoryPlanValue hpv in hpr.Values)
									{
										if (hpv != null)
										{
                                            // Begin TT#739-MD - JSmith - Delete Stores
                                            //// Begin TT#155 - JSmith - Size Curve Method
                                            //if (_useNewTables && 
                                            //    hpr.IsSizeHistory)
                                            //{
                                            //    // begin TT#173 - JEllis - Provide database container to hold large data collections
                                            //    //AddSizeHistoryValue(sizeVariableHistoryDictionary, hpv.VariableProfile.Key, hpr.StyleRID, hpr.TimeID,
                                            //    //    hpr.ColorCodeRID, hpr.SizeCodeRID, hpr.StoreRID, hpv.Value);
                                            //    //Begin TT#467 - JSmith - EnqueueConflictException during size history load
                                            //    //AddSizeHistoryValue(sizeStyleHistoryDictionary, hpr.StyleRID, hpv.VariableProfile.Key, hpr.TimeID,
                                            //    //     hpr.ColorCodeRID, hpr.SizeCodeRID, hpr.StoreRID, hpv.Value);
                                            //    AddSizeHistoryValue(sizeStyleHistoryDictionary, hpr.StyleRID, hpv.VariableProfile.Key, hpr.TimeID,
                                            //         hpr.ColorCodeRID, hpr.SizeCodeRID, hpr.StoreRID, hpv.Value, hpr.NodeRID);
                                            //    //End TT#467
                                            //    // end TT#173 - JEllist - Provide database container to hold large data collections
                                            //}
                                            //// Begin TT#297 - Posted size values not showing in OTS Forecast Reivew
                                            ////else
                                            ////{
                                            //// End TT#297
                                            //// End TT#155
                                            // End TT#739-MD - JSmith - Delete Stores
											//Begin TT#575 - JSmith - HistoryPlanLoad does not save Store Weekly Values when "UseSizeStoreProcedureRead" config setting is true
											//if (_useOldTables)
                                            // Begin TT#739-MD - JSmith - Delete Stores
                                            //if (_useOldTables ||
                                            //    !hpr.IsSizeHistory)
                                            // End TT#739-MD - JSmith - Delete Stores
											//End TT#575 - JSmith - HistoryPlanLoad does not save Store Weekly Values when "UseSizeStoreProcedureRead" config setting is true
                                            {
                                                newValues.Add(hpv.VariableProfile, hpv.Value);
                                                if (hpv.VariableProfile.VariableType == eVariableType.BegStock ||
                                                    hpv.VariableProfile.VariableType == eVariableType.EndStock)
                                                {
                                                    stockValueFound = true;
                                                }
                                                else
                                                {
                                                    nonStockValueFound = true;
                                                }
                                                if (_computationModel.ComputationModelFound &&
                                                    !computationItemLogged)
                                                {
                                                    BuildComputationItems(eComputationType.Store, hpr.NodeRID,
                                                        hpr.FiscalYearWeek, hpr.VersionRID, hpv.VariableProfile.Key,
                                                        ref computationItemLogged);
                                                }
                                            }
                                            // Begin TT#155 - JSmith - Size Curve Method
                                            // Begin TT#297 - Posted size values not showing in OTS Forecast Reivew
                                            //}
                                            // End TT#297
                                            // End TT#155
										}
									}
                                    // Begin TT#155 - JSmith - Size Curve Method
                                    if (newValues.Count > 0)
                                    {
                                    // End TT#155
                                        varData.StoreWeek_Update_Insert(hpr.NodeRID, hpr.TimeID, hpr.VersionRID, hpr.StoreRID, newValues, newLocks, false);
                                        if (_rollStoreWeeklyUpHierarchy || _rollStoreToChain)
                                        {
                                            BuildRollupItems(eVariableDataType.storeWeeklyHistory, hpr.NodeRID, hpr.TimeID, Include.FV_ActualRID, nonStockValueFound, stockValueFound, false);
                                        }
                                    // Begin TT#155 - JSmith - Size Curve Method
                                    }
                                    // End TT#155
								}
							}
						}
					}
					varData.OpenUpdateConnection();
                    // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
                    //varData.StoreWeek_XMLUpdate(Include.FV_ActualRID, aTableNumber, false);
                    varData.StoreWeek_XMLUpdate(Include.FV_ActualRID, aTableNumber);
                    // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
					varData.CommitData();

                    // Begin TT#739-MD - JSmith - Delete Stores
                    //// Begin TT#155 - JSmith - Size Curve Method
                    //// begin TT#173 - JEllis - Provide database container for large data collections
                    ////if (sizeVariableHistoryDictionary.Count > 0)
                    ////{
                    ////    WriteSizeHistoryValues(sizeVariableHistoryDictionary, eSQLTimeIdType.TimeIdIsWeekly);
                    ////}
                    //if (sizeStyleHistoryDictionary.Count > 0)
                    //{
                    //    WriteSizeHistoryValues(sizeStyleHistoryDictionary, eSQLTimeIdType.TimeIdIsWeekly);
                    //}
                    //// end TT#173 - JEllist - Provide database container for large data collections
                    //// End TT#155
                    // End TT#739-MD - JSmith - Delete Stores
				}
				catch ( Exception ex )
				{
#if(DEBUG)
					StreamWriter debugWriter = new StreamWriter("error.txt", true);
					debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
					debugWriter.Flush();
					debugWriter.Close();
#endif
					_audit.Log_Exception(ex, sourceModule);
					returnCode = eReturnCode.severe;
				}
				finally
				{
					varData.CloseUpdateConnection();
				}
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				returnCode = eReturnCode.severe;
			}
			return returnCode;
		}

		protected eReturnCode WriteStoreWeeklyForecast(Hashtable aVersionHash, int aTableNumber)
		{
			eReturnCode returnCode = eReturnCode.successful;
			try
			{
                // Begin TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
                //VariablesData varData = new VariablesData();
				VariablesData varData = new VariablesData(_numberVariableTypes);
                // End TT#3259 - JSmith - Modify how variable values are updated to not use temporary tables
				try
				{
					Hashtable newValues = new Hashtable();
					Hashtable newLocks = new Hashtable();
					// initialize XML document to send one week at a time
					varData.Variable_XMLInit();
					foreach (Hashtable timeHash in aVersionHash.Values)
					{
						foreach (Hashtable nodeHash in timeHash.Values)
						{
							bool computationItemLogged = false;
							foreach (Hashtable storeHash in nodeHash.Values)
							{
								foreach (HistoryPlanRecord hpr in storeHash.Values)
								{
									bool stockValueFound = false;
									bool nonStockValueFound = false;
									newValues.Clear();
									foreach (HistoryPlanValue hpv in hpr.Values)
									{
										if (hpv != null)
										{
											newValues.Add(hpv.VariableProfile, hpv.Value);
											if (hpv.VariableProfile.VariableType == eVariableType.BegStock ||
												hpv.VariableProfile.VariableType == eVariableType.EndStock)
											{
												stockValueFound = true;
											}
											else
											{
												nonStockValueFound = true;
											}
											if (_computationModel.ComputationModelFound &&
												!computationItemLogged)
											{
												BuildComputationItems(eComputationType.Store, hpr.NodeRID, 
													hpr.FiscalYearWeek, hpr.VersionRID, hpv.VariableProfile.Key, 
													ref computationItemLogged);
											}
										}
									}
									varData.StoreWeek_Update_Insert(hpr.NodeRID, hpr.TimeID, hpr.VersionRID, hpr.StoreRID, newValues, newLocks, false);
									if (_rollStoreWeeklyUpHierarchy || _rollStoreToChain)
									{
										BuildRollupItems(eVariableDataType.storeWeeklyForecast, hpr.NodeRID, hpr.TimeID, hpr.VersionRID, nonStockValueFound, stockValueFound, false);
									}
								}
							}
						}
					}
					// use any version other than actual to perform forcast update 
					varData.OpenUpdateConnection();
                    // Begin TT#3373 - JSmith - Save Store Forecast receive DBNull error
                    //varData.StoreWeek_XMLUpdate(Include.FV_ActionRID, aTableNumber, false);
                    varData.StoreWeek_XMLUpdate(Include.FV_ActionRID, aTableNumber);
                    // End TT#3373 - JSmith - Save Store Forecast receive DBNull error
					varData.CommitData();
				}
				catch ( Exception ex )
				{
#if(DEBUG)
					StreamWriter debugWriter = new StreamWriter("error.txt", true);
					debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
					debugWriter.Flush();
					debugWriter.Close();
#endif
					_audit.Log_Exception(ex, sourceModule);
					returnCode = eReturnCode.severe;
				}
				finally
				{
					varData.CloseUpdateConnection();
				}
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				returnCode = eReturnCode.severe;
			}
			return returnCode;
		}

		protected eReturnCode WriteStoreIntransit(Hashtable aVersionHash, int aTableNumber)
		{
			eReturnCode returnCode = eReturnCode.successful;
			try
			{
				Intransit intransitData = new Intransit();
				try
				{
					intransitData.OpenUpdateConnection();
					foreach (Hashtable timeHash in aVersionHash.Values)
					{
						foreach (Hashtable nodeHash in timeHash.Values)
						{
							int nodeRID = Include.NoRID;
							int timeID = 0;
							bool isSizeIntransit = false;
							foreach (Hashtable storeHash in nodeHash.Values)
							{
								foreach (HistoryPlanRecord hpr in storeHash.Values)
								{
									intransitData.SetExternal(hpr.NodeRID, hpr.TimeID, hpr.StoreRID, Convert.ToInt32(hpr.Values[0].Value));
									// all node and time IDs must be the same 
									nodeRID = hpr.NodeRID;
									timeID = hpr.TimeID;
									isSizeIntransit = hpr.IsSizeIntransit;
								}
								if (_rollIntransit)
								{
									BuildRollupItems(eVariableDataType.storeExternalIntransit, nodeRID, timeID, Include.NoRID, true, false, false);
								}
								if (isSizeIntransit)
								{
									BuildDummyColorRollupItems(nodeRID, timeID);
								}
							}
						}
					}
					intransitData.CommitData();
				}
				catch ( Exception ex )
				{
					_audit.Log_Exception(ex, sourceModule);
					returnCode = eReturnCode.severe;
				}
				finally
				{
					intransitData.CloseUpdateConnection();
				}
			}
			catch ( Exception ex )
			{
				_audit.Log_Exception(ex, sourceModule);
				returnCode = eReturnCode.severe;
			}
			return returnCode;
		}

        // Begin TT#739-MD - JSmith - Delete Stores
        //// Begin TT#155 - JSmith - Size Curve Method
        //// begin TT#173 - JEllis - Provide database container for large data collections
        //// NOTE: put the "style" dictionary first and the "variable" dictionary second so that the complete style can be processed together
        ////protected eReturnCode AddSizeHistoryValue(Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>> aSizeVariableHistoryDictionary,
        ////    int aVariableNumber, int aStyleRID, int aTimeID, int aColorCodeRID, int aSizeCodeRID, int aStoreRID, double aValue)
        ////Begin TT#467 - JSmith - EnqueueConflictException during size history load
        ////protected eReturnCode AddSizeHistoryValue(Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>> aSizeStyleHistoryDictionary,
        ////    int aStyleRID, int aVariableNumber, int aTimeID, int aColorCodeRID, int aSizeCodeRID, int aStoreRID, double aValue)
        //protected eReturnCode AddSizeHistoryValue(Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>> aSizeStyleHistoryDictionary,
        //    int aStyleRID, int aVariableNumber, int aTimeID, int aColorCodeRID, int aSizeCodeRID, int aStoreRID, double aValue, int aSizeNodeRID)
        ////End TT#467
        //    // end TT#173 - JEllis - Provide database container for large data collections
        //{
        //    eReturnCode returnCode = eReturnCode.successful;
        //    // begin TT#173 - JEllis - Provide database container for large data collections
        //    //Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>> sizeStyleHistoryDictionary;
        //    Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>> sizeVariableHistoryDictionary;
        //    // end TT#173 - JEllis - Provide database container for large data collections
        //    Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>> sizeTimeHistoryDictionary;
        //    Dictionary<int, Dictionary<int, ArrayList>> sizeColorCodeHistoryDictionary;
        //    Dictionary<int, ArrayList> sizeCodeHistoryDictionary;
        //    ArrayList storeValues;
        //    bool found = false;
        //    StoreSizeValue storeSizeValue;
        //    try
        //    {
        //        // begin TT#173 - JEllis - Provide database container for large data collections
        //        //found = aSizeVariableHistoryDictionary.TryGetValue(aVariableNumber, out sizeStyleHistoryDictionary);
        //        //if (!found)
        //        //{
        //        //    sizeStyleHistoryDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>();
        //        //    aSizeVariableHistoryDictionary.Add(aVariableNumber, sizeStyleHistoryDictionary);
        //        //}
        //        //
        //        //found = sizeStyleHistoryDictionary.TryGetValue(aStyleRID, out sizeTimeHistoryDictionary);
        //        //if (!found)
        //        //{
        //        //    sizeTimeHistoryDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>();
        //        //    sizeStyleHistoryDictionary.Add(aStyleRID, sizeTimeHistoryDictionary);
        //        //}
        //        found = aSizeStyleHistoryDictionary.TryGetValue(aStyleRID, out sizeVariableHistoryDictionary);
        //        if (!found)
        //        {
        //            sizeVariableHistoryDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>();
        //            aSizeStyleHistoryDictionary.Add(aStyleRID, sizeVariableHistoryDictionary);
        //        }
        //        found = sizeVariableHistoryDictionary.TryGetValue(aVariableNumber, out sizeTimeHistoryDictionary);
        //        if (!found)
        //        {
        //            sizeTimeHistoryDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>();
        //            sizeVariableHistoryDictionary.Add(aVariableNumber, sizeTimeHistoryDictionary);
        //        }
        //        // end TT#173 - JEllis - Provide database container for large data collections
        //        found = sizeTimeHistoryDictionary.TryGetValue(aTimeID, out sizeColorCodeHistoryDictionary);
        //        if (!found)
        //        {
        //            sizeColorCodeHistoryDictionary = new Dictionary<int, Dictionary<int, ArrayList>>();
        //            sizeTimeHistoryDictionary.Add(aTimeID, sizeColorCodeHistoryDictionary);
        //        }

        //        found = sizeColorCodeHistoryDictionary.TryGetValue(aColorCodeRID, out sizeCodeHistoryDictionary);
        //        if (!found)
        //        {
        //            sizeCodeHistoryDictionary = new Dictionary<int, ArrayList>();
        //            //sizeColorCodeHistoryDictionary.Add(aSizeCodeRID, sizeCodeHistoryDictionary);  // TT#173  Provide database container for large data collections
        //            sizeColorCodeHistoryDictionary.Add(aColorCodeRID, sizeCodeHistoryDictionary);   // TT#173  Provide database container for large data collections
        //        }

        //        found = sizeCodeHistoryDictionary.TryGetValue(aSizeCodeRID, out storeValues);
        //        if (!found)
        //        {
        //            storeValues = new ArrayList();
        //            sizeCodeHistoryDictionary.Add(aSizeCodeRID, storeValues);
        //        }

        //        //Begin TT#467 - JSmith - EnqueueConflictException during size history load
        //        //storeSizeValue = new StoreSizeValue(aStoreRID, aValue);
        //        storeSizeValue = new StoreSizeValue(aStoreRID, aValue, aSizeNodeRID);
        //        //End TT#467
        //        storeValues.Add(storeSizeValue);

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return returnCode;
        //}

        //// begin TT#173 - JEllis - Provide database container to hold large data collections
        ////protected eReturnCode WriteSizeHistoryValues(Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>> aSizeVariableHistoryDictionary,
        ////    eSQLTimeIdType aSQLTimeIdType)
        //protected eReturnCode WriteSizeHistoryValues(Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>> aSizeStyleHistoryDictionary,
        //    eSQLTimeIdType aSQLTimeIdType)
        //    // end TT#173 - JEllis - Provide database container to hold large data collections
        //{
        //    eReturnCode returnCode = eReturnCode.successful;
        //    //Begin TT#739-MD -jsobek -Delete Stores -History
        //    //StoreVariableHistoryBin svhb = null;  // TT#173  Provide database container for large data collections
        //    StoreHistoryVariableManager svhb = null;
        //    //End TT#739-MD -jsobek -Delete Stores -History
        //    int[] styleRIDVector;
        //    int[] storeRIDVector;
        //    double[] variableValueVector;
        //    int i;
        //    int userRID;
        //    SQL_TimeID SQL_TimeID;
        //    try
        //    {
        //        userRID = _SAB.ClientServerSession.UserRID;
        //        //Begin TT#739-MD -jsobek -Delete Stores -History
        //        //svhb = new StoreVariableHistoryBin(true, 0);
        //        svhb = new StoreHistoryVariableManager();
        //        //End TT#739-MD -jsobek -Delete Stores -History

        //        // begin TT#173 - JEllis - Provide database container for large data collections
        //        //foreach (KeyValuePair<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>> variableNumber in aSizeVariableHistoryDictionary)
        //        //{
        //        //    foreach (KeyValuePair<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>> styleRID in variableNumber.Value)
        //        //    {
        //        //        styleRIDVector = new int[1];
        //        //        styleRIDVector[0] = styleRID.Key;
        //        //        svhb.LockStyleNode(userRID, styleRIDVector);
        //        //        string databaseVariableName = ((VariableProfile)(_variables.VariableProfileList.FindKey(variableNumber.Key))).DatabaseColumnName;
        //        //        foreach (KeyValuePair<int, Dictionary<int, Dictionary<int, ArrayList>>> timeID in styleRID.Value)
        //        foreach (KeyValuePair<int, Dictionary<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>>> styleRID in aSizeStyleHistoryDictionary) 
        //        {
        //            styleRIDVector = new int[1];
        //            styleRIDVector[0] = styleRID.Key;
        //            //Begin TT#467 - JSmith - EnqueueConflictException during size history load
        //            //svhb.LockStyleNode(userRID, styleRIDVector);
        //            //End TT#467
        //            foreach (KeyValuePair<int, Dictionary<int, Dictionary<int, Dictionary<int, ArrayList>>>> variableNumber in styleRID.Value)
        //            {
        //                string databaseVariableName = ((VariableProfile)(_variables.VariableProfileList.FindKey(variableNumber.Key))).DatabaseColumnName;
        //                foreach (KeyValuePair<int, Dictionary<int, Dictionary<int, ArrayList>>> timeID in variableNumber.Value)
        //                // end TT#173 - JEllis - Provide database container for large data collections
        //                {
        //                    foreach (KeyValuePair<int, Dictionary<int, ArrayList>> colorRID in timeID.Value)
        //                    {
        //                        foreach (KeyValuePair<int, ArrayList> sizeRID in colorRID.Value)
        //                        {
        //                            storeRIDVector = new int[sizeRID.Value.Count];
        //                            variableValueVector = new double[sizeRID.Value.Count];
        //                            i = 0;
        //                            foreach (StoreSizeValue storeSizeValue in sizeRID.Value)
        //                            {
        //                                storeRIDVector[i] = storeSizeValue.StoreRID;
        //                                variableValueVector[i] = storeSizeValue.ColorSizeValue;
        //                                i++;   // TT#173  Provide database container for large data collections
        //                            }
        //                            SQL_TimeID = new SQL_TimeID(aSQLTimeIdType, timeID.Key);

        //                            //Begin TT#467 - JSmith - EnqueueConflictException during size history load
        //                            StoreSizeValue ssv = (StoreSizeValue)sizeRID.Value[0];
        //                            //ssv.SizeNodeRID
        //                            //Begin TT#467 - JSmith - EnqueueConflictException during size history load
        //                            int[] ColorCodeRIDVector = { colorRID.Key };
        //                            int[] SizeCodeRIDVector = { sizeRID.Key };
        //                            int[] sizeNodeRIDVector = { ssv.SizeNodeRID };
        //                            SQL_TimeID[] SQL_TimeIDVector = { SQL_TimeID };
        //                            svhb.LockTimeHnRIDNode(userRID, SQL_TimeIDVector, sizeNodeRIDVector, styleRIDVector, ColorCodeRIDVector, SizeCodeRIDVector);
        //                            //End TT#467

        //                            // begin TT#173  Provide database container for large data collections
        //                            // svhb.SetStoreColorSizeVariableValue((ushort)(variableNumber.Key), styleRID.Key, SQL_TimeID, colorRID.Key, sizeRID.Key, storeRIDVector, variableValueVector);
        //                            //svhb.SetStoreColorSizeVariableValue((ushort)(variableNumber.Key - 1), styleRID.Key, SQL_TimeID, colorRID.Key, sizeRID.Key, storeRIDVector, variableValueVector);
        //                            svhb.SetStoreVariableValue(databaseVariableName, styleRID.Key, SQL_TimeID, colorRID.Key, sizeRID.Key, storeRIDVector, variableValueVector);
        //                            // end TT#173  Provide database container for large data collections
        //                            //Begin TT#467 - JSmith - EnqueueConflictException during size history load
        //                            string message;
        //                            if (!svhb.Commit(out message))
        //                            {
        //                                returnCode = eReturnCode.severe;
        //                            }
        //                            svhb.UnLockTimeHnRID(SQL_TimeIDVector, styleRIDVector, ColorCodeRIDVector, SizeCodeRIDVector);
        //                            //svhb.UnLockTimeHnRIDNode(SQL_TimeIDVector, sizeNodeRIDVector, styleRIDVector, ColorCodeRIDVector, SizeCodeRIDVector);
        //                            //End TT#467
        //                        }
        //                    }
        //                }
        //                // begin TT#173 - JEllis - Provide database container for large data collections
        //                //svhb.Commit();
        //                //svhb.UnLockStyleNode(styleRIDVector);
        //            //}
        //            }
        //            //Begin TT#467 - JSmith - EnqueueConflictException during size history load
        //            //string message;
        //            //if (!svhb.Commit(out message))
        //            //{
        //            //    returnCode = eReturnCode.severe;
        //            //}
        //            //svhb.UnLockStyleNode(styleRIDVector);
        //            //End TT#467
        //            // end TT#173 - JEllis - Provide database container for large data collections
        //        }

        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    // begin TT#173  Provide database container for large data collections
        //    finally
        //    {
        //        if (svhb != null)
        //        {
        //            svhb.RemoveAllLocks();
        //        }
        //    }
        //    // end TT#173  Provide database container for large data collections
        //    return returnCode;
        //}
        //// End TT#155
        // End TT#739-MD - JSmith - Delete Stores

		protected bool BuildRollupItems(eVariableDataType aVariableDataType, int aNodeRID, int aTimeID, int aVersionRID, bool aNonStockValueFound, bool aStockValueFound,
			bool aRollAlternatesOnly)
		{
			try
			{
				switch (aVariableDataType)
				{
					case eVariableDataType.storeDailyHistory:
					{
						if (aTimeID != _currDayTimeID)
						{
							_currDayProfile = _cal.GetDay(aTimeID);
							_currDayTimeID = aTimeID;
						}
						if (_rollStoreDailyUpHierarchy)
						{
							if (aNonStockValueFound || aStockValueFound)
							{
								BuildHierarchyRollupItems(aVariableDataType, aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
							}
						}
						if (_rollStoreDailyToWeekly)
						{
							BuildDaysToWeeksRollupItems(aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
						}
						// don't roll weekly up hierarchy if did not roll day to week
						if (_rollStoreWeeklyUpHierarchy && _rollStoreDailyToWeekly)
						{
							if (aNonStockValueFound || aStockValueFound)
							{
								BuildHierarchyRollupItems(eVariableDataType.storeWeeklyHistory, aNodeRID, _currDayProfile.Week.Key, aVersionRID, aRollAlternatesOnly);
							}
						}
						// don't roll store to chain if did not roll day to week
						if (_rollStoreToChain && _rollStoreDailyToWeekly)
						{
							if (aNonStockValueFound || aStockValueFound)
							{
								BuildStoreToChainRollupItems(aNodeRID, _currDayProfile.Week.Key, aVersionRID, aRollAlternatesOnly);
							}
						}
						// don't roll chain if other rolls did not occur
						if (_rollChainUpHierarchy && _rollStoreToChain && _rollStoreDailyToWeekly)
						{
							if (aNonStockValueFound || aStockValueFound)
							{
								BuildHierarchyRollupItems(eVariableDataType.chainWeeklyHistory, aNodeRID, _currDayProfile.Week.Key, aVersionRID, aRollAlternatesOnly);
							}
						}
						break;
					}
					case eVariableDataType.storeWeeklyHistory:
					{
						if (_rollStoreWeeklyUpHierarchy ||
							aRollAlternatesOnly)
						{
							BuildHierarchyRollupItems(aVariableDataType, aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
						}
						if (_rollStoreToChain)
						{
							BuildStoreToChainRollupItems(aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
						}
						if (_rollChainUpHierarchy)
						{
							BuildHierarchyRollupItems(eVariableDataType.chainWeeklyHistory, aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
						}
						break;
					}
					case eVariableDataType.storeWeeklyForecast:
					{
						if (_rollStoreWeeklyUpHierarchy)
						{
							BuildHierarchyRollupItems(aVariableDataType, aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
						}
						if (_rollStoreToChain)
						{
							BuildStoreToChainRollupItems(aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
						}
						if (_rollChainUpHierarchy)
						{
							BuildHierarchyRollupItems(eVariableDataType.chainWeeklyForecast, aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
						}
						break;
					}
					case eVariableDataType.chainWeeklyHistory:
					{
						if (_rollChainUpHierarchy ||
							aRollAlternatesOnly)
						{
							BuildHierarchyRollupItems(aVariableDataType, aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
						}
						break;
					}
					case eVariableDataType.chainWeeklyForecast:
					{
						if (_rollChainUpHierarchy)
						{
							BuildHierarchyRollupItems(aVariableDataType, aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
						}
						break;
					}
					case eVariableDataType.storeExternalIntransit:
					{
						BuildHierarchyRollupItems(aVariableDataType, aNodeRID, aTimeID, aVersionRID, aRollAlternatesOnly);
						break;
					}

				}
			}
			catch ( Exception ex )
			{
				_audit.Log_Exception(ex, sourceModule);
				return false;
			}
			return true;
		}

		protected bool BuildDaysToWeeksRollupItems(int aNodeRID, int aTimeID, int aVersionRID, bool aRollAlternatesOnly)
		{
			try
			{
				// add key field to rollup manager to build rollup items
				// the organization of the Hashtables are:
				// version
				//   type
				//     time
				//       nodes
				Hashtable typeHash;
				Hashtable timeHash;
				Hashtable nodeHash;
				HistoryPlanNodeRollupRecord rollupRecord = new HistoryPlanNodeRollupRecord(aNodeRID, aRollAlternatesOnly);
				typeHash = (Hashtable)_rollupManager[aVersionRID];
				if (typeHash == null)
				{
					typeHash = new Hashtable();
					timeHash = new Hashtable();
					nodeHash = new Hashtable();

					if (!nodeHash.Contains(aNodeRID))
					{
						nodeHash.Add(aNodeRID, rollupRecord);
					}

					timeHash.Add(aTimeID,nodeHash);
					typeHash.Add((int)eRollType.storeDailyHistoryToWeeks,timeHash);
					_rollupManager.Add(aVersionRID, typeHash);
				}
				else
				{
					timeHash = (Hashtable)typeHash[(int)eRollType.storeDailyHistoryToWeeks];
					if (timeHash == null)
					{
						timeHash = new Hashtable();
						nodeHash = new Hashtable();

						if (!nodeHash.Contains(aNodeRID))
						{
							nodeHash.Add(aNodeRID, rollupRecord);
						}

						timeHash.Add(aTimeID,nodeHash);
						typeHash.Add((int)eRollType.storeDailyHistoryToWeeks,timeHash);
					}
					else
					{
						nodeHash = (Hashtable)timeHash[aTimeID];
						if (nodeHash == null)
						{
							nodeHash = new Hashtable();

							if (!nodeHash.Contains(aNodeRID))
							{
								nodeHash.Add(aNodeRID, rollupRecord);
							}

							timeHash.Add(aTimeID,nodeHash);
						}
						else
							if (!nodeHash.Contains(aNodeRID))
						{
							nodeHash.Add(aNodeRID, rollupRecord);
						}
					}
				}

				return true;
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				return false;
			}
		}

		protected bool BuildHierarchyRollupItems(eVariableDataType aVariableDataType, int aNodeRID, int aTimeID, int aVersionRID, bool aRollAlternatesOnly)
		{
			try
			{
				// add key field to rollup manager to build rollup items
				// the organization of the Hashtables are:
				// version
				//   type
				//     time
				//       nodes
				Hashtable typeHash;
				Hashtable timeHash;
				Hashtable nodeHash;
				HistoryPlanNodeRollupRecord rollupRecord = new HistoryPlanNodeRollupRecord(aNodeRID, aRollAlternatesOnly);
				typeHash = (Hashtable)_rollupManager[aVersionRID];
				if (typeHash == null)
				{
					typeHash = new Hashtable();
					timeHash = new Hashtable();
					nodeHash = new Hashtable();

					if (!nodeHash.Contains(aNodeRID))
					{
						nodeHash.Add(aNodeRID, rollupRecord);
					}
					timeHash.Add(aTimeID,nodeHash);
					typeHash.Add((int)aVariableDataType,timeHash);
					_rollupManager.Add(aVersionRID, typeHash);
				}
				else
				{
					timeHash = (Hashtable)typeHash[(int)aVariableDataType];
					if (timeHash == null)
					{
						timeHash = new Hashtable();
						nodeHash = new Hashtable();

						if (!nodeHash.Contains(aNodeRID))
						{
							nodeHash.Add(aNodeRID, rollupRecord);
						}
						timeHash.Add(aTimeID,nodeHash);
						typeHash.Add((int)aVariableDataType,timeHash);
					}
					else
					{
						nodeHash = (Hashtable)timeHash[aTimeID];
						if (nodeHash == null)
						{
							nodeHash = new Hashtable();

							if (!nodeHash.Contains(aNodeRID))
							{
								nodeHash.Add(aNodeRID, rollupRecord);
							}
							timeHash.Add(aTimeID,nodeHash);
						}
						else
							if (!nodeHash.Contains(aNodeRID))
						{
							if (!nodeHash.Contains(aNodeRID))
							{
								nodeHash.Add(aNodeRID, rollupRecord);
							}
						}
					}
				}

				return true;
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				return false;
			}
		}

		protected bool BuildStoreToChainRollupItems(int aNodeRID, int aTimeID, int aVersionRID, bool aRollAlternatesOnly)
		{
			try
			{
				// add key field to rollup manager to build rollup items
				// the organization of the Hashtables are:
				// version
				//   type
				//     time
				//       node
				Hashtable typeHash;
				Hashtable timeHash;
				Hashtable nodeHash;
				HistoryPlanNodeRollupRecord rollupRecord = new HistoryPlanNodeRollupRecord(aNodeRID, aRollAlternatesOnly);
				typeHash = (Hashtable)_rollupManager[aVersionRID];
				if (typeHash == null)
				{
					typeHash = new Hashtable();
					timeHash = new Hashtable();
					nodeHash = new Hashtable();

					if (!nodeHash.Contains(aNodeRID))
					{
						nodeHash.Add(aNodeRID, rollupRecord);
					}

					timeHash.Add(aTimeID,nodeHash);
					typeHash.Add((int)eRollType.storeToChain,timeHash);
					_rollupManager.Add(aVersionRID, typeHash);
				}
				else
				{
					timeHash = (Hashtable)typeHash[(int)eRollType.storeToChain];
					if (timeHash == null)
					{
						timeHash = new Hashtable();
						nodeHash = new Hashtable();

						if (!nodeHash.Contains(aNodeRID))
						{
							nodeHash.Add(aNodeRID, rollupRecord);
						}

						timeHash.Add(aTimeID,nodeHash);
						typeHash.Add((int)eRollType.storeToChain,timeHash);
					}
					else
					{
						nodeHash = (Hashtable)timeHash[aTimeID];
						if (nodeHash == null)
						{
							nodeHash = new Hashtable();

							if (!nodeHash.Contains(aNodeRID))
							{
								nodeHash.Add(aNodeRID, rollupRecord);
							}

							timeHash.Add(aTimeID,nodeHash);
						}
						else
							if (!nodeHash.Contains(aNodeRID))
						{
							nodeHash.Add(aNodeRID, rollupRecord);
						}
					}
				}

				return true;
			}
			catch ( Exception ex )
			{
				_audit.Log_Exception(ex, sourceModule);
				return false;
			}
		}

		protected bool BuildDummyColorRollupItems(int aNodeRID, int aTimeID)
		{
			try
			{
				// add key field to rollup manager to build rollup items
				// the organization of the Hashtables are:
				// version
				//   type
				//     time
				//       nodes
				Hashtable typeHash;
				Hashtable timeHash;
				Hashtable nodeHash;
				HistoryPlanNodeRollupRecord rollupRecord = new HistoryPlanNodeRollupRecord(aNodeRID, false);
				typeHash = (Hashtable)_rollupManager[Include.FV_ActualRID];
				if (typeHash == null)
				{
					typeHash = new Hashtable();
					timeHash = new Hashtable();
					nodeHash = new Hashtable();

					if (!nodeHash.Contains(aNodeRID))
					{
						nodeHash.Add(aNodeRID, rollupRecord);
					}
					timeHash.Add(aTimeID,nodeHash);
					typeHash.Add((int)eRollType.dummyColor,timeHash);
					_rollupManager.Add(Include.FV_ActualRID, typeHash);
				}
				else
				{
					timeHash = (Hashtable)typeHash[(int)eRollType.dummyColor];
					if (timeHash == null)
					{
						timeHash = new Hashtable();
						nodeHash = new Hashtable();

						if (!nodeHash.Contains(aNodeRID))
						{
							nodeHash.Add(aNodeRID, rollupRecord);
						}
						timeHash.Add(aTimeID,nodeHash);
						typeHash.Add((int)eRollType.dummyColor,timeHash);
					}
					else
					{
						nodeHash = (Hashtable)timeHash[aTimeID];
						if (nodeHash == null)
						{
							nodeHash = new Hashtable();

							if (!nodeHash.Contains(aNodeRID))
							{
								nodeHash.Add(aNodeRID, rollupRecord);
							}
							timeHash.Add(aTimeID,nodeHash);
						}
						else
							if (!nodeHash.Contains(aNodeRID))
						{
							if (!nodeHash.Contains(aNodeRID))
							{
								nodeHash.Add(aNodeRID, rollupRecord);
							}
						}
					}
				}

				return true;
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				return false;
			}
		}

		protected bool BuildComputationItems(eComputationType aComputationType, int aNodeRID, int aFiscalYearWeek, 
			int aVersionRID, int aVariableKey, ref bool aComputationItemLogged)
		{
			try
			{
				if (_computationModel.NeedsComputationItem(aVersionRID, aVariableKey, aComputationType))
				{
					aComputationItemLogged = true;
					// add key field to computation manager to build rollup items
					// the organization of the Hashtables are:
					// version
					//   type
					//     yearWeek
					//       nodes
					Hashtable typeHash;
					Hashtable yearWeekHash;
					Hashtable nodeHash;
					HistoryPlanNodeComputationRecord computationRecord = new HistoryPlanNodeComputationRecord(aNodeRID);
					typeHash = (Hashtable)_computationManager[aVersionRID];
					if (typeHash == null)
					{
						typeHash = new Hashtable();
						yearWeekHash = new Hashtable();
						nodeHash = new Hashtable();

						if (!nodeHash.Contains(aNodeRID))
						{
							nodeHash.Add(aNodeRID, computationRecord);
						}
						yearWeekHash.Add(aFiscalYearWeek,nodeHash);
						typeHash.Add((int)aComputationType,yearWeekHash);
						_computationManager.Add(aVersionRID, typeHash);
					}
					else
					{
						yearWeekHash = (Hashtable)typeHash[(int)aComputationType];
						if (yearWeekHash == null)
						{
							yearWeekHash = new Hashtable();
							nodeHash = new Hashtable();

							if (!nodeHash.Contains(aNodeRID))
							{
								nodeHash.Add(aNodeRID, computationRecord);
							}
							yearWeekHash.Add(aFiscalYearWeek,nodeHash);
							typeHash.Add((int)aComputationType,yearWeekHash);
						}
						else
						{
							nodeHash = (Hashtable)yearWeekHash[aFiscalYearWeek];
							if (nodeHash == null)
							{
								nodeHash = new Hashtable();

								if (!nodeHash.Contains(aNodeRID))
								{
									nodeHash.Add(aNodeRID, computationRecord);
								}
								yearWeekHash.Add(aFiscalYearWeek,nodeHash);
							}
							else
								if (!nodeHash.Contains(aNodeRID))
							{
								if (!nodeHash.Contains(aNodeRID))
								{
									nodeHash.Add(aNodeRID, computationRecord);
								}
							}
						}
					}
				}

				return true;
			}
			catch ( Exception ex )
			{
#if(DEBUG)
				StreamWriter debugWriter = new StreamWriter("error.txt", true);
				debugWriter.WriteLine("exception " + new System.Diagnostics.StackFrame(0,true).GetFileLineNumber());
				debugWriter.Flush();
				debugWriter.Close();
#endif
				_audit.Log_Exception(ex, sourceModule);
				return false;
			}
		}

		protected bool WriteRollupItems()
		{
			RollupData rd = null;
			try
			{
				int current_Time_ID = 0;
				int versionRID;
				int type_ID;
				int time_ID;
				int firstDayOfWeek = 0;
				int lastDayOfWeek = 0;
				int firstDayOfNextWeek = 0;
				Hashtable typeHash;
				Hashtable timeHash;
				Hashtable nodeHash;
				int recordsWritten = 0;
				rd = new RollupData();
				
				rd.Rollup_XMLInit();
				
				foreach (DictionaryEntry version in _rollupManager)
				{
					versionRID = (int)version.Key;
					typeHash = (Hashtable)version.Value;
					foreach (DictionaryEntry type in typeHash)
					{
						type_ID = (int)type.Key;
						timeHash = (Hashtable)type.Value;
						foreach (DictionaryEntry node in timeHash)
						{
							time_ID = (int)node.Key;
							if (type_ID == eRollType.storeDailyHistoryToWeeks.GetHashCode())
							{
								if (time_ID != current_Time_ID)
								{
									DayProfile Day = _cal.GetDay(time_ID);
									firstDayOfWeek = Day.Week.Days[0].Key;
									lastDayOfWeek = Day.Week.Days[Day.Week.Days.Count - 1].Key;
									current_Time_ID = time_ID;
									firstDayOfNextWeek = _cal.AddWeeks(Day.Week.Key, 1);
								}
							}
							nodeHash = (Hashtable)node.Value;
							ArrayList nodesToRoll = new ArrayList();
							Hashtable scheduledNodes = new Hashtable(); 
							foreach (HistoryPlanNodeRollupRecord nodeRecord in nodeHash.Values)
							{
									rd.Rollup_XMLInsert((int)eProcesses.historyPlanLoad, nodeRecord.NodeRID, type_ID, time_ID, versionRID, 0, 0, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek, nodeRecord.RollAlternatesOnly);
									++recordsWritten;
									if (recordsWritten >= 1000)
									{
										if (!rd.ConnectionIsOpen)
										{
											rd.OpenUpdateConnection(eLockType.RollupItem);
										}
										rd.Rollup_XMLWrite();
										rd.CommitData();
										// close unlocked connection opened by CommitData so new locked connection 
										// will be opened after building the next xml document
										rd.CloseUpdateConnection();
										rd.Rollup_XMLInit();
										recordsWritten = 0;
									}
							}
						}
					}
				}
				if (recordsWritten >= 0)
				{
					if (!rd.ConnectionIsOpen)
					{
						rd.OpenUpdateConnection(eLockType.RollupItem);
					}
					rd.Rollup_XMLWrite();
					rd.CommitData();
				}
				return true;
			}
			catch ( Exception ex )
			{
				_audit.Log_Exception(ex, sourceModule);
				return false;
			}
			finally
			{
				if (rd != null)
				{
					rd.CloseUpdateConnection();
				}
			}
		}

		protected bool WriteComputationItems()
		{
			ComputationData cd = null;
			try
			{
				int versionRID;
				eComputationType type_ID;
				int time_ID;
				Hashtable typeHash;
				Hashtable yearWeekHash;
				Hashtable nodeHash;
				int recordsWritten = 0;
				cd = new ComputationData();
				
				cd.Computation_XMLInit();
				
				foreach (DictionaryEntry version in _computationManager)
				{
					versionRID = (int)version.Key;
					typeHash = (Hashtable)version.Value;
					foreach (DictionaryEntry type in typeHash)
					{
						type_ID = (eComputationType)type.Key;
						yearWeekHash = (Hashtable)type.Value;
						foreach (DictionaryEntry node in yearWeekHash)
						{
							time_ID = (int)node.Key;
							nodeHash = (Hashtable)node.Value;
							foreach (HistoryPlanNodeComputationRecord nodeRecord in nodeHash.Values)
							{
								cd.Computation_XMLInsert(eProcesses.historyPlanLoad, _computationGroupRID, nodeRecord.NodeRID, type_ID, versionRID, time_ID);
								++recordsWritten;
								if (recordsWritten >= 1000)
								{
									if (!cd.ConnectionIsOpen)
									{
										cd.OpenUpdateConnection(eLockType.ComputationItem);
									}
									cd.Computation_XMLWrite();
									cd.CommitData();
									cd.Computation_XMLInit();
									recordsWritten = 0;
								}
							}
						}
					}
				}
				if (recordsWritten >= 0)
				{
					if (!cd.ConnectionIsOpen)
					{
						cd.OpenUpdateConnection(eLockType.RollupItem);
					}
					cd.Computation_XMLWrite();
					cd.CommitData();
				}
				return true;
			}
			catch ( Exception ex )
			{
				_audit.Log_Exception(ex, sourceModule);
				return false;
			}
			finally
			{
				if (cd != null)
				{
					cd.CloseUpdateConnection();
				}
			}
		}

		protected eReturnCode AddLookupRecord(string aProductID, string aParent, 
			string aSizeProductCategory, string aSizePrimary, string aSizeSecondary, 
			string aProductDescription, string aProductName)
		{
			try
			{
				eReturnCode returnCode = eReturnCode.successful;
				if (aProductID != null) 
				{
					if (aProductID != _transNodeID)
					{
						object nodeRID = _nodes[aProductID];
						if (nodeRID == null)
						{
							if (!_lookupNodes.ContainsKey(aProductID))
							{
								NodeLookup nl = new NodeLookup();
								nl.NodeID = aProductID;
								nl.NodeName = aProductName;
								nl.NodeDescription = aProductDescription;
								nl.ParentID = aParent;
								nl.SizeProductCategory = aSizeProductCategory;
								nl.SizePrimary = aSizePrimary;
								nl.SizeSecondary = aSizeSecondary;
								_lookupNodes.Add(aProductID, nl);
							}
						}
						_transNodeID = aProductID;
					}
				}
				return returnCode;
			}
			catch
			{
				throw;
			}
		}

		public eReturnCode LookupNodes()
		{
			try
			{
				eReturnCode returnCode = eReturnCode.successful;
				if (_lookupNodes.Count > 0) 
				{
					// if cache is not used, call database for IDs
					if (!_SAB.HierarchyServerSession.ColorSizesCacheUsed())
					{
						LookupNodesInDatabase();
						// process any remaining nodes in case needing autoadded
						LookupNodesInHierarchy();
					}
					else
					{
						LookupNodesInHierarchy();
					}
				}

				// clear Hashtable to free memory
				_lookupNodes = null;
				return returnCode;
			}
			catch
			{
				throw;
			}
		}

		private eReturnCode LookupNodesInHierarchy()
		{
			try
			{
				eReturnCode returnCode = eReturnCode.successful;
				Hashtable wrkLookupNodes = new Hashtable();
				foreach (NodeLookup nl in _lookupNodes.Values)
				{
					wrkLookupNodes.Add(nl.NodeID, nl);
					// lookup nodes in groups of 5000
					if (wrkLookupNodes.Count >= 5000) 
					{
						LookupNodesInHierarchy(wrkLookupNodes);
						wrkLookupNodes.Clear();
					}
				}
				// lookup remaining nodes
				if (wrkLookupNodes.Count > 0) 
				{
					LookupNodesInHierarchy(wrkLookupNodes);
				}
				
				return returnCode;
			}
			catch
			{
				throw;
			}
		}

		private eReturnCode LookupNodesInHierarchy(Hashtable aWrkLookupNodes)
		{
			try
			{
				eReturnCode returnCode = eReturnCode.successful;
				aWrkLookupNodes = _SAB.HierarchyServerSession.LookupNodes(_SAB, aWrkLookupNodes, _allowAutoAdds, _commitLimit);
				foreach (NodeLookup nl in aWrkLookupNodes.Values)
				{
					// save node information to master Hashtable
					if (nl.NodeRID != Include.NoRID)
					{
						_nodes[nl.NodeID] = nl.NodeRID;
					}
					if (nl.EditMsgs != null &&
						nl.EditMsgs.ErrorFound)
					{
						for (int e=0; e<nl.EditMsgs.EditMessages.Count; e++)
						{
							EditMsgs.Message emm = (EditMsgs.Message) nl.EditMsgs.EditMessages[e];
							_SAB.ClientServerSession.Audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.lineNumber, emm.module);
						}
					}
					_nodesAdded += nl.NodesAdded;
				}
							
				return returnCode;
			}
			catch
			{
				throw;
			}
		}

		private eReturnCode LookupNodesInDatabase()
		{
			try
			{
				eReturnCode returnCode = eReturnCode.successful;
				ArrayList validNodes = new ArrayList();
				int count = 0;
				_mhd.NodeLookup_XMLInit();
				foreach (NodeLookup nl in _lookupNodes.Values)
				{
					_mhd.Node_XMLInsert(nl.NodeID,_levelDelimiter);
					++count;
					// lookup nodes in groups of 5000
					if (count >= 5000) 
					{
						LookupNodesInDatabase(validNodes);
						_mhd.NodeLookup_XMLInit();
						count = 0;
					}
				}
				// lookup remaining nodes
				LookupNodesInDatabase(validNodes);
					
				// remove nodes that were found on the database from the lookup list
				foreach (string nodeID in validNodes)
				{
					_lookupNodes.Remove(nodeID);
				}
		
				return returnCode;
			}
			catch
			{
				throw;
			}
		}

		private eReturnCode LookupNodesInDatabase(ArrayList aValidNodes)
		{
			try
			{
				eReturnCode returnCode = eReturnCode.successful;
				string nodeID;
				int nodeRID;
				
				// check for size nodes
				DataTable dt = _mhd.GetSizeRIDs();
				foreach (DataRow dr in dt.Rows)
				{
					nodeID = Convert.ToString(dr["NODE_ID"], CultureInfo.CurrentUICulture);
					nodeRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
					if (nodeRID != Include.NoRID)
					{
						// save node information to master Hashtable
						_nodes[nodeID] = nodeRID;
						// keep node ID to remove from main list
						aValidNodes.Add(nodeID);
					}
				}
				// check for color nodes
				dt = _mhd.GetColorRIDs();
				foreach (DataRow dr in dt.Rows)
				{
					nodeID = Convert.ToString(dr["NODE_ID"], CultureInfo.CurrentUICulture);
					nodeRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
					if (nodeRID != Include.NoRID)
					{
						// save node information to master Hashtable
						_nodes[nodeID] = nodeRID;
						// keep node ID to remove from main list
						aValidNodes.Add(nodeID);
					}
				}
				// check for other nodes
				dt = _mhd.GetNodeRIDs();
				foreach (DataRow dr in dt.Rows)
				{
					nodeID = Convert.ToString(dr["NODE_ID"], CultureInfo.CurrentUICulture);
					nodeRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
					if (nodeRID != Include.NoRID)
					{
						// save node information to master Hashtable
						_nodes[nodeID] = nodeRID;
						// keep node ID to remove from main list
						aValidNodes.Add(nodeID);
					}
				}
							
				return returnCode;
			}
			catch
			{
				throw;
			}
		}

		public void ClearWorkingVariables()
		{
			try
			{
				_transCurrentDateID = 0;
				_transStoreID = null;
				_transStoreRID = Include.NoRID;
				_transNodeID = null;
				_transNodeRID = Include.NoRID;
				_transDate = null;
				_transVariable = null;
				_variableProfile = null;
				_date = DateTime.MinValue;
				_transDateID = -1;
				_transCurrentDateID = -1;
				_transNextDateID = -1;
				_recordsRead = 0;
				_recordsWithErrors = 0;
				_recordsNotCommitted = 0;
			}
			catch
			{
				throw;
			}
		}
	}

	public class HistoryPlanValue
	{
		// Fields

		private double				_value;
		private VariableProfile		_variableProfile;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HistoryPlanValue()
		{
			
		}

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		public double Value 
		{
			get { return _value ; }
			set { _value = value; }
		}

		/// <summary>
		/// Gets or sets the variable profile for the value.
		/// </summary>
		public VariableProfile VariableProfile 
		{
			get { return _variableProfile ; }
			set { _variableProfile = value; }
		}

	}

	public class HistoryPlanRecord
	{
		// Fields

		private int					_nodeRID;
		private int					_versionRID;
		private int					_storeRID;
		private int					_timeID;
		private int					_fiscalYearWeek;
		private HistoryPlanValue[]	_values;
		private object[]			_locks;
		private bool				_isSizeIntransit;
        // Begin TT#155 - JSmith - Size Curve Method 
        private bool                _isSizeHistory;
        private int                 _styleRID;
        private int                 _colorCodeRID;
        private int                 _sizeCodeRID;
        // End TT#155 
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
        public HistoryPlanRecord()
        {
            _isSizeIntransit = false;
            // Begin TT#155 - JSmith - Size Curve Method
            _isSizeHistory = false;
            _styleRID = Include.NoRID;
            _colorCodeRID = Include.NoRID;
            _sizeCodeRID = Include.NoRID;
            // End TT#155
        }

		/// <summary>
		/// Gets or sets the record ID for the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
			set { _nodeRID = value; }
		}

		/// <summary>
		/// Gets or sets the record ID for the version.
		/// </summary>
		public int VersionRID 
		{
			get { return _versionRID ; }
			set { _versionRID = value; }
		}

		/// <summary>
		/// Gets or sets the time ID.
		/// </summary>
		public int TimeID 
		{
			get { return _timeID ; }
			set { _timeID = value; }
		}

		/// <summary>
		/// Gets or sets the fiscal year week.
		/// </summary>
		public int FiscalYearWeek 
		{
			get { return _fiscalYearWeek ; }
			set { _fiscalYearWeek = value; }
		}

		/// <summary>
		/// Gets or sets the record ID for the store.
		/// </summary>
		public int StoreRID 
		{
			get { return _storeRID ; }
			set { _storeRID = value; }
		}

		/// <summary>
		/// Gets or sets the array of values for the record.
		/// </summary>
		public HistoryPlanValue[] Values 
		{
			get { return _values ; }
			set { _values = value; }
		}

		/// <summary>
		/// Gets or sets the array of locks for the record.
		/// </summary>
		public object[] Locks 
		{
			get { return _locks ; }
			set { _locks = value; }
		}

		/// <summary>
		/// Gets or sets the flag identifying if the record is for size intransit.
		/// </summary>
		public bool IsSizeIntransit 
		{
			get { return _isSizeIntransit ; }
			set { _isSizeIntransit = value; }
		}

        // Begin TT#155 - JSmith - Size Curve Method
        /// <summary>
        /// Gets or sets the flag identifying if the record is for size history.
        /// </summary>
        public bool IsSizeHistory
        {
            get { return _isSizeHistory; }
            set { _isSizeHistory = value; }
        }

        /// <summary>
        /// Gets or sets the key for the style associated with the value.
        /// </summary>
        public int StyleRID
        {
            get { return _styleRID; }
            set { _styleRID = value; }
        }

        /// <summary>
        /// Gets or sets the key for the color code associated with the value.
        /// </summary>
        public int ColorCodeRID
        {
            get { return _colorCodeRID; }
            set { _colorCodeRID = value; }
        }

        /// <summary>
        /// Gets or sets the key for the size code associated with the value.
        /// </summary>
        public int SizeCodeRID
        {
            get { return _sizeCodeRID; }
            set { _sizeCodeRID = value; }
        }
        // End TT#155
	}

	public class HistoryPlanNodeRollupRecord
	{
		// Fields

		private int					_nodeRID;
		private bool				_rollAlternatesOnly;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HistoryPlanNodeRollupRecord()
		{
			
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HistoryPlanNodeRollupRecord(int aNodeRID, bool aRollAlternatesOnly)
		{
			_nodeRID = aNodeRID;
			_rollAlternatesOnly = aRollAlternatesOnly;
		}

		/// <summary>
		/// Gets or sets the record ID for the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
			set { _nodeRID = value; }
		}

		/// <summary>
		/// Gets or sets the flag to roll alternate hierarchies only.
		/// </summary>
		public bool RollAlternatesOnly 
		{
			get { return _rollAlternatesOnly ; }
			set { _rollAlternatesOnly = value; }
		}
	}

	public class HistoryPlanNodeComputationRecord
	{
		// Fields

		private int					_nodeRID;
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HistoryPlanNodeComputationRecord()
		{
			
		}

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HistoryPlanNodeComputationRecord(int aNodeRID)
		{
			_nodeRID = aNodeRID;
		}

		/// <summary>
		/// Gets or sets the record ID for the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
			set { _nodeRID = value; }
		}
	}

    // Begin TT#155 - JSmith - Size Curve Method
    #region StoreSizeValue
    /// <summary>
    /// Read Only Vector structure containing color size variable Int16 values for a store 
    /// </summary>
    public struct StoreSizeValue
    {
        int _storeRID;
        double _colorSizeValue;
        //Begin TT#467 - JSmith - EnqueueConflictException during size history load
        int _sizeNodeRID;
        //End TT#467
        /// <summary>
        /// Creates an instance of this variable vector
        /// </summary>
        /// <param name="aStoreRID">Store RID (Key)</param>
        /// <param name="aColorSizeValue">The value for the color and size</param>
        /// <param name="aSizeNodeRID">The size node RID (Key) </param>
        //Begin TT#467 - JSmith - EnqueueConflictException during size history load
        //public StoreSizeValue(int aStoreRID, double aColorSizeValue)
        public StoreSizeValue(int aStoreRID, double aColorSizeValue, int aSizeNodeRID)
        //End TT#467
        {
            _storeRID = aStoreRID;
            _colorSizeValue = aColorSizeValue;
            //Begin TT#467 - JSmith - EnqueueConflictException during size history load
            _sizeNodeRID = aSizeNodeRID;
            //End TT#467
        }
        /// <summary>
        /// Gets the store RID associated with the value
        /// </summary>
        public int StoreRID
        {
            get { return _storeRID; }
        }

        /// <summary>
        /// Gets the  value
        /// </summary>
        public double ColorSizeValue
        {
            get { return _colorSizeValue; }
        }

        //Begin TT#467 - JSmith - EnqueueConflictException during size history load
        /// <summary>
        /// Gets the  value
        /// </summary>
        public int SizeNodeRID
        {
            get { return _sizeNodeRID; }
        }
        //End TT#467
    }
    #endregion StoreSizeValue
    // End TT#155
}
