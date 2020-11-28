using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using System.Text;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	public class AssortmentHeaderColorDetail : AssortmentCube
	{
		//=======
		// FIELDS
		//=======

		private bool _loadingAssortmentData;

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

		public AssortmentHeaderColorDetail(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
			: base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, 0, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
			_loadingAssortmentData = false;
			LoadValuesFromHeader = true;	// TT#2 - stodd - assortment
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eCubeType of this cube.
		/// </summary>

		override public eCubeType CubeType
		{
			get
			{
				return eCubeType.AssortmentHeaderColorDetail;
			}
		}

		/// <summary>
		/// Abstract property returns the eProfileType for the VariableProfile object for the Cell specified by the ComputationCellReference.
		/// </summary>
		/// <remarks>
		/// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
		/// </remarks>

		override public eProfileType VariableProfileType
		{
			get
			{
				return eProfileType.AssortmentDetailVariable;
			}
		}

		/// <summary>
		/// Returns the eProfileType of the Header dimension
		/// </summary>

		override public eProfileType HeaderProfileType
		{
			get
			{
				return eProfileType.AllocationHeader;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if data is being loaded to the cube
		/// </summary>

		public bool LoadingAssortmentData
		{
			get
			{
				return _loadingAssortmentData;
			}
		}

		//========
		// METHODS
		//========

		public override CellCoordinates CreateCellCoordinates(int aNumIndices)
		{
			try
			{
				return new AssortmentHeaderColorDetailCellCoordinates(aNumIndices, this);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

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
			return eCubeType.AssortmentPlaceholderColorDetail;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Header cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetComponentHeaderCubeType()
		{
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the summary cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetSummaryCubeType()
		{
			// Begin TT#2 - stodd
			//return eCubeType.None;
			return eCubeType.AssortmentSummaryGrade;
			// End TT#2 - stodd
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the Sub-total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetSubTotalCubeType()
		{
			// Begin TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
			//return eCubeType.None;
            return eCubeType.AssortmentComponentPlaceholderGrade;
			// End TT#3750 - stodd - On the matrix grid, "Total %" is not locking during a cascade lock.
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the total cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the total cube.
		/// </returns>

		override public eCubeType GetTotalCubeType()
		{
			return eCubeType.None;
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
				return (ComputationVariableProfile)MasterAssortmentDetailVariableProfileList.FindKey(aCompCellRef[eProfileType.AssortmentDetailVariable]);
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
			aAssrtCellRef.isCellLoadedFromDB = true;
		}

		/// <summary>
		/// Allows a cube to specify custom initializations for the Hidden flag.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to initialize.
		/// </param>

		override public void InitCellValue(ComputationCellReference aCompCellRef)
		{
			AssortmentCellReference asrtCellRef;
			// Begin TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error. 
			//ArrayList plcCellRefList;
			//int plchldRID;
			// End TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error. 
			
			try
			{
				base.InitCellValue(aCompCellRef);

				asrtCellRef = (AssortmentCellReference)aCompCellRef;
				// Begin TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error. 
				//plcCellRefList = asrtCellRef.GetTotalCellRefArray(eCubeType.AssortmentPlaceholderColorDetail);
				//plchldRID = ((CellReference)plcCellRefList[0])[eProfileType.PlaceholderHeader];
				//asrtCellRef.isCellBlocked = asrtCellRef.AssortmentCube.AssortmentCubeGroup.isPlaceholderBlocked(plchldRID, asrtCellRef[eProfileType.StoreGroupLevel], asrtCellRef[eProfileType.StoreGrade]);
				//asrtCellRef.isCellDisplayOnly |= asrtCellRef.AssortmentCube.AssortmentCubeGroup.isPackColorReadOnly(plchldRID, asrtCellRef[eProfileType.AllocationHeader], asrtCellRef[eProfileType.HeaderPack], asrtCellRef[eProfileType.HeaderPackColor]);
                asrtCellRef.isCellBlocked = asrtCellRef.AssortmentCube.AssortmentCubeGroup.isPlaceholderBlocked(asrtCellRef[eProfileType.AllocationHeader], asrtCellRef[eProfileType.StoreGroupLevel], asrtCellRef[eProfileType.StoreGrade]);
                asrtCellRef.isCellDisplayOnly |= asrtCellRef.AssortmentCube.AssortmentCubeGroup.isPackColorReadOnly(asrtCellRef[eProfileType.AllocationHeader], asrtCellRef[eProfileType.AllocationHeader], asrtCellRef[eProfileType.HeaderPack], asrtCellRef[eProfileType.HeaderPackColor]);
				// End TT#1491-MD - stodd - open a post receipt asst- run need on the headers- go to matrix and close a style- receive and MID exception error. 

				// Begin TT#1205-MD - stodd - Lock graphic on Matrix Grid disappears after running an action- 
                asrtCellRef.isCellLocked = asrtCellRef.AssortmentCube.AssortmentCubeGroup.isHeaderLocked(
                    asrtCellRef[eProfileType.AllocationHeader],
                    asrtCellRef[eProfileType.HeaderPack],
                    asrtCellRef[eProfileType.HeaderPackColor],
                    asrtCellRef[eProfileType.StoreGroupLevel],
                    asrtCellRef[eProfileType.StoreGrade]);
				// End TT#1205-MD - stodd - Lock graphic on Matrix Grid disappears after running an action- 

				// Begin TT#1204-MD - stodd - Provide a message within Matrix when there are no stores available to spread to -
                // Determines if all stores are "out'd" for the cell
                asrtCellRef.isCellFixed = asrtCellRef.AssortmentCube.AssortmentCubeGroup.isCellFixed(
                    asrtCellRef[eProfileType.AllocationHeader],
                    asrtCellRef[eProfileType.HeaderPack],
                    asrtCellRef[eProfileType.HeaderPackColor],
                    asrtCellRef[eProfileType.StoreGroupLevel],
                    asrtCellRef[eProfileType.StoreGrade]);
				// End TT#1204-MD - stodd - Provide a message within Matrix when there are no stores available to spread to -

                //Debug.WriteLine("InitCellValue HEADER: " + asrtCellRef[eProfileType.AllocationHeader] +
                //            " qVar: " + asrtCellRef[eProfileType.AssortmentQuantityVariable] +
                //            " dVar: " + asrtCellRef[eProfileType.AssortmentDetailVariable] +
                //            " SGL: " + asrtCellRef[eProfileType.StoreGroupLevel] + " " +
                //            " grade: " + asrtCellRef[eProfileType.StoreGrade] + " " +
                //            " pack: " + asrtCellRef[eProfileType.HeaderPack] + " " +
                //            " color: " + asrtCellRef[eProfileType.HeaderPackColor] + " " +
                //            " val: " + asrtCellRef.CurrentCellValue +
                //            " Locked: " + asrtCellRef.isCellLocked);

			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN TT#2 - stodd
		public void ReadAndLoadCube()
		{
			ReadAndLoadCube(false);
		}		
		// END TT#2 - stodd

		/// <summary>
		/// Reads all values into the Cube.
		/// </summary>

		public void ReadAndLoadCube(bool reload)	// TT#2 - stodd
		{
			ComponentProfileXRef compXRef;
			AssortmentCellReference cellRef;
			ArrayList headerList;
			ProfileList sglList;
			ProfileList gradeList;

			_loadingAssortmentData = true;
			try
			{
				compXRef = (ComponentProfileXRef)AssortmentCubeGroup.GetProfileXRef(new ComponentProfileXRef(this.CubeType));

				if (compXRef != null)
				{
					sglList = AssortmentCubeGroup.GetMasterProfileList(eProfileType.StoreGroupLevel);
					gradeList = AssortmentCubeGroup.GetMasterProfileList(eProfileType.StoreGrade);

					foreach (AllocationHeaderProfile hdrProf in AssortmentCubeGroup.GetReceiptList())
					{
						cellRef = (AssortmentCellReference)CreateCellReference();
						cellRef[eProfileType.AllocationHeader] = hdrProf.Key;
						headerList = (ArrayList)compXRef.GetTotalList(cellRef);

						if (headerList != null)
						{
							foreach (Profile sglProf in sglList)
							{
								cellRef[eProfileType.StoreGroupLevel] = sglProf.Key;
								StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sglProf;

								foreach (Profile gradeProf in gradeList)
								{
									cellRef[eProfileType.StoreGrade] = gradeProf.Key;
									// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
									//foreach (Hashtable profKeyHash in headerList)
									foreach (Dictionary<eProfileType, int> profKeyHash in headerList)
									// END TT#773-MD - Stodd - replace hashtable with dictionary
									{
										if ((int)profKeyHash[eProfileType.AllocationHeader] != int.MaxValue)
										{
											cellRef[eProfileType.AllocationHeader] = (int)profKeyHash[eProfileType.AllocationHeader];
											cellRef[eProfileType.HeaderPack] = (int)profKeyHash[eProfileType.HeaderPack];
											cellRef[eProfileType.HeaderPackColor] = (int)profKeyHash[eProfileType.HeaderPackColor];
											cellRef[eProfileType.AssortmentQuantityVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
											ProfileList storeList = ((AssortmentCubeGroup)this._cubeGroup).GetStoresInSetGrade(cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade]);
											//Debug.WriteLine(cellRef[eProfileType.StoreGroupLevel] + " " + cellRef[eProfileType.StoreGrade]);
											//foreach (StoreProfile sp in storeList.ArrayList)
											//{
											//	Debug.Write(sp.StoreId + "(" + sp.Key + ") ");
											//}
											//Debug.WriteLine(" ");

											foreach (AssortmentDetailVariableProfile varProf in AssortmentCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList)
											{
												if (varProf.DatabaseColumnName != null)
												{
													cellRef[eProfileType.AssortmentDetailVariable] = varProf.Key;

													if (!cellRef.isCellLoadedFromDB || reload)
													{
														// BEGIN TT#779-MD - Stodd - New placeholder does not hold total units
														//AllocationProfile ap = this.Transaction.GetAllocationProfile(cellRef[eProfileType.AllocationHeader]);
														AllocationProfile ap = this.Transaction.GetAssortmentMemberProfile(cellRef[eProfileType.AllocationHeader]);
														// END TT#779-MD - Stodd - New placeholder does not hold total units
														double value = 0;
														// Begin TT#1282 - stodd - assortment
														if (ap != null)
														{
															if (cellRef[eProfileType.HeaderPack] != int.MaxValue)
															{
																if (cellRef[eProfileType.HeaderPackColor] != int.MaxValue)
																{
																	// pack and color
																	value = ap.GetAllocatedUnits(cellRef[eProfileType.HeaderPack], cellRef[eProfileType.HeaderPackColor], storeList);
																	//value = ap.GetAllocatedUnits(cellRef[eProfileType.HeaderPack], cellRef[eProfileType.HeaderPackColor], sglp.GroupRid, cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade]);
																}
																else
																{
																	// Pack
																	value = ap.GetAllocatedPackUnits(cellRef[eProfileType.HeaderPack], storeList);
																	//value = ap.GetAllocatedPackUnits(cellRef[eProfileType.HeaderPack], sglp.GroupRid, cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade]);

																}
															}
															else
															{
																if (cellRef[eProfileType.HeaderPackColor] != int.MaxValue)
																{
																	// color
																	value = ap.GetAllocatedColorUnits(cellRef[eProfileType.HeaderPackColor], storeList);
																	//value = ap.GetAllocatedColorUnits(cellRef[eProfileType.HeaderPackColor], sglp.GroupRid, cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade]);

																}
																else
																{
																	// no pack, no color
																	value = ap.GetAllocatedUnits(storeList);
																	//value = ap.GetAllocatedUnits(sglp.GroupRid, cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade]);

																}
															}
														}
														// End TT#1282 - stodd - assortment

                                                        //Debug.WriteLine("Read HEADER: " + cellRef[eProfileType.AllocationHeader] +
                                                        //    " Store Cnt: " + storeList.Count +
                                                        //    " SGL: " + cellRef[eProfileType.StoreGroupLevel] + " " +
                                                        //    " grade: " + cellRef[eProfileType.StoreGrade] + " " +
                                                        //    " pack: " + cellRef[eProfileType.HeaderPack] + " " +
                                                        //    " color: " + cellRef[eProfileType.HeaderPackColor] + " " +
                                                        //    " val: " + value);

														cellRef.SetLoadCellValue(value, false);
														cellRef.isCellLoadedFromDB = true;

                                                        //Cube cube2 = AssortmentCubeGroup.GetCube(eCubeType.AssortmentComponentHeaderGradeSubTotal);
                                                        //AssortmentCellReference cellRef2 = (AssortmentCellReference)cube2.CreateCellReference();
                                                        //if (cellRef2.GetDimensionProfileTypeIndex(eProfileType.HeaderPackColor) > -1)
                                                        //{
                                                        //    cellRef2[eProfileType.HeaderPackColor] = cellRef[eProfileType.HeaderPackColor];
                                                        //}
                                                        ////cellRef2[eProfileType.AllocationHeader] = hdrProf.Key;
                                                        //cellRef2[eProfileType.StoreGroupLevel] = sglProf.Key;
                                                        //cellRef2[eProfileType.StoreGrade] = gradeProf.Key;
                                                        //cellRef2[eProfileType.AssortmentQuantityVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
                                                        //cellRef2[eProfileType.AssortmentDetailVariable] = (int)eAssortmentDetailVariables.TotalPct;

                                                        //Debug.WriteLine("Read HEADER2: " + 
                                                        //    " SGL: " + cellRef2[eProfileType.StoreGroupLevel] + " " +
                                                        //    " grade: " + cellRef2[eProfileType.StoreGrade] + " " +
                                                        //    " Lock: " + cellRef2.isCellLocked +
                                                        //    " val: " + cellRef2.CurrentCellValue);

                                                        //Cube cube3 = AssortmentCubeGroup.GetCube(eCubeType.AssortmentComponentHeaderGrade);
                                                        //AssortmentCellReference cellRef3 = (AssortmentCellReference)cube3.CreateCellReference();
                                                        //cellRef3[eProfileType.AllocationHeader] = hdrProf.Key;
                                                        //cellRef3[eProfileType.StoreGroupLevel] = sglProf.Key;
                                                        //cellRef3[eProfileType.StoreGrade] = gradeProf.Key;
                                                        //cellRef3[eProfileType.AssortmentQuantityVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
                                                        //cellRef3[eProfileType.AssortmentDetailVariable] = varProf.Key;

                                                        //Debug.WriteLine("Read HEADER3: " + cellRef3[eProfileType.AllocationHeader] +
                                                        //    " SGL: " + cellRef3[eProfileType.StoreGroupLevel] + " " +
                                                        //    " grade: " + cellRef3[eProfileType.StoreGrade] + " " +
                                                        //    " val: " + cellRef3.CurrentCellValue);
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
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			finally
			{
				_loadingAssortmentData = false;
			}
		}

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
			ArrayList headerList;
            ArrayList removedHeaderList; // TT#2 - RMatelic - Assortment Planning-ASSORTMENT_MATRIX_DETAIL database rows not deleted when header removed from assortment
			ProfileList sglList;
			ProfileList gradeList;
			int writeCount;

			try
			{
				AssortmentCubeGroup.AssortmentDetailData.OpenUpdateConnection();

				try
				{
					AssortmentCubeGroup.AssortmentDetailData.Variable_XMLInit();
					compXRef = (ComponentProfileXRef)AssortmentCubeGroup.GetProfileXRef(new ComponentProfileXRef(this.CubeType));

					if (compXRef != null)
					{
						sglList = AssortmentCubeGroup.GetMasterProfileList(eProfileType.StoreGroupLevel);
						gradeList = AssortmentCubeGroup.GetMasterProfileList(eProfileType.StoreGrade);
						writeCount = 0;
                        removedHeaderList = new ArrayList(); // TT#2 - RMatelic - Assortment Planning-ASSORTMENT_MATRIX_DETAIL database rows not deleted when header removed from assortment
						foreach (AllocationHeaderProfile hdrProf in AssortmentCubeGroup.GetReceiptList())
						{
							cellRef = (AssortmentCellReference)CreateCellReference();
							cellRef[eProfileType.AllocationHeader] = hdrProf.Key;
							headerList = (ArrayList)compXRef.GetTotalList(cellRef);

							if (headerList != null)
							{
								foreach (Profile sglProf in sglList)
								{
									cellRef[eProfileType.StoreGroupLevel] = sglProf.Key;
									StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sglProf;

									foreach (Profile gradeProf in gradeList)
									{
										cellRef[eProfileType.StoreGrade] = gradeProf.Key;
										// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
										//foreach (Hashtable profKeyHash in headerList)
										foreach (Dictionary<eProfileType, int> profKeyHash in headerList)
										// END TT#773-MD - Stodd - replace hashtable with dictionary
										{
											if ((int)profKeyHash[eProfileType.AllocationHeader] != int.MaxValue)
											{
												cellRef[eProfileType.AllocationHeader] = (int)profKeyHash[eProfileType.AllocationHeader];
												cellRef[eProfileType.HeaderPack] = (int)profKeyHash[eProfileType.HeaderPack];
												cellRef[eProfileType.HeaderPackColor] = (int)profKeyHash[eProfileType.HeaderPackColor];
												cellRef[eProfileType.AssortmentQuantityVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
												ProfileList storeList = ((AssortmentCubeGroup)this._cubeGroup).GetStoresInSetGrade(cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade]);

												foreach (AssortmentDetailVariableProfile varProf in AssortmentCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList)
												{
													if (varProf.DatabaseColumnName != null)
													{
														cellRef[eProfileType.AssortmentDetailVariable] = varProf.Key;

														// temp change stodd
                                                        if (cellRef.doesCellExist && cellRef.isCellChanged)
                                                        //if (cellRef.doesCellExist)
														{
															AssortmentCubeGroup.AssortmentDetailData.AssortmentMatrixDetail_XMLInsert(
																cellRef[eProfileType.AllocationHeader],
																cellRef[eProfileType.HeaderPack],
																cellRef[eProfileType.HeaderPackColor],
																cellRef[eProfileType.StoreGroupLevel],
																cellRef[eProfileType.StoreGrade],
																cellRef.CurrentCellValue, cellRef.isCellLocked);

															writeCount++;

															if (writeCount > MIDConnectionString.CommitLimit)
															{
																AssortmentCubeGroup.AssortmentDetailData.AssortmentMatrixDetail_XMLUpdate();
																AssortmentCubeGroup.AssortmentDetailData.Variable_XMLInit();
																writeCount = 0;
															}
                                                            // Begin TT#2 - RMatelic -Assortment Planning- allocate headers attached to placeholders
                                                            //AllocationProfile ap = this.Transaction.GetAllocationProfile(cellRef[eProfileType.PlaceholderHeader]);
															// BEGIN TT#779-MD - Stodd - New placeholder does not hold total units
                                                            //AllocationProfile ap = this.Transaction.GetAllocationProfile(cellRef[eProfileType.AllocationHeader]);
															AllocationProfile ap = this.Transaction.GetAssortmentMemberProfile(cellRef[eProfileType.AllocationHeader]);
															// END TT#779-MD - Stodd - New placeholder does not hold total units
//
                                                            // End TT#2

															// Begin TT#1227 - stodd
                                                            if (ap != null)
                                                            {

                                                                if (cellRef[eProfileType.HeaderPack] != int.MaxValue)
                                                                {
                                                                    if (cellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                                                                    {
                                                                        // pack and color
																		// BEGIN TT#2046 - stodd - pack units keep increasing
																		PackHdr packHdr = (PackHdr)ap.GetPackHdr(cellRef[eProfileType.HeaderPack]);
																		int packUnits = (int)cellRef.CurrentCellValue;
																		int packRemainder = packUnits % packHdr.PackMultiple;
																		if (packRemainder > 0)
																		{
																			packUnits = (packUnits / packHdr.PackMultiple) * packHdr.PackMultiple;
																		}
																		int packs = packUnits / packHdr.PackMultiple;
																		ap.SetAllocatedNoOfPacks(cellRef[eProfileType.HeaderPack], storeList, packs);
                                                                        //ap.SetAllocatedUnits(cellRef[eProfileType.HeaderPack], cellRef[eProfileType.HeaderPackColor], storeList, (int)cellRef.CurrentCellValue);
                                                                        //ap.SetAllocatedUnits(cellRef[eProfileType.HeaderPack], cellRef[eProfileType.HeaderPackColor], sglp.GroupRid, cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade], (int)cellRef.CurrentCellValue);
																		// END TT#2046 - stodd - pack units keep increasing
                                                                    }
                                                                    else
                                                                    {
                                                                        // Pack
																		// BEGIN TT#2 - stodd - pack units keep increasing
																		PackHdr packHdr = (PackHdr)ap.GetPackHdr(cellRef[eProfileType.HeaderPack]);
																		int packUnits = (int)cellRef.CurrentCellValue;
																		int packRemainder = packUnits % packHdr.PackMultiple;
																		if (packRemainder > 0)
																		{
																			packUnits = (packUnits / packHdr.PackMultiple) * packHdr.PackMultiple;
																		}
																		int packs = packUnits / packHdr.PackMultiple;
																		ap.SetAllocatedNoOfPacks(cellRef[eProfileType.HeaderPack], storeList, packs);
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
																		// END TT#2 - stodd - pack units keep increasing
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (cellRef[eProfileType.HeaderPackColor] != int.MaxValue)
                                                                    {
                                                                        // color
                                                                        ap.SetAllocatedColorUnits(cellRef[eProfileType.HeaderPackColor], storeList, (int)cellRef.CurrentCellValue);
                                                                        //ap.SetAllocatedColorUnits(cellRef[eProfileType.HeaderPackColor], sglp.GroupRid, cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade], (int)cellRef.CurrentCellValue);
                                                                    }
                                                                    else
                                                                    {
                                                                        ap.SetAllocatedUnits(storeList, (int)cellRef.CurrentCellValue);
                                                                        //ap.SetAllocatedUnits(sglp.GroupRid, cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade], (int)cellRef.CurrentCellValue);
                                                                    }
                                                                }
                                                            }
                                                            // End TT#1227 - stodd
                                                            // Begin TT#2 - RMatelic - Assortment Planning-ASSORTMENT_MATRIX_DETAIL database rows not deleted when header removed from assortment
                                                            else if (!removedHeaderList.Contains(cellRef[eProfileType.AllocationHeader]))
                                                            {
                                                                removedHeaderList.Add(cellRef[eProfileType.AllocationHeader]);
                                                            }
                                                            // End TT#2
														}
													}
												}
											}
										}
									}
								}
							}
						}

						if (writeCount > 0)
						{
							AssortmentCubeGroup.AssortmentDetailData.AssortmentMatrixDetail_XMLUpdate();
						}
                        // Begin TT#2 - RMatelic - Assortment Planning-ASSORTMENT_MATRIX_DETAIL database rows not deleted when header removed from assortment
                        if (removedHeaderList.Count > 0)
                        {
                            AssortmentCubeGroup.AssortmentDetailData.AssortmentMatrixDetail_Delete(removedHeaderList);
                        }
                        // End TT#2
						AssortmentCubeGroup.AssortmentDetailData.CommitData();
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					if (AssortmentCubeGroup.AssortmentDetailData.ConnectionIsOpen)
					{
						AssortmentCubeGroup.AssortmentDetailData.CloseUpdateConnection();
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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
			AssortmentDetailVariableProfile detailVarProf;
			QuantityVariableProfile quantityVarProf;

			try
			{
				AssortCellRef = (AssortmentCellReference)aCompCellRef;
				//nodeProf = GetHierarchyNodeProfile(planCellRef);
				//versProf = GetVersionProfile(planCellRef);
				detailVarProf = (AssortmentDetailVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentDetailVariable]);
				quantityVarProf = (QuantityVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentQuantityVariable]);

				return "Assortment Header Color Detail" +
					", Header \"" + AssortCellRef[eProfileType.AllocationHeader] +
					", HeaderPack \"" + AssortCellRef[eProfileType.HeaderPack] +
					", Header Pack Color " + AssortCellRef[eProfileType.HeaderPackColor] +
					", Store Group Level " + AssortCellRef[eProfileType.StoreGroupLevel] +
					", Store Grade " + AssortCellRef[eProfileType.StoreGrade] +
					", Detail \"" + detailVarProf.VariableName + "\"" +
					", Qauntity Variable \"" + quantityVarProf.VariableName + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	
}
