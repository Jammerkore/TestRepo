using System;
using System.Collections;
using MIDRetail.Data;
using MIDRetail.DataCommon;


namespace MIDRetail.Common
{
	/// <summary>
	/// Container to hold store information for calculating store grades.
	/// </summary>
	/// <remarks>
	/// This structure describes the store information necessary to calculate a store grade.
	/// The contents include:
	/// <list type="bullet">
	/// <item>StoreKey: Identifies the store.</item>
	/// <item>StoreGradeUnits: The store's total units that are to used to calculate its grade (these units should be of the same type for each store: sales, inventory or other variable type).</item>
	/// <item>StoreEligilbe: True indicates this store is eligible; False indicates it is not eligible. Grades are calculated for eligible stores only.</item>
	/// </list></remarks>
	public struct GradeStoreBin
	{
		//=======
		// FIELDS
		//=======
		int _storeKey;
		double _storeGradeUnits;
		bool _storeEligible;
        int _storeSglRID;   // TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)

		//=============
		// CONSTRUCTORS
		//=============

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets the store's key.
		/// </summary>
		public int StoreKey
		{
			get
			{
				return _storeKey;
			}
			set
			{
				_storeKey = value;
			}
		}
		/// <summary>
		/// Gets or sets the store's total units on which to base its grade calculation.
		/// </summary>
		public double StoreGradeUnits
		{
			get
			{
				return _storeGradeUnits;
			}
			set
			{
				_storeGradeUnits = value;
			}
		}
		/// <summary>
		/// Gets or sets the store's eligiblity flag value. True indicates eligible and False indicates not eligible.
		/// </summary>
		public bool StoreEligible
		{
			get
			{
				return _storeEligible;
			}
			set
			{
				_storeEligible = value;
			}
		}
        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
        /// <summary>
        /// Gets or sets the store's attribute set.
        /// </summary>
        public int StoreSglRID
        {
            get
            {
                return _storeSglRID;
            }
            set
            {
                _storeSglRID = value;
            }
        }
        // End TT3618
		//========
		// METHODS
		//========
	}
	/// <summary>
	/// A class containing static calculations for store grade.
	/// </summary>
	/// <remarks>
	/// The following static calculations are provided:
	/// <list type="bullet">
	/// <item>GetGradeProfileKey: A StoreGradeList and GradeStoreBin array are the input parameters to this calculation. Output is an array of the grade profile key (the array is in the same order as the GradeStoreBin input array).</item>
	/// <item>GetGradeIndex: An array of the low boundaries for each grade and a GradeStoreBin array are the input parameters to this calculation.  Output is an array of the grade indices (the array is in the same order as the GradeStoreBin input array and each entry is an index to the Low Boundary Grade array that was input.</item>
	/// </list>
	/// </remarks>
	public class StoreGrade
	{
		/// <summary>
		/// Calculates the grade profile key for each eligible store based on the input from the GradeStoreBin array. 
		/// </summary>
		/// <param name="aGradeProfileList">Profile List of grades.</param>
		/// <param name="aGradeStoreBin">Array containing the units by store that are to be used in the calculation.</param>
		/// <returns>Grade Profile Key array associated with and in same sequence as the GradeStoreBin array.</returns>
		public static int [] GetGradeProfileKey (StoreGradeList aGradeProfileList, GradeStoreBin[] aGradeStoreBin) 
		{
			double[] gradeBoundary = new double[aGradeProfileList.Count];
			StoreGradeProfile sgp;
			for (int i = 0; i < aGradeProfileList.Count; i++)
			{
				sgp = (StoreGradeProfile) aGradeProfileList.ArrayList[i];
				gradeBoundary[i] = sgp.Boundary;
			}
			int [] storeGrade = GetGradeIndex(gradeBoundary, aGradeStoreBin);
			int [] storeGradeKey = new int[storeGrade.Length];
			for (int sg = 0; sg < storeGrade.Length; sg++)
			{
				sgp = (StoreGradeProfile) aGradeProfileList.ArrayList[storeGrade[sg]];
				storeGradeKey[sg] = sgp.Key;
			}
			return storeGradeKey;
		}
		/// <summary>
		/// Calculates the grade index (index into the GradeBoundary array) for each store based on the input from GradeStoreBin array.
		/// </summary>
		/// <param name="aGradeBoundary">Grade Boundaries.</param>
		/// <param name="aGradeStoreBin">GradeStoreBin containing the units by store on which to base the calculation.</param>
		/// <returns>Array of grade indices associated with and in the same order as the GradeStoreBin array.</returns>
		public static int [] GetGradeIndex (double[] aGradeBoundary, GradeStoreBin[] aGradeStoreBin)
		{
			if (aGradeBoundary == null)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_NoGradeDefinition,     
					MIDText.GetText(eMIDTextCode.msg_NoGradeDefinition));
			}
            MIDGenericSortItem[] gse = new MIDGenericSortItem[aGradeBoundary.Length];
            for (int i = 0; i < aGradeBoundary.Length; i++)
            {
                gse[i].Item = i;
                gse[i].SortKey = new double[1];
                gse[i].SortKey[0] = aGradeBoundary[i];
            }
            Array.Sort(gse, new MIDGenericSortDescendingComparer());
			double eligibleTotal = 0.0d;
			int eligibleCount = 0;
			foreach (GradeStoreBin gsb in aGradeStoreBin)
			{
				if (gsb.StoreEligible == true)
				{
					eligibleCount++;
					eligibleTotal += gsb.StoreGradeUnits;
				}
			}
			double averageStore;
			if (eligibleCount > 0)
			{
				averageStore = eligibleTotal / eligibleCount;
			}
			else
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_GradeCalcFail_NoEligibleStores,
					MIDText.GetText(eMIDTextCode.msg_GradeCalcFail_NoEligibleStores));
			}
			int [] storeGrade = new int[aGradeStoreBin.Length];
			for (int i = 0; i < aGradeStoreBin.Length; i++)
			{
				double percentToAverage = 0.0;
				if (aGradeStoreBin[i].StoreEligible == true)
				{
					if (averageStore > 0)
					{
						percentToAverage = (aGradeStoreBin[i].StoreGradeUnits * 100) / averageStore;
						for (storeGrade[i] = 0;
							(storeGrade[i] < gse.Length && percentToAverage < aGradeBoundary[gse[storeGrade[i]].Item]);
							storeGrade[i]+=1);
						if (storeGrade[i] == gse.Length)
						{
							storeGrade[i] = gse.Length - 1;
						}
					}
					else
					{
						storeGrade[i] = gse.Length - 1;
					}
				}
				else
				{
					// BEGIN MID Track #2539 Grades not same
//					storeGrade[i] = 0;
                    storeGrade[i] = gse.Length - 1;
					// END MID Track #2539
				}
			}
			return storeGrade;
		}

        // Begin TT#618 - RMatelic - Allocation Override - Add Attribute Sets (#35)
        public static int[] GetGradeIndex(Hashtable aBoundaryHT, GradeStoreBin[] aGradeStoreBin)
        {
            if (aBoundaryHT == null)
            {
                throw new MIDException(eErrorLevel.severe,
                    (int)eMIDTextCode.msg_NoGradeDefinition,
                    MIDText.GetText(eMIDTextCode.msg_NoGradeDefinition));
            }
            double eligibleTotal = 0.0d;
            int eligibleCount = 0;
            foreach (GradeStoreBin gsb in aGradeStoreBin)
            {
                if (gsb.StoreEligible == true)
                {
                    eligibleCount++;
                    eligibleTotal += gsb.StoreGradeUnits;
                }
            }
            double averageStore;
            if (eligibleCount > 0)
            {
                averageStore = eligibleTotal / eligibleCount;
            }
            else
            {
                throw new MIDException(eErrorLevel.severe,
                    (int)eMIDTextCode.msg_GradeCalcFail_NoEligibleStores,
                    MIDText.GetText(eMIDTextCode.msg_GradeCalcFail_NoEligibleStores));
            }
            int[] storeGrade = new int[aGradeStoreBin.Length];
            for (int i = 0; i < aGradeStoreBin.Length; i++)
            {
                double percentToAverage = 0.0;
                if (aGradeStoreBin[i].StoreEligible == true)
                {
                    if (averageStore > 0)
                    {
                        percentToAverage = (aGradeStoreBin[i].StoreGradeUnits * 100) / averageStore;
                    }
                }
                foreach (int sglRID in aBoundaryHT.Keys)
                {
                    bool boundaryFound = false;
                    if (aGradeStoreBin[i].StoreSglRID == sglRID)
                    {
                        SortedList boundarySL = (SortedList)aBoundaryHT[sglRID];
                        // Begin TT#902 - RMatelic - Store Grades in Style Review and OTS Forecast not matching
                        //                original code went incorrectly top > down thru the sorted list; correction goes from the bottom up  
                        //foreach (double lowBoundary in boundarySL.Keys)
                        //{
                        //    if (percentToAverage <= lowBoundary)
                        //    {
                        //        storeGrade[i] = (int)boundarySL[lowBoundary];
                        //        boundaryFound = true;
                        //        break;
                        //    }
                        //}
                        for (int k = boundarySL.Count - 1; k >= 0; k--)
                        {
                            double lowBoundary = (double)boundarySL.GetKey(k);
                            if (percentToAverage >= lowBoundary)
                            {
                                storeGrade[i] = (int)boundarySL[lowBoundary];
                                boundaryFound = true;
                                break;
                            }
                        }
                        // End TT#902
                    }
                    if (boundaryFound)
                    {
                        break;  
                    }
                }
            }
            return storeGrade;
        }
        // End TT#618

		// BEGIN issue 4288 - stodd 2.14.2007
		/// <summary>
		/// Calculates the grade profile key for each eligible store based on the input from the GradeStoreBin array. Allows for
		/// average store value to be sent in.
		/// </summary>
		/// <param name="aGradeProfileList">Profile List of grades.</param>
		/// <param name="aGradeStoreBin">Array containing the units by store that are to be used in the calculation.</param>
		/// <returns>Grade Profile Key array associated with and in same sequence as the GradeStoreBin array.</returns>
		public static int [] GetGradeProfileKey (StoreGradeList aGradeProfileList, GradeStoreBin[] aGradeStoreBin, double averageStore) 
		{
			double[] gradeBoundary = new double[aGradeProfileList.Count];
			StoreGradeProfile sgp;
			for (int i = 0; i < aGradeProfileList.Count; i++)
			{
				sgp = (StoreGradeProfile) aGradeProfileList.ArrayList[i];
				gradeBoundary[i] = sgp.Boundary;
			}
			int [] storeGrade = GetGradeIndex(gradeBoundary, aGradeStoreBin, averageStore);
			int [] storeGradeKey = new int[storeGrade.Length];
			for (int sg = 0; sg < storeGrade.Length; sg++)
			{
				sgp = (StoreGradeProfile) aGradeProfileList.ArrayList[storeGrade[sg]];
				storeGradeKey[sg] = sgp.Key;
			}
			return storeGradeKey;
		}

		/// <summary>
		/// Calculates the grade index (index into the GradeBoundary array) for each store based on the input from GradeStoreBin array.
		/// Allows for average store value to be sent in.
		/// </summary>
		/// <param name="aGradeBoundary">Grade Boundaries.</param>
		/// <param name="aGradeStoreBin">GradeStoreBin containing the units by store on which to base the calculation.</param>
		/// <returns>Array of grade indices associated with and in the same order as the GradeStoreBin array.</returns>
		public static int [] GetGradeIndex (double[] aGradeBoundary, GradeStoreBin[] aGradeStoreBin, double averageStore)
		{
			if (aGradeBoundary == null)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_NoGradeDefinition,     
					MIDText.GetText(eMIDTextCode.msg_NoGradeDefinition));
			}
			MIDGenericSortItem[] gse = new MIDGenericSortItem[aGradeBoundary.Length];
			for (int i = 0; i < aGradeBoundary.Length; i++)
			{
				gse[i].Item = i;
				gse[i].SortKey = new double[1] ;
				gse[i].SortKey[0] = aGradeBoundary[i];
			}
			Array.Sort(gse,new MIDGenericSortDescendingComparer());
			int [] storeGrade = new int[aGradeStoreBin.Length];
			for (int i = 0; i < aGradeStoreBin.Length; i++)
			{
				double percentToAverage = 0.0;
				if (aGradeStoreBin[i].StoreEligible == true)
				{
					if (averageStore > 0)
					{
						percentToAverage = (aGradeStoreBin[i].StoreGradeUnits * 100) / averageStore;
						for (storeGrade[i] = 0;
							(storeGrade[i] < gse.Length && percentToAverage < aGradeBoundary[gse[storeGrade[i]].Item]);
							storeGrade[i]+=1);
						if (storeGrade[i] == gse.Length)
						{
							storeGrade[i] = gse.Length - 1;
						}
					}
					else
					{
						storeGrade[i] = gse.Length - 1;
					}
				}
				else
				{
					// BEGIN MID Track #2539 Grades not same
					//					storeGrade[i] = 0;
					storeGrade[i] = gse.Length - 1;
					// END MID Track #2539
				}
			}
			return storeGrade;
		}
		// END issue 4288 - stodd 2.14.2007

		public static int GetGradeProfileKey(StoreGradeList aGradeProfileList, double aStoreSales, double aAverageStore)
		{
			double percentToAverage = 0.0;
			int storeGrade;

			if (aAverageStore > 0)
			{
				percentToAverage = (aStoreSales * 100) / aAverageStore;
			}
			else
			{
				percentToAverage = 0;
			}

			for (storeGrade = 0;
				(storeGrade < aGradeProfileList.Count && percentToAverage < ((StoreGradeProfile)aGradeProfileList[storeGrade]).Boundary);
				storeGrade++);
			if (storeGrade == aGradeProfileList.Count)
			{
				storeGrade = aGradeProfileList.Count - 1;
			}

			return aGradeProfileList[storeGrade].Key;
		}
	}
}
