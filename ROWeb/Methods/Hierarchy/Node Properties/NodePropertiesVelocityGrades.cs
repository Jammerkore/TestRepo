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
    public class NodePropertiesVelocityGrades : NodePropertiesBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesVelocityGrades(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base(SAB: SAB, ROWebTools: ROWebTools, profileType: eProfileType.VelocityGrade)
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
            VelocityGradeList velocityGradeList = (VelocityGradeList)nodePropertiesData;

            KeyValuePair<int, string> node = new KeyValuePair<int, string>(key: _hierarchyNodeProfile.Key, value: _hierarchyNodeProfile.Text);
            RONodePropertiesVelocityGrades nodeProperties = new RONodePropertiesVelocityGrades(node: node);

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
            return SAB.HierarchyServerSession.GetVelocityGradeList(nodeRID: parms.Key, forCopy: false);
        }

        override protected void GetFunctionSecurity(int key)
        {
            if (_hierarchyNodeProfile == null
                                || _hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesOrgNodePropertyVelocity, (int)eSecurityTypes.Allocation);
            }
            else
            {
                _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltGlobalNodePropertyVelocity, (int)eSecurityTypes.Allocation);
            }

        }
    }
}
