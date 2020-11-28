// Begin TT#3284 - JSmith - Rollup Performance
// removed comments and commented out code for readability
// use compare tool for differences
// End TT#3284 - JSmith - Rollup Performance

// Begin TT#3523 - JSmith - Performance of Anthro morning processing jobs
// Modified build methods to use common code.
// use compare tool for differences
// End TT#3523 - JSmith - Performance of Anthro morning processing jobs

using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
    /// <summary>
    /// Summary description for Rollup.
    /// </summary>
    public class Rollup
    {
        //=======
        // FIELDS
        //=======

        SessionAddressBlock _SAB;
        int _batchSize;
        int _concurrentProcesses;
        Hashtable _forecastVersions = new Hashtable();
        RollupData _rd = new RollupData();
        private bool _schedulePostingRollup = false;
        private bool _scheduleReclassRollup = false;
        private bool _scheduleRollup = false;
        private ArrayList _chainWeeklyHistoryDatabaseVariables;
        private ArrayList _chainWeeklyForecastDatabaseVariables;
        private ArrayList _storeDailyHistoryDatabaseVariables;
        private ArrayList _storeWeeklyHistoryDatabaseVariables;
        private ArrayList _storeWeeklyForecastDatabaseVariables;
        private ArrayList _storeToChainHistoryDatabaseVariables;
        private ArrayList _storeToChainForecastDatabaseVariables;
        private ArrayList _dayToWeekHistoryDatabaseVariables;
        private Requests _requests = null;
        private int _lowestRollLevel = 0;
        private bool _includeZeroInAverage;
        private bool _honorLocks = false;
        private bool _zeroParentsWithNoChildren = true;
        private int _totalItems = 0;
        private int _totalBatches = 0;
        private int _totalErrors = 0;

        //=============
        // CONSTRUCTORS
        //=============

        public Rollup(SessionAddressBlock aSAB, int aBatchSize, int aConcurrentProcesses, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren)
        {
            _SAB = aSAB;
            _batchSize = aBatchSize;
            _concurrentProcesses = aConcurrentProcesses;
            _includeZeroInAverage = aIncludeZeroInAverage;
            _honorLocks = aHonorLocks;
            _zeroParentsWithNoChildren = aZeroParentsWithNoChildren;
            _chainWeeklyHistoryDatabaseVariables = new ArrayList();
            _chainWeeklyForecastDatabaseVariables = new ArrayList();
            _storeDailyHistoryDatabaseVariables = new ArrayList();
            _storeWeeklyHistoryDatabaseVariables = new ArrayList();
            _storeWeeklyForecastDatabaseVariables = new ArrayList();
            _dayToWeekHistoryDatabaseVariables = new ArrayList();
            _storeToChainHistoryDatabaseVariables = new ArrayList();
            _storeToChainForecastDatabaseVariables = new ArrayList();
        }

        //===========
        // PROPERTIES
        //===========

        public bool SchedulePostingRollup
        {
            get { return _schedulePostingRollup; }
        }

        public bool ScheduleReclassRollup
        {
            get { return _scheduleReclassRollup; }
        }

        public bool ScheduleRollup
        {
            get { return _scheduleRollup; }
        }

        public bool NoVariablesToRoll
        {
            get
            {
                if (_chainWeeklyHistoryDatabaseVariables.Count == 0 &&
                    _chainWeeklyForecastDatabaseVariables.Count == 0 &&
                    _storeDailyHistoryDatabaseVariables.Count == 0 &&
                    _storeWeeklyHistoryDatabaseVariables.Count == 0 &&
                    _storeWeeklyForecastDatabaseVariables.Count == 0 &&
                    _storeToChainHistoryDatabaseVariables.Count == 0 &&
                    _storeToChainForecastDatabaseVariables.Count == 0 &&
                    _dayToWeekHistoryDatabaseVariables.Count == 0 &&
                    _storeToChainForecastDatabaseVariables.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int TotalItems
        {
            get { return _totalItems; }
        }

        public int TotalBatches
        {
            get { return _totalBatches; }
        }

        public int TotalErrors
        {
            get { return _totalErrors; }
        }

        //========
        // METHODS
        //========

        public void DetermineRollupVariables(Session aSession, string aInputFile, int aOverrideLevel)
        {
            VariableProfile vp = null;
            HierarchyProfile hp = null;
            string text = null;
            try
            {
                _chainWeeklyHistoryDatabaseVariables.Clear();
                _chainWeeklyForecastDatabaseVariables.Clear();
                _storeDailyHistoryDatabaseVariables.Clear();
                _dayToWeekHistoryDatabaseVariables.Clear();
                _storeWeeklyHistoryDatabaseVariables.Clear();
                _storeWeeklyForecastDatabaseVariables.Clear();
                _storeToChainHistoryDatabaseVariables.Clear();
                _storeToChainForecastDatabaseVariables.Clear();
                hp = _SAB.HierarchyServerSession.GetMainHierarchyData();

                if (LoadRequestFile(aInputFile, aSession))
                {

                    if (_requests != null &&
                        _requests.Variables != null)
                    {
                        foreach (RequestsVariables v in _requests.Variables)
                        {
                            vp = (VariableProfile)_SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.GetVariableProfileByName(v.Name);
                            if (vp == null)
                            {
                                text = MIDText.GetText(eMIDTextCode.msg_VariableNotFound);
                                text += ":" + v.Name;
                                aSession.Audit.Add_Msg(eMIDMessageLevel.Warning, text, GetType().Name);
                            }
                            else
                            {
                                vp = (VariableProfile)vp.Clone();
                                if (vp.DatabaseColumnName == null)
                                {
                                    text = MIDText.GetText(eMIDTextCode.msg_VariableCanNotBeRolled);
                                    text = text.Replace("{0}", v.Name);
                                    aSession.Audit.Add_Msg(eMIDMessageLevel.Warning, text, GetType().Name);
                                }

                                if (aOverrideLevel > 0)
                                {
                                    vp.ChainForecastRollLevel = aOverrideLevel;
                                }
                                else if (v.ChainForecastLevel == null ||
                                    v.ChainForecastLevel.Trim().Length == 0)
                                {
                                    vp.ChainForecastRollLevel = int.MaxValue;
                                }
                                else
                                {
                                    vp.ChainForecastRollLevel = DetermineLevel(hp, v.ChainForecastLevel);
                                }
                                if (vp.ChainForecastRollLevel == -1)
                                {
                                    text = MIDText.GetText(eMIDTextCode.msg_InvalidRollLevel);
                                    text = text.Replace("{0}", v.ChainForecastLevel);
                                    text = text.Replace("{1}", v.Name);
                                    aSession.Audit.Add_Msg(eMIDMessageLevel.Warning, text, GetType().Name);
                                }
                                else if (vp.ChainForecastRollLevel > _lowestRollLevel)
                                {
                                    _lowestRollLevel = vp.ChainForecastRollLevel;
                                }

                                if (aOverrideLevel > 0)
                                {
                                    vp.ChainHistoryRollLevel = aOverrideLevel;
                                }
                                else if (v.ChainHistoryLevel.Trim().Length == 0)
                                {
                                    vp.ChainHistoryRollLevel = int.MaxValue;
                                }
                                else
                                {
                                    vp.ChainHistoryRollLevel = DetermineLevel(hp, v.ChainHistoryLevel);
                                }
                                if (vp.ChainHistoryRollLevel == -1)
                                {
                                    text = MIDText.GetText(eMIDTextCode.msg_InvalidRollLevel);
                                    text = text.Replace("{0}", v.ChainHistoryLevel);
                                    text = text.Replace("{1}", v.Name);
                                    aSession.Audit.Add_Msg(eMIDMessageLevel.Warning, text, GetType().Name);
                                }
                                else if (vp.ChainHistoryRollLevel > _lowestRollLevel)
                                {
                                    _lowestRollLevel = vp.ChainHistoryRollLevel;
                                }

                                if (aOverrideLevel > 0)
                                {
                                    vp.StoreForecastRollLevel = aOverrideLevel;
                                }
                                else if (v.StoreForecastLevel == null ||
                                    v.StoreForecastLevel.Trim().Length == 0)
                                {
                                    vp.StoreForecastRollLevel = int.MaxValue;
                                }
                                else
                                {
                                    vp.StoreForecastRollLevel = DetermineLevel(hp, v.StoreForecastLevel);
                                }
                                if (vp.StoreForecastRollLevel == -1)
                                {
                                    text = MIDText.GetText(eMIDTextCode.msg_InvalidRollLevel);
                                    text = text.Replace("{0}", v.StoreForecastLevel);
                                    text = text.Replace("{1}", v.Name);
                                    aSession.Audit.Add_Msg(eMIDMessageLevel.Warning, text, GetType().Name);
                                }
                                else if (vp.StoreForecastRollLevel > _lowestRollLevel)
                                {
                                    _lowestRollLevel = vp.StoreForecastRollLevel;
                                }

                                if (aOverrideLevel > 0)
                                {
                                    vp.StoreDailyHistoryRollLevel = aOverrideLevel;
                                }
                                else if (v.StoreDailyHistoryLevel == null ||
                                    v.StoreDailyHistoryLevel.Trim().Length == 0)
                                {
                                    vp.StoreDailyHistoryRollLevel = int.MaxValue;
                                }
                                else
                                {
                                    vp.StoreDailyHistoryRollLevel = DetermineLevel(hp, v.StoreDailyHistoryLevel);
                                }
                                if (vp.StoreDailyHistoryRollLevel == -1)
                                {
                                    text = MIDText.GetText(eMIDTextCode.msg_InvalidRollLevel);
                                    text = text.Replace("{0}", v.StoreDailyHistoryLevel);
                                    text = text.Replace("{1}", v.Name);
                                    aSession.Audit.Add_Msg(eMIDMessageLevel.Warning, text, GetType().Name);
                                }
                                else if (vp.StoreDailyHistoryRollLevel > _lowestRollLevel)
                                {
                                    _lowestRollLevel = vp.StoreDailyHistoryRollLevel;
                                }

                                if (aOverrideLevel > 0)
                                {
                                    vp.StoreWeeklyHistoryRollLevel = aOverrideLevel;
                                }
                                else if (v.StoreWeeklyHistoryLevel == null ||
                                    v.StoreWeeklyHistoryLevel.Trim().Length == 0)
                                {
                                    vp.StoreWeeklyHistoryRollLevel = int.MaxValue;
                                }
                                else
                                {
                                    vp.StoreWeeklyHistoryRollLevel = DetermineLevel(hp, v.StoreWeeklyHistoryLevel);
                                }
                                if (vp.StoreWeeklyHistoryRollLevel == -1)
                                {
                                    text = MIDText.GetText(eMIDTextCode.msg_InvalidRollLevel);
                                    text = text.Replace("{0}", v.StoreWeeklyHistoryLevel);
                                    text = text.Replace("{1}", v.Name);
                                    aSession.Audit.Add_Msg(eMIDMessageLevel.Warning, text, GetType().Name);
                                }
                                else if (vp.StoreWeeklyHistoryRollLevel > _lowestRollLevel)
                                {
                                    _lowestRollLevel = vp.StoreWeeklyHistoryRollLevel;
                                }

                                if (vp.DatabaseColumnName != null)
                                {
                                    if (vp.LevelRollType != eLevelRollType.None &&
                                        vp.ChainHistoryModelType != eVariableDatabaseModelType.None)
                                    {
                                        _chainWeeklyHistoryDatabaseVariables.Add(vp);
                                    }

                                    if (vp.LevelRollType != eLevelRollType.None &&
                                        vp.ChainForecastModelType != eVariableDatabaseModelType.None)
                                    {
                                        _chainWeeklyForecastDatabaseVariables.Add(vp);
                                    }

                                    if (vp.LevelRollType != eLevelRollType.None &&
                                        vp.StoreDailyHistoryModelType != eVariableDatabaseModelType.None)
                                    {
                                        _storeDailyHistoryDatabaseVariables.Add(vp);
                                    }

                                    if (vp.LevelRollType != eLevelRollType.None &&
                                        vp.StoreWeeklyHistoryModelType != eVariableDatabaseModelType.None)
                                    {
                                        _storeWeeklyHistoryDatabaseVariables.Add(vp);
                                    }

                                    if (vp.LevelRollType != eLevelRollType.None &&
                                        vp.StoreForecastModelType != eVariableDatabaseModelType.None)
                                    {
                                        _storeWeeklyForecastDatabaseVariables.Add(vp);
                                    }
                                }
                            }
                        }
                    }
                }

                // default variables not found in file to roll all levels
                foreach (VariableProfile vp2 in _SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList)
                {
                    if (vp2.DatabaseColumnName != null)
                    {
                        if (aOverrideLevel > 0)
                        {
                            vp2.ChainForecastRollLevel = aOverrideLevel;
                        }
                        else
                        {
                            vp2.ChainForecastRollLevel = int.MaxValue;
                        }
                        if (aOverrideLevel > 0)
                        {
                            vp2.ChainHistoryRollLevel = aOverrideLevel;
                        }
                        else
                        {
                            vp2.ChainHistoryRollLevel = int.MaxValue;
                        }
                        if (aOverrideLevel > 0)
                        {
                            vp2.StoreForecastRollLevel = aOverrideLevel;
                        }
                        else
                        {
                            vp2.StoreForecastRollLevel = int.MaxValue;
                        }
                        if (aOverrideLevel > 0)
                        {
                            vp2.StoreDailyHistoryRollLevel = aOverrideLevel;
                        }
                        else
                        {
                            vp2.StoreDailyHistoryRollLevel = int.MaxValue;
                        }
                        if (aOverrideLevel > 0)
                        {
                            vp2.StoreWeeklyHistoryRollLevel = aOverrideLevel;
                        }
                        else
                        {
                            vp2.StoreWeeklyHistoryRollLevel = int.MaxValue;
                        }
                        // add to daily variables
                        bool variableFound = false;

                        foreach (VariableProfile vp3 in _storeDailyHistoryDatabaseVariables)
                        {
                            if (vp2.Key == vp3.Key)
                            {
                                variableFound = true;
                                continue;
                            }
                        }
                        if (!variableFound)
                        {
                            if (vp2.LevelRollType != eLevelRollType.None &&
                                vp2.StoreDailyHistoryModelType != eVariableDatabaseModelType.None)
                            {
                                _storeDailyHistoryDatabaseVariables.Add(vp2);
                            }
                        }

                        // add to weekly variables
                        variableFound = false;
                        foreach (VariableProfile vp3 in _chainWeeklyHistoryDatabaseVariables)
                        {
                            if (vp2.Key == vp3.Key)
                            {
                                variableFound = true;
                                continue;
                            }
                        }
                        if (!variableFound)
                        {
                            if (vp2.LevelRollType != eLevelRollType.None &&
                                vp2.ChainHistoryModelType != eVariableDatabaseModelType.None)
                            {
                                _chainWeeklyHistoryDatabaseVariables.Add(vp2);
                            }
                        }

                        variableFound = false;
                        foreach (VariableProfile vp3 in _chainWeeklyForecastDatabaseVariables)
                        {
                            if (vp2.Key == vp3.Key)
                            {
                                variableFound = true;
                                continue;
                            }
                        }
                        if (!variableFound)
                        {
                            if (vp2.LevelRollType != eLevelRollType.None &&
                                vp2.ChainForecastModelType != eVariableDatabaseModelType.None)
                            {
                                _chainWeeklyForecastDatabaseVariables.Add(vp2);
                            }
                        }

                        variableFound = false;
                        foreach (VariableProfile vp3 in _storeWeeklyHistoryDatabaseVariables)
                        {
                            if (vp2.Key == vp3.Key)
                            {
                                variableFound = true;
                                continue;
                            }
                        }
                        if (!variableFound)
                        {
                            if (vp2.LevelRollType != eLevelRollType.None &&
                                vp2.StoreWeeklyHistoryModelType != eVariableDatabaseModelType.None)
                            {
                                _storeWeeklyHistoryDatabaseVariables.Add(vp2);
                            }
                        }

                        variableFound = false;
                        foreach (VariableProfile vp3 in _storeWeeklyForecastDatabaseVariables)
                        {
                            if (vp2.Key == vp3.Key)
                            {
                                variableFound = true;
                                continue;
                            }
                        }
                        if (!variableFound)
                        {
                            if (vp2.LevelRollType != eLevelRollType.None &&
                                vp2.StoreForecastModelType != eVariableDatabaseModelType.None)
                            {
                                _storeWeeklyForecastDatabaseVariables.Add(vp2);
                            }
                        }
                    }
                }

                BuildIntersectionOfVariableLists(_storeDailyHistoryDatabaseVariables, _storeWeeklyHistoryDatabaseVariables, _dayToWeekHistoryDatabaseVariables, eRollType.storeDailyHistoryToWeeks);
                BuildIntersectionOfVariableLists(_storeWeeklyHistoryDatabaseVariables, _chainWeeklyHistoryDatabaseVariables, _storeToChainHistoryDatabaseVariables, eRollType.storeToChain);
                BuildIntersectionOfVariableLists(_storeWeeklyForecastDatabaseVariables, _chainWeeklyForecastDatabaseVariables, _storeToChainForecastDatabaseVariables, eRollType.storeToChain);
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
                throw;
            }
        }

        public void BuildIntersectionOfVariableLists(ArrayList aList1, ArrayList aList2, ArrayList aResultList, eRollType aRollType)
        {
            // must be in both lists
            foreach (VariableProfile vp in aList1)
            {
                if (aRollType == eRollType.storeDailyHistoryToWeeks &&
                    vp.DayToWeekRollType == eDayToWeekRollType.None)
                {
                    continue;
                }
                else if (aRollType == eRollType.storeToChain &&
                    (vp.StoreToChainRollType == eStoreToChainRollType.None ||
                        vp.VariableCategory != eVariableCategory.Both))
                {
                    continue;
                }

                foreach (VariableProfile vp2 in aList2)
                {
                    if (aRollType == eRollType.storeDailyHistoryToWeeks &&
                    vp2.DayToWeekRollType == eDayToWeekRollType.None)
                    {
                        continue;
                    }
                    else if (aRollType == eRollType.storeToChain &&
                        (vp2.StoreToChainRollType == eStoreToChainRollType.None ||
                        vp2.VariableCategory != eVariableCategory.Both))
                    {
                        continue;
                    }

                    if (vp.Key == vp2.Key)
                    {
                        aResultList.Add(vp);
                        break;
                    }
                }
            }
        }

        public bool AnyOverrides(Session aSession, string aInputFile)
        {
            try
            {
                if (LoadRequestFile(aInputFile, aSession))
                {
                    if (_requests != null &&
                        _requests.Request != null)
                    {
                        foreach (RequestsRequest r in _requests.Request)
                        {
                            if (r.OverrideVariableLevels)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
                throw;
            }
        }

		public bool ProcessEachRollupRequests(string aInputFile, int aProcessID, Session aSession)
        {
            string message = null;
            try
            {
                try
                {
                    if (LoadRequestFile(aInputFile, aSession))
                    {
                        RequestsOptionsRollupDays rollupDays = RequestsOptionsRollupDays.All;
                        if (_requests.Options != null)
                        {
                            rollupDays = _requests.Options.RollupDays;
                        }
                        GetForecastVersions();
                        foreach (RequestsRequest r in _requests.Request)
                        {
                            _schedulePostingRollup = false;
                            _scheduleReclassRollup = false;
                            if (BuildRollupRequests(aSession, r.Type, r.Product, r.Version, r.Data, r.Period,
                                r.DateType, r.FromDate, r.ToDate, r.FromLevel, r.ToLevel, r.OverrideVariableLevels, rollupDays) == eReturnCode.successful)
                            {
                                if (r.OverrideVariableLevels)
                                {
                                    int overrideLevel = 0;
                                    try
                                    {
                                        overrideLevel = Convert.ToInt32(r.FromLevel);
                                    }
                                    catch
                                    {
                                        overrideLevel = 0;
                                    }
                                    DetermineRollupVariables(aSession, aInputFile, overrideLevel);
                                }
                                else
                                {
                                    DetermineRollupVariables(aSession, aInputFile, 0);
                                }

                                if (SchedulePostingRollup)
                                {
                                    ProcessPostingRollupRequests(_SAB.ClientServerSession);
                                }
                                else
                                {
                                    ProcessRollupRequests(aProcessID, _SAB.ClientServerSession);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = ex.ToString();
                    message = String.Format("Error encountered while processing the file '{0}'", aInputFile);
                    aSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, GetType().Name);
                    return false;
                }
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
                return false;
            }
            return true;
        }

		public void ProcessRollupRequests(int aProcess, Session aSession)
        {
            try
            {
                string returnMessage = null;
                eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
                HierarchyProfileList hpl = _SAB.HierarchyServerSession.GetHierarchiesByDependency(ref returnMessage, ref messageLevel);
                if (returnMessage != null)
                {
                    aSession.Audit.Add_Msg(messageLevel, returnMessage, GetType().Name);
                }
                foreach (HierarchyProfile hp in hpl)
                {
                    RollupHierarchy(aSession, aProcess, hp.Key, false);
                }
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
            }
            finally
            {
                // remove any remaining items that are not for Reclass or Posting
                if (aProcess != (int)eProcesses.historyPlanLoad &&
                    aProcess != (int)eProcesses.hierarchyLoad)
                {
                    if (!_rd.ConnectionIsOpen)
                    {
                        _rd.OpenUpdateConnection();
                    }
                    _rd.DeleteRollupItems(aProcess);
                    _rd.CommitData();
                }

                if (_rd.ConnectionIsOpen)
                {
                    _rd.CloseUpdateConnection();
                }
            }
        }

        public bool BuildRollupRequestsFromMethod(int aMethodRID, Session aSession, int aProcessID)
        {
			HierarchyNodeProfile hnp = null;
			int hnRID = Include.NoRID;
			int phRID = Include.NoRID;
			int versionRID = Include.NoRID;
			int fromLevel = Include.Undefined;
			int toLevel = Include.Undefined;
			int typeID = 0;
			int cdrRID = 0;
			bool rollDayToWeek = false;
			bool rollDays = false;
			bool rollWeeks = false;
			bool rollStore = false;
			bool rollChain = false;
			bool rollStoreToChain = false;
			bool rollLevel = false;
			bool rollIntransit = false;
			bool fromPhOffsetIndFound = false;
			bool intransitOnly = false;
			eHierarchyDescendantType fromPhOffsetInd = eHierarchyDescendantType.offset;
			int fromOffset = 0;
			bool toPhOffsetIndFound = false;
			eHierarchyDescendantType toPhOffsetInd = eHierarchyDescendantType.offset;
			int toOffset = 0;
			ProfileList weekProfileList = null;
			bool schedulePostingRollup = false;
			bool scheduleReclassRollup = false;
            HierarchyProfile hp = null;
            HierarchyProfile hpOrg = null;
            string message = null;
            ForecastVersion fv;
            bool requestsBuiltSuccessfully = true;
            
			try
			{
				RequestsOptionsRollupDays rollupDays = RequestsOptionsRollupDays.All;
                OTSRollupMethodData otsRollupMethodData = new OTSRollupMethodData();
                DataTable dtRollupMethodData = otsRollupMethodData.MethodRollup_Read(aMethodRID);
				try
				{
					_rd.OpenUpdateConnection();
                    foreach (DataRow dr in dtRollupMethodData.Rows)
					{
						if (dr["HN_RID"] != System.DBNull.Value)
						{
							hnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
						}
						if (dr["TO_LEVEL_HRID"] != System.DBNull.Value)
						{
                            phRID = Convert.ToInt32(dr["TO_LEVEL_HRID"], CultureInfo.CurrentUICulture);
						}
						if (dr["FV_RID"] != System.DBNull.Value)
						{
							versionRID = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
						}
						if (dr["CDR_RID"] != System.DBNull.Value)
						{
							cdrRID = Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture);
						}
						if (hnRID != Include.NoRID)
						{
							hnp = _SAB.HierarchyServerSession.GetNodeData(hnRID, false);
							if (dr["FROM_LEVEL_SEQ"] == System.DBNull.Value)
							{
								fromLevel = hnp.HomeHierarchyLevel;
							}
							else
							{
                                fromLevel = Convert.ToInt32(dr["FROM_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
							}
							if (dr["TO_LEVEL_SEQ"] == System.DBNull.Value)
							{
								toLevel = hnp.HomeHierarchyLevel;
							}
							else
							{
                                toLevel = Convert.ToInt32(dr["TO_LEVEL_SEQ"], CultureInfo.CurrentUICulture);
							}
						}
						if (hnp != null)
						{
							phRID = hnp.HomeHierarchyRID;
						}
						if (dr["FROM_LEVEL_TYPE"] != System.DBNull.Value)
						{
							fromPhOffsetIndFound = true;
                            fromPhOffsetInd = (eHierarchyDescendantType)Convert.ToInt32(dr["FROM_LEVEL_TYPE"], CultureInfo.CurrentUICulture); ;
						}
						if (dr["FROM_LEVEL_OFFSET"] != System.DBNull.Value)
						{
                            fromOffset = Convert.ToInt32(dr["FROM_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
						}
						if (dr["TO_LEVEL_TYPE"] != System.DBNull.Value)
						{
							toPhOffsetIndFound = true;
							toPhOffsetInd = (eHierarchyDescendantType)Convert.ToInt32(dr["TO_LEVEL_TYPE"], CultureInfo.CurrentUICulture);;
						}
                        if (dr["TO_LEVEL_OFFSET"] != System.DBNull.Value)
						{
                            toOffset = Convert.ToInt32(dr["TO_LEVEL_OFFSET"], CultureInfo.CurrentUICulture);
						}
						if (fromPhOffsetIndFound)
						{
							switch (fromPhOffsetInd)
							{
								case eHierarchyDescendantType.offset:
									fromLevel += fromOffset;
									break;
								case eHierarchyDescendantType.levelType:
									break;
								case eHierarchyDescendantType.masterType:
									break;
							}
						}
						if (toPhOffsetIndFound)
						{
							switch (toPhOffsetInd)
							{
								case eHierarchyDescendantType.offset:
									break;
								case eHierarchyDescendantType.levelType:
									break;
								case eHierarchyDescendantType.masterType:
									break;
							}
						}
                        rollDayToWeek = false;
						rollDays = false;
						rollWeeks = true;
						rollStore = Include.ConvertCharToBool(Convert.ToChar(dr["STORE_IND"], CultureInfo.CurrentUICulture));
						rollChain = Include.ConvertCharToBool(Convert.ToChar(dr["CHAIN_IND"], CultureInfo.CurrentUICulture));
						rollStoreToChain = Include.ConvertCharToBool(Convert.ToChar(dr["STORE_TO_CHAIN_IND"], CultureInfo.CurrentUICulture));
						rollIntransit = false;
                        if (rollStore || rollChain)
                        {
                            rollLevel = true;
                        }

						if (rollDayToWeek || rollDays || rollWeeks || rollStore || rollChain || rollStoreToChain || rollLevel || rollIntransit)
						{
							_scheduleRollup = true;
						}

						if (rollIntransit)
						{
							intransitOnly = true;
						}
						if (rollDayToWeek || rollDays || rollWeeks || rollStore || rollChain || rollStoreToChain || rollLevel)
						{
							intransitOnly = false;
						}

						schedulePostingRollup = false;
						if (schedulePostingRollup)
						{
                            message = "Tasklist Roll for Posting Rollup";
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
							_schedulePostingRollup = schedulePostingRollup;
						}

						scheduleReclassRollup = false;
						if (scheduleReclassRollup)
						{
                            message = "Tasklist Roll for Reclass Rollup";
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
							_scheduleReclassRollup = scheduleReclassRollup;
						}

						if (_scheduleRollup)
						{
							DateRangeProfile rollDates = aSession.Calendar.GetDateRange(cdrRID, aSession.Calendar.CurrentDate);
							weekProfileList = aSession.Calendar.GetWeekRange(rollDates, aSession.Calendar.CurrentDate);

                            fv = new ForecastVersion();
                            hpOrg = _SAB.HierarchyServerSession.GetMainHierarchyData();
                            message = "Method ";
                            message += Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentUICulture);
                            message += " (";
                            message += Convert.ToString(dr["USER_FULLNAME"], CultureInfo.CurrentUICulture);
                            message += ") Roll for ";
                            if (phRID != Include.NoRID)
                            {
                                hp = _SAB.HierarchyServerSession.GetHierarchyData(phRID);
                                message += "Hierarchy:" + hp.HierarchyID + ";";
                            }
                            if (hnp == null &&
                                hnRID != Include.NoRID)
                            {
                                hnp = _SAB.HierarchyServerSession.GetNodeData(hnRID, false);
                            }
                            if (hnRID != Include.NoRID)
                            {
                                message += " Merchandise:" + hnp.Text + ";";
                            }
                            if (versionRID != Include.NoRID)
                            {
                                message += " Version:" + fv.GetVersionText(versionRID) + ";";
                            }
                            if (cdrRID != Include.NoRID)
                            {
                                message += " Dates:" + rollDates.DisplayDate + ";";
                            }
                            if (fromPhOffsetInd == eHierarchyDescendantType.levelType &&
                                fromLevel != Include.Undefined)
                            {
                                if (fromLevel == 0)
                                {
                                    message += " From:" + hp.HierarchyID + ";";
                                }
                                else
                                {
                                    message += " From:" + ((HierarchyLevelProfile)hpOrg.HierarchyLevels[fromLevel]).LevelID + ";";
                                }
                            }
                            if (toPhOffsetInd == eHierarchyDescendantType.levelType &&
                                toLevel != Include.Undefined)
                            {
                                if (toLevel == 0)
                                {
                                    message += " To:" + hp.HierarchyID + ";";
                                }
                                else
                                {
                                    message += " To:" + ((HierarchyLevelProfile)hpOrg.HierarchyLevels[toLevel]).LevelID + ";";
                                }
                            }
                            if (fromPhOffsetInd == eHierarchyDescendantType.offset)
                            {
                                message += " From:" + fromOffset + ";";
                            }
                            if (toPhOffsetInd == eHierarchyDescendantType.offset)
                            {
                                if (toLevel == 0)
                                {
                                    message += " To:" + hp.HierarchyID + ";";
                                }
                                else
                                {
                                    message += " To:" + toOffset + ";";
                                }
                            }
                            message += " Options(";
                            if (rollDayToWeek)
                            {
                                message += "Day To Week,";
                            }
                            if (rollDays)
                            {
                                message += "Days,";
                            }
                            if (rollWeeks)
                            {
                                message += "Weeks,";
                            }
                            if (rollStore)
                            {
                                message += "Store,";
                            }
                            if (rollChain)
                            {
                                message += "Chain,";
                            }
                            if (rollStoreToChain)
                            {
                                message += "Store To Chain,";
                            }
                            if (rollLevel)
                            {
                                message += "Levels,";
                            }
                            if (rollIntransit)
                            {
                                message += "Intransit,";
                            }
                            if (_schedulePostingRollup)
                            {
                                message += "Posting,";
                            }
                            if (_scheduleReclassRollup)
                            {
                                message += "Reclass";
                            }
                            message += ")";
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
            			
						
							if (rollChain &&
								rollLevel)
							{
								if (versionRID == Include.FV_ActualRID)
								{
									typeID = Convert.ToInt32(eRollType.chainWeeklyHistory);
								}
								else
								{
									typeID = Convert.ToInt32(eRollType.chainWeeklyForecast);
								}
                                requestsBuiltSuccessfully = BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType,
                                    hnRID, versionRID,
                                    typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, aProcessID);
                                if (requestsBuiltSuccessfully)
                                {
								_rd.CommitData();
							}
                                else
                                {
                                    return false;
                                }
                            }

							if (rollStore || rollDayToWeek || rollDays)
							{
								if (rollDayToWeek)
								{
									typeID = Convert.ToInt32(eRollType.storeDailyHistoryToWeeks);
                                    requestsBuiltSuccessfully = BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                        hnRID, versionRID,
                                        typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, aProcessID);
                                    if (requestsBuiltSuccessfully)
                                    {
									_rd.CommitData();
								}
                                    else
                                    {
                                        return false;
                                    }
                                }
								if (rollDays &&
									rollLevel)
								{
									typeID = Convert.ToInt32(eRollType.storeDailyHistory);
                                    requestsBuiltSuccessfully = BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                        hnRID, versionRID,
                                        typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, aProcessID);
                                    if (requestsBuiltSuccessfully)
                                    {
									_rd.CommitData();
                                    }
                                    else
                                    {
                                        return false;
                                    }
								}
								if (rollWeeks &&
									rollLevel)
								{
									if (versionRID == Include.FV_ActualRID)
									{
										typeID = Convert.ToInt32(eRollType.storeWeeklyHistory);
									}
									else
									{
										typeID = Convert.ToInt32(eRollType.storeWeeklyForecast);
									}
                                    requestsBuiltSuccessfully = BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                        hnRID, versionRID,
                                        typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, aProcessID);
                                    if (requestsBuiltSuccessfully)
                                    {
									_rd.CommitData();
								}
                                    else
                                    {
                                        return false;
                                    }
                                }
							}
							if (rollStoreToChain)
							{
								typeID = Convert.ToInt32(eRollType.storeToChain);
                                requestsBuiltSuccessfully = BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                    hnRID, versionRID,
                                    typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, aProcessID);
                                if (requestsBuiltSuccessfully)
                                {
								_rd.CommitData();
							}
                                else
                                {
                                    return false;
                                }
                            }
							if (rollIntransit)
							{
								typeID = Convert.ToInt32(eRollType.storeExternalIntransit);
                                requestsBuiltSuccessfully = BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                    hnRID, Include.FV_ActualRID,
                                    typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, aProcessID);
                                if (requestsBuiltSuccessfully)
                                {
								_rd.CommitData();
                                }
                                else
                                {
                                    return false;
                                }
							}
						}
					}
				}
				catch(Exception ex)
				{
					aSession.Audit.Log_Exception(ex, GetType().Name);
					return false;
				}
				finally
				{
					_rd.CloseUpdateConnection();
				}
				return true;
			}
			catch ( Exception ex )
			{
				aSession.Audit.Log_Exception(ex, GetType().Name);
				return false;
			}
		}

        public bool BuildRollupRequestsFromSchedule(int aTaskListRID, int aTaskSequence, Session aSession)
        {
            HierarchyNodeProfile hnp = null;
            int hnRID = Include.NoRID;
            int phRID = Include.NoRID;
            int versionRID = Include.NoRID;
            int fromLevel = Include.Undefined;
            int toLevel = Include.Undefined;
            int typeID = 0;
            int cdrRID = 0;
            bool rollDayToWeek = false;
            bool rollDays = false;
            bool rollWeeks = false;
            bool rollStore = false;
            bool rollChain = false;
            bool rollStoreToChain = false;
            bool rollLevel = false;
            bool rollIntransit = false;
            bool fromPhOffsetIndFound = false;
            bool intransitOnly = false;
            eHierarchyDescendantType fromPhOffsetInd = eHierarchyDescendantType.offset;
            int fromOffset = 0;
            bool toPhOffsetIndFound = false;
            eHierarchyDescendantType toPhOffsetInd = eHierarchyDescendantType.offset;
            int toOffset = 0;
            ProfileList weekProfileList = null;
            bool schedulePostingRollup = false;
            bool scheduleReclassRollup = false;
            HierarchyProfile hp = null;
            HierarchyProfile hpOrg = null;
            string message = null;
            ForecastVersion fv;
            

            try
            {
                RequestsOptionsRollupDays rollupDays = RequestsOptionsRollupDays.All;
                if (_requests != null)
                {
                    if (_requests.Options != null)
                    {
                        rollupDays = _requests.Options.RollupDays;
                        message = "Roll Options are ";
                        if (_requests.Options.RollupDays == RequestsOptionsRollupDays.Current)
                        {
                            message += "RollupDays=Current;";
                        }
                        else
                        {
                            message += "RollupDays=All";
                        }
                        if (_requests.Options.HonorLocks)
                        {
                            message += "HonorLocks=True;";
                        }
                        else
                        {
                            message += "HonorLocks=False";
                        }
                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
                    }

                    if (_requests.Variables != null)
                    {
                        foreach (RequestsVariables v in _requests.Variables)
                        {
                            message = "Variable Name=" + v.Name;
                            if (v.ChainForecastLevel != null &&
                                v.ChainForecastLevel.Trim().Length > 0)
                            {
                                message += "ChainForecastLevel=" + v.ChainForecastLevel.Trim() + ";";
                            }
                            if (v.ChainHistoryLevel != null &&
                                v.ChainHistoryLevel.Trim().Length > 0)
                            {
                                message += "ChainHistoryLevel=" + v.ChainHistoryLevel.Trim() + ";";
                            }
                            if (v.StoreDailyHistoryLevel != null &&
                                v.StoreDailyHistoryLevel.Trim().Length > 0)
                            {
                                message += "StoreDailyHistoryLevel=" + v.StoreDailyHistoryLevel.Trim() + ";";
                            }
                            if (v.StoreForecastLevel != null &&
                                v.StoreForecastLevel.Trim().Length > 0)
                            {
                                message += "StoreForecastLevel=" + v.StoreForecastLevel.Trim() + ";";
                            }
                            if (v.StoreWeeklyHistoryLevel != null &&
                                v.StoreWeeklyHistoryLevel.Trim().Length > 0)
                            {
                                message += "StoreWeeklyHistoryLevel=" + v.StoreWeeklyHistoryLevel.Trim() + ";";
                            }
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
                        }
                    }
                }
                
                fv = new ForecastVersion();
                hpOrg = _SAB.HierarchyServerSession.GetMainHierarchyData();
                ScheduleData scheduleData = new ScheduleData();
                DataTable dtTaskRollup = scheduleData.TaskRollup_ReadByTaskList(aTaskListRID, aTaskSequence);
                try
                {
                    _rd.OpenUpdateConnection();
                    foreach (DataRow dr in dtTaskRollup.Rows)
                    {
                        if (dr["HN_RID"] != System.DBNull.Value)
                        {
                            hnRID = Convert.ToInt32(dr["HN_RID"], CultureInfo.CurrentUICulture);
                        }
                        if (dr["TO_PH_RID"] != System.DBNull.Value)
                        {
                            phRID = Convert.ToInt32(dr["TO_PH_RID"], CultureInfo.CurrentUICulture);
                        }
                        if (dr["FV_RID"] != System.DBNull.Value)
                        {
                            versionRID = Convert.ToInt32(dr["FV_RID"], CultureInfo.CurrentUICulture);
                        }
                        if (dr["ROLLUP_CDR_RID"] != System.DBNull.Value)
                        {
                            cdrRID = Convert.ToInt32(dr["ROLLUP_CDR_RID"], CultureInfo.CurrentUICulture);
                        }
                        if (hnRID != Include.NoRID)
                        {
                            hnp = _SAB.HierarchyServerSession.GetNodeData(hnRID, false);
                            if (dr["FROM_PHL_SEQUENCE"] == System.DBNull.Value)
                            {
                                fromLevel = hnp.HomeHierarchyLevel;
                            }
                            else
                            {
                                fromLevel = Convert.ToInt32(dr["FROM_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
                            }
                            if (dr["TO_PHL_SEQUENCE"] == System.DBNull.Value)
                            {
                                toLevel = hnp.HomeHierarchyLevel;
                            }
                            else
                            {
                                toLevel = Convert.ToInt32(dr["TO_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
                            }
                        }
                        if (hnp != null)
                        {
                            phRID = hnp.HomeHierarchyRID;
                        }
                        if (dr["FROM_PH_OFFSET_IND"] != System.DBNull.Value)
                        {
                            fromPhOffsetIndFound = true;
                            fromPhOffsetInd = (eHierarchyDescendantType)Convert.ToInt32(dr["FROM_PH_OFFSET_IND"], CultureInfo.CurrentUICulture); ;
                        }
                        if (dr["FROM_OFFSET"] != System.DBNull.Value)
                        {
                            fromOffset = Convert.ToInt32(dr["FROM_OFFSET"], CultureInfo.CurrentUICulture);
                        }
                        if (dr["TO_PH_OFFSET_IND"] != System.DBNull.Value)
                        {
                            toPhOffsetIndFound = true;
                            toPhOffsetInd = (eHierarchyDescendantType)Convert.ToInt32(dr["TO_PH_OFFSET_IND"], CultureInfo.CurrentUICulture); ;
                        }
                        if (dr["TO_OFFSET"] != System.DBNull.Value)
                        {
                            toOffset = Convert.ToInt32(dr["TO_OFFSET"], CultureInfo.CurrentUICulture);
                        }
                        if (fromPhOffsetIndFound)
                        {
                            switch (fromPhOffsetInd)
                            {
                                case eHierarchyDescendantType.offset:
                                    fromLevel += fromOffset;
                                    break;
                                case eHierarchyDescendantType.levelType:
                                    break;
                                case eHierarchyDescendantType.masterType:
                                    break;
                            }
                        }
                        if (toPhOffsetIndFound)
                        {
                            switch (toPhOffsetInd)
                            {
                                case eHierarchyDescendantType.offset:
                                    //BEGIN TT#4829 - DOConnell - Alternate Hierarchy Rollup Executed from Incorrect Level
                                    if (hnp.HomeHierarchyType == eHierarchyType.alternate)
                                    {
                                        toLevel = toOffset;
                                    }
                                    //END TT#4829 - DOConnell - Alternate Hierarchy Rollup Executed from Incorrect Level
                                    break;
                                case eHierarchyDescendantType.levelType:
                                    break;
                                case eHierarchyDescendantType.masterType:
                                    break;
                            }
                        }
                        rollDayToWeek = Include.ConvertCharToBool(Convert.ToChar(dr["DAY_TO_WEEK_IND"], CultureInfo.CurrentUICulture));
                        rollDays = Include.ConvertCharToBool(Convert.ToChar(dr["DAY_IND"], CultureInfo.CurrentUICulture));
                        rollWeeks = Include.ConvertCharToBool(Convert.ToChar(dr["WEEK_IND"], CultureInfo.CurrentUICulture));
                        rollStore = Include.ConvertCharToBool(Convert.ToChar(dr["STORE_IND"], CultureInfo.CurrentUICulture));
                        rollChain = Include.ConvertCharToBool(Convert.ToChar(dr["CHAIN_IND"], CultureInfo.CurrentUICulture));
                        rollStoreToChain = Include.ConvertCharToBool(Convert.ToChar(dr["STORE_TO_CHAIN_IND"], CultureInfo.CurrentUICulture));
                        if (dr["INTRANSIT_IND"] != System.DBNull.Value)
                        {
                            rollIntransit = Include.ConvertCharToBool(Convert.ToChar(dr["INTRANSIT_IND"], CultureInfo.CurrentUICulture));
                        }
                        rollLevel = Include.ConvertCharToBool(Convert.ToChar(dr["HIERARCHY_LEVELS_IND"], CultureInfo.CurrentUICulture));
                        if (rollDayToWeek || rollDays || rollWeeks || rollStore || rollChain || rollStoreToChain || rollLevel || rollIntransit)
                        {
                            _scheduleRollup = true;
                        }

                        if (rollIntransit)
                        {
                            intransitOnly = true;
                        }
                        if (rollDayToWeek || rollDays || rollWeeks || rollStore || rollChain || rollStoreToChain || rollLevel)
                        {
                            intransitOnly = false;
                        }

                        schedulePostingRollup = Include.ConvertCharToBool(Convert.ToChar(dr["POSTING_IND"], CultureInfo.CurrentUICulture));
                        if (schedulePostingRollup)
                        {
                            message = "Tasklist Roll for Posting Rollup";
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
                            _schedulePostingRollup = schedulePostingRollup;
                        }

                        if (dr["RECLASS_IND"] != System.DBNull.Value)
                        {
                            scheduleReclassRollup = Include.ConvertCharToBool(Convert.ToChar(dr["RECLASS_IND"], CultureInfo.CurrentUICulture));
                            if (scheduleReclassRollup)
                            {
                                message = "Tasklist Roll for Reclass Rollup";
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
                                _scheduleReclassRollup = scheduleReclassRollup;
                            }
                        }

                        if (_scheduleRollup)
                        {
                            DateRangeProfile rollDates = aSession.Calendar.GetDateRange(cdrRID, aSession.Calendar.CurrentDate);
                            weekProfileList = aSession.Calendar.GetWeekRange(rollDates, aSession.Calendar.CurrentDate);

                            message = "Tasklist Roll for ";
                            if (phRID != Include.NoRID)
                            {
                                hp = _SAB.HierarchyServerSession.GetHierarchyData(phRID);
                                message += "Hierarchy:" + hp.HierarchyID + ";";
                            }
                            if (hnp == null &&
                                hnRID != Include.NoRID)
                            {
                                hnp = _SAB.HierarchyServerSession.GetNodeData(hnRID, false);
                            }
                            if (hnRID != Include.NoRID)
                            {
                                message += " Merchandise:" + hnp.Text + ";";
                            }
                            if (versionRID != Include.NoRID)
                            {
                                message += " Version:" + fv.GetVersionText(versionRID) + ";";
                            }
                            if (cdrRID != Include.NoRID)
                            {
                                message += " Dates:" + rollDates.DisplayDate + ";";
                            }
                            if (fromPhOffsetInd == eHierarchyDescendantType.levelType &&
                                fromLevel != Include.Undefined)
                            {
                                if (fromLevel == 0)
                                {
                                    message += " From:" + hp.HierarchyID + ";";
                                }
                                else
                                {
                                    message += " From:" + ((HierarchyLevelProfile)hpOrg.HierarchyLevels[fromLevel]).LevelID + ";";
                                }
                            }
                            if (toPhOffsetInd == eHierarchyDescendantType.levelType && 
                                toLevel != Include.Undefined)
                            {
                                if (toLevel == 0)
                                {
                                    message += " To:" + hp.HierarchyID + ";";
                                }
                                else
                                {
                                    message += " To:" + ((HierarchyLevelProfile)hpOrg.HierarchyLevels[toLevel]).LevelID + ";";
                                }
                            }
                            if (fromPhOffsetInd == eHierarchyDescendantType.offset)
                            {
                                message += " From:" + fromOffset + ";";
                            }
                            if (toPhOffsetInd == eHierarchyDescendantType.offset)
                            {
                                if (toLevel == 0)
                                {
                                    message += " To:" + hp.HierarchyID + ";";
                                }
                                else
                                {
                                    message += " To:" + toOffset + ";";
                                }
                            }
                            message += " Options(";
                            if (rollDayToWeek)
                            {
                                message += "Day To Week,";
                            }
                            if (rollDays)
                            {
                                message += "Days,";
                            }
                            if (rollWeeks)
                            {
                                message += "Weeks,";
                            }
                            if (rollStore)
                            {
                                message += "Store,";
                            }
                            if (rollChain)
                            {
                                message += "Chain,";
                            }
                            if (rollStoreToChain)
                            {
                                message += "Store To Chain,";
                            }
                            if (rollLevel)
                            {
                                message += "Levels,";
                            }
                            if (rollIntransit)
                            {
                                message += "Intransit,";
                            }
                            if (_schedulePostingRollup)
                            {
                                message += "Posting,";
                            }
                            if (_scheduleReclassRollup)
                            {
                                message += "Reclass";
                            }
                            message += ")";
                            _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
                        

                            if (rollChain &&
                                rollLevel)
                            {
                                if (versionRID == Include.FV_ActualRID)
                                {
                                    typeID = Convert.ToInt32(eRollType.chainWeeklyHistory);
                                }
                                else
                                {
                                    typeID = Convert.ToInt32(eRollType.chainWeeklyForecast);
								}
                                BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                    hnRID, versionRID,
                                    typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, (int)eProcesses.rollup);
                                _rd.CommitData();
                            }

                            if (rollStore || rollDayToWeek || rollDays)
                            {
                                if (rollDayToWeek)
                                {
                                    typeID = Convert.ToInt32(eRollType.storeDailyHistoryToWeeks);
                                    BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                        hnRID, versionRID,
                                        typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, (int)eProcesses.rollup);
                                    _rd.CommitData();
                                }
                                if (rollDays &&
                                    rollLevel)
                                {
                                    typeID = Convert.ToInt32(eRollType.storeDailyHistory);
                                    BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                        hnRID, versionRID,
                                        typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, (int)eProcesses.rollup);
                                    _rd.CommitData();
                                }
                                if (rollWeeks &&
                                    rollLevel)
                                {
                                    if (versionRID == Include.FV_ActualRID)
                                    {
                                        typeID = Convert.ToInt32(eRollType.storeWeeklyHistory);
                                    }
                                    else
                                    {
                                        typeID = Convert.ToInt32(eRollType.storeWeeklyForecast);
									}
                                    BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                        hnRID, versionRID,
                                        typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, (int)eProcesses.rollup);
									_rd.CommitData();
								}
							}
							if (rollStoreToChain)
							{
								typeID = Convert.ToInt32(eRollType.storeToChain);
                                BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                    hnRID, versionRID,
                                    typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, (int)eProcesses.rollup);
								_rd.CommitData();
							}
							if (rollIntransit)
							{
								typeID = Convert.ToInt32(eRollType.storeExternalIntransit);
                                BuildRollupRequests(weekProfileList, phRID, hnp.HomeHierarchyType, 
                                    hnRID, Include.FV_ActualRID,
                                    typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, rollupDays, intransitOnly, (int)eProcesses.rollup);
								_rd.CommitData();
							}
						}
                    }
                }
                catch (Exception ex)
                {
                    aSession.Audit.Log_Exception(ex, GetType().Name);
                    return false;
                }
                finally
                {
                    _rd.CloseUpdateConnection();
                }
                return true;
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
                return false;
            }
        }

        public bool BuildRollupRequests(ProfileList aWeekProfileList, int aPhRID, eHierarchyType aHierarchyType,
            int aHnRID, int aVersionRID,
            int aTypeID, eHierarchyDescendantType aFromLevelType, int aFromLevel,
            eHierarchyDescendantType aToLevelType, int aToLevel, 
            Session aSession, RequestsOptionsRollupDays aRollupDays,
            bool aIntransitOnly, int aProcessID)
        {
            //BEGIN TT#4586-VStuart-Change to validate requests before processing-MID
            HierarchyProfile hp = null;
            try
            {
                int firstDayOfWeek = 0;
                int lastDayOfWeek = 0;
                int firstDayOfNextWeek = 0;
                hp = _SAB.HierarchyServerSession.GetMainHierarchyData();

                //BEGIN TT#4586-VStuart-Change to validate requests before processing-MID
                if (aHierarchyType == eHierarchyType.alternate)
                {
                    //BEGIN TT#5066-VStuart-Alternate Hierarchies not rolling properly-BonTon
                    if (aFromLevelType == eHierarchyDescendantType.offset)
                    { 
                        //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                        //int longestBranchCount = _SAB.HierarchyServerSession.GetLongestBranch(aHnRID, true);
                        HierarchyNodeProfile merchandise = _SAB.HierarchyServerSession.GetNodeData(nodeRID: aHnRID);
                        DataTable hierarchyLevels = _SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHnRID);
                        int longestBranchCount = hierarchyLevels.Rows.Count + merchandise.HomeHierarchyLevel - 1;
                        //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly

                        if (!(aFromLevel <= longestBranchCount))
                        {
                            string message = MIDText.GetText(eMIDTextCode.msg_FromLevelInvalid);
                            message = message.Replace("{0}", aFromLevel.ToString());
                            aSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, GetType().Name);
                            return false;
                        }

                        if (!(aToLevel <= longestBranchCount))
                        {
                            string message = MIDText.GetText(eMIDTextCode.msg_ToLevelInvalid);
                            message = message.Replace("{0}", aToLevel.ToString());
                            aSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, GetType().Name);
                            return false;
                        }
                    }
                    //END TT#566-VStuart-Alternate Hierarchies not rolling properly-BonTon
                }
                else //This is an Organizational Hierarchy.
                {
                    int orgLevelsCount = hp.HierarchyLevels.Count;
                    if (!(aFromLevel <= orgLevelsCount))
                    {
                        string message = MIDText.GetText(eMIDTextCode.msg_FromLevelInvalid);
                        message = message.Replace("{0}", aFromLevel.ToString());
                        aSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, GetType().Name);
                        return false;
                    }

                    if (!(aToLevel <= orgLevelsCount))
                    {
                        string message = MIDText.GetText(eMIDTextCode.msg_ToLevelInvalid);
                        message = message.Replace("{0}", aToLevel.ToString());
                        aSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, GetType().Name);
                        return false;
                    }

                }
                //END TT#4586-VStuart-Change to validate requests before processing-MID
                //END TT#4586-VStuart-Change to validate requests before processing-MID

                //BEGIN TT4676- DOConnell - Daily Rollup Message
                bool versionOk = false; 

                if (_forecastVersions == null || _forecastVersions.Count == 0)
                {
                    GetForecastVersions();
                }
                //END TT4676- DOConnell - Daily Rollup Message

                //BEGIN TT#4586-VStuart-Change to validate requests before processing-MID
                foreach (DictionaryEntry val in _forecastVersions)
                {
                    if (Convert.ToInt32(val.Value) == aVersionRID)
                    {
                        versionOk = true; //TT4676- DOConnell - Daily Rollup Message
                        break;
                    }
                    //BEGIN TT4676- DOConnell - Daily Rollup Message
                    //else
                    //{
                    //    string message = MIDText.GetText(eMIDTextCode.lbl_Inactive);
                    //    aSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, GetType().Name);
                    //    return false;
                    //}
                    //END TT4676- DOConnell - Daily Rollup Message
                }
                //END TT#4586-VStuart-Change to validate requests before processing-MID

                //BEGIN TT4676- DOConnell - Daily Rollup Message
                if (!versionOk)
                {
                    string message = MIDText.GetText(eMIDTextCode.msg_VersionNotFound);
                    aSession.Audit.Add_Msg(eMIDMessageLevel.Edit, message, GetType().Name);
                    return false;
                }
                //END TT4676- DOConnell - Daily Rollup Message

                string sTimeID = null;   // TT#5485 - JSmith - Performance

                foreach (WeekProfile weekProfile in aWeekProfileList)
                {
                    if (aTypeID == Convert.ToInt32(eRollType.storeDailyHistory) ||
                        aTypeID == Convert.ToInt32(eRollType.storeExternalIntransit))
                    {
                        if (aRollupDays == RequestsOptionsRollupDays.Current)
                        {
                            foreach (DayProfile dayProfile in weekProfile.Days)
                            {
                                if (dayProfile.Key == aSession.Calendar.CurrentDate.Key ||
                                    dayProfile.Key == aSession.Calendar.PostDate.Key)
                                {
								    // Begin TT#5485 - JSmith - Performance
                                    if (sTimeID == null)
                                    {
                                        sTimeID = dayProfile.Key.ToString();
                                    }
                                    else
                                    {
                                        sTimeID += "," + dayProfile.Key.ToString();
                                    }
                                    //if (aHierarchyType == eHierarchyType.alternate &&
                                    //    aFromLevelType == eHierarchyDescendantType.levelType)
                                    //{
                                    //    _rd.BuildAlternateRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                                    //        dayProfile.Key, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                                    //}
                                    //else
                                    //{
                                    //    _rd.BuildRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                                    //        dayProfile.Key, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                                    //}
									// End TT#5485 - JSmith - Performance
                                }
                            }
                        }
                        else
                        {
                            foreach (DayProfile dayProfile in weekProfile.Days)
                            {
                                if (aIntransitOnly)
                                {
                                    if (dayProfile.Key >= aSession.Calendar.CurrentDate.Key)
                                    {
									    // Begin TT#5485 - JSmith - Performance
                                        if (sTimeID == null)
                                        {
                                            sTimeID = dayProfile.Key.ToString();
                                        }
                                        else
                                        {
                                            sTimeID += "," + dayProfile.Key.ToString();
                                        }
                                        //if (aHierarchyType == eHierarchyType.alternate &&
                                        //aFromLevelType == eHierarchyDescendantType.levelType)
                                        //{
                                        //    _rd.BuildAlternateRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                                        //        dayProfile.Key, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                                        //}
                                        //else
                                        //{
                                        //    _rd.BuildRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                                        //        dayProfile.Key, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                                        //}
										// End TT#5485 - JSmith - Performance
                                    }
                                }
                                else
                                {
     								// Begin TT#5485 - JSmith - Performance
                                    if (sTimeID == null)
                                    {
                                        sTimeID = dayProfile.Key.ToString();
                                    }
                                    else
                                    {
                                        sTimeID += "," + dayProfile.Key.ToString();
                                    }
                                    //if (aHierarchyType == eHierarchyType.alternate &&
                                    //    aFromLevelType == eHierarchyDescendantType.levelType)
                                    //{
                                    //    _rd.BuildAlternateRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                                    //        dayProfile.Key, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                                    //}
                                    //else
                                    //{
                                    //    _rd.BuildRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                                    //        dayProfile.Key, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                                    //}
									// End TT#5485 - JSmith - Performance
                                }
                            }
                        }
                    }
                    else if (aTypeID == Convert.ToInt32(eRollType.storeDailyHistoryToWeeks))
                    {
                        // only schedule the last day of the week
                        DayProfile dayProfile = (DayProfile)weekProfile.Days[weekProfile.Days.Count - 1];
                        firstDayOfWeek = dayProfile.Week.Days[0].Key;
                        lastDayOfWeek = dayProfile.Week.Days[dayProfile.Week.Days.Count - 1].Key;
                        firstDayOfNextWeek = aSession.Calendar.AddWeeks(dayProfile.Week.Key, 1);
						// Begin TT#5485 - JSmith - Performance
                        if (sTimeID == null)
                        {
                            sTimeID = dayProfile.Key.ToString();
                        }
                        else
                        {
                            sTimeID += "," + dayProfile.Key.ToString();
                        }
                        //if (aHierarchyType == eHierarchyType.alternate &&
                        //                aFromLevelType == eHierarchyDescendantType.levelType)
                        //{
                        //    _rd.BuildAlternateRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                        //        dayProfile.Key, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                        //}
                        //else
                        //{
                        //    _rd.BuildRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                        //        dayProfile.Key, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                        //}
						// End TT#5485 - JSmith - Performance
                    }
                    else
                    {
					    // Begin TT#5485 - JSmith - Performance
                        if (sTimeID == null)
                        {
                            sTimeID = weekProfile.Key.ToString();
                        }
                        else
                        {
                            sTimeID += "," + weekProfile.Key.ToString();
                        }
                        //if (aHierarchyType == eHierarchyType.alternate &&
                        //                aFromLevelType == eHierarchyDescendantType.levelType)
                        //{
                        //    _rd.BuildAlternateRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                        //        weekProfile.Key, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                        //}
                        //else
                        //{
                        //    _rd.BuildRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                        //        weekProfile.Key, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                        //}
						// End TT#5485 - JSmith - Performance
                    }
                }

                // Begin TT#5485 - JSmith - Performance
                if (sTimeID != null)
                {
                    if (aHierarchyType == eHierarchyType.alternate &&
                                        aFromLevelType == eHierarchyDescendantType.levelType)
                    {
                        _rd.BuildAlternateRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                            sTimeID, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                    }
                    else
                    {
                        _rd.BuildRollupItems(aProcessID, aPhRID, aHnRID, aVersionRID,
                            sTimeID, aTypeID, aFromLevel, aToLevel, firstDayOfWeek, lastDayOfWeek, firstDayOfNextWeek);
                    }
                }
				// End TT#5485 - JSmith - Performance

                return true;
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
                return false;
            }
        }


        public bool BuildRollupRequestsFromFile(string aInputFile, Session aSession)
        {
            string message = null;
            try
            {
                if (LoadRequestFile(aInputFile, aSession))
                {
                    try
                    {
                        RequestsOptionsRollupDays rollupDays = RequestsOptionsRollupDays.All;
                        if (_requests != null)
                        {
                            if (_requests.Options != null)
                            {
                                rollupDays = _requests.Options.RollupDays;
                                
                                message = "Roll Options are ";
                                if (_requests.Options.RollupDays == RequestsOptionsRollupDays.Current)
                                {
                                    message += "RollupDays=Current;";
                                }
                                else
                                {
                                    message += "RollupDays=All";
                                }
                                if (_requests.Options.HonorLocks)
                                {
                                    message += "HonorLocks=True;";
                                }
                                else
                                {
                                    message += "HonorLocks=False";
                                }
                                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
                            }

                            if (_requests.Variables != null)
                            {
                                foreach (RequestsVariables v in _requests.Variables)
                                {
                                    message = "Variable Name=" + v.Name;
                                    if (v.ChainForecastLevel != null &&
                                        v.ChainForecastLevel.Trim().Length > 0)
                                    {
                                        message += "ChainForecastLevel=" + v.ChainForecastLevel.Trim() + ";";
                                    }
                                    if (v.ChainHistoryLevel != null &&
                                        v.ChainHistoryLevel.Trim().Length > 0)
                                    {
                                        message += "ChainHistoryLevel=" + v.ChainHistoryLevel.Trim() + ";";
                                    }
                                    if (v.StoreDailyHistoryLevel != null &&
                                        v.StoreDailyHistoryLevel.Trim().Length > 0)
                                    {
                                        message += "StoreDailyHistoryLevel=" + v.StoreDailyHistoryLevel.Trim() + ";";
                                    }
                                    if (v.StoreForecastLevel != null &&
                                        v.StoreForecastLevel.Trim().Length > 0)
                                    {
                                        message += "StoreForecastLevel=" + v.StoreForecastLevel.Trim() + ";";
                                    }
                                    if (v.StoreWeeklyHistoryLevel != null &&
                                        v.StoreWeeklyHistoryLevel.Trim().Length > 0)
                                    {
                                        message += "StoreWeeklyHistoryLevel=" + v.StoreWeeklyHistoryLevel.Trim() + ";";
                                    }
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
                                }
                            }
                        

                            if (_requests.Request != null)
                            {
                                GetForecastVersions();
                                foreach (RequestsRequest r in _requests.Request)
                                {
                                    message = "File Roll for ";
                                    if (r.Product != null)
                                    {
                                        message += " Merchandise:" + r.Product + ";";
                                    }
                                    if (r.Version != null)
                                    {
                                        message += " Version:" + r.Version + ";";
                                    }
                                    if (r.DateType == RequestsRequestDateType.Fiscal)
                                    {
                                        message += " Date Type:Fiscal;";
                                    }
                                    else
                                    {
                                        message += " Date Type:Calendar;";
                                    }
                                    if (r.FromDate != null)
                                    {
                                        message += " Dates:" + r.FromDate + "-" + r.ToDate + ";";
                                    }
                                    if (r.FromLevel != null)
                                    {
                                        message += " From:" + r.FromLevel + ";";
                                    }
                                    if (r.ToLevel != null)
                                    {
                                        message += " To:" + r.ToLevel + ";";
                                    }
                                    message += " Options(";
                                    if (r.Type == RequestsRequestType.DayToWeek)
                                    {
                                        message += "Day To Week,";
                                    }
                                    if (r.Period == RequestsRequestPeriod.Day)
                                    {
                                        message += "Days,";
                                    }
                                    if (r.Period == RequestsRequestPeriod.Week)
                                    {
                                        message += "Weeks,";
                                    }
                                    if (r.Data == RequestsRequestData.Store ||
                                        r.Data == RequestsRequestData.All)
                                    {
                                        message += "Store,";
                                    }
                                    if (r.Data == RequestsRequestData.Chain ||
                                        r.Data == RequestsRequestData.All)
                                    {
                                        message += "Chain,";
                                    }
                                    if (r.Type == RequestsRequestType.StoreToChain)
                                    {
                                        message += "Store To Chain,";
                                    }
                                    if (r.Type == RequestsRequestType.Level)
                                    {
                                        message += "Levels,";
                                    }
                                    if (r.Type == RequestsRequestType.Intransit)
                                    {
                                        message += "Intransit,";
                                    }
                                    if (r.Type == RequestsRequestType.Posting)
                                    {
                                        message += "Posting,";
                                    }
                                    if (r.Type == RequestsRequestType.Reclass)
                                    {
                                        message += "Reclass";
                                    }
                                    message += ")";
                                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, GetType().Name, true);
                                    

                                    if (r.Data == RequestsRequestData.All)
                                    {
                                        // process chain
                                        if (r.Type != RequestsRequestType.StoreToChain &&
                                            r.Type != RequestsRequestType.DayToWeek &&
                                            r.Type != RequestsRequestType.Intransit &&
                                            !(r.Type == RequestsRequestType.Level && r.Period == RequestsRequestPeriod.Day))
                                        {
                                            BuildRollupRequests(aSession, r.Type, r.Product, r.Version, RequestsRequestData.Chain, r.Period,
                                                r.DateType, r.FromDate, r.ToDate, r.FromLevel, r.ToLevel, r.OverrideVariableLevels, rollupDays);
                                        }
                                        // process store
                                        BuildRollupRequests(aSession, r.Type, r.Product, r.Version, RequestsRequestData.Store, r.Period,
                                            r.DateType, r.FromDate, r.ToDate, r.FromLevel, r.ToLevel, r.OverrideVariableLevels, rollupDays);
                                    }
                                    else
                                    {
                                        BuildRollupRequests(aSession, r.Type, r.Product, r.Version, r.Data, r.Period,
                                            r.DateType, r.FromDate, r.ToDate, r.FromLevel, r.ToLevel, r.OverrideVariableLevels, rollupDays);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        message = ex.ToString();
                        message = String.Format("Error encountered while processing the file '{0}'", aInputFile);
                        aSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, GetType().Name);
                        return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
                return false;
            }
        }

        private bool LoadRequestFile(string aInputFile, Session aSession)
        {
            string message = null;
            try
            {
                if (aInputFile == null) 
                {
                    //--Everythings OK use presets------------
                    return true;
                }
				else if(!File.Exists(aInputFile))	// Make sure our file exists before attempting to deserialize
                {
                    message = " : " + aInputFile;
                    aSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_InputFileNotFound, message, GetType().Name);
                    return false;
                }
                TextReader r = null;
                try
                {
                    /* I created MIDRetail.ROLLUP.RollupSchema.xsd to define and validate
                            what a Rollup XML file should look like. From the Visual Studio command prompt I
                            run xsd /c RollupSchema.xsd to generate a class file that is a strongly typed
                            represenation of that schema. 
                        */
                    if (_requests == null)
                    {
                        XmlSerializer s = new XmlSerializer(typeof(Requests));	// Create a Serializer
                        r = new StreamReader(aInputFile);			// Load the Xml File
                        _requests = (Requests)s.Deserialize(r);					// Deserialize the Xml File to a strongly typed object
                        if (_requests.Options != null)
                        {
                            _honorLocks = _requests.Options.HonorLocks;
                        }
                    }// Close the input file.
                }
                catch (Exception ex)
                {
                    message = ex.ToString();
                    message = String.Format("Error encountered during deserialization of the file '{0}'", aInputFile);
                    aSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, GetType().Name);
                    return false;
                }
                // Begin Track #4229 - JSmith - API locks .XML input file
                finally
                {
                    if (r != null)
                        r.Close();
                }
                // End Track #4229
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
                return false;
            }
     
            return true;
        }

        private eReturnCode BuildRollupRequests(Session aSession, RequestsRequestType aType,
            string aProduct, string aVersion, RequestsRequestData aData, RequestsRequestPeriod aPeriod,
            RequestsRequestDateType aDateType, string aFromDate, string aToDate, string aFromLevel, string aToLevel,
            bool aOverrideVariableLevels, RequestsOptionsRollupDays aRollupDays)
        {
            eReturnCode returnCode = eReturnCode.successful;
            string message = null;
            string version = "Actual";
            int versionRID = Include.NoRID;
            int typeID = 0;
            HierarchyNodeProfile hnp = null;
            HierarchyProfile hp = null;

            eCalendarRangeType fromDateType = eCalendarRangeType.Static;
            DateTime fromDateStatic = Include.UndefinedDate;
            int fromDateDynamic = 0;
            DateTime toDateStatic = Include.UndefinedDate;
            int toDateDynamic = 0;

            int fromLevel = 0;
            int toLevel = 0;

            ProfileList weekProfileList = null;
            eHierarchyDescendantType fromPhOffsetInd = eHierarchyDescendantType.offset;
            eHierarchyDescendantType toPhOffsetInd = eHierarchyDescendantType.offset;

            try
            {
                EditMsgs em = new EditMsgs();
                if (aType == RequestsRequestType.Posting)
                {
                    _schedulePostingRollup = true;
                }
                else if (aType == RequestsRequestType.Reclass)
                {
                    _scheduleReclassRollup = true;
                }
                else
                {
					message =  "Product: " + aProduct + "; Version: " + aVersion + "; From Date: " + aFromDate + "; To Date: " + aToDate + "; From Level: " + aFromLevel + "; To Level: " + aToLevel + "; Type: " + aType.ToString(CultureInfo.CurrentCulture);

                    if (aProduct == null)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductRequired, message, GetType().Name);
                    }
                    else
                    {
                        hnp = _SAB.HierarchyServerSession.GetNodeData(aProduct);
                        if (hnp.Key == Include.NoRID)
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_ProductNotFound, message, GetType().Name);
                        }
                        else
                        {
                            // get hierarchy information for level lookup
                            hp = _SAB.HierarchyServerSession.GetHierarchyData(hnp.HomeHierarchyRID);
                        }
                    }

                    if (aVersion == null)
                    {
                        em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VersionRequired, message, GetType().Name);
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
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_VersionNotFound, message, GetType().Name);
                        }
                    }

                    // Begin TT#5479 - JSmith - Rollup should not build requests for hierarchies not set as API
                    if (
                        (hp == null
                        || hp.HierarchyRollupOption != eHierarchyRollupOption.API)
                        && versionRID == Include.FV_ActualRID
                       )
                    {
                        message = MIDText.GetText(eMIDTextCode.msg_HierarchyNotAPIRollup, hp.HierarchyID);
                        em.AddMsg(eMIDMessageLevel.Edit, message, GetType().Name);
                    }
                    // End TT#5479 - JSmith - Rollup should not build requests for hierarchies not set as API

                    //determine from date
                    if (aDateType == RequestsRequestDateType.Fiscal)
                    {
                        try
                        {
                            WeekProfile fiscalWeek = aSession.Calendar.GetFiscalWeek(Convert.ToInt32(aFromDate));
                            fromDateStatic = fiscalWeek.Date;
                            fromDateType = eCalendarRangeType.Static;
                        }
                        catch (SystemException e)
                        {
                            string exceptionMessage = e.Message;
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DateNotFound, message, GetType().Name);
                        }
                    }
                    else
                    {
                        try
                        {
                            fromDateStatic = Convert.ToDateTime(aFromDate);
                            fromDateType = eCalendarRangeType.Static;
                        }
                        catch
                        {
                            try
                            {
                                fromDateDynamic = Convert.ToInt32(aFromDate);
                                fromDateType = eCalendarRangeType.Dynamic;
                            }
                            catch (SystemException e)
                            {
                                string exceptionMessage = e.Message;
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DateNotFound, message, GetType().Name);
                            }
                        }
                    }

                    //determine to date
                    if (aDateType == RequestsRequestDateType.Fiscal)
                    {
                        try
                        {
                            WeekProfile fiscalWeek = aSession.Calendar.GetFiscalWeek(Convert.ToInt32(aToDate));
                            toDateStatic = fiscalWeek.Date;
                        }
                        catch (SystemException e)
                        {
                            string exceptionMessage = e.Message;
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DateNotFound, message, GetType().Name);
                        }
                    }
                    else
                    {
                        try
                        {
                            toDateStatic = Convert.ToDateTime(aToDate);
                        }
                        catch
                        {
                            try
                            {
                                toDateDynamic = Convert.ToInt32(aToDate);
                            }
                            catch (SystemException e)
                            {
                                string exceptionMessage = e.Message;
                                em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_DateNotFound, message, GetType().Name);
                            }
                        }
                    }

                    if (aFromLevel == null)
                    {
                        if (aOverrideVariableLevels)
                        {
                            em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_FromLevelRequired, message, GetType().Name);
                        }
                        else
                        {
                            fromLevel = _lowestRollLevel;
                        }
                    }
                    else
                    {
                        // check for offset
                        try
                        {
                            fromLevel = Convert.ToInt32(aFromLevel);
                             fromPhOffsetInd = eHierarchyDescendantType.offset;
                             // Begin TT#4427 - JSmith - Rolling up wrong level from input file
                             if (hnp != null)
                             {
                                 fromLevel += hnp.HomeHierarchyLevel;
                             }
                             // End TT#4427 - JSmith - Rolling up wrong level from input file
                        }
                        catch
                        {
                            // look for level name if found node
                            if (hnp.Key > Include.NoRID)
                            {
                                fromLevel = DetermineLevel(hp, aFromLevel);
                                if (fromLevel < 0)
                                {
                                    message = MIDText.GetText(eMIDTextCode.msg_FromLevelInvalid);
                                    message = message.Replace("{0}.", aFromLevel);
                                    em.AddMsg(eMIDMessageLevel.Edit, message, GetType().Name);
                                }
                                else
                                {
                                    fromPhOffsetInd = eHierarchyDescendantType.levelType;
                                }
                            }
                        }
                    }


                    if (aToLevel == null)
                    {
                        toLevel = hnp.HomeHierarchyLevel;
                    }
                    else
                    {
                        // check for offset
                        try
                        {
                            toLevel = Convert.ToInt32(aToLevel);
                            toPhOffsetInd = eHierarchyDescendantType.offset;
                        }
                        catch
                        {
                            // look for level name if found node
                            if (hnp.Key > Include.NoRID)
                            {
                                toLevel = DetermineLevel(hp, aToLevel);
                                if (toLevel < 0)
                                {
                                    message = MIDText.GetText(eMIDTextCode.msg_ToLevelInvalid);
                                    message = message.Replace("{0}.", aToLevel);
                                    em.AddMsg(eMIDMessageLevel.Edit, message, GetType().Name);
                                }
                                else
                                {
                                    toPhOffsetInd = eHierarchyDescendantType.levelType;
                                }
                            }
                        }
                    }

                    if (fromLevel < toLevel)
                    {
                        message = MIDText.GetText(eMIDTextCode.msg_FromLessThanToLevel);
                        message = message.Replace("{0}.", aFromLevel);
                        message = message.Replace("{1}.", aToLevel);
                        em.AddMsg(eMIDMessageLevel.Edit, message, GetType().Name);
                    }

                    if (em.ErrorFound)
                    {
                        for (int e = 0; e < em.EditMessages.Count; e++)
                        {
                            EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[e];
                            aSession.Audit.Add_Msg(emm.messageLevel, emm.code, emm.msg, emm.lineNumber, emm.module);
                        }
                        returnCode = eReturnCode.editErrors;
                    }
                    else
                    {
                        if (aType == RequestsRequestType.StoreToChain)
                        {
                            typeID = Convert.ToInt32(eRollType.storeToChain);
                            aData = RequestsRequestData.Store;
                        }
                        else if (aType == RequestsRequestType.Intransit)
                        {
                            typeID = Convert.ToInt32(eRollType.storeExternalIntransit);
                            aData = RequestsRequestData.Store;
                        }
                        else if (aType == RequestsRequestType.DayToWeek)
                        {
                            typeID = Convert.ToInt32(eRollType.storeDailyHistoryToWeeks);
                            aData = RequestsRequestData.Store;
                        }
                        else
                            if (aData == RequestsRequestData.Chain)
                            {
                                if (versionRID == Include.FV_ActualRID)
                                {
                                    typeID = Convert.ToInt32(eRollType.chainWeeklyHistory);
                                }
                                else
                                {
                                    typeID = Convert.ToInt32(eRollType.chainWeeklyForecast);
                                }
                            }
                            else
                            {
                                if (aPeriod == RequestsRequestPeriod.Day)
                                {
                                    typeID = Convert.ToInt32(eRollType.storeDailyHistory);
                                }
                                else if (versionRID == Include.FV_ActualRID)
                                {
                                    typeID = Convert.ToInt32(eRollType.storeWeeklyHistory);
                                }
                                else
                                {
                                    typeID = Convert.ToInt32(eRollType.storeWeeklyForecast);
                                }
                            }
                        if (fromDateType == eCalendarRangeType.Static)
                        {
                            weekProfileList = aSession.Calendar.GetWeekRange(fromDateStatic, toDateStatic);
                        }
                        else
                        {
                    //Begin TT#2042 - MD - Rollup fails on Dynamic Dates - RBeck

                            WeekProfile _cCW = aSession.Calendar.CurrentWeek;
                            WeekProfile _fDD, _tDD;
                            _fDD = aSession.Calendar.ConvertToStaticWeek(_cCW, fromDateDynamic);
                            _tDD = aSession.Calendar.ConvertToStaticWeek(_cCW, toDateDynamic);
                            weekProfileList = aSession.Calendar.GetWeekRange(_fDD, _tDD);

                            //weekProfileList = aSession.Calendar.GetWeekRange(fromDateDynamic, toDateDynamic);

                    //End   TT#2042 - MD - Rollup fails on Dynamic Dates - RBeck
                        }

                        try
                        {
                            _rd.OpenUpdateConnection();
                            BuildRollupRequests(weekProfileList, hnp.HierarchyRID, hnp.HomeHierarchyType, 
                                hnp.Key, versionRID,
                                typeID, fromPhOffsetInd, fromLevel, toPhOffsetInd, toLevel, aSession, aRollupDays, (aType == RequestsRequestType.Intransit), (int)eProcesses.rollup);
                            _rd.CommitData();
                        }
                        catch (Exception ex)
                        {
                            aSession.Audit.Log_Exception(ex, GetType().Name);
                            return eReturnCode.fatal;
                        }
                        finally
                        {
                            if (_rd != null &&
                                _rd.ConnectionIsOpen)
                            {
                                _rd.CloseUpdateConnection();
                            }
                        }
                    }
                }

                return returnCode;
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
                return eReturnCode.fatal;
            }
        }

        public int DetermineLevel(HierarchyProfile aHierarchyProfile, string aLevel)
        {
            int level = -1;
            // check for offset
            try
            {
                // level not defined so do not roll
                if (aLevel.Trim().Length == 0)
                {
                    level = int.MaxValue;
                }
                else
                {
                    level = Convert.ToInt32(aLevel);
                }
            }
            catch
            {
                // look for level name if found node
                if (aHierarchyProfile.HierarchyID == aLevel)
                {
                    level = 0;
                }
                else
                {
                    for (int levelIndex = 1; levelIndex <= aHierarchyProfile.HierarchyLevels.Count; levelIndex++)
                    {
                        HierarchyLevelProfile hlp = (HierarchyLevelProfile)aHierarchyProfile.HierarchyLevels[levelIndex];
                        // disregard case in test
                        if (hlp.LevelID.ToLower() == aLevel.ToLower())
                        {
                            level = hlp.Level;
                            break;
                        }
                    }
                }
            }
            return level;
        }

        public void ProcessPostingRollupRequests(Session aSession)
        {
            try
            {
                // process requests for all hierarchies
				_rd.BuildRollupAncestors((int)eProcesses.historyPlanLoad, false);
                string returnMessage = null;
                eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
                HierarchyProfileList hpl = _SAB.HierarchyServerSession.GetHierarchiesByDependency(ref returnMessage, ref messageLevel);
                if (returnMessage != null)
                {
                    aSession.Audit.Add_Msg(messageLevel, returnMessage, GetType().Name);
                }
                foreach (HierarchyProfile hp in hpl)
                {
                    RollupHierarchy(aSession, eProcesses.historyPlanLoad, hp.Key, false);
                }

                // process requests for alternate hierarchies only
                _rd.BuildRollupAncestors((int)eProcesses.historyPlanLoad, true);
                foreach (HierarchyProfile hp in hpl)
                {
                    RollupHierarchy(aSession, eProcesses.historyPlanLoad, hp.Key, true);
                }
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
            }

        }

        public void ProcessReclassRollupRequests(Session aSession)
        {
            try
            {
                string returnMessage = null;
                eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
                HierarchyProfileList hpl = _SAB.HierarchyServerSession.GetHierarchiesByDependency(ref returnMessage, ref messageLevel);
                if (returnMessage != null)
                {
                    aSession.Audit.Add_Msg(messageLevel, returnMessage, GetType().Name);
                }
                foreach (HierarchyProfile hp in hpl)
                {
                    RollupHierarchy(aSession, eProcesses.hierarchyLoad, hp.Key, false);
                }
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
            }

        }

		private void RollupHierarchy(Session aSession, eProcesses aProcess, int aHierarchyRID, bool aRollAlternatesOnly)
        {
            RollupHierarchy(aSession, (int)aProcess, aHierarchyRID, aRollAlternatesOnly);
        }

		private void RollupHierarchy(Session aSession, int aProcess, int aHierarchyRID, bool aRollAlternatesOnly)
		{
            int totalItems = 0;  // TT#5485 - JSmith - Performance
            try
            {
                try
                {
				    // Begin TT#5485 - JSmith - Performance
					//int totalItems = _rd.GetItemCount(aProcess, aHierarchyRID);
                    totalItems = _rd.GetItemCount(aProcess, aHierarchyRID);
                    if (totalItems == 0)
                    {
                        return;
                    }
					// End TT#5485 - JSmith - Performance
                    _totalItems += totalItems;
                    Stack rollupStack;
                    // use ArrayList to maintain reference until errors are counted
                    ArrayList rollupArrayList = new ArrayList();
                    // get types to roll
                    ArrayList types = _rd.GetRollupTypes(aProcess, aHierarchyRID);

                    foreach (int type in types)
                    {
                        if (type == (int)eRollType.dummyColor)
                        {
                            _rd.BuildDummyColorRollupItems(aProcess);
                        }

                        // get versions to roll
                        ArrayList versions = _rd.GetRollupVersions(aProcess, aHierarchyRID, type);

                        foreach (int version in versions)
                        {
                            // get min and max level for hierarchy
                            int maxLevel = _rd.GetMaxRollupLevel(aProcess, aHierarchyRID, type, version);
                            int minLevel = _rd.GetMinRollupLevel(aProcess, aHierarchyRID, type, version);

                            // walk max to 0 calling stored procedures
                            for (int level = maxLevel; level >= minLevel; --level)
                            {
								int batchCount = _rd.DetermineBatches(aProcess, aHierarchyRID, type, version, level, _batchSize, _SAB.ClientServerSession.GlobalOptions.NumberOfStoreDataTables);
                                _totalBatches += batchCount;
                                if (batchCount > 0)
                                {
                                    rollupStack = new Stack();
                                    BuildRollupStack(_SAB.ClientServerSession.Audit, rollupStack, rollupArrayList, aProcess, aHierarchyRID, (eRollType)type, version, level, batchCount, aSession);
                                    if (_concurrentProcesses > 1)
                                    {
                                        ConcurrentProcessManager cpm = new ConcurrentProcessManager(_SAB.ClientServerSession.Audit, rollupStack, _concurrentProcesses, 5000);
                                        cpm.ProcessCommands();
                                        foreach (ConcurrentProcess cp in rollupArrayList)
                                        {
                                            _totalErrors += cp.NumberOfErrors;
                                        }
                                    }
                                    else
                                    {
                                        while (rollupStack.Count > 0)
                                        {
                                            ConcurrentProcess cp = (ConcurrentProcess)rollupStack.Pop();
                                            cp.ExecuteProcess();
                                            _totalErrors += cp.NumberOfErrors;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Begin TT#2131-MD - JSmith - Halo Integration
                    if (MIDEnvironment.ExtractIsEnabled)
                    {
                        AddExtractPlanningControlRecords(aProcess);
                    }
                    // End TT#2131-MD - JSmith - Halo Integration

                    // remove all processed items
                    if (!_rd.ConnectionIsOpen)
                    {
                        _rd.OpenUpdateConnection();
                    }
                    _rd.DeleteProcessedRollupItems(aRollAlternatesOnly);
                    _rd.CommitData();

                }
                catch (Exception ex)
                {
                    aSession.Audit.Log_Exception(ex, GetType().Name);
                }
                finally
                {
                    // remove all items that are not for Reclass or Posting
					// Begin TT#5485 - JSmith - Performance
                    if (totalItems > 0)
                    {
					// End TT#5485 - JSmith - Performance
                        if (aProcess != (int)eProcesses.historyPlanLoad &&
                        aProcess != (int)eProcesses.hierarchyLoad)
                        {
                            if (!_rd.ConnectionIsOpen)
                            {
                                _rd.OpenUpdateConnection();
                            }
                            _rd.DeleteRollupItems(aProcess, aHierarchyRID);
                            _rd.CommitData();
                        }


                        if (_rd.ConnectionIsOpen)
                        {
                            _rd.CloseUpdateConnection();
                        }
					// Begin TT#5485 - JSmith - Performance
                    }
					// End TT#5485 - JSmith - Performance
                }
            }
            catch (Exception ex)
            {
                aSession.Audit.Log_Exception(ex, GetType().Name);
            }

        }

        // Begin TT#2131-MD - JSmith - Halo Integration
        /// <summary>
        /// Adds Extract Planning Control records for organizational hierarchy weekly values
        /// </summary>
        /// <param name="aProcess"></param>
        /// <returns></returns>
        private bool AddExtractPlanningControlRecords(int aProcess)
        {
            VariablesData varData = new VariablesData(_SAB.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
            HierarchyProfile hp = _SAB.HierarchyServerSession.GetMainHierarchyData();
            DataTable dt = _rd.GetProcessedItems(aProcess, hp.Key);
            int type;
            int writeCount = 0;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    varData.OpenUpdateConnection();
                    foreach (DataRow dr in dt.Rows)
                    {
                        type = Convert.ToInt32(dr["ITEM_TYPE"]);
                        if (type == (int)eRollType.chainWeeklyForecast
                            || type == (int)eRollType.chainWeeklyHistory
                            || type == (int)eRollType.storeToChain
                            || type == (int)eRollType.storeWeeklyForecast
                            || type == (int)eRollType.storeWeeklyHistory)
                        {

                            varData.AddPlanningExtractControlValue(
                            Convert.ToInt32(dr["HN_RID"]),
                            Convert.ToInt32(dr["TIME_ID"]),
                            Convert.ToInt32(dr["FV_RID"]),
                            GetPlanType(type)
                            );
                            ++writeCount;
                            if (writeCount > MIDConnectionString.CommitLimit)
                            {
                                varData.EXTRACT_PLANNING_CONTROL_Update();
                            }
                        }
                    }

                    if (writeCount > 0)
                    {
                        varData.EXTRACT_PLANNING_CONTROL_Update();
                    }
                    varData.CommitData();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                if (varData.ConnectionIsOpen)
                {
                    varData.CloseUpdateConnection();
                }
            }
            return true;
        }

        private ePlanType GetPlanType(int type)
        {
            if (type == (int)eRollType.chainWeeklyForecast
                    || type == (int)eRollType.chainWeeklyHistory
                    || type == (int)eRollType.storeToChain)
            {
                return ePlanType.Chain;
            }
            else
            {
                return ePlanType.Store;
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration

        public void GetForecastVersions()
        {
            string description = null;
            int RID;
            ForecastVersion fv = new ForecastVersion();
            DataTable dt = fv.GetForecastVersions(false);
            foreach (DataRow dr in dt.Rows)
            {
                RID = Convert.ToInt32(dr["FV_RID"]);
                description = (string)dr["DESCRIPTION"];
                _forecastVersions.Add(description.ToLower(), RID);
            }
        }

		private eReturnCode BuildRollupStack(Audit aAudit, Stack aRollupStack, ArrayList rollupArrayList, int aProcess, int aHierarchyRID, eRollType aRollupType, int aVersion, int aLevel, int aBatchCount, Session aSession)
        {
            bool useStaticStoredProcedure = false;
            try
            {
                ArrayList rollupVariables = new ArrayList();

                if (aRollupType != eRollType.dummyColor)
                {
                    ArrayList databaseVariables = null;

                    switch (aRollupType)
                    {
                        case eRollType.storeDailyHistoryToWeeks:
                            databaseVariables = _dayToWeekHistoryDatabaseVariables;
                            break;
                        case eRollType.chainDailyHistory:
                        case eRollType.storeDailyHistory:
                        case eRollType.storeExternalIntransit:
                        case eRollType.storeIntransit:
                            databaseVariables = _storeDailyHistoryDatabaseVariables;
                            break;
                        case eRollType.chainWeeklyForecast:
                            databaseVariables = _chainWeeklyForecastDatabaseVariables;
                            break;
                        case eRollType.chainWeeklyHistory:
                            databaseVariables = _chainWeeklyHistoryDatabaseVariables;
                            break;
                        case eRollType.storeWeeklyHistory:
                            databaseVariables = _storeWeeklyHistoryDatabaseVariables;
                            break;
                        case eRollType.storeWeeklyForecast:
                            databaseVariables = _storeWeeklyForecastDatabaseVariables;
                            break;
                        case eRollType.storeToChain:
                            if (aVersion == Include.FV_ActualRID)
                            {
                                databaseVariables = _storeToChainHistoryDatabaseVariables;
                            }
                            else
                            {
                                databaseVariables = _storeToChainForecastDatabaseVariables;
                            }
                            break;
                    }
                    foreach (VariableProfile vp in databaseVariables)
                    {
                        switch (aRollupType)
                        {
                            case eRollType.storeDailyHistoryToWeeks:
                                if (vp.StoreDailyHistoryRollLevel > 0 &&
                                    (vp.StoreDailyHistoryRollLevel == int.MaxValue ||
                                    aLevel <= vp.StoreDailyHistoryRollLevel))
                                {
                                    rollupVariables.Add(vp);
                                }
                                break;
                            case eRollType.storeDailyHistory:
                                if (vp.StoreDailyHistoryRollLevel > 0 &&
                                    (vp.StoreDailyHistoryRollLevel == int.MaxValue ||
                                    aLevel < vp.StoreDailyHistoryRollLevel))
                                {
                                    rollupVariables.Add(vp);
                                }
                                break;
                            case eRollType.storeWeeklyHistory:
                                if (vp.StoreWeeklyHistoryRollLevel > 0 &&
                                    (vp.StoreWeeklyHistoryRollLevel == int.MaxValue ||
                                    aLevel < vp.StoreWeeklyHistoryRollLevel))
                                {
                                    rollupVariables.Add(vp);
                                }
                                break;
                            case eRollType.storeWeeklyForecast:
                                if (vp.StoreForecastRollLevel > 0 &&
                                    (vp.StoreForecastRollLevel == int.MaxValue ||
                                    aLevel < vp.StoreForecastRollLevel))
                                {
                                    rollupVariables.Add(vp);
                                }
                                break;
                            case eRollType.storeToChain:
                                if (aVersion == Include.FV_ActualRID)
                                {
                                    if (vp.ChainHistoryRollLevel > 0 &&
                                        (vp.ChainHistoryRollLevel == int.MaxValue ||
                                        aLevel <= vp.ChainHistoryRollLevel))
                                    {
                                        rollupVariables.Add(vp);
                                    }
                                }
                                else
                                {
                                    if (vp.ChainForecastRollLevel > 0 &&
                                        (vp.ChainForecastRollLevel == int.MaxValue ||
                                        aLevel <= vp.ChainForecastRollLevel))
                                    {
                                        rollupVariables.Add(vp);
                                    }
                                }
                                break;
                            case eRollType.chainDailyHistory:
                                break;
                            case eRollType.chainWeeklyHistory:
                                if (vp.ChainHistoryRollLevel > 0 &&
                                    (vp.ChainHistoryRollLevel == int.MaxValue ||
                                    aLevel < vp.ChainHistoryRollLevel))
                                {
                                    rollupVariables.Add(vp);
                                }
                                break;
                            case eRollType.chainWeeklyForecast:
                                if (vp.ChainForecastRollLevel > 0 &&
                                    (vp.ChainForecastRollLevel == int.MaxValue ||
                                    aLevel < vp.ChainForecastRollLevel))
                                {
                                    rollupVariables.Add(vp);
                                }
                                break;
                            case eRollType.storeExternalIntransit:
                                if (vp.VariableType == eVariableType.Intransit)
                                {
                                    rollupVariables.Add(vp);
                                }
                                break;
                            case eRollType.storeIntransit:
                                if (vp.VariableType == eVariableType.Intransit)
                                {
                                    rollupVariables.Add(vp);
                                }
                                break;
                        }
                    }
                    // if all variables are used set the rollupVariables to null so 
                    // the static stored procedure will be used.
                    if (aRollupType == eRollType.storeIntransit ||
                        aRollupType == eRollType.storeExternalIntransit ||
                        aRollupType == eRollType.dummyColor ||
                        rollupVariables.Count == databaseVariables.Count)
                    {
                        useStaticStoredProcedure = true;
                    }
                }

                rollupArrayList.Clear();
                ConcurrentProcess rollupProcess = null;
                Stack[] batchNumbersByTable = GetBatchNumbersByTable(aSession, aProcess, aHierarchyRID, aRollupType, aVersion, aLevel);

                int i = 0;
                bool moreBatches = true;
                int numberOfTables = aSession.GlobalOptions.NumberOfStoreDataTables;
                while (moreBatches)
                {
                    moreBatches = false;
                    for (int tableNumber = numberOfTables - 1; tableNumber >= 0; tableNumber--)
                    {
                        if (batchNumbersByTable[tableNumber] != null)
                        {
                            Stack tableStack = (Stack)batchNumbersByTable[tableNumber];
                            if (tableStack.Count > 0)
                            {
                                moreBatches = true;
                                i = (int)tableStack.Pop();
                                switch (aRollupType)
                                {
                                    case eRollType.storeDailyHistoryToWeeks:
                                        rollupProcess = new StoreDayWeekRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "StoreWeeklyHistory" + tableNumber.ToString());
                                        break;
                                    case eRollType.storeDailyHistory:
                                        rollupProcess = new StoreHistoryDayRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "StoreDailyHistory" + tableNumber.ToString());
                                        break;
                                    case eRollType.storeWeeklyHistory:
                                        rollupProcess = new StoreHistoryWeekRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "StoreWeeklyHistory" + tableNumber.ToString());
                                        break;
                                    case eRollType.storeWeeklyForecast:
                                        rollupProcess = new StoreForecastWeekRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "StoreWeeklyForecast" + tableNumber.ToString());
                                        break;
                                    case eRollType.storeToChain:
                                        if (aVersion == Include.FV_ActualRID)
                                        {
                                            rollupProcess = new StoreToChainHistoryRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "ChainWeeklyHistory");
                                        }
                                        else
                                        {
                                            rollupProcess = new StoreToChainForecastRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "ChainWeeklyForecast");
                                        }
                                        break;
                                    case eRollType.chainDailyHistory:
                                        rollupProcess = new ChainHistoryDayRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "ChainDailyHistory");
                                        break;
                                    case eRollType.chainWeeklyHistory:
                                        rollupProcess = new ChainHistoryWeekRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "ChainWeeklyHistory");
                                        break;
                                    case eRollType.chainWeeklyForecast:
                                        rollupProcess = new ChainForecastWeekRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "ChainWeeklyForecast");
                                        break;
                                    case eRollType.storeExternalIntransit:
                                        rollupProcess = new StoreExternalIntransitRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "StoreExternalIntransit");
                                        break;
                                    case eRollType.storeIntransit:
                                        rollupProcess = new StoreIntransitRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "StoreIntransit");
                                        break;
                                    case eRollType.dummyColor:
                                        rollupProcess = new DummyColorExternalIntransitRollupProcess(_SAB, aAudit, rollupVariables, aProcess, aHierarchyRID, aVersion, aLevel, i, tableNumber, _includeZeroInAverage, _honorLocks, _zeroParentsWithNoChildren, useStaticStoredProcedure, "DummyColor");
                                        break;
                                }
                                aRollupStack.Push(rollupProcess);
                                rollupArrayList.Add(rollupProcess);
                            }
                        }
                    }
                }
                return eReturnCode.successful;
            }
            catch (Exception exc)
            {
                aAudit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessRollupBatches", "TaskListProcessor");
                aAudit.Log_Exception(exc, GetType().Name);

                return eReturnCode.severe;
            }
        }

		private Stack[] GetBatchNumbersByTable(Session aSession, int aProcess, int aHierarchyRID, eRollType aRollupType, int aVersion, int aLevel)
        {
            try
            {
                Stack[] batchNumbersByTable = new Stack[aSession.GlobalOptions.NumberOfStoreDataTables];
                DataTable dt = _rd.GetBatchesNumbers(aProcess, aHierarchyRID, (int)aRollupType, aVersion, aLevel);
                Stack tableStack = new Stack();
                int currentTableNumber = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    int tableNumber = Convert.ToInt32(dr["HN_MOD"]);
                    int batchNumber = Convert.ToInt32(dr["BATCH_NUMBER"]);
                    if (tableNumber != currentTableNumber)
                    {
                        batchNumbersByTable[currentTableNumber] = tableStack;
                        currentTableNumber = tableNumber;
                        tableStack = new Stack();
                        tableStack.Push(batchNumber);
                    }
                    else
                    {
                        tableStack.Push(batchNumber);
                    }
                }
                batchNumbersByTable[currentTableNumber] = tableStack;
                return batchNumbersByTable;
            }
            catch
            {
                throw;
            }
        }
    }

    abstract public class RollupProcess : ConcurrentProcess
    {
        //=======
        // FIELDS
        //=======
		private int _process;
        private eRollType _rollType;
        private int _rollupRID;
        private int _hierarchyRID;
        private int _version;
        private int _level;
        private int _batchNumber;
        private string _batchNumberString;
        private int _tableNumber;
        private bool _includeZeroInAverage;
        private bool _honorLocks;
        private bool _zeroParentsWithNoChildren;
        private bool _useStaticStoredProcedure;
        private RollupData _rd = null;
        private SessionAddressBlock _SAB;
        private ArrayList _databaseVariables;
        private StoredProcedure _storedProcedure = null;
        private string _indent5 = "     ";
        private string _indent10 = "          ";
        private string _indent15 = "               ";
        private string _blankLine = new string(' ', 100);

		private string _lockID = "RILock";
		
        //=============
        // CONSTRUCTORS
        //=============

        public RollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, eRollType aRollType, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren,
            bool aUseStaticStoredProcedure, string aProcessID)
            : base(aAudit, aProcessID)
        {
            try
            {
                _SAB = aSAB;
                _process = aProcess;
                _hierarchyRID = aHierarchyRID;
                _rollType = aRollType;
                _version = aVersion;
                _level = aLevel;
                _batchNumber = aBatchNumber;
                _batchNumberString = aBatchNumber.ToString();
                _tableNumber = aTableNumber;
                _includeZeroInAverage = aIncludeZeroInAverage;
                _honorLocks = aHonorLocks;
                _databaseVariables = aDatabaseVariables;
                _zeroParentsWithNoChildren = aZeroParentsWithNoChildren;
                _useStaticStoredProcedure = aUseStaticStoredProcedure;
            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========

        public int Process
        {
            get { return _process; }
        }

        public int RollupRID
        {
            get { return _rollupRID; }
        }

        public int HierarchyRID
        {
            get { return _hierarchyRID; }
        }

        public int Version
        {
            get { return _version; }
        }

        public int Level
        {
            get { return _level; }
        }

        public int BatchNumber
        {
            get { return _batchNumber; }
        }

        public string BatchNumberString
        {
            get { return _batchNumberString; }
        }

        public int TableNumber
        {
            get { return _tableNumber; }
        }

        public bool IncludeZeroInAverage
        {
            get { return _includeZeroInAverage; }
        }

        public bool HonorLocks
        {
            get { return _honorLocks; }
        }

        public bool ZeroParentsWithNoChildren
        {
            get { return _zeroParentsWithNoChildren; }
        }

        public bool UseStaticStoredProcedure
        {
            get { return _useStaticStoredProcedure; }
        }

        public RollupData RollupData
        {
            get
            {
                if (_rd == null)
                {
                    _rd = new RollupData();
                }
                return _rd;
            }
        }

        public SessionAddressBlock SAB
        {
            get { return _SAB; }
        }

        public ArrayList DatabaseVariables
        {
            get { return _databaseVariables; }
        }

        public StoredProcedure StoredProcedure
        {
            get
            {
                return _storedProcedure;
            }
        }

        public string Indent5
        {
            get { return _indent5; }
        }

        public string Indent10
        {
            get { return _indent10; }
        }

        public string Indent15
        {
            get { return _indent15; }
        }

        public string BlankLine
        {
            get
            {
                return new string(' ', 100);
            }
        }

		public string LockID 
		{
			get { return _lockID ; }
		}
		
        //========
        // METHODS
        //========

        public void CreateStoredProcedure(string aName)
        {
            try
            {
                try
                {
                    RollupData.OpenUpdateConnection();
                    _rollupRID = RollupData.AddRollupProcess(SAB.ClientServerSession.Audit.ProcessRID, _rollType, eProcessCompletionStatus.None, BatchNumber);
                    RollupData.CommitData();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (RollupData != null &&
                        RollupData.ConnectionIsOpen)
                    {
                        RollupData.CloseUpdateConnection();
                    }
                }

                _storedProcedure = new StoredProcedure(Audit, aName + _rollupRID.ToString());
                DropStoredProcedure();
            }
            catch (Exception exc)
            {
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        public void CreateStaticStoredProcedure(string aName)
        {
            try
            {
                _storedProcedure = new StoredProcedure(Audit, aName);
            }
            catch 
            {
                throw;
            }
        }
        
        public void WriteStoredProcedure()
        {
            try
            {
#if (DEBUG)
                //				StoredProcedure.OutputSP();
#endif
                if (_storedProcedure != null)
                {
                    RollupData.OpenUpdateConnection();
                    RollupData.ExecuteNonQuery(_storedProcedure.Text);
                    RollupData.CommitData();
                }
            }
            catch (Exception exc)
            {
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
            finally
            {
                if (RollupData.ConnectionIsOpen)
                {
                    RollupData.CloseUpdateConnection();
                }
            }
        }

        public void DropStoredProcedure()
        {
            try
            {
                if (_storedProcedure != null)
                {
                    _storedProcedure.BuildDrop();
                    RollupData.OpenUpdateConnection();
                    RollupData.ExecuteNonQuery(_storedProcedure.Text);
                    RollupData.CommitData();
                    _storedProcedure.ClearText();
                }
            }
            catch (Exception exc)
            {
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
            finally
            {
                if (RollupData.ConnectionIsOpen)
                {
                    RollupData.CloseUpdateConnection();
                }
            }
        }

        public void UpdateRollupProcess(eProcessCompletionStatus aCompletionStatus)
        {
            try
            {
                try
                {
                    RollupData.OpenUpdateConnection();
                    RollupData.UpdateRollupProcess(_rollupRID, aCompletionStatus);
                    RollupData.CommitData();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (RollupData != null &&
                        RollupData.ConnectionIsOpen)
                    {
                        RollupData.CloseUpdateConnection();
                    }
                }
            }
            catch (Exception exc)
            {
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }

    public class StoreDayWeekRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public StoreDayWeekRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.storeDailyHistoryToWeeks, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "StoreDayWeekRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        public string BuildStoredProcedure(string aProcedureName)
        {
            CreateStaticStoredProcedure(aProcedureName);
            BuildStoredProcedure();
            return StoredProcedure.Text;
        }
        
        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			eProcessCompletionStatus status = eProcessCompletionStatus.None;
			
            string message = "Executing store day to week rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumber.ToString();
            try
            {
                if (!UseStaticStoredProcedure &&
                    DatabaseVariables.Count == 0)
                {
                    return;
                }
                
                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "StoreDayWeekRollupProcess");

                try
                {
                    try
                    {
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreDayToWeekHistoryRollupSP.Replace(Include.DBTableCountReplaceString, TableNumber.ToString()));
                        }
                        else
                        {
                            CreateStoredProcedure("SP_MID_ST_HIS_DAY_WEEK_ROLLUP");
                            BuildStoredProcedure();
                            WriteStoredProcedure();
                        }
                        RollupData.OpenUpdateConnection();
                        RollupData.ProcessDaysToWeeksRollup(StoredProcedure.Name, Process, HierarchyRID, (int)eRollType.storeDailyHistoryToWeeks, Version, Level, BatchNumber);
                        RollupData.CommitData();
						status = eProcessCompletionStatus.Successful;
                    }
                    catch (Exception ex)
                    {
                        ++NumberOfErrors;
                        status = eProcessCompletionStatus.Failed;
                        Audit.Log_Exception(ex, GetType().Name);
                    }
                    finally
                    {
                        RollupData.CloseUpdateConnection();
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						UpdateRollupProcess(status);
                    }
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedure()
        {
            int count;
            string line;
            bool isStore = true;
            bool isForecast = false;
            bool isExternalIntransit = false;

            try
            {
                // xxxxxxxxxx
                string dayTableName = "STORE_HISTORY_DAY" + TableNumber.ToString();
                string weekTableName = "STORE_HISTORY_WEEK" + TableNumber.ToString();

                StoredProcedure.AddCreateProcedure(eRollType.storeDailyHistoryToWeeks, isStore, isForecast);

                StoredProcedure.WriteLine("-- set up fields to get first and last values");
                StoredProcedure.WriteLine("DECLARE  @factorm bigint, @factora bigint");
                StoredProcedure.WriteLine("SET @factora = 0x100000000 -- needs to be a power of 2 > maximum user column absolute value");
                StoredProcedure.WriteLine("SET @factorm = @factora * 2");
                StoredProcedure.WriteLine(" ");
                StoredProcedure.WriteLine(" ");
                StoredProcedure.WriteLine("-- get the posting date");
                StoredProcedure.WriteLine("DECLARE  @PostDate INT");
                StoredProcedure.WriteLine("SELECT @PostDate = CURR_DATE_YYYYDDD FROM POSTING_DATE");
                StoredProcedure.WriteLine(" ");

                StoredProcedure.AddRollupItemTable(BatchNumberString, TableNumber, Include.FV_ActualRID.ToString(), isStore, isExternalIntransit, isDayToWeek : true);

                StoredProcedure.AddTempSumTable(BatchNumberString, DatabaseVariables, isStore, isForecast, isExternalIntransit, true);
                //StoredProcedure.WriteLine("alter table #TEMP_SUM" + BatchNumberString + " add PRESENTFLAG int null	");

                // process non stock values
                bool haveNonStockValue = false;
                int countStockVariables = 0;
                int countNonStockVariables = 0;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    if (vp.VariableType == eVariableType.BegStock ||
                        vp.VariableType == eVariableType.EndStock)
                    {
                        ++countStockVariables;
                        continue;
                    }
                    else
                    {
                        haveNonStockValue = true;
                        ++countNonStockVariables;
                    }
                }

                count = 0;
                line = Indent10;
                if (haveNonStockValue)
                {
                    StoredProcedure.WriteLine("-- build a temp table of all non stock values");
                    StoredProcedure.WriteLine("INSERT INTO #TEMP_SUM" + BatchNumberString + " (HN_RID, TIME_ID, ST_RID, PRESENTFLAG, ");
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType == eVariableType.BegStock ||
                            vp.VariableType == eVariableType.EndStock)
                        {
                            continue;
                        }
                        ++count;
                        line += vp.DatabaseColumnName + ", ";

                        if (line.Length > 80)
                        {
                            StoredProcedure.WriteLine(line);
                            line = Indent10;
                        }
                    }
                    line = line.TrimEnd();
                    line += "HN_MOD)";
                    if (line.Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }

                    line = Indent10;
                    count = 0;
                    StoredProcedure.WriteLine("SELECT tmp.HN_RID, tmp.FIRST_DAY_OF_WEEK, vw.ST_RID, 0, ");
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType == eVariableType.BegStock ||
                            vp.VariableType == eVariableType.EndStock)
                        {
                            continue;
                        }
                        ++count;
                        switch (vp.DayToWeekRollType)
                        {
                            case eDayToWeekRollType.Sum:
                                line += "sum(" + vp.DatabaseColumnName + ") as " + vp.DatabaseColumnName + ", ";
                                break;
                            case eDayToWeekRollType.Average:
                                if (IncludeZeroInAverage)
                                {
                                    line += "avg(" + vp.DatabaseColumnName + ") as " + vp.DatabaseColumnName + ", ";
                                }
                                else
                                {
                                    line += "avg(case when " + vp.DatabaseColumnName + " = 0 then null else " + vp.DatabaseColumnName + " end) as " + vp.DatabaseColumnName + ", ";
                                }
                                break;
                            case eDayToWeekRollType.First:
                                if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Integer)
                                {
                                    line += "MIN(convert(int, " + vp.DatabaseColumnName + ") + @factora + (tmp.FIRST_DAY_OF_NEXT_WEEK - vw.TIME_ID) * @factorm) as " + vp.DatabaseColumnName + ", ";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Real)
                                {
                                    line += "MIN(convert(real, " + vp.DatabaseColumnName + ") + @factora + (tmp.FIRST_DAY_OF_NEXT_WEEK - vw.TIME_ID) * @factorm) as " + vp.DatabaseColumnName + ", ";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Float)
                                {
                                    line += "MIN(convert(float, " + vp.DatabaseColumnName + ") + @factora + (tmp.FIRST_DAY_OF_NEXT_WEEK - vw.TIME_ID) * @factorm) as " + vp.DatabaseColumnName + ", ";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.BigInteger)
                                {
                                    line += "MIN(convert(bigint, " + vp.DatabaseColumnName + ") + @factora + (tmp.FIRST_DAY_OF_NEXT_WEEK - vw.TIME_ID) * @factorm) as " + vp.DatabaseColumnName + ", ";
                                }
                                break;
                            case eDayToWeekRollType.Last:
                                if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Integer)
                                {
                                    line += "MAX(convert(int, " + vp.DatabaseColumnName + ") + @factora + (tmp.FIRST_DAY_OF_NEXT_WEEK - vw.TIME_ID) * @factorm) as " + vp.DatabaseColumnName + ", ";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Real)
                                {
                                    line += "MAX(convert(real, " + vp.DatabaseColumnName + ") + @factora + (tmp.FIRST_DAY_OF_NEXT_WEEK - vw.TIME_ID) * @factorm) as " + vp.DatabaseColumnName + ", ";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Float)
                                {
                                    line += "MAX(convert(float, " + vp.DatabaseColumnName + ") + @factora + (tmp.FIRST_DAY_OF_NEXT_WEEK - vw.TIME_ID) * @factorm) as " + vp.DatabaseColumnName + ", ";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.BigInteger)
                                {
                                    line += "MAX(convert(bigint, " + vp.DatabaseColumnName + ") + @factora + (tmp.FIRST_DAY_OF_NEXT_WEEK - vw.TIME_ID) * @factorm) as " + vp.DatabaseColumnName + ", ";
                                }
                                break;
                        }
                        if (line.Length > 80)
                        {
                            StoredProcedure.WriteLine(line);
                            line = Indent10;
                        }
                    }
                    line = line.TrimEnd();
                    if (line.Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }

                    StoredProcedure.WriteLine("	 vw.HN_MOD");
                    StoredProcedure.WriteLine("         FROM " + dayTableName + " vw with (nolock), #TEMP_ROLLUP_ITEM" + BatchNumberString + " tmp");
                    StoredProcedure.WriteLine("         WHERE tmp.PROCESS = @PROCESS ");
                    StoredProcedure.WriteLine("	AND tmp.ITEM_TYPE = @ITEM_TYPE");
                    StoredProcedure.WriteLine("	AND tmp.PH_RID = @PH_RID");
                    StoredProcedure.WriteLine("	AND tmp.HOME_LEVEL = @HOME_LEVEL");
                    StoredProcedure.WriteLine("	AND vw.HN_RID = tmp.HN_RID");
                    StoredProcedure.WriteLine("	AND vw.TIME_ID between tmp.FIRST_DAY_OF_WEEK and tmp.LAST_DAY_OF_WEEK");
                    StoredProcedure.WriteLine("       GROUP BY vw.HN_MOD, tmp.HN_RID, ST_RID, tmp.FIRST_DAY_OF_WEEK");
                    StoredProcedure.WriteLine("	");
                    StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_SUM" + BatchNumberString);
                    StoredProcedure.WriteLine("	");
                    StoredProcedure.WriteLine("-- update existing values");
                    StoredProcedure.WriteLine("UPDATE " + weekTableName + " with (rowlock)");
                    count = 0;
                    line = BlankLine;
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType == eVariableType.BegStock ||
                            vp.VariableType == eVariableType.EndStock)
                        {
                            continue;
                        }
                        if (line.TrimEnd().Length > 0)
                        {
                            line += ",";
                            StoredProcedure.WriteLine(line);
                        }
                        line = BlankLine;
                        ++count;
                        if (count == 1)
                        {
                            line = line.Insert(9, "SET");
                        }
                        line = line.Insert(13, vp.DatabaseColumnName);
                        line = line.TrimEnd();
                        switch (vp.DayToWeekRollType)
                        {
                            case eDayToWeekRollType.None:
                                break;
                            case eDayToWeekRollType.Sum:
                                line += " = COALESCE(ts." + vp.DatabaseColumnName + ", vw." + vp.DatabaseColumnName + ")";
                                break;
                            case eDayToWeekRollType.Average:
                                line += " = COALESCE(ts." + vp.DatabaseColumnName + ", vw." + vp.DatabaseColumnName + ")";
                                break;
                            case eDayToWeekRollType.First:
                                if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Integer)
                                {
                                    line += " = COALESCE(convert(int, ts." + vp.DatabaseColumnName + " % @factorm - @factora), vw." + vp.DatabaseColumnName + ")";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Real)
                                {
                                    line += " = COALESCE(convert(real, ts." + vp.DatabaseColumnName + " - floor(ts." + vp.DatabaseColumnName + " / @factorm) * @factorm - @factora), vw." + vp.DatabaseColumnName + ")";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Float)
                                {
                                    line += " = COALESCE(convert(float, ts." + vp.DatabaseColumnName + " - floor(ts." + vp.DatabaseColumnName + " / @factorm) * @factorm - @factora), vw." + vp.DatabaseColumnName + ")";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.BigInteger)
                                {
                                    line += " = COALESCE(convert(bigint, ts." + vp.DatabaseColumnName + " % @factorm - @factora), vw." + vp.DatabaseColumnName + ")";
                                }
                                break;
                            case eDayToWeekRollType.Last:
                                if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Integer)
                                {
                                    line += " = COALESCE(convert(int, ts." + vp.DatabaseColumnName + " % @factorm - @factora), vw." + vp.DatabaseColumnName + ")";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Real)
                                {
                                    line += " = COALESCE(convert(real, ts." + vp.DatabaseColumnName + " - floor(ts." + vp.DatabaseColumnName + " / @factorm) * @factorm - @factora), vw." + vp.DatabaseColumnName + ")"; ;
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Float)
                                {
                                    line += " = COALESCE(convert(float, ts." + vp.DatabaseColumnName + " - floor(ts." + vp.DatabaseColumnName + " / @factorm) * @factorm - @factora), vw." + vp.DatabaseColumnName + ")";
                                }
                                else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Float)
                                {
                                    line += " = COALESCE(convert(bigint, ts." + vp.DatabaseColumnName + " % @factorm - @factora), vw." + vp.DatabaseColumnName + ")";
                                }
                                break;
                        }
                    }
                    if (line.TrimEnd().Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }
                    StoredProcedure.WriteLine("       	FROM " + weekTableName + " vw");
                    StoredProcedure.WriteLine("            JOIN #TEMP_SUM" + BatchNumberString + " ts ON vw.HN_RID = ts.HN_RID ");
                    StoredProcedure.WriteLine("               AND vw.TIME_ID = ts.TIME_ID ");
                    StoredProcedure.WriteLine("               AND vw.ST_RID = ts.ST_RID");
                    StoredProcedure.WriteLine(" ");
                    StoredProcedure.WriteLine("-- flag rows that were updated");
                    StoredProcedure.WriteLine("UPDATE #TEMP_SUM" + BatchNumberString + " SET PRESENTFLAG = 1");
                    StoredProcedure.WriteLine("       FROM #TEMP_SUM" + BatchNumberString + " ts");
                    StoredProcedure.WriteLine("       JOIN " + weekTableName + " vw ON ts.HN_RID = vw.HN_RID AND ts.TIME_ID = vw.TIME_ID AND ts.ST_RID = vw.ST_RID");
                    count = 0;
                    StoredProcedure.WriteLine(" ");

                    StoredProcedure.WriteLine("-- write new values");
                    line = "INSERT " + weekTableName + " (HN_RID, TIME_ID, ST_RID, ";
                    int lineLength = line.Length;
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType == eVariableType.BegStock ||
                            vp.VariableType == eVariableType.EndStock)
                        {
                            continue;
                        }
                        line += vp.DatabaseColumnName + ", ";
                        if (line.Length > 120)
                        {
                            StoredProcedure.WriteLine(line);
                            line = Indent10;
                        }
                    }
                    line += " HN_MOD)";
                    line = line.TrimEnd();
                    if (line.Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }

                    count = 0;
                    line = Indent10 + "SELECT ts.HN_RID, ts.TIME_ID, ts.ST_RID, ";
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType == eVariableType.BegStock ||
                            vp.VariableType == eVariableType.EndStock)
                        {
                            continue;
                        }
                        else
                        {
                            switch (vp.DayToWeekRollType)
                            {
                                case eDayToWeekRollType.Sum:
                                    line += " ts." + vp.DatabaseColumnName;
                                    break;
                                case eDayToWeekRollType.Average:
                                    line += " ts." + vp.DatabaseColumnName;
                                    break;
                                case eDayToWeekRollType.First:
                                    if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Integer)
                                    {
                                        line += " convert(int, ts." + vp.DatabaseColumnName + " % @factorm - @factora)";
                                    }
                                    else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Real)
                                    {
                                        line += " convert(real, ts." + vp.DatabaseColumnName + " - floor(ts." + vp.DatabaseColumnName + " / @factorm) * @factorm - @factora)";
                                    }
                                    else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Float)
                                    {
                                        line += " = COALESCE(convert(float, ts." + vp.DatabaseColumnName + " - floor(ts." + vp.DatabaseColumnName + " / @factorm) * @factorm - @factora), vw." + vp.DatabaseColumnName + ")";
                                    }
                                    else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.BigInteger)
                                    {
                                        line += " convert(bigint, ts." + vp.DatabaseColumnName + " % @factorm - @factora)";
                                    }
                                    break;
                                case eDayToWeekRollType.Last:
                                    if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Integer)
                                    {
                                        line += " convert(int, ts." + vp.DatabaseColumnName + " % @factorm - @factora)";
                                    }
                                    else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Real)
                                    {
                                        line += " convert(real, ts." + vp.DatabaseColumnName + " - floor(ts." + vp.DatabaseColumnName + " / @factorm) * @factorm - @factora)";
                                    }
                                    else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.Float)
                                    {
                                        line += " = COALESCE(convert(float, ts." + vp.DatabaseColumnName + " - floor(ts." + vp.DatabaseColumnName + " / @factorm) * @factorm - @factora), vw." + vp.DatabaseColumnName + ")";
                                    }
                                    else if (vp.StoreDatabaseVariableType == eVariableDatabaseType.BigInteger)
                                    {
                                        line += " convert(bigint, ts." + vp.DatabaseColumnName + " % @factorm - @factora)";
                                    }
                                    break;
                            }
                        }
                        line += ", ";
                        if (line.Length > 80)
                        {
                            StoredProcedure.WriteLine(line);
                            line = Indent10;
                        }
                    }
                    line = line.TrimEnd();
                    if (line.Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }
                    StoredProcedure.WriteLine("		" + TableNumber.ToString());
                    StoredProcedure.WriteLine("	FROM #TEMP_SUM" + BatchNumberString + " ts");
                    StoredProcedure.WriteLine("	    WHERE PRESENTFLAG = 0");
                } // end nonstock values

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("truncate table #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("	");

                // process stock values
                line = Indent10;
                count = 0;
                bool haveStockValue = false;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    if (vp.VariableType == eVariableType.BegStock ||
                        vp.VariableType == eVariableType.EndStock)
                    {
                        haveStockValue = true;
                        break;
                    }
                }
                if (haveStockValue)
                {
                    StoredProcedure.WriteLine("-- build a temp table of all stock values");
                    StoredProcedure.WriteLine("INSERT INTO #TEMP_SUM" + BatchNumberString + " (HN_RID, TIME_ID, ST_RID, PRESENTFLAG, ");
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType != eVariableType.BegStock &&
                            vp.VariableType != eVariableType.EndStock)
                        {
                            continue;
                        }
                        ++count;
                        line += vp.DatabaseColumnName + ", ";

                        if (line.Length > 80)
                        {
                            StoredProcedure.WriteLine(line);
                            line = Indent10;
                        }
                    }
                    line = line.TrimEnd();
                    line += "HN_MOD)";
                    if (line.Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }

                    StoredProcedure.WriteLine("SELECT tmp.HN_RID as HN_RID, tmp.FIRST_DAY_OF_WEEK as TIME_ID, vw.ST_RID as ST_RID, 0 AS PRESENTFLAG, ");
                    count = 0;
                    line = Indent10;
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType != eVariableType.BegStock &&
                            vp.VariableType != eVariableType.EndStock)
                        {
                            continue;
                        }
                        ++count;
                        switch (vp.DayToWeekRollType)
                        {
                            case eDayToWeekRollType.None:
                                break;
                            default:
                                line += "COALESCE(vw." + vp.DatabaseColumnName + ", 0) as " + vp.DatabaseColumnName + ", ";
                                break;
                        }
                        if (line.Length > 80)
                        {
                            StoredProcedure.WriteLine(line);
                            line = Indent10;
                        }
                    }
                    line = line.TrimEnd();
                    if (line.Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }

                    StoredProcedure.WriteLine("	 vw.HN_MOD");
                    StoredProcedure.WriteLine("         FROM " + dayTableName + " vw with (nolock), #TEMP_ROLLUP_ITEM" + BatchNumberString + " tmp");
                    StoredProcedure.WriteLine("         WHERE tmp.PROCESS = @PROCESS ");
                    StoredProcedure.WriteLine("	AND tmp.ITEM_TYPE = @ITEM_TYPE");
                    StoredProcedure.WriteLine("	AND tmp.PH_RID = @PH_RID");
                    StoredProcedure.WriteLine("	AND tmp.HOME_LEVEL = @HOME_LEVEL");
                    StoredProcedure.WriteLine("	AND vw.HN_RID = tmp.HN_RID");
                    StoredProcedure.WriteLine("	AND vw.TIME_ID = tmp.FIRST_DAY_OF_WEEK");
                    StoredProcedure.WriteLine("	");
                    StoredProcedure.WriteLine("	");
                    StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_SUM" + BatchNumberString);
                    StoredProcedure.WriteLine("	");
                    StoredProcedure.WriteLine("-- set weekly values to null incase stock value not posted");
                    StoredProcedure.WriteLine("UPDATE " + weekTableName + " with (rowlock)");
                    count = 0;
                    line = BlankLine;
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType != eVariableType.BegStock &&
                            vp.VariableType != eVariableType.EndStock)
                        {
                            continue;
                        }
                        if (line.TrimEnd().Length > 0)
                        {
                            line += ",";
                            StoredProcedure.WriteLine(line);
                        }
                        line = BlankLine;
                        ++count;
                        if (count == 1)
                        {
                            line = line.Insert(9, "SET");
                        }
                        line = line.Insert(13, vp.DatabaseColumnName);
                        line = line.TrimEnd();
                        line += " = null";
                    }
                    if (line.TrimEnd().Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }
                    line = BlankLine;
                    StoredProcedure.WriteLine("       	FROM " + weekTableName + " vw");
                    StoredProcedure.WriteLine("            JOIN #TEMP_ROLLUP_ITEM" + BatchNumberString + " ri ON vw.HN_RID = ri.HN_RID ");
                    StoredProcedure.WriteLine("               AND vw.TIME_ID = ri.FIRST_DAY_OF_WEEK ");

                    StoredProcedure.WriteLine("	");
                    StoredProcedure.WriteLine("-- update existing values");
                    StoredProcedure.WriteLine("UPDATE " + weekTableName + " with (rowlock)");
                    count = 0;
                    line = BlankLine;
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType != eVariableType.BegStock &&
                            vp.VariableType != eVariableType.EndStock)
                        {
                            continue;
                        }
                        if (line.TrimEnd().Length > 0)
                        {
                            line += ",";
                            StoredProcedure.WriteLine(line);
                        }
                        line = BlankLine;
                        ++count;
                        if (count == 1)
                        {
                            line = line.Insert(9, "SET");
                        }
                        line = line.Insert(13, vp.DatabaseColumnName);
                        line = line.TrimEnd();
                        switch (vp.DayToWeekRollType)
                        {
                            case eDayToWeekRollType.None:
                                break;
                            default:
                                line += " = ts." + vp.DatabaseColumnName;
                                break;
                        }
                    }
                    if (line.TrimEnd().Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }
                    line = BlankLine;
                    StoredProcedure.WriteLine("       	FROM " + weekTableName + " vw");
                    StoredProcedure.WriteLine("            JOIN #TEMP_SUM" + BatchNumberString + " ts ON vw.HN_RID = ts.HN_RID ");
                    StoredProcedure.WriteLine("               AND vw.TIME_ID = ts.TIME_ID ");
                    StoredProcedure.WriteLine("               AND vw.ST_RID = ts.ST_RID");
                    StoredProcedure.WriteLine(" ");
                    StoredProcedure.WriteLine("-- flag rows that were updated");
                    StoredProcedure.WriteLine("UPDATE #TEMP_SUM" + BatchNumberString + " SET PRESENTFLAG = 1");
                    StoredProcedure.WriteLine("       FROM #TEMP_SUM" + BatchNumberString + " ts");
                    StoredProcedure.WriteLine("       JOIN " + weekTableName + " vw ON ts.HN_RID = vw.HN_RID AND ts.TIME_ID = vw.TIME_ID AND ts.ST_RID = vw.ST_RID");
                    count = 0;
                    StoredProcedure.WriteLine(" ");
                    StoredProcedure.WriteLine("-- write new values");
                    line = "INSERT " + weekTableName + " (HN_RID, TIME_ID, ST_RID, ";
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType != eVariableType.BegStock &&
                            vp.VariableType != eVariableType.EndStock)
                        {
                            continue;
                        }
                        line += vp.DatabaseColumnName + ", ";
                        if (line.Length > 120)
                        {
                            StoredProcedure.WriteLine(line);
                            line = Indent10;
                        }
                    }
                    line += " HN_MOD)";
                    line = line.TrimEnd();
                    if (line.Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }

                    count = 0;
                    line = Indent10 + "SELECT ts.HN_RID, ts.TIME_ID, ts.ST_RID, ";
                    foreach (VariableProfile vp in DatabaseVariables)
                    {
                        if (vp.VariableType != eVariableType.BegStock &&
                            vp.VariableType != eVariableType.EndStock)
                        {
                            continue;
                        }
                        else
                        {
                            switch (vp.DayToWeekRollType)
                            {
                                case eDayToWeekRollType.None:
                                    break;
                                default:
                                    line += " ts." + vp.DatabaseColumnName;
                                    break;
                            }
                        }
                        line += ", ";
                        if (line.Length > 80)
                        {
                            StoredProcedure.WriteLine(line);
                            line = Indent10;
                        }
                    }
                    line = line.TrimEnd();
                    if (line.Length > 0)
                    {
                        StoredProcedure.WriteLine(line);
                    }
                    StoredProcedure.WriteLine("		" + TableNumber.ToString());
                    StoredProcedure.WriteLine("	FROM #TEMP_SUM" + BatchNumberString + " ts");
                    StoredProcedure.WriteLine("	    WHERE PRESENTFLAG = 0");
                    StoredProcedure.WriteLine("	");
                } // end stock values

                StoredProcedure.AddUpdateProcessedRecords(BatchNumberString, LockID, false);

                StoredProcedure.AddDeleteTempTables(BatchNumberString);
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in BuildStoredProcedure", "StoreDayWeekRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }

    public class StoreHistoryDayRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public StoreHistoryDayRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.storeDailyHistory, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "StoreHistoryDayRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        public string BuildStoredProcedure(string aProcedureName)
        {
            CreateStaticStoredProcedure(aProcedureName);
            BuildStoredProcedure();
            return StoredProcedure.Text;
        }

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			eProcessCompletionStatus status = eProcessCompletionStatus.None;

            string message = "Executing store daily history rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumberString;
            try
            {
                if (!UseStaticStoredProcedure &&
                    DatabaseVariables.Count == 0)
                {
                    return;
                }

                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "StoreHistoryDayRollupProcess");

                try
                {
                    try
                    {
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreDailyHistoryRollupSP.Replace(Include.DBTableCountReplaceString, TableNumber.ToString()));
                        }
                        else if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            !ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreDailyHistoryNoZeroRollupSP.Replace(Include.DBTableCountReplaceString, TableNumber.ToString()));
                        }
                        else
                        {
                            CreateStoredProcedure("SP_MID_ST_HIS_DAY_ROLLUP");
                            BuildStoredProcedure();
                            WriteStoredProcedure();
                        }
                        RollupData.OpenUpdateConnection();
                        RollupData.ProcessRollup(StoredProcedure.Name, Process, HierarchyRID, (int)eRollType.storeDailyHistory, Version, Level, BatchNumber);
                        RollupData.CommitData();
						status = eProcessCompletionStatus.Successful;

					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
						status = eProcessCompletionStatus.Failed;

					}
					finally
					{
						RollupData.CloseUpdateConnection();
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						UpdateRollupProcess(status);
					}
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedure()
        {
            string tableName;
            bool isStore = true;
            bool isForecast = false;
            bool isExternalIntransit = false;

            try
            {
                tableName = "STORE_HISTORY_DAY" + TableNumber.ToString();

                StoredProcedure.AddCreateProcedure(eRollType.storeDailyHistory, isStore, isForecast);

                StoredProcedure.AddRollupItemTable(BatchNumberString, TableNumber, Include.FV_ActualRID.ToString(), isStore, isExternalIntransit, isDayToWeek: false);

                StoredProcedure.AddClearValues(BatchNumberString, DatabaseVariables, tableName, Include.FV_ActualRID.ToString(), ZeroParentsWithNoChildren, isForecast, isLockTable: false);

                StoredProcedure.AddTempJoinTable(BatchNumberString, isStore);

                StoredProcedure.AddTempSumTable(BatchNumberString, DatabaseVariables, isStore, isForecast, isExternalIntransit);

                StoredProcedure.AddSum(BatchNumberString, TableNumber, DatabaseVariables, "VW_STORE_HISTORY_DAY", IncludeZeroInAverage, isStore, isForecast, isExternalIntransit, setMaxDOPToOne: false, isStoreToChain: false);
				
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("declare @TEMP_SUM" + BatchNumberString + " as MID_ST_HIS_DAY_TYPE	");
                StoredProcedure.WriteLine("	");

                StoredProcedure.AddSumToTypeInsert(BatchNumberString, DatabaseVariables, isStore, isForecast);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("exec dbo.SP_MID_ST_HIS_DAY" + BatchNumberString + "_WRITE @TEMP_SUM" + BatchNumberString + "	");

                StoredProcedure.AddUpdateProcessedRecords(BatchNumberString, LockID, isExternalIntransit);
                
                StoredProcedure.AddDeleteTempTables(BatchNumberString);
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }

    public class StoreHistoryWeekRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public StoreHistoryWeekRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.storeWeeklyHistory, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "StoreHistoryWeekRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        
        public string BuildStoredProcedure(string aProcedureName)
        {
            CreateStaticStoredProcedure(aProcedureName);
            BuildStoredProcedure();
            return StoredProcedure.Text;
        }
        

        override public void ExecuteProcess()
        {
			eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			
			eProcessCompletionStatus status = eProcessCompletionStatus.None;

            string message = "Executing store weekly history rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumberString;
            try
            {
                
                if (!UseStaticStoredProcedure &&
                    DatabaseVariables.Count == 0)
                {
                    return;
                }
                

                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "StoreHistoryWeekRollupProcess");

                try
                {
                    try
                    {
                        
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreWeeklyHistoryRollupSP.Replace(Include.DBTableCountReplaceString, TableNumber.ToString()));
                        }
                        else if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            !ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreWeeklyHistoryNoZeroRollupSP.Replace(Include.DBTableCountReplaceString, TableNumber.ToString()));
                        }
                        else
                        {
                            CreateStoredProcedure("SP_MID_ST_HIS_WK_ROLLUP");
                            BuildStoredProcedure();
                            WriteStoredProcedure();
                        }
                        
                        RollupData.OpenUpdateConnection();
                        RollupData.ProcessRollup(StoredProcedure.Name, Process, HierarchyRID, (int)eRollType.storeWeeklyHistory, Version, Level, BatchNumber);
                        RollupData.CommitData();
						
						status = eProcessCompletionStatus.Successful;
						
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
						
						status = eProcessCompletionStatus.Failed;
						
					}
					finally
					{
						RollupData.CloseUpdateConnection();
                        
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						
						UpdateRollupProcess(status);
						
                    }
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedure()
        {
            string tableName;
            bool isStore = true;
            bool isForecast = false;
            bool isExternalIntransit = false;

            try
            {

                tableName = "STORE_HISTORY_WEEK" + TableNumber.ToString();

                StoredProcedure.AddCreateProcedure(eRollType.storeWeeklyHistory, isStore, isForecast);

                StoredProcedure.AddRollupItemTable(BatchNumberString, TableNumber, Include.FV_ActualRID.ToString(), isStore, isExternalIntransit, isDayToWeek: false);

                StoredProcedure.AddClearValues(BatchNumberString, DatabaseVariables, tableName, Include.FV_ActualRID.ToString(), ZeroParentsWithNoChildren, isForecast, isLockTable: false);

                StoredProcedure.AddTempJoinTable(BatchNumberString, isStore);

                StoredProcedure.AddTempSumTable(BatchNumberString, DatabaseVariables, isStore, isForecast, isExternalIntransit);

                StoredProcedure.AddSum(BatchNumberString, TableNumber, DatabaseVariables, "VW_STORE_HISTORY_WEEK", IncludeZeroInAverage, isStore, isForecast, isExternalIntransit, setMaxDOPToOne: false, isStoreToChain: false);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("declare @TEMP_SUM" + BatchNumberString + " as MID_ST_HIS_WK_TYPE	");
                StoredProcedure.WriteLine("	");

                StoredProcedure.AddSumToTypeInsert(BatchNumberString, DatabaseVariables, isStore, isForecast);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("exec dbo.SP_MID_ST_HIS_WK" + BatchNumberString + "_WRITE @TEMP_SUM" + BatchNumberString + "	");

                StoredProcedure.AddUpdateProcessedRecords(BatchNumberString, LockID, isExternalIntransit);

                StoredProcedure.AddDeleteTempTables(BatchNumberString);

            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }

    public class StoreForecastWeekRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public StoreForecastWeekRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.storeWeeklyForecast, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "StoreForecastWeekRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        
        public string BuildStoredProcedure(string aProcedureName, bool aHonorLocks)
        {
            CreateStaticStoredProcedure(aProcedureName);
            if (aHonorLocks)
            {
                BuildStoredProcedureWithLocks();
            }
            else
            {
                BuildStoredProcedure();
            }
            return StoredProcedure.Text;
        }
        

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			
			eProcessCompletionStatus status = eProcessCompletionStatus.None;
			

            string message = "Executing store forecast week rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumberString;
            try
            {
                
                if (!UseStaticStoredProcedure &&
                    DatabaseVariables.Count == 0)
                {
                    return;
                }
                

                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "StoreForecastWeekRollupProcess");

                try
                {
                    try
                    {
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreWeeklyForecastRollupSP.Replace(Include.DBTableCountReplaceString, TableNumber.ToString()));
                        }
                        else if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            !ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreWeeklyForecastNoZeroRollupSP.Replace(Include.DBTableCountReplaceString, TableNumber.ToString()));
                        }
                        else if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreWeeklyForecastHonorLocksRollupSP.Replace(Include.DBTableCountReplaceString, TableNumber.ToString()));
                        }
                        else
                        {
                            CreateStoredProcedure("SP_MID_ST_FOR_WK_ROLLUP");
                            if (HonorLocks)
                            {
                                BuildStoredProcedureWithLocks();
                            }
                            else
                            {
                                BuildStoredProcedure();
                            }
                            WriteStoredProcedure();
                        }
                        
                        RollupData.OpenUpdateConnection();
                        RollupData.ProcessRollup(StoredProcedure.Name, Process, HierarchyRID, (int)eRollType.storeWeeklyForecast, Version, Level, BatchNumber);
                        RollupData.CommitData();
						
						status = eProcessCompletionStatus.Successful;
						

					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
						
						status = eProcessCompletionStatus.Failed;
						

					}
					finally
					{
						RollupData.CloseUpdateConnection();
                        
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						
						UpdateRollupProcess(status);
						
					}
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedureWithLocks()
        {
            int count;
            string line;
            string tableName;
            try
            {
                tableName = "STORE_FORECAST_WEEK" + TableNumber.ToString();
                


                StoredProcedure.WriteLine("CREATE PROCEDURE " + StoredProcedure.Name);
                StoredProcedure.WriteLine("(");
                StoredProcedure.WriteLine("	@PROCESS INT,");
                StoredProcedure.WriteLine("	@PH_RID INT,");
                StoredProcedure.WriteLine("	@HOME_LEVEL INT,");
                StoredProcedure.WriteLine(" @ITEM_TYPE INT = " + Convert.ToInt32(eRollType.storeWeeklyForecast).ToString() + ",");
                StoredProcedure.WriteLine("	@FV_RID INT,");
                StoredProcedure.WriteLine(" @BATCH_NUMBER INT,");
                StoredProcedure.WriteLine(" @debug bit = 0");
                StoredProcedure.WriteLine(")");
                StoredProcedure.WriteLine("AS");
                StoredProcedure.WriteLine("SET NOCOUNT ON");
                StoredProcedure.WriteLine("SET ANSI_WARNINGS OFF"); 
                StoredProcedure.WriteLine("DECLARE  @Tables INT");
                StoredProcedure.WriteLine("-- get the number of store tables");
                StoredProcedure.WriteLine("SELECT @Tables = STORE_TABLE_COUNT FROM SYSTEM_OPTIONS");
                StoredProcedure.WriteLine("-- select records to process");
                StoredProcedure.WriteLine("select * INTO #TEMP_ROLLUP_ITEM" + BatchNumberString + " FROM ROLLUP_ITEM ri with (nolock)");
                StoredProcedure.WriteLine("   where ri.FV_RID = @FV_RID     ");
                StoredProcedure.WriteLine("     AND ri.PROCESS = @PROCESS");
                StoredProcedure.WriteLine("     AND ri.ITEM_TYPE = @ITEM_TYPE");
                StoredProcedure.WriteLine("     AND ri.PH_RID = @PH_RID");
                StoredProcedure.WriteLine("     AND ri.HOME_LEVEL = @HOME_LEVEL");
                StoredProcedure.WriteLine("     AND ri.BATCH_NUMBER = @BATCH_NUMBER");
                StoredProcedure.WriteLine("     AND ri.ITEM_PROCESSED is null");
                StoredProcedure.WriteLine("     AND ri.HN_MOD = " + TableNumber.ToString());
                StoredProcedure.WriteLine("     AND ri.HN_RID % @Tables = " + TableNumber.ToString());
                StoredProcedure.WriteLine(" create clustered index #TEMP_ROLLUP_ITEM_IDX" + BatchNumberString);
                StoredProcedure.WriteLine("	    on #TEMP_ROLLUP_ITEM" + BatchNumberString);
                StoredProcedure.WriteLine("	    (PH_RID, HN_RID)");
                // End TT#500
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_ROLLUP_ITEM" + BatchNumberString);
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- create temp table to hold values	");
                StoredProcedure.WriteLine("	");
                count = 0;
                line = "create table #TEMP" + BatchNumberString + " (tmpHN_RID   int not null,";
                StoredProcedure.WriteLine(line);
                line = "                tmpFV_RID   int not null,";
                StoredProcedure.WriteLine(line);
                line = "                tmpST_RID   int not null,";
                StoredProcedure.WriteLine(line);
                line = "                tmpTIME_ID  int not null,";
                StoredProcedure.WriteLine(line);
                line = "                tmpPRESENTFLAG  int null,";
                StoredProcedure.WriteLine(line);
                // add value columns
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line = "                tmp" + vp.DatabaseColumnName;
                    switch (vp.StoreDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += "  int    null,";
                            break;
                        case eVariableDatabaseType.Real:
                            line += "  real   null,";
                            break;
                        case eVariableDatabaseType.Float:
                            line += "  float  null,";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += "  bigint null,";
                            break;
                    }
                    StoredProcedure.WriteLine(line);
                }
                line = "                tmpHN_MOD  smallint null,";
                StoredProcedure.WriteLine(line);
                // add lock columns
                count = 0;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line = "                tmp" + vp.DatabaseColumnName + "_LOCK  char(1) null";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    else
                    {
                        line += ")";
                    }
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine(" alter table #TEMP" + BatchNumberString);
                StoredProcedure.WriteLine("	    add primary key clustered (tmpHN_RID, tmpFV_RID, tmpTIME_ID, tmpST_RID)");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- build a temp table of keys to be updated along with their lock flags");
                StoredProcedure.WriteLine("INSERT #TEMP" + BatchNumberString + " (tmpHN_RID, tmpFV_RID, tmpST_RID, tmpTIME_ID, tmpPRESENTFLAG, ");
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "tmp" + vp.DatabaseColumnName + ", ";
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                line += "tmpHN_MOD,";
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "tmp" + vp.DatabaseColumnName + "_LOCK";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ")";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("SELECT vw.HN_RID, vw.FV_RID, vw.ST_RID, ri.TIME_ID, 0,");
                count = 0;
                line = Indent10;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "0, ";
                    if (line.Length > 80)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent10;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine(Indent10 + "vw.HN_RID % @Tables,");
                count = 0;
                line = Indent10;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(sfwl." + vp.DatabaseColumnName + "_LOCK, '0')";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    StoredProcedure.WriteLine(line);
                    line = Indent10;
                }
                StoredProcedure.WriteLine("         from #TEMP_ROLLUP_ITEM" + BatchNumberString + " ri, VW_STORE_FORECAST_WEEK vw with (nolock) LEFT OUTER JOIN STORE_FORECAST_WEEK_LOCK sfwl ");
                StoredProcedure.WriteLine("           ON vw.HN_RID = sfwl.HN_RID AND vw.TIME_ID = sfwl.TIME_ID AND vw.FV_RID = sfwl.FV_RID AND vw.ST_RID = sfwl.ST_RID ");
                StoredProcedure.WriteLine("         WHERE ri.PROCESS = @PROCESS ");
                StoredProcedure.WriteLine("           AND ri.ITEM_TYPE = @ITEM_TYPE");
                StoredProcedure.WriteLine("           AND ri.PH_RID = @PH_RID");
                StoredProcedure.WriteLine("           AND ri.HOME_LEVEL = @HOME_LEVEL");
                StoredProcedure.WriteLine("           AND ri.FV_RID = @FV_RID");
                StoredProcedure.WriteLine("           AND vw.HN_RID = ri.HN_RID");
                StoredProcedure.WriteLine("           AND vw.FV_RID = ri.FV_RID");
                StoredProcedure.WriteLine("           AND vw.TIME_ID = ri.TIME_ID");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP" + BatchNumberString);
				StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- select nodes to roll");

                StoredProcedure.WriteLine("select distinct hnj.PH_RID PH_RID, hnj.PARENT_HN_RID PARENT_HN_RID, hnj.HN_RID HN_RID, hnj.HN_RID % @Tables HN_MOD  ");
                StoredProcedure.WriteLine("  INTO #TEMP_JOIN" + BatchNumberString);
                StoredProcedure.WriteLine("  from #TEMP_ROLLUP_ITEM" + BatchNumberString + " ri");
                StoredProcedure.WriteLine("  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine(" alter table #TEMP_JOIN" + BatchNumberString);
                StoredProcedure.WriteLine("	    add primary key clustered (PARENT_HN_RID, HN_RID)");
                StoredProcedure.WriteLine("	");

                StoredProcedure.WriteLine("-- build a temp table of all summed values");
                StoredProcedure.WriteLine("SELECT ri.HN_RID as sumHN_RID, ri.FV_RID as sumFV_RID, vw.ST_RID as sumST_RID, ri.TIME_ID as sumTIME_ID,  ");
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    switch (vp.LevelRollType)
                    {
                        case eLevelRollType.Sum:
                            line += "sum(" + vp.DatabaseColumnName + ") as sum" + vp.DatabaseColumnName;
                            break;
                        case eLevelRollType.Average:
                            if (IncludeZeroInAverage)
                            {
                                line += "avg(" + vp.DatabaseColumnName + ") as sum" + vp.DatabaseColumnName;
                            }
                            else
                            {
                                line += "avg(case when " + vp.DatabaseColumnName + " = 0 then null else " + vp.DatabaseColumnName + " end) as sum" + vp.DatabaseColumnName;
                            }
                            break;
                    }
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 80)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent10;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }

				StoredProcedure.WriteLine("  INTO #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("	FROM #TEMP_ROLLUP_ITEM" + BatchNumberString + " ri");
                StoredProcedure.WriteLine("	inner join #TEMP_JOIN" + BatchNumberString + " hnj on hnj.PARENT_HN_RID = ri.HN_RID");
                StoredProcedure.WriteLine("	inner join VW_STORE_HISTORY_WEEK vw with (nolock) on vw.HN_MOD = hnj.HN_MOD and vw.HN_RID = hnj.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID");
				StoredProcedure.WriteLine("       GROUP BY ri.HN_RID, ri.FV_RID, ST_RID, ri.TIME_ID");
                StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine(" alter table #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("	    add primary key clustered (sumHN_RID, sumFV_RID, sumTIME_ID, sumST_RID)");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- update #TEMP with the summed values");
                StoredProcedure.WriteLine("UPDATE #TEMP" + BatchNumberString);
                count = 0;
                line = BlankLine;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    if (count == 1)
                    {
                        line = line.Insert(9, "SET");
                    }
                    line = line.Insert(13, "tmp" + vp.DatabaseColumnName);
                    line = line.TrimEnd();
                    line += " = sum" + vp.DatabaseColumnName;
                    if (count < DatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    StoredProcedure.WriteLine(line);
                    line = BlankLine;
                }
                StoredProcedure.WriteLine("       	FROM #TEMP" + BatchNumberString + " tmp");
                StoredProcedure.WriteLine("            JOIN #TEMP_SUM" + BatchNumberString + " ts ON tmpHN_RID = sumHN_RID ");
                StoredProcedure.WriteLine("               AND tmpTIME_ID = sumTIME_ID AND tmpFV_RID = sumFV_RID");
                StoredProcedure.WriteLine("               AND tmpST_RID = sumST_RID");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP" + BatchNumberString);
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- deleted updated records from #TEMP_SUM");
                StoredProcedure.WriteLine("delete #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("   FROM #TEMP_SUM" + BatchNumberString + " ts INNER JOIN #TEMP" + BatchNumberString + " tmp");
                StoredProcedure.WriteLine("               ON ts.sumHN_RID = tmp.tmpHN_RID");
                StoredProcedure.WriteLine("                 AND ts.sumTIME_ID = tmp.tmpTIME_ID");
                StoredProcedure.WriteLine("                 AND ts.sumFV_RID = tmp.tmpFV_RID");
                StoredProcedure.WriteLine("                 AND ts.sumST_RID = tmp.tmpST_RID");
                StoredProcedure.WriteLine("-- insert other records from #TEMP_SUM");
                StoredProcedure.WriteLine("INSERT #TEMP" + BatchNumberString + " (tmpHN_RID, tmpFV_RID, tmpST_RID, tmpTIME_ID, tmpPRESENTFLAG, ");
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "tmp" + vp.DatabaseColumnName;
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ", tmpHN_MOD,";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "tmp" + vp.DatabaseColumnName + "_LOCK";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ")";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                count = 0;
                line = Indent10 + "SELECT ts.sumHN_RID, ts.sumFV_RID, ts.sumST_RID, ts.sumTIME_ID, 0,  ";
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(ts.sum" + vp.DatabaseColumnName + ", 0)";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ",  ts.sumHN_RID % @Tables,";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    string varName = vp.DatabaseColumnName;
                    ++count;
                    line += "0";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("	FROM #TEMP_SUM" + BatchNumberString + " ts");
                count = 0;
                line = Indent15 + "WHERE COALESCE (";
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "ts.sum" + vp.DatabaseColumnName;
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ") IS NOT NULL";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("-- update " + tableName + " with the summed values where the values are not locked");
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    StoredProcedure.WriteLine("UPDATE " + tableName + " with (rowlock)");
                    StoredProcedure.WriteLine("       	SET " + vp.DatabaseColumnName + " = tmp.tmp" + vp.DatabaseColumnName);
                    StoredProcedure.WriteLine("           FROM " + tableName + " vw");
                    StoredProcedure.WriteLine("            JOIN #TEMP" + BatchNumberString + " tmp ON HN_RID = tmp.tmpHN_RID ");
                    StoredProcedure.WriteLine("               AND vw.TIME_ID = tmp.tmpTIME_ID ");
                    StoredProcedure.WriteLine("               AND vw.FV_RID = tmp.tmpFV_RID");
                    StoredProcedure.WriteLine("               AND vw.ST_RID = tmp.tmpST_RID");
                    StoredProcedure.WriteLine("          where tmp.tmp" + vp.DatabaseColumnName + "_LOCK = 0     ");
                }

                StoredProcedure.WriteLine("       ");
                StoredProcedure.WriteLine("-- flag updated records");
                StoredProcedure.WriteLine("update #TEMP" + BatchNumberString + " SET tmpPRESENTFLAG = 1");
                StoredProcedure.WriteLine("   from #TEMP" + BatchNumberString + " tmp INNER JOIN " + tableName + " vw");
                StoredProcedure.WriteLine("               ON  tmp.tmpHN_RID = vw.HN_RID");
                StoredProcedure.WriteLine("	AND tmp.tmpTIME_ID = vw.TIME_ID");
                StoredProcedure.WriteLine("	AND tmp.tmpFV_RID = vw.FV_RID");
                StoredProcedure.WriteLine("	AND tmp.tmpST_RID = vw.ST_RID");
                StoredProcedure.WriteLine("-- insert new values ");
                count = 0;
                line = "INSERT " + tableName + " (HN_RID, FV_RID, TIME_ID, ST_RID, ";
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += vp.DatabaseColumnName;
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ", HN_MOD)";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("       	SELECT tmp.tmpHN_RID, tmp.tmpFV_RID, tmp.tmpTIME_ID, tmp.tmpST_RID, ");
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "tmp.tmp" + vp.DatabaseColumnName;
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ", tmp.tmpHN_MOD";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("         FROM #TEMP" + BatchNumberString + " tmp");
                StoredProcedure.WriteLine("	   WHERE tmpPRESENTFLAG = 0");
                StoredProcedure.WriteLine("-- update rollup items that were processed ");
				
				
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) ");
				StoredProcedure.WriteLine(" values(" + eLockType.RollupItem.GetHashCode().ToString() + ", '" + LockID + "') ");
				StoredProcedure.WriteLine("	");

                // Begin TT#4155 - JSmith - Modify Rollup Procedures to Alway Select Correct Database Index
                //StoredProcedure.WriteLine("update ROLLUP_ITEM with (rowlock) ");
                //StoredProcedure.WriteLine("   set ITEM_PROCESSED = '1' ");
                //StoredProcedure.WriteLine("   from ROLLUP_ITEM ri ");
                //StoredProcedure.WriteLine("      JOIN #TEMP_ROLLUP_ITEM" + BatchNumberString + " tri      ");
                //StoredProcedure.WriteLine("         on ri.FV_RID = tri.FV_RID     ");
                //StoredProcedure.WriteLine("        AND ri.PROCESS = tri.PROCESS");
                //StoredProcedure.WriteLine("        AND ri.ITEM_TYPE = tri.ITEM_TYPE");
                //StoredProcedure.WriteLine("        AND ri.PH_RID = tri.PH_RID");
                //StoredProcedure.WriteLine("        AND ri.HOME_LEVEL = tri.HOME_LEVEL");
                //StoredProcedure.WriteLine("        AND ri.HN_RID = tri.HN_RID");
                //StoredProcedure.WriteLine("        AND ri.TIME_ID = tri.TIME_ID");
                //StoredProcedure.WriteLine("        AND ri.BATCH_NUMBER = @BATCH_NUMBER");

                StoredProcedure.WriteLine("update ROLLUP_ITEM with (rowlock) ");
                StoredProcedure.WriteLine("   set ITEM_PROCESSED = '1' ");
                StoredProcedure.WriteLine("   from ROLLUP_ITEM ri ");
                StoredProcedure.WriteLine("      JOIN #TEMP_ROLLUP_ITEM" + BatchNumberString + " tri      ");
                StoredProcedure.WriteLine("         on ri.PROCESS = tri.PROCESS     ");
                StoredProcedure.WriteLine("        AND ri.HN_RID = tri.HN_RID");
                StoredProcedure.WriteLine("        AND ri.TIME_ID = tri.TIME_ID");
                StoredProcedure.WriteLine("        AND ri.FV_RID = tri.FV_RID     ");
                StoredProcedure.WriteLine("        AND ri.ITEM_TYPE = tri.ITEM_TYPE");
                StoredProcedure.WriteLine("        AND ri.PH_RID = tri.PH_RID");
                StoredProcedure.WriteLine("        AND ri.HOME_LEVEL = tri.HOME_LEVEL");
                StoredProcedure.WriteLine("        AND ri.BATCH_NUMBER = @BATCH_NUMBER");
                // End TT#4155 - JSmith - Modify Rollup Procedures to Alway Select Correct Database Index
				
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("delete from VIRTUAL_LOCK ");
				StoredProcedure.WriteLine(" where LOCK_TYPE = " + eLockType.RollupItem.GetHashCode().ToString());
				StoredProcedure.WriteLine("   and LOCK_ID = '" + LockID + "'");
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM" + BatchNumberString);
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_JOIN" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_JOIN" + BatchNumberString);
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_SUM" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("SET ANSI_WARNINGS ON"); 
				StoredProcedure.WriteLine("	");
				
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        private void BuildStoredProcedure()
        {
            string tableName;
            bool isStore = true;
            bool isForecast = true;
            bool isExternalIntransit = false;

            try
            {
                
                tableName = "STORE_FORECAST_WEEK" + TableNumber.ToString();

                StoredProcedure.AddCreateProcedure(eRollType.storeWeeklyForecast, isStore, isForecast);

                StoredProcedure.AddRollupItemTable(BatchNumberString, TableNumber, "@FV_RID", isStore, isExternalIntransit, isDayToWeek: false);

                StoredProcedure.AddClearValues(BatchNumberString, DatabaseVariables, tableName, "@FV_RID", ZeroParentsWithNoChildren, isForecast, isLockTable: false);

                StoredProcedure.AddClearValues(BatchNumberString, DatabaseVariables, "STORE_FORECAST_WEEK_LOCK", "@FV_RID", ZeroParentsWithNoChildren, isForecast, isLockTable: true);

                StoredProcedure.AddTempJoinTable(BatchNumberString, isStore);

                StoredProcedure.AddTempSumTable(BatchNumberString, DatabaseVariables, isStore, isForecast, isExternalIntransit);

                StoredProcedure.AddSum(BatchNumberString, TableNumber, DatabaseVariables, "VW_STORE_FORECAST_WEEK", IncludeZeroInAverage, isStore, isForecast, isExternalIntransit, setMaxDOPToOne: false, isStoreToChain: false);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("declare @TEMP_SUM" + BatchNumberString + " as MID_ST_FOR_WK_TYPE	");
                StoredProcedure.WriteLine("	");

                StoredProcedure.AddSumToTypeInsert(BatchNumberString, DatabaseVariables, isStore, isForecast);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("exec dbo.SP_MID_ST_FOR_WK" + BatchNumberString + "_WRITE @TEMP_SUM" + BatchNumberString + "	");

                StoredProcedure.AddUpdateProcessedRecords(BatchNumberString, LockID, isExternalIntransit);

                StoredProcedure.AddDeleteTempTables(BatchNumberString);

            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }

    public class StoreToChainHistoryRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public StoreToChainHistoryRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.storeToChain, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "StoreToChainHistoryRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        
        public string BuildStoredProcedure(string aProcedureName)
        {
            CreateStaticStoredProcedure(aProcedureName);
            BuildStoredProcedure();
            return StoredProcedure.Text;
        }
        

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			
			eProcessCompletionStatus status = eProcessCompletionStatus.None;
			

            string message = "Executing store to chain history rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumberString;
            try
            {
                
                if (!UseStaticStoredProcedure &&
                    DatabaseVariables.Count == 0)
                {
                    return;
                }
                

                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "StoreToChainHistoryRollupProcess");

                try
                {
                    try
                    {
                        
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreToChainHistoryRollupSP);
                        }
                        else if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            !ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreToChainHistoryNoZeroRollupSP);
                        }
                        else
                        {
                            CreateStoredProcedure("SP_MID_HIS_ST_TO_CHN_ROLLUP");
                            BuildStoredProcedure();
                            WriteStoredProcedure();
                        }
                        
                        RollupData.OpenUpdateConnection();
                        RollupData.ProcessRollup(StoredProcedure.Name, Process, HierarchyRID, (int)eRollType.storeToChain, Version, Level, BatchNumber);
						RollupData.CommitData();
						
						status = eProcessCompletionStatus.Successful;
						

					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
						
						status = eProcessCompletionStatus.Failed;
						

					}
					finally
					{
						RollupData.CloseUpdateConnection();
                        
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						
						UpdateRollupProcess(status);
						
                    }
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedure()
        {
            bool isStore = false;
            bool isForecast = false;
            bool isExternalIntransit = false;

            try
            {
                StoredProcedure.AddCreateProcedure(eRollType.storeToChain, isStore, isForecast);

                StoredProcedure.AddRollupItemTable(BatchNumberString, TableNumber, Include.FV_ActualRID.ToString(), isStore, isExternalIntransit, isDayToWeek: false);

                StoredProcedure.AddClearValues(BatchNumberString, DatabaseVariables, "CHAIN_HISTORY_WEEK", Include.FV_ActualRID.ToString(), ZeroParentsWithNoChildren, isForecast, isLockTable: false);

                StoredProcedure.AddTempJoinTable(BatchNumberString, isStore);

                StoredProcedure.AddTempSumTable(BatchNumberString, DatabaseVariables, isStore, isForecast, isExternalIntransit);

                StoredProcedure.AddSum(BatchNumberString, TableNumber, DatabaseVariables, "VW_STORE_HISTORY_WEEK", IncludeZeroInAverage, isStore, isForecast, isExternalIntransit, setMaxDOPToOne: false, isStoreToChain: true);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("declare @TEMP_SUM" + BatchNumberString + " as MID_CHN_HIS_WK_TYPE	");
                StoredProcedure.WriteLine("	");

                StoredProcedure.AddSumToTypeInsert(BatchNumberString, DatabaseVariables, isStore, isForecast);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("exec dbo.SP_MID_CHN_HIS_WK_WRITE @TEMP_SUM" + BatchNumberString + "	");

                StoredProcedure.AddUpdateProcessedRecords(BatchNumberString, LockID, isExternalIntransit);

                StoredProcedure.AddDeleteTempTables(BatchNumberString);
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}
	}


	public class StoreToChainForecastRollupProcess : RollupProcess
	{
		//=======
		// FIELDS
		//=======
		
		//=============
		// CONSTRUCTORS
		//=============

        public StoreToChainForecastRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.storeToChain, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
			try
			{
				
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "StoreToChainForecastRollupProcess");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========


		//========
		// METHODS
		//========

        
        public string BuildStoredProcedure(string aProcedureName)
        {
            CreateStaticStoredProcedure(aProcedureName);
            BuildStoredProcedure();
            return StoredProcedure.Text;
        }
        

		override public void ExecuteProcess()
		{
			eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			
			eProcessCompletionStatus status = eProcessCompletionStatus.None;
			

			string message = "Executing store to chain forecast rollup for hierarchy:" + HierarchyRID.ToString() +
				"; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumberString;
			try
			{
                
                if (!UseStaticStoredProcedure &&
                    DatabaseVariables.Count == 0)
                {
                    return;
                }
                

				IsRunning = true;

				Audit.Add_Msg(eMIDMessageLevel.Information, message, "StoreToChainForecastRollupProcess");

				try
				{
					try
					{
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreToChainForecastRollupSP);
                        }
                        else if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreToChainForecastHonorLocksRollupSP);
                        }
                        else if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            !ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreToChainForecastNoZeroRollupSP);
                        }
                        else
                        {
                            CreateStoredProcedure("SP_MID_FOR_ST_TO_CHN_ROLLUP");
                            if (HonorLocks)
                            {
                                BuildStoredProcedureWithLocks();
                            }
                            else
                            {
                                BuildStoredProcedure();
                            }
                            WriteStoredProcedure();
                        }
                        
						RollupData.OpenUpdateConnection();
						RollupData.ProcessRollup(StoredProcedure.Name, Process, HierarchyRID, (int)eRollType.storeToChain, Version, Level, BatchNumber);
						RollupData.CommitData();
						
						status = eProcessCompletionStatus.Successful;
					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
						
						status = eProcessCompletionStatus.Failed;
						

					}
					finally
					{
						RollupData.CloseUpdateConnection();
                        
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						
						UpdateRollupProcess(status);
						
					}
				}
				catch ( Exception ex )
				{
					Audit.Log_Exception(ex, GetType().Name);
				}


			}
			catch (ThreadAbortException)
			{
				try
				{
					Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

					messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);

				messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				IsRunning = false;
				CompletionDateTime = DateTime.Now;
				ExitMessageLevel = messageLevel;

				Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
			}
		}

		private void BuildStoredProcedureWithLocks()
		{
            int count;
            string line;
			try
			{
				StoredProcedure.WriteLine("CREATE PROCEDURE " + StoredProcedure.Name);
				StoredProcedure.WriteLine("(");
				StoredProcedure.WriteLine(" @PROCESS INT,");
				StoredProcedure.WriteLine(" @PH_RID INT,");
				StoredProcedure.WriteLine(" @HOME_LEVEL INT,");
				StoredProcedure.WriteLine(" @ITEM_TYPE INT = " + Convert.ToInt32(eRollType.storeToChain).ToString() + ",");
				StoredProcedure.WriteLine(" @FV_RID INT,");
				StoredProcedure.WriteLine(" @BATCH_NUMBER INT,");
				StoredProcedure.WriteLine(" @debug bit = 0");
				StoredProcedure.WriteLine(")");
				StoredProcedure.WriteLine("AS");
				StoredProcedure.WriteLine("SET NOCOUNT ON");
                StoredProcedure.WriteLine("SET ANSI_WARNINGS OFF"); 
				StoredProcedure.WriteLine("-- select records to process");
				StoredProcedure.WriteLine("select * INTO #TEMP_ROLLUP_ITEM" + BatchNumberString + " FROM ROLLUP_ITEM ri with (nolock)");
				StoredProcedure.WriteLine("  where ri.FV_RID = @FV_RID ");
				StoredProcedure.WriteLine("    AND ri.PROCESS = @PROCESS");
				StoredProcedure.WriteLine("    AND ri.ITEM_TYPE = @ITEM_TYPE");
				StoredProcedure.WriteLine("    AND ri.PH_RID = @PH_RID");
				StoredProcedure.WriteLine("    AND ri.HOME_LEVEL = @HOME_LEVEL");
				StoredProcedure.WriteLine("    AND ri.BATCH_NUMBER = @BATCH_NUMBER");
				StoredProcedure.WriteLine("    AND ri.ITEM_PROCESSED is null");
                StoredProcedure.WriteLine(" create clustered index #TEMP_ROLLUP_ITEM_IDX" + BatchNumberString);
                StoredProcedure.WriteLine("	    on #TEMP_ROLLUP_ITEM" + BatchNumberString);
                StoredProcedure.WriteLine("	    (PH_RID, HN_RID)");
                StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_ROLLUP_ITEM" + BatchNumberString);
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("-- create temp table to hold values	");
				StoredProcedure.WriteLine("	");
				count = 0;
				line = "create table #TEMP" + BatchNumberString + " (tmpHN_RID   int not null,";
				StoredProcedure.WriteLine(line);
				line = "                tmpFV_RID   int not null,";
				StoredProcedure.WriteLine(line);
				line = "                tmpTIME_ID  int not null,";
				StoredProcedure.WriteLine(line);
				line = "                tmpPRESENTFLAG  int null,";
				StoredProcedure.WriteLine(line);
				// add value columns
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line = "                tmp" + vp.DatabaseColumnName;
					switch (vp.ChainDatabaseVariableType)
					{
						case eVariableDatabaseType.Integer:
							line += "  int    null,";
							break;
						case eVariableDatabaseType.Real:
							line += "  real   null,";
							break;
						case eVariableDatabaseType.Float:
							line += "  float  null,";
							break;
						case eVariableDatabaseType.BigInteger:
							line += "  bigint null,";
							break;
					}
					StoredProcedure.WriteLine(line);
				}
				// add lock columns
				count = 0;
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line = "                tmp" + vp.DatabaseColumnName + "_LOCK  char(1) null";
					if (count < DatabaseVariables.Count)
					{
						line += ",";
					}
					else
					{
						line += ")";
					}
					StoredProcedure.WriteLine(line);
				}
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine(" alter table #TEMP" + BatchNumberString);
				StoredProcedure.WriteLine("	    add primary key clustered (tmpHN_RID, tmpFV_RID, tmpTIME_ID)");
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("-- build a temp table of keys to be updated along with their lock flags");
				StoredProcedure.WriteLine("INSERT #TEMP" + BatchNumberString + " (tmpHN_RID, tmpFV_RID, tmpTIME_ID, tmpPRESENTFLAG, ");
				count = 0;
				line = Indent15;
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line += "tmp" + vp.DatabaseColumnName + ", ";
					if (line.Length > 90)
					{
						StoredProcedure.WriteLine(line);
						line = Indent15;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}
				count = 0;
				line = Indent15;
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line += "tmp" + vp.DatabaseColumnName + "_LOCK";
					if (count < DatabaseVariables.Count)
					{
						line += ", ";
					}
					else
					{
						line += ")";
					}
					if (line.Length > 90)
					{
						StoredProcedure.WriteLine(line);
						line = Indent15;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}

				StoredProcedure.WriteLine("SELECT vw.HN_RID, vw.FV_RID, ri.TIME_ID, 0, ");
				count = 0;
				line = Indent10;
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line += "0, ";
					if (line.Length > 80)
					{
						StoredProcedure.WriteLine(line);
						line = Indent10;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}
				count = 0;
				line = Indent10;
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line += "COALESCE(cfwl." + vp.DatabaseColumnName + "_LOCK, '0')";
					if (count < DatabaseVariables.Count)
					{
						line += ", ";
					}
					StoredProcedure.WriteLine(line);
					line = Indent10;
				}
				StoredProcedure.WriteLine("         from #TEMP_ROLLUP_ITEM" + BatchNumberString + " ri, VW_CHAIN_FORECAST_WEEK vw LEFT OUTER JOIN CHAIN_FORECAST_WEEK_LOCK cfwl ");
				StoredProcedure.WriteLine("           ON vw.HN_RID = cfwl.HN_RID AND vw.TIME_ID = cfwl.TIME_ID AND vw.FV_RID = cfwl.FV_RID ");
				StoredProcedure.WriteLine("         WHERE ri.PROCESS = @PROCESS ");
				StoredProcedure.WriteLine("           AND ri.ITEM_TYPE = @ITEM_TYPE");
				StoredProcedure.WriteLine("           AND ri.PH_RID = @PH_RID");
				StoredProcedure.WriteLine("           AND ri.HOME_LEVEL = @HOME_LEVEL");
				StoredProcedure.WriteLine("           AND ri.FV_RID = @FV_RID");
				StoredProcedure.WriteLine("           AND vw.HN_RID = ri.HN_RID");
				StoredProcedure.WriteLine("           AND vw.FV_RID = ri.FV_RID");
				StoredProcedure.WriteLine("           AND vw.TIME_ID = ri.TIME_ID");
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP" + BatchNumberString);
				StoredProcedure.WriteLine("	");
				
				StoredProcedure.WriteLine("-- build a temp table of all summed values");
				StoredProcedure.WriteLine("SELECT ri.HN_RID as sumHN_RID, ri.FV_RID as sumFV_RID, ri.TIME_ID as sumTIME_ID, ");
				count = 0;
				line = Indent15;
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					switch (vp.LevelRollType)
					{
						case eLevelRollType.Sum:
							line += "sum(" + vp.DatabaseColumnName + ") as sum" + vp.DatabaseColumnName;
							break;
						case eLevelRollType.Average:
							if (IncludeZeroInAverage)
							{
								line += "avg(" + vp.DatabaseColumnName + ") as sum" + vp.DatabaseColumnName;
							}
							else
							{
								line += "avg(case when " + vp.DatabaseColumnName + " = 0 then null else " + vp.DatabaseColumnName + " end) as sum" + vp.DatabaseColumnName;
							}
							break;
					}
					if (count < DatabaseVariables.Count)
					{
						line += ", ";
					}
					if (line.Length > 80)
					{
						StoredProcedure.WriteLine(line);
						line = Indent10;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}

				StoredProcedure.WriteLine("  INTO #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("  from #TEMP_ROLLUP_ITEM" + BatchNumberString + " ri");
                StoredProcedure.WriteLine("   inner join VW_STORE_FORECAST_WEEK vw with (nolock) on vw.HN_MOD = ri.HN_MOD and vw.HN_RID = ri.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID");
				StoredProcedure.WriteLine("       GROUP BY ri.HN_RID, ri.FV_RID, ri.TIME_ID");
				StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine(" alter table #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("	    add primary key clustered (sumHN_RID, sumFV_RID, sumTIME_ID)");
                StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_SUM" + BatchNumberString);
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("-- update #TEMP with the summed values");
				StoredProcedure.WriteLine("UPDATE #TEMP" + BatchNumberString);
				count = 0;
				line = BlankLine;
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					if (count == 1)
					{
						line = line.Insert(9, "SET");
					}
					line = line.Insert(13, "tmp" + vp.DatabaseColumnName); 
					line = line.TrimEnd();
					line += " = sum" + vp.DatabaseColumnName;
					if (count < DatabaseVariables.Count)
					{
						line += ",";
					}
					StoredProcedure.WriteLine(line);
					line = BlankLine;
				}
				StoredProcedure.WriteLine("       	FROM #TEMP" + BatchNumberString + " tmp");
				StoredProcedure.WriteLine("            JOIN #TEMP_SUM" + BatchNumberString + " ts ON tmpHN_RID = sumHN_RID ");
				StoredProcedure.WriteLine("               AND tmpTIME_ID = sumTIME_ID AND tmpFV_RID = sumFV_RID");
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP" + BatchNumberString);
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("-- deleted updated records from #TEMP_SUM");
				StoredProcedure.WriteLine("delete #TEMP_SUM" + BatchNumberString);
				StoredProcedure.WriteLine("   FROM #TEMP_SUM" + BatchNumberString + " ts INNER JOIN #TEMP" + BatchNumberString + " tmp");
				StoredProcedure.WriteLine("               ON ts.sumHN_RID = tmp.tmpHN_RID");
				StoredProcedure.WriteLine("                 AND ts.sumTIME_ID = tmp.tmpTIME_ID");
				StoredProcedure.WriteLine("                 AND ts.sumFV_RID = tmp.tmpFV_RID");
				StoredProcedure.WriteLine("-- insert other records from #TEMP_SUM");
				StoredProcedure.WriteLine("INSERT #TEMP" + BatchNumberString + " (tmpHN_RID, tmpTIME_ID, tmpFV_RID, tmpPRESENTFLAG, ");
				count = 0;
				line = Indent15;
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line += "tmp" + vp.DatabaseColumnName;
					line += ", ";
					if (line.Length > 90)
					{
						StoredProcedure.WriteLine(line);
						line = Indent15;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}
				count = 0;
				line = Indent15;
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line += "tmp" + vp.DatabaseColumnName + "_LOCK";
					if (count < DatabaseVariables.Count)
					{
						line += ", ";
					}
					else
					{
						line += ")";
					}
					if (line.Length > 90)
					{
						StoredProcedure.WriteLine(line);
						line = Indent15;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}
				count = 0;
				line = Indent10 + "SELECT ts.sumHN_RID, ts.sumTIME_ID, ts.sumFV_RID, 0, ";
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line += "COALESCE(ts.sum" + vp.DatabaseColumnName + ", 0)";
					line += ", ";
					if (line.Length > 90)
					{
						StoredProcedure.WriteLine(line);
						line = Indent15;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}
				count = 0;
				line = Indent15; 
				foreach (VariableProfile vp in DatabaseVariables)
				{
                    string varName = vp.DatabaseColumnName;
					++count;
					line += "0";
					if (count < DatabaseVariables.Count)
					{
						line += ", ";
					}
					if (line.Length > 90)
					{
						StoredProcedure.WriteLine(line);
						line = Indent15;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}
				StoredProcedure.WriteLine("	FROM #TEMP_SUM" + BatchNumberString + " ts");
				count = 0;
				line = Indent15 + "WHERE COALESCE ("; 
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line += "ts.sum" + vp.DatabaseColumnName;
					if (count < DatabaseVariables.Count)
					{
						line += ", ";
					}
					else
					{
						line += ") IS NOT NULL";
					}
					if (line.Length > 90)
					{
						StoredProcedure.WriteLine(line);
						line = Indent15;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}
				StoredProcedure.WriteLine("-- update CHAIN_FORECAST_WEEK" + TableNumber.ToString() + " with the summed values where the values are not locked");
				foreach (VariableProfile vp in DatabaseVariables)
				{
					StoredProcedure.WriteLine("UPDATE CHAIN_FORECAST_WEEK with (rowlock)");
					StoredProcedure.WriteLine("       	SET " + vp.DatabaseColumnName + " = tmp.tmp" + vp.DatabaseColumnName);
					StoredProcedure.WriteLine("           FROM CHAIN_FORECAST_WEEK vw");
					StoredProcedure.WriteLine("            JOIN #TEMP" + BatchNumberString + " tmp ON HN_RID = tmp.tmpHN_RID ");
					StoredProcedure.WriteLine("               AND vw.TIME_ID = tmp.tmpTIME_ID ");
					StoredProcedure.WriteLine("               AND vw.FV_RID = tmp.tmpFV_RID");
					StoredProcedure.WriteLine("          where tmp.tmp" + vp.DatabaseColumnName + "_LOCK = 0     ");
				}
					
				StoredProcedure.WriteLine("       ");
				StoredProcedure.WriteLine("-- flag updated records");
				StoredProcedure.WriteLine("update #TEMP" + BatchNumberString + " SET tmpPRESENTFLAG = 1");
				StoredProcedure.WriteLine("   from #TEMP" + BatchNumberString + " tmp INNER JOIN CHAIN_FORECAST_WEEK vw");
				StoredProcedure.WriteLine("               ON  tmp.tmpHN_RID = vw.HN_RID");
				StoredProcedure.WriteLine("	AND tmp.tmpTIME_ID = vw.TIME_ID");
				StoredProcedure.WriteLine("	AND tmp.tmpFV_RID = vw.FV_RID");
				StoredProcedure.WriteLine("-- insert new values ");
				count = 0;
				line = "INSERT CHAIN_FORECAST_WEEK (HN_RID, FV_RID, TIME_ID, "; 
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line += vp.DatabaseColumnName;
					if (count < DatabaseVariables.Count)
					{
						line += ", ";
					}
					else
					{
						line += ")";
					}
					if (line.Length > 90)
					{
						StoredProcedure.WriteLine(line);
						line = Indent15;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}
				StoredProcedure.WriteLine("       	SELECT tmp.tmpHN_RID, tmp.tmpFV_RID, tmp.tmpTIME_ID, ");
				count = 0;
				line = Indent15; 
				foreach (VariableProfile vp in DatabaseVariables)
				{
					++count;
					line += "tmp.tmp" + vp.DatabaseColumnName;
					if (count < DatabaseVariables.Count)
					{
						line += ", ";
					}
					if (line.Length > 90)
					{
						StoredProcedure.WriteLine(line);
						line = Indent15;
					}
				}
				line = line.TrimEnd();
				if (line.Length > 0)
				{
					StoredProcedure.WriteLine(line);
				}
				StoredProcedure.WriteLine("         FROM #TEMP" + BatchNumberString + " tmp");
				StoredProcedure.WriteLine("	   WHERE tmpPRESENTFLAG = 0");

				StoredProcedure.WriteLine("-- update rollup items that were processed ");
				
				
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) ");
				StoredProcedure.WriteLine(" values(" + eLockType.RollupItem.GetHashCode().ToString() + ", '" + LockID + "') ");
				StoredProcedure.WriteLine("	");

                // Begin TT#4155 - JSmith - Modify Rollup Procedures to Alway Select Correct Database Index
                //StoredProcedure.WriteLine("update ROLLUP_ITEM with (rowlock) ");
                //StoredProcedure.WriteLine("   set ITEM_PROCESSED = '1' ");
                //StoredProcedure.WriteLine("   from ROLLUP_ITEM ri ");
                //StoredProcedure.WriteLine("      JOIN #TEMP_ROLLUP_ITEM" + BatchNumberString + " tri      ");
                //StoredProcedure.WriteLine("         on ri.FV_RID = tri.FV_RID     ");
                //StoredProcedure.WriteLine("        AND ri.PROCESS = tri.PROCESS");
                //StoredProcedure.WriteLine("        AND ri.ITEM_TYPE = tri.ITEM_TYPE");
                //StoredProcedure.WriteLine("        AND ri.PH_RID = tri.PH_RID");
                //StoredProcedure.WriteLine("        AND ri.HOME_LEVEL = tri.HOME_LEVEL");
                //StoredProcedure.WriteLine("        AND ri.HN_RID = tri.HN_RID");
                //StoredProcedure.WriteLine("        AND ri.TIME_ID = tri.TIME_ID");
                //StoredProcedure.WriteLine("        AND ri.BATCH_NUMBER = @BATCH_NUMBER");

                StoredProcedure.WriteLine("update ROLLUP_ITEM with (rowlock) ");
                StoredProcedure.WriteLine("   set ITEM_PROCESSED = '1' ");
                StoredProcedure.WriteLine("   from ROLLUP_ITEM ri ");
                StoredProcedure.WriteLine("      JOIN #TEMP_ROLLUP_ITEM" + BatchNumberString + " tri      ");
                StoredProcedure.WriteLine("         on ri.PROCESS = tri.PROCESS     ");
                StoredProcedure.WriteLine("        AND ri.HN_RID = tri.HN_RID");
                StoredProcedure.WriteLine("        AND ri.TIME_ID = tri.TIME_ID");
                StoredProcedure.WriteLine("        AND ri.FV_RID = tri.FV_RID     ");
                StoredProcedure.WriteLine("        AND ri.ITEM_TYPE = tri.ITEM_TYPE");
                StoredProcedure.WriteLine("        AND ri.PH_RID = tri.PH_RID");
                StoredProcedure.WriteLine("        AND ri.HOME_LEVEL = tri.HOME_LEVEL");
                StoredProcedure.WriteLine("        AND ri.BATCH_NUMBER = @BATCH_NUMBER");
                // End TT#4155 - JSmith - Modify Rollup Procedures to Alway Select Correct Database Index
				
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("delete from VIRTUAL_LOCK ");
				StoredProcedure.WriteLine(" where LOCK_TYPE = " + eLockType.RollupItem.GetHashCode().ToString());
				StoredProcedure.WriteLine("   and LOCK_ID = '" + LockID + "'");
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM" + BatchNumberString);
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_SUM" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("SET ANSI_WARNINGS ON"); 
				StoredProcedure.WriteLine("	");
				
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}

		private void BuildStoredProcedure()
		{
            bool isStore = false;
            bool isForecast = true;
            bool isExternalIntransit = false;

			try
			{
                StoredProcedure.AddCreateProcedure(eRollType.storeToChain, isStore, isForecast);

                StoredProcedure.AddRollupItemTable(BatchNumberString, TableNumber, "@FV_RID", isStore, isExternalIntransit, isDayToWeek: false);

                StoredProcedure.AddClearValues(BatchNumberString, DatabaseVariables, "CHAIN_FORECAST_WEEK", "@FV_RID", ZeroParentsWithNoChildren, isForecast, isLockTable: false);

                StoredProcedure.AddClearValues(BatchNumberString, DatabaseVariables, "CHAIN_FORECAST_WEEK_LOCK", "@FV_RID", ZeroParentsWithNoChildren, isForecast, isLockTable: true);

                StoredProcedure.AddTempJoinTable(BatchNumberString, isStore);

                StoredProcedure.AddTempSumTable(BatchNumberString, DatabaseVariables, isStore, isForecast, isExternalIntransit);

                StoredProcedure.AddSum(BatchNumberString, TableNumber, DatabaseVariables, "VW_STORE_FORECAST_WEEK", IncludeZeroInAverage, isStore, isForecast, isExternalIntransit, setMaxDOPToOne: false, isStoreToChain: true);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("declare @TEMP_SUM" + BatchNumberString + " as MID_CHN_FOR_WK_TYPE	");
                StoredProcedure.WriteLine("declare @Locks as MID_CHN_FOR_WK_LOCK_TYPE	");
                StoredProcedure.WriteLine("declare @SaveLocks char	");
                StoredProcedure.WriteLine("set @SaveLocks = '0'");
                StoredProcedure.WriteLine("	");

                StoredProcedure.AddSumToTypeInsert(BatchNumberString, DatabaseVariables, isStore, isForecast);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("exec dbo.SP_MID_CHN_FOR_WK_WRITE @TEMP_SUM" + BatchNumberString + ",@Locks,@SaveLocks	");

                StoredProcedure.AddUpdateProcessedRecords(BatchNumberString, LockID, isExternalIntransit);

                StoredProcedure.AddDeleteTempTables(BatchNumberString);
			}
			catch (Exception exc)
			{
				Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
				Audit.Log_Exception(exc, GetType().Name);
				throw;
			}
		}
	}


    public class StoreExternalIntransitRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public StoreExternalIntransitRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.storeExternalIntransit, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "StoreExternalIntransitRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        
        public string BuildStoredProcedure(string aProcedureName)
        {
            CreateStaticStoredProcedure(aProcedureName);
            BuildStoredProcedure();
            return StoredProcedure.Text;
        }
        

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			
			eProcessCompletionStatus status = eProcessCompletionStatus.None;
			

            string message = "Executing store external intransit rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumberString;
            try
            {
                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "StoreExternalIntransitRollupProcess");

                try
                {
                    try
                    {
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreExternalIntransitRollupSP);
                        }
                        else
                        {
                            //CreateStoredProcedure("SP_MID_ST_INTRANSIT_ROLLUP");
                            CreateStoredProcedure("Include.DBStoreExternalIntransitRollupSP");
                            BuildStoredProcedure();
                            WriteStoredProcedure();
                        }
                        
                        RollupData.OpenUpdateConnection();
                        RollupData.ProcessRollup(StoredProcedure.Name, Process, HierarchyRID, (int)eRollType.storeExternalIntransit, Version, Level, BatchNumber);
                        RollupData.CommitData();
						
						status = eProcessCompletionStatus.Successful;
					}
					catch ( Exception ex )
					{
						Audit.Log_Exception(ex, GetType().Name);
						
						status = eProcessCompletionStatus.Failed;
						

					}
					finally
					{
						RollupData.CloseUpdateConnection();
                        
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						
						UpdateRollupProcess(status);
						
                    }
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedure()
        {
            bool isStore = false;
            bool isForecast = false;
            bool isExternalIntransit = true;
            ArrayList externalIntransitList = new ArrayList();

            try
            {
                // create VariableProfile for external intransit so can use same methods
                externalIntransitList.Add(new VariableProfile(0, "Intransit", eVariableCategory.Store, eVariableType.Intransit, "UNITS", eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.Sum, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other"));

                StoredProcedure.AddCreateProcedure(eRollType.storeExternalIntransit, isStore, isForecast);

                StoredProcedure.AddRollupItemTable(BatchNumberString, TableNumber, Include.FV_ActualRID.ToString(), isStore, isExternalIntransit, isDayToWeek: false);

                StoredProcedure.WriteLine("-- delete old records");
                StoredProcedure.WriteLine("delete STORE_EXTERNAL_INTRANSIT ");
                StoredProcedure.WriteLine(" from STORE_EXTERNAL_INTRANSIT vw with (rowlock) INNER JOIN #TEMP_ROLLUP_ITEM0 tri");
                StoredProcedure.WriteLine("         ON vw.HN_RID = tri.HN_RID");
                StoredProcedure.WriteLine("        AND vw.TIME_ID = tri.TIME_ID");

                StoredProcedure.AddTempJoinTable(BatchNumberString, isStore);

                StoredProcedure.WriteLine("Delete from #TEMP_JOIN0 where HN_RID in (select HN_RID from COLOR_NODE where COLOR_CODE_RID = 0)");

                StoredProcedure.AddTempSumTable(BatchNumberString, externalIntransitList, isStore, isForecast, isExternalIntransit);

                StoredProcedure.AddSum(BatchNumberString, TableNumber, externalIntransitList, "STORE_EXTERNAL_INTRANSIT", IncludeZeroInAverage, isStore, isForecast, isExternalIntransit, setMaxDOPToOne: true, isStoreToChain: false);
				
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("INSERT STORE_EXTERNAL_INTRANSIT (HN_RID, TIME_ID, ST_RID, UNITS)");
                StoredProcedure.WriteLine("SELECT ts.HN_RID, ts.TIME_ID, ts.ST_RID, ts.UNITS");
                StoredProcedure.WriteLine("FROM #TEMP_SUM0 ts");

                StoredProcedure.AddUpdateProcessedRecords(BatchNumberString, LockID, isExternalIntransit);
                
                StoredProcedure.AddDeleteTempTables(BatchNumberString);
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }


    public class StoreIntransitRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public StoreIntransitRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.storeIntransit, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "StoreIntransitRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        
        public string BuildStoredProcedure(string aProcedureName)
        {
            CreateStaticStoredProcedure(aProcedureName);
            BuildStoredProcedure();
            return StoredProcedure.Text;
        }
        

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			
			eProcessCompletionStatus status = eProcessCompletionStatus.None;
			

            string message = "Executing store intransit rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumberString;
            try
            {
                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "StoreIntransitRollupProcess");

                try
                {
                    try
                    {
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBStoreIntransitRollupSP);
                        }
                        else
                        {
                            //CreateStoredProcedure("SP_MID_ST_INTRANSIT_ROLLUP");
                            CreateStoredProcedure(Include.DBStoreIntransitRollupSP);
                            BuildStoredProcedure();
                            WriteStoredProcedure();
                        }
                        
                        RollupData.OpenUpdateConnection();
                        RollupData.ProcessRollup(StoredProcedure.Name, Process, HierarchyRID, (int)eRollType.storeIntransit, Version, Level, BatchNumber);
						RollupData.CommitData();
						
						status = eProcessCompletionStatus.Successful;
						

					}
					catch ( Exception ex )
					{
						Audit.Log_Exception(ex, GetType().Name);
						
						status = eProcessCompletionStatus.Failed;
						

					}
					finally
					{
						RollupData.CloseUpdateConnection();
                        
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						
						UpdateRollupProcess(status);
						
					}
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedure()
        {
            bool isStore = false;
            bool isForecast = false;
            bool isExternalIntransit = true;
            ArrayList intransitList = new ArrayList();

            try
            {
                // create VariableProfile for intransit so can use same methods
                intransitList.Add(new VariableProfile(0, "Intransit", eVariableCategory.Store, eVariableType.Intransit, "UNITS", eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.Units, eVariableAccess.DisplayOnly, eVariableScope.Static, eVariableSpreadType.None, eVariableWeekType.EOW, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.All, eVariableForecastType.None, eValueFormatType.GenericNumeric, 0, false, false, false, eLevelRollType.Sum, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, "Other"));

                StoredProcedure.AddCreateProcedure(eRollType.storeIntransit, isStore, isForecast);

                StoredProcedure.AddRollupItemTable(BatchNumberString, TableNumber, Include.FV_ActualRID.ToString(), isStore, isExternalIntransit, isDayToWeek: false);

                StoredProcedure.WriteLine("-- delete old records");
                StoredProcedure.WriteLine("delete STORE_INTRANSIT ");
                StoredProcedure.WriteLine(" from STORE_INTRANSIT vw with (rowlock) INNER JOIN #TEMP_ROLLUP_ITEM0 tri");
                StoredProcedure.WriteLine("         ON vw.HN_RID = tri.HN_RID");
                StoredProcedure.WriteLine("        AND vw.TIME_ID = tri.TIME_ID");

                StoredProcedure.AddTempJoinTable(BatchNumberString, isStore);

                StoredProcedure.WriteLine("Delete from #TEMP_JOIN0 where HN_RID in (select HN_RID from COLOR_NODE where COLOR_CODE_RID = 0)");

                StoredProcedure.AddTempSumTable(BatchNumberString, intransitList, isStore, isForecast, isExternalIntransit);

                StoredProcedure.AddSum(BatchNumberString, TableNumber, intransitList, "STORE_INTRANSIT", IncludeZeroInAverage, isStore, isForecast, isExternalIntransit, setMaxDOPToOne: true, isStoreToChain: false);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("INSERT STORE_INTRANSIT (HN_RID, TIME_ID, ST_RID, UNITS)");
                StoredProcedure.WriteLine("SELECT ts.HN_RID, ts.TIME_ID, ts.ST_RID, ts.UNITS");
                StoredProcedure.WriteLine("FROM #TEMP_SUM0 ts");

                StoredProcedure.AddUpdateProcessedRecords(BatchNumberString, LockID, isExternalIntransit);

                StoredProcedure.AddDeleteTempTables(BatchNumberString);
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }


    public class ChainHistoryDayRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public ChainHistoryDayRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.chainDailyHistory, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "ChainHistoryDayRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        
        public string BuildStoredProcedure(string aProcedureName)
        {
            CreateStaticStoredProcedure(aProcedureName);
            BuildStoredProcedure();
            return StoredProcedure.Text;
        }
        

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			
			eProcessCompletionStatus status = eProcessCompletionStatus.None;
			

            string message = "Executing chain daily history rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumberString;
            try
            {
                
                if (!UseStaticStoredProcedure &&
                    DatabaseVariables.Count == 0)
                {
                    return;
                }

                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "ChainHistoryDayRollupProcess");

                try
                {
                    try
                    {
                        
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBChainDailyHistoryRollupSP);
                        }
                        else
                        {
                            CreateStoredProcedure("SP_MID_CHN_HIS_DAY_WEEK_ROLLUP");
                            BuildStoredProcedure();
                            WriteStoredProcedure();
                        }
                        
                        RollupData.OpenUpdateConnection();
						RollupData.CommitData();
						
						status = eProcessCompletionStatus.Successful;
						

					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
						
						status = eProcessCompletionStatus.Failed;
						

					}
					finally
					{
						RollupData.CloseUpdateConnection();
                        
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						
						UpdateRollupProcess(status);
						
                    }
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedure()
        {

            try
            {
                
                //CreateStoredProcedure("SP_MID_CHN_HIS_DAY_WEEK_ROLLUP");
                
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }

    public class ChainHistoryWeekRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public ChainHistoryWeekRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.chainWeeklyHistory, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "ChainHistoryWeekRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        
        public string BuildStoredProcedure(string aProcedureName)
        {
            CreateStaticStoredProcedure(aProcedureName);
            BuildStoredProcedure();
            return StoredProcedure.Text;
        }
        

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			
			eProcessCompletionStatus status = eProcessCompletionStatus.None;
			

            string message = "Executing chain weekly history rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumber.ToString();
            try
            {
                if (!UseStaticStoredProcedure &&
                    DatabaseVariables.Count == 0)
                {
                    return;
                }

                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "ChainHistoryWeekRollupProcess");

                try
                {
                    try
                    {
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBChainWeeklyHistoryRollupSP);
                        }
                        else if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            !ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBChainWeeklyHistoryNoZeroRollupSP);
                        }
                        else
                        {
                            CreateStoredProcedure("SP_MID_CHN_HIS_WK_ROLLUP");
                            BuildStoredProcedure();
                            WriteStoredProcedure();
                        }
                        
                        RollupData.OpenUpdateConnection();
                        RollupData.ProcessRollup(StoredProcedure.Name, Process, HierarchyRID, (int)eRollType.chainWeeklyHistory, Version, Level, BatchNumber);
						RollupData.CommitData();
						
						status = eProcessCompletionStatus.Successful;
						

					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
						
						status = eProcessCompletionStatus.Failed;
						

					}
					finally
					{
						RollupData.CloseUpdateConnection();
                        
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						
						UpdateRollupProcess(status);
						
					}
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedure()
        {
            bool isStore = false;
            bool isForecast = false;
            bool isExternalIntransit = false;

            try
            {
                StoredProcedure.AddCreateProcedure(eRollType.chainWeeklyHistory, isStore, isForecast);

                StoredProcedure.AddRollupItemTable(BatchNumberString, TableNumber, Include.FV_ActualRID.ToString(), isStore, isExternalIntransit, isDayToWeek: false);

                StoredProcedure.AddClearValues(BatchNumberString, DatabaseVariables, "CHAIN_HISTORY_WEEK", Include.FV_ActualRID.ToString(), ZeroParentsWithNoChildren, isForecast, isLockTable: false);

                StoredProcedure.AddTempJoinTable(BatchNumberString, isStore);

                StoredProcedure.AddTempSumTable(BatchNumberString, DatabaseVariables, isStore, isForecast, isExternalIntransit);

                StoredProcedure.AddSum(BatchNumberString, TableNumber, DatabaseVariables, "CHAIN_HISTORY_WEEK", IncludeZeroInAverage, isStore, isForecast, isExternalIntransit, setMaxDOPToOne: true, isStoreToChain: false);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("declare @TEMP_SUM" + BatchNumberString + " as MID_CHN_HIS_WK_TYPE	");
                StoredProcedure.WriteLine("	");

                StoredProcedure.AddSumToTypeInsert(BatchNumberString, DatabaseVariables, isStore, isForecast);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("exec dbo.SP_MID_CHN_HIS_WK_WRITE @TEMP_SUM" + BatchNumberString + "	");

                StoredProcedure.AddUpdateProcessedRecords(BatchNumberString, LockID, isExternalIntransit);

                StoredProcedure.AddDeleteTempTables(BatchNumberString);
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }

    public class ChainForecastWeekRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public ChainForecastWeekRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.chainWeeklyForecast, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "ChainForecastWeekRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        
        public string BuildStoredProcedure(string aProcedureName, bool aHonorLocks)
        {
            CreateStaticStoredProcedure(aProcedureName);
            if (aHonorLocks)
            {
                BuildStoredProcedureWithLocks();
            }
            else
            {
                BuildStoredProcedure();
            }
            return StoredProcedure.Text;
        }
        

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			
			eProcessCompletionStatus status = eProcessCompletionStatus.None;
			

            string message = "Executing chain weekly forcast rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumberString;
            try
            {
                if (!UseStaticStoredProcedure &&
                    DatabaseVariables.Count == 0)
                {
                    return;
                }
                

                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "ChainForecastWeekRollupProcess");

                try
                {
                    try
                    {
                        if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            !HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBChainWeeklyForecastRollupSP);
                        }
                        else if (UseStaticStoredProcedure &&
                            IncludeZeroInAverage &&
                            HonorLocks &&
                            ZeroParentsWithNoChildren)
                        {
                            CreateStaticStoredProcedure(Include.DBChainWeeklyForecastHonorLocksRollupSP);
                        }
                        else
                        {
                            CreateStoredProcedure("SP_MID_CHN_FOR_WK_ROLLUP");
                            if (HonorLocks)
                            {
                                BuildStoredProcedureWithLocks();
                            }
                            else
                            {
                                BuildStoredProcedure();
                            }
                            WriteStoredProcedure();
                        }
                        
                        
                        RollupData.OpenUpdateConnection();
                        RollupData.ProcessRollup(StoredProcedure.Name, Process, HierarchyRID, (int)eRollType.chainWeeklyForecast, Version, Level, BatchNumber);
						RollupData.CommitData();
						
						status = eProcessCompletionStatus.Successful;
						

					}
					catch ( Exception ex )
					{
						++NumberOfErrors;
						Audit.Log_Exception(ex, GetType().Name);
						
						status = eProcessCompletionStatus.Failed;
						

					}
					finally
					{
						RollupData.CloseUpdateConnection();
                        
                        if (!UseStaticStoredProcedure)
                        {
                            DropStoredProcedure();
                        }
						
						UpdateRollupProcess(status);
						
					}
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedureWithLocks()
        {
            int count;
            string line;
            try
            {
                StoredProcedure.WriteLine("CREATE PROCEDURE " + StoredProcedure.Name);
                StoredProcedure.WriteLine("(");
                StoredProcedure.WriteLine("	@PROCESS INT,");
                StoredProcedure.WriteLine("	@PH_RID INT,");
                StoredProcedure.WriteLine("	@HOME_LEVEL INT,");
                StoredProcedure.WriteLine(" @ITEM_TYPE INT = " + Convert.ToInt32(eRollType.chainWeeklyForecast).ToString() + ",");
                StoredProcedure.WriteLine("	@FV_RID INT,");
                StoredProcedure.WriteLine(" @BATCH_NUMBER INT,");
                StoredProcedure.WriteLine(" @debug bit = 0");

                StoredProcedure.WriteLine(")");
                StoredProcedure.WriteLine("AS");
                StoredProcedure.WriteLine("SET NOCOUNT ON");
                StoredProcedure.WriteLine("SET ANSI_WARNINGS OFF"); 
                StoredProcedure.WriteLine("-- select records to process");
                StoredProcedure.WriteLine("select * INTO #TEMP_ROLLUP_ITEM" + BatchNumberString + " FROM ROLLUP_ITEM ri with (nolock)");
                StoredProcedure.WriteLine("   where ri.FV_RID = @FV_RID     ");
                StoredProcedure.WriteLine("     AND ri.PROCESS = @PROCESS");
                StoredProcedure.WriteLine("     AND ri.ITEM_TYPE = @ITEM_TYPE");
                StoredProcedure.WriteLine("     AND ri.PH_RID = @PH_RID");
                StoredProcedure.WriteLine("     AND ri.HOME_LEVEL = @HOME_LEVEL");
                StoredProcedure.WriteLine("     AND ri.BATCH_NUMBER = @BATCH_NUMBER");
                StoredProcedure.WriteLine("     AND ri.ITEM_PROCESSED is null");
                StoredProcedure.WriteLine(" create clustered index #TEMP_ROLLUP_ITEM_IDX" + BatchNumberString);
                StoredProcedure.WriteLine("	    on #TEMP_ROLLUP_ITEM" + BatchNumberString);
                StoredProcedure.WriteLine("	    (PH_RID, HN_RID)");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_ROLLUP_ITEM" + BatchNumberString);
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- create temp table to hold values	");
                StoredProcedure.WriteLine("	");
                count = 0;
                line = "create table #TEMP" + BatchNumberString + " (tmpHN_RID   int not null,";
                StoredProcedure.WriteLine(line);
                line = "                tmpFV_RID   int not null,";
                StoredProcedure.WriteLine(line);
                line = "                tmpTIME_ID  int not null,";
                StoredProcedure.WriteLine(line);
                line = "                tmpPRESENTFLAG  int null,";
                StoredProcedure.WriteLine(line);
                // add value columns
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line = "                tmp" + vp.DatabaseColumnName;
                    switch (vp.ChainDatabaseVariableType)
                    {
                        case eVariableDatabaseType.Integer:
                            line += "  int    null,";
                            break;
                        case eVariableDatabaseType.Real:
                            line += "  real   null,";
                            break;
                        case eVariableDatabaseType.Float:
                            line += "  float  null,";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line += "  bigint null,";
                            break;
                    }
                    StoredProcedure.WriteLine(line);
                }
                // add lock columns
                count = 0;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line = "                tmp" + vp.DatabaseColumnName + "_LOCK  char(1) null";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    else
                    {
                        line += ")";
                    }
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine(" alter table #TEMP" + BatchNumberString);
                StoredProcedure.WriteLine("	    add primary key clustered (tmpHN_RID, tmpFV_RID, tmpTIME_ID)");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- build a temp table of keys to be updated along with their lock flags");
                StoredProcedure.WriteLine("INSERT #TEMP" + BatchNumberString + " (tmpHN_RID, tmpFV_RID, tmpTIME_ID, tmpPRESENTFLAG, ");
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "tmp" + vp.DatabaseColumnName + ", ";
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "tmp" + vp.DatabaseColumnName + "_LOCK";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ")";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }

                StoredProcedure.WriteLine("SELECT vw.HN_RID, vw.FV_RID, ri.TIME_ID, 0, ");
                count = 0;
                line = Indent10;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "0, ";
                    if (line.Length > 80)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent10;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                count = 0;
                line = Indent10;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(cfwl." + vp.DatabaseColumnName + "_LOCK, '0')";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    StoredProcedure.WriteLine(line);
                    line = Indent10;
                }
                StoredProcedure.WriteLine("         from #TEMP_ROLLUP_ITEM" + BatchNumberString + " ri, CHAIN_FORECAST_WEEK vw LEFT OUTER JOIN CHAIN_FORECAST_WEEK_LOCK cfwl ");
                StoredProcedure.WriteLine("           ON vw.HN_RID = cfwl.HN_RID AND vw.TIME_ID = cfwl.TIME_ID AND vw.FV_RID = cfwl.FV_RID ");
                StoredProcedure.WriteLine("         WHERE ri.PROCESS = @PROCESS ");
                StoredProcedure.WriteLine("           AND ri.ITEM_TYPE = @ITEM_TYPE");
                StoredProcedure.WriteLine("           AND ri.PH_RID = @PH_RID");
                StoredProcedure.WriteLine("           AND ri.HOME_LEVEL = @HOME_LEVEL");
                StoredProcedure.WriteLine("           AND ri.FV_RID = @FV_RID");
                StoredProcedure.WriteLine("           AND vw.HN_RID = ri.HN_RID");
                StoredProcedure.WriteLine("           AND vw.FV_RID = ri.FV_RID");
                StoredProcedure.WriteLine("           AND vw.TIME_ID = ri.TIME_ID");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP" + BatchNumberString);
                StoredProcedure.WriteLine("	");

                StoredProcedure.WriteLine("-- select nodes to roll");

				StoredProcedure.WriteLine("select distinct hnj.PH_RID PH_RID, hnj.PARENT_HN_RID PARENT_HN_RID, hnj.HN_RID HN_RID  ");
                StoredProcedure.WriteLine("  INTO #TEMP_JOIN" + BatchNumberString);
                StoredProcedure.WriteLine("  from #TEMP_ROLLUP_ITEM" + BatchNumberString + " ri");
                StoredProcedure.WriteLine("  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine(" alter table #TEMP_JOIN" + BatchNumberString);
                StoredProcedure.WriteLine("	    add primary key clustered (PARENT_HN_RID, HN_RID)");
                StoredProcedure.WriteLine("	");

                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_JOIN" + BatchNumberString);
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- build a temp table of all summed values");
                StoredProcedure.WriteLine("SELECT ri.HN_RID as sumHN_RID, ri.FV_RID as sumFV_RID, ri.TIME_ID as sumTIME_ID, ");
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "sum(" + vp.DatabaseColumnName + ") as sum" + vp.DatabaseColumnName;
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
				StoredProcedure.WriteLine("         INTO #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("	FROM #TEMP_ROLLUP_ITEM" + BatchNumberString + " ri");
                StoredProcedure.WriteLine("	inner join #TEMP_JOIN" + BatchNumberString + " hnj on hnj.PARENT_HN_RID = ri.HN_RID");
                StoredProcedure.WriteLine(" inner join CHAIN_FORECAST_WEEK vw with (nolock) on vw.HN_RID = hnj.HN_RID and vw.FV_RID = hnj.FV_RID and vw.TIME_ID = hnj.TIME_ID");
				StoredProcedure.WriteLine("       GROUP BY ri.HN_RID, ri.FV_RID, ri.TIME_ID");
                StoredProcedure.WriteLine("	      OPTION (MAXDOP 1)");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine(" alter table #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("	    add primary key clustered (sumHN_RID, sumFV_RID, sumTIME_ID)");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- update #TEMP with the summed values");
                StoredProcedure.WriteLine("UPDATE #TEMP" + BatchNumberString);
                count = 0;
                line = BlankLine;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    if (count == 1)
                    {
                        line = line.Insert(9, "SET");
                    }
                    line = line.Insert(13, "tmp" + vp.DatabaseColumnName);
                    line = line.TrimEnd();
                    line += " = sum" + vp.DatabaseColumnName;
                    if (count < DatabaseVariables.Count)
                    {
                        line += ",";
                    }
                    StoredProcedure.WriteLine(line);
                    line = BlankLine;
                }
                StoredProcedure.WriteLine("       	FROM #TEMP" + BatchNumberString + " tmp");
                StoredProcedure.WriteLine("            JOIN #TEMP_SUM" + BatchNumberString + " ON tmpHN_RID = sumHN_RID ");
                StoredProcedure.WriteLine("               AND tmpTIME_ID = sumTIME_ID AND tmpFV_RID = sumFV_RID");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP" + BatchNumberString);
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- deleted updated records from #TEMP_SUM");
                StoredProcedure.WriteLine("delete #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("   FROM #TEMP_SUM" + BatchNumberString + " ts INNER JOIN #TEMP" + BatchNumberString + " tmp");
                StoredProcedure.WriteLine("               ON ts.sumHN_RID = tmp.tmpHN_RID");
                StoredProcedure.WriteLine("                 AND ts.sumTIME_ID = tmp.tmpTIME_ID");
                StoredProcedure.WriteLine("                 AND ts.sumFV_RID = tmp.tmpFV_RID");
                StoredProcedure.WriteLine("-- insert other records from #TEMP_SUM");
                StoredProcedure.WriteLine("INSERT #TEMP" + BatchNumberString + " (tmpHN_RID, tmpFV_RID, tmpTIME_ID,  tmpPRESENTFLAG, ");
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "tmp" + vp.DatabaseColumnName + ", ";
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "tmp" + vp.DatabaseColumnName + "_LOCK";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ")";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                count = 0;
                line = Indent10 + "SELECT sumHN_RID, sumFV_RID, sumTIME_ID,  0, ";
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "COALESCE(sum" + vp.DatabaseColumnName + ", 0), ";
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    string varName = vp.DatabaseColumnName;
                    ++count;
                    line += "0";
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("	FROM #TEMP_SUM" + BatchNumberString);
                count = 0;
                line = Indent15 + "WHERE COALESCE (";
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "sum" + vp.DatabaseColumnName;
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ") IS NOT NULL";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("-- update CHAIN_FORECAST_WEEK with the summed values where the values are not locked");
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    StoredProcedure.WriteLine("UPDATE CHAIN_FORECAST_WEEK with (rowlock)");
                    StoredProcedure.WriteLine("       	SET " + vp.DatabaseColumnName + " = tmp" + vp.DatabaseColumnName);
                    StoredProcedure.WriteLine("           FROM CHAIN_FORECAST_WEEK vw ");
                    StoredProcedure.WriteLine("            JOIN #TEMP" + BatchNumberString + " ON HN_RID = tmpHN_RID ");
                    StoredProcedure.WriteLine("               AND TIME_ID = tmpTIME_ID ");
                    StoredProcedure.WriteLine("               AND FV_RID = tmpFV_RID");
                    StoredProcedure.WriteLine("          where tmp" + vp.DatabaseColumnName + "_LOCK = 0     ");
                }

                StoredProcedure.WriteLine(" ");
                StoredProcedure.WriteLine("-- flag updated records");
                StoredProcedure.WriteLine("update #TEMP" + BatchNumberString + " SET tmpPRESENTFLAG = 1");
                StoredProcedure.WriteLine("   from #TEMP" + BatchNumberString + " tmp INNER JOIN  CHAIN_FORECAST_WEEK vw");
                StoredProcedure.WriteLine("               ON  tmp.tmpHN_RID = vw.HN_RID");
                StoredProcedure.WriteLine("	AND tmp.tmpTIME_ID = vw.TIME_ID");
                StoredProcedure.WriteLine("	AND tmp.tmpFV_RID = vw.FV_RID");
                StoredProcedure.WriteLine(" ");
                StoredProcedure.WriteLine("-- insert new values ");
                count = 0;
                line = "INSERT CHAIN_FORECAST_WEEK (HN_RID, FV_RID, TIME_ID, ";
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += vp.DatabaseColumnName;
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    else
                    {
                        line += ")";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("       	SELECT tmp.tmpHN_RID, tmp.tmpFV_RID, tmp.tmpTIME_ID, ");
                count = 0;
                line = Indent15;
                foreach (VariableProfile vp in DatabaseVariables)
                {
                    ++count;
                    line += "tmp.tmp" + vp.DatabaseColumnName;
                    if (count < DatabaseVariables.Count)
                    {
                        line += ", ";
                    }
                    if (line.Length > 90)
                    {
                        StoredProcedure.WriteLine(line);
                        line = Indent15;
                    }
                }
                line = line.TrimEnd();
                if (line.Length > 0)
                {
                    StoredProcedure.WriteLine(line);
                }
                StoredProcedure.WriteLine("         FROM #TEMP" + BatchNumberString + " tmp");
                StoredProcedure.WriteLine("	   WHERE tmpPRESENTFLAG = 0");

                StoredProcedure.WriteLine("-- update rollup items that were processed ");
				
				
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) ");
				StoredProcedure.WriteLine(" values(" + eLockType.RollupItem.GetHashCode().ToString() + ", '" + LockID + "') ");
				StoredProcedure.WriteLine("	");

                // Begin TT#4155 - JSmith - Modify Rollup Procedures to Alway Select Correct Database Index
                //StoredProcedure.WriteLine("update ROLLUP_ITEM with (rowlock) ");
                //StoredProcedure.WriteLine("   set ITEM_PROCESSED = '1' ");
                //StoredProcedure.WriteLine("   from ROLLUP_ITEM ri ");
                //StoredProcedure.WriteLine("      JOIN #TEMP_ROLLUP_ITEM" + BatchNumberString + " tri      ");
                //StoredProcedure.WriteLine("         on ri.FV_RID = tri.FV_RID     ");
                //StoredProcedure.WriteLine("        AND ri.PROCESS = tri.PROCESS");
                //StoredProcedure.WriteLine("        AND ri.ITEM_TYPE = tri.ITEM_TYPE");
                //StoredProcedure.WriteLine("        AND ri.PH_RID = tri.PH_RID");
                //StoredProcedure.WriteLine("        AND ri.HOME_LEVEL = tri.HOME_LEVEL");
                //StoredProcedure.WriteLine("        AND ri.HN_RID = tri.HN_RID");
                //StoredProcedure.WriteLine("        AND ri.TIME_ID = tri.TIME_ID");
                //StoredProcedure.WriteLine("        AND ri.BATCH_NUMBER = @BATCH_NUMBER");

                StoredProcedure.WriteLine("update ROLLUP_ITEM with (rowlock) ");
                StoredProcedure.WriteLine("   set ITEM_PROCESSED = '1' ");
                StoredProcedure.WriteLine("   from ROLLUP_ITEM ri ");
                StoredProcedure.WriteLine("      JOIN #TEMP_ROLLUP_ITEM" + BatchNumberString + " tri      ");
                StoredProcedure.WriteLine("         on ri.PROCESS = tri.PROCESS     ");
                StoredProcedure.WriteLine("        AND ri.HN_RID = tri.HN_RID");
                StoredProcedure.WriteLine("        AND ri.TIME_ID = tri.TIME_ID");
                StoredProcedure.WriteLine("        AND ri.FV_RID = tri.FV_RID     ");
                StoredProcedure.WriteLine("        AND ri.ITEM_TYPE = tri.ITEM_TYPE");
                StoredProcedure.WriteLine("        AND ri.PH_RID = tri.PH_RID");
                StoredProcedure.WriteLine("        AND ri.HOME_LEVEL = tri.HOME_LEVEL");
                StoredProcedure.WriteLine("        AND ri.BATCH_NUMBER = @BATCH_NUMBER");
                // End TT#4155 - JSmith - Modify Rollup Procedures to Alway Select Correct Database Index
				
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("delete from VIRTUAL_LOCK ");
				StoredProcedure.WriteLine(" where LOCK_TYPE = " + eLockType.RollupItem.GetHashCode().ToString());
				StoredProcedure.WriteLine("   and LOCK_ID = '" + LockID + "'");
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM" + BatchNumberString);
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_JOIN" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_JOIN" + BatchNumberString);
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_SUM" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("SET ANSI_WARNINGS ON"); 
				StoredProcedure.WriteLine("	");
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        private void BuildStoredProcedure()
        {
            bool isStore = false;
            bool isForecast = true;
            bool isExternalIntransit = false;

            try
            {
                StoredProcedure.AddCreateProcedure(eRollType.chainWeeklyForecast, isStore, isForecast);

                StoredProcedure.AddRollupItemTable(BatchNumberString, TableNumber, "@FV_RID", isStore, isExternalIntransit, isDayToWeek: false);

                StoredProcedure.AddClearValues(BatchNumberString, DatabaseVariables, "CHAIN_FORECAST_WEEK", "@FV_RID", ZeroParentsWithNoChildren, isForecast, isLockTable: false);

                StoredProcedure.AddClearValues(BatchNumberString, DatabaseVariables, "CHAIN_FORECAST_WEEK_LOCK", "@FV_RID", ZeroParentsWithNoChildren, isForecast, isLockTable: true);

                StoredProcedure.AddTempJoinTable(BatchNumberString, isStore);

                StoredProcedure.AddTempSumTable(BatchNumberString, DatabaseVariables, isStore, isForecast, isExternalIntransit);

                StoredProcedure.AddSum(BatchNumberString, TableNumber, DatabaseVariables, "CHAIN_FORECAST_WEEK", IncludeZeroInAverage, isStore, isForecast, isExternalIntransit, setMaxDOPToOne: true, isStoreToChain: false);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("declare @TEMP_SUM" + BatchNumberString + " as MID_CHN_FOR_WK_TYPE	");
                StoredProcedure.WriteLine("declare @Locks as MID_CHN_FOR_WK_LOCK_TYPE	");
                StoredProcedure.WriteLine("declare @SaveLocks char	");
                StoredProcedure.WriteLine("set @SaveLocks = '0'");
                StoredProcedure.WriteLine("	");

                StoredProcedure.AddSumToTypeInsert(BatchNumberString, DatabaseVariables, isStore, isForecast);

                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("exec dbo.SP_MID_CHN_FOR_WK_WRITE @TEMP_SUM" + BatchNumberString + ",@Locks,@SaveLocks	");

                StoredProcedure.AddUpdateProcessedRecords(BatchNumberString, LockID, isExternalIntransit);

                StoredProcedure.AddDeleteTempTables(BatchNumberString);
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }


    public class DummyColorExternalIntransitRollupProcess : RollupProcess
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public DummyColorExternalIntransitRollupProcess(SessionAddressBlock aSAB, Audit aAudit, ArrayList aDatabaseVariables, int aProcess, int aHierarchyRID, int aVersion, int aLevel, int aBatchNumber, int aTableNumber, bool aIncludeZeroInAverage, bool aHonorLocks, bool aZeroParentsWithNoChildren, bool aUseStaticStoredProcedure, string aProcessID)
            : base(aSAB, aAudit, aDatabaseVariables, eRollType.dummyColor, aProcess, aHierarchyRID, aVersion, aLevel, aBatchNumber, aTableNumber, aIncludeZeroInAverage, aHonorLocks, aZeroParentsWithNoChildren, aUseStaticStoredProcedure, aProcessID)
        {
            try
            {

            }
            catch (Exception exc)
            {
                ExitMessageLevel = eMIDMessageLevel.Severe;
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "DummyColorExternalIntransitRollupProcess");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========


        //========
        // METHODS
        //========

        override public void ExecuteProcess()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
			
			eProcessCompletionStatus status = eProcessCompletionStatus.None;
			

            string message = "Executing external intransit color rollup for hierarchy:" + HierarchyRID.ToString() +
                "; version=" + Version.ToString() + "; level=" + Level.ToString() + "; batch number=" + BatchNumberString;
            try
            {
                IsRunning = true;

                Audit.Add_Msg(eMIDMessageLevel.Information, message, "DummyColorExternalIntransitRollupProcess");

                try
                {
                    try
                    {
                        RollupData.OpenUpdateConnection();
                        RollupData.ProcessRollup("SP_MID_DUMMY_COLOR_ROLLUP", Process, HierarchyRID, (int)eRollType.dummyColor, Version, Level, BatchNumber);
						RollupData.CommitData();
						
						status = eProcessCompletionStatus.Successful;
						

                    }
                    catch (Exception ex)
                    {
                        Audit.Log_Exception(ex, GetType().Name);
						
						status = eProcessCompletionStatus.Failed;
						

                    }
                    finally
                    {
                        RollupData.CloseUpdateConnection();
						
						UpdateRollupProcess(status);
						
                    }
                }
                catch (Exception ex)
                {
                    Audit.Log_Exception(ex, GetType().Name);
                }


            }
            catch (ThreadAbortException)
            {
                try
                {
                    Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

                    messageLevel = eMIDMessageLevel.Severe;
                }
                catch (InvalidOperationException)
                {
                    messageLevel = eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                IsRunning = false;
                CompletionDateTime = DateTime.Now;
                ExitMessageLevel = messageLevel;

                Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
            }
        }

        private void BuildStoredProcedure()
        {
            int count;
            string line;
            try
            {
                CreateStoredProcedure("SP_MID_ST_INTRANSIT_ROLLUP");

                StoredProcedure.WriteLine("CREATE PROCEDURE " + StoredProcedure.Name);
                StoredProcedure.WriteLine("(");
                StoredProcedure.WriteLine(" @PROCESS INT,");
                StoredProcedure.WriteLine(" @PH_RID INT,");
                StoredProcedure.WriteLine(" @HOME_LEVEL INT,");
                StoredProcedure.WriteLine(" @ITEM_TYPE INT = " + Convert.ToInt32(eRollType.storeIntransit).ToString() + ",");
                StoredProcedure.WriteLine(" @BATCH_NUMBER INT,");
                StoredProcedure.WriteLine(" @debug bit = 0");
                StoredProcedure.WriteLine(")");
                StoredProcedure.WriteLine("AS");
                StoredProcedure.WriteLine("SET NOCOUNT ON");
                StoredProcedure.WriteLine("SET ANSI_WARNINGS OFF"); 
                StoredProcedure.WriteLine("DECLARE  @Tables INT");
                StoredProcedure.WriteLine("-- select records to process");
                StoredProcedure.WriteLine("select *  INTO #TEMP_ROLLUP_ITEM FROM ROLLUP_ITEM ri with (nolock)");
                StoredProcedure.WriteLine("  where ri.PROCESS = @PROCESS");
                StoredProcedure.WriteLine("    AND ri.ITEM_TYPE = @ITEM_TYPE");
                StoredProcedure.WriteLine("    AND ri.PH_RID = @PH_RID");
                StoredProcedure.WriteLine("    AND ri.HOME_LEVEL = @HOME_LEVEL");
                StoredProcedure.WriteLine("    AND ri.BATCH_NUMBER = @BATCH_NUMBER");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_ROLLUP_ITEM");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- delete old records");
                StoredProcedure.WriteLine("delete STORE_INTRANSIT ");
                StoredProcedure.WriteLine("  from STORE_INTRANSIT vw with (rowlock) INNER JOIN #TEMP_ROLLUP_ITEM tri");
                StoredProcedure.WriteLine("          ON vw.HN_RID = tri.HN_RID");
                StoredProcedure.WriteLine("         AND vw.TIME_ID = tri.TIME_ID");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("-- select nodes to roll");

				StoredProcedure.WriteLine("select distinct hnj.PH_RID PH_RID, hnj.PARENT_HN_RID PARENT_HN_RID, hnj.HN_RID HN_RID  ");
                StoredProcedure.WriteLine("  INTO #TEMP_JOIN" + BatchNumberString);
                StoredProcedure.WriteLine("  from #TEMP_ROLLUP_ITEM" + BatchNumberString + " ri");
                StoredProcedure.WriteLine("  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine(" alter table #TEMP_JOIN" + BatchNumberString);
                StoredProcedure.WriteLine("	    add primary key clustered (PARENT_HN_RID, HN_RID)");
                StoredProcedure.WriteLine("	");
				
                line = "SELECT tmp.HN_RID as HN_RID, tmp.TIME_ID as TIME_ID, ST_RID as ST_RID, sum(UNITS) as UNITS";
				StoredProcedure.WriteLine(line);
				StoredProcedure.WriteLine("  INTO #TEMP_SUM");
				StoredProcedure.WriteLine("	FROM #TEMP_ROLLUP_ITEM ri");
                StoredProcedure.WriteLine("	inner join #TEMP_JOIN hnj on hnj.PARENT_HN_RID = ri.HN_RID");
                StoredProcedure.WriteLine("	inner join STORE_INTRANSIT vw with (nolock) on vw.HN_RID = hnj.HN_RID and vw.TIME_ID = ri.TIME_ID");
				StoredProcedure.WriteLine("	GROUP BY ri.HN_RID, ST_RID, ri.TIME_ID");
                StoredProcedure.WriteLine("	");
                StoredProcedure.WriteLine("if @debug <> 0 select * from #TEMP_SUM");
                StoredProcedure.WriteLine("	");
                line = Indent10;
                StoredProcedure.WriteLine(" ");
                StoredProcedure.WriteLine("-- insert into the view");
                line = "INSERT STORE_INTRANSIT (HN_RID, TIME_ID, ST_RID, UNITS)";
                line = line.TrimEnd();
                StoredProcedure.WriteLine(line);

                line = Indent10 + "SELECT ts.HN_RID, ts.TIME_ID, ts.ST_RID, ts.UNITS";
                line = line.TrimEnd();
                StoredProcedure.WriteLine(line);
                StoredProcedure.WriteLine("	FROM #TEMP_SUM ts");
                StoredProcedure.WriteLine("	");

				StoredProcedure.WriteLine("-- update rollup items that were processed ");
				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) ");
				StoredProcedure.WriteLine(" values(" + eLockType.RollupItem.GetHashCode().ToString() + ", '" + LockID + "') ");
				StoredProcedure.WriteLine("	");

                // Begin TT#4155 - JSmith - Modify Rollup Procedures to Alway Select Correct Database Index
                //StoredProcedure.WriteLine("update ROLLUP_ITEM with (rowlock) ");
                //StoredProcedure.WriteLine("   set ITEM_PROCESSED = '1' ");
                //StoredProcedure.WriteLine("   from ROLLUP_ITEM ri ");
                //StoredProcedure.WriteLine("      JOIN #TEMP_ROLLUP_ITEM tri      ");
				
                //StoredProcedure.WriteLine("     ON ri.PROCESS = tri.PROCESS");
                //StoredProcedure.WriteLine("        AND ri.ITEM_TYPE = tri.ITEM_TYPE");
                //StoredProcedure.WriteLine("        AND ri.PH_RID = tri.PH_RID");
                //StoredProcedure.WriteLine("        AND ri.HOME_LEVEL = tri.HOME_LEVEL");
                //StoredProcedure.WriteLine("        AND ri.HN_RID = tri.HN_RID");
                //StoredProcedure.WriteLine("        AND ri.TIME_ID = tri.TIME_ID");
                //StoredProcedure.WriteLine("        AND ri.BATCH_NUMBER = @BATCH_NUMBER");

                StoredProcedure.WriteLine("update ROLLUP_ITEM with (rowlock) ");
                StoredProcedure.WriteLine("   set ITEM_PROCESSED = '1' ");
                StoredProcedure.WriteLine("   from ROLLUP_ITEM ri ");
                StoredProcedure.WriteLine("      JOIN #TEMP_ROLLUP_ITEM tri      ");
                StoredProcedure.WriteLine("         on ri.PROCESS = tri.PROCESS     ");
                StoredProcedure.WriteLine("        AND ri.HN_RID = tri.HN_RID");
                StoredProcedure.WriteLine("        AND ri.TIME_ID = tri.TIME_ID");
                StoredProcedure.WriteLine("        AND ri.ITEM_TYPE = tri.ITEM_TYPE");
                StoredProcedure.WriteLine("        AND ri.PH_RID = tri.PH_RID");
                StoredProcedure.WriteLine("        AND ri.HOME_LEVEL = tri.HOME_LEVEL");
                StoredProcedure.WriteLine("        AND ri.BATCH_NUMBER = @BATCH_NUMBER");
                // End TT#4155 - JSmith - Modify Rollup Procedures to Alway Select Correct Database Index

				StoredProcedure.WriteLine("	");
				StoredProcedure.WriteLine("delete from VIRTUAL_LOCK ");
				StoredProcedure.WriteLine(" where LOCK_TYPE = " + eLockType.RollupItem.GetHashCode().ToString());
				StoredProcedure.WriteLine("   and LOCK_ID = '" + LockID + "'");
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM" + BatchNumberString);
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_JOIN" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_JOIN" + BatchNumberString);
                StoredProcedure.WriteLine("   IF OBJECT_ID('tempdb..#TEMP_SUM" + BatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_SUM" + BatchNumberString);
                StoredProcedure.WriteLine("SET ANSI_WARNINGS ON"); 
				StoredProcedure.WriteLine("	");
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteRollup", "CommandThreadProcessor");
                Audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }
    }

    public class StoredProcedure
    {
        //=======
        // FIELDS
        //=======
        private string _name = null;
        private Audit _audit;
        private StringBuilder _text = null;
        private string _indent5 = "     ";
        private string _indent10 = "          ";
        private string _indent15 = "               ";
        private string _blankLine = new string(' ', 100);

        //=============
        // CONSTRUCTORS
        //=============

        public StoredProcedure(Audit aAudit, string aName)
        {
            try
            {
                _audit = aAudit;
                _name = "[dbo].[" + aName + "]";
                _text = new StringBuilder();
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandThreadProcessor");
                _audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========

        public string Name
        {
            get { return _name; }
        }

        public string Text
        {
            get { return _text.ToString(); }
        }

        //========
        // METHODS
        //========

        public void ClearText()
        {
            _text = new StringBuilder();
        }

        private void AddDrop()
        {
            try
            {
                WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + _name + "') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
                WriteLine("drop procedure " + _name);
            }
            catch (Exception exc)
            {
                _audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        public void WriteLine(string aText)
        {
            try
            {
                _text.Append(aText);
                _text.Append(System.Environment.NewLine);
            }
            catch (Exception exc)
            {
                _audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        public void BuildDrop()
        {
            try
            {
                _text = new StringBuilder();
                AddDrop();
            }
            catch (Exception exc)
            {
                _audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        public void OutputSP()
        {
            try
            {
                StreamWriter sw = new StreamWriter(@"./" + this.Name + ".txt");
                sw.WriteLine(_text.ToString());
                sw.Close();
            }
            catch (Exception exc)
            {
                _audit.Log_Exception(exc, GetType().Name);
                throw;
            }
        }

        public void AddCreateProcedure(eRollType aRollType, bool isStore, bool isForecast)
        {
            WriteLine("CREATE PROCEDURE " + Name);
            WriteLine("(");
            WriteLine(" @PROCESS INT,");
            WriteLine(" @PH_RID INT,");
            WriteLine(" @HOME_LEVEL INT,");
            WriteLine(" @ITEM_TYPE INT = " + Convert.ToInt32(aRollType).ToString() + ",");
            if (isForecast)
            {
                WriteLine(" @FV_RID INT,");
            }
            WriteLine(" @BATCH_NUMBER INT,");
            WriteLine(" @debug bit = 0");
            WriteLine(")");
            WriteLine("AS");
            WriteLine("SET NOCOUNT ON");
            WriteLine("SET ANSI_WARNINGS OFF");
            if (isStore)
            {
                WriteLine("DECLARE  @Tables INT");
                WriteLine("-- get the number of store tables");
                WriteLine("SELECT @Tables = STORE_TABLE_COUNT FROM SYSTEM_OPTIONS");
            }
        }

        public void AddRollupItemTable(string aBatchNumberString, int aTableNumber, string aFV_RID, bool isStore, bool isExternalIntransit, bool isDayToWeek)
        {
            WriteLine(" ");
            WriteLine("-- select records to process");
            WriteLine("CREATE table #TEMP_ROLLUP_ITEM" + aBatchNumberString + " (");
            WriteLine("   [PROCESS] [int] NOT NULL,");
            WriteLine("   [HN_RID] [int] NOT NULL,");
            WriteLine("   [TIME_ID] [int] NOT NULL,");
            WriteLine("   [FV_RID] [int] NOT NULL,");
            WriteLine("   [ITEM_TYPE] [int] NOT NULL,");
            WriteLine("   [PH_RID] [int] NOT NULL,");
            WriteLine("   [HOME_LEVEL] [int] NOT NULL,");
            WriteLine("   [FIRST_DAY_OF_WEEK] [int] NOT NULL,");
            WriteLine("   [LAST_DAY_OF_WEEK] [int] NOT NULL,");
            WriteLine("   [BATCH_NUMBER] [int] NULL,");
            WriteLine("   [FIRST_DAY_OF_NEXT_WEEK] [int] NULL,");
            WriteLine("   [HN_MOD] [int] NULL,");
            WriteLine("   [GENERATED_ITEM] [char](1) NULL,");
            WriteLine("   [ITEM_PROCESSED] [char](1) NULL,");
            WriteLine("   [ALTERNATES_ONLY] [char](1) NULL,");
            WriteLine("PRIMARY KEY CLUSTERED ");
            WriteLine("(");
            WriteLine("	[PH_RID],");
            WriteLine("	[HN_RID],");
            WriteLine("	[FV_RID],");
            WriteLine("	[TIME_ID],");
            WriteLine("	[ITEM_TYPE],");
            WriteLine("	[PROCESS]");
            WriteLine(")WITH (IGNORE_DUP_KEY = OFF)");
            WriteLine(")");
            WriteLine(" ");
            WriteLine("insert INTO #TEMP_ROLLUP_ITEM" + aBatchNumberString + " (PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK, ");
            WriteLine("   BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY)");
            WriteLine("select PROCESS, HN_RID, TIME_ID, FV_RID, ITEM_TYPE, PH_RID, HOME_LEVEL, FIRST_DAY_OF_WEEK, LAST_DAY_OF_WEEK,");
            WriteLine("  BATCH_NUMBER, FIRST_DAY_OF_NEXT_WEEK, HN_MOD, GENERATED_ITEM, ITEM_PROCESSED, ALTERNATES_ONLY");
            WriteLine("  FROM ROLLUP_ITEM ri with (nolock)");
            if (isExternalIntransit ||
                isDayToWeek)
            {
                WriteLine("  where ri.PROCESS = @PROCESS");
            }
            else
            {
                WriteLine("  where ri.FV_RID = " + aFV_RID);
                WriteLine("    AND ri.PROCESS = @PROCESS");
            }
            WriteLine("    AND ri.ITEM_TYPE = @ITEM_TYPE");
            WriteLine("    AND ri.PH_RID = @PH_RID");
            WriteLine("    AND ri.HOME_LEVEL = @HOME_LEVEL");
            WriteLine("    AND ri.BATCH_NUMBER = @BATCH_NUMBER");
            WriteLine("    AND ri.ITEM_PROCESSED is null");
            if (isStore)
            {
                WriteLine("    AND ri.HN_MOD = " + aTableNumber.ToString());
                WriteLine("    AND ri.HN_RID % @Tables = " + aTableNumber.ToString());
            }
            WriteLine(" ");
            WriteLine("if @debug <> 0 select * from #TEMP_ROLLUP_ITEM" + aBatchNumberString);
            WriteLine("	");
        }

        public void AddClearValues(string aBatchNumberString, ArrayList aDatabaseVariables, string aTableName, string aFV_RID, bool aZeroParentsWithNoChildren, bool isForecast, bool isLockTable)
        {
            int count;
            string line;

            if (aZeroParentsWithNoChildren)
            {
                WriteLine("-- clear existing values");
                WriteLine("UPDATE " + aTableName + " with (rowlock)");
                count = 0;
                line = _blankLine;
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    if (line.TrimEnd().Length > 0)
                    {
                        line += ",";
                        WriteLine(line);
                    }
                    line = _blankLine;
                    ++count;
                    if (count == 1)
                    {
                        line = line.Insert(9, "SET");
                    }
                    line = line.Insert(13, vp.DatabaseColumnName);
                    if (isLockTable)
                    {
                        line = line.TrimEnd() + "_LOCK";
                    }
                    line = line.TrimEnd();
                    line += " = null";

                }
                if (line.TrimEnd().Length > 0)
                {
                    WriteLine(line);
                }
                WriteLine("  from " + aTableName + " vw INNER JOIN #TEMP_ROLLUP_ITEM" + aBatchNumberString + " tri");
                // Begin TT#5485 - JSmith - Performance
                //if (isForecast)
                //{
                //    WriteLine("         ON vw.FV_RID = @FV_RID");
                //    WriteLine("        AND vw.FV_RID = tri.FV_RID");
                //}
                //else
                //{
                //    WriteLine("          ON tri.FV_RID = " + aFV_RID);
                //}
                //WriteLine("         AND vw.HN_RID = tri.HN_RID");
                //WriteLine("         AND vw.TIME_ID = tri.TIME_ID");
                WriteLine("          ON vw.HN_RID = tri.HN_RID");
                WriteLine("         AND vw.TIME_ID = tri.TIME_ID");
                if (isForecast)
                {
                    WriteLine("        AND vw.FV_RID = @FV_RID");
                    WriteLine("        AND vw.FV_RID = tri.FV_RID");
                }
                else
                {
                    WriteLine("         AND tri.FV_RID = " + aFV_RID);
                }
                
                // End TT#5485 - JSmith - Performance
                WriteLine(" ");
            }
        }

        public void AddTempJoinTable(string aBatchNumberString, bool isStore)
        {
            WriteLine("-- select nodes to roll");
            WriteLine("CREATE table #TEMP_JOIN" + aBatchNumberString + " (");
            WriteLine("   [PH_RID] [int] NOT NULL,");
            WriteLine("   [PARENT_HN_RID] [int] NOT NULL,");
            WriteLine("   [HN_RID] [int] NOT NULL,");
            if (isStore)
            {
                WriteLine("   [HN_MOD] [int] NOT NULL,");
            }
            WriteLine("PRIMARY KEY CLUSTERED ");
            WriteLine("(");
            WriteLine("	[PARENT_HN_RID],");
            WriteLine("	[HN_RID]");
            WriteLine(")WITH (IGNORE_DUP_KEY = OFF)");
            WriteLine(")");
            WriteLine(" ");
            
            if (isStore)
            {
                WriteLine("insert INTO #TEMP_JOIN" + aBatchNumberString + "(PH_RID, PARENT_HN_RID, HN_RID, HN_MOD)");
                WriteLine("select distinct hnj.PH_RID, hnj.PARENT_HN_RID, hnj.HN_RID, hnj.HN_RID % @Tables   ");
            }
            else
            {
                WriteLine("insert INTO #TEMP_JOIN" + aBatchNumberString + "(PH_RID, PARENT_HN_RID, HN_RID)");
                WriteLine("select distinct hnj.PH_RID, hnj.PARENT_HN_RID, hnj.HN_RID   ");
            }
            WriteLine("  from #TEMP_ROLLUP_ITEM" + aBatchNumberString + " ri");
            WriteLine("  inner join HIER_NODE_JOIN hnj with (nolock) on hnj.PH_RID = ri.PH_RID and hnj.PARENT_HN_RID = ri.HN_RID");
            WriteLine("	");
        }

        public void AddTempSumTable(string aBatchNumberString, ArrayList aDatabaseVariables, bool isStore, bool isForecast, bool isExternalIntransit, bool addPresentFlag = false)
        {
            //int count = 0;
            string line;

            WriteLine("CREATE table #TEMP_SUM" + aBatchNumberString + "(");
            if (isStore)
            {
                WriteLine("[HN_MOD] [int] NOT NULL,");
            }
            WriteLine("[HN_RID] [int] NOT NULL,");
            if (isForecast)
            {
                WriteLine("[FV_RID] [int] NOT NULL,");
            }
            WriteLine("[TIME_ID] [int] NOT NULL,");
            if (isStore ||
                isExternalIntransit)
            {
                WriteLine("[ST_RID] [int] NOT NULL,");
            }
            if (addPresentFlag)
            {
                WriteLine("[PRESENTFLAG] [int] NULL,");
            }
            if (isExternalIntransit)
            {
                WriteLine("[UNITS] [int] NULL,");
            }
            else
            {
                foreach (VariableProfile vp in aDatabaseVariables)
                {
                    // Begin TT#4412 - JSmith - Overflow
                    //line = "[" + vp.DatabaseColumnName + "] [int] NULL,";
                    // Begin TT#5647 - JSmith - SP_MID_CHN_HIS_WK_ROLLUP temp tables in the procedure contain fields that are not big enough.
                    //switch (vp.StoreDatabaseVariableType)
                    eVariableDatabaseType databaseVariableType;
                    if (isStore)
                    {
                        databaseVariableType = vp.StoreDatabaseVariableType;
                    }
                    else
                    {
                        databaseVariableType = vp.ChainDatabaseVariableType;
                    }
                    switch (databaseVariableType)
                    // End TT#5647 - JSmith - SP_MID_CHN_HIS_WK_ROLLUP temp tables in the procedure contain fields that are not big enough.
                    {
                        case eVariableDatabaseType.Integer:
                            line = "[" + vp.DatabaseColumnName + "] [int] NULL,";
                            break;
                        case eVariableDatabaseType.Real:
                            line = "[" + vp.DatabaseColumnName + "] [real] NULL,";
                            break;
                        case eVariableDatabaseType.Float:
                            line = "[" + vp.DatabaseColumnName + "] [float] NULL,";
                            break;
                        case eVariableDatabaseType.BigInteger:
                            line = "[" + vp.DatabaseColumnName + "] [bigint] NULL,";
                            break;
                        default:
                            line = "[" + vp.DatabaseColumnName + "] [int] NULL,";
                            break;
                    }
                    // End TT#4412 - JSmith - Overflow
                    //line = "[" + vp.DatabaseColumnName + "] [int] NULL";
                    //if (count < aDatabaseVariables.Count)
                    //{
                    //    line += ", ";
                    //}
                    WriteLine(line);
                }
            }
            WriteLine("PRIMARY KEY CLUSTERED ");
            WriteLine("(");
            if (isStore)
            {
                WriteLine("	[HN_MOD],");
            }
            WriteLine("	[HN_RID],");
            if (isForecast)
            {
                WriteLine("	[FV_RID],");
            }
            if (isStore ||
                isExternalIntransit)
            {
                WriteLine("	[TIME_ID],");
                WriteLine("	[ST_RID]");
            }
            else
            {
                WriteLine("	[TIME_ID]");
            }
            WriteLine(")WITH (IGNORE_DUP_KEY = OFF)");
            WriteLine(")");
        }

        public void AddSum(string aBatchNumberString, int aTableNumber, ArrayList aDatabaseVariables, string aViewName, bool aIncludeZeroInAverage, bool isStore, bool isForecast, bool isExternalIntransit, bool setMaxDOPToOne, bool isStoreToChain)
        {
            int count = 0;
            string line;
            if (isStore)
            {
                if (isForecast)
                {
                    line = "insert into #TEMP_SUM" + aBatchNumberString + " (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, ";
                }
                else
                {
                    line = "insert into #TEMP_SUM" + aBatchNumberString + " (HN_MOD, HN_RID, TIME_ID, ST_RID, ";
                }
            }
            else
            {
                if (isForecast)
                {
                    line = "insert into #TEMP_SUM" + aBatchNumberString + " (HN_RID, FV_RID, TIME_ID, ";
                }
                else if (isExternalIntransit)
                {
                    line = "insert into #TEMP_SUM" + aBatchNumberString + " (HN_RID, TIME_ID, ST_RID, ";
                }
                else
                {
                    line = "insert into #TEMP_SUM" + aBatchNumberString + " (HN_RID, TIME_ID, ";
                }
            }
            foreach (VariableProfile vp in aDatabaseVariables)
            {
                ++count;
                line += vp.DatabaseColumnName;

                if (count < aDatabaseVariables.Count)
                {
                    line += ", ";
                }
                if (line.Length > 80)
                {
                    WriteLine(line);
                    line = _indent10;
                }
            }
            line = line.TrimEnd();
            line += ")";
            if (line.Length > 0)
            {
                WriteLine(line);
            }

            count = 0;
            if (isStore)
            {
                if (isForecast)
                {
                    line = "SELECT " + aTableNumber.ToString() + ", ri.HN_RID, ri.FV_RID, ri.TIME_ID, ST_RID, ";
                }
                else
                {
                    line = "SELECT " + aTableNumber.ToString() + ", ri.HN_RID, ri.TIME_ID, ST_RID, ";
                }
            }
            else
            {
                if (isForecast)
                {
                    line = "SELECT ri.HN_RID, ri.FV_RID, ri.TIME_ID, ";
                }
                else if (isExternalIntransit)
                {
                    line = "SELECT ri.HN_RID, ri.TIME_ID, ST_RID, ";
                }
                else
                {
                    line = "SELECT ri.HN_RID, ri.TIME_ID, ";
                }
            }
            foreach (VariableProfile vp in aDatabaseVariables)
            {
                ++count;
                if (isStoreToChain)
                {
                    switch (vp.StoreToChainRollType)
                    {
                        case eStoreToChainRollType.Sum:
                            line += "sum(" + vp.DatabaseColumnName + ")";
                            break;
                        case eStoreToChainRollType.Average:
                            if (aIncludeZeroInAverage)
                            {
                                line += "avg(" + vp.DatabaseColumnName + ")";
                            }
                            else
                            {
                                line += "avg(case when " + vp.DatabaseColumnName + " = 0 then null else " + vp.DatabaseColumnName + " end)";
                            }
                            break;
                    }
                }
                else
                {
                    switch (vp.LevelRollType)
                    {
                        case eLevelRollType.Sum:
                            line += "sum(" + vp.DatabaseColumnName + ")";
                            break;
                        case eLevelRollType.Average:
                            if (aIncludeZeroInAverage)
                            {
                                line += "avg(" + vp.DatabaseColumnName + ")";
                            }
                            else
                            {
                                line += "avg(case when " + vp.DatabaseColumnName + " = 0 then null else " + vp.DatabaseColumnName + " end)";
                            }
                            break;
                    }
                }
                if (count < aDatabaseVariables.Count)
                {
                    line += ", ";
                }
                if (line.Length > 80)
                {
                    WriteLine(line);
                    line = _indent10;
                }
            }
            line = line.TrimEnd();
            if (line.Length > 0)
            {
                WriteLine(line);
            }

            WriteLine("	FROM #TEMP_ROLLUP_ITEM" + aBatchNumberString + " ri");
            if (!isStoreToChain)
            {
                WriteLine("	inner join #TEMP_JOIN" + aBatchNumberString + " hnj on hnj.PARENT_HN_RID = ri.HN_RID");
            }
            if (isStore)
            {
                if (isForecast)
                {
                    if (isStoreToChain)
                    {
                        WriteLine("	inner join " + aViewName + " vw with (nolock) on vw.HN_MOD = ri.HN_MOD and vw.HN_RID = ri.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID");
                        WriteLine("	GROUP BY ri.HN_RID, ri.FV_RID, ri.TIME_ID");
                    }
                    else
                    {
                        WriteLine("	inner join " + aViewName + " vw with (nolock) on vw.HN_MOD = hnj.HN_MOD and vw.HN_RID = hnj.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID");
                        WriteLine("	GROUP BY ri.HN_RID, ri.FV_RID, ST_RID, ri.TIME_ID");
                    }
                }
                else
                {
                    if (isStoreToChain)
                    {
                        WriteLine("	inner join " + aViewName + " vw with (nolock) on vw.HN_MOD = ri.HN_MOD and vw.HN_RID = ri.HN_RID and vw.TIME_ID = ri.TIME_ID");
                        WriteLine("	GROUP BY ri.HN_RID, ri.TIME_ID");
                    }
                    else
                    {
                        WriteLine("	inner join " + aViewName + " vw with (nolock) on vw.HN_MOD = hnj.HN_MOD and vw.HN_RID = hnj.HN_RID and vw.TIME_ID = ri.TIME_ID");
                        WriteLine("	GROUP BY ri.HN_RID, ST_RID, ri.TIME_ID");
                    }
                }
            }
            else
            {
                if (isForecast)
                {
                    if (isStoreToChain)
                    {
                        WriteLine("	inner join " + aViewName + " vw with (nolock) on vw.HN_MOD = ri.HN_MOD and vw.HN_RID = ri.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID");
                    }
                    else
                    {
                        WriteLine("	inner join " + aViewName + " vw with (nolock) on vw.HN_RID = hnj.HN_RID and vw.FV_RID = ri.FV_RID and vw.TIME_ID = ri.TIME_ID");
                    }
                    WriteLine("	GROUP BY ri.HN_RID, ri.FV_RID, ri.TIME_ID");
                }
                else
                {
                    if (isStoreToChain)
                    {
                        WriteLine("	inner join " + aViewName + " vw with (nolock) on vw.HN_MOD = ri.HN_MOD and vw.HN_RID = ri.HN_RID and vw.TIME_ID = ri.TIME_ID");
                    }
                    else
                    {
                        WriteLine("	inner join " + aViewName + " vw with (nolock) on vw.HN_RID = hnj.HN_RID and vw.TIME_ID = ri.TIME_ID");
                    }
                    if (isExternalIntransit)
                    {
                        WriteLine("	GROUP BY ri.HN_RID, ri.TIME_ID, ST_RID");
                    }
                    else
                    {
                        WriteLine("	GROUP BY ri.HN_RID, ri.TIME_ID");
                    }
                }
            }
            if (setMaxDOPToOne)
            {
                WriteLine("	OPTION (MAXDOP 1)");
            }
            WriteLine("	");
            WriteLine("if @debug <> 0 select * from #TEMP_SUM" + aBatchNumberString);
            WriteLine("	");
        }

        public void AddSumToTypeInsert(string aBatchNumberString, ArrayList aDatabaseVariables, bool isStore, bool isForecast)
        {
            int count = 0;
            string line;
            if (isStore)
            {
                if (isForecast)
                {
                    line = "insert into @TEMP_SUM" + aBatchNumberString + " (HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, ";
                }
                else
                {
                    line = "insert into @TEMP_SUM" + aBatchNumberString + " (HN_MOD, HN_RID, TIME_ID, ST_RID, ";
                }
            }
            else
            {
                if (isForecast)
                {
                    line = "insert into @TEMP_SUM" + aBatchNumberString + " (HN_RID, FV_RID, TIME_ID, ";
                }
                else
                {
                    line = "insert into @TEMP_SUM" + aBatchNumberString + " (HN_RID, TIME_ID, ";
                }
            }
            foreach (VariableProfile vp in aDatabaseVariables)
            {
                ++count;
                line += vp.DatabaseColumnName;

                if (count < aDatabaseVariables.Count)
                {
                    line += ", ";
                }
                if (line.Length > 80)
                {
                    WriteLine(line);
                    line = _indent10;
                }
            }
            line = line.TrimEnd();
            line += ")";
            if (line.Length > 0)
            {
                WriteLine(line);
            }

            count = 0;
            if (isStore)
            {
                if (isForecast)
                {
                    line = "SELECT HN_MOD, HN_RID, FV_RID, TIME_ID, ST_RID, ";
                }
                else
                {
                    line = "SELECT HN_MOD, HN_RID, TIME_ID, ST_RID, ";
                }
            }
            else
            {
                if (isForecast)
                {
                    line = "SELECT HN_RID, FV_RID, TIME_ID, ";
                }
                else
                {
                    line = "SELECT HN_RID, TIME_ID, ";
                }
            }
            foreach (VariableProfile vp in aDatabaseVariables)
            {
                ++count;
                line += vp.DatabaseColumnName;
                
                if (count < aDatabaseVariables.Count)
                {
                    line += ", ";
                }
                if (line.Length > 80)
                {
                    WriteLine(line);
                    line = _indent10;
                }
            }
            line = line.TrimEnd();
            if (line.Length > 0)
            {
                WriteLine(line);
            }

            WriteLine("	FROM #TEMP_SUM" + aBatchNumberString);
            WriteLine("	");
            WriteLine("if @debug <> 0 select * from @TEMP_SUM" + aBatchNumberString);
            WriteLine("	");
        }

        public void AddUpdateProcessedRecords(string aBatchNumberString, string aLockID, bool isExternalIntransit)
        {
            WriteLine("	");
            WriteLine("-- update rollup items that were processed ");

            WriteLine("	");
            WriteLine("insert into VIRTUAL_LOCK(LOCK_TYPE, LOCK_ID) ");
            WriteLine(" values(" + eLockType.RollupItem.GetHashCode().ToString() + ", '" + aLockID + "') ");
            WriteLine("	");

            WriteLine("update ROLLUP_ITEM with (rowlock) ");
            WriteLine("   set ITEM_PROCESSED = '1' ");
            WriteLine("   from ROLLUP_ITEM ri ");
            WriteLine("      JOIN #TEMP_ROLLUP_ITEM" + aBatchNumberString + " tri      ");
            // Begin TT#4155 - JSmith - Modify Rollup Procedures to Alway Select Correct Database Index
            //if (isExternalIntransit)
            //{
            //    WriteLine("         on ri.PROCESS = tri.PROCESS");
            //}
            //else
            //{
            //    WriteLine("         on ri.FV_RID = tri.FV_RID     ");
            //    WriteLine("        AND ri.PROCESS = tri.PROCESS");
            //}
            //WriteLine("        AND ri.ITEM_TYPE = tri.ITEM_TYPE");
            //WriteLine("        AND ri.PH_RID = tri.PH_RID");
            //WriteLine("        AND ri.HOME_LEVEL = tri.HOME_LEVEL");
            //WriteLine("        AND ri.HN_RID = tri.HN_RID");
            //WriteLine("        AND ri.TIME_ID = tri.TIME_ID");
            //WriteLine("        AND ri.BATCH_NUMBER = @BATCH_NUMBER");

            
            WriteLine("         ON ri.HN_RID = tri.HN_RID");
            WriteLine("        AND ri.TIME_ID = tri.TIME_ID");
            if (!isExternalIntransit)
            {
                WriteLine("        AND ri.FV_RID = tri.FV_RID     ");
            }
            WriteLine("        AND ri.ITEM_TYPE = tri.ITEM_TYPE");
            WriteLine("        AND ri.PROCESS = tri.PROCESS");
            WriteLine("        AND ri.PH_RID = tri.PH_RID");
            WriteLine("        AND ri.HOME_LEVEL = tri.HOME_LEVEL");
            WriteLine("        AND ri.BATCH_NUMBER = @BATCH_NUMBER");
            // End TT#4155 - JSmith - Modify Rollup Procedures to Alway Select Correct Database Index

            WriteLine("	");
            WriteLine("delete from VIRTUAL_LOCK ");
            WriteLine(" where LOCK_TYPE = " + eLockType.RollupItem.GetHashCode().ToString());
            WriteLine("   and LOCK_ID = '" + aLockID + "'");
        }

        public void AddDeleteTempTables(string aBatchNumberString)
        {
            WriteLine("   IF OBJECT_ID('tempdb..#TEMP_ROLLUP_ITEM" + aBatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_ROLLUP_ITEM" + aBatchNumberString);
            WriteLine("   IF OBJECT_ID('tempdb..#TEMP_JOIN" + aBatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_JOIN" + aBatchNumberString);
            WriteLine("   IF OBJECT_ID('tempdb..#TEMP_SUM" + aBatchNumberString + "') IS NOT NULL DROP TABLE #TEMP_SUM" + aBatchNumberString);
            WriteLine("SET ANSI_WARNINGS ON");
            WriteLine("	");
        }
    }
}
