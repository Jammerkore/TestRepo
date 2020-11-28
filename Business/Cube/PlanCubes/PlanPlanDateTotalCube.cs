using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class defines the PlanPlanDateTotalCube.
	/// </summary>
	/// <remarks>
	/// The PlanPlanDateTotalCube defines a cube that contains the date totals of the plan values.
	/// </remarks>

	abstract public class PlanPlanDateTotalCube : PlanPlanCube
	{
		//=======
		// FIELDS
		//=======

		private Hashtable _versionProtectedHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanPlanDateTotalCube, using the given SessionAddressBlock, Transaction, and PlanCubeGroup.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this PlanPlanDateTotalCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this PlanPlanDateTotalCube is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this PlanPlanDateTotalCube is a part of.
		/// </param>

		public PlanPlanDateTotalCube(
			SessionAddressBlock aSAB,
			ApplicationSessionTransaction aTransaction,
			PlanCubeGroup aPlanCubeGroup,
			CubeDefinition aCubeDef,
			ushort aCubeAttributes,
			int aCubePriority,
			bool aReadOnly,
			bool aCheckNodeSecurity)

			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDef, (ushort)(aCubeAttributes | PlanCubeAttributesFlagValues.DateTotal), aCubePriority, aReadOnly, aCheckNodeSecurity)
		{
			_versionProtectedHash = new Hashtable();
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		//Begin Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
		/// <summary>
		/// Returns a boolean indicating if the cell referenced can be scheduled for calculation.
		/// </summary>
		/// <param name="aCompCellRef">
		/// The ComputationCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is displayable.
		/// </returns>

		override public bool canCellBeScheduled(ComputationCellReference aCompCellRef)
		{
			return true;
		}

		//End Track #5966 - JScott - Changing Sales totals (time, quarters) gets errors
		/// <summary>
		/// Returns an eProfileType of the time dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the time dimension.
		/// </returns>

		override public eProfileType GetTimeType()
		{
			throw new MIDException (eErrorLevel.severe,
				(int)eMIDTextCode.msg_pl_InvalidCall,
				MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
		}

		//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line
		///// <summary>
		///// Returns an eProfileType of the version dimension for the given PlanCellReference.
		///// </summary>
		///// <returns>
		///// An eProfileType of the version dimension.
		///// </returns>

		//override public eProfileType GetVersionType()
		//{
		//    return eProfileType.Version;
		//}

		//End Track #4581 - JScott - Custom Variables not calculating on Basis line

		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		/// <summary>
		/// Abstract method that returns a boolean indicating if the given PlanCellReference PlanCell should be marked as read-only.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the PlanCell to get the read-only status for.
		/// </param>
		/// <returns>
		/// A boolean indicating if the given PlanCellReference PlanCell should be marked as read-only.
		/// </returns>

		override public bool isPlanCellReadOnly(PlanCellReference aPlanCellRef)
		{
			return false;
		}

		//End Track #5121 - JScott - Add Year/Season/Quarter totals
		/// <summary>
		/// Returns an incremented Time key.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <param name="aIncrement">
		/// The amount to increment the time key by.
		/// </param>
		/// <returns>
		/// The incremented time key.
		/// </returns>

		override public int IncrementTimeKey(PlanCellReference aPlanCellRef, int aIncrement)
		{
			throw new MIDException (eErrorLevel.severe,
				(int)eMIDTextCode.msg_pl_InvalidCall,
				MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
		}

		/// <summary>
		/// Returns an incremented Time key.
		/// </summary>
		/// <param name="aTimeKey">
		/// The key of the time index.
		/// </param>
		/// <param name="aIncrement">
		/// The amount to increment the time key by.
		/// </param>
		/// <returns>
		/// The incremented time key.
		/// </returns>

		override public int IncrementTimeKey(int aTimeKey, int aIncrement)
		{
			throw new MIDException (eErrorLevel.severe,
				(int)eMIDTextCode.msg_pl_InvalidCall,
				MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
		}

		/// <summary>
		/// Returns the VariableProfile object for the Cell specified by the PlanCellReference.
		/// </summary>
		/// <remarks>
		/// A PlanCellReference object that identifies the PlanCubeCell to retrieve.
		/// </remarks>

		override public ComputationVariableProfile GetVariableProfile(ComputationCellReference aPlanCellRef)
		{
			try
			{
				return (ComputationVariableProfile)MasterTimeTotalVariableProfileList.FindKey(aPlanCellRef[eProfileType.TimeTotalVariable]);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean value indicating if the version of the Cell pointed to by  the given PlanCellReference is protected.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the version for the given PlanCellReference is protected.
		/// </returns>

		override public bool isVersionProtected(PlanCellReference aPlanCellRef)
		{
			HashKeyObject hashKey;
			object hashValue;
			bool isProtected;

			try
			{
				hashKey = new HashKeyObject(aPlanCellRef[eProfileType.Version], aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.TimeTotalVariable]);
				hashValue = _versionProtectedHash[hashKey];

				if (hashValue == null)
				{
					isProtected = aPlanCellRef.GetComponentDetailVersionProtected();
					_versionProtectedHash[hashKey] = isProtected;
				}
				else
				{
					isProtected = (bool)hashValue;
				}
				return isProtected;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//Begin Track #5669 - JScott - BMU %

		/// <summary>
		/// Returns a boolean value indicating if the version of the Cell pointed to by  the given PlanCellReference is display-only.
		/// </summary>
		/// <param name="aPlanCellRef"></param>
		/// <returns></returns>

		override public bool isDisplayOnly(PlanCellReference aPlanCellRef)
		{
			//Being Track #5702 - JScott - Total Variables that should be Display only but allow editing
			ComputationVariableProfile baseVarProf;

			//End Track #5702 - JScott - Total Variables that should be Display only but allow editing
			try
			{
				//Being Track #5702 - JScott - Total Variables that should be Display only but allow editing
				//return aPlanCellRef.GetComponentDetailDisplayOnly(); 
				baseVarProf = aPlanCellRef.GetVariableAccessVariableProfile();

				if (baseVarProf != null)
				{
					//Begin Track #5009 - JScott - Change Edits for XFER
					//return baseVarProf.VariableAccess == eVariableAccess.DisplayOnly || aPlanCellRef.GetComponentDetailDisplayOnly();
					if (baseVarProf.VariableAccess == eVariableAccess.FollowDetail)
					{
						return aPlanCellRef.GetComponentDetailDisplayOnly();
					}
					else
					{
						return baseVarProf.VariableAccess == eVariableAccess.DisplayOnly;
					}
					//End Track #5009 - JScott - Change Edits for XFER
				}
				else
				{
					return true;
				}
				//End Track #5702 - JScott - Total Variables that should be Display only but allow editing
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #5669 - JScott - BMU %
		//Begin Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line

		/// <summary>
		/// Returns the Time key of the data for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to get the Time key of.
		/// </param>
		/// <returns>
		/// The Time key of the data for the given PlanCellReference.
		/// </returns>

		override public int GetWeekKeyOfData(PlanCellReference aPlanCellRef)
		{
			return -1;
		}
		//End Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line
		//Begin Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly

		override public void ClearCubeForHierarchyVersion(PlanCellReference aCellRef)
		{
			try
			{
				Clear(aCellRef, 2);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly
	}
}
