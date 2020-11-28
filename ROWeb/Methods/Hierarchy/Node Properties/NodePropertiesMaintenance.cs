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
    public class RONodePropertiesMaintenance
    {
        //=======
        // FIELDS
        //=======
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private long _ROInstanceID;
        private NodePropertiesBase _nodePropertiesClass;
        private eProfileType _currentDataType = eProfileType.None;
        private object _currentData = null;

        //=============
        // CONSTRUCTORS
        //=============
        public RONodePropertiesMaintenance(SessionAddressBlock SAB, ROWebTools ROWebTools, long ROInstanceID)
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _ROInstanceID = ROInstanceID;
        }

        public void CleanUp()
        {
            if (_currentData != null
                && _nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus == eLockStatus.Locked)
            {
                _nodePropertiesClass.UnlockNode(key: _nodePropertiesClass.HierarchyNodeProfile.Key);
            }

            if (_nodePropertiesClass != null)
            {
                _nodePropertiesClass.OnClosing();
            }
        }

        /// <summary>
        /// Get node optionally for update
        /// </summary>
        /// <param name="parms">The RONodePropertiesParms containing the node type and key</param>
        /// <returns>A RONodePropertiesOut instance with the properties of the node</returns>
        public ROOut NodePropertiesGet(ROProfileKeyParms parms)
        {
            string message;

            RONodePropertiesOut RONodePropertiesOut = null;

            if (NodePropertiesInitializeData(parms: parms, message: out message))
            {
                if (_nodePropertiesClass.AllowView)
                {
                    RONodeProperties nodeProperties = _nodePropertiesClass.NodePropertiesGetData(parms: parms, nodePropertiesData: _currentData, message: ref message);
                    if (_nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus == eLockStatus.Locked)
                    {
                        nodeProperties.CanBeDeleted = _nodePropertiesClass.AllowDelete;
                        nodeProperties.IsReadOnly = _nodePropertiesClass.IsReadOnly;
                    }
                    RONodePropertiesOut = new RONodePropertiesOut(eROReturnCode.Successful, null, _ROInstanceID, nodeProperties);
                    _currentDataType = parms.ProfileType;
                }
                else
                {
                    RONodePropertiesOut = new RONodePropertiesOut(eROReturnCode.Failure, MIDText.GetText(eMIDTextCode.msg_NotAuthorized), _ROInstanceID, null);
                }
            }
            else
            {
                RONodePropertiesOut = new RONodePropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            return RONodePropertiesOut;
        }

        private bool NodePropertiesInitializeData(ROProfileKeyParms parms, out string message)
        {
            message = null;

            _nodePropertiesClass = GetNodePropertiesClass(profileType: parms.ProfileType);

            if (_nodePropertiesClass == null)
            {
                message = "Invalid profile type";
                return false;
            }

            int securityKey = parms.Key;
            if (securityKey == Include.NoRID
                && parms is RONodePropertyKeyParms)
            {
                securityKey = ((RONodePropertyKeyParms)parms).ParentKey;
            }

            // Unlock previously locked node
            if (_nodePropertiesClass.HierarchyNodeProfile != null
                && _nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus == eLockStatus.Locked
                && (_currentDataType != parms.ProfileType
                    || _nodePropertiesClass.HierarchyNodeProfile.Key != parms.Key))
            {
                _SAB.HierarchyServerSession.DequeueNode(nodeRID: _nodePropertiesClass.HierarchyNodeProfile.Key);
                _nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus = eLockStatus.Undefined;
            }

            _nodePropertiesClass.SetSecurity(key: parms.Key, securityKey: securityKey, setReadOnly: parms.ReadOnly);

            if (_currentData == null
                || (_nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus != eLockStatus.Locked && !parms.ReadOnly)
                || (_currentDataType != parms.ProfileType || _nodePropertiesClass.HierarchyNodeProfile.Key != parms.Key)
                )
            {
                _currentData = _nodePropertiesClass.NodePropertiesGetValues(parms: parms);
                _currentDataType = parms.ProfileType;
            }

            if (parms.Key != Include.NoRID
                && _nodePropertiesClass.HierarchyNodeProfile.Key == Include.NoRID)
            {
                _currentData = null;
                _currentDataType = eProfileType.None;
                message = _SAB.ClientServerSession.Audit.GetText(
                    messageCode: eMIDTextCode.msg_ValueWasNotFound,
                    addToAuditReport: true,
                    args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideModel_Model) }
                    );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Applies Node Properties to memory only
        /// </summary>
        /// <returns>The key of the node if the save was successful</returns>
        public ROOut NodePropertiesApply(RONodePropertiesParms parms)
        {
            string message = null;
            bool successful;
            ROProfileKeyParms NodePropertiesGetParms;
            eROReturnCode returnCode = eROReturnCode.Successful;

            _nodePropertiesClass = GetNodePropertiesClass(profileType: parms.RONodeProperties.ProfileType);

            int securityKey = parms.RONodeProperties.Node.Key;
            if (securityKey == Include.NoRID
                && parms.RONodeProperties is RONodePropertiesProfile)
            {
                securityKey = ((RONodePropertiesProfile)parms.RONodeProperties).Parent.Key;
            }

            if (_currentData == null
                || _currentDataType != parms.RONodeProperties.ProfileType
                || _nodePropertiesClass.HierarchyNodeProfile.Key != parms.RONodeProperties.Node.Key)
            {
                NodePropertiesGetParms = _nodePropertiesClass.NodePropertiesGetParms(parms: parms, profileType: parms.RONodeProperties.ProfileType, key: parms.RONodeProperties.Node.Key);
                if (!NodePropertiesInitializeData(parms: NodePropertiesGetParms, message: out message))
                {
                    return new RONodePropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }
            }

            _nodePropertiesClass.SetSecurity(key: parms.RONodeProperties.Node.Key, securityKey: securityKey, setReadOnly: parms.ReadOnly);

            if (!_nodePropertiesClass.AllowUpdate)
            {
                message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new RONodePropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            if (parms.RONodeProperties.Node.Key != Include.NoRID
                && _nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus != eLockStatus.Locked)
            {
                message = MIDText.GetText(eMIDTextCode.msg_DataNotLocked);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new RONodePropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            object data = _nodePropertiesClass.NodePropertiesUpdateData(nodeProperties: parms.RONodeProperties, cloneDates: false, message: ref message, successful: out successful, applyOnly: true);

            if (!successful)
            {
                returnCode = eROReturnCode.Failure;
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
               //MIDEnvironment.Message = message;
            }

            NodePropertiesGetParms = _nodePropertiesClass.NodePropertiesGetParms(parms: parms, profileType: _currentDataType, key: parms.RONodeProperties.Node.Key);

            if (parms.RONodeProperties.Node.Key == Include.NoRID)
            {
                // get node properties and lock
                return NodePropertiesGet(parms: NodePropertiesGetParms);
            }
            else
            {
                // replace with update data and get node Properties 
                _currentData = data;
                RONodeProperties nodeProperties = _nodePropertiesClass.NodePropertiesGetData(parms: NodePropertiesGetParms, nodePropertiesData: _currentData, message: ref message, applyOnly: true);
                if (_nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus == eLockStatus.Locked)
                {
                    nodeProperties.CanBeDeleted = _nodePropertiesClass.AllowDelete;
                    nodeProperties.IsReadOnly = _nodePropertiesClass.IsReadOnly;
                }
                return new RONodePropertiesOut(returnCode, message, _ROInstanceID, nodeProperties);
            }
        }

        /// <summary>
        /// Save Node Properties to database
        /// </summary>
        /// <returns>The key of the node properties if the save was successful</returns>
        public ROOut NodePropertiesSave(RONodePropertiesParms parms)
        {
            string message = null;
            bool successful;
            ROProfileKeyParms NodePropertiesGetParms;
            eROReturnCode returnCode = eROReturnCode.Successful;

            _nodePropertiesClass = GetNodePropertiesClass(profileType: parms.RONodeProperties.ProfileType);

            int securityKey = parms.RONodeProperties.Node.Key;
            if (securityKey == Include.NoRID)
            {
                if (parms.RONodeProperties.ProfileType == eProfileType.HierarchyNode)
                {
                    securityKey = ((RONodePropertiesProfile)parms.RONodeProperties).Parent.Key;
                }
                else
                {
                    return new RONodePropertiesOut(eROReturnCode.Failure, "Hierarchy node not found", _ROInstanceID, null);
                }
            }

            if (_currentData == null
                || _currentDataType != parms.RONodeProperties.ProfileType
                || _nodePropertiesClass.HierarchyNodeProfile.Key != parms.RONodeProperties.Node.Key)
            {
                NodePropertiesGetParms = _nodePropertiesClass.NodePropertiesGetParms(parms: parms, profileType: parms.RONodeProperties.ProfileType, key: parms.RONodeProperties.Node.Key);
                if (!NodePropertiesInitializeData(parms: NodePropertiesGetParms, message: out message))
                {
                    return new RONodePropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }
            }

            _nodePropertiesClass.SetSecurity(key: parms.RONodeProperties.Node.Key, securityKey: securityKey, setReadOnly: parms.ReadOnly);

            if (!_nodePropertiesClass.AllowUpdate)
            {
                message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new RONodePropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            if (parms.RONodeProperties.Node.Key != Include.NoRID
                && _nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus != eLockStatus.Locked)
            {
                message = MIDText.GetText(eMIDTextCode.msg_DataNotLocked);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new RONodePropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            object data = _nodePropertiesClass.NodePropertiesUpdateData(nodeProperties: parms.RONodeProperties, cloneDates: false, message: ref message, successful: out successful);

            if (!successful)
            {
                returnCode = eROReturnCode.Failure;
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                //MIDEnvironment.Message = message;
            }

            NodePropertiesGetParms = _nodePropertiesClass.NodePropertiesGetParms(parms: parms, profileType: _currentDataType, key: _nodePropertiesClass.HierarchyNodeProfile.Key);


            if (parms.RONodeProperties.Node.Key == Include.NoRID
                && successful)
            {
                // get node properties and lock
                return NodePropertiesGet(parms: NodePropertiesGetParms);
            }
            else
            {
                // replace with update data and get node Properties 
                _currentData = data;
                RONodeProperties nodeProperties = _nodePropertiesClass.NodePropertiesGetData(parms: NodePropertiesGetParms, nodePropertiesData: _currentData, message: ref message);
                if (_nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus == eLockStatus.Locked)
                {
                    nodeProperties.CanBeDeleted = _nodePropertiesClass.AllowDelete;
                    nodeProperties.IsReadOnly = _nodePropertiesClass.IsReadOnly;
                }
                return new RONodePropertiesOut(returnCode, message, _ROInstanceID, nodeProperties);
                //return new RONodePropertiesOut(eROReturnCode.Successful, null, _ROInstanceID, _nodePropertiesClass.NodePropertiesGetData(parms: NodePropertiesGetParms, nodePropertiesData: _currentData, message: ref message));
            }
        }



        /// <summary>
        /// Delete node properties fora node
        /// </summary>
        /// <param name="parms">A ROProfileKeyParms instance containing the type and key of the node properties to be deleted</param>
        /// <returns>A flag identifying if the delete was successful</returns>
        public ROOut NodePropertiesDelete(ROProfileKeyParms parms)
        {
            string message = null;

            _nodePropertiesClass = GetNodePropertiesClass(profileType: parms.ProfileType);

            int securityKey = parms.Key;
            if (securityKey == Include.NoRID
                && parms is RONodePropertyKeyParms)
            {
                securityKey = ((RONodePropertyKeyParms)parms).ParentKey;
            }

            if (_currentData == null
                || _currentDataType != parms.ProfileType
                || (_nodePropertiesClass.HierarchyNodeProfile != null && _nodePropertiesClass.HierarchyNodeProfile.Key != parms.Key)
                )
            {
                ROProfileKeyParms NodePropertiesGetParms = _nodePropertiesClass.NodePropertiesGetParms(parms: parms, profileType: parms.ProfileType, key: parms.Key);
                if (!NodePropertiesInitializeData(parms: NodePropertiesGetParms, message: out message))
                {
                    return new RONodePropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }
            }

            _nodePropertiesClass.SetSecurity(key: parms.Key, securityKey: securityKey, setReadOnly: parms.ReadOnly);

            if (!_nodePropertiesClass.AllowDelete
                || _nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus != eLockStatus.Locked
                )
            {
                if (!_nodePropertiesClass.AllowDelete)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                else if (_nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus != eLockStatus.Locked)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_DataIsReadOnly);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                return new ROBoolOut(eROReturnCode.Failure, message, parms.ROInstanceID, false);
            }

            bool success = _nodePropertiesClass.NodePropertiesDelete(key: parms.Key, message: ref message);
            if (success)
            {
                _currentData = _nodePropertiesClass.NodePropertiesGetValues(parms: parms);
            }
            else
            {
                MIDEnvironment.requestFailed = true;
                MIDEnvironment.Message = message;
            }

            return NodePropertiesGet(parms: parms);
        }

        /// <summary>
        /// Delete node properties for all descendants of a node
        /// </summary>
        /// <param name="parms">A ROProfileKeyParms instance containing the type and key of the node for which the descendants' node properties are to be deleted</param>
        /// <returns>A flag identifying if the delete was successful</returns>
        public ROOut NodePropertiesDeleteDescendants(ROProfileKeyParms parms)
        {
            string message = null;

            _nodePropertiesClass = GetNodePropertiesClass(profileType: parms.ProfileType);

            int securityKey = parms.Key;
            if (securityKey == Include.NoRID
                && parms is RONodePropertyKeyParms)
            {
                securityKey = ((RONodePropertyKeyParms)parms).ParentKey;
            }

            if (_currentData == null
                || _currentDataType != parms.ProfileType
                || (_nodePropertiesClass.HierarchyNodeProfile != null && _nodePropertiesClass.HierarchyNodeProfile.Key != parms.Key)
                )
            {
                ROProfileKeyParms NodePropertiesGetParms = _nodePropertiesClass.NodePropertiesGetParms(parms: parms, profileType: parms.ProfileType, key: parms.Key);
                if (!NodePropertiesInitializeData(parms: NodePropertiesGetParms, message: out message))
                {
                    return new RONodePropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }
            }

            _nodePropertiesClass.SetSecurity(key: parms.Key, securityKey: securityKey, setReadOnly: parms.ReadOnly);

            if (!_nodePropertiesClass.AllowApplyToLowerLevels
                || _nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus != eLockStatus.Locked
                )
            {
                if (!_nodePropertiesClass.AllowApplyToLowerLevels)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                else if (_nodePropertiesClass.HierarchyNodeProfile.NodeLockStatus != eLockStatus.Locked)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_DataIsReadOnly);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                return new ROBoolOut(eROReturnCode.Failure, message, parms.ROInstanceID, false);
            }

            bool success = _nodePropertiesClass.NodePropertiesDescendantsDelete(key: parms.Key, message: ref message);
            if (success)
            {
                _currentData = _nodePropertiesClass.NodePropertiesGetValues(parms: parms);
            }
            else
            {
                MIDEnvironment.requestFailed = true;
                MIDEnvironment.Message = message;
            }

            return NodePropertiesGet(parms: parms);
        }


        private NodePropertiesBase GetNodePropertiesClass(eProfileType profileType)
        {
            if (_nodePropertiesClass != null
                && _nodePropertiesClass.ProfileType == profileType)
            {
                return _nodePropertiesClass;
            }

            switch (profileType)
            {
                case eProfileType.HierarchyNode:
                   return new NodePropertiesProfile(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.StoreEligibility:
                    return new NodePropertiesEligibility(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.ChainSetPercent:
                    return new NodePropertiesChainSetPercent(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.ProductCharacteristic:
                    return new NodePropertiesCharacteristics(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.DailyPercentages:
                    return new NodePropertiesDailyPercentages(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.PurgeCriteria:
                    return new NodePropertiesPurgeCriteria(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.SizeCurve:
                    return new NodePropertiesSizeCurves(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.StockMinMax:
                    return new NodePropertiesStockMinMax(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.StoreCapacity:
                    return new NodePropertiesStoreCapacity(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.StoreGrade:
                    return new NodePropertiesStoreGrades(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.VelocityGrade:
                    return new NodePropertiesVelocityGrades(SAB: _SAB, ROWebTools: _ROWebTools);
                case eProfileType.IMO:
                    return new NodePropertiesVSW(SAB: _SAB, ROWebTools: _ROWebTools);

            }

            return null;
        }

        

    }
}
