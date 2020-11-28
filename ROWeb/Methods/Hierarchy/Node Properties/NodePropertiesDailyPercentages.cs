using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;

namespace Logility.ROWeb
{
    public class NodePropertiesDailyPercentages : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesDailyPercentages(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.DailyPercentages)
        {
            
        }

        //===========
        // PROPERTIES
        //===========



        //========
        // METHODS
        //========

        override public bool OnClosing()
        {
            return true;
        }


        override public RONodeProperties NodePropertiesGetData(ROProfileKeyParms parms, object nodePropertiesData, ref string message, bool applyOnly = false)
        {
            StoreDailyPercentagesList dailyPercentagesList = (StoreDailyPercentagesList)nodePropertiesData;

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesDailyPercentages nodeProperties = new RONodePropertiesDailyPercentages(node: node);

            // populate modelProperties using Windows\NodeProperties.cs as a reference


            return nodeProperties;
        }
    

        override public object NodePropertiesUpdateData(RONodeProperties nodePropertiesData, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;

            

            return null;
        }

        override public bool NodePropertiesDelete(int key, ref string message)
        {


            return true;
        }

        override public bool NodePropertiesDescendantsDelete(int key, ref string message)
        {



            return true;
        }

        override public object NodePropertiesGetValues(ROProfileKeyParms parms)
        {
            return SAB.HierarchyServerSession.GetStoreDailyPercentagesList(storeList: StoreMgmt.StoreProfiles_GetActiveStoresList(), nodeRID: parms.Key);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyDailyPcts, (int)eSecurityTypes.Allocation);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyDailyPcts, (int)eSecurityTypes.Allocation);
            }
        }
    }
}
