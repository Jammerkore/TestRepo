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
