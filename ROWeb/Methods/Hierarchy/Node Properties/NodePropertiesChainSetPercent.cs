using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public class NodePropertiesChainSetPercent : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======
        private ChainSetPercentList _chainSetPercentList = null;
        RONodePropertiesChainSetPercent _nodeProperties = null;
        private int _attributeKey = Include.Undefined;
        private int _begin_CDR_RID = Include.Undefined;
        private DateRangeProfile _beginningDateRangeProfile = null;
        private ProfileList _CSPweeks;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesChainSetPercent(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.ChainSetPercent)
        {
            
        }

        //===========
        // PROPERTIES
        //===========



        //========
        // METHODS
        //========

        override public bool OnClosing()
        {
            return true;
        }


        override public RONodeProperties NodePropertiesGetData(ROProfileKeyParms parms, object nodePropertiesData, ref string message, bool applyOnly = false)
        {
            _chainSetPercentList = (ChainSetPercentList)nodePropertiesData;

            int attributeKey = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
            int dateKey = Include.Undefined;
            KeyValuePair<int, string> timePeriod = default(KeyValuePair<int, string>);

            if (parms is RONodePropertyAttributeDateKeyParms)
            {
                RONodePropertyAttributeDateKeyParms nodePropertyChainSetPercentParms = (RONodePropertyAttributeDateKeyParms)parms;
                if (nodePropertyChainSetPercentParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = nodePropertyChainSetPercentParms.AttributeKey;
                }
                if (nodePropertyChainSetPercentParms.DateKey != Include.NoRID)
                {
                    dateKey = nodePropertyChainSetPercentParms.DateKey;
                    _hierarchyNodeProfile.Begin_CDR_RID = dateKey;
                    _begin_CDR_RID = _hierarchyNodeProfile.Begin_CDR_RID;
                    _beginningDateRangeProfile = SAB.ClientServerSession.Calendar.GetDateRange(_begin_CDR_RID);
                }
            }
            else if (_attributeKey != Include.Undefined)
            {
                attributeKey = _attributeKey;
            }


            if (_beginningDateRangeProfile != null)
            {
                timePeriod = new KeyValuePair<int, string>(_beginningDateRangeProfile.Key, _beginningDateRangeProfile.DisplayDate);
            }

            _attributeKey = attributeKey;
            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            _nodeProperties = new RONodePropertiesChainSetPercent(node: node,
                attribute: GetName.GetAttributeName(key: attributeKey),
                timePeriod: timePeriod
                );

            // populate modelProperties using Windows\NodeProperties.cs as a reference

            AddAttributeSets(nodeProperties: _nodeProperties,
                ChainSetPercentList: _chainSetPercentList);

            return _nodeProperties;
        }

        private void AddAttributeSets(RONodePropertiesChainSetPercent nodeProperties, ChainSetPercentList ChainSetPercentList)
        {
            RONodePropertiesChainSetPercentWeek chainSetPercentWeek;
            HierarchyNodeProfile hnp = null;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);
            bool updated = false;
            bool isInherited = false;
            int inheritedRID = Include.NoRID;
            string storeGroupID = null;
            decimal? percentage = null;
            int yearWeekKey;
            int yearWeek;
            int seqId = 0;
            string strId = null;
            int strRID = 0;
            string stringWeek;
            Dictionary<int, SortedList> CSPGroup = new Dictionary<int, SortedList>();
            SortedList weekKeySL;
            SortedList pctKeySL;

            ProfileList storeChainSetPercentGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(_attributeKey, true);

            foreach (StoreGroupLevelListViewProfile sglp in storeChainSetPercentGroupLevelList)
            {
                nodeProperties.AttributeSets.Add(new RONodePropertiesChainSetPercentAttributeSet(attributeSet: new KeyValuePair<int, string>(sglp.Key, sglp.Name)));

                if (_CSPweeks != null)
                {
                    weekKeySL = new SortedList();
                    strId = sglp.Name;
                    strRID = sglp.Key;
                    seqId = sglp.Sequence;

                    foreach (WeekProfile wp in _CSPweeks)
                    {
                        stringWeek = Convert.ToString(wp);
                        pctKeySL = new SortedList();
                        yearWeek = Convert.ToInt32(Convert.ToString(wp.YearWeek));
                        yearWeekKey = Convert.ToInt32(Convert.ToString(wp.YearWeek - 200000) + Convert.ToString(sglp.Key));
                        ChainSetPercentProfiles yearWeekObj = new ChainSetPercentProfiles(yearWeekKey);
                        if (!_chainSetPercentList.ArrayList.Contains(yearWeekObj))
                        {
                            isInherited = false;
                            inheritedRID = Include.NoRID;
                            percentage = null;
                        }
                        else
                        {
                            foreach (ChainSetPercentProfiles cspp in _chainSetPercentList)
                            {
                                if (cspp.TimeID == wp.YearWeek)
                                {
                                    if (cspp.StoreGroupLevelRID == sglp.Key)
                                    {
                                        storeGroupID = cspp.StoreGroupID;
                                        isInherited = cspp.ChainSetPercentIsInherited;
                                        inheritedRID = cspp.ChainSetPercentInheritedFromNodeRID;
                                        strRID = cspp.StoreGroupLevelRID;
                                        strId = cspp.StoreGroupLevelID;
                                        yearWeek = cspp.TimeID;
                                        if (cspp.NodeRID > Include.Undefined)
                                        {
                                            percentage = cspp.ChainSetPercent;

                                            if (cspp.ChainSetPercentChangeType != eChangeType.none)
                                            {
                                                updated = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        ChainSetPercentValues pctValueList = new ChainSetPercentValues(stringWeek, storeGroupID, strRID, strId, percentage, isInherited, inheritedRID, yearWeek, updated, seqId);

                        if (!weekKeySL.ContainsKey(wp.YearWeek))
                            weekKeySL.Add(wp.YearWeek, pctValueList);

                    }
                    if (!CSPGroup.ContainsKey(sglp.Key))
                        CSPGroup.Add(sglp.Key, weekKeySL);

                }
            }

            nodeProperties.DefinePercentages(numberOfRows: nodeProperties.AttributeSets.Count, numberOfColumns: _CSPweeks.Count);

            // add weeks 
            int week = 0;
            bool weekIsInherited = true;

            foreach (WeekProfile wp in _CSPweeks)
            {
                weekIsInherited = true;
                hnp = null;
                chainSetPercentWeek = new RONodePropertiesChainSetPercentWeek(new KeyValuePair<int, string>(wp.Key, wp.ToString()));
                int attributeSet = 0;
                foreach (StoreGroupLevelListViewProfile sglp in storeChainSetPercentGroupLevelList)
                {
                    if (CSPGroup.TryGetValue(sglp.Key, out weekKeySL))
                    {
                        ChainSetPercentValues pct = (ChainSetPercentValues)weekKeySL[wp.YearWeek];
                        if (pct != null)
                        {
                            nodeProperties.Percentages[attributeSet][week] = pct.Percentage;
                            if (nodeProperties.Totals[week] == null)
                            {
                                nodeProperties.Totals[week] = 0;
                            }
                            nodeProperties.Totals[week] += pct.Percentage;
                            if (!pct.ChainSetPercentIsInherited)
                            {
                                weekIsInherited = false;
                            }
                            else if (hnp == null)
                            {
                                hnp = GetHierarchyNodeProfile(pct.ChainSetPercentInheritedFromNodeRID);
                            }
                        }
                    }

                    ++attributeSet;
                }

                if (weekIsInherited
                    && hnp != null)
                {
                    chainSetPercentWeek.ChainSetPercentWeekInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                }
                nodeProperties.Weeks.Add(chainSetPercentWeek);
                ++week;
            }

        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            RONodePropertiesChainSetPercent nodePropertiesChainSetPercentData = (RONodePropertiesChainSetPercent)nodePropertiesData;

            if (_chainSetPercentList == null)
            {
                _chainSetPercentList = GetChainSetPercent(key: nodePropertiesChainSetPercentData.Node.Key);
            }

            if (nodePropertiesChainSetPercentData.Attribute.Key != _attributeKey
                && _chainSetPercentList.Count > 0)
            {
                message = "Cannot change Attribute when percentages are defined.";
                nodePropertiesChainSetPercentData.Attribute = GetName.GetAttributeName(key: _attributeKey);
                successful = false;
            }
            else if (_hierarchyNodeProfile.Begin_CDR_RID != Include.UndefinedCalendarDateRange
                && nodePropertiesChainSetPercentData.TimePeriod.Key != _hierarchyNodeProfile.Begin_CDR_RID)
            {
                _attributeKey = nodePropertiesChainSetPercentData.Attribute.Key;
                _hierarchyNodeProfile.Begin_CDR_RID = nodePropertiesChainSetPercentData.TimePeriod.Key;
                ChainSetPercent_UpdateUserSettings();
                _chainSetPercentList = GetChainSetPercent(key: _hierarchyNodeProfile.Key,
                    attributeKey: _attributeKey,
                    dateKey: _hierarchyNodeProfile.Begin_CDR_RID
                    );
            }
            else if (SetChainSetPercent(nodePropertiesChainSetPercentData: nodePropertiesChainSetPercentData, message: ref message))
            {
                if (!applyOnly)
                {
                    if (!UpdateChainSetPercent(nodeKey: nodePropertiesChainSetPercentData.Node.Key, message: ref message))
                    {
                        successful = false;
                    }
                }
            }
            else
            {
                successful = false;
            }


            return _chainSetPercentList;
        }

        private bool SetChainSetPercent(RONodePropertiesChainSetPercent nodePropertiesChainSetPercentData, ref string message)
        {
            int weekIndex = 0;
            int setIndex = 0;

            // spread values
            foreach (RONodePropertiesChainSetPercentWeek week in nodePropertiesChainSetPercentData.Weeks)
            {
                nodePropertiesChainSetPercentData.Totals[weekIndex] = 0;

                setIndex = 0;
                foreach (RONodePropertiesChainSetPercentAttributeSet attributeSet in nodePropertiesChainSetPercentData.AttributeSets)
                {
                    if (nodePropertiesChainSetPercentData.Percentages[setIndex][weekIndex] != null)
                    {
                        nodePropertiesChainSetPercentData.Totals[weekIndex] += Convert.ToDecimal(nodePropertiesChainSetPercentData.Percentages[setIndex][weekIndex]);
                    }
                    ++setIndex;
                }
                if (nodePropertiesChainSetPercentData.Totals[weekIndex] > 0
                    && nodePropertiesChainSetPercentData.Totals[weekIndex] != 100)
                {
                    nodePropertiesChainSetPercentData.Weeks[weekIndex].ChainSetPercentWeekInheritedFromNode = default(KeyValuePair<int, string>);
                    SpreadChainSetPercentages(nodePropertiesChainSetPercentData: nodePropertiesChainSetPercentData, weekIndex: weekIndex);
                }
                ++weekIndex;
            }

            // update data
            weekIndex = 0;
            foreach (RONodePropertiesChainSetPercentWeek week in nodePropertiesChainSetPercentData.Weeks)
            {
                WeekProfile wp = (WeekProfile)_CSPweeks[weekIndex];
                setIndex = 0;
                foreach (RONodePropertiesChainSetPercentAttributeSet attributeSet in nodePropertiesChainSetPercentData.AttributeSets)
                {
                    int yearWeek = wp.YearWeek;
                    int StoreGroupRID = attributeSet.AttributeSet.Key;
                    int yearWeekKey = Convert.ToInt32((Convert.ToString(yearWeek - 200000) + Convert.ToString(StoreGroupRID)));

                    ChainSetPercentProfiles cspp = (ChainSetPercentProfiles)_chainSetPercentList.FindKey(yearWeekKey);

                    if (cspp == null)
                    {
                        cspp = new ChainSetPercentProfiles(yearWeekKey);

                        cspp.TimeID = yearWeek;
                        cspp.NodeRID = _hierarchyNodeProfile.Key;
                        cspp.NodeID = _hierarchyNodeProfile.NodeID;

                        cspp.StoreGroupID = nodePropertiesChainSetPercentData.Attribute.Value;
                        cspp.StoreGroupRID = nodePropertiesChainSetPercentData.Attribute.Key;
                        cspp.StoreGroupVersion = StoreMgmt.StoreGroup_GetVersion(cspp.StoreGroupRID);
                        cspp.StoreGroupLevelID = attributeSet.AttributeSet.Value;
                        cspp.StoreGroupLevelRID = StoreGroupRID;

                        cspp.NewRecord = true;
                    }

                    if (nodePropertiesChainSetPercentData.Percentages[setIndex][weekIndex] == null
                        || nodePropertiesChainSetPercentData.Percentages[setIndex][weekIndex] == 0)
                    {
                        cspp.ChainSetPercentChangeType = eChangeType.delete;
                        cspp.ChainSetPercent = 0;
                    }
                    else  if (nodePropertiesChainSetPercentData.Percentages[setIndex][weekIndex] != cspp.ChainSetPercent)
                    {
                        cspp.ChainSetPercent = Convert.ToDecimal(nodePropertiesChainSetPercentData.Percentages[setIndex][weekIndex]);
                        cspp.ChainSetPercentChangeType = eChangeType.update;
                        cspp.ChainSetPercentIsInherited = false;
                        cspp.ChainSetPercentInheritedFromNodeRID = Include.NoRID;
                    }

                    if (_chainSetPercentList.Contains(cspp.Key))
                    {
                        _chainSetPercentList.Update(cspp);
                    }
                    else if (cspp.ChainSetPercentChangeType != eChangeType.delete)
                    {
                        cspp.NewRecord = true;
                        cspp.ChainSetPercentChangeType = eChangeType.add;
                        _chainSetPercentList.Add(cspp);
                    }

                    ++setIndex;
                }
                ++weekIndex;
            }

            return true;
        }

        /// <summary>
        /// Spread Chain Set Percent values
        /// </summary>
        /// <param name="nodePropertiesChainSetPercentData">The Chain Set Percent to be updated</param>
        /// <returns>A flag identifying if successful</returns>
        private bool SpreadChainSetPercentages(RONodePropertiesChainSetPercent nodePropertiesChainSetPercentData, int weekIndex)
        {
            BasicSpread spreader = new BasicSpread();
            ArrayList inValues = new ArrayList();
            ArrayList outValues;
            int setIndex = 0;

            foreach (RONodePropertiesChainSetPercentAttributeSet attributeSet in nodePropertiesChainSetPercentData.AttributeSets)
            {
                if (nodePropertiesChainSetPercentData.Percentages[setIndex][weekIndex] != null)
                {
                    inValues.Add(Convert.ToDouble(nodePropertiesChainSetPercentData.Percentages[setIndex][weekIndex]));
                }
                else
                {
                    inValues.Add(0);
                }
                ++setIndex;
            }

            spreader.ExecuteSimpleSpread(100, inValues, 3, out outValues);

            setIndex = 0;
            foreach (RONodePropertiesChainSetPercentAttributeSet attributeSet in nodePropertiesChainSetPercentData.AttributeSets)
            {
                nodePropertiesChainSetPercentData.Percentages[setIndex][weekIndex] = Convert.ToDecimal(outValues[setIndex]);
                ++setIndex;
            }

            return true;
        }

        private void ChainSetPercent_UpdateUserSettings()
        {
            MerchandiseHierarchyData merchandiseData = null;
            try
            {
                if (_hierarchyNodeProfile.Begin_CDR_RID <= 1 ||
                    _attributeKey <= 1)

                {
                    return;
                }

                merchandiseData = new MerchandiseHierarchyData();
                merchandiseData.OpenUpdateConnection();
                merchandiseData.ChainSetPercentUser_Update(SAB.ClientServerSession.UserRID, _hierarchyNodeProfile.Begin_CDR_RID, _attributeKey);
                merchandiseData.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (merchandiseData != null)
                {
                    merchandiseData.CloseUpdateConnection();

                }
            }
        }

        private bool UpdateChainSetPercent(int nodeKey, ref string message)
        {
            SAB.HierarchyServerSession.ChainSetPercentUpdate(nodeKey, _chainSetPercentList, _beginningDateRangeProfile.Key, false);
            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            if (_chainSetPercentList == null)
            {
                _chainSetPercentList = GetChainSetPercent(key: key);
            }

            foreach (ChainSetPercentProfiles cspp in _chainSetPercentList)
            {
                cspp.ChainSetPercentChangeType = eChangeType.delete;
            }

            SAB.HierarchyServerSession.ChainSetPercentUpdate(key, _chainSetPercentList, _beginningDateRangeProfile.Key, false);

            // read to get inheritance
            _chainSetPercentList = GetChainSetPercent(key: key);

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deleteChainSetPercentages: true);
                }
                else
                {
                    message = MIDText.GetText(eMIDTextCode.lbl_ACLL_LockAttemptFailed);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            finally
            {
                SAB.HierarchyServerSession.DequeueBranch(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key);
            }

            return true;
        }

        override public object NodePropertiesGetValues(ROProfileKeyParms parms)
        {
            int attributeKey = Include.Undefined;
            int dateKey = Include.Undefined;

            if (parms is RONodePropertyAttributeDateKeyParms)
            {
                RONodePropertyAttributeDateKeyParms dateKeyParms = (RONodePropertyAttributeDateKeyParms)parms;
                if (dateKeyParms.AttributeKey != Include.Undefined)
                {
                    attributeKey = dateKeyParms.AttributeKey;
                }
                if (dateKeyParms.DateKey != Include.Undefined)
                {
                    dateKey = dateKeyParms.DateKey;
                }
            }
            return GetChainSetPercent(key: parms.Key, attributeKey: attributeKey, dateKey: dateKey);
        }

        private ChainSetPercentList GetChainSetPercent(int key, int attributeKey = Include.Undefined, int dateKey = Include.Undefined)
        {
            // initialize with user fields
            if (_attributeKey == Include.Undefined)
            {
                ChainSetPercent_GetUserSettings();
            }

            // override with input fields
            if (attributeKey != Include.Undefined)
            {
                _attributeKey = attributeKey;
            }

            if (dateKey != Include.Undefined)
            {
                _hierarchyNodeProfile.Begin_CDR_RID = dateKey;
                _begin_CDR_RID = _hierarchyNodeProfile.Begin_CDR_RID;
                _beginningDateRangeProfile = SAB.ClientServerSession.Calendar.GetDateRange(_begin_CDR_RID);
            }

            ProfileList storeGroupLevelList = StoreMgmt.StoreGroup_GetLevelListFilled(_attributeKey);
            if (_beginningDateRangeProfile != null)
            {
                _CSPweeks = SAB.ApplicationServerSession.Calendar.GetWeekRange(_beginningDateRangeProfile, null);
                return SAB.HierarchyServerSession.GetChainSetPercentList(storeGroupLevelList, _hierarchyNodeProfile.Key, false, false, true, _CSPweeks);
            }
            else
            {
                return new ChainSetPercentList(eProfileType.ChainSetPercent);
            }
        }

        private void ChainSetPercent_GetUserSettings()
        {
            MerchandiseHierarchyData merchandiseData;
            DataTable dt;
            try
            {
                _attributeKey = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
                _hierarchyNodeProfile.Begin_CDR_RID = Include.UndefinedCalendarDateRange;
                _begin_CDR_RID = Include.UndefinedCalendarDateRange;
                _beginningDateRangeProfile = null;

                merchandiseData = new MerchandiseHierarchyData();
                dt = merchandiseData.ChainSetPercentUser_Read(SAB.ClientServerSession.UserRID);
                if (dt.Rows.Count > 0)
                {
                    _attributeKey = Convert.ToInt32(dt.Rows[0]["SG_RID"]);
                    _hierarchyNodeProfile.Begin_CDR_RID = Convert.ToInt32(dt.Rows[0]["CDR_RID"]);
                    _begin_CDR_RID = _hierarchyNodeProfile.Begin_CDR_RID;
                    _beginningDateRangeProfile = SAB.ClientServerSession.Calendar.GetDateRange(_begin_CDR_RID);
                }

            }
            catch 
            {
                throw;
            }
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyChainSetPcts, (int)eSecurityTypes.Allocation | (int)eSecurityTypes.Store);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyChainSetPcts, (int)eSecurityTypes.Allocation);
            }
        }

        override public ROProfileKeyParms NodePropertiesGetParms(RONodePropertiesParms parms, eProfileType profileType, int key, bool readOnly = false)
        {
            int attributeKey = Include.NoRID;
            int dateKey = Include.NoRID;
            if (parms.RONodeProperties is RONodePropertiesChainSetPercent)
            {
                RONodePropertiesChainSetPercent nodePropertiesChainSetPercentData = (RONodePropertiesChainSetPercent)parms.RONodeProperties;
                attributeKey = nodePropertiesChainSetPercentData.Attribute.Key;
                dateKey = nodePropertiesChainSetPercentData.TimePeriod.Key;
            }

            RONodePropertyAttributeDateKeyParms profileKeyParms = new RONodePropertyAttributeDateKeyParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.GetModel,
                ROInstanceID: parms.ROInstanceID,
                profileType: profileType,
                key: key,
                readOnly: readOnly,
                attributeKey: attributeKey,
                dateKey: dateKey
                );

            return profileKeyParms;
        }
    }
}
