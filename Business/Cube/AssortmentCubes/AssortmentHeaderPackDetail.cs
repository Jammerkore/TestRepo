//Begin TT#2 - JScott - Assortment Planning - Phase 2
//using System;
//using System.Collections;
//using System.Text;
//using MIDRetail.Common;
//using MIDRetail.Data;
//using MIDRetail.DataCommon;
//using MIDRetail.Business.Allocation;

//namespace MIDRetail.Business
//{
//    public class AssortmentHeaderPackDetail : AssortmentComponentHeaderCube
//    {
//        //=======
//        // FIELDS
//        //=======

//        //=============
//        // CONSTRUCTORS
//        //=============

//        /// <summary>
//        /// Creates a new instance of AssortmentCube, using the given SessionAddressBlock, Transaction, AssortmentCubeGroup, CubeDefinition, and ComputationProcessor.
//        /// </summary>
//        /// <param name="aSAB">
//        /// A reference to a SessionAddressBlock that this AssortmentCube is a part of.
//        /// </param>
//        /// <param name="aTransaction">
//        /// A reference to a Transaction that this AssortmentCube is a part of.
//        /// </param>
//        /// <param name="aAssrtCubeGroup">
//        /// A reference to a AssortmentCubeGroup that this AssortmentCube is a part of.
//        /// </param>
//        /// <param name="aCubeDefinition">
//        /// The CubeDefinition that describes the structure of this AssortmentCube.
//        /// </param>

//        public AssortmentHeaderPackDetail(SessionAddressBlock aSAB, ApplicationSessionTransaction aTransaction, AssortmentCubeGroup aAssrtCubeGroup, CubeDefinition aCubeDefinition, int aCubePriority, bool aReadOnly, bool aCheckNodeSecurity)
//            : base(aSAB, aTransaction, aAssrtCubeGroup, aCubeDefinition, PlanCubeAttributesFlagValues.GroupTotal, aCubePriority, aReadOnly, aCheckNodeSecurity)
//        {
//        }

//        //===========
//        // PROPERTIES
//        //===========

//        /// <summary>
//        /// Gets the eCubeType of this cube.
//        /// </summary>

//        public override eCubeType CubeType
//        {
//            get
//            {
//                return eCubeType.AssortmentHeaderPackDetail;
//            }
//        }

//        //========
//        // METHODS
//        //========

//        /// <summary>
//        /// Abstract method that initializes the Cube.
//        /// </summary>

//        override public void InitializeCube()
//        {
//        }

//        /// <summary>
//        /// Abstract method that returns the eCubeType of the Detail Group Level Total cube for this cube.
//        /// </summary>
//        /// <returns>
//        /// The eCubeType of the Detail Group Level Total cube.
//        /// </returns>

//        override public eCubeType GetComponentGroupLevelCubeType()
//        {
//            return eCubeType.None;
//        }

//        /// <summary>
//        /// Abstract method that returns the eCubeType of the Detail Group Total cube for this cube.
//        /// </summary>
//        /// <returns>
//        /// The eCubeType of the Detail Group Total cube.
//        /// </returns>

//        override public eCubeType GetComponentTotalCubeType()
//        {
//            return eCubeType.None;
//        }

//        /// <summary>
//        /// Abstract method that returns the eCubeType of the corresponding Placeholder cube for this cube.
//        /// </summary>
//        /// <returns>
//        /// The eCubeType of the Sub-total cube.
//        /// </returns>

//        override public eCubeType GetComponentPlaceholderCubeType()
//        {
//            return eCubeType.None;
//        }

//        /// <summary>
//        /// Abstract method that returns the eCubeType of the corresponding Header cube for this cube.
//        /// </summary>
//        /// <returns>
//        /// The eCubeType of the Sub-total cube.
//        /// </returns>

//        override public eCubeType GetComponentHeaderCubeType()
//        {
//            return eCubeType.None;
//        }

//        /// <summary>
//        /// Abstract method that returns the eCubeType of the summary cube for this cube.
//        /// </summary>
//        /// <returns>
//        /// The eCubeType of the Sub-total cube.
//        /// </returns>

//        override public eCubeType GetSummaryCubeType()
//        {
//            return eCubeType.None;
//        }

//        /// <summary>
//        /// Abstract method that returns the eCubeType of the Sub-total cube for this cube.
//        /// </summary>
//        /// <returns>
//        /// The eCubeType of the Sub-total cube.
//        /// </returns>

//        override public eCubeType GetSubTotalCubeType()
//        {
//            return eCubeType.None;
//        }

//        /// <summary>
//        /// Abstract method that returns the eCubeType of the total cube for this cube.
//        /// </summary>
//        /// <returns>
//        /// The eCubeType of the total cube.
//        /// </returns>

//        override public eCubeType GetTotalCubeType()
//        {
//            return eCubeType.None;
//        }

//        /// <summary>
//        /// Abstract method that returns the VariableProfile object for the Cell specified by the ComputationCellReference.
//        /// </summary>
//        /// <remarks>
//        /// A ComputationCellReference object that identifies the ComputationCubeCell to retrieve.
//        /// </remarks>

//        override public ComputationVariableProfile GetVariableProfile(ComputationCellReference aCompCellRef)
//        {
//            try
//            {
//                return (ComputationVariableProfile)MasterAssortmentDetailVariableProfileList.FindKey(aCompCellRef[eProfileType.AssortmentDetailVariable]);
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        /// <summary>
//        /// Abstract method that allows the inheritor the ability to dictate the way a Cell is read.
//        /// </summary>
//        /// <param name="aAssrtCellRef">
//        /// A AssortmentCellReference object that identifies the AssortmentCubeCell to read.
//        /// </param>

//        override public void ReadCell(AssortmentCellReference aAssrtCellRef)
//        {
//        }

//        /// <summary>
//        /// Returns a string describing the given PlanCellReference
//        /// </summary>
//        /// <param name="aCompCellRef">
//        /// The PlanCellReference that points to the Cell to inspect.
//        /// </param>
//        /// <returns>
//        /// A string describing the PlanCellReference.
//        /// </returns>

//        override public string GetCellDescription(ComputationCellReference aCompCellRef)
//        {
//            AssortmentCellReference AssortCellRef;
//            //HierarchyNodeProfile nodeProf;
//            //VersionProfile versProf;
//            AssortmentDetailVariableProfile detailVarProf;
//            QuantityVariableProfile quantityVarProf;

//            try
//            {
//                AssortCellRef = (AssortmentCellReference)aCompCellRef;
//                //nodeProf = GetHierarchyNodeProfile(planCellRef);
//                //versProf = GetVersionProfile(planCellRef);
//                detailVarProf = (AssortmentDetailVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentDetailVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentDetailVariable]);
//                quantityVarProf = (QuantityVariableProfile)AssortmentCubeGroup.AssortmentComputations.AssortmentQuantityVariables.VariableProfileList.FindKey(AssortCellRef[eProfileType.AssortmentQuantityVariable]);

//                return "Assortment Header Pack Detail" +
//                    ", Header \"" + AssortCellRef[eProfileType.AllocationHeader] +
//                    ", HeaderPack \"" + AssortCellRef[eProfileType.HeaderPack] +
//                    ", Store Group Level " + AssortCellRef[eProfileType.StoreGroupLevel] +
//                    ", Store Grade " + AssortCellRef[eProfileType.StoreGrade] +
//                    ", Detail \"" + detailVarProf.VariableName + "\"" +
//                    ", Quantity Variable \"" + quantityVarProf.VariableName + "\"";
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }
//    }
//}
//End TT#2 - JScott - Assortment Planning - Phase 2
