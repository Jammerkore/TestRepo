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

	public class OTSForecastPlanningExtractMethodData : MethodBaseData
	{
		//=======
		// FIELDS
		//=======

		private ProfileList _varProfList;

		private int _merchandiseRID;
		private int _versionRID;
		private int _dateRangeRID;
		private int _filterRID;
		private bool _chain;
        private bool _store;
        private bool _attributeSet;
        private int _attributeRID;
        private bool _lowLevels;
		private bool _lowLevelsOnly;
		private eLowLevelsType _lowLevelsType;
		private int _lowLevelSequence;
		private int _lowLevelOffset;
		private bool _showIneligible;
		private bool _excludeZeroValues;
		private int _concurrentProcesses;
		private int _overrideLowLevelRid; 
        private DateTime _updateDate;
        private DateTime _extractDate;

		private ArrayList _variableList;
        private ArrayList _timeTotalVariableList;
        private ArrayList _versionOverrideList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of the OTSForecastPlanningExtractMethodData class.
		/// </summary>

		public OTSForecastPlanningExtractMethodData(ProfileList aVariableProfileList = null)
		{
			try
			{
                if (aVariableProfileList != null)
                {
                    _varProfList = aVariableProfileList;
                }

				InitFields();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates an instance of the OTSForecastPlanningExtractMethodData class.
		/// </summary>
		/// <param name="aMethodRID">
		/// The record ID of the method
		/// </param>

		public OTSForecastPlanningExtractMethodData(ProfileList aVariableProfileList, int method_RID, eChangeType changeType)
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
		/// Creates an instance of the OTSForecastPlanningExtractMethodData class.
		/// </summary>
		/// <param name="td">
		/// An instance of the TransactionData class containing the database connection.
		/// </param>
		/// <param name="aMethodRID">
		/// The record ID of the method
		/// </param>
		
		public OTSForecastPlanningExtractMethodData(ProfileList aVariableProfileList, TransactionData td, int aMethodRID)
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

		public OTSForecastPlanningExtractMethodData(ProfileList aVariableProfileList, TransactionData td)
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

		public bool Chain
		{
			get
			{
				return _chain;
			}
			set
			{
				_chain = value;
			}
		}

        public bool Store
        {
            get
            {
                return _store;
            }
            set
            {
                _store = value;
            }
        }

        public bool AttributeSet
        {
            get
            {
                return _attributeSet;
            }
            set
            {
                _attributeSet = value;
            }
        }

        public int AttributeRID
        {
            get
            {
                return _attributeRID;
            }
            set
            {
                _attributeRID = value;
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

        public ArrayList TimeTotalVariableList
        {
            get
            {
                return _timeTotalVariableList;
            }
            set
            {
                _timeTotalVariableList = value;
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

        public DateTime UpdateDate
        {
            get
            {
                return _updateDate;
            }
            set
            {
                _updateDate = value;
            }
        }

        public DateTime ExtractDate
        {
            get
            {
                return _extractDate;
            }
            set
            {
                _extractDate = value;
            }
        }

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
				_chain = false;
                _store = false;
                _attributeSet = false;
                _attributeRID = Include.NoRID;
                _lowLevels = false;
				_lowLevelsOnly = false;
				_lowLevelsType = eLowLevelsType.None;
				_lowLevelSequence = 0;
				_lowLevelOffset = 0;
				_showIneligible = false;
				_excludeZeroValues = false;
				_concurrentProcesses = 0;
				_variableList = new ArrayList();
                _timeTotalVariableList = new ArrayList();
                _overrideLowLevelRid = Include.NoRID;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void PopulateForecastExportMethod(int aMethodRID)
		{
			DataTable dtForecastExportMethod;
			DataTable dtForecastExportVariables;
			DataRow dr;

			try
			{
				if (PopulateMethod(aMethodRID))
				{
                    dtForecastExportMethod = StoredProcedures.MID_METHOD_PLANNING_EXTRACT_READ.Read(_dba, METHOD_RID: aMethodRID);

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

                        _chain = dr["CHAIN_IND"].ToString() == "1";
                        _store = dr["STORE_IND"].ToString() == "1";
                        _attributeSet = dr["ATTRIBUTE_SET_IND"].ToString() == "1";
                        if (dr["ATTRIBUTE_RID"] != System.DBNull.Value)
                        {
                            _attributeRID = Convert.ToInt32(dr["ATTRIBUTE_RID"], CultureInfo.CurrentUICulture);
                        }
                        _lowLevels = dr["LOW_LEVELS_IND"].ToString() == "1";
						_lowLevelsOnly = dr["LOW_LEVELS_ONLY_IND"].ToString() == "1";
						_lowLevelsType = (eLowLevelsType)Convert.ToInt32(dr["LOW_LEVELS_TYPE"]);
						_lowLevelSequence = Convert.ToInt32(dr["LOW_LEVEL_SEQUENCE"], CultureInfo.CurrentUICulture);
						_lowLevelOffset = Convert.ToInt32(dr["LOW_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
						_showIneligible = dr["SHOW_INELIGIBLE_IND"].ToString() == "1";
						_excludeZeroValues = dr["EXCLUDE_ZERO_VALUES_IND"].ToString() == "1";
						_concurrentProcesses = Convert.ToInt32(dr["CONCURRENT_PROCESSES"], CultureInfo.CurrentUICulture);
						_overrideLowLevelRid = Convert.ToInt32(dr["OLL_RID"], CultureInfo.CurrentUICulture);
                        if (dr["UPDATE_DATE"] != System.DBNull.Value)
                        {
                            _updateDate = Convert.ToDateTime(dr["UPDATE_DATE"], CultureInfo.CurrentUICulture);
                        }
                        if (dr["EXTRACT_DATE"] != System.DBNull.Value)
                        {
                            _extractDate = Convert.ToDateTime(dr["EXTRACT_DATE"], CultureInfo.CurrentUICulture);
                        }

						// Load Variables
                        dtForecastExportVariables = StoredProcedures.MID_METHOD_PLANNING_EXTRACT_VARIABLES_READ.Read(_dba, METHOD_RID: aMethodRID);

						foreach (DataRow row in dtForecastExportVariables.Rows)
						{
                            if ((eVariableTimeType)Convert.ToInt32(row["VARIABLE_TYPE"], CultureInfo.CurrentUICulture) == eVariableTimeType.Weekly)
                            {
                                _variableList.Add(new ForecastPlanningExtractMethodVariableEntry(row));
                            }
                            else
                            {
                                _timeTotalVariableList.Add(new ForecastPlanningExtractMethodVariableEntry(row));
                            }
                        }
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
                int? ATTRIBUTE_RID_Nullable = null;
                if (_attributeRID != Include.NoRID) ATTRIBUTE_RID_Nullable = _attributeRID;
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;

                StoredProcedures.MID_METHOD_PLANNING_EXTRACT_INSERT.Insert(_dba,
                                                                 METHOD_RID: aMethodRID,
                                                                 HN_RID: _merchandiseRID,
                                                                 FV_RID: _versionRID,
                                                                 CDR_RID: _dateRangeRID,
                                                                 STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                                 CHAIN_IND: Include.ConvertBoolToChar(_chain),
                                                                 STORE_IND: Include.ConvertBoolToChar(_store),
                                                                 ATTRIBUTE_SET_IND: Include.ConvertBoolToChar(_attributeSet),
                                                                 ATTRIBUTE_RID: ATTRIBUTE_RID_Nullable,
                                                                 LOW_LEVELS_IND: Include.ConvertBoolToChar(_lowLevels),
                                                                 LOW_LEVELS_ONLY_IND: Include.ConvertBoolToChar(_lowLevelsOnly),
                                                                 LOW_LEVELS_TYPE: (int)_lowLevelsType,
                                                                 LOW_LEVEL_SEQUENCE: _lowLevelSequence,
                                                                 LOW_LEVEL_OFFSET: _lowLevelOffset,
                                                                 SHOW_INELIGIBLE_IND: Include.ConvertBoolToChar(_showIneligible),
                                                                 EXCLUDE_ZERO_VALUES_IND: Include.ConvertBoolToChar(_excludeZeroValues),
                                                                 CONCURRENT_PROCESSES: _concurrentProcesses,
                                                                 OLL_RID: OLL_RID_Nullable,
                                                                 UPDATE_DATE: _updateDate,
                                                                 EXTRACT_DATE: _extractDate
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
                int? ATTRIBUTE_RID_Nullable = null;
                if (_attributeRID != Include.NoRID) ATTRIBUTE_RID_Nullable = _attributeRID;
                int? OLL_RID_Nullable = null;
                if (_overrideLowLevelRid != Include.NoRID) OLL_RID_Nullable = _overrideLowLevelRid;
                StoredProcedures.MID_METHOD_PLANNING_EXTRACT_UPDATE.Update(_dba,
                                                                 METHOD_RID: aMethodRID,
                                                                 HN_RID: _merchandiseRID,
                                                                 FV_RID: _versionRID,
                                                                 CDR_RID: _dateRangeRID,
                                                                 STORE_FILTER_RID: STORE_FILTER_RID_Nullable,
                                                                 CHAIN_IND: Include.ConvertBoolToChar(_chain),
                                                                 STORE_IND: Include.ConvertBoolToChar(_store),
                                                                 ATTRIBUTE_SET_IND: Include.ConvertBoolToChar(_attributeSet),
                                                                 ATTRIBUTE_RID: ATTRIBUTE_RID_Nullable,
                                                                 LOW_LEVELS_IND: Include.ConvertBoolToChar(_lowLevels),
                                                                 LOW_LEVELS_ONLY_IND: Include.ConvertBoolToChar(_lowLevelsOnly),
                                                                 LOW_LEVELS_TYPE: (int)_lowLevelsType,
                                                                 LOW_LEVEL_SEQUENCE: _lowLevelSequence,
                                                                 LOW_LEVEL_OFFSET: _lowLevelOffset,
                                                                 SHOW_INELIGIBLE_IND: Include.ConvertBoolToChar(_showIneligible),
                                                                 EXCLUDE_ZERO_VALUES_IND: Include.ConvertBoolToChar(_excludeZeroValues),
                                                                 CONCURRENT_PROCESSES: _concurrentProcesses,
                                                                 OLL_RID: OLL_RID_Nullable,
                                                                 UPDATE_DATE: _updateDate,
                                                                 EXTRACT_DATE: _extractDate
                                                                 );

				InsertChildData(aMethodRID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        public void ExtractDateUpdateMethod(int aMethodRID)
        {
            try
            {

                StoredProcedures.MID_METHOD_PLANNING_EXTRACT_EXTRACT_DATE_UPDATE.Update(_dba,
                                                                 METHOD_RID: aMethodRID,
                                                                 EXTRACT_DATE: _extractDate
                                                                 );

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

                StoredProcedures.MID_METHOD_PLANNING_EXTRACT_DELETE.Delete(_dba, METHOD_RID: aMethodRID);
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
                return StoredProcedures.MID_METHOD_PLANNING_EXTRACT_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
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
                StoredProcedures.MID_METHOD_PLANNING_EXTRACT_VARIABLES_DELETE.Delete(_dba, METHOD_RID: aMethodRID);		
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
               
				
				foreach (ForecastPlanningExtractMethodVariableEntry entry in _variableList)
				{
                   
                    StoredProcedures.MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT.Insert(_dba,
                                                                               METHOD_RID: aMethodRID,
                                                                               VARIABLE_RID: entry.VariableRID,
                                                                               VARIABLE_TYPE: entry.VariableType,
                                                                               VARIABLE_SEQ: entry.VariableSequence
                                                                               );
				}

                foreach (ForecastPlanningExtractMethodVariableEntry entry in _timeTotalVariableList)
                {

                    StoredProcedures.MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT.Insert(_dba,
                                                                               METHOD_RID: aMethodRID,
                                                                               VARIABLE_RID: entry.VariableRID,
                                                                               VARIABLE_TYPE: entry.VariableType,
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

	public class ForecastPlanningExtractMethodVariableEntry
	{
		//=======
		// FIELDS
		//=======

		private int _methodRID;
		private int _variableRID;
        private int _variableType;
        private int _variableSequence;

		//=============
		// CONSTRUCTORS
		//=============

		public ForecastPlanningExtractMethodVariableEntry(DataRow aDataRow)
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

		public ForecastPlanningExtractMethodVariableEntry(int aMethodRID, int aVariableRID, int aVariableType, int aVariableSequence)
		{
			try
			{
				_methodRID = aMethodRID;
				_variableRID = aVariableRID;
                _variableType = aVariableType;
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

        public int VariableType
        {
            get
            {
                return _variableType;
            }
            set
            {
                _variableType = value;
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
                _variableType = Convert.ToInt32(aRow["VARIABLE_TYPE"], CultureInfo.CurrentUICulture);
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
				aRow["VARIABLE_TYPE"] = _variableType;
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

	public class ForecastPlanningExtractMethodVersionOverrideEntry
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

		public ForecastPlanningExtractMethodVersionOverrideEntry(DataRow aDataRow)
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

		public ForecastPlanningExtractMethodVersionOverrideEntry(int aMethodRID, int aMerchandiseRID, int aVersionRID, bool aExclude)
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
