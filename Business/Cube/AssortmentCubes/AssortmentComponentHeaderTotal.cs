using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	public class AssortmentComponentHeaderTotal : AssortmentComponentTotalCube
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentCube, using the given SessionAddressBlock, Transaction, AssortmentCubeGroup, CubeDefinition, and ComputationProcessor.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aAssrtCubeGroup">
		/// A reference to a AssortmentCubeGroup that this AssortmentCube is a part of.
		/// </param>
		/// <param name="aCubeDefinition">
		/// The CubeDefinition that describes the structure of this AssortmentCube.
		/// </param>

		public AssortmentComponentHeaderTotal(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
			: base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, PlanCubeAttributesFlagValues.GroupTotal, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eCubeType of this cube.
		/// </summary>

		public override eCubeType CubeType
		{
			get
			{
				return eCubeType.AssortmentComponentHeaderTotal;
			}
		}

		/// <summary>
		/// Returns the eProfileType of the Header dimension
		/// </summary>

		public override eProfileType HeaderProfileType
		{
			get
			{
				return eProfileType.AllocationHeader;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that initializes the Cube.
		/// </summary>

		override public void InitializeCube()
		{
		}

        //Begin TT#2 - JScott - Assortment Planning - Phase 2
        ///// <summary>
        ///// Abstract method that returns the eCubeType of the corresponding Pack cube for this cube.
        ///// </summary>
        ///// <returns>
        ///// The eCubeType of the Sub-total cube.
        ///// </returns>

        //override public eCubeType GetDetailPackCubeType()
        //{
        //    return eCubeType.AssortmentHeaderPackDetail;
        //}

        //End TT#2 - JScott - Assortment Planning - Phase 2
        /// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Color cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetDetailColorCubeType()
		{
			return eCubeType.AssortmentHeaderColorDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Detail Group Level Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Detail Group Level Total cube.
		/// </returns>

		override public eCubeType GetComponentGroupLevelCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Detail Group Total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Detail Group Total cube.
		/// </returns>

		override public eCubeType GetComponentTotalCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Placeholder cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetComponentPlaceholderCubeType()
		{
			return eCubeType.AssortmentComponentPlaceholderTotal;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Header cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetComponentHeaderCubeType()
		{
			return eCubeType.AssortmentComponentHeaderTotal;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the summary cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetSummaryCubeType()
		{
			return eCubeType.AssortmentSummaryTotal;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Sub-total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetSubTotalCubeType()
		{
			return new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, AssortmentCubeGroup.NumberOfSummaryLevels);
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the total cube.
		/// </returns>

		override public eCubeType GetTotalCubeType()
		{
			return new eCubeType(eCubeType.cAssortmentComponentHeaderTotalSubTotal, 0);
		}

		/// <summary>
		/// Abstract method that returns the VariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		override public ComputationVariableProfile GetVariableProfile(ComputationCellReference aCompCellRef)
		{
			try
			{
				return (ComputationVariableProfile)MasterAssortmentTotalVariableProfileList.FindKey(aCompCellRef[eProfileType.AssortmentTotalVariable]);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a Cell is read.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// A AssortmentCellReference object that identifies the AssortmentCubeCell to read.
		/// </param>

		override public void ReadCell(AssortmentCellReference aAssrtCellRef)
		{
		}

		/// <summary>
		/// Returns a string describing the given PlanCellReference
		/// </summary>
		/// <param name="aCompCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A string describing the PlanCellReference.
		/// </returns>

		override public string GetCellDescription(ComputationCellReference aCompCellRef)
		{
			AssortmentCellReference AssortCellRef;
			//HierarchyNodeProfile nodeProf;
			//VersionProfile versProf;
			AssortmentTotalVariableProfile totalVarProf;
			QuantityVariableProfile quantityVarProf;

			try
			{
				AssortCellRef = (AssortmentCellReference)aCompCellRef;
				//nodeProf = GetHierarchyNodeProfile(planCellRef);
				//versProf = GetVersionProfile(planCellRef);
				totalVarProf = (AssortmentTotalVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentTotalVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentTotalVariable]);
				quantityVarProf = (QuantityVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentQuantityVariable]);

				return "Assortment Component Header Detail Group Level" +
					", Total Variable \"" + totalVarProf.VariableName + "\"" +
					", Quanitity Variable \"" + quantityVarProf.VariableName + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated
        // NOTE: I thought I might need this for TT#3884, but didn't. I wanted to keep the code for future in case it was needed.
        //public void ReadAndLoadCube(bool reload)	// TT#2 - stodd
        //{
        //    ComponentProfileXRef compXRef;
        //    AssortmentCellReference cellRef;
        //    ArrayList headerList = null;
        //    bool headerPackExists = false;
        //    bool headerPackColorExists = false;

        //    //_loadingAssortmentData = true;
        //    try
        //    {
        //        compXRef = (ComponentProfileXRef)AssortmentCubeGroup.GetProfileXRef(new ComponentProfileXRef(this.CubeType));

        //        if (compXRef != null)
        //        {

        //            foreach (AllocationHeaderProfile hdrProf in AssortmentCubeGroup.GetReceiptList())
        //            {
        //                cellRef = (AssortmentCellReference)CreateCellReference();
        //                cellRef[eProfileType.AllocationHeader] = hdrProf.Key;

        //                if (cellRef.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader) > -1)
        //                {
        //                    cellRef[eProfileType.AllocationHeader] = hdrProf.Key;
        //                    headerList = (ArrayList)compXRef.GetTotalList(cellRef);
        //                }

        //                if (headerList != null)
        //                {
        //                    foreach (Dictionary<eProfileType, int> profKeyHash in headerList)
        //                    {
        //                        if ((int)profKeyHash[eProfileType.AllocationHeader] != int.MaxValue)
        //                        {
        //                            cellRef[eProfileType.AllocationHeader] = (int)profKeyHash[eProfileType.AllocationHeader];
        //                            if (cellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPack) > -1)
        //                            {
        //                                headerPackExists = true;
        //                                cellRef[eProfileType.HeaderPack] = (int)profKeyHash[eProfileType.HeaderPack];
        //                            }
        //                            if (cellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor) > -1)
        //                            {
        //                                headerPackColorExists = true;
        //                                cellRef[eProfileType.HeaderPackColor] = (int)profKeyHash[eProfileType.HeaderPackColor];
        //                            }

        //                            cellRef[eProfileType.AssortmentQuantityVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;

        //                            AssortmentTotalVariableProfile varProf = (AssortmentTotalVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentTotalVariables.VariableProfileList.FindKey((int)eAssortmentTotalVariables.Balance);

        //                            cellRef[eProfileType.AssortmentTotalVariable] = varProf.Key;

        //                            AllocationProfile ap = this.Transaction.GetAssortmentMemberProfile(cellRef[eProfileType.AllocationHeader]);
        //                            double value = 0;
        //                            if (ap != null)
        //                            {
        //                                if (headerPackExists && cellRef[eProfileType.HeaderPack] != int.MaxValue)
        //                                {
        //                                    int packRID = cellRef[eProfileType.HeaderPack];
        //                                    PackHdr packHdr = ap.GetPackHdr(packRID);

        //                                    if (headerPackColorExists && cellRef[eProfileType.HeaderPackColor] != int.MaxValue)
        //                                    {
        //                                        // pack and color
        //                                        //value = ap.GetAllocatedBalance(cellRef[eProfileType.HeaderPack], cellRef[eProfileType.HeaderPackColor], storeList);
        //                                    }
        //                                    else
        //                                    {
        //                                        // Pack
        //                                        value = ap.GetAllocatedBalance(packHdr);

        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (headerPackColorExists && cellRef[eProfileType.HeaderPackColor] != int.MaxValue)
        //                                    {
        //                                        // color
        //                                        value = ap.GetAllocatedBalance(cellRef[eProfileType.HeaderPackColor]);

        //                                    }
        //                                    else
        //                                    {
        //                                        // no pack, no color
        //                                        value = ap.GetAllocatedBalance(eAllocationSummaryNode.Total);

        //                                    }
        //                                }
        //                            }

        //                            //Debug.WriteLine("Read HEADER: " + cellRef[eProfileType.AllocationHeader] +
        //                            //    " Store Cnt: " + storeList.Count +
        //                            //    " SGL: " + cellRef[eProfileType.StoreGroupLevel] + " " +
        //                            //    " grade: " + cellRef[eProfileType.StoreGrade] + " " +
        //                            //    " pack: " + cellRef[eProfileType.HeaderPack] + " " +
        //                            //    " color: " + cellRef[eProfileType.HeaderPackColor] + " " +
        //                            //    " val: " + value);

        //                            cellRef.SetLoadCellValue(value, false);
        //                            cellRef.isCellLoadedFromDB = true;


        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //    finally
        //    {
        //        //_loadingAssortmentData = false;
        //    }
        //}
        // End TT#3884 - stodd - Add Balance column in Group Allocation Matrix for the remaining units to be allocated

		// Begin TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units
        /// <summary>
        /// Saves values in the Cube.
        /// </summary>
        /// <param name="aResetChangeFlags">
        /// A boolean indicating if the Cell ChangeFlags should be reset after writing.
        /// </param>

        public void SaveCube(bool aResetChangeFlags)
        {
            ComponentProfileXRef compXRef;
            AssortmentCellReference cellRef;
            ArrayList headerList = null;	// TT#3871 - error when exiting size review - 
			// Begin TT#1224-MD - stodd - Closing or Saving a group allocation can sometimes cause a "Key Not Found" exception - 
            bool headerPackExists = false;
            bool headerPackColorExists = false;
			// End TT#1224-MD - stodd - Closing or Saving a group allocation can sometimes cause a "Key Not Found" exception - 
            try
            {

                try
                {
                    AssortmentCubeGroup.AssortmentDetailData.Variable_XMLInit();
                    compXRef = (ComponentProfileXRef)AssortmentCubeGroup.GetProfileXRef(new ComponentProfileXRef(this.CubeType));

                    if (compXRef != null)
                    {
                        cellRef = (AssortmentCellReference)CreateCellReference();
                        int styleIndex = cellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.Day);
                        int parentOfStyleIndex = cellRef.Cube.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.BasisWeek);

                        if (cellRef.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader) > -1)
                        {
                            foreach (AllocationHeaderProfile hdrProf in AssortmentCubeGroup.GetReceiptList())
                            {
                                //cellRef = (AssortmentCellReference)CreateCellReference();
                                // Begin TT#3871 - stodd - Error when exiting size review
                                if (cellRef.GetDimensionProfileTypeIndex(eProfileType.AllocationHeader) > -1)
                                {
                                    cellRef[eProfileType.AllocationHeader] = hdrProf.Key;
                                    headerList = (ArrayList)compXRef.GetTotalList(cellRef);
                                }
                                // End TT#3871 - stodd - Error when exiting size revie

                                if (headerList != null)
                                {
                                    foreach (Dictionary<eProfileType, int> profKeyHash in headerList)
                                    {
                                        if ((int)profKeyHash[eProfileType.AllocationHeader] != int.MaxValue)
                                        {
                                            foreach (KeyValuePair<eProfileType, int> aCoord in profKeyHash)
                                            {
                                                // do something with entry.Value or entry.Key
                                                if (aCoord.Key != eProfileType.HeaderPack && aCoord.Key != eProfileType.HeaderPackColor && aCoord.Key != eProfileType.AllocationHeader)
                                                {
                                                    cellRef[aCoord.Key] = aCoord.Value;
                                                }
                                            }
                                            cellRef[eProfileType.AllocationHeader] = (int)profKeyHash[eProfileType.AllocationHeader];
                                            // Begin TT#1224-MD - stodd - Closing or Saving a group allocation can sometimes cause a "Key Not Found" exception - 
                                            if (cellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPack) > -1)
                                            {
                                                headerPackExists = true;
                                                cellRef[eProfileType.HeaderPack] = (int)profKeyHash[eProfileType.HeaderPack];
                                            }
                                            if (cellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor) > -1)
                                            {
                                                headerPackColorExists = true;
                                                cellRef[eProfileType.HeaderPackColor] = (int)profKeyHash[eProfileType.HeaderPackColor];
                                            }
                                            // End TT#1224-MD - stodd - Closing or Saving a group allocation can sometimes cause a "Key Not Found" exception - 
                                            cellRef[eProfileType.AssortmentQuantityVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;

                                            Index_RID reserveStore = this.Transaction.ReserveStore;
                                            StoreProfile reserveStoreProfile = ((AssortmentCubeGroup)this._cubeGroup).GetStore(reserveStore.RID);

                                            cellRef[eProfileType.AssortmentTotalVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentTotalVariables.VariableProfileList.FindKey((int)eAssortmentTotalVariables.ReserveUnits).Key;

                                            if (cellRef.isCellValid && cellRef.doesCellExist && cellRef.isCellChanged)  // TT#3769 - stodd - invalid logical coordinate error
                                            {
                                                AllocationProfile ap = this.Transaction.GetAssortmentMemberProfile(cellRef[eProfileType.AllocationHeader]);
                                                if (ap != null)
                                                {

                                                    if (headerPackExists && cellRef[eProfileType.HeaderPack] != int.MaxValue)	// TT#1224-MD - stodd - Closing or Saving a group allocation can sometimes cause a "Key Not Found" exception - 
                                                    {
                                                        if (headerPackColorExists && cellRef[eProfileType.HeaderPackColor] != int.MaxValue)	// TT#1224-MD - stodd - Closing or Saving a group allocation can sometimes cause a "Key Not Found" exception - 
                                                        {
                                                            // pack and color
                                                            PackHdr packHdr = (PackHdr)ap.GetPackHdr(cellRef[eProfileType.HeaderPack]);
                                                            int packUnits = (int)cellRef.CurrentCellValue;
                                                            int packRemainder = packUnits % packHdr.PackMultiple;
                                                            if (packRemainder > 0)
                                                            {
                                                                packUnits = (packUnits / packHdr.PackMultiple) * packHdr.PackMultiple;
                                                            }
                                                            int packs = packUnits / packHdr.PackMultiple;
                                                            ap.SetStoreQtyAllocated(packHdr.PackName, reserveStoreProfile.Key, packs);
                                                        }
                                                        else
                                                        {
                                                            // Pack
                                                            PackHdr packHdr = (PackHdr)ap.GetPackHdr(cellRef[eProfileType.HeaderPack]);
                                                            int packUnits = (int)cellRef.CurrentCellValue;
                                                            int packRemainder = packUnits % packHdr.PackMultiple;
                                                            if (packRemainder > 0)
                                                            {
                                                                packUnits = (packUnits / packHdr.PackMultiple) * packHdr.PackMultiple;
                                                            }
                                                            int packs = packUnits / packHdr.PackMultiple;
                                                            ap.SetStoreQtyAllocated(packHdr.PackName, reserveStoreProfile.Key, packs);
                                                            // BEGIN Debugging
                                                            //string stores = string.Empty;
                                                            //for (int s = 0; s < storeList.Count; s++)
                                                            //{
                                                            //    stores += ((StoreProfile)storeList[s]).StoreId + ", ";
                                                            //}

                                                            //Debug.WriteLine("HDR " + cellRef[eProfileType.AllocationHeader] + " SGL " +
                                                            //    cellRef[eProfileType.StoreGroupLevel] + " GRD " +
                                                            //    cellRef[eProfileType.StoreGrade] + " PK " +
                                                            //    cellRef[eProfileType.HeaderPack] + " PKCL " +
                                                            //    cellRef[eProfileType.HeaderPackColor] + " VAL " +
                                                            //    cellRef.CurrentCellValue + " STRS " + stores);
                                                            // END Debugging
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (headerPackColorExists && cellRef[eProfileType.HeaderPackColor] != int.MaxValue)	// TT#1224-MD - stodd - Closing or Saving a group allocation can sometimes cause a "Key Not Found" exception - 
                                                        {
                                                            // color
                                                            ap.SetStoreQtyAllocated(cellRef[eProfileType.HeaderPackColor], reserveStoreProfile.Key, (int)cellRef.CurrentCellValue);
                                                        }
                                                        else
                                                        {
                                                            ap.SetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStoreProfile.Key, (int)cellRef.CurrentCellValue);

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
						// Begin TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
                        else
                        {
                            //List<int> styleList = new List<int>();
                            //List<int> parentOfStyleList = new List<int>();

                            //if (parentOfStyleIndex != -1)
                            //{
                            //    parentOfStyleList = this.AssortmentCubeGroup.GetParentOfStyleList();
                            //}

                            //if (styleIndex != -1)
                            //{
                            //    styleList = this.AssortmentCubeGroup.GetStyleList();
                            //}

                            ////AllocationProfileList apl = GetHeaderList(parentOfStyleList, styleList);

                            //if (parentOfStyleList.Count > 0)
                            //{
                            //    foreach (int aParentOfStyle in parentOfStyleList)
                            //    {
                            //        cellRef = (AssortmentCellReference)CreateCellReference();
                            //        cellRef[eProfileType.BasisWeek] = aParentOfStyle;
                            //        headerList = (ArrayList)compXRef.GetTotalList(cellRef);
                            //        SaveCubeByComponent(cellRef, headerList);
                            //    }
                            //}
                            //else if (styleList.Count > 0)
                            //{
                            //    foreach (int aStyle in styleList)
                            //    {
                            //        cellRef = (AssortmentCellReference)CreateCellReference();
                            //        cellRef[eProfileType.Day] = aStyle;
                            //        headerList = (ArrayList)compXRef.GetTotalList(cellRef);
                            //        SaveCubeByComponent(cellRef, headerList);
                            //    }
                            //}
                            //else if (cellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPack) > -1)
                            //{
                            //    headerList = (ArrayList)compXRef.GetTotalList(cellRef);
                            //    SaveCubeByComponent(cellRef, headerList);
                            //}
                            //else if (cellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor) > -1)
                            //{
                            //    headerList = (ArrayList)compXRef.GetTotalList(cellRef);
                            //    SaveCubeByComponent(cellRef, headerList);
                            //}

                        }
						// End TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix

                    }
                }

                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#1217-MD - stodd - Entering Reserve Units on the matrix grid does not populate the reserve store with those units

		// Begin TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
        private AllocationProfileList GetHeaderList(List<int> parentOfStyleList, List<int> styleList)
        {
            AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
            AllocationProfileList masterApl = (AllocationProfileList)this.Transaction.GetMasterProfileList(eProfileType.AssortmentMember);

            foreach (AllocationProfile ap in masterApl.ArrayList)
            {
                if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                {
                    continue;
                }

                apl.Add(ap);

                //HierarchyNodeProfile hnParent = ap.AppSessionTransaction.GetParentNodeData(ap.StyleHnRID);

                //if (!parentOfStyleList.Contains(hnParent.Key))
                //{
                //    parentOfStyleList.Add(hnParent.Key);
                //}
            }

            return apl;

        }

        private void SaveCubeByComponent(AssortmentCellReference cellRef, ArrayList headerList)
        {
            bool headerPackExists = false;
            bool headerPackColorExists = false;

            foreach (Dictionary<eProfileType, int> profKeyHash in headerList)
            {
                foreach (KeyValuePair<eProfileType, int> aCoord in profKeyHash)
                {
                    // do something with entry.Value or entry.Key
                    if (aCoord.Key != eProfileType.HeaderPack && aCoord.Key != eProfileType.HeaderPackColor)
                    {
                        cellRef[aCoord.Key] = aCoord.Value;
                    }
                }
                //if ((int)profKeyHash[eProfileType.AllocationHeader] != int.MaxValue)
                //{
                    //cellRef[eProfileType.AllocationHeader] = (int)profKeyHash[eProfileType.AllocationHeader];
                    if (cellRef.GetDimensionProfileTypeIndex(eProfileType.Day) > -1)
                    {
                        cellRef[eProfileType.Day] = (int)profKeyHash[eProfileType.Day];
                    }
                    if (cellRef.GetDimensionProfileTypeIndex(eProfileType.BasisWeek) > -1)
                    {
                        cellRef[eProfileType.BasisWeek] = (int)profKeyHash[eProfileType.BasisWeek];
                    }
                    if (cellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPack) > -1)
                    {
                        headerPackExists = true;
                        cellRef[eProfileType.HeaderPack] = (int)profKeyHash[eProfileType.HeaderPack];
                    }
                    if (cellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor) > -1)
                    {
                        headerPackColorExists = true;
                        cellRef[eProfileType.HeaderPackColor] = (int)profKeyHash[eProfileType.HeaderPackColor];
                    }
                    cellRef[eProfileType.AssortmentQuantityVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;

                    Index_RID reserveStore = this.Transaction.ReserveStore;
                    StoreProfile reserveStoreProfile = ((AssortmentCubeGroup)this._cubeGroup).GetStore(reserveStore.RID);

                    cellRef[eProfileType.AssortmentTotalVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentTotalVariables.VariableProfileList.FindKey((int)eAssortmentTotalVariables.ReserveUnits).Key;

                    if (cellRef.isCellValid && cellRef.doesCellExist && cellRef.isCellChanged)  // TT#3769 - stodd - invalid logical coordinate error
                    {
                        //AllocationProfile ap = this.Transaction.GetAssortmentMemberProfile(cellRef[eProfileType.AllocationHeader]);
                        //if (ap != null)
                        //{

                            if (headerPackExists && cellRef[eProfileType.HeaderPack] != int.MaxValue)	// TT#1224-MD - stodd - Closing or Saving a group allocation can sometimes cause a "Key Not Found" exception - 
                            {
                                if (headerPackColorExists && cellRef[eProfileType.HeaderPackColor] != int.MaxValue)	// TT#1224-MD - stodd - Closing or Saving a group allocation can sometimes cause a "Key Not Found" exception - 
                                {
                                    // pack and color
                                    //PackHdr packHdr = (PackHdr)ap.GetPackHdr(cellRef[eProfileType.HeaderPack]);
                                    //int packUnits = (int)cellRef.CurrentCellValue;
                                    //int packRemainder = packUnits % packHdr.PackMultiple;
                                    //if (packRemainder > 0)
                                    //{
                                    //    packUnits = (packUnits / packHdr.PackMultiple) * packHdr.PackMultiple;
                                    //}
                                    //int packs = packUnits / packHdr.PackMultiple;
                                    //ap.SetStoreQtyAllocated(packHdr.PackName, reserveStoreProfile.Key, packs);
                                }
                                else
                                {
                                    // Pack
                                    //PackHdr packHdr = (PackHdr)ap.GetPackHdr(cellRef[eProfileType.HeaderPack]);
                                    //int packUnits = (int)cellRef.CurrentCellValue;
                                    //int packRemainder = packUnits % packHdr.PackMultiple;
                                    //if (packRemainder > 0)
                                    //{
                                    //    packUnits = (packUnits / packHdr.PackMultiple) * packHdr.PackMultiple;
                                    //}
                                    //int packs = packUnits / packHdr.PackMultiple;

                                    //ap.SetStoreQtyAllocated(packHdr.PackName, reserveStoreProfile.Key, packs);
                                    // BEGIN Debugging
                                    //Debug.WriteLine("HDR " + cellRef[eProfileType.AllocationHeader] + " SGL " +
                                    //    cellRef[eProfileType.StoreGroupLevel] + " GRD " +
                                    //    cellRef[eProfileType.StoreGrade] + " PK " +
                                    //    cellRef[eProfileType.HeaderPack] + " PKCL " +
                                    //    cellRef[eProfileType.HeaderPackColor] + " VAL " +
                                    //    cellRef.CurrentCellValue + " STRS " + stores);
                                    // END Debugging
                                }
                            }
                            else
                            {
                                if (headerPackColorExists && cellRef[eProfileType.HeaderPackColor] != int.MaxValue)	// TT#1224-MD - stodd - Closing or Saving a group allocation can sometimes cause a "Key Not Found" exception - 
                                {
                                    // color
                                    int units = (int)cellRef.CurrentCellValue;
                                    int newValue = 0;
                                    AllocationProfileList matchApl = GetMatchingHeaders(cellRef);
                                    if (matchApl.Count > 0)
                                    {
                                        // Change to DO SPREAD!!!!
                                        newValue = units / matchApl.Count;
                                    }
                                    foreach (AllocationProfile ap in matchApl)
                                    {
                                        ap.SetStoreQtyAllocated(cellRef[eProfileType.HeaderPackColor], reserveStoreProfile.Key, newValue);
                                    }

                                    //ap.SetStoreQtyAllocated(cellRef[eProfileType.HeaderPackColor], reserveStoreProfile.Key, (int)cellRef.CurrentCellValue);
                                }
                                else
                                {
                                    //ap.SetStoreQtyAllocated(eAllocationSummaryNode.Total, reserveStoreProfile.Key, (int)cellRef.CurrentCellValue);

                                }
                            }
                        //}
                    }
                //}
            }
        }

        private AllocationProfileList GetMatchingHeaders(AssortmentCellReference cellRef)
        {

            AllocationProfileList apl = cellRef.AssortmentCube.Transaction.GetAssortmentMemberProfileList();

            if (cellRef.GetDimensionProfileTypeIndex(eProfileType.BasisWeek) > -1 && cellRef[eProfileType.BasisWeek] != int.MaxValue)
            {
                int parentOfStyleRid = cellRef[eProfileType.BasisWeek];
                apl = GetHeaderParentOfStyleMatch(parentOfStyleRid, apl);
            }

            if (cellRef.GetDimensionProfileTypeIndex(eProfileType.Day) > -1 && cellRef[eProfileType.Day] != int.MaxValue)
            {
                int styleRid = cellRef[eProfileType.Day];
                apl = GetHeaderStyleMatch(styleRid, apl);
            }

            int packRid = -1;
            int colorRid = -1;
            if (cellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor) > -1 && cellRef[eProfileType.HeaderPackColor] != int.MaxValue)
            {
                colorRid = cellRef[eProfileType.HeaderPackColor];
            }
            if (cellRef.GetDimensionProfileTypeIndex(eProfileType.HeaderPack) > -1 && cellRef[eProfileType.HeaderPack] != int.MaxValue)
            {
                packRid = cellRef[eProfileType.HeaderPack];
            }

            apl = GetHeaderPackColorMatch(packRid, colorRid, apl);

            apl = GetHeaderHnLevelMatch(cellRef, apl);

            return apl;
        }

        private AllocationProfileList GetHeaderHnLevelMatch(AssortmentCellReference cellRef, AllocationProfileList apl)
        {
            AllocationProfileList hnLevelList = new AllocationProfileList(eProfileType.AssortmentMember);

            for (int i = 0; i < cellRef.CellCoordinates.NumIndices; i++)
            {
                int key = cellRef.CellCoordinates.GetRawCoordinate(i);
                DimensionDefinition prof = cellRef.Cube.CubeDefinition[i];

                if (prof.ProfileType != eProfileType.AllocationHeader &&
                    prof.ProfileType != eProfileType.Day &&
                    prof.ProfileType != eProfileType.BasisWeek &&
                    prof.ProfileType != eProfileType.HeaderPackColor &&
                    prof.ProfileType != eProfileType.HeaderPack &&
                    prof.ProfileType != eProfileType.AssortmentQuantityVariable &&
                    prof.ProfileType != eProfileType.AssortmentTotalVariable)
                {
                    foreach (AllocationProfile ap in apl.ArrayList)
                    {
                        if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                        {
                            continue;
                        }

                        SortedList allAncestorList = ap.StyleAncestorHnRidList;

                        foreach (DictionaryEntry dictEnt in allAncestorList)
                        {
                            NodeAncestorList hierNal = (NodeAncestorList)dictEnt.Value;

                            foreach (NodeAncestorProfile nap in hierNal)
                            {
                                if (nap.Key == key)
                                {
                                    hnLevelList.Add(ap);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return hnLevelList;
        }

        private AllocationProfileList GetHeaderParentOfStyleMatch(int parentOfStyleRid, AllocationProfileList apl)
        {
            AllocationProfileList parentOfStyleList = new AllocationProfileList(eProfileType.AssortmentMember);

            foreach (AllocationProfile ap in apl.ArrayList)
            {
                if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                {
                    continue;
                }

                HierarchyNodeProfile hnParent = ap.AppSessionTransaction.GetParentNodeData(ap.StyleHnRID);

                if (hnParent.Key == parentOfStyleRid)
                {
                    parentOfStyleList.Add(ap);
                }
            }

            return parentOfStyleList;
        }

        private AllocationProfileList GetHeaderStyleMatch(int styleRid, AllocationProfileList apl)
        {
            AllocationProfileList styleList = new AllocationProfileList(eProfileType.AssortmentMember);


            foreach (AllocationProfile ap in apl.ArrayList)
            {
                if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                {
                    continue;
                }

                if (ap.StyleHnRID == styleRid)
                {
                    styleList.Add(ap);
                }
            }

            return styleList;
        }

        private AllocationProfileList GetHeaderPackColorMatch(int packRid, int colorRid, AllocationProfileList apl)
        {
            AllocationProfileList newApl = new AllocationProfileList(eProfileType.Allocation);

            // PACK
            if (packRid != -1)
            {

                foreach (AllocationProfile ap in apl.ArrayList)
                {
                    if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                    {
                        continue;
                    }

                    PackHdr packHdr = ap.GetPackHdr(packRid);

                    if (packHdr != null)
                    {
                        newApl.Add(ap);                    
                    }
                }
            }
            // COLOR
            else if (colorRid != -1)
            {
                foreach (AllocationProfile ap in apl.ArrayList)
                {
                    if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                    {
                        continue;
                    }

                    if (ap.BulkColorIsOnHeader(colorRid))
                    {
                        newApl.Add(ap);
                    }
                }
            }
            // TOTAL
            else
            {
                foreach (AllocationProfile ap in apl.ArrayList)
                {
                    if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                    {
                        continue;
                    }
                    newApl.Add(ap);
                }
            }


            return newApl;
        }
		// End TT#3906 - stodd - Reserve Units disappear when Header ID is removed from Matrix
	}
}