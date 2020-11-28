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

namespace Logility.ROWeb
{
    public partial class ROAllocation : ROWebFunction
    {

        #region Bulk Color
        protected bool AddBulkColor(AllocationProfile ap, int colorCodeKey, ref string errorMessage)
        {
            errorMessage = null;
            ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorCodeKey);
            return UpdateColorValue(eProfileType.HeaderBulkColor, ap, ccp, ref errorMessage);
        }

        protected bool UpdateColorValue(eProfileType dataType, AllocationProfile ap, ColorCodeProfile ccp, ref string errorMessage)
        {
            bool validColor = false;
            try
            {
                if (ccp == null
                    || ccp.Key == Include.NoRID)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorRequired);
                    return false;
                }

                switch (dataType)
                {
                    case eProfileType.HeaderBulkColor:
                        validColor = ValidBulkColor(ap, ccp, ref errorMessage);
                        if (validColor)
                        {
                            UpdateBulkColorData(ap: ap, ccp: ccp, ccpOrig: null, colorDescription: null, errorMessage: ref errorMessage);
                        }
                        break;

                    case eProfileType.HeaderPackColor:
                        validColor = ValidPackColor(ap, ccp, ref errorMessage);
                        if (validColor)
                        {
                            
                        }
                        break;
                }
            }
            catch
            {
                throw;
            }

            return validColor;
        }

        private bool ValidBulkColor(AllocationProfile ap, ColorCodeProfile ccp, ref string errorMessage)
        {
            bool errorFound = false;
            try
            {
                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                {
                    if (hcb.ColorCodeRID == ccp.Key)
                    {
                        errorFound = true;
                        break;
                    }
                }

                if (errorFound)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_DuplicateBulkColorNotAllowed);
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidPackColor(AllocationProfile ap, ColorCodeProfile ccp, ref string errorMessage)
        {
            bool errorFound = false;
            try
            {
                //UltraGridRow row = gridCell.Row.GetSibling(SiblingRow.First, false);
                //while (row != null)
                //{
                //    // BEGIN MID Track #6127 - ComponentOne Case Insensitve issue: prohibit duplicate name regardless of case
                //    //if (row != gridCell.Row &&
                //    //    Convert.ToString(row.Cells["PackColor"].Value, CultureInfo.CurrentUICulture) == aPackColor)
                //    string packColor = Convert.ToString(row.Cells["PackColor"].Value, CultureInfo.CurrentUICulture);
                //    if (row != gridCell.Row && packColor.ToUpper().Trim() == aPackColor.ToUpper().Trim())
                //    // END MID Track #6127
                //    {
                //        errorFound = true;
                //        break;
                //    }
                //    row = row.GetSibling(SiblingRow.Next, false);
                //}
                if (errorFound)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_DuplicateColorInPackNotAllowed);
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool UpdateBulkColorData(AllocationProfile ap, ColorCodeProfile ccp, ColorCodeProfile ccpOrig, string colorDescription, ref string errorMessage)
        {
            try
            {
                int colorHnRID;
                EditMsgs em;
                int headerRID, prevColorCodeRID = Include.NoRID;
                headerRID = ap.Key;

                //AllocationProfile ap = GetAllocationProfile(headerRID);

                string bulkColor = ccp.ColorCodeID;
                string bulkDescription = ccp.ColorCodeName;

                if (ccpOrig != null)
                {
                    prevColorCodeRID = ccpOrig.Key;
                }

                HdrColorBin currentBulkColor = null;
                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                {
                    if (hcb.ColorCodeRID == ccp.Key)
                    {
                        currentBulkColor = hcb;
                        break;
                    }
                }

                //ccp = SAB.HierarchyServerSession.GetColorCodeProfile(bulkColor);

                bool bulkColorColumnChanged = ccpOrig != null && ccpOrig.Key != ccp.Key;

                //case "BulkColor":
                string origValue = null;
                if (ccpOrig != null)
                {
                    origValue = ccpOrig.ColorCodeName;
                }
                if (ccp.Key == Include.NoRID)
                {
                    // color doesn't exist, need to add it
                    ccp.ColorCodeID = bulkColor;
                    ccp.ColorCodeName = bulkColor;
                    bulkDescription = bulkColor;

                    if (!ccp.VirtualInd)
                    {
                        ccp.ColorCodeChangeType = eChangeType.add;
                        ccp = SAB.HierarchyServerSession.ColorCodeUpdate(ccp);
                    }
                }
                else if (bulkDescription.Trim() == string.Empty || bulkColor != origValue)
                {
                    if (!ccp.VirtualInd)
                    {
                        colorHnRID = Include.NoRID;
                        HierarchyNodeProfile hnp_style = SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID);
                        if (SAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
                        {
                            HierarchyNodeProfile hnp_color = SAB.HierarchyServerSession.GetNodeData(colorHnRID);
                            bulkDescription = hnp_color.NodeDescription;
                        }
                        else
                        {
                            bulkDescription = ccp.ColorCodeName;
                        }
                    }
                    else
                    {
                        bulkDescription = ccp.ColorCodeName;
                    }
                }
                // now add it to the hierarchy
                if (bulkColor != string.Empty &&
                    bulkDescription != string.Empty && !ccp.VirtualInd)
                {
                    em = new EditMsgs();
                    colorHnRID = HierMaint.QuickAdd(ref em, ap.StyleHnRID, bulkColor, bulkDescription);
                    if (em.ErrorFound)
                    {
                        errorMessage = FormatMessage(em);
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMessage, this.GetType().Name);
                        return false;
                    }
                }

                if (ccpOrig == null)
                {
                    // finally, add it to the header

                    ap.AddBulkColor(ccp.Key, 0, 0);

                    if (ccp.VirtualInd)
                    {
                        ap.SetBulkColorName(ccp.Key, bulkColor);
                    }
                }
                else
                {
                    bool bypassChecks = true;
                    if (prevColorCodeRID == Include.NoRID)
                    {
                        //ccpOrig = SAB.HierarchyServerSession.GetColorCodeProfile(e.Cell.OriginalValue.ToString());
                        ap.SetBulkColorCodeRID(ccpOrig.Key, ccp.Key, bypassChecks);
                    }
                    else
                    {
                        ap.SetBulkColorCodeRID(prevColorCodeRID, ccp.Key, bypassChecks);
                    }

                    if (!ccp.VirtualInd)
                    {
                        ap.SetBulkColorName(ccp.Key, null);
                        ap.SetBulkColorDescription(ccp.Key, null);
                    }
                    else
                    {
                        ap.SetBulkColorName(ccp.Key, bulkColor);
                    }
                }

                //case "Description":
                errorMessage = string.Empty;

                if (ccp.Key != Include.NoRID)
                {
                    if (!ccp.VirtualInd)
                    {
                        em = new EditMsgs();
                        colorHnRID = Include.NoRID;
                        HierarchyNodeProfile hnp_style = SAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID);
                        if (SAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
                        {
                            HierarchyNodeProfile hnp_color = SAB.HierarchyServerSession.GetNodeData(colorHnRID);
                            hnp_color.NodeDescription = bulkDescription;
                            hnp_color.NodeChangeType = eChangeType.update;
                            HierMaint.ProcessNodeProfileInfo(ref em, hnp_color);
                        }
                        if (colorDescription != null
                         && bulkDescription.Trim() != ccp.ColorCodeName)
                        {
                            ccp.ColorCodeName = bulkDescription;
                            ccp.ColorCodeChangeType = eChangeType.update;
                            ccp = SAB.HierarchyServerSession.ColorCodeUpdate(ccp);
                        }
                    }
                    else       // Placeholder color
                    {
                        ap.SetBulkColorDescription(ccp.Key, bulkDescription);
                    }
                }
            }
            catch
            {
                throw;
            }

            return true;
        }

        protected bool DeleteBulkColor(AllocationProfile ap, int colorCodeKey, ref string errorMessage)
        {
            errorMessage = null;
            ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorCodeKey);

            return DeleteColorValue(eProfileType.HeaderBulkColor, ap, ccp, ref errorMessage);
        }

        protected bool DeleteColorValue(eProfileType dataType, AllocationProfile ap, ColorCodeProfile ccp, ref string errorMessage)
        {
            bool validColor = false;
            try
            {
                if (ccp == null
                    || ccp.Key == Include.NoRID)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorRequired);
                    return false;
                }

                switch (dataType)
                {
                    case eProfileType.HeaderBulkColor:
                        validColor = ValidBulkColorForDelete(ap, ccp, ref errorMessage);
                        if (validColor)
                        {
                            ap.SetColorUnitsAllocated(ccp.Key, 0);
                            ap.RemoveBulkColor(ccp.Key);
                        }
                        break;

                    case eProfileType.HeaderPackColor:

                        break;
                }
            }
            catch
            {
                throw;
            }

            return validColor;
        }
        private bool ValidBulkColorForDelete(AllocationProfile ap, ColorCodeProfile ccp, ref string errorMessage)
        {
            bool errorFound = true;
            try
            {
                foreach (HdrColorBin hcb in ap.BulkColors.Values)
                {
                    if (hcb.ColorCodeRID == ccp.Key)
                    {
                        errorFound = false;
                        break;
                    }
                }

                if (errorFound)
                {
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorNotDefinedInBulk);
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Bulk Color

        #region Bulk Sizes

        protected bool AddSizeGroupToBulkColor(AllocationProfile ap, int colorCodeKey, int sizeGroupKey, ref string message)
        {
            message = null;
            SizeGroupProfile sgp = null;
            SizeCodeList scl = null;

            if (colorCodeKey > 0)
            {
                if (sizeGroupKey > Include.UndefinedSizeGroupRID)
                {
                    if (ap.BulkColors.ContainsKey(colorCodeKey))
                    {
                        HdrColorBin hcb = (HdrColorBin)ap.BulkColors[colorCodeKey];
                        if (hcb.ColorSizes.Count == 0)
                        {
                            // load existing group
                            sgp = GetSizeGroup(sizeGroupKey);
                            scl = sgp.SizeCodeList;

                            foreach (SizeCodeProfile scp in scl.ArrayList)
                            {
                                ap.AddBulkSizeToColor(colorCodeKey, scp.Key, 0, -1);
                            }
                            return true;
                        }
                        else
                        {
                            message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_ColorAlreadyContainsSizes);
                        }
                    }
                    else
                    {
                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorNotDefinedInBulk);
                    }
                }
                else
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeGroupRequired);
                }
            }
            else
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorRequired);
            }

            return false;
        }

        protected bool DeleteSizesFromBulkColor(AllocationProfile ap, int colorCodeKey, ref string message)
        {
            message = null;
            int[] sizeKeys;

            if (colorCodeKey > 0)
            {
                sizeKeys = ap.GetBulkColorSizeCodeRIDs(colorCodeKey);
                for (int i = 0; i < sizeKeys.Length; i++)
                {
                    ap.RemoveBulkColorSize(colorCodeKey, sizeKeys[i]);
                }
                return true;
            }
            else
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorRequired);
            }

            return false;
        }

        #endregion Bulk Sizes
    }

}
