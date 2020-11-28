using System;
using System.Collections;
using System.Globalization;	
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
//Begin Track #4309 - JScott - Onhand incorrect for multiple detail basis
//    /// <summary>
//    /// The PlanBasisDetailCube is an cube used to store basis detail values.
//    /// </summary>
//
//    abstract public class PlanBasisDetailCube : Cube
//    {
//        //=======
//        // FIELDS
//        //=======
//
//        protected PlanReadLog _planReadLog;
//        protected VariablesData _varData;
//        private ProfileList _masterVariableProfileList;
//
//        //=============
//        // CONSTRUCTORS
//        //=============
//
//        /// <summary>
//        /// Creates a new instance of PlanBasisDetailCube, using the given SessionAddressBlock, Transaction, PlanCubeGroup, and CubeDefinition.
//        /// </summary>
//        /// <param name="aSAB">
//        /// A reference to a SessionAddressBlock that this PlanBasisDetailCube is a part of.
//        /// </param>
//        /// <param name="aTransaction">
//        /// A reference to a Transaction that this PlanBasisDetailCube is a part of.
//        /// </param>
//        /// <param name="aPlanCubeGroup">
//        /// A reference to a PlanCubeGroup that this PlanBasisDetailCube is a part of.
//        /// </param>
//
//        public PlanBasisDetailCube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, PlanCubeGroup aPlanCubeGroup, CubeDefinition aCubeDefinition)
//            : base(aSAB, aTransaction, aPlanCubeGroup, aCubeDefinition)
//        {
//            try
//            {
//                _planReadLog = new PlanReadLog();
//                _varData = new VariablesData();
//                _masterVariableProfileList = CubeGroup.GetMasterProfileList(eProfileType.Variable);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//
//        //===========
//        // PROPERTIES
//        //===========
//
//// BEGIN Modification - Implement ability to create model store for similar stores calculations.
//        /// <summary>
//        /// Gets a boolean indicating if Similar Store is to be calculated.
//        /// </summary>
//
//        abstract public bool isSimlarStore { get; }
//
//// END Modification - Implement ability to create model store for similar stores calculations.
//        /// <summary>
//        /// Gets the master Variable ProfileList.
//        /// </summary>
//
//        public ProfileList MasterVariableProfileList
//        {
//            get
//            {
//                return _masterVariableProfileList;
//            }
//        }
//
//        //========
//        // METHODS
//        //========
//
//        /// <summary>
//        /// Creates a new instance of a PlanBasisDetailCubeCell.
//        /// </summary>
//        /// <returns>
//        /// A reference to a new PlanBasisDetailCubeCell.
//        /// </returns>
//
//        override public Cell CreateCell(CellReference aCellReference)
//        {
//            BasisDetailCell basisDetailCell;
//
//            try
//            {
//                basisDetailCell = new BasisDetailCell();
//                return basisDetailCell;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//
//        /// <summary>
//        /// Creates a new instance of a PlanCellReference.
//        /// </summary>
//        /// <returns>
//        /// A reference to a new PlanCellReference.
//        /// </returns>
//
//        override public CellReference CreateCellReference()
//        {
//            try
//            {
//                return new BasisDetailCellReference(this);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//
//        /// <summary>
//        /// Creates a new instance of a PlanCellReference using the given CellCoordinates.
//        /// </summary>
//        /// <param name="aCoordinates">
//        /// The CellCoordinates object that describes the PlanBasisDetailCubeCell's position in this PlanBasisDetailCube.
//        /// </param>
//        /// <returns>
//        /// A reference to a new PlanCellReference.
//        /// </returns>
//
//        override public CellReference CreateCellReference(CellCoordinates aCoordinates)
//        {
//            try
//            {
//                return new BasisDetailCellReference(this, aCoordinates);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//
//        /// <summary>
//        /// Creates a new instance of a PlanCellReference using the given CellReference.
//        /// </summary>
//        /// <param name="aCellRef">
//        /// The CellReference object that describes the PlanBasisDetailCubeCell's position in this PlanBasisDetailCube.  This indices in this object will be translated to 
//        /// cooresponding coordinates in this PlanBasisDetailCube.
//        /// </param>
//        /// <returns>
//        /// A reference to a new PlanCellReference.
//        /// </returns>
//
//        override public CellReference CreateCellReference(CellReference aCellRef)
//        {
//            try
//            {
//                return new BasisDetailCellReference(this, aCellRef);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//    }
	/// <summary>
	/// The BasisDetailCube is an cube used to store basis detail values.
	/// </summary>

	abstract public class PlanBasisDetailCube : PlanBasisWeekDetailCube
	{
		//=======
		// FIELDS
		//=======

		protected VariablesData _varData;
		protected int _basisDetailDimensionIndex;
        // Begin Track #5841 - JSmith - Performance
        private VersionProfile _versionProfile = null;
        // End Track #5841

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of BasisDetailCube, using the given SessionAddressBlock, Transaction, PlanCubeGroup, and CubeDefinition.
		/// </summary>
		/// <param name="aSAB">
		/// A reference to a SessionAddressBlock that this BasisDetailCube is a part of.
		/// </param>
		/// <param name="aTransaction">
		/// A reference to a Transaction that this BasisDetailCube is a part of.
		/// </param>
		/// <param name="aPlanCubeGroup">
		/// A reference to a PlanCubeGroup that this BasisDetailCube is a part of.
		/// </param>

		public PlanBasisDetailCube(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, PlanCubeGroup aPlanCubeGroup, CubeDefinition aCubeDefinition, ushort aCubeAttributes)
			: base(aSAB, aTransaction, aPlanCubeGroup, aCubeDefinition, aCubeAttributes, 0)
		{
			try
			{
                // Begin TT#5124 - JSmith - Performance
                //_varData = new VariablesData();
                _varData = new VariablesData(aSAB.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
                // End TT#5124 - JSmith - Performance
				_basisDetailDimensionIndex = this.CubeDefinition.GetDimensionProfileTypeIndex(eProfileType.BasisDetail);
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
		/// Gets a boolean indicating if Similar Store is to be calculated.
		/// </summary>

		abstract public bool isSimlarStore { get; }

		//========
		// METHODS
		//========

		public override CellCoordinates CreateCellCoordinates(int aNumIndices)
		{
			try
			{
				return new PlanBasisDetailCellCoordinates(aNumIndices, this, _hierarchyDimensionIndex, _versionDimensionIndex, _basisDimensionIndex, _basisDetailDimensionIndex);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #4581 - JScott - Custom Variables not calculating on Basis line
		/// <summary>
		/// Returns an eProfileType of the hierarchy node dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the Hierarchy Node dimension.
		/// </returns>

		override public eProfileType GetHierarchyNodeType()
		{
			return eProfileType.BasisHierarchyNode;
		}

		/// <summary>
		/// Returns an eProfileType of the version dimension for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// An eProfileType of the version dimension.
		/// </returns>

		override public eProfileType GetVersionType()
		{
			return eProfileType.BasisVersion;
		}

		//End Track #4581 - JScott - Custom Variables not calculating on Basis line
		/// <summary>
		/// Returns a boolean indicating if the cell referenced by the given PlanCellReference is displayable.
		/// </summary>
        /// <param name="aCompCellRef">
		/// The PlanCellReference to check.
		/// </param>
		/// <returns>
		/// A boolean indicating if the cell is displayable.
		/// </returns>

		override public bool isCellDisplayable(ComputationCellReference aCompCellRef)
		{
			return true;
		}

		/// <summary>
		/// Method that returns the VersionProfile for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The VersionProfile for the given PlanCellReference.
		/// </returns>

		override public VersionProfile GetVersionProfile(PlanCellReference aPlanCellRef) 
		{
			try
			{
                // Begin Track #5841 - JSmith - Performance
                ////Begin Track #4457 - JSmith - Add forecast versions
                ////return ((BasisDetailProfile)((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).BasisDetailProfileList.FindKey(aPlanCellRef[eProfileType.BasisDetail])).VersionProfile;
                //return (VersionProfile)_SAB.ApplicationServerSession.GetProfileList(eProfileType.Version).FindKey(aPlanCellRef[eProfileType.BasisVersion]);
                ////End Track #4457
                if (_versionProfile == null ||
                    _versionProfile.Key != aPlanCellRef[eProfileType.BasisVersion])
                {
                    _versionProfile = (VersionProfile)_SAB.ApplicationServerSession.GetProfileListVersion().FindKey(aPlanCellRef[eProfileType.BasisVersion]); //TT#1517-MD -jsobek -Store Service Optimization
                }
                return _versionProfile;
                // End Track #5841
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the HierarchyNodeProfile for the given PlanCellReference.
		/// </summary>
		/// <returns>
		/// The HierarchyNodeProfile for the given PlanCellReference.
		/// </returns>

		override public HierarchyNodeProfile GetHierarchyNodeProfile(PlanCellReference aPlanCellRef)
		{
			try
			{
				//Begin Track #4457 - JSmith - Add forecast versions
				//return ((BasisDetailProfile)((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).BasisDetailProfileList.FindKey(aPlanCellRef[eProfileType.BasisDetail])).HierarchyNodeProfile;
				if (aPlanCellRef[eProfileType.BasisHierarchyNode] == ((BasisDetailProfile)((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).BasisDetailProfileList.FindKey(aPlanCellRef[eProfileType.BasisDetail])).HierarchyNodeProfile.Key)
				{
					return ((BasisDetailProfile)((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).BasisDetailProfileList.FindKey(aPlanCellRef[eProfileType.BasisDetail])).HierarchyNodeProfile;
				}
				else
				{
					return _SAB.HierarchyServerSession.GetNodeData(aPlanCellRef[eProfileType.BasisHierarchyNode], false);
				}
				//End Track #4457
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that returns the store status for the Cell.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// A PlanCellReference object that identifies the PlanCubeCell to process.
		/// </param>
		/// <returns>
		/// The eStoreStatus value that describes the store status of the Cell.
		/// </returns>

		override public eStoreStatus GetStoreStatus(PlanCellReference aPlanCellRef)
		{
			try
			{
				if (isStoreCube)
				{
					return SAB.ApplicationServerSession.GetStoreStatus(aPlanCellRef[eProfileType.Store],
						((BasisDetailProfile)((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).BasisDetailProfileList.FindKey(aPlanCellRef[eProfileType.BasisDetail])).GetBasisWeekIdFromPlanWeekId(SAB.ApplicationServerSession, aPlanCellRef[eProfileType.Week]));
				}
				else
				{
					return eStoreStatus.None;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//Begin Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line

		/// <summary>
		/// Returns the Time key of the data for the given PlanCellReference.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference to get the Time key of.
		/// </param>
		/// <returns>
		/// The Time key of the data for the given PlanCellReference.
		/// </returns>

		override public int GetWeekKeyOfData(PlanCellReference aPlanCellRef)
		{
			try
			{
				return aPlanCellRef[eProfileType.Week];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6088 - JScott - Bad 2008 Dsc Rsv % on basis line for B OTB if 2009 is on Plan line
		//Begin Track #6345 - JScott - Basis receipts are incorrect for Jan

		/// Method that returns a ProfileList of the weeks for the given PlanCellReference and eProfileType.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference of the PlanCell to find the week ProfileList for.
		/// </param>
		/// <param name="aDateProfileType">
		/// The eProfileType of the date type to look up.
		/// </param>
		/// <returns>
		/// A ProfileList of time.
		/// </returns>

		override public ProfileList GetTimeDetailProfileList(PlanCellReference aPlanCellRef, eProfileType aDateProfileType)
		{
			try
			{
				if (aDateProfileType == eProfileType.Week)
				{
					return ((BasisDetailProfile)((BasisProfile)PlanCubeGroup.OpenParms.GetBasisProfileList(aPlanCellRef.PlanCube.PlanCubeGroup, aPlanCellRef[eProfileType.HierarchyNode], aPlanCellRef[eProfileType.Version]).FindKey(aPlanCellRef[eProfileType.Basis])).BasisDetailProfileList.FindKey(aPlanCellRef[eProfileType.BasisDetail])).GetWeekProfileList(SAB.ApplicationServerSession);
				}
				else
				{
					throw new Exception("Invalid Call -- Period call to a Week cube");
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		//End Track #6345 - JScott - Basis receipts are incorrect for Jan
	}
//End Track #4309 - JScott - Onhand incorrect for multiple detail basis
}
