
using Logility.ROWeb.Methods.Administration.Audit;
using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace Logility.ROWeb
{
    public partial class ROAdministration : ROWebFunction
    {
        private ROCharacteristicMaintenance _ROCharacteristicMaintenance = null;
        private ROModelMaintenance _ROModelMaintenance = null;
        private ROAuditContainer _ROAuditContainer = null;

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROAdministration(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {

        }

        public ROCharacteristicMaintenance ROCharacteristicMaintenance
        {
            get
            {
                if (_ROCharacteristicMaintenance == null)
                {
                    _ROCharacteristicMaintenance = new ROCharacteristicMaintenance(SAB: SAB, ROWebTools: ROWebTools, ROInstanceID: ROInstanceID);
                }
                return _ROCharacteristicMaintenance;
            }
        }

        public ROModelMaintenance ROModelMaintenance
        {
            get
            {
                if (_ROModelMaintenance == null)
                {
                    _ROModelMaintenance = new ROModelMaintenance(SAB: SAB, ROWebTools: ROWebTools, ROInstanceID: ROInstanceID);
                }
                return _ROModelMaintenance;
            }
        }
        public ROAuditContainer ROAuditContainer
        {
            get
            {
                if (_ROAuditContainer == null)
                {
                    _ROAuditContainer = new ROAuditContainer(SAB: SAB, ROWebTools: ROWebTools, ROInstanceID: ROInstanceID);
                }
                return _ROAuditContainer;
            }
        }


        override public void CleanUp()
        {
            if (_ROCharacteristicMaintenance != null)
            {
                _ROCharacteristicMaintenance.CleanUp();
            }

            if (_ROModelMaintenance != null)
            {
                _ROModelMaintenance.CleanUp();
            }
        }
        
        override public ROOut ProcessRequest(ROParms parms)
        {
            switch (parms.RORequest)
            {
                case eRORequest.GetUserInformation:
                    if (parms is ROKeyParms)
                    {
                        return GetUserInformation((ROKeyParms)parms);
                    }
                    else if (parms is ROStringParms)
                    {
                        return GetUserInformation((ROStringParms)parms);
                    }
                    else
                    {
                        return GetUserInformation((RONoParms)parms);
                    }
                case eRORequest.SaveUser:
                    return SaveUserInformation((RUserInformationParms)parms);

                //Characteristics
                case eRORequest.GetCharacteristics:
                    return ROCharacteristicMaintenance.GetCharacteristics((ROProfileKeyParms)parms);
                case eRORequest.GetCharacteristic:
                    return ROCharacteristicMaintenance.GetCharacteristic((ROProfileKeyParms)parms);
                case eRORequest.SaveCharacteristics:
                    return ROCharacteristicMaintenance.SaveCharacteristics((ROCharacteristicsPropertiesParms)parms);
                case eRORequest.DeleteCharacteristic:
                    return ROCharacteristicMaintenance.DeleteCharacteristics(parms);

                //Audit 
                case eRORequest.AuditFilterOption:
                    return ROAuditContainer.GetAuditFilterOption((ROProfileKeyParms)parms);
                case eRORequest.AuditGenerateReport:
                    return ROAuditContainer.GenerateAuditReport((ROProfileKeyParms)parms);

                //Models
                case eRORequest.GetModels:
                    return ROModelMaintenance.GetModels((ROModelParms)parms);
                case eRORequest.GetModel:
                    return ROModelMaintenance.GetModel((ROModelParms)parms);
                case eRORequest.SaveModel:
                    return ROModelMaintenance.SaveModel((ROModelPropertiesParms)parms);
                case eRORequest.ApplyModel:
                    return ROModelMaintenance.ApplyModel((ROModelPropertiesParms)parms);
                case eRORequest.SaveAsModel:
                    return ROModelMaintenance.SaveAsModel((ROModelPropertiesParms)parms);
                case eRORequest.CopyModel:
                    return ROModelMaintenance.CopyModel((ROModelParms)parms);
                case eRORequest.DeleteModel:
                    return ROModelMaintenance.DeleteModel((ROModelParms)parms);
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

    }
}
