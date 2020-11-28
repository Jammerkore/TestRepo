using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using System.IO;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Summary description for DCFulfillmentMonitor.
	/// </summary>
	public class DCFulfillmentMonitor : MIDLog
	{
		private ArrayList _miscMsgList;
		private SessionAddressBlock _sab;

		#region Properties
		//************
		// PROPERTIES
		//************

		#endregion

		//************
		// CONSTRUCTOR
		//************
        public DCFulfillmentMonitor(DCFulfillmentMethod DCFulfillmentMethod, SessionAddressBlock aSab, 
                               string filePrefix, string filePath, int userRid, string methodName, 
                               string qualifiedNodeID)
            : base(filePrefix, filePath, userRid, methodName,  qualifiedNodeID)    
		{
			_sab = aSab;

            string msgText = MIDText.GetText(eMIDTextCode.msg_DC_Fulfillment_Method_LogInformation);
            msgText = msgText.Replace("{0}", methodName);
            msgText = msgText.Replace("{1}", LogLocation);
            msgText = msgText.Replace("{2}", userRid.ToString());
            msgText = msgText.Replace("{3}", UserName);
            _sab.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msgText, this.ToString());

		}

		//*********
		// METHODS
		//*********
		

		/// <summary>
		/// Adds message to Misc Message List
		/// </summary>
		/// <param name="message"></param>
		public void AddMiscMessage(string message)
		{
			_miscMsgList.Add(message);
		}

        public void WriteOptions(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            DCFulfillmentMethod DCFulfillmentMethod,
            eDCFulfillmentSplitOption splitOption,
            bool bApplyMinimumsInd,
            eDCFulfillmentSplitByOption SplitByOption,
            eDCFulfillmentWithinDC WithinDC,
            eDCFulfillmentReserve SplitByReserve,
            eDCFulfillmentMinimums ApplyMinimums,
            MasterHeaderProfile masterHeader,
            List<AllocationProfile> subordinateHeaders,
            List<string> DCOrder,
            Dictionary<string, List<int>> DCStoreOrder,
            Dictionary<string, List<AllocationProfile>> DCHeaderOrder,
            ProfileList storeList
            )
        {
            string message;
            int count = 0;
            //string distributionCenter;

            SW.WriteLine(" ");
            message = "Method: " + DCFulfillmentMethod.Name + " (" + DCFulfillmentMethod.Key + ")" + Environment.NewLine;
            SW.WriteLine(message);

            //message = "Master Header: " + masterHeader.HeaderID + " (" + masterHeader.Key + ")";
            //SW.WriteLine(message);
            WriteHeaderValues("Master Header", masterHeader, true, aSAB, aApplicationTransaction, storeList);

            message = Environment.NewLine + "Subordinate Headers: " + Environment.NewLine;
            //foreach (AllocationProfile ap in subordinateHeaders)
            //{
            //    message += ap.HeaderID + " (" + ap.Key + ")" + Environment.NewLine;
            //}
            SW.WriteLine(message);

            foreach (AllocationProfile ap in subordinateHeaders)
            {
                WriteHeaderValues("Subordinater Header", ap, true, aSAB, aApplicationTransaction, storeList);
                SW.WriteLine(" ");
            }

            SW.WriteLine("Fulfillment Options");
            message = "Split Option: " + splitOption + Environment.NewLine;
            message += "Apply Minimums: " + bApplyMinimumsInd + " " + ApplyMinimums + Environment.NewLine;
            message += "Split By: " + SplitByOption + Environment.NewLine;
            message += "Within DC: " + WithinDC + Environment.NewLine;
            message += "Reserve: " + SplitByReserve + Environment.NewLine;
            SW.WriteLine(message);

            // debug code to write out store order by distribution center
            List<int> stores;
            StoreProfile store;
            foreach (string distributionCenter in DCOrder)
            {
                //foreach (KeyValuePair<string, List<int>> keyPair in DCStoreOrder)
                if (DCStoreOrder.TryGetValue(distributionCenter, out stores))
                {
                    count = 0;
                    //distributionCenter = keyPair.Key;
                    //stores = keyPair.Value;
                    message = "DC Store Order:" + distributionCenter;
                    SW.WriteLine(message);
                    message = "Stores(";
                    foreach (int ST_RID in stores)
                    {
                        store = (StoreProfile)storeList.FindKey(ST_RID);
                        if (count > 0)
                        {
                            message += ",";
                        }
                        message += store.Text + " (" + store.Key + ")";
                        ++count;
                        if (message.Length > 150)
                        {
                            SW.WriteLine(message);
                            message = string.Empty;
                            count = 0;
                        }
                    }
                    message += ")";
                    SW.WriteLine(message);
                    SW.WriteLine(" ");
                }
            }

            // debug code to write out header order by distribution center
            List<AllocationProfile> aps;
            count = 0;
            foreach (string distributionCenter in DCOrder)
            {
                //foreach (KeyValuePair<string, List<AllocationProfile>> keyPair in DCHeaderOrder)
                if (DCHeaderOrder.TryGetValue(distributionCenter, out aps))
                {
                    count = 0;
                    //distributionCenter = keyPair.Key;
                    //aps = keyPair.Value;
                    message = "DC Header Order:" + distributionCenter + " Headers(";
                    foreach (AllocationProfile ap in aps)
                    {
                        if (count > 0)
                        {
                            message += ",";
                        }
                        message += ap.HeaderID + " (" + ap.Key + ")"; ;
                        ++count;
                        if (message.Length > 150)
                        {
                            SW.WriteLine(message);
                            message = string.Empty;
                            count = 0;
                        }
                    }
                    message += ")";
                    SW.WriteLine(message);
                }
            }
            SW.WriteLine(" ");
        }

        public void WriteHeaderValues(string sHeaderLabel,
            AllocationProfile ap,
            bool bIncludeAllocatedValues,
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ProfileList storeList
            )
        {
            string message = null;

            HierarchyNodeProfile hnp_style = aSAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID, false);

            message = sHeaderLabel + ":" + ap.HeaderID + " (" + ap.Key + ")" + Environment.NewLine;
            SW.WriteLine(message);

            message = "Total Quantity:" + ap.TotalUnitsToAllocate + Environment.NewLine;
            SW.WriteLine(message);

            WriteHeaderDefinition(ap, aSAB, aApplicationTransaction, storeList);

            if (bIncludeAllocatedValues)
            {
                WriteHeaderAllocatedValues(ap, aSAB, aApplicationTransaction, storeList);
            }
        }

        public void WriteHeaderDefinition(AllocationProfile ap,
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ProfileList storeList
            )
        {
            string message = null;

            HierarchyNodeProfile hnp_style = aSAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID, false);

            // total header
            if (ap.BulkColors.Count == 0
                        && ap.NonGenericPackCount == 0
                        && ap.GenericPackCount == 0)
            {
                message = "Total Units Allocated:" + ap.TotalUnitsAllocated + Environment.NewLine;
                SW.WriteLine(message);
            }
            else
            {
                // PACKS
                if (ap.NonGenericPackCount > 0
                    || ap.GenericPackCount > 0)
                {
                    foreach (PackHdr aPack in ap.Packs.Values)
                    {
                        WritePack(aSAB, aApplicationTransaction, ap, hnp_style, aPack);
                    }
                }
                // BULK
                if (ap.BulkColors.Count > 0)
                {
                    // bulk color size
                    if (ap.HasSizes)
                    {
                        foreach (HdrColorBin aColor in ap.BulkColors.Values)
                        {
                            WriteBulkColorValues(aSAB, aApplicationTransaction, hnp_style, aColor);
                            if (aColor.ColorSizes != null && aColor.ColorSizes.Count > 0)
                            {
                                WriteBulkColorSize(aSAB, aApplicationTransaction, ap, hnp_style, aColor);
                            }
                        }
                    }
                    // bulk color
                    else
                    {
                        //WriteBulkColorLabels();
                        foreach (HdrColorBin aColor in ap.BulkColors.Values)
                        {
                            WriteBulkColorValues(aSAB, aApplicationTransaction, hnp_style, aColor);
                        }
                    }
                }
            }
        }

        public void WriteHeaderAllocatedValues(AllocationProfile ap,
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ProfileList storeList
            )
        {
            int mhStoreAllocated = 0;
            Index_RID storeIndex;

            if (!ap.AllocationStarted)
            {
                return;
            }

            SW.WriteLine(Environment.NewLine + "Allocated Values");

            // determine the longest store label for formatting
            int longestStoreLable = 0;
            foreach (StoreProfile store in storeList)
            {
                string label = store.Text + "(" + store.Key.ToString(CultureInfo.CurrentUICulture) + ")";
                if (label.Length > longestStoreLable)
                {
                    longestStoreLable = label.Length;
                }
            }

            HierarchyNodeProfile hnp_style = aSAB.HierarchyServerSession.GetNodeData(ap.StyleHnRID, false);

            // total header
            if (ap.BulkColors.Count == 0
                        && ap.NonGenericPackCount == 0
                        && ap.GenericPackCount == 0)
            {
                //message = "Total Units Allocated:" + ap.TotalUnitsAllocated + Environment.NewLine;
                //SW.WriteLine(message);

                WriteTotalAllocationLabels(longestStoreLable);
                foreach (StoreProfile aStore in storeList.ArrayList)
                {
                    storeIndex = (Index_RID)aApplicationTransaction.StoreIndexRID(aStore.Key);
                    mhStoreAllocated = ap.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);
                    WriteTotalAllocationValues(aSAB, aApplicationTransaction, longestStoreLable, aStore, mhStoreAllocated);
                }
            }
            else
            {
                // PACKS
                if (ap.NonGenericPackCount > 0
                    || ap.GenericPackCount > 0)
                {
                    foreach (PackHdr aPack in ap.Packs.Values)
                    {
                        WritePackAllocationValues(aSAB, aApplicationTransaction, longestStoreLable, ap, storeList, aPack);
                    }
                }
                // BULK
                if (ap.BulkColors.Count > 0)
                {
                    // bulk color size
                    if (ap.HasSizes)
                    {
                        foreach (HdrColorBin aColor in ap.BulkColors.Values)
                        {
                            //WriteBulkColorAllocationLabels();
                            if (aColor.ColorSizes != null && aColor.ColorSizes.Count > 0)
                            {
                                WriteBulkColorSizeAllocationValues(aSAB, aApplicationTransaction, longestStoreLable, ap, storeList, aColor);
                            }
                        }
                    }
                    // bulk color
                    else
                    {
                        WriteBulkColorAllocationLabels(longestStoreLable);
                        foreach (HdrColorBin aColor in ap.BulkColors.Values)
                        {
                            ColorCodeProfile ccp = aSAB.HierarchyServerSession.GetColorCodeProfile(aColor.ColorCodeRID);
                            foreach (StoreProfile aStore in storeList.ArrayList)
                            {
                                WriteBulkColorAllocationValues(aSAB, aApplicationTransaction, longestStoreLable, ap, aStore, ccp);
                            }
                        }
                    }
                }
            }
        }

        private void WritePack(
            SessionAddressBlock aSAB, 
            ApplicationSessionTransaction aApplicationTransaction,
            AllocationProfile ap,
            HierarchyNodeProfile hnp_style,
            PackHdr aPack
            )
        {
            eAllocationType packType;
            ArrayList sizeID = new ArrayList();
            SortedList primarySL = new SortedList();
            ArrayList secondaryAL = new ArrayList();
            SortedList sortedList = new SortedList();
            Hashtable bothHash = new Hashtable();
            SizeGroupProfile sgp = null;
            SizeCodeList scl = null;

            string dupSizeNameSeparator = MIDText.GetTextOnly((int)eMIDTextCode.lbl_DupSizeNameSeparator);

            if (aPack.GenericPack)
                packType = eAllocationType.GenericType;
            else
                packType = eAllocationType.DetailType;

            WritePackLabels();
            WritePackValues(aSAB, aApplicationTransaction, aPack, packType);

            if (aPack.PackColors != null && aPack.PackColors.Count > 0)
            {
                SW.WriteLine(" ");
                WritePackColorLabels();
                foreach (PackColorSize aColor in aPack.PackColors.Values)
                {
                     WritePackColorValues(aSAB, aApplicationTransaction, hnp_style, aPack, aColor);
                     if (aColor.ColorSizes != null && aColor.ColorSizes.Count > 0)
                     {
                         SW.WriteLine(" ");
                         BuildPackSizeCodeLists(aSAB, aApplicationTransaction, ap, aColor, ref  sizeID, ref  primarySL, ref  secondaryAL, ref  sortedList, ref  bothHash);
                         WritePackColorSizeLabels(aSAB, aApplicationTransaction, hnp_style, aPack, aColor, primarySL);
                         WriteColorSizeValues(primarySL, true, scl, secondaryAL, bothHash);
                     }
                }
            }
        }

        private void BuildPackSizeCodeLists(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            AllocationProfile ap,
            PackColorSize aColor,
            ref ArrayList aSizeID,
            ref SortedList aPrimarySL,
            ref ArrayList aSecondaryAL,
            ref SortedList aSortedList,
            ref Hashtable bothHash
            )
        {
            SizeGroupProfile sgp = null;
            SizeCodeList scl = null;

            string dupSizeNameSeparator = MIDText.GetTextOnly((int)eMIDTextCode.lbl_DupSizeNameSeparator);

            if (ap.SizeGroupRID > Include.UndefinedSizeGroupRID)
            {
                sgp = new SizeGroupProfile(ap.SizeGroupRID);
                scl = sgp.SizeCodeList;
                LoadSizeArraysFromGroup(aSAB, scl, ref aSizeID, ref aPrimarySL, ref aSecondaryAL);
                foreach (PackContentBin aSize in aColor.ColorSizes.Values)
                {
                    if (scl.Contains(aSize.ContentCodeRID))
                    {
                        SizeCodeProfile scp = (SizeCodeProfile)scl.FindKey(aSize.ContentCodeRID);
                        if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID))
                        {
                            bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID, aSize);
                        }
                        else
                        {
                            throw new MIDException(eErrorLevel.severe, 0, aSAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_LabelsNotUnique));
                        }
                    }
                }
            }
            else
            {
  				foreach(PackContentBin aSize in aColor.ColorSizes.Values) 
				{
					aSortedList.Add(aSize.Sequence,aSize);
				}

                foreach (PackContentBin aSize in aSortedList.Values)
                {
                    SizeCodeProfile scp = aSAB.HierarchyServerSession.GetSizeCodeProfile(aSize.ContentCodeRID);
                    if (scp.Key == Include.NoRID)
                    {
                        throw new MIDException(eErrorLevel.severe, 0,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_SizeCodeRetrieveError), aSize.ContentCodeRID.ToString(CultureInfo.CurrentUICulture)));
                    }
                    LoadSizeArraysFromHeader(aSAB, aSize.ContentCodeRID, ref aSizeID, ref aPrimarySL, ref aSecondaryAL);

                    if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID))
                    {
                        bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID, aSize);
                    }
                    else
                    {
                        string newPrimaryBoth = scp.SizeCodePrimary + dupSizeNameSeparator + scp.SizeCodeID;
                        if (!bothHash.ContainsKey(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID))
                        {
                            bothHash.Add(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID, aSize);
                        }

                    }
                }
            }
        }

        private void LoadSizeArraysFromGroup(
            SessionAddressBlock aSAB, 
            SizeCodeList aScl, 
            ref ArrayList aSizeID, 
            ref SortedList aPrimarySL, 
            ref ArrayList aSecondaryAL
            )
        {
            try
            {
                foreach (SizeCodeProfile scp in aScl.ArrayList)
                {
                    if (scp.Key == Include.NoRID)
                    {
                        throw new MIDException(eErrorLevel.severe,
                            (int)eMIDTextCode.msg_CantRetrieveSizeCode,
                            MIDText.GetText(eMIDTextCode.msg_CantRetrieveSizeCode));
                    }

                    if (!aSizeID.Contains(scp.SizeCodeID))
                    {
                        aSizeID.Add(scp.SizeCodeID);		
                    }
                    if (!aPrimarySL.ContainsValue(scp.SizeCodePrimary + "~" + scp.SizeCodePrimaryRID))
                    {
                        aPrimarySL.Add(scp.PrimarySequence, scp.SizeCodePrimary + "~" + scp.SizeCodePrimaryRID);
                    }

                    if (!aSecondaryAL.Contains(scp.SizeCodeSecondary + "~" + scp.SizeCodeSecondaryRID))
                    {
                        aSecondaryAL.Add(scp.SizeCodeSecondary + "~" + scp.SizeCodeSecondaryRID);
                    }
                }     
            }
            catch
            {
                throw;
            }
        }

        private void LoadSizeArraysFromHeader(
            SessionAddressBlock aSAB,
            int aSizeKey, 
            ref ArrayList aSizeID, 
            ref SortedList aPrimarySL, 
            ref ArrayList aSecondaryAL
            )
        {
            try
            {
                SizeCodeProfile scp = aSAB.HierarchyServerSession.GetSizeCodeProfile(aSizeKey);
                if (scp.Key == Include.NoRID)
                {
                    throw new MIDException(eErrorLevel.severe, 0,
                            string.Format(MIDText.GetText(eMIDTextCode.msg_SizeCodeRetrieveError), aSizeKey.ToString(CultureInfo.CurrentUICulture)));
                }
                if (!aSizeID.Contains(scp.SizeCodeID))
                {
                    aSizeID.Add(scp.SizeCodeID);
                }
                  int seq = aPrimarySL.Count;
                if (!aPrimarySL.ContainsValue(scp.SizeCodePrimary + "~" + scp.SizeCodePrimaryRID))
                {
                    seq++;
                    aPrimarySL.Add(seq, scp.SizeCodePrimary + "~" + scp.SizeCodePrimaryRID);
                }

                if (!aSecondaryAL.Contains(scp.SizeCodeSecondary + "~" + scp.SizeCodeSecondaryRID))
                {
                    aSecondaryAL.Add(scp.SizeCodeSecondary + "~" + scp.SizeCodeSecondaryRID);
                }
            }     
            catch
            {
               throw;
            }
        }

        private void WritePackLabels()
        {
            String rec = new String(' ', 150);
            rec = rec.Insert(0, "Pack Name(RID)");
            rec = rec.Insert(26, "Pack Type");
            rec = rec.Insert(40, "Total Packs");
            rec = rec.Insert(55, "Quantity Per Pack");
            rec = rec.Insert(75, "Total Quantity");
            rec = rec.TrimEnd();
            SW.WriteLine(rec);
        }
        private void WritePackValues(
            SessionAddressBlock aSAB, 
            ApplicationSessionTransaction aApplicationTransaction, 
            PackHdr aPack, 
            eAllocationType packType
            )
        {
            String rec = new String(' ', 150);
            rec = rec.Insert(0, aPack.PackName + "(" + aPack.PackRID.ToString(CultureInfo.CurrentUICulture) + ")");
            rec = rec.Insert(26, packType.ToString());
            rec = rec.Insert(40, aPack.PacksToAllocate.ToString());
            rec = rec.Insert(55, aPack.PackMultiple.ToString());
            rec = rec.Insert(75, aPack.UnitsToAllocate.ToString());
            rec = rec.TrimEnd();
            SW.WriteLine(rec);
        }

        private void WritePackColorLabels()
        {
            String rec = new String(' ', 150);
            rec = rec.Insert(5, "Pack Color(RID)");
            rec = rec.Insert(26, "Description");
            rec = rec.Insert(45, "Quantity Per Pack");
            rec = rec.TrimEnd();
             SW.WriteLine(rec);
        }

        private void WritePackColorValues(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction, 
            HierarchyNodeProfile hnp_style,
            PackHdr aPack, 
            PackColorSize aColor
            )
        {
            int colorHnRID = Include.NoRID;
            ColorCodeProfile ccp = aSAB.HierarchyServerSession.GetColorCodeProfile(aColor.ColorCodeRID);

            string packColorDescription = string.Empty;
            string colorID = string.Empty;

            if (ccp.VirtualInd)
            {
                colorID = aColor.ColorName;
                packColorDescription = aColor.ColorDescription;
            }
            else
            {
                colorID = ccp.ColorCodeID;
                if (aSAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
                {
                    HierarchyNodeProfile hnp_color = aSAB.HierarchyServerSession.GetNodeData(colorHnRID, false);
                    packColorDescription = hnp_color.NodeDescription;
                }
                else
                {
                    packColorDescription = ccp.ColorCodeName;
                }
            } 

            String rec = new String(' ', 150);
            rec = rec.Insert(5, colorID + "(" + aColor.ColorCodeRID.ToString(CultureInfo.CurrentUICulture) + ")");
            rec = rec.Insert(26, packColorDescription);
            rec = rec.Insert(45, aColor.ColorUnitsInPack.ToString());
            rec = rec.TrimEnd();
            SW.WriteLine(rec);
        }

        private void WritePackColorSizeLabels(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            HierarchyNodeProfile hnp_style,
            PackHdr aPack,
            PackColorSize aColor,
            SortedList aPrimarySL
            )
        {
            String rec = new String(' ', 250);
            rec = rec.Insert(15, "Total Quantity");

            int column = 35;
            foreach (int seq in aPrimarySL.Keys)
            {
                string[] sizeParts = aPrimarySL[seq].ToString().Split(new char[] { '~' });
                string colName = sizeParts[0];
                rec = rec.Insert(column, colName + " (RID)");
                column += 15;
                if (column > 245)
                {
                    break;
                }
            }

            rec = rec.TrimEnd();
            SW.WriteLine(rec);
        }

        private void WriteColorSizeValues(
            SortedList aPrimarySL, 
            bool aPackSizes, 
            SizeCodeList aSizeCodeList,  
            ArrayList aSecondaryAL, 
            Hashtable aBothHash
            )
        {
            try
            {
                string lblQuantity = MIDText.GetTextOnly(eMIDTextCode.lbl_Quantity);
                string noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
                String rec = new String(' ', 250);
                ArrayList values = new ArrayList();
                foreach (string secondary in aSecondaryAL)
                {
                    rec = new String(' ', 250);

                    string[] secondaryParts = secondary.ToString().Split(new char[] { '~' });
                    string secondaryName = secondaryParts[0];
                    int secondaryRID = Convert.ToInt32(secondaryParts[1], CultureInfo.CurrentUICulture);
                    int total = 0;
                    
                    if (secondaryName == Include.NoSecondarySize || secondaryName.Trim() == string.Empty || secondaryName.Trim() == noSizeDimensionLbl)
                    {
                        secondaryName = lblQuantity;
                    }

                    rec = rec.Insert(5, secondaryName);
                    
                    foreach (int seq in aPrimarySL.Keys)
                    {
                        string[] sizeParts = aPrimarySL[seq].ToString().Split(new char[] { '~' });
                        string primary = sizeParts[0];
                        int primaryRID = Convert.ToInt32(sizeParts[1], CultureInfo.CurrentUICulture);

                        if (aBothHash.Contains(primary  + "~" + secondaryRID)) 
						{
							if (aPackSizes)
							{
                                PackContentBin pcb = (PackContentBin)aBothHash[primary  + "~" + secondaryRID];
								total += pcb.ContentUnits;
                                values.Add(pcb.ContentUnits + " (" + pcb.ContentCodeRID + ")");
                                

							}
							else
							{
                                HdrSizeBin hsb = (HdrSizeBin)aBothHash[primary  + "~" + secondaryRID];
								total += hsb.SizeUnitsToAllocate;
                                values.Add(hsb.SizeUnitsToAllocate + " (" + hsb.SizeCodeRID + ")");
							}
						}
						else
						{
                            values.Add(int.MinValue.ToString());
						}
                    }

                    rec = rec.Insert(15, total.ToString());

                    int column = 35;
                    foreach (string value in values)
                    {
                        if (value != int.MinValue.ToString())
                        {
                            rec = rec.Insert(column, value.ToString());
                        }
                        column += 15;
                        if (column > 245)
                        {
                            break;
                        }
                    }

                    rec = rec.TrimEnd();
                    SW.WriteLine(rec);
                }
            }
            catch 
            {
                throw;
            }
        }

        private void WriteBulkColorLabels()
        {
            String rec = new String(' ', 150);
            rec = rec.Insert(5, "Bulk Color(RID)");
            rec = rec.Insert(26, "Description");
            rec = rec.Insert(45, "Quantity");
            rec = rec.TrimEnd();
            SW.WriteLine(rec);
        }

        private void WriteBulkColorValues(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            HierarchyNodeProfile hnp_style,
            HdrColorBin aColor
            )
        {
            int colorHnRID = Include.NoRID;

            WriteBulkColorLabels();

            ColorCodeProfile ccp = aSAB.HierarchyServerSession.GetColorCodeProfile(aColor.ColorCodeRID);

            string colorID = string.Empty;
            string description = string.Empty;

            if (ccp.VirtualInd)
            {
                colorID = aColor.ColorName;
                description = aColor.ColorDescription;
            }
            else
            {
                colorID = ccp.ColorCodeID;
                if (aSAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
                {
                    HierarchyNodeProfile hnp_color = aSAB.HierarchyServerSession.GetNodeData(colorHnRID, false);
                    description = hnp_color.NodeDescription;
                }
                else
                {
                    description = ccp.ColorCodeName;
                }
            } 

            String rec = new String(' ', 150);
            rec = rec.Insert(5, ccp.ColorCodeID + "(" + ccp.Key.ToString(CultureInfo.CurrentUICulture) + ")");
            rec = rec.Insert(26, description);
            rec = rec.Insert(45, aColor.ColorUnitsToAllocate.ToString());
            rec = rec.TrimEnd();
            //rec += Environment.NewLine;
            SW.WriteLine(rec);
        }

        private void WriteBulkColorSize(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            AllocationProfile ap,
            HierarchyNodeProfile hnp_style,
            HdrColorBin aColor
            )
        {
            ArrayList sizeID = new ArrayList();
            SortedList primarySL = new SortedList();
            ArrayList secondaryAL = new ArrayList();
            Hashtable bothHash = new Hashtable();
            SortedList sortedList = new SortedList();
            SizeGroupProfile sgp = null;
            SizeCodeList scl = null;

            string dupSizeNameSeparator = MIDText.GetTextOnly((int)eMIDTextCode.lbl_DupSizeNameSeparator);

            if (aColor.ColorSizes != null && aColor.ColorSizes.Count > 0)
            {
                BuildBulkSizeCodeLists(aSAB, aApplicationTransaction, ap, aColor, ref sizeID, ref  primarySL, ref  secondaryAL, ref  sortedList, ref  bothHash);
                WriteBulkColorSizeLabels(aSAB, aApplicationTransaction, hnp_style, aColor, primarySL);
                WriteColorSizeValues(primarySL, false, scl, secondaryAL, bothHash);
            }
        }

        private void BuildBulkSizeCodeLists(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            AllocationProfile ap,
            HdrColorBin aColor,
            ref ArrayList aSizeID,
            ref SortedList aPrimarySL,
            ref ArrayList aSecondaryAL,
            ref SortedList aSortedList,
            ref Hashtable bothHash
            )
        {
            SizeGroupProfile sgp = null;
            SizeCodeList scl = null;

            string dupSizeNameSeparator = MIDText.GetTextOnly((int)eMIDTextCode.lbl_DupSizeNameSeparator);

            if (ap.SizeGroupRID > Include.UndefinedSizeGroupRID)
            {
                sgp = new SizeGroupProfile(ap.SizeGroupRID);
                scl = sgp.SizeCodeList;
                LoadSizeArraysFromGroup(aSAB, scl, ref aSizeID, ref aPrimarySL, ref aSecondaryAL);
                foreach (HdrSizeBin aSize in aColor.ColorSizes.Values)
                {
                    aSortedList.Add(aSize.SizeSequence, aSize);
                }
                foreach (HdrSizeBin aSize in aSortedList.Values)
                {
                    if (scl.Contains(aSize.SizeKey))
                    {
                        SizeCodeProfile scp = (SizeCodeProfile)scl.FindKey(aSize.SizeKey);
                        if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID))
                        {
                            bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID, aSize);
                        }
                    }
                }
            }
            else
            {
                foreach (HdrSizeBin aSize in aColor.ColorSizes.Values)
                {
                    aSortedList.Add(aSize.SizeSequence, aSize);
                }
                scl = new SizeCodeList(eProfileType.SizeCode);
                foreach (HdrSizeBin aSize in aSortedList.Values)
                {
                    SizeCodeProfile scp = aSAB.HierarchyServerSession.GetSizeCodeProfile(aSize.SizeKey);
                    scl.Add(scp);
                    LoadSizeArraysFromHeader(aSAB, aSize.SizeKey, ref aSizeID, ref aPrimarySL, ref aSecondaryAL);
                    if (!bothHash.ContainsKey(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID))
                    {
                        bothHash.Add(scp.SizeCodePrimary + "~" + scp.SizeCodeSecondaryRID, aSize);
                    }
                    else
                    {
                        string newPrimaryBoth = scp.SizeCodePrimary + dupSizeNameSeparator + scp.SizeCodeID;
                        if (!bothHash.ContainsKey(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID))
                        {
                            bothHash.Add(newPrimaryBoth + "~" + scp.SizeCodeSecondaryRID, aSize);
                        }
                    }
                }
            }
        }

        private void WriteBulkColorSizeLabels(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            HierarchyNodeProfile hnp_style,
            HdrColorBin aColor,
            SortedList aPrimarySL
            )
        {
            String rec = new String(' ', 250);
            rec = rec.Insert(15, "Total Quantity");

            int column = 35;
            foreach (int seq in aPrimarySL.Keys)
            {
                string[] sizeParts = aPrimarySL[seq].ToString().Split(new char[] { '~' });
                string colName = sizeParts[0];
                rec = rec.Insert(column, colName + " (RID)");
                column += 15;
                if (column > 245)
                {
                    break;
                }
            }

            rec = rec.TrimEnd();
            //rec += Environment.NewLine;
            SW.WriteLine(rec);
        }

        private void WriteTotalAllocationLabels(int longestStoreLable)
        {
            String rec = new String(' ', 150);
            rec = rec.Insert(0, "Store Name(RID)");
            rec = rec.Insert(longestStoreLable + 5, "Qty Allocated");
            SW.WriteLine(rec);
        }

        private void WriteTotalAllocationValues(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            int longestStoreLable,
            StoreProfile aStore,
            int aQuantity)
        {
            String rec = new String(' ', 150);
            rec = rec.Insert(0, aStore.Text + "(" + aStore.Key.ToString(CultureInfo.CurrentUICulture) + ")");
            rec = rec.Insert(longestStoreLable + 5, aQuantity.ToString(CultureInfo.CurrentUICulture));
            rec = rec.TrimEnd();
            rec += Environment.NewLine;
            SW.WriteLine(rec);
        }

        private void WriteBulkColorAllocationLabels(int longestStoreLable)
        {
            int column = longestStoreLable + 5;
            String rec = new String(' ', 150);
            rec = rec.Insert(0, "Store Name(RID)");
            rec = rec.Insert(column, "Color");
            column += 15;
            rec = rec.Insert(column, "Qty Allocated");
            SW.WriteLine(rec);
        }

        private void WriteBulkColorAllocationValues(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            int longestStoreLable,
            AllocationProfile ap,
            StoreProfile aStore,
            ColorCodeProfile aColor)
        {
            int column = longestStoreLable + 5;

            int quantity = ap.GetStoreQtyAllocated(aColor.Key, aStore.Key);

            String rec = new String(' ', 150);
            rec = rec.Insert(0, aStore.Text + "(" + aStore.Key.ToString(CultureInfo.CurrentUICulture) + ")");
            rec = rec.Insert(column, aColor.ColorCodeID);
            column += 15;
            rec = rec.Insert(column, quantity.ToString(CultureInfo.CurrentUICulture));
            rec = rec.TrimEnd();
            //rec += Environment.NewLine;
            SW.WriteLine(rec);
        }

        private void WriteBulkColorSizeAllocationLabels(int longestStoreLable, HdrColorBin aColor, SortedList aPrimarySL)
        {
            int column = longestStoreLable + 5;
            String rec = new String(' ', 250);
            rec = rec.Insert(0, "Store Name(RID)");
            rec = rec.Insert(column, "Color");
            column += 15;
            rec = rec.Insert(column, "Secondary");

            column += 15;
            foreach (int seq in aPrimarySL.Keys)
            {
                string[] sizeParts = aPrimarySL[seq].ToString().Split(new char[] { '~' });
                string colName = sizeParts[0];
                rec = rec.Insert(column, colName);
                column += 15;
                if (column > 245)
                {
                    break;
                }
            }
            rec = rec.TrimEnd();
            SW.WriteLine(rec);
        }

        private void WriteBulkColorSizeAllocationValues(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            int longestStoreLable,
            AllocationProfile ap,
            ProfileList storeList,
            HdrColorBin aColor)
        {
            ArrayList sizeID = new ArrayList();
            SortedList primarySL = new SortedList();
            ArrayList secondaryAL = new ArrayList();
            Hashtable bothHash = new Hashtable();
            SortedList sortedList = new SortedList();
            int quantity;
            HdrSizeBin hsb;

            string lblQuantity = MIDText.GetTextOnly(eMIDTextCode.lbl_Quantity);
            string noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);

            BuildBulkSizeCodeLists(aSAB, aApplicationTransaction, ap, aColor, ref sizeID, ref primarySL, ref secondaryAL, ref sortedList, ref bothHash);

            WriteBulkColorSizeAllocationLabels(longestStoreLable, aColor, primarySL);

            ColorCodeProfile ccp = aSAB.HierarchyServerSession.GetColorCodeProfile(aColor.ColorCodeRID);

            foreach (StoreProfile aStore in storeList)
            {
                quantity = ap.GetStoreQtyAllocated(aColor.ColorCodeRID, aStore.Key);
                foreach (string secondary in secondaryAL)
                {
                    string[] secondaryParts = secondary.ToString().Split(new char[] { '~' });
                    string secondaryName = secondaryParts[0];
                    int secondaryRID = Convert.ToInt32(secondaryParts[1], CultureInfo.CurrentUICulture);
                    if (secondaryName == Include.NoSecondarySize || secondaryName.Trim() == string.Empty || secondaryName.Trim() == noSizeDimensionLbl)
                    {
                        secondaryName = lblQuantity;
                    }
                    String rec = new String(' ', 250);
                    int column = longestStoreLable + 5;
                    rec = rec.Insert(0, aStore.Text + "(" + aStore.Key.ToString(CultureInfo.CurrentUICulture) + ")");
                    rec = rec.Insert(column, ccp.ColorCodeID);
                    column += 15;
                    rec = rec.Insert(column, secondaryName);

                    column += 15;
                    foreach (int seq in primarySL.Keys)
                    {
                        string[] sizeParts = primarySL[seq].ToString().Split(new char[] { '~' });
                        string primary = sizeParts[0];
                        int primaryRID = Convert.ToInt32(sizeParts[1], CultureInfo.CurrentUICulture);

                        if (bothHash.Contains(primary + "~" + secondaryRID))
                        {
                            hsb = (HdrSizeBin)bothHash[primary + "~" + secondaryRID];
                            quantity = ap.GetStoreQtyAllocated(aColor.ColorCodeRID, hsb.SizeCodeRID, aStore.Key);
                        }
                        else
                        {
                            quantity = 0;
                        }
                        
                        rec = rec.Insert(column, quantity.ToString());
                        column += 15;
                        if (column > 245)
                        {
                            break;
                        }
                    }

                    rec = rec.TrimEnd();
                    SW.WriteLine(rec);
                }
            }
        }

        private void WritePackAllocationLabels(int longestStoreLable)
        {
            int column = longestStoreLable + 5;
            String rec = new String(' ', 150);
            rec = rec.Insert(0, "Store Name(RID)");
            rec = rec.Insert(column, "Pack");
            column += 15;
            rec = rec.Insert(column, "Qty Allocated");
            rec = rec.TrimEnd();
            SW.WriteLine(rec);
        }

        private void WritePackAllocationValues(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            int longestStoreLable,
            AllocationProfile ap,
            ProfileList storeList,
            PackHdr aPack)
        {
            int quantity;
            WritePackAllocationLabels(longestStoreLable);

            foreach (StoreProfile aStore in storeList)
            {
                int column = longestStoreLable + 5;
                String rec = new String(' ', 150);
                rec = rec.Insert(0, aStore.Text + "(" + aStore.Key.ToString(CultureInfo.CurrentUICulture) + ")");
                rec = rec.Insert(column, aPack.PackName);
                column += 15;
                quantity = ap.GetStoreQtyAllocated(aPack.PackName, aStore.Key);
                rec = rec.Insert(column, quantity.ToString());
                rec = rec.TrimEnd();
                 SW.WriteLine(rec);
            }

            
        }


        public void WriteSpreadValues(double spreadValue, ArrayList basisValues, ArrayList spreadValues)
        {
            string message = "Executing Spread with value " + spreadValue;
            SW.WriteLine(message);
            message = string.Empty;
            message = "Basis values:";
            bool firstValue = true;
            foreach (int value in basisValues)
            {
                if (!firstValue)
                {
                    message += ",";
                }
                message += value;
                firstValue = false;
            }
            SW.WriteLine(message);

            message = "Spread values:";
            firstValue = true;
            foreach (double value in spreadValues)
            {
                if (!firstValue)
                {
                    message += ",";
                }
                message += value;
                firstValue = false;
            }
            SW.WriteLine(message);
        }

		public void WriteSetHeader()
		{
            //SW.WriteLine(" ");
            //SW.WriteLine("Forecast Type: " + _forecastType.ToString());
            //SW.WriteLine("Is Default: " + _isDefault.ToString(CultureInfo.CurrentUICulture));
            //SW.WriteLine("Smooth By: " + _smoothBy.ToString());
            //SW.WriteLine("Chain Value: " + _chainValue.ToString(CultureInfo.CurrentUICulture));
            //SW.WriteLine("For Week: " + _yearWeek.ToString(CultureInfo.CurrentUICulture));

            //SW.WriteLine("SETS:");
            //foreach(GroupLevelFunctionProfile aSet in _sets)
            //{

            //    StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)_groupSets.FindKey(aSet.Key);

            //    SW.Write("  " + sglp.Name + "(" + aSet.Key.ToString(CultureInfo.CurrentUICulture) + ")" + "\t");
            //}
            //SW.WriteLine();
            //SW.WriteLine("STORES in this SET(S):");
            //foreach(string aStore in _stores)
            //{
            //    SW.Write(aStore + "\t");
            //}
			SW.WriteLine();
		}

		public void WriteMiscMessageQueue()
		{
			foreach(string msg in this._miscMsgList)
			{
				SW.WriteLine(msg);
			}
			SW.WriteLine();
			_miscMsgList.Clear();

		}

		public void DumpToFile()
		{
			WriteStoreDataLabels();
            //foreach(DCFulfillmentMonitorStoreData sd in _storeData)
            //{

            //    WriteStoreData(sd, false);
            //}
			SW.WriteLine();

		}



		//**************************************************************************************
		// New methods for on demand printing of log
		//**************************************************************************************
		public void WriteStoreDataLabels()
		{
			String rec = new String(' ',150);
			SW.WriteLine(rec);
//            switch (_monitorType)
//            {
//                case eDCFulfillmentMonitorType.PercentContribution:
//                    rec = rec.Insert(0, "StoreName(RID)");
//                    rec = rec.Insert(26, "isElig");
//                    rec = rec.Insert(33, "grade");
//                    rec = rec.Insert(39, "set");
//                    rec = rec.Insert(46, "isLocked");
//                    rec = rec.Insert(55, "initValue");
//                    rec = rec.Insert(67, "salesMod");
//                    rec = rec.Insert(76, "ResultValue");
//                    rec = rec.TrimEnd();
//                    rec += "\r";
//                    break;

//                case eDCFulfillmentMonitorType.AverageSales:
//                    rec = rec.Insert(0, "StoreName(RID)");
//                    rec = rec.Insert(26, "isElig");
//                    rec = rec.Insert(33, "grade");
//                    rec = rec.Insert(39, "set");
//                    rec = rec.Insert(46, "isLocked");
//                    rec = rec.Insert(55, "initValue");
//                    rec = rec.Insert(67, "salesMod");
//                    rec = rec.Insert(76, "ResultValue");
//                    rec = rec.TrimEnd();
//                    rec += "\r";
//                    break;

//                case eDCFulfillmentMonitorType.CurrentTrend:
//                    rec = rec.Insert(0, "StoreName(RID)");
//                    rec = rec.Insert(26, "isElig");
//                    rec = rec.Insert(33, "grade");
//                    rec = rec.Insert(39, "set");
//                    rec = rec.Insert(46, "isLocked");
//                    rec = rec.Insert(55, "initValue");
//                    rec = rec.Insert(67, "salesMod");
//                    rec = rec.Insert(76, "ResultValue");
//                    rec = rec.TrimEnd();
//                    rec += "\r";
//                    break;

//                case eDCFulfillmentMonitorType.TyLyTrend:
//                    rec = rec.Insert(0, "StoreName(RID)");
//                    rec = rec.Insert(26, "isElig");
//                    rec = rec.Insert(33, "grade");
//                    rec = rec.Insert(39, "set");
//                    rec = rec.Insert(46, "isLocked");
//                    rec = rec.Insert(55, "LY");
//                    rec = rec.Insert(67, "TY");
//                    rec = rec.Insert(79, "Trend");
//                    rec = rec.Insert(91, "ApplyTo");
//                    rec = rec.Insert(103, "Result");
//                    rec = rec.TrimEnd();
//                    rec += "\r";
//                    break;
			
//                case eDCFulfillmentMonitorType.Inventory:
//                    rec = rec.Insert(0, "StoreName(RID)");
//                    rec = rec.Insert(26, "isElig");
//                    rec = rec.Insert(33, "grade");
//                    rec = rec.Insert(39, "set");
//                    rec = rec.Insert(46, "isLocked");
//                    rec = rec.Insert(55, "totalSales");
//                    rec = rec.Insert(67, "avgSales");
//                    rec = rec.Insert(79, "weeksUsed");
//                    // BEGIN MID Track #4370 - John Smith - FWOS Models
//                    rec = rec.Insert(89, "WOS");
//                    //Begin TT#875 - JScott - Add Base Code to Support A&F Custom Features
//                    //rec = rec.Insert(99, "WOSMod");
//                    rec = rec.Insert(99, _customBusinessRoutines.GetDCFulfillmentMonitorWOSModLabel());
//                    //End TT#875 - JScott - Add Base Code to Support A&F Custom Features
//                    rec = rec.Insert(109, "WOSIndex");
//                    rec = rec.Insert(118, "stkMod");
//                    rec = rec.Insert(125, "stkMin");
//                    rec = rec.Insert(132, "stkMax");
//                    rec = rec.Insert(139, "inventory");
//                    //Begin TT#875 - JScott - Add Base Code to Support A&F Custom Features
//                    //rec = rec.Insert(149, "Apply-Pre Min");  // Issue 4827
//                    rec = rec.Insert(149, _customBusinessRoutines.GetDCFulfillmentMonitorApplyPreMinLabel());
//                    //End TT#875 - JScott - Add Base Code to Support A&F Custom Features
////					rec = rec.Insert(89, "WOSIndex");
////					rec = rec.Insert(98, "stkMod");
////					rec = rec.Insert(105, "stkMin");
////					rec = rec.Insert(112, "stkMax");
////					rec = rec.Insert(119, "inventory");
//                    // END MID Track #4370
//                    rec = rec.TrimEnd();
//                    rec += "\r";
//                    break;

//            }

			SW.WriteLine(rec);
		}

		
	}

	
}
