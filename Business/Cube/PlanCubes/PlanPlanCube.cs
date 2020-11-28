using System;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the PlanPlanCube.
	/// </summary>
	/// <remarks>
	/// The PlanPlanCube defines a cube that contains plan values.
	/// </remarks>

	abstract public class PlanPlanCube : PlanCube
	{
		//=======
		// FIELDS
		//=======

		private ProfileList _timeDetailProfList;
        // Begin Track #5841 - JSmith - Performance
        private VersionProfile _versionProfile = null;
        // End Track #5841

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanPlanCube, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this PlanPlanCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this PlanPlanCube is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this PlanPlanCube is a part of.
		/// </param>

		public PlanPlanCube(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			ushort aCubeAttributes,
			int aCubePriority,
			bool aReadOnly,
			bool aCheckNodeSecurity)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, (ushort)(aCubeAttributes | PlanCubeAttributesFlagValues.Plan), aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
			_timeDetailProfList = null;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line
		/// <summary>
		/// Returns an eProfileType of the version dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the version dimension.
		/// </returns>

		override public eProfileType GetVersionType()
		{
			return eProfileType.Version;
		}

		//End Track #4581 - JScott - Custom Variables not calculating on Basis line
		/// <summary>
		/// Returns an eProfileType of the hierarchy node dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the Hierarchy Node dimension.
		/// </returns>

		override public eProfileType GetHierarchyNodeType()
		{
			return eProfileType.HierarchyNode;
		}

		/// <summary>
		/// Method that returns the VersionProfile for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The VersionProfile for the given Cell Reference.
		/// </returns>

		override public VersionProfile GetVersionProfile(PlanCellReference aPlanCellRef)
		{
			try
			{
                // Begin Track #5841 - JSmith - Performance
                //return (VersionProfile)aPlanCellRef.PlanCube.MasterVersionProfileList.FindKey(aPlanCellRef[eProfileType.Version]);
                if (_versionProfile == null ||
                    _versionProfile.Key != aPlanCellRef[eProfileType.Version])
                {
                    _versionProfile = (VersionProfile)aPlanCellRef.PlanCube.MasterVersionProfileList.FindKey(aPlanCellRef[eProfileType.Version]);
                }
                return _versionProfile;
                // End Track #5841
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the HierarchyNodeProfile for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The HierarchyNodeProfile for the given Cell Reference.
		/// </returns>

		override public HierarchyNodeProfile GetHierarchyNodeProfile(PlanCellReference aPlanCellRef)
		{
			int nodeId;
			ProfileList nodeProfList;
			HierarchyNodeProfile nodeProf;

			try
			{
				nodeId = aPlanCellRef[eProfileType.HierarchyNode];
				nodeProfList = aPlanCellRef.PlanCube.MasterNodeProfileList;
				nodeProf = (HierarchyNodeProfile)nodeProfList.FindKey(nodeId);

				if (nodeProf == null)
				{
					nodeProf = _SAB.HierarchyServerSession.GetNodeData(nodeId);

					switch (PlanCubeGroup.OpenParms.PlanSessionType)
					{
						case ePlanSessionType.ChainMultiLevel:
						case ePlanSessionType.ChainSingleLevel:

							nodeProf.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(nodeId, (int)eSecurityTypes.Chain);
							break;

						case ePlanSessionType.StoreMultiLevel:
						case ePlanSessionType.StoreSingleLevel:

							nodeProf.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(nodeId, (int)eSecurityTypes.Chain);
							nodeProf.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(nodeId, (int)eSecurityTypes.Store);
							break;
					}

					nodeProfList.Add(nodeProf);
				}

				return nodeProf;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the cell referenced by the given PlanCellReference is displayable.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The PlanCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is displayable.
		/// </returns>

		override public bool isCellDisplayable(ComputationCellReference aCompCellRef)
		{
			return true;
		}

		/// <summary>
		/// Override.  Method that returns a ProfileList of the weeks for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the PlanCell to find the week ProfileList for.
		/// </param>
		/// <returns>
		/// A ProfileList of time.
		/// </returns>

		override public ProfileList GetTimeDetailProfileList(PlanCellReference aPlanCellRef)
		{
			try
			{
				if (_timeDetailProfList == null)
				{
					_timeDetailProfList = GetTimeDetailProfileList(aPlanCellRef, GetTimeType());
				}

				return _timeDetailProfList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
//Begin Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity

		/// <summary>
		/// Method that returns the AverageDivisor for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The AverageDivisor for the given PlanCellReference.
		/// </returns>

		override public double GetAverageDivisor(PlanCellReference aPlanCellRef)
		{
			try
			{
				return PlanCubeGroup.OpenParms.GetAverageDivisor(aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns a boolean indicating if the given PlanCellReference contains the current week.
		/// </summary>
		/// <returns>
		/// A boolean indicating if the given PlanCellReference contains the current week.
		/// </returns>

		override public bool ContainsCurrentWeek(PlanCellReference aPlanCellRef)
		{
			try
			{
				return PlanCubeGroup.OpenParms.GetContainsCurrentWeek(aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
//End Track #3681 - JScott - Fix WTD Sales Time Average values for Velocity

		/// <summary>
		/// Method that returns a ProfileList of the weeks for the given PlanCellReference and eProfileType.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the PlanCell to find the week ProfileList for.
		/// </param>
        /// <param name="aDateProfileType">
		/// The eProfileType of the date type to look up.
		/// </param>
		/// <returns>
		/// A ProfileList of time.
		/// </returns>

		override public ProfileList GetTimeDetailProfileList(PlanCellReference aPlanCellRef, eProfileType aDateProfileType)
		{
			try
			{
				if (aDateProfileType == eProfileType.Week)
				{
					return PlanCubeGroup.OpenParms.GetWeekProfileList(aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession);
				}
				else
				{
					return PlanCubeGroup.OpenParms.GetPeriodProfileList(aPlanCellRef.PlanCube.PlanCubeGroup.SAB.ApplicationServerSession);
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
