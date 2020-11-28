using System;
using System.Collections;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The CubeWaferCoordinate defines a single eWaferCoordinateType and int pair that describe a global, row, or column coordinate.
	/// </summary>
	/// <remarks>
	/// This class is used to define a specific coordinate for a global, row, or column entry.  The collection of all of the Cell's
	/// CubeWaferCoordinates from the global entry and its specific row and column entries, define the logical coordinates for the Cell.
	/// </remarks>

	[Serializable]
	public class CubeWaferCoordinate
	{
		//=======
		// FIELDS
		//=======

		private eProfileType _waferCoordinateType;
		private int _key;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of CubeWaferCoordinate using the given eWaferCoordinateType and integer.
		/// </summary>
		/// <param name="aWaferCoordinateType">
		/// The eWaferCoordinateType that identifies the type of profile for this coordinate.
		/// </param>
		/// <param name="aKey">
		/// The integer that identifies the logical RID of this coordinate.
		/// </param>

		public CubeWaferCoordinate(eProfileType aWaferCoordinateType, int aKey)
		{
			_waferCoordinateType = aWaferCoordinateType;
			_key = aKey;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the eWaferCoordinateType of this coordinate.
		/// </summary>

		public eProfileType WaferCoordinateType
		{
			get
			{
				return _waferCoordinateType;
			}
		}

		/// <summary>
		/// Gets the integer key of this coordinate.
		/// </summary>

		public int Key
		{
			get
			{
				return _key;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Creates a copy of this CubeWaferCoordinate.
		/// </summary>
		/// <returns>
		/// A new instance of CubeWaferCoordinate with a copy of this objects information.
		/// </returns>

		public CubeWaferCoordinate Copy()
		{
			try
			{
				return new CubeWaferCoordinate(_waferCoordinateType, _key);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The CubeWaferCoordinateList class contains a collection of CubeWaferCoordinate objects.
	/// </summary>
	/// <remarks>
	/// Each global, row, and column of a grid can identify a CubeWaferCoordinate or CubeWaferCoordinates for a Cell.  The collection of these CubeWaferCoordinates is
	/// defined in this CubeWaferCoordinateList class.
	/// </remarks>

	[Serializable]
	public class CubeWaferCoordinateList : ArrayList
	{
		//=======
		// FIELDS
		//=======

		private Hashtable _coordinateTypeHash;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of CubeWaferCoordinateList.
		/// </summary>

		public CubeWaferCoordinateList()
			: base()
		{
			_coordinateTypeHash = new Hashtable();
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the CubeWaferCoordinate at the given integer index.
		/// </summary>

		new public CubeWaferCoordinate this[int aIndex]
		{
			get
			{
				try
				{
					return (CubeWaferCoordinate)base[aIndex];
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
		
		//========
		// METHODS
		//========

		/// <summary>
		/// Creates a copy of this CubeWaferCoordinateList.
		/// </summary>
		/// <returns>
		/// A new instance of CubeWaferCoordinateList with a copy of this objects information.
		/// </returns>

		public CubeWaferCoordinateList Copy()
		{
			CubeWaferCoordinateList waferCoordinateList;

			try
			{
				waferCoordinateList = new CubeWaferCoordinateList();

				foreach (CubeWaferCoordinate waferCoordinate in this)
				{
					waferCoordinateList.Add(waferCoordinate.Copy());
				}

				return waferCoordinateList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds the given CubeWaferCoordinate to this collection.
		/// </summary>
		/// <param name="aCubeWaferCoor">
		/// The CubeWaferCoordinate to add.
		/// </param>

		public void Add(CubeWaferCoordinate aCubeWaferCoor)
		{
			try
			{
				_coordinateTypeHash.Add(aCubeWaferCoor.WaferCoordinateType, aCubeWaferCoor);
				base.Add(aCubeWaferCoor);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Finds the CubeWaferCoordinate for the given eProfileType.
		/// </summary>
		/// <param name="aCoorProfType">
		/// The eProfileType to find.
		/// </param>
		/// <returns>
		/// The CubeWaferCoordinate that cooresponds to the given eProfileType.
		/// </returns>

		public CubeWaferCoordinate FindCoordinateType(eProfileType aCoorProfType)
		{
			try
			{
				return (CubeWaferCoordinate)_coordinateTypeHash[aCoorProfType];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//Begin Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw

		public override bool Equals(object obj)
		{
			CubeWaferCoordinateList inList;
			IDictionaryEnumerator iEnum;
			CubeWaferCoordinate myCoor;
			CubeWaferCoordinate inCoor;

			try
			{
				inList = (CubeWaferCoordinateList)obj;

				if (_coordinateTypeHash.Count != inList._coordinateTypeHash.Count)
				{
					return false;
				}

				iEnum = _coordinateTypeHash.GetEnumerator();

				while (iEnum.MoveNext())
				{
					inCoor = (CubeWaferCoordinate)inList._coordinateTypeHash[iEnum.Key];

					if (inCoor == null)
					{
						return false;
					}

					myCoor = (CubeWaferCoordinate)iEnum.Value;

					if (inCoor.Key != myCoor.Key)
					{
						return false;
					}
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6124 - JScott - After Sort, Week is not visible in OTS Frcst Rvw
	}

	/// <summary>
	/// The CubeWaferCoordinateListGroup class contains a collection of CubeWaferCoordinateList objects.
	/// </summary>
	/// <remarks>
	/// This class allows for each column and row to have different collections of CubeWaferCoordinateList objects.
	/// </remarks>

	[Serializable]
	public class CubeWaferCoordinateListGroup : ArrayList
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of CubeWaferCoordinateListGroup.
		/// </summary>

		public CubeWaferCoordinateListGroup()
			: base()
		{
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the CubeWaferCoordinateList at the given integer index.
		/// </summary>
		
		new public CubeWaferCoordinateList this[int aIndex]
		{
			get
			{
				try
				{
					return (CubeWaferCoordinateList)base[aIndex];
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The CubeWafer class defines the entire request for a wafer of data from a PlanCubeGroup.
	/// </summary>
	/// <remarks>
	/// This class is used to define the layout of an array of Cells that will be returned from a call to the PlanCubeGroup.  This class consists of: 1) a
	/// eCubeType field that indicates the cube type to retrieve the data from; 2) a global WaferCoordinateList that holds all WaferCoordinates that are global
	/// to all requested Cells; and 3) a WaferCoordinateListGroup for both rows and columns that contain a WaferCoordinateList for each row or column.
	/// </remarks>

	[Serializable]
	public class CubeWafer
	{
		//=======
		// FIELDS
		//=======

		private CubeWaferCoordinateList _commonWaferCoordinateList;
		private CubeWaferCoordinateListGroup _rowWaferCoordinateListGroup;
		private CubeWaferCoordinateListGroup _colWaferCoordinateListGroup;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of CubeWafer.
		/// </summary>

		public CubeWafer()
		{
			try
			{
				_rowWaferCoordinateListGroup = new CubeWaferCoordinateListGroup();
				_colWaferCoordinateListGroup = new CubeWaferCoordinateListGroup();
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
		/// Gets the global WaferCoordinateList.
		/// </summary>

		public CubeWaferCoordinateList CommonWaferCoordinateList
		{
			get
			{
				return _commonWaferCoordinateList;
			}
			set
			{
				_commonWaferCoordinateList = value;
			}
		}

		/// <summary>
		/// Gets the row's WaferCoordinateListGroup.
		/// </summary>

		public CubeWaferCoordinateListGroup RowWaferCoordinateListGroup
		{
			get
			{
				return _rowWaferCoordinateListGroup;
			}
		}

		/// <summary>
		/// Gets the columns's WaferCoordinateListGroup.
		/// </summary>

		public CubeWaferCoordinateListGroup ColWaferCoordinateListGroup
		{
			get
			{
				return _colWaferCoordinateListGroup;
			}
		}

		//========
		// METHODS
		//========
	}
}
