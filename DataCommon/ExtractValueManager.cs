using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDRetail.DataCommon
{
    public class ExtractValueManager
    {
        // dimensions <node,version,plantype,time ID>
        Dictionary<int, Dictionary<int, Dictionary<ePlanType, Dictionary<int, string>>>> _changeValues = null;
        int _currentNodeRID = -1;
        Dictionary<int, Dictionary<ePlanType, Dictionary<int, string>>> _changedNode;
        int _currentVersionRID = -1;
        Dictionary<ePlanType, Dictionary<int, string>> _changedVersion;
        ePlanType _currentPlanType = ePlanType.None;
        Dictionary<int, string> _changedPlanType;  // contains list of time IDs

        public ExtractValueManager()
        {
            try
            {
                _changeValues = new Dictionary<int, Dictionary<int, Dictionary<ePlanType, Dictionary<int, string>>>>();
            }
            catch
            {
                throw;
            }
        }

        public bool ContainsValues
        {
            get { return _changeValues.Count > 0; }
        }

        public bool Clear()
        {
            _changeValues.Clear();
            _currentNodeRID = -1;
            _currentVersionRID = -1;
            _currentPlanType = ePlanType.None;
            return true;
        }

        public bool AddValue(int HN_RID, int FV_RID, ePlanType planType, int TIME_ID)
        {
            // get node dimension
            if (HN_RID != _currentNodeRID)
            {
                if (!_changeValues.TryGetValue(HN_RID, out _changedNode))
                {
                    _changedNode = new Dictionary<int, Dictionary<ePlanType, Dictionary<int, string>>>();
                    _changeValues.Add(HN_RID, _changedNode);
                }
                _currentNodeRID = HN_RID;
                _currentVersionRID = -1;
                _currentPlanType = ePlanType.None;
            }

            // get version dimension
            if (FV_RID != _currentVersionRID)
            {
                if (!_changedNode.TryGetValue(FV_RID, out _changedVersion))
                {
                    _changedVersion = new Dictionary<ePlanType, Dictionary<int, string>>();
                    _changedNode.Add(FV_RID, _changedVersion);
                }
                _currentVersionRID = FV_RID;
                _currentPlanType = ePlanType.None;
            }

            // get plan type dimension
            if (planType != _currentPlanType)
            {
                if (!_changedVersion.TryGetValue(planType, out _changedPlanType))
                {
                    _changedPlanType = new Dictionary<int, string>();
                    _changedVersion.Add(planType, _changedPlanType);
                }
                _currentPlanType = planType;
            }

            // add time ID
            if (!_changedPlanType.ContainsKey(TIME_ID))
            {
                _changedPlanType.Add(TIME_ID, null);
            }

            return true;
        }

        public void PopulateDataTable(DataTable dtVariableValues, bool forExtract = false)
        {
            DateTime updateDateTime;
            DateTime extractDateTime;
            try
            {
                if (forExtract)
                {
                    updateDateTime = DateTime.MinValue;
                    extractDateTime = DateTime.Now;
                }
                else
                {
                    updateDateTime = DateTime.Now;
                    extractDateTime = DateTime.MinValue;
                }

                foreach (KeyValuePair<int, Dictionary<int, Dictionary<ePlanType, Dictionary<int, string>>>> changedNode in _changeValues)
                {
                    _currentNodeRID = changedNode.Key;
                    _changedNode = changedNode.Value;
                    foreach (KeyValuePair<int, Dictionary<ePlanType, Dictionary<int, string>>> changedVersion in _changedNode)
                    {
                        _currentVersionRID = changedVersion.Key;
                        _changedVersion = changedVersion.Value;
                        foreach (KeyValuePair<ePlanType, Dictionary<int, string>> changedPlanType in _changedVersion)
                        {
                            _currentPlanType = changedPlanType.Key;
                            _changedPlanType = changedPlanType.Value;
                            foreach (int timeID in _changedPlanType.Keys)
                            {
                                AddRow(
                                        dtVariableValues,
                                        _currentNodeRID,
                                        _currentVersionRID,
                                        timeID,
                                        _currentPlanType,
                                        updateDateTime,
                                        extractDateTime
                                        );
                            }
                        }
                    }
                }

                return;
            }
            catch
            {
                throw;
            }
        }

        private void AddRow(
            DataTable dtVariableValues,
            int aNodeRID,
            int aFvRID,
            int aTimeID,
            ePlanType aPlanType,
            DateTime aChangeDate,
            DateTime aExtractDate
            )
        {
            try
            {

                DataRow drVariableValues;

                drVariableValues = dtVariableValues.NewRow();
                dtVariableValues.Rows.Add(drVariableValues);

                drVariableValues["HN_RID"] = aNodeRID;
                drVariableValues["FV_RID"] = aFvRID;
                drVariableValues["TIME_ID"] = aTimeID;
                drVariableValues["PLAN_TYPE"] = Convert.ToInt32(aPlanType);
                if (aChangeDate != DateTime.MinValue)
                {
                    drVariableValues["UPDATE_DATE"] = aChangeDate;
                }
                if (aExtractDate != DateTime.MinValue)
                {
                    drVariableValues["EXTRACT_DATE"] = aExtractDate;
                }

                return;
            }
            catch
            {
                throw;
            }
        }
    }
}
