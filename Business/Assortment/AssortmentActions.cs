using System;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Business.Allocation;

using Logility.ROWebSharedTypes;

namespace MIDRetail.Business
{
	[Serializable]
	public class AssortmentAction : AllocationAction
	{
		//=======
		// FIELDS
		//=======
		private SessionAddressBlock SAB;
		private ApplicationSessionTransaction _transaction;
		private AssortmentCubeGroup _asrtCubeGroup;
		//==========================================
		// Used by the Create Placeholders action
		//==========================================
		private List<int> _averageUnitList;
		eAllocationAssortmentViewGroupBy _viewGroupBy;
		private int _sgRid;
		//==========================================
		// Used By Index to Average Action
		//==========================================
		private eIndexToAverageReturnType _indexToAverageReturnType;
		private eSpreadAverage _spreadAverage;
		private double _averageUnits;
		private Hashtable _gradeAverageUnitsHash;
		private int _currSglRid;
        private int _placeholderCount = 0;
		//========
		// STRUCTS
		//========
		struct AssortStyleClosed
		{
			private int _headerRid;
			private int _storeGroupLevelRid;
			private int _grade;
			public int HeaderRid
			{
				get { return _headerRid; }
				set { _headerRid = value; }
			}
			public int StoreGroupLevelRid
			{
				get { return _storeGroupLevelRid; }
				set { _storeGroupLevelRid = value; }
			}
			public int Grade
			{
				get { return _grade; }
				set { _grade = value; }
			}
		}

		//=============
		// CONSTRUCTORS
		//=============
		public AssortmentAction(eMethodType aAssortmentAction)
			: base(aAssortmentAction)
		{
			_sgRid = Include.NoRID;
		}
		//===========
		// PROPERTIES
		//===========
		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.AssortmentAction;
			}
		}

		/// <summary>
		/// Used by create placeholder action.
		/// </summary>
		public List<int> AverageUnitList
		{
			get { return _averageUnitList; }
			set { _averageUnitList = value; }
		}

		/// <summary>
		/// Used by create placeholder action.
		/// </summary>
		public eAllocationAssortmentViewGroupBy ViewGroupBy
		{
			get { return _viewGroupBy; }
			set { _viewGroupBy = value; }
		}

		/// <summary>
		/// Used by create placeholder action.
		/// </summary>
		public int StoreGroupRid
		{
			get { return _sgRid; }
			set { _sgRid = value; }
		}

		/// <summary>
		/// Used by Index to Average. Tells what type of spread is to be done.
		/// </summary>
		public eIndexToAverageReturnType IndexToAverageReturnType
		{
			get { return _indexToAverageReturnType; }
			set { _indexToAverageReturnType = value; }
		}

		/// <summary>
		/// Used by Index to Average. The amount for total and total set spread.
		/// </summary>
		public double AverageUnits
		{
			get { return _averageUnits; }
			set { _averageUnits = value; }
		}

		/// <summary>
		/// Used by Index to Average. The amount for grade spreads.
		/// </summary>
		public Hashtable GradeAverageUnitsHash
		{
			get { return _gradeAverageUnitsHash; }
			set { _gradeAverageUnitsHash = value; }
		}
		
		/// <summary>
		/// Used by Index to Average. Current Store Group Level Rid.
		/// </summary>
		public int CurrSglRid
		{
			get { return _currSglRid; }
			set { _currSglRid = value; }
		}

		public eSpreadAverage SpreadAverageOption
		{
			get { return _spreadAverage; }
			set { _spreadAverage = value; }
		}
		//========
		// METHODS
		//========
		public override void ProcessMethod(ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
		}

		public override bool WithinTolerance(double aTolerancePercent)
		{
			return true;
		}

		public override void ProcessAction(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aApplicationTransaction,
			ApplicationWorkFlowStep aAssortmentWorkFlowStep,
			Profile aAssortmentProfile,
			bool writeToDB,
			int aStoreFilterRID)
		{
			bool actionSuccessful = false;
			try
			{
				SAB = aSAB;
				_transaction = aApplicationTransaction;
				AssortmentWorkFlowStep awfs = (AssortmentWorkFlowStep)aAssortmentWorkFlowStep;
				AssortmentProfile asp = (AssortmentProfile)aAssortmentProfile;
				_asrtCubeGroup = awfs.AssortmentCubeGroup;

				switch ((eAssortmentActionType)base.MethodType)
				{
					case (eAssortmentActionType.Redo):
						actionSuccessful = Redo(asp, writeToDB);
						break;
					case (eAssortmentActionType.CancelAssortment):
						actionSuccessful = CancelAssortment(asp, writeToDB);
						break;
					case (eAssortmentActionType.SpreadAverage):
						actionSuccessful = SpreadAverage(asp, writeToDB);
						break;
					case (eAssortmentActionType.CreatePlaceholders):
						actionSuccessful = CreatePlaceholders(asp, writeToDB);
						break;
                    case (eAssortmentActionType.CreatePlaceholdersBasedOnRevenue):
                        actionSuccessful = CreatePlaceholdersBasedOnRevenue(asp, writeToDB);
                        break;
					case (eAssortmentActionType.BalanceAssortment):
						actionSuccessful = BalanceAssortment(asp, writeToDB);
						break;
					// Begin TT#1224 - stodd - committed
					case (eAssortmentActionType.ChargeCommitted):
						actionSuccessful = ProcessAllocationAction(			
							 aAssortmentWorkFlowStep,
							 asp,
							 writeToDB,
							aStoreFilterRID);

					break;
					case (eAssortmentActionType.CancelCommitted):
						actionSuccessful = CancelCommitted(asp, writeToDB);
						break;
					// End TT#1224 - stodd - committed
					// Begin TT#1225 - stodd
					//case (eAssortmentActionType.ChargeIntransit):
					//    actionSuccessful = ChargeIntransit(asp, writeToDB);
					//    break;
					//case (eAssortmentActionType.CancelIntransit):
					//    actionSuccessful = CancelIntransit(asp, writeToDB);
					//    break;
					// End TT#1225 - stodd
					default:
						// Error Invalid assortment action
						break;
				}

				if (!actionSuccessful)
				{
					_transaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionFailed);
				}
			}
			catch
			{
				throw;
			}
		}

		private bool ProcessAllocationAction(ApplicationWorkFlowStep aAssortmentWorkFlowStep,
			AssortmentProfile asp,
			bool writeToDB,
			int aStoreFilterRID)
		{
			bool actionSuccessful = true;
			try
			{
				GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
				bool aReviewFlag = false;
				bool aUseSystemTolerancePercent = false;
				double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
				int aWorkFlowStepKey = -1;

				List<int> headerRidList = asp.GetHeaderRidList();


				//ApplicationBaseAction aMethod = _transaction.CreateNewMethodAction(eMethodType.ChargeIntransit);
				//AllocationWorkFlowStep aAllocationWorkFlowStep
				//    = new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
				//_transaction.DoAllocationAction(aAllocationWorkFlowStep);

				//if (ap.Action((eAllocationMethodType)base.MethodType, awfs.Component, awfs.TolerancePercent, aStoreFilterRID, writeToDB))
				//{
				//    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
				//}
				//else
				//{
				//    aApplicationTransaction.SetAllocationActionStatus(ap.Key, eAllocationActionStatus.ActionFailed);
				//}
				//actionSuccessful = ChargeCommitted(asp, writeToDB);
	



				asp.WriteHeader();
				if (writeToDB)
				{
					asp.WriteHeader();
				}
				asp.AppSessionTransaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
				return actionSuccessful;
			}
			finally
			{
			}
		}

		internal override void VerifyAction(eMethodType aMethodType)
		{
			if (Enum.IsDefined(typeof(eMethodTypeUI), (eMethodTypeUI)aMethodType))
			{
				throw new MIDException(eErrorLevel.severe,
					(int)eMIDTextCode.msg_ActionCannotHaveUI,
					MIDText.GetText(eMIDTextCode.msg_ActionCannotHaveUI));
			}

		}

		private bool Redo(AssortmentProfile asp, bool commitToDb)
		{
			//asp = (AssortmentProfile)_asrtCubeGroup.DefaultAllocationProfile;

			try
			{
				bool doCommit = false;

				// If an update connection is already open, then lets use it.
				// Otherwise, we open one...and we'll commit and close when we're done.
				if (!asp.HeaderDataRecord.ConnectionIsOpen)
				{
					asp.HeaderDataRecord.OpenUpdateConnection();
					doCommit = true;
				}
				try
				{
					//================================================
					// Redo
					//================================================

					//=======================================
					// Update Summary Area
					//=======================================

					// re-init the assortment summary
					asp.BuildAssortmentSummary();

					List<int> hierNodeList = new List<int>();
					List<int> versionList = new List<int>();
					List<int> dateRangeList = new List<int>();
					List<double> weightList = new List<double>();
					foreach (AssortmentBasis ab in asp.AssortmentBasisList)
					{
						hierNodeList.Add(ab.HierarchyNodeProfile.Key);
						versionList.Add(ab.VersionProfile.Key);
						dateRangeList.Add(ab.HorizonDate.Key);
						weightList.Add(ab.Weight);
					}

                    // Begin TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                    if (asp.IsAssortment
                        //&& asp.BeginDay != Include.UndefinedDate    // TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected. 
                        && asp.BeginDay != asp.AssortmentBeginDay.Date)
                    {
                        asp.BeginDayIsSet = false;  // TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.           
                        asp.BeginDay = Include.UndefinedDate;
                        asp.BeginDay = asp.AssortmentBeginDay.Date;
                    }
                    // End TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
                    // Begin TT#2038-MD - JSmith - Intransit is incorrect when the Delivery Week is changed
                    if (asp.IsAssortment
                        //&& asp.ShipToDay != Include.UndefinedDate    // TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected. 
                        && asp.ShipToDay != asp.AssortmentApplyToDate.Date)
                    {
                        asp.ShipToDayIsSet = false;  // TT#2101-MD - JSmith - Matrix values change when beg and Del dates changed without REDO being selected.           
                        asp.ResetStoreShipDates();
                        asp.ShipToDay = Include.UndefinedDate;
                        asp.ShipToDay = asp.AssortmentApplyToDate.Date;
                    }
                    // End TT#2038-MD - JSmith - Intransit is incorrect when the Delivery Week is changed
					// re-read and process the summary information
					asp.AssortmentSummaryProfile.ClearAssortmentSummaryTable(); // BEGIN TT#1876 - stodd 
					asp.AssortmentSummaryProfile.Process(_transaction, asp.AssortmentAnchorNodeRid, asp.AssortmentVariableType, hierNodeList,
						   versionList, dateRangeList, weightList, asp.AssortmentIncludeSimilarStores, asp.AssortmentIncludeIntransit,
						   asp.AssortmentIncludeOnhand, asp.AssortmentIncludeCommitted, asp.AssortmentAverageBy, true, true);


					asp.AssortmentSummaryProfile.WriteAssortmentStoreSummary(asp.HeaderDataRecord);


					if (doCommit)
					{
						asp.HeaderDataRecord.CommitData();
					}

					asp.AssortmentSummaryProfile.RereadStoreSummaryData();
					asp.AssortmentSummaryProfile.BuildSummary(StoreGroupRid);


					//=============================
					// Update Detail Placeholders
					//=============================
					ProfileList gradeList = asp.GetAssortmentStoreGrades();		// TT#488-MD - STodd - Group Allocation
					ProfileList sgll = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);
					
					//================================
					// Num stores from all set/grades
					//================================
					//double cellValue = 0.0d;
					//Cube myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryGrade);
					//AssortmentCellReference asrtCellRef = new AssortmentCellReference((AssortmentSummaryGrade)myCube);
					MID2DimIntDictionary numStoreDict = new MID2DimIntDictionary();
					foreach (StoreGradeProfile sgp in gradeList.ArrayList)
					{
						foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
						{
							AssortmentSummaryItemProfile asip = asp.GetAssortmentSummary(asp.AssortmentVariableNumber, sglp.Key, sgp.Key);
							//asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
							//asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
							//asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.NumStores).Key;
							//asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
							//cellValue = asrtCellRef.CurrentCellValue;
							numStoreDict[sgp.Key, sglp.Key] = asip.NumberOfStores;
							//Debug.WriteLine(sgp.StoreGrade + " / " + sglp.Key +	" Stores: " + cellValue.ToString());
						}
					}

					//===============================================
					// Average Units from all set/grades/placeholders
					//===============================================
					Cube myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentPlaceholderGradeTotal);
					double cellValue = 0.0d;
					AssortmentCellReference asrtCellRef = new AssortmentCellReference((AssortmentPlaceholderGradeTotal)myCube);
					AllocationHeaderProfileList ahpl = (AllocationHeaderProfileList)_asrtCubeGroup.GetHeaderList();
					MID3DimIntDictionary AvgUnitsDict = new MID3DimIntDictionary();

					foreach (StoreGradeProfile sgp in gradeList.ArrayList)
					{
						foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
						{
							foreach (AllocationHeaderProfile headerProf in ahpl)
							{
								if (headerProf.HeaderType == eHeaderType.Placeholder && !IsPostReceipt())	// TT#841 - MD - stodd - Null Ref on Redo of Post Receipt
								{
									asrtCellRef[eProfileType.PlaceholderHeader] = headerProf.Key;
									asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
									asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
									asrtCellRef[eProfileType.AssortmentDetailVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList.FindKey((int)eAssortmentDetailVariables.AvgUnits).Key;
									asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
									cellValue = asrtCellRef.CurrentCellValue;
									AvgUnitsDict[sgp.Key, sglp.Key, headerProf.Key] = cellValue;
									//Debug.WriteLine("Set: " + sglp.Key +
									//    "  Grade: " + sgp.Key +
									//    "  Hdr: " + headerProf.Key +
									//    "  Avg: " + cellValue.ToString());
								}
							}
						}
					}

					//=============================================
					// Write new total unit values to placeholders
					//=============================================
					foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
					{
						foreach (StoreGradeProfile sgp in gradeList.ArrayList)
						{
							foreach (AllocationHeaderProfile headerProf in ahpl)
							{
								if (headerProf.HeaderType == eHeaderType.Placeholder && !IsPostReceipt())	// TT#841 - MD - stodd - Null Ref on Redo of Post Receipt
								{
                                    // Begin TT#2100-MD - JSmith - After REDO Asst Grade did not change and stores are not blocked or unblocked correctly
                                    ProfileList storeList = asp.GetStoresInSetGrade(sglp.Key, sgp.Key);
                                    AllocationProfile ap = _transaction.GetAssortmentMemberProfile(headerProf.Key);
                                    // End TT#2100-MD - JSmith - After REDO Asst Grade did not change and stores are not blocked or unblocked correctly
									if (!_asrtCubeGroup.BlockedList.Contains(new BlockedListHashKey(headerProf.Key, sglp.Key, sgp.Key)))
									{
                                        //ProfileList storeList = asp.GetStoresInSetGrade(sglp.Key, sgp.Key);   // TT#2100-MD - JSmith - After REDO Asst Grade did not change and stores are not blocked or unblocked correctly
										// Begin TT#1451-MD - stodd - On Assortment Content tab, cannot process Assortment actions unless a header/placeholder is selected. This is incorrect. 
										//AllocationProfile ap = _transaction.GetAllocationProfile(headerProf.Key);
                                        //AllocationProfile ap = _transaction.GetAssortmentMemberProfile(headerProf.Key);   // TT#2100-MD - JSmith - After REDO Asst Grade did not change and stores are not blocked or unblocked correctly
										// End TT#1451-MD - stodd - On Assortment Content tab, cannot process Assortment actions unless a header/placeholder is selected. This is incorrect. 
										int storeCount = (int)numStoreDict[sgp.Key, sglp.Key];
										double avgUnits = (double)AvgUnitsDict[sgp.Key, sglp.Key, headerProf.Key];
                                        // Begin TT#2100-MD - JSmith - After REDO Asst Grade did not change and stores are not blocked or unblocked correctly
                                        //int value = (int)(storeCount * avgUnits);
                                        //ap.SetAllocatedUnits(storeList, value);
                                        int value = (int)(avgUnits);
                                        ap.SetAllocatedUnits(aStoreList: storeList, quantity: value, plugValue: true);
                                        // End TT#2100-MD - JSmith - After REDO Asst Grade did not change and stores are not blocked or unblocked correctly
										//ap.TotalUnitsToAllocate += value;		// TT#618-MD - stodd - Created Placeholders by entering avg per grade.  The result is not = to or close to the average entered. 
                                        //ap.WriteHeader();  // TT#2126-MD - JSmith - After REDO Need and Need % changes.  Expected it to be the same as before the REDO.  

										//assortDetailData.AssortmentMatrixDetail_XMLInsert(
										//    headerRid, int.MaxValue, int.MaxValue, sglp.Key, sgp.Key, avg, false);

										//writeCount++;

										//if (writeCount > MIDConnectionString.CommitLimit)
										//{
										//    assortDetailData.AssortmentMatrixDetail_XMLUpdate();
										//    assortDetailData.Variable_XMLInit();
										//    writeCount = 0;
										//}
									}
                                    // Begin TT#2100-MD - JSmith - After REDO Asst Grade did not change and stores are not blocked or unblocked correctly
                                    else
                                    {
                                        ap.SetAllocatedUnits(storeList, 0);
                                    }
                                    // End TT#2100-MD - JSmith - After REDO Asst Grade did not change and stores are not blocked or unblocked correctly
								}
							}
						}
					}

                    // Begin TT#2126-MD - JSmith - After REDO Need and Need % changes.  Expected it to be the same as before the REDO.  
                    foreach (AllocationHeaderProfile headerProf in ahpl)
                    {
                        AllocationProfile ap = _transaction.GetAssortmentMemberProfile(headerProf.Key);
                        ap.WriteHeader();
                    }

                    // Recalculate values impacted by the allocated values
                    asp.AssortmentSummaryProfile.RecalculateSummaryValues();
                    if (!asp.HeaderDataRecord.ConnectionIsOpen)
                    {
                        asp.HeaderDataRecord.OpenUpdateConnection();
                    }
                    asp.AssortmentSummaryProfile.WriteAssortmentStoreSummary(asp.HeaderDataRecord);
					asp.HeaderDataRecord.CommitData();

                    asp.AssortmentSummaryProfile.BuildSummary(StoreGroupRid);
                    // End TT#2126-MD - JSmith - After REDO Need and Need % changes.  Expected it to be the same as before the REDO.  

					//if (writeCount > 0)
					//{
					//    assortDetailData.AssortmentMatrixDetail_XMLUpdate();
					//}

					//assortDetailData.CommitData();


					//InsertAssortStyleClosed(closedList);

                    asp.BuildAssortmentGradesByStore();  // TT#2122-MD - JSmith - After a REDO the Assortment Grades in Style Review are the same as before doing the REDO.  Expected them to change based on the REDO being processed.

					if (commitToDb)
					{
						asp.WriteHeader();
					}


					_transaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
					string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodCompletedSuccessfully);
					msg = msg.Replace("{0}", "Assortment");
					msg = msg.Replace("{1}", "Redo");
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());

					return true;
				}
				finally
				{
					if (doCommit)
					{
						asp.HeaderDataRecord.CloseUpdateConnection();
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// BEGIN TT#841 - MD - stodd - Null Ref on Redo of Post Receipt
		private bool IsPostReceipt()
		{
			bool isPost = false;
			AllocationHeaderProfileList ahpl = (AllocationHeaderProfileList)_asrtCubeGroup.GetHeaderList();
			foreach (AllocationHeaderProfile ahp in ahpl.ArrayList)
			{
				if (ahp.Assortment) 
				{
					if ((eAssortmentType)ahp.AsrtType == eAssortmentType.PostReceipt)
					{
						isPost = true;
						break;
					}
				}
			}
			return isPost;
		}
		// END TT#841 - MD - stodd - Null Ref on Redo of Post Receipt

		private bool BalanceAssortment(AssortmentProfile asp, bool commitToDb)
		{
			try
			{
				_transaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
				string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodCompletedSuccessfully);
				msg = msg.Replace("{0}", "Assortment");
				msg = msg.Replace("{1}", "Assortment Balance");
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());

				return true;
			}
			catch
			{
				throw;
			}
		}

		private bool CancelAssortment(AssortmentProfile asp, bool commitToDb)
		{
			try
			{
				bool doCommit = false;
				bool successful = true;
				// If an update connection is already open, then lets use it.
				// Otherwise, we open one...and we'll commit and close when we're done.
				//if (!asp.HeaderDataRecord.ConnectionIsOpen)
				//{
				//    asp.HeaderDataRecord.OpenUpdateConnection();
				//    doCommit = true;
				//}
				try
				{
					//asp.AssortmentSummaryProfile.ClearTotalAssortment();
					//asp.AssortmentSummaryProfile.SetAssortmentStoreSummary(0, eAssortmentVariableType.None, new System.Collections.Generic.List<int>(), new System.Collections.Generic.List<int>(),
					//    new System.Collections.Generic.List<double>(), new System.Collections.Generic.List<int>(), new System.Collections.Generic.List<double
					//    new System.Collections.Generic.List<int>(), new System.Collections.Generic.List<int>(), new System.Collections.Generic.List<decimal

					// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                    SelectedHeaderList selectedHeaderList = asp.AppSessionTransaction.AssortmentSelectedHdrList;
					//List<int> headerRidList = asp.GetHeaderRidList();
					successful = CancelAssortmentDetail(selectedHeaderList);
					// END TT#371-MD - stodd -  Velocity Interactive on Assortment

					// Begin TT#1261 - stodd - refreshing assortment workspace
					//asp.TotalUnitsToAllocate = 0; //TT#774 - MD - DOConnell - Change how TotalUnitsToAllocate are calculated for an assortment profile so it is calculated as needed instead of maintained by the assortment review.
					asp.WriteHeader();
					if (commitToDb)
					{
						asp.WriteHeader();
					}
					// End TT#1261 - stodd - refreshing assortment workspace

					asp.AppSessionTransaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);

					return successful;
				}
				finally
				{
					//if (doCommit)
					//{
					//    asp.HeaderDataRecord.CloseUpdateConnection();
					//}
				}
			}
			catch
			{
				throw;
			}
		}

		// Begin TT#1224 - stodd - committed
		private bool ChargeCommitted(AssortmentProfile asp, bool commitToDb)
		{
			try
			{
				bool successful = true;

				try
				{
					List<int> headerRidList = asp.GetHeaderRidList();
					successful = ChargeCommittedDetail(headerRidList);

					asp.WriteHeader();
					if (commitToDb)
					{
						asp.WriteHeader();
					}
					asp.AppSessionTransaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
					return successful;
				}
				finally
				{
				}
			}
			catch
			{
				throw;
			}
		}

		private bool CancelCommitted(AssortmentProfile asp, bool commitToDb)
		{
			try
			{
				bool successful = true;

				try
				{
					List<int> headerRidList = asp.GetHeaderRidList();
					successful = CancelCommittedDetail(headerRidList);

					asp.WriteHeader();
					if (commitToDb)
					{
						asp.WriteHeader();
					}
					asp.AppSessionTransaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
					return successful;
				}
				finally
				{
				}
			}
			catch
			{
				throw;
			}
		}
		// End TT#1224 - stodd - committed


		// Begin TT#1225 - stodd - committed
		private bool ChargeIntransit(AssortmentProfile asp, bool commitToDb)
		{
			try
			{
				bool successful = true;

				try
				{
					successful = ChargeIntransitDetail();

					asp.WriteHeader();
					if (commitToDb)
					{
						asp.WriteHeader();
					}
					asp.AppSessionTransaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
					return successful;
				}
				finally
				{
				}
			}
			catch
			{
				throw;
			}
		}

		private bool CancelIntransit(AssortmentProfile asp, bool commitToDb)
		{
			try
			{
				bool successful = true;

				try
				{
					successful = CancelIntransitDetail();

					asp.WriteHeader();
					if (commitToDb)
					{
						asp.WriteHeader();
					}
					asp.AppSessionTransaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
					return successful;
				}
				finally
				{
				}
			}
			catch
			{
				throw;
			}
		}
		// End TT#1225 - stodd - committed

		private bool ReadTest(AssortmentProfile asp)
		{
			try
			{
				DataTable dt = AssortmentProfile.CreateAssortmentComponentTable(_asrtCubeGroup.AssortmentComponentVariables);
				PackColorProfileXRef packColorXRef = new PackColorProfileXRef();
				asp.GetAssortmentComponents(this._asrtCubeGroup, dt, packColorXRef);

				Debug.WriteLine("BEGIN TEST");
				foreach (DataRow aRow in dt.Rows)
				{
					int placeholderRid = Convert.ToInt32(aRow["PLACEHOLDER_RID"], CultureInfo.CurrentUICulture);
					int planlevelRid = Convert.ToInt32(aRow["PLANLEVEL_RID"], CultureInfo.CurrentUICulture);
					int headerRid = Convert.ToInt32(aRow["HEADER_RID"], CultureInfo.CurrentUICulture);
					int packRid = Convert.ToInt32(aRow["PACK_RID"], CultureInfo.CurrentUICulture);
					int colorRid = Convert.ToInt32(aRow["COLOR_RID"], CultureInfo.CurrentUICulture);

					if (headerRid != int.MaxValue)
					{

						Cube myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentComponentHeaderGrade);
						AssortmentCellReference asrtCellRef = new AssortmentCellReference((AssortmentComponentHeaderGrade)myCube);

						ProfileList gradeList = _asrtCubeGroup.GetStoreGrades(headerRid);
						ProfileList sgll = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);

						double totalVal = 0.0d;
						foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
						{
							foreach (StoreGradeProfile sgp in gradeList.ArrayList)
							{
								asrtCellRef[eProfileType.AllocationHeader] = headerRid;
								//asrtCellRef[eProfileType.HeaderPack] = PackRid;
								//asrtCellRef[eProfileType.HeaderPackColor] = ColorRid;
								asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
								asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
								asrtCellRef[eProfileType.AssortmentDetailVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList.FindKey((int)eAssortmentDetailVariables.TotalUnits).Key;
								asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
								//double totalValue = asrtCellRef.CurrentCellValue;
								double aValue = asrtCellRef.GetComponentDetailSum(eGetCellMode.Current, eSetCellMode.Initialize, false);
								if (aValue != 0)
								{
									totalVal += aValue;
                                    //Debug.WriteLine(aValue.ToString());
								}
							}
						}
                        //Debug.WriteLine("HDR TOTAL: " + totalVal.ToString());

						myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryTotal);
						asrtCellRef = new AssortmentCellReference((AssortmentSummaryTotal)myCube);


						//==============================
						// Total num stores from cube
						//==============================
						double totNumStores = 0.0d;
						asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.NumStores).Key;
						asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
						totNumStores = asrtCellRef.CurrentCellValue;
						Debug.WriteLine("Total Num Stores: " + totNumStores.ToString());

						//==============================
						// Header Average
						//==============================
						double headerAvg = totalVal / totNumStores;

						//================================
						// Num stores from all set/grades
						//================================
						totalVal = 0.0d;
						myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryGrade);
						asrtCellRef = new AssortmentCellReference((AssortmentSummaryGrade)myCube);
						double numStores = 0.0d;
						foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
						{
							foreach (StoreGradeProfile sgp in gradeList.ArrayList)
							{
								asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
								asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
								asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.NumStores).Key;
								asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
								numStores = asrtCellRef.CurrentCellValue;
								Debug.WriteLine("Set: " + sglp.Key +
									"  Grade: " + sgp.Key +
									"  # Strs: " + numStores.ToString());
								totalVal += numStores;
							}
						}
						Debug.WriteLine("Total Num Stores: " + totalVal.ToString());

						//================================
						// Index for each grade
						//================================
						totalVal = 0.0d;
						myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryGrade);
						asrtCellRef = new AssortmentCellReference((AssortmentSummaryGrade)myCube);
						double gradeIndex = 0.0d;
						foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
						{
							foreach (StoreGradeProfile sgp in gradeList.ArrayList)
							{
								asrtCellRef[eProfileType.StoreGroupLevel] = sgll[0].Key;
								asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
								asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.Index).Key;
								asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
								gradeIndex = asrtCellRef.CurrentCellValue;
								Debug.WriteLine("  Grade: " + sgp.Key +
									"  Index: " + gradeIndex.ToString());
							}
						}
					}
				}
				Debug.WriteLine("END TEST");

				return true;
			}
			catch
			{
				throw;
			}

		}

		private bool CreatePlaceholders(AssortmentProfile asp, bool commitToDb)
		{
			int listCnt = this._averageUnitList.Count;

			List<int> asrtTotalList = new List<int>();
			List<int> numStoresList = new List<int>();
			List<int> avgStoreList = new List<int>();
			List<int> newPlaceholderList = new List<int>();
			for (int i = 0; i < listCnt; i++)
			{
				asrtTotalList.Add(0);
				numStoresList.Add(0);
				avgStoreList.Add(0);
				newPlaceholderList.Add(0);
			}
			List<AssortStyleClosed> closedList = new List<AssortStyleClosed>();

			AssortmentDetailData assortDetailData = _asrtCubeGroup.AssortmentDetailData;
			try
			{
				DataTable dt = AssortmentProfile.CreateAssortmentComponentTable(_asrtCubeGroup.AssortmentComponentVariables);
				PackColorProfileXRef packColorXRef = new PackColorProfileXRef();
				asp.GetAssortmentComponents(this._asrtCubeGroup, dt, packColorXRef);

				ProfileList gradeList = asp.GetAssortmentStoreGrades();	// TT#488-MD - STodd - Group Allocation
				ProfileList sgll = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);

				Cube myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryTotal);
				AssortmentCellReference asrtCellRef = new AssortmentCellReference((AssortmentSummaryTotal)myCube);

				//==============================
				// Total num stores from cube
				//==============================
				double totNumStores = 0.0d;
				asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.NumStores).Key;
				asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
				totNumStores = asrtCellRef.CurrentCellValue;
				Debug.WriteLine("Total Num Stores: " + totNumStores.ToString());

				//================================
				// Num stores from all set/grades
				//================================
				double cellValue = 0.0d;
				int aIdx = -1;

				//if (this.ViewGroupBy == eAllocationAssortmentViewGroupBy.StoreGrade)
				//{
				myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryGrade);
				asrtCellRef = new AssortmentCellReference((AssortmentSummaryGrade)myCube);

				foreach (StoreGradeProfile sgp in gradeList.ArrayList)
				{
					aIdx++;
					foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
					{
						asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
						asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
						asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.NumStores).Key;
						asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
						cellValue = asrtCellRef.CurrentCellValue;
						//Debug.WriteLine("Set: " + sglp.Key +
						//    "  Grade: " + sgp.Key +
						//    "  # Strs: " + cellValue.ToString());
						numStoresList[aIdx] += (int)cellValue;

						asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
						asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
						asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.Units).Key;
						asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
						cellValue = asrtCellRef.CurrentCellValue;
						//Debug.WriteLine("Set: " + sglp.Key +
						//    "  Grade: " + sgp.Key +
						//    "  Units: " + cellValue.ToString());
						asrtTotalList[aIdx] += (int)cellValue;
					}
					//Debug.WriteLine(sgp.StoreGrade + " Units: " + asrtTotalList[aIdx].ToString() +
					//	" Stores: " + numStoresList[aIdx].ToString());
				}

				//==================================================================
				// Find the average store per grade.
				// then divide the average store per grade by the average units
				// entered by the user.
				//==================================================================
				for (int i = 0; i < listCnt; i++)
				{
					if (numStoresList[i] > 0)
					{
						avgStoreList[i] = asrtTotalList[i] / numStoresList[i];
						if (_averageUnitList[i] > 0)
						{
							newPlaceholderList[i] = avgStoreList[i] / _averageUnitList[i];
						}
					}
				}

				//==============================================
				// Determine the maximum number of placeholders
				//==============================================
				int maxPlaceHolders = GetMaxPlaceholders(listCnt, newPlaceholderList);

				//=============================================================
				// Get new placeholder nodes and add placeholder style headers
				//=============================================================
				int placeholderCount = GetPlaceHolderStyleCount(dt);
				ArrayList headerRids = new ArrayList();
				if (maxPlaceHolders > placeholderCount)
				{
					int numOfNewPlaceholders = maxPlaceHolders - placeholderCount;
					HierarchyNodeList hierNodeList = SAB.HierarchyServerSession.GetPlaceholderStyles(asp.AssortmentAnchorNodeRid,
							numOfNewPlaceholders, placeholderCount, asp.Key);

					AllocationProfile ap = null;
					int asrtPlaceholderSeq = 0;
					foreach (HierarchyNodeProfile hnp in hierNodeList)
					{
						//HierarchyNodeProfile hnp = (HierarchyNodeProfile)hierNodeList[0];
						placeholderCount++;
						string lblPhStyle = MIDText.GetTextOnly(eMIDTextCode.lbl_PhStyle);
						ap = new AllocationProfile(_transaction, asp.HeaderID + " " + lblPhStyle + " " + placeholderCount.ToString(CultureInfo.CurrentUICulture),
								Include.NoRID, SAB.ApplicationServerSession);

						ap.StyleHnRID = hnp.Key;
						ap.AsrtRID = asp.Key;
                        ap.PlanHnRID = asp.PlanHnRID;   // TT#2 - RMatelic - Assortment Planning - add headers to placeholders
						MIDException midException;
						if (!ap.SetHeaderType(eHeaderType.Placeholder, out midException))
						{
							throw midException;
						}
                        //ap.Placeholder = true;
						//ap.TotalUnitsToAllocate = 0;	//TT#618-MD - stodd - Created Placeholders by entering avg per grade.  The result is not = to or close to the average entered. 
						ap.AsrtPlaceholderSeq = ++asrtPlaceholderSeq;	// TT#1227 - stodd
						//ap.HeaderDescription = "Placeholder Style";
						ap.HeaderDescription = hnp.NodeDescription;
						ap.HeaderDay = DateTime.Now;
						// Begin TT#1224 - stodd - committed
						DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(asp.AssortmentCalendarDateRangeRid);
						ap.ShipToDay = SAB.ClientServerSession.Calendar.GetFirstDayOfRange(drp).Date;

                        // Begin TT#2085-MD - JSmith - Create PH function in an Asst when process receive Severe Error:30006:Invalid Calendar Date
                        //DateRangeProfile drpBeginDay = SAB.ClientServerSession.Calendar.GetDateRange(asp.AssortmentBeginDayCalendarDateRangeRid);
                        //ap.BeginDay = SAB.ClientServerSession.Calendar.GetFirstDayOfRange(drpBeginDay).Date;
                        if (asp.AssortmentBeginDayCalendarDateRangeRid != Include.UndefinedCalendarDateRange)
                        {
                            DateRangeProfile drpBeginDay = SAB.ClientServerSession.Calendar.GetDateRange(asp.AssortmentBeginDayCalendarDateRangeRid);
                            ap.BeginDay = SAB.ClientServerSession.Calendar.GetFirstDayOfRange(drpBeginDay).Date;
                        }
                        // End TT#2085-MD - JSmith - Create PH function in an Asst when process receive Severe Error:30006:Invalid Calendar Date

						// End TT#1224 - stodd
						ap.WriteHeader();
						_transaction.AddAllocationProfile(ap);

						headerRids.Add(ap.Key);
						AddToClosedList(listCnt, newPlaceholderList, closedList, gradeList, sgll, placeholderCount, ap);
					}
				}

				//=============================================
				// Write Average values to new placeholders
				//=============================================
				assortDetailData.OpenUpdateConnection();
				assortDetailData.Variable_XMLInit();
				int writeCount = 0;
				int totalAssortmentUnits = 0;
				foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
				{
					int gradeIdx = 0;
					foreach (StoreGradeProfile sgp in gradeList.ArrayList)
					{
						int avg = _averageUnitList[gradeIdx++];

						foreach (int headerRid in headerRids)
						{
							// Begin TT#1467 - stodd - totals not takinging closed styles into consideration
							bool closedStyle = false;
							foreach (AssortStyleClosed asc in closedList)
							{
								if (asc.HeaderRid == headerRid && asc.StoreGroupLevelRid == sglp.Key && asc.Grade == sgp.Key)
								{
									closedStyle = true;
								}
							}

							if (!closedStyle)
							{
								ProfileList storeList = _asrtCubeGroup.GetStoresInSetGrade(sglp.Key, sgp.Key);
								AllocationProfile ap = _transaction.GetAllocationProfile(headerRid);
								int value = storeList.Count * avg;
								ap.SetAllocatedUnits(storeList, value);
								//BEGIN TT#618-MD - stodd - Created Placeholders by entering avg per grade.  The result is not = to or close to the average entered. 
								//ap.TotalUnitsToAllocate += value;
								//Debug.WriteLine("HDR " + ap.Key + " SGL " + sglp.Name + " GRD " + sgp.StoreGrade + " NUMSTRS " + storeList.Count + " VAL " + value + " TOT " + ap.TotalUnitsAllocated);
								//END TT#618-MD - stodd - Created Placeholders by entering avg per grade.  The result is not = to or close to the average entered. 
								totalAssortmentUnits += value;
								//ap.WriteHeader();   // TT#618-MD - stodd - Created Placeholders by entering avg per grade.  The result is not = to or close to the average entered. 

								assortDetailData.AssortmentMatrixDetail_XMLInsert(
									headerRid, int.MaxValue, int.MaxValue, sglp.Key, sgp.Key, avg, false);

								writeCount++;
								if (writeCount > MIDConnectionString.CommitLimit)
								{
									assortDetailData.AssortmentMatrixDetail_XMLUpdate();
									assortDetailData.Variable_XMLInit();
									writeCount = 0;
								}
							}
							// End TT#1467 - stodd - totals not takinging closed styles into consideration
						}
						
					}
				}

				//BEGIN TT#618-MD - stodd - Created Placeholders by entering avg per grade.  The result is not = to or close to the average entered. 
				foreach (int headerRid in headerRids)
				{
					AllocationProfile ap = _transaction.GetAllocationProfile(headerRid);
					ap.WriteHeader();
				}
				//END TT#618-MD - stodd - Created Placeholders by entering avg per grade.  The result is not = to or close to the average entered. 

				// Update total assortment units
				//asp.TotalUnitsToAllocate = totalAssortmentUnits; TT#774 - MD - DOConnell - Change how TotalUnitsToAllocate are calculated for an assortment profile so it is calculated as needed instead of maintained by the assortment review.
				asp.WriteHeader();

				if (writeCount > 0)
				{
					assortDetailData.AssortmentMatrixDetail_XMLUpdate();
				}

				assortDetailData.CommitData();


				InsertAssortStyleClosed(closedList);

				if (commitToDb)
				{
					asp.WriteHeader();
				}
				_transaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
				string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodCompletedSuccessfully);
				msg = msg.Replace("{0}", "Assortment");
				msg = msg.Replace("{1}", "Create Placeholders");
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());

				//Debug.WriteLine("END TEST");
				return true;
			}
			catch (Exception ex)
			{
				_transaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionFailed);
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), this.ToString());
				string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
				msg = msg.Replace("{0}", "Assortment");
				msg = msg.Replace("{1}", "Create Placeholders");
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.ToString());
				throw;
			}
			finally
			{
				if (assortDetailData.ConnectionIsOpen)
				{
					assortDetailData.CloseUpdateConnection();
				}
			}

		}

        private bool CreatePlaceholdersBasedOnRevenue(AssortmentProfile asp, bool commitToDb)
        {
            bool successful = true;
            try
            {
                if (_transaction.CurrentProcessStep == 0)
                {
                    successful = CreatePlaceholdersBasedOnRevenue_DeterimePlaceholderCount(asp, commitToDb);
                }
                if (_transaction.CurrentProcessStep == 1
                    && successful == true
                    && _transaction.MessageResponse == eMessageResponse.Yes)
                {
                    successful = CreatePlaceholdersBasedOnRevenue_CreatePlaceholders(asp, commitToDb);
                }

                return successful;
            }
            catch 
            {
                throw;
            }
        }

        private bool CreatePlaceholdersBasedOnRevenue_DeterimePlaceholderCount(AssortmentProfile asp, bool commitToDb)
        {
            NodeDescendantList colorNodeList;
            PlanOpenParms planOpenParms;
            PlanCubeGroup cubeGroup = null;
            double colorRevenue = 0;
            double colorSellThru = 0;
            HierarchyNodeProfile nodeProfile;

            int colorNodeCount = 0;   // count of color nodes for a basis to create placeholder
            int totalColorNodeCount = 0;    // total count of color nodes across all basis
            double sellThruThreshold = Include.Undefined;    // sell thru threshold from config
            int currentStyleKey = 0;   // key of current style
            int styleColorNodeCount = 0;   // count of color nodes for current style
            int colorNodeCountWithRevenue = 0;   // count of color nodes for style whose sales > 0
            int colorNodeCountThatExceedsSellThru = 0;   // count of color nodes for style whose sell thru exceeds the threshold
            int thresholdAdjustment = 0;   // adjust for style adjustments based on threshold

            string parmStr = MIDConfigurationManager.AppSettings["SellThruThreshold"];
            if (parmStr != null)
            {
                try
                {
                    sellThruThreshold = Convert.ToDouble(parmStr);
                }
                catch
                {
                    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, "Sell Thru Threshold is invalid and will be ignored.", this.GetType().Name);
                    sellThruThreshold = Include.Undefined;
                }
            }

            if (sellThruThreshold == Include.Undefined)
            {
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, "Adjustments based on Sell Thru Threshold will not be performed.", this.GetType().Name);
            }

            // Determine the starting number of style/colors for the basis with sales
            foreach (AssortmentBasis ab in asp.AssortmentBasisList)
            {
                colorNodeCount = 0;
                thresholdAdjustment = 0;
                colorNodeList = SAB.HierarchyServerSession.GetNodeDescendantList(ab.HierarchyNodeProfile.Key, eHierarchyLevelType.Color, eNodeSelectType.NoVirtual);

                foreach (NodeDescendantProfile colorProf in colorNodeList)
                {
                    nodeProfile = SAB.HierarchyServerSession.GetNodeData(nodeRID: colorProf.Key);
                    if (nodeProfile.ColorOrSizeCodeRID > 0)
                    {
                        // adjust count based on sell thru
                        if (currentStyleKey != 0
                            && currentStyleKey != nodeProfile.HomeHierarchyParentRID)
                        {
                            if (styleColorNodeCount > 1
                                && sellThruThreshold > 0
                                && colorNodeCountWithRevenue > 0)
                            {
                                if (colorNodeCountThatExceedsSellThru > 1) // if multiple colors for a style sell thru exceed threshold increment count
                                {
                                    ++thresholdAdjustment;
                                }
                                else if (colorNodeCountThatExceedsSellThru == 0) // if no colors for a style sell thru exceed threshold decrement count
                                {
                                    --thresholdAdjustment;
                                }
                            }

                            // reset counts for next style
                            colorNodeCountThatExceedsSellThru = 0;
                            styleColorNodeCount = 0;
                            colorNodeCountWithRevenue = 0;
                        }

                        // Create chain cube with color basis as the basis items to obtain sales values.
                        cubeGroup = (PlanCubeGroup)_transaction.CreateChainPlanMaintCubeGroup();
                        planOpenParms = new PlanOpenParms(ePlanSessionType.ChainSingleLevel, null);
                        FillOpenParmForPlan(planOpenParms: planOpenParms, ab: ab, nodeProfile: nodeProfile);
                        ((ChainPlanMaintCubeGroup)cubeGroup).OpenCubeGroup(planOpenParms);
                        colorRevenue = GetSalesValueFromCube(ab: ab, cubeGroup: cubeGroup, nodeProfile: nodeProfile);
                        if (colorRevenue > 0)
                        {
                            ++colorNodeCount;
                            ++colorNodeCountWithRevenue;
                        }

                        ++styleColorNodeCount;
                        if (sellThruThreshold > 0)
                        {
                            colorSellThru = GetSellThruValueFromCube(ab: ab, cubeGroup: cubeGroup, nodeProfile: nodeProfile);
                            if (colorSellThru >= sellThruThreshold)
                            {
                                ++colorNodeCountThatExceedsSellThru;
                            }
                        }
                        // clean up cube resources
                        cubeGroup.Dispose();
                        cubeGroup = null;
                    }

                    currentStyleKey = nodeProfile.HomeHierarchyParentRID;
                }  // end descendent loop

                if (styleColorNodeCount > 1
                    && sellThruThreshold > 0
                    && colorNodeCountWithRevenue > 0)
                {
                    if (colorNodeCountThatExceedsSellThru > 1)
                    {
                        ++thresholdAdjustment;
                    }
                    else if (colorNodeCountThatExceedsSellThru == 0)
                    {
                        --thresholdAdjustment;
                    }
                }

                if (asp.CalculatedRevenue > 0)
                {
                    // use decimal math for rounding
                    colorNodeCount = Convert.ToInt32(((double)asp.TargetRevenue / (double)asp.CalculatedRevenue) * colorNodeCount);
                }
                colorNodeCount += thresholdAdjustment;

                // Adjust the style/color node count with the basis percentage
                totalColorNodeCount += Convert.ToInt32(colorNodeCount * (ab.Weight / 100));
            }

            if (totalColorNodeCount == 0)
            {
                MIDEnvironment.Message = "Revenue values not found.  Placeholders will not be created.";
                MIDEnvironment.requestFailed = true;
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, "Revenue values not found.  Placeholders will not be created.", this.ToString());
                return false;
            }

            _placeholderCount = totalColorNodeCount;
            _transaction.CurrentProcessStep = 1;

            int maximumPlaceholderCount = 100;

            if (_placeholderCount > maximumPlaceholderCount)
            {
                throw new MessageRequestException(
                    messageRequest: eMessageRequest.CreatePlaceholderContinue,
                    messageDetails: new ROMessageDetailsCreatePlaceholders(placeholderCount: _placeholderCount)
                    );
            }

            // set response to process will continue if no error occurs.
            _transaction.MessageResponse = eMessageResponse.Yes;

            return true;
        }

        private bool CreatePlaceholdersBasedOnRevenue_CreatePlaceholders(AssortmentProfile asp, bool commitToDb)
        {
            int placeHolderCount = _placeholderCount;

            //int listCnt = this._averageUnitList.Count;

            //List<int> asrtTotalList = new List<int>();
            //List<int> numStoresList = new List<int>();
            //List<int> avgStoreList = new List<int>();
            //List<int> newPlaceholderList = new List<int>();
            //for (int i = 0; i < listCnt; i++)
            //{
            //    asrtTotalList.Add(0);
            //    numStoresList.Add(0);
            //    avgStoreList.Add(0);
            //    newPlaceholderList.Add(0);
            //}
            List<AssortStyleClosed> closedList = new List<AssortStyleClosed>();

            AssortmentDetailData assortDetailData = _asrtCubeGroup.AssortmentDetailData;
            try
            {
                DataTable dt = AssortmentProfile.CreateAssortmentComponentTable(_asrtCubeGroup.AssortmentComponentVariables);
                PackColorProfileXRef packColorXRef = new PackColorProfileXRef();
                asp.GetAssortmentComponents(this._asrtCubeGroup, dt, packColorXRef);

                ProfileList gradeList = asp.GetAssortmentStoreGrades();
                ProfileList sgll = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);

                Cube myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryTotal);
                AssortmentCellReference asrtCellRef = new AssortmentCellReference((AssortmentSummaryTotal)myCube);

                //==============================
                // Total num stores from cube
                //==============================
                double totNumStores = 0.0d;
                asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.NumStores).Key;
                asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
                totNumStores = asrtCellRef.CurrentCellValue;
                Debug.WriteLine("Total Num Stores: " + totNumStores.ToString());

                //================================
                // Num stores from all set/grades
                //================================
                double cellValue = 0.0d;
                int aIdx = -1;

                myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryGrade);
                asrtCellRef = new AssortmentCellReference((AssortmentSummaryGrade)myCube);

                //foreach (StoreGradeProfile sgp in gradeList.ArrayList)
                //{
                //    aIdx++;
                //    foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
                //    {
                //        asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
                //        asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
                //        asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.NumStores).Key;
                //        asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
                //        cellValue = asrtCellRef.CurrentCellValue;
                //        numStoresList[aIdx] += (int)cellValue;

                //        asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
                //        asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
                //        asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.Units).Key;
                //        asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
                //        cellValue = asrtCellRef.CurrentCellValue;
                //        asrtTotalList[aIdx] += (int)cellValue;
                //    }
                //}

                //==================================================================
                // Find the average store per grade.
                // then divide the average store per grade by the average units
                // entered by the user.
                //==================================================================
                //for (int i = 0; i < listCnt; i++)
                //{
                //    if (numStoresList[i] > 0)
                //    {
                //        avgStoreList[i] = asrtTotalList[i] / numStoresList[i];
                //        if (_averageUnitList[i] > 0)
                //        {
                //            newPlaceholderList[i] = avgStoreList[i] / _averageUnitList[i];
                //        }
                //    }
                //}

                //==============================================
                // Determine the maximum number of placeholders
                //==============================================
                //int maxPlaceHolders = GetMaxPlaceholders(listCnt, newPlaceholderList);
                int maxPlaceHolders = placeHolderCount;

                //=============================================================
                // Get new placeholder nodes and add placeholder style headers
                //=============================================================
                int placeholderCount = GetPlaceHolderStyleCount(dt);
                ArrayList headerRids = new ArrayList();
                if (maxPlaceHolders > placeholderCount)
                {
                    int numOfNewPlaceholders = maxPlaceHolders - placeholderCount;
                    HierarchyNodeList hierNodeList = SAB.HierarchyServerSession.GetPlaceholderStyles(asp.AssortmentAnchorNodeRid,
                            numOfNewPlaceholders, placeholderCount, asp.Key);

                    AllocationProfile ap = null;
                    int asrtPlaceholderSeq = 0;
                    foreach (HierarchyNodeProfile hnp in hierNodeList)
                    {
                        placeholderCount++;
                        string lblPhStyle = MIDText.GetTextOnly(eMIDTextCode.lbl_PhStyle);
                        ap = new AllocationProfile(_transaction, asp.HeaderID + " " + lblPhStyle + " " + placeholderCount.ToString(CultureInfo.CurrentUICulture),
                                Include.NoRID, SAB.ApplicationServerSession);

                        ap.StyleHnRID = hnp.Key;
                        ap.AsrtRID = asp.Key;
                        ap.PlanHnRID = asp.PlanHnRID;
                        MIDException midException;
                        if (!ap.SetHeaderType(eHeaderType.Placeholder, out midException))
                        {
                            throw midException;
                        }
                        ap.AsrtPlaceholderSeq = ++asrtPlaceholderSeq;

                        ap.HeaderDescription = hnp.NodeDescription;
                        ap.HeaderDay = DateTime.Now;
                        DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(asp.AssortmentCalendarDateRangeRid);
                        ap.ShipToDay = SAB.ClientServerSession.Calendar.GetFirstDayOfRange(drp).Date;

                        if (asp.AssortmentBeginDayCalendarDateRangeRid != Include.UndefinedCalendarDateRange)
                        {
                            DateRangeProfile drpBeginDay = SAB.ClientServerSession.Calendar.GetDateRange(asp.AssortmentBeginDayCalendarDateRangeRid);
                            ap.BeginDay = SAB.ClientServerSession.Calendar.GetFirstDayOfRange(drpBeginDay).Date;
                        }

                        ap.WriteHeader();
                        _transaction.AddAllocationProfile(ap);

                        headerRids.Add(ap.Key);
                        //AddToClosedList(listCnt, newPlaceholderList, closedList, gradeList, sgll, placeholderCount, ap);
                    }
                }

                //=============================================
                // Write Average values to new placeholders
                //=============================================
                assortDetailData.OpenUpdateConnection();
                assortDetailData.Variable_XMLInit();
                int writeCount = 0;
                int totalAssortmentUnits = 0;
                foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
                {
                    int gradeIdx = 0;
                    foreach (StoreGradeProfile sgp in gradeList.ArrayList)
                    {
                        //int avg = _averageUnitList[gradeIdx++]; 
                        int avg = 0;

                        foreach (int headerRid in headerRids)
                        {
                            bool closedStyle = false;
                            foreach (AssortStyleClosed asc in closedList)
                            {
                                if (asc.HeaderRid == headerRid && asc.StoreGroupLevelRid == sglp.Key && asc.Grade == sgp.Key)
                                {
                                    closedStyle = true;
                                }
                            }

                            if (!closedStyle)
                            {
                                ProfileList storeList = _asrtCubeGroup.GetStoresInSetGrade(sglp.Key, sgp.Key);
                                AllocationProfile ap = _transaction.GetAllocationProfile(headerRid);
                                int value = storeList.Count * avg;
                                ap.SetAllocatedUnits(storeList, value);
                                totalAssortmentUnits += value;

                                assortDetailData.AssortmentMatrixDetail_XMLInsert(
                                    headerRid, int.MaxValue, int.MaxValue, sglp.Key, sgp.Key, avg, false);

                                writeCount++;
                                if (writeCount > MIDConnectionString.CommitLimit)
                                {
                                    assortDetailData.AssortmentMatrixDetail_XMLUpdate();
                                    assortDetailData.Variable_XMLInit();
                                    writeCount = 0;
                                }
                            }
                        }

                    }
                }

                foreach (int headerRid in headerRids)
                {
                    AllocationProfile ap = _transaction.GetAllocationProfile(headerRid);
                    ap.WriteHeader();
                }

                // Update total assortment units
                asp.WriteHeader();

                if (writeCount > 0)
                {
                    assortDetailData.AssortmentMatrixDetail_XMLUpdate();
                }

                assortDetailData.CommitData();


                InsertAssortStyleClosed(closedList);

                if (commitToDb)
                {
                    asp.WriteHeader();
                }
                _transaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
                string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodCompletedSuccessfully);
                msg = msg.Replace("{0}", "Assortment");
                msg = msg.Replace("{1}", "Create Placeholders");
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());

                _transaction.CurrentProcessStep = 2;

                return true;
            }
            catch (Exception ex)
            {
                _transaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionFailed);
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), this.ToString());
                string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
                msg = msg.Replace("{0}", "Assortment");
                msg = msg.Replace("{1}", "Create Placeholders");
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.ToString());
                throw;
            }
            finally
            {
                if (assortDetailData.ConnectionIsOpen)
                {
                    assortDetailData.CloseUpdateConnection();
                }
            }
        }

        /// <summary>
		/// Fills in the plan part of the CubeGroup open parms
		/// </summary>
		private void FillOpenParmForPlan(PlanOpenParms planOpenParms, AssortmentBasis ab, HierarchyNodeProfile nodeProfile)
        {
            planOpenParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.NotSpecified);

            planOpenParms.FunctionSecurityProfile.SetReadOnly();

            planOpenParms.StoreGroupRID = SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
            planOpenParms.GroupBy = eStorePlanSelectedGroupBy.ByTimePeriod;
            planOpenParms.ViewRID = Include.DefaultPlanViewRID;
            planOpenParms.DisplayTimeBy = eDisplayTimeBy.ByWeek;
            planOpenParms.IneligibleStores = false;
            planOpenParms.SimilarStores = false;
            planOpenParms.LowLevelsType = eLowLevelsType.None;
            planOpenParms.IsMulti = false;
            planOpenParms.IsTotRT = false;

            planOpenParms.ChainHLPlanProfile.VersionProfile = ab.VersionProfile;
            planOpenParms.ChainHLPlanProfile.NodeProfile = nodeProfile;
            planOpenParms.DateRangeProfile = ab.HorizonDate;

            planOpenParms.ComputationsMode = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
        }

        private double GetSalesValueFromCube(AssortmentBasis ab, PlanCubeGroup cubeGroup, HierarchyNodeProfile nodeProfile)
        {
            double targetRevenue = 0;
            PlanCellReference planCellRef;
            int variableKey;

            variableKey = DetermineSalesVariable(planLevlType: ab.HierarchyNodeProfile.OTSPlanLevelType);

            if (variableKey > 0)
            {
                planCellRef = (PlanCellReference)((PlanCube)cubeGroup.GetCube(eCubeType.ChainPlanDateTotal)).CreateCellReference();
                planCellRef[eProfileType.Version] = ab.VersionProfile.Key;
                planCellRef[eProfileType.HierarchyNode] = nodeProfile.Key;
                planCellRef[eProfileType.TimeTotalVariable] = ((VariableProfile)_transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(variableKey)).GetChainTimeTotalVariable(1).Key;
                planCellRef[eProfileType.QuantityVariable] = _transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
                planCellRef[eProfileType.Variable] = variableKey;

                targetRevenue = planCellRef.CurrentCellValue * (ab.Weight / 100);
            }

            return targetRevenue;
        }

        private int DetermineSalesVariable(eOTSPlanLevelType planLevlType)
        {
            foreach (VariableProfile vp in _transaction.PlanComputations.PlanVariables.VariableProfileList)
            {
                if (vp.VariableName.Trim() == "Tot Sales $")
                {
                    return vp.Key;
                }
            }

            foreach (VariableProfile vp in _transaction.PlanComputations.PlanVariables.VariableProfileList)
            {
                if (planLevlType == eOTSPlanLevelType.Regular
                    && vp.DatabaseColumnName == "SALES_REG_DLR")
                {
                    return vp.Key;
                }
                else if (planLevlType != eOTSPlanLevelType.Regular
                    && vp.DatabaseColumnName == "TOT_STR_SALES_DLR")
                {
                    return vp.Key;
                }
            }

            foreach (VariableProfile vp in _transaction.PlanComputations.PlanVariables.VariableProfileList)
            {
                if (planLevlType == eOTSPlanLevelType.Regular
                    && vp.DatabaseColumnName == "SALES_REG")
                {
                    return vp.Key;
                }
                else if (planLevlType != eOTSPlanLevelType.Regular
                    && vp.DatabaseColumnName == "SALES")
                {
                    return vp.Key;
                }
            }

            return Include.Undefined;
        }

        private double GetSellThruValueFromCube(AssortmentBasis ab, PlanCubeGroup cubeGroup, HierarchyNodeProfile nodeProfile)
        {
            double sellThru = 0;
            PlanCellReference planCellRef;
            int variableKey;

            variableKey = DetermineSellThruVariable(planLevlType: ab.HierarchyNodeProfile.OTSPlanLevelType);

            if (variableKey > 0)
            {
                planCellRef = (PlanCellReference)((PlanCube)cubeGroup.GetCube(eCubeType.ChainPlanDateTotal)).CreateCellReference();
                planCellRef[eProfileType.Version] = ab.VersionProfile.Key;
                planCellRef[eProfileType.HierarchyNode] = nodeProfile.Key;
                planCellRef[eProfileType.TimeTotalVariable] = ((VariableProfile)_transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(variableKey)).GetChainTimeTotalVariable(1).Key;
                planCellRef[eProfileType.QuantityVariable] = _transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;
                planCellRef[eProfileType.Variable] = variableKey;

                //sell thru percentage is not affected by weighting
                sellThru = planCellRef.CurrentCellValue;
            }

            return sellThru;
        }

        private int DetermineSellThruVariable(eOTSPlanLevelType planLevlType)
        {
            foreach (VariableProfile vp in _transaction.PlanComputations.PlanVariables.VariableProfileList)
            {
                if (vp.VariableName == "Str Sell Thru")
                {
                    return vp.Key;
                }
            }

            foreach (VariableProfile vp in _transaction.PlanComputations.PlanVariables.VariableProfileList)
            {
                if (planLevlType == eOTSPlanLevelType.Regular
                    && vp.VariableName == "Sell Thru % R/P")
                {
                    return vp.Key;
                }
                else if (planLevlType != eOTSPlanLevelType.Regular
                    && vp.VariableName == "Sell Thru %")
                {
                    return vp.Key;
                }
            }

            return Include.Undefined;
        }

        private bool Quantity(AssortmentProfile asp, bool commitToDb)
		{
			//List<int> asrtTotalList = new List<int>();
			//List<int> numStoresList = new List<int>();
			//List<int> avgStoreList = new List<int>();
			//List<int> newPlaceholderList = new List<int>();
			//for (int i = 0; i < gradeCnt; i++)
			//{
			//    asrtTotalList.Add(0);
			//    numStoresList.Add(0);
			//    avgStoreList.Add(0);
			//    newPlaceholderList.Add(0);
			//}
			//List<AssortStyleClosed> closedList = new List<AssortStyleClosed>();

			try
			{
				//DataTable dt = AssortmentProfile.CreateComponentTable(_asrtCubeGroup.AssortmentComponentVariables);
				//asp.GetComponents(this._asrtCubeGroup, dt);

				//Debug.WriteLine("Quantity");


				Cube myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentComponentHeaderGrade);
				AssortmentCellReference asrtCellRef = new AssortmentCellReference((AssortmentComponentHeaderGrade)myCube);

				ProfileList gradeList = asp.GetAssortmentStoreGrades();		// TT#488-MD - STodd - Group Allocation 

				ProfileList sgll = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);


				myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryGrade);
				asrtCellRef = new AssortmentCellReference((AssortmentSummaryGrade)myCube);
				double cellValue = 0.0d;
				int gradeIdx = -1;

				foreach (StoreGradeProfile sgp in gradeList.ArrayList)
				{
					gradeIdx++;
					foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
					{
						asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
						asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
						asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.NumStores).Key;
						asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
						cellValue = asrtCellRef.CurrentCellValue;
						//Debug.WriteLine("Set: " + sglp.Key +
						//    "  Grade: " + sgp.Key +
						//    "  # Strs: " + cellValue.ToString());
						//numStoresList[gradeIdx] += (int)cellValue;

						asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
						asrtCellRef[eProfileType.StoreGrade] = sgp.Key;
						asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.AvgUnits).Key;
						asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
						cellValue = asrtCellRef.CurrentCellValue;
						//Debug.WriteLine("Set: " + sglp.Key +
						//    "  Grade: " + sgp.Key +
						//    "  Avg Units: " + cellValue.ToString());
						//asrtTotalList[gradeIdx] += (int)cellValue;
					}
				}



				//if (commitToDb)
				//{
				//    asp.WriteHeader();
				//}
				_transaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
				string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodCompletedSuccessfully);
				msg = msg.Replace("{0}", "Assortment");
				msg = msg.Replace("{1}", "Quantity");
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());

				//Debug.WriteLine("END Quantity");
				return true;
			}
			catch (Exception ex)
			{
				_transaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionFailed);
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), this.ToString());
				string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
				msg = msg.Replace("{0}", "Assortment");
				msg = msg.Replace("{1}", "Quantity");
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.ToString());
				throw;
			}

		}

		/// <summary>
		/// Inserts the closed values for the new placeholders.
		/// </summary>
		/// <param name="closedList"></param>
		private void InsertAssortStyleClosed(List<AssortStyleClosed> closedList)
		{
			AssortmentDetailData assortDetailData = _asrtCubeGroup.AssortmentDetailData;
			try
			{
				if (!assortDetailData.ConnectionIsOpen)
					assortDetailData.OpenUpdateConnection();
				foreach (AssortStyleClosed asc in closedList)
				{
					assortDetailData.AssortmentStyleClosed_InsertClosed(asc.HeaderRid, asc.StoreGroupLevelRid, asc.Grade);
				}
				assortDetailData.CommitData();
			}
			catch
			{
				throw;
			}
			finally
			{
				assortDetailData.CloseUpdateConnection();
			}
		}

		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		//private bool CancelAssortmentDetail(List<int> headerRidList)
		private bool CancelAssortmentDetail(SelectedHeaderList selectedHeaderList)
		// END TT#371-MD - stodd -  Velocity Interactive on Assortment
		{
			bool success = true;
			AssortmentDetailData assortDetailData = _asrtCubeGroup.AssortmentDetailData;
			try
			{
				GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
				bool aReviewFlag = false;
				bool aUseSystemTolerancePercent = false;
				double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
				int aStoreFilter = Include.AllStoreFilterRID;
				int aWorkFlowStepKey = -1;

				// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
				foreach (SelectedHeaderProfile shp in selectedHeaderList)
				// END TT#371-MD - stodd -  Velocity Interactive on Assortment
				{
                    // Begin TT#2003-MD - JSmith - Select Cancel Assortment and Process.  After processing the Asstorment and Style review are not cancelled.  Expected them to be 0'd out.
                    if (shp.HeaderType != eHeaderType.Assortment)
                    {
                        continue;
                    }
                    // End TT#2003-MD - JSmith - Select Cancel Assortment and Process.  After processing the Asstorment and Style review are not cancelled.  Expected them to be 0'd out.
					ApplicationBaseAction aMethod = _transaction.CreateNewMethodAction(eMethodType.BackoutAllocation);
					AllocationWorkFlowStep aAllocationWorkFlowStep
						= new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
					_transaction.DoAllocationAction(aAllocationWorkFlowStep);

					// BEGIN TT#1936 - stodd - cancel allocation/assortment
					// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
					AllocationProfile ap = _transaction.GetAllocationProfile(shp.Key);
					// END TT#371-MD - stodd -  Velocity Interactive on Assortment
					if (ap != null)
					{
						if (ap.HeaderType == eHeaderType.Placeholder)
						{
							//ap.TotalUnitsToAllocate = 0; TT#774 - MD - DOConnell - Change how TotalUnitsToAllocate are calculated for an assortment profile so it is calculated as needed instead of maintained by the assortment review.
							ap.WriteHeader();
						}
					}
					// END TT#1936 - stodd - cancel allocation/assortment
				}

				return success;
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}

		// Begin TT#1224 - stodd - committed
		private bool ChargeCommittedDetail(List<int> headerRidList)
		{
			bool success = true;
			AssortmentDetailData assortDetailData = _asrtCubeGroup.AssortmentDetailData;
			try
			{
				GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
				bool aReviewFlag = false;
				bool aUseSystemTolerancePercent = false;
				double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
				int aStoreFilter = Include.AllStoreFilterRID;
				int aWorkFlowStepKey = -1;

				foreach (int headerRid in headerRidList)
				{
					ApplicationBaseAction aMethod = _transaction.CreateNewMethodAction(eMethodType.ChargeCommitted);
					AllocationWorkFlowStep aAllocationWorkFlowStep
						= new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
					_transaction.DoAllocationAction(aAllocationWorkFlowStep);
				}

				return success;
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}

		private bool CancelCommittedDetail(List<int> headerRidList)
		{
			bool success = true;
			AssortmentDetailData assortDetailData = _asrtCubeGroup.AssortmentDetailData;
			try
			{
				GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
				bool aReviewFlag = false;
				bool aUseSystemTolerancePercent = false;
				double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
				int aStoreFilter = Include.AllStoreFilterRID;
				int aWorkFlowStepKey = -1;

				foreach (int headerRid in headerRidList)
				{
					ApplicationBaseAction aMethod = _transaction.CreateNewMethodAction(eMethodType.CancelCommitted);
					AllocationWorkFlowStep aAllocationWorkFlowStep
						= new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
					_transaction.DoAllocationAction(aAllocationWorkFlowStep);
				}

				return success;
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}
		// End TT#1224 - stodd - committed


		// Begin TT#1225 - stodd - committed
		private bool ChargeIntransitDetail()
		{
			bool success = true;
			try
			{
				GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
				bool aReviewFlag = false;
				bool aUseSystemTolerancePercent = false;
				double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
				int aStoreFilter = Include.AllStoreFilterRID;
				int aWorkFlowStepKey = -1;

				ApplicationBaseAction aMethod = _transaction.CreateNewMethodAction(eMethodType.ChargeIntransit);
				AllocationWorkFlowStep aAllocationWorkFlowStep
					= new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
				_transaction.DoAllocationAction(aAllocationWorkFlowStep);

				return success;
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}

		private bool CancelIntransitDetail()
		{
			bool success = true;
			try
			{
				GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
				bool aReviewFlag = false;
				bool aUseSystemTolerancePercent = false;
				double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
				int aStoreFilter = Include.AllStoreFilterRID;
				int aWorkFlowStepKey = -1;

				ApplicationBaseAction aMethod = _transaction.CreateNewMethodAction(eMethodType.BackoutStyleIntransit);
				AllocationWorkFlowStep aAllocationWorkFlowStep
					= new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
				_transaction.DoAllocationAction(aAllocationWorkFlowStep);

				return success;
			}
			catch
			{
				throw;
			}
			finally
			{
			}
		}
		// End TT#1225 - stodd - committed

		/// <summary>
		/// Gets the largest new placeholder count amoung all grades.
		/// </summary>
		/// <param name="listCnt"></param>
		/// <param name="newPlaceholderList"></param>
		/// <returns></returns>
		private static int GetMaxPlaceholders(int listCnt, List<int> newPlaceholderList)
		{
			int maxIndex = 0;
			int maxPlaceHolders = 0;
			for (int i = 0; i < listCnt; i++)
			{
				if (newPlaceholderList[i] > maxPlaceHolders)
				{
					maxIndex = i;
					maxPlaceHolders = newPlaceholderList[i];
				}
			}
			return maxPlaceHolders;
		}

		/// <summary>
		/// Determines which grades for the placeholder style should be closed.
		/// </summary>
		/// <param name="listCnt"></param>
		/// <param name="newPlaceholderList"></param>
		/// <param name="closedList"></param>
		/// <param name="gradeList"></param>
		/// <param name="placeholderCount"></param>
		/// <param name="ap"></param>
		private static void AddToClosedList(int listCnt, List<int> newPlaceholderList, List<AssortStyleClosed> closedList,
			ProfileList gradeList, ProfileList sgll, int placeholderCount, AllocationProfile ap)
		{
			for (int i = 0; i < listCnt; i++)
			{
				if (newPlaceholderList[i] < placeholderCount)
				{
					//=================================================
					// These need to be created for ALL Groupl Levels
					//=================================================
					foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
					{
						AssortStyleClosed asc = new AssortStyleClosed();
						asc.HeaderRid = ap.Key;
						asc.StoreGroupLevelRid = sglp.Key;
						StoreGradeProfile sgp = (StoreGradeProfile)gradeList[i];
						asc.Grade = sgp.Key;
						closedList.Add(asc);
					}
				}
			}
		}



		private int GetPlaceHolderStyleCount(DataTable dt)
		{
			ArrayList placeholderList = new ArrayList();
			int placeholderRid = Include.NoRID;
			try
			{
				foreach (DataRow aRow in dt.Rows)
				{
					placeholderRid = Convert.ToInt32(aRow["PLACEHOLDER_RID"], CultureInfo.CurrentUICulture);
					if (placeholderRid != int.MaxValue)
					{
						if (!placeholderList.Contains(placeholderRid))
							placeholderList.Add(placeholderRid);
					}
				}
				return placeholderList.Count;
			}
			catch
			{

				throw;
			}
		}

		private bool SpreadAverage(AssortmentProfile asp, bool commitToDb)
		{
			try
			{
				ProfileList gradeList = asp.GetAssortmentStoreGrades();	// TT#488-MD - STodd - Group Allocation 
				//================================================================
				// Get Store Group Level list.
				// if Index to Average is "Total", all sets are processed.
				// Otherwise, only the current set is processed.
				//================================================================
				ProfileList sgll = null;
				if (IndexToAverageReturnType == eIndexToAverageReturnType.Total)
				{
					sgll = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);
				}
				else
				{
					sgll = new ProfileList(eProfileType.StoreGroupLevel);
					sgll.Add(new StoreGroupLevelProfile(CurrSglRid));
				}
				AllocationHeaderProfileList ahpl = (AllocationHeaderProfileList)_asrtCubeGroup.GetHeaderList();

				//======================================================
				// If average by all stores, get that average now...
				//======================================================
				double basisAvg = 0.0d;
				if (asp.AssortmentAverageBy == eStoreAverageBy.AllStores)
				{
					Cube myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryTotal);
					AssortmentCellReference asrtCellRef = new AssortmentCellReference((AssortmentSummaryTotal)myCube);
					asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.AvgStore).Key;
					asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
					basisAvg = asrtCellRef.CurrentCellValue;
				}

				int totalUnitsAllocated = 0;
				foreach (AllocationHeaderProfile headerProf in ahpl)
				{
					if ((eAssortmentType)asp.AsrtType == eAssortmentType.PostReceipt)
					{
						if (headerProf.HeaderType != eHeaderType.Placeholder)
						{
                            // Begin TT#219 - MD - DOConnell - Spread Average not getting expected results
                            //SpreadAverageForHeader(asp, gradeList, sgll, ref basisAvg, ref totalUnitsAllocated, headerProf);
							// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                            //if (asp.AppSessionTransaction.AllocationSelHdrList.Contains(headerProf.Key))
							if (asp.AppSessionTransaction.AssortmentSelectedHdrList.Contains(headerProf.Key))
							// END TT#371-MD - stodd -  Velocity Interactive on Assortment
                            {
                                // Begin TT#1563-MD - stodd - Post Receipt run Spread Avg over a group of headers. Values in Matrix for stores does not match the header or style review.
                                // Only send the headers to the method, not the assortment profile.
                                if (headerProf.Assortment != true)
                                {
                                    SpreadAverageForHeader(asp, gradeList, sgll, ref basisAvg, ref totalUnitsAllocated, headerProf);
                                }
                                // End TT#1563-MD - stodd - Post Receipt run Spread Avg over a group of headers. Values in Matrix for stores does not match the header or style review.
                            }
							//BEGIN TT#388 - MD - DOConnell - When actions are run only on specific headers/placeholders the assortment total is not being changed correctly on the Assortment workspace
                            else
                            {
                                totalUnitsAllocated = totalUnitsAllocated + headerProf.AllocatedUnits;
                            }
							//END TT#388 - MD - DOConnell - When actions are run only on specific headers/placeholders the assortment total is not being changed correctly on the Assortment workspace
                            // End TT#219 - MD - DOConnell - Spread Average not getting expected results
						}
					}
					else if ((eAssortmentType)asp.AsrtType == eAssortmentType.PreReceipt)
					{
                        // Begin TT#219 - MD - DOConnell - Spread Average not getting expected results
                        //SpreadAverageForHeader(asp, gradeList, sgll, ref basisAvg, ref totalUnitsAllocated, headerProf);
						// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
                        //if (asp.AppSessionTransaction.AllocationSelHdrList.Contains(headerProf.Key))
						if (asp.AppSessionTransaction.AssortmentSelectedHdrList.Contains(headerProf.Key))
						// END TT#371-MD - stodd -  Velocity Interactive on Assortment
                        {
                            if (headerProf.Assortment != true)
                            {
                                SpreadAverageForHeader(asp, gradeList, sgll, ref basisAvg, ref totalUnitsAllocated, headerProf);
                            }
						//BEGIN TT#388 - MD - DOConnell - When actions are run only on specific headers/placeholders the assortment total is not being changed correctly on the Assortment workspace
                        }
                        else
                        {
                            totalUnitsAllocated = totalUnitsAllocated + headerProf.AllocatedUnits;
                        }
						//END TT#388 - MD - DOConnell - When actions are run only on specific headers/placeholders the assortment total is not being changed correctly on the Assortment workspace
                        // End TT#219 - MD - DOConnell - Spread Average not getting expected results
					}
				}

				// Begin TT#1261 - stodd - refreshing assortment workspace
				//asp.TotalUnitsToAllocate = totalUnitsAllocated; TT#774 - MD - DOConnell - Change how TotalUnitsToAllocate are calculated for an assortment profile so it is calculated as needed instead of maintained by the assortment review.
				asp.WriteHeader();
				if (commitToDb)
				{
					asp.WriteHeader();
				}
				// End TT#1261 - stodd - refreshing assortment workspace

				_transaction.SetAllocationActionStatus(asp.Key, eAllocationActionStatus.ActionCompletedSuccessfully);
				string msg = MIDText.GetText(eMIDTextCode.msg_al_MethodCompletedSuccessfully);
				msg = msg.Replace("{0}", "Assortment");
				msg = msg.Replace("{1}", "Spread Average");
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString());

				return true;
			}
			catch
			{
				throw;
			}

		}

		private void SpreadAverageForHeader(AssortmentProfile asp, ProfileList gradeList, ProfileList sgll, ref double basisAvg, ref int totalUnitsAllocated, AllocationHeaderProfile headerProf)
		{
			bool debug = false;
			Hashtable groupHash = new Hashtable();
			foreach (StoreGroupLevelProfile sglp in sgll.ArrayList)
			{
				Debug.WriteLineIf(debug, "Attr Set: " + sglp.Name);
				//======================================================
				// If average by set, get that average now...
				//======================================================
				if (asp.AssortmentAverageBy == eStoreAverageBy.Set)
				{
					Cube myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentSummaryGroupLevel);
					AssortmentCellReference asrtCellRef = new AssortmentCellReference((AssortmentSummaryGroupLevel)myCube);
					asrtCellRef[eProfileType.StoreGroupLevel] = sglp.Key;
                    // Begin TT#2012-MD - JSmith - Spread Avg - Smooth - Set Total - Process - receivev Null reference exception
                    //asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.AvgStore).Key;
                    asrtCellRef[eProfileType.AssortmentSummaryVariable] = _asrtCubeGroup.AssortmentComputations.AssortmentSummaryVariables.VariableProfileList.FindKey((int)eAssortmentSummaryVariables.AvgStore).Key;
                    // End TT#2012-MD - JSmith - Spread Avg - Smooth - Set Total - Process - receivev Null reference exception
					asrtCellRef[eProfileType.AssortmentQuantityVariable] = (int)eAssortmentQuantityVariables.Value;
					basisAvg = asrtCellRef.CurrentCellValue;
					Debug.WriteLineIf(debug, " Attr Set Average: " + basisAvg);
				}

				bool skipGrade = false;
				foreach (StoreGradeProfile sgp in gradeList)
				{
					//===============================================================
					// Determine where the proper Index to Average value is stored
					//===============================================================
					double averageUnits = 0;
					if (IndexToAverageReturnType == eIndexToAverageReturnType.Total || IndexToAverageReturnType == eIndexToAverageReturnType.SetTotal)
					{
						averageUnits = this.AverageUnits;
					}
					else
					{
						if (_gradeAverageUnitsHash.ContainsKey(sgp.StoreGrade))
						{
							averageUnits = (double)_gradeAverageUnitsHash[sgp.StoreGrade];
						}
						else
						{
							skipGrade = true;
						}
					}
					Debug.WriteLineIf(debug, "  " + IndexToAverageReturnType + " Grade: " + sgp.StoreGrade + " Average Units: " + averageUnits);

					//=================================================================================
					// There's a chance that if the Index to Average values where entered by grade, 
					// that some grades where skipped. 
					//=================================================================================
					if (!skipGrade)
					{
						ProfileList storeList = _asrtCubeGroup.GetStoresInSetGrade(sglp.Key, sgp.Key);

						foreach (StoreProfile sp in storeList)
						{
							AssortmentSummaryStoreDetailProfile sdp = asp.AssortmentSummaryProfile.GetStoreDetail(asp.AssortmentVariableNumber, sp.Key);
							double indexToAverage = sdp.Index;
							//double spreadQty = (gradeIndexToAverage * (basisAvg / basisAvg));
							double spreadQty = (averageUnits * indexToAverage) / 100;
							if (double.IsNaN(spreadQty) || double.IsInfinity(spreadQty))
							{ spreadQty = 0; }


							int spreadRound = (int)spreadQty;
							spreadQty = spreadRound;

							if (asp.AssortmentAverageBy == eStoreAverageBy.AllStores)
							{
								AddToStoreSortListChain(ref groupHash, sglp.Key, sgp.Key, sp.Key, spreadRound);
							}
							else
							{
								AddToStoreSortListSet(ref groupHash, sglp.Key, sgp.Key, sp.Key, spreadRound);
							}
							Debug.WriteLineIf(debug, "   " + "Set: " + sglp.Name + " Grade: " + sgp.StoreGrade + " store: " + sp.StoreId + "(" + sp.Key + ") " + " Units: " +  sdp.Units + " IndexToAvg: " + indexToAverage + " <avgUnits * IndexTAvg> " + spreadQty);
						}
					}
				}
			}

			if (groupHash.Count > 0)
			{
				foreach (int sglRID in groupHash.Keys)
				{
					SortedList sl = (SortedList)groupHash[sglRID];
					{
						foreach (string sglGradeKey in sl.Keys)
						{
							string[] keys = new string[2];
							keys = sglGradeKey.Split(',');
							int gradeBoundary = int.Parse(keys[1]);

							int sglGradeQtyTotal = 0;
							Hashtable ht = (Hashtable)sl[sglGradeKey];
							foreach (ActionStoreValue store in ht.Values)
							{
								sglGradeQtyTotal = sglGradeQtyTotal + store.StoreValue;
							}
							int avg = (int)sglGradeQtyTotal / ht.Count;
							avg = Math.Max(avg, 0);
							Debug.WriteLineIf(debug, "SetRid: " + sglRID + " Grade: " + sglGradeKey + " SglGradeTotal: " + sglGradeQtyTotal + " Store Count: " + ht.Count + " Average: " + avg);

							//=========================
							// SMOOTHING
							//=========================
							if (_spreadAverage == eSpreadAverage.Smooth)
							{
								Debug.WriteLineIf(debug, "Smoothing is ON");
								foreach (ActionStoreValue asv in ht.Values)
								{
									asv.StoreValue = avg;
								}
							}

							if (!_asrtCubeGroup.BlockedList.Contains(new BlockedListHashKey(headerProf.Key, sglRID, gradeBoundary)))
							{
								AllocationProfile ap = _transaction.GetAllocationProfile(headerProf.Key);
								if (ap != null)
								{
								    IDictionaryEnumerator storeEnumerator = ht.GetEnumerator();
                                    Index_RID storeIdxRID; // TT#495 - MD - DOConnell/JEllis - When Spread Average is processed against a header or placeholder with more than one color, the colors do not get allocated
									while (storeEnumerator.MoveNext())
									{
										int storeRid = (int)storeEnumerator.Key;
										ActionStoreValue actionStoreValue = (ActionStoreValue)storeEnumerator.Value;
										int storeValue = actionStoreValue.StoreValue;
										Debug.WriteLineIf(debug, "   StoreRid: " + storeRid + " Store Value: " + storeValue);
										//ProfileList storeList = asp.GetStoresInSetGrade(sglp.Key, sgp.Key);
										//int storeCount = (int)numStoreDict[sgp.Key, sglp.Key];
										//double avgUnits = (double)AvgUnitsDict[sgp.Key, sglp.Key, headerProf.Key];
										//int value = (int)(storeCount * avgUnits);
                                        // Begin TT#495 - MD - DOConnell/JEllis - When Spread Average is processed against a header or placeholder with more than one color, the colors do not get allocated
										storeIdxRID = ap.StoreIndex(storeRid);
										// BEGIN TT#487-MD - Stodd - respect manually allocated values
										////foreach (HdrColorBin hcb in ap.BulkColors.Values)
										////{
										////    ap.SetStoreQtyAllocated(hcb, storeIdxRID, 1, eDistributeChange.ToParent, false);
										////}
										////foreach (PackHdr ph in ap.Packs.Values)
										////{
										////    ap.SetStoreQtyAllocated(ph, storeIdxRID, 1, eDistributeChange.ToParent, false);
										////}
										//////ap.SetStoreQtyAllocated(new GeneralComponent(eComponentType.Total), storeRid, storeValue);
										////ap.SetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID, storeValue, eDistributeChange.ToChildren, false, false, true, true);

										if (!ap.GetStoreIsManuallyAllocated(eAllocationSummaryNode.Total, storeIdxRID))
										{
											try
											{
												ap.ResetTempLocks(false);

												if (ap.GetStoreIsManuallyAllocated(eAllocationSummaryNode.Bulk, storeIdxRID))
												{
													ap.SetStoreTempLock(eAllocationSummaryNode.Bulk, storeIdxRID, true, eDistributeChange.ToChildren);
												}
												if (ap.GetStoreIsManuallyAllocated(eAllocationSummaryNode.DetailType, storeIdxRID))
												{
													ap.SetStoreTempLock(eAllocationSummaryNode.DetailType, storeIdxRID, true, eDistributeChange.ToChildren);
												}

												foreach (HdrColorBin hcb in ap.BulkColors.Values)
												{
													if (ap.GetStoreIsManuallyAllocated(hcb, storeIdxRID))
													{
														ap.SetStoreTempLock(hcb, storeIdxRID, true, eDistributeChange.ToChildren);
													}
													else
													{
														ap.SetStoreQtyAllocated(hcb, storeIdxRID, 1, eDistributeChange.ToParent, false);
													}
												}
												foreach (PackHdr ph in ap.Packs.Values)
												{
													if (ap.GetStoreIsManuallyAllocated(ph, storeIdxRID))
													{
														ap.SetStoreTempLock(ph, storeIdxRID, true, eDistributeChange.ToChildren);
													}
													else
													{
														ap.SetStoreQtyAllocated(ph, storeIdxRID, 1, eDistributeChange.ToParent, false);
													}
												}
												bool success = ap.SpreadStoreTotalUnitsAllocatedToType(storeIdxRID, storeValue, false);
												if (success)
												{
													ap.SetStoreQtyAllocated(eAllocationSummaryNode.Total, storeIdxRID, storeValue, eDistributeChange.ToNone, false);
												}
												
											}
											finally
											{
												ap.ResetTempLocks(true);
											}
										}
										// END TT#487-MD - Stodd - respect manually allocated values

										//BEGIN TT#219 - MD - DOConnell - Trying to allocate using the spread avg feature and not getting expected results.
                                        //if (headerProf.Placeholder)
                                        //{
                                        //    ap.TotalUnitsToAllocate += storeValue;
                                        //}
										//END TT#219 - MD - DOConnell - Trying to allocate using the spread avg feature and not getting expected results.
                                        // End TT#495 - MD - DOConnell/JEllis - When Spread Average is processed against a header or placeholder with more than one color, the colors do not get allocated
									}
									//ap.WriteHeader();	// TT#2098 - stodd - After Assortment Balance Quantity on Content tab does not change
								}
							}
						}
					}
				}
			}
			// Begin TT#1261 - stodd - refreshing assortment workspace
			AllocationProfile ap1 = _transaction.GetAllocationProfile(headerProf.Key);
			if (ap1 != null)
			{
				//BEGIN TT#219 - MD - DOConnell - Trying to allocate using the spread avg feature and not getting expected results.
                if (headerProf.Placeholder)
                {
                    // BEGIN TT#2098 - stodd - After Assortment Balance Quantity on Content tab does not change
                    int hdrUnitsAllocated = ap1.GetQtyAllocated(new GeneralComponent(eGeneralComponentType.Total));
                    totalUnitsAllocated += hdrUnitsAllocated;
                    //ap1.TotalUnitsToAllocate = hdrUnitsAllocated; TT#774 - MD - DOConnell - Change how TotalUnitsToAllocate are calculated for an assortment profile so it is calculated as needed instead of maintained by the assortment review.
                }
				//BEGIN TT#388 - MD - DOConnell - When actions are run only on specific headers/placeholders the assortment total is not being changed correctly on the Assortment workspace
                else
                {
                    totalUnitsAllocated = totalUnitsAllocated + ap1.TotalUnitsAllocated;
                }
				//END TT#388 - MD - DOConnell - When actions are run only on specific headers/placeholders the assortment total is not being changed correctly on the Assortment workspace
				//END TT#219 - MD - DOConnell - Trying to allocate using the spread avg feature and not getting expected results.
				ap1.WriteHeader();
				// END TT#2098 - stodd - After Assortment Balance Quantity on Content tab does not change
			}
			// End TT#1261 - stodd - refreshing assortment workspace
		}

		private void AddToStoreSortListSet(ref Hashtable aGroupHash, int sglRid, int grade, int storeRid, int storeValue)
		{
			try
			{
				SortedList sglGradeSortedList;
				MIDHashtable storeData;
				string sglGradeKey = sglRid + "," + grade;
				if (!aGroupHash.ContainsKey(sglRid))
				{
					sglGradeSortedList = new SortedList();
					storeData = new MIDHashtable();
					storeData.Add(storeRid, new ActionStoreValue(storeRid, storeValue));
					sglGradeSortedList.Add(sglGradeKey, storeData);
					aGroupHash.Add(sglRid, sglGradeSortedList);
				}
				else
				{
					sglGradeSortedList = (SortedList)aGroupHash[sglRid];
					if (!sglGradeSortedList.ContainsKey(sglGradeKey))
					{
						storeData = new MIDHashtable();
						storeData.Add(storeRid, new ActionStoreValue(storeRid, storeValue));
						sglGradeSortedList.Add(sglGradeKey, storeData);
					}
					else
					{
						storeData = (MIDHashtable)sglGradeSortedList[sglGradeKey];
						if (!storeData.ContainsKey(storeRid))
						{
							storeData.Add(storeRid, new ActionStoreValue(storeRid, storeValue));
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void AddToStoreSortListChain(ref Hashtable aGroupHash, int sglRid, int grade, int storeRid, int storeValue)
		{
			try
			{
				SortedList sglGradeSortedList;
				MIDHashtable storeData;
				string sglGradeKey = sglRid + "," + grade;

				if (!aGroupHash.ContainsKey(sglRid))
				{
					sglGradeSortedList = new SortedList();
					storeData = new MIDHashtable();
					storeData.Add(storeRid, new ActionStoreValue(storeRid, storeValue));
					sglGradeSortedList.Add(sglGradeKey, storeData);
					aGroupHash.Add(sglRid, sglGradeSortedList);
				}
				else
				{
					sglGradeSortedList = (SortedList)aGroupHash[sglRid];
					if (!sglGradeSortedList.ContainsKey(sglGradeKey))
					{
						storeData = new MIDHashtable();
						storeData.Add(storeRid, new ActionStoreValue(storeRid, storeValue));
						sglGradeSortedList.Add(sglGradeKey, storeData);
					}
					else
					{
						storeData = (MIDHashtable)sglGradeSortedList[sglGradeKey];
						if (!storeData.ContainsKey(storeRid))
						{
							storeData.Add(storeRid, new ActionStoreValue(storeRid, storeValue));
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void Read()
		{
			//List<int> valueList = new List<int>();
			//Cube myCube = _asrtCubeGroup.GetCube(eCubeType.AssortmentHeaderDetail);
			//PlanCellReference planCellRef = new PlanCellReference((AssortmentHeaderDetail)myCube);
			//SetCommonPlanCellRefIndexes(variableType, planCellRef);
			////===========================================================
			//// NOTE: The version is ALWAYS being overriden to be Actuals
			////===========================================================
			//planCellRef[eProfileType.Version] = Include.FV_ActualRID;
			//ArrayList storePlanValues = planCellRef.GetCellRefArray(_storeList);
			//for (int i = 0; i < storePlanValues.Count; i++)
			//{
			//    PlanCellReference pcr = (PlanCellReference)storePlanValues[i];
			//    valueList.Add((int)pcr.HiddenCurrentCellValue);
			//}


		}
	}

	public class ActionStoreValue
	{
		private int _storeRid;
		private int _storeValue;

		public ActionStoreValue(int storeRid, int storeValue)
		{
			_storeRid = storeRid;
			_storeValue = storeValue;
		}

		public int StoreRid
		{
			get { return _storeRid; }
			set { _storeRid = value; }
		}

		public int StoreValue
		{
			get { return _storeValue; }
			set { _storeValue = value; }
		}
	}
}
