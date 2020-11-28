using System;
using System.Collections;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains cross-reference hash tables that relate detail Profile Ids to total Profile Ids.
	/// </summary>
	/// <remarks>
	/// This class supports the ability for auto-totaling during computations.  It is composed of two HashTables, one for the detail Profile and one for
	/// the total Profile.  Each entry in the HashTable is an ArrayList of total Profiles for the detail HashTable, and detail Profiles for the total
	/// HashTable.  This allows the system to determine which detail Profiles are related to a total Profile, or which total Profiles are related to
	/// a detail Profile, with a single HashTable access.
	/// </remarks>

	[Serializable]
	public class DateProfileXRef : ProfileXRef
	{
		//=======
		// FIELDS
		//=======

		private MRSCalendar _calendar;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ProfileXRef using the given total eProfileType and detail eProfileType.
		/// </summary>
		/// <param name="aTotalType">
		/// The eProfileType of the total Profile.
		/// </param>
		/// <param name="aDetailType">
		/// The eProfileType of the detail Profile.
		/// </param>

		public DateProfileXRef(MRSCalendar aCalendar)
			: base(eProfileType.Period, eProfileType.Week)
		{
			_calendar = aCalendar;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Returns the list of detail Ids for a given total Id.
		/// </summary>
		/// <param name="aTotalId">
		/// The total Id to get a detail list for.
		/// </param>
		/// <returns>
		/// The list of detail Ids.
		/// </returns>

		override public ArrayList GetDetailList(int aTotalId)
		{
			try
			{
				intGetCalendarInfo(aTotalId, -1);

				return base.GetDetailList(aTotalId);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the list of total Ids for a given detail Id.
		/// </summary>
		/// <param name="aDetailId">
		/// The detail Id to get a total list for.
		/// </param>
		/// <returns>
		/// the list of total Ids.
		/// </returns>

		override public ArrayList GetTotalList(int aDetailId)
		{
			try
			{
				intGetCalendarInfo(-1, aDetailId);

				return base.GetTotalList(aDetailId);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the first detail Id for a given total Id.
		/// </summary>
		/// <param name="aTotalId">
		/// The total Id to get the first detail for.
		/// </param>
		/// <returns>
		/// The first detail Id.
		/// </returns>

		override public ArrayList GetDetailFirst(int aTotalId)
		{
			intGetCalendarInfo(aTotalId, -1);

			return base.GetDetailFirst(aTotalId);
		}

		/// <summary>
		/// Returns the total Id for a given first detail Id.
		/// </summary>
		/// <param name="aDetailId">
		/// The first detail Id to get the total for.
		/// </param>
		/// <returns>
		/// The total Id.
		/// </returns>

		override public ArrayList GetTotalFirst(int aDetailId)
		{
			intGetCalendarInfo(-1, aDetailId);

			return base.GetTotalFirst(aDetailId);
		}

		/// <summary>
		/// Returns the last detail Id for a given total Id.
		/// </summary>
		/// <param name="aTotalId">
		/// The total Id to get the last detail for.
		/// </param>
		/// <returns>
		/// The last detail Id.
		/// </returns>

		override public ArrayList GetDetailNext(int aTotalId)
		{
			intGetCalendarInfo(aTotalId, -1);

			return base.GetDetailNext (aTotalId);
		}

		/// <summary>
		/// Returns the total Id for a given last detail Id.
		/// </summary>
		/// <param name="aDetailId">
		/// The lasg detail Id to get the total for.
		/// </param>
		/// <returns>
		/// The total Id.
		/// </returns>

		override public ArrayList GetTotalNext(int aDetailId)
		{
			intGetCalendarInfo(-1, aDetailId);

			return base.GetTotalNext (aDetailId);
		}

		//Begin Track #5840 -- Certain Monthly variables that use Last week show zero
		/// <summary>
		/// Returns the last detail Id for a given total Id.
		/// </summary>
		/// <param name="aTotalId">
		/// The total Id to get the last detail for.
		/// </param>
		/// <returns>
		/// The last detail Id.
		/// </returns>

		override public ArrayList GetDetailLast(int aTotalId)
		{
			intGetCalendarInfo(aTotalId, -1);

			return base.GetDetailLast(aTotalId);
		}

		/// <summary>
		/// Returns the total Id for a given last detail Id.
		/// </summary>
		/// <param name="aDetailId">
		/// The last detail Id to get the total for.
		/// </param>
		/// <returns>
		/// The total Id.
		/// </returns>

		override public ArrayList GetTotalLast(int aDetailId)
		{
			intGetCalendarInfo(-1, aDetailId);

			return base.GetTotalLast(aDetailId);
		}

		//End Track #5840 -- Certain Monthly variables that use Last week show zero
		/// <summary>
		/// Builds the total/detail cross-reference using the given total and detail Ids.
		/// </summary>
		/// <remarks>
		/// An entry is added to the ArrayList of both total and detail HashTables at the location of the Profile Ids specified by the given total
		/// and detail Ids.  The ArrayLists in the HashTables are created upon first usage.
		/// </remarks>
		/// <param name="aTotalId">
		/// The total Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aDetailId">
		/// The detail Profile Id to add a cross reference for.
		/// </param>

		override public void AddXRefIdEntry(int aTotalId, int aDetailId)
		{
			throw new MIDException (eErrorLevel.severe,
				(int)eMIDTextCode.msg_pl_InvalidCall,
				MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
		}

		/// <summary>
		/// Builds the total/detail cross-reference using the given total Id and list of detail Ids.
		/// </summary>
		/// <remarks>
		/// This routine loops through all detail Ids on the given list and calls AddXRefIdEntry with each total/detail pair.
		/// </remarks>
		/// <param name="aTotalId">
		/// The total Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aDetailList">
		/// The list of detail Profile Ids to add a cross reference for.
		/// </param>

		override public void AddXRefIdEntry(int aTotalId, ArrayList aDetailList)
		{
			throw new MIDException (eErrorLevel.severe,
				(int)eMIDTextCode.msg_pl_InvalidCall,
				MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
		}

		/// <summary>
		/// Builds the total/detail cross-reference using the given total Profile and list of detail Profiles.
		/// </summary>
		/// <remarks>
		/// This routine loops through all detail Profiles on the given list and calls AddXRefIdEntry with the keys of each total/detail pair.
		/// </remarks>
		/// <param name="aTotalProfile">
		/// The total Profile to add a cross reference for.
		/// </param>
		/// <param name="aDetailProfileList">
		/// The list of detail Profiles to add a cross reference for.
		/// </param>

		override public void AddXRefIdEntry(Profile aTotalProfile, ProfileList aDetailProfileList)
		{
			throw new MIDException (eErrorLevel.severe,
				(int)eMIDTextCode.msg_pl_InvalidCall,
				MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
		}

		/// <summary>
		/// Builds the total/detail cross-reference using the given detail Id and list of total Ids.
		/// </summary>
		/// <remarks>
		/// This routine loops through all total Ids on the given list and calls AddXRefIdEntry with each total/detail pair.
		/// </remarks>
		/// <param name="aTotalList">
		/// The list of total Profile Ids to add a cross reference for.
		/// </param>
		/// <param name="aDetailId">
		/// The detail Profile Id to add a cross reference for.
		/// </param>

		override public void AddXRefIdEntry(ArrayList aTotalList, int aDetailId)
		{
			throw new MIDException (eErrorLevel.severe,
				(int)eMIDTextCode.msg_pl_InvalidCall,
				MIDText.GetText(eMIDTextCode.msg_pl_InvalidCall));
		}

		private void intGetCalendarInfo(int aTotalId, int aDetailId)
		{
			PeriodProfile periodProf;

			try
			{
				periodProf = null;

				if (aDetailId != -1 && !DetailAllIdHash.Contains(aDetailId))
				{
					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
					//periodProf = _calendar.GetPeriodByKey(_calendar.GetWeek(aDetailId).Period.Key);
					periodProf = _calendar.GetPeriod(_calendar.GetWeek(aDetailId).Period.Key);
					//End Track #5121 - JScott - Add Year/Season/Quarter totals
				}
				else if (aTotalId != -1 && !TotalAllIdHash.Contains(aTotalId))
				{
					//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
					//periodProf = _calendar.GetPeriodByKey(aTotalId);
					periodProf = _calendar.GetPeriod(aTotalId);
					//End Track #5121 - JScott - Add Year/Season/Quarter totals
				}

				if (periodProf != null)
				{
					foreach (WeekProfile weekProf in periodProf.Weeks)
					{
						base.AddXRefIdEntry(periodProf.Key, weekProf.Key);
					}
					base.SetNextXRefIdEntry(periodProf.Key, _calendar.AddWeeks(periodProf.Weeks[periodProf.Weeks.Count - 1].Key, 1));
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
