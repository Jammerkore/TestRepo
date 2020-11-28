using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Logility.ROWeb
{
    public partial class ROAssortment : ROAllocation
    {
        private AssortmentView _assortmentView = null;

        #region Add placeholder Styles
        private bool OkToAddPlaceholders(ref string message)
        {
            bool okToAdd = true;
            return okToAdd;
        }

        private void AddPlaceholders(AssortmentProfile asp, int aReqPhCount, int aStyleHnRID, bool addingHeaderToAssortment, ref string message)
        {
            bool isFirstHeader = false;
            try
            {
                int curPhCount = 0;

                string asrtID = asp.HeaderID;
                int asrtHdrRID = asp.HeaderRID;
                int anchorRID = asp.StyleHnRID;
                List<string> placeholderList = new List<string>();

                foreach (AllocationHeaderProfile ho in _asrtCubeGroup.GetHeaderList().ArrayList)
                {
                    if (ho.HeaderType == eHeaderType.Placeholder)
                    {
                        curPhCount++;
                        placeholderList.Add(ho.HeaderID);
                    }
                }

                if (aStyleHnRID == Include.NoRID)
                {
                    HierarchyNodeList hierNodeList = HierMaint.GetPlaceholderStyles(anchorRID, aReqPhCount, curPhCount, asrtHdrRID);

                    for (int i = 0; i < aReqPhCount; i++)
                    {
                        AddPlaceholderStyle(asp, (HierarchyNodeProfile)hierNodeList[i], asrtHdrRID, placeholderList);
                    }
                }
                else
                {
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(aStyleHnRID);
                    AddPlaceholderStyle(asp, hnp, asrtHdrRID, placeholderList);
                }

            }
            catch (Exception ex)
            {
                throw new MIDException(eErrorLevel.information, 0, ex.Message);
            }
            //finally
            //{

            //    if (!addingHeaderToAssortment)
            //    {
            //        OnAssortmentSaveHeaderData(null);
            //        //SaveAndUpdateAssortmentTab();                 
            //    }
            //}
        }

        private void AddPlaceholderStyle(AssortmentProfile asp, HierarchyNodeProfile aHnp, int aAsrtRID, List<string> placeholderList)
        {
            try
            {
                string phHeaderID = DeterminePlaceholderID(asp, aAsrtRID, placeholderList);

                int newKey = (_allocProfileList.MinValue > 0) ? -1 : _allocProfileList.MinValue - 1;  	// all new keys are negative
                AllocationProfile ap = new AllocationProfile(_applicationSessionTransaction, null, newKey, SAB.ClientServerSession);

                ap.HeaderDay = DateTime.Today;
                MIDException exception = null;
                ap.SetHeaderType(eHeaderType.Receipt, out exception);
                _allocProfileList.Add(ap);

                ap.HeaderID = phHeaderID;
                ap.StyleHnRID = aHnp.Key;
                ap.HeaderDescription = aHnp.NodeDescription;

                ap.ShipToDay = asp.AssortmentApplyToDate.Date;  
                ap.BeginDay = asp.AssortmentBeginDay.Date;

                HierarchyNodeProfile productHnp = SAB.HierarchyServerSession.GetNodeData(aHnp.HomeHierarchyParentRID);

                HierarchyNodeProfile anchorHnp = this.GetNodeData(asp.AssortmentAnchorNodeRid);
                ap.AsrtAnchorNodeRid = anchorHnp.Key;
                ap.AsrtRID = aAsrtRID;
                ap.SetHeaderType(eHeaderType.Placeholder, out exception);
                ap.PlanHnRID = asp.AssortmentAnchorNodeRid;

                ap.AsrtPlaceholderSeq = GetPlaceholderSeq(asp);

                ap.TotalUnitsToAllocate = 0;
                ap.DistributionCenter = string.Empty;
                ap.AllocationNotes = string.Empty;
                ap.Vendor = string.Empty;
                ap.PurchaseOrder = string.Empty;

                placeholderList.Add(ap.HeaderID);

            }
            catch
            {
                throw;
            }
        }

        private string DeterminePlaceholderID(AssortmentProfile asp, int aAsrtRID, List<string> placeholderList)
        {
            
            string phHeaderID = string.Empty;
            try
            {
                int placeholderCount = placeholderList.Count;
                placeholderCount++;
                phHeaderID = asp.HeaderID + " " + _lblPhStyle + " " + placeholderCount.ToString(CultureInfo.CurrentUICulture);
                while (placeholderList.Contains(phHeaderID))
                {
                    placeholderCount++;
                    phHeaderID = asp.HeaderID + " " + _lblPhStyle + " " + placeholderCount.ToString(CultureInfo.CurrentUICulture);
                }
                return phHeaderID;
            }
            catch
            {

                throw;
            }
        }

        private int GetPlaceholderSeq(AssortmentProfile asp)
        {
            try
            {
                int newSeq = 0;
                foreach (AllocationProfile ap in asp.AssortmentPlaceHolders)
                {
                    if (ap.AsrtPlaceholderSeq > newSeq)
                    {
                        newSeq = ap.AsrtPlaceholderSeq;
                    }
                }
                
                return newSeq + 1;
            }
            catch
            {
                throw;
            }
        }

        #endregion Add placeholder Styles

        #region Add placeholder Colors
        // from form_OnColorBrowserSelectHandler in AssortmentView.Extended.cs
        private bool AddPlaceholderColors(eProfileType dataType, AssortmentProfile asp, int aReqPhCount, AllocationProfile ap, ref string message)
        {
            bool canSpreadUnits = false;
            ArrayList colorCodeList;
            int hdrRID = Include.NoRID;
            ArrayList placeHolders = new ArrayList();

            canSpreadUnits = CanSpreadUnits(asp, ap);
            colorCodeList = new ArrayList();

            eHeaderType headerType = ap.HeaderType;
            hdrRID = ap.Key;
            

            if (aReqPhCount > 0)
            {
                if (headerType == eHeaderType.Assortment || headerType == eHeaderType.Placeholder)
                {
                    ArrayList colorRIDs = new ArrayList();

                    colorRIDs = LoadCurrentPlaceholderColors(ap);

                    ColorCodeList placeHolderColors = HierMaint.GetPlaceholderColors(aReqPhCount, colorRIDs);
                    foreach (ColorCodeProfile ccp in placeHolderColors)
                    {
                        placeHolders.Add(ccp);
                    }
                    _skipRowUpdate = true;
                    if (!AddPlaceholderColorRows(dataType, ap, colorRIDs, placeHolders, aReqPhCount, ref colorCodeList, ref message))
                    {
                        _skipRowUpdate = false;
                        return false;
                    }
                    _skipRowUpdate = false;
                }
                return true;
            }

            return false;

        }

        private bool AddPlaceholderColorRows(eProfileType dataType, AllocationProfile ap, ArrayList aExistingRIDs, ArrayList aPhColorList, int aRequestedCount, ref ArrayList aColorCodeList, ref string errorMessage)
        {
            bool okToContinue = true;
            try
            {
                for (int i = 0; i < aRequestedCount; i++)
                {
                    ColorCodeProfile ccp = null;
                    int colorCodeRID = Include.NoRID;
                    for (int j = 0; j < aPhColorList.Count; j++)
                    {
                        ccp = (ColorCodeProfile)aPhColorList[j];

                        if (!aExistingRIDs.Contains(ccp.Key))
                        {
                            aExistingRIDs.Add(ccp.Key);
                            colorCodeRID = ccp.Key;
                            break;
                        }
                    }
                    if (colorCodeRID == Include.NoRID)
                    {
                        errorMessage = "Not enough available Placeholder colors";
                        return false;
                    }

                    switch (dataType)
                    {
                        case eProfileType.AllocationHeader:
                        case eProfileType.HeaderBulkColor:
                            okToContinue = AddBulkColorRow(ap);
                            break;

                        case eProfileType.HeaderPack:
                        case eProfileType.HeaderPackColor:
                            okToContinue = AddPackColorRow(ap);
                            break;
                    }
                    if (okToContinue)
                    {
                        okToContinue = UpdateColorValue(dataType, ap, ccp, ref errorMessage);
                        if (okToContinue)
                        {
                            if (!aColorCodeList.Contains(colorCodeRID))
                            {
                                aColorCodeList.Add(colorCodeRID);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch
            {
                throw;
            }
            return okToContinue;
        }

        private bool CanSpreadUnits(AssortmentProfile asp, AllocationProfile ap)
        {
            bool canSpreadUnits = false;
            if (ap.BulkColorCount > 0)
            {
                canSpreadUnits = true;
            }

            return canSpreadUnits;
        }

        private ArrayList LoadCurrentPlaceholderColors(AllocationProfile ap)
        {
            try
            {
                ColorCodeProfile ccp;
                ArrayList colorRIDs = new ArrayList();
                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                {
                    ccp = SAB.HierarchyServerSession.GetColorCodeProfile(hcb.ColorCodeRID);
                    if (ccp.VirtualInd)
                    {
                        if (!colorRIDs.Contains(hcb.ColorCodeRID))
                        {
                            colorRIDs.Add(hcb.ColorCodeRID);
                        }
                    }
                }

                return colorRIDs;
            }
            catch
            {
                throw;
            }

        }

        private bool AddBulkColorRow(AllocationProfile ap)
        {
            bool addOK = true;
            try
            {
                int hdrRID = ap.Key;

                //UltraGridRow row = this.ugDetails.DisplayLayout.Bands["BulkColor"].AddNew();

                //if (row == null || row.Cells["ColorCodeRID"].Value != DBNull.Value)
                //{
                //    object[] Keys = new object[2];
                //    Keys[0] = hdrRID;
                //    Keys[1] = 0;
                //    DataRow drBulk = _dsDetails.Tables["BulkColor"].Rows.Find(Keys);
                //    if (drBulk != null)
                //    {
                //        return false;
                //    }
                //    DataRow newRow = _dsDetails.Tables["BulkColor"].NewRow();
                //    newRow["KeyH"] = hdrRID;
                //    newRow["KeyC"] = 0;
                //    newRow["ColorCodeRID"] = 0;
                //    newRow["BulkColor"] = string.Empty;
                //    _dsDetails.Tables["BulkColor"].Rows.Add(newRow);
                //    _dsDetails.Tables["BulkColor"].AcceptChanges();
                //    this.ugDetails.UpdateData();
                //    _dsDetails.Tables["Header"].AcceptChanges();

                //    row = ugDetails.ActiveRow.GetChild(ChildRow.First, this.ugDetails.DisplayLayout.Bands["BulkColor"]);
                //    while (row != null)
                //    {
                //        if ((int)row.Cells["KeyC"].Value == 0)
                //        {
                //            break;
                //        }
                //        row = row.GetSibling(SiblingRow.Next, false, false);
                //    }
                //}
                //row.Cells["KeyH"].Value = hdrRID;

                //if (_addingMTColorRow && !row.HasPrevSibling(false) && !row.HasNextSibling(false))
                //{
                //    row.Cells["Quantity"].Value = row.ParentRow.Cells["HdrQuantity"].Value;
                //}

                //CalculateBalances(eBalanceAction.RowAdded, row.Cells["Quantity"]);

            }
            catch
            {
                addOK = false;
                throw;
            }
            return addOK;
        }

        private bool AddPackColorRow(AllocationProfile ap)
        {
            bool addOK = true;

            try
            {
                //while (ugDetails.ActiveRow.Band.Key != "Pack")
                //{
                //    ugDetails.ActiveRow = this.ugDetails.ActiveRow.ParentRow;
                //}

                //int hdrRID = Convert.ToInt32(ugDetails.ActiveRow.Cells["KeyH"].Value, CultureInfo.CurrentUICulture);
                //int packRID = Convert.ToInt32(ugDetails.ActiveRow.Cells["KeyP"].Value, CultureInfo.CurrentUICulture);

                //UltraGridRow row = this.ugDetails.DisplayLayout.Bands["PackColor"].AddNew();

                //row.Cells["KeyH"].Value = hdrRID;
                //row.Cells["KeyP"].Value = packRID;

                //this.ugDetails.ActiveRow.Cells["PackColor"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
                //ugDetails.ActiveCell = row.Cells["PackColor"];
                //if (!_addingHeaderToAssortment)
                //{
                //    ugDetails.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
                //}
                //CalculateBalances(eBalanceAction.RowAdded, row.Cells["QuantityPerPack"]);
                //if (!_skipRowUpdate && !_addingHeaderToAssortment)
                //{
                //    row.Update();
                //}
                //_rClickRow = row;
            }
            catch
            {
                addOK = false;
                throw;
            }
            return addOK;
        }

        #endregion Add placeholder Colors

        #region Replace Bulk Colors
        private bool ReplaceBulkColor(AssortmentProfile asp, AllocationProfile ap, int colorKey, int newColorKey, ref string message)
        {
            try
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow colorRow = null;
                ColorCodeProfile ccp;
                HierarchyNodeProfile hnp;
                int colorHnRID = Include.NoRID;
                HierarchyNodeSecurityProfile securityNode;

                // verify current color code
                ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorKey);
                if (ccp == null
                    || ccp.Key == Include.NoRID)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_AppErrorColorNotFound);
                    return false;
                }

                // verify new color code
                ccp = SAB.HierarchyServerSession.GetColorCodeProfile(newColorKey);
                if (ccp == null
                    || ccp.Key == Include.NoRID)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_AppErrorColorNotFound);
                    return false;
                }

                hnp = SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID);

                if (SAB.HierarchyServerSession.ColorExistsForStyle(hnp.HomeHierarchyRID, hnp.Key, ccp.ColorCodeID, ref colorHnRID))
                {
                    securityNode = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(colorHnRID, (int)eSecurityTypes.Allocation);
                    if (!securityNode.AllowUpdate)
                    {
                        message = MIDText.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
                        return false;
                    }
                }
                else // if color is not already part of style, check the style security
                {
                    securityNode = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Allocation);
                    if (!securityNode.AllowUpdate)
                    {
                        message = MIDText.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
                        return false;
                    }
                }

                BuildAssortmentView();

                // find color row in header.
                UltraGridRow row = _assortmentView.UltraGridDetails.GetRow(ChildRow.First);
                while (row != null)
                {
                    if (ap.Key == Convert.ToInt32(row.Cells["KeyH"].Value))
                    {
                        foreach (UltraGridChildBand childBand in row.ChildBands)
                        {
                            if (childBand.Key == "BulkColor")
                            {
                                foreach (UltraGridRow childRow in childBand.Rows)
                                {
                                    if (colorKey == Convert.ToInt32(childRow.Cells["ColorCodeRID"].Value))
                                    {
                                        colorRow = childRow;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    row = row.GetSibling(SiblingRow.Next, false);
                }

                if (colorRow == null)
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorNotOnHeader, true);
                    return false;
                }

                _assortmentView.ColorDragDrop(colorRow, newColorKey);

                if (MIDEnvironment.requestFailed)
                {
                    if (message == null)
                    {
                        message = MIDEnvironment.Message;
                    }
                    return false;
                }
                _headerList = (AllocationHeaderProfileList)_asrtCubeGroup.GetHeaderList();
                _allocProfileList = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

                return true;

            }
            catch (MIDException ex)
            {
                message = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Replace Bulk Colors

        #region Add Headers
        private bool AddHeader(AssortmentProfile asp, List<int> headerKeys, ref string message)
        {
            try
            {
                BindingSource bindSourceHeader;
                Infragistics.Win.UltraWinGrid.UltraGrid ugHeaders;

                if (headerKeys.Count == 0)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_as_NoHeaderSelectedForAssrt);
                    return false;
                }

                if (_dtHeader == null)
                {
                    BuildDataSets();
                }

                foreach (int headerKey in headerKeys)
                {
                    if (headerKey <= 0)
                    {
                        message = MIDText.GetText(eMIDTextCode.msg_as_NoHeaderSelectedForAssrt);
                        return false;
                    }

                    AllocationProfile ap = _applicationSessionTransaction.GetAssortmentMemberProfile(headerKey);

                    if (ap != null)
                    {
                        message = string.Format(MIDText.GetText(eMIDTextCode.msg_as_DupPhStylesNotAllowed),
                                                         MIDText.GetTextOnly(eMIDTextCode.lbl_Header));
                        return false;
                    }

                    AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(headerKey, true, true, true);
                    if (ahp == null
                        || ahp.Key == Include.NoRID)
                    {
                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotFound, true);
                        return false;
                    }

                    // Use edit checks like in AssortmentView.Extended.cs method ugDetails_DragOver
                    if (ahp.HeaderGroupRID > Include.DefaultHeaderRID
                        || (ahp.HeaderType == eHeaderType.Assortment && ahp.AsrtType == (int)eAssortmentType.GroupAllocation)
                        || ahp.AsrtRID > Include.DefaultHeaderRID)
                    {
                        message = MIDText.GetTextOnly(eMIDTextCode.msg_as_HeaderBelongsToOtherAssortment);
                        message = message.Replace("{0}", ProcessName);
                        return false;
                    }
                    else if (!(ahp.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedInBalance || ahp.HeaderAllocationStatus == eHeaderAllocationStatus.AllocationStarted) ||
                        (ahp.HeaderType == eHeaderType.WorkupTotalBuy ||
                        ahp.HeaderType == eHeaderType.IMO ||
                        ahp.HeaderType == eHeaderType.MultiHeader)
                        )
                    {
                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_HeaderNotReceivedInBalance);
                        message = message.Replace("{0}", ProcessName);
                        return false;
                    }

                    LoadHeader(ahp: ahp, assortmentHeadersOnly: false);
                }

                BuildAssortmentView();

                // Build Infragistics selected list to emulate selecting from the Workspace
                bindSourceHeader = new BindingSource(_dsMain, "Header");

                WorkUltraGrid wg = new WorkUltraGrid();
                wg.BuildGrid(bindSourceHeader);
                ugHeaders = wg.Grid;

                UltraGridRow row = ugHeaders.GetRow(ChildRow.First);
                while (row != null)
                {
                    if (headerKeys.Contains(Convert.ToInt32(row.Cells["KeyH"].Value)))
                    {
                        row.Selected = true;
                    }
                    row = row.GetSibling(SiblingRow.Next, false);
                }

                if (ugHeaders.Selected.Rows.Count == 0)
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotFound, true);
                    return false;
                }

                _assortmentView.HeaderDragDrop(null, ugHeaders.Selected.Rows);

                if (MIDEnvironment.requestFailed)
                {
                    if (message == null)
                    {
                        message = MIDEnvironment.Message;
                    }
                    return false;
                }
                _headerList = (AllocationHeaderProfileList)_asrtCubeGroup.GetHeaderList();
                _allocProfileList = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

                return true;

            }
            catch (MIDException ex)
            {
                message = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Add Headers

        #region Update Headers
        private bool ReplaceHeader(AssortmentProfile asp, int headerKey, int newHeaderKey, ref string message)
        {
            try
            {
                BindingSource bindSourceHeader;
                Infragistics.Win.UltraWinGrid.UltraGrid ugHeaders;
                Infragistics.Win.UltraWinGrid.UltraGridRow headerRow = null;


                if (headerKey == 0
                    || newHeaderKey == 0)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_as_NoHeaderSelectedForAssrt);
                    return false;
                }

                if (_dtHeader == null)
                {
                    BuildDataSets();
                }

                if (headerKey <= 0)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_as_NoHeaderSelectedForAssrt);
                    return false;
                }

                AllocationProfile ap = _applicationSessionTransaction.GetAssortmentMemberProfile(newHeaderKey);

                if (ap != null)
                {
                    message = string.Format(MIDText.GetText(eMIDTextCode.msg_as_DupPhStylesNotAllowed),
                                                     MIDText.GetTextOnly(eMIDTextCode.lbl_Header));
                    return false;
                }

                AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(newHeaderKey, true, true, true);
                if (ahp == null
                    || ahp.Key == Include.NoRID)
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotFound, true);
                    return false;
                }

                // Use edit checks like in AssortmentView.Extended.cs method ugDetails_DragOver
                if (ahp.HeaderGroupRID > Include.DefaultHeaderRID
                    || (ahp.HeaderType == eHeaderType.Assortment && ahp.AsrtType == (int)eAssortmentType.GroupAllocation)
                    || ahp.AsrtRID > Include.DefaultHeaderRID)
                {
                    message = MIDText.GetTextOnly(eMIDTextCode.msg_as_HeaderBelongsToOtherAssortment);
                    message = message.Replace("{0}", ProcessName);
                    return false;
                }
                else if (!(ahp.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedInBalance || ahp.HeaderAllocationStatus == eHeaderAllocationStatus.AllocationStarted) ||
                    (ahp.HeaderType == eHeaderType.WorkupTotalBuy ||
                    ahp.HeaderType == eHeaderType.IMO ||
                    ahp.HeaderType == eHeaderType.MultiHeader)
                    )
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_HeaderNotReceivedInBalance);
                    message = message.Replace("{0}", ProcessName);
                    return false;
                }

                LoadHeader(ahp: ahp, assortmentHeadersOnly: false);

                BuildAssortmentView();

                // Build Infragistics selected list to emulate selecting from the Workspace
                bindSourceHeader = new BindingSource(_dsMain, "Header");

                WorkUltraGrid wg = new WorkUltraGrid();
                wg.BuildGrid(bindSourceHeader);
                ugHeaders = wg.Grid;

                UltraGridRow row = ugHeaders.GetRow(ChildRow.First);
                while (row != null)
                {
                    if (newHeaderKey == Convert.ToInt32(row.Cells["KeyH"].Value))
                    {
                        row.Selected = true;
                    }
                    row = row.GetSibling(SiblingRow.Next, false);
                }

                if (ugHeaders.Selected.Rows.Count == 0)
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotFound, true);
                    return false;
                }

                // Get header row from content grid
                row = _assortmentView.UltraGridDetails.GetRow(ChildRow.First);
                while (row != null)
                {
                    if (headerKey == Convert.ToInt32(row.Cells["KeyH"].Value))
                    {
                        headerRow = row;
                        break;
                    }
                    row = row.GetSibling(SiblingRow.Next, false);
                }

                if (headerRow == null)
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotFound, true);
                    return false;
                }

                _assortmentView.HeaderDragDrop(headerRow, ugHeaders.Selected.Rows);

                if (MIDEnvironment.requestFailed)
                {
                    if (message == null)
                    {
                        message = MIDEnvironment.Message;
                    }
                    return false;
                }
                _headerList = (AllocationHeaderProfileList)_asrtCubeGroup.GetHeaderList();
                _allocProfileList = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

                return true;

            }
            catch (MIDException ex)
            {
                message = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Update Headers

        private void BuildAssortmentView()
        {
            if (_assortmentView == null)
            {
                if (!_applicationSessionTransaction.AllocationCriteriaExists)
                {
                    _applicationSessionTransaction.CreateAllocationViewSelectionCriteria(useExistingAllocationProfileList: true);
                }
                _assortmentView = new MIDRetail.Windows.AssortmentView(null, _applicationSessionTransaction, eAssortmentWindowType.Assortment);
                _assortmentView.InitializeNonWindows(asrtCubeGroup: _asrtCubeGroup, viewKey: _applicationSessionTransaction.AssortmentViewRid);
                _assortmentView.LoadContentGrid();
            }
        }

        #region Delete Headers

        private bool DeleteHeader(AssortmentProfile asp, List<int> headerKeys, ref string message)
        {
            try
            {
                if (headerKeys.Count == 0)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_as_NoHeaderSelectedForAssrt);
                    return false;
                }

                foreach (int headerKey in headerKeys)
                {
                    if (headerKey <= 0)
                    {
                        message = MIDText.GetText(eMIDTextCode.msg_as_NoHeaderSelectedForAssrt);
                        return false;
                    }

                    AllocationProfile ap = _applicationSessionTransaction.GetAssortmentMemberProfile(headerKey);

                    if (ap == null)
                    {
                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotFound, true);
                        return false;
                    }
                }

                BuildAssortmentView();

                // find header row
                UltraGridRow row = _assortmentView.UltraGridDetails.GetRow(ChildRow.First);
                while (row != null)
                {
                    if (headerKeys.Contains(Convert.ToInt32(row.Cells["KeyH"].Value)))
                    {
                        row.Selected = true;
                    }
                    row = row.GetSibling(SiblingRow.Next, false);
                }

                if (_assortmentView.UltraGridDetails.Selected.Rows.Count == 0)
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeaderNotFound, true);
                    return false;
                }

                _assortmentView.RemoveFromAssortment();

                if (MIDEnvironment.requestFailed)
                {
                    if (message == null)
                    {
                        message = MIDEnvironment.Message;
                    }
                    return false;
                }
                _headerList = (AllocationHeaderProfileList)_asrtCubeGroup.GetHeaderList();
                _allocProfileList = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

                return true;

            }
            catch (MIDException ex)
            {
                message = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Delete Headers
    }

}
