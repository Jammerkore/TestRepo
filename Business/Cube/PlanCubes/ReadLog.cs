using System;
using System.Collections;
using System.Globalization;	
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// This class is used to store information about a plan that has been read into the Cube.
	/// </summary>

	abstract public class ReadLogKey
	{
	}

	/// <summary>
	/// This class is used to store information about a plan that has been read into the Cube.
	/// </summary>

	public class PlanReadLogKey : ReadLogKey
	{
		//=======
		// FIELDS
		//=======

		private int _versionKey;
		private int _hierarchyNodeKey;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanReadLogEntry using the given verion key, hierarchy node key, and week key.
		/// </summary>
		/// <param name="aVersionKey">
		/// The version key of the plan read.
		/// </param>
		/// <param name="aHierarchyNodeKey">
		/// The hierarchy node key of the plan read.
		/// </param>

		public PlanReadLogKey(int aVersionKey, int aHierarchyNodeKey)
		{
			_versionKey = aVersionKey;
			_hierarchyNodeKey = aHierarchyNodeKey;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Returns the hashcode of this object.
		/// </summary>
		/// <returns>
		/// The hashcode for this object.
		/// </returns>

		override public int GetHashCode()
		{
			return (int)(_versionKey & 0xFF) | ((_hierarchyNodeKey & 0xFF) << 8);
		}

		/// <summary>
		/// Returns a boolean indicating if the given object is equal to this object.
		/// </summary>
		/// <param name="obj">
		/// The object to compare.
		/// </param>
		/// <returns>
		/// A boolean indicating if the two objects are equal.
		/// </returns>

		override public bool Equals(object obj)
		{
			return ((PlanReadLogKey)obj)._versionKey == _versionKey &&
				((PlanReadLogKey)obj)._hierarchyNodeKey == _hierarchyNodeKey;
		}
	}

	/// <summary>
	/// This class is used to store information about a basis that has been read into the Cube.
	/// </summary>

	public class BasisReadLogKey : ReadLogKey
	{
		//=======
		// FIELDS
		//=======

		private int _versionKey;
		private int _hierarchyNodeKey;
		private int _basisKey;
		private int _basisDetailKey;
		private int _basisDetailHierarchyNodeKey;
		private int _basisDetailVersionKey;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of BasisReadLogEntry using the given verion key, hierarchy node key, and week key.
		/// </summary>
		/// <param name="aVersionKey">
		/// The version key of the basis read.
		/// </param>
		/// <param name="aHierarchyNodeKey">
		/// The hierarchy node key of the basis read.
		/// </param>

		public BasisReadLogKey(int aVersionKey, int aHierarchyNodeKey, int aBasisKey, int aBasisDetailKey,
			int aBasisDetailHierarchyNodeKey, int aBasisDetailVersionKey)
		{
			_versionKey = aVersionKey;
			_hierarchyNodeKey = aHierarchyNodeKey;
			_basisKey = aBasisKey;
			_basisDetailKey = aBasisDetailKey;
			_basisDetailHierarchyNodeKey = aBasisDetailHierarchyNodeKey;
			_basisDetailVersionKey = aBasisDetailVersionKey;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Returns the hashcode of this object.
		/// </summary>
		/// <returns>
		/// The hashcode for this object.
		/// </returns>

		override public int GetHashCode()
		{
			return Include.CreateHashKey(_versionKey, _hierarchyNodeKey, _basisKey, _basisDetailKey);
		}

		/// <summary>
		/// Returns a boolean indicating if the given object is equal to this object.
		/// </summary>
		/// <param name="obj">
		/// The object to compare.
		/// </param>
		/// <returns>
		/// A boolean indicating if the two objects are equal.
		/// </returns>

		override public bool Equals(object obj)
		{
			return ((BasisReadLogKey)obj)._versionKey == _versionKey &&
				((BasisReadLogKey)obj)._hierarchyNodeKey == _hierarchyNodeKey &&
				((BasisReadLogKey)obj)._basisKey == _basisKey &&
				((BasisReadLogKey)obj)._basisDetailKey == _basisDetailKey &&
				((BasisReadLogKey)obj)._basisDetailHierarchyNodeKey == _basisDetailHierarchyNodeKey &&
				((BasisReadLogKey)obj)._basisDetailVersionKey == _basisDetailVersionKey;
		}
	}

	/// <summary>
	/// This class is used to store information about a plan that has been read into the Cube.
	/// </summary>

	public class ReadLogValue
	{
		//=======
		// FIELDS
		//=======

		private Hashtable _weekHash;
		private SortedList _weekList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ReadLogEntry using the given verion key, hierarchy node key, and week key.
		/// </summary>

		public ReadLogValue()
		{
			_weekHash = new Hashtable();
			_weekList = new SortedList();
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Adds a set of keys describing a read plan to the hashtable.
		/// </summary>
		/// <param name="aWeekKey">
		/// The WeekProfile key that describes the Week of the plan.
		/// </param>

		public void AddWeek(int aWeekKey)
		{
			try
			{
				_weekHash.Add(aWeekKey, aWeekKey);
				_weekList.Add(aWeekKey, aWeekKey);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds a set of keys describing a read plan to the hashtable.
		/// </summary>
		/// <param name="aWeekProfileList">
		/// A ProfileList of WeekProfiles to add.
		/// </param>

		public void AddWeeks(ProfileList aWeekProfileList)
		{
			try
			{
				foreach (WeekProfile weekProfile in aWeekProfileList)
				{
					_weekHash.Add(weekProfile.Key, weekProfile.Key);
					_weekList.Add(weekProfile.Key, weekProfile.Key);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating whether the plan described by the given set of keys exists on the hashtable and has been read.
		/// </summary>
		/// <param name="aWeekKey">
		/// The WeekProfile key that describes the Week of the plan.
		/// </param>
		/// <returns>
		/// A boolean indicating if the plan has been read.
		/// </returns>
		
		public bool ContainsWeek(int aWeekKey)
		{
			try
			{
				return _weekHash.Contains(aWeekKey);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a ProfileList containing WeekProfiles that have not been read for the plan identified by the given Version and HierarchyNode keys.
		/// </summary>
		/// <param name="aWeekProfileList">
		/// A ProfileList of WeekProfiles that describe the weeks to inspect.
		/// </param>
		/// <returns>
		/// A ProfileList of WeekProfiles that have not yet been read.
		/// </returns>

		public ProfileList DetermineWeeksToRead(ProfileList aWeekProfileList)
		{
			ProfileList newWeekList;

			try
			{
				newWeekList = new ProfileList(eProfileType.Week);

				foreach (WeekProfile weekProfile in aWeekProfileList)
				{
					if (!_weekHash.Contains(weekProfile.Key))
					{
						newWeekList.Add(weekProfile);
					}
				}

				return newWeekList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Gets the starting and ending week keys for an expansion read.
		/// </summary>
		/// <param name="aWeekKey">
		/// The WeekProfile key that describes the Week of the plan.
		/// </param>
		/// <param name="aStartWeekKey">
		/// The output start week key.
		/// </param>
		/// <param name="aEndWeekKey">
		/// The output end week key.
		/// </param>

		public void GetStartEndWeeks(int aWeekKey, out int aStartWeekKey, out int aEndWeekKey)
		{
			try
			{
				if (aWeekKey < (int)_weekList.GetByIndex(0))
				{
					aStartWeekKey = -1;
					aEndWeekKey = aWeekKey;
				}
				else
				{
					aStartWeekKey = aWeekKey;
					aEndWeekKey = -1;
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
	/// This class is used to store information about plans that have been read into the Cube.
	/// </summary>

	public class ReadLog
	{
		//=======
		// FIELDS
		//=======

		private System.Collections.Hashtable _readLogHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ReadLog.
		/// </summary>

		public ReadLog()
		{
			try
			{
				_readLogHash = new System.Collections.Hashtable();
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
		/// Adds a set of keys describing a read plan to the hashtable.
		/// </summary>
		/// <param name="aReadLogKey">
		/// The ReadLogKey key that describes the plan.
		/// </param>
		/// <param name="aWeekKey">
		/// The WeekProfile key that describes the Week of the plan.
		/// </param>

		public void Add(ReadLogKey aReadLogKey, int aWeekKey)
		{
			ReadLogValue readLogValue;

			try
			{
				readLogValue = (ReadLogValue)_readLogHash[aReadLogKey];

				if (readLogValue == null)
				{
					readLogValue = new ReadLogValue();
					_readLogHash.Add(aReadLogKey, readLogValue);
				}

				readLogValue.AddWeek(aWeekKey);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void Clear()
		{
			try
			{
				_readLogHash.Clear();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds sets of keys describing a read plan to the hashtable.  Multiple weeks are supplied as a ProfileList of WeekProfiles.
		/// </summary>
		/// <param name="aReadLogKey">
		/// The ReadLogKey key that describes the plan.
		/// </param>
		/// <param name="aWeekProfileList">
		/// A ProfileList of WeekProfiles that describe the weeks to read.
		/// </param>

		public void Add(ReadLogKey aReadLogKey, ProfileList aWeekProfileList)
		{
			ReadLogValue readLogValue;

			try
			{
				readLogValue = (ReadLogValue)_readLogHash[aReadLogKey];

				if (readLogValue == null)
				{
					readLogValue = new ReadLogValue();
					_readLogHash.Add(aReadLogKey, readLogValue);
				}

				readLogValue.AddWeeks(aWeekProfileList);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean indicating whether the plan described by the given set of keys exists on the hashtable and has been read.
		/// </summary>
		/// <param name="aReadLogKey">
		/// The ReadLogKey key that describes the plan.
		/// </param>
		/// <param name="aWeekKey">
		/// The WeekProfile key that describes the Week of the plan.
		/// </param>
		/// <returns>
		/// A boolean indicating if the plan has been read.
		/// </returns>
		
		public bool Contains(ReadLogKey aReadLogKey, int aWeekKey)
		{
			ReadLogValue readLogValue;

			try
			{
				readLogValue = (ReadLogValue)_readLogHash[aReadLogKey];

				if (readLogValue == null)
				{
					return false;
				}
				else
				{
					return readLogValue.ContainsWeek(aWeekKey);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a ProfileList containing WeekProfiles that have not been read for the plan identified by the given Version and HierarchyNode keys.
		/// </summary>
		/// <param name="aReadLogKey">
		/// The ReadLogKey key that describes the plan.
		/// </param>
		/// <param name="aWeekProfileList">
		/// A ProfileList of WeekProfiles that describe the weeks to inspect.
		/// </param>
		/// <returns>
		/// A ProfileList of WeekProfiles that have not yet been read.
		/// </returns>

		public ProfileList DetermineWeeksToRead(SessionAddressBlock aSAB, ReadLogKey aReadLogKey, ProfileList aWeekProfileList, int aPreWeeks, int aPostWeeks)
		{
			ProfileList weekProfList;
			SortedList sortList;
			int i;
			int week;
			ReadLogValue readLogValue;

			try
			{
				weekProfList = (ProfileList)aWeekProfileList.Clone();
				sortList = new SortedList();

				foreach (WeekProfile weekProf in weekProfList)
				{
					sortList.Add(weekProf.Key, weekProf);
				}

				for (i = 0,	week = aSAB.ApplicationServerSession.Calendar.AddWeeks(((WeekProfile)sortList.GetByIndex(0)).Key, -1);
					i < aPreWeeks;
					i++, week = aSAB.ApplicationServerSession.Calendar.AddWeeks(week, -1))
				{
					weekProfList.Add(aSAB.ApplicationServerSession.Calendar.GetWeek(week));
				}

				for (i = 0, week = aSAB.ApplicationServerSession.Calendar.AddWeeks(((WeekProfile)sortList.GetByIndex(sortList.Count - 1)).Key, 1);
					i < aPostWeeks;
					i++, week = aSAB.ApplicationServerSession.Calendar.AddWeeks(week, 1))
				{
					weekProfList.Add(aSAB.ApplicationServerSession.Calendar.GetWeek(week));
				}

				readLogValue = (ReadLogValue)_readLogHash[aReadLogKey];

				if (readLogValue == null)
				{
					return (ProfileList)weekProfList.Clone();
				}
				else
				{
					return readLogValue.DetermineWeeksToRead(weekProfList);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a ProfileList containing WeekProfiles that have not been read for the plan identified by the given Version and HierarchyNode keys.
		/// </summary>
		/// <param name="aReadLogKey">
		/// The ReadLogKey key that describes the plan.
		/// </param>
		/// <param name="aWeekKey">
		/// The WeekProfile key that describes the Week of the plan.
		/// </param>

		public ProfileList DetermineWeeksToRead(SessionAddressBlock aSAB, ReadLogKey aReadLogKey, int aWeekKey)
		{
			ReadLogValue readLogValue;
			ProfileList newWeekList;
			int startWeek;
			int endWeek;
			int weeksFound;
			int week;
			int i;

			try
			{
				readLogValue = (ReadLogValue)_readLogHash[aReadLogKey];

				if (readLogValue == null)
				{
					startWeek = aWeekKey;
					endWeek = -1;
				}
				else
				{
					readLogValue.GetStartEndWeeks(aWeekKey, out startWeek, out endWeek);
				}

				weeksFound = 0;
				newWeekList = new ProfileList(eProfileType.Week);

				if (endWeek < 0)
				{
					for (i = 0, week = startWeek;
						i < Include.PlanReadExpansionSize;
						i++, week = aSAB.ApplicationServerSession.Calendar.AddWeeks(week, 1))
					{
						if (readLogValue == null || !readLogValue.ContainsWeek(week))
						{
							newWeekList.Add(aSAB.ApplicationServerSession.Calendar.GetWeek(week));
							weeksFound++;
						}
					}

					if (weeksFound < Include.PlanReadExpansionSize)
					{
						for (i = 0, week = aSAB.ApplicationServerSession.Calendar.AddWeeks(startWeek, -1);
							i < Include.PlanReadExpansionSize && weeksFound < Include.PlanReadExpansionSize;
							i++, week = aSAB.ApplicationServerSession.Calendar.AddWeeks(week, -1))
						{
							if (readLogValue == null || !readLogValue.ContainsWeek(week))
							{
								newWeekList.Add(aSAB.ApplicationServerSession.Calendar.GetWeek(week));
								weeksFound++;
							}
						}
					}
				}
				else
				{
					if (weeksFound < Include.PlanReadExpansionSize)
					{
						for (i = 0, week = endWeek;
							i < Include.PlanReadExpansionSize;
							i++, week = aSAB.ApplicationServerSession.Calendar.AddWeeks(week, -1))
						{
							if (readLogValue == null || !readLogValue.ContainsWeek(week))
							{
								newWeekList.Add(aSAB.ApplicationServerSession.Calendar.GetWeek(week));
								weeksFound++;
							}
						}
					}

					for (i = 0, week = aSAB.ApplicationServerSession.Calendar.AddWeeks(endWeek, 1);
						i < Include.PlanReadExpansionSize && weeksFound < Include.PlanReadExpansionSize;
						i++, week = aSAB.ApplicationServerSession.Calendar.AddWeeks(week, 1))
					{
						if (readLogValue == null || !readLogValue.ContainsWeek(week))
						{
							newWeekList.Add(aSAB.ApplicationServerSession.Calendar.GetWeek(week));
							weeksFound++;
						}
					}
				}

				return newWeekList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//Begin Track #6427 - JSCott - Urgent - In Prod BOTB and Vendor OTB, Copy Low to High is not working correctly

		public void ClearReadLogForKey(ReadLogKey aReadLogKey)
		{
			try
			{
				if (_readLogHash.Contains(aReadLogKey))
				{
					_readLogHash.Remove(aReadLogKey);
				}
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
