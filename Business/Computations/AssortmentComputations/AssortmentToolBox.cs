using System;
using System.Collections;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// The Tools class contains routines that are used by the formulas, spreads, initializations, and change rules.
	/// </summary>

	public class AssortmentToolBox : BaseToolBox
	{

		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public AssortmentToolBox(BaseComputations aComputations)
			: base(aComputations)
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference to the AssortmentCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The AssortmentCellReference of the AssortmentCubeCell.
		/// </returns>

		public AssortmentCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			AssortmentCellReference aAssrtCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aAssrtCellRef, aCubeType, aQuantityVariableProfile, true);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference to the AssortmentCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The AssortmentCellReference of the AssortmentCubeCell.
		/// </returns>

		public AssortmentCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			AssortmentCellReference aAssrtCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending)
		{
			AssortmentCellReference assrtCellRef;
			AssortmentCellReference totCellRef;

			try
			{
				assrtCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
				assrtCellRef[eProfileType.AssortmentQuantityVariable] = aQuantityVariableProfile.Key;
				totCellRef = intGetAssortmentCellReferenceFromTotalCube(assrtCellRef, aCubeType);
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, totCellRef);
				}
				return totCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference to the AssortmentCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// The AssortmentCellReference of the AssortmentCubeCell.
		/// </returns>

		public AssortmentCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			AssortmentCellReference aAssrtCellRef,
			eCubeType aCubeType,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aAssrtCellRef, aCubeType, aVariableProfile, aQuantityVariableProfile, true);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference to the AssortmentCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aVariableProfile">
		/// The VariableProfile of the variable to be substituted before retrieving.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// The AssortmentCellReference of the AssortmentCubeCell.
		/// </returns>

		public AssortmentCellReference GetTotalOperandCell(
			ComputationScheduleEntry aScheduleEntry,
			eSetCellMode aSetCellMode,
			AssortmentCellReference aAssrtCellRef,
			eCubeType aCubeType,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending)
		{
			AssortmentCellReference assrtCellRef;
			AssortmentCellReference totCellRef;

			try
			{
				assrtCellRef = (AssortmentCellReference)aAssrtCellRef.Copy();
				assrtCellRef[aAssrtCellRef.AssortmentCube.VariableProfileType] = aVariableProfile.Key;
				assrtCellRef[eProfileType.AssortmentQuantityVariable] = aQuantityVariableProfile.Key;
				totCellRef = intGetAssortmentCellReferenceFromTotalCube(assrtCellRef, aCubeType);
				if (aCheckPending)
				{
					intCheckOperandCell(aScheduleEntry, aSetCellMode, totCellRef);
				}
				return totCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference to the AssortmentCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the AssortmentCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			AssortmentCellReference aAssrtCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aAssrtCellRef, aCubeType, aQuantityVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference to the AssortmentCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the AssortmentCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			AssortmentCellReference aAssrtCellRef,
			eCubeType aCubeType,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aAssrtCellRef, aCubeType, aQuantityVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference to the AssortmentCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <returns>
		/// A double containing the AssortmentCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			AssortmentCellReference aAssrtCellRef,
			eCubeType aCubeType,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCellValue(aScheduleEntry, aGetCellMode, aSetCellMode, aAssrtCellRef, aCubeType, aVariableProfile, aQuantityVariableProfile, true, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets an operand Cell Quantity value from the given cube, substituting the QuantityVariableProfile with the given quantity variable Id
		/// before retrieving.
		/// </summary>
		/// <param name="aGetCellMode">
		/// An eGetCellMode value that indicates which Cell value to return. 
		/// </param>
		/// <param name="aSetCellMode">
		/// An eSetCellMode value that indicates if this set is being made by an init or a computation.
		/// </param>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference to the AssortmentCubeCell being requested.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to retrieve the value from.
		/// </param>
		/// <param name="aQuantityVariableProfile">
		/// The VariableProfile of the quantity variable to be substituted before retrieving.
		/// </param>
		/// <param name="aCheckPending">
		/// A boolean indicating if an error should be thrown in the cell is pending.
		/// </param>
		/// <returns>
		/// A double containing the AssortmentCubeCell's value
		/// </returns>

		public double GetTotalOperandCellValue(
			ComputationScheduleEntry aScheduleEntry,
			eGetCellMode aGetCellMode,
			eSetCellMode aSetCellMode,
			AssortmentCellReference aAssrtCellRef,
			eCubeType aCubeType,
			ComputationVariableProfile aVariableProfile,
			QuantityVariableProfile aQuantityVariableProfile,
			bool aCheckPending,
			bool aUseHiddenValues)
		{
			try
			{
				return GetTotalOperandCell(aScheduleEntry, aSetCellMode, aAssrtCellRef, aCubeType, aVariableProfile, aQuantityVariableProfile, aCheckPending).GetCellValue(aGetCellMode, aUseHiddenValues);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Inserts a Forumla into the schedule queue for the given ComputationCellReference, FormulaSpreadProfile, and
		/// VariableProfile.
		/// </summary>
		/// <param name="aCompSchd">
		/// The ComputationSchedule to insert the Formula in.
		/// </param>
        /// <param name="aAssrtCellRef">
        /// The AssortmentCellReference that identifies the cell being computed.
		/// </param>
		/// <param name="aFormula">
		/// The FormulaSpreadProfile of the forumla to execute.
		/// </param>
		
		public void InsertFormula(
			ComputationSchedule aCompSchd,
			AssortmentCellReference aAssrtCellRef,
			FormulaProfile aFormula,
			eCubeType aCubeType)
		{
			AssortmentCellReference assrtCellRef;

			try
			{
				assrtCellRef = intGetAssortmentCellReferenceFromTotalCube(aAssrtCellRef, aCubeType);
				aCompSchd.InsertFormula(aAssrtCellRef, assrtCellRef, aFormula);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Private method that returns a AssortmentCellReference to the total cell requested for the given AssortmentCellReference and eCubeType.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference for the cell to look up the total for.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType of the cube to find the total for.
		/// </param>
		/// <returns>
		/// The AssortmentCellReference to the total cell in the requested cube.
		/// </returns>

		private AssortmentCellReference intGetAssortmentCellReferenceFromTotalCube(AssortmentCellReference aAssrtCellRef, eCubeType aCubeType)
		{
			eCubeType cubeType;
			ArrayList arrList;
			AssortmentCellReference assrtCellRef;

			try
			{
				cubeType = intGetCubeType(aAssrtCellRef, aCubeType);

				arrList = aAssrtCellRef.GetTotalCellRefArray(cubeType);

				if (arrList.Count == 1)
				{
					assrtCellRef = (AssortmentCellReference)arrList[0];
				}
				else
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_InvalidTotalRelationship,
						MIDText.GetText(eMIDTextCode.msg_pl_InvalidTotalRelationship));
				}

				return assrtCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an eCubeType that indicates the cube type for the given eCubeType for the given AssortmentCellReference.
		/// </summary>
		/// <param name="aAssrtCellRef">
		/// The AssortmentCellReference of the cell to find the cube type for.
		/// </param>
		/// <param name="aCubeType">
		/// The eCubeType to translate.
		/// </param>
		/// <returns>
		/// The eCubeType of the translated cube.
		/// </returns>

		private eCubeType intGetCubeType(AssortmentCellReference aAssrtCellRef, eCubeType aCubeType)
		{
			eCubeType cubeType;

			try
			{
				switch (aCubeType.Id)
				{
					case eCubeType.cAssortmentComponentGroupLevel:
						cubeType = aAssrtCellRef.AssortmentCube.GetComponentGroupLevelCubeType();
						break;
					case eCubeType.cAssortmentComponentTotal:
						cubeType = aAssrtCellRef.AssortmentCube.GetComponentTotalCubeType();
						break;
					case eCubeType.cAssortmentSubTotal:
						cubeType = aAssrtCellRef.AssortmentCube.GetSubTotalCubeType();
						break;
					case eCubeType.cAssortmentTotal:
						cubeType = aAssrtCellRef.AssortmentCube.GetTotalCubeType();
						break;
					default:
						cubeType = aCubeType;
						break;
				}

				if (cubeType != eCubeType.None)
				{
					return cubeType;
				}
				else
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_CubeTypeNotDetermined,
						MIDText.GetText(eMIDTextCode.msg_pl_CubeTypeNotDetermined));
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
