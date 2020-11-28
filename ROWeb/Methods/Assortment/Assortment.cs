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
            try
            {
                if (Parms is ROMessageResponseParms
                    && _applicationSessionTransaction.MessageStatus != eMessagingStatus.WaitingForResponse)
                {
                    return new RONoDataOut(eROReturnCode.Failure, "Not expecting a message response.", Parms.ROInstanceID);
                }

                ROParms processingParms;
                if (Parms.RORequest != eRORequest.RespondToMessage)
                {
                    _currentParms = Parms;
                }
                else
                {
                    ROMessageResponseParms responseParms = (ROMessageResponseParms)Parms;
                    _applicationSessionTransaction.MessageStatus = eMessagingStatus.ResponseReceived;
                    _applicationSessionTransaction.MessageResponse = responseParms.MessageResponse;
                    _applicationSessionTransaction.MessageResponseDetails = responseParms.MessageDetails;
                }

                processingParms = _currentParms;

                switch (processingParms.RORequest)
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
                        return GetAllocationWorklistViewDetails(rOKeyParams: (ROKeyParms)processingParms);

                    case eRORequest.AssortmentSelectedFilterHeaderData:
                        return GetAssortmentFilterHeaderData(headerFilterRID: (ROIntParms)processingParms);

                    case eRORequest.GetAssortmentReviewViews:
                        return GetAssortmentReviewViews();

                    case eRORequest.GetAssortmentProperties:
                        return GetAssortmentPropertiesData(rOKeyParams: (ROKeyParms)processingParms);

                    case eRORequest.GetAssortmentReviewSelection:
                        return GetAssortmentReviewSelectionData(rOKeyParams: (ROKeyParms)processingParms);

                    case eRORequest.UpdateAssortmentReview:
                        return UpdateAssortmentReviewSelection(roCubeParams: (ROCubeOpenParms)processingParms);

                    case eRORequest.UpdateAssortmentProperties:
                        return UpdateAssortmentPropertiesData(rOAssortmentPropertiesParms: (ROAssortmentPropertiesParms)processingParms);

                    case eRORequest.UpdateAssortmentSelection:
                        return UpdateAssortmentSelection(rOAssortmentPropertiesParms: (ROAssortmentPropertiesParms)processingParms);

                    case eRORequest.GetAssortmentUserLastValues:
                        return GetAssortmentUserLastValues();

                    case eRORequest.SaveAssortmentUserLastValues:
                        return SaveAssortmentUserLastValues((ROAllocationWorklistLastDataParms)processingParms);

                    case eRORequest.GetAssortmentReviewViewList:
                        return GetAssortmentReviewViewList();

                    case eRORequest.GetAssortmentReviewMatrixData:
                        return GetAssortmentReviewMatrixData((ROAssortmentReviewOptionsParms)processingParms);

                    case eRORequest.GetAssortmentContentCharacteristics:
                        return GetAssortmentContentCharacteristics();

                    case eRORequest.ProcessAssortmentAction:
                        return ProcessAssortmentAction((ROAssortmentActionParms)processingParms);

                    case eRORequest.ProcessAssortmentReviewAllocationAction:
                        return ProcessAssortmentReviewAllocationAction((ROAssortmentAllocationActionParms)processingParms);

                    case eRORequest.UpdateAssortmentContentCharacteristics:
                        return UpdateAssortmentContentCharacteristics(rOAssortmentUpdateContentCharacteristicsParms: (ROAssortmentUpdateContentCharacteristicsParms)processingParms);


                    case eRORequest.ApplyAssortmentReviewMatrixChanges:
                        return ApplyAssortmentReviewMatrixCellChanges((ROGridChangesParms)processingParms);

                    case eRORequest.SaveAssortmentReviewChanges:
                        return SaveAssortmentReviewChanges((RONoParms)processingParms);

                    case eRORequest.SetAssortmentSelectedHeaders:
                        return SetAssortmentSelectedHeaders((ROListParms)processingParms);

                    case eRORequest.Rename:
                        return RenameWorklistItems((ROBaseUpdateParms)processingParms);

                    case eRORequest.Delete:
                        return DeleteWorklistItems((ROBaseUpdateParms)processingParms);

                    case eRORequest.Copy:
                        return CopyWorklistItems((ROBaseUpdateParms)processingParms);

                    default:
                        return base.ProcessRequest(processingParms);
                }
            }
            catch (MessageRequestException ex)
            {
                _applicationSessionTransaction.MessageStatus = eMessagingStatus.WaitingForResponse;
                return new ROMessageRequest(ROReturnCode: eROReturnCode.MessageRequest,
                    sROMessage: null,
                    ROInstanceID: ROInstanceID,
                    messageRequest: ex.MessageRequest,
                    messageDetails: ex.MessageDetails
                    );
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, Parms.RORequest.ToString() + " failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }
    }
}
