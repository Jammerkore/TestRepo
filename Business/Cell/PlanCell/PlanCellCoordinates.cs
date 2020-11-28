using System;
using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// ChainPlanLowLevelCellCoordinates defines the coordinates to a Cell within a Cube.
	/// </summary>
	/// <remarks>
	/// The ChainPlanLowLevelCellCoordinates class is used to define the n-number of coordinates to locate a Cell within a Cube.
	/// </remarks>

	public class ChainPlanLowLevelCellCoordinates : CellCoordinates
	{
		//=======
		// FIELDS
		//=======

		private PlanCube _planCube;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of ChainPlanLowLevelCellCoordinates, with a specified number of indices. 
		/// </summary>
		/// <param name="aNumCoordinates">
		/// The number of indices in the ChainPlanLowLevelCellCoordinates object.
		/// </param>

		public ChainPlanLowLevelCellCoordinates(int aNumCoordinates, PlanCube aPlanCube)
			: base(aNumCoordinates)
		{
			_planCube = aPlanCube;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets the indices of the CellCoordinates.
		/// </summary>

		override public int GetCoordinate(eProfileType aProfType, int aDimIdx)
		{
			try
			{
				switch (aProfType)
				{
					case eProfileType.HierarchyNode:

						return _planCube.PlanCubeGroup.OpenParms.ChainHLPlanProfile.NodeProfile.Key;
					
					case eProfileType.Version:

						return _planCube.PlanCubeGroup.OpenParms.LowLevelVersionDefault.Key;
					
					default:
					
						return base.GetRawCoordinate(aDimIdx);
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
	/// ChainBasisLowLevelCellCoordinates defines the coordinates to a Cell within a Cube.
	/// </summary>
	/// <remarks>
	/// The ChainBasisLowLevelCellCoordinates class is used to define the n-number of coordinates to locate a Cell within a Cube.
	/// </remarks>

	public class ChainBasisLowLevelCellCoordinates : CellCoordinates
	{
		//=======
		// FIELDS
		//=======

		private PlanCube _planCube;
		private int _basisIndex;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of ChainBasisLowLevelCellCoordinates, with a specified number of indices. 
		/// </summary>
		/// <param name="aNumCoordinates">
		/// The number of indices in the ChainBasisLowLevelCellCoordinates object.
		/// </param>

		public ChainBasisLowLevelCellCoordinates(int aNumCoordinates, PlanCube aPlanCube, int aBasisDimensionIndex)
			: base(aNumCoordinates)
		{
			_planCube = aPlanCube;
			_basisIndex = aBasisDimensionIndex;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets the indices of the CellCoordinates.
		/// </summary>

		override public int GetCoordinate(eProfileType aProfType, int aDimIdx)
		{
			PlanOpenParms planOpenParms;

			try
			{
				switch (aProfType)
				{
					case eProfileType.HierarchyNode:
					//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line

						return _planCube.PlanCubeGroup.OpenParms.ChainHLPlanProfile.NodeProfile.Key;

					case eProfileType.Version:

						return _planCube.PlanCubeGroup.OpenParms.LowLevelVersionDefault.Key;

					case eProfileType.BasisHierarchyNode:
					//End Track #4581 - JScott - Custom Variables not calculating on Basis line

						planOpenParms = _planCube.PlanCubeGroup.OpenParms;

						return ((BasisDetailProfile)((BasisProfile)planOpenParms
							.GetBasisProfileList(_planCube.PlanCubeGroup, planOpenParms.ChainHLPlanProfile.NodeProfile.Key, planOpenParms.ChainHLPlanProfile.VersionProfile.Key)
								.FindKey(base.GetRawCoordinate(_basisIndex))).BasisDetailProfileList[0]).HierarchyNodeProfile.Key;

					//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line
					//case eProfileType.Version:
					case eProfileType.BasisVersion:
					//End Track #4581 - JScott - Custom Variables not calculating on Basis line

						planOpenParms = _planCube.PlanCubeGroup.OpenParms;

						return ((BasisDetailProfile)((BasisProfile)planOpenParms
							.GetBasisProfileList(_planCube.PlanCubeGroup, planOpenParms.ChainHLPlanProfile.NodeProfile.Key, planOpenParms.ChainHLPlanProfile.VersionProfile.Key)
								.FindKey(base.GetRawCoordinate(_basisIndex))).BasisDetailProfileList[0]).VersionProfile.Key;

					default:

						return base.GetRawCoordinate(aDimIdx);
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
	/// StorePlanLowLevelCellCoordinates defines the coordinates to a Cell within a Cube.
	/// </summary>
	/// <remarks>
	/// The StorePlanLowLevelCellCoordinates class is used to define the n-number of coordinates to locate a Cell within a Cube.
	/// </remarks>

	public class StorePlanLowLevelCellCoordinates : CellCoordinates
	{
		//=======
		// FIELDS
		//=======

		private PlanCube _planCube;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of StorePlanLowLevelCellCoordinates, with a specified number of indices. 
		/// </summary>
		/// <param name="aNumCoordinates">
		/// The number of indices in the StorePlanLowLevelCellCoordinates object.
		/// </param>

		public StorePlanLowLevelCellCoordinates(int aNumCoordinates, PlanCube aPlanCube)
			: base(aNumCoordinates)
		{
			_planCube = aPlanCube;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets the indices of the CellCoordinates.
		/// </summary>

		override public int GetCoordinate(eProfileType aProfType, int aDimIdx)
		{
			try
			{
				switch (aProfType)
				{
					case eProfileType.HierarchyNode:

						return _planCube.PlanCubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.Key;

					case eProfileType.Version:

						return _planCube.PlanCubeGroup.OpenParms.LowLevelVersionDefault.Key;

					default:

						return base.GetRawCoordinate(aDimIdx);
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
	/// StoreBasisLowLevelCellCoordinates defines the coordinates to a Cell within a Cube.
	/// </summary>
	/// <remarks>
	/// The StoreBasisLowLevelCellCoordinates class is used to define the n-number of coordinates to locate a Cell within a Cube.
	/// </remarks>

	public class StoreBasisLowLevelCellCoordinates : CellCoordinates
	{
		//=======
		// FIELDS
		//=======

		private PlanCube _planCube;
		private int _basisIndex;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of StoreBasisLowLevelCellCoordinates, with a specified number of indices. 
		/// </summary>
		/// <param name="aNumCoordinates">
		/// The number of indices in the StoreBasisLowLevelCellCoordinates object.
		/// </param>

		public StoreBasisLowLevelCellCoordinates(int aNumCoordinates, PlanCube aPlanCube, int aBasisDimensionIndex)
			: base(aNumCoordinates)
		{
			_planCube = aPlanCube;
			_basisIndex = aBasisDimensionIndex;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Gets the indices of the CellCoordinates.
		/// </summary>

		override public int GetCoordinate(eProfileType aProfType, int aDimIdx)
		{
			PlanOpenParms planOpenParms;

			try
			{
				switch (aProfType)
				{
					case eProfileType.HierarchyNode:
					//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line

						return _planCube.PlanCubeGroup.OpenParms.StoreHLPlanProfile.NodeProfile.Key;

					case eProfileType.Version:

						return _planCube.PlanCubeGroup.OpenParms.LowLevelVersionDefault.Key;

					case eProfileType.BasisHierarchyNode:
					//End Track #4581 - JScott - Custom Variables not calculating on Basis line

						planOpenParms = _planCube.PlanCubeGroup.OpenParms;

						return ((BasisDetailProfile)((BasisProfile)planOpenParms
							.GetBasisProfileList(_planCube.PlanCubeGroup, planOpenParms.StoreHLPlanProfile.NodeProfile.Key, planOpenParms.StoreHLPlanProfile.VersionProfile.Key)
								.FindKey(base.GetRawCoordinate(_basisIndex))).BasisDetailProfileList[0]).HierarchyNodeProfile.Key;

					//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line
					//case eProfileType.Version:
					case eProfileType.BasisVersion:
					//End Track #4581 - JScott - Custom Variables not calculating on Basis line

						planOpenParms = _planCube.PlanCubeGroup.OpenParms;

						return ((BasisDetailProfile)((BasisProfile)planOpenParms
							.GetBasisProfileList(_planCube.PlanCubeGroup, planOpenParms.StoreHLPlanProfile.NodeProfile.Key, planOpenParms.StoreHLPlanProfile.VersionProfile.Key)
								.FindKey(base.GetRawCoordinate(_basisIndex))).BasisDetailProfileList[0]).VersionProfile.Key;

					default:

						return base.GetRawCoordinate(aDimIdx);
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
	/// PlanBasisDetailCellCoordinates defines the coordinates to a Cell within a Cube.
	/// </summary>
	/// <remarks>
	/// The PlanBasisDetailCellCoordinates class is used to define the n-number of coordinates to locate a Cell within a Cube.
	/// </remarks>

	public class PlanBasisDetailCellCoordinates : CellCoordinates
	{
		//=======
		// FIELDS
		//=======

		private PlanBasisDetailCube _planCube;
		private int _hierarchyIndex;
		private int _versionIndex;
		private int _basisIndex;
		private int _basisDetailIndex;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of PlanBasisDetailCellCoordinates, with a specified number of indices. 
		/// </summary>
		/// <param name="aNumCoordinates">
		/// The number of indices in the PlanBasisDetailCellCoordinates object.
		/// </param>

		public PlanBasisDetailCellCoordinates(
			int aNumCoordinates,
			PlanBasisDetailCube aPlanCube,
			int aHierarchyDimensionIndex,
			int aVersionDimensionIndex,
			int aBasisDimensionIndex,
			int aBasisDetailDimensionIndex)
			: base(aNumCoordinates)
		{
			_planCube = aPlanCube;
			_hierarchyIndex = aHierarchyDimensionIndex;
			_versionIndex = aVersionDimensionIndex;
			_basisIndex = aBasisDimensionIndex;
			_basisDetailIndex = aBasisDetailDimensionIndex;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		//Begin Track #4457 - JSmith - Add forecast versions
		///// <summary>
		///// Gets the indices of the CellCoordinates.
		///// </summary>

		//override public int GetCoordinate(eProfileType aProfType, int aDimIdx)
		//{
		//    PlanOpenParms planOpenParms;

		//    try
		//    {
		//        switch (aProfType)
		//        {
		//            case eProfileType.HierarchyNode:

		//                planOpenParms = _planCube.PlanCubeGroup.OpenParms;

		//                return ((BasisDetailProfile)((BasisProfile)planOpenParms
		//                    .GetBasisProfileList(_planCube.PlanCubeGroup,
		//                        GetRawCoordinate(_hierarchyIndex),
		//                            GetRawCoordinate(_versionIndex))
		//                                .FindKey(GetRawCoordinate(_basisIndex))).BasisDetailProfileList.FindKey(GetRawCoordinate(_basisDetailIndex))).HierarchyNodeProfile.Key;

		//            case eProfileType.Version:

		//                planOpenParms = _planCube.PlanCubeGroup.OpenParms;

		//                return ((BasisDetailProfile)((BasisProfile)planOpenParms
		//                    .GetBasisProfileList(_planCube.PlanCubeGroup,
		//                        GetRawCoordinate(_hierarchyIndex),
		//                            GetRawCoordinate(_versionIndex))
		//                                .FindKey(GetRawCoordinate(_basisIndex))).BasisDetailProfileList.FindKey(GetRawCoordinate(_basisDetailIndex))).VersionProfile.Key;

		//            default:

		//                return base.GetRawCoordinate(aDimIdx);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		//End Track #4457 - JSmith - Add forecast versions
	}

	/// <summary>
	/// PlanBasisCellCoordinates defines the coordinates to a Cell within a Cube.
	/// </summary>
	/// <remarks>
	/// The PlanBasisCellCoordinates class is used to define the n-number of coordinates to locate a Cell within a Cube.
	/// </remarks>

	public class PlanBasisCellCoordinates : CellCoordinates
	{
		//=======
		// FIELDS
		//=======

		private PlanBasisCube _planCube;
		private int _hierarchyIndex;
		private int _versionIndex;
		private int _basisIndex;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of PlanBasisCellCoordinates, with a specified number of indices. 
		/// </summary>
		/// <param name="aNumCoordinates">
		/// The number of indices in the PlanBasisCellCoordinates object.
		/// </param>

		public PlanBasisCellCoordinates(
			int aNumCoordinates,
			PlanBasisCube aPlanCube,
			int aHierarchyDimensionIndex,
			int aVersionDimensionIndex,
			int aBasisDimensionIndex)
			: base(aNumCoordinates)
		{
			_planCube = aPlanCube;
			_hierarchyIndex = aHierarchyDimensionIndex;
			_versionIndex = aVersionDimensionIndex;
			_basisIndex = aBasisDimensionIndex;
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		//Begin Track #4457 - JSmith - Add forecast versions
		///// <summary>
		///// Gets the indices of the CellCoordinates.
		///// </summary>

		//override public int GetCoordinate(eProfileType aProfType, int aDimIdx)
		//{
		//    PlanOpenParms planOpenParms;

		//    try
		//    {
		//        switch (aProfType)
		//        {
		//            case eProfileType.HierarchyNode:

		//                planOpenParms = _planCube.PlanCubeGroup.OpenParms;

		//                return ((BasisDetailProfile)((BasisProfile)planOpenParms
		//                    .GetBasisProfileList(_planCube.PlanCubeGroup,
		//                        GetRawCoordinate(_hierarchyIndex),
		//                            GetRawCoordinate(_versionIndex))
		//                                .FindKey(GetRawCoordinate(_basisIndex))).BasisDetailProfileList[0]).HierarchyNodeProfile.Key;

		//            case eProfileType.Version:

		//                planOpenParms = _planCube.PlanCubeGroup.OpenParms;

		//                return ((BasisDetailProfile)((BasisProfile)planOpenParms
		//                    .GetBasisProfileList(_planCube.PlanCubeGroup,
		//                        GetRawCoordinate(_hierarchyIndex),
		//                            GetRawCoordinate(_versionIndex))
		//                                .FindKey(GetRawCoordinate(_basisIndex))).BasisDetailProfileList[0]).VersionProfile.Key;

		//            default:

		//                return base.GetRawCoordinate(aDimIdx);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		//End Track #4457 - JSmith - Add forecast versions
	}
}
