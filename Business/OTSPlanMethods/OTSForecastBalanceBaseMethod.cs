using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

using Logility.ROWebSharedTypes;

namespace MIDRetail.Business
{
	/// <summary>
	/// The base OTS Plan business method
	/// </summary>
	/// <remarks>
	/// This method inherits from the base application method
	/// </remarks>
	public abstract class OTSForecastBalanceBaseMethod : ApplicationBaseMethod
	{
		/// <summary>
		/// Creates an instance of OTS Plan Base Method
		/// </summary>
		/// <param name="SAB">SessionAddressBlock</param>
		/// <param name="aMethodRID">RID for the Method.</param>
		/// <param name="aMethodType">Method Type.</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public OTSForecastBalanceBaseMethod(SessionAddressBlock SAB, int aMethodRID, 
		//    eMethodType aMethodType):base(SAB, aMethodRID, aMethodType)
		public OTSForecastBalanceBaseMethod(SessionAddressBlock SAB, int aMethodRID, 
			eMethodType aMethodType, eProfileType aProfileType):base(SAB, aMethodRID, aMethodType, aProfileType)
		//End TT#523 - JScott - Duplicate folder when new folder added
		{
			// 
			// TODO: Add constructor logic here
			//
		}

		internal override void VerifyAction(eMethodType aMethodType)
		{
			if (!Enum.IsDefined(typeof(eForecastMethodType), Convert.ToInt32(base.MethodType, CultureInfo.CurrentUICulture)))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_UnknownMethodInWorkFlow,
					MIDText.GetText(eMIDTextCode.msg_UnknownMethodInWorkFlow));
			}
		}

        protected ProfileList GetForecastVersionList(
            eSecuritySelectType securitySelectType,
            eSecurityTypes chainOrStore
            )
        {
            return GetForecastVersionList(securitySelectType, chainOrStore, false, Include.NoRID, false);
        }

        protected ProfileList GetForecastVersionList(
            eSecuritySelectType securitySelectType,
            eSecurityTypes chainOrStore,
            bool excludeActual
            )
        {
            return GetForecastVersionList(securitySelectType, chainOrStore, false, Include.NoRID, excludeActual);
        }

        protected ProfileList GetForecastVersionList(
            eSecuritySelectType securitySelectType,
            eSecurityTypes chainOrStore,
            bool addEmptySelection,
            int methodVersionKey
            )
        {
            return GetForecastVersionList(securitySelectType, chainOrStore, addEmptySelection, methodVersionKey, false);
        }

        protected ProfileList GetForecastVersionList(
            eSecuritySelectType securitySelectType,
            eSecurityTypes chainOrStore,
            bool addEmptySelection,
            int methodVersionKey,
            bool excludeActual
            )

        {

            ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions(securitySelectType, chainOrStore, methodVersionKey);
            if (addEmptySelection)
            {
                VersionProfile versionProfile = new VersionProfile(Include.NoRID, " ", false);
                versionProfList.Insert(0, versionProfile);
            }
            if (excludeActual)
            {
                if (versionProfList.Contains(Include.FV_ActualRID))
                {
                    VersionProfile versionProfile = (VersionProfile)versionProfList.FindKey(Include.FV_ActualRID);
                    versionProfList.Remove(versionProfile);
                }
            }
            return versionProfList;
        }

        protected void BuildLowLevelsList(
            int hierarchyNodeRID,
            List<KeyValuePair<int, string>> lowLevels,
            ref eMerchandiseType lowLevelMerchandiseType
            )
        {
            eMerchandiseType merchandiseType;
            int homeHierarchyKey;
            List<HierarchyLevelComboObject> levelList = HierarchyTools.GetLevelsList(
                sessionAddressBlock: SAB,
                nodeKey: hierarchyNodeRID,
                includeHomeLevel: false,
                includeLowestLevel: true,
                includeOrganizationLevelsForAlternate: false,
                merchandiseType: out merchandiseType,
                homeHierarchyKey: out homeHierarchyKey
                );

            lowLevelMerchandiseType = merchandiseType;
            foreach (HierarchyLevelComboObject level in levelList)
            {
                if (merchandiseType == eMerchandiseType.LevelOffset)
                {
                    lowLevels.Add(new KeyValuePair<int, string>(level.Level, level.ToString()));
                }
                else
                {
                    lowLevels.Add(new KeyValuePair<int, string>(level.Level, level.LevelName));
                }
            }
        }

        protected void AdjustLevelList(
            ref ROLevelInformation lowLevel,
            List<KeyValuePair<int, string>> lowLevels,
            ref eMerchandiseType lowLevelMerchandiseType
            )
        {
            eLowLevelsType lowLevelType = (eLowLevelsType)lowLevel.LevelType;
            int lowLevelOffset = lowLevel.LevelOffset;
            int lowLevelSequence = lowLevel.LevelSequence;
            bool setFromToFirstEntry = false;
            int toOffset = -1;
            if (lowLevel != null
                && lowLevel.LevelType != eROLevelsType.None)
            {
                // if different hierarchy types, update from level to 1st from entry
                if (lowLevel != null
                    && !LevelTypesSame(
                        merchandiseType: lowLevelMerchandiseType,
                        ROLevelType: lowLevel.LevelType)
                    )
                {
                    setFromToFirstEntry = true;
                }
                else
                {
                    // set from to first entry if no longer in the list
                    // keep track of level offset so know how many to levels to remove
                    setFromToFirstEntry = true;
                    foreach (KeyValuePair<int, string> level in lowLevels)
                    {
                        ++toOffset;
                        if (lowLevel.LevelType == eROLevelsType.HierarchyLevel)
                        {
                            if (lowLevel.LevelSequence == level.Key)
                            {
                                setFromToFirstEntry = false;
                                break;
                            }
                        }
                        else if (lowLevel.LevelType == eROLevelsType.LevelOffset)
                        {
                            if (lowLevel.LevelOffset == level.Key)
                            {
                                setFromToFirstEntry = false;
                                break;
                            }
                        }
                    }
                }

                // set selected values to first entry if no longer in the list
                if (setFromToFirstEntry)
                {
                    if (lowLevels.Count == 0)
                    {
                        lowLevelType = eLowLevelsType.None;
                        lowLevelOffset = -1;
                        lowLevelSequence = -1;
                    }
                    else if (lowLevelMerchandiseType == eMerchandiseType.HierarchyLevel)
                    {
                        lowLevelType = eLowLevelsType.HierarchyLevel;
                        lowLevelOffset = -1;
                        lowLevelSequence = lowLevels[0].Key;
                    }
                    else
                    {
                        lowLevelType = eLowLevelsType.LevelOffset;
                        lowLevelOffset = lowLevels[0].Key;
                        lowLevelSequence = -1;
                    }
                    lowLevel = new ROLevelInformation();
                    lowLevel.LevelType = (eROLevelsType)lowLevelType;
                    lowLevel.LevelOffset = lowLevelOffset;
                    lowLevel.LevelSequence = lowLevelSequence;
                    lowLevel.LevelValue = GetName.GetLevelName(
                        levelType: (eROLevelsType)lowLevelType,
                        levelSequence: lowLevelSequence,
                        levelOffset: lowLevelOffset,
                        SAB: SAB
                        );
                }
            }
        }

        private bool LevelTypesSame(eMerchandiseType merchandiseType, eROLevelsType ROLevelType)
        {
            if (merchandiseType == eMerchandiseType.HierarchyLevel
                && ROLevelType == eROLevelsType.HierarchyLevel)
            {
                return true;
            }

            if (merchandiseType == eMerchandiseType.LevelOffset
                && ROLevelType == eROLevelsType.LevelOffset)
            {
                return true;
            }

            return false;
        }

        protected List<KeyValuePair<int, string>> BuildOverrideLowLevelList(
            int overrideLowLevelRid,
            int customOverrideLowLevelRid)
        {
            string ROOVERRIDELOWLEVELNAME = " (Custom)";

            List<KeyValuePair<int, string>> outOverrideLowLevelList = new List<KeyValuePair<int, string>>();

            FunctionSecurityProfile userSecurity;
            FunctionSecurityProfile globalSecurity;
            ProfileList overrideLowLevelList;
            string overrideLowLevelName;

            try
            {
                userSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsUserOverrideLowLevels);
                globalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsGlobalOverrideLowLevels);

                overrideLowLevelList = OverrideLowLevelProfile.LoadAllProfiles(
                    aOllRID: overrideLowLevelRid,
                    aUserRID: SAB.ClientServerSession.UserRID,
                    globalAllowView: globalSecurity.AllowView,
                    userAllowView: userSecurity.AllowView,
                    customOllRID: customOverrideLowLevelRid
                    );

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
    }
}
