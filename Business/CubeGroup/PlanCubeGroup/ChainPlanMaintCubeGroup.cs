using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the ChainPlanMaintCubeGroup.
	/// </summary>
	/// <remarks>
	/// The ChainPlanMaintCubeGroup predefines all Cubes that are required for single-level Chain Plan Maintenance.  Those Cubes include ChainDetailWeekCube, ChainDetailWeekTimeTotalCube.
	/// </remarks>

	public class ChainPlanMaintCubeGroup : PlanCubeGroup
	{
		//=======
		// FIELDS
		//=======

		private PlanEnqueueGroup _planEnqGrp;
		private PlanEnqueueInfo _chainPlanEnqueue;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ChainPlanMainCubeGroup, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this ChainPlanMainCubeGroup is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this ChainPlanMainCubeGroup is a part of.
		/// </param>

		public ChainPlanMaintCubeGroup(
			SessionAddressBlock aSAB,
			Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
			try
			{
				_planEnqGrp = new PlanEnqueueGroup();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Opens a ChainPlanMaintCubeGroup and creats master Profile Lists, XRef Lists, and Cubes that are required for support.
		/// </summary>
		/// <param name="aOpenParms">
		/// The PlanOpenParms object that contains information about the plan.
		/// </param>

		override public void OpenCubeGroup(PlanOpenParms aOpenParms)
		{
#if (DEBUG)
			DateTime startTime;
#endif
			ArrayList planEnqueueList;
			ChainVariableFilter varFilter;
            //IDictionaryEnumerator dictEnum;
			ProfileList planWeekList;
			ProfileList planPeriodList;
			ProfileList versionProfileList;
			ProfileList hierarchyNodeProfileList;
			CubeDefinition cubeDef;
			PlanCube planCube;
			int weekCubeSize;

			try
			{
				base.OpenCubeGroup(aOpenParms);

				//==================
				// Initialize fields
				//==================

				planWeekList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);
				planPeriodList = _openParms.GetPeriodProfileList(_SAB.ApplicationServerSession);
				versionProfileList = GetMasterProfileList(eProfileType.Version);

				//=======================
				// Enqueue the chain plan
				//=======================

				_chainPlanEnqueue = new PlanEnqueueInfo(_SAB, _openParms.ChainHLPlanProfile, ePlanType.Chain, planWeekList[0].Key, planWeekList[planWeekList.Count - 1].Key);

				if (_openParms.FunctionSecurityProfile.AllowUpdate)
				{
					planEnqueueList = new ArrayList();
					planEnqueueList.Add(_chainPlanEnqueue);
					_planEnqGrp.EnqueuePlans(_SAB, planEnqueueList, _openParms.AllowReadOnlyOnConflict, _openParms.FormatErrorsForMessageBox, _openParms.UpdateAuditHeaderOnError);
				}

				//========================================
				// Create HierarchyNode Master ProfileList
				//========================================
			
				hierarchyNodeProfileList = new ProfileList(eProfileType.HierarchyNode);

				if (!hierarchyNodeProfileList.Contains(_openParms.ChainHLPlanProfile.NodeProfile.Key))
				{
					hierarchyNodeProfileList.Add(_openParms.ChainHLPlanProfile.NodeProfile);
				}

				//========================================
				// Set Master Profile Lists and XRef Lists
				//========================================

				SetMasterProfileList(planWeekList);
				SetMasterProfileList(_openParms.GetDateProfileList(SAB.ApplicationServerSession));
				SetMasterProfileList(_openParms.GetPeriodProfileList(SAB.ApplicationServerSession));
				SetMasterProfileList(_openParms.BasisProfileList);
				SetMasterProfileList(hierarchyNodeProfileList);

				SetProfileXRef(_openParms.GetDateToWeekXRef(SAB.ApplicationServerSession));
				SetProfileXRef(_openParms.GetDateToPeriodXRef(SAB.ApplicationServerSession));

				weekCubeSize = planWeekList.Count + Include.PlanReadPreWeeks + Include.PlanReadPostWeeks;

				//=========================
				// Create and apply filters
				//=========================

				varFilter = new ChainVariableFilter(this);
				ApplyFilter(varFilter, eFilterType.Permanent);
				this.GetFilteredProfileList(eProfileType.Variable);

				//========================================
				// Create ChainPlanWeekDetail in CubeGroup
				//========================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainPlanWeekDetail);

				if (planCube == null)
				{
					planCube = new ChainPlanWeekDetail(SAB, Transaction, this, cubeDef, 1, !_chainPlanEnqueue.PlanEnqueue.isEnqueued, true);
					Transaction.PlanComputations.PlanCubeInitialization.ChainPlanWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainPlanWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//============================================
				// Create ChainPlanPeriodDetail in CubeGroup
				//============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainPlanPeriodDetail);

				if (planCube == null)
				{
					planCube = new ChainPlanPeriodDetail(SAB, Transaction, this, cubeDef, 2, !_chainPlanEnqueue.PlanEnqueue.isEnqueued, true);
					Transaction.PlanComputations.PlanCubeInitialization.ChainPlanPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainPlanPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//============================================
				// Create ChainPlanDateTotal in CubeGroup
				//============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainPlanDateTotal);

				if (planCube == null)
				{
					planCube = new ChainPlanDateTotal(SAB, Transaction, this, cubeDef, 3, !_chainPlanEnqueue.PlanEnqueue.isEnqueued, true);
					Transaction.PlanComputations.PlanCubeInitialization.ChainPlanDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainPlanDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=========================================
				// Create ChainBasisWeekDetail in CubeGroup
				//=========================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Week, weekCubeSize),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainBasisWeekDetail);

				if (planCube == null)
				{
					planCube = new ChainBasisWeekDetail(SAB, Transaction, this, cubeDef, 1);
					Transaction.PlanComputations.PlanCubeInitialization.ChainBasisWeekDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainBasisWeekDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//============================================
				// Create ChainBasisPeriodDetail in CubeGroup
				//============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.Period, GetMasterProfileList(eProfileType.Period).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainBasisPeriodDetail);

				if (planCube == null)
				{
					planCube = new ChainBasisPeriodDetail(SAB, Transaction, this, cubeDef, 2);
					Transaction.PlanComputations.PlanCubeInitialization.ChainBasisPeriodDetail(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainBasisPeriodDetail, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//=============================================
				// Create ChainBasisDateTotal in CubeGroup
				//=============================================

				cubeDef = new CubeDefinition(
					new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
					new DimensionDefinition(eProfileType.HierarchyNode, hierarchyNodeProfileList.Count),
					new DimensionDefinition(eProfileType.Basis, GetMasterProfileList(eProfileType.Basis).Count),
					new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
					new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.ChainDetailCube | QuantityLevelFlagValues.HighLevel), (ushort)(QuantityLevelFlagValues.ChainDetailCube))),
					new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count));

				planCube = (PlanCube)GetCube(eCubeType.ChainBasisDateTotal);

				if (planCube == null)
				{
					planCube = new ChainBasisDateTotal(SAB, Transaction, this, cubeDef, 3);
					Transaction.PlanComputations.PlanCubeInitialization.ChainBasisDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
					SetCube(eCubeType.ChainBasisDateTotal, planCube);
				}
				else
				{
					planCube.ExpandDimensionSize(cubeDef);
				}

				//==============================
				// Load Cube with initial values
				//==============================

#if (DEBUG)
				startTime = DateTime.Now;
#endif
				((ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail)).ReadAndLoadCube(_openParms.ChainHLPlanProfile, planWeekList);
				((ChainBasisWeekDetail)GetCube(eCubeType.ChainBasisWeekDetail)).ReadAndLoadCube(_openParms.ChainHLPlanProfile);
#if (DEBUG)
				TotalDBReadAndLoadTime = TotalPageBuildTime.Add(DateTime.Now.Subtract(startTime));
#endif
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}

		}

		/// <summary>
		/// Saves this PlanCubeGroup
		/// </summary>
		/// <param name="aSaveParms">
		/// The PlanSaveParms that contains information about the save.
		/// </param>

		override public void SaveCubeGroup(PlanSaveParms aSaveParms)
		{
			ArrayList planEnqList;
			PlanProfile planProf;
			DateRangeProfile dateRangeProf;
			ProfileList chainHighLevelWeekProfList = null;
			PlanEnqueueGroup planEnqGrp = null;
			ChainPlanWeekDetail chainCube;
			bool overrideChainHigh;

			try
			{
				planEnqList = new ArrayList();
				chainCube = (ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail);
				overrideChainHigh = false;

				if (aSaveParms.SaveChainHighLevel &&
					(aSaveParms.ChainHighLevelNodeRID != _openParms.ChainHLPlanProfile.NodeProfile.Key ||
					aSaveParms.ChainHighLevelVersionRID != _openParms.ChainHLPlanProfile.VersionProfile.Key ||
					aSaveParms.ChainHighLevelDateRangeRID != _openParms.DateRangeProfile.Key))
				{
					dateRangeProf = _SAB.ClientServerSession.Calendar.GetDateRange(aSaveParms.ChainHighLevelDateRangeRID);
					chainHighLevelWeekProfList = SAB.ApplicationServerSession.Calendar.GetWeekRange(dateRangeProf, dateRangeProf);

					planProf = new PlanProfile(aSaveParms.ChainHighLevelNodeRID);
					planProf.NodeProfile = _SAB.HierarchyServerSession.GetNodeData(aSaveParms.ChainHighLevelNodeRID);
					planProf.NodeProfile.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(aSaveParms.ChainHighLevelNodeRID, (int)eSecurityTypes.Chain);
					planProf.NodeProfile.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(aSaveParms.ChainHighLevelNodeRID, (int)eSecurityTypes.Store);
					planProf.VersionProfile = (VersionProfile)chainCube.MasterVersionProfileList.FindKey(aSaveParms.ChainHighLevelVersionRID);

					planEnqList.Add(new PlanEnqueueInfo(_SAB, planProf, ePlanType.Chain, ((WeekProfile)chainHighLevelWeekProfList[0]).Key, ((WeekProfile)chainHighLevelWeekProfList[chainHighLevelWeekProfList.Count - 1]).Key));

					overrideChainHigh = true;
				}

				planEnqGrp = new PlanEnqueueGroup();

				try
				{
					planEnqGrp.EnqueuePlans(_SAB, planEnqList, false, _openParms.FormatErrorsForMessageBox, _openParms.UpdateAuditHeaderOnError);

					if (chainCube != null)
					{
						if (aSaveParms.SaveChainHighLevel)
						{
							if (overrideChainHigh)
							{
								chainCube.SaveCube(
									_openParms.ChainHLPlanProfile.NodeProfile.Key,
									_openParms.ChainHLPlanProfile.VersionProfile.Key,
									aSaveParms.ChainHighLevelNodeRID,
									aSaveParms.ChainHighLevelVersionRID,
									//Begin Enhancement - JScott - Add Balance Low Levels functionality
									false,
									//End Enhancement - JScott - Add Balance Low Levels functionality
									chainHighLevelWeekProfList,
									false,
									//Begin Track #5690 - JScott - Can not save low to high
									//false,
									//End Track #5690 - JScott - Can not save low to high
									aSaveParms.SaveLocks);
							}
							else
							{
								chainCube.SaveCube(
									_openParms.ChainHLPlanProfile.NodeProfile.Key,
									_openParms.ChainHLPlanProfile.VersionProfile.Key,
									//Begin Enhancement - JScott - Add Balance Low Levels functionality
									false,
									//End Enhancement - JScott - Add Balance Low Levels functionality
									_openParms.GetWeekProfileList(SAB.ApplicationServerSession),
									true,
									//Begin Track #5690 - JScott - Can not save low to high
									//true,
									//End Track #5690 - JScott - Can not save low to high
									aSaveParms.SaveLocks);
							}
						}
					}
					else
					{
						throw new MIDException (eErrorLevel.severe,
							(int)eMIDTextCode.msg_pl_CubeNotDefined,
							MIDText.GetText(eMIDTextCode.msg_pl_CubeNotDefined));
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					planEnqGrp.DequeuePlans();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Closes this PlanCubeGroup.
		/// </summary>

		override public void CloseCubeGroup()
		{
			try
			{
				base.CloseCubeGroup();

				_planEnqGrp.DequeuePlans();
				//Begin Enhancement - JScott - Add Balance Low Levels functionality
				//_cubeTable.Clear();
				//End Enhancement - JScott - Add Balance Low Levels functionality
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void GetReadOnlyFlags(out bool aChainReadOnly)
		{
			try
			{
				aChainReadOnly = !_chainPlanEnqueue.PlanEnqueue.isEnqueued;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines if changes have occurred to either the chain cube.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the chain cube has changed.
		/// </returns>

		public bool hasChainCubeChanged()
		{
			ChainPlanWeekDetail cube;

			try
			{
				if (_chainPlanEnqueue.PlanEnqueue.isEnqueued)
				{
					cube = (ChainPlanWeekDetail)GetCube(eCubeType.ChainPlanWeekDetail);

					if (cube != null)
					{
						return cube.hasPlanChanged(_openParms.ChainHLPlanProfile);
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean value indicating if the variable is stored on the database.
		/// </summary>
		/// <param name="aVarProf">
		/// The VariableProfile containing the variable to inspect
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the variable is stored on the database.
		/// </returns>

		override public bool isDatabaseVariable(VariableProfile aVarProf, PlanCellReference aPlanCellRef)
		{
			return aVarProf.isDatabaseVariable(eVariableCategory.Chain, aPlanCellRef.GetVersionProfile().Key, eCalendarDateType.Week);
		}
		/// <summary>
		/// copies chain values in the basis to the chain plan.
		/// This copies from the first Basis Profile defined.
		/// </summary>

		public int CopyBasisToChain()
		{
			PlanCellReference planCellRef;
			PlanCellReference basisCellRef;
			ProfileList weekProfList;
			int i;
			int cellsCopied = 0;

			try
			{
				planCellRef = (PlanCellReference)GetCube(eCubeType.ChainPlanWeekDetail).CreateCellReference();
				basisCellRef = (PlanCellReference)GetCube(eCubeType.ChainBasisWeekDetail).CreateCellReference();

				planCellRef[eProfileType.Version] = _openParms.ChainHLPlanProfile.VersionProfile.Key;
				planCellRef[eProfileType.HierarchyNode] = _openParms.ChainHLPlanProfile.NodeProfile.Key;
				planCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

				basisCellRef[eProfileType.Version] = _openParms.ChainHLPlanProfile.VersionProfile.Key;
				basisCellRef[eProfileType.HierarchyNode] = _openParms.ChainHLPlanProfile.NodeProfile.Key;
				basisCellRef[eProfileType.Basis] = _openParms.BasisProfileList[0].Key;
				basisCellRef[eProfileType.QuantityVariable] = Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

				weekProfList = _openParms.GetWeekProfileList(this._SAB.ApplicationServerSession);

				for (i = 0; i < weekProfList.Count; i++)
				{
					planCellRef[eProfileType.Week] = weekProfList[i].Key;
					basisCellRef[eProfileType.Week] = weekProfList[i].Key;

					foreach (VariableProfile varProf in Transaction.PlanComputations.PlanVariables.VariableProfileList)
					{
						planCellRef[eProfileType.Variable] = varProf.Key;
						basisCellRef[eProfileType.Variable] = varProf.Key;

						if (planCellRef.PlanCube.isDatabaseVariable(varProf, planCellRef))
						{
							planCellRef.SetLoadCellValue(basisCellRef.CurrentCellValue, false);
							planCellRef.isCellChanged = true;
                            // Begin Track #5971, #5981 - JSmith - Cannot copy value in blended version
                            planCellRef.isCellLoadedFromDB = true;
                            // End Track #5971, #5981
                            // Begin Track #6004 - JSmith - Destination values not equal source values
                            // The performance change removing the pre-initialization of the database values now
                            // causes the initializations for the destination value to execute after the value
                            // is copied causing the value to potentially change.  But, the value will be 
                            // initialized using the destination criteria when the data is retrieved causing
                            // it to still potentially change.
                            planCellRef.isCellInitialized = true;
                            // End Track #6004
							cellsCopied++;
						}
					}
				}

				ClearUndoStack();
				return cellsCopied;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#5276 - JSmith - Read Only Saves
        override public bool IsNodeEnqueued(int aNodeKey, bool aIsChain)
        {
            return _chainPlanEnqueue.PlanEnqueue.isEnqueued;
        }
        // End TT#5276 - JSmith - Read Only Saves
	}
}
