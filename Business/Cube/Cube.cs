using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using System.Diagnostics;

namespace MIDRetail.Business
{
	/// <summary>
	/// Delegate that defines the structure of a call back routine used during recursion of the Cube.
	/// </summary>

	public delegate void RecurseCallbackDelegate(CellReference aCellRef, RecurseCubeArguments aRecurseArguments);
	public delegate void AddCellRefToListDelegate(ArrayList aCellRefList, CellReference aCellRef, object aAddCellRefParm);

	/// <summary>
	/// Abstract class that defines the base definition of a cube of data.
	/// </summary>

	abstract public class Cube
	{
		//=======
		// FIELDS
		//=======

		protected SessionAddressBlock _SAB;
		protected ApplicationSessionTransaction _transaction;
		protected CubeGroup _cubeGroup;
		protected CubeDefinition _cubeDef;
		protected Hashtable[] _logicalToPhysicalHash;
		protected Hashtable[] _physicalToLogicalHash;
		protected object[] _cubeRoot;
		protected Hashtable _cubeRelationships;
		protected Hashtable _totalCubeTypes;
		protected eCubeType _componentDetailCubeType;
		protected eCubeType _spreadDetailCubeType;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of Cube using the given CubeAddressBlock and CubeDefinition.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to the SessionAddressBlock which contains pointers to the other available Sessions.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to the Transaction for this object.
		/// </param>
		/// <param name="aCubeGroup">
		/// A reference to the CubeGroup that owns this Cube.
		/// </param>
		/// <param name="aCubeDef">
		/// A reference to a CubeDefinition, which describes the structure of this cube.
		/// </param>

		public Cube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, CubeGroup aCubeGroup, CubeDefinition aCubeDef)
		{
			int i;

			try
			{
				_SAB = aSAB;
				_transaction = aTransaction;
				_cubeGroup = aCubeGroup;
				_cubeDef = aCubeDef;
				_logicalToPhysicalHash = new Hashtable[_cubeDef.NumDimensions];
				_physicalToLogicalHash = new Hashtable[_cubeDef.NumDimensions];
				_cubeRelationships = new Hashtable();
				_totalCubeTypes = new Hashtable();
				_componentDetailCubeType = eCubeType.None;
				_spreadDetailCubeType = eCubeType.None;

				for (i = 0; i < _cubeDef.NumDimensions; i++)
				{
					_logicalToPhysicalHash[i] = new Hashtable();
					_physicalToLogicalHash[i] = new Hashtable();
				}
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
		/// Gets the eCubeType of this cube.
		/// </summary>

		abstract public eCubeType CubeType { get ; }

		/// <summary>
		/// Gets the SessionAddressBlock associated with this Cube.
		/// </summary>

		public SessionAddressBlock SAB
		{
			get
			{
				return _SAB;
			}
		}

		/// <summary>
		/// Gets the Transaction associates with this Cube.
		/// </summary>

		public ApplicationSessionTransaction Transaction
		{
			get
			{
				return _transaction;
			}
		}

		/// <summary>
		/// Gets the CubeGroup associated with this Cube.
		/// </summary>

		public CubeGroup CubeGroup
		{
			get
			{
				return _cubeGroup;
			}
		}

		/// <summary>
		/// Gets the CubeDefintion for this Cube.
		/// </summary>

		public CubeDefinition CubeDefinition
		{
			get
			{
				return _cubeDef;
			}
		}

		/// <summary>
		/// Gets the LogicalToPhysicalHash HashTable that correlates the logical Id to the physical array dimension.
		/// </summary>

		public Hashtable[] LogicalToPhysicalHash
		{
			get
			{
				return _logicalToPhysicalHash;
			}
		}

		/// <summary>
		/// Gets the PhysicalToLogicalHash HashTable that correlates the physical array dimension to the logical Id.
		/// </summary>

		public Hashtable[] PhysicalToLogicalHash
		{
			get
			{
				return _physicalToLogicalHash;
			}
		}

		/// <summary>
		/// Gets the Hashtable containing CubeRelationship objects that define relationships between the cubes.
		/// </summary>

		public Hashtable CubeRelationships
		{
			get
			{
				return _cubeRelationships;
			}
		}

		/// <summary>
		/// Gets the Hashtable containing eCubeType objects that point to total cubes for this cube.
		/// </summary>

		public Hashtable TotalCubeTypes
		{
			get
			{
				return _totalCubeTypes;
			}
		}

		/// <summary>
		/// Gets the eCubeType object that points to the primary detail cube for this cube.
		/// </summary>

		public eCubeType ComponentDetailCubeType
		{
			get
			{
				return _componentDetailCubeType;
			}
		}

		/// <summary>
		/// Gets the eCubeType object that points to the secondary detail cube for this cube.
		/// </summary>

		public eCubeType SpreadDetailCubeType
		{
			get
			{
				return _spreadDetailCubeType;
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a CellCoordinates are created.
		/// </summary>
		/// <returns>
		/// A reference to a new CellCoordinates object.
		/// </returns>

		abstract public CellCoordinates CreateCellCoordinates(int aNumIndices);

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a Cell is created.
		/// </summary>
		/// <returns>
		/// A reference to a new Cell.
		/// </returns>

		abstract public Cell CreateCell(CellReference aCellReference);

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a CellReference is created.
		/// </summary>
		/// <returns>
		/// A reference to a new CellReference.
		/// </returns>

		abstract public CellReference CreateCellReference();

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a CellReference is created.
		/// </summary>
		/// <param name="aCoordinates">
		/// A reference to a CellCoordinates object that identifies the Cell.
		/// </param>
		/// <returns>
		/// A reference to a new CellReference.
		/// </returns>

		abstract public CellReference CreateCellReference(CellCoordinates aCoordinates);

		/// <summary>
		/// Abstract method that allows the inheritor the ability to dictate the way a CellReference is created.
		/// </summary>
		/// <param name="aCellRef">
		/// A reference to a CellReference object that identifies the Cell.
		/// </param>
		/// <returns>
		/// A reference to a new CellReference.
		/// </returns>

		abstract public CellReference CreateCellReference(CellReference aCellRef);

		/// <summary>
		/// Gets the dimension index for the given eProfileType.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType to find the index for.
		/// </param>
		/// <returns>
		/// The dimension index of the given eProfileType.
		/// </returns>

		virtual public int GetDimensionProfileTypeIndex(eProfileType aProfileType)
		{
			try
			{
				return CubeDefinition.GetDimensionProfileTypeIndex(aProfileType);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the coordinate for a given eProfileType.  Virtual.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType to find the coordinate for.
		/// </param>
		/// <returns>
		/// The coordinate for the eProfileType.
		/// </returns>

		public int GetCoordinate(CellReference aCellReference, eProfileType aProfileType, int aDimIdx)
		{
			try
			{
				return aCellReference.CellCoordinates.GetCoordinate(aProfileType, aDimIdx);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the coordinate for a given eProfileType.  Virtual.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType to set the coordinate for.
		/// </param>
		/// <param name="aValue">
		/// The value to assign to the coordinate.
		/// </param>
		
		public void SetCoordinate(CellReference aCellReference, eProfileType aProfileType, int aDimIdx, int aValue)
		{
			try
			{
				aCellReference.CellCoordinates.SetCoordinate(aProfileType, aDimIdx, aValue);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Expands a dimension size so that future creations/access will create the new length.
		/// </summary>
		/// <param name="aCubeDef">
		/// The CubeDefinition to expand the cube with.
		/// </param>

		public void ExpandDimensionSize(CubeDefinition aCubeDef)
		{
			int i;

			try
			{
				for (i = 0; i < _cubeDef.NumDimensions; i++)
				{
					if (aCubeDef[i].DimensionSize > _cubeDef[i].DimensionSize)
					{
						_cubeDef[i].DimensionSize = aCubeDef[i].DimensionSize;
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
		/// Expands a dimension size so that future creations/access will create the new length.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType to set the size for.
		/// </param>
		/// <param name="aIncrement">
		/// The additional entries in the dimension.
		/// </param>

		public void ExpandDimensionSize(eProfileType aProfileType, int aIncrement)
		{
			int dimensionIdx;

			try
			{
				dimensionIdx = GetDimensionProfileTypeIndex(aProfileType);

				if (dimensionIdx != -1)
				{
					_cubeDef[dimensionIdx].DimensionSize = _cubeDef[dimensionIdx].DimensionSize + aIncrement;
				}
				else
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_DimensionNotDefinedOnCube,
						MIDText.GetText(eMIDTextCode.msg_pl_DimensionNotDefinedOnCube));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Expands a dimension size so that future creations/access will create the new length.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType to set the size for.
		/// </param>
		/// <param name="aProfileList">
		/// A ProfileList of Profiles who's keys will be added to the logical-to-physical hashtable.  The new length will become the new DimesionSize.
		/// </param>

		public void ExpandDimensionSize(eProfileType aProfileType, ProfileList aProfileList)
		{
			int dimensionIdx;

			try
			{
				dimensionIdx = GetDimensionProfileTypeIndex(aProfileType);

				if (dimensionIdx != -1)
				{
					foreach (Profile prof in aProfileList)
					{
						intGetPhysicalCoordinate(prof.Key, dimensionIdx);
					}

					_cubeDef[dimensionIdx].DimensionSize = System.Math.Max(_cubeDef[dimensionIdx].DimensionSize, _logicalToPhysicalHash[dimensionIdx].Count);
				}
				else
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_DimensionNotDefinedOnCube,
						MIDText.GetText(eMIDTextCode.msg_pl_DimensionNotDefinedOnCube));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds an entry to the CubeRelationship table that associates another cube to this cube.
		/// </summary>
		/// <param name="aCubeRel">
		/// The CubeRelationship to add.
		/// </param>

		public void AddRelationship(CubeRelationship aCubeRel)
		{
			try
			{
				_cubeRelationships[aCubeRel.CubeType] = aCubeRel;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds an entry to the TotalCubeType table that associates a total cube to this detail cube.
		/// </summary>
		/// <param name="aTotalCubeType">
		/// The eCubeType of the total cube.
		/// </param>

		public void AddTotalCube(eCubeType aTotalCubeType)
		{
			try
			{
				_totalCubeTypes[aTotalCubeType] = aTotalCubeType;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the DetailCubeType that associates a primary detail cube to this total cube.
		/// </summary>
		/// <param name="aDetailCubeType">
		/// The eCubeType of the detail cube.
		/// </param>
		
		public void SetComponentDetailCube(eCubeType aDetailCubeType)
		{
			_componentDetailCubeType = aDetailCubeType;
		}

		/// <summary>
		/// Sets the DetailCubeType that associates a secondary detail cube to this total cube.
		/// </summary>
		/// <param name="aDetailCubeType">
		/// The eCubeType of the detail cube.
		/// </param>
		
		public void SetSpreadDetailCube(eCubeType aDetailCubeType)
		{
			_spreadDetailCubeType = aDetailCubeType;
		}

		/// <summary>
		/// Clears the contents of the Cube, but not the definition.
		/// </summary>

		virtual public void Clear()
		{
			_cubeRoot = null;
		}

		/// <summary>
		/// Clears the contents of the Cube, starting at the nth dimension of the given CellReference.
		/// </summary>

		public void Clear(CellReference aCellReference, int aNumDimension)
		{
			int[] physicalCoordinates;
			int i;
			object[] branch;

			try
			{
				if (aCellReference.CellCoordinates.NumIndices != _cubeDef.NumDimensions ||
					aNumDimension > _cubeDef.NumDimensions ||
					aNumDimension < 1)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_InvalidNumberOfIndices,
						MIDText.GetText(eMIDTextCode.msg_pl_InvalidNumberOfIndices));
				}

				physicalCoordinates = intGetPhysicalCoordinates(aCellReference.CellCoordinates, aNumDimension);

				// Check all dimensions to see if they've been created

				branch = _cubeRoot;
	
				for (i = 0; i < _cubeDef.NumDimensions - 1 && i < aNumDimension - 1 && branch != null && physicalCoordinates[i] < branch.Length; i++)
				{
					branch = (object[])(branch[physicalCoordinates[i]]);
				}

				if (branch != null)
				{
					branch[physicalCoordinates[i]] = null;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines whether a cell identified by the given CellCoordinates physically exists.
		/// </summary>
		/// <remarks>
		/// Cells are created at the time they are needed, creating a sparse cube.  This method determines if a
		/// specific Cell has been created yet.
		/// </remarks>
		/// <param name="aCellReference">
		/// The CellReference that identifies the Cell.
		/// </param>
		/// <returns>
		/// A bool indicating if the Cell exists.
		/// </returns>

		public bool doesCellExist(CellReference aCellReference)
		{
			return doesCellExist(aCellReference.CellCoordinates);
		}

		/// <summary>
		/// Determines whether a cell identified by the given CellCoordinates physically exists.
		/// </summary>
		/// <remarks>
		/// Cells are created at the time they are needed, creating a sparse cube.  This method determines if a
		/// specific Cell has been created yet.
		/// </remarks>
		/// <param name="aCellCoordinates">
		/// The CellReference that identifies the Cell.
		/// </param>
		/// <returns>
		/// A bool indicating if the Cell exists.
		/// </returns>

		public bool doesCellExist(CellCoordinates aCellCoordinates)
		{
			int[] physicalCoordinates;
			int i;
			object[] branch;

			try
			{
				if (aCellCoordinates.NumIndices != _cubeDef.NumDimensions)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_InvalidNumberOfIndices,
						MIDText.GetText(eMIDTextCode.msg_pl_InvalidNumberOfIndices));
				}

				physicalCoordinates = intGetPhysicalCoordinates(aCellCoordinates);

				// Check all dimensions to see if they've been created

				branch = _cubeRoot;
	
				for (i = 0; i < _cubeDef.NumDimensions - 1 && branch != null && physicalCoordinates[i] < branch.Length; i++)
				{
					branch = (object[])(branch[physicalCoordinates[i]]);
				}

				if (branch == null || physicalCoordinates[i] >= branch.Length)
				{
					return false;
				}

				if (branch[physicalCoordinates[_cubeDef.NumDimensions - 1]] == null)
				{
					return false;
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a reference to the Cell requested by the given CellCoordinates.
		/// </summary>
		/// <remarks>
		/// Cells are created at the time they are needed, creating a sparse cube.  This method will retrieve a Cell
		/// and create it, if necessary.
		/// </remarks>
		/// <param name="aCellReference">
		/// The reference to the Cell.
		/// </param>
		/// <returns>
		/// The reference to the Cell.
		/// </returns>

		public Cell GetCell(CellReference aCellReference)
		{
			int[] physicalCoordinates;
			object[] cellArray;

			try
			{
				if (aCellReference.CellCoordinates.NumIndices != _cubeDef.NumDimensions)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_InvalidNumberOfIndices,
						MIDText.GetText(eMIDTextCode.msg_pl_InvalidNumberOfIndices));
				}

				physicalCoordinates = intGetPhysicalCoordinates(aCellReference.CellCoordinates);
				cellArray = intGetCellArrayInCube(physicalCoordinates);

				if (cellArray[physicalCoordinates[_cubeDef.NumDimensions - 1]] == null)
				{
					cellArray[physicalCoordinates[_cubeDef.NumDimensions - 1]] = CreateCell(aCellReference);
				}

				return (Cell)cellArray[physicalCoordinates[_cubeDef.NumDimensions - 1]];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Starting point for recurse functionality that will recursively traverse the Cube.
		/// </summary>
		/// <remarks>
		/// The recursive traverse of the Cube will inspect every branch of the Cube, and call the given RecurseCubeArguments
		/// delegate for every existing Cell.
		/// </remarks>
		/// <param name="aRecurseArguments">
		/// RecurseCubeArguments class containing callback information.
		/// </param>

		public void RecurseCubeExisting(RecurseCubeArguments aRecurseArguments)
		{
			CellCoordinates cellCoordinates;

			try
			{
				if (_cubeRoot != null)
				{
					cellCoordinates = this.CreateCellCoordinates(CubeDefinition.NumDimensions);
					intRecurseCubeExisting(aRecurseArguments, cellCoordinates, 0, _cubeRoot);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Starting point for recurse functionality that will recursively traverse the Cube.
		/// </summary>
		/// <remarks>
		/// The recursive traverse of the Cube will inspect every branch of the Cube, and call the given RecurseCubeArguments
		/// delegate for every existing Cell.
		/// </remarks>
		/// <param name="aRecurseArguments">
		/// RecurseCubeArguments class containing callback information.
		/// </param>

		public void RecurseCubeMaster(RecurseCubeArguments aRecurseArguments)
		{
			CellCoordinates cellCoordinates;

			try
			{
				cellCoordinates = this.CreateCellCoordinates(CubeDefinition.NumDimensions);
				intRecurseCubeMaster(aRecurseArguments, cellCoordinates, 0);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an ArrayList of CellReference objects of all the total cells for the given total cube that are associated with the given Cell using
		/// the CubeRelationship objects that identify the total cells.
		/// </summary>
		/// <remarks>
		/// For the CubeRelationship associated with the given total cube, the ProfileXRef is retrieved from the Transaction object.  A CellReference object
		/// is added for each total Id identified on the ProfileXRef.
		/// </remarks>
		/// <param name="aCellRef">
		/// A reference to a CellReference object that describes the Cell to retrieve total cells for.
		/// </param>
		/// <returns>
		/// An ArrayList of CellReference objects pointing to total Cells.
		/// </returns>

		public ArrayList GetTotalCellRefArray(CellReference aCellRef, eCubeType aTotalCubeType)
		{
			AllSelector cellSelector;
			CellReference totalRef;
			CubeRelationship cubeRel;
			Cube cube;
			bool cancel;

			try
			{
				cellSelector = new AllSelector();

				cubeRel = (CubeRelationship)_cubeRelationships[aTotalCubeType];
				if (cubeRel != null)
				{
					cube = CubeGroup.GetCube(cubeRel.CubeType);
					if (cube != null)
					{
						totalRef = cube.CreateCellReference(aCellRef);
						cubeRel.ProcessTotalSelector(aCellRef, totalRef, cellSelector, out cancel);
					}
				}

				return cellSelector.CellRefList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an ArrayList of CellReference objects of all the total cells that are affected by a change to the given Cell using
		/// the eCubeType objects that identify the total cells.
		/// </summary>
		/// <remarks>
		/// For the CubeRelationship associated with each eCubeType defined as a total, the ProfileXRef is retrieved from the Transaction object.  A
		/// CellReference object is added for each total Id identified on the ProfileXRef.
		/// </remarks>
		/// <param name="aCellRef">
		/// A reference to a CellReference object that describes the Cell to retrieve total cells for.
		/// </param>
		/// <returns>
		/// An ArrayList of CellReference objects pointing to total Cells.
		/// </returns>

		public ArrayList GetTotalCellRefArray(CellReference aCellRef)
		{
			AllSelector cellSelector;
			CellReference totalRef;
			IDictionaryEnumerator totEnum;
			CubeRelationship cubeRel;
			Cube cube;
			bool cancel;

			try
			{
				cellSelector = new AllSelector();

				totEnum = _totalCubeTypes.GetEnumerator();
				while (totEnum.MoveNext())
				{
					cubeRel = (CubeRelationship)_cubeRelationships[(eCubeType)totEnum.Value];
					if (cubeRel != null)
					{
						cube = CubeGroup.GetCube(cubeRel.CubeType);
						if (cube != null)
						{
							totalRef = cube.CreateCellReference(aCellRef);
							cubeRel.ProcessTotalSelector(aCellRef, totalRef, cellSelector, out cancel);
						}
					}
				}

				return cellSelector.CellRefList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an ArrayList of CellReference objects of all the detail cells that are affected by a change to the given Cell using
		/// the CubeRelationship object that identify the detail cells.
		/// </summary>
		/// <remarks>
		/// For the CubeRelationship associated with the detail, the ProfileXRef is retrieved from the Transaction object.  A CellReference object
		/// is added for each detail Id identified on the ProfileXRef.
		/// </remarks>
		/// <param name="aCellRef">
		/// A reference to a CellReference object that describes the Cell to retrieve detail cells for.
		/// </param>
		/// <returns>
		/// An ArrayList of CellReference objects pointing to detail Cells.
		/// </returns>

		public void ProcessDetailCellSelector(CellReference aCellRef, eCubeType aCubeType, CellSelector aCellSelector)
		{
			try
			{
				intGetDetailCellRefArray(aCellRef, aCubeType, aCellSelector);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an ArrayList of CellReference objects of all the detail cells that are affected by a change to the given Cell using
		/// the CubeRelationship object that identify the detail cells.
		/// </summary>
		/// <remarks>
		/// For the CubeRelationship associated with the detail, the ProfileXRef is retrieved from the Transaction object.  A CellReference object
		/// is added for each detail Id identified on the ProfileXRef.
		/// </remarks>
		/// <param name="aCellRef">
		/// A reference to a CellReference object that describes the Cell to retrieve detail cells for.
		/// </param>
		/// <returns>
		/// An ArrayList of CellReference objects pointing to detail Cells.
		/// </returns>

		public void ProcessComponentDetailCellSelector(CellReference aCellRef, CellSelector aCellSelector)
		{
			try
			{
				intGetDetailCellRefArray(aCellRef, _componentDetailCubeType, aCellSelector);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an ArrayList of CellReference objects of all the detail cells that are affected by a change to the given Cell using
		/// the CubeRelationship object that identify the detail cells.
		/// </summary>
		/// <remarks>
		/// For the CubeRelationship associated with the detail, the ProfileXRef is retrieved from the Transaction object.  A CellReference object
		/// is added for each detail Id identified on the ProfileXRef.
		/// </remarks>
		/// <param name="aCellRef">
		/// A reference to a CellReference object that describes the Cell to retrieve detail cells for.
		/// </param>
		/// <returns>
		/// An ArrayList of CellReference objects pointing to detail Cells.
		/// </returns>

		public void ProcessSpreadDetailCellSelector(CellReference aCellRef, CellSelector aCellSelector)
		{
			try
			{
				intGetDetailCellRefArray(aCellRef, _spreadDetailCubeType, aCellSelector);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an ArrayList of CellReferences with the given ProfileList.
		/// </summary>
		/// <remarks>
		/// This method will return an ArrayList of CellReferences of all the Cells in the specified dimension in relation to
		/// the given CellReference.  Only the selected (filtered) profile Ids will be added to the ArrayList.
		/// </remarks>
		/// <param name="aCellRef">
		/// The CellReference object that identifies a Cell on the dimension array being requested.
		/// </param>
		/// <returns>
		/// The ArrayList of dimension CellReferences.
		/// </returns>

		public ArrayList GetCellRefArray(
			CellReference aCellRef,
			ProfileList aProfileList)
		{
			int i;
			int dimensionIdx;
			ArrayList selectedCellRefs;
			CellReference cellRef;

			try
			{
				selectedCellRefs = new ArrayList();
				dimensionIdx = GetDimensionProfileTypeIndex(aProfileList.ProfileType);

				if (dimensionIdx != -1)
				{
					for (i = 0; i < aProfileList.Count; i++)
					{
						cellRef = CreateCellReference(aCellRef);
						cellRef[dimensionIdx] = aProfileList[i].Key;
						selectedCellRefs.Add(cellRef);
					}

					return selectedCellRefs;
				}
				else
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_DimensionNotDefinedOnCube,
						MIDText.GetText(eMIDTextCode.msg_pl_DimensionNotDefinedOnCube));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Translates the logical, external CellCoordinates to the internal, physical CellCoordinates.
		/// </summary>
		/// <remarks>
		/// Logical coordinates are the external Ids of the profiles.  Physical coordinates are the actual indexes
		/// in the dimension arrays where those Ids are stored.  This separation between the logical and physical
		/// coordinates allows for adding logical coordinates "on the fly" and in a random fashion without having to 
		/// reserve space for them.
		/// </remarks>
		/// <param name="aLogicalCoordinates">
		/// The logical CellCoordinates.
		/// </param>
		/// <returns>
		/// The physical CellCoordinates.
		/// </returns>

		private int[] intGetPhysicalCoordinates(CellCoordinates aLogicalCoordinates)
		{
			try
			{
				return intGetPhysicalCoordinates(aLogicalCoordinates, _cubeDef.NumDimensions);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Translates the logical, external CellCoordinates to the internal, physical CellCoordinates.
		/// </summary>
		/// <remarks>
		/// Logical coordinates are the external Ids of the profiles.  Physical coordinates are the actual indexes
		/// in the dimension arrays where those Ids are stored.  This separation between the logical and physical
		/// coordinates allows for adding logical coordinates "on the fly" and in a random fashion without having to 
		/// reserve space for them.
		/// </remarks>
		/// <param name="aLogicalCoordinates">
		/// The logical CellCoordinates.
		/// </param>
		/// <returns>
		/// The physical CellCoordinates.
		/// </returns>

		private int[] intGetPhysicalCoordinates(CellCoordinates aLogicalCoordinates, int aNumDimensions)
		{
			int[] physicalCoordinates;
			int i;

			try
			{
				physicalCoordinates = new int[aLogicalCoordinates.NumIndices];

				for (i = 0; i < aNumDimensions; i++)
				{
					physicalCoordinates[i] = intGetPhysicalCoordinate(aLogicalCoordinates.GetRawCoordinate(i), i);
				}

				return physicalCoordinates;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Translates the logical, external CellCoordinates to the internal, physical CellCoordinates.
		/// </summary>
		/// <remarks>
		/// Logical coordinates are the external Ids of the profiles.  Physical coordinates are the actual indexes
		/// in the dimension arrays where those Ids are stored.  This separation between the logical and physical
		/// coordinates allows for adding logical coordinates "on the fly" and in a random fashion without having to 
		/// reserve space for them.
		/// </remarks>
        /// <param name="aLogicalCoordinate">
		/// The logical CellCoordinate.
		/// </param>
		/// <returns>
		/// The physical CellCoordinates.
		/// </returns>

		private int intGetPhysicalCoordinate(int aLogicalCoordinate, int aDimension)
		{
			object hashEntry;
			int physicalCoordinate;

			try
			{
				if (aLogicalCoordinate == -1)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_InvalidLogicalCoordinate,
						MIDText.GetText(eMIDTextCode.msg_pl_InvalidLogicalCoordinate));
				}

				hashEntry = _logicalToPhysicalHash[aDimension][aLogicalCoordinate];

				if (hashEntry != null)
				{
					physicalCoordinate = (int)hashEntry;
				}
				else
				{
					physicalCoordinate = _logicalToPhysicalHash[aDimension].Count;
					_logicalToPhysicalHash[aDimension].Add(aLogicalCoordinate, physicalCoordinate);
					_physicalToLogicalHash[aDimension].Add(physicalCoordinate, aLogicalCoordinate);
				}

				return physicalCoordinate;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a reference to the last dimension array in the cube for the specified CellCoordinates.
		/// </summary>
		/// <remarks>
		/// The arrays that comprise the different dimensions on the cube are not created until a cell in that
		/// dimension is requested, allow for the highest level of sparseness.  As each dimension is referenced,
		/// it is created as necessary.  The final dimension array is the one that is returned.
		/// </remarks>
		/// <param name="aPhysicalCoordinates">
		/// The physical CellCoordinates of the cell being retrieved.
		/// </param>
		/// <returns>
		/// A reference to the last dimension array in the Cube for the specified CellCoordinates.
		/// </returns>

		private object[] intGetCellArrayInCube(int[] aPhysicalCoordinates)
		{
			int i;
			object[] objectVector;
			object[] branch;

			try
			{
				// Create main cube vector

				if (aPhysicalCoordinates[0] >= _cubeDef[0].DimensionSize)
				{
					_cubeDef[0].DimensionSize = aPhysicalCoordinates[0] + 1;
				}

				if (_cubeRoot == null)
				{
					_cubeRoot = new object[_cubeDef[0].DimensionSize];
				}
				else
				{
					if (aPhysicalCoordinates[0] >= _cubeRoot.Length)
					{
						objectVector = new object[_cubeDef[0].DimensionSize];
						System.Array.Copy(_cubeRoot, objectVector, _cubeRoot.Length);
						_cubeRoot = objectVector;
					}
				}			

				// Loop and create middle cube vectors

				branch = _cubeRoot;

				for (i = 1; i < _cubeDef.NumDimensions; i++)
				{
					if (aPhysicalCoordinates[i] >= _cubeDef[i].DimensionSize)
					{
						_cubeDef[i].DimensionSize = aPhysicalCoordinates[i] + 1;
					}

					if (branch[aPhysicalCoordinates[i - 1]] == null)
					{
						branch[aPhysicalCoordinates[i - 1]] = new object[_cubeDef[i].DimensionSize];
					}
					else
					{
						if (aPhysicalCoordinates[i] >= ((object[])branch[aPhysicalCoordinates[i - 1]]).Length)
						{
							objectVector = new object[_cubeDef[i].DimensionSize];
							System.Array.Copy((object[])(branch[aPhysicalCoordinates[i - 1]]), objectVector, ((object[])branch[aPhysicalCoordinates[i - 1]]).Length);
							branch[aPhysicalCoordinates[i - 1]] = objectVector;
						}
					}			

					branch = (object[])(branch[aPhysicalCoordinates[i - 1]]);
				}

				// Retrieve Cell array and return

				return branch;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an ArrayList of CellReference objects of all the detail cells for the given detail cube that are associated with the given Cell using
		/// the CubeRelationship objects that identify the detail cells.
		/// </summary>
		/// <remarks>
		/// For the CubeRelationship associated with the given detail cube, the ProfileXRef is retrieved from the Transaction object.  A CellReference object
		/// is added for each detail Id identified on the ProfileXRef.
		/// </remarks>
		/// <param name="aCellRef">
		/// A reference to a CellReference object that describes the Cell to retrieve detail cells for.
		/// </param>
		/// <returns>
		/// An ArrayList of CellReference objects pointing to detail Cells.
		/// </returns>

		private ArrayList intGetDetailCellRefArray(CellReference aCellRef, eCubeType aDetailCubeType)
		{
			AllSelector cellSelector;

			try
			{
				cellSelector = new AllSelector();
				intGetDetailCellRefArray(aCellRef, aDetailCubeType, cellSelector);
				return cellSelector.CellRefList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns an ArrayList of CellReference objects of all the detail cells for the given detail cube that are associated with the given Cell using
		/// the CubeRelationship objects that identify the detail cells.
		/// </summary>
		/// <remarks>
		/// For the CubeRelationship associated with the given detail cube, the ProfileXRef is retrieved from the Transaction object.  A CellReference object
		/// is added for each detail Id identified on the ProfileXRef.
		/// </remarks>
		/// <param name="aCellRef">
		/// A reference to a CellReference object that describes the Cell to retrieve detail cells for.
		/// </param>
		/// <returns>
		/// An ArrayList of CellReference objects pointing to detail Cells.
		/// </returns>

		private void intGetDetailCellRefArray(CellReference aCellRef, eCubeType aDetailCubeType, CellSelector aCellSelector)
		{
			CellReference detailRef;
			CubeRelationship cubeRel;
			Cube cube;
			bool cancel;

			try
			{
				cubeRel = (CubeRelationship)_cubeRelationships[aDetailCubeType];
				if (cubeRel != null)
				{
					cube = CubeGroup.GetCube(cubeRel.CubeType);
					if (cube != null)
					{
						detailRef = cube.CreateCellReference(aCellRef);
                        //if (detailRef is AssortmentCellReference)
                        //{
                        //    Debug.WriteLine("Cube.intGetDetailCellRefArray PARENT " + (int)aCellRef.Cube.CubeType.Id + " CHILD " + ((AssortmentCellReference)detailRef).CellKeys + "  " + (int)detailRef.Cube.CubeType.Id);
                        //}
						cubeRel.ProcessDetailSelector(aCellRef, detailRef, aCellSelector, out cancel);
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
		/// Protected routine that performs recursive travers of the Cube.
		/// </summary>
		/// <remarks>
		/// The recursive traverse of the Cube will inspect every branch of the Cube, and call the given RecurseCubeArguments
		/// delegate for every existing Cell.
		/// </remarks>
		/// <param name="aRecurseArguments">
		/// RecurseCubeArguments class containing callback information.
		/// </param>
		/// <param name="aCellCoordinates">
		/// The CellCoordinates object that indices are added to.
		/// </param>
		/// <param name="aCurrDimension">
		/// Indicates the current level of recursion.
		/// </param>
		/// <param name="aBranch">
		/// The current branch of the Cube being processed.
		/// </param>

		protected void intRecurseCubeExisting(RecurseCubeArguments aRecurseArguments, CellCoordinates aCellCoordinates, int aCurrDimension, object[] aBranch)
		{
			int i;
			CellReference cellRef;

			try
			{
				if (aCurrDimension < _cubeDef.NumDimensions - 1)
				{
					for (i = 0; i < aBranch.Length && !aRecurseArguments.Cancel; i++)
					{
						if (aBranch[i] != null)
						{
							aCellCoordinates.SetRawCoordinate(aCurrDimension, (int)_physicalToLogicalHash[aCurrDimension][i]);
							intRecurseCubeExisting(aRecurseArguments, aCellCoordinates, aCurrDimension + 1, (object[])aBranch[i]);
						}
					}
				}
				else
				{
					for (i = 0; i < aBranch.Length && !aRecurseArguments.Cancel; i++)
					{
						if (aBranch[i] != null)
						{
							aCellCoordinates.SetRawCoordinate(aCurrDimension, (int)_physicalToLogicalHash[aCurrDimension][i]);

							cellRef = CreateCellReference(aCellCoordinates);
							aRecurseArguments.CallbackRoutine(cellRef, aRecurseArguments);
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
		/// Protected routine that performs recursive traverse of the Cube.
		/// </summary>
		/// <remarks>
		/// The recursive traverse of the Cube will inspect every possible coordinate combination of the Cube, and call the given RecurseCubeArguments
		/// delegate for every Cell.
		/// </remarks>
		/// <param name="aRecurseArguments">
		/// RecurseCubeArguments class containing callback information.
		/// </param>
		/// <param name="aCellCoordinates">
		/// The CellCoordinates object that indices are added to.
		/// </param>
		/// <param name="aCurrDimension">
		/// Indicates the current level of recursion.
		/// </param>

		protected void intRecurseCubeMaster(RecurseCubeArguments aRecurseArguments, CellCoordinates aCellCoordinates, int aCurrDimension)
		{
			CellReference cellRef;
			eProfileType profileType;
			ProfileList profileList;

			try
			{
				if (aCurrDimension < _cubeDef.NumDimensions)
				{
					profileType = _cubeDef[aCurrDimension].ProfileType;
					profileList = CubeGroup.GetMasterProfileList(profileType);
					foreach (Profile profile in profileList)
					{
						aCellCoordinates.SetCoordinate(profileType, aCurrDimension, profile.Key);
						intRecurseCubeMaster(aRecurseArguments, aCellCoordinates, aCurrDimension + 1);

						if (aRecurseArguments.Cancel)
						{
							break;
						}
					}
				}
				else
				{
					cellRef = CreateCellReference(aCellCoordinates);
					aRecurseArguments.CallbackRoutine(cellRef, aRecurseArguments);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Adds the given CellReference to the given list.  Used to selectively add when desired.
		/// </summary>
		/// <param name="aCellRefList">
		/// The ArrayList to add the CellReference to.
		/// </param>
		/// <param name="aCellRef">
		/// The CellReference to add to the list.
		/// </param>

		private void intAddCellRefToList(ArrayList aCellRefList, CellReference aCellRef, object aAddCellRefParm)
		{
			aCellRefList.Add(aCellRef.Copy());
		}
	}

	/// <summary>
	/// Class that defines arguments passed to the RecurseCube methods.
	/// </summary>

	public class RecurseCubeArguments
	{
		//=======
		// FIELDS
		//=======

		private bool _cancel;
		private RecurseCallbackDelegate _callbackRoutine;
		private object _callbackArgs;

		//=============
		// CONSTRUCTORS
		//=============

		public RecurseCubeArguments(RecurseCallbackDelegate aCallbackRoutine, object aCallbackArguments)
		{
			_cancel = false;
			_callbackRoutine = aCallbackRoutine;
			_callbackArgs = aCallbackArguments;
		}
		
		//===========
		// PROPERTIES
		//===========

		public bool Cancel
		{
			get
			{
				return _cancel;
			}
			set
			{
				_cancel = value;
			}
		}

		public RecurseCallbackDelegate CallbackRoutine
		{
			get
			{
				return _callbackRoutine;
			}
		}

		public object CallbackArguments
		{
			get
			{
				return _callbackArgs;
			}
		}
		
		//========
		// METHODS
		//========
	}
}
