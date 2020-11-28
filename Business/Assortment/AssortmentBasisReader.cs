using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	public class AssortmentBasisReader
	{
		private AssortmentPlanCubeGroup _cubeGroup;
		private PlanOpenParms _openParms;
		private SessionAddressBlock SAB;
		private string _computationsMode;
		private int _anchorNodeRid;		// TT#1188 - stodd - reader not using anchor date
		private int _anchorCdrRid;		// TT#1188 - stodd - reader not using anchor date
		private DateRangeProfile _anchorDateRangeProfile;
		private List<AssortmentBasis> _basisList;
		private int _sgRid;
		private ApplicationSessionTransaction _trans;
		private ProfileList _storeList;
		private bool _inclSimStore;
		private bool _inclIntrasit;
		private bool _inclOnHand;
		private bool _inclCommitted;


		public AssortmentBasisReader(SessionAddressBlock sab, ApplicationSessionTransaction trans,
			int anchorNodeRid,
			int anchorCdrRid,	// TT#1188 - stodd - reader not using anchor date
			List<AssortmentBasis> basisList,
			int sgRid,
			bool inclSimStore,
			bool inclIntransit,
			bool inclOnHand,
			bool inclCommitted,
			ProfileList storeList)
		{
			_anchorNodeRid = anchorNodeRid;
			_anchorCdrRid = anchorCdrRid;	// TT#1188 - stodd - reader not using anchor date
			_basisList = basisList;
			_trans = trans;
			SAB = sab;
			_sgRid = sgRid;
			_inclSimStore = inclSimStore;
			_inclIntrasit = inclIntransit;
			_inclOnHand = inclOnHand;
			_inclCommitted = inclCommitted;
			_storeList = storeList;

			_cubeGroup = new AssortmentPlanCubeGroup(sab, trans);
			_computationsMode = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;

			// Build plan data from first basis information
			FillOpenParmForPlan();
			_openParms.BasisProfileList.Clear();
			FillOpenParmForBasis();
			((PlanCubeGroup)_cubeGroup).OpenCubeGroup(_openParms);

		}

		public AssortmentPlanCubeGroup AssortmentPlanCubeGroup
		{
			get
			{
				return _cubeGroup;
			}
		}

		public int GetVariableNumber(eAssortmentVariableType variableType)
		{
			try
			{
				int varNumber = Include.Undefined;
				switch (variableType)
				{
					case eAssortmentVariableType.Sales:
						if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.Key;
						}
						else
						{
							varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key;
						}
						break;
					case eAssortmentVariableType.Stock:
						if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key;
						}
						else
						{
							varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key;
						}
						break;
					case eAssortmentVariableType.Receipts:
						if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.ReceiptRegularUnitsVariable.Key;
						}
						else
						{
							varNumber = _cubeGroup.Transaction.PlanComputations.PlanVariables.ReceiptTotalUnitsVariable.Key;
						}
						break;
					default:
						string msg = MIDText.GetText(eMIDTextCode.msg_as_InvalidVariable);
						msg = msg.Replace("{0}", variableType.ToString());
						throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_as_InvalidVariable, msg);
				}

				return varNumber;
			}
			catch
			{
				throw;
			}
		}


		public double GetBasisTotalUnits(eAssortmentVariableType variableType)
		{
			try
			{
				Cube myCube = _cubeGroup.GetCube(eCubeType.StoreBasisStoreTotalDateTotal);
				PlanCellReference planCellRef = new PlanCellReference((StoreBasisStoreTotalDateTotal)myCube);
				SetCommonPlanCellRefIndexes(variableType, planCellRef);
				planCellRef[eProfileType.Basis] = 1;
				double totalValue = planCellRef.HiddenCurrentCellValue;
				return totalValue;
			}
			catch
			{
				throw;
			}
		}

		// BEGIN TT#831-MD - Stodd - Need / Intransit not displayed
		public double GetPlanTotalUnits(eAssortmentVariableType variableType, int version)
		// END TT#831-MD - Stodd - Need / Intransit not displayed
		{
			try
			{
				Cube myCube = _cubeGroup.GetCube(eCubeType.StorePlanStoreTotalDateTotal);
				PlanCellReference planCellRef = new PlanCellReference((StorePlanStoreTotalDateTotal)myCube);
				SetCommonPlanCellRefIndexes(variableType, planCellRef);
				//===========================================================
				// NOTE: The version is ALWAYS being overriden to be Actuals
				//===========================================================
				// BEGIN TT#831-MD - Stodd - Need / Intransit not displayed
				//planCellRef[eProfileType.Version] = Include.FV_ActualRID;
				planCellRef[eProfileType.Version] = version;
				// END TT#831-MD - Stodd - Need / Intransit not displayed
				double totalValue = planCellRef.HiddenCurrentCellValue;
				return totalValue;
			}
			catch
			{
				throw;
			}
		}

		public List<int> GetBasisStoreUnits(eAssortmentVariableType variableType)
		{
			try
			{
				List<int> valueList = new List<int>();
				Cube myCube = _cubeGroup.GetCube(eCubeType.StoreBasisDateTotal);
				PlanCellReference planCellRef = new PlanCellReference((StoreBasisDateTotal)myCube);
				SetCommonPlanCellRefIndexes(variableType, planCellRef);
				planCellRef[eProfileType.Basis] = 1;
				ArrayList storeBasisValues = planCellRef.GetCellRefArray(_storeList);
				for (int i = 0; i < storeBasisValues.Count; i++)
				{
					PlanCellReference pcr = (PlanCellReference)storeBasisValues[i];
					valueList.Add((int)pcr.HiddenCurrentCellValue);
				}
				return valueList;
			}
			catch
			{
				throw;
			}
		}

		// Begin TT#2 - stodd - assortment
		public Hashtable GetStoreEligibility(eAssortmentVariableType variableType)
		{
			try
			{
				Hashtable storeEligibilityHash = new Hashtable();
				eAssortmentVariableType eligibilityVarType = eAssortmentVariableType.Stock;
				if (variableType == eAssortmentVariableType.Sales)
					eligibilityVarType = variableType;
				Cube myCube = _cubeGroup.GetCube(eCubeType.StoreBasisDateTotal);
				PlanCellReference planCellRef = new PlanCellReference((StoreBasisDateTotal)myCube);
				SetCommonPlanCellRefIndexes(eligibilityVarType, planCellRef);
				planCellRef[eProfileType.Basis] = 1;
				ArrayList storeBasisValues = planCellRef.GetCellRefArray(_storeList);
				for (int i = 0; i < storeBasisValues.Count; i++)
				{
					PlanCellReference pcr = (PlanCellReference)storeBasisValues[i];
					StoreProfile sp = (StoreProfile)_storeList[i];
					storeEligibilityHash.Add(sp.Key, !pcr.isCellIneligible);
				}
				return storeEligibilityHash;
			}
			catch
			{
				throw;
			}
		}
		// End TT#2 - stodd - assortment

		// BEGIN TT#831-MD - Stodd - Need / Intransit not displayed
		public List<int> GetPlanStoreUnits(eAssortmentVariableType variableType, int version)
		{
			try
			{
				List<int> valueList = new List<int>();
				ArrayList storePlanValues =	GetPlanStoreValueArrayList(variableType, version);
				for (int i = 0; i < storePlanValues.Count; i++)
				{
					PlanCellReference pcr = (PlanCellReference)storePlanValues[i];
					valueList.Add((int)pcr.HiddenCurrentCellValue);
				}
				return valueList;
			}
			catch
			{
				throw;
			}
		}

		public List<decimal> GetPlanStorePctNeed(eAssortmentVariableType variableType, int version)
		{
			try
			{
				List<decimal> valueList = new List<decimal>();
				ArrayList storePlanValues = GetPlanStoreValueArrayList(variableType, version);
				for (int i = 0; i < storePlanValues.Count; i++)
				{
					PlanCellReference pcr = (PlanCellReference)storePlanValues[i];
					valueList.Add((decimal)pcr.HiddenCurrentCellValue);
				}
				return valueList;
			}
			catch
			{
				throw;
			}
		}

		private ArrayList GetPlanStoreValueArrayList(eAssortmentVariableType variableType, int version)
		{
			Cube myCube = _cubeGroup.GetCube(eCubeType.StorePlanDateTotal);
			PlanCellReference planCellRef = new PlanCellReference((StorePlanDateTotal)myCube);
			SetCommonPlanCellRefIndexes(variableType, planCellRef);
			planCellRef[eProfileType.Version] = version;
			return planCellRef.GetCellRefArray(_storeList);
		}
		// END TT#831-MD - Stodd - Need / Intransit not displayed


		private void SetCommonPlanCellRefIndexes(eAssortmentVariableType variableType, PlanCellReference planCellRef)
		{
			try
			{
				planCellRef[eProfileType.Version] = _basisList[0].VersionProfile.Key;

				// BEGIN TT#1865 - stodd - summary values not reading when plan node differs from basis node
				//planCellRef[eProfileType.HierarchyNode] = _basisList[0].HierarchyNodeProfile.Key;
				planCellRef[eProfileType.HierarchyNode] = _anchorNodeRid;
				// END TT#1401 
				planCellRef[eProfileType.QuantityVariable] = _cubeGroup.Transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

				switch (variableType)
				{
					case eAssortmentVariableType.Sales:
						if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.Key;
							planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalSalesRegPromoUnitsVariable.Key;
						}
						else
						{
							planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.Key;
							planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalSalesTotalUnitsVariable.Key;
						}
						break;
					case eAssortmentVariableType.Stock:
						if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key;
							planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.BeginningInventoryRegularUnitsVariable.Key;
						}
						else
						{
							planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key;
							planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.BeginningInventoryTotalUnitsVariable.Key;
						}
						break;
					case eAssortmentVariableType.Receipts:
						if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.ReceiptRegularUnitsVariable.Key;
							planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalReceiptRegularUnitsVariable.Key;
						}
						else
						{
							planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.ReceiptTotalUnitsVariable.Key;
							planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalReceiptTotalUnitsVariable.Key;
						}
						break;
					case eAssortmentVariableType.Intransit:
						planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.IntransitVariable.Key;
						planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalIntransitVariable.Key;
						break;
					case eAssortmentVariableType.Onhand:
						if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key;
							planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalOnHandRegularUnitsVariable.Key;
						}
						else
						{
							planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key;
							planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalOnHandTotalUnitsVariable.Key;
						}
						break;
					case eAssortmentVariableType.Need:
						if (_cubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Regular)
						{
							planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.Key;
							planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalNeedRegularVariable.Key;
						}
						else
						{
							planCellRef[eProfileType.Variable] = _cubeGroup.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.Key;
							planCellRef[eProfileType.TimeTotalVariable] = _cubeGroup.Transaction.PlanComputations.PlanTimeTotalVariables.TotalNeedTotalVariable.Key;
						}
						break;
					default:
						string msg = MIDText.GetText(eMIDTextCode.msg_as_InvalidVariable);
						msg = msg.Replace("{0}", variableType.ToString());
						throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_as_InvalidVariable, msg);
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Fills in the plan part of the CubeGroup open parms
		/// </summary>
		private void FillOpenParmForPlan()
		{
			_openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, _computationsMode);
			_openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.AssortmentMethodsUserGeneralAssortment);
			_openParms.FunctionSecurityProfile.SetAllowUpdate();

			HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_anchorNodeRid);
			hnp.ChainSecurityProfile.SetReadOnly();
			hnp.StoreSecurityProfile.SetReadOnly();

			//ProfileList vList = SAB.ClientServerSession.GetUserForecastVersions();
			//VersionProfile uvp = (VersionProfile)vList.FindKey(2);

			VersionProfile vp = _basisList[0].VersionProfile;
			//vp.StoreSecurity = new VersionSecurityProfile(vp.Key);
			//vp.StoreSecurity.SetReadOnly();

			//vp.ChainSecurity = new VersionSecurityProfile(vp.Key);
			//vp.ChainSecurity.SetReadOnly();


			_openParms.StoreHLPlanProfile.VersionProfile = vp;
			_openParms.StoreHLPlanProfile.NodeProfile = hnp;
			_openParms.ChainHLPlanProfile.VersionProfile = vp;
			_openParms.ChainHLPlanProfile.NodeProfile = hnp;

			int maxWeeks = 1;
			foreach (AssortmentBasis ab in _basisList)
			{
				DateRangeProfile drp = ab.HorizonDate;
				ProfileList weekList = SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(drp, null);
				maxWeeks = Math.Max(maxWeeks, weekList.Count);
			}
			// BEGIN TT#1188 - stodd - reader not using anchor date
			WeekProfile anchorWeek = SAB.ApplicationServerSession.Calendar.GetFirstWeekOfRange(_anchorCdrRid);
			WeekProfile	endWeek = this.SAB.ApplicationServerSession.Calendar.Add(anchorWeek, maxWeeks-1);
			_openParms.DateRangeProfile = this.SAB.ApplicationServerSession.Calendar.AddDateRangeFromWeeks(anchorWeek.YearWeek, endWeek.YearWeek);
			//_openParms.DateRangeProfile = SAB.ApplicationServerSession.Calendar.AddDateRangeWithCurrent(maxWeeks);
			// END TT#1188 - stodd - reader not using anchor date
			_openParms.StoreGroupRID = this._sgRid;
			_openParms.FilterRID = Include.UndefinedStoreFilter;
			_openParms.IneligibleStores = true;
			_openParms.SimilarStores = _inclSimStore;

			if (_computationsMode != null)
			{
				_openParms.ComputationsMode = _computationsMode;
			}
			else
			{
				_openParms.ComputationsMode = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
			}
		}

		/// <summary>
		/// Fills in the basis part of the CubeGroup open parms.
		/// </summary>
		private void FillOpenParmForBasis()
		{
			BasisProfile basisProfile;
			BasisDetailProfile basisDetailProfile;
			int bdpKey = 1;

			//=======================
			// Set up Basis Profile
			//=======================
			basisProfile = new BasisProfile(1, null, _openParms);
			basisProfile.BasisType = eTyLyType.NonTyLy;

			int maxBasis = _basisList.Count;
			for (int basisRow = 0; basisRow < maxBasis; basisRow++)
			{
				basisDetailProfile = new BasisDetailProfile(bdpKey++, _openParms);
				basisDetailProfile.VersionProfile = new VersionProfile(_basisList[basisRow].VersionProfile.Key);
				basisDetailProfile.HierarchyNodeProfile = _basisList[basisRow].HierarchyNodeProfile;
				basisDetailProfile.DateRangeProfile = _basisList[basisRow].HorizonDate;
				basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
				// The "weights" are really "percents", so we change them to weights.
				double wgt = _basisList[basisRow].Weight / 100;
				basisDetailProfile.Weight = (float)wgt;
				basisProfile.BasisDetailProfileList.Add(basisDetailProfile);
			}
			_openParms.BasisProfileList.Add(basisProfile);
		}
	}
}
