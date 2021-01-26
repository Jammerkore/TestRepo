using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Collections;

namespace Logility.ROWeb
{
    public class MIDTextRowHandler : DBRowHandler
    {
        private TypedDBColumnHandler<string> _NameColumnHandler;

        private string _sName;

        public MIDTextRowHandler(string sDBIDColName, string sUIIDColName, string sDBNameColName, string sUINameColName)
            : base(sDBIDColName, sUIIDColName, eMIDTextCode.Unassigned)
        {
            _NameColumnHandler = new TypedDBColumnHandler<string>(sDBNameColName, sUINameColName, eMIDTextCode.Unassigned, true, string.Empty);
            _aColumnHandlers = new ColumnHandler[] { _RIDColumnHandler, _NameColumnHandler };
        }

        protected override void ParseDataRow(DataRow dr, bool bIsDBRow)
        {
            base.ParseDataRow(dr, bIsDBRow);

            _sName = _NameColumnHandler.ParseColumn(dr, bIsDBRow);
        }
    }

    public class MIDTextDataHandler
    {
        private MIDTextRowHandler rowHandler;
        private DataTable dtUIText;

        public MIDTextDataHandler(string sUITableName, string sUIIDColName, string sUINameColName)
        {
            rowHandler = new MIDTextRowHandler("TEXT_CODE", sUIIDColName, "TEXT_VALUE", sUINameColName);
            dtUIText = new DataTable(sUITableName);
            rowHandler.AddUITableColumns(dtUIText);
        }

        public DataTable GetUITextTable(eMIDTextType eTextType, eMIDTextOrderBy eOrderBy, params int[] excludedIDs)
        {
            DataTable dtDBText = MIDText.GetTextType(eTextType, eOrderBy, excludedIDs);

            foreach (DataRow drDB in dtDBText.Rows)
            {
                DataRow drUI = dtUIText.NewRow();

                rowHandler.TranslateDBRowToUI(drDB, drUI);
                dtUIText.Rows.Add(drUI);
            }

            return dtUIText;
        }
    }

    public class eNumConverter
    {
        private static Dictionary<int, string> ConvertEnumToDictionary<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Type must be an enum");
            }

            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToDictionary(t =>
                   (int)Convert.ChangeType(t, t.GetType()),
                   t => t.ToString()
                );
        }

        public static DataTable AddEnumsToTable<T>(string sTableName)
        {
            DataTable dt = new DataTable(sTableName);

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("NAME", typeof(string));

            Dictionary<int, string> dictionaryValues = ConvertEnumToDictionary<T>();

            foreach (KeyValuePair<int, string> kvp in dictionaryValues)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = kvp.Key;
                dr["NAME"] = kvp.Value;

                dt.Rows.Add(dr);
            }

            return dt;
        }


    }

    public class WorkflowMethodUtilities
    {
        public static int GetWorkflowMethodFolderRID(eProfileType applicationFolderType, int folderKey, int userKey, eProfileType profileType, string uniqueID)
        {
            int folderRID, userRID, folderType;
            bool folderValid = false;
            DataTable dt;
            FolderDataLayer dlFolder = new FolderDataLayer();


            folderValid = isFolderValid(dlFolder: dlFolder, applicationFolderType: applicationFolderType, folderKey: folderKey, userKey: userKey, profileType: profileType, folderType: out folderType);
            

            // Get application root folder for user
            if (!folderValid)
            {
                if (uniqueID != null)
                {
                    folderRID = GetWorkflowMethodFolderRIDFromUniqueID(dlFolder: dlFolder, applicationFolderType: applicationFolderType, userKey: userKey, profileType: profileType, uniqueID: uniqueID);
                    if (folderRID != Include.NoRID)
                    {
                        return folderRID;
                    }
                }

                if (folderType == eProfileType.WorkflowMethodSubFolder.GetHashCode())
                {
                    folderRID = GetWorkflowMethodFolderRIDFromGrouping(dlFolder: dlFolder, applicationFolderType: applicationFolderType, userKey: userKey, profileType: profileType, folderKey: folderKey);
                    if (folderRID != Include.NoRID)
                    {
                        return folderRID;
                    }
                }

                // override type to actual folders
                if (applicationFolderType == eProfileType.WorkflowMethodAllocationWorkflowsFolder
                    || applicationFolderType == eProfileType.WorkflowMethodAllocationMethodsFolder
                    || isAllocationMethodFolder(applicationFolderType))
                {
                    applicationFolderType = eProfileType.WorkflowMethodAllocationFolder;
                }
                else if (applicationFolderType == eProfileType.WorkflowMethodOTSForcastWorkflowsFolder
                    || applicationFolderType == eProfileType.WorkflowMethodOTSForcastMethodsFolder
                    || isOTSForecastMethodFolder(applicationFolderType))
                {
                    applicationFolderType = eProfileType.WorkflowMethodOTSForcastFolder;
                }

                dt = dlFolder.Folder_Read(userKey, applicationFolderType);
                if (dt != null
                && dt.Rows.Count > 0)
                {
                    folderRID = Convert.ToInt32(dt.Rows[0]["FOLDER_RID"]);
                    userRID = Convert.ToInt32(dt.Rows[0]["USER_RID"]);
                    folderType = Convert.ToInt32(dt.Rows[0]["FOLDER_TYPE"]);

                    folderValid = true;
                    folderKey = folderRID;
                }
                else
                {
                    throw new Exception("Unable to determine folder");
                }
            }

            return folderKey;
        }

        private static bool isFolderValid(FolderDataLayer dlFolder, eProfileType applicationFolderType, int folderKey, int userKey, eProfileType profileType, out int folderType)
        {
            bool folderValid = false;
            int folderRID, userRID;

            folderType = Include.Undefined;

            DataTable dt = dlFolder.Folder_Read(folderKey);
            if (dt != null
                && dt.Rows.Count > 0)
            {
                folderRID = Convert.ToInt32(dt.Rows[0]["FOLDER_RID"]);
                userRID = Convert.ToInt32(dt.Rows[0]["USER_RID"]);
                folderType = Convert.ToInt32(dt.Rows[0]["FOLDER_TYPE"]);

                // Is favorites folder
                if (folderType == eProfileType.WorkflowMethodMainFavoritesFolder.GetHashCode())
                {
                    folderValid = true;
                }
                // Is valid application root folder
                else if (userRID == userKey
                    && folderType == applicationFolderType.GetHashCode())
                {
                    folderValid = true;
                }
                // Is valid subfolder
                else
                {
                    int subfolderType = GetSubFolderType(applicationFolderType, profileType).GetHashCode();
                    if (userRID == userKey
                        && folderType == subfolderType)
                    {
                        folderValid = true;
                    }
                }

            }

            return folderValid;
        }

        private static int GetWorkflowMethodFolderRIDFromUniqueID(FolderDataLayer dlFolder, eProfileType applicationFolderType, int userKey, eProfileType profileType, string uniqueID)
        {
            int folderKey = Include.NoRID;
            int folderType;

            string[] keys = uniqueID.Split('_');

            for (int i = keys.Length - 1; i >= 0; --i )
            {
                folderKey = Convert.ToInt32(keys[i]);
                if (isFolderValid(dlFolder: dlFolder, applicationFolderType: applicationFolderType, folderKey: folderKey, userKey: userKey, profileType: profileType, folderType: out folderType))
                {
                    return folderKey;
                }

                if (folderType == eProfileType.WorkflowMethodSubFolder.GetHashCode())
                {
                    folderKey = GetWorkflowMethodFolderRIDFromGrouping(dlFolder: dlFolder, applicationFolderType: applicationFolderType, userKey: userKey, profileType: profileType, folderKey: folderKey);
                    if (folderKey != Include.NoRID)
                    {
                        return folderKey;
                    }
                }
            }

            return Include.NoRID;
        }

        private static int GetWorkflowMethodFolderRIDFromGrouping(FolderDataLayer dlFolder, eProfileType applicationFolderType, int userKey, eProfileType profileType, int folderKey)
        {
            DataTable dt;
            int userRID, folderType;

            dt = dlFolder.Folder_Children_Read(aUserRID: userKey, aFolderRID: folderKey);
            if (dt != null
            && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    folderKey = Convert.ToInt32(dr["CHILD_ITEM_RID"]);
                    userRID = Convert.ToInt32(dr["USER_RID"]);
                    folderType = Convert.ToInt32(dr["CHILD_ITEM_TYPE"]);
                    if (folderType == eProfileType.WorkflowMethodSubFolder.GetHashCode())
                    {
                        return GetWorkflowMethodFolderRIDFromGrouping(dlFolder: dlFolder, applicationFolderType: applicationFolderType, userKey: userKey, profileType: profileType, folderKey: folderKey);
                    }
                    else if (isFolderValid(dlFolder: dlFolder, applicationFolderType: applicationFolderType, folderKey: folderKey, userKey: userKey, profileType: profileType, folderType: out folderType))
                    {
                        return folderKey;
                    }
                }
            }

            return Include.NoRID;
        }

        private static eProfileType GetSubFolderType(eProfileType applicationFolderType, eProfileType profileType)
        {
            switch (profileType)
            {
                case eProfileType.Workflow:
                    if (applicationFolderType == eProfileType.WorkflowMethodAllocationFolder)
                    {
                        return eProfileType.WorkflowMethodAllocationWorkflowsSubFolder;
                    }
                    else
                    {
                        return eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder;
                    }
                case eProfileType.MethodOTSPlan:
                    return eProfileType.MethodOTSPlanSubFolder;
                case eProfileType.MethodForecastBalance:
                    return eProfileType.MethodForecastBalanceSubFolder;
                case eProfileType.MethodModifySales:
                    return eProfileType.MethodModifySalesSubFolder;
                case eProfileType.MethodForecastSpread:
                    return eProfileType.MethodForecastSpreadSubFolder;
                case eProfileType.MethodCopyChainForecast:
                    return eProfileType.MethodCopyChainForecastSubFolder;
                case eProfileType.MethodCopyStoreForecast:
                    return eProfileType.MethodCopyStoreForecastSubFolder;
                case eProfileType.MethodExport:
                    return eProfileType.MethodExportSubFolder;
                case eProfileType.MethodPlanningExtract:
                    return eProfileType.MethodPlanningExtractSubFolder;
                case eProfileType.MethodGlobalUnlock:
                    return eProfileType.MethodGlobalUnlockSubFolder;
                case eProfileType.MethodGlobalLock:
                    return eProfileType.MethodGlobalLockSubFolder;
                case eProfileType.MethodRollup:
                    return eProfileType.MethodRollupSubFolder;
                case eProfileType.MethodGeneralAllocation:
                    return eProfileType.MethodGeneralAllocationSubFolder;
                case eProfileType.MethodAllocationOverride:
                    return eProfileType.MethodAllocationOverrideSubFolder;
                case eProfileType.MethodRule:
                    return eProfileType.MethodRuleSubFolder;
                case eProfileType.MethodVelocity:
                    return eProfileType.MethodVelocitySubFolder;
                case eProfileType.MethodFillSizeHolesAllocation:
                    return eProfileType.MethodFillSizeHolesSubFolder;
                case eProfileType.MethodBasisSizeAllocation:
                    return eProfileType.MethodBasisSizeSubFolder;
                case eProfileType.MethodSizeCurve:
                    return eProfileType.MethodSizeCurveSubFolder;
                case eProfileType.MethodBuildPacks:
                    return eProfileType.MethodBuildPacksSubFolder;
                case eProfileType.MethodGroupAllocation:
                    return eProfileType.MethodGroupAllocationSubFolder;
                case eProfileType.MethodDCCartonRounding:
                    return eProfileType.MethodDCCartonRoundingSubFolder;
                case eProfileType.MethodCreateMasterHeaders:
                    return eProfileType.MethodCreateMasterHeadersSubFolder;
                case eProfileType.MethodDCFulfillment:
                    return eProfileType.MethodDCFulfillmentSubFolder;
                case eProfileType.MethodSizeNeedAllocation:
                    return eProfileType.MethodSizeNeedSubFolder;
                default:
                    return eProfileType.None;
            }
        }

        public static List<ROTreeNodeOut> BuildMethodNode(eROApplicationType applicationType, ApplicationBaseMethod abm)
        {
            List<ROTreeNodeOut> nodeList = new List<ROTreeNodeOut>();
            string name;

            if (abm.Template_IND)
            {
                name = abm.Name;
            }
            else
            {
                name = Include.Custom;
            }

            nodeList.Add(new ROTreeNodeOut(
                    key: abm.Key,
                    text: name,
                    ownerUserRID: abm.User_RID,
                    treeNodeType: eTreeNodeType.ObjectNode,
                    profileType: abm.ProfileType,
                    isReadOnly: false,
                    canBeDeleted: true,
                    canCreateNewFolder: true,
                    canCreateNewItem: true,
                    canBeProcessed: true,
                    canBeCopied: true,
                    canBeCut: true,
                    hasChildren: false,
                    ROApplicationType: applicationType
                    )
                    );

            return nodeList;
        }

        public static List<ROTreeNodeOut> BuildWorkflowNode(eROApplicationType applicationType, ApplicationBaseWorkFlow abw)
        {
            List<ROTreeNodeOut> nodeList = new List<ROTreeNodeOut>();
            nodeList.Add(new ROTreeNodeOut(key: abw.Key, text: abw.WorkFlowName,
                         ownerUserRID: abw.UserRID,
                         treeNodeType: eTreeNodeType.ObjectNode,
                         profileType: abw.ProfileType,
                         isReadOnly: false,
                         canBeDeleted: true,
                         canCreateNewFolder: true,
                         canCreateNewItem: true,
                         canBeProcessed: true,
                         canBeCopied: true,
                         canBeCut: true,
                         hasChildren: false,
                         ROApplicationType: applicationType));

            return nodeList;
        }

        /// <summary>
		/// Use to lock a workflow or method before allowing updating
		/// </summary>
		/// <param name="aWorkflowMethodRID">The record ID of the workflow or method</param>
		/// <param name="aNode">The node being updated</param>
		public static eLockStatus LockWorkflowMethod(SessionAddressBlock SAB, eWorkflowMethodIND workflowMethodIND, eChangeType aChangeType, int Key, string Name, bool allowReadOnly, out string message)
        {
            eLockStatus lockStatus = eLockStatus.Undefined;
            message = null;
            string userLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_User);
            string methodLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
            string workflowLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow);

            if (workflowMethodIND == eWorkflowMethodIND.Workflows)
            {
                WorkflowEnqueue workflowEnqueue = new WorkflowEnqueue(
                    Key,
                    SAB.ClientServerSession.UserRID,
                    SAB.ClientServerSession.ThreadID);

                try
                {
                    workflowEnqueue.EnqueueWorkflow();
                    lockStatus = eLockStatus.Locked;
                }
                catch (WorkflowConflictException)
                {
                    message = MIDText.GetTextOnly(eMIDTextCode.msg_WorkflowRequested) + ":" + System.Environment.NewLine;
                    foreach (WorkflowConflict WCon in workflowEnqueue.ConflictList)
                    {
                        message += System.Environment.NewLine + workflowLabel + ": " + Name + ", " + userLabel + ": " + WCon.UserName;
                    }
                    message += System.Environment.NewLine + System.Environment.NewLine;
                    if (aChangeType == eChangeType.update &&
                        allowReadOnly)
                    {
                        message += MIDText.GetTextOnly(eMIDTextCode.msg_ReadOnlyMode);

                        lockStatus = eLockStatus.ReadOnly;
                    }
                    else
                        if (aChangeType == eChangeType.delete)
                    {
                        message += MIDText.GetTextOnly(eMIDTextCode.msg_WorkflowCannotBeDeleted);

                        lockStatus = eLockStatus.Cancel;
                    }
                    else
                    {
                        message += MIDText.GetTextOnly(eMIDTextCode.msg_WorkflowCannotBeUpdated);

                        lockStatus = eLockStatus.Cancel;
                    }
                }
            }
            else
            {
                MethodEnqueue methodEnqueue = new MethodEnqueue(
                    Key,
                    SAB.ClientServerSession.UserRID,
                    SAB.ClientServerSession.ThreadID);

                try
                {
                    methodEnqueue.EnqueueMethod();
                    lockStatus = eLockStatus.Locked;
                }
                catch (MethodConflictException)
                {
                    message = MIDText.GetTextOnly(eMIDTextCode.msg_MethodRequested) + ":" + System.Environment.NewLine;
                    foreach (MethodConflict MCon in methodEnqueue.ConflictList)
                    {
                        message += System.Environment.NewLine + methodLabel + ": " + Name + ", " + userLabel + ": " + MCon.UserName;
                    }
                    message += System.Environment.NewLine + System.Environment.NewLine;
                    if (aChangeType == eChangeType.update &&
                        allowReadOnly)
                    {
                        MIDEnvironment.isChangedToReadOnly = true;
                        message += System.Environment.NewLine + MIDText.GetTextOnly(eMIDTextCode.msg_ReadOnlyMode);

                        lockStatus = eLockStatus.ReadOnly;
                    }
                    else
                        if (aChangeType == eChangeType.delete)
                    {
                        message += MIDText.GetTextOnly(eMIDTextCode.msg_MethodCannotBeDeleted);

                        lockStatus = eLockStatus.Cancel;
                    }
                    else
                    {
                        message += MIDText.GetTextOnly(eMIDTextCode.msg_MethodCannotBeUpdated);

                        lockStatus = eLockStatus.Cancel;
                    }
                }
            }

            return lockStatus;
        }

        public static eLockStatus UnlockWorkflowMethod(SessionAddressBlock SAB, eWorkflowMethodIND workflowMethodIND, int Key, out string message)
        {
            eLockStatus lockStatus = eLockStatus.Locked;
            message = null;
            if (lockStatus == eLockStatus.Locked)
            {
                if (workflowMethodIND == eWorkflowMethodIND.Workflows)
                {
                    WorkflowEnqueue workflowEnqueue = new WorkflowEnqueue(
                        Key,
                        SAB.ClientServerSession.UserRID,
                        SAB.ClientServerSession.ThreadID);

                    try
                    {
                        workflowEnqueue.DequeueWorkflow();
                        lockStatus = eLockStatus.Undefined;
                    }
                    catch
                    {
                        throw;
                    }
                }
                else
                {
                    MethodEnqueue methodEnqueue = new MethodEnqueue(
                        Key,
                        SAB.ClientServerSession.UserRID,
                        SAB.ClientServerSession.ThreadID);

                    try
                    {
                        methodEnqueue.DequeueMethod();
                        lockStatus = eLockStatus.Undefined;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return lockStatus;
        }



        public static bool isMethod(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodOTSPlan ||
                profileType == eProfileType.MethodForecastBalance ||
                profileType == eProfileType.MethodModifySales ||
                profileType == eProfileType.MethodForecastSpread ||
                profileType == eProfileType.MethodCopyChainForecast ||
                profileType == eProfileType.MethodCopyStoreForecast ||
                profileType == eProfileType.MethodExport ||
                profileType == eProfileType.MethodPlanningExtract ||
                profileType == eProfileType.MethodGlobalUnlock ||
                profileType == eProfileType.MethodGlobalLock ||
                profileType == eProfileType.MethodRollup ||
                profileType == eProfileType.MethodGeneralAllocation ||
                profileType == eProfileType.MethodAllocationOverride ||
                profileType == eProfileType.MethodRule ||
                profileType == eProfileType.MethodVelocity ||
                profileType == eProfileType.MethodSizeNeedAllocation ||
                profileType == eProfileType.MethodSizeCurve ||
                profileType == eProfileType.MethodBuildPacks ||
                profileType == eProfileType.MethodGroupAllocation ||
                profileType == eProfileType.MethodDCCartonRounding ||
                profileType == eProfileType.MethodCreateMasterHeaders ||
                profileType == eProfileType.MethodDCFulfillment ||
                profileType == eProfileType.MethodFillSizeHolesAllocation ||
                profileType == eProfileType.MethodBasisSizeAllocation ||
                profileType == eProfileType.MethodWarehouseSizeAllocation)
            {
                return true;
            }
            return false;
        }

        public static bool isOTSForecastMethod(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodOTSPlan ||
                profileType == eProfileType.MethodForecastBalance ||
                profileType == eProfileType.MethodModifySales ||
                profileType == eProfileType.MethodForecastSpread ||
                profileType == eProfileType.MethodCopyChainForecast ||
                profileType == eProfileType.MethodCopyStoreForecast ||
                profileType == eProfileType.MethodExport ||
                profileType == eProfileType.MethodPlanningExtract ||
                profileType == eProfileType.MethodGlobalUnlock ||
                profileType == eProfileType.MethodGlobalLock ||
                profileType == eProfileType.MethodRollup)
            {
                return true;
            }
            return false;
        }

        public static bool isOTSForecastWorkflow(eProfileType profileType, eWorkflowType workflowType)
        {
                if (profileType == eProfileType.Workflow &&
                    workflowType == eWorkflowType.Forecast)
                {
                    return true;
                }
                return false;
        }

        public static bool isAllocationMethod(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodGeneralAllocation ||
                profileType == eProfileType.MethodAllocationOverride ||
                profileType == eProfileType.MethodRule ||
                profileType == eProfileType.MethodVelocity ||
                profileType == eProfileType.MethodSizeNeedAllocation ||
                profileType == eProfileType.MethodSizeCurve ||
                profileType == eProfileType.MethodBuildPacks ||
                profileType == eProfileType.MethodGroupAllocation ||
                profileType == eProfileType.MethodDCCartonRounding ||
                profileType == eProfileType.MethodCreateMasterHeaders ||
                profileType == eProfileType.MethodDCFulfillment ||
                profileType == eProfileType.MethodFillSizeHolesAllocation ||
                profileType == eProfileType.MethodBasisSizeAllocation ||
                profileType == eProfileType.MethodWarehouseSizeAllocation)
            {
                return true;
            }
            return false;
        }

        public static bool isAllocationWorkflow(eProfileType profileType, eWorkflowType workflowType)
        {
            if (profileType == eProfileType.Workflow &&
                workflowType == eWorkflowType.Allocation)
            {
                return true;
            }
            return false;
        }

        public static bool isSizeMethod(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodSizeNeedAllocation ||
                profileType == eProfileType.MethodSizeCurve ||
                profileType == eProfileType.MethodFillSizeHolesAllocation ||
                profileType == eProfileType.MethodBasisSizeAllocation ||
                profileType == eProfileType.MethodWarehouseSizeAllocation)
            {
                return true;
            }
            return false;
        }

        public static bool isWorkflow(eProfileType profileType)
        {
            if (profileType == eProfileType.Workflow)
            {
                return true;
            }
            return false;
        }

        public static bool isWorkflowMethod(eProfileType profileType)
        {
            if (isMethod(profileType) || isWorkflow(profileType))
            {
                return true;
            }
            return false;
        }

        public static bool isWorkflowFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.WorkflowMethodAllocationWorkflowsFolder ||
                profileType == eProfileType.WorkflowMethodOTSForcastWorkflowsFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isWorkflowMethodSubFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.WorkflowMethodAllocationWorkflowsSubFolder ||
                profileType == eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder ||
                profileType == eProfileType.MethodOTSPlanSubFolder ||
                profileType == eProfileType.MethodForecastBalanceSubFolder ||
                profileType == eProfileType.MethodModifySalesSubFolder ||
                profileType == eProfileType.MethodForecastSpreadSubFolder ||
                profileType == eProfileType.MethodCopyChainForecastSubFolder ||
                profileType == eProfileType.MethodCopyStoreForecastSubFolder ||
                profileType == eProfileType.MethodExportSubFolder ||
                profileType == eProfileType.MethodPlanningExtractSubFolder ||
                profileType == eProfileType.MethodGlobalUnlockSubFolder ||
                profileType == eProfileType.MethodGlobalLockSubFolder ||
                profileType == eProfileType.MethodRollupSubFolder ||
                profileType == eProfileType.MethodGeneralAllocationSubFolder ||
                profileType == eProfileType.MethodAllocationOverrideSubFolder ||
                profileType == eProfileType.MethodRuleSubFolder ||
                profileType == eProfileType.MethodVelocitySubFolder ||
                profileType == eProfileType.MethodFillSizeHolesSubFolder ||
                profileType == eProfileType.MethodBasisSizeSubFolder ||
                profileType == eProfileType.MethodSizeCurveSubFolder ||
                profileType == eProfileType.MethodBuildPacksSubFolder ||
                profileType == eProfileType.MethodGroupAllocationSubFolder ||
                profileType == eProfileType.MethodDCCartonRoundingSubFolder ||
                profileType == eProfileType.MethodCreateMasterHeadersSubFolder ||
                profileType == eProfileType.MethodDCFulfillmentSubFolder ||
                profileType == eProfileType.MethodSizeNeedSubFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isWorkflowSubFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.WorkflowMethodAllocationWorkflowsSubFolder ||
                profileType == eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isMethodFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodOTSPlanFolder ||
                profileType == eProfileType.MethodForecastBalanceFolder ||
                profileType == eProfileType.MethodModifySalesFolder ||
                profileType == eProfileType.MethodForecastSpreadFolder ||
                profileType == eProfileType.MethodCopyChainForecastFolder ||
                profileType == eProfileType.MethodCopyStoreForecastFolder ||
                profileType == eProfileType.MethodExportFolder ||
                profileType == eProfileType.MethodPlanningExtractFolder ||
                profileType == eProfileType.MethodGlobalUnlockFolder ||
                profileType == eProfileType.MethodGlobalLockFolder ||
                profileType == eProfileType.MethodRollupFolder ||
                profileType == eProfileType.MethodGeneralAllocationFolder ||
                profileType == eProfileType.MethodAllocationOverrideFolder ||
                profileType == eProfileType.MethodRuleFolder ||
                profileType == eProfileType.MethodVelocityFolder ||
                profileType == eProfileType.MethodFillSizeHolesFolder ||
                profileType == eProfileType.MethodBasisSizeFolder ||
                profileType == eProfileType.MethodSizeCurveFolder ||
                profileType == eProfileType.MethodBuildPacksFolder ||
                profileType == eProfileType.MethodGroupAllocationFolder ||
                profileType == eProfileType.MethodDCCartonRoundingFolder ||
                profileType == eProfileType.MethodCreateMasterHeadersFolder ||
                profileType == eProfileType.MethodDCFulfillmentFolder ||
                profileType == eProfileType.MethodSizeNeedFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isMethodsFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.WorkflowMethodOTSForcastMethodsFolder ||
                profileType == eProfileType.WorkflowMethodAllocationMethodsFolder ||
                profileType == eProfileType.WorkflowMethodAllocationSizeMethodsFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isMethodSubFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodOTSPlanSubFolder ||
                profileType == eProfileType.MethodForecastBalanceSubFolder ||
                profileType == eProfileType.MethodModifySalesSubFolder ||
                profileType == eProfileType.MethodForecastSpreadSubFolder ||
                profileType == eProfileType.MethodCopyChainForecastSubFolder ||
                profileType == eProfileType.MethodCopyStoreForecastSubFolder ||
                profileType == eProfileType.MethodExportSubFolder ||
                profileType == eProfileType.MethodPlanningExtractSubFolder ||
                profileType == eProfileType.MethodGlobalUnlockSubFolder ||
                profileType == eProfileType.MethodGlobalLockSubFolder ||
                profileType == eProfileType.MethodRollupSubFolder ||
                profileType == eProfileType.MethodGeneralAllocationSubFolder ||
                profileType == eProfileType.MethodAllocationOverrideSubFolder ||
                profileType == eProfileType.MethodRuleSubFolder ||
                profileType == eProfileType.MethodVelocitySubFolder ||
                profileType == eProfileType.MethodFillSizeHolesSubFolder ||
                profileType == eProfileType.MethodBasisSizeSubFolder ||
                profileType == eProfileType.MethodSizeCurveSubFolder ||
                profileType == eProfileType.MethodBuildPacksSubFolder ||
                profileType == eProfileType.MethodGroupAllocationSubFolder ||
                profileType == eProfileType.MethodDCCartonRoundingSubFolder ||
                profileType == eProfileType.MethodCreateMasterHeadersSubFolder ||
                profileType == eProfileType.MethodDCFulfillmentSubFolder ||
                profileType == eProfileType.MethodSizeNeedSubFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isOTSForecastMethodFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodOTSPlanFolder ||
                profileType == eProfileType.MethodForecastBalanceFolder ||
                profileType == eProfileType.MethodModifySalesFolder ||
                profileType == eProfileType.MethodForecastSpreadFolder ||
                profileType == eProfileType.MethodCopyChainForecastFolder ||
                profileType == eProfileType.MethodCopyStoreForecastFolder ||
                profileType == eProfileType.MethodExportFolder ||
                profileType == eProfileType.MethodPlanningExtractFolder ||
                profileType == eProfileType.MethodGlobalUnlockFolder ||
                profileType == eProfileType.MethodGlobalLockFolder ||
                profileType == eProfileType.MethodRollupFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isOTSForecastMethodSubFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodOTSPlanSubFolder ||
                profileType == eProfileType.MethodForecastBalanceSubFolder ||
                profileType == eProfileType.MethodModifySalesSubFolder ||
                profileType == eProfileType.MethodForecastSpreadSubFolder ||
                profileType == eProfileType.MethodCopyChainForecastSubFolder ||
                profileType == eProfileType.MethodCopyStoreForecastSubFolder ||
                profileType == eProfileType.MethodExportSubFolder ||
                profileType == eProfileType.MethodPlanningExtractSubFolder ||
                profileType == eProfileType.MethodGlobalUnlockSubFolder ||
                profileType == eProfileType.MethodGlobalLockSubFolder ||
                profileType == eProfileType.MethodRollupSubFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isAllocationMethodFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodGeneralAllocationFolder ||
                profileType == eProfileType.MethodAllocationOverrideFolder ||
                profileType == eProfileType.MethodRuleFolder ||
                profileType == eProfileType.MethodVelocityFolder ||
                profileType == eProfileType.MethodFillSizeHolesFolder ||
                profileType == eProfileType.MethodBasisSizeFolder ||
                profileType == eProfileType.MethodSizeCurveFolder ||
                profileType == eProfileType.MethodBuildPacksFolder ||
                profileType == eProfileType.MethodGroupAllocationFolder ||
                profileType == eProfileType.MethodDCCartonRoundingFolder ||
                profileType == eProfileType.MethodCreateMasterHeadersFolder ||
                profileType == eProfileType.MethodDCFulfillmentFolder ||
                profileType == eProfileType.MethodSizeNeedFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isAllocationMethodSubFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodGeneralAllocationSubFolder ||
                profileType == eProfileType.MethodAllocationOverrideSubFolder ||
                profileType == eProfileType.MethodRuleSubFolder ||
                profileType == eProfileType.MethodVelocitySubFolder ||
                profileType == eProfileType.MethodFillSizeHolesSubFolder ||
                profileType == eProfileType.MethodBasisSizeSubFolder ||
                profileType == eProfileType.MethodSizeCurveSubFolder ||
                profileType == eProfileType.MethodBuildPacksSubFolder ||
                profileType == eProfileType.MethodGroupAllocationSubFolder ||
                profileType == eProfileType.MethodDCCartonRoundingSubFolder ||
                profileType == eProfileType.MethodCreateMasterHeadersSubFolder ||
                profileType == eProfileType.MethodDCFulfillmentSubFolder ||
                profileType == eProfileType.MethodSizeNeedSubFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isSizeMethodFolder(eProfileType profileType)
        {

            if (profileType == eProfileType.MethodFillSizeHolesFolder ||
                profileType == eProfileType.MethodBasisSizeFolder ||
                profileType == eProfileType.MethodSizeCurveFolder ||
                profileType == eProfileType.MethodSizeNeedFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isSizeMethodSubFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.MethodFillSizeHolesSubFolder ||
                profileType == eProfileType.MethodBasisSizeSubFolder ||
                profileType == eProfileType.MethodSizeCurveSubFolder ||
                profileType == eProfileType.MethodSizeNeedSubFolder)
            {
                return true;
            }
            return false;
        }

        public static bool isGroupFolder(eProfileType profileType)
        {
            if (profileType == eProfileType.WorkflowMethodAllocationFolder ||
                 profileType == eProfileType.WorkflowMethodOTSForcastFolder)
            {
                return true;
            }
            return false;
        }

        public static eProfileType GetFolderType(eProfileType profileType)
        {
            switch (profileType)
            {
                case eProfileType.WorkflowMethodOTSForcastWorkflowsFolder:
                case eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder:
                    return eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder;
                case eProfileType.MethodOTSPlanFolder:
                case eProfileType.MethodOTSPlanSubFolder:
                    return eProfileType.MethodOTSPlanSubFolder;
                case eProfileType.MethodForecastBalanceFolder:
                case eProfileType.MethodForecastBalanceSubFolder:
                    return eProfileType.MethodForecastBalanceSubFolder;
                case eProfileType.MethodModifySalesFolder:
                case eProfileType.MethodModifySalesSubFolder:
                    return eProfileType.MethodModifySalesSubFolder;
                case eProfileType.MethodForecastSpreadFolder:
                case eProfileType.MethodForecastSpreadSubFolder:
                    return eProfileType.MethodForecastSpreadSubFolder;
                case eProfileType.MethodCopyChainForecastFolder:
                case eProfileType.MethodCopyChainForecastSubFolder:
                    return eProfileType.MethodCopyChainForecastSubFolder;
                case eProfileType.MethodCopyStoreForecastFolder:
                case eProfileType.MethodCopyStoreForecastSubFolder:
                    return eProfileType.MethodCopyStoreForecastSubFolder;
                case eProfileType.MethodExportFolder:
                case eProfileType.MethodExportSubFolder:
                    return eProfileType.MethodExportSubFolder;
                case eProfileType.MethodPlanningExtractFolder:
                case eProfileType.MethodPlanningExtractSubFolder:
                    return eProfileType.MethodPlanningExtractSubFolder;
                case eProfileType.MethodGlobalUnlockFolder:
                case eProfileType.MethodGlobalUnlockSubFolder:
                    return eProfileType.MethodGlobalUnlockSubFolder;
                case eProfileType.MethodGlobalLockFolder:
                case eProfileType.MethodGlobalLockSubFolder:
                    return eProfileType.MethodGlobalLockSubFolder;
                case eProfileType.MethodRollupFolder:
                case eProfileType.MethodRollupSubFolder:
                    return eProfileType.MethodRollupSubFolder;
                case eProfileType.WorkflowMethodAllocationWorkflowsFolder:
                case eProfileType.WorkflowMethodAllocationWorkflowsSubFolder:
                    return eProfileType.WorkflowMethodAllocationWorkflowsSubFolder;
                case eProfileType.MethodGeneralAllocationFolder:
                case eProfileType.MethodGeneralAllocationSubFolder:
                    return eProfileType.MethodGeneralAllocationSubFolder;
                case eProfileType.MethodAllocationOverrideFolder:
                case eProfileType.MethodAllocationOverrideSubFolder:
                    return eProfileType.MethodAllocationOverrideSubFolder;
                case eProfileType.MethodGroupAllocationFolder:
                case eProfileType.MethodGroupAllocationSubFolder:
                    return eProfileType.MethodGroupAllocationSubFolder;
                case eProfileType.MethodDCCartonRoundingFolder:
                case eProfileType.MethodDCCartonRoundingSubFolder:
                    return eProfileType.MethodDCCartonRoundingSubFolder;
                case eProfileType.MethodCreateMasterHeadersFolder:
                case eProfileType.MethodCreateMasterHeadersSubFolder:
                    return eProfileType.MethodCreateMasterHeadersSubFolder;
                case eProfileType.MethodDCFulfillmentFolder:
                case eProfileType.MethodDCFulfillmentSubFolder:
                    return eProfileType.MethodDCFulfillmentSubFolder;

                case eProfileType.MethodRuleFolder:
                case eProfileType.MethodRuleSubFolder:
                    return eProfileType.MethodRuleSubFolder;
                case eProfileType.MethodVelocityFolder:
                case eProfileType.MethodVelocitySubFolder:
                    return eProfileType.MethodVelocitySubFolder;
                case eProfileType.MethodSizeNeedFolder:
                case eProfileType.MethodSizeNeedSubFolder:
                    return eProfileType.MethodSizeNeedSubFolder;
                case eProfileType.MethodSizeCurveFolder:
                case eProfileType.MethodSizeCurveSubFolder:
                    return eProfileType.MethodSizeCurveSubFolder;
                case eProfileType.MethodBuildPacksFolder:
                case eProfileType.MethodBuildPacksSubFolder:
                    return eProfileType.MethodBuildPacksSubFolder;
                case eProfileType.MethodFillSizeHolesFolder:
                case eProfileType.MethodFillSizeHolesSubFolder:
                    return eProfileType.MethodFillSizeHolesSubFolder;
                case eProfileType.MethodBasisSizeFolder:
                case eProfileType.MethodBasisSizeSubFolder:
                    return eProfileType.MethodBasisSizeSubFolder;
                default:
                    return eProfileType.WorkflowMethodSubFolder;
            }
        }
    }

    /// <summary>
    /// Utility methods for task lists
    /// </summary>
    public class TaskListUtilities
    {
        /// <summary>
        /// Get key of the folder where to save the task list
        /// </summary>
        /// <param name="folderKey"></param>
        /// <param name="userKey"></param>
        /// <param name="profileType"></param>
        /// <param name="uniqueID"></param>
        /// <returns></returns>
        public static int GetTaskListFolderRID(
            eProfileType profileType, 
            int folderKey, 
            int userKey, 
            string uniqueID
            )
        {
            int folderRID, userRID, folderType;
            bool folderValid = false;
            DataTable dt;
            FolderDataLayer folderDataLayer = new FolderDataLayer();

            folderValid = isFolderValid(
                folderDataLayer: folderDataLayer,
                profileType: profileType,
                folderKey: folderKey, 
                userKey: userKey, 
                folderType: out folderType
                );


            // Get application root folder for user
            if (!folderValid)
            {
                if (uniqueID != null)
                {
                    folderRID = GetTaskListFolderRIDFromUniqueID(
                        folderDataLayer: folderDataLayer,  
                        userKey: userKey, 
                        profileType: profileType, 
                        uniqueID: uniqueID
                        );
                    if (folderRID != Include.NoRID)
                    {
                        return folderRID;
                    }
                }

                // locate tasklist folder by user
                if (userKey == Include.GlobalUserRID)
                {
                    profileType = eProfileType.TaskListTaskListMainGlobalFolder;
                }
                else if (userKey == Include.SystemUserRID)
                {
                    profileType = eProfileType.TaskListTaskListMainSystemFolder;
                }

                dt = folderDataLayer.Folder_Read(userKey, profileType);
                if (dt != null
                    && dt.Rows.Count > 0)
                {
                    folderRID = Convert.ToInt32(dt.Rows[0]["FOLDER_RID"]);
                    userRID = Convert.ToInt32(dt.Rows[0]["USER_RID"]);
                    folderType = Convert.ToInt32(dt.Rows[0]["FOLDER_TYPE"]);

                    folderValid = true;
                    folderKey = folderRID;
                }
                else
                {
                    throw new Exception("Unable to determine folder");
                }
            }

            return folderKey;
        }

        /// <summary>
        /// Determines if the folder is valid for the item type
        /// </summary>
        /// <param name="folderDataLayer">The data layer for folder database calls</param>
        /// <param name="profileType">The eProfileType of the item</param>
        /// <param name="folderKey">The key of the folder</param>
        /// <param name="userKey">The key of the user</param>
        /// <param name="folderType">The type of the folder</param>
        /// <returns>A flag identifying if the folder is valid for the item</returns>
        private static bool isFolderValid(
            FolderDataLayer folderDataLayer, 
            eProfileType profileType, 
            int folderKey, 
            int userKey, 
            out int folderType
            )
        {
            bool folderValid = false;
            int folderRID, userRID;

            folderType = Include.Undefined;

            DataTable dt = folderDataLayer.Folder_Read(folderKey);
            if (dt != null
                && dt.Rows.Count > 0)
            {
                folderRID = Convert.ToInt32(dt.Rows[0]["FOLDER_RID"]);
                userRID = Convert.ToInt32(dt.Rows[0]["USER_RID"]);
                folderType = Convert.ToInt32(dt.Rows[0]["FOLDER_TYPE"]);

                // Is favorites folder
                if (folderType == eProfileType.TaskListMainFavoritesFolder.GetHashCode())
                {
                    folderValid = true;
                }
                // Is valid subfolder
                else
                {
                    int subfolderType = GetSubFolderType(profileType).GetHashCode();
                    if (userRID == userKey
                        && folderType == subfolderType)
                    {
                        folderValid = true;
                    }
                }

            }

            return folderValid;
        }

        /// <summary>
        /// Get the folder key using the unique ID
        /// </summary>
        /// <param name="folderDataLayer">The data layer for folder database calls</param>
        /// <param name="userKey">The key of the user</param>
        /// <param name="profileType">The eProfileType of the item</param>
        /// <param name="uniqueID">The unique ID of the item</param>
        /// <returns></returns>
        private static int GetTaskListFolderRIDFromUniqueID(
            FolderDataLayer folderDataLayer, 
            int userKey, 
            eProfileType profileType, 
            string uniqueID
            )
        {
            int folderKey = Include.NoRID;
            int folderType;

            string[] keys = uniqueID.Split('_');

            for (int i = keys.Length - 1; i >= 0; --i)
            {
                folderKey = Convert.ToInt32(keys[i]);
                if (isFolderValid(
                    folderDataLayer: folderDataLayer, 
                    folderKey: folderKey, 
                    userKey: userKey, 
                    profileType: profileType, 
                    folderType: out folderType)
                    )
                {
                    return folderKey;
                }
            }

            return Include.NoRID;
        }

        /// <summary>
        /// Get the folder type from the item type
        /// </summary>
        /// <param name="profileType">The eProfileType of the data</param>
        /// <returns></returns>
        private static eProfileType GetSubFolderType(
            eProfileType profileType)
        {
            switch (profileType)
            {
                case eProfileType.TaskList:
                    return eProfileType.TaskListSubFolder;
                case eProfileType.Job:
                    return eProfileType.TaskListSubFolder;
                default:
                    return eProfileType.None;
            }
        }

        /// <summary>
        /// Creates a new task list node
        /// </summary>
        /// <param name="profileType">The eProfileType of the data</param>
        /// <param name="taskListProfile">The profile containing the task list properties</param>
        /// <returns></returns>
        public static List<ROTreeNodeOut> BuildTaskListNode(
            eProfileType profileType, 
            TaskListProfile taskListProfile
            )
        {
            List<ROTreeNodeOut> nodeList = new List<ROTreeNodeOut>();

            nodeList.Add(new ROTreeNodeOut(
                    key: taskListProfile.Key,
                    text: taskListProfile.Name,
                    ownerUserRID: taskListProfile.UserRID,
                    treeNodeType: eTreeNodeType.ObjectNode,
                    profileType: taskListProfile.ProfileType,
                    isReadOnly: false,
                    canBeDeleted: true,
                    canCreateNewFolder: false,
                    canCreateNewItem: false,
                    canBeProcessed: true,
                    canBeCopied: true,
                    canBeCut: true,
                    hasChildren: false
                    )
                    );

            return nodeList;
        }

        /// <summary>
        /// Lock a Task List or Job before allowing updating
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock containing connectivity to the services</param>
        /// <param name="profileType">The eProfileType of the item to lock</param>
        /// <param name="changeType">The eChangeType of the maintenance being performed</param>
        /// <param name="Key">The key of the item to lock</param>
        /// <param name="Name">The name of the item to lock</param>
        /// <param name="allowReadOnly">A flag identifying if the item can be returned read only if a lock cannot be acquired</param>
        /// <param name="message">A message if an error occurs</param>
        /// <returns></returns>
        public static eLockStatus LockItem(
            SessionAddressBlock sessionAddressBlock, 
            eProfileType profileType, 
            eChangeType changeType, 
            int Key, 
            string Name, 
            bool allowReadOnly, 
            out string message
            )
        {
            eLockStatus lockStatus = eLockStatus.Undefined;
            message = null;
            GenericEnqueue objEnqueue;
            string itemType;
            string errMsg;
            eLockType lockType;

            try
            {
                switch (profileType)
                {
                    case eProfileType.TaskList:
                        lockType = eLockType.TaskList;
                        itemType = "Task List";
                        break;
                    case eProfileType.Job:
                        lockType = eLockType.Job;
                        itemType = "Job";
                        break;
                    default:
                        return lockStatus;
                }

                objEnqueue = new GenericEnqueue(
                    aLockType: lockType, 
                    aObjectRID: Key, 
                    aUserRID: sessionAddressBlock.ClientServerSession.UserRID, 
                    aClientThreadID: sessionAddressBlock.ClientServerSession.ThreadID
                    );

                try
                {
                    objEnqueue.EnqueueGeneric();
                    lockStatus = eLockStatus.Locked;
                }
                catch (GenericConflictException)
                {
                    string[] errParms = new string[3];
                    errParms.SetValue(itemType, 0);
                    errParms.SetValue(Name.Trim(), 1);
                    errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
                    errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);

                    lockStatus = eLockStatus.ReadOnly;
                    if (allowReadOnly)
                    {
                        MIDEnvironment.isChangedToReadOnly = true;
                        message = itemType + " returned as read only";
                    }
                    else
                    {
                        message = errMsg;
                    } 
                }

                return lockStatus;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionAddressBlock">The SessionAddressBlock containing connectivity to the services</param>
        /// <param name="profileType">The eProfileType of the item to unlock</param>
        /// <param name="Key">The key of the item to unlock</param>
        /// <param name="message">A message if an error occurs</param>
        /// <returns></returns>
        public static eLockStatus UnLockItem(
            SessionAddressBlock sessionAddressBlock, 
            eProfileType profileType, 
            int Key, 
            out string message
            )
        {
            eLockStatus lockStatus = eLockStatus.Undefined;
            message = null;
            GenericEnqueue objEnqueue;
            string itemType;
            eLockType lockType;

            try
            {
                switch (profileType)
                {
                    case eProfileType.TaskList:
                        lockType = eLockType.TaskList;
                        itemType = "Task List";
                        break;
                    case eProfileType.Job:
                        lockType = eLockType.Job;
                        itemType = "Job";
                        break;
                    default:
                        return lockStatus;
                }

                objEnqueue = new GenericEnqueue(
                    aLockType: lockType,
                    aObjectRID: Key,
                    aUserRID: sessionAddressBlock.ClientServerSession.UserRID,
                    aClientThreadID: sessionAddressBlock.ClientServerSession.ThreadID
                    );

                objEnqueue.DequeueGeneric();
                lockStatus = eLockStatus.Undefined;

                return lockStatus;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a flag identifying if the profile is a task list
        /// </summary>
        /// <param name="profileType">The eProfileType of the item</param>
        /// <returns></returns>
        public static bool isTaskList(
            eProfileType profileType
            )
        {
            if (profileType == eProfileType.TaskList)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns a flag identifying if the profile is a job
        /// </summary>
        /// <param name="profileType">The eProfileType of the item</param>
        /// <returns></returns>
        public static bool isJob(
            eProfileType profileType
            )
        {
            if (profileType == eProfileType.Job)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get folder profile type based on type of the item
        /// </summary>
        /// <param name="profileType">The eProfileType of the item</param>
        /// <returns></returns>
        public static eProfileType GetFolderType(
            eProfileType profileType
            )
        {
            switch (profileType)
            {
                case eProfileType.TaskList:
                    return eProfileType.TaskListSubFolder;
                case eProfileType.Job:
                    return eProfileType.TaskListSubFolder;
                default:
                    return eProfileType.TaskListSubFolder;
            }
        }
    }

    public class InUse
    {
        /// <summary>
        /// This method resolves the display of In Use data.
        /// </summary>
        /// <param name="itemProfileType">The eProfileType of the item</param>
        /// <param name="key">The key of the item</param>
        /// <param name="inQuiry"></param> This value is true if the call is for display.
        public static ROInUse CheckInUse(eProfileType itemProfileType, int key, bool inQuiry)
        {
            var myInfo = new InUseInfo(key, itemProfileType);
            var myfrm = new InUseDialog(myInfo);
            bool display = false;
            bool showDialog = false;
            myfrm.ResolveInUseData(ref display, inQuiry, true, out showDialog);

            string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(itemProfileType);

            if (myfrm.InUseDatatable.Rows.Count > 0)
            {
                inUseTitle += ": " + myfrm.InUseDatatable.Rows[0]["Header1"].ToString();
            }

            ROInUse ROInUse = new ROInUse(allowDelete: myfrm.AllowDelete, title: inUseTitle);

            // Only build list of values if for display
            if (inQuiry)
            {
                foreach (DataRow row in myfrm.InUseHeadings.Rows)
                {
                    string heading = row[0].ToString();
                    ROInUse.ColumnLabels.Add(heading);
                }

                ROInUseEntry inUseEntry;
                foreach (DataRow row in myfrm.InUseDatatable.Rows)
                {
                    inUseEntry = new ROInUseEntry();
                    int i = 0;
                    foreach (DataColumn dataColumn in myfrm.InUseDatatable.Columns)
                    {
                        string fieldValue = row[dataColumn].ToString();
                        inUseEntry.InUseValues.Add(fieldValue);
                        i++;
                        if (i == ROInUse.ColumnLabels.Count)
                        {
                            break;
                        }
                    }
                    ROInUse.InUseList.Add(inUseEntry);
                }
            }

            return ROInUse;
        }


    }

    public class VariableGroupings
    {
        /// <summary>
        /// Builds a list of variable by grouping.
        /// </summary>
        /// <param name="planType">The ePlanType of the request</param>
        /// <param name="variables">The list of RowColProfileHeader objects containing the variables</param>
        /// <param name="groupings">The list of strings containing the group names</param> 
        public static ROVariableGroupings BuildVariableGroupings(ePlanType planType, ArrayList variables, ArrayList groupings)
        {
            ROVariable variable;
            List<ROVariable> groupVariables;
            ROVariableGrouping grouping;
            List<ROVariableGrouping> variableGrouping = new List<ROVariableGrouping>();
            ROVariableGroupings variableGroupings = new ROVariableGroupings(variableGrouping);
            SortedList<int, ROVariable> selectedList = new SortedList<int, ROVariable>();

            UpdateSelectableList(planType, variables);

            foreach (string groupName in groupings)
            {
                groupVariables = new List<ROVariable>();
                foreach (RowColProfileHeader vp in variables)
                {
                    if (vp.Grouping == groupName)
                    {
                        variable = new ROVariable(vp.Profile.Key, vp.Name, vp.IsSelectable, vp.IsDisplayed, vp.Sequence);
                        groupVariables.Add(variable);
                        if (vp.IsDisplayed
                            && vp.Sequence != Include.Undefined)
                        {
                            selectedList.Add(vp.Sequence, variable);
                        }
                    }
                }
                grouping = new ROVariableGrouping(name: groupName, variables: groupVariables);
                variableGroupings.VariableGrouping.Add(grouping);
            }

            foreach (KeyValuePair<int, ROVariable> selectedVariable in selectedList)
            {
                variableGroupings.SelectedVariables.Add(new ROSelectedField(fieldkey: selectedVariable.Value.Number.ToString(),
                    field: selectedVariable.Value.Name,
                    selected: selectedVariable.Value.IsDisplayed)
                    );
            }

            return variableGroupings;
        }

        private static void UpdateSelectableList(ePlanType aPlanType, ArrayList aSelectableList)
        {
            try
            {
                switch (aPlanType)
                {
                    case ePlanType.Chain:

                        foreach (RowColProfileHeader rowHeader in aSelectableList)
                        {
                            if (((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Both && ((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Chain)
                            {
                                rowHeader.IsSelectable = false;
                            }
                            else
                            {
                                rowHeader.IsSelectable = true;
                            }

                            if (rowHeader.Profile.GetType() == typeof(VariableProfile))
                            {
                                rowHeader.Grouping = ((VariableProfile)rowHeader.Profile).Groupings;
                            }
                        }

                        break;

                    case ePlanType.Store:

                        foreach (RowColProfileHeader rowHeader in aSelectableList)
                        {
                            if (((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Both && ((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Store)
                            {
                                rowHeader.IsSelectable = false;
                            }
                            else
                            {
                                rowHeader.IsSelectable = true;
                            }
                            if (rowHeader.Profile.GetType() == typeof(VariableProfile))
                            {
                                rowHeader.Grouping = ((VariableProfile)rowHeader.Profile).Groupings;
                            }
                        }

                        break;
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }


    }

    public class WorklistUtilities
    {
        /// <summary>
        /// Gets a unique name for the folder.
        /// </summary>
        /// <param name="aFolderName">The original name of the folder</param>
        /// <param name="aUserRID">The owner user</param>
        /// <param name="aParentRID">The key of the parent of the folder</param> 
        /// <param name="aItemType"></param>
        public static string FindNewFolderName(string aFolderName, int aUserRID, int aParentRID, eProfileType aItemType)
        {
            int index;
            string newName;
            int key;
            FolderDataLayer dlFolder;

            try
            {
                dlFolder = new FolderDataLayer();
                index = 1;
                newName = aFolderName;
                key = dlFolder.Folder_GetKey(aUserRID, newName, aParentRID, aItemType);

                while (key != -1)
                {
                    index++;
                    newName = aFolderName + " (" + index + ")";
                    key = dlFolder.Folder_GetKey(aUserRID, newName, aParentRID, aItemType);
                }

                return newName;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


    }

}
