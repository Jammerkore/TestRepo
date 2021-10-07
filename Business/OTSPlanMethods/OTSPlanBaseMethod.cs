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
	public abstract class OTSPlanBaseMethod : ApplicationBaseMethod
	{
		/// <summary>
		/// Creates an instance of OTS Plan Base Method
		/// </summary>
		/// <param name="SAB">SessionAddressBlock</param>
		/// <param name="aMethodRID">RID for the Method.</param>
		/// <param name="aMethodType">Method Type.</param>
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public OTSPlanBaseMethod(SessionAddressBlock SAB, int aMethodRID, 
		//                         eMethodType aMethodType):base(SAB, aMethodRID, aMethodType)
		public OTSPlanBaseMethod(SessionAddressBlock SAB, int aMethodRID, 
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

        protected void BuildLowLevelLists(
            int hierarchyNodeRID,
            List<KeyValuePair<int, string>> fromLevels,
            ref eMerchandiseType fromMerchandiseType,
            List<KeyValuePair<int, string>> toLevels,
            ref eMerchandiseType toMerchandiseType
            )
        {
            eMerchandiseType merchandiseType;
            int homeHierarchyKey;
            List<HierarchyLevelComboObject> levelList = HierarchyTools.GetLevelsList(
                sessionAddressBlock: SAB,
                nodeKey: hierarchyNodeRID,
                includeHomeLevel: true,
                includeLowestLevel: false,
                includeOrganizationLevelsForAlternate: false,
                merchandiseType: out merchandiseType,
                homeHierarchyKey: out homeHierarchyKey
                );

            fromMerchandiseType = merchandiseType;
            foreach (HierarchyLevelComboObject level in levelList)
            {
                if (merchandiseType == eMerchandiseType.LevelOffset)
                {
                    fromLevels.Add(new KeyValuePair<int, string>(level.Level, level.ToString()));
                }
                else
                {
                    fromLevels.Add(new KeyValuePair<int, string>(level.Level, level.LevelName));
                }
            }

            levelList = HierarchyTools.GetLevelsList(
                sessionAddressBlock: SAB,
                nodeKey: hierarchyNodeRID,
                includeHomeLevel: false,
                includeLowestLevel: true,
                includeOrganizationLevelsForAlternate: false,
                merchandiseType: out merchandiseType,
                homeHierarchyKey: out homeHierarchyKey
                );

            toMerchandiseType = merchandiseType;
            foreach (HierarchyLevelComboObject level in levelList)
            {
                if (merchandiseType == eMerchandiseType.LevelOffset)
                {
                    toLevels.Add(new KeyValuePair<int, string>(level.Level, level.ToString()));
                }
                else
                {
                    toLevels.Add(new KeyValuePair<int, string>(level.Level, level.LevelName));
                }
            }
        }

        protected void AdjustLevelLists(
            ref ROLevelInformation fromLevel,
            List<KeyValuePair<int, string>> fromLevels,
            ref eMerchandiseType fromMerchandiseType,
        
            ref ROLevelInformation toLevel,
            List<KeyValuePair<int, string>> toLevels,
            ref eMerchandiseType toMerchandiseType
            )
        {
            eFromLevelsType fromLevelType = (eFromLevelsType)fromLevel.LevelType;
            int fromLevelOffset = fromLevel.LevelOffset;
            int fromLevelSequence = fromLevel.LevelSequence;
            eToLevelsType toLevelType = (eToLevelsType)toLevel.LevelType;
            int toLevelOffset = toLevel.LevelOffset;
            int toLevelSequence = toLevel.LevelSequence;
            bool setFromToFirstEntry = false;
            bool setToToFirstEntry = false;
            int toOffset = -1;
            if (fromLevel != null
                && fromLevel.LevelType != eROLevelsType.None)
            {
                // if different hierarchy types, update from level to 1st from entry
                if (fromLevel != null
                    && !LevelTypesSame(
                        merchandiseType: fromMerchandiseType,
                        ROLevelType: fromLevel.LevelType)
                    )
                {
                    setFromToFirstEntry = true;
                }
                else
                {
                    // set from to first entry if no longer in the list
                    // keep track of level offset so know how many to levels to remove
                    setFromToFirstEntry = true;
                    foreach (KeyValuePair<int, string> level in fromLevels)
                    {
                        ++toOffset;
                        if (fromLevel.LevelType == eROLevelsType.HierarchyLevel)
                        {
                            if (fromLevel.LevelSequence == level.Key)
                            {
                                setFromToFirstEntry = false;
                                break;
                            }
                        }
                        else if (fromLevel.LevelType == eROLevelsType.LevelOffset)
                        {
                            if (fromLevel.LevelOffset == level.Key)
                            {
                                setFromToFirstEntry = false;
                                break;
                            }
                        }
                    }
                }

                // if different hierarchy types, update from level to 1st from entry
                if (toLevel != null
                    && !LevelTypesSame(
                        merchandiseType: toMerchandiseType,
                        ROLevelType: toLevel.LevelType)
                    )
                {
                    setToToFirstEntry = true;
                }
                else
                {
                    // remove entries in to level list that are before the selected from level
                    if (!setFromToFirstEntry)
                    {
                        for (int i = 0; i < toOffset; i++)
                        {
                            toLevels.RemoveAt(0);
                        }
                    }

                    // set To to first entry if no longer in the list
                    setToToFirstEntry = true;
                    foreach (KeyValuePair<int, string> level in toLevels)
                    {
                        if (toLevel.LevelType == eROLevelsType.HierarchyLevel)
                        {
                            if (toLevel.LevelSequence == level.Key)
                            {
                                setToToFirstEntry = false;
                                break;
                            }
                        }
                        else if (toLevel.LevelType == eROLevelsType.LevelOffset)
                        {
                            if (toLevel.LevelOffset == level.Key)
                            {
                                setToToFirstEntry = false;
                                break;
                            }
                        }
                    }
                }

                // set selected values to first entry if no longer in the list
                if (setFromToFirstEntry)
                {
                    if (fromLevels.Count == 0)
                    {
                        fromLevelType = eFromLevelsType.None;
                        fromLevelOffset = -1;
                        fromLevelSequence = -1;
                    }
                    else if (fromMerchandiseType == eMerchandiseType.HierarchyLevel)
                    {
                        fromLevelType = eFromLevelsType.HierarchyLevel;
                        fromLevelOffset = -1;
                        fromLevelSequence = fromLevels[0].Key;
                    }
                    else
                    {
                        fromLevelType = eFromLevelsType.LevelOffset;
                        fromLevelOffset = fromLevels[0].Key;
                        fromLevelSequence = -1;
                    }
                    fromLevel = new ROLevelInformation();
                    fromLevel.LevelType = (eROLevelsType)fromLevelType;
                    fromLevel.LevelOffset = fromLevelOffset;
                    fromLevel.LevelSequence = fromLevelSequence;
                    fromLevel.LevelValue = GetName.GetLevelName(
                        levelType: (eROLevelsType)fromLevelType,
                        levelSequence: fromLevelSequence,
                        levelOffset: fromLevelOffset,
                        SAB: SAB
                        );
                }

                if (setToToFirstEntry)
                {
                    if (toLevels.Count == 0)
                    {
                        toLevelType = eToLevelsType.None;
                        toLevelOffset = -1;
                        toLevelSequence = -1;
                    }
                    else if (toMerchandiseType == eMerchandiseType.HierarchyLevel)
                    {
                        toLevelType = eToLevelsType.HierarchyLevel;
                        toLevelOffset = -1;
                        toLevelSequence = toLevels[0].Key;
                    }
                    else
                    {
                        toLevelType = eToLevelsType.LevelOffset;
                        toLevelOffset = toLevels[0].Key;
                        toLevelSequence = -1;
                    }
                    toLevel = new ROLevelInformation();
                    toLevel.LevelType = (eROLevelsType)toLevelType;
                    toLevel.LevelOffset = toLevelOffset;
                    toLevel.LevelSequence = toLevelSequence;
                    toLevel.LevelValue = GetName.GetLevelName(
                       levelType: (eROLevelsType)toLevelType,
                       levelSequence: toLevelSequence,
                       levelOffset: toLevelOffset,
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
    }
}
