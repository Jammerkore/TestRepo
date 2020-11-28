using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
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
    abstract public class NodePropertiesBase : HierarchyBase
    {
        //=======
        // FIELDS
        //=======
        
        protected HierarchyNodeSecurityProfile _nodeSecurity = null;
        protected HierarchyNodeProfile _hierarchyNodeProfile = null;
        private HierarchyNodeProfile _securityNodeProfile = null;

        //=============
        // CONSTRUCTORS
        //=============
        public NodePropertiesBase(SessionAddressBlock SAB, ROWebTools ROWebTools, eProfileType profileType) :
            base(SAB: SAB, ROWebTools: ROWebTools, profileType: profileType)
        {

        }

        //===========
        // PROPERTIES
        //===========

        public HierarchyNodeSecurityProfile HierarchyNodeSecurity
        {
            get
            {
                if (_nodeSecurity == null)
                {
                    _nodeSecurity = new HierarchyNodeSecurityProfile(aKey: Include.NoRID);
                }
                return _nodeSecurity;
            }
        }

        public HierarchyNodeProfile HierarchyNodeProfile
        {
            get
            {
                return _hierarchyNodeProfile;
            }
        }

        public bool AllowView
        {
            get
            {
                return _functionSecurity.AllowView && _nodeSecurity.AllowView;
            }
        }

        public bool AllowUpdate
        {
            get
            {
                return _functionSecurity.AllowUpdate && _nodeSecurity.AllowUpdate;
            }
        }

        public bool AllowDelete
        {
            get
            {
                return _functionSecurity.AllowDelete && _nodeSecurity.AllowDelete;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _functionSecurity.IsReadOnly || _nodeSecurity.IsReadOnly;
            }
        }

        public bool AllowApplyToLowerLevels
        {
            get
            {
                return _functionSecurity.AllowApplyToLowerLevels && _nodeSecurity.AllowDelete;
            }
        }

        //========
        // METHODS
        //========


        abstract public RONodeProperties NodePropertiesGetData(ROProfileKeyParms parms, object nodePropertiesData, ref string message, bool applyOnly = false);

        abstract public object NodePropertiesUpdateData(RONodeProperties nodeProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false);

        abstract public bool NodePropertiesDelete(int key, ref string message);

        abstract public bool NodePropertiesDescendantsDelete(int key, ref string message);

        abstract public object NodePropertiesGetValues(ROProfileKeyParms parms);

        abstract protected void GetFunctionSecurity(int key);

        abstract public bool OnClosing();

        virtual public ROProfileKeyParms NodePropertiesGetParms(RONodePropertiesParms parms, eProfileType profileType, int key, bool readOnly = false)
        {
            int parentKey = Include.NoRID;
            if (parms.RONodeProperties.ProfileType == eProfileType.HierarchyNode)
            {
                parentKey = ((RONodePropertiesProfile)parms.RONodeProperties).Parent.Key;
            }

            RONodePropertyKeyParms profileKeyParms = new RONodePropertyKeyParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.NodePropertiesGet,
                ROInstanceID: parms.ROInstanceID,
                profileType: profileType,
                key: key,
                readOnly: readOnly,
                parentKey: parentKey
                );

            return profileKeyParms;
    }

    virtual public ROProfileKeyParms NodePropertiesGetParms(ROProfileKeyParms parms, eProfileType profileType, int key, bool readOnly = false)
        {
            ROProfileKeyParms profileKeyParms = new ROProfileKeyParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.NodePropertiesGet,
                ROInstanceID: parms.ROInstanceID,
                profileType: profileType,
                key: key,
                readOnly: readOnly
                );

            return profileKeyParms;
        }

        public void UnlockNode(int key)
        {
            SAB.HierarchyServerSession.DequeueNode(nodeRID: key);
        }

        public eLockStatus LockNode(eModelType modelType, int key, string name, bool allowReadOnly, out string message)
        {
            message = null;
            eLockStatus lockStatus = eLockStatus.Undefined;

            return lockStatus;
        }

        public void SetSecurity(int key, int securityKey, bool setReadOnly)
        {
            if (_securityNodeProfile == null
                || _securityNodeProfile.Key != securityKey)
            {
                _securityNodeProfile = SAB.HierarchyServerSession.GetNodeData(nodeRID: securityKey, chaseHierarchy: false);
            }

            if (_nodeSecurity == null
                    || (_hierarchyNodeProfile != null
                        && _securityNodeProfile.Key != securityKey)
                    )
            {
                if (key == Include.NoRID)
                {
                    _nodeSecurity = new HierarchyNodeSecurityProfile(Include.NoRID);
                    _nodeSecurity.SetFullControl();
                }
                else
                {
                    _nodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(securityKey, (int)eSecurityTypes.All);
                }
            }

            if (_functionSecurity == null
                    || (_hierarchyNodeProfile != null
                        && _securityNodeProfile.Key != securityKey)
                    )
            {
                if (key == Include.NoRID)
                {
                    _hierarchyNodeProfile = new HierarchyNodeProfile(Include.NoRID);
                    _hierarchyNodeProfile.HomeHierarchyType = _securityNodeProfile.HomeHierarchyType;
                    _hierarchyNodeProfile.HomeHierarchyRID = _securityNodeProfile.HomeHierarchyRID;
                    _hierarchyNodeProfile.HomeHierarchyOwner = _securityNodeProfile.HomeHierarchyOwner;
                    _hierarchyNodeProfile.NodeLockStatus = eLockStatus.Locked;
                }
                else if (setReadOnly
                    || !_nodeSecurity.AllowUpdate)
                {
                    _hierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(nodeRID: key, chaseHierarchy: true);
                }
                else
                {
                    _hierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeDataForUpdate(aNodeRID: key, aAllowReadOnly: true);
                }

                if (_securityNodeProfile.HomeHierarchyOwner != Include.GlobalUserRID)
                {
                    if (key == Include.NoRID)
                    {
                        _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(securityKey, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
                    }
                    else
                    {
                        _functionSecurity = SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
                    }
                }
                else
                {
                    GetFunctionSecurity(key: securityKey);
                }
            }
        }

        protected EditMsgs ApplyToLowerLevels(bool deleteEligibility = false,
            bool deleteStoreGrades = false,
            bool deleteSizeCurveCriteria = false,
            bool deleteSizeCurveTolerance = false,
            bool deleteSizeCurveSimilarStore = false,
            bool deleteSizeOutOfStock = false,
            bool deleteSizeSellThru = false,
            bool deleteVelocityGrades = false,
            bool deleteSellThruPcts = false,
            bool deleteStoreCapacity = false,
            bool deletePurgeCriteria = false,
            bool deleteDailyPercentages = false,
            bool deleteStockMinMaxes = false,
            bool deleteCharacteristics = false,
            bool deleteChainSetPercentages = false,
            bool deleteIMO = false
            )
        {
            try
            {
                bool continueProcessing = false;

                int descendantCount = 0;

                if (HierarchyNodeProfile != null &&
                    HierarchyNodeProfile.HasChildren &&
                    (deleteEligibility ||
                    deleteStoreGrades ||
                    deleteSizeCurveCriteria ||
                    deleteSizeCurveTolerance ||
                    deleteSizeCurveSimilarStore ||
                    deleteSizeOutOfStock ||
                    deleteSizeSellThru ||
                    deleteVelocityGrades ||
                    deleteSellThruPcts ||
                    deleteStoreCapacity ||
                    deletePurgeCriteria ||
                    deleteDailyPercentages ||
                    deleteStockMinMaxes ||
                    deleteCharacteristics
                    || deleteChainSetPercentages
                    || deleteIMO 
                    ))
                {
                    continueProcessing = true;

                }
                else
                {
                    continueProcessing = false;
                }

                EditMsgs em = new EditMsgs();
                if (continueProcessing)
                {
                    string applyMsg = MIDText.GetText(eMIDTextCode.msg_ApplyStatus);
                    descendantCount = SAB.HierarchyServerSession.GetDescendantCount(HierarchyNodeProfile.HomeHierarchyRID, HierarchyNodeProfile.Key);
                    applyMsg = applyMsg.Replace("{1}", descendantCount.ToString(CultureInfo.CurrentUICulture));
                    int count = 0;
                    
                    HierarchyNodeList hierarchyChildrenList = SAB.HierarchyServerSession.GetHierarchyChildren(HierarchyNodeProfile.NodeLevel, HierarchyNodeProfile.HierarchyRID, HierarchyNodeProfile.HomeHierarchyRID, HierarchyNodeProfile.Key, false, eNodeSelectType.All);
                    foreach (HierarchyNodeProfile hnp in hierarchyChildrenList) // apply to children
                    {
                        ApplyToDescendant(ref em, applyMsg, ref count, descendantCount, hnp.Key, hnp.NodeLevel,
                            hnp.HierarchyRID, hnp.HomeHierarchyRID, deleteEligibility, deleteStoreGrades, deleteVelocityGrades,
                            deleteStoreCapacity, deletePurgeCriteria, deleteDailyPercentages, deleteSellThruPcts,
                            deleteStockMinMaxes, deleteSizeCurveCriteria, deleteSizeCurveTolerance, deleteSizeCurveSimilarStore,
                            deleteSizeOutOfStock, deleteSizeSellThru, deleteCharacteristics, deleteChainSetPercentages,
                            deleteIMO                             );
                    }
                }

                return em;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ApplyToDescendant(ref EditMsgs em, string applyMsg, ref int count, int descendantCount,
            int applyNodeRID, int applyNodeLevel, int applyNodeHierarchyRID, int applyNodeHomeHierarchyRID, bool deleteEligibility,
            bool deleteStoreGrades, bool deleteVelocityGrades, bool deleteStoreCapacity, bool deletePurgeCriteria,
            bool deleteDailyPercentages, bool deleteSellThruPcts, bool deleteStockMinMaxes,
            bool deleteSizeCurveCriteria, bool deleteSizeCurveTolerance, bool deleteSizeCurveSimilarStore,
            bool deleteSizeOutOfStock, bool deleteSizeSellThru, bool deleteCharacteristics, bool deleteChainSetPercentages, bool deleteIMO
            )
        {
            try
            {
                ++count;
                string updatedMessage = applyMsg.Replace("{0}", count.ToString(CultureInfo.CurrentUICulture));

                SAB.HierarchyServerSession.DeleteNodeData(applyNodeRID, false, deleteEligibility,
                    deleteStoreGrades, deleteVelocityGrades, deleteStoreCapacity, deletePurgeCriteria,
                    deleteDailyPercentages, deleteSellThruPcts, deleteStockMinMaxes,
                    deleteSizeCurveCriteria, deleteSizeCurveTolerance, deleteSizeCurveSimilarStore,
                    deleteSizeOutOfStock, deleteSizeSellThru, deleteCharacteristics, deleteChainSetPercentages, deleteIMO //TT#1401 - Reservation Stores - gtaylor
                    );
                HierarchyNodeList hierarchyChildrenList = SAB.HierarchyServerSession.GetHierarchyChildren(applyNodeLevel, applyNodeHierarchyRID,
                    applyNodeHomeHierarchyRID, applyNodeRID, false, eNodeSelectType.All);
                foreach (HierarchyNodeProfile hnp in hierarchyChildrenList)
                {
                    ApplyToDescendant(ref em, applyMsg, ref count, descendantCount, hnp.Key, hnp.NodeLevel,
                        hnp.HierarchyRID, hnp.HomeHierarchyRID, deleteEligibility, deleteStoreGrades, deleteVelocityGrades,
                        deleteStoreCapacity, deletePurgeCriteria, deleteDailyPercentages, deleteSellThruPcts, deleteStockMinMaxes,
                        deleteSizeCurveCriteria, deleteSizeCurveTolerance, deleteSizeCurveSimilarStore,
                        deleteSizeOutOfStock, deleteSizeSellThru, deleteCharacteristics, deleteChainSetPercentages, deleteIMO //TT#1401 - Reservation Stores - gtaylor
                        );

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
