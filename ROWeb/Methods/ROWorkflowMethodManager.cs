using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business.Allocation;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public abstract class ROWorkflowMethodManager
    {
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private FunctionSecurityProfile _functionSecurity;
        protected ApplicationBaseMethod _ABM = null;
        protected ApplicationBaseWorkFlow _ABW = null;
        private ClientSessionTransaction _clientTransaction = null;
        private ApplicationSessionTransaction _applicationSessionTransaction = null;
        private GetMethods _getMethods;
        private WorkflowMethodManager _wmManager = null;
        private long _ROInstanceID;
        private eROApplicationType _applicationType;
        private bool _nameValid = false;
        private string _nameMessage;
		// Collection to manage methods within the workflow
        private Dictionary<int, ApplicationBaseMethod> _workflowMethods = new Dictionary<int, ApplicationBaseMethod>();

        /// <summary>
        /// Gets the SessionAddressBlock
        /// </summary>
        protected SessionAddressBlock SAB
        {
            get { return _SAB; }
        }

        /// <summary>
        /// Gets the ROWebTools
        /// </summary>
        public ROWebTools ROWebTools
        {
            get { return _ROWebTools; }
        }

        /// <summary>
        /// Gets the unique function ID
        /// </summary>
        protected long ROInstanceID
        {
            get { return _ROInstanceID; }
        }

        /// <summary>
        /// Gets the key associated with the type of object
        /// </summary>
        public int Key
        {
            get
            {
                if (_ABW != null)
                {
                    return _ABW.Key;
                }
                else if (_ABM != null)
                {
                    return _ABM.Key;
                }
                else
                {
                    return Include.NoRID;
                }
            }
        }

        /// <summary>
        /// Gets the eProfileType associated with the type of object
        /// </summary>
        public eProfileType ProfileType
        {
            get
            {
                if (_ABW != null)
                {
                    return _ABW.ProfileType;
                }
                else if (_ABM != null)
                {
                    return _ABM.ProfileType;
                }
                else
                {
                    return eProfileType.None;
                }
            }
        }

        /// <summary>
        /// Gets or sets the function security profile
        /// </summary>
        protected FunctionSecurityProfile FunctionSecurity
        {
            get { return _functionSecurity; }
            set { _functionSecurity = value; }
        }

        protected ClientSessionTransaction ClientTransaction
        {
            get
            {
                if (_clientTransaction == null)
                {
                    _clientTransaction = SAB.ClientServerSession.CreateTransaction();
                }
                return _clientTransaction;
            }
        }

        public ApplicationBaseMethod ABM
        {
            get
            {
                return _ABM;
            }
            set
            {
                _ABM = value;
            }
        }

        public ApplicationBaseWorkFlow ABW
        {
            get
            {
                return _ABW;
            }
            set
            {
                _ABW = value;
            }
        }

        /// <summary>
        /// Gets the application of the manager
        /// </summary>
        public eROApplicationType ApplicationType
        {
            get { return _applicationType; }
        }

        protected ApplicationSessionTransaction GetApplicationSessionTransaction(bool getNewTransaction = false)
        {
            if (_applicationSessionTransaction == null
                || getNewTransaction)
            {
                if (_applicationSessionTransaction != null)
                {
                    _applicationSessionTransaction.Dispose();
                }
                _applicationSessionTransaction = SAB.ApplicationServerSession.CreateTransaction();
            }
            return _applicationSessionTransaction;
        }

        protected GetMethods GetMethods
        {
            get
            {
                if (_getMethods == null)
                {
                    _getMethods = new GetMethods(SAB);
                }
                return _getMethods;
            }
        }

        protected WorkflowMethodManager WmManager
        {
            get
            {
                if (_wmManager == null)
                {
                    _wmManager = new WorkflowMethodManager(SAB.ClientServerSession.UserRID);
                }
                return _wmManager;
            }
        }

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        /// <param name="ROInstanceID">The instance ID of the session</param>
        /// <param name="applicationType">The application that instantiated the instance</param>
        /// <param name="applicationSessionTransaction">The transaction to use for processing</param>
        public ROWorkflowMethodManager(SessionAddressBlock SAB, ROWebTools ROWebTools, long ROInstanceID, eROApplicationType applicationType, ApplicationSessionTransaction applicationSessionTransaction)
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _ROInstanceID = ROInstanceID;
            _applicationType = applicationType;
            _applicationSessionTransaction = applicationSessionTransaction;
        }

        public void CleanUp()
        {
            // release locks for all methods retrieved in the workflow
            foreach (KeyValuePair<int, ApplicationBaseMethod> entry in _workflowMethods)
            {
                _ABM = entry.Value;
                if (_ABM != null
                    && _ABM.LockStatus == eLockStatus.Locked)
                {
                    string message = null;
                    _ABM.LockStatus = WorkflowMethodUtilities.UnlockWorkflowMethod(
                        SAB: SAB,
                        workflowMethodIND: eWorkflowMethodIND.Methods,
                        Key: _ABM.Key,
                        message: out message
                        );
                    if (_ABM.LockStatus == eLockStatus.Cancel)
                    {
                        MIDEnvironment.Message = message;
                    }
                }
            }
			_workflowMethods.Clear();

            if (_ABW != null
                && _ABW.LockStatus == eLockStatus.Locked)
            {
                string message = null;
                _ABW.LockStatus = WorkflowMethodUtilities.UnlockWorkflowMethod(
                    SAB: SAB,
                    workflowMethodIND: eWorkflowMethodIND.Workflows,
                    Key: _ABW.Key,
                    message: out message
                    );
                if (_ABW.LockStatus == eLockStatus.Cancel)
                {
                    MIDEnvironment.Message = message;
                }
            }

        }

        #region Get the WorkFlows Method List
        public ROOut GetWorkFlowMethodList(ROKeyParms parms)
        {
            try
            {
                List<KeyValuePair<int, string>> workflowMethods;
                workflowMethods = GetWorkFlowMethods(methodType: (eMethodTypeUI)parms.Key);
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, workflowMethods);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetWorkFlowMethodList failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        public ROOut GetWorkFlowMethodList(ROWorkflowMethodParms parameters)
        {
            try
            {
                List<KeyValuePair<int, string>> workflowMethods;
                workflowMethods = GetWorkFlowMethods(methodType: (eMethodTypeUI)parameters.MethodType,
                    workflowKey: parameters.WorkflowKey,
                    workflowStep: parameters.WorkflowStep);
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, workflowMethods);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetWorkFlowMethodList failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        internal List<KeyValuePair<int, string>> GetWorkFlowMethods(
            eMethodTypeUI methodType,
            int workflowKey = Include.NoRID,
            int workflowStep = Include.Undefined
            )
        {
            List<KeyValuePair<int, string>> methodList = new List<KeyValuePair<int, string>>();

            // if it is not a method, return an empty list
            if (!Enum.IsDefined(typeof(eMethodTypeUI), (int)methodType))
            {
                return methodList;
            }

            DataTable methodsDataTable = WmManager.GetMethodList(methodType, SAB.ClientServerSession.UserRID);

            // Get only template methods
            DataRow[] filteredDataRows = methodsDataTable.Select("TEMPLATE_IND = '1'");

            DataTable filteredDataTable = new DataTable();

            if (filteredDataRows.Length != 0)
            {
                filteredDataTable = filteredDataRows.CopyToDataTable();
            }

            methodsDataTable = ApplicationUtilities.SortDataTable(dataTable: filteredDataTable, sColName: "METHOD_NAME", bAscending: true);
            
            methodList =  ApplicationUtilities.DataTableToKeyValues(methodsDataTable, "METHOD_RID", "METHOD_NAME", true);

            int customKey = Include.Undefined;

            // Determine if workflow step already has custom method
            if (workflowStep != Include.Undefined)
            {
                customKey = DetermineCustomKey(methodType: methodType,
                    workflowKey: workflowKey,
                    workflowStep: workflowStep);
            }

            methodList.Add(new KeyValuePair<int, string>(customKey, "Custom"));

            return methodList;
        }

        private int DetermineCustomKey(
            eMethodTypeUI methodType,
            int workflowKey = Include.NoRID,
            int workflowStep = Include.Undefined)
        {
            int customKey = Include.Undefined;

            // no workflow or step to check
            if (_ABW == null
                || _ABW.Workflow_Steps.Count <= workflowStep
                || _ABW.Workflow_Steps[workflowStep] == null
                )
            {
                return customKey;
            }

            // check if method or action
            ApplicationWorkFlowStep applicationWorkflowStep;
            applicationWorkflowStep = (ApplicationWorkFlowStep)_ABW.Workflow_Steps[workflowStep];
            // make sure is method and same type as requested
            if (applicationWorkflowStep.Method != null
                && Enum.IsDefined(typeof(eMethodTypeUI), (int)applicationWorkflowStep.Method.MethodType)  // check if method
                && methodType == (eMethodTypeUI)applicationWorkflowStep.Method.MethodType)  // make sure is same type 
            {
                // if method, convert to type to test for template or custom method
                ApplicationBaseMethod method = (ApplicationBaseMethod)applicationWorkflowStep.Method;
                if (!method.Template_IND)
                {
                    customKey = method.Key;
                }
            }

            return customKey;
        }

        #endregion

        #region "Method Processing"
        public ROOut GetMethod(ROMethodParms methodParm, bool processingApply = false)
        {
            bool successful;
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;

            // Do not obtain object during Apply since will already have one
            if (!processingApply)
            {
                if (_ABM == null  
                    || _ABM.Key != methodParm.Key)
                {
                    // check if already have method in the collection.  If not create it.
                    if (!_workflowMethods.TryGetValue(methodParm.Key, out _ABM))
                    {
                        _ABM = (ApplicationBaseMethod)GetMethods.GetMethod(methodParm.Key, methodParm.MethodType);
                        _workflowMethods[_ABM.Key] = _ABM;
                    }
                    if (_ABM is VelocityMethod)
                    {
                        ((VelocityMethod)_ABM).AST = _applicationSessionTransaction;
                    }
                }
            }

            if (methodParm.Key != Include.NoRID
                && !_ABM.AuthorizedToView(SAB.ClientServerSession, SAB.ClientServerSession.UserRID))
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForItem);
                return new ROMethodPropertiesOut(eROReturnCode.Failure, message, ROInstanceID, null);
            }

            ROMethodProperties mp = _ABM.MethodGetData(successful: out successful, message: ref message, processingApply: processingApply);

            if (!successful)
            {
                returnCode = eROReturnCode.Failure;
            }

            FunctionSecurity = _ABM.GetFunctionSecurity();
            mp.CanBeProcessed = FunctionSecurity.AllowExecute;
            mp.CanBeDeleted = FunctionSecurity.AllowDelete;
            mp.IsReadOnly = FunctionSecurity.IsReadOnly;
            // if not read only, check based on data in the method
            if (!mp.IsReadOnly)
            {
                mp.IsReadOnly = !_ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID);
            }

            // Do not attempt to lock during Apply since locking processed during initial Get
            if (!mp.IsReadOnly
                && _ABM.LockStatus != eLockStatus.Locked
                && !processingApply
                && successful)
            {
                message = null;
                _ABM.LockStatus = WorkflowMethodUtilities.LockWorkflowMethod(
                    SAB: SAB,
                    workflowMethodIND: eWorkflowMethodIND.Methods,
                    aChangeType: eChangeType.update,
                    Key: _ABM.Key,
                    Name: _ABM.Name,
                    allowReadOnly: true,
                    message: out message
                    );
                if (_ABM.LockStatus == eLockStatus.ReadOnly)
                {
                    MIDEnvironment.isChangedToReadOnly = true;
                    MIDEnvironment.Message = message;
                }
            }

            return new ROMethodPropertiesOut(returnCode, message, ROInstanceID, mp);
        }

        public ROOut SaveMethod(ROMethodPropertiesParms methodParm)
        {
            string message = null;

            if (_ABM == null
                || methodParm.ROMethodProperties.Method.Key == Include.NoRID
                || methodParm.ROMethodProperties.MethodType != _ABM.MethodType
                )
            {
                // check if already have method in the collection.  If not create it.
                if (!_workflowMethods.TryGetValue(methodParm.ROMethodProperties.Method.Key, out _ABM))
                {
                    _ABM = (ApplicationBaseMethod)GetMethods.GetMethod(methodParm.ROMethodProperties.Method.Key, methodParm.ROMethodProperties.MethodType);
                    // only save if not new method
                    if (methodParm.ROMethodProperties.Method.Key != Include.NoRID)
                    {
                        _workflowMethods[_ABM.Key] = _ABM;
                    }
                }
                FunctionSecurity = _ABM.GetFunctionSecurity();
                _ABM.User_RID = methodParm.ROMethodProperties.UserKey;
                if (_ABM is VelocityMethod)
                {
                    ((VelocityMethod)_ABM).AST = _applicationSessionTransaction;
                }
            }

            bool cleanseName = false;
            if (methodParm.ROMethodProperties.Method.Key == Include.NoRID)
            {
                _ABM.Method_Change_Type = eChangeType.add;
                cleanseName = true;
            }
            else
            {
                _ABM.Method_Change_Type = eChangeType.update;
                cleanseName = _ABM.Name != methodParm.ROMethodProperties.Method.Value;
            }

            if (methodParm.ROMethodProperties.IsTemplate)
            {
                _ABM.Name = methodParm.ROMethodProperties.Method.Value;
            }
            else if (!methodParm.ROMethodProperties.IsTemplate
                && _ABM.Method_Change_Type == eChangeType.add
                && string.IsNullOrWhiteSpace(_ABM.Name))
            {
                _ABM.Name = "Custom" + DateTime.Now.Ticks;
            }
            if (cleanseName)
            {
                CleanseMethodName();
            }
            _ABM.Method_Description = methodParm.ROMethodProperties.Description;
            _ABM.User_RID = methodParm.ROMethodProperties.UserKey;
            _ABM.Template_IND = methodParm.ROMethodProperties.IsTemplate;

            if (_ABM.MethodSetData(methodProperties: methodParm.ROMethodProperties, message: ref message, processingApply: false))
            {
                try
                {
                    if (!FunctionSecurity.AllowUpdate
                        || !_ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID))
                    {
                        return new ROIListOut(eROReturnCode.Failure, null, ROInstanceID, null);
                    }

                    int folderKey = methodParm.FolderKey;
                    if (_ABM.Method_Change_Type == eChangeType.add)
                    {
                        folderKey = WorkflowMethodUtilities.GetWorkflowMethodFolderRID(GetFolderProfileType(), methodParm.FolderKey, _ABM.User_RID, _ABM.ProfileType, methodParm.FolderUniqueID);
                    }
                    ClientTransaction.DataAccess.OpenUpdateConnection();
                    _ABM.Update(ClientTransaction.DataAccess);
                    if (_ABM.Method_Change_Type == eChangeType.add)
                    {
                        FolderDataLayer dlFolder = new FolderDataLayer(ClientTransaction.DataAccess);
                        dlFolder.Folder_Item_Insert(folderKey, _ABM.Key, _ABM.ProfileType);
                    }
                    ClientTransaction.DataAccess.CommitData();
                    // add or update the method in the collection
                    _workflowMethods[_ABM.Key] = _ABM;
                }
                finally
                {
                    if (ClientTransaction.DataAccess.ConnectionIsOpen)
                    {
                        ClientTransaction.DataAccess.CloseUpdateConnection();
                    }
                }
            }

            return new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, WorkflowMethodUtilities.BuildMethodNode(GetApplicationType(), _ABM));
        }

        public ROOut ApplyMethod(ROMethodPropertiesParms methodParm)
        {
            string message = null;

            if (_ABM == null)
            {
                message = SAB.ClientServerSession.Audit.GetText(
                    messageCode: eMIDTextCode.msg_ValueWasNotFound,
                    addToAuditReport: true,
                    args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_Method) }
                    );
                throw new MIDException(eErrorLevel.severe,
                    (int)eMIDTextCode.msg_ValueWasNotFound,
                    message);
            }

            // Save values to memory only
            _ABM.Name = methodParm.ROMethodProperties.Method.Value;
            _ABM.Method_Description = methodParm.ROMethodProperties.Description;
            _ABM.User_RID = methodParm.ROMethodProperties.UserKey;
            _ABM.MethodSetData(methodProperties: methodParm.ROMethodProperties, message: ref message, processingApply: true);

            // add or update the method in the collection
            _workflowMethods[_ABM.Key] = _ABM;

            // Build new object with updated values
			ROMethodParms methodGetParm = new ROMethodParms(
                sROUserID: methodParm.ROUserID, 
                sROSessionID: methodParm.ROSessionID, 
                ROClass: methodParm.ROClass,
                RORequest: eRORequest.GetMethod, 
                ROInstanceID: methodParm.ROInstanceID, 
                methodType: methodParm.ROMethodProperties.MethodType,
                key: methodParm.ROMethodProperties.Method.Key, 
                readOnly: false
                );
            return GetMethod(methodParm: methodGetParm, processingApply: true);
        }

        private void CleanseMethodName()
        {
            string name = _ABM.Name;

            int userKey = SAB.ClientServerSession.UserRID;
            if (_ABM.GlobalUserType == eGlobalUserType.Global)
            {
                userKey = Include.GlobalUserRID;
            }

            int nameCntr = 0;
            while (true)
            {
                if (!WmManager.CheckForDuplicateMethodID(userKey, _ABM))
                {
                    break;
                }
                else
                {
                    nameCntr++;
                    //_ABM.Name = name + ":" + nameCntr;
                    _ABM.Name = Include.GetNewName(name: name, index: nameCntr);
                }
            }
        }


        public ROOut ProcessMethod(ROMethodParms methodParm)
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            string message = null;

            SetMethodObject(methodParm);

            if (!_ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID))
            {
                return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
            }

            // front end always uses new instance to add headers and process method.  So no need to always get new instance like in Windows code.
            ApplicationSessionTransaction applicationSessionTransaction = GetApplicationSessionTransaction(getNewTransaction: false);
            bool allowProcess = true;
            if (Enum.IsDefined(typeof(eNoHeaderMethodType), (eNoHeaderMethodType)methodParm.MethodType))
            {
                applicationSessionTransaction.NeedHeaders = false;
                //allowProcess = VerifySecurity(applicationSessionTransaction: applicationSessionTransaction);
            }
            else
            {
                applicationSessionTransaction.NeedHeaders = true;
                allowProcess = VerifySecurity(applicationSessionTransaction: applicationSessionTransaction);
            }

            if (Enum.IsDefined(typeof(eAllocationMethodType), (eAllocationMethodType)methodParm.MethodType))
            {
                SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
                ArrayList headerInGAList = new ArrayList();
                if (applicationSessionTransaction.ContainsGroupAllocationHeaders(ref headerInGAList, selectedHeaderList))
                {
                    return new RONoDataOut(eROReturnCode.Failure, MIDText.GetText(eMIDTextCode.msg_al_HeaderBelongsToGroupAllocation), ROInstanceID);
                }
            }

            if (allowProcess)
            {
                string enqMessage;


                if (EnqueueHeadersForProcessing(methodType: methodParm.MethodType))
                {
                    if (applicationSessionTransaction.EnqueueSelectedHeaders(out enqMessage))
                    {
                        applicationSessionTransaction.ProcessMethod(methodParm.MethodType, _ABM.Key);
                    }
                    else
                    {
                        return new RONoDataOut(eROReturnCode.Failure, enqMessage, ROInstanceID);
                    }
                }
                else
                {
                    applicationSessionTransaction.ProcessMethod(methodParm.MethodType, _ABM.Key);
                } 

                // OTS Plan actions and Allocation Actions resolve thier status a little differently...
                if (Enum.IsDefined(typeof(eForecastMethodType), (eForecastMethodType)methodParm.MethodType))
                {
                    eOTSPlanActionStatus actionStatus = applicationSessionTransaction.OTSPlanActionStatus;
                    message = MIDText.GetTextOnly((int)actionStatus);
                    if (actionStatus != eOTSPlanActionStatus.ActionCompletedSuccessfully)
                    {
                        returnCode = eROReturnCode.Failure;
                    }

                }
                else
                {
                    eAllocationActionStatus actionStatus = applicationSessionTransaction.AllocationActionAllHeaderStatus;

                    message = MIDText.GetTextOnly((int)actionStatus);
                    if (actionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                    {
                        returnCode = eROReturnCode.Failure;
                    }
                }
            }
            else
            {
                return new RONoDataOut(eROReturnCode.Failure, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode), ROInstanceID);
            }

            return new RONoDataOut(returnCode, message, ROInstanceID);
        }

        private bool VerifySecurity(ApplicationSessionTransaction applicationSessionTransaction)
        {
            HierarchyNodeSecurityProfile hierNodeSecProfile;
            try
            {
                bool allowUpdate = true;
                SelectedHeaderList selectedHeaderList = (SelectedHeaderList)applicationSessionTransaction.GetSelectedHeaders();
                foreach (SelectedHeaderProfile shp in selectedHeaderList)
                {
                    hierNodeSecProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(shp.StyleHnRID, (int)eSecurityTypes.Allocation);
                    if (!hierNodeSecProfile.AllowUpdate)
                    {
                        allowUpdate = false;
                        break;
                    }
                }
                return allowUpdate;
            }
            catch
            {
                throw;
            }
        }

        private bool EnqueueHeadersForProcessing(eMethodType methodType)
        {
            if (Enum.IsDefined(typeof(eNoHeaderMethodType), (eNoHeaderMethodType)methodType))
            {
                return false;
            }

            return DoEnqueue();

        }

        private bool DoEnqueue()
        {
            bool doEnqueue = true;
            try
            {
                SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
                foreach (SelectedHeaderProfile shp in selectedHeaderList)
                {
                    if (shp.HeaderType != eHeaderType.Assortment)
                    {
                        if (shp.BypassEnqueue)
                        {
                            doEnqueue = false;
                        }
                        else
                        {
                            doEnqueue = true;
                            break;
                        }
                    }
                    else
                    {
                        selectedHeaderList.Remove(shp);
                    }
                }
            }
            catch (Exception)
            {
               throw;
            }
            return doEnqueue;
        }

        public ROOut CopyMethod(ROMethodParms methodParm)
        {
            SetMethodObject(methodParm);

            if (!_ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID))
            {
                return new ROMethodPropertiesOut(eROReturnCode.Failure, null, ROInstanceID, null);
            }

            ROMethodProperties mp = _ABM.MethodCopyData();

            return new ROMethodPropertiesOut(eROReturnCode.Successful, null, ROInstanceID, mp);
        }

        public ROOut DeleteMethod(ROMethodParms methodParm)
        {
            string message = null;
            SetMethodObject(methodParm);

            if (!_ABM.Filled)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                    messageCode: eMIDTextCode.msg_ValueWasNotFound,
                    addToAuditReport: true,
                    args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_Method) }
                    );
                MIDEnvironment.requestFailed = true;
                return new RONoDataOut(eROReturnCode.Failure, MIDEnvironment.Message, ROInstanceID);
            }

            if (!FunctionSecurity.AllowDelete
                || _ABM.LockStatus == eLockStatus.ReadOnly
                || !ApplicationUtilities.AllowDeleteFromInUse(key: _ABM.Key, profileType: _ABM.MethodProfileType, SAB: SAB))
            {
                message = string.Empty;
                if (!FunctionSecurity.AllowDelete)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                    ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                else if (_ABM.LockStatus == eLockStatus.ReadOnly)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_DataIsReadOnly);
                    ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                return new RONoDataOut(eROReturnCode.Failure, message, ROInstanceID);
            }

            if (_ABM.LockStatus != eLockStatus.Locked)
            {
                message = LockMethod();
            }

            if (_ABM.LockStatus != eLockStatus.Locked)
            {
                return new RONoDataOut(eROReturnCode.Failure, message, ROInstanceID);
            }

            try
            {
                ClientTransaction.DataAccess.OpenUpdateConnection();
                DeleteReferences(key: _ABM.Key, profileType: _ABM.ProfileType, dataAccess: ClientTransaction.DataAccess);
                _ABM.Method_Change_Type = eChangeType.delete;
                _ABM.Update(ClientTransaction.DataAccess);
                ClientTransaction.DataAccess.CommitData();
                // remove from method collection
                _workflowMethods.Remove(_ABM.Key);

                _ABM.LockStatus = WorkflowMethodUtilities.UnlockWorkflowMethod(
                    SAB: SAB,
                    workflowMethodIND: eWorkflowMethodIND.Methods,
                    Key: _ABM.Key,
                    message: out message
                    );
                if (_ABM.LockStatus == eLockStatus.Cancel)
                {
                    MIDEnvironment.Message = message;
                }
            }
            catch (DatabaseForeignKeyViolation)
            {
                return new RONoDataOut(eROReturnCode.Failure, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), ROInstanceID);
            }
            finally
            {
                ClientTransaction.DataAccess.CloseUpdateConnection();
            }

            return new RONoDataOut(eROReturnCode.Successful, message, ROInstanceID);
        }

        private string LockMethod()
        {
            string message = null;
            _ABM.LockStatus = WorkflowMethodUtilities.LockWorkflowMethod(
                   SAB: SAB,
                   workflowMethodIND: eWorkflowMethodIND.Methods,
                   aChangeType: eChangeType.delete,
                   Key: _ABM.Key,
                   Name: _ABM.Name,
                   allowReadOnly: false,
                   message: out message
                    );
            return message;
        }

        private void SetMethodObject(ROMethodParms methodParm)
        {
            // if don't have a method or have a different method, check if need to create it
            if (_ABM == null
                || _ABM.Key != methodParm.Key)
            {
                // need to determine type of method
                if (!Enum.IsDefined(typeof(eMethodType), (int)methodParm.MethodType) ||
                    methodParm.MethodType == eMethodType.NotSpecified)
                {
                    MethodBaseData DlMethodData = new MethodBaseData();
                    methodParm.MethodType = DlMethodData.GetMethodType(methodParm.Key);
                }

                // check if already have method in the collection.  If not create it.
                if (!_workflowMethods.TryGetValue(methodParm.Key, out _ABM))
                {
                    _ABM = (ApplicationBaseMethod)GetMethods.GetMethod(methodParm.Key, methodParm.MethodType);
                    _workflowMethods[_ABM.Key] = _ABM;
                }
                FunctionSecurity = _ABM.GetFunctionSecurity();
                if (_ABM is VelocityMethod)
                {
                    ((VelocityMethod)_ABM).AST = _applicationSessionTransaction;
                }
            }
        }

        #endregion Method Processing

        #region Get WorkFlows
        public ROOut GetWorkflowsData()
        {
            return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, GetWorkflows());
        }

        private List<KeyValuePair<int, string>> GetWorkflows()
        {
            DataTable dtWorkflows = new DataTable();
            WorkflowBaseData workflowData = new WorkflowBaseData();


            // Get global workflows
            dtWorkflows = workflowData.GetWorkflows(GetWorkflowType(), Include.GlobalUserRID, false);

            // Add user workflows
            CopyDataRows(dtWorkflows, workflowData.GetWorkflows(GetWorkflowType(), SAB.ClientServerSession.UserRID, false));

            // Add shared workflows
            CopyDataRows(dtWorkflows, workflowData.GetSharedWorkflows(SAB.ClientServerSession.UserRID));

            dtWorkflows = ApplicationUtilities.SortDataTable(dataTable: dtWorkflows, sColName: "WORKFLOW_NAME", bAscending: true);

            return ApplicationUtilities.DataTableToKeyValues(dtWorkflows, "WORKFLOW_RID", "WORKFLOW_NAME");

        }
        #endregion Get WorkFlows

        #region Workflow Details
        public ROOut GetWorkflowDetails(ROKeyParms parms)
        {
            try
            {
                ROWorkflow roWorkflow = BuildWorkflowOut(parms);

                SetWorkflowObject();

                if (!FunctionSecurity.IsReadOnly)
                {
                    string message = null;
                    _ABW.LockStatus = WorkflowMethodUtilities.LockWorkflowMethod(
                        SAB: SAB,
                        workflowMethodIND: eWorkflowMethodIND.Workflows,
                        aChangeType: eChangeType.update,
                        Key: roWorkflow.Workflow.Key,
                        Name: roWorkflow.Workflow.Value,
                        allowReadOnly: true,
                        message: out message
                        );
                    if (_ABW.LockStatus == eLockStatus.ReadOnly)
                    {
                        MIDEnvironment.isChangedToReadOnly = true;
                        MIDEnvironment.Message = message;
                    }
                }

                return new ROWorkflowOut(eROReturnCode.Successful, null, ROInstanceID, roWorkflow);
            }
            catch
            {
                throw;
            }
        }

        #endregion Workflow Details

        #region Workflows  - Save/Update
        public ROOut SaveWorkflow(ROWorkflowPropertiesParms rOWorkflow)
        {
            bool WorkflowSaved = false;

            WorkflowSaved = SaveWorkflowData(rOWorkflow);

            return new ROIListOut(eROReturnCode.Successful, null, ROInstanceID,
                                    WorkflowMethodUtilities.BuildWorkflowNode(GetApplicationType(), _ABW));
        }

        internal bool SaveWorkflowData(ROWorkflowPropertiesParms rOWorkflow)
        {
            SetWorkflowCommonFields(rOWorkflow);

            return SaveWorkflowChanges(rOWorkflow);
        }

        internal void SetWorkflowCommonFields(ROWorkflowPropertiesParms rOWorkflow)
        {
            int userRID;
            if (rOWorkflow.ROWorkflow.GlobalUserType == eGlobalUserType.Global)
            {
                userRID = Include.GlobalUserRID;
            }
            else
            {
                userRID = SAB.ClientServerSession.UserRID;
            }

            bool cleanseName = false;
            if (rOWorkflow.ROWorkflow.Workflow.Key != Include.NoRID)
            {
                SetWorkflowObject();
                if (_ABW == null)
                {
                    CreateWorkflowObject(key: rOWorkflow.ROWorkflow.Workflow.Key, userRID: userRID, globalFlagValue: false);
                    SetWorkflowObject();
                    _ABW.UserRID = userRID;
                }

                _ABW.Workflow_Change_Type = eChangeType.update;
                cleanseName = _ABW.WorkFlowName != rOWorkflow.ROWorkflow.Workflow.Value;
            }
            else
            {
                CreateWorkflowObject(key: rOWorkflow.ROWorkflow.Workflow.Key, userRID: userRID, globalFlagValue: false);
                SetWorkflowObject();

                _ABW.Workflow_Change_Type = eChangeType.add;
                cleanseName = true;
            }

            _ABW.UserRID = userRID;
            _ABW.WorkFlowName = rOWorkflow.ROWorkflow.Workflow.Value;
            if (cleanseName)
            {
                CleanseWorkflowName();
            }
            _ABW.WorkFlowDescription = rOWorkflow.ROWorkflow.WorkflowDescription;
        }

        internal bool SaveWorkflowChanges(ROWorkflowPropertiesParms rOWorkflow)
        {
            try
            {
                _ABW.Workflow_Steps.Clear();

                _ABW.StoreFilterRID = rOWorkflow.ROWorkflow.Filter.Key;

                SetWorkflowSteps(rOWorkflow);

                int folderKey = rOWorkflow.FolderKey;
                if (_ABW.Workflow_Change_Type == eChangeType.add)
                {
                    folderKey = WorkflowMethodUtilities.GetWorkflowMethodFolderRID(GetFolderProfileType(), rOWorkflow.FolderKey, _ABW.UserRID, _ABW.ProfileType, rOWorkflow.FolderUniqueID);
                }

                try
                {
                    ClientTransaction.DataAccess.OpenUpdateConnection();


                    _ABW.Update(ClientTransaction.DataAccess);

                    switch (_ABW.Key)
                    {
                        case (int)eGenericDBError.GenericDBError:
                            _nameValid = false;
                            _nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_GenericMethodInsertError);
                            return true;
                        case (int)eGenericDBError.DuplicateKey:
                            _nameValid = false;
                            _nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateMethod);
                            return true;
                        default:
                            break;
                    }

                    if (_ABW.Workflow_Change_Type == eChangeType.add)
                    {
                        FolderDataLayer dlFolder = new FolderDataLayer(ClientTransaction.DataAccess);
                        dlFolder.Folder_Item_Insert(folderKey, _ABW.Key, _ABW.ProfileType);
                    }

                    ClientTransaction.DataAccess.CommitData();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    ClientTransaction.DataAccess.CloseUpdateConnection();
                }

            }
            catch
            {
                throw;
            }

            return true;
        }

        #endregion Workflows  - Save/Update


        #region Delete WorkFlow
        public ROOut DeleteWorkflow(ROKeyParms parms)
        {
            try
            {
                if (WorkflowDeleted(parms))
                {
                    return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
                }
                else
                {
                    return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool WorkflowDeleted(ROKeyParms parms)
        {
            string message = null;

            if (parms.Key != Include.NoRID)
            {
                if (_ABW == null)
                {
                    CreateWorkflowObject(key: parms.Key, userRID: Include.Undefined, globalFlagValue: false);
                    SetWorkflowObject();
                    if (!_ABW.Filled)
                    {
                        MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_ValueWasNotFound,
                            addToAuditReport: true,
                            args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow) }
                            );
                        MIDEnvironment.requestFailed = true;
                        return false;
                    }
                }
            }

            if (parms.Key != _ABW.Key)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                       messageCode: eMIDTextCode.msg_CallParmsDoNotMatchInstance,
                       addToAuditReport: true
                       );
                MIDEnvironment.requestFailed = true;
                return false;
            }

            if (!FunctionSecurity.AllowDelete
                || _ABW.LockStatus == eLockStatus.ReadOnly
                || !ApplicationUtilities.AllowDeleteFromInUse(key: _ABW.Key, profileType: _ABW.ProfileType, SAB: SAB))
            {
                message = string.Empty;
                if (!FunctionSecurity.AllowDelete)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                    ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                else if (_ABW.LockStatus == eLockStatus.ReadOnly)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_DataIsReadOnly);
                    ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = LockWorkflow();
                }
                return false;
            }

            if (_ABW.LockStatus != eLockStatus.Locked)
            {
                message = LockWorkflow();
                if (_ABW.LockStatus != eLockStatus.Locked)
                {
                    MIDEnvironment.Message = message;
                    return false;
                }
            }

            try
            {
                ClientTransaction.DataAccess.OpenUpdateConnection();
                DeleteReferences(key: _ABW.Key, profileType: _ABW.ProfileType, dataAccess: ClientTransaction.DataAccess);
                _ABW.Workflow_Change_Type = eChangeType.delete;
                _ABW.Update(ClientTransaction.DataAccess);
                ClientTransaction.DataAccess.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                ClientTransaction.DataAccess.CloseUpdateConnection();
            }

            _ABW.LockStatus = WorkflowMethodUtilities.UnlockWorkflowMethod(
                SAB: SAB,
                workflowMethodIND: eWorkflowMethodIND.Workflows,
                Key: _ABW.Key,
                message: out message
                );
            if (_ABW.LockStatus == eLockStatus.Cancel)
            {
                MIDEnvironment.Message = message;
            }

            return true;
        }

        
        #endregion Delete WorkFlow

        #region Rename WorkFlow/Method

        public ROOut Rename(RODataExplorerRenameParms parms)
        {
            try
            {
                if (parms.ProfileType == eProfileType.Workflow)
                {
                    if (WorkflowRenamed(parms))
                    {
                        return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
                    }
                    else
                    {
                        return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                    }
                }
                else
                {
                    return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool WorkflowRenamed(RODataExplorerRenameParms parms)
        {
            string message = null;

            if (parms.NewName.Trim().Length == 0)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_NameRequired,
                            addToAuditReport: true
                            );
                MIDEnvironment.requestFailed = true;
                return false;
            }

            if (parms.Key != Include.NoRID)
            {
                if (_ABW == null)
                {
                    CreateWorkflowObject(key: parms.Key, userRID: Include.Undefined, globalFlagValue: false);
                    SetWorkflowObject();
                    if (!_ABW.Filled)
                    {
                        MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_ValueWasNotFound,
                            addToAuditReport: true,
                            args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow) }
                            );
                        MIDEnvironment.requestFailed = true;
                        return false;
                    }
                }
            }

            if (parms.Key != _ABW.Key)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                       messageCode: eMIDTextCode.msg_CallParmsDoNotMatchInstance,
                       addToAuditReport: true
                       );
                MIDEnvironment.requestFailed = true;
                return false;
            }

            if (_ABW.WorkFlowName == parms.NewName)
            {
                return true;
            }

            if (_ABW.LockStatus != eLockStatus.Locked)
            {
                message = LockWorkflow();
                if (_ABW.LockStatus != eLockStatus.Locked)
                {
                    MIDEnvironment.Message = message;
                    return false;
                }
            }

            _ABW.Workflow_Change_Type = eChangeType.update;
            _ABW.WorkFlowName = parms.NewName;

            if (isDuplicateName(ref message))
            {
                MIDEnvironment.Message = message;
                return false;
            }

            try
            {
                ClientTransaction.DataAccess.OpenUpdateConnection();
                _ABW.Workflow_Change_Type = eChangeType.update;
                _ABW.Update(ClientTransaction.DataAccess);
                ClientTransaction.DataAccess.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                ClientTransaction.DataAccess.CloseUpdateConnection();
            }

            _ABW.LockStatus = WorkflowMethodUtilities.UnlockWorkflowMethod(
                SAB: SAB,
                workflowMethodIND: eWorkflowMethodIND.Workflows,
                Key: _ABW.Key,
                message: out message
                );
            if (_ABW.LockStatus == eLockStatus.Cancel)
            {
                MIDEnvironment.Message = message;
            }

            return true;
        }

        #endregion Rename WorkFlow/Method

        #region Copy WorkFlow/Method

        public ROOut Copy(RODataExplorerCopyParms parms)
        {
            try
            {
                if (parms.ProfileType == eProfileType.Workflow)
                {
                    if (WorkflowCopied(parms))
                    {
                        return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
                    }
                    else
                    {
                        return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                    }
                }
                else
                {
                    return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool WorkflowCopied(RODataExplorerCopyParms parms)
        {
            if (parms.Key != Include.NoRID)
            {
                if (_ABW == null)
                {
                    CreateWorkflowObject(key: parms.Key, userRID: Include.Undefined, globalFlagValue: false);
                    SetWorkflowObject();
                    if (!_ABW.Filled)
                    {
                        MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_ValueWasNotFound,
                            addToAuditReport: true,
                            args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow) }
                            );
                        MIDEnvironment.requestFailed = true;
                        return false;
                    }
                }
            }

            if (parms.Key != _ABW.Key)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                       messageCode: eMIDTextCode.msg_CallParmsDoNotMatchInstance,
                       addToAuditReport: true
                       );
                MIDEnvironment.requestFailed = true;
                return false;
            }

            _ABW = _ABW.Copy(SAB.ClientServerSession, true);
            _ABW.Key = Include.NoRID;
            _ABW.UserRID = parms.NewUserKey;
            _ABW.Workflow_Change_Type = eChangeType.add;
            CleanseWorkflowName();

            int folderKey = WorkflowMethodUtilities.GetWorkflowMethodFolderRID(GetFolderProfileType(), parms.ToParentKey, _ABW.UserRID, _ABW.ProfileType, parms.ToParentUniqueID);

            try
            {
                ClientTransaction.DataAccess.OpenUpdateConnection();

                _ABW.Update(ClientTransaction.DataAccess);

                // Add folder entry
                FolderDataLayer dlFolder = new FolderDataLayer(ClientTransaction.DataAccess);
                dlFolder.Folder_Item_Insert(folderKey, _ABW.Key, _ABW.ProfileType);

                ClientTransaction.DataAccess.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                ClientTransaction.DataAccess.CloseUpdateConnection();
            }

            return true;
        }

        #endregion Copy WorkFlow/Method

        #region SaveAs WorkFlow/Method

        public ROOut SaveAs(RODataExplorerSaveAsParms parms)
        {
            try
            {
                if (parms.ProfileType == eProfileType.Workflow)
                {
                    if (WorkflowSaveAs(parms))
                    {
                        return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
                    }
                    else
                    {
                        return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                    }
                }
                else
                {
                    return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool WorkflowSaveAs(RODataExplorerSaveAsParms parms)
        {
            if (parms.Key != Include.NoRID)
            {
                if (_ABW == null)
                {
                    CreateWorkflowObject(key: parms.Key, userRID: Include.Undefined, globalFlagValue: false);
                    SetWorkflowObject();
                    if (!_ABW.Filled)
                    {
                        MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_ValueWasNotFound,
                            addToAuditReport: true,
                            args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow) }
                            );
                        MIDEnvironment.requestFailed = true;
                        return false;
                    }
                }
            }

            if (parms.Key != _ABW.Key)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                       messageCode: eMIDTextCode.msg_CallParmsDoNotMatchInstance,
                       addToAuditReport: true
                       );
                MIDEnvironment.requestFailed = true;
                return false;
            }

            _ABW = _ABW.Copy(SAB.ClientServerSession, true);
            _ABW.Key = Include.NoRID;
            _ABW.UserRID = parms.NewUserKey;
            if (!string.IsNullOrEmpty(parms.NewName))
            {
                _ABW.WorkFlowName = parms.NewName;
            }
            _ABW.Workflow_Change_Type = eChangeType.add;
            CleanseWorkflowName();

            int folderKey = WorkflowMethodUtilities.GetWorkflowMethodFolderRID(GetFolderProfileType(), parms.ToParentKey, _ABW.UserRID, _ABW.ProfileType, parms.ToParentUniqueID);

            try
            {
                ClientTransaction.DataAccess.OpenUpdateConnection();

                _ABW.Update(ClientTransaction.DataAccess);

                // Add folder entry
                FolderDataLayer dlFolder = new FolderDataLayer(ClientTransaction.DataAccess);
                dlFolder.Folder_Item_Insert(folderKey, _ABW.Key, _ABW.ProfileType);

                ClientTransaction.DataAccess.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                ClientTransaction.DataAccess.CloseUpdateConnection();
            }

            return true;
        }

        #endregion Save As WorkFlow/Method

        #region ShortCut

        public ROOut AddShortCut(RODataExplorerShortcutParms parms)
        {
            try
            {
                if (parms.ProfileType == eProfileType.Workflow)
                {
                    if (ShortCutAdded(parms))
                    {
                        return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
                    }
                    else
                    {
                        return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                    }
                }
                else
                {
                    return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool ShortCutAdded(RODataExplorerShortcutParms parms)
        {
            if (parms.Key != Include.NoRID)
            {
                if (_ABW == null)
                {
                    CreateWorkflowObject(key: parms.Key, userRID: Include.Undefined, globalFlagValue: false);
                    SetWorkflowObject();
                    if (!_ABW.Filled)
                    {
                        MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_ValueWasNotFound,
                            addToAuditReport: true,
                            args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow) }
                            );
                        MIDEnvironment.requestFailed = true;
                        return false;
                    }
                }
            }

            if (parms.Key != _ABW.Key)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                       messageCode: eMIDTextCode.msg_CallParmsDoNotMatchInstance,
                       addToAuditReport: true
                       );
                MIDEnvironment.requestFailed = true;
                return false;
            }

            int folderKey = WorkflowMethodUtilities.GetWorkflowMethodFolderRID(GetFolderProfileType(), parms.ToParentKey, _ABW.UserRID, _ABW.ProfileType, parms.ToParentUniqueID);

            try
            {
                ClientTransaction.DataAccess.OpenUpdateConnection();

                // Add folder entry
                FolderDataLayer dlFolder = new FolderDataLayer(ClientTransaction.DataAccess);

                if (dlFolder.Folder_Shortcut_Exists(folderKey, _ABW.Key, _ABW.ProfileType))
                {
                    MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ShortcutExists);
                    MIDEnvironment.requestFailed = true;
                    return false;
                }

                dlFolder.Folder_Shortcut_Insert(folderKey, _ABW.Key, _ABW.ProfileType);

                ClientTransaction.DataAccess.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                ClientTransaction.DataAccess.CloseUpdateConnection();
            }

            return true;
        }

        public ROOut DeleteShortCut(RODataExplorerShortcutParms parms)
        {
            try
            {
                if (parms.ProfileType == eProfileType.Workflow)
                {
                    if (ShortCutDeleted(parms))
                    {
                        return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
                    }
                    else
                    {
                        return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                    }
                }
                else
                {
                    return new RONoDataOut(eROReturnCode.Failure, null, ROInstanceID);
                }
            }
            catch
            {
                throw;
            }
        }

        private bool ShortCutDeleted(RODataExplorerShortcutParms parms)
        {
            if (parms.Key != Include.NoRID)
            {
                if (_ABW == null)
                {
                    CreateWorkflowObject(key: parms.Key, userRID: Include.Undefined, globalFlagValue: false);
                    SetWorkflowObject();
                    if (!_ABW.Filled)
                    {
                        MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                            messageCode: eMIDTextCode.msg_ValueWasNotFound,
                            addToAuditReport: true,
                            args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow) }
                            );
                        MIDEnvironment.requestFailed = true;
                        return false;
                    }
                }
            }

            if (parms.Key != _ABW.Key)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(
                       messageCode: eMIDTextCode.msg_CallParmsDoNotMatchInstance,
                       addToAuditReport: true
                       );
                MIDEnvironment.requestFailed = true;
                return false;
            }

            int folderKey = WorkflowMethodUtilities.GetWorkflowMethodFolderRID(GetFolderProfileType(), parms.ParentKey, _ABW.UserRID, _ABW.ProfileType, parms.ParentUniqueID);

            try
            {
                ClientTransaction.DataAccess.OpenUpdateConnection();

                // Add folder entry
                FolderDataLayer dlFolder = new FolderDataLayer(ClientTransaction.DataAccess);

                dlFolder.Folder_Shortcut_Delete(folderKey, _ABW.Key, _ABW.ProfileType);

                ClientTransaction.DataAccess.CommitData();
            }
            catch
            {
                throw;
            }
            finally
            {
                ClientTransaction.DataAccess.CloseUpdateConnection();
            }

            return true;
        }

        #endregion ShortCut

        private void CopyDataRows(DataTable dtWorkflows, DataTable dtInput)
        {
            if (dtInput.Rows.Count > 0)
            {
                foreach (DataRow dr in dtInput.Rows)
                {
                    DataRow newRow = dtWorkflows.NewRow();
                    newRow["WORKFLOW_RID"] = dr["WORKFLOW_RID"];
                    newRow["WORKFLOW_NAME"] = dr["WORKFLOW_NAME"] + " (" + UserNameStorage.GetUserName(Convert.ToInt32(dr["OWNER_USER_RID"])) + ")";
                    newRow["WORKFLOW_TYPE_ID"] = dr["WORKFLOW_TYPE_ID"];
                    newRow["WORKFLOW_DESCRIPTION"] = dr["WORKFLOW_DESCRIPTION"];
                    newRow["STORE_FILTER_RID"] = dr["STORE_FILTER_RID"];
                    newRow["WORKFLOW_OVERRIDE"] = dr["WORKFLOW_OVERRIDE"];
                    newRow["WORKFLOW_USER_RID"] = dr["WORKFLOW_USER_RID"];
                    newRow["ITEM_TYPE"] = dr["ITEM_TYPE"];
                    newRow["OWNER_USER_RID"] = dr["OWNER_USER_RID"];

                    dtWorkflows.Rows.Add(newRow);
                }
            }
        }

        private string LockWorkflow()
        {
            string message = null;
            _ABW.LockStatus = WorkflowMethodUtilities.LockWorkflowMethod(
                    SAB: SAB,
                    workflowMethodIND: eWorkflowMethodIND.Workflows,
                    aChangeType: eChangeType.delete,
                    Key: _ABW.Key,
                    Name: _ABW.WorkFlowName,
                    allowReadOnly: false,
                    message: out message
                    );
            return message;
        }

        private void CleanseWorkflowName()
        {
            string name = _ABW.WorkFlowName;

            int userKey = SAB.ClientServerSession.UserRID;
            if (_ABW.GlobalUserType == eGlobalUserType.Global)
            {
                userKey = Include.GlobalUserRID;
            }

            int nameCntr = 0;
            while (true)
            {
                if (!WmManager.CheckForDuplicateWorkflowID(userKey, _ABW))
                {
                    break;
                }
                else
                {
                    nameCntr++;
                    //_ABW.WorkFlowName = name + ":" + nameCntr;
                    _ABW.WorkFlowName = Include.GetNewName(name: name, index: nameCntr);
                }
            }
        }

        private bool isDuplicateName(ref string message)
        {
            bool duplicateFound = false;
            try
            {
                if (_ABM != null)
                {
                    if (WmManager.CheckForDuplicateMethodID(SAB.ClientServerSession.UserRID, _ABM))
                    {
                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateMethod);
                        duplicateFound = true;
                    }
                }
                else
                {
                    if (WmManager.CheckForDuplicateWorkflowID(SAB.ClientServerSession.UserRID, _ABW))
                    {
                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateWorkflow);
                        duplicateFound = true;
                    }
                }
            }
            catch
            {
                throw;
            }
            return duplicateFound;
        }

        protected string CleanseFolderName(string folderName, int userRID, int parentRID, eProfileType itemType)
        {
            int index;
            string newName;
            int key;
            FolderDataLayer dlFolder;

            try
            {
                dlFolder = new FolderDataLayer();
                index = 1;
                newName = folderName;
                key = dlFolder.Folder_GetKey(userRID, newName, parentRID, itemType);

                while (key != -1)
                {
                    index++;
                    //newName = folderName + " (" + index + ")";
                    newName = Include.GetNewName(name: folderName, index: index);
                    key = dlFolder.Folder_GetKey(userRID, newName, parentRID, itemType);
                }

                return newName;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        abstract protected eROApplicationType GetApplicationType();

        abstract protected eProfileType GetFolderProfileType();

        abstract protected void CreateWorkflowObject(int key, int userRID, bool globalFlagValue);

        abstract protected void SetWorkflowObject();

        abstract protected eWorkflowType GetWorkflowType();

        abstract protected void SetWorkflowSteps(ROWorkflowPropertiesParms rOWorkflow);

        abstract protected ROWorkflow BuildWorkflowOut(ROKeyParms parms);

        protected bool DeleteReferences(int key, eProfileType profileType, TransactionData dataAccess)
        {
            FolderDataLayer _dlFolder = new FolderDataLayer(td: dataAccess);

            try
            {
                _dlFolder.Folder_Item_Delete(aChildRID: key, aFolderChildType: profileType);
                _dlFolder.Folder_Shortcut_DeleteAll(aChildRID: key, aFolderChildType: profileType);
            }
            catch (DatabaseForeignKeyViolation)
            {
                MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                MIDEnvironment.requestFailed = true;
                return false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }

            return true;
        }


    }

    public class ROAllocationWorkflowMethodManager : ROWorkflowMethodManager
    {
        private AllocationWorkFlow _allocationWorkflow;


        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        /// <param name="ROInstanceID">The instance ID of the session</param>
        public ROAllocationWorkflowMethodManager(SessionAddressBlock SAB, ApplicationSessionTransaction applicationSessionTransaction, ROWebTools ROWebTools, long ROInstanceID)
            : base(SAB, ROWebTools, ROInstanceID, eROApplicationType.Allocation, applicationSessionTransaction)
        {

        }

        override protected eROApplicationType GetApplicationType()
        {
            return eROApplicationType.Allocation;
        }

        override protected eProfileType GetFolderProfileType()
        {
            return eProfileType.WorkflowMethodAllocationFolder;
        }

        override protected void CreateWorkflowObject(int key, int userRID, bool globalFlagValue)
        {
            _allocationWorkflow = new AllocationWorkFlow(SAB, key, userRID, globalFlagValue);
        }

        override protected void SetWorkflowObject()
        {
            if (_allocationWorkflow != null
                    && _ABW == null)
            {
                _ABW = _allocationWorkflow;
            }
            FunctionSecurity = _ABW.GetFunctionSecurity();
        }

        override protected eWorkflowType GetWorkflowType()
        {
            return eWorkflowType.Allocation;
        }

        override protected ROWorkflow BuildWorkflowOut(ROKeyParms parms)
        {
            ROWorkflow rOWorkflow;
            int aWorkflowRID = parms.Key;

            _allocationWorkflow = new AllocationWorkFlow(SAB, aWorkflowRID, SAB.ClientServerSession.UserRID, false);

            eWorkflowType workflowType = _allocationWorkflow.WorkFlowType;

            KeyValuePair<int, string> workflow = new KeyValuePair<int, string>(aWorkflowRID, _allocationWorkflow.WorkFlowName);

            KeyValuePair<int, string> filter = GetName.GetFilterName(_allocationWorkflow.StoreFilterRID);

            string workflowDescription = _allocationWorkflow.WorkFlowDescription;

            eGlobalUserType userType = _allocationWorkflow.GlobalUserType;

            int userId = _allocationWorkflow.UserRID;

            eProfileType profileType = _allocationWorkflow.ProfileType;

            bool isFilled = _allocationWorkflow.Filled;

            var allocationworkflowSteps = BuildAllocationWorkflowSteps(parms);

            rOWorkflow = new ROWorkflow(workflowType, workflow, workflowDescription, filter, userType, userId, profileType, isFilled, allocationworkflowSteps);

            eSecurityFunctions securityfunctions;
            if (rOWorkflow.GlobalUserType == eGlobalUserType.Global)
            {
                securityfunctions = eSecurityFunctions.AllocationWorkflowsGlobal;
            }
            else
            {
                securityfunctions = eSecurityFunctions.AllocationWorkflowsUser;
            }

            FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(securityfunctions);
            rOWorkflow.IsReadOnly = FunctionSecurity.IsReadOnly;
            rOWorkflow.CanBeProcessed = FunctionSecurity.AllowExecute;
            rOWorkflow.CanBeDeleted = FunctionSecurity.AllowDelete;

            return rOWorkflow;
        }

        internal List<ROWorkflowStep> BuildAllocationWorkflowSteps(ROKeyParms parms)
        {
            int aWorkflowRID = parms.Key;

            List<ROWorkflowStep> rOWorkflowSteps = new List<ROWorkflowStep>();
            _allocationWorkflow = new AllocationWorkFlow(SAB, aWorkflowRID, SAB.ClientServerSession.UserRID, false);
            int rowPosition = 0;
            int specificRID = Include.NoRID;
            string specificName = string.Empty;
            string methodName = string.Empty;
            int methodType;
            int componentType;
            ApplicationBaseMethod abm = null;
            SecurityAdmin secAdmin = new SecurityAdmin();
            bool review;
            double dTolerancePercent = 0;

            foreach (AllocationWorkFlowStep aws in _allocationWorkflow.Workflow_Steps)
            {
                GeneralComponentWrapper gcw = new GeneralComponentWrapper(aws.Component);
                specificRID = Include.NoRID;

                specificName = string.Empty;
                methodName = string.Empty;
                review = aws.Review;

                switch (gcw.ComponentType)
                {
                    case eComponentType.SpecificColor:
                        specificRID = gcw.ColorRID;
                        ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(specificRID);
                        specificName = ccp.ColorCodeID;
                        break;
                    case eComponentType.SpecificSize:
                        specificRID = gcw.SizeRID;
                        SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(specificRID);
                        specificName = scp.SizeCodeID;
                        break;
                    case eComponentType.SpecificPack:
                        specificName = gcw.PackName;
                        break;
                    default:
                        break;
                }

                methodType = Convert.ToInt32(aws.Method.MethodType, CultureInfo.CurrentUICulture);
                if (Enum.IsDefined(typeof(eMethodTypeUI), methodType))
                {
                    abm = (ApplicationBaseMethod)aws.Method;
                    methodName = ApplicationUtilities.Adjust_Name(abm.Name, abm.User_RID);
                }

                componentType = Convert.ToInt32(aws.Component.ComponentType, CultureInfo.CurrentUICulture);
                if (aws.TolerancePercent == SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent)
                {
                    dTolerancePercent = Include.UseSystemTolerancePercent;
                }
                else
                {
                    dTolerancePercent = aws.TolerancePercent;
                }

                KeyValuePair<int, string> methodAction = new KeyValuePair<int, string>(methodType, MIDText.GetTextOnly(methodType));

                KeyValuePair<int, string> method = new KeyValuePair<int, string>(aws.Method.Key, methodName);

                KeyValuePair<int, string> specific = new KeyValuePair<int, string>(specificRID, specificName);

                rOWorkflowSteps.Add(new ROAllocationWorkflowStep(rowPosition, methodAction, method, specific, aws.Component.ComponentType, review, dTolerancePercent));

                ++rowPosition;
            }


            return rOWorkflowSteps;

        }

        override protected void SetWorkflowSteps(ROWorkflowPropertiesParms rOWorkflow)
        {
            AllocationWorkFlowStep awfs = null;
            ApplicationBaseAction method;
            eMethodType methodType;
            bool reviewFlag = false;
            bool useSystemTolerancePercent = true;
            double tolerancePercent = Include.UseSystemTolerancePercent;
            int storeFilter = Include.UndefinedStoreFilter;
            int workFlowStepKey = 0;

            tolerancePercent = SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent;

            storeFilter = rOWorkflow.ROWorkflow.Filter.Key;
            foreach (ROAllocationWorkflowStep item in rOWorkflow.ROWorkflow.WorkflowSteps)
            {
                if (Enum.IsDefined(typeof(eAllocationActionType), Convert.ToInt32((eMethodType)item.MethodAction.Key, CultureInfo.CurrentUICulture)))
                {
                    methodType = (eMethodType)Convert.ToInt32((eMethodType)item.MethodAction.Key, CultureInfo.CurrentUICulture);
                    method = new AllocationAction(methodType);
                }
                else
                {
                    method = GetMethods.GetMethod(item.Method.Key, (eMethodType)item.MethodAction.Key);
                }

                //GeneralComponent generalComponent = new GeneralComponent(eGeneralComponentType.GenericType);
                GeneralComponent generalComponent = new GeneralComponent((eGeneralComponentType)item.ComponentType);

                if (item.TolerancePercent == Include.UseSystemTolerancePercent
                    || item.TolerancePercent == SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent)
                {
                    useSystemTolerancePercent = true;
                    tolerancePercent = SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent;
                }
                else
                {
                    useSystemTolerancePercent = false;
                    tolerancePercent = item.TolerancePercent;
                }

                awfs = new AllocationWorkFlowStep(method, generalComponent, reviewFlag,
                              useSystemTolerancePercent, tolerancePercent, storeFilter, workFlowStepKey);

                _ABW.Workflow_Steps.Add(awfs);

                ++workFlowStepKey;
            }
        }
    }

    public class ROPlanningWorkflowMethodManager : ROWorkflowMethodManager
    {
        private OTSPlanWorkFlow _planningWorkflow;

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        /// <param name="ROInstanceID">The instance ID of the session</param>
        public ROPlanningWorkflowMethodManager(SessionAddressBlock SAB, ApplicationSessionTransaction applicationSessionTransaction, ROWebTools ROWebTools, long ROInstanceID)
            : base(SAB, ROWebTools, ROInstanceID, eROApplicationType.Forecast, applicationSessionTransaction)
        {

        }

        override protected eROApplicationType GetApplicationType()
        {
            return eROApplicationType.Forecast;
        }

        override protected eProfileType GetFolderProfileType()
        {
            return eProfileType.WorkflowMethodOTSForcastFolder;
        }

        override protected void CreateWorkflowObject(int key, int userRID, bool globalFlagValue)
        {
            _planningWorkflow = new OTSPlanWorkFlow(SAB, key, userRID, globalFlagValue);
        }

        override protected void SetWorkflowObject()
        {
            if (_planningWorkflow != null
                    && _ABW == null)
            {
                _ABW = _planningWorkflow;
            }
            FunctionSecurity = _ABW.GetFunctionSecurity();
        }

        override protected eWorkflowType GetWorkflowType()
        {
            return eWorkflowType.Forecast;
        }

        override protected ROWorkflow BuildWorkflowOut(ROKeyParms parms)
        {
            ROWorkflow rOWorkflow;
            int aWorkflowRID = parms.Key;

            _planningWorkflow = new OTSPlanWorkFlow(SAB, aWorkflowRID, SAB.ClientServerSession.UserRID, false);

            eWorkflowType workflowType = _planningWorkflow.WorkFlowType;

            KeyValuePair<int, string> workflow = new KeyValuePair<int, string>(aWorkflowRID, _planningWorkflow.WorkFlowName);

            KeyValuePair<int, string> filter = GetName.GetFilterName(_planningWorkflow.StoreFilterRID);

            string workflowDescription = _planningWorkflow.WorkFlowDescription;

            eGlobalUserType userType = _planningWorkflow.GlobalUserType;

            int userId = _planningWorkflow.UserRID;

            eProfileType profileType = _planningWorkflow.ProfileType;

            bool isFilled = _planningWorkflow.Filled;

            var otsforecastworkflowSteps = BuildOTSForecastWorkflowSteps(parms);

            rOWorkflow = new ROWorkflow(workflowType, workflow, workflowDescription, filter, userType, userId, profileType, isFilled, otsforecastworkflowSteps);

            eSecurityFunctions securityfunctions;
            if (rOWorkflow.GlobalUserType == eGlobalUserType.Global)
            {
                securityfunctions = eSecurityFunctions.ForecastWorkflowsGlobal;
            }
            else
            {
                securityfunctions = eSecurityFunctions.ForecastWorkflowsUser;
            }

            FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(securityfunctions);
            rOWorkflow.IsReadOnly = FunctionSecurity.IsReadOnly;
            rOWorkflow.CanBeProcessed = FunctionSecurity.AllowExecute;
            rOWorkflow.CanBeDeleted = FunctionSecurity.AllowDelete;

            return rOWorkflow;
        }

        internal List<ROWorkflowStep> BuildOTSForecastWorkflowSteps(ROKeyParms parms)
        {
            int aWorkflowRID = parms.Key;

            List<ROWorkflowStep> rOWorkflowSteps = new List<ROWorkflowStep>();
            _planningWorkflow = new OTSPlanWorkFlow(SAB, aWorkflowRID, SAB.ClientServerSession.UserRID, false);
            int rowPosition = 0;
            int specificRID = Include.NoRID;
            string specificName = string.Empty;
            string methodName = string.Empty;
            int methodType;
            ApplicationBaseMethod abm = null;
            SecurityAdmin secAdmin = new SecurityAdmin();
            bool review;
            double dTolerancePercent = 0;
            int variableNumber = -1;

            foreach (OTSPlanWorkFlowStep otspws in _planningWorkflow.Workflow_Steps)
            {
                specificRID = Include.NoRID;
                specificName = string.Empty;
                methodName = string.Empty;
                review = otspws.Review;

                methodType = Convert.ToInt32(otspws.Method.MethodType, CultureInfo.CurrentUICulture);
                if (Enum.IsDefined(typeof(eMethodTypeUI), methodType))
                {
                    abm = (ApplicationBaseMethod)otspws.Method;
                    methodName = ApplicationUtilities.Adjust_Name(abm.Name, abm.User_RID);
                }

                if (!otspws.UsedSystemTolerancePercent)
                {
                    dTolerancePercent = otspws.TolerancePercent;
                }

                variableNumber = otspws.VariableNumber;

                KeyValuePair<int, string> methodAction = new KeyValuePair<int, string>(methodType, MIDText.GetTextOnly(methodType));

                KeyValuePair<int, string> method = new KeyValuePair<int, string>(otspws.Method.Key, methodName);

                KeyValuePair<int, string> specific = new KeyValuePair<int, string>(specificRID, specificName);

                KeyValuePair<int, string> variable = GetName.GetVariable(variableNumber, SAB);

                rOWorkflowSteps.Add(new ROPlanningWorkflowStep(rowPosition, methodAction, method, specific, review, dTolerancePercent, variable, otspws.Balance, otspws.ComputationMode));

                ++rowPosition;
            }

            return rOWorkflowSteps;

        }
        override protected void SetWorkflowSteps(ROWorkflowPropertiesParms rOWorkflow)
        {
            OTSPlanWorkFlowStep awfs = null;
            ApplicationBaseAction method;

            bool reviewFlag = false;
            bool useSystemTolerancePercent = true;
            double tolerancePercent = Include.UseSystemTolerancePercent;
            int storeFilter = Include.UndefinedStoreFilter;
            int workFlowStepKey = 0;

            tolerancePercent = SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent;

            storeFilter = rOWorkflow.ROWorkflow.Filter.Key;

            foreach (ROPlanningWorkflowStep item in rOWorkflow.ROWorkflow.WorkflowSteps)
            {
                method = GetMethods.GetMethod(item.Method.Key, (eMethodType)item.MethodAction.Key);

                awfs = new OTSPlanWorkFlowStep(method, reviewFlag,
                        useSystemTolerancePercent, tolerancePercent, storeFilter, workFlowStepKey,
                        item.Variable.Key, item.ComputationMode, item.BalanceIndicator);

                _ABW.Workflow_Steps.Add(awfs);

                ++workFlowStepKey;
            }
        }
    }
}
