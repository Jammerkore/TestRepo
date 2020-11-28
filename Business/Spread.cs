//Begin Track #4254 - JScott - Spreading not working correctly
// Too many lines changed to mark.  Use SCM Compare for details.
//End Track #4254 - JScott - Spreading not working correctly
using System;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Abstract class that defines basic functionality of a spread.
	/// </summary>

	abstract public class Spread
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of Spread.
		/// </summary>

		public Spread()
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that determines if a particular value in the spread is to be excluded.
		/// </summary>
		/// <param name="aIndex">
		/// The index of the value to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the value should be excluded.
		/// </returns>

		abstract protected bool ExcludeValue(int aIndex);

		/// <summary>
		/// Abstract method that determines if a particular value in the spread is to be excluded.
		/// </summary>
		/// <param name="aIndex">
		/// The index of the value to inspect.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>
		/// <returns>
		/// A boolean indicating if the value should be excluded.
		/// </returns>

		abstract protected bool ExcludeValue(int aIndex, bool ignoreLocks);

		/// <summary>
		/// Executes the spread from the given spread-from value to the given spread-to value array.  The basis value array is used to determine the
		/// % contribution of each store.  The number of decimals determines the decimal length rounding of the result values.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aBasisValueArray">
		/// The basis values to determine the % contribution.
		/// </param>
		/// <param name="aSpreadToValueArray">
		/// The array containing the current values which will be replaces with adjusted values.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>

		protected void ExecutePctContributionSpread(double aSpreadFromValue, double[] aBasisValueArray, double[] aSpreadToValueArray, int aDecimals)
		{
			ExecutePctContributionSpread(aSpreadFromValue, aBasisValueArray, aSpreadToValueArray, aDecimals, false);
		}

		/// <summary>
		/// Executes the spread from the given spread-from value to the given spread-to value array.  The basis value array is used to determine the
		/// % contribution of each store.  The number of decimals determines the decimal length rounding of the result values.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aBasisValueArray">
		/// The basis values to determine the % contribution.
		/// </param>
		/// <param name="aSpreadToValueArray">
		/// The array containing the current values which will be replaces with adjusted values.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		protected void ExecutePctContributionSpread(double aSpreadFromValue, double[] aBasisValueArray, double[] aSpreadToValueArray, 
			int aDecimals, bool ignoreLocks)
		{
			int i;
			int numValues;
			int spreadValueSign;
			decimal[] basisValueArray;
			decimal[] spreadToValueArray;
			BitArray spreadToFlag;
			int excluded = 0;
			decimal basisTotal = 0;
			decimal adjustTotal = 0;
			decimal excludedBasisTotal = 0;
			decimal excludedAdjustTotal = 0;

			try
			{
				numValues = aSpreadToValueArray.Length;

				basisValueArray = new decimal[numValues];
				spreadToValueArray = new decimal[numValues];
				spreadToFlag = new BitArray(numValues, true);

				for (i = 0; i < numValues; i++)
				{
					basisValueArray[i] = (decimal)System.Math.Round(aBasisValueArray[i], aDecimals);
					basisTotal += basisValueArray[i];
				}

				spreadValueSign = System.Math.Sign(basisTotal);

				for (i = 0; i < numValues; i++)
				{
					if (System.Math.Sign(aBasisValueArray[i]) != spreadValueSign)
					{
						basisTotal -= basisValueArray[i];
						basisValueArray[i] = 0;
					}
				}

				for (i = 0; i < numValues; i++)
				{
					spreadToValueArray[i] = (decimal)System.Math.Round(aSpreadToValueArray[i], aDecimals);

					if (ExcludeValue(i, ignoreLocks))
					{
						excludedAdjustTotal = excludedAdjustTotal + spreadToValueArray[i];
						excludedBasisTotal = excludedBasisTotal + basisValueArray[i];
						spreadToFlag[i] = false;
						excluded++;
					}
				}

				if (excluded == numValues)
				{
					throw new NothingToSpreadException();
				}

				basisTotal = basisTotal - excludedBasisTotal;

				if (basisTotal == 0)
				{
					for (i = 0; i < numValues; i++)
					{
						if (spreadToFlag[i])
						{
							basisValueArray[i] = 1;
							basisTotal = basisTotal + 1;
						}
					}
				}

				adjustTotal = (decimal)System.Math.Round(aSpreadFromValue, aDecimals) - excludedAdjustTotal;

				for (i = 0; i < numValues; i++)
				{
					if (spreadToFlag[i])
					{
						if (basisTotal != 0)
						{
							spreadToValueArray[i] = System.Math.Round(adjustTotal * (basisValueArray[i] / basisTotal), aDecimals);
							basisTotal = basisTotal - basisValueArray[i];
							adjustTotal = adjustTotal - spreadToValueArray[i];
						}
						else
						{
							spreadToValueArray[i] = 0;
						}

						aSpreadToValueArray[i] = (double)spreadToValueArray[i];
					}
				}

				if (basisTotal != 0 || adjustTotal != 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_NotAllValuesSpread,
						MIDText.GetText(eMIDTextCode.msg_NotAllValuesSpread));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes the % change spread from the given spread-from values to the given spread-to value array.  The % change is calculated from the old
		/// and new "from" values, and each "to" values is adjust by that amount.  The number of decimals determines the decimal length rounding of the result values.
		/// </summary>
		/// <param name="aSpreadFromOldValue">
		/// The old value of the spread-from cell.
		/// </param>
		/// <param name="aSpreadFromNewValue">
		/// The new value of the spread-from cell.
		/// </param>
		/// <param name="aSpreadToValueArray">
		/// The array containing the current values which will be replaces with adjusted values.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>

		protected void ExecutePctChangeSpread(double aSpreadFromOldValue, 
			double aSpreadFromNewValue, 
			double[] aSpreadToValueArray, 
			int aDecimals)
		{
			try
			{
				ExecutePctChangeSpread(
					aSpreadFromOldValue, 
					aSpreadFromNewValue, 
					aSpreadToValueArray, 
					aDecimals,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes the % change spread from the given spread-from values to the given spread-to value array.  The % change is calculated from the old
		/// and new "from" values, and each "to" values is adjust by that amount.  The number of decimals determines the decimal length rounding of the result values.
		/// </summary>
		/// <param name="aSpreadFromOldValue">
		/// The old value of the spread-from cell.
		/// </param>
		/// <param name="aSpreadFromNewValue">
		/// The new value of the spread-from cell.
		/// </param>
		/// <param name="aSpreadToValueArray">
		/// The array containing the current values which will be replaces with adjusted values.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		protected void ExecutePctChangeSpread(double aSpreadFromOldValue, 
				double aSpreadFromNewValue, 
				double[] aSpreadToValueArray, 
				int aDecimals,
				bool ignoreLocks)
		{
			int i;
			int numValues;
			int spreadValueSign;
			decimal pctChange;
			decimal spreadToValue;

			try
			{
				numValues = aSpreadToValueArray.Length;
				spreadValueSign = System.Math.Sign(aSpreadFromOldValue);

				if (aSpreadFromOldValue > 0)
				{
					pctChange = (decimal)aSpreadFromNewValue / (decimal)aSpreadFromOldValue;

					for (i = 0; i < numValues; i++)
					{
						spreadToValue = (decimal)aSpreadToValueArray[i];

						if (System.Math.Sign(spreadToValue) == spreadValueSign && !ExcludeValue(i, ignoreLocks))
						{
							spreadToValue = System.Math.Round(pctChange * spreadToValue, aDecimals);
							aSpreadToValueArray[i] = (double)spreadToValue;
						}
					}
				}
				else
				{
					for (i = 0; i < numValues; i++)
					{
						aSpreadToValueArray[i] = (double)aSpreadFromNewValue;
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
		/// Executes the % change spread from the given spread-from values to the given spread-to value array.  The % change is calculated from the old
		/// and new "from" values, and each "to" values is adjust by that amount.  The number of decimals determines the decimal length rounding of the result values.
		/// </summary>
		/// <param name="aSpreadFromOldValue">
		/// The old value of the spread-from cell.
		/// </param>
		/// <param name="aSpreadFromNewValue">
		/// The new value of the spread-from cell.
		/// </param>
		/// <param name="aSpreadToValueArray">
		/// The array containing the current values which will be replaces with adjusted values.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>

		protected void ExecutePctChangeSpread(
			double aSpreadFromOldValue, 
			double aSpreadFromNewValue, 
			double[] aBasisValueArray, 
			double[] aSpreadToValueArray, 
			int aDecimals)
		{
			try
			{
				ExecutePctChangeSpread(
					aSpreadFromOldValue, 
					aSpreadFromNewValue, 
					aSpreadToValueArray, 
					aDecimals,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes the % change spread from the given spread-from values to the given spread-to value array.  The % change is calculated from the old
		/// and new "from" values, and each "to" values is adjust by that amount.  The number of decimals determines the decimal length rounding of the result values.
		/// </summary>
		/// <param name="aSpreadFromOldValue">
		/// The old value of the spread-from cell.
		/// </param>
		/// <param name="aSpreadFromNewValue">
		/// The new value of the spread-from cell.
		/// </param>
		/// <param name="aSpreadToValueArray">
		/// The array containing the current values which will be replaces with adjusted values.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		protected void ExecutePctChangeSpread(
			double aSpreadFromOldValue, 
			double aSpreadFromNewValue, 
			double[] aBasisValueArray, 
			double[] aSpreadToValueArray, 
			int aDecimals,
			bool ignoreLocks)
		{
			int i;
			int numValues;
			int spreadValueSign;
			decimal pctChange;
			decimal basisValue;

			try
			{
				numValues = aSpreadToValueArray.Length;
				spreadValueSign = System.Math.Sign(aSpreadFromOldValue);

				if (aSpreadFromOldValue > 0)
				{
					pctChange = (decimal)aSpreadFromNewValue / (decimal)aSpreadFromOldValue;

					for (i = 0; i < numValues; i++)
					{
						basisValue = (decimal)aBasisValueArray[i];

						if (System.Math.Sign(basisValue) == spreadValueSign && !ExcludeValue(i, ignoreLocks))
						{
							basisValue = System.Math.Round(pctChange * basisValue, aDecimals);
							aSpreadToValueArray[i] = (double)basisValue;
						}
					}
				}
				else
				{
					for (i = 0; i < numValues; i++)
					{
						aSpreadToValueArray[i] = (double)aSpreadFromNewValue;
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
		/// Executes the plug spread from the given spread-from values to the given spread-to value array.  The given from value is plugged into each
		/// adjusted amount.  The number of decimals determines the decimal length rounding of the result values.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The old value of the spread-from cell.
		/// </param>
		/// <param name="aSpreadToValueArray">
		/// The array containing the current values which will be replaces with adjusted values.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>

		protected void ExecutePlugSpread(double aSpreadFromValue, double[] aSpreadToValueArray, int aDecimals)
		{
			try
			{
				ExecutePlugSpread(aSpreadFromValue, aSpreadToValueArray, aDecimals, false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes the plug spread from the given spread-from values to the given spread-to value array.  The given from value is plugged into each
		/// adjusted amount.  The number of decimals determines the decimal length rounding of the result values.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The old value of the spread-from cell.
		/// </param>
		/// <param name="aSpreadToValueArray">
		/// The array containing the current values which will be replaces with adjusted values.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		protected void ExecutePlugSpread(double aSpreadFromValue, double[] aSpreadToValueArray, int aDecimals, bool ignoreLocks)
		{
			int i;
			int numValues;
			decimal spreadToValue;

			try
			{
				numValues = aSpreadToValueArray.Length;

				for (i = 0; i < numValues; i++)
				{
					if (!ExcludeValue(i, ignoreLocks))
					{
						spreadToValue = System.Math.Round((decimal)aSpreadFromValue, aDecimals);
						aSpreadToValueArray[i] = (double)spreadToValue;
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

	/// <summary>
	/// Class that defines the PlanSpread object.  This class is used by the planning function.
	/// </summary>

	public class PlanSpread : Spread
	{
		//=======
		// FIELDS
		//=======

		ArrayList _spreadToCellRefList;
		Hashtable _excludedCellRefHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanSpread.
		/// </summary>

		public PlanSpread()
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
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>
		/// <returns>
		/// A boolean indicating if the value should be excluded.
		/// </returns>

		override protected bool ExcludeValue(int aIndex, bool ignoreLocks)
		{
			PlanCellReference planCellRef;

			try
			{
				planCellRef = (PlanCellReference)_spreadToCellRefList[aIndex];

				if (_excludedCellRefHash != null && _excludedCellRefHash.Contains(planCellRef))
				{
					return true;
				}
				else
				{
					if (ignoreLocks)
					{
						if (planCellRef.isCellProtected ||
							planCellRef.isCellIneligible ||
							planCellRef.isCellClosed ||
							planCellRef.isCellCompChanged ||
							planCellRef.isCellReadOnly)
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
							planCellRef.isCellProtected ||
							planCellRef.isCellIneligible ||
							planCellRef.isCellClosed ||
							planCellRef.isCellCompChanged ||
							planCellRef.isCellReadOnly)
						{
							return true;
						}
						else
						{
							return false;
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
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>

		public void ExecuteSimpleSpread(
			double aSpreadFromValue,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList)
		{
			try
			{
				ExecuteSimpleSpread(
					aSpreadFromValue,
					aSpreadToCellRefList,
					aExcludedCellRefHash,
					aDecimals,
					out aChangedValueList,
					out aChangedCellRefList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecuteSimpleSpread(
			double aSpreadFromValue,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToCellRefList = aSpreadToCellRefList;
				_excludedCellRefHash = aExcludedCellRefHash;
				adjustedValueArray = new double[aSpreadToCellRefList.Count];

				for (i = 0; i < aSpreadToCellRefList.Count; i++)
				{
					adjustedValueArray[i] = ((PlanCellReference)aSpreadToCellRefList[i]).CurrentCellValue;
				}

				ExecutePctContributionSpread(aSpreadFromValue, adjustedValueArray, adjustedValueArray, aDecimals, ignoreLocks);

				intGetChangedCellList(aSpreadToCellRefList, adjustedValueArray, out aChangedValueList, out aChangedCellRefList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a basis spread.  A "basis" spread is one who's basis values are different than the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aBasisCellRefList">
		/// The list of basis PlanCellReference objects.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>
		
		public void ExecuteBasisSpread(
			double aSpreadFromValue,
			ArrayList aBasisCellRefList,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList)
		{
			try
			{
				ExecuteBasisSpread(
					aSpreadFromValue,
					aBasisCellRefList,
					aSpreadToCellRefList,
					aExcludedCellRefHash,
					aDecimals,
					out aChangedValueList,
					out aChangedCellRefList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "basis" spread is one who's basis values are different than the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aBasisCellRefList">
		/// The list of PlanCellReference objects that determine the % contribution of each entry.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecuteBasisSpread(
			double aSpreadFromValue,
			ArrayList aBasisCellRefList,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;
			double[] basisValueArray;

			try
			{
				_spreadToCellRefList = aSpreadToCellRefList;
				_excludedCellRefHash = aExcludedCellRefHash;
				adjustedValueArray = new double[aSpreadToCellRefList.Count];
				basisValueArray = new double[aSpreadToCellRefList.Count];

				for (i = 0; i < aSpreadToCellRefList.Count; i++)
				{
					adjustedValueArray[i] = ((PlanCellReference)aSpreadToCellRefList[i]).CurrentCellValue;
					if (i-1 > aBasisCellRefList.Count)
					{
						basisValueArray[i] = 0;
					}
					else
					{
						basisValueArray[i] = ((PlanCellReference)aBasisCellRefList[i]).CurrentCellValue;
					}
				}

				ExecutePctContributionSpread(aSpreadFromValue, basisValueArray, adjustedValueArray, aDecimals, ignoreLocks);

				intGetChangedCellList(aSpreadToCellRefList, adjustedValueArray, out aChangedValueList, out aChangedCellRefList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "basis" spread is one who's basis values are different than the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="basisValueArray">
		/// The list of doubles that determine the % contribution of each entry.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecuteBasisSpread(
			double aSpreadFromValue,
			double[] basisValueArray,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToCellRefList = aSpreadToCellRefList;
				_excludedCellRefHash = aExcludedCellRefHash;
				adjustedValueArray = new double[aSpreadToCellRefList.Count];

				for (i = 0; i < aSpreadToCellRefList.Count; i++)
				{
					adjustedValueArray[i] = ((PlanCellReference)aSpreadToCellRefList[i]).CurrentCellValue;
				}

				ExecutePctContributionSpread(aSpreadFromValue, basisValueArray, adjustedValueArray, aDecimals, ignoreLocks);

				intGetChangedCellList(aSpreadToCellRefList, adjustedValueArray, out aChangedValueList, out aChangedCellRefList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadOldValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadNewValue">
		/// The new value.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>

		public void ExecutePctChangeSpread(
			double aSpreadOldValue,
			double aSpreadNewValue,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList)
		{
			try
			{
				ExecutePctChangeSpread(
					aSpreadOldValue,
					aSpreadNewValue,
					aSpreadToCellRefList,
					aExcludedCellRefHash,
					aDecimals,
					out aChangedValueList,
					out aChangedCellRefList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadOldValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadNewValue">
		/// The new value.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecutePctChangeSpread(
			double aSpreadOldValue,
			double aSpreadNewValue,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToCellRefList = aSpreadToCellRefList;
				_excludedCellRefHash = aExcludedCellRefHash;
				adjustedValueArray = new double[aSpreadToCellRefList.Count];

				for (i = 0; i < aSpreadToCellRefList.Count; i++)
				{
					adjustedValueArray[i] = ((PlanCellReference)aSpreadToCellRefList[i]).CurrentCellValue;
				}

				ExecutePctChangeSpread(aSpreadOldValue, aSpreadNewValue, adjustedValueArray, aDecimals, ignoreLocks);

				intGetChangedCellList(aSpreadToCellRefList, adjustedValueArray, out aChangedValueList, out aChangedCellRefList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadOldValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadNewValue">
		/// The new value.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>

		public void ExecutePctChangeSpread(
			double aSpreadOldValue,
			double aSpreadNewValue,
			ArrayList aBasisCellRefList,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList)
		{
			try
			{
				ExecutePctChangeSpread(
					aSpreadOldValue,
					aSpreadNewValue,
					aBasisCellRefList,
					aSpreadToCellRefList,
					aExcludedCellRefHash,
					aDecimals,
					out aChangedValueList,
					out aChangedCellRefList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadOldValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadNewValue">
		/// The new value.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecutePctChangeSpread(
			double aSpreadOldValue,
			double aSpreadNewValue,
			ArrayList aBasisCellRefList,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;
			double[] basisValueArray;

			try
			{
				_spreadToCellRefList = aSpreadToCellRefList;
				_excludedCellRefHash = aExcludedCellRefHash;
				adjustedValueArray = new double[aSpreadToCellRefList.Count];
				basisValueArray = new double[aSpreadToCellRefList.Count];

				for (i = 0; i < aSpreadToCellRefList.Count; i++)
				{
					adjustedValueArray[i] = ((PlanCellReference)aSpreadToCellRefList[i]).CurrentCellValue;
					if (i-1 > aBasisCellRefList.Count)
					{
						basisValueArray[i] = 0;
					}
					else
					{
						basisValueArray[i] = ((PlanCellReference)aBasisCellRefList[i]).CurrentCellValue;
					}
				}

				ExecutePctChangeSpread(aSpreadOldValue, aSpreadNewValue, basisValueArray, adjustedValueArray, aDecimals, ignoreLocks);

				intGetChangedCellList(aSpreadToCellRefList, adjustedValueArray, out aChangedValueList, out aChangedCellRefList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadOldValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadNewValue">
		/// The new value.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecutePctChangeSpread(
			double aSpreadOldValue,
			double aSpreadNewValue,
			double[] basisValueArray,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToCellRefList = aSpreadToCellRefList;
				_excludedCellRefHash = aExcludedCellRefHash;
				adjustedValueArray = new double[aSpreadToCellRefList.Count];


				for (i = 0; i < aSpreadToCellRefList.Count; i++)
				{
					adjustedValueArray[i] = ((PlanCellReference)aSpreadToCellRefList[i]).CurrentCellValue;
				}

				ExecutePctChangeSpread(aSpreadOldValue, aSpreadNewValue, basisValueArray, adjustedValueArray, aDecimals, ignoreLocks);

				intGetChangedCellList(aSpreadToCellRefList, adjustedValueArray, out aChangedValueList, out aChangedCellRefList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>

		public void ExecutePlugSpread(
			double aSpreadValue,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList)
		{
			try
			{
				ExecutePlugSpread(
					aSpreadValue,
					aSpreadToCellRefList,
					aExcludedCellRefHash,
					aDecimals,
					out aChangedValueList,
					out aChangedCellRefList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of PlanCellReferences for each value that has changed.
		/// </summary>
		/// <param name="aSpreadValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadToCellRefList">
		/// The list of PlanCellReference objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// A list of PlanCellReference objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecutePlugSpread(
			double aSpreadValue,
			ArrayList aSpreadToCellRefList,
			Hashtable aExcludedCellRefHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToCellRefList = aSpreadToCellRefList;
				_excludedCellRefHash = aExcludedCellRefHash;
				adjustedValueArray = new double[aSpreadToCellRefList.Count];

				for (i = 0; i < aSpreadToCellRefList.Count; i++)
				{
					adjustedValueArray[i] = ((PlanCellReference)aSpreadToCellRefList[i]).CurrentCellValue;
				}

				ExecutePlugSpread(aSpreadValue, adjustedValueArray, aDecimals, ignoreLocks);

				intGetChangedCellList(aSpreadToCellRefList, adjustedValueArray, out aChangedValueList, out aChangedCellRefList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns an ArrayList of spread values and PlanCellReferences.  Only values that changed during the execution of the spread will
		/// be returned.
		/// </summary>
		/// <param name="aSpreadToCellRefList">
		/// An ArrayList that contains PlanCellReference objects to spread to.
		/// </param>
		/// <param name="aAdjustedValueArray">
		/// An array of adjusted values.
		/// </param>
		/// <param name="aChangedValueList">
		/// An out parameter that contains an ArrayList of spread values.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// An out parameter that contains an ArrayList of the original PlanCellReference objects used in the spread.
		/// </param>

		private void intGetChangedCellList(
			ArrayList aSpreadToCellRefList,
			double[] aAdjustedValueArray,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList)
		{
			try
			{
				intGetChangedCellList(
					aSpreadToCellRefList,
					aAdjustedValueArray,
					out  aChangedValueList,
					out  aChangedCellRefList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns an ArrayList of spread values and PlanCellReferences.  Only values that changed during the execution of the spread will
		/// be returned.
		/// </summary>
		/// <param name="aSpreadToCellRefList">
		/// An ArrayList that contains PlanCellReference objects to spread to.
		/// </param>
		/// <param name="aAdjustedValueArray">
		/// An array of adjusted values.
		/// </param>
		/// <param name="aChangedValueList">
		/// An out parameter that contains an ArrayList of spread values.
		/// </param>
		/// <param name="aChangedCellRefList">
		/// An out parameter that contains an ArrayList of the original PlanCellReference objects used in the spread.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		private void intGetChangedCellList(
			ArrayList aSpreadToCellRefList,
			double[] aAdjustedValueArray,
			out ArrayList aChangedValueList,
			out ArrayList aChangedCellRefList,
			bool ignoreLocks)
		{
			int i;
			aChangedValueList = new ArrayList();
			aChangedCellRefList = new ArrayList();

			try
			{
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
	//Begin TT#155 - JScott - Add Size Curve info to Node Properties

	/// <summary>
	/// Class that defines the PlanSpread object.  This class is used by the planning function.
	/// </summary>

	public class SizeCurveSpread : Spread
	{
		//=======
		// FIELDS
		//=======

		ArrayList _spreadToBalItemList;
		Hashtable _excludedBalItemHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanSpread.
		/// </summary>

		public SizeCurveSpread()
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
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>
		/// <returns>
		/// A boolean indicating if the value should be excluded.
		/// </returns>

		override protected bool ExcludeValue(int aIndex, bool ignoreLocks)
		{
			SizeCurveBalanceItem curveBalItem;

			try
			{
				curveBalItem = (SizeCurveBalanceItem)_spreadToBalItemList[aIndex];

				if (_excludedBalItemHash != null && _excludedBalItemHash.Contains(curveBalItem))
				{
					return true;
				}
				else
				{
					if (curveBalItem.Locked)
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
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>

		public void ExecuteSimpleSpread(
			double aSpreadFromValue,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList)
		{
			try
			{
				ExecuteSimpleSpread(
					aSpreadFromValue,
					aSpreadToBalItemList,
					aExcludedBalItemHash,
					aDecimals,
					out aChangedValueList,
					out aChangedBalItemList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecuteSimpleSpread(
			double aSpreadFromValue,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToBalItemList = aSpreadToBalItemList;
				_excludedBalItemHash = aExcludedBalItemHash;
				adjustedValueArray = new double[aSpreadToBalItemList.Count];

				for (i = 0; i < aSpreadToBalItemList.Count; i++)
				{
					adjustedValueArray[i] = ((SizeCurveBalanceItem)aSpreadToBalItemList[i]).Value;
				}

				ExecutePctContributionSpread(aSpreadFromValue, adjustedValueArray, adjustedValueArray, aDecimals, ignoreLocks);

				intGetBalItemList(aSpreadToBalItemList, adjustedValueArray, out aChangedValueList, out aChangedBalItemList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a basis spread.  A "basis" spread is one who's basis values are different than the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aBasisBalItemList">
		/// The list of basis SizeCurveBalanceItem objects.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>

		public void ExecuteBasisSpread(
			double aSpreadFromValue,
			ArrayList aBasisBalItemList,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList)
		{
			try
			{
				ExecuteBasisSpread(
					aSpreadFromValue,
					aBasisBalItemList,
					aSpreadToBalItemList,
					aExcludedBalItemHash,
					aDecimals,
					out aChangedValueList,
					out aChangedBalItemList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "basis" spread is one who's basis values are different than the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aBasisBalItemList">
		/// The list of SizeCurveBalanceItem objects that determine the % contribution of each entry.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecuteBasisSpread(
			double aSpreadFromValue,
			ArrayList aBasisBalItemList,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;
			double[] basisValueArray;

			try
			{
				_spreadToBalItemList = aSpreadToBalItemList;
				_excludedBalItemHash = aExcludedBalItemHash;
				adjustedValueArray = new double[aSpreadToBalItemList.Count];
				basisValueArray = new double[aSpreadToBalItemList.Count];

				for (i = 0; i < aSpreadToBalItemList.Count; i++)
				{
					adjustedValueArray[i] = ((SizeCurveBalanceItem)aSpreadToBalItemList[i]).Value;
					if (i - 1 > aBasisBalItemList.Count)
					{
						basisValueArray[i] = 0;
					}
					else
					{
						basisValueArray[i] = ((SizeCurveBalanceItem)aBasisBalItemList[i]).Value;
					}
				}

				ExecutePctContributionSpread(aSpreadFromValue, basisValueArray, adjustedValueArray, aDecimals, ignoreLocks);

				intGetBalItemList(aSpreadToBalItemList, adjustedValueArray, out aChangedValueList, out aChangedBalItemList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "basis" spread is one who's basis values are different than the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="basisValueArray">
		/// The list of doubles that determine the % contribution of each entry.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecuteBasisSpread(
			double aSpreadFromValue,
			double[] basisValueArray,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToBalItemList = aSpreadToBalItemList;
				_excludedBalItemHash = aExcludedBalItemHash;
				adjustedValueArray = new double[aSpreadToBalItemList.Count];

				for (i = 0; i < aSpreadToBalItemList.Count; i++)
				{
					adjustedValueArray[i] = ((SizeCurveBalanceItem)aSpreadToBalItemList[i]).Value;
				}

				ExecutePctContributionSpread(aSpreadFromValue, basisValueArray, adjustedValueArray, aDecimals, ignoreLocks);

				intGetBalItemList(aSpreadToBalItemList, adjustedValueArray, out aChangedValueList, out aChangedBalItemList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadOldValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadNewValue">
		/// The new value.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>

		public void ExecutePctChangeSpread(
			double aSpreadOldValue,
			double aSpreadNewValue,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList)
		{
			try
			{
				ExecutePctChangeSpread(
					aSpreadOldValue,
					aSpreadNewValue,
					aSpreadToBalItemList,
					aExcludedBalItemHash,
					aDecimals,
					out aChangedValueList,
					out aChangedBalItemList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadOldValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadNewValue">
		/// The new value.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecutePctChangeSpread(
			double aSpreadOldValue,
			double aSpreadNewValue,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToBalItemList = aSpreadToBalItemList;
				_excludedBalItemHash = aExcludedBalItemHash;
				adjustedValueArray = new double[aSpreadToBalItemList.Count];

				for (i = 0; i < aSpreadToBalItemList.Count; i++)
				{
					adjustedValueArray[i] = ((SizeCurveBalanceItem)aSpreadToBalItemList[i]).Value;
				}

				ExecutePctChangeSpread(aSpreadOldValue, aSpreadNewValue, adjustedValueArray, aDecimals, ignoreLocks);

				intGetBalItemList(aSpreadToBalItemList, adjustedValueArray, out aChangedValueList, out aChangedBalItemList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadOldValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadNewValue">
		/// The new value.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>

		public void ExecutePctChangeSpread(
			double aSpreadOldValue,
			double aSpreadNewValue,
			ArrayList aBasisBalItemList,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList)
		{
			try
			{
				ExecutePctChangeSpread(
					aSpreadOldValue,
					aSpreadNewValue,
					aBasisBalItemList,
					aSpreadToBalItemList,
					aExcludedBalItemHash,
					aDecimals,
					out aChangedValueList,
					out aChangedBalItemList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadOldValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadNewValue">
		/// The new value.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecutePctChangeSpread(
			double aSpreadOldValue,
			double aSpreadNewValue,
			ArrayList aBasisBalItemList,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;
			double[] basisValueArray;

			try
			{
				_spreadToBalItemList = aSpreadToBalItemList;
				_excludedBalItemHash = aExcludedBalItemHash;
				adjustedValueArray = new double[aSpreadToBalItemList.Count];
				basisValueArray = new double[aSpreadToBalItemList.Count];

				for (i = 0; i < aSpreadToBalItemList.Count; i++)
				{
					adjustedValueArray[i] = ((SizeCurveBalanceItem)aSpreadToBalItemList[i]).Value;
					if (i - 1 > aBasisBalItemList.Count)
					{
						basisValueArray[i] = 0;
					}
					else
					{
						basisValueArray[i] = ((SizeCurveBalanceItem)aBasisBalItemList[i]).Value;
					}
				}

				ExecutePctChangeSpread(aSpreadOldValue, aSpreadNewValue, basisValueArray, adjustedValueArray, aDecimals, ignoreLocks);

				intGetBalItemList(aSpreadToBalItemList, adjustedValueArray, out aChangedValueList, out aChangedBalItemList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadOldValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadNewValue">
		/// The new value.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecutePctChangeSpread(
			double aSpreadOldValue,
			double aSpreadNewValue,
			double[] basisValueArray,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToBalItemList = aSpreadToBalItemList;
				_excludedBalItemHash = aExcludedBalItemHash;
				adjustedValueArray = new double[aSpreadToBalItemList.Count];


				for (i = 0; i < aSpreadToBalItemList.Count; i++)
				{
					adjustedValueArray[i] = ((SizeCurveBalanceItem)aSpreadToBalItemList[i]).Value;
				}

				ExecutePctChangeSpread(aSpreadOldValue, aSpreadNewValue, basisValueArray, adjustedValueArray, aDecimals, ignoreLocks);

				intGetBalItemList(aSpreadToBalItemList, adjustedValueArray, out aChangedValueList, out aChangedBalItemList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>

		public void ExecutePlugSpread(
			double aSpreadValue,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList)
		{
			try
			{
				ExecutePlugSpread(
					aSpreadValue,
					aSpreadToBalItemList,
					aExcludedBalItemHash,
					aDecimals,
					out aChangedValueList,
					out aChangedBalItemList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  This method returns a changed value list
		/// and a cooresponding list of SizeCurveBalanceItem for each value that has changed.
		/// </summary>
		/// <param name="aSpreadValue">
		/// The old value.
		/// </param>
		/// <param name="aSpreadToBalItemList">
		/// The list of SizeCurveBalanceItem objects that the value is to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of values that have changed.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// A list of SizeCurveBalanceItem objects that coorespond to the changed value.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		public void ExecutePlugSpread(
			double aSpreadValue,
			ArrayList aSpreadToBalItemList,
			Hashtable aExcludedBalItemHash,
			int aDecimals,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList,
			bool ignoreLocks)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				_spreadToBalItemList = aSpreadToBalItemList;
				_excludedBalItemHash = aExcludedBalItemHash;
				adjustedValueArray = new double[aSpreadToBalItemList.Count];

				for (i = 0; i < aSpreadToBalItemList.Count; i++)
				{
					adjustedValueArray[i] = ((SizeCurveBalanceItem)aSpreadToBalItemList[i]).Value;
				}

				ExecutePlugSpread(aSpreadValue, adjustedValueArray, aDecimals, ignoreLocks);

				intGetBalItemList(aSpreadToBalItemList, adjustedValueArray, out aChangedValueList, out aChangedBalItemList, ignoreLocks);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns an ArrayList of spread values and SizeCurveBalanceItem.  Only values that changed during the execution of the spread will
		/// be returned.
		/// </summary>
		/// <param name="aSpreadToBalItemList">
		/// An ArrayList that contains SizeCurveBalanceItem objects to spread to.
		/// </param>
		/// <param name="aAdjustedValueArray">
		/// An array of adjusted values.
		/// </param>
		/// <param name="aChangedValueList">
		/// An out parameter that contains an ArrayList of spread values.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// An out parameter that contains an ArrayList of the original SizeCurveBalanceItem objects used in the spread.
		/// </param>

		private void intGetBalItemList(
			ArrayList aSpreadToBalItemList,
			double[] aAdjustedValueArray,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList)
		{
			try
			{
				intGetBalItemList(
					aSpreadToBalItemList,
					aAdjustedValueArray,
					out  aChangedValueList,
					out  aChangedBalItemList,
					false);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns an ArrayList of spread values and SizeCurveBalanceItem.  Only values that changed during the execution of the spread will
		/// be returned.
		/// </summary>
		/// <param name="aSpreadToBalItemList">
		/// An ArrayList that contains PlanBalItemerence objects to spread to.
		/// </param>
		/// <param name="aAdjustedValueArray">
		/// An array of adjusted values.
		/// </param>
		/// <param name="aChangedValueList">
		/// An out parameter that contains an ArrayList of spread values.
		/// </param>
		/// <param name="aChangedBalItemList">
		/// An out parameter that contains an ArrayList of the original PlanBalItemerence objects used in the spread.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>

		private void intGetBalItemList(
			ArrayList aSpreadToBalItemList,
			double[] aAdjustedValueArray,
			out ArrayList aChangedValueList,
			out ArrayList aChangedBalItemList,
			bool ignoreLocks)
		{
			int i;
			aChangedValueList = new ArrayList();
			aChangedBalItemList = new ArrayList();

			try
			{
				for (i = 0; i < aAdjustedValueArray.Length; i++)
				{
					aChangedValueList.Add(aAdjustedValueArray[i]);
					aChangedBalItemList.Add(aSpreadToBalItemList[i]);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	
	//End TT#155 - JScott - Add Size Curve info to Node Properties
	/// <summary>
	/// Class that defines the BasicSpread object.  This class can be used by any process that needs to spread to all values.
	/// This class does not used locks or other flags to inhibit a value from being spread to.
	/// </summary>

	public class BasicSpread : Spread
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of BasicSpread.
		/// </summary>

		public BasicSpread()
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Override.  Method that determines if a particular value in the spread is to be excluded.
		/// </summary>
		/// <param name="aIndex">
		/// The index of the value to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the value should be excluded.
		/// </returns>

		override protected bool ExcludeValue(int aIndex)
		{
			return false;
		}

		/// <summary>
		/// Override.  Method that determines if a particular value in the spread is to be excluded.
		/// </summary>
		/// <param name="aIndex">
		/// The index of the value to inspect.
		/// </param>
		/// <param name="ignoreLocks">
		/// indicates whether to ignore locks.
		/// </param>
		/// <returns>
		/// A boolean indicating if the value should be excluded.
		/// </returns>

		override protected bool ExcludeValue(int aIndex, bool ignoreLocks)
		{
			return false;
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aSpreadToList">
		/// The list of values to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of spread values.
		/// </param>
		
		public void ExecuteSimpleSpread(
			double aSpreadFromValue,
			ArrayList aSpreadToList,
			int aDecimals,
			out ArrayList aChangedValueList)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				adjustedValueArray = new double[aSpreadToList.Count];
				aChangedValueList = new ArrayList();

				for (i = 0; i < aSpreadToList.Count; i++)
				{
					if (aSpreadToList[i].GetType() == typeof(System.Int32))
					{
						adjustedValueArray[i] = Convert.ToDouble((int)aSpreadToList[i], CultureInfo.CurrentUICulture);
					}
					else
					{
						adjustedValueArray[i] = (double)aSpreadToList[i];
					}
				}

				ExecutePctContributionSpread(aSpreadFromValue, adjustedValueArray, adjustedValueArray, aDecimals);

				for (i = 0; i < aSpreadToList.Count; i++)
				{
					aChangedValueList.Add(adjustedValueArray[i]);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        /// <summary>
        /// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  
        /// </summary>
        /// <param name="aSpreadFromValue">
        /// The value to spread.
        /// </param>
        /// <param name="aSpreadToList">
        /// The list of values to be spread to.
        /// </param>
        /// <param name="aDecimals">
        /// The number of decimals to round the spread amounts to.
        /// </param>
        /// <param name="aChangedValueList">
        /// The list of spread values.
        /// </param>

        public void ExecuteSimpleSpread(
            double aSpreadFromValue,
            ArrayList aSpreadToList,
            int aDecimals,
            out ArrayList aChangedValueList,
            bool ignoreLocks)
        {
            int i;
            double[] adjustedValueArray;

            try
            {
                adjustedValueArray = new double[aSpreadToList.Count];
                aChangedValueList = new ArrayList();

                for (i = 0; i < aSpreadToList.Count; i++)
                {
                    if (aSpreadToList[i].GetType() == typeof(System.Int32))
                    {
                        adjustedValueArray[i] = Convert.ToDouble((int)aSpreadToList[i], CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        adjustedValueArray[i] = (double)aSpreadToList[i];
                    }
                }

                ExecutePctContributionSpread(aSpreadFromValue, adjustedValueArray, adjustedValueArray, aDecimals, ignoreLocks);

                for (i = 0; i < aSpreadToList.Count; i++)
                {
                    aChangedValueList.Add(adjustedValueArray[i]);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		/// <summary>
		/// Executes a simple spread.  A "basis" spread is one who's basis values are different than the spread-to values.  This method returns a changed value list.
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aBasisList">
		/// The list of basis values that determine the % contribution of each entry.
		/// </param>
		/// <param name="aSpreadToList">
		/// The list of values that to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of spread values.
		/// </param>
		
		public void ExecuteBasisSpread(
			double aSpreadFromValue,
			ArrayList aBasisList,
			ArrayList aSpreadToList,
			int aDecimals,
			out ArrayList aChangedValueList)
		{
			int i;
			double[] adjustedValueArray;
			double[] basisValueArray;

			try
			{
				adjustedValueArray = new double[aSpreadToList.Count];
				basisValueArray = new double[aBasisList.Count];
				aChangedValueList = new ArrayList();

				for (i = 0; i < aSpreadToList.Count; i++)
				{
					if (aSpreadToList[i].GetType() == typeof(System.Int32))
					{
						adjustedValueArray[i] = Convert.ToDouble((int)aSpreadToList[i], CultureInfo.CurrentUICulture);
						basisValueArray[i] = Convert.ToDouble((int)aBasisList[i], CultureInfo.CurrentUICulture);
					}
					else
					{
						adjustedValueArray[i] = (double)aSpreadToList[i];
						basisValueArray[i] = (double)aBasisList[i];
					}
				}

				ExecutePctContributionSpread(aSpreadFromValue, basisValueArray, adjustedValueArray, aDecimals);

				for (i = 0; i < aSpreadToList.Count; i++)
				{
					aChangedValueList.Add(adjustedValueArray[i]);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  
		/// </summary>
		/// <param name="aSpreadFromOldValue">
		/// The original value.
		/// </param>
		/// <param name="aSpreadFromNewValue">
		/// The value to spread.
		/// </param>
		/// <param name="aSpreadToList">
		/// The list of values to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of spread values.
		/// </param>
		
		public void ExecutePctChangeSpread(
			double aSpreadFromOldValue,
			double aSpreadFromNewValue,
			ArrayList aSpreadToList,
			int aDecimals,
			out ArrayList aChangedValueList)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				adjustedValueArray = new double[aSpreadToList.Count];
				aChangedValueList = new ArrayList();

				for (i = 0; i < aSpreadToList.Count; i++)
				{
					if (aSpreadToList[i].GetType() == typeof(System.Int32))
					{
						adjustedValueArray[i] = Convert.ToDouble((int)aSpreadToList[i], CultureInfo.CurrentUICulture);
					}
					else
					{
						adjustedValueArray[i] = (double)aSpreadToList[i];
					}
				}

				ExecutePctChangeSpread(aSpreadFromOldValue, aSpreadFromNewValue, adjustedValueArray, aDecimals);

				for (i = 0; i < aSpreadToList.Count; i++)
				{
					aChangedValueList.Add(adjustedValueArray[i]);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Executes a simple spread.  A "simple" spread is one who's basis values are the same as the spread-to values.  
		/// </summary>
		/// <param name="aSpreadFromValue">
		/// The value to spread.
		/// </param>
		/// <param name="aSpreadToList">
		/// The list of values to be spread to.
		/// </param>
		/// <param name="aDecimals">
		/// The number of decimals to round the spread amounts to.
		/// </param>
		/// <param name="aChangedValueList">
		/// The list of spread values.
		/// </param>
		
		public void ExecutePlugSpread(
			double aSpreadFromValue,
			ArrayList aSpreadToList,
			int aDecimals,
			out ArrayList aChangedValueList)
		{
			int i;
			double[] adjustedValueArray;

			try
			{
				adjustedValueArray = new double[aSpreadToList.Count];
				aChangedValueList = new ArrayList();

				for (i = 0; i < aSpreadToList.Count; i++)
				{
					if (aSpreadToList[i].GetType() == typeof(System.Int32))
					{
						adjustedValueArray[i] = Convert.ToDouble((int)aSpreadToList[i], CultureInfo.CurrentUICulture);
					}
					else
					{
						adjustedValueArray[i] = (double)aSpreadToList[i];
					}
				}

				ExecutePlugSpread(aSpreadFromValue, adjustedValueArray, aDecimals);

				for (i = 0; i < aSpreadToList.Count; i++)
				{
					aChangedValueList.Add(adjustedValueArray[i]);
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
