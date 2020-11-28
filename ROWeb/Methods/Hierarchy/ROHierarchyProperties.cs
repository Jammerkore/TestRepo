using System;
using System.Collections.Generic;
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
    public class ROHierarchyProperties : ROWebFunction
    {

        private ROHierarchyPropertiesMaintenance _ROHierarchyPropertiesMaintenance = null;
        private RONodePropertiesMaintenance _RONodePropertiesMaintenance = null;

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROHierarchyProperties(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
		{
            
		}

        public ROHierarchyPropertiesMaintenance ROHierarchyPropertiesMaintenance
        {
            get
            {
                if (_ROHierarchyPropertiesMaintenance == null)
                {
                    _ROHierarchyPropertiesMaintenance = new ROHierarchyPropertiesMaintenance(SAB: SAB, ROWebTools: ROWebTools, ROInstanceID: ROInstanceID);
                }
                return _ROHierarchyPropertiesMaintenance;
            }
        }

        public RONodePropertiesMaintenance RONodePropertiesMaintenance
        {
            get
            {
                if (_RONodePropertiesMaintenance == null)
                {
                    _RONodePropertiesMaintenance = new RONodePropertiesMaintenance(SAB: SAB, ROWebTools: ROWebTools, ROInstanceID: ROInstanceID);
                }
                return _RONodePropertiesMaintenance;
            }
        }

        override public void CleanUp()
        {
            if (_ROHierarchyPropertiesMaintenance != null)
            {
                _ROHierarchyPropertiesMaintenance.CleanUp();
            }

            if (_RONodePropertiesMaintenance != null)
            {
                _RONodePropertiesMaintenance.CleanUp();
            }
        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                //Hierarchy Properties
                case eRORequest.HierarchyPropertiesGet:
                    return ROHierarchyPropertiesMaintenance.HierarchyPropertiesGet((ROHierarchyPropertyKeyParms)Parms);
                case eRORequest.HierarchyPropertiesApply:
                    return ROHierarchyPropertiesMaintenance.HierarchyPropertiesApply((ROHierarchyPropertiesParms)Parms);
                case eRORequest.HierarchyPropertiesSave:
                    return ROHierarchyPropertiesMaintenance.HierarchyPropertiesSave((ROHierarchyPropertiesParms)Parms);
                case eRORequest.HierarchyPropertiesDelete:
                    return ROHierarchyPropertiesMaintenance.HierarchyPropertiesDelete((ROHierarchyPropertyKeyParms)Parms);

                //Node Properties
                case eRORequest.NodePropertiesGet:
                    if (Parms is ROStringParms)
                    {
                        ROStringParms parms = (ROStringParms)Parms;
                        return GetNodeProperties(parms.ROString, SAB);
                    }
                    else
                    {
                        return RONodePropertiesMaintenance.NodePropertiesGet((ROProfileKeyParms)Parms);
                    }
                case eRORequest.NodePropertiesApply:
                    return RONodePropertiesMaintenance.NodePropertiesApply((RONodePropertiesParms)Parms);
                case eRORequest.NodePropertiesSave:
                    return RONodePropertiesMaintenance.NodePropertiesSave((RONodePropertiesParms)Parms);
                case eRORequest.NodePropertiesDelete:
                    return RONodePropertiesMaintenance.NodePropertiesDelete((ROProfileKeyParms)Parms);
                case eRORequest.NodePropertiesDeleteDescendants:
                    return RONodePropertiesMaintenance.NodePropertiesDeleteDescendants((ROProfileKeyParms)Parms);
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        /// <summary>
        /// Returns the node properties from the hierarchy
        /// </summary>
        /// <param name="sNodeID">The node description.</param>
        /// <param name="aSAB">The reference to the SessionAddressBlock object for the user and environment</param>
        /// <returns>A datatable</returns>
        private ROOut GetNodeProperties(string sNodeID, SessionAddressBlock aSAB)
        {
            try
            {

                DataTable dt = new DataTable();
                dt.TableName = "Node Properties";
                dt.Columns.Add("NODE_RID", typeof(int));
                dt.Columns.Add("PARENT_RID", typeof(int));
                dt.Columns.Add("NODE_ID", typeof(string));
                dt.Columns.Add("LEVEL", typeof(string));

                //get nodes from hierarchy service
                HierarchyNodeProfile hnp = aSAB.HierarchyServerSession.GetNodeDataFromBaseSearchString(sNodeID);
                if (hnp.Key != Include.NoRID)
                {
                    DataRow dr = dt.NewRow();
                    dr["NODE_RID"] = hnp.Key;
                    dr["PARENT_RID"] = hnp.HomeHierarchyParentRID;

                    if (hnp.Text != null)
                    {
                        dr["NODE_ID"] = hnp.Text;
                    }
                    if (hnp.LevelText != null)
                    {
                        dr["LEVEL"] = hnp.LevelText;
                    }
                    dt.Rows.Add(dr);

                }
                return new RODataTableOut(eROReturnCode.Successful, null, ROInstanceID, dt);
                //return dt;
            }
            catch 
            {
                throw;
            }
        }
    }
}
