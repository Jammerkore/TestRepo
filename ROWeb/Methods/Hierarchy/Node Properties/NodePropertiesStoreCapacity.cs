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
    public class NodePropertiesStoreCapacity : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesStoreCapacity(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.StoreCapacity)
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
            StoreCapacityList storeCapacityList = (StoreCapacityList)nodePropertiesData;

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesStoreCapacity nodeProperties = new RONodePropertiesStoreCapacity(node: node);

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
            return SAB.HierarchyServerSession.GetStoreCapacityList(storeList: StoreMgmt.StoreProfiles_GetActiveStoresList(), nodeRID: parms.Key, stopOnFind: false, forCopy: false);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyCapacity, (int)eSecurityTypes.Allocation);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyCapacity, (int)eSecurityTypes.Allocation);
            }
        }
    }
}
