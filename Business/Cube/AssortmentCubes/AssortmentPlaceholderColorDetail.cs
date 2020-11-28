using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Diagnostics;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	public class AssortmentPlaceholderColorDetail : AssortmentCube
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

		public AssortmentPlaceholderColorDetail(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
			: base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, 0, aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
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
				return eCubeType.AssortmentPlaceholderColorDetail;
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

		public override eProfileType HeaderProfileType
		{
			get
			{
				return eProfileType.PlaceholderHeader;
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
        //    return eCubeType.AssortmentPlaceholderPackDetail;
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
			return eCubeType.AssortmentPlaceholderColorDetail;
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
			return eCubeType.None;
		}

		/// <summary>
		/// Abstract method that returns the eCubeType of the corresponding Header cube for this cube.
		/// </summary>
		/// <returns>
		/// The eCubeType of the Sub-total cube.
		/// </returns>

		override public eCubeType GetComponentHeaderCubeType()
		{
			return eCubeType.AssortmentHeaderColorDetail;
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

			try
			{
				base.InitCellValue(aCompCellRef);

				asrtCellRef = (AssortmentCellReference)aCompCellRef;
				asrtCellRef.isCellBlocked = asrtCellRef.AssortmentCube.AssortmentCubeGroup.isPlaceholderBlocked(asrtCellRef[eProfileType.PlaceholderHeader], asrtCellRef[eProfileType.StoreGroupLevel], asrtCellRef[eProfileType.StoreGrade]);
				asrtCellRef.isCellDisplayOnly |= asrtCellRef.AssortmentCube.AssortmentCubeGroup.isPackColorReadOnly(asrtCellRef[eProfileType.PlaceholderHeader], int.MaxValue, asrtCellRef[eProfileType.HeaderPack], asrtCellRef[eProfileType.HeaderPackColor]);

				// Begin TT#1462-MD - stodd - Assortment Review-> Matrix Tab-> The lock functionality is not being honored. 
                asrtCellRef.isCellLocked = asrtCellRef.AssortmentCube.AssortmentCubeGroup.isHeaderLocked(
                    asrtCellRef[eProfileType.PlaceholderHeader],
                    asrtCellRef[eProfileType.HeaderPack],
                    asrtCellRef[eProfileType.HeaderPackColor],
                    asrtCellRef[eProfileType.StoreGroupLevel],
                    asrtCellRef[eProfileType.StoreGrade]);

                asrtCellRef.isCellFixed = asrtCellRef.AssortmentCube.AssortmentCubeGroup.isCellFixed(
                    asrtCellRef[eProfileType.PlaceholderHeader],
                    asrtCellRef[eProfileType.HeaderPack],
                    asrtCellRef[eProfileType.HeaderPackColor],
                    asrtCellRef[eProfileType.StoreGroupLevel],
                    asrtCellRef[eProfileType.StoreGrade]);
				// End TT#1462-MD - stodd - Assortment Review-> Matrix Tab-> The lock functionality is not being honored. 
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

		//public void ReadAndLoadCube(bool reloadFromDB)	// TT#2 - stodd
		//{
		//    string assrtColName;
		//    DataTable dtComponents;
		//    DataView dvComponents;
		//    DataTable dtAssortments;
		//    ArrayList assrtList;
		//    AssortmentDetailData dlAssrtDtl;
		//    DataTable dtHeaderData;
		//    ProfileList varProfList;
		//    AssortmentCellReference assrtCellRef;
		//    int strGrpLvlRID;
		//    int strGrdRID;
		//    int placeholderRID;

		//    try
		//    {
		//        assrtColName = ((AssortmentViewComponentVariables)AssortmentCubeGroup.AssortmentComponentVariables).Assortment.RIDColumnName;

		//        dtComponents = AssortmentCubeGroup.GetAssortmentComponents();
		//        dvComponents = dtComponents.DefaultView;
		//        dtAssortments = dvComponents.ToTable("Assortments", true, assrtColName);

		//        if (dtAssortments.Rows.Count > 0)
		//        {
		//            assrtList = new ArrayList();

		//            foreach (DataRow row in dtAssortments.Rows)
		//            {
		//                assrtList.Add(Convert.ToInt32(row[assrtColName], CultureInfo.CurrentUICulture));
		//            }

		//            dlAssrtDtl = new AssortmentDetailData();
		//            dtHeaderData = dlAssrtDtl.AssortmentMatrixDetailPlaceholders_Read(assrtList);
		//            varProfList = AssortmentCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList;

		//            assrtCellRef = (AssortmentCellReference)CreateCellReference();
		//            assrtCellRef[eProfileType.AssortmentQuantityVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;

		//            foreach (DataRow dataRow in dtHeaderData.Rows)
		//            {
		//                strGrpLvlRID = Convert.ToInt32(dataRow["SGL_RID"], CultureInfo.CurrentUICulture);
		//                strGrdRID = Convert.ToInt32(dataRow["GRADE"], CultureInfo.CurrentUICulture);

		//                if (strGrpLvlRID != -1 && strGrdRID != -1)
		//                {
		//                    placeholderRID = Convert.ToInt32(dataRow["HDR_RID"], CultureInfo.CurrentUICulture);

		//                    assrtCellRef[eProfileType.PlaceholderHeader] = placeholderRID;
		//                    assrtCellRef[eProfileType.HeaderPack] = Convert.ToInt32(dataRow["HDR_PACK_RID"], CultureInfo.CurrentUICulture);
		//                    assrtCellRef[eProfileType.HeaderPackColor] = Convert.ToInt32(dataRow["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
		//                    assrtCellRef[eProfileType.StoreGroupLevel] = strGrpLvlRID;
		//                    assrtCellRef[eProfileType.StoreGrade] = strGrdRID;

		//                    foreach (AssortmentDetailVariableProfile varProf in varProfList)
		//                    {
		//                        if (varProf.DatabaseColumnName != null)
		//                        {
		//                            assrtCellRef[eProfileType.AssortmentDetailVariable] = varProf.Key;

		//                            // BEGIN TT#2 - stodd
		//                            if (!assrtCellRef.isCellLoadedFromDB || reloadFromDB)
		//                            // END TT#2 - stodd
		//                            {
		//                                if (Convert.ToChar(dataRow["LOCKED"], CultureInfo.CurrentUICulture) == '1')
		//                                {
		//                                    assrtCellRef.SetLoadCellValue(Convert.ToDouble(dataRow[varProf.DatabaseColumnName], CultureInfo.CurrentUICulture), true);
		//                                }
		//                                else
		//                                {
		//                                    assrtCellRef.SetLoadCellValue(Convert.ToDouble(dataRow[varProf.DatabaseColumnName], CultureInfo.CurrentUICulture), false);
		//                                }

		//                                assrtCellRef.isCellLoadedFromDB = true;
		//                            }
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
		//}

		public void ReadAndLoadCube(bool reload)
		{
			ComponentProfileXRef compXRef;
			AssortmentCellReference cellRef;
			ArrayList placeholderList;
			ProfileList sglList;
			ProfileList gradeList;
			//int writeCount;

			try
			{
				compXRef = (ComponentProfileXRef)AssortmentCubeGroup.GetProfileXRef(new ComponentProfileXRef(this.CubeType));

				if (compXRef != null)
				{
					sglList = AssortmentCubeGroup.GetMasterProfileList(eProfileType.StoreGroupLevel);
					gradeList = AssortmentCubeGroup.GetMasterProfileList(eProfileType.StoreGrade);
					//writeCount = 0;

					foreach (AllocationHeaderProfile hdrProf in AssortmentCubeGroup.GetPlaceholderList())
					{
						cellRef = (AssortmentCellReference)CreateCellReference();
						cellRef[eProfileType.PlaceholderHeader] = hdrProf.Key;
						placeholderList = (ArrayList)compXRef.GetTotalList(cellRef);

						if (placeholderList != null)
						{
							foreach (Profile sglProf in sglList)
							{
								cellRef[eProfileType.StoreGroupLevel] = sglProf.Key;
								StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sglProf;

								foreach (Profile gradeProf in gradeList)
								{
									cellRef[eProfileType.StoreGrade] = gradeProf.Key;
									// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
									//foreach (Hashtable profKeyHash in placeholderList)
									foreach (Dictionary<eProfileType, int> profKeyHash in placeholderList)
									// END TT#773-MD - Stodd - replace hashtable with dictionary
									{
										if ((int)profKeyHash[eProfileType.AllocationHeader] == int.MaxValue)
										{
											cellRef[eProfileType.PlaceholderHeader] = (int)profKeyHash[eProfileType.PlaceholderHeader];
											cellRef[eProfileType.HeaderPack] = (int)profKeyHash[eProfileType.HeaderPack];
											cellRef[eProfileType.HeaderPackColor] = (int)profKeyHash[eProfileType.HeaderPackColor];
											cellRef[eProfileType.AssortmentQuantityVariable] = AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.ValueVariableProfile.Key;
											ProfileList storeList = ((AssortmentCubeGroup)this._cubeGroup).GetStoresInSetGrade(cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade]);

											foreach (AssortmentDetailVariableProfile varProf in AssortmentCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList)
											{
												if (varProf.DatabaseColumnName != null)
												{
													cellRef[eProfileType.AssortmentDetailVariable] = varProf.Key;

													if (!cellRef.isCellLoadedFromDB || reload)
													{
														AllocationProfile ap = this.Transaction.GetAssortmentMemberProfile(cellRef[eProfileType.PlaceholderHeader]);	// TT#629, TT#638, TT#694 -MD - Stodd - Assortment vs interactive velocity Assortment member
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



														cellRef.SetLoadCellValue(value, false);
														cellRef.isCellLoadedFromDB = true;

														//double dVal = ((AssortmentCubeGroup)CubeGroup).DebugAssortmentPlaceholderColorDetail(cellRef[eProfileType.StoreGrade]);
														//Debug.WriteLine("Read PLACEHOLDER: " + cellRef[eProfileType.PlaceholderHeader] + " SGL: " +
														//	cellRef[eProfileType.StoreGroupLevel] + " grade: " +
														//	cellRef[eProfileType.StoreGrade] + " pack: " +
														//	cellRef[eProfileType.HeaderPack] + " color: " +
														//	cellRef[eProfileType.HeaderPackColor] + " val: " + value 
														//	// + " dVal " + dVal
														//	);
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
			ArrayList placeholderList;
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

						foreach (AllocationHeaderProfile hdrProf in AssortmentCubeGroup.GetPlaceholderList())
						{
							// BEGIN Debugging
							//Debug.WriteLine("BEGIN Placeholder Save Cube");
							// END Debugging
							cellRef = (AssortmentCellReference)CreateCellReference();
							cellRef[eProfileType.PlaceholderHeader] = hdrProf.Key;
							placeholderList = (ArrayList)compXRef.GetTotalList(cellRef);

							if (placeholderList != null)
							{
								foreach (Profile sglProf in sglList)
								{
									cellRef[eProfileType.StoreGroupLevel] = sglProf.Key;
									StoreGroupLevelProfile sglp = (StoreGroupLevelProfile)sglProf;

									foreach (Profile gradeProf in gradeList)
									{
										cellRef[eProfileType.StoreGrade] = gradeProf.Key;
										// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
										//foreach (Hashtable profKeyHash in placeholderList)
										foreach (Dictionary<eProfileType, int> profKeyHash in placeholderList)
										// END TT#773-MD - Stodd - replace hashtable with dictionary
										{
											if ((int)profKeyHash[eProfileType.AllocationHeader] == int.MaxValue)
											{
												cellRef[eProfileType.PlaceholderHeader] = (int)profKeyHash[eProfileType.PlaceholderHeader];
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
																cellRef[eProfileType.PlaceholderHeader],
																cellRef[eProfileType.HeaderPack],
																cellRef[eProfileType.HeaderPackColor],
																cellRef[eProfileType.StoreGroupLevel],
																cellRef[eProfileType.StoreGrade],
																cellRef.CurrentCellValue,
																cellRef.isCellLocked);

															writeCount++;

															if (writeCount > MIDConnectionString.CommitLimit)
															{
																AssortmentCubeGroup.AssortmentDetailData.AssortmentMatrixDetail_XMLUpdate();
																AssortmentCubeGroup.AssortmentDetailData.Variable_XMLInit();
																writeCount = 0;
															}

															// BEGIN TT#779-MD - Stodd - New placeholder does not hold total units 
															//AllocationProfile ap = this.Transaction.GetAllocationProfile(cellRef[eProfileType.PlaceholderHeader]);
															AllocationProfile ap = this.Transaction.GetAssortmentMemberProfile(cellRef[eProfileType.PlaceholderHeader]);
															// END TT#779-MD - Stodd - New placeholder does not hold total units
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
                                                                        // BEGIN Debugging
                                                                        //DebugSaveCube(cellRef, storeList);
                                                                        // END Debugging
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
                                                                        //DebugSaveCube(cellRef, storeList);
																		// END Debugging
																		// END TT#2 - stodd - pack units keep increasing
																		//ap.SetAllocatedNoOfPacks(cellRef[eProfileType.HeaderPack], storeList, (int)cellRef.CurrentCellValue);
																		//ap.SetAllocatedPackUnits(cellRef[eProfileType.HeaderPack], sglp.GroupRid, cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade], (int)cellRef.CurrentCellValue);
																	}
																}
																else
																{
																	if (cellRef[eProfileType.HeaderPackColor] != int.MaxValue)
																	{
																		// color
                                                                        //BEGIN TT#201 - MD - DOConnell - Tried to Remove a color on a place holder and receive an error.
                                                                        if (ap.BulkColorIsOnHeader(cellRef[eProfileType.HeaderPackColor]))
                                                                        {
                                                                            ap.SetAllocatedColorUnits(cellRef[eProfileType.HeaderPackColor], storeList, (int)cellRef.CurrentCellValue);
                                                                            // BEGIN Debugging
                                                                            //DebugSaveCube(cellRef, storeList);
                                                                            // END Debugging
                                                                            //ap.SetAllocatedColorUnits(cellRef[eProfileType.HeaderPackColor], sglp.GroupRid, cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade], (int)cellRef.CurrentCellValue);
                                                                        }
                                                                        //END TT#201 - MD - DOConnell - Tried to Remove a color on a place holder and receive an error.
                                                                    }
																	else
																	{
																		ap.SetAllocatedUnits(storeList, (int)cellRef.CurrentCellValue);
                                                                        // BEGIN Debugging
                                                                        //DebugSaveCube(cellRef, storeList);
                                                                        // END Debugging
																		//ap.SetAllocatedUnits(sglp.GroupRid, cellRef[eProfileType.StoreGroupLevel], cellRef[eProfileType.StoreGrade], (int)cellRef.CurrentCellValue);
																	}
																}
															}
															// End TT#1227 - stodd
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

        private static void DebugSaveCube(AssortmentCellReference cellRef, ProfileList storeList)
        {
            string stores = string.Empty;
            for (int s = 0; s < storeList.Count; s++)
            {
                stores += ((StoreProfile)storeList[s]).StoreId + ", ";
            }

            Debug.WriteLine("PH " + cellRef[eProfileType.PlaceholderHeader] + " SGL " +
                cellRef[eProfileType.StoreGroupLevel] + " GRD " +
                cellRef[eProfileType.StoreGrade] + " PK " +
                cellRef[eProfileType.HeaderPack] + " PKCL " +
                cellRef[eProfileType.HeaderPackColor] + " VAL " +
                cellRef.CurrentCellValue + " STRS " + stores);
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

				return "Assortment Placeholder Color Detail" +
					", Header \"" + AssortCellRef[eProfileType.PlaceholderHeader] +
					", HeaderPack \"" + AssortCellRef[eProfileType.HeaderPack] +
					", Header Pack Color " + AssortCellRef[eProfileType.HeaderPackColor] +
					", Store Group Level " + AssortCellRef[eProfileType.StoreGroupLevel] +
					", Store Grade " + AssortCellRef[eProfileType.StoreGrade] +
					", Detail \"" + detailVarProf.VariableName + "\"" +
					", Quantity Variable \"" + quantityVarProf.VariableName + "\"";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
