using System;
using System.Collections;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Base class that contains cross-reference hash tables that relate detail Profile Ids to total Profile Ids.
	/// </summary>
	/// <remarks>
	/// This class supports the ability for auto-totaling during computations.  It is composed of two HashTables, one for the detail Profile and one for
	/// the total Profile.  Each entry in the HashTable is an ArrayList of total Profiles for the detail HashTable, and detail Profiles for the total
	/// HashTable.  This allows the system to determine which detail Profiles are related to a total Profile, or which total Profiles are related to
	/// a detail Profile, with a single HashTable access.
	/// </remarks>

	[Serializable]
	abstract public class BaseProfileXRef
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ProfileXRef using the given total eProfileType and detail eProfileType.
		/// </summary>
		
		public BaseProfileXRef()
		{
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// Defines a simple cross reference between a single total type and detail type.
	/// </summary>
	/// <remarks>
	/// This class supports the ability for auto-totaling during computations.  It is composed of two HashTables, one for the detail Profile and one for
	/// the total Profile.  Each entry in the HashTable is an ArrayList of total Profiles for the detail HashTable, and detail Profiles for the total
	/// HashTable.  This allows the system to determine which detail Profiles are related to a total Profile, or which total Profiles are related to
	/// a detail Profile, with a single HashTable access.
	/// </remarks>

	[Serializable]
	public class ProfileXRef : BaseProfileXRef
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _totalType;
		private eProfileType _detailType;
		protected Hashtable _totalAllIdHash;
		protected Hashtable _totalAllPlusNextIdHash;
		protected Hashtable _totalFirstIdHash;
		protected Hashtable _totalLastIdHash;
		protected Hashtable _totalFirstAndLastIdHash;
		protected Hashtable _totalNextIdHash;
		protected Hashtable _detailAllIdHash;
		protected Hashtable _detailAllPlusNextIdHash;
		protected Hashtable _detailFirstIdHash;
		protected Hashtable _detailLastIdHash;
		protected Hashtable _detailFirstAndLastIdHash;
		protected Hashtable _detailNextIdHash;
		protected bool _detailInited;
       
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

		public ProfileXRef(eProfileType aTotalType, eProfileType aDetailType)
			: base()
		{
			try
			{
				_totalType = aTotalType;
				_detailType = aDetailType;
				_totalAllIdHash = new Hashtable();
				_totalAllPlusNextIdHash = new Hashtable();
				_totalFirstIdHash = new Hashtable();
				_totalLastIdHash = new Hashtable();
				_totalFirstAndLastIdHash = new Hashtable();
				_totalNextIdHash = new Hashtable();
				_detailAllIdHash = new Hashtable();
				_detailAllPlusNextIdHash = new Hashtable();
				_detailFirstIdHash = null;
				_detailLastIdHash = null;
				_detailFirstAndLastIdHash = null;
				_detailNextIdHash = null;
				_detailInited = false;
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

		/// <summary>
		/// Gets the eProfileType of the total Profile.
		/// </summary>

		public eProfileType TotalType
		{
			get
			{
				return _totalType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the detail Profile.
		/// </summary>

		public eProfileType DetailType
		{
			get
			{
				return _detailType;
			}
		}

		/// <summary>
		/// Gets the Hashtable for the Total-to-all hash.
		/// </summary>

		protected Hashtable TotalAllIdHash
		{
			get
			{
				return _totalAllIdHash;
			}
		}

		/// <summary>
		/// Gets the Hashtable for the Detail-to-all hash.
		/// </summary>

		protected Hashtable DetailAllIdHash
		{
			get
			{
				return _detailAllIdHash;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Override.  Returns the hashcode for this object.
		/// </summary>
		/// <remarks>
		/// Calulation of this hashcode is accomplished by multiplying the total type by 100 and then adding the detail type
		/// </remarks>
		/// <returns>
		/// The hashcode for this object.
		/// </returns>

		override public int GetHashCode()
		{
			int hashCode;

			hashCode = ((int)_totalType * 100) + (int)_detailType;

			return hashCode;
		}

		/// <summary>
		/// Returns a value indicating whether an instance of ProfileXRef is equal to a specified object.
		/// </summary>
		/// <param name="obj">
		/// An object to compare with this instance.
		/// </param>
		/// <returns>
		/// <I>true</I> if value is an instance of ProfileXRef and equals the value of this instance; otherwise,
		/// <I>false</I>.
		/// </returns>

		override public bool Equals(object obj)
		{
			return ((obj.GetType() == typeof(ProfileXRef) || obj.GetType().IsSubclassOf(typeof(ProfileXRef))) &&
				_totalType == ((ProfileXRef)obj)._totalType &&
				_detailType == ((ProfileXRef)obj)._detailType);
		}

		/// <summary>
		/// Returns the list of detail Ids for a given total Id.
		/// </summary>
		/// <param name="aTotalId">
		/// The total Id to get a detail list for.
		/// </param>
		/// <returns>
		/// The list of detail Ids.
		/// </returns>

		virtual public ArrayList GetDetailList(int aTotalId)
		{
			try
			{
				return (ArrayList)_totalAllIdHash[aTotalId];
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

		virtual public ArrayList GetTotalList(int aDetailId)
		{
			try
			{
				return (ArrayList)_detailAllIdHash[aDetailId];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the list of detail Ids for a given total Id.
		/// </summary>
		/// <param name="aTotalId">
		/// The total Id to get a detail list for.
		/// </param>
		/// <returns>
		/// The list of detail Ids.
		/// </returns>

		virtual public ArrayList GetDetailPlusNextList(int aTotalId)
		{
			try
			{
				return (ArrayList)_totalAllPlusNextIdHash[aTotalId];
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

		virtual public ArrayList GetTotalPlusNextList(int aDetailId)
		{
			try
			{
				return (ArrayList)_detailAllPlusNextIdHash[aDetailId];
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

		virtual public ArrayList GetDetailFirst(int aTotalId)
		{
			try
			{
				return (ArrayList)_totalFirstIdHash[aTotalId];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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

		virtual public ArrayList GetTotalFirst(int aDetailId)
		{
			try
			{
				if (!_detailInited)
				{
					intInitDetail();
				}

				return (ArrayList)_detailFirstIdHash[aDetailId];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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

		virtual public ArrayList GetDetailLast(int aTotalId)
		{
			try
			{
				return (ArrayList)_totalLastIdHash[aTotalId];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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

		virtual public ArrayList GetTotalLast(int aDetailId)
		{
			try
			{
				if (!_detailInited)
				{
					intInitDetail();
				}

				return (ArrayList)_detailLastIdHash[aDetailId];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the list of detail Ids for a given total Id.
		/// </summary>
		/// <param name="aTotalId">
		/// The total Id to get a detail list for.
		/// </param>
		/// <returns>
		/// The list of detail Ids.
		/// </returns>

		virtual public ArrayList GetDetailFirstLastList(int aTotalId)
		{
			ArrayList arrList;

			try
			{
				arrList = (ArrayList)_totalFirstAndLastIdHash[aTotalId];

				if (arrList == null)
				{
					arrList = new ArrayList();
					arrList.AddRange((ArrayList)_totalFirstIdHash[aTotalId]);
					arrList.AddRange((ArrayList)_totalLastIdHash[aTotalId]);
					_totalFirstAndLastIdHash.Add(aTotalId, arrList);
				}

				return arrList;
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

		virtual public ArrayList GetTotalFirstLastList(int aDetailId)
		{
			try
			{
				if (!_detailInited)
				{
					intInitDetail();
				}

				return (ArrayList)_detailFirstAndLastIdHash[aDetailId];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the next detail Id for a given total Id.
		/// </summary>
		/// <param name="aTotalId">
		/// The total Id to get the next detail for.
		/// </param>
		/// <returns>
		/// The next detail Id.
		/// </returns>

		virtual public ArrayList GetDetailNext(int aTotalId)
		{
			try
			{
				return (ArrayList)_totalNextIdHash[aTotalId];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the total Id for a given next detail Id.
		/// </summary>
		/// <param name="aDetailId">
		/// The lasg detail Id to get the total for.
		/// </param>
		/// <returns>
		/// The total Id.
		/// </returns>

		virtual public ArrayList GetTotalNext(int aDetailId)
		{
			try
			{
				if (!_detailInited)
				{
					intInitDetail();
				}

				return (ArrayList)_detailNextIdHash[aDetailId];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

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

		virtual public void AddXRefIdEntry(int aTotalId, int aDetailId)
		{
			ArrayList arrList;

			try
			{
				// Update Detail list

				arrList = (ArrayList)_detailAllIdHash[aDetailId];

				if (arrList == null)
				{
					arrList = new ArrayList();
					arrList.Add(aTotalId);
					_detailAllIdHash.Add(aDetailId, arrList);
				}
				else
				{
					if (!arrList.Contains(aTotalId))
					{
						arrList.Add(aTotalId);
					}
				}

				// Update Total list

				arrList = (ArrayList)_totalAllIdHash[aTotalId];

				if (arrList == null)
				{
					arrList = new ArrayList();
					arrList.Add(aDetailId);
					_totalAllIdHash.Add(aTotalId, arrList);
				}
				else
				{
					if (!arrList.Contains(aDetailId))
					{
						arrList.Add(aDetailId);
					}
				}

				// Update Detail PlusNext list

				arrList = (ArrayList)_detailAllPlusNextIdHash[aDetailId];

				if (arrList == null)
				{
					arrList = new ArrayList();
					arrList.Add(aTotalId);
					_detailAllPlusNextIdHash.Add(aDetailId, arrList);
				}
				else
				{
					if (!arrList.Contains(aTotalId))
					{
						arrList.Add(aTotalId);
					}
				}

				// Update Total PlusNext list

				arrList = (ArrayList)_totalAllPlusNextIdHash[aTotalId];

				if (arrList == null)
				{
					arrList = new ArrayList();
					arrList.Add(aDetailId);
					_totalAllPlusNextIdHash.Add(aTotalId, arrList);
				}
				else
				{
					if (!arrList.Contains(aDetailId))
					{
						arrList.Add(aDetailId);
					}
				}

				// Set First Total List

				if (!_totalFirstIdHash.Contains(aTotalId))
				{
					arrList = new ArrayList();
					arrList.Add(aDetailId);
					_totalFirstIdHash.Add(aTotalId, arrList);
				}

				// Set Last Total List

				arrList = (ArrayList)_totalLastIdHash[aTotalId];

				if (arrList == null)
				{
					arrList = new ArrayList();
					_totalLastIdHash.Add(aTotalId, arrList);
					arrList.Add(aDetailId);
				}
				else
				{
					arrList.Clear();
					arrList.Add(aDetailId);
				}

				_detailInited = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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

		virtual public void AddXRefIdEntry(int aTotalId, ArrayList aDetailList)
		{
			try
			{
				foreach (int detailId in aDetailList)
				{
					AddXRefIdEntry(aTotalId, detailId);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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

		virtual public void AddXRefIdEntry(Profile aTotalProfile, ProfileList aDetailProfileList)
		{
			try
			{
				foreach (Profile detailProfile in aDetailProfileList)
				{
					AddXRefIdEntry(aTotalProfile.Key, detailProfile.Key);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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

		virtual public void AddXRefIdEntry(ArrayList aTotalList, int aDetailId)
		{
			try
			{
				foreach (int totalId in aTotalList)
				{
					AddXRefIdEntry(totalId, aDetailId);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

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

		virtual public void SetNextXRefIdEntry(int aTotalId, int aDetailId)
		{
			ArrayList arrList;

			try
			{
				// Update Next list

				arrList = new ArrayList();
				arrList.Add(aDetailId);
				_totalNextIdHash.Add(aTotalId, arrList);

				// Update Total PlusNext list

				arrList = (ArrayList)_totalAllPlusNextIdHash[aTotalId];

				if (arrList == null)
				{
					arrList = new ArrayList();
					arrList.Add(aDetailId);
					_totalAllPlusNextIdHash.Add(aTotalId, arrList);
				}
				else
				{
					if (!arrList.Contains(aDetailId))
					{
						arrList.Add(aDetailId);
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
		/// Builds the total/detail cross-reference using the given total Id and list of detail Ids.
		/// </summary>
		/// <remarks>
		/// This routine loops through all detail Ids on the given list and calls SetNextXRefIdEntry with each total/detail pair.
		/// </remarks>
		/// <param name="aTotalId">
		/// The total Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aDetailList">
		/// The list of detail Profile Ids to add a cross reference for.
		/// </param>

		virtual public void SetNextXRefIdEntry(int aTotalId, ArrayList aDetailList)
		{
			try
			{
				foreach (int detailId in aDetailList)
				{
					SetNextXRefIdEntry(aTotalId, detailId);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds the total/detail cross-reference using the given total Profile and list of detail Profiles.
		/// </summary>
		/// <remarks>
		/// This routine loops through all detail Profiles on the given list and calls SetNextXRefIdEntry with the keys of each total/detail pair.
		/// </remarks>
		/// <param name="aTotalProfile">
		/// The total Profile to add a cross reference for.
		/// </param>
		/// <param name="aDetailProfileList">
		/// The list of detail Profiles to add a cross reference for.
		/// </param>

		virtual public void SetNextXRefIdEntry(Profile aTotalProfile, ProfileList aDetailProfileList)
		{
			try
			{
				foreach (Profile detailProfile in aDetailProfileList)
				{
					SetNextXRefIdEntry(aTotalProfile.Key, detailProfile.Key);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds the total/detail cross-reference using the given detail Id and list of total Ids.
		/// </summary>
		/// <remarks>
		/// This routine loops through all total Ids on the given list and calls SetNextXRefIdEntry with each total/detail pair.
		/// </remarks>
		/// <param name="aTotalList">
		/// The list of total Profile Ids to add a cross reference for.
		/// </param>
		/// <param name="aDetailId">
		/// The detail Profile Id to add a cross reference for.
		/// </param>

		virtual public void SetNextXRefIdEntry(ArrayList aTotalList, int aDetailId)
		{
			try
			{
				foreach (int totalId in aTotalList)
				{
					SetNextXRefIdEntry(totalId, aDetailId);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This routine initializes the detail tables.
		/// </summary>

		private void intInitDetail()
		{
			IDictionaryEnumerator dictEnum;
			ArrayList detailIdList;
			ArrayList detailList;

			try
			{
				_detailFirstIdHash = new Hashtable();
				_detailFirstAndLastIdHash = new Hashtable();
				dictEnum = _totalFirstIdHash.GetEnumerator();

				while (dictEnum.MoveNext())
				{
					detailIdList = (ArrayList)dictEnum.Value;

					foreach (int detailId in detailIdList)
					{
						detailList = (ArrayList)_detailFirstIdHash[detailId];

						if (detailList == null)
						{
							detailList = new ArrayList();
							_detailFirstIdHash.Add(detailId, detailList);
							detailList.Add(dictEnum.Key);
						}
						else
						{
							if (!detailList.Contains(dictEnum.Key))
							{
								detailList.Add(dictEnum.Key);
							}
						}

						detailList = (ArrayList)_detailFirstAndLastIdHash[detailId];

						if (detailList == null)
						{
							detailList = new ArrayList();
							_detailFirstAndLastIdHash.Add(detailId, detailList);
							detailList.Add(dictEnum.Key);
						}
						else
						{
							if (!detailList.Contains(dictEnum.Key))
							{
								detailList.Add(dictEnum.Key);
							}
						}
					}
				}
				
				_detailLastIdHash = new Hashtable();
				dictEnum = _totalLastIdHash.GetEnumerator();

				while (dictEnum.MoveNext())
				{
					detailIdList = (ArrayList)dictEnum.Value;

					foreach (int detailId in detailIdList)
					{
						detailList = (ArrayList)_detailLastIdHash[detailId];

						if (detailList == null)
						{
							detailList = new ArrayList();
							_detailLastIdHash.Add(detailId, detailList);
							detailList.Add(dictEnum.Key);
						}
						else
						{
							if (!detailList.Contains(dictEnum.Key))
							{
								detailList.Add(dictEnum.Key);
							}
						}

						detailList = (ArrayList)_detailFirstAndLastIdHash[detailId];

						if (detailList == null)
						{
							detailList = new ArrayList();
							_detailFirstAndLastIdHash.Add(detailId, detailList);
							detailList.Add(dictEnum.Key);
						}
						else
						{
							if (!detailList.Contains(dictEnum.Key))
							{
								detailList.Add(dictEnum.Key);
							}
						}
					}
				}

				_detailNextIdHash = new Hashtable();
				dictEnum = _totalNextIdHash.GetEnumerator();

				while (dictEnum.MoveNext())
				{
					detailIdList = (ArrayList)dictEnum.Value;

					foreach (int detailId in detailIdList)
					{
						// Update Detail Next list

						detailList = (ArrayList)_detailNextIdHash[detailId];

						if (detailList == null)
						{
							detailList = new ArrayList();
							detailList.Add(dictEnum.Key);
							_detailNextIdHash.Add(detailId, detailList);
						}
						else
						{
							if (!detailList.Contains(dictEnum.Key))
							{
								detailList.Add(dictEnum.Key);
							}
						}

						// Update Detail PlusNext list

						detailList = (ArrayList)_detailAllPlusNextIdHash[detailId];

						if (detailList == null)
						{
							detailList = new ArrayList();
							detailList.Add(dictEnum.Key);
							_detailAllPlusNextIdHash.Add(detailId, detailList);
						}
						else
						{
							if (!detailList.Contains(dictEnum.Key))
							{
								detailList.Add(dictEnum.Key);
							}
						}
					}
				}

				_detailInited = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Defines a complex cross reference between a single total type and two detail types.
	/// </summary>
	/// <remarks>
	/// This class supports the ability for auto-totaling during computations.  It is composed of two HashTables, one for the detail Profile and one for
	/// the total Profile.  Each entry in the HashTable is an ArrayList of total Profiles for the detail HashTable, and detail Profiles for the total
	/// HashTable.  This allows the system to determine which detail Profiles are related to a total Profile, or which total Profiles are related to
	/// a detail Profile, with a single HashTable access.
	/// </remarks>

	[Serializable]
	public class ComplexProfileXRef : BaseProfileXRef
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _totalType;
		private eProfileType _primaryDetailType;
		private eProfileType _secondaryDetailType;
		private Hashtable _totalAllIdHash;
		private Hashtable _detailAllIdHash;
       
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ProfileXRef using the given total eProfileType and detail eProfileType.
		/// </summary>
		/// <param name="aTotalType">
		/// The eProfileType of the total Profile.
		/// </param>
		/// <param name="aPrimaryDetailType">
		/// The eProfileType of the primary detail Profile.
		/// </param>
		/// <param name="aSecondaryDetailType">
		/// The eProfileType of the secondary detail Profile.
		/// </param>

		public ComplexProfileXRef(eProfileType aTotalType, eProfileType aPrimaryDetailType, eProfileType aSecondaryDetailType)
			: base()
		{
			try
			{
				_totalType = aTotalType;
				_primaryDetailType = aPrimaryDetailType;
				_secondaryDetailType = aSecondaryDetailType;
				_totalAllIdHash = new Hashtable();
				_detailAllIdHash = new Hashtable();
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

		/// <summary>
		/// Gets the eProfileType of the total Profile.
		/// </summary>

		virtual public eProfileType TotalType
		{
			get
			{
				return _totalType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the primary detail Profile.
		/// </summary>

		virtual public eProfileType PrimaryDetailType
		{
			get
			{
				return _primaryDetailType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the secondary detail Profile.
		/// </summary>

		virtual public eProfileType SecondaryDetailType
		{
			get
			{
				return _secondaryDetailType;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Override.  Returns the hashcode for this object.
		/// </summary>
		/// <remarks>
		/// Calulation of this hashcode is accomplished by multiplying the total type by 100 and then adding the detail type
		/// </remarks>
		/// <returns>
		/// The hashcode for this object.
		/// </returns>

		override public int GetHashCode()
		{
			return Include.CreateHashKey((int)_totalType, (int)_primaryDetailType, (int)_secondaryDetailType);
		}

		/// <summary>
		/// Returns a value indicating whether an instance of ProfileXRef is equal to a specified object.
		/// </summary>
		/// <param name="obj">
		/// An object to compare with this instance.
		/// </param>
		/// <returns>
		/// <I>true</I> if value is an instance of ProfileXRef and equals the value of this instance; otherwise,
		/// <I>false</I>.
		/// </returns>

		override public bool Equals(object obj)
		{
			return ((this.GetType() == obj.GetType() || obj.GetType().IsSubclassOf(this.GetType())) &&
				_totalType == ((ComplexProfileXRef)obj)._totalType &&
				_primaryDetailType == ((ComplexProfileXRef)obj)._primaryDetailType &&
				_secondaryDetailType == ((ComplexProfileXRef)obj)._secondaryDetailType);
		}

		/// <summary>
		/// Returns the list of detail Ids for a given total Id.
		/// </summary>
		/// <param name="aTotalId">
		/// The total Id to get a detail list for.
		/// </param>
		/// <returns>
		/// The list of detail Ids.
		/// </returns>

		virtual public ArrayList GetDetailList(int aTotalId)
		{
			ComplexProfileXRefTotalHashEntry totHashEntry;

			try
			{
				totHashEntry = (ComplexProfileXRefTotalHashEntry)_totalAllIdHash[aTotalId];

				if (totHashEntry != null)
				{
					return totHashEntry.DetailList;
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the list of detail Ids for a given total Id.
		/// </summary>
		/// <param name="aTotalId">
		/// The total Id to get a detail list for.
		/// </param>
		/// <returns>
		/// The list of detail Ids.
		/// </returns>

		virtual public ArrayList GetDetailList(int aTotalId, int aPrimaryDetailId)
		{
			ComplexProfileXRefTotalHashEntry totHashEntry;

			try
			{
				totHashEntry = (ComplexProfileXRefTotalHashEntry)_totalAllIdHash[aTotalId];

				if (totHashEntry != null)
				{
					return totHashEntry.DetailXRef.GetDetailList(aPrimaryDetailId);
				}
				else
				{
					return null;
				}
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
		/// <param name="aPrimaryDetailId">
		/// The primary detail Id to get a total list for.
		/// </param>
		/// <param name="aSecondaryDetailId">
		/// The secondary detail Id to get a total list for.
		/// </param>
		/// <returns>
		/// the list of total Ids.
		/// </returns>

		virtual public ArrayList GetTotalList(int aPrimaryDetailId, int aSecondaryDetailId)
		{
			ComplexProfileXRefDetailEntry detailEntry;

			try
			{
				detailEntry = new ComplexProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId);
				return (ArrayList)_detailAllIdHash[detailEntry];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

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
		/// <param name="aDetailEntry">
		/// The ComplexProfileXRefDetailEntry of the detail to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(int aTotalId, ComplexProfileXRefDetailEntry aDetailEntry)
		{
			ArrayList arrList;
			ComplexProfileXRefTotalHashEntry hashEntry;

			try
			{
				// Update Detail list

				arrList = (ArrayList)_detailAllIdHash[aDetailEntry];

				if (arrList == null)
				{
					arrList = new ArrayList();
					arrList.Add(aTotalId);
					_detailAllIdHash.Add(aDetailEntry, arrList);
				}
				else
				{
					if (!arrList.Contains(aTotalId))
					{
						arrList.Add(aTotalId);
					}
				}

				// Update Total list

				hashEntry = (ComplexProfileXRefTotalHashEntry)_totalAllIdHash[aTotalId];

				if (hashEntry == null)
				{
					hashEntry = new ComplexProfileXRefTotalHashEntry(_primaryDetailType, _secondaryDetailType);
					hashEntry.Add(aDetailEntry);
					_totalAllIdHash.Add(aTotalId, hashEntry);
				}
				else
				{
					if (!hashEntry.Contains(aDetailEntry))
					{
						hashEntry.Add(aDetailEntry);
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
		/// Builds the total/detail cross-reference using the given total and detail Ids.
		/// </summary>
		/// <remarks>
		/// An entry is added to the ArrayList of both total and detail HashTables at the location of the Profile Ids specified by the given total
		/// and detail Ids.  The ArrayLists in the HashTables are created upon first usage.
		/// </remarks>
		/// <param name="aTotalId">
		/// The total Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aPrimaryDetailId">
		/// The primary detail Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aSecondaryDetailId">
		/// The secondary detail Profile Id to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(int aTotalId, int aPrimaryDetailId, int aSecondaryDetailId)
		{
			try
			{
				AddXRefIdEntry(aTotalId, new ComplexProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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
		/// The list of ComplexProfileXRefDetailEntry of the details to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(int aTotalId, ArrayList aDetailList)
		{
			try
			{
				foreach (ComplexProfileXRefDetailEntry detailEntry in aDetailList)
				{
					AddXRefIdEntry(aTotalId, detailEntry);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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
		/// <param name="aDetailEntry">
		/// The ComplexProfileXRefDetailEntry of the detail to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(ArrayList aTotalList, ComplexProfileXRefDetailEntry aDetailEntry)
		{
			try
			{
				foreach (int totalId in aTotalList)
				{
					AddXRefIdEntry(totalId, aDetailEntry);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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
		/// <param name="aPrimaryDetailId">
		/// The primary detail Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aSecondaryDetailId">
		/// The secondary detail Profile Id to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(ArrayList aTotalList, int aTotalId, int aPrimaryDetailId, int aSecondaryDetailId)
		{
			ComplexProfileXRefDetailEntry detailEntry;

			try
			{
				detailEntry = new ComplexProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId);

				foreach (int totalId in aTotalList)
				{
					AddXRefIdEntry(totalId, detailEntry);
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
	/// This class defines the detail hash entry for a ComplexProfileXRef.
	/// </summary>

	[Serializable]
	public class ComplexProfileXRefDetailEntry
	{
		//=======
		// FIELDS
		//=======

		private int _primaryId;
		private int _secondaryId;

		//=============
		// CONSTRUCTORS
		//=============

		public ComplexProfileXRefDetailEntry(int aPrimaryId, int aSecondaryId)
		{
			_primaryId = aPrimaryId;
			_secondaryId = aSecondaryId;
		}

		//===========
		// PROPERTIES
		//===========

		public int PrimaryId
		{
			get
			{
				return _primaryId;
			}
		}

		public int SecondaryId
		{
			get
			{
				return _secondaryId;
			}
		}

		//========
		// METHODS
		//========

		override public bool Equals(object obj)
		{
			return (_primaryId == ((ComplexProfileXRefDetailEntry)obj)._primaryId &&
				_secondaryId == ((ComplexProfileXRefDetailEntry)obj)._secondaryId);
		}

		override public int GetHashCode()
		{
			return Include.CreateHashKey(_primaryId, _secondaryId);
		}
	}

	/// <summary>
	/// This class defines the total hash entry for a ComplexProfileXRef.
	/// </summary>

	[Serializable]
	public class ComplexProfileXRefTotalHashEntry
	{
		//=======
		// FIELDS
		//=======

		private ArrayList _detailList;
		private ProfileXRef _detailXRef;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ComplexProfileXRefTotalHashEntry using the given primary detail type and secondary detail type.
		/// </summary>
		/// <param name="aPrimaryDetailType">
		/// The eProfileType that defines the primary detail type.
		/// </param>
		/// <param name="aSecondaryDetailType">
		/// The eProfileType that defines the secondary detail type.
		/// </param>

		public ComplexProfileXRefTotalHashEntry(eProfileType aPrimaryDetailType, eProfileType aSecondaryDetailType)
		{
			_detailList = new ArrayList();
			_detailXRef = new ProfileXRef(aPrimaryDetailType, aSecondaryDetailType);
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the ArrayList containing the detail list of ComplexProfileXRefDetailEntry objects.
		/// </summary>

		public ArrayList DetailList
		{
			get
			{
				return _detailList;
			}
		}

		/// <summary>
		/// Gets the ProfileXRef containing the primary to secondary detail relationship.
		/// </summary>

		public ProfileXRef DetailXRef
		{
			get
			{
				return _detailXRef;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// This method adds a ComplexProfileXRefDetailEntry object to the ArrayList and ProfileXRef.
		/// </summary>
		/// <param name="aDetailEntry">
		/// The ComplexProfileXRefDetailEntry to add.
		/// </param>

		public void Add(ComplexProfileXRefDetailEntry aDetailEntry)
		{
			try
			{
				_detailList.Add(aDetailEntry);
				_detailXRef.AddXRefIdEntry(aDetailEntry.PrimaryId, aDetailEntry.SecondaryId);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// This method returns a boolean indicating if the given ComplexProfileXRefDetailEntry exists in the ArrayList.
		/// </summary>
		/// <param name="aDetailEntry">
		/// The ComplexProfileXRefDetailEntry to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the given ComplexProfileXRefDetailEntry exists in the ArrayList.
		/// </returns>

		public bool Contains(ComplexProfileXRefDetailEntry aDetailEntry)
		{
			try
			{
				return _detailList.Contains(aDetailEntry);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	
	/// <summary>
	/// Defines a complex cross reference between a single total type and three detail types.
	/// </summary>
	/// <remarks>
	/// This class supports the ability for auto-totaling during computations.  It is composed of two HashTables, one for the detail Profile and one for
	/// the total Profile.  Each entry in the HashTable is an ArrayList of total Profiles for the detail HashTable, and detail Profiles for the total
	/// HashTable.  This allows the system to determine which detail Profiles are related to a total Profile, or which total Profiles are related to
	/// a detail Profile, with a single HashTable access.
	/// </remarks>

	[Serializable]
	public class TriProfileXRef : BaseProfileXRef
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _primaryTotalType;
		private eProfileType _secondaryTotalType;
		private eProfileType _primaryDetailType;
		private eProfileType _secondaryDetailType;
		private eProfileType _tertiaryDetailType;
		private Hashtable _totalAllIdHash;
		private Hashtable _detailAllIdHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ProfileXRef using the given total eProfileType and detail eProfileType.
		/// </summary>
		/// <param name="aPrimaryTotalType">
		/// The eProfileType of the primary total Profile.
		/// </param>
		/// <param name="aSecondaryTotalType">
		/// The eProfileType of the secondary total Profile.
		/// </param>
		/// <param name="aPrimaryDetailType">
		/// The eProfileType of the primary detail Profile.
		/// </param>
		/// <param name="aSecondaryDetailType">
		/// The eProfileType of the secondary detail Profile.
		/// </param>
		/// <param name="aTertiaryDetailType">
		/// The eProfileType of the tertiary detail Profile.
		/// </param>

		public TriProfileXRef(eProfileType aPrimaryTotalType, eProfileType aSecondaryTotalType, eProfileType aPrimaryDetailType, eProfileType aSecondaryDetailType,
			eProfileType aTertiaryDetailType)
			: base()
		{
			try
			{
				_primaryTotalType = aPrimaryTotalType;
				_secondaryTotalType = aSecondaryTotalType;
				_primaryDetailType = aPrimaryDetailType;
				_secondaryDetailType = aSecondaryDetailType;
				_tertiaryDetailType = aTertiaryDetailType;
				_totalAllIdHash = new Hashtable();
				_detailAllIdHash = new Hashtable();
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

		/// <summary>
		/// Gets the eProfileType of the primary total Profile.
		/// </summary>

		virtual public eProfileType PrimaryTotalType
		{
			get
			{
				return _primaryTotalType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the secondary total Profile.
		/// </summary>

		virtual public eProfileType SecondaryTotalType
		{
			get
			{
				return _secondaryTotalType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the primary detail Profile.
		/// </summary>

		virtual public eProfileType PrimaryDetailType
		{
			get
			{
				return _primaryDetailType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the secondary detail Profile.
		/// </summary>

		virtual public eProfileType SecondaryDetailType
		{
			get
			{
				return _secondaryDetailType;
			}
		}

		/// <summary>
		/// Gets the eProfileType of the secondary detail Profile.
		/// </summary>

		virtual public eProfileType TertiaryDetailType
		{
			get
			{
				return _tertiaryDetailType;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Override.  Returns the hashcode for this object.
		/// </summary>
		/// <remarks>
		/// Calulation of this hashcode is accomplished by multiplying the total type by 100 and then adding the detail type
		/// </remarks>
		/// <returns>
		/// The hashcode for this object.
		/// </returns>

		override public int GetHashCode()
		{
			return Include.CreateHashKey((int)_primaryTotalType, (int)_primaryDetailType, (int)_secondaryDetailType, (int)_tertiaryDetailType);
		}

		/// <summary>
		/// Returns a value indicating whether an instance of ProfileXRef is equal to a specified object.
		/// </summary>
		/// <param name="obj">
		/// An object to compare with this instance.
		/// </param>
		/// <returns>
		/// <I>true</I> if value is an instance of ProfileXRef and equals the value of this instance; otherwise,
		/// <I>false</I>.
		/// </returns>

		override public bool Equals(object obj)
		{
			return ((this.GetType() == obj.GetType() || obj.GetType().IsSubclassOf(this.GetType())) &&
				_primaryTotalType == ((TriProfileXRef)obj)._primaryTotalType &&
				_secondaryTotalType == ((TriProfileXRef)obj)._secondaryTotalType &&
				_primaryDetailType == ((TriProfileXRef)obj)._primaryDetailType &&
				_secondaryDetailType == ((TriProfileXRef)obj)._secondaryDetailType &&
				_tertiaryDetailType == ((TriProfileXRef)obj)._tertiaryDetailType);
		}

		/// <summary>
		/// Returns the list of detail Ids for a given total Id.
		/// </summary>
        /// <param name="aPrimaryTotalId">
		/// The primary total Id to get a detail list for.
		/// </param>
        /// <param name="aSecondaryTotalID">
        /// The secondary total Id to get a detail list for.
        /// </param>
		/// <returns>
		/// The list of detail Ids.
		/// </returns>

		virtual public ArrayList GetDetailList(int aPrimaryTotalId, int aSecondaryTotalID)
		{
			TriProfileXRefTotalEntry totalEntry;
			try
			{
				totalEntry = new TriProfileXRefTotalEntry(aPrimaryTotalId, aSecondaryTotalID);
				return (ArrayList)_totalAllIdHash[totalEntry];
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
		/// <param name="aPrimaryDetailId">
		/// The primary detail Id to get a total list for.
		/// </param>
		/// <param name="aSecondaryDetailId">
		/// The secondary detail Id to get a total list for.
		/// </param>
		/// <param name="aTertiaryDetailId">
		/// The tertiary detail Id to get a total list for.
		/// </param>
		/// <returns>
		/// the list of total Ids.
		/// </returns>

		virtual public ArrayList GetTotalList(int aPrimaryDetailId, int aSecondaryDetailId, int aTertiaryDetailId)
		{
			TriProfileXRefDetailEntry detailEntry;

			try
			{
				detailEntry = new TriProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId, aTertiaryDetailId);
				return (ArrayList)_detailAllIdHash[detailEntry];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		///// <summary>
		///// Returns the list of detail Ids for a given total Id.
		///// </summary>
		///// <param name="aPrimaryTotalId">
		///// The primary total Id to get a detail list for.
		///// </param>
		///// <param name="aSecondaryTotalID">
		///// The secondary total Id to get a detail list for.
		///// </param>
		///// <returns>
		///// The list of detail Ids.
		///// </returns>

		//virtual public ArrayList GetDetailPlusNextList(int aPrimaryTotalId, int aSecondaryTotalID)
		//{
		//    TriProfileXRefTotalEntry totalEntry;
		//    try
		//    {
		//        totalEntry = new TriProfileXRefTotalEntry(aPrimaryTotalId, aSecondaryTotalID);
		//        return (ArrayList)_totalAllPlusNextIdHash[totalEntry];
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Returns the list of total Ids for a given detail Id.
		///// </summary>
		///// <param name="aPrimaryDetailId">
		///// The primary detail Id to get a total list for.
		///// </param>
		///// <param name="aSecondaryDetailId">
		///// The secondary detail Id to get a total list for.
		///// </param>
		///// <param name="aTertiaryDetailId">
		///// The tertiary detail Id to get a total list for.
		///// </param>
		///// <returns>
		///// the list of total Ids.
		///// </returns>

		//virtual public ArrayList GetTotalPlusNextList(int aPrimaryDetailId, int aSecondaryDetailId, int aTertiaryDetailId)
		//{
		//    TriProfileXRefDetailEntry detailEntry;

		//    try
		//    {
		//        detailEntry = new TriProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId, aTertiaryDetailId);
		//        return (ArrayList)_detailAllPlusNextIdHash[detailEntry];
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Returns the first detail Id for a given total Id.
		///// </summary>
		///// <param name="aPrimaryTotalId">
		///// The primary total Id to get a detail list for.
		///// </param>
		///// <param name="aSecondaryTotalID">
		///// The secondary total Id to get a detail list for.
		///// </param>
		///// <returns>
		///// The first detail Id.
		///// </returns>

		//virtual public ArrayList GetDetailFirst(int aPrimaryTotalId, int aSecondaryTotalID)
		//{
		//    TriProfileXRefTotalEntry totalEntry;
		//    try
		//    {
		//        totalEntry = new TriProfileXRefTotalEntry(aPrimaryTotalId, aSecondaryTotalID);
		//        return (ArrayList)_totalFirstIdHash[totalEntry];
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Returns the total Id for a given first detail Id.
		///// </summary>
		///// <param name="aPrimaryDetailId">
		///// The first primary detail Id to get the total for.
		///// </param>
		///// <param name="aSecondaryDetailId">
		///// The first secondary detail Id to get a total for.
		///// </param>
		///// <param name="aTertiaryDetailId">
		///// The first tertiary detail Id to get a total list for.
		///// </param>
		///// <returns>
		///// The total Id.
		///// </returns>

		//virtual public ArrayList GetTotalFirst(int aPrimaryDetailId, int aSecondaryDetailId, int aTertiaryDetailId)
		//{
		//    TriProfileXRefDetailEntry detailEntry;

		//    try
		//    {
		//        if (!_detailInited)
		//        {
		//            intInitDetail();
		//        }

		//        detailEntry = new TriProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId, aTertiaryDetailId);
		//        return (ArrayList)_detailFirstIdHash[detailEntry];
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Returns the last detail Id for a given total Id.
		///// </summary>
		///// <param name="aPrimaryTotalId">
		///// The primary total Id to get a detail list for.
		///// </param>
		///// <param name="aSecondaryTotalID">
		///// The secondary total Id to get a detail list for.
		///// </param>
		///// <returns>
		///// The last detail Id.
		///// </returns>

		//virtual public ArrayList GetDetailLast(int aPrimaryTotalId, int aSecondaryTotalID)
		//{
		//    TriProfileXRefTotalEntry totalEntry;
		//    try
		//    {
		//        totalEntry = new TriProfileXRefTotalEntry(aPrimaryTotalId, aSecondaryTotalID);
		//        return (ArrayList)_totalLastIdHash[totalEntry];
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Returns the total Id for a given last detail Id.
		///// </summary>
		///// <param name="aPrimaryDetailId">
		///// The last primary detail Id to get the total for.
		///// </param>
		///// <param name="aSecondaryDetailId">
		///// The last secondary detail Id to get a total for.
		///// </param>
		///// <param name="aTertiaryDetailId">
		///// The last tertiary detail Id to get a total list for.
		///// </param>
		///// <returns>
		///// The total Id.
		///// </returns>

		//virtual public ArrayList GetTotalLast(int aPrimaryDetailId, int aSecondaryDetailId, int aTertiaryDetailId)
		//{
		//    TriProfileXRefDetailEntry detailEntry;

		//    try
		//    {
		//        if (!_detailInited)
		//        {
		//            intInitDetail();
		//        }

		//        detailEntry = new TriProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId, aTertiaryDetailId);
		//        return (ArrayList)_detailLastIdHash[detailEntry];
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Returns the next detail Id for a given total Id.
		///// </summary>
		///// <param name="aPrimaryTotalId">
		///// The primary total Id to get a detail list for.
		///// </param>
		///// <param name="aSecondaryTotalID">
		///// The secondary total Id to get a detail list for.
		///// </param>
		///// <returns>
		///// The next detail Id.
		///// </returns>

		//virtual public ArrayList GetDetailNext(int aPrimaryTotalId, int aSecondaryTotalID)
		//{
		//    TriProfileXRefTotalEntry totalEntry;
		//    try
		//    {
		//        totalEntry = new TriProfileXRefTotalEntry(aPrimaryTotalId, aSecondaryTotalID);
		//        return (ArrayList)_totalNextIdHash[totalEntry];
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Returns the total Id for a given next detail Id.
		///// </summary>
		///// <param name="aPrimaryDetailId">
		///// The last primary detail Id to get the total for.
		///// </param>
		///// <param name="aSecondaryDetailId">
		///// The last secondary detail Id to get a total for.
		///// </param>
		///// <param name="aTertiaryDetailId">
		///// The last tertiary detail Id to get a total list for.
		///// </param>
		///// <returns>
		///// The total Id.
		///// </returns>

		//virtual public ArrayList GetTotalNext(int aPrimaryDetailId, int aSecondaryDetailId, int aTertiaryDetailId)
		//{
		//    TriProfileXRefDetailEntry detailEntry;

		//    try
		//    {
		//        if (!_detailInited)
		//        {
		//            intInitDetail();
		//        }

		//        detailEntry = new TriProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId, aTertiaryDetailId);
		//        return (ArrayList)_detailNextIdHash[detailEntry];
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		/// <summary>
		/// Builds the total/detail cross-reference using the given total and detail Ids.
		/// </summary>
		/// <remarks>
		/// An entry is added to the ArrayList of both total and detail HashTables at the location of the Profile Ids specified by the given total
		/// and detail Ids.  The ArrayLists in the HashTables are created upon first usage.
		/// </remarks>
        /// <param name="aTotalEntry">
        /// The TriProfileXRefDetailEntry of the total to add a cross reference for.
		/// </param>
		/// <param name="aDetailEntry">
		/// The TriProfileXRefDetailEntry of the detail to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(TriProfileXRefTotalEntry aTotalEntry, TriProfileXRefDetailEntry aDetailEntry)
		{
			ArrayList arrList;

			try
			{
				// Update Detail list

				arrList = (ArrayList)_detailAllIdHash[aDetailEntry];

				if (arrList == null)
				{
					arrList = new ArrayList();
					arrList.Add(aTotalEntry);
					_detailAllIdHash.Add(aDetailEntry, arrList);
				}
				else
				{
					if (!arrList.Contains(aTotalEntry))
					{
						arrList.Add(aTotalEntry);
					}
				}

				// Update Total list

				arrList = (ArrayList)_totalAllIdHash[aTotalEntry];

				if (arrList == null)
				{
					arrList = new ArrayList();
					arrList.Add(aDetailEntry);
					_totalAllIdHash.Add(aTotalEntry, arrList);
				}
				else
				{
					if (!arrList.Contains(aDetailEntry))
					{
						arrList.Add(aDetailEntry);
					}
				}

				//// Update Detail list

				//arrList = (ArrayList)_detailAllPlusNextIdHash[aDetailEntry];

				//if (arrList == null)
				//{
				//    arrList = new ArrayList();
				//    arrList.Add(aTotalEntry);
				//    _detailAllPlusNextIdHash.Add(aDetailEntry, arrList);
				//}
				//else
				//{
				//    if (!arrList.Contains(aTotalEntry))
				//    {
				//        arrList.Add(aTotalEntry);
				//    }
				//}

				//// Update Total list

				//arrList = (ArrayList)_totalAllPlusNextIdHash[aTotalEntry];

				//if (arrList == null)
				//{
				//    arrList = new ArrayList();
				//    arrList.Add(aDetailEntry);
				//    _totalAllPlusNextIdHash.Add(aTotalEntry, arrList);
				//}
				//else
				//{
				//    if (!arrList.Contains(aDetailEntry))
				//    {
				//        arrList.Add(aDetailEntry);
				//    }
				//}

				//// Set First Total List

				//if (!_totalFirstIdHash.Contains(aTotalEntry))
				//{
				//    arrList = new ArrayList();
				//    arrList.Add(aDetailEntry);
				//    _totalFirstIdHash.Add(aTotalEntry, arrList);
				//}

				//// Set Last Total List

				//arrList = (ArrayList)_totalLastIdHash[aTotalEntry];

				//if (arrList == null)
				//{
				//    arrList = new ArrayList();
				//    _totalLastIdHash.Add(aTotalEntry, arrList);
				//    arrList.Add(aDetailEntry);
				//}
				//else
				//{
				//    arrList.Clear();
				//    arrList.Add(aDetailEntry);
				//}

				//_detailInited = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds the total/detail cross-reference using the given total and detail Ids.
		/// </summary>
		/// <remarks>
		/// An entry is added to the ArrayList of both total and detail HashTables at the location of the Profile Ids specified by the given total
		/// and detail Ids.  The ArrayLists in the HashTables are created upon first usage.
		/// </remarks>
        /// <param name="aPrimaryTotalId">
		/// The primary total Profile Id to add a cross reference for.
		/// </param>
        /// <param name="aSecondaryTotalId">
        /// The secondary total Profile Id to add a cross reference for.
        /// </param>
		/// <param name="aPrimaryDetailId">
		/// The primary detail Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aSecondaryDetailId">
		/// The secondary detail Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aTertiaryDetailId">
		/// The tertiary detail Profile Id to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(int aPrimaryTotalId, int aSecondaryTotalId, 
			int aPrimaryDetailId, int aSecondaryDetailId, int aTertiaryDetailId)
		{
			TriProfileXRefTotalEntry totalEntry;
			try
			{
				totalEntry = new TriProfileXRefTotalEntry(aPrimaryTotalId, aSecondaryTotalId);
				AddXRefIdEntry(totalEntry, new TriProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId, aTertiaryDetailId));
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds the total/detail cross-reference using the given total Id and list of detail Ids.
		/// </summary>
		/// <remarks>
		/// This routine loops through all detail Ids on the given list and calls AddXRefIdEntry with each total/detail pair.
		/// </remarks>
        /// <param name="aTotalEntry">
        /// The TriProfileXRefDetailEntry of the total.
		/// </param>
		/// <param name="aDetailList">
		/// The list of TriProfileXRefDetailEntry of the details to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(TriProfileXRefTotalEntry aTotalEntry, ArrayList aDetailList)
		{
			try
			{
				foreach (TriProfileXRefDetailEntry detailEntry in aDetailList)
				{
					AddXRefIdEntry(aTotalEntry, detailEntry);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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
		/// <param name="aDetailEntry">
		/// The TriProfileXRefDetailEntry of the detail to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(ArrayList aTotalList, TriProfileXRefDetailEntry aDetailEntry)
		{
			try
			{
				foreach (TriProfileXRefTotalEntry totalEntry in aTotalList)
				{
					AddXRefIdEntry(totalEntry, aDetailEntry);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
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
		/// <param name="aPrimaryDetailId">
		/// The primary detail Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aSecondaryDetailId">
		/// The secondary detail Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aTertiaryDetailId">
		/// The tertiary detail Profile Id to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(ArrayList aTotalList, int aPrimaryTotalId, int aSecondaryTotalId, 
			int aPrimaryDetailId, int aSecondaryDetailId, int aTertiaryDetailId)
		{
			TriProfileXRefDetailEntry detailEntry;

			try
			{
				detailEntry = new TriProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId, aTertiaryDetailId);

				foreach (TriProfileXRefTotalEntry totalEntry in aTotalList)
				{
					AddXRefIdEntry(totalEntry, detailEntry);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		///// <summary>
		///// Builds the total/detail cross-reference using the given total and detail Ids.
		///// </summary>
		///// <remarks>
		///// An entry is added to the ArrayList of both total and detail HashTables at the location of the Profile Ids specified by the given total
		///// and detail Ids.  The ArrayLists in the HashTables are created upon first usage.
		///// </remarks>
		///// <param name="aTotalEntry">
		///// The TriProfileXRefTotalEntry total Profile to add a cross reference for.
		///// </param>
		///// <param name="aDetailEntry">
		///// The TriProfileXRefDetailEntry of the detail to add a cross reference for.
		///// </param>

		//virtual public void SetNextXRefIdEntry(TriProfileXRefTotalEntry aTotalEntry, TriProfileXRefDetailEntry aDetailEntry)
		//{
		//    ArrayList arrList;

		//    try
		//    {
		//        arrList = new ArrayList();
		//        arrList.Add(aDetailEntry);
		//        _totalNextIdHash.Add(aTotalEntry, arrList);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Builds the total/detail cross-reference using the given total and detail Ids.
		///// </summary>
		///// <remarks>
		///// An entry is added to the ArrayList of both total and detail HashTables at the location of the Profile Ids specified by the given total
		///// and detail Ids.  The ArrayLists in the HashTables are created upon first usage.
		///// </remarks>
		///// <param name="aPrimaryTotalId">
		///// The primary total Profile Id to add a cross reference for.
		///// </param>
		///// <param name="aSecondaryTotalId">
		///// The secondary total Profile Id to add a cross reference for.
		///// </param>
		///// <param name="aPrimaryDetailId">
		///// The primary detail Profile Id to add a cross reference for.
		///// </param>
		///// <param name="aSecondaryDetailId">
		///// The secondary detail Profile Id to add a cross reference for.
		///// </param>
		///// <param name="aTertiaryDetailId">
		///// The tertiary detail Profile Id to add a cross reference for.
		///// </param>

		//virtual public void SetNextXRefIdEntry(int aPrimaryTotalId, int aSecondaryTotalId, 
		//    int aPrimaryDetailId, int aSecondaryDetailId, int aTertiaryDetailId)
		//{
		//    TriProfileXRefTotalEntry totalEntry;
		//    try
		//    {
		//        totalEntry = new TriProfileXRefTotalEntry(aPrimaryTotalId, aSecondaryTotalId);
		//        SetNextXRefIdEntry(totalEntry, new TriProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId, aTertiaryDetailId));
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Builds the total/detail cross-reference using the given total Id and list of detail Ids.
		///// </summary>
		///// <remarks>
		///// This routine loops through all detail Ids on the given list and calls AddXRefIdEntry with each total/detail pair.
		///// </remarks>
		///// <param name="aTotalEntry">
		///// The TriProfileXRefTotalEntry of the total Profile to add a cross reference for.
		///// </param>
		///// <param name="aDetailList">
		///// The list of TriProfileXRefDetailEntry of the details to add a cross reference for.
		///// </param>

		//virtual public void SetNextXRefIdEntry(TriProfileXRefTotalEntry aTotalEntry, ArrayList aDetailList)
		//{
		//    try
		//    {
		//        foreach (TriProfileXRefDetailEntry detailEntry in aDetailList)
		//        {
		//            SetNextXRefIdEntry(aTotalEntry, detailEntry);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Builds the total/detail cross-reference using the given detail Id and list of total Ids.
		///// </summary>
		///// <remarks>
		///// This routine loops through all total Ids on the given list and calls AddXRefIdEntry with each total/detail pair.
		///// </remarks>
		///// <param name="aTotalList">
		///// The list of total Profile Ids to add a cross reference for.
		///// </param>
		///// <param name="aDetailEntry">
		///// The TriProfileXRefDetailEntry of the detail to add a cross reference for.
		///// </param>

		//virtual public void SetNextXRefIdEntry(ArrayList aTotalList, TriProfileXRefDetailEntry aDetailEntry)
		//{
		//    try
		//    {
		//        foreach (TriProfileXRefTotalEntry totalEntry in aTotalList)
		//        {
		//            SetNextXRefIdEntry(totalEntry, aDetailEntry);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// Builds the total/detail cross-reference using the given detail Id and list of total Ids.
		///// </summary>
		///// <remarks>
		///// This routine loops through all total Ids on the given list and calls AddXRefIdEntry with each total/detail pair.
		///// </remarks>
		///// <param name="aTotalList">
		///// The list of total Profile Ids to add a cross reference for.
		///// </param>
		///// <param name="aPrimaryDetailId">
		///// The primary detail Profile Id to add a cross reference for.
		///// </param>
		///// <param name="aSecondaryDetailId">
		///// The secondary detail Profile Id to add a cross reference for.
		///// </param>
		///// <param name="aTertiaryDetailId">
		///// The tertiary detail Profile Id to add a cross reference for.
		///// </param>

		//virtual public void SetNextXRefIdEntry(ArrayList aTotalList, TriProfileXRefTotalEntry aTotalEntry, 
		//    int aPrimaryDetailId, int aSecondaryDetailId, int aTertiaryDetailId)
		//{
		//    TriProfileXRefDetailEntry detailEntry;

		//    try
		//    {
		//        detailEntry = new TriProfileXRefDetailEntry(aPrimaryDetailId, aSecondaryDetailId, aTertiaryDetailId);

		//        foreach (TriProfileXRefTotalEntry totalEntry in aTotalList)
		//        {
		//            SetNextXRefIdEntry(totalEntry, detailEntry);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		///// <summary>
		///// This routine initializes the First and Next Detail tables.
		///// </summary>

		//private void intInitDetail()
		//{
		//    IDictionaryEnumerator dictEnum;
		//    ArrayList detailEntryList;
		//    ArrayList detailList;

		//    try
		//    {
		//        _detailFirstIdHash = new Hashtable();
		//        dictEnum = _totalFirstIdHash.GetEnumerator();

		//        while (dictEnum.MoveNext())
		//        {
		//            detailEntryList = (ArrayList)dictEnum.Value;

		//            foreach (TriProfileXRefDetailEntry detailEntry in detailEntryList)
		//            {
		//                detailList = (ArrayList)_detailFirstIdHash[detailEntry];

		//                if (detailList == null)
		//                {
		//                    detailList = new ArrayList();
		//                    _detailFirstIdHash.Add(detailEntry, detailList);
		//                    detailList.Add(dictEnum.Key);
		//                }
		//                else
		//                {
		//                    if (!detailList.Contains(dictEnum.Key))
		//                    {
		//                        detailList.Add(dictEnum.Key);
		//                    }
		//                }
		//            }
		//        }

		//        _detailLastIdHash = new Hashtable();
		//        dictEnum = _totalLastIdHash.GetEnumerator();

		//        while (dictEnum.MoveNext())
		//        {
		//            detailEntryList = (ArrayList)dictEnum.Value;

		//            foreach (int detailEntry in detailEntryList)
		//            {
		//                detailList = (ArrayList)_detailLastIdHash[detailEntry];

		//                if (detailList == null)
		//                {
		//                    detailList = new ArrayList();
		//                    _detailLastIdHash.Add(detailEntry, detailList);
		//                    detailList.Add(dictEnum.Key);
		//                }
		//                else
		//                {
		//                    if (!detailList.Contains(dictEnum.Key))
		//                    {
		//                        detailList.Add(dictEnum.Key);
		//                    }
		//                }
		//            }
		//        }

		//        _detailNextIdHash = new Hashtable();
		//        dictEnum = _totalNextIdHash.GetEnumerator();

		//        while (dictEnum.MoveNext())
		//        {
		//            detailEntryList = (ArrayList)dictEnum.Value;

		//            foreach (int detailEntry in detailEntryList)
		//            {
		//                // Update Detail Next list

		//                detailList = (ArrayList)_detailNextIdHash[detailEntry];

		//                if (detailList == null)
		//                {
		//                    detailList = new ArrayList();
		//                    detailList.Add(dictEnum.Key);
		//                    _detailNextIdHash.Add(detailEntry, detailList);
		//                }
		//                else
		//                {
		//                    if (!detailList.Contains(dictEnum.Key))
		//                    {
		//                        detailList.Add(dictEnum.Key);
		//                    }
		//                }
					
		//                // Update Detail PlusNext list

		//                detailList = (ArrayList)_detailAllPlusNextIdHash[detailEntry];

		//                if (detailList == null)
		//                {
		//                    detailList = new ArrayList();
		//                    detailList.Add(dictEnum.Key);
		//                    _detailAllPlusNextIdHash.Add(detailEntry, detailList);
		//                }
		//                else
		//                {
		//                    if (!detailList.Contains(dictEnum.Key))
		//                    {
		//                        detailList.Add(dictEnum.Key);
		//                    }
		//                }
		//            }
		//        }

		//        _detailInited = true;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
	}

	/// <summary>
	/// This class defines the detail hash entry for a TriProfileXRef.
	/// </summary>

	[Serializable]
	public class TriProfileXRefDetailEntry
	{
		//=======
		// FIELDS
		//=======

		private int _primaryId;
		private int _secondaryId;
		private int _tertiaryId;

		//=============
		// CONSTRUCTORS
		//=============

		public TriProfileXRefDetailEntry(int aPrimaryId, int aSecondaryId, int aTertiaryId)
		{
			_primaryId = aPrimaryId;
			_secondaryId = aSecondaryId;
			_tertiaryId = aTertiaryId;
		}

		//===========
		// PROPERTIES
		//===========

		public int PrimaryId
		{
			get
			{
				return _primaryId;
			}
		}

		public int SecondaryId
		{
			get
			{
				return _secondaryId;
			}
		}

		public int TertiaryId
		{
			get
			{
				return _tertiaryId;
			}
		}

		//========
		// METHODS
		//========

		override public bool Equals(object obj)
		{
			return (_primaryId == ((TriProfileXRefDetailEntry)obj)._primaryId &&
				_secondaryId == ((TriProfileXRefDetailEntry)obj)._secondaryId &&
				_tertiaryId == ((TriProfileXRefDetailEntry)obj)._tertiaryId);
		}

		override public int GetHashCode()
		{
			return Include.CreateHashKey(_primaryId, _secondaryId, _tertiaryId);
		}
	}

	/// <summary>
	/// This class defines the total hash entry for a TriProfileXRef.
	/// </summary>

	[Serializable]
	public class TriProfileXRefTotalEntry
	{
		//=======
		// FIELDS
		//=======

		private int _primaryId;
		private int _secondaryId;

		//=============
		// CONSTRUCTORS
		//=============

		public TriProfileXRefTotalEntry(int aPrimaryId, int aSecondaryId)
		{
			_primaryId = aPrimaryId;
			_secondaryId = aSecondaryId;
		}

		//===========
		// PROPERTIES
		//===========

		public int PrimaryId
		{
			get
			{
				return _primaryId;
			}
		}

		public int SecondaryId
		{
			get
			{
				return _secondaryId;
			}
		}

		//========
		// METHODS
		//========

		override public bool Equals(object obj)
		{
			return (_primaryId == ((TriProfileXRefTotalEntry)obj)._primaryId &&
				_secondaryId == ((TriProfileXRefTotalEntry)obj)._secondaryId);
		}

		override public int GetHashCode()
		{
			return Include.CreateHashKey(_primaryId, _secondaryId);
		}
	}

	/// <summary>
	/// Defines a cross reference between a placeholder pack-color and a header pack-color.
	/// </summary>

	[Serializable]
	public class PackColorProfileXRef : BaseProfileXRef
	{
		//=======
		// FIELDS
		//=======

		private Hashtable _plcAllIdHash;
		private Hashtable _hdrAllIdHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ProfileXRef.
		/// </summary>

		public PackColorProfileXRef()
			: base()
		{
			try
			{
				_plcAllIdHash = new Hashtable();
				_hdrAllIdHash = new Hashtable();
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
		/// Returns the list of header pack-colors for a given placholder pack-color.
		/// </summary>
        /// <param name="aPlaceholderID">
		/// The placeholder Id to get a header pack-color list for.
		/// </param>
		/// <param name="aPlaceholderPackId">
		/// The placeholder pack Id to get a header pack-color list for.
		/// </param>
		/// <param name="aPlaceholderColorId">
		/// The placeholder color Id to get a header pack-color list for.
		/// </param>
		/// <returns>
		/// The list of header pack-color Ids.
		/// </returns>

		virtual public Hashtable GetHeaderList(int aPlaceholderID, int aPlaceholderPackId, int aPlaceholderColorId)
		{
			PackColorProfileXRefEntry plcEntry;

			try
			{
				plcEntry = new PackColorProfileXRefEntry(aPlaceholderID, aPlaceholderPackId, aPlaceholderColorId);
				return (Hashtable)_plcAllIdHash[plcEntry];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the list of placeholder pack-color Ids for given header pack-color Ids.
		/// </summary>
		/// <param name="aHeaderId">
		/// The header Id to get a placeholder pack-color list for.
		/// </param>
		/// <param name="aHeaderPackId">
		/// The header pack Id to get a placeholder pack-color list for.
		/// </param>
		/// <param name="aHeaderColorId">
		/// The header color Id to get a placeholder pack-color list for.
		/// </param>
		/// <returns>
		/// the list of placeholder pack-color Ids.
		/// </returns>

		virtual public Hashtable GetPlaceholderList(int aHeaderId, int aHeaderPackId, int aHeaderColorId)
		{
			PackColorProfileXRefEntry hdrEntry;

			try
			{
				hdrEntry = new PackColorProfileXRefEntry(aHeaderId, aHeaderPackId, aHeaderColorId);
				return (Hashtable)_hdrAllIdHash[hdrEntry];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds the placeholder/header cross-reference using the given placeholder and header Ids.
		/// </summary>
		/// <param name="aPlaceholderId">
		/// The Placeholder Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aPlaceholderPackId">
		/// The Placeholder Pack Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aPlaceholderColorId">
		/// The Placeholder Color Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aHeaderId">
		/// The Header Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aHeaderPackId">
		/// The Header Pack Profile Id to add a cross reference for.
		/// </param>
		/// <param name="aHeaderColorId">
		/// The Header Color Profile Id to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(int aPlaceholderId, int aPlaceholderPackId, int aPlaceholderColorId, int aHeaderId, int aHeaderPackId, int aHeaderColorId)
		{
			PackColorProfileXRefEntry hdrEntry;
			PackColorProfileXRefEntry plcEntry;
			Hashtable hdrHashEntry;
			Hashtable plcHashEntry;

			try
			{
				hdrEntry = new PackColorProfileXRefEntry(aHeaderId, aHeaderPackId, aHeaderColorId);
				plcEntry = new PackColorProfileXRefEntry(aPlaceholderId, aPlaceholderPackId, aPlaceholderColorId);

				// Update header list

				hdrHashEntry = (Hashtable)_hdrAllIdHash[hdrEntry];

				if (hdrHashEntry == null)
				{
					hdrHashEntry = new Hashtable();
					_hdrAllIdHash[hdrEntry] = hdrHashEntry;
				}

				hdrHashEntry[plcEntry] = plcEntry;

				// Update placeholder list

				plcHashEntry = (Hashtable)_plcAllIdHash[plcEntry];

				if (plcHashEntry == null)
				{
					plcHashEntry = new Hashtable();
					_plcAllIdHash[plcEntry] = plcHashEntry;
				}

				plcHashEntry[hdrEntry] = hdrEntry;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// This class defines the hash entry for a PackColorProfileXRef.
	/// </summary>

	public class PackColorProfileXRefEntry
	{
		//=======
		// FIELDS
		//=======

		private int _plcHdrId;
		private int _packId;
		private int _colorId;

		//=============
		// CONSTRUCTORS
		//=============

		public PackColorProfileXRefEntry(int aPlaceholderHeaderId, int aPackId, int aColorId)
		{
			_plcHdrId = aPlaceholderHeaderId;
			_packId = aPackId;
			_colorId = aColorId;
		}

		//===========
		// PROPERTIES
		//===========

		public int PlaceholderHeaderId
		{
			get
			{
				return _plcHdrId;
			}
		}

		public int PackId
		{
			get
			{
				return _packId;
			}
		}

		public int ColorId
		{
			get
			{
				return _colorId;
			}
		}

		//========
		// METHODS
		//========

		override public bool Equals(object obj)
		{
			return (_plcHdrId == ((PackColorProfileXRefEntry)obj)._plcHdrId &&
				_packId == ((PackColorProfileXRefEntry)obj)._packId &&
				_colorId == ((PackColorProfileXRefEntry)obj)._colorId);
		}

		override public int GetHashCode()
		{
			return Include.CreateHashKey(_plcHdrId, _packId, _colorId);
		}
	}
	//Begin TT#2 - JScott - Assortment Planning - Phase 2

	/// <summary>
	/// Defines a cross reference between the Set, Grade, and Store types.
	/// </summary>
	/// <remarks>
	/// This class supports the ability for auto-totaling during computations.  It is composed of two HashTables, one for the detail Profile and one for
	/// the total Profile.  Each entry in the HashTable is an ArrayList of total Profiles for the detail HashTable, and detail Profiles for the total
	/// HashTable.  This allows the system to determine which detail Profiles are related to a total Profile, or which total Profiles are related to
	/// a detail Profile, with a single HashTable access.
	/// </remarks>

	[Serializable]
	public class SetGradeStoreXRef : BaseProfileXRef
	{
		//=======
		// FIELDS
		//=======

		private Hashtable _totalAllIdHash;
		private Hashtable _detailAllIdHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of SetGradeStoreXRef.
		/// </summary>

		public SetGradeStoreXRef()
			: base()
		{
			try
			{
				_totalAllIdHash = new Hashtable();
				_detailAllIdHash = new Hashtable();
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
		/// Override.  Returns the hashcode for this object.
		/// </summary>
		/// <remarks>
		/// Calulation of this hashcode is accomplished by multiplying the total type by 100 and then adding the detail type
		/// </remarks>
		/// <returns>
		/// The hashcode for this object.
		/// </returns>

		override public int GetHashCode()
		{
			return Include.CreateHashKey((int)eProfileType.StoreGroupLevel, (int)eProfileType.StoreGrade, (int)eProfileType.Store);
		}

		/// <summary>
		/// Returns a value indicating whether an instance of ProfileXRef is equal to a specified object.
		/// </summary>
		/// <param name="obj">
		/// An object to compare with this instance.
		/// </param>
		/// <returns>
		/// <I>true</I> if value is an instance of ProfileXRef and equals the value of this instance; otherwise,
		/// <I>false</I>.
		/// </returns>

		override public bool Equals(object obj)
		{
			return ((this.GetType() == obj.GetType() || obj.GetType().IsSubclassOf(this.GetType())));
		}

		/// <summary>
		/// Returns the list of detail Ids for a given total Id.
		/// </summary>
		/// <param name="aSetId">
		/// The Set Id to get a detail list for.
		/// </param>
		/// <param name="aGradeId">
		/// The Grade Id to get a detail list for.
		/// </param>
		/// <returns>
		/// The list of detail Ids.
		/// </returns>

		virtual public ArrayList GetDetailList(int aSetId, int aGradeId)
		{
			SetGradeStoreXRefTotalEntry totalEntry;

			try
			{
				totalEntry = new SetGradeStoreXRefTotalEntry(aSetId, aGradeId);
				return (ArrayList)_totalAllIdHash[totalEntry];
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
		/// <param name="aStoreId">
		/// The Store Id to get a total list for.
		/// </param>
		/// <returns>
		/// the list of total Ids.
		/// </returns>

		virtual public ArrayList GetTotalList(int aStoreId)
		{
			SetGradeStoreXRefDetailHashEntry detHashEntry;

			try
			{
				detHashEntry = (SetGradeStoreXRefDetailHashEntry)_detailAllIdHash[aStoreId];

				if (detHashEntry != null)
				{
					return detHashEntry.TotalList;
				}
				else
				{
					return null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds the total/detail cross-reference using the given total and detail Ids.
		/// </summary>
		/// <remarks>
		/// An entry is added to the ArrayList of both total and detail HashTables at the location of the Profile Ids specified by the given total
		/// and detail Ids.  The ArrayLists in the HashTables are created upon first usage.
		/// </remarks>
		/// <param name="aTotalEntry">
		/// The SetGradeStoreXRefTotalEntry of the total to add a cross reference for.
		/// </param>
		/// <param name="aStoreId">
		/// The Store Id to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(SetGradeStoreXRefTotalEntry aTotalEntry, int aStoreId)
		{
			ArrayList arrList;
			SetGradeStoreXRefDetailHashEntry hashEntry;

			try
			{
				// Update Detail list

				hashEntry = (SetGradeStoreXRefDetailHashEntry)_detailAllIdHash[aStoreId];

				if (hashEntry == null)
				{
					hashEntry = new SetGradeStoreXRefDetailHashEntry();
					hashEntry.Add(aTotalEntry);
					_detailAllIdHash.Add(aStoreId, hashEntry);
				}
				else
				{
					if (!hashEntry.Contains(aTotalEntry))
					{
						hashEntry.Add(aTotalEntry);
					}
				}

				// Update Total list

				arrList = (ArrayList)_totalAllIdHash[aTotalEntry];

				if (arrList == null)
				{
					arrList = new ArrayList();
					arrList.Add(aStoreId);
					_totalAllIdHash.Add(aTotalEntry, arrList);
				}
				else
				{
					if (!arrList.Contains(aStoreId))
					{
						arrList.Add(aStoreId);
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
		/// Builds the total/detail cross-reference using the given total and detail Ids.
		/// </summary>
		/// <remarks>
		/// An entry is added to the ArrayList of both total and detail HashTables at the location of the Profile Ids specified by the given total
		/// and detail Ids.  The ArrayLists in the HashTables are created upon first usage.
		/// </remarks>
		/// <param name="aSetId">
		/// The Set Id to add a cross reference for.
		/// </param>
		/// <param name="aGradeId">
		/// The Grade Id to add a cross reference for.
		/// </param>
		/// <param name="aStoreId">
		/// The Store Id to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(int aSetId, int aGradeId, int aStoreId)
		{
			try
			{
				AddXRefIdEntry(new SetGradeStoreXRefTotalEntry(aSetId, aGradeId), aStoreId);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds the total/detail cross-reference using the given total Id and list of detail Ids.
		/// </summary>
		/// <remarks>
		/// This routine loops through all detail Ids on the given list and calls AddXRefIdEntry with each total/detail pair.
		/// </remarks>
		/// <param name="aTotalList">
		/// The list of SetGradeStoreXRefTotalEntry of the totals to add a cross reference for.
		/// </param>
		/// <param name="aStoreId">
		/// The Store Id to add a cross reference for.
		/// </param>

		virtual public void AddXRefIdEntry(ArrayList aTotalList, int aStoreId)
		{
			try
			{
				foreach (SetGradeStoreXRefTotalEntry totalEntry in aTotalList)
				{
					AddXRefIdEntry(totalEntry, aStoreId);
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
	/// This class defines the detail hash entry for a SetGradeStoreXRefTotalEntry.
	/// </summary>

	[Serializable]
	public class SetGradeStoreXRefTotalEntry
	{
		//=======
		// FIELDS
		//=======

		private int _setId;
		private int _gradeId;

		//=============
		// CONSTRUCTORS
		//=============

		public SetGradeStoreXRefTotalEntry(int aSetId, int aGradeId)
		{
			_setId = aSetId;
			_gradeId = aGradeId;
		}

		//===========
		// PROPERTIES
		//===========

		public int SetId
		{
			get
			{
				return _setId;
			}
		}

		public int GradeId
		{
			get
			{
				return _gradeId;
			}
		}

		//========
		// METHODS
		//========

		override public bool Equals(object obj)
		{
			return (_setId == ((SetGradeStoreXRefTotalEntry)obj)._setId &&
				_gradeId == ((SetGradeStoreXRefTotalEntry)obj)._gradeId);
		}

		override public int GetHashCode()
		{
			return Include.CreateHashKey(_setId, _gradeId);
		}
	}

	/// <summary>
	/// This class defines the total hash entry for a SetGradeStoreXRefDetailHashEntry.
	/// </summary>

	[Serializable]
	public class SetGradeStoreXRefDetailHashEntry
	{
		//=======
		// FIELDS
		//=======

		private ArrayList _totalList;
		private ProfileXRef _totalXRef;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of SetGradeStoreXRefDetailHashEntry.
		/// </summary>

		public SetGradeStoreXRefDetailHashEntry()
		{
			_totalList = new ArrayList();
			_totalXRef = new ProfileXRef(eProfileType.StoreGroupLevel, eProfileType.StoreGrade);
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the ArrayList containing the total list of SetGradeStoreXRefTotalEntry objects.
		/// </summary>

		public ArrayList TotalList
		{
			get
			{
				return _totalList;
			}
		}

		/// <summary>
		/// Gets the ProfileXRef containing the primary to secondary total relationship.
		/// </summary>

		public ProfileXRef TotalXRef
		{
			get
			{
				return _totalXRef;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// This method adds a SetGradeStoreXRefTotalEntry object to the ArrayList and ProfileXRef.
		/// </summary>
		/// <param name="aTotalEntry">
		/// The SetGradeStoreXRefTotalEntry to add.
		/// </param>

		public void Add(SetGradeStoreXRefTotalEntry aTotalEntry)
		{
			try
			{
				_totalList.Add(aTotalEntry);
				_totalXRef.AddXRefIdEntry(aTotalEntry.SetId, aTotalEntry.GradeId);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method returns a boolean indicating if the given SetGradeStoreXRefTotalEntry exists in the ArrayList.
		/// </summary>
		/// <param name="aTotalEntry">
		/// The SetGradeStoreXRefTotalEntry to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the given SetGradeStoreXRefTotalEntry exists in the ArrayList.
		/// </returns>

		public bool Contains(SetGradeStoreXRefTotalEntry aTotalEntry)
		{
			try
			{
				return _totalList.Contains(aTotalEntry);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
	//End TT#2 - JScott - Assortment Planning - Phase 2
}
