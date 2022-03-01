using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;


namespace Logility.ROWeb
{
    public partial class ApplicationCommon : ROWebFunction
    {
        private int _currentLowLevelNode = Include.NoRID;
        private int _longestBranch = Include.NoRID;
        private int _longestHighestGuest = Include.NoRID;
        private SecurityAdmin _secAdmin;
        private DataTable _dtUser;
        private MIDEnqueue _midNQ;

        private const string ROOVERRIDELOWLEVELNAME = " (Custom)";

        // <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ApplicationCommon(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {

        }
        override public void CleanUp()
        {

        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                case eRORequest.StoreAttributeList:
                    return GetAttributeList();
                case eRORequest.PlanningVersionsList:
                    return GetPlanningVersionsData((ROPlanningVersionsParms)Parms);
                case eRORequest.GetStoreAttributeSet:
                    return GetStoreAttributeSets(roKeyParm: (ROKeyParms)Parms);
                case eRORequest.GetSizeGroupDetails:
                    return GetSizeGroupDetails((ROKeyParms)Parms);
                case eRORequest.GetSizeGroupList:
                    return GetSizeGroupList();
                case eRORequest.GetSizeCurveGroupList:
                    return GetSizeCurveGroupList();
                case eRORequest.GetOtsPlanningOverrideLowLevelList:
                    return GetOtsPlanningOverrideLowLevelList(roKeyParm: (ROLowLevelModelParms)Parms);
                case eRORequest.GetLowLevelList:
                    return GetLowLevelList((ROKeyParms)Parms);
                case eRORequest.GetInUse:
                    return GetInUse((ROInUseParms)Parms);
                case eRORequest.GetFunctionSecurity:
                    return GetFunctionSecurity((ROListParms)Parms);
                case eRORequest.ReleaseResources:
                    return ReleaseResource((ROReleaseResourceParms)Parms);
                case eRORequest.GetAbout:
                    return GetAbout((RONoParms)Parms);
                case eRORequest.GetColors:
                    return GetColors((RONoParms)Parms);
                case eRORequest.GetVendorList:
                    return GetVendorList();
                case eRORequest.ClearCache:
                    return ClearCache(Parms);
                    
            }


            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }


        internal ROOut GetAttributeList()
        {

            ProfileList sgpl;
            StoreGroupListViewProfile sgp;
            FunctionSecurityProfile userAttrSecLvl;

            List<KeyValuePair<int, string>> lstAttributeValues = new List<KeyValuePair<int, string>>();

            try
            {
                userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
                sgpl = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MyUserAndGlobal, !userAttrSecLvl.AccessDenied);
                sgp = (StoreGroupListViewProfile)sgpl[0];
                userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);

                lstAttributeValues = BuildStoreAttributeOut(sgpl.ArrayList);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }

            ROIntStringPairListOut ROKeyValuePairOut = new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, lstAttributeValues);

            return ROKeyValuePairOut;

        }

        internal List<KeyValuePair<int, string>> BuildStoreAttributeOut(ArrayList arrayListIn)
        {
            List<KeyValuePair<int, string>> attributeValues = new List<KeyValuePair<int, string>>();

            for (int i = 0; i < arrayListIn.Count; i++)
            {
                int attributeId = ((Profile)arrayListIn[i]).Key;
                string attributeName = ((StoreGroupProfile)arrayListIn[i]).Name;
                attributeValues.Add(new KeyValuePair<int, string>(attributeId, attributeName));
            }
            return attributeValues;
        }


        #region Method to get the Planning versions
        internal ROOut GetPlanningVersionsData(ROPlanningVersionsParms planningVersionsParms)
        {
            ProfileList versionProfileList = null;
            eSecuritySelectType securitySelectType = eSecuritySelectType.None;

            List<KeyValuePair<int, string>> lstPlanningVersionKeyValues = new List<KeyValuePair<int, string>>();

            try
            {
                eSecurityTypes securityType = (planningVersionsParms.eROVersionsType == eROVersionsType.Chain) ? eSecurityTypes.Chain : eSecurityTypes.Store;

                if (planningVersionsParms.eROVersionsType == eROVersionsType.Chain || planningVersionsParms.eROVersionsType == eROVersionsType.Store)
                {
                    if (planningVersionsParms.MinimumSecurity == eSecuritySelectType.View)
                    {
                        securitySelectType = eSecuritySelectType.View;
                    }
                    else if (planningVersionsParms.MinimumSecurity == eSecuritySelectType.Update)
                    {
                        securitySelectType = eSecuritySelectType.View | eSecuritySelectType.Update;
                    }

                    versionProfileList = SAB.ClientServerSession.GetUserForecastVersions(securitySelectType, securityType);
                }
                else
                {
                    versionProfileList = SAB.ClientServerSession.GetUserForecastVersions();
                }
                
                lstPlanningVersionKeyValues = BuildPlanningVersionListOut(versionProfileList.ArrayList);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }

            ROIntStringPairListOut ROKeyValuePairOut = new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, lstPlanningVersionKeyValues);

            return ROKeyValuePairOut;

        }

        internal List<KeyValuePair<int, string>> BuildPlanningVersionListOut(ArrayList arrayListIn)
        {
            List<KeyValuePair<int, string>> planningVersionList = new List<KeyValuePair<int, string>>();

            for (int i = 0; i < arrayListIn.Count; i++)
            {
                int fv_rid = ((Profile)arrayListIn[i]).Key;
                string sPlanningVersion = ((VersionProfile)arrayListIn[i]).Description;
                planningVersionList.Add(new KeyValuePair<int, string>(fv_rid, sPlanningVersion));
            }

            return planningVersionList;
        }
        #endregion

        #region "Method to get the store attribute set"
        internal ROOut GetStoreAttributeSets(ROKeyParms roKeyParm)
        {
            List<KeyValuePair<int, string>> attrSetsList = BuildStoreAttributeSets(roKeyParm);

            ROIntStringPairListOut ROKeyValuePairOut = new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, attrSetsList);

            return ROKeyValuePairOut;

        }

        internal List<KeyValuePair<int, string>> BuildStoreAttributeSets(ROKeyParms roKeyParms)
        {
            int grpid = roKeyParms.Key;
            ApplicationSessionTransaction _transaction = null;
            List<KeyValuePair<int, string>> storeAttrSetsList = new List<KeyValuePair<int, string>>();
            ProfileList _storeGroupListViewProfileList = null;

            try
            {
                _transaction = SAB.ApplicationServerSession.CreateTransaction();
                PlanCubeGroup _planCubeGroup = _transaction.CreateStorePlanMaintCubeGroup();

                ProfileList attrSetList = new ProfileList(eProfileType.StoreGroupLevel);

                _storeGroupListViewProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupListView);

                ((StorePlanMaintCubeGroup)_planCubeGroup).SetStoreGroup(new StoreGroupProfile((
                    (StoreGroupListViewProfile)_storeGroupListViewProfileList.FindKey((int)grpid)).Key));

                ProfileList _storeGroupLevelProfileList = _planCubeGroup.GetFilteredProfileList(eProfileType.StoreGroupLevel);

                for (int i = 0; i < _storeGroupLevelProfileList.ArrayList.Count; i++)
                {
                    int attrsetKey = ((Profile)_storeGroupLevelProfileList.ArrayList[i]).Key;
                    string attrsetName = ((StoreGroupLevelProfile)_storeGroupLevelProfileList.ArrayList[i]).Name;
                    storeAttrSetsList.Add(new KeyValuePair<int, string>(attrsetKey, attrsetName));
                }

                return storeAttrSetsList;
            }
            catch
            {
                throw;
            }
            finally
            {
                _transaction.Dispose();
            }
        }


        #endregion

        #region "Get Size Group Details"

        internal ROSizeGroupProfileOut GetSizeGroupDetails(ROKeyParms sizeGroupID)
        {
            SizeGroupProfile sizeGroupProfile = null;
            sizeGroupProfile = new SizeGroupProfile(sizeGroupID.Key);


            //Get the list of SizeCodeProfile out from SizeCodeList's Arraylist
            List<SizeCodeProfile> scp = ExtractProfileFromArraylist(sizeGroupProfile.SizeCodeList.ArrayList);

            ROSizeGroupProfileOut sgpOut = new ROSizeGroupProfileOut(eROReturnCode.Successful, null, ROInstanceID,
                sizeGroupID.Key, sizeGroupProfile.SizeGroupName, sizeGroupProfile.SizeGroupDescription, scp);

            return sgpOut;

        }

        internal List<SizeCodeProfile> ExtractProfileFromArraylist(ArrayList sizeCodeList)
        {
            List<SizeCodeProfile> codeProfiles = new List<SizeCodeProfile>();
            foreach (SizeCodeProfile profile in sizeCodeList)
            {
                codeProfiles.Add(profile);
            }

            return codeProfiles;
        }

        #endregion

        #region Get Size groups

        internal ROOut GetSizeGroupList()
        {
            try
            {
                SizeGroupList sizeGroupList = null;
                sizeGroupList = new SizeGroupList(eProfileType.SizeGroup);
                sizeGroupList.LoadAll(false);

                List<KeyValuePair<int, string>> sizeGroupKeyValueList = BuildSizeGroupList(sizeGroupList);

                ROIntStringPairListOut ROKeyValuePairOut = new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, sizeGroupKeyValueList);

                return ROKeyValuePairOut;
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetSizeGroupList failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }

        }
        private List<KeyValuePair<int, string>> BuildSizeGroupList(SizeGroupList sizeGroupList)
        {
            List<KeyValuePair<int, string>> sizeGroupKeyValueList = new List<KeyValuePair<int, string>>();

            for (int intIndex = 0; intIndex < sizeGroupList.ArrayList.Count; intIndex++)
            {
                int sizeGroupKey = sizeGroupList[intIndex].Key;
                string sizeGroupName = ((MIDRetail.Business.SizeGroupProfile)sizeGroupList[intIndex]).SizeGroupName.Trim();

                sizeGroupKeyValueList.Add(new KeyValuePair<int, string>(sizeGroupKey, sizeGroupName));
            }

            return sizeGroupKeyValueList;
        }
        #endregion

        #region Get Size Curve Group List
        internal ROOut GetSizeCurveGroupList()
        {
            ROIntStringPairListOut ROKeyValuePairOut;
            try
            {
                ROKeyValuePairOut = new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, BuildSizeCurveGroupData());
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetSizeCurveGroupData failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }

            return ROKeyValuePairOut;
        }

        internal List<KeyValuePair<int, string>> BuildSizeCurveGroupData()
        {
            DataTable dtSizeCurveGroups = new DataTable();
            SizeCurve dlSizeCurve;
            List<KeyValuePair<int, string>> lstSizeCurveGroupsComboData = new List<KeyValuePair<int, string>>();

            dlSizeCurve = new SizeCurve();
            dtSizeCurveGroups = dlSizeCurve.GetSizeCurveGroups();
            lstSizeCurveGroupsComboData = DataTableTools.DataTableToKeyValues(dtSizeCurveGroups, "SIZE_CURVE_GROUP_RID", "SIZE_CURVE_GROUP_NAME");

            return lstSizeCurveGroupsComboData;
        }

        #endregion

        #region "Method to get the override low level list"
        internal ROOut GetOtsPlanningOverrideLowLevelList(ROLowLevelModelParms roKeyParm)
        {
            try
            {
                List<KeyValuePair<int, string>> overrideLowLevelList = BuildOverrideLowLevelList(roKeyParm);

                ROIntStringPairListOut ROKeyValuePairOut = new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, overrideLowLevelList);

                return ROKeyValuePairOut;
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetOtsPlanningOverrideLowLevelList failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }

        }

        internal List<KeyValuePair<int, string>> BuildOverrideLowLevelList(ROLowLevelModelParms roKeyParms)
        {

            int OverrideLowLevelRid = roKeyParms.ModelKey;
            int CustomOverrideLowLevelRid = roKeyParms.CustomModelKey;

            List<KeyValuePair<int, string>> outOverrideLowLevelList = new List<KeyValuePair<int, string>>();

            FunctionSecurityProfile userSecurity;
            FunctionSecurityProfile globalSecurity;
            ProfileList overrideLowLevelList;
            string overrideLowLevelName;

            try
            {
                userSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsUserOverrideLowLevels);
                globalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsGlobalOverrideLowLevels);

                overrideLowLevelList = OverrideLowLevelProfile.LoadAllProfiles(OverrideLowLevelRid, SAB.ClientServerSession.UserRID, globalSecurity.AllowView, userSecurity.AllowView, CustomOverrideLowLevelRid);

                foreach (OverrideLowLevelProfile ollp in overrideLowLevelList)
                {
                    overrideLowLevelName = ollp.Name;

                    switch (ollp.User_RID)
                    {
                        case Include.GlobalUserRID:
                            break;

                        case Include.CustomUserRID:
                            overrideLowLevelName = overrideLowLevelName + ROOVERRIDELOWLEVELNAME;
                            break;

                        default:
                            overrideLowLevelName = overrideLowLevelName + " (" + UserNameStorage.GetUserName(ollp.User_RID) + ")";
                            break;
                    }

                    outOverrideLowLevelList.Add(new KeyValuePair<int, string>(ollp.Key, overrideLowLevelName));
                }

                return outOverrideLowLevelList;
            }
            catch
            {
                throw;
            }
        }
        // Get List of Vendors for Build Packs Method
        internal ROOut GetVendorList()
        {
            List<KeyValuePair<int, string>> bpcNames = new List<KeyValuePair<int, string>>();
            ROIntStringPairListOut ROKeyValuePairOut = null;
            BuildPacksMethodData data = new BuildPacksMethodData();
            DataTable dtBPCNames = new DataTable();
            dtBPCNames = data.GetBPCList();

            //Sort Table and List by Name
            DataTable sorteddtBPCNames = new DataTable();

            if (dtBPCNames.Rows.Count > 0)
            {
                dtBPCNames.DefaultView.Sort = "BPC_NAME";
                sorteddtBPCNames = dtBPCNames.DefaultView.ToTable();
            }

            try
            {
                foreach (DataRow row in sorteddtBPCNames.Rows)
                {
                    bpcNames.Add(new KeyValuePair<int, string>(Convert.ToInt32(row["BPC_RID"]), Convert.ToString(row["BPC_NAME"])));
                }

                ROKeyValuePairOut = new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, bpcNames);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ROKeyValuePairOut;
        }
        #endregion

        #region "Method to get the Low Level List"
        internal ROLowLevelsOut GetLowLevelList(ROKeyParms roKeyParm)
        {
            List<LowLevelCombo> llc = BuildLowLevleList(roKeyParm);

            ROLowLevelsOut rLlcOut = new ROLowLevelsOut(eROReturnCode.Successful, null, ROInstanceID, llc, roKeyParm.Key);

            return rLlcOut;

        }

        internal List<LowLevelCombo> BuildLowLevleList(ROKeyParms roKeyParm)
        {
            List<LowLevelCombo> lowLevelComboLst = new List<LowLevelCombo>();
            LowLevelCombo lowLevelCombo = null;
            HierarchyNodeProfile aHierarchyNodeProfile = null;
            aHierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(roKeyParm.Key);
            try
            {
                HierarchyProfile hierProf;
                if (aHierarchyNodeProfile != null)
                {
                    hierProf = SAB.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HierarchyRID);
                    if (hierProf.HierarchyType == eHierarchyType.organizational)
                    {
                        for (int i = aHierarchyNodeProfile.HomeHierarchyLevel + 1; i <= hierProf.HierarchyLevels.Count; i++)
                        {
                            HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
                            lowLevelCombo = new LowLevelCombo(eLowLevelsType.HierarchyLevel, i - aHierarchyNodeProfile.HomeHierarchyLevel, hlp.Key, hlp.LevelID);
                            lowLevelComboLst.Add(lowLevelCombo);
                        }
                    }
                    else
                    {
                        HierarchyProfile mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();

                        if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
                        {
                            _longestHighestGuest = SAB.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);
                        }
                        int highestGuestLevel = _longestHighestGuest;

                        if ((highestGuestLevel != int.MaxValue) && (aHierarchyNodeProfile.HomeHierarchyType != eHierarchyType.alternate))
                        {
                            for (int i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
                            {
                                if (i == 0)
                                {
                                    lowLevelCombo = new LowLevelCombo(eLowLevelsType.HierarchyLevel, 0, 0, mainHierProf.HierarchyID);
                                    lowLevelComboLst.Add(lowLevelCombo);
                                }
                                else
                                {
                                    HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];

                                    lowLevelCombo = new LowLevelCombo(eLowLevelsType.HierarchyLevel, i, hlp.Key, hlp.LevelID);
                                    lowLevelComboLst.Add(lowLevelCombo);
                                }
                            }
                        }

                        if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
                        {
                            DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                            _longestBranch = hierarchyLevels.Rows.Count - 1;
                        }
                        int longestBranchCount = _longestBranch;
                        int offset = 0;
                        for (int i = 0; i < longestBranchCount; i++)
                        {
                            ++offset;
                            lowLevelCombo = new LowLevelCombo(eLowLevelsType.LevelOffset, offset, 0, null);
                            lowLevelComboLst.Add(lowLevelCombo);
                        }
                    }
                    _currentLowLevelNode = aHierarchyNodeProfile.Key;
                }
                return lowLevelComboLst;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }

        }
        #endregion 

        private ROOut GetInUse(ROInUseParms parms)
        {
            ROInUse ROInUse = InUse.CheckInUse(itemProfileType: parms.ItemProfileType, key: parms.Key, inQuiry: true);
           return new ROInUseOut(ROReturnCode: eROReturnCode.Successful, sROMessage: null, ROInstanceID: ROInstanceID, ROInUse: ROInUse);
        }

        #region Method get the Function Security Details
        private ROOut GetFunctionSecurity(ROListParms securityFunctions)
        {
            Dictionary<eSecurityFunctions, ROSecurityProfile> functionSecurity = new Dictionary<eSecurityFunctions, ROSecurityProfile>();
            ROSecurityProfile securityProfile;
            foreach (eSecurityFunctions securityFunction in securityFunctions.ListValues)
            {
                FunctionSecurityProfile security = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(securityFunction);
                securityProfile = new ROSecurityProfile(fullControll: security.AllowFullControl, accessdenied: security.AccessDenied);
                foreach (eSecurityActions action in Enum.GetValues(typeof(eSecurityActions)))
                {
                    securityProfile.ROActions.Add(action, security.GetSecurityLevel(action));
                }

                functionSecurity.Add(securityFunction, securityProfile);
            }
            return new ROFunctionSecurityOut(ROReturnCode: eROReturnCode.Successful, sROMessage: null, ROInstanceID: ROInstanceID, ROFunctionSecurity: functionSecurity);
        }
        #endregion  

        #region "Method to Release Resources - 3395"

        private ROOut ReleaseResource(ROReleaseResourceParms parms)
        {
            bool bIsReleased = false;
            _secAdmin = new SecurityAdmin();
            _midNQ = new MIDEnqueue();

            try
            {

                _midNQ.OpenUpdateConnection();
                int userKey = Include.Undefined;

                if (parms.AllUsers)
                {
                    _dtUser = _secAdmin.GetUsers();
                    if (_dtUser.Rows.Count != 0)
                    {
                        foreach (DataRow userRow in _dtUser.Rows)
                        {
                            bIsReleased = ReleaseUser(Convert.ToInt32(userRow["USER_RID"], CultureInfo.CurrentUICulture));
                        }
                    }
                }
                else if (parms.UserKey == Include.Undefined)
                {
                    bIsReleased = ReleaseUser(SAB.ClientServerSession.UserRID);
                    userKey = SAB.ClientServerSession.UserRID;
                }
                else
                {
                    bIsReleased = ReleaseUser(Convert.ToInt32(parms.UserKey, CultureInfo.CurrentUICulture));
                    userKey = parms.UserKey;
                }

                _midNQ.CommitData();

                string sessionUserName = Convert.ToString(SAB.ClientServerSession.GetUserName(SAB.ClientServerSession.UserRID), CultureInfo.CurrentUICulture);
                string resourceUserName = Convert.ToString(SAB.ClientServerSession.GetUserName(userKey));
                string auditMessage = MIDText.GetText(eMIDTextCode.msg_ResourcesReleased);
                auditMessage = auditMessage.Replace("{0}", sessionUserName);
                auditMessage = auditMessage.Replace("{1}", resourceUserName);
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, auditMessage, "Release Resources", true);

            }
            catch (Exception e)
            {
                _midNQ.Rollback();
            }
            finally
            {
                _midNQ.CloseUpdateConnection();
            }

            return new ROBoolOut(eROReturnCode.Successful, null, ROInstanceID, bIsReleased);
        }

        internal bool ReleaseUser(int aUserRID)
        {
            bool bIsReleased = false;

            try
            {
                _midNQ.ChainWeekEnqueue_Delete(aUserRID);
                _midNQ.StoreWeekEnqueue_Delete(aUserRID);
                _midNQ.Enqueue_Delete(eLockType.Header, aUserRID);
                _midNQ.Enqueue_DeleteAll(aUserRID);
                bIsReleased = true;
            }
            catch (Exception ex)
            {
                bIsReleased = false;
            }

            return bIsReleased;
        }

        #endregion

        private ROOut GetAbout(RONoParms parms)
        {
            ROAboutProperties aboutProperties = new ROAboutProperties();
            aboutProperties.CurrentUser = SAB.ClientServerSession.GetUserName(SAB.ClientServerSession.UserRID);
            aboutProperties.Environment = SAB.ControlServerSession.GetMIDEnvironment();
            aboutProperties.ProductName = EnvironmentInfo.MIDInfo.productName;
            aboutProperties.ProductVersion = EnvironmentInfo.MIDInfo.productVersion;
            aboutProperties.EmailSupport = EnvironmentInfo.MIDInfo.emailSupport;
            aboutProperties.AfterHoursPhone = EnvironmentInfo.MIDInfo.afterHoursPhone;
            aboutProperties.LastUpdateDateTime = EnvironmentInfo.MIDInfo.lastUpdateDateTime;
            aboutProperties.SessionEnviroment = EnvironmentBusinessInfo.GetSessionEnviroment(SAB);
            aboutProperties.OpertatingSystem = EnvironmentInfo.MIDInfo.opertatingSystem;
            aboutProperties.OpertatingSystemVersion = EnvironmentInfo.OSInfo.VersionString;
            aboutProperties.OpertatingSystemEdition = EnvironmentInfo.OSInfo.Edition;
            aboutProperties.OpertatingSystemServicePack = EnvironmentInfo.OSInfo.ServicePack;
            aboutProperties.LegalCopyright = EnvironmentInfo.MIDInfo.legalCopyright;
            AddConfigurationInformation(aboutProperties);
            AddAddOnInformation(aboutProperties);

            return new ROAboutOut(eROReturnCode.Successful, null, ROInstanceID, aboutProperties);
        }

        private void AddConfigurationInformation(ROAboutProperties aboutProperties)
        {
            aboutProperties.Configurations.Add("Database=" + EnvironmentBusinessInfo.GetClientDatbaseName(SAB) + "; Application Version=" + EnvironmentInfo.MIDInfo.applicationVersion);

            aboutProperties.Configurations.Add(EnvironmentBusinessInfo.GetControlServerSessionInfo(SAB));
            aboutProperties.Configurations.Add(EnvironmentBusinessInfo.GetApplicationServerSessionInfo(SAB));
            aboutProperties.Configurations.Add(EnvironmentBusinessInfo.GetHierarchyServerSessionInfo(SAB));
            aboutProperties.Configurations.Add(EnvironmentBusinessInfo.GetSchedulerServerSessionInfo(SAB));
            aboutProperties.Configurations.Add(EnvironmentBusinessInfo.GetStoreServerSessionInfo(SAB));
        }

        private void AddAddOnInformation(ROAboutProperties aboutProperties)
        {
            aboutProperties.AddOns.Add(EnvironmentBusinessInfo.GetAllocationInstalledInfo(SAB));
            aboutProperties.AddOns.Add(EnvironmentBusinessInfo.GetSizeInstalledInfo(SAB));
            aboutProperties.AddOns.Add(EnvironmentBusinessInfo.GetPlanningInstalledInfo(SAB));
            aboutProperties.AddOns.Add(EnvironmentBusinessInfo.GetAssortmentInstalledInfo(SAB));
            aboutProperties.AddOns.Add(EnvironmentBusinessInfo.GetGroupAllocationInstalledInfo(SAB));
            aboutProperties.AddOns.Add(EnvironmentBusinessInfo.GetAnalyticsInstalledInfo(SAB));
        }

        private ROIListOut GetColors(RONoParms parms)
        {
            ColorData data = new ColorData();
            DataTable dtCcolorGroups = null;
            dtCcolorGroups = data.GetColorsForGroup(null);

            DataView dv = new DataView(dtCcolorGroups);
            dv.Sort = "COLOR_CODE_GROUP ASC, COLOR_CODE_ID ASC";

            int colorCodeRID;
            string colorCodeID, colorCodeName, colorCodeGroupName;
            bool virtualInd;
            ePurpose purpose;

            List<ROColorGroup> colorGroupList = new List<ROColorGroup>();
            ROColorGroup colorGroup = null;

            foreach (DataRowView dr in dv)
            {
                colorCodeRID = Convert.ToInt32(dr["COLOR_CODE_RID"]);
                colorCodeID = dr["COLOR_CODE_ID"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["COLOR_CODE_ID"], CultureInfo.CurrentUICulture);
                // protect against blank IDs
                if (colorCodeID.Trim().Length == 0)
                {
                    continue;
                }
                colorCodeName = Convert.ToString(dr["COLOR_CODE_NAME"], CultureInfo.CurrentUICulture);
                if (colorCodeName.Trim().Length == 0)
                {
                    colorCodeName = colorCodeID;
                }
                colorCodeGroupName = dr["COLOR_CODE_GROUP"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["COLOR_CODE_GROUP"], CultureInfo.CurrentUICulture);
                if (colorCodeGroupName.Trim().Length == 0)
                {
                    colorCodeGroupName = MIDText.GetTextOnly(eMIDTextCode.Unassigned);
                }
                virtualInd = dr["VIRTUAL_IND"] == System.DBNull.Value ? false : Include.ConvertCharToBool(Convert.ToChar(dr["VIRTUAL_IND"], CultureInfo.CurrentUICulture));
                purpose = dr["PURPOSE"] == System.DBNull.Value ? ePurpose.Default : (ePurpose)(Convert.ToInt32(dr["PURPOSE"]));

                if (colorGroup == null)
                {
                    colorGroup = new ROColorGroup(colorGroupName: colorCodeGroupName);
                }
                else if (colorCodeGroupName != colorGroup.ColorGroupName)
                {
                    colorGroupList.Add(colorGroup);
                    colorGroup = new ROColorGroup(colorGroupName: colorCodeGroupName);
                }

                if (!virtualInd)
                {
                    colorGroup.Colors.Add(new ROColor(key: colorCodeRID, colorID: colorCodeID, colorName: colorCodeName, colorGroupName: colorCodeGroupName));
                }
            }

            colorGroupList.Add(colorGroup);

            return new ROIListOut(eROReturnCode.Successful, null, parms.ROInstanceID, colorGroupList);
        }


        #region "Method to Clear Cache - RO Refresh Data"
        private ROBoolOut ClearCache(ROParms Parms)
        {
            try
            {

           
            SAB.ClientServerSession.Refresh();
            SAB.ApplicationServerSession.Refresh();
            SAB.HierarchyServerSession.Refresh();
            SAB.StoreServerSession.Refresh();
            SAB.HeaderServerSession.Refresh();

            StoreMgmt.LoadInitialStoresAndGroups(SAB, SAB.ClientServerSession, false, true);  // TT#1876-MD - JSmith - Store Load shows zero stores in sets after Tools>Refresh
            StoreMgmt.BuildUsersAssignedToMe();  // TT#5664 - JSmith - All User Store Attributes appear in User Methods 
            filterDataHelper.Refresh();  // TT#1909-MD - JSmith - Str_Vesioning - Interfaced Store not available for selection in the STore List for a Static Store Attribute
            MaxStoresHelper.Refresh();
                return new ROBoolOut(eROReturnCode.Successful,"",ROInstanceID,true) ;
            }
            catch (Exception ex)
            {

                return new ROBoolOut(eROReturnCode.Failure, ex.Message, ROInstanceID, false);
            }
        }

        #endregion

    }



}
