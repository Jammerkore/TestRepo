using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	public class AssortmentPlanCubeGroup : StorePlanMaintCubeGroup
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of StorePlanMainCubeGroup, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this StorePlanMainCubeGroup is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this StorePlanMainCubeGroup is a part of.
		/// </param>

		public AssortmentPlanCubeGroup(
			SessionAddressBlock aSAB,
			Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
			try
			{
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

		public void OpenGradeCubes(ProfileList aGradeList, SetGradeStoreXRef aSetGradeStoreXRef)
		{
			CubeDefinition cubeDef;

			ProfileList versionProfileList;
			PlanCube planCube;

			try
			{
				if (GetCube(eCubeType.StoreBasisGradeTotalDateTotal) == null)
				{
					//========================================
					// Set Master Profile Lists and XRef Lists
					//========================================

					SetProfileXRef(aSetGradeStoreXRef);

					//==================
					// Initialize fields
					//==================

					versionProfileList = GetMasterProfileList(eProfileType.Version);

					//==================================================
					// Create StoreBasisGradeTotalDateTotal in CubeGroup
					//==================================================

					cubeDef = new CubeDefinition(
							new DimensionDefinition(eProfileType.Version, versionProfileList.Count),
							new DimensionDefinition(eProfileType.HierarchyNode, 1),
							new DimensionDefinition(eProfileType.Basis, 1),
							new DimensionDefinition(eProfileType.TimeTotalVariable, GetMasterProfileList(eProfileType.TimeTotalVariable).Count),
							new DimensionDefinition(eProfileType.QuantityVariable, Transaction.PlanComputations.PlanQuantityVariables.GetQuantityVariableCountByCube((ushort)(QuantityLevelFlagValues.StoreSetCube | QuantityLevelFlagValues.HighLevel))),
							new DimensionDefinition(eProfileType.Variable, GetMasterProfileList(eProfileType.Variable).Count),
							new DimensionDefinition(eProfileType.StoreGroupLevel, GetFilteredProfileList(eProfileType.StoreGroupLevel).Count),
							new DimensionDefinition(eProfileType.StoreGrade, aGradeList.Count));

					planCube = (PlanCube)GetCube(eCubeType.StoreBasisGradeTotalDateTotal);

					if (planCube == null)
					{
						planCube = new StoreBasisStoreTotalDateTotal(SAB, Transaction, this, cubeDef, 9);
						Transaction.PlanComputations.PlanCubeInitialization.StoreBasisGradeTotalDateTotal(planCube, _openParms.GetDisplayType(SAB.ApplicationServerSession));
						SetCube(eCubeType.StoreBasisGradeTotalDateTotal, planCube);
					}
					else
					{
						planCube.ExpandDimensionSize(cubeDef);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
