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
    public class NodePropertiesDailyPercentages : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======
        StoreDailyPercentagesList _storeDailyPercentagesList = null;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesDailyPercentages(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.DailyPercentages)
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
            _storeDailyPercentagesList = (StoreDailyPercentagesList)nodePropertiesData;

            int attributeKey = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
            if (parms is RONodePropertyAttributeKeyParms)
            {
                RONodePropertyAttributeKeyParms nodePropertyDailyPercentagesParms = (RONodePropertyAttributeKeyParms)parms;
                if (nodePropertyDailyPercentagesParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = nodePropertyDailyPercentagesParms.AttributeKey;
                }
            }

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesDailyPercentages nodeProperties = new RONodePropertiesDailyPercentages(node: node,
                attribute: GetName.GetAttributeName(key: attributeKey)
                );

            // populate nodeProperties using Windows\NodeProperties.cs as a reference

            AddAttributeSets(nodeProperties: nodeProperties,
                dailyPercentagesList: _storeDailyPercentagesList);


            return nodeProperties;
        }

        private void AddAttributeSets(RONodePropertiesDailyPercentages nodeProperties, StoreDailyPercentagesList dailyPercentagesList)
        {
            RONodePropertiesDailyPercentagesAttributeSet dailyPercentagesAttributeSet;
            RONodePropertiesDailyPercentagesStore dailyPercentagesStore;
            RONodePropertiesDailyPercentagesValues dailyPercentagesValues;
            StoreDailyPercentagesProfile storeDailyPercentagesProfile;
            HierarchyNodeProfile hnp;
            string inheritedFromText = MIDText.GetTextOnly(eMIDTextCode.lbl_Inherited_From);

            ProfileList storeDailyPercentagesGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(nodeProperties.Attribute.Key, true);

            foreach (StoreGroupLevelListViewProfile sglp in storeDailyPercentagesGroupLevelList)
            {
                dailyPercentagesAttributeSet = new RONodePropertiesDailyPercentagesAttributeSet(attributeSet: new KeyValuePair<int, string>(sglp.Key, sglp.Name));
                foreach (StoreProfile storeProfile in sglp.Stores)
                {
                    dailyPercentagesStore = new RONodePropertiesDailyPercentagesStore(store: new KeyValuePair<int, string>(storeProfile.Key, storeProfile.Text));

                    if (_storeDailyPercentagesList.Contains(storeProfile.Key))
                    {
                        storeDailyPercentagesProfile = (StoreDailyPercentagesProfile)_storeDailyPercentagesList.FindKey(storeProfile.Key);

                        if (storeDailyPercentagesProfile.StoreDailyPercentagesInheritedFromNodeRID != nodeProperties.Node.Key
                            && storeDailyPercentagesProfile.StoreDailyPercentagesIsInherited
                            && storeDailyPercentagesProfile.StoreDailyPercentagesInheritedFromNodeRID != Include.NoRID)
                        {
                            hnp = GetHierarchyNodeProfile(storeDailyPercentagesProfile.StoreDailyPercentagesInheritedFromNodeRID);
                            dailyPercentagesStore.DailyPercentagesInheritedFromNode = new KeyValuePair<int, string>(hnp.Key, inheritedFromText + hnp.Text);
                        }

                        if (storeDailyPercentagesProfile.HasDefaultValues)
                        {
                            dailyPercentagesStore.DefaultDailyPercentagesValues.Total = storeDailyPercentagesProfile.Day1Default + storeDailyPercentagesProfile.Day2Default + storeDailyPercentagesProfile.Day3Default + storeDailyPercentagesProfile.Day4Default + storeDailyPercentagesProfile.Day5Default
                                + storeDailyPercentagesProfile.Day6Default + storeDailyPercentagesProfile.Day7Default;

                            if (storeDailyPercentagesProfile.Day1Default > 0)
                            {
                                dailyPercentagesStore.DefaultDailyPercentagesValues.Day[0] = RoundValue(value: storeDailyPercentagesProfile.Day1Default);
                            }
                            if (storeDailyPercentagesProfile.Day2Default > 0)
                            {
                                dailyPercentagesStore.DefaultDailyPercentagesValues.Day[1] = RoundValue(value: storeDailyPercentagesProfile.Day2Default);
                            }
                            if (storeDailyPercentagesProfile.Day3Default > 0)
                            {
                                dailyPercentagesStore.DefaultDailyPercentagesValues.Day[2] = RoundValue(value: storeDailyPercentagesProfile.Day3Default);
                            }
                            if (storeDailyPercentagesProfile.Day4Default > 0)
                            {
                                dailyPercentagesStore.DefaultDailyPercentagesValues.Day[3] = RoundValue(value: storeDailyPercentagesProfile.Day4Default);
                            }
                            if (storeDailyPercentagesProfile.Day5Default > 0)
                            {
                                dailyPercentagesStore.DefaultDailyPercentagesValues.Day[4] = RoundValue(value: storeDailyPercentagesProfile.Day5Default);
                            }
                            if (storeDailyPercentagesProfile.Day6Default > 0)
                            {
                                dailyPercentagesStore.DefaultDailyPercentagesValues.Day[5] = RoundValue(value: storeDailyPercentagesProfile.Day6Default);
                            }
                            if (storeDailyPercentagesProfile.Day7Default > 0)
                            {
                                dailyPercentagesStore.DefaultDailyPercentagesValues.Day[6] = RoundValue(value: storeDailyPercentagesProfile.Day7Default);
                            }
                        }
                        
                        foreach (DailyPercentagesProfile dpp in storeDailyPercentagesProfile.DailyPercentagesList)
                        {
                            dailyPercentagesValues = new RONodePropertiesDailyPercentagesValues(dateRange: new KeyValuePair<int, string>(dpp.DateRange.Key, dpp.DateRange.DisplayDate));

                            dailyPercentagesValues.Total = dpp.Day1 + dpp.Day2 + dpp.Day3 + dpp.Day4 + dpp.Day5 + dpp.Day6 + dpp.Day7;

                            if (dpp.Day1 > 0)
                            {
                                dailyPercentagesValues.Day[0] = RoundValue(value: dpp.Day1);
                            }
                            if (dpp.Day2 > 0)
                            {
                                dailyPercentagesValues.Day[1] = RoundValue(value: dpp.Day2);
                            }
                            if (dpp.Day3 > 0)
                            {
                                dailyPercentagesValues.Day[2] = RoundValue(value: dpp.Day3);
                            }
                            if (dpp.Day4 > 0)
                            {
                                dailyPercentagesValues.Day[3] = RoundValue(value: dpp.Day4);
                            }
                            if (dpp.Day5 > 0)
                            {
                                dailyPercentagesValues.Day[4] = RoundValue(value: dpp.Day5);
                            }
                            if (dpp.Day6 > 0)
                            {
                                dailyPercentagesValues.Day[5] = RoundValue(value: dpp.Day6);
                            }
                            if (dpp.Day7 > 0)
                            {
                                dailyPercentagesValues.Day[6] = RoundValue(value: dpp.Day7);
                            }

                            dailyPercentagesStore.DailyPercentagesValues.Add(dailyPercentagesValues);
                         }
                    }

                    dailyPercentagesAttributeSet.Store.Add(dailyPercentagesStore);
                }

                nodeProperties.AttributeSet.Add(dailyPercentagesAttributeSet);
            }
        }

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;
            RONodePropertiesDailyPercentages nodePropertiesDailyPercentagesData = (RONodePropertiesDailyPercentages)nodePropertiesData;

            if (_storeDailyPercentagesList == null)
            {
                _storeDailyPercentagesList = GetDailyPercentages(key: nodePropertiesDailyPercentagesData.Node.Key);
            }

            if (SetDailyPercentages(nodePropertiesDailyPercentagesData: nodePropertiesDailyPercentagesData, message: ref message))
            {
                if (!applyOnly)
                {
                    if (!UpdateDailyPercentages(nodeKey: nodePropertiesDailyPercentagesData.Node.Key, message: ref message))
                    {
                        successful = false;
                    }
                }
            }
            else
            {
                successful = false;
            }


            return _storeDailyPercentagesList;
        }

        private bool SetDailyPercentages(RONodePropertiesDailyPercentages nodePropertiesDailyPercentagesData, ref string message)
        {
            StoreDailyPercentagesProfile storeDailyPercentagesProfile;
            bool profileFound;
            bool storeUpdated;

            // spread values
            foreach (RONodePropertiesDailyPercentagesAttributeSet dailyPercentagesAttributeSet in nodePropertiesDailyPercentagesData.AttributeSet)
            {
                // balance set default
                if (dailyPercentagesAttributeSet.DefaultDailyPercentagesValues.DailyPercentagesEntered)
                {
                    BalanceDays(percentageValues: dailyPercentagesAttributeSet.DefaultDailyPercentagesValues);
                }
                // balance set date ranges
                foreach (RONodePropertiesDailyPercentagesValues setDateRange in dailyPercentagesAttributeSet.DailyPercentagesValues)
                {
                    if (setDateRange.DailyPercentagesEntered)
                    {
                        BalanceDays(percentageValues: setDateRange);
                    }
                }

                // balance stores
                foreach (RONodePropertiesDailyPercentagesStore dailyPercentagesStore in dailyPercentagesAttributeSet.Store)
                {
                    // balance store default
                    if (dailyPercentagesStore.DefaultDailyPercentagesValues.DailyPercentagesEntered)
                    {
                        if (PercentagesChanged(storeKey: dailyPercentagesStore.Store.Key, percentageValues: dailyPercentagesStore.DefaultDailyPercentagesValues))
                        {
                            BalanceDays(percentageValues: dailyPercentagesStore.DefaultDailyPercentagesValues);
                        }
                    }
                    // balance store date ranges
                    foreach (RONodePropertiesDailyPercentagesValues storeDateRange in dailyPercentagesStore.DailyPercentagesValues)
                    {
                        if (storeDateRange.DailyPercentagesEntered)
                        {
                            if (PercentagesChanged(storeKey: dailyPercentagesStore.Store.Key, percentageValues: storeDateRange))
                            {
                                BalanceDays(percentageValues: storeDateRange);
                            }
                        }
                    }
                }
            }

            // apply attribute set values
            bool dateRangeFound = false;
            DateRangeProfile applySetDateRange;
            ProfileList setWeekProfileList;
            DateRangeProfile applyStoreDateRange = null;
            RONodePropertiesDailyPercentagesValues applyStoreDateRangeValues = null;

            foreach (RONodePropertiesDailyPercentagesAttributeSet dailyPercentagesAttributeSet in nodePropertiesDailyPercentagesData.AttributeSet)
            {
                // apply set defaults
                if (dailyPercentagesAttributeSet.DefaultDailyPercentagesValues.DailyPercentagesEntered)
                {
                    foreach (RONodePropertiesDailyPercentagesStore dailyPercentagesStore in dailyPercentagesAttributeSet.Store)
                    {
                        for (int dayIndex = 0; dayIndex < dailyPercentagesAttributeSet.DefaultDailyPercentagesValues.Day.Length; ++dayIndex)
                        {
                            dailyPercentagesStore.DefaultDailyPercentagesValues.Day[dayIndex] = dailyPercentagesAttributeSet.DefaultDailyPercentagesValues.Day[dayIndex];
                        }
                    }
                }
                // apply set date ranges
                foreach (RONodePropertiesDailyPercentagesValues setDateRange in dailyPercentagesAttributeSet.DailyPercentagesValues)
                {
                    if (setDateRange.DailyPercentagesEntered)
                    {
                        applySetDateRange = GetDateRangeProfile(key: setDateRange.DateRange.Key);
                        setWeekProfileList = SAB.ClientServerSession.Calendar.GetWeekRange(applySetDateRange, null);

                        // apply to stores
                        foreach (RONodePropertiesDailyPercentagesStore dailyPercentagesStore in dailyPercentagesAttributeSet.Store)
                        {
                            // check store date ranges
                            dateRangeFound = false;
                            foreach (RONodePropertiesDailyPercentagesValues storeDateRange in dailyPercentagesStore.DailyPercentagesValues)
                            {
                                if (storeDateRange.DailyPercentagesEntered)
                                {
                                    applyStoreDateRange = GetDateRangeProfile(key: storeDateRange.DateRange.Key);
                                    if (applyStoreDateRange.Key == applySetDateRange.Key)
                                    {
                                        applyStoreDateRangeValues = storeDateRange;
                                        dateRangeFound = true;
                                        break;
                                    }
                                    else
                                    {
                                        ProfileList storeWeekProfileList = SAB.ClientServerSession.Calendar.GetWeekRange(applyStoreDateRange, null);
                                        if (setWeekProfileList.Count == storeWeekProfileList.Count &&
                                            ((WeekProfile)setWeekProfileList[0]).Key == ((WeekProfile)storeWeekProfileList[0]).Key &&
                                            ((WeekProfile)setWeekProfileList[setWeekProfileList.Count - 1]).Key == ((WeekProfile)storeWeekProfileList[storeWeekProfileList.Count - 1]).Key)
                                        {
                                            applyStoreDateRangeValues = storeDateRange;
                                            dateRangeFound = true;
                                        }
                                    }
                                }
                            }
                            if (dateRangeFound
                                && applyStoreDateRangeValues != null)
                            {
                                applyStoreDateRangeValues.Total = Convert.ToDouble(setDateRange.Total);
                                for (int dayIndex = 0; dayIndex < setDateRange.Day.Length; ++dayIndex)
                                {
                                    applyStoreDateRangeValues.Day[dayIndex] = Convert.ToDouble(setDateRange.Day[dayIndex]);
                                }
                            }
                            else
                            {
                                DateRangeProfile drp = GetDateRangeProfile(setDateRange.DateRange.Key);
                                if (drp.Name == null ||
                                    drp.Name.Trim().Length == 0)  // only dup date ranges that DON'T have a name
                                {
                                    drp = SAB.ClientServerSession.Calendar.GetDateRangeClone(setDateRange.DateRange.Key);
                                }
                                applyStoreDateRangeValues = new RONodePropertiesDailyPercentagesValues(dateRange: new KeyValuePair<int, string>(drp.Key, drp.DisplayDate));
                                applyStoreDateRangeValues.Total = Convert.ToDouble(setDateRange.Total);
                                for (int dayIndex = 0; dayIndex < setDateRange.Day.Length; ++dayIndex)
                                {
                                    applyStoreDateRangeValues.Day[dayIndex] = Convert.ToDouble(setDateRange.Day[dayIndex]);
                                }
                                dailyPercentagesStore.DailyPercentagesValues.Add(applyStoreDateRangeValues);
                            }
                        }
                    }
                }
            }

            // update values
            foreach (RONodePropertiesDailyPercentagesAttributeSet dailyPercentagesAttributeSet in nodePropertiesDailyPercentagesData.AttributeSet)
            {
                // update stores
                foreach (RONodePropertiesDailyPercentagesStore dailyPercentagesStore in dailyPercentagesAttributeSet.Store)
                {
                    storeUpdated = false;

                    if (_storeDailyPercentagesList.Contains(dailyPercentagesStore.Store.Key))
                    {
                        storeDailyPercentagesProfile = (StoreDailyPercentagesProfile)_storeDailyPercentagesList.FindKey(dailyPercentagesStore.Store.Key);
                        profileFound = true;
                    }
                    else
                    {
                        storeDailyPercentagesProfile = new StoreDailyPercentagesProfile(dailyPercentagesStore.Store.Key);
                        storeDailyPercentagesProfile.DailyPercentagesList = new DailyPercentagesList(eProfileType.StoreDailyPercentages);
                        storeDailyPercentagesProfile.StoreDailyPercentagesDefaultChangeType = eChangeType.add;
                        storeDailyPercentagesProfile.StoreDailyPercentagesIsInherited = false;
                        storeDailyPercentagesProfile.StoreDailyPercentagesInheritedFromNodeRID = Include.NoRID;
                        profileFound = false;
                    }

                    // update store default
                    if (dailyPercentagesStore.DefaultDailyPercentagesValues.DailyPercentagesEntered)
                    {
                        if (PercentagesChanged(storeKey: dailyPercentagesStore.Store.Key, percentageValues: dailyPercentagesStore.DefaultDailyPercentagesValues))
                        {
                            storeDailyPercentagesProfile.HasDefaultValues = true;

                            storeDailyPercentagesProfile.Day1Default = UpdateDay(
                                currentValue: storeDailyPercentagesProfile.Day1Default,
                                newValue: dailyPercentagesStore.DefaultDailyPercentagesValues.Day[0],
                                valueUpdated: ref storeUpdated);
                            storeDailyPercentagesProfile.Day2Default = UpdateDay(
                                currentValue: storeDailyPercentagesProfile.Day2Default,
                                newValue: dailyPercentagesStore.DefaultDailyPercentagesValues.Day[1],
                                valueUpdated: ref storeUpdated);
                            storeDailyPercentagesProfile.Day3Default = UpdateDay(
                                currentValue: storeDailyPercentagesProfile.Day3Default,
                                newValue: dailyPercentagesStore.DefaultDailyPercentagesValues.Day[2],
                                valueUpdated: ref storeUpdated);
                            storeDailyPercentagesProfile.Day4Default = UpdateDay(
                                currentValue: storeDailyPercentagesProfile.Day4Default,
                                newValue: dailyPercentagesStore.DefaultDailyPercentagesValues.Day[3],
                                valueUpdated: ref storeUpdated);
                            storeDailyPercentagesProfile.Day5Default = UpdateDay(
                                currentValue: storeDailyPercentagesProfile.Day5Default,
                                newValue: dailyPercentagesStore.DefaultDailyPercentagesValues.Day[4],
                                valueUpdated: ref storeUpdated);
                            storeDailyPercentagesProfile.Day6Default = UpdateDay(
                                currentValue: storeDailyPercentagesProfile.Day6Default,
                                newValue: dailyPercentagesStore.DefaultDailyPercentagesValues.Day[5],
                                valueUpdated: ref storeUpdated);
                            storeDailyPercentagesProfile.Day7Default = UpdateDay(
                                currentValue: storeDailyPercentagesProfile.Day7Default,
                                newValue: dailyPercentagesStore.DefaultDailyPercentagesValues.Day[6],
                                valueUpdated: ref storeUpdated);
                        }
                    }
                    else
                    {
                        storeDailyPercentagesProfile.HasDefaultValues = false;
                        if (storeDailyPercentagesProfile.StoreDailyPercentagesDefaultChangeType == eChangeType.update)
                        {
                            storeDailyPercentagesProfile.StoreDailyPercentagesDefaultChangeType = eChangeType.delete;
                        }
                        else if (storeDailyPercentagesProfile.StoreDailyPercentagesDefaultChangeType == eChangeType.add)
                        {
                            storeDailyPercentagesProfile.StoreDailyPercentagesDefaultChangeType = eChangeType.none;
                        }
                    }

                    if (!profileFound
                        && storeUpdated)
                    {
                        storeDailyPercentagesProfile.StoreDailyPercentagesDefaultChangeType = eChangeType.update;
                    }
                    else if (dailyPercentagesStore.DailyPercentagesIsInherited
                        && storeUpdated)
                    {
                        storeDailyPercentagesProfile.StoreDailyPercentagesDefaultChangeType = eChangeType.add;
                        storeDailyPercentagesProfile.StoreDailyPercentagesIsInherited = false;
                        storeDailyPercentagesProfile.StoreDailyPercentagesInheritedFromNodeRID = Include.NoRID;
                    }

                    // update store date ranges 
                    storeUpdated = false;
                    foreach (RONodePropertiesDailyPercentagesValues storeDateRange in dailyPercentagesStore.DailyPercentagesValues)
                    {
                        if (storeDateRange.DailyPercentagesEntered)
                        {
                            DailyPercentagesProfile dpp = null;
                            bool newDateRange = false;
                            dpp = (DailyPercentagesProfile)storeDailyPercentagesProfile.DailyPercentagesList.FindKey(storeDateRange.DateRange.Key);
                            if (dpp == null)
                            {
                                dpp = new DailyPercentagesProfile(storeDateRange.DateRange.Key);
                                dpp.DailyPercentagesChangeType = eChangeType.add;
                                newDateRange = true;
                                dpp.DateRange = GetDateRangeProfile(key: storeDateRange.DateRange.Key);
                            }
                            else if (dailyPercentagesStore.DailyPercentagesIsInherited)
                            {
                                dpp.DailyPercentagesChangeType = eChangeType.add;
                            }

                            if (PercentagesChanged(storeKey: dailyPercentagesStore.Store.Key, percentageValues: storeDateRange))
                            {
                                dpp.Day1 = UpdateDay(
                                    currentValue: dpp.Day1,
                                    newValue: storeDateRange.Day[0],
                                    valueUpdated: ref storeUpdated);
                                dpp.Day2 = UpdateDay(
                                    currentValue: dpp.Day2,
                                    newValue: storeDateRange.Day[1],
                                    valueUpdated: ref storeUpdated);
                                dpp.Day3 = UpdateDay(
                                    currentValue: dpp.Day3,
                                    newValue: storeDateRange.Day[2],
                                    valueUpdated: ref storeUpdated);
                                dpp.Day4 = UpdateDay(
                                    currentValue: dpp.Day4,
                                    newValue: storeDateRange.Day[3],
                                    valueUpdated: ref storeUpdated);
                                dpp.Day5 = UpdateDay(
                                    currentValue: dpp.Day5,
                                    newValue: storeDateRange.Day[4],
                                    valueUpdated: ref storeUpdated);
                                dpp.Day6 = UpdateDay(
                                    currentValue: dpp.Day6,
                                    newValue: storeDateRange.Day[5],
                                    valueUpdated: ref storeUpdated);
                                dpp.Day7 = UpdateDay(
                                    currentValue: dpp.Day7,
                                    newValue: storeDateRange.Day[6],
                                    valueUpdated: ref storeUpdated);

                                if (newDateRange)
                                {
                                    storeDailyPercentagesProfile.DailyPercentagesList.Add(dpp);
                                }
                                else if (storeUpdated)
                                {
                                    dpp.DailyPercentagesChangeType = eChangeType.update;
                                }
                            }
                        }
                    }

                    foreach (DailyPercentagesProfile dpp in storeDailyPercentagesProfile.DailyPercentagesList)
                    {
                        if (dpp.DailyPercentagesChangeType == eChangeType.none)
                        {
                            dpp.DailyPercentagesChangeType = eChangeType.delete;
                            storeUpdated = true;
                        }
                    }

                    if (!profileFound)
                    {
                        storeDailyPercentagesProfile.StoreDailyPercentagesIsInherited = false;
                        storeDailyPercentagesProfile.StoreDailyPercentagesInheritedFromNodeRID = Include.NoRID;
                        _storeDailyPercentagesList.Add(storeDailyPercentagesProfile);
                    }
                    else if (storeDailyPercentagesProfile.StoreDailyPercentagesDefaultChangeType == eChangeType.update
                        || storeUpdated)
                    {
                        storeDailyPercentagesProfile.StoreDailyPercentagesIsInherited = false;
                        storeDailyPercentagesProfile.StoreDailyPercentagesInheritedFromNodeRID = Include.NoRID;
                        _storeDailyPercentagesList.Update(storeDailyPercentagesProfile);
                    }
                }
            }

            return true;
        }

        private double RoundValue(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private double UpdateDay(double currentValue, double? newValue, ref bool valueUpdated)
        {
            if (newValue != currentValue)
            {
                if (newValue == null)
                {
                    currentValue = 0;
                }
                else
                {
                    currentValue = Convert.ToDouble(newValue);
                }
                valueUpdated = true;
            }

            return currentValue;
        }

        private bool PercentagesChanged(int storeKey, RONodePropertiesDailyPercentagesValues percentageValues)
        {
            StoreDailyPercentagesProfile storeDailyPercentagesProfile;

            if (_storeDailyPercentagesList.Contains(storeKey))
            {
                storeDailyPercentagesProfile = (StoreDailyPercentagesProfile)_storeDailyPercentagesList.FindKey(storeKey);
                if (percentageValues.DateRangeIsDefault)
                {
                    if (percentageValues.Day[0] != RoundValue(value: storeDailyPercentagesProfile.Day1Default)
                        || percentageValues.Day[1] != RoundValue(value: storeDailyPercentagesProfile.Day2Default)
                        || percentageValues.Day[2] != RoundValue(value: storeDailyPercentagesProfile.Day3Default)
                        || percentageValues.Day[3] != RoundValue(value: storeDailyPercentagesProfile.Day4Default)
                        || percentageValues.Day[4] != RoundValue(value: storeDailyPercentagesProfile.Day5Default)
                        || percentageValues.Day[5] != RoundValue(value: storeDailyPercentagesProfile.Day6Default)
                        || percentageValues.Day[6] != RoundValue(value: storeDailyPercentagesProfile.Day7Default)
                        )
                    {
                        return true;
                    }
                }
                else
                {
                    DailyPercentagesProfile dpp = null;
                    dpp = (DailyPercentagesProfile)storeDailyPercentagesProfile.DailyPercentagesList.FindKey(percentageValues.DateRange.Key);
                    if (dpp == null)
                    {
                        return true;
                    }
                    else if (percentageValues.Day[0] != RoundValue(value: dpp.Day1)
                        || percentageValues.Day[1] != RoundValue(value: dpp.Day2)
                        || percentageValues.Day[2] != RoundValue(value: dpp.Day3)
                        || percentageValues.Day[3] != RoundValue(value: dpp.Day4)
                        || percentageValues.Day[4] != RoundValue(value: dpp.Day5)
                        || percentageValues.Day[5] != RoundValue(value: dpp.Day6)
                        || percentageValues.Day[6] != RoundValue(value: dpp.Day7)
                        )
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private bool BalanceDays(RONodePropertiesDailyPercentagesValues percentageValues)
        {
            double?[] day;
            decimal total = 0;

            if (percentageValues.DailyPercentagesEntered)
            {
                percentageValues.Total = 0;
                day = new double?[7];
                for (int dayIndex = 0; dayIndex < percentageValues.Day.Length; ++dayIndex)
                {
                    if (percentageValues.Day[dayIndex] != null)
                    {
                        day[dayIndex] = percentageValues.Day[dayIndex];
                        total += Convert.ToDecimal(percentageValues.Day[dayIndex]);
                    }
                }
                percentageValues.Total = Convert.ToDouble(total);


                if (percentageValues.Total > 0
                    && percentageValues.Total != 100)
                {
                    percentageValues.Total = 0;
                    total = 0;
                    if (SpreadDailyPercentages(day: ref day))
                    {
                        for (int dayIndex = 0; dayIndex < percentageValues.Day.Length; ++dayIndex)
                        {
                            if (day[dayIndex] > 0)
                            {
                                percentageValues.Day[dayIndex] = day[dayIndex];
                                total += Convert.ToDecimal(day[dayIndex]);
                            }
                        }
                        percentageValues.Total = Convert.ToDouble(total);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Spread Daily Percent values
        /// </summary>
        /// <param name="day">The Daily Percentages to be updated</param>
        /// <returns>A flag identifying if successful</returns>
        private bool SpreadDailyPercentages(ref double?[] day)
        {
            BasicSpread spreader = new BasicSpread();
            ArrayList inValues = new ArrayList();
            ArrayList outValues;

            for (int dayIndex = 0; dayIndex < day.Length; ++dayIndex)
            {
                if (day[dayIndex] != null)
                {
                    inValues.Add(Convert.ToDouble(day[dayIndex]));
                }
                else
                {
                    inValues.Add(0);
                }
            }

            spreader.ExecuteSimpleSpread(100, inValues, 3, out outValues);

            for (int dayIndex = 0; dayIndex < day.Length; ++dayIndex)
            {
                day[dayIndex] = (double?)(outValues[dayIndex]);
            }

            return true;
        }

        private bool UpdateDailyPercentages(int nodeKey, ref string message)
        {
            SAB.HierarchyServerSession.StoreDailyPercentagesUpdate(nodeKey, _storeDailyPercentagesList, false);

            return true;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {
            if (_storeDailyPercentagesList == null)
            {
                _storeDailyPercentagesList = GetDailyPercentages(key: key);
            }

            foreach (StoreDailyPercentagesProfile storeDailyPercentages in _storeDailyPercentagesList)
            {
                storeDailyPercentages.StoreDailyPercentagesDefaultChangeType = eChangeType.delete;
                foreach (DailyPercentagesProfile dailyPercentages in storeDailyPercentages.DailyPercentagesList)
                {
                    dailyPercentages.DailyPercentagesChangeType = eChangeType.delete;
                }
            }

            SAB.HierarchyServerSession.StoreDailyPercentagesUpdate(key, _storeDailyPercentagesList, false);

            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {
            try
            {
                if (SAB.HierarchyServerSession.LockHierarchyBranchForUpdate(_hierarchyNodeProfile.HomeHierarchyRID, _hierarchyNodeProfile.Key, false))
                {
                    EditMsgs em = ApplyToLowerLevels(deleteDailyPercentages: true);
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
            return GetDailyPercentages(key: parms.Key);
        }

        private StoreDailyPercentagesList GetDailyPercentages(int key)
        {
            return SAB.HierarchyServerSession.GetStoreDailyPercentagesList(storeList: StoreMgmt.StoreProfiles_GetActiveStoresList(), nodeRID: key);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyDailyPcts, (int)eSecurityTypes.Allocation);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyDailyPcts, (int)eSecurityTypes.Allocation);
            }
        }

        override public ROProfileKeyParms NodePropertiesGetParms(RONodePropertiesParms parms, eProfileType profileType, int key, bool readOnly = false)
        {
            int attributeKey = Include.NoRID;
            if (parms.RONodeProperties is RONodePropertiesDailyPercentages)
            {
                RONodePropertiesDailyPercentages nodePropertiesDailyPercentagesData = (RONodePropertiesDailyPercentages)parms.RONodeProperties;
                attributeKey = nodePropertiesDailyPercentagesData.Attribute.Key;
            }

            RONodePropertyAttributeKeyParms profileKeyParms = new RONodePropertyAttributeKeyParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.GetModel,
                ROInstanceID: parms.ROInstanceID,
                profileType: profileType,
                key: key,
                readOnly: readOnly,
                attributeKey: attributeKey
                );

            return profileKeyParms;
        }
    }
}
