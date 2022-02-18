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
    public class ModelSizeCurveGroup : ModelBase
    {
        //=======
        // FIELDS
        //=======
        private SizeCurveGroupProfile _sizeCurveGroupProf;
        private ROModelSizeCurveProperties _modelProperties;

        //=============
        // CONSTRUCTORS
        //=============
        public ModelSizeCurveGroup(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.SizeCurve)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeCurves);
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

        override public List<KeyValuePair<int, string>> ModelGetList()
        {
            SizeCurve dlSizeCurve;
            DataTable dtSizeCurveGroups;

            dlSizeCurve = new SizeCurve();

            dtSizeCurveGroups = dlSizeCurve.GetSizeCurveGroups();

            dtSizeCurveGroups = ApplicationUtilities.SortDataTable(dataTable: dtSizeCurveGroups, sColName: "SIZE_CURVE_GROUP_NAME", bAscending: true);

            return ApplicationUtilities.DataTableToKeyValues(dtSizeCurveGroups, "SIZE_CURVE_GROUP_RID", "SIZE_CURVE_GROUP_NAME");
        }

        override public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false)
        {
            // populate modelProperties using Windows\SizeCurveMaint.cs as a reference

            _sizeCurveGroupProf = (SizeCurveGroupProfile)modelProfile;

            int attributeKey = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
            int sizeGroupKey = _sizeCurveGroupProf.DefinedSizeGroupRID;

            if (parms is ROSizeCurveModelParms)
            {
                ROSizeCurveModelParms sizeCurveModelParms = (ROSizeCurveModelParms)parms;
                if (sizeCurveModelParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = sizeCurveModelParms.AttributeKey;
                }
                if (sizeCurveModelParms.SizeGroupKey != Include.NoRID)
                {
                    sizeGroupKey = sizeCurveModelParms.SizeGroupKey;
                }
            }

            KeyValuePair<int, string> model = new KeyValuePair<int, string>(key: _sizeCurveGroupProf.Key, value: _sizeCurveGroupProf.SizeCurveGroupName);
            _modelProperties = new ROModelSizeCurveProperties(model: model,
                attribute: GetName.GetAttributeName(key: attributeKey),
                sizeGroup: GetName.GetSizeGroup(key: sizeGroupKey)
                );

            SortedList primarySizeList = new SortedList();
            SortedList secondarySizeList = new SortedList();
            Dictionary<int, SizeCodeProfile> sizeCodes = new Dictionary<int, SizeCodeProfile>();

            SizeGroupProfile sizeGroupProf = null;

            if (_sizeCurveGroupProf.DefinedSizeGroupRID != Include.NoRID)
            {
                sizeGroupProf = new SizeGroupProfile(_sizeCurveGroupProf.DefinedSizeGroupRID);
                AddSizeCodeToTable(sizeGroupProf.SizeCodeList, ref primarySizeList, ref secondarySizeList, ref sizeCodes);
            }
            else
            {
                _sizeCurveGroupProf.Resequence();

                foreach (SizeCurveProfile szCrvProf in _sizeCurveGroupProf.SizeCurveList)
                {
                    AddSizeCodeToTable(szCrvProf.SizeCodeList, ref primarySizeList, ref secondarySizeList, ref sizeCodes);
                }

                if (_sizeCurveGroupProf.DefaultSizeCurve != null)
                {
                    AddSizeCodeToTable(_sizeCurveGroupProf.DefaultSizeCurve.SizeCodeList, ref primarySizeList, ref secondarySizeList, ref sizeCodes);
                }
            }

            _modelProperties.DefaultLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Default);
            if (_sizeCurveGroupProf.DefaultSizeCurve != null)
            {
                _modelProperties.DefaultSizeCurve = new ROSizeCurve(sizeCurve: new KeyValuePair<int, string>(_sizeCurveGroupProf.DefaultSizeCurveRid, _sizeCurveGroupProf.DefaultSizeCurve.SizeCurveName));
                _modelProperties.DefaultSizeCurve.SizeCurveEntry = BuildSizeCurve(modelProperties: _modelProperties,
                    primarySizeList: primarySizeList,
                    secondarySizeList: secondarySizeList,
                    sizeCodes: sizeCodes, 
                    sizeCodeList: _sizeCurveGroupProf.DefaultSizeCurve.SizeCodeList);
            }

            BuildSizeCurvesForStores(modelProperties: _modelProperties,
                    sizeCurveGroupProf: _sizeCurveGroupProf,
                    primarySizeList: primarySizeList,
                    secondarySizeList: secondarySizeList,
                    sizeCodes: sizeCodes,
                    attributeKey: attributeKey
                    );

            return _modelProperties;
        }

        private void AddSizeCodeToTable(SizeCodeList aSizeCodeList, 
            ref SortedList primarySizeList, 
            ref SortedList secondarySizeList, 
            ref Dictionary<int, SizeCodeProfile> sizeCodes
            )
        {
            try
            {
                foreach (SizeCodeProfile sizeCodeProf in aSizeCodeList)
                {
                    if (!primarySizeList.Contains(sizeCodeProf.PrimarySequence))
                    {
                        primarySizeList.Add(sizeCodeProf.PrimarySequence, sizeCodeProf.SizeCodePrimaryRID);
                        if (!sizeCodes.ContainsKey(sizeCodeProf.Key))
                        {
                            sizeCodes.Add(sizeCodeProf.Key, sizeCodeProf);
                        }
                    }

                    if (!secondarySizeList.Contains(sizeCodeProf.SecondarySequence))
                    {
                        secondarySizeList.Add(sizeCodeProf.SecondarySequence, sizeCodeProf.SizeCodeSecondaryRID);
                        if (!sizeCodes.ContainsKey(sizeCodeProf.Key))
                        {
                            sizeCodes.Add(sizeCodeProf.Key, sizeCodeProf);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private ROSizeCurveEntry[][] BuildSizeCurve(ROModelSizeCurveProperties modelProperties,
            SortedList primarySizeList, 
            SortedList secondarySizeList, 
            Dictionary<int, SizeCodeProfile> sizeCodes, 
            SizeCodeList sizeCodeList
            )
        {
            // The first entry of each row or column is reserved for the total
            ROSizeCurveEntry[][] sizeCurveSizeList = new ROSizeCurveEntry[secondarySizeList.Count + 1][];

            // initialize two dimensional list
            for (int s = 0; s < secondarySizeList.Count + 1; s++)
            {
                sizeCurveSizeList[s] = new ROSizeCurveEntry[primarySizeList.Count + 1];
            }

            int primaryIndex, secondaryIndex;

            // initialize grand total
            sizeCurveSizeList[0][0] =
                new ROSizeCurveEntry(sizeKey: Include.NoRID,
                    percent: 0
                        );

            try
            {

                foreach (SizeCodeProfile szCdPrf in sizeCodeList)
                {
                    // Skip the first entry for the total
                    primaryIndex = primarySizeList.IndexOfValue(szCdPrf.SizeCodePrimaryRID) + 1;
                    secondaryIndex = secondarySizeList.IndexOfValue(szCdPrf.SizeCodeSecondaryRID) + 1;
                    sizeCurveSizeList[secondaryIndex][primaryIndex] =
                        new ROSizeCurveEntry(sizeKey: szCdPrf.Key,
                            percent: Convert.ToDecimal(szCdPrf.SizeCodePercent)
                        );

                    // Build list of sizes, primary and secondary fields
                    if (!modelProperties.SizeCurveSizes.ContainsKey(szCdPrf.Key))
                    {
                        modelProperties.SizeCurveSizes.Add(szCdPrf.Key, new ROSize(size: new KeyValuePair<int, string>(szCdPrf.Key, szCdPrf.SizeCodeID),
                            name: szCdPrf.SizeCodeName,
                            productCategory: szCdPrf.SizeCodeProductCategory,
                            primary: new KeyValuePair<int, string>(szCdPrf.SizeCodePrimaryRID, szCdPrf.SizeCodePrimary),
                            primarySequence: szCdPrf.PrimarySequence,
                            secondary: new KeyValuePair<int, string>(szCdPrf.SizeCodeSecondaryRID, szCdPrf.SizeCodeSecondary),
                            secondarySequence: szCdPrf.SecondarySequence
                            ));
                    }

                    // initialize totals for primary and secondary
                    if (sizeCurveSizeList[0][primaryIndex] == null)
                    {
                        sizeCurveSizeList[0][primaryIndex] =
                        new ROSizeCurveEntry(sizeKey: Include.NoRID,
                            percent: 0
                        );
                    }

                    if (sizeCurveSizeList[secondaryIndex][0] == null)
                    {
                        sizeCurveSizeList[secondaryIndex][0] =
                        new ROSizeCurveEntry(sizeKey: Include.NoRID,
                            percent: 0
                        );
                    }

                    // Accumulate totals
                    sizeCurveSizeList[0][primaryIndex].Percent += sizeCurveSizeList[secondaryIndex][primaryIndex].Percent;
                    sizeCurveSizeList[secondaryIndex][0].Percent += sizeCurveSizeList[secondaryIndex][primaryIndex].Percent;
                    sizeCurveSizeList[0][0].Percent += sizeCurveSizeList[secondaryIndex][primaryIndex].Percent;
                }

                // round values
                for (int s = 0; s < secondarySizeList.Count + 1; s++)
                {
                    for (int p = 0; p < primarySizeList.Count + 1; p++)
                    {
                        sizeCurveSizeList[s][p].Percent = Math.Round(sizeCurveSizeList[s][p].Percent, 3, MidpointRounding.AwayFromZero);
                    }
                }

                return sizeCurveSizeList;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildSizeCurvesForStores(ROModelSizeCurveProperties modelProperties,
            SizeCurveGroupProfile sizeCurveGroupProf,
            SortedList primarySizeList,
            SortedList secondarySizeList,
            Dictionary<int, SizeCodeProfile> sizeCodes,
            int attributeKey)
        {
            ProfileList grpLevelList;
            ROSizeCurveAttributeSet attributeSetSizeCurve;
            ROSizeCurveStore storeSizeCurve;
            SizeCurveProfile sizeCurveProf;

            try
            {
                grpLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(attributeKey, true);

                // Build attribute sets and store curves

                foreach (StoreGroupLevelListViewProfile lstViewProf in grpLevelList)
                {
                    attributeSetSizeCurve = new ROSizeCurveAttributeSet(new KeyValuePair<int, string>(lstViewProf.Key, lstViewProf.Name));

                    foreach (StoreProfile storeProf in lstViewProf.Stores)
                    {
                        storeSizeCurve = new ROSizeCurveStore(new KeyValuePair<int, string>(storeProf.Key, storeProf.Text));

                        if (sizeCurveGroupProf.StoreSizeCurveHash.ContainsKey(storeProf.Key))
                        {
                            sizeCurveProf = (SizeCurveProfile)sizeCurveGroupProf.StoreSizeCurveHash[storeProf.Key];
                            storeSizeCurve = BuildSizeCurveForStore(modelProperties: modelProperties,
                                    storeSizeCurve: storeSizeCurve,
                                    primarySizeList: primarySizeList,
                                    secondarySizeList: secondarySizeList,
                                    sizeCodes: sizeCodes,
                                    sizeCurveProf: sizeCurveProf
                                    );
                        }

                        attributeSetSizeCurve.StoreSizeCurve.Add(storeSizeCurve);
                    }

                    modelProperties.AttributeSetSizeCurve.Add(attributeSetSizeCurve);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private ROSizeCurveStore BuildSizeCurveForStore(ROModelSizeCurveProperties modelProperties,
            ROSizeCurveStore storeSizeCurve,
            SortedList primarySizeList,
            SortedList secondarySizeList,
            Dictionary<int, SizeCodeProfile> sizeCodes,
            SizeCurveProfile sizeCurveProf)
        {
            try
            {
                storeSizeCurve.SizeCurve = new ROSizeCurve(sizeCurve: new KeyValuePair<int, string>(sizeCurveProf.Key, sizeCurveProf.SizeCurveName));
                storeSizeCurve.SizeCurve.SizeCurveEntry = BuildSizeCurve(modelProperties: modelProperties,
                    primarySizeList: primarySizeList,
                    secondarySizeList: secondarySizeList,
                    sizeCodes: sizeCodes,
                    sizeCodeList: sizeCurveProf.SizeCodeList);

                return storeSizeCurve;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Updates the Size Curves in memory and then updates the database
        /// </summary>
        /// <param name="modelsProperties">Input values for the Size Curve</param>
        /// <param name="cloneDates">A flag identifying if any dates are to be cloned</param>
        /// <param name="message">Output message</param>
        /// <returns></returns>
        override public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;

            if (SetSizeCurveGroupProfile(modelsProperties: modelsProperties, message: ref message))
            {
                if (!applyOnly)
                {
                    if (_sizeCurveGroupProf.Key == Include.NoRID)
                    {
                        foreach (SizeCurveProfile sizeCurveProf in _sizeCurveGroupProf.SizeCurveList)
                        {
                            sizeCurveProf.Key = Include.NoRID;
                        }
                    }

                    _sizeCurveGroupProf.WriteSizeCurveGroup(SAB, false);
                }
            }
            else
            {
                successful = false;
            }

            return _sizeCurveGroupProf;
        }

        /// <summary>
        /// Takes values from input class and updates the Size Curve memory object
        /// </summary>
        /// <param name="modelsProperties">Input values for the Size Curve</param>
        /// <param name="message">The message</param>
        private bool SetSizeCurveGroupProfile(ROModelProperties modelsProperties, ref string message)
        {
            SizeCurveProfile sizeCurveProf;
            ROModelSizeCurveProperties sizeCurveProperties = (ROModelSizeCurveProperties)modelsProperties;

            // Verify size group contains all sizes in the curve
            if (CheckForValidSizeGroup(sizeGroupKey: sizeCurveProperties.SizeGroup.Key))
            {
                _sizeCurveGroupProf.DefinedSizeGroupRID = sizeCurveProperties.SizeGroup.Key;
            }
            else
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_CurveSizeCodeNotOnSizeGroup);
                return false;
            }

            _sizeCurveGroupProf.Key = sizeCurveProperties.Model.Key;
            _sizeCurveGroupProf.SizeCurveGroupName = sizeCurveProperties.Model.Value;

            if (sizeCurveProperties.DefaultSizeCurve != null)
            {
                UpdateSizeCurve(sizeCurve: sizeCurveProperties.DefaultSizeCurve, sizeCurveProfile: _sizeCurveGroupProf.DefaultSizeCurve, originalSizeCurve: _modelProperties.DefaultSizeCurve);
            }

            // obtains original values to compare totals
            foreach (ROSizeCurveAttributeSet setSizeCurve in sizeCurveProperties.AttributeSetSizeCurve)
            {
                ROSizeCurveAttributeSet originalSetSizeCurve = _modelProperties.AttributeSetSizeCurve.Find(a => a.AttributeSet.Key == setSizeCurve.AttributeSet.Key);
                if (setSizeCurve.StoreSizeCurve != null)
                {
                    foreach (ROSizeCurveStore storeSizeCurve in setSizeCurve.StoreSizeCurve)
                    {
                        if (storeSizeCurve.SizeCurve != null)
                        {
                            sizeCurveProf = (SizeCurveProfile)_sizeCurveGroupProf.SizeCurveList.FindKey(storeSizeCurve.SizeCurve.SizeCurve.Key);
                            // get original size curve from the store
                            ROSizeCurve originalSizeCurve = null;
                            if (originalSetSizeCurve != null)
                            {
                                ROSizeCurveStore originalStoreSizeCurve = originalSetSizeCurve.StoreSizeCurve.Find(s => s.Store.Key == storeSizeCurve.Store.Key);
                                if (originalStoreSizeCurve != null
                                    && originalStoreSizeCurve.SizeCurve != null)
                                {
                                    originalSizeCurve = originalStoreSizeCurve.SizeCurve;
                                }
                            }
                            UpdateSizeCurve(sizeCurve: storeSizeCurve.SizeCurve, sizeCurveProfile: sizeCurveProf, originalSizeCurve: originalSizeCurve);
                        }
                        else
                        {
                            if (_sizeCurveGroupProf.StoreSizeCurveHash.ContainsKey(storeSizeCurve.Store.Key))
                            {
                                _sizeCurveGroupProf.StoreSizeCurveHash.Remove(storeSizeCurve.Store.Key);
                                _sizeCurveGroupProf.DeleteStoreSizeCurveProfile(storeSizeCurve.Store.Key);
                            }
                        }
                    }
                }
            }

            if (sizeCurveProperties.SizeCurveKeyToCopyToDefault != Include.NoRID)
            {
                if (!CopyCurveToDefault(sizeCurveKey: sizeCurveProperties.SizeCurveKeyToCopyToDefault, message: ref message))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckForValidSizeGroup(int sizeGroupKey)
        {
            SizeGroupProfile sizeGroupProf;

            try
            {
                if (sizeGroupKey != Include.NoRID
                    && _sizeCurveGroupProf.DefinedSizeGroupRID != sizeGroupKey)
                {
                    sizeGroupProf = new SizeGroupProfile(sizeGroupKey);

                    foreach (SizeCurveProfile sizeCurveProf in _sizeCurveGroupProf.SizeCurveList)
                    {
                        foreach (SizeCodeProfile sizeCodeProf in sizeCurveProf.SizeCodeList)
                        {
                            if (!sizeGroupProf.SizeCodeList.Contains(sizeCodeProf.Key))
                            {
                                return false;
                            }
                        }
                    }

                    foreach (SizeCodeProfile sizeCodeProf in _sizeCurveGroupProf.DefaultSizeCurve.SizeCodeList)
                    {
                        if (!sizeGroupProf.SizeCodeList.Contains(sizeCodeProf.Key))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        private bool CopyCurveToDefault(int sizeCurveKey, ref string message)
        {
            SizeCurveProfile sizeCurveProf;

            try
            {

                sizeCurveProf = (SizeCurveProfile)_sizeCurveGroupProf.SizeCurveList.FindKey(sizeCurveKey);
                if (sizeCurveProf != null)
                {
                    _sizeCurveGroupProf.DefaultSizeCurve.SizeCodeList.Clear();

                    foreach (SizeCodeProfile sizeCodeProf in sizeCurveProf.SizeCodeList)
                    {
                        _sizeCurveGroupProf.DefaultSizeCurve.SizeCodeList.Add((SizeCodeProfile)sizeCodeProf.Clone());
                    }

                    return true;
                }
                else
                {
                    message = SAB.ClientServerSession.Audit.GetText(
                        messageCode: eMIDTextCode.msg_ValueWasNotFound,
                        addToAuditReport: true,
                        args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_SizeCurve) }
                        );
                    return false;
                }
            }
            catch 
            {
                throw;
            }

        }

        /// <summary>
        /// Updates an individual Size Curve in memory with input values
        /// </summary>
        /// <param name="sizeCurve">The input Size Curve values</param>
        /// <param name="sizeCurveProfile">The memory Size Curve values</param>
        private void UpdateSizeCurve(ROSizeCurve sizeCurve, SizeCurveProfile sizeCurveProfile, ROSizeCurve originalSizeCurve)
        {
            SizeCodeProfile sizeCodeProf;
            SizeCodeProfile curveSizeCodeProf = null;

            SpreadChangesToTotals(sizeCurve: sizeCurve, originalSizeCurve: originalSizeCurve);

            if (BalanceCurveTo100(sizeCurve: sizeCurve))
            {
                for (int s = 1; s < sizeCurve.SizeCurveEntry.GetLength(0); s++)
                {
                    for (int p = 1; p < sizeCurve.SizeCurveEntry[s].GetLength(0); p++)
                    {
                        curveSizeCodeProf = (SizeCodeProfile)sizeCurveProfile.SizeCodeList.FindKey(sizeCurve.SizeCurveEntry[s][p].SizeKey);

                        if (curveSizeCodeProf == null)
                        {
                            sizeCodeProf = (SizeCodeProfile)_sizeCurveGroupProf.SizeCodeList.FindKey(sizeCurve.SizeCurveEntry[s][p].SizeKey);
                            if (sizeCodeProf == null)
                            {
                                sizeCodeProf = SAB.HierarchyServerSession.GetSizeCodeProfile(sizeCurve.SizeCurveEntry[s][p].SizeKey);
                            }

                            curveSizeCodeProf = (SizeCodeProfile)sizeCodeProf.Clone();
                            sizeCurveProfile.SizeCodeList.Add(curveSizeCodeProf);
                        }

                        curveSizeCodeProf.SizeCodePercent = (float)(decimal)sizeCurve.SizeCurveEntry[s][p].Percent;
                    }
                }
            }
        }

        /// <summary>
        /// Checks for changes to totals and spreads them accordingly
        /// </summary>
        /// <param name="sizeCurve">The Size Curve to be updated</param>
        /// <returns>A flag identifying if successful</returns>
        private bool SpreadChangesToTotals(ROSizeCurve sizeCurve, ROSizeCurve originalSizeCurve)
        {
            // spread primary column totals
            // skip the first column since it is grand total
            for (int p = 1; p < sizeCurve.SizeCurveEntry[0].GetLength(0); p++) // number of primary columns is the length of the first row
            {
                // check for change in column (primary) total
                // if no original value, spread if value not equal zero
                if (originalSizeCurve == null
                    || p > originalSizeCurve.SizeCurveEntry[0].GetLength(0)
                    || originalSizeCurve.SizeCurveEntry[0][p] == null)
                {
                    if (sizeCurve.SizeCurveEntry[0][p] != null
                        && sizeCurve.SizeCurveEntry[0][p].Percent != 0)
                    {
                        SpreadSizeCurvePercentages(sizeCurve: sizeCurve, primaryIndex: p, secondaryIndex: 0);
                    }
                }
                else if (sizeCurve.SizeCurveEntry[0][p].Percent != originalSizeCurve.SizeCurveEntry[0][p].Percent)
                {
                    SpreadSizeCurvePercentages(sizeCurve: sizeCurve, primaryIndex: p, secondaryIndex: 0);
                }
            }

            // spread secondary row totals
            // skip the first row since it is grand total
            for (int s = 1; s < sizeCurve.SizeCurveEntry.GetLength(0); s++)
            {
                // check for change in row (secondary) total
                // if no original value, spread if value not equal zero
                if (originalSizeCurve == null
                    || s > originalSizeCurve.SizeCurveEntry.GetLength(0)
                    || originalSizeCurve.SizeCurveEntry[s][0] == null)
                {
                    if (sizeCurve.SizeCurveEntry[s][0] != null
                        && sizeCurve.SizeCurveEntry[s][0].Percent != 0)
                    {
                        SpreadSizeCurvePercentages(sizeCurve: sizeCurve, primaryIndex: 0, secondaryIndex: s);
                    }
                }
                else if (sizeCurve.SizeCurveEntry[s][0].Percent != originalSizeCurve.SizeCurveEntry[s][0].Percent)
                {
                    SpreadSizeCurvePercentages(sizeCurve: sizeCurve, primaryIndex: 0, secondaryIndex: s);
                }
            }

            // spread change to grand total
            if (sizeCurve.SizeCurveEntry[0][0].Percent != 100)
            {
                BalanceSizeCurvePercentages(sizeCurve: sizeCurve, balanceToValue: Convert.ToDouble(sizeCurve.SizeCurveEntry[0][0].Percent));
            }

            return true;
        }

        /// <summary>
        /// Verifies Size Curve balances to 100
        /// </summary>
        /// <param name="sizeCurve">The Size Curve to be updated</param>
        /// <returns>A flag identifying if successful</returns>
        private bool BalanceCurveTo100(ROSizeCurve sizeCurve)
        {
            if (SumSizeCurvePercentages(sizeCurve: sizeCurve) != 100)
            {
                BalanceSizeCurvePercentages(sizeCurve: sizeCurve);
                SumSizeCurvePercentages(sizeCurve: sizeCurve);
            }

            return true;
        }

        /// <summary>
        /// Summarizes size curve values and creates objects null entries when needed 
        /// </summary>
        /// <param name="sizeCurve">The Size Curve to be updated</param>
        /// <returns>The grand total for the Size Curve</returns>
        private decimal SumSizeCurvePercentages(ROSizeCurve sizeCurve)
        {
            // initialize totals to zero
            // initialize grand total
            if (sizeCurve.SizeCurveEntry[0][0] == null)
            {
                sizeCurve.SizeCurveEntry[0][0] = new ROSizeCurveEntry(sizeKey: Include.NoRID,
                    percent: 0
                        );
            }
            else
            {
                sizeCurve.SizeCurveEntry[0][0].Percent = 0;
            }
            // initialize row secondary totals
            // skip the first row since it is the grand total
            for (int s = 1; s < sizeCurve.SizeCurveEntry.GetLength(0); s++)
            {
                if (sizeCurve.SizeCurveEntry[s][0] == null)
                {
                    sizeCurve.SizeCurveEntry[s][0] =
                        new ROSizeCurveEntry(sizeKey: Include.NoRID,
                            percent: 0
                        );
                }
                else
                {
                    sizeCurve.SizeCurveEntry[s][0].Percent = 0;
                }
            }
            // initialize column primary totals
            // skip the first column since it is the grand total
            for (int p = 1; p < sizeCurve.SizeCurveEntry[0].GetLength(0); p++)
            {
                if (sizeCurve.SizeCurveEntry[0][p] == null)
                {
                    sizeCurve.SizeCurveEntry[0][p] =
                   new ROSizeCurveEntry(sizeKey: Include.NoRID,
                       percent: 0
                   );
                }
                else
                {
                    sizeCurve.SizeCurveEntry[0][p].Percent = 0;
                }
            }

            // accumulate new totals
            for (int s = 1; s < sizeCurve.SizeCurveEntry.GetLength(0); s++)
            {
                for (int p = 1; p < sizeCurve.SizeCurveEntry[s].GetLength(0); p++)
                {
                    if (sizeCurve.SizeCurveEntry[s][p] == null)
                    {
                        sizeCurve.SizeCurveEntry[s][p] = new ROSizeCurveEntry(sizeKey: Include.NoRID,
                           percent: 0
                           );
                    }
                    decimal curveValue = Math.Round(sizeCurve.SizeCurveEntry[s][p].Percent, 3, MidpointRounding.AwayFromZero);
                    sizeCurve.SizeCurveEntry[0][p].Percent += curveValue;
                    sizeCurve.SizeCurveEntry[s][0].Percent += curveValue;
                    sizeCurve.SizeCurveEntry[0][0].Percent += curveValue;
                }
            }

            return sizeCurve.SizeCurveEntry[0][0].Percent;
        }

        /// <summary>
        /// Balances Size Curve to 100
        /// </summary>
        /// <param name="sizeCurve">The Size Curve to be updated</param>
        /// <returns>A flag identifying if successful</returns>
        private bool BalanceSizeCurvePercentages(ROSizeCurve sizeCurve, double balanceToValue = 100)
        {
            BasicSpread spreader = new BasicSpread();
            ArrayList inValues = new ArrayList();
            ArrayList outValues;
            int i;

            // spread value directly to detail values
            for (int s = 1; s < sizeCurve.SizeCurveEntry.GetLength(0); s++)
            {
                for (int p = 1; p < sizeCurve.SizeCurveEntry[0].GetLength(0); p++)
                {
                    inValues.Add(Convert.ToDouble(sizeCurve.SizeCurveEntry[s][p].Percent));
                }
            }

            spreader.ExecuteSimpleSpread(balanceToValue, inValues, 3, out outValues);

            i = 0;
            for (int s = 1; s < sizeCurve.SizeCurveEntry.GetLength(0); s++)
            {
                for (int p = 1; p < sizeCurve.SizeCurveEntry[0].GetLength(0); p++)
                {
                    sizeCurve.SizeCurveEntry[s][p].Percent = (decimal)Convert.ToDouble(outValues[i]);
                    i++;
                }
            }

            return true;
        }

        /// <summary>
        /// Spread Size Curve values
        /// </summary>
        /// <param name="sizeCurve">The Size Curve to be updated</param>
        /// <returns>A flag identifying if successful</returns>
        private bool SpreadSizeCurvePercentages(ROSizeCurve sizeCurve, int primaryIndex, int secondaryIndex)
        {
            BasicSpread spreader = new BasicSpread();
            ArrayList inValues = new ArrayList();
            ArrayList outValues;

            if (secondaryIndex == 0)  // spread primary
            {
                // get secondary values for primary
                for (int s = 1; s < sizeCurve.SizeCurveEntry.GetLength(0); s++)
                {
                    inValues.Add(Convert.ToDouble(sizeCurve.SizeCurveEntry[s][primaryIndex].Percent));
                }

                spreader.ExecuteSimpleSpread(Convert.ToDouble(sizeCurve.SizeCurveEntry[0][primaryIndex].Percent), inValues, 3, out outValues);

                // update secondary values for primary
                for (int s = 1; s < sizeCurve.SizeCurveEntry.GetLength(0); s++)
                {
                    sizeCurve.SizeCurveEntry[s][primaryIndex].Percent = Convert.ToDecimal(outValues[s - 1]);
                }

            }
            else if (primaryIndex == 0)  // spread secondary
            {
                // get primary values for secondary
                for (int p = 1; p < sizeCurve.SizeCurveEntry[0].GetLength(0); p++)
                {
                    inValues.Add(Convert.ToDouble(sizeCurve.SizeCurveEntry[secondaryIndex][p].Percent));
                }

                spreader.ExecuteSimpleSpread(Convert.ToDouble(sizeCurve.SizeCurveEntry[secondaryIndex][0].Percent), inValues, 3, out outValues);

                // update primary values for secondary
                for (int p = 1; p < sizeCurve.SizeCurveEntry[0].GetLength(0); p++)
                {
                    sizeCurve.SizeCurveEntry[secondaryIndex][p].Percent = Convert.ToDecimal(outValues[p - 1]);
                }

            }

            return true;
        }

        override public bool ModelDelete(int key, ref string message)
        {
            message = null;
            if (_sizeCurveGroupProf == null)
            {
                _sizeCurveGroupProf = (SizeCurveGroupProfile)SAB.HierarchyServerSession.GetModelData(aModelType: eModelType.SizeCurve, aModelRID: key);
            }

            _sizeCurveGroupProf.DeleteSizeCurveGroup();

            return true;
        }

        override public ModelProfile GetModelProfile(ROModelParms parms)
        {
            ModelProfile modelProfile;
            int attributeKey = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
            int sizeGroupKey = Include.NoRID;

            if (parms is ROSizeCurveModelParms)
            {
                ROSizeCurveModelParms sizeCurveModelParms = (ROSizeCurveModelParms)parms;
                if (sizeCurveModelParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = sizeCurveModelParms.AttributeKey;
                }
                if (sizeCurveModelParms.SizeGroupKey != Include.NoRID)
                {
                    sizeGroupKey = sizeCurveModelParms.SizeGroupKey;
                }
            }

            if (parms.Key == Include.NoRID)
            {
                SizeCurveGroupProfile sizeCurveGroupProf = new SizeCurveGroupProfile(Include.NoRID);
                sizeCurveGroupProf.DefaultSizeCurve = new SizeCurveProfile(Include.NoRID);
                sizeCurveGroupProf.DefaultSizeCurve.SizeCurveName = Include.DefaultSizeCurveName;

                sizeCurveGroupProf.DefinedSizeGroupRID = sizeGroupKey;

                modelProfile = sizeCurveGroupProf;
            }
            else if (parms.ReadOnly
                || !FunctionSecurity.AllowUpdate)
            {
                modelProfile = SAB.HierarchyServerSession.GetModelData(aModelType: parms.ModelType, aModelRID: parms.Key);
            }
            else
            {
                modelProfile = (ModelProfile)SAB.HierarchyServerSession.GetModelDataForUpdate(aModelType: parms.ModelType, aModelRID: parms.Key, aAllowReadOnly: true, isWindows: MIDEnvironment.isWindows);
                if (modelProfile.ModelLockStatus == eLockStatus.ReadOnly)
                {
                    MIDEnvironment.isChangedToReadOnly = true;
                }
            }

            return modelProfile;
        }

        override public ROModelParms GetModelParms(ROModelPropertiesParms parms, eModelType modelType, int key, bool readOnly = false)
        {
            ROModelSizeCurveProperties sizeCurveProperties = (ROModelSizeCurveProperties)parms.ROModelProperties;

            ROModelParms modelParms = new ROSizeCurveModelParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.GetModel,
                ROInstanceID: parms.ROInstanceID,
                modelType: modelType,
                key: key,
                readOnly: readOnly,
                attributeKey: sizeCurveProperties.Attribute.Key,
                sizeGroupKey: sizeCurveProperties.SizeGroup.Key
                );

            return modelParms;
        }

        override public ROModelParms GetModelParms(ROModelParms parms, eModelType modelType, int key, bool readOnly = false)
        {
            int attributeKey = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
            int sizeGroupKey = Include.NoRID;
            ROModelParms modelParms;

            if (parms is ROSizeCurveModelParms)
            {
                ROSizeCurveModelParms sizeCurveModelParms = (ROSizeCurveModelParms)parms;
                if (sizeCurveModelParms.AttributeKey != Include.NoRID)
                {
                    attributeKey = sizeCurveModelParms.AttributeKey;
                }
                if (sizeCurveModelParms.SizeGroupKey != Include.NoRID)
                {
                    sizeGroupKey = sizeCurveModelParms.SizeGroupKey;
                }
                modelParms = new ROSizeCurveModelParms(sROUserID: parms.ROUserID,
                    sROSessionID: parms.ROSessionID,
                    ROClass: parms.ROClass,
                    RORequest: eRORequest.GetModel,
                    ROInstanceID: parms.ROInstanceID,
                    modelType: modelType,
                    key: key,
                    readOnly: readOnly,
                    attributeKey: attributeKey,
                    sizeGroupKey: sizeGroupKey
                    );
            }
            else
            {
                modelParms = new ROModelParms(sROUserID: parms.ROUserID,
                    sROSessionID: parms.ROSessionID,
                    ROClass: parms.ROClass,
                    RORequest: eRORequest.GetModel,
                    ROInstanceID: parms.ROInstanceID,
                    modelType: modelType,
                    key: key,
                    readOnly: readOnly
                    );
            }

            return modelParms;
        }

        override public bool ModelNameExists(string name, int userKey)
        {
            ModelProfile checkExists = SAB.HierarchyServerSession.GetModelData(aModelType: eModelType.SizeCurve, modelID: name);

            return checkExists.Key != Include.NoRID;
        }
    }
}
