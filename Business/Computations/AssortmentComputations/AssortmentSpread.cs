using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Class that defines the AssortmentSpread object.  This class is used by the planning function.
	/// </summary>

	public class AssortmentSpread : Spread
	{
		//=======
		// FIELDS
		//=======

		System.Collections.ArrayList _spreadToCellRefList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of AssortmentSpread.
		/// </summary>

		public AssortmentSpread()
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aIndex"></param>
		/// <returns></returns>

		override protected bool ExcludeValue(int aIndex)
		{
			try
			{
				return ExcludeValue(aIndex, false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Override.  Method that determines if a particular value in the spread is to be excluded.
		/// </summary>
		/// <param name="aIndex">
		/// The index of the value to inspect.
		/// </param>
        /// <param name="aIgnoreTempLock">
		/// indicates whether to ignore locks.
		/// </param>
		/// <returns>
		/// A boolean indicating if the value should be excluded.
		/// </returns>

		override protected bool ExcludeValue(int aIndex, bool aIgnoreTempLock)
		{
			ComputationCellReference planCellRef;

			try
			{
				planCellRef = (ComputationCellReference)_spreadToCellRefList[aIndex];

				if (aIgnoreTempLock)
				{
					if (planCellRef.isCellLocked || 
						planCellRef.isCellBlocked ||
						planCellRef.isCellProtected ||
						planCellRef.isCellIneligible ||
						planCellRef.isCellClosed ||
						planCellRef.isCellReadOnly ||
						planCellRef.isCellExcludedFromSpread)
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					if (planCellRef.isCellLocked ||
						planCellRef.isCellFixed ||
						planCellRef.isCellBlocked ||
						planCellRef.isCellProtected ||
						planCellRef.isCellIneligible ||
						planCellRef.isCellClosed ||
						planCellRef.isCellCompChanged ||
						planCellRef.isCellReadOnly ||
						planCellRef.isCellExcludedFromSpread)
					{
						return true;
					}
					else
					{
						return false;
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
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of ComputationCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of ComputationCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of ComputationCellReference objects that coorespond to the changed value.
		/// </param>

		public void ExecuteSimpleSpread(
			double aSpreadFromValue,
			System.Collections.ArrayList aSpreadToCellRefList,
			int aDecimals,
			out System.Collections.ArrayList aChangedValueList,
			out System.Collections.ArrayList aChangedCellRefList,
			out bool aTempLocksIgnored)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToCellRefList = aSpreadToCellRefList;
				adjustedValueArray = new double[aSpreadToCellRefList.Count];
				aTempLocksIgnored = true;

				for (i = 0; i < aSpreadToCellRefList.Count; i++)
				{
					adjustedValueArray[i] = ((ComputationCellReference)aSpreadToCellRefList[i]).CurrentCellValue;

					if (!ExcludeValue(i, false))
					{
						aTempLocksIgnored = false;
					}
				}

				ExecutePctContributionSpread(aSpreadFromValue, adjustedValueArray, adjustedValueArray, aDecimals, aTempLocksIgnored);

				intGetChangedCellList(aSpreadToCellRefList, adjustedValueArray, out aChangedValueList, out aChangedCellRefList, aTempLocksIgnored);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns an ArrayList of spread values and ComputationCellReferences.  Only values that changed during the execution of the spread will
		/// be returned.
		/// </summary>
		/// <param name="aSpreadToCellRefList">
		/// An ArrayList that contains ComputationCellReference objects to spread to.
		/// </param>
		/// <param name="aAdjustedValueArray">
		/// An array of adjusted values.
		/// </param>
		/// <param name="aChangedValueList">
		/// An out parameter that contains an ArrayList of spread values.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// An out parameter that contains an ArrayList of the original ComputationCellReference objects used in the spread.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		private void intGetChangedCellList(
			System.Collections.ArrayList aSpreadToCellRefList,
			double[] aAdjustedValueArray,
			out System.Collections.ArrayList aChangedValueList,
			out System.Collections.ArrayList aChangedCellRefList,
			bool ignoreLocks)
		{
			int i;

			try
			{
				aChangedValueList = new System.Collections.ArrayList();
				aChangedCellRefList = new System.Collections.ArrayList();

				for (i = 0; i < aAdjustedValueArray.Length; i++)
				{
					if (!ExcludeValue(i, ignoreLocks))
					{
						aChangedValueList.Add(aAdjustedValueArray[i]);
						aChangedCellRefList.Add(aSpreadToCellRefList[i]);
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
