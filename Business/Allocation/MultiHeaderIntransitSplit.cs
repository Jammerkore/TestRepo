using System;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	#region MultiHeaderIntransitSplit
	/// <summary>
	/// Summary description for MultiHeaderIntransitSplit.
	/// </summary>
	public class MultiHeaderIntransitSplit
	{
		private AllocationProfile _allocProfileMultiHeader;
		private SessionAddressBlock SAB;
		private ApplicationSessionTransaction _transaction;
		private AllocationProfileList _allocProfileList;
		private Header _headerData;
        //private Hashtable _childIktHash;				// hash of each individual IktHash for each child header. 
        //private Hashtable _childIntransitValuesHash;	// hash of each individual intransitValues for each child header. 
		private ProfileList _storeList;

		/// <summary>
		/// Class that takes the a Multi header and splits the Intransit between
		/// all of the child header's belonging to it.
		/// </summary>
		/// <param name="aTrans"></param>
		/// <param name="allocProfile"></param>
		public MultiHeaderIntransitSplit(ApplicationSessionTransaction aTrans, AllocationProfile allocProfile, AllocationProfileList childProfileList)
		{
			_transaction = aTrans;
			SAB = _transaction.SAB;
			_allocProfileMultiHeader = allocProfile;
			_allocProfileList = childProfileList;
			_headerData = new Header();
		}

		/// <summary>
		/// Takes the Multi header defined in the class and splits the Intransit between
		/// all of the header's belonging to it.
		/// </summary>
		/// <remarks>
		/// If the header defined within the class is NOT a multi header, it returns false.
		/// </remarks>
		/// <returns></returns>
		public bool Process(eAllocationMethodType methodType)
		{
			bool successful = false;
			if (_allocProfileMultiHeader.HeaderType == eHeaderType.MultiHeader)
			{
				_storeList = _transaction.GetMasterProfileList(eProfileType.Store);

				if (methodType == eAllocationMethodType.ChargeIntransit
					|| methodType == eAllocationMethodType.ChargeSizeIntransit)
				{
                    // begin TT#37 - Cannot Release Multi when headers have no packs and no bulk
                    if (_allocProfileMultiHeader.BulkColors.Count == 0
                        && _allocProfileMultiHeader.NonGenericPackCount == 0
                        && _allocProfileMultiHeader.GenericPackCount == 0)
                    {
                        SplitTotalAllocation();
                    }
                    else
                    {
                        // end TT#37 - Cannot Release Multi when Headers have no packs and no bulk
                        // BULK
                        if (_allocProfileMultiHeader.BulkColors.Count > 0)
                        {
                            //if (_allocProfileMultiHeader.BulkSizeBreakoutPerformed && // MID track 4021 cannot release multi with sizes
                            if (_allocProfileMultiHeader.AtLeastOneSizeAllocated && // MID track 4021 cannot release multi with sizes
                                _allocProfileMultiHeader.BulkSizeAllocationInBalance)
                                SplitBulkSizeAllocation();
                            else
                                SplitBulkAllocation();
                        }

                        // PACKS
                        if (_allocProfileMultiHeader.NonGenericPackCount > 0
                            || _allocProfileMultiHeader.GenericPackCount > 0)
                            SplitPackAllocation();
                    } // TT#37 - Cannot Release Multi when headers have no packs and no bulk

					successful = true;

				}
				else
				{
					successful = true;
				}
				
				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Information,
					eMIDTextCode.msg_MultiHeaderInstransitSplitComplete,
                    SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_MultiHeaderInstransitSplitComplete, false) + " Header [" + _allocProfileMultiHeader.HeaderID + "] ", // MID Track 5778 Scheduler 'Run Now' feature gets error in audit
					this.GetType().Name);
			}
			
			return successful;
		}

//		/// <summary>
//		/// Gets the child headers of a multi header and loads them into a allocation profile list
//		/// </summary>
//		public void LoadChildHeaderList()
//		{
//			DataTable dtChildHeaders = _headerData.GetHeaderGroupChildren(_allocProfileMultiHeader.HeaderRID);
//			int headerIndex = 0;
//			int[] selectedHeaders = new int[dtChildHeaders.Rows.Count];
//			foreach (DataRow row in dtChildHeaders.Rows)
//			{
//				selectedHeaders[headerIndex] = Convert.ToInt32(row["HDR_RID"],CultureInfo.CurrentUICulture);
//				++headerIndex;
//			}
//			_allocProfileList.LoadHeaders(_transaction, selectedHeaders, SAB.ApplicationServerSession);
//		}

        // begin TT#37 - Cannot Release Multi When no packs and no bulk
        private void SplitTotalAllocation()
        {
            try
            {
                ArrayList mcAllocProfileList = new ArrayList();
                ArrayList mcToAllocateList = new ArrayList();
                BasicSpread spread = new BasicSpread();

                foreach (AllocationProfile childAp in _allocProfileList.ArrayList)
                {
                    mcAllocProfileList.Add(childAp);
                    int QtyToAllocate = childAp.TotalUnitsToAllocate;
                    mcToAllocateList.Add(QtyToAllocate);
                }
                //========================================================================================
                // For each store use the child header's total to allocate  as the basis to
                // spread the multi header's allocated value.
                //========================================================================================
                AllocationSubtotalProfile mhAllocSubtotalProfile = _transaction.GetAllocationGrandTotalProfile();
                foreach (StoreProfile aStore in _storeList.ArrayList)
                {
                    Index_RID storeIndex = (Index_RID)_transaction.StoreIndexRID(aStore.Key);
                    int mhStoreAllocated = _allocProfileMultiHeader.GetStoreQtyAllocated(eAllocationSummaryNode.Total, aStore.Key);

                    ArrayList spreadToList = new ArrayList();
                    ArrayList changedList = new ArrayList();
                    //==============
                    // SPREAD
                    //==============
                    double spreadValue = Convert.ToDouble(mhStoreAllocated);
                    spread.ExecuteSimpleSpread(spreadValue, mcToAllocateList, 0, out changedList);

                    //================================================
                    // Apply spread to Total/store in child headers
                    //================================================
                    for (int i = 0; i < mcAllocProfileList.Count; i++)
                    {
                        mcToAllocateList[i] = (int)mcToAllocateList[i] - Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture);
                        AllocationProfile childAp = (AllocationProfile)mcAllocProfileList[i];
                        childAp.SetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIndex, Convert.ToInt32(changedList[i], CultureInfo.CurrentUICulture), eDistributeChange.ToParent, false);
                        // begin MID Track 4448 AnF Audit Enhancement
                        childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex, _allocProfileMultiHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIndex));
                        // end MID Track 4448 AnF Audit Enhancement
                    }
                }
                foreach (AllocationProfile childAp in _allocProfileList.ArrayList)
                {
                    childAp.SetAllocationFromMultiHeader(true);
                    foreach (Index_RID storeIdxRID in childAp.AppSessionTransaction.StoreIndexRIDArray())
                    {
                        childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIdxRID, _allocProfileMultiHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIdxRID));
                    }
                }
            }
            catch
            {
                throw;
            }

        }
        // end TT#37 - Cannot Release Multi When no packs and no bulk
		private void SplitBulkAllocation()
		{
			try
			{
				int [] bulkColorCodeRIDs = _allocProfileMultiHeader.GetBulkColorCodeRIDs();
				ArrayList mcAllocProfileList = new ArrayList();
				ArrayList mcToAllocateList = new ArrayList();
				BasicSpread spread = new BasicSpread(); 
			
				foreach (int colorKey in bulkColorCodeRIDs)
				{
					//int mhColorQtyToAllocate = _allocProfileMultiHeader.GetColorUnitsToAllocate(colorKey);

					mcAllocProfileList.Clear();
					mcToAllocateList.Clear();
					foreach (AllocationProfile childAp in _allocProfileList.ArrayList)
					{
						if (childAp.BulkColorIsOnHeader(colorKey))
						{
							mcAllocProfileList.Add(childAp);
							int QtyToAllocate = childAp.GetColorUnitsToAllocate(colorKey);
							mcToAllocateList.Add(QtyToAllocate);
						}
					}
					//========================================================================================
					// For each store use the child header's total to allocate for the color as the basis to
					// spread the multi header's allocated color value.
					//========================================================================================
					int colorMatchCnt = mcAllocProfileList.Count;
					AllocationSubtotalProfile mhAllocSubtotalProfile = _transaction.GetAllocationGrandTotalProfile();
					foreach (StoreProfile aStore in _storeList.ArrayList)
					{
						int mhStoreAllocated = _allocProfileMultiHeader.GetStoreQtyAllocated(colorKey, aStore.Key);

						ArrayList spreadToList = new ArrayList();
						ArrayList changedList = new ArrayList();
						//==============
						// SPREAD
						//==============
						double spreadValue = Convert.ToDouble(mhStoreAllocated);
						spread.ExecuteSimpleSpread(spreadValue, mcToAllocateList, 0, out changedList);

						//================================================
						// Apply spread to color/store in child headers
						//================================================
						for (int i=0;i<colorMatchCnt;i++)
						{
							mcToAllocateList[i] = (int)mcToAllocateList[i] - Convert.ToInt32(changedList[i],CultureInfo.CurrentUICulture);
							AllocationProfile childAp = (AllocationProfile)mcAllocProfileList[i];
							Index_RID storeIndex = (Index_RID)childAp.StoreIndex(aStore.Key);
							childAp.SetStoreQtyAllocated(colorKey, storeIndex, Convert.ToInt32(changedList[i],CultureInfo.CurrentUICulture), eDistributeChange.ToParent, false);
							// begin MID Track 4448 AnF Audit Enhancement
							childAp.SetAllDetailAuditFlags(colorKey, storeIndex, _allocProfileMultiHeader.GetAllDetailAuditFlags(colorKey, storeIndex));
							// end MID Track 4448 AnF Audit Enhancement
						}
					}
				}
				// begin MID Track 4448 AnF Audit Enhancement
				foreach (AllocationProfile childAp in _allocProfileList.ArrayList)
				{
					if (childAp.BulkColors.Count > 0)
					{
						childAp.SetAllocationFromMultiHeader(true);
						foreach (Index_RID storeIdxRID in childAp.AppSessionTransaction.StoreIndexRIDArray())
						{
							childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIdxRID, _allocProfileMultiHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIdxRID));
							childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Bulk, storeIdxRID, _allocProfileMultiHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Bulk, storeIdxRID));
							if (childAp.BulkIsDetail)
							{
								childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIdxRID, _allocProfileMultiHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIdxRID));
							}
						}
					}
				}
				// end MID Track 4448 AnF Audit Enhancement
			}
			catch
			{
				throw;
			}

		}

		private void SplitBulkSizeAllocation()
		{
			try
			{
				int [] bulkColorCodeRIDs = _allocProfileMultiHeader.GetBulkColorCodeRIDs();
				ArrayList mcAllocProfileList = new ArrayList();		// alloc profiles w/ matching color
				ArrayList msAllocProfileList = new ArrayList();		// alloc profiles w/ matching size
				ArrayList msToAllocateList = new ArrayList();

				BasicSpread spread = new BasicSpread(); 
			
				foreach (int colorKey in bulkColorCodeRIDs)
				{
					mcAllocProfileList.Clear();
					foreach (AllocationProfile childAp in _allocProfileList.ArrayList)
					{
						if (childAp.BulkColorIsOnHeader(colorKey))
						{
							mcAllocProfileList.Add(childAp);
						}
					}
                     
					//int[] mhSizeKeys = _allocProfileMultiHeader.GetBulkColorSizeKeys(colorKey);  // Assortment: Color/Size changes
                    int[] mhSizeKeys = _allocProfileMultiHeader.GetBulkColorSizeCodeRIDs(colorKey); // Assortment: Color/Size chnges
					foreach (int sizeKey in mhSizeKeys)
					{
						int mhSizeQtyToAllocate = _allocProfileMultiHeader.GetSizeUnitsToAllocate(colorKey, sizeKey);
						msAllocProfileList.Clear();
						msToAllocateList.Clear();
						foreach (AllocationProfile childAp in mcAllocProfileList)
						{
							if (childAp.SizeIsOnBulkColor(colorKey, sizeKey))
							{
								msAllocProfileList.Add(childAp);
								int QtyToAllocate = childAp.GetSizeUnitsToAllocate(colorKey, sizeKey);
								msToAllocateList.Add(QtyToAllocate);
							}
						}
				
						//========================================================================================
						// For each store use the child header's size total to allocate for the size as the basis to
						// spread the multi header's allocated size value.
						//========================================================================================
						int sizeMatchCnt = msAllocProfileList.Count;
						foreach (StoreProfile aStore in _storeList.ArrayList)
						{
							int mhStoreSizeAllocated = _allocProfileMultiHeader.GetStoreQtyAllocated(colorKey, sizeKey, aStore.Key);
							ArrayList spreadToList = new ArrayList();
							ArrayList changedList = new ArrayList();

							double spreadValue = Convert.ToDouble(mhStoreSizeAllocated);
							spread.ExecuteSimpleSpread(spreadValue, msToAllocateList, 0, out changedList);

							//===================================================
							// Apply spread to color/size/store in child headers
							//===================================================
							for (int i=0;i<sizeMatchCnt;i++)
							{
								msToAllocateList[i] = (int)msToAllocateList[i] - Convert.ToInt32(changedList[i],CultureInfo.CurrentUICulture);
								AllocationProfile childAp = (AllocationProfile) msAllocProfileList[i];
								Index_RID storeIndex = (Index_RID)childAp.StoreIndex(aStore.Key);
								childAp.SetStoreQtyAllocated(colorKey, sizeKey, storeIndex, Convert.ToInt32(changedList[i],CultureInfo.CurrentUICulture), eDistributeChange.ToParent, false);
								// begin MID Track 4448 AnF Audit Enhancement
								childAp.SetAllDetailAuditFlags(colorKey, storeIndex, _allocProfileMultiHeader.GetAllDetailAuditFlags(colorKey, storeIndex));
								childAp.SetAllDetailAuditFlags(colorKey, sizeKey, storeIndex, _allocProfileMultiHeader.GetAllDetailAuditFlags(colorKey, sizeKey, storeIndex));
								// end MID Track 4448 AnF Audit Enhancement
							}
	
						}
					}
				}
				// begin MID Track 4448 AnF Audit Enhancement
				foreach (AllocationProfile childAp in _allocProfileList.ArrayList)
				{
					if (childAp.BulkColors.Count > 0)
					{
						childAp.SetAllocationFromMultiHeader(true);
						foreach (Index_RID storeIdxRID in childAp.AppSessionTransaction.StoreIndexRIDArray())
						{
							childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIdxRID, _allocProfileMultiHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIdxRID));
							childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Bulk, storeIdxRID, _allocProfileMultiHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Bulk, storeIdxRID));
							if (childAp.BulkIsDetail)
							{
								childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIdxRID, _allocProfileMultiHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIdxRID));
							}
						}
					}
				}
				// end MID Track 4448 AnF Audit Enhancement
			}
			catch
			{
				throw;
			}

		}

		private void SplitPackAllocation()
		{
			try
			{
				bool packFound = false;
				string [] PackIds = _allocProfileMultiHeader.GetPackNames();
				//=========================================================================================
				// Foreach pack on the multi-header, search the child headers looking for a maching pack. 
				//=========================================================================================
				foreach (string packName in PackIds)
				{
					int associatedPackRid = _allocProfileMultiHeader.GetAssociatedPackRID(packName);
					packFound = false;
					foreach (AllocationProfile childAp in _allocProfileList.ArrayList)
					{
						string [] childPackIds = childAp.GetPackNames();
						foreach (string childPackName in childPackIds)
						{
							int packRid = childAp.GetPackRID(childPackName);
							//=======================================================================
							// If we find a match, copy the units allocated to each store from the
							// multi-header to the child header.
							//=======================================================================
							if (associatedPackRid == packRid)
							{
								packFound = true;
								foreach (StoreProfile aStore in _storeList.ArrayList)
								{			
									int units = _allocProfileMultiHeader.GetStoreQtyAllocated(packName, aStore.Key);
									Index_RID storeIndex = (Index_RID)childAp.StoreIndex(aStore.Key);
									childAp.SetStoreQtyAllocated(childPackName, storeIndex, units, eDistributeChange.ToParent, false);
									// begin MID Track 4448 AnF Audit Enhancement
									childAp.SetAllDetailAuditFlags(childPackName, storeIndex, _allocProfileMultiHeader.GetAllDetailAuditFlags(packName, storeIndex));
									// end MID Track 4448 AnF Audit Enhancement
								}
							}
						}
					}
					if (!packFound)
					{
						string errMsg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_MultiHeaderNotMachingPack, false);
						errMsg = errMsg.Replace("{0}",packName);
						errMsg = errMsg.Replace("{1}",_allocProfileMultiHeader.HeaderID);
						SAB.ApplicationServerSession.Audit.Add_Msg(
							eMIDMessageLevel.Severe,
							eMIDTextCode.msg_MultiHeaderNotMachingPack,
							errMsg,
							this.GetType().Name);

						throw new MIDException(eErrorLevel.severe,
							(int)eMIDTextCode.msg_MultiHeaderNotMachingPack,
							errMsg);
					}
				}
				// begin MID Track 4448 AnF Audit Enhancement
				foreach (AllocationProfile childAp in _allocProfileList.ArrayList)
				{
					if (childAp.Packs.Count > 0)
					{
						childAp.SetAllocationFromMultiHeader(true);
						foreach (Index_RID storeIdxRID in childAp.AppSessionTransaction.StoreIndexRIDArray())
						{
							childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIdxRID, _allocProfileMultiHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.Total, storeIdxRID));
							if (childAp.NonGenericPackCount > 0)
							{
								childAp.SetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIdxRID, _allocProfileMultiHeader.GetAllDetailAuditFlags(eAllocationSummaryNode.DetailType, storeIdxRID));
							}
						}
					}
				}
				// end MID Track 4448 AnF Audit Enhancement
			}
			catch
			{
				throw;
			}
		}

	

//		private MultiHeaderColorIntransitWorkArea LoadColorWorkArea(int colorKey)
//		{
//			try
//			{
//				HdrColorBin colorBin = _allocProfileMultiHeader.GetHdrColorBin(colorKey);
//				int colorQtyAllocated = _allocProfileMultiHeader.GetColorUnitsAllocated(colorKey);
//				int colorQtyToAllocate = _allocProfileMultiHeader.GetColorUnitsToAllocate(colorKey);
//				MultiHeaderColorIntransitWorkArea colorWa = new MultiHeaderColorIntransitWorkArea(colorKey, colorQtyToAllocate, colorQtyAllocated);
//				int [] sizeKeys = _allocProfileMultiHeader.GetBulkColorSizeKeys(colorKey);
//				foreach (int sizeKey in sizeKeys)
//				{
//					int sizeQtyToAllocate = _allocProfileMultiHeader.GetSizeUnitsToAllocate(colorKey, sizeKey);
//					int sizeQtyAllocated = _allocProfileMultiHeader.GetSizeUnitsAllocated(colorKey, sizeKey);
//					colorWa.AddSizeQtyToAllocate(sizeKey, sizeQtyToAllocate);
//					colorWa.AddSizeQtyAllocated(sizeKey, sizeQtyAllocated);
//				}
//				// load color workarea with child header info for each child with a bulk color match
//				foreach (AllocationProfile childAp in _allocProfileList.ArrayList)
//				{
//					if (childAp.BulkColorIsOnHeader(colorKey))
//					{
//						int childColorQtyAllocated = childAp.GetColorUnitsAllocated(colorKey);
//						int childColorQtyToAllocate = childAp.GetColorUnitsToAllocate(colorKey);
//						MultiHeaderHeaderIntransitWorkArea hdrWa = 
//							new MultiHeaderHeaderIntransitWorkArea(childAp.HeaderRID, colorKey, childColorQtyToAllocate, childColorQtyAllocated);
//						// load hdr workarea with size info
//						int [] colorSizeKeys = childAp.GetBulkColorSizeKeys(colorKey);
//						foreach (int sizeKey in colorSizeKeys)
//						{
//							//int sizeQtyAllocated = childAp.GetSizeUnitsAllocated(colorKey, sizeKey);
//							//hdrWa.AddSizeQtyAllocated(sizeKey, sizeQtyAllocated);
//							int sizeQtyToAllocate = childAp.GetSizeUnitsToAllocate(colorKey, sizeKey);
//							hdrWa.AddSizeQtyToAllocate(sizeKey, sizeQtyToAllocate);
//						}
//						colorWa.AddHeaderWorkArea(childAp.HeaderRID, hdrWa);
//					}
//				}
//				return colorWa;
//			}
//			catch
//			{
//				throw;
//			}
//		
//		}


	}
	#endregion

	#region  MultiHeaderColorIntransitWorkArea
//	public class MultiHeaderColorIntransitWorkArea
//	{
//		private int _colorRid;
//		private int _mhQtyToAllocate;  // 'mh' stands for Multi Header
//		private int _mhQtyAllocated; 
//		private Hashtable _mhSizeQtyToAllocateHash;  // key is size, value is MH qty to allocate for that size
//		private Hashtable _mhSizeQtyAllocatedHash;  // key is size, value is MH qty allocated for that size
//		private Hashtable _headerHash;
//
//		/// <summary>
//		/// Class that takes the a Multi header and splits the Intransit between
//		/// all of the child header's belonging to it.
//		/// </summary>
//		/// <param name="colorRid"></param>
//		/// <param name="multiHeaderQty"></param>
//		public MultiHeaderColorIntransitWorkArea(int colorRid, int qtyToAllocate, int qtyAllocated)
//		{
//			_colorRid = colorRid;
//			_mhQtyToAllocate = qtyToAllocate;
//			_mhQtyAllocated = qtyAllocated;
//			_mhSizeQtyToAllocateHash = new Hashtable();
//			_mhSizeQtyAllocatedHash = new Hashtable();
//			_headerHash = new Hashtable();
//		}
//
//		public void AddSizeQtyToAllocate(int sizeKey, int qtyToAllocate)
//		{
//			_mhSizeQtyToAllocateHash.Add(sizeKey, qtyToAllocate);
//		}
//
//		public void AddSizeQtyAllocated(int sizeKey, int qtyAllocated)
//		{
//			_mhSizeQtyAllocatedHash.Add(sizeKey, qtyAllocated);
//		}
//
//		public void AddHeaderWorkArea(int headerKey, MultiHeaderHeaderIntransitWorkArea wa)
//		{
//			_headerHash.Add(headerKey, wa);
//		}
//	}
	#endregion

	#region MultiHeaderHeaderIntransitWorkArea
//	public class MultiHeaderHeaderIntransitWorkArea
//	{
//		private int _headerRid;
//		private int _colorRid;
//		private int _qtyToAllocate;
//		private int _qtyAllocated;  
//		private Hashtable _sizeQtyToAllocateHash;  // key is size, value is hdr qty allocated for that size
//
//		/// <summary>
//		/// Class that takes the a Multi header and splits the Intransit between
//		/// all of the child header's belonging to it.
//		/// </summary>
//		/// <param name="aSab"></param>
//		/// <param name="aTrans"></param>
//		/// <param name="allocProfile"></param>
//		public MultiHeaderHeaderIntransitWorkArea(int headerRid, int colorRid, int qtyToAllocate, int qtyAllocated)
//		{
//			_headerRid = headerRid;
//			_colorRid = colorRid;
//			_qtyToAllocate = qtyToAllocate;
//			_qtyToAllocate = qtyAllocated;
//
//			_sizeQtyToAllocateHash = new Hashtable();
//		}
//
//		public void AddSizeQtyToAllocate(int sizeKey, int qtyToAllocate)
//		{
//			_sizeQtyToAllocateHash.Add(sizeKey, qtyToAllocate);
//		}
//	}
	#endregion
}
