using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Globalization;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	/// <summary>
	/// Inherits MethodBase containing all properties for a Method.
	/// Adds properties for OTS_Plan
	/// </summary>

	public class OTSForecastExportMethodData : MethodBaseData
	{
		//=======
		// FIELDS
		//=======

		private ProfileList _varProfList;

		private int _merchandiseRID;
		private int _versionRID;
		private int _dateRangeRID;
		private int _filterRID;
		private ePlanType _planType;
		private bool _lowLevels;
		private bool _lowLevelsOnly;
		private eLowLevelsType _lowLevelsType;
		private int _lowLevelSequence;
		private int _lowLevelOffset;
		private bool _showIneligible;
		private bool _useDefaultSettings;
		private eExportType _exportType;
		private string _delimeter;
//Begin Track #4942 - JScott - Correct problems in Export Method
		private string _csvFileExtension;
		private eExportDateType _dateType;
//End Track #4942 - JScott - Correct problems in Export Method
		private bool _preinitValues;
		//Begin Track #5395 - JScott - Add ability to discard zero values in Export
		private bool _excludeZeroValues;
		//End Track #5395 - JScott - Add ability to discard zero values in Export
		private string _filePath;
		private bool _addDateStamp;
		private bool _addTimeStamp;
		private eExportSplitType _splitType;
		private int _splitNumEntries;
		private int _concurrentProcesses;
		private bool _createFlagFile;
		private string _flagFileExtension;
		private bool _createEndFile;
		private string _endFileExtension;
		private int _overrideLowLevelRid; // Override LowLevel Enhancement

		private ArrayList _variableList;
		private ArrayList _versionOverrideList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of the OTSForecastExportMethodData class.
		/// </summary>

		public OTSForecastExportMethodData(ProfileList aVariableProfileList)
		{
			try
			{
				_varProfList = aVariableProfileList;

				InitFields();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates an instance of the OTSForecastExportMethodData class.
		/// </summary>
		/// <param name="aMethodRID">
		/// The record ID of the method
		/// </param>

		public OTSForecastExportMethodData(ProfileList aVariableProfileList, int method_RID, eChangeType changeType)
		{
			try
			{
				_varProfList = aVariableProfileList;

				InitFields();

				switch (changeType)
				{
					case eChangeType.populate:
						PopulateForecastExportMethod(method_RID);
						break;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates an instance of the OTSForecastExportMethodData class.
		/// </summary>
		/// <param name="td">
		/// An instance of the TransactionData class containing the database connection.
		/// </param>
		/// <param name="aMethodRID">
		/// The record ID of the method
		/// </param>
		
		public OTSForecastExportMethodData(ProfileList aVariableProfileList, TransactionData td, int aMethodRID)
		{
			try
			{
				_varProfList = aVariableProfileList;
				_dba = td.DBA;

				InitFields();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates an instance of the GeneralAllocationMethodData class.
		/// </summary>
		/// <param name="td">
		/// An instance of the TransactionData class containing the database connection.
		/// </param>

		public OTSForecastExportMethodData(ProfileList aVariableProfileList, TransactionData td)
		{
			try
			{
				_varProfList = aVariableProfileList;
				_dba = td.DBA;

				InitFields();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//============
		// PROPERTIES
		//============

		public int HierarchyRID
		{
			get
			{
				return _merchandiseRID;
			}
			set
			{
				_merchandiseRID = value;
			}
		}

		public int VersionRID
		{
			get
			{
				return _versionRID;
			}
			set
			{
				_versionRID = value;
			}
		}

		public int DateRangeRID
		{
			get
			{
				return _dateRangeRID;
			}
			set
			{
				_dateRangeRID = value;
			}
		}

		public int FilterRID
		{
			get
			{
				return _filterRID;
			}
			set
			{
				_filterRID = value;
			}
		}

		public ePlanType PlanType
		{
			get
			{
				return _planType;
			}
			set
			{
				_planType = value;
			}
		}

		public bool LowLevels
		{
			get
			{
				return _lowLevels;
			}
			set
			{
				_lowLevels = value;
			}
		}

		public bool LowLevelsOnly
		{
			get
			{
				return _lowLevelsOnly;
			}
			set
			{
				_lowLevelsOnly = value;
			}
		}

		public eLowLevelsType LowLevelsType
		{
			get
			{
				return _lowLevelsType;
			}
			set
			{
				_lowLevelsType = value;
			}
		}

		public int LowLevelSequence
		{
			get
			{
				return _lowLevelSequence;
			}
			set
			{
				_lowLevelSequence = value;
			}
		}

		public int LowLevelOffset
		{
			get
			{
				return _lowLevelOffset;
			}
			set
			{
				_lowLevelOffset = value;
			}
		}

		public bool ShowIneligible
		{
			get
			{
				return _showIneligible;
			}
			set
			{
				_showIneligible = value;
			}
		}

		public bool UseDefaultSettings
		{
			get
			{
				return _useDefaultSettings;
			}
			set
			{
				_useDefaultSettings = value;
			}
		}

		public eExportType ExportType
		{
			get
			{
				return _exportType;
			}
			set
			{
				_exportType = value;
			}
		}

		public string Delimeter
		{
			get
			{
				return _delimeter;
			}
			set
			{
				_delimeter = value;
			}
		}

//Begin Track #4942 - JScott - Correct problems in Export Method
		public string CSVFileExtension
		{
			get
			{
				return _csvFileExtension;
			}
			set
			{
				_csvFileExtension = value;
			}
		}

		public eExportDateType DateType
		{
			get
			{
				return _dateType;
			}
			set
			{
				_dateType = value;
			}
		}

//End Track #4942 - JScott - Correct problems in Export Method
		public bool PreinitValues
		{
			get
			{
				return _preinitValues;
			}
			set
			{
				_preinitValues = value;
			}
		}

		//Begin Track #5395 - JScott - Add ability to discard zero values in Export
		public bool ExcludeZeroValues
		{
			get
			{
				return _excludeZeroValues;
			}
			set
			{
				_excludeZeroValues = value;
			}
		}

		//End Track #5395 - JScott - Add ability to discard zero values in Export
		public string FilePath
		{
			get
			{
				return _filePath;
			}
			set
			{
				_filePath = value;
			}
		}

		public bool AddDateStamp
		{
			get
			{
				return _addDateStamp;
			}
			set
			{
				_addDateStamp = value;
			}
		}

		public bool AddTimeStamp
		{
			get
			{
				return _addTimeStamp;
			}
			set
			{
				_addTimeStamp = value;
			}
		}

		public eExportSplitType SplitType
		{
			get
			{
				return _splitType;
			}
			set
			{
				_splitType = value;
			}
		}

		public int SplitNumEntries
		{
			get
			{
				return _splitNumEntries;
			}
			set
			{
				_splitNumEntries = value;
			}
		}

		public int ConcurrentProcesses
		{
			get
			{
				return _concurrentProcesses;
			}
			set
			{
				_concurrentProcesses = value;
			}
		}

		public bool CreateFlagFile
		{
			get
			{
				return _createFlagFile;
			}
			set
			{
				_createFlagFile = value;
			}
		}

		public string FlagFileExtension
		{
			get
			{
				return _flagFileExtension;
			}
			set
			{
				_flagFileExtension = value;
			}
		}

		public bool CreateEndFile
		{
			get
			{
				return _createEndFile;
			}
			set
			{
				_createEndFile = value;
			}
		}

		public string EndFileExtension
		{
			get
			{
				return _endFileExtension;
			}
			set
			{
				_endFileExtension = value;
			}
		}

		public ArrayList VariableList
		{
			get
			{
				return _variableList;
			}
			set
			{
				_variableList = value;
			}
		}

		public ArrayList VersionOverrideList
		{
			get
			{
				return _versionOverrideList;
			}
			set
			{
				_versionOverrideList = value;
			}
		}

		// BEGIN Override LowLevel Enhancement
		public int OverrideLowLevelRid
		{
			get
			{
				return _overrideLowLevelRid;
			}
			set
			{
				_overrideLowLevelRid = value;
			}
		}
		// END Override LowLevel Enhancement

		//========
		// METHODS
		//========

		private void InitFields()
		{
			try
			{
				_merchandiseRID = Include.NoRID;
				_versionRID = Include.NoRID;
				_dateRangeRID = Include.NoRID;
				_filterRID = Include.NoRID;
				_planType = ePlanType.Chain;
				_lowLevels = false;
				_lowLevelsOnly = false;
				_lowLevelsType = eLowLevelsType.None;
				_lowLevelSequence = 0;
				_lowLevelOffset = 0;
				_showIneligible = false;
				_useDefaultSettings = false;
				_exportType = eExportType.XML;
				_delimeter = string.Empty;
//Begin Track #4942 - JScott - Correct problems in Export Method
				_csvFileExtension = string.Empty;
				_dateType = 0;
//End Track #4942 - JScott - Correct problems in Export Method
				_preinitValues = false;
				//Begin Track #5395 - JScott - Add ability to discard zero values in Export
				_excludeZeroValues = false;
				//End Track #5395 - JScott - Add ability to discard zero values in Export
				_filePath = string.Empty;
				_addDateStamp = false;
				_addTimeStamp = false;
				_splitType = eExportSplitType.None;
				_splitNumEntries = 0;
				_concurrentProcesses = 0;
				_createFlagFile = false;
				_flagFileExtension = string.Empty;
				_createEndFile = false;
				_endFileExtension = string.Empty;
				_variableList = new ArrayList();
				// BEGIN Override LowLevel Enhancement
				//_versionOverrideList = new ArrayList();
				_overrideLowLevelRid = Include.NoRID;
				// END Override LowLevel Enhancement
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void PopulateForecastExportMethod(int aMethodRID)
		{
			//string SQLCommand;
			DataTable dtForecastExportMethod;
			DataTable dtForecastExportVariables;
			//DataTable dtForecastExportVersionOverrides; //TT#846-MD -jsobek -New Stored Procedures for Performance -Fix Warnings
			DataRow dr;

			try
			{
				if (PopulateMethod(aMethodRID))
				{
                    dtForecastExportMethod = StoredProcedures.MID_METHOD_EXPORT_READ.Read(_dba, METHOD_RID: aMethodRID);

					if (dtForecastExportMethod.Rows.Count != 0)
					{
						dr = dtForecastExportMethod.Rows[0];
						_merchandiseRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						_versionRID = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
						_dateRangeRID = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);

						if (dr["STORE_FILTER_RID"] != System.DBNull.Value)
						{
							_filterRID = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);
						}

						_planType = (ePlanType)Convert.ToInt32(dr["PLAN_TYPE"]);
						_lowLevels = dr["LOW_LEVELS_IND"].ToString() == "1";
						_lowLevelsOnly = dr["LOW_LEVELS_ONLY_IND"].ToString() == "1";
						_lowLevelsType = (eLowLevelsType)Convert.ToInt32(dr["LOW_LEVELS_TYPE"]);
						_lowLevelSequence = Convert.ToInt32(dr["LOW_LEVEL_SEQUENCE"], CultureInfo.CurrentUICulture);
						_lowLevelOffset = Convert.ToInt32(dr["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
						_showIneligible = dr["SHOW_INELIGIBLE_IND"].ToString() == "1";
						_useDefaultSettings = dr["USE_DEFAULT_SETTINGS_IND"].ToString() == "1";
						_exportType = (eExportType)Convert.ToInt32(dr["EXPORT_TYPE"]);
						_delimeter = Convert.ToString(dr["DELIMETER"], CultureInfo.CurrentUICulture);
//Begin Track #4942 - JScott - Correct problems in Export Method
						_csvFileExtension = Convert.ToString(dr["CSV_FILE_EXTENSION"], CultureInfo.CurrentUICulture);
						_dateType = (eExportDateType)Convert.ToInt32(dr["DATE_TYPE"]);
//End Track #4942 - JScott - Correct problems in Export Method
						_preinitValues = dr["PREINIT_VALUES_IND"].ToString() == "1";
						//Begin Track #5395 - JScott - Add ability to discard zero values in Export
						_excludeZeroValues = dr["EXCLUDE_ZERO_VALUES_IND"].ToString() == "1";
						//End Track #5395 - JScott - Add ability to discard zero values in Export
						_filePath = Convert.ToString(dr["FILE_PATH"], CultureInfo.CurrentUICulture);
						_addDateStamp = dr["ADD_DATE_STAMP_IND"].ToString() == "1";
						_addTimeStamp = dr["ADD_TIME_STAMP_IND"].ToString() == "1";
						_splitType = (eExportSplitType)Convert.ToInt32(dr["SPLIT_TYPE"]);

						_splitNumEntries = Convert.ToInt32(dr["SPLIT_NUM_ENTRIES"], CultureInfo.CurrentUICulture);
						_concurrentProcesses = Convert.ToInt32(dr["CONCURRENT_PROCESSES"], CultureInfo.CurrentUICulture);
						_createFlagFile = dr["CREATE_FLAG_FILE_IND"].ToString() == "1";
						_flagFileExtension = Convert.ToString(dr["FLAG_FILE_EXTENSION"], CultureInfo.CurrentUICulture);
						_createEndFile = dr["CREATE_END_FILE_IND"].ToString() == "1";
						_endFileExtension = Convert.ToString(dr["END_FILE_EXTENSION"], CultureInfo.CurrentUICulture);
						// BEGIN Override Low Level Enhancements
						_overrideLowLevelRid = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
						// END Override Low Level Enhancements


						// Load Variables
                        dtForecastExportVariables = StoredProcedures.MID_METHOD_EXPORT_VARIABLES_READ.Read(_dba, METHOD_RID: aMethodRID);

						foreach (DataRow row in dtForecastExportVariables.Rows)
						{
							_variableList.Add(new ForecastExportMethodVariableEntry(row));
						}

						//// Load Version Overrides

						//SQLCommand = "SELECT " +
						//    "COALESCE(METHOD_RID," + Include.NoRID.ToString(CultureInfo.CurrentUICulture) + ") METHOD_RID," +
						//    "COALESCE(HN_RID," + Include.NoRID.ToString(CultureInfo.CurrentUICulture) + ") HN_RID," +
						//    "COALESCE(FV_RID," + Include.NoRID.ToString(CultureInfo.CurrentUICulture) + ") FV_RID," +
						//    "COALESCE(EXCLUDE_IND, '0') EXCLUDE_IND " +
						//    "FROM METHOD_EXPORT_VERSION_OVERRIDES WHERE METHOD_RID = " + aMethodRID.ToString(CultureInfo.CurrentUICulture);
				
						//dtForecastExportVersionOverrides = _dba.ExecuteSQLQuery(SQLCommand, "ForecastExport");

						//foreach (DataRow row in dtForecastExportVersionOverrides.Rows)
						//{
						//    _versionOverrideList.Add(new ForecastExportMethodVersionOverrideEntry(row));
						//}
					}
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void InsertMethod(int aMethodRID, TransactionData td)
		{

			try
			{	
				_dba = td.DBA;

         
                int? STORE_FILTER_RID_Nullable = null;
                if (_filterRID != Include.NoRID) STORE_FILTER_RID_Nullable = _filterRID;
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;
                StoredProcedures.MID_METHOD_EXPORT_INSERT.Insert(_dba,
                                                                 METHOD_RID: aMethodRID,
                                                                 HN_RID: _merchandiseRID,
                                                                 FV_RID: _versionRID,
                                                                 CDR_RID: _dateRangeRID,
                                                                 STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                                 PLAN_TYPE: (int)_planType,
                                                                 LOW_LEVELS_IND: (_lowLevels) ? '1' : '0',
                                                                 LOW_LEVELS_ONLY_IND: (_lowLevelsOnly) ? '1' : '0',
                                                                 LOW_LEVELS_TYPE: (int)_lowLevelsType,
                                                                 LOW_LEVEL_SEQUENCE: _lowLevelSequence,
                                                                 LOW_LEVEL_OFFSET: _lowLevelOffset,
                                                                 SHOW_INELIGIBLE_IND: (_showIneligible) ? '1' : '0',
                                                                 USE_DEFAULT_SETTINGS_IND: (_useDefaultSettings) ? '1' : '0',
                                                                 EXPORT_TYPE: (int)_exportType,
                                                                 DELIMETER: Include.ConvertStringToChar(_delimeter),
                                                                 CSV_FILE_EXTENSION: _csvFileExtension,
                                                                 DATE_TYPE: (int)_dateType,
                                                                 PREINIT_VALUES_IND: (_preinitValues) ? '1' : '0',
                                                                 EXCLUDE_ZERO_VALUES_IND: (_excludeZeroValues) ? '1' : '0',
                                                                 FILE_PATH: _filePath,
                                                                 ADD_DATE_STAMP_IND: (_addDateStamp) ? '1' : '0',
                                                                 ADD_TIME_STAMP_IND: (_addTimeStamp) ? '1' : '0',
                                                                 SPLIT_TYPE: (int)_splitType,
                                                                 SPLIT_NUM_ENTRIES: _splitNumEntries,
                                                                 CONCURRENT_PROCESSES: _concurrentProcesses,
                                                                 CREATE_FLAG_FILE_IND: (_createFlagFile) ? '1' : '0',
                                                                 FLAG_FILE_EXTENSION: _flagFileExtension,
                                                                 CREATE_END_FILE_IND: (_createEndFile) ? '1' : '0',
                                                                 END_FILE_EXTENSION: _endFileExtension,
                                                                 OLL_RID: OLL_RID_Nullable
				                                                 );
				
				InsertChildData(aMethodRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public void UpdateMethod(int aMethodRID, TransactionData td)
		{
			try
			{
				_dba = td.DBA;

				DeleteChildData(aMethodRID);

                
                int? STORE_FILTER_RID_Nullable = null;
                if (_filterRID != Include.NoRID) STORE_FILTER_RID_Nullable = _filterRID;
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;
                StoredProcedures.MID_METHOD_EXPORT_UPDATE.Update(_dba,
                                                                 METHOD_RID: aMethodRID,
                                                                 HN_RID: _merchandiseRID,
                                                                 FV_RID: _versionRID,
                                                                 CDR_RID: _dateRangeRID,
                                                                 STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                                 PLAN_TYPE: (int)_planType,
                                                                 LOW_LEVELS_IND: (_lowLevels) ? '1' : '0',
                                                                 LOW_LEVELS_ONLY_IND: (_lowLevelsOnly) ? '1' : '0',
                                                                 LOW_LEVELS_TYPE: (int)_lowLevelsType,
                                                                 LOW_LEVEL_SEQUENCE: _lowLevelSequence,
                                                                 LOW_LEVEL_OFFSET: _lowLevelOffset,
                                                                 SHOW_INELIGIBLE_IND: (_showIneligible) ? '1' : '0',
                                                                 USE_DEFAULT_SETTINGS_IND: (_useDefaultSettings) ? '1' : '0',
                                                                 EXPORT_TYPE: (int)_exportType,
                                                                 DELIMETER: Include.ConvertStringToChar(_delimeter),
                                                                 CSV_FILE_EXTENSION: _csvFileExtension,
                                                                 DATE_TYPE: (int)_dateType,
                                                                 PREINIT_VALUES_IND: (_preinitValues) ? '1' : '0',
                                                                 EXCLUDE_ZERO_VALUES_IND: (_excludeZeroValues) ? '1' : '0',
                                                                 FILE_PATH: _filePath,
                                                                 ADD_DATE_STAMP_IND: (_addDateStamp) ? '1' : '0',
                                                                 ADD_TIME_STAMP_IND: (_addTimeStamp) ? '1' : '0',
                                                                 SPLIT_TYPE: (int)_splitType,
                                                                 SPLIT_NUM_ENTRIES: _splitNumEntries,
                                                                 CONCURRENT_PROCESSES: _concurrentProcesses,
                                                                 CREATE_FLAG_FILE_IND: (_createFlagFile) ? '1' : '0',
                                                                 FLAG_FILE_EXTENSION: _flagFileExtension,
                                                                 CREATE_END_FILE_IND: (_createEndFile) ? '1' : '0',
                                                                 END_FILE_EXTENSION: _endFileExtension,
                                                                 OLL_RID: OLL_RID_Nullable
                                                                 );

				InsertChildData(aMethodRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void DeleteMethod(int aMethodRID, TransactionData td)
		{
			
			try
			{
				_dba = td.DBA;

				DeleteChildData(aMethodRID);

                StoredProcedures.MID_METHOD_EXPORT_DELETE.Delete(_dba, METHOD_RID: aMethodRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		
		public DataTable GetMethodsByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_METHOD_EXPORT_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		private void DeleteChildData(int aMethodRID)
		{
			try
			{
                StoredProcedures.MID_METHOD_EXPORT_VARIABLES_DELETE.Delete(_dba, METHOD_RID: aMethodRID);		
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		private void InsertChildData(int aMethodRID)
		{
			try
			{
               
				
				foreach (ForecastExportMethodVariableEntry entry in _variableList)
				{
                   
                    StoredProcedures.MID_METHOD_EXPORT_VARIABLES_INSERT.Insert(_dba,
                                                                               METHOD_RID: aMethodRID,
                                                                               VARIABLE_RID: entry.VariableRID,
                                                                               VARIABLE_SEQ: entry.VariableSequence
                                                                               );
				}

				
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Summary description for ForecastExportMethodVariable.
	/// </summary>

	public class ForecastExportMethodVariableEntry
	{
		//=======
		// FIELDS
		//=======

		private int _methodRID;
		private int _variableRID;
		private int _variableSequence;

		//=============
		// CONSTRUCTORS
		//=============

		public ForecastExportMethodVariableEntry(DataRow aDataRow)
		{
			try
			{
				LoadFromDataRow(aDataRow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ForecastExportMethodVariableEntry(int aMethodRID, int aVariableRID, int aVariableSequence)
		{
			try
			{
				_methodRID = aMethodRID;
				_variableRID = aVariableRID;
				_variableSequence = aVariableSequence;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public int MethodRID
		{
			get
			{
				return _methodRID;
			}
			set
			{
				_methodRID = value;
			}
		}

		public int VariableRID
		{
			get
			{
				return _variableRID;
			}
			set
			{
				_variableRID = value;
			}
		}

		public int VariableSequence
		{
			get
			{
				return _variableSequence;
			}
			set
			{
				_variableSequence = value;
			}
		}

		//========
		// METHODS
		//========

		private void LoadFromDataRow(DataRow aRow)
		{
			try
			{
				_methodRID = Convert.ToInt32(aRow["METHOD_RID"], CultureInfo.CurrentUICulture);
				_variableRID = Convert.ToInt32(aRow["VARIABLE_RID"], CultureInfo.CurrentUICulture);
				_variableSequence = Convert.ToInt32(aRow["VARIABLE_SEQ"], CultureInfo.CurrentUICulture);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			try
			{
				aRow["METHOD_RID"] = _methodRID;
				aRow["VARIABLE_RID"] = _variableRID;
				aRow["VARIABLE_SEQ"] = _variableSequence;

				return aRow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Summary description for ForecastExportMethodVersionOverride.
	/// </summary>

	public class ForecastExportMethodVersionOverrideEntry
	{
		//=======
		// FIELDS
		//=======

		private int _methodRID;
		private int _merchandiseRID;
		private int _versionRID;
		private bool _exclude;

		//=============
		// CONSTRUCTORS
		//=============

		public ForecastExportMethodVersionOverrideEntry(DataRow aDataRow)
		{
			try
			{
				LoadFromDataRow(aDataRow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ForecastExportMethodVersionOverrideEntry(int aMethodRID, int aMerchandiseRID, int aVersionRID, bool aExclude)
		{
			try
			{
				_methodRID = aMethodRID;
				_merchandiseRID = aMerchandiseRID;
				_versionRID = aVersionRID;
				_exclude = aExclude;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public int MethodRID
		{
			get
			{
				return _methodRID;
			}
			set
			{
				_methodRID = value;
			}
		}

		public int MerchandiseRID
		{
			get
			{
				return _merchandiseRID;
			}
			set
			{
				_merchandiseRID = value;
			}
		}

		public int VersionRID
		{
			get
			{
				return _versionRID;
			}
			set
			{
				_versionRID = value;
			}
		}

		public bool Exclude
		{
			get
			{
				return _exclude;
			}
			set
			{
				_exclude = value;
			}
		}

		//========
		// METHODS
		//========

		private void LoadFromDataRow(DataRow aRow)
		{
			try
			{
				_methodRID = Convert.ToInt32(aRow["METHOD_RID"], CultureInfo.CurrentUICulture);
				_merchandiseRID = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);
				_versionRID = Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture);
				_exclude = aRow["EXCLUDE_IND"].ToString() == "1";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			try
			{
				aRow["METHOD_RID"] = _methodRID;
				aRow["HN_RID"] = _merchandiseRID;
				aRow["FV_RID"] = _versionRID;
				aRow["EXCLUDE_IND"] = (_exclude) ? "1" : "0";

				return aRow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
