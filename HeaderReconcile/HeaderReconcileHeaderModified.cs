using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace HeaderReconcile
{
    //==========================================================================
    // HeaderReconcileHeaderModified
    // Houses all of the code that determines if the transaction header
    // is modifying an existing header.
    //==========================================================================


    public partial class HeaderReconcileProcess
    {
		// Begin TT#4696 - stodd - Matching Headers with no changes are being passed through to the Header Load
        private HeaderCharacteristicsData _hdrCharData = null;

        private HeaderCharacteristicsData HeaderCharacteristicsData
        {
            get
            {
                if (_hdrCharData == null)
                {
                    _hdrCharData = new HeaderCharacteristicsData();
                }
                return _hdrCharData;
            }
        }
		// End TT#4696 - stodd - Matching Headers with no changes are being passed through to the Header Load
		
        /// <summary>
        /// Once a matching header is found in the system, is it modifying the header?
        /// </summary>
        /// <param name="aHeader"></param>
        /// <returns></returns>
        private bool IsTransactionModifyingHeader(ref eReturnCode returnCode)
        {
            bool isModified = false;

            try
            {
                // New Header
                if (_allocationHeaderProfile.Key == Include.NoRID)
                {
                    return true;
                }

                isModified = IsModifyingHeaderInfo();
                if (!isModified)
                {
                    isModified = IsModifyingComponentInfo();
                    if (!isModified)
                    {
                        isModified = IsModifyingCharacteristicInfo();
                    }
                }  

                if (isModified)
                {
                    //====================================================================================
                    // This checks to see if the transaction is missing any packs already on the header.
                    // If we find any, we set up a "zero" pack to delete them from the header.
                    //====================================================================================
                    IsModifyingPacksToZero();
                    //====================================================================================
                    // These check to see if the transaction is missing any sizes already on the header.
                    // If we find any, we set up a "zero" size to delete them from the header.
                    //====================================================================================
                    IsModifyingBulkColorSizeToZero();
                    IsModifyingPackColorSizeToZero();
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }

            return isModified;
        }

        private bool IsModifyingHeaderInfo()
        {
            bool isModified = false;

            try
            {
                // Begin TT#4696 - stodd - Matching Headers with no changes are being passed through to the Header Load
                //if (_transactionHeader.HeaderDescription == null || _transactionHeader.HeaderDescription.Trim() == string.Empty)  
                if (_transactionHeader.HeaderDescription == null)
                {
                    //if (!(_allocationHeaderProfile.HeaderDescription == null || _allocationHeaderProfile.HeaderDescription.Trim() == string.Empty))
                    //{
                    //    return true;
                    //}
                }
                // End TT#4696 - stodd - Matching Headers with no changes are being passed through to the Header Load
                else if (_transactionHeader.HeaderDescription != _allocationHeaderProfile.HeaderDescription)
                {
                    return true;
                }
                if (_transactionHeader.HeaderDateSpecified && _transactionHeader.HeaderDate != _allocationHeaderProfile.HeaderDay)
                {
                    return true;
                }
                if (_transactionHeader.UnitRetailSpecified && _transactionHeader.UnitRetail != _allocationHeaderProfile.UnitRetail)
                {
                    return true;
                }
                if (_transactionHeader.UnitCostSpecified && _transactionHeader.UnitCost != _allocationHeaderProfile.UnitCost)
                {
                    return true;
                }
                if (_transactionHeader.TotalUnitsSpecified && _transactionHeader.TotalUnits != _allocationHeaderProfile.TotalUnitsToAllocate)
                {
                    return true;
                }

                // Begin TT#5495 - JSmith - Units Per Carton changes are not passed through Header Reconcile
                if (_transactionHeader.UnitsPerCarton != null
                    && _transactionHeader.UnitsPerCarton != Convert.ToString(_allocationHeaderProfile.UnitsPerCarton))
                {
                    return true;
                }
                // End TT#5495 - JSmith - Units Per Carton changes are not passed through Header Reconcile

                HierarchyNodeProfile np = _SAB.HierarchyServerSession.GetNodeData(_allocationHeaderProfile.StyleHnRID, false);
                if (np == null || np.Key == Include.NoRID)
                {
                    // ERROR - merchandice node not found!!!
                }
                else if (_transactionHeader.StyleId != np.NodeID)
                {
                    return true;
                }

                //HierarchyNodeProfile pnp = _SAB.HierarchyServerSession.GetNodeData(aHeader.ParentOfStyleId, false);
                //if (pnp == null || pnp.Key == Include.NoRID)
                //{
                //    // ERROR - merchandice node not found!!!
                //}
                //if (aHeader.ParentOfStyleId != pnp.LevelText)
                //{
                //    return true;
                //}

                if (_transactionHeader.SizeGroupName == null || _transactionHeader.SizeGroupName.Trim() == string.Empty)
                {
                    if (_allocationHeaderProfile.SizeGroupRID != Include.UndefinedSizeGroupRID)
                    {
                        return true;
                    }
                }
                else
                {
                    SizeGroup sizeGroupData = new SizeGroup();
                    DataTable sizeGroupTable = sizeGroupData.GetSizeGroup(_transactionHeader.SizeGroupName);
                    if (sizeGroupTable.Rows.Count > 0)
                    {
                        int sizeGroupRid = int.Parse(sizeGroupTable.Rows[0]["SIZE_GROUP_RID"].ToString());
                        if (sizeGroupRid != _allocationHeaderProfile.SizeGroupRID)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        // Error - can't find size group rid
                    }
                }


                // BEGIN TT#5442 - AGallagher - Enable Total Level Multiple Changes via XML
                if (_transactionHeader.BulkMultipleSpecified && _transactionHeader.Pack == null && _transactionHeader.BulkColor == null )
                {
                    if (_transactionHeader.BulkMultiple != _allocationHeaderProfile.AllocationMultiple)
                     return true;
                }

                else
                // END TT#5442 - AGallagher - Enable Total Level Multiple Changes via XML

                if (_transactionHeader.BulkMultipleSpecified && _transactionHeader.BulkMultiple != _allocationHeaderProfile.BulkMultiple)
                {
                    return true;
                }

                if (IsModifyingHeaderType())
                {
                    return true;
                }

                if (_transactionHeader.DistCenter == null || _transactionHeader.DistCenter.Trim() == string.Empty)
                {
                    if (!(_allocationHeaderProfile.DistributionCenter == null || _allocationHeaderProfile.DistributionCenter.Trim() == string.Empty))
                    {
                        return true;
                    }
                }
                else if (_transactionHeader.DistCenter != _allocationHeaderProfile.DistributionCenter)
                {
                    return true;
                }

                if (_transactionHeader.Vendor == null || _transactionHeader.Vendor.Trim() == string.Empty)
                {
                    if (!(_allocationHeaderProfile.Vendor == null || _allocationHeaderProfile.Vendor.Trim() == string.Empty))
                    {
                        return true;
                    }
                }
                else if (_transactionHeader.Vendor != _allocationHeaderProfile.Vendor)
                {
                    return true;
                }

                if (_transactionHeader.PurchaseOrder == null || _transactionHeader.PurchaseOrder.Trim() == string.Empty)
                {
                    if (!(_allocationHeaderProfile.PurchaseOrder == null || _allocationHeaderProfile.PurchaseOrder.Trim() == string.Empty))
                    {
                        return true;
                    }
                }
                else if (_transactionHeader.PurchaseOrder != _allocationHeaderProfile.PurchaseOrder)
                {
                    return true;
                }

                // Begin TT#5120 - JSmith - Header Reconcile Creating Update Records for Inputs w/o Workflow tags
                if (!string.IsNullOrEmpty(_transactionHeader.Workflow))
                {
                // End TT#5120 - JSmith - Header Reconcile Creating Update Records for Inputs w/o Workflow tags
                    // Begin TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating
                    string workflowName;
                    // Begin TT#5093 - JSmith - Header Reconcile Process Creating Unexpected Modify Files
                    //if (string.IsNullOrEmpty(_allocationHeaderProfile.WorkflowName) &&
                    //    _allocationHeaderProfile.WorkflowRID != Include.NoRID)
                    if (string.IsNullOrEmpty(_allocationHeaderProfile.WorkflowName) &&
                        _allocationHeaderProfile.WorkflowRID != Include.NoRID &&
                        _allocationHeaderProfile.WorkflowRID != Include.UndefinedWorkflowRID)
                    // End TT#5093 - JSmith - Header Reconcile Process Creating Unexpected Modify Files
                    {
                        _dictWorkflows.TryGetValue(_allocationHeaderProfile.WorkflowRID, out workflowName);
                        if (string.IsNullOrEmpty(workflowName))
                        {
                            workflowName = _workflowData.GetWorkflowName(_allocationHeaderProfile.WorkflowRID);
                            _dictWorkflows.Add(_allocationHeaderProfile.WorkflowRID, workflowName);
                        }
                        _allocationHeaderProfile.WorkflowName = workflowName;
                    }
                    // End TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating

                    if (_transactionHeader.Workflow == null || _transactionHeader.Workflow.Trim() == string.Empty)
                    {
                        if (!(_allocationHeaderProfile.WorkflowName == null || _allocationHeaderProfile.WorkflowName.Trim() == string.Empty))
                        {
                            return true;
                        }
                    }
                    else if (_transactionHeader.Workflow != _allocationHeaderProfile.WorkflowName)
                    {
                        return true;
                    }
                // Begin TT#5120 - JSmith - Header Reconcile Creating Update Records for Inputs w/o Workflow tags 
                }
                // End TT#5120 - JSmith - Header Reconcile Creating Update Records for Inputs w/o Workflow tags

                // Begin TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating
                // Return as modified any time the workflow trigger is true.
                //if (_transactionHeader.WorkflowTriggerSpecified && _transactionHeader.WorkflowTrigger != _allocationHeaderProfile.WorkflowTrigger)
                //{
                //    return true;
                //}
                if (_transactionHeader.WorkflowTriggerSpecified && _transactionHeader.WorkflowTrigger)
                {
                    return true;
                }
                // End TT#4972 - JSmith - Header Reconcile Generating Updates Files for Headers That Do Not Need Updating

                if (_transactionHeader.VSWID == null || _transactionHeader.VSWID.Trim() == string.Empty)
                {
                    if (!(_allocationHeaderProfile.ImoID == null || _allocationHeaderProfile.ImoID.Trim() == string.Empty))
                    {
                        return true;
                    }
                }
                else if (_transactionHeader.VSWID != _allocationHeaderProfile.ImoID)
                {
                    return true;
                }

                if ((_transactionHeader.VSWProcess == HeadersHeaderVSWProcess.Adjust && !_allocationHeaderProfile.AdjustVSW_OnHand)
                    || _transactionHeader.VSWProcess == HeadersHeaderVSWProcess.Replace && _allocationHeaderProfile.AdjustVSW_OnHand)
                {
                    return true;
                }

            }
            catch
            {
                throw;
            }
            finally
            {

            }

            return isModified;
        }

        private bool IsModifyingHeaderType()
        {
            string strHeaderType = _transactionHeader.HeaderType.ToString().ToUpper();
            bool isModified = false;
            string errorMsg = string.Empty;

            switch (strHeaderType)
            {
                case "RECEIPT":
                {
                    if (_allocationHeaderProfile.HeaderType != eHeaderType.Receipt)
                    {
                        isModified = true;
                    }
                    break;
                }
                case "PO":
                {
                    if (_allocationHeaderProfile.HeaderType != eHeaderType.PurchaseOrder)
                    {
                        isModified = true;
                    }
                    break;
                }
                case "ASN":
                {
                    if (_allocationHeaderProfile.HeaderType != eHeaderType.ASN)
                    {
                        isModified = true;
                    }
                    break;
                }
                case "DUMMY":
                {
                    if (_allocationHeaderProfile.HeaderType != eHeaderType.Dummy)
                    {
                        isModified = true;
                    }
                    break;
                }
                case "DROPSHIP":
                {
                    if (_allocationHeaderProfile.HeaderType != eHeaderType.DropShip)
                    {
                        isModified = true;
                    }
                    break;
                }
                case "RESERVE":
                {
                    if (_allocationHeaderProfile.HeaderType != eHeaderType.Reserve)
                    {
                        isModified = true;
                    }
                    break;
                }
                case "WORKUPTOTALBUY":
                {
                    if (_allocationHeaderProfile.HeaderType != eHeaderType.WorkupTotalBuy)
                    {
                        isModified = true;
                    }
                    break;
                }
                case "VSW":
                {
                    if (_allocationHeaderProfile.HeaderType != eHeaderType.IMO)
                    {
                        isModified = true;
                    }
                    break;
                }

                default:
                {
                    //errorMsg = "Allocation Header, " + aHeader.HeaderId;
                    //errorMsg += ", has an invalid Header Type [" + strHeaderType + "]";
                    //errorMsg += System.Environment.NewLine;
                    //_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMsg, GetType().Name);
                    //returnCode = eReturnCode.severe;
                    break;
                }
            }

            return isModified;
        }

        private bool IsModifyingComponentInfo()
        {
            bool isModified = false;

            try
            {
                // Compare total number of Bulk Colors
                if (_transactionHeader.BulkColor == null)
                {
                    if (_allocationHeaderProfile.BulkColors.Count > 0)
                    {
                        return true;
                    }
                }
                else if (_transactionHeader.BulkColor.Length != _allocationHeaderProfile.BulkColors.Count)
                {
                    return true;
                }
                

                // Compare total number of packs
                if (_transactionHeader.Pack == null)
                {
                    if (_allocationHeaderProfile.Packs.Count > 0)
                    {
                        return true;
                    }
                }
                //else if (_transactionHeader.Pack.Length != _allocationHeaderProfile.Packs.Count)
                //{
                //    return true;
                //}
                
                // Compare Bulk Colors
                if (_allocationHeaderProfile.BulkColors.Count > 0)
                {
                    foreach (HeaderBulkColorProfile aColor in _allocationHeaderProfile.BulkColors.Values)
                    {
                        if (IsModifyingBulkColor(aColor))
                        {
                            return true;
                        }
                    }
                }

                // Compare Packs
                if (_allocationHeaderProfile.Packs.Count > 0)
                {
                    foreach (HeaderPackProfile aPack in _allocationHeaderProfile.Packs.Values)
                    {
                        if (IsModifyingPack(aPack))
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }

            return isModified;
        }

        private bool IsModifyingBulkColor(HeaderBulkColorProfile aHdrColor)
        {
            bool isModified = false;
            bool colorMatch = false;
            ColorCodeProfile colorCodeProfile = _SAB.HierarchyServerSession.GetColorCodeProfile(aHdrColor.Key);
            if (colorCodeProfile.Key == Include.UndefinedColor)
            {
                return true;
            }

            try
            {
                foreach (HeadersHeaderBulkColor aTransColor in _transactionHeader.BulkColor)
                {
                    if (aTransColor.ColorCodeID == colorCodeProfile.ColorCodeID)
                    {
                        colorMatch = true;
                        if (aTransColor.Units != aHdrColor.Units)
                        {
                            return true;
                        }

                        // Compare Bulk / Sizes
                        foreach (HeaderBulkColorSizeProfile aHdrSize in aHdrColor.Sizes.Values)
                        {
                            isModified = IsModifyingBulkColorSize(aTransColor, aHdrSize);
                            if (isModified)
                            {
                                break;
                            }
                        }

                        break;
                    }
                }

                if (!colorMatch)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }

            return isModified;
        }

        private bool IsModifyingBulkColorSize(HeadersHeaderBulkColor aTransColor, HeaderBulkColorSizeProfile aHdrSize)
        {
            bool isModified = false;
            bool sizeMatch = false;
            SizeCodeProfile hdrSizeCodeProf = _SAB.HierarchyServerSession.GetSizeCodeProfile(aHdrSize.Key);
            if (hdrSizeCodeProf.Key == Include.NoRID)
            {
                return true;
            }

            try
            {
                foreach (HeadersHeaderBulkColorBulkColorSize aTransSize in aTransColor.BulkColorSize)
                {
                    if (aTransSize.SizeCodeID == hdrSizeCodeProf.SizeCodeID)
                    {
                        sizeMatch = true;
                        if (aTransSize.Units != aHdrSize.Units)
                        {
                            return true;
                        }

                        if (aTransSize.SizeCodeName != null &&  aTransSize.SizeCodeName != string.Empty) 
                        {
                            if (aTransSize.SizeCodeName != hdrSizeCodeProf.SizeCodeName)
                            {
                                return true;
                            }
                        }
                        if (aTransSize.SizeCodePrimary != null && aTransSize.SizeCodePrimary != string.Empty)
                        {
                            if (aTransSize.SizeCodePrimary != hdrSizeCodeProf.SizeCodePrimary)
                            {
                                return true;
                            }
                        }
                        if (aTransSize.SizeCodeSecondary != null && aTransSize.SizeCodeSecondary != string.Empty)
                        {
                            if (aTransSize.SizeCodeSecondary != hdrSizeCodeProf.SizeCodeSecondary)
                            {
                                return true;
                            }
                        }
                        if (aTransSize.SizeCodeProductCategory != null && aTransSize.SizeCodeProductCategory != string.Empty)
                        {
                            if (aTransSize.SizeCodeProductCategory != hdrSizeCodeProf.SizeCodeProductCategory)
                            {
                                return true;
                            }
                        }

                        break;
                    }
                }

                if (!sizeMatch)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }

            return isModified;
        }

        /// <summary>
        /// Checks to see if any current packs are no longer needed and updates the transaction to remove them.
        /// </summary>
        /// <returns></returns>
        private void IsModifyingPacksToZero()
        {
            try
            {
                if (_allocationHeaderProfile.Packs == null)
                {
                    return;
                }

                foreach (HeaderPackProfile aHdrPack in _allocationHeaderProfile.Packs.Values)
                {
                    bool packFound = false;
                    if (_transactionHeader.Pack != null)
                    {
                        foreach (HeadersHeaderPack aTransPack in _transactionHeader.Pack)
                        {
                            if (aTransPack.Name == aHdrPack.HeaderPackName)
                            {
                                packFound = true;
                                continue;
                            }
                        }
                        if (!packFound)
                        {
                            AddZeroPackToTransaction(aHdrPack);
                        }
                    }
                    else
                    {
                        AddZeroPackToTransaction(aHdrPack);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the current transaction by adding a zero pack that was found in the system, but is no longer needed.
        /// </summary>
        /// <param name="aHdrPack"></param>
        private void AddZeroPackToTransaction(HeaderPackProfile aHdrPack)
        {
            // Copy current transaction packs to List<>.
            List<HeadersHeaderPack> transPacks = new List<HeadersHeaderPack>();
            if (_transactionHeader.Pack != null)
            {
                foreach (HeadersHeaderPack aTransPack in _transactionHeader.Pack)
                {
                    transPacks.Add(aTransPack);
                }
            }

            // Add new pack to List<>
            HeadersHeaderPack aNewTransPack = new HeadersHeaderPack();
            aNewTransPack.Name = aHdrPack.HeaderPackName;
            aNewTransPack.Packs = 0;
            aNewTransPack.Multiple = 0;
            aNewTransPack.IsGeneric = aHdrPack.GenericInd;
            aNewTransPack.IsGenericSpecified = true;
            transPacks.Add(aNewTransPack);

            // No need to zero out colors and sizes. Removing Pack will remove other components
            //if (aHdrPack.Colors != null)
            //{
            //    foreach (HeaderPackColorProfile aHdrPackColor in aHdrPack.Colors.Values)
            //    {
            //        if (aHdrPackColor.Key == Include.DummyColorRID)
            //        {
            //            if (aHdrPackColor.Sizes != null)
            //            {
            //                // Compare Pack / Size
            //                foreach (HeaderPackColorSizeProfile aHdrPackColorSize in aHdrPackColor.Sizes.Values)
            //                {
            //                    // add zero sizes to pack
            //                }
            //            }
            //        }
            //        else
            //        {
            //            AddZeroPackColorToTransaction(aNewTransPack, aHdrPackColor);
            //        }
            //    }
            //}

            // Replace transaction packs with array that contains added pack
            _transactionHeader.Pack = transPacks.ToArray();
        }

        /// <summary>
        /// Updates the current transaction by adding a zero pack-color that was found in the system, but is no longer needed.
        /// </summary>
        /// <param name="aTransPack"></param>
        /// <param name="aHdrPackColor"></param>
        private void AddZeroPackColorToTransaction(HeadersHeaderPack aTransPack, HeaderPackColorProfile aHdrPackColor)
        {
            // Copy current transaction pack colors to List<>.
            List<HeadersHeaderPackPackColor> transPackColors = new List<HeadersHeaderPackPackColor>();
            if (aTransPack.PackColor != null)
            {
                foreach (HeadersHeaderPackPackColor aTransPackColor in aTransPack.PackColor)
                {
                    transPackColors.Add(aTransPackColor);
                }
            }

            // Add new pack color to List<>
            HeadersHeaderPackPackColor aNewTransPackColor = new HeadersHeaderPackPackColor();
            ColorCodeProfile colorCodeProfile = _SAB.HierarchyServerSession.GetColorCodeProfile(aHdrPackColor.Key);
            aNewTransPackColor.ColorCodeID = colorCodeProfile.ColorCodeID;
            aNewTransPackColor.ColorCodeName = colorCodeProfile.ColorCodeName;
            aNewTransPackColor.ColorCodeDescription = aHdrPackColor.ColorDescription;
            //aNewTransPackColor.ColorCodeGroup = aHdrPackColor.;
            aNewTransPackColor.Units = 0;
            transPackColors.Add(aNewTransPackColor);

            if (aHdrPackColor.Sizes != null)
            {
                foreach (HeaderPackColorSizeProfile aHdrPackColorSize in aHdrPackColor.Sizes.Values)
                {
                    AddZeroPackColorSizeToTransaction(aNewTransPackColor, aHdrPackColorSize);                   
                }
            }

            // Replace transaction pack colors with array that contains added color
            aTransPack.PackColor = transPackColors.ToArray();
        }

        /// <summary>
        /// Checks to see if any current bulk color sizes are no longer needed and updates the transaction to remove them.
        /// </summary>
        private void IsModifyingBulkColorSizeToZero()
        {
            try
            {
                if (_allocationHeaderProfile.BulkColors == null)
                {
                    return;
                }

                foreach (HeaderBulkColorProfile aHdrBulkColor in _allocationHeaderProfile.BulkColors.Values)
                {
                    ColorCodeProfile colorCodeProfile = _SAB.HierarchyServerSession.GetColorCodeProfile(aHdrBulkColor.Key);
                    if (_transactionHeader.BulkColor != null)
                    {
                        foreach (HeadersHeaderBulkColor aTransBulkColor in _transactionHeader.BulkColor)
                        {
                            if (aTransBulkColor.ColorCodeID == colorCodeProfile.ColorCodeID)
                            {
                                // Check sizes on color
                                foreach (HeaderBulkColorSizeProfile aHdrSize in aHdrBulkColor.Sizes.Values)
                                {
                                    SizeCodeProfile hdrSizeCodeProf = _SAB.HierarchyServerSession.GetSizeCodeProfile(aHdrSize.Key);
                                    bool sizeFound = false;
                                    if (aTransBulkColor.BulkColorSize != null)
                                    {
                                        foreach (HeadersHeaderBulkColorBulkColorSize aTransSize in aTransBulkColor.BulkColorSize)
                                        {
                                            if (aTransSize.SizeCodeID == hdrSizeCodeProf.SizeCodeID)
                                            {
                                                sizeFound = true;
                                                continue;
                                            }
                                        }
                                        if (!sizeFound)
                                        {
                                            AddZeroBulkSizeToTransaction(aTransBulkColor, aHdrSize);
                                        }
                                    }
                                    else
                                    {
                                        AddZeroBulkSizeToTransaction(aTransBulkColor, aHdrSize);

                                    }
                                }
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

        /// <summary>
        /// Updates the current transaction by adding a zero bulk zise that was found in the system, but is no longer needed.
        /// </summary>
        /// <param name="aTransBulkColor"></param>
        /// <param name="aHdrSize"></param>
        private void AddZeroBulkSizeToTransaction(HeadersHeaderBulkColor aTransBulkColor, HeaderBulkColorSizeProfile aHdrSize)
        {
            // Copy current transaction color sizes to List<>.
            List<HeadersHeaderBulkColorBulkColorSize> transBulkColorSizes = new List<HeadersHeaderBulkColorBulkColorSize>();
            if (aTransBulkColor.BulkColorSize != null)
            {
                foreach (HeadersHeaderBulkColorBulkColorSize aTransBulkColorSize in aTransBulkColor.BulkColorSize)
                {
                    transBulkColorSizes.Add(aTransBulkColorSize);
                }
            }

            // Add new pack color to List<>
            HeadersHeaderBulkColorBulkColorSize aNewTransBulkColorSize = new HeadersHeaderBulkColorBulkColorSize();
            SizeCodeProfile hdrSizeCodeProf = _SAB.HierarchyServerSession.GetSizeCodeProfile(aHdrSize.Key);
            aNewTransBulkColorSize.SizeCodeID = hdrSizeCodeProf.SizeCodeID;
            //aNewTransBulkColorSize.SizeCodeName = hdrSizeCodeProf.SizeCodeName;
            //aNewTransBulkColorSize.SizeCodeDescription = "";
            //aNewTransBulkColorSize.SizeCodePrimary = hdrSizeCodeProf.SizeCodePrimary;
            //aNewTransBulkColorSize.SizeCodeSecondary = hdrSizeCodeProf.SizeCodeSecondary;
            //aNewTransBulkColorSize.SizeCodeProductCategory = hdrSizeCodeProf.SizeCodeProductCategory;
            aNewTransBulkColorSize.Units = 0;
            transBulkColorSizes.Add(aNewTransBulkColorSize);

            // Replace transaction bulk color sizes with array that contains added color
            aTransBulkColor.BulkColorSize = transBulkColorSizes.ToArray();
        }

        /// <summary>
        /// Checks to see if any current pack-color-sizes or pack-sizes are no longer needed and updates the transaction to remove them.
        /// </summary>
        private void IsModifyingPackColorSizeToZero()
        {
            try
            {
                if (_allocationHeaderProfile.Packs == null)
                {
                    return;
                }

                // HEADER PACK
                foreach (HeaderPackProfile aHdrPack in _allocationHeaderProfile.Packs.Values)
                {
                    if (aHdrPack.Colors == null)
                    {
                        return;
                    }

                    // TRANS PACK
                    foreach (HeadersHeaderPack aTransPack in _transactionHeader.Pack)
                    {
                        if (aHdrPack.HeaderPackName == aTransPack.Name)
                        {
                            // HEADER PACK-COLOR
                            foreach (HeaderPackColorProfile aHdrPackColor in aHdrPack.Colors.Values)
                            {
                                // DUMMY COLOR
                                if (aHdrPackColor.Key == Include.DummyColorRID)
                                {
                                    ColorCodeProfile colorCodeProfile = _SAB.HierarchyServerSession.GetColorCodeProfile(aHdrPackColor.Key);
                                    // HEADER PACK-COLOR-SIZES
                                    foreach (HeaderPackColorSizeProfile aHdrPackColorSize in aHdrPackColor.Sizes.Values)
                                    {
                                        SizeCodeProfile hdrSizeCodeProf = _SAB.HierarchyServerSession.GetSizeCodeProfile(aHdrPackColorSize.Key);
                                        bool sizeFound = false;
                                        if (aTransPack.PackSize != null)
                                        {
                                            // TRANS PACK-SIZES
                                            foreach (HeadersHeaderPackPackSize aTransPackSize in aTransPack.PackSize)
                                            {
                                                if (aTransPackSize.SizeCodeID == hdrSizeCodeProf.SizeCodeID)
                                                {
                                                    sizeFound = true;
                                                    continue;
                                                }
                                            }
                                            if (!sizeFound)
                                            {
                                                AddZeroPackSizeToTransaction(aTransPack, aHdrPackColorSize);
                                            }
                                        }
                                        else
                                        {
                                            AddZeroPackSizeToTransaction(aTransPack, aHdrPackColorSize);

                                        }
                                    }
                                }
                                else
                                {
                                    ColorCodeProfile colorCodeProfile = _SAB.HierarchyServerSession.GetColorCodeProfile(aHdrPackColor.Key);
                                    if (aTransPack.PackColor != null)
                                    {
                                        // TRANS PACK-COLOR
                                        foreach (HeadersHeaderPackPackColor aTransPackColor in aTransPack.PackColor)
                                        {
                                            if (aTransPackColor.ColorCodeID == colorCodeProfile.ColorCodeID)
                                            {
                                                // HEADER PACK-COLOR-SIZES
                                                foreach (HeaderPackColorSizeProfile aHdrPackColorSize in aHdrPackColor.Sizes.Values)
                                                {
                                                    SizeCodeProfile hdrSizeCodeProf = _SAB.HierarchyServerSession.GetSizeCodeProfile(aHdrPackColorSize.Key);
                                                    bool sizeFound = false;
                                                    if (aTransPackColor.PackColorSize != null)
                                                    {
                                                        // TRANS PACK-COLOR-SIZES
                                                        foreach (HeadersHeaderPackPackColorPackColorSize aTransPackColorSize in aTransPackColor.PackColorSize)
                                                        {
                                                            if (aTransPackColorSize.SizeCodeID == hdrSizeCodeProf.SizeCodeID)
                                                            {
                                                                sizeFound = true;
                                                                continue;
                                                            }
                                                        }
                                                        if (!sizeFound)
                                                        {
                                                            AddZeroPackColorSizeToTransaction(aTransPackColor, aHdrPackColorSize);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        AddZeroPackColorSizeToTransaction(aTransPackColor, aHdrPackColorSize);

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
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

        /// <summary>
        /// Updates the current transaction by adding a zero pack-color-size that was found in the system, but is not longer needed.
        /// </summary>
        /// <param name="aTransPackColor"></param>
        /// <param name="aHdrPackColorSize"></param>
        private void AddZeroPackColorSizeToTransaction(HeadersHeaderPackPackColor aTransPackColor, HeaderPackColorSizeProfile aHdrPackColorSize)
        {
            // Copy current transaction pack color sizes to List<>.
            List<HeadersHeaderPackPackColorPackColorSize> transPackColorSizes = new List<HeadersHeaderPackPackColorPackColorSize>();
            if (aTransPackColor.PackColorSize != null)
            {
                foreach (HeadersHeaderPackPackColorPackColorSize aTransPackColorSize in aTransPackColor.PackColorSize)
                {
                    transPackColorSizes.Add(aTransPackColorSize);
                }
            }

            // Add new pack color size to List<>
            HeadersHeaderPackPackColorPackColorSize aNewTransPackColorSize = new HeadersHeaderPackPackColorPackColorSize();
            SizeCodeProfile hdrSizeCodeProf = _SAB.HierarchyServerSession.GetSizeCodeProfile(aHdrPackColorSize.Key);
            aNewTransPackColorSize.SizeCodeID = hdrSizeCodeProf.SizeCodeID;
            //aNewTransPackColorSize.SizeCodeName = hdrSizeCodeProf.SizeCodeName;
            //aNewTransPackColorSize.SizeCodeDescription = "";
            //aNewTransPackColorSize.SizeCodePrimary = hdrSizeCodeProf.SizeCodePrimary;
            //aNewTransPackColorSize.SizeCodeSecondary = hdrSizeCodeProf.SizeCodeSecondary;
            //aNewTransPackColorSize.SizeCodeProductCategory = hdrSizeCodeProf.SizeCodeProductCategory;
            aNewTransPackColorSize.Units = 0;
            transPackColorSizes.Add(aNewTransPackColorSize);

            // Replace transaction pack color sizes with array that contains added size
            aTransPackColor.PackColorSize = transPackColorSizes.ToArray();
        }

        /// <summary>
        /// Updates the current transaction by adding a zero pack-size that was found in the system, but is not longer needed.
        /// </summary>
        /// <param name="aTransPack"></param>
        /// <param name="aHdrPackColorSize"></param>
        private void AddZeroPackSizeToTransaction(HeadersHeaderPack aTransPack, HeaderPackColorSizeProfile aHdrPackColorSize)
        {
            // Copy current transaction pack sizes to List<>.
            List<HeadersHeaderPackPackSize> transPackSizes = new List<HeadersHeaderPackPackSize>();
            if (aTransPack.PackSize != null)
            {
                foreach (HeadersHeaderPackPackSize aTransPackSize in aTransPack.PackSize)
                {
                    transPackSizes.Add(aTransPackSize);
                }
            }

            // Add new pack size to List<>
            HeadersHeaderPackPackSize aNewTransPackSize = new HeadersHeaderPackPackSize();
            SizeCodeProfile hdrSizeCodeProf = _SAB.HierarchyServerSession.GetSizeCodeProfile(aHdrPackColorSize.Key);
            aNewTransPackSize.SizeCodeID = hdrSizeCodeProf.SizeCodeID;
            //aNewTransPackSize.SizeCodeName = hdrSizeCodeProf.SizeCodeName;
            //aNewTransPackSize.SizeCodeDescription = "";
            //aNewTransPackSize.SizeCodePrimary = hdrSizeCodeProf.SizeCodePrimary;
            //aNewTransPackSize.SizeCodeSecondary = hdrSizeCodeProf.SizeCodeSecondary;
            //aNewTransPackSize.SizeCodeProductCategory = hdrSizeCodeProf.SizeCodeProductCategory;
            aNewTransPackSize.Units = 0;
            transPackSizes.Add(aNewTransPackSize);

            // Replace transaction pack sizes with array that contains added size
            aTransPack.PackSize = transPackSizes.ToArray();

        }

        private bool IsModifyingPack(HeaderPackProfile aHdrPack)
        {
            bool isModified = false;
            bool packMatch = false;

            try
            {
                foreach (HeadersHeaderPack aTransPack in _transactionHeader.Pack)
                {
                    if (aTransPack.Name == aHdrPack.HeaderPackName)
                    {
                        packMatch = true;
                        if (aTransPack.Packs != aHdrPack.Packs)
                        {
                            return true;
                        }
                        if (aTransPack.Multiple != aHdrPack.Multiple)
                        {
                            return true;
                        }
                        if (aTransPack.IsGenericSpecified && aTransPack.IsGeneric != aHdrPack.GenericInd)
                        {
                            return true;
                        }

                        // Compare Pack / Color
                        foreach ( HeaderPackColorProfile aHdrPackColor in aHdrPack.Colors.Values)
                        {
                            if (aHdrPackColor.Key == Include.DummyColorRID)
                            {
                                if (aHdrPackColor.Sizes != null)
                                {
                                    // Compare Pack / Size
                                    foreach (HeaderPackColorSizeProfile aHdrPackColorSize in aHdrPackColor.Sizes.Values)
                                    {
                                        isModified = IsModifyingPackSize(aTransPack, aHdrPackColorSize);
                                        if (isModified)
                                        {
                                            break;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                isModified = IsModifyingPackColor(aTransPack, aHdrPackColor);
                                if (isModified)
                                {
                                    break;
                                }
                            }
                        }
                        


                        break;
                    }
                }

                if (!packMatch)
                {
                    return true;
                }

            }
            catch
            {
                throw;
            }
            finally
            {

            }

            return isModified;
        }


        private bool IsModifyingPackColor(HeadersHeaderPack aTransPack, HeaderPackColorProfile aHdrPackColor)
        {
            bool isModified = false;
            bool colorMatch = false;
            ColorCodeProfile colorCodeProfile = _SAB.HierarchyServerSession.GetColorCodeProfile(aHdrPackColor.Key);
            if (colorCodeProfile.Key == Include.UndefinedColor)
            {
                return true;
            }

            try
            {
                foreach (HeadersHeaderPackPackColor aTransPackColor in aTransPack.PackColor)
                {
                    if (aTransPackColor.ColorCodeID == colorCodeProfile.ColorCodeID)
                    {
                        colorMatch = true;
                        if (aTransPackColor.Units != aHdrPackColor.Units)
                        {
                            return true;
                        }

                        // Compare Pack / Color / Sizes
                        foreach (HeaderPackColorSizeProfile aHdrPackColorSize in aHdrPackColor.Sizes.Values)
                        {
                            isModified = IsModifyingPackColorSize(aTransPackColor, aHdrPackColorSize);
                            if (isModified)
                            {
                                break;
                            }
                        }

                        break;
                    }
                }

                if (!colorMatch)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }

            return isModified;
        }


        private bool IsModifyingPackColorSize(HeadersHeaderPackPackColor aTransPackColor, HeaderPackColorSizeProfile aHdrPackColorSize)
        {
            bool isModified = false;
            bool sizeMatch = false;
            SizeCodeProfile hdrPackColorSizeCodeProf = _SAB.HierarchyServerSession.GetSizeCodeProfile(aHdrPackColorSize.Key);
            if (hdrPackColorSizeCodeProf.Key == Include.NoRID)
            {
                return true;
            }

            try
            {
                foreach (HeadersHeaderPackPackColorPackColorSize aTransSize in aTransPackColor.PackColorSize)
                {
                    if (aTransSize.SizeCodeID == hdrPackColorSizeCodeProf.SizeCodeID)
                    {
                        sizeMatch = true;
                        if (aTransSize.Units != aHdrPackColorSize.Units)
                        {
                            return true;
                        }

                        //if (aTransSize.SizeCodeName != null && aTransSize.SizeCodeName != string.Empty)
                        //{
                        //    if (aTransSize.SizeCodeName != hdrPackColorSizeCodeProf.SizeCodeName)
                        //    {
                        //        return true;
                        //    }
                        //}
                        //if (aTransSize.SizeCodePrimary != null && aTransSize.SizeCodePrimary != string.Empty)
                        //{
                        //    if (aTransSize.SizeCodePrimary != hdrPackColorSizeCodeProf.SizeCodePrimary)
                        //    {
                        //        return true;
                        //    }
                        //}
                        //if (aTransSize.SizeCodeSecondary != null && aTransSize.SizeCodeSecondary != string.Empty)
                        //{
                        //    if (aTransSize.SizeCodeSecondary != hdrPackColorSizeCodeProf.SizeCodeSecondary)
                        //    {
                        //        return true;
                        //    }
                        //}
                        //if (aTransSize.SizeCodeProductCategory != null && aTransSize.SizeCodeProductCategory != string.Empty)
                        //{
                        //    if (aTransSize.SizeCodeProductCategory != hdrPackColorSizeCodeProf.SizeCodeProductCategory)
                        //    {
                        //        return true;
                        //    }
                        //}

                        break;
                    }
                }

                if (!sizeMatch)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }

            return isModified;
        }

        private bool IsModifyingPackSize(HeadersHeaderPack aTransPack, HeaderPackColorSizeProfile aHdrPackColorSize)
        {
            bool isModified = false;
            bool sizeMatch = false;
            SizeCodeProfile hdrPackColorSizeCodeProf = _SAB.HierarchyServerSession.GetSizeCodeProfile(aHdrPackColorSize.Key);
            if (hdrPackColorSizeCodeProf.Key == Include.NoRID)
            {
                return true;
            }

            try
            {
                foreach (HeadersHeaderPackPackSize aTransPackSize in aTransPack.PackSize)
                {
                    if (aTransPackSize.SizeCodeID == hdrPackColorSizeCodeProf.SizeCodeID)
                    {
                        sizeMatch = true;
                        if (aTransPackSize.Units != aHdrPackColorSize.Units)
                        {
                            return true;
                        }

                        //if (aTransPackSize.SizeCodeName != null && aTransPackSize.SizeCodeName != string.Empty)
                        //{
                        //    if (aTransPackSize.SizeCodeName != hdrPackColorSizeCodeProf.SizeCodeName)
                        //    {
                        //        return true;
                        //    }
                        //}
                        //if (aTransPackSize.SizeCodePrimary != null && aTransPackSize.SizeCodePrimary != string.Empty)
                        //{
                        //    if (aTransPackSize.SizeCodePrimary != hdrPackColorSizeCodeProf.SizeCodePrimary)
                        //    {
                        //        return true;
                        //    }
                        //}
                        //if (aTransPackSize.SizeCodeSecondary != null && aTransPackSize.SizeCodeSecondary != string.Empty)
                        //{
                        //    if (aTransPackSize.SizeCodeSecondary != hdrPackColorSizeCodeProf.SizeCodeSecondary)
                        //    {
                        //        return true;
                        //    }
                        //}
                        //if (aTransPackSize.SizeCodeProductCategory != null && aTransPackSize.SizeCodeProductCategory != string.Empty)
                        //{
                        //    if (aTransPackSize.SizeCodeProductCategory != hdrPackColorSizeCodeProf.SizeCodeProductCategory)
                        //    {
                        //        return true;
                        //    }
                        //}

                        break;
                    }
                }

                if (!sizeMatch)
                {
                    return true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }

            return isModified;
        }



        private bool IsModifyingCharacteristicInfo()
        {
            bool isModified = false;

            try
            {
                // Begin TT#4696 - stodd - Matching Headers with no changes are being passed through to the Header Load
                if (_transactionHeader.Characteristic != null)
                {
                    foreach (HeadersHeaderCharacteristic aTransChar in _transactionHeader.Characteristic)
                    {
                        bool charFound = false;
                        // Try to find the characteristic in the allocation header
                        foreach (HeaderCharProfile aHeaderChar in _allocationHeaderProfile.Characteristics)
                        {
                            int aTransCharGroupRid = GetHeaderCharacteristicGroupRid(aTransChar.Name);
                            if (aTransCharGroupRid == aHeaderChar.Key)
                            {
                                charFound = true;
                                string charValue = string.Empty;

                                if (aTransChar.Value != null)
                                {
                                    switch (aHeaderChar.HeaderCharType)
                                    {
                                        case eHeaderCharType.text:
                                            {
                                                charValue = aHeaderChar.TextValue;
                                                if (aTransChar.Value != charValue)
                                                {
                                                    return true;
                                                }
                                                break;
                                            }
                                        case eHeaderCharType.number:
                                            {
                                                charValue = aHeaderChar.NumberValue.ToString();
                                                if (aTransChar.Value != charValue)
                                                {
                                                    return true;
                                                }
                                                break;
                                            }
                                        case eHeaderCharType.dollar:
                                            {
                                                charValue = aHeaderChar.DollarValue.ToString();
                                                if (aTransChar.Value != charValue)
                                                {
                                                    return true;
                                                }
                                                break;
                                            }
                                        case eHeaderCharType.date:
                                            {
                                                try
                                                {
                                                    DateTime tranDate = DateTime.Parse(aTransChar.Value);
                                                    if (tranDate != aHeaderChar.DateValue)
                                                    {
                                                        return true;
                                                    }
                                                }
                                                catch
                                                {
                                                    return true;
                                                }
                                                break;
                                            }
                                    }
                                }
                            }
                            if (charFound)
                            {
                                break;
                            }
                        }

                        if (!charFound)
                        {
                            return true;
                        }
                    }
                }
                // End TT#4696 - stodd - Matching Headers with no changes are being passed through to the Header Load
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return isModified;
        }

		// Begin TT#4696 - stodd - Matching Headers with no changes are being passed through to the Header Load
        private int GetHeaderCharacteristicGroupRid(string aHeaderCharGroupName)
        {
            int hdrCharGroupRid = Include.NoRID;

            try
            {
                DataTable HdrCharGrpTable = HeaderCharacteristicsData.HeaderCharGroup_Read(aHeaderCharGroupName);
                if (HdrCharGrpTable.Rows.Count > 0)
                {
                    hdrCharGroupRid = int.Parse(HdrCharGrpTable.Rows[0]["HCG_RID"].ToString());
                }
                else
                {
                    hdrCharGroupRid = Include.UndefinedHeaderGroupRID;
                }
            }
            catch
            {
                throw;
            }

            return hdrCharGroupRid;
        }
		// End TT#4696 - stodd - Matching Headers with no changes are being passed through to the Header Load


    }
}