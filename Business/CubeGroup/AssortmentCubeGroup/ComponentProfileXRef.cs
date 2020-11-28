using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business
{
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
	public class ComponentProfileXRef : BaseProfileXRef
	{
		//=======
		// FIELDS
		//=======

		private eCubeType _detailCubeType;
		private eProfileType[] _compTypes;
		// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
		//private Hashtable _compIdHash;
		private Dictionary<int, object> _compIdHash;
		// END TT#773-MD - Stodd - replace hashtable with dictionary
		private int[] _hashCodeTypes;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ProfileXRef using the given total eProfileType and detail eProfileType.
		/// </summary>
		
		public ComponentProfileXRef(eCubeType aDetailCubeType, params eProfileType[] aCompTypes)
			: base()
		{
			int i;

			try
			{
				_detailCubeType = aDetailCubeType;
				_compTypes = aCompTypes;
				// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
				//_compIdHash = new Hashtable();
				_compIdHash = new Dictionary<int, object>();
				// END TT#773-MD - Stodd - replace hashtable with dictionary
				_hashCodeTypes = new int[_compTypes.Length];

				for (i = 0; i < _compTypes.Length; i++)
				{
					_hashCodeTypes[i] = (int)_compTypes[i];
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ComponentProfileXRef(eCubeType aDetailCubeType, ComponentProfileXRef aCompProfXRef)
			: base()
		{
			try
			{
				_detailCubeType = aDetailCubeType;
				_compTypes = aCompProfXRef._compTypes;
				_compIdHash = aCompProfXRef._compIdHash;
				_hashCodeTypes = aCompProfXRef._hashCodeTypes;
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
			return Include.CreateHashKey((int)_detailCubeType.Id);
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
			if (this.GetType() == obj.GetType() || obj.GetType().IsSubclassOf(this.GetType()))
			{
				return (_detailCubeType == ((ComponentProfileXRef)obj)._detailCubeType);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns the list of detail Ids for a given component IDs.
		/// </summary>
        /// <param name="aCellRef">
		/// The Component IDs to get a detail list for.
		/// </param>
		/// <returns>
		/// The list of detail Ids.
		/// </returns>

		virtual public ArrayList GetDetailList(AssortmentCellReference aCellRef)
		{
			try
			{
				return GetTotalList(aCellRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns the list of total Ids for a given Component IDs.
		/// </summary>
        /// <param name="aCellRef">
		/// The Component IDs to get a total list for.
		/// </param>
		/// <returns>
		/// the list of total Ids.
		/// </returns>

		virtual public ArrayList GetTotalList(AssortmentCellReference aCellRef)
		{
			int[] componentIds;
			ArrayList outList;
			// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
			//Hashtable outKeys;
			Dictionary<eProfileType, int> outKeys;
			// END TT#773-MD - Stodd - replace hashtable with dictionary
			try
			{
				componentIds = intCellRefToIds(aCellRef);

				if (_compTypes.Length == componentIds.Length)
				{
					outList = new ArrayList();
					// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
					//outKeys = new Hashtable();
					outKeys = new Dictionary<eProfileType, int>();
					// END TT#773-MD - Stodd - replace hashtable with dictionary
					intGetXRefList(outList, outKeys, _compIdHash, componentIds, 0);

					return outList;
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

		virtual public void AddXRefIdEntry(params int[] aComponentIds)
		{
			try
			{
				intAddXRefEntry(this._compIdHash, aComponentIds, 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private int[] intCellRefToIds(AssortmentCellReference aCellRef)
		{
			int[] componentIds;
			int i;
			int dimIdx;

			try
			{
				componentIds = new int[_compTypes.Length];

				for (i = 0; i < _compTypes.Length; i++)
				{
					dimIdx = aCellRef.Cube.GetDimensionProfileTypeIndex(_compTypes[i]);

					if (dimIdx != -1)
					{
						componentIds[i] = aCellRef[dimIdx];
					}
					else
					{
						componentIds[i] = dimIdx;
					}
				}

				return componentIds;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
		//private void intAddXRefEntry(Hashtable aComponentHash, int[] aComponentIds, int aCurrIdx)
		private void intAddXRefEntry(Dictionary<int, object> aComponentHash, int[] aComponentIds, int aCurrIdx)
		// END TT#773-MD - Stodd - replace hashtable with dictionary
		{
			object compHash;

			try
			{
				// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
				//compHash = aComponentHash[aComponentIds[aCurrIdx]];
				if (!aComponentHash.TryGetValue(aComponentIds[aCurrIdx], out compHash))
				{
					compHash = null;
				}
				// END TT#773-MD - Stodd - replace hashtable with dictionary
				if (aCurrIdx < aComponentIds.Length - 1)
				{
					if (compHash == null)
					{
						// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
						//compHash = new Hashtable();
						compHash = new Dictionary<int, object>();
						// END TT#773-MD - Stodd - replace hashtable with dictionary
						aComponentHash.Add(aComponentIds[aCurrIdx], compHash);
					}

					// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
					//intAddXRefEntry((Hashtable)compHash, aComponentIds, aCurrIdx + 1);
					intAddXRefEntry((Dictionary<int, object>)compHash, aComponentIds, aCurrIdx + 1);
					// END TT#773-MD - Stodd - replace hashtable with dictionary
				}
				else
				{
					if (compHash == null)
					{
						aComponentHash.Add(aComponentIds[aCurrIdx], 0);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
		//private void intGetXRefList(ArrayList aOutList, Hashtable aOutKeys, Hashtable aComponentHash, int[] aComponentIds, int aCurrIdx)
		private void intGetXRefList(ArrayList aOutList, Dictionary<eProfileType, int> aOutKeys, Dictionary<int, object> aComponentHash, int[] aComponentIds, int aCurrIdx)
		// END TT#773-MD - Stodd - replace hashtable with dictionary
		{
			object compHash;
			IDictionaryEnumerator iEnum;

			try
			{
				if (aComponentIds[aCurrIdx] != -1)
				{
					// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
					//compHash = aComponentHash[aComponentIds[aCurrIdx]];
					if (!aComponentHash.TryGetValue(aComponentIds[aCurrIdx], out compHash))
					{
						compHash = null;
					}
					// END TT#773-MD - Stodd - replace hashtable with dictionary

					if (compHash != null)
					{
						aOutKeys[_compTypes[aCurrIdx]] = aComponentIds[aCurrIdx];

						if (aCurrIdx < aComponentIds.Length - 1)
						{
							// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
							intGetXRefList(aOutList, aOutKeys, (Dictionary<int, object>)compHash, aComponentIds, aCurrIdx + 1);
							//intGetXRefList(aOutList, aOutKeys, (Hashtable)compHash, aComponentIds, aCurrIdx + 1);
							// END TT#773-MD - Stodd - replace hashtable with dictionary
						}
						else
						{	
							// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
							//aOutList.Add(aOutKeys.Clone());
							aOutList.Add(new Dictionary<eProfileType, int>(aOutKeys));
							// END TT#773-MD - Stodd - replace hashtable with dictionary
						}
					}
				}
				else
				{
					iEnum = aComponentHash.GetEnumerator();

					while (iEnum.MoveNext())
					{
						aOutKeys[_compTypes[aCurrIdx]] = (int)iEnum.Key;

						if (aCurrIdx < aComponentIds.Length - 1)
						{
							// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
							//intGetXRefList(aOutList, aOutKeys, (Hashtable)iEnum.Value, aComponentIds, aCurrIdx + 1);
							intGetXRefList(aOutList, aOutKeys, (Dictionary<int, object>)iEnum.Value, aComponentIds, aCurrIdx + 1);
							// END TT#773-MD - Stodd - replace hashtable with dictionary
						}
						else
						{
							// BEGIN TT#773-MD - Stodd - replace hashtable with dictionary
							//aOutList.Add(aOutKeys.Clone());
							aOutList.Add(new Dictionary<eProfileType, int>(aOutKeys));
							// END TT#773-MD - Stodd - replace hashtable with dictionary
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
	}
}
