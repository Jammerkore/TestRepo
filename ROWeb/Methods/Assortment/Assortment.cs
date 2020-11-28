using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System;

namespace Logility.ROWeb
{
    public partial class ROAssortment : ROAllocation 
    {

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROAssortment(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {
            _assortmentSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.Assortment);
        }

        override public void CleanUp()
        {
            if (_applicationSessionTransaction != null)
            {
                _applicationSessionTransaction.DequeueHeaders();
            }
            base.CleanUp();
        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                case eRORequest.AssortmentActions:
                    return GetAssortmentActionsInfo();

                case eRORequest.AssortmentFilters:
                    return GetAssortmentFiltersInfo();

                case eRORequest.AssortmentHeaderData:
                    return GetAssortmentHeaderData();

                case eRORequest.GetAssortmentWorklistViews:
                    return GetAssortmentWorklistViews();

                case eRORequest.GetAssortmentWorklistViewDetails:
                    return GetAllocationWorklistViewDetails(rOKeyParams: (ROKeyParms)Parms);

                case eRORequest.AssortmentSelectedFilterHeaderData:
                    return GetAssortmentFilterHeaderData(headerFilterRID: (ROIntParms)Parms);
                    
                case eRORequest.GetAssortmentReviewViews:
                    return GetAssortmentReviewViews();

                case eRORequest.GetAssortmentProperties:
                    return GetAssortmentPropertiesData(rOKeyParams: (ROKeyParms)Parms);

                case eRORequest.GetAssortmentReviewSelection:
                    return GetAssortmentReviewSelectionData(rOKeyParams: (ROKeyParms)Parms);

                case eRORequest.UpdateAssortmentReview:
                    return UpdateAssortmentReviewSelection(roCubeParams: (ROCubeOpenParms)Parms);

                case eRORequest.UpdateAssortmentProperties:
                    return UpdateAssortmentPropertiesData(rOAssortmentPropertiesParms: (ROAssortmentPropertiesParms)Parms);

                case eRORequest.UpdateAssortmentSelection:
                    return UpdateAssortmentSelection(rOAssortmentPropertiesParms: (ROAssortmentPropertiesParms)Parms);

                case eRORequest.GetAssortmentUserLastValues:
                    return GetAssortmentUserLastValues();

                case eRORequest.SaveAssortmentUserLastValues:
                    return SaveAssortmentUserLastValues((ROAllocationWorklistLastDataParms)Parms);

                case eRORequest.GetAssortmentReviewViewList:
                    return GetAssortmentReviewViewList();

                case eRORequest.GetAssortmentReviewMatrixData:
                    return GetAssortmentReviewMatrixData((ROAssortmentReviewOptionsParms)Parms);

                case eRORequest.GetAssortmentContentCharacteristics:
                    return GetAssortmentContentCharacteristics();
					
				case eRORequest.ProcessAssortmentAction:
                    return ProcessAssortmentAction((ROAssortmentActionParms)Parms);

                case eRORequest.ProcessAssortmentReviewAllocationAction:
                    return ProcessAssortmentReviewAllocationAction((ROAssortmentAllocationActionParms)Parms);

                case eRORequest.UpdateAssortmentContentCharacteristics:
                    return UpdateAssortmentContentCharacteristics(rOAssortmentUpdateContentCharacteristicsParms: (ROAssortmentUpdateContentCharacteristicsParms)Parms);


				case eRORequest.ApplyAssortmentReviewMatrixChanges:
                    return ApplyAssortmentReviewMatrixCellChanges((ROGridChangesParms)Parms);

                case eRORequest.SaveAssortmentReviewChanges:
                    return SaveAssortmentReviewChanges((RONoParms)Parms);

                case eRORequest.SetAssortmentSelectedHeaders:
                    return SetAssortmentSelectedHeaders((ROListParms)Parms);

                case eRORequest.Rename:
                    return RenameWorklistItems((ROBaseUpdateParms)Parms);

                case eRORequest.Delete:
                    return DeleteWorklistItems((ROBaseUpdateParms)Parms);

                case eRORequest.Copy:
                    return CopyWorklistItems((ROBaseUpdateParms)Parms);

                default:
                    return base.ProcessRequest(Parms);
            }
        }
    }
}
