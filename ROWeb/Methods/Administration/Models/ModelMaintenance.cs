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
    public class ROModelMaintenance
    {
        //=======
        // FIELDS
        //=======
        private SessionAddressBlock _SAB;
        private ROWebTools _ROWebTools;
        private long _ROInstanceID;
        private ModelBase _modelClass;
        private eModelType _currentModelType = eModelType.None;
        private int _currentLockKey = Include.NoRID;
        private ModelProfile _currentModelProfile = null;
        private Dictionary<eModelType, Dictionary<int, ModelBase>> _modelTypes;
        private Dictionary<int, ModelBase> _modelClasses;

        //=============
        // CONSTRUCTORS
        //=============
        public ROModelMaintenance(SessionAddressBlock SAB, ROWebTools ROWebTools, long ROInstanceID)
        {
            _SAB = SAB;
            _ROWebTools = ROWebTools;
            _ROInstanceID = ROInstanceID;
            _modelTypes = new Dictionary<eModelType, Dictionary<int, ModelBase>>();
        }

        public void CleanUp()
        {
            // release locks for all models retrieved
            foreach (KeyValuePair<eModelType, Dictionary<int, ModelBase>> classEntry in _modelTypes)
            {
                if (classEntry.Value != null)
                {
                    Dictionary<int, ModelBase> modelClass = classEntry.Value;
                    List<int> keys = new List<int>();
                    foreach (KeyValuePair<int, ModelBase> modelEntry in modelClass)
                    {
                       keys.Add(modelEntry.Key);
                    }
                    foreach (int key in keys)
                    {
                        CloseModel(key: key);
                    }
                }
            }
            _modelTypes.Clear();
        }

        /// <summary>
        /// Get list of models 
        /// </summary>
        /// <param name="parms">The ROModelParms containing the model type and key</param>
        /// <returns>A ROIntStringPairListOut instance with the list of the models</returns>
        public ROOut GetModels(ROModelParms parms)
        {
            string message;
            ModelBase modelClass;

            // Do not add class object used to get model list to the cache
			// and do not update the global field used to save values across calls
			modelClass = GetModelClass(
                modelType: parms.ModelType,
                key: parms.Key,
                addToCache: false
                );

            if (modelClass == null)
            {
                return new ROIntStringPairListOut(eROReturnCode.Failure, "Invalid model type", parms.ROInstanceID, null);
            }

            if (!modelClass.FunctionSecurity.AllowView)
            {
                message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROIntStringPairListOut(eROReturnCode.Failure, message, parms.ROInstanceID, null);
            }

            return new ROIntStringPairListOut(eROReturnCode.Successful, null, _ROInstanceID, modelClass.ModelGetList());
        }

        /// <summary>
        /// Get model optionally for update
        /// </summary>
        /// <param name="parms">The ROModelParms containing the model type and key</param>
        /// <returns>A ROModelPropertiesOut instance with the properties of the model</returns>
        public ROOut GetModel(ROModelParms parms)
        {
            string message;

            ROModelPropertiesOut ROModelsOut = null;

            if (InitializeModelData(parms: parms, message: out message))
            {
                if (_modelClass.FunctionSecurity.AllowView)
                {
                    ROModelProperties modelProperties = _modelClass.ModelGetData(parms: parms, modelProfile: _currentModelProfile, message: ref message);
                    if (_currentModelProfile.ModelLockStatus == eLockStatus.Locked
                        || _currentModelProfile.Key == Include.NoRID)
                    {
                        modelProperties.CanBeDeleted = _modelClass.FunctionSecurity.AllowDelete;
                        modelProperties.IsReadOnly = _modelClass.FunctionSecurity.IsReadOnly;
                        _currentLockKey = _currentModelProfile.Key;
                    }
                    ROModelsOut = new ROModelPropertiesOut(eROReturnCode.Successful, null, _ROInstanceID, modelProperties);
                    _currentModelType = parms.ModelType;
                }
                else
                {
                    ROModelsOut = new ROModelPropertiesOut(eROReturnCode.Failure, MIDText.GetText(eMIDTextCode.msg_NotAuthorized), _ROInstanceID, null);
                }
            }
            else
            {
                ROModelsOut = new ROModelPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            return ROModelsOut;
        }

        private bool InitializeModelData(ROModelParms parms, out string message)
        {
            message = null;

            _modelClass = GetModelClass(
                modelType: parms.ModelType,
                key: parms.Key
                );

            if (_modelClass == null)
            {
                message = "Invalid model type";
                return false;
            }

            // Unlock previously locked model
            if (_currentModelProfile != null
                && _currentModelProfile.ModelLockStatus == eLockStatus.Locked
                && (_currentModelType != parms.ModelType
                    || _currentLockKey != parms.Key))
            {
                _SAB.HierarchyServerSession.DequeueModel(aModelType: GetModelType(_currentModelProfile.ProfileType), aModelRID: _currentLockKey);
                _currentModelProfile.ModelLockStatus = eLockStatus.Undefined;
                _currentLockKey = Include.NoRID;
            }

            if (_currentModelProfile == null
                || (_currentModelProfile.ModelLockStatus != eLockStatus.Locked && !parms.ReadOnly)
                || (_currentModelType != parms.ModelType || _currentModelProfile.Key != parms.Key)
                )
            {
                _currentModelProfile = _modelClass.GetModelProfile(parms: parms);
                _modelClass.CurrentModelProfile = _currentModelProfile;
                _currentModelType = parms.ModelType;
            }

            if (parms.Key != Include.NoRID
                && _currentModelProfile.Key == Include.NoRID)
            {
                _currentModelProfile = null;
                _currentModelType = eModelType.None;
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
        /// Applies Model to memory only
        /// </summary>
        /// <returns>The key of the model if the save was successful</returns>
        public ROOut ApplyModel(ROModelPropertiesParms parms)
        {
            string message = null;
            bool successful;
            ROModelParms getModelParms;
            eROReturnCode returnCode = eROReturnCode.Successful;

            _modelClass = GetModelClass(
                modelType: parms.ROModelProperties.ModelType,
                key: parms.ROModelProperties.Model.Key,
                performingSave: true
                );

            if (!_modelClass.FunctionSecurity.AllowUpdate)
            {
                message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROModelPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            if (parms.ROModelProperties.Model.Key != Include.NoRID
                && _currentModelProfile.ModelLockStatus != eLockStatus.Locked)
            {
                message = MIDText.GetText(eMIDTextCode.msg_DataNotLocked);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROModelPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            ModelProfile mp = _modelClass.ModelUpdateData(modelsProperties: parms.ROModelProperties, cloneDates: false, message: ref message, successful: out successful, applyOnly: true);

            if (!successful)
            {
                returnCode = eROReturnCode.Failure;
            }

            getModelParms = _modelClass.GetModelParms(parms: parms, modelType: _currentModelType, key: mp.Key);

            //if (parms.ROModelProperties.Model.Key == Include.NoRID)
            //{
            //    // get model and lock since new model
            //    return GetModel(parms: getModelParms);
            //}
            //else
            {
                // replace with update data and get model 
                _currentModelProfile = mp;
                _modelClass.CurrentModelProfile = _currentModelProfile;
                ROModelProperties modelProperties = _modelClass.ModelGetData(parms: getModelParms, modelProfile: _currentModelProfile, message: ref message, applyOnly: true);
                if (_currentModelProfile.ModelLockStatus == eLockStatus.Locked)
                {
                    modelProperties.CanBeDeleted = _modelClass.FunctionSecurity.AllowDelete;
                    modelProperties.IsReadOnly = _modelClass.FunctionSecurity.IsReadOnly;
                }
                return new ROModelPropertiesOut(returnCode, message, _ROInstanceID, modelProperties);
            }
        }

        /// <summary>
        /// Save Model to database
        /// </summary>
        /// <returns>The key of the model if the save was successful</returns>
        public ROOut SaveModel(ROModelPropertiesParms parms)
        {
            string message = null;
            bool successful = true;
            bool applyOnly = false;
            string modelName;
            ROModelParms getModelParms;
            eROReturnCode returnCode = eROReturnCode.Successful;

            _modelClass = GetModelClass(
                modelType: parms.ROModelProperties.ModelType,
                key: parms.ROModelProperties.Model.Key,
                performingSave: true
                );
            if (_currentModelProfile == null
                || _currentModelType != parms.ROModelProperties.ModelType
                || _currentModelProfile.Key != parms.ROModelProperties.Model.Key)
            {
                getModelParms = _modelClass.GetModelParms(parms: parms, modelType: parms.ROModelProperties.ModelType, key: parms.ROModelProperties.Model.Key);
                if (!InitializeModelData(parms: getModelParms, message: out message))
                {
                    return new ROModelPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }
            }

            if (!_modelClass.FunctionSecurity.AllowUpdate)
            {
                message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROModelPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            if (parms.ROModelProperties.Model.Key != Include.NoRID
                && _currentModelProfile.ModelLockStatus != eLockStatus.Locked)
            {
                message = MIDText.GetText(eMIDTextCode.msg_DataNotLocked);
                _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                MIDEnvironment.Message = message;
                return new ROModelPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
            }

            if (parms.ROModelProperties.Model.Key == Include.NoRID
                && _modelClass.ModelNameExists(parms.ROModelProperties.Model.Value))
            {
                message = "Models must be unique.";
                MIDEnvironment.Message = message;
                MIDEnvironment.requestFailed = true;
                returnCode = eROReturnCode.Failure;
                applyOnly = true;
            }

            ModelProfile mp = _modelClass.ModelUpdateData(modelsProperties: parms.ROModelProperties, cloneDates: false, message: ref message, successful: out successful, applyOnly: applyOnly);

            if (!successful)
            {
                returnCode = eROReturnCode.Failure;
            }

            // remove undefined model from cache and add new key
			if (parms.ROModelProperties.Model.Key < 0)
            {
                _modelClasses.Remove(parms.ROModelProperties.Model.Key);
            }

            _modelClasses[mp.Key] = _modelClass;

            getModelParms = _modelClass.GetModelParms(parms: parms, modelType: _currentModelType, key: mp.Key);

            if (parms.ROModelProperties.Model.Key == Include.NoRID)
            {
                // get model and lock since new model
                return GetModel(parms: getModelParms);
            }
            else
            {
                // replace with update data and get model 
                _currentModelProfile = mp;
                _modelClass.CurrentModelProfile = _currentModelProfile;
                ROModelProperties rOModelProperties = _modelClass.ModelGetData(parms: getModelParms, modelProfile: _currentModelProfile, message: ref message);
                if (_currentModelProfile.ModelLockStatus == eLockStatus.Locked)
                {
                    rOModelProperties.CanBeDeleted = _modelClass.FunctionSecurity.AllowDelete;
                    rOModelProperties.IsReadOnly = _modelClass.FunctionSecurity.IsReadOnly;
                    _currentLockKey = _currentModelProfile.Key;
                }
                return new ROModelPropertiesOut(returnCode, null, _ROInstanceID, rOModelProperties);
            }
        }

        /// <summary>
        /// Save model data as a new name
        /// </summary>
        /// <param name="parms"> A ROModelCopyParms class containing the new model name</param>
        /// <returns>The key of the model if the save was successful</returns>
        public ROOut SaveAsModel(ROModelPropertiesParms parms)
        {
            string message = null;
            bool successful = true;
            bool applyOnly = false;
            string modelName;;
            eROReturnCode returnCode = eROReturnCode.Successful;

            try
            {
                _modelClass = GetModelClass(
                    modelType: parms.ROModelProperties.ModelType,
                    key: parms.ROModelProperties.Model.Key,
                    performingSave: true
                    );
                _currentModelType = _modelClass.ModelType;
                if (!_modelClass.FunctionSecurity.AllowUpdate)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                    return new ROModelPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }
               
                modelName = parms.ROModelProperties.Model.Value;
                if (parms.ROModelProperties.Model.Key == Include.NoRID
                    && _modelClass.ModelNameExists(parms.ROModelProperties.Model.Value))
                {
                    message = "Models must be unique.";
					MIDEnvironment.Message = message;
                    MIDEnvironment.requestFailed = true;
                    returnCode = eROReturnCode.Failure;
                    applyOnly = true;
                }

                parms.ROModelProperties.Model = new KeyValuePair<int, string>(Include.NoRID, modelName);

                ModelProfile mp = _modelClass.ModelUpdateData(modelsProperties: parms.ROModelProperties, cloneDates: true, message: ref message, successful: out successful, applyOnly: applyOnly);

                if (!successful)
                {
                    returnCode = eROReturnCode.Failure;
                }

                // remove undefined model from cache and add new key
				if (parms.ROModelProperties.Model.Key < 0)
                {
                    _modelClasses.Remove(parms.ROModelProperties.Model.Key);
                }

                _modelClasses[mp.Key] = _modelClass;

                // unlock original model and get new model and lock
                ROModelParms getModelParms = _modelClass.GetModelParms(parms: parms, modelType: _currentModelType, key: mp.Key);
                return GetModel(parms: getModelParms);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// copy model data as a new name
        /// </summary>
        /// <param name="parms"> A ROModelParms class containing the model key to be copied</param>
        /// <returns>The key of the model if the save was successful</returns>
        public ROOut CopyModel(ROModelParms parms)
        {
            string message = null;
            bool successful;
            string modelName;
            ROModelParms getModelParms;

            try
            {
                _modelClass = GetModelClass(
                    modelType: parms.ModelType,
                    key: parms.Key
                    );

                if (!_modelClass.FunctionSecurity.AllowUpdate)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                    return new ROModelPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }

                getModelParms = _modelClass.GetModelParms(parms: parms, modelType: parms.ModelType, key: parms.Key, readOnly: true);
                ROModelPropertiesOut modelProperties = (ROModelPropertiesOut)GetModel(parms: getModelParms);

                if (modelProperties.ROModelProperties.Model.Key == Include.NoRID)
                {
                    message = _SAB.ClientServerSession.Audit.GetText(
                    messageCode: eMIDTextCode.msg_ValueWasNotFound,
                    addToAuditReport: true,
                    args: new object[] { MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideModel_Model) }
                    );
                    return new ROModelPropertiesOut(eROReturnCode.Failure, message, _ROInstanceID, null);
                }

                modelName = CleanseModelName(modelProperties.ROModelProperties.Model.Value);

                modelProperties.ROModelProperties.Model = new KeyValuePair<int, string>(Include.NoRID, modelName);

                ModelProfile mp = _modelClass.ModelUpdateData(modelsProperties: modelProperties.ROModelProperties, cloneDates: true, message: ref message, successful: out successful);

                // get new model
                getModelParms = _modelClass.GetModelParms(parms: parms, modelType: _currentModelType, key: mp.Key, readOnly: true);
                return GetModel(parms: getModelParms);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Delete a model
        /// </summary>
        /// <param name="parms">A ROModelParms instance containing the type and key of the model to be deleted</param>
        /// <returns>A flag identifying if the delete was successfule</returns>
        public ROOut DeleteModel(ROModelParms parms)
        {
            string message = null;

            _modelClass = GetModelClass(
                modelType: parms.ModelType,
                key: parms.Key
                );
            if (_currentModelProfile == null
                || _currentModelType != parms.ModelType
                || _currentModelProfile.Key != parms.Key)
            {
                ROModelParms getModelParms = _modelClass.GetModelParms(parms: parms, modelType: parms.ModelType, key: parms.Key);
                if (!InitializeModelData(parms: getModelParms, message: out message))
                {
                    return new ROBoolOut(eROReturnCode.Failure, message, _ROInstanceID, false);
                }
            }

            if (!_modelClass.FunctionSecurity.AllowDelete
                || _currentModelProfile.ModelLockStatus != eLockStatus.Locked
                || !ApplicationUtilities.AllowDeleteFromInUse(key: _currentModelProfile.Key, profileType: GetProfileType(_currentModelType), SAB: _SAB))
            {
                if (!_modelClass.FunctionSecurity.AllowDelete)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_NotAuthorized);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                else if (_currentModelProfile.ModelLockStatus != eLockStatus.Locked)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_DataIsReadOnly);
                    _ROWebTools.LogMessage(eROMessageLevel.Information, message);
                    MIDEnvironment.Message = message;
                }
                return new ROBoolOut(eROReturnCode.Failure, message, parms.ROInstanceID, false);
            }

            bool success = _modelClass.ModelDelete(key: parms.Key, message: ref message);
            if (success)
            {
                _modelClass.UnlockModel(modelType: _currentModelType, key: _currentModelProfile.Key);
                _currentModelProfile = null;
                _currentModelType = eModelType.None;
                _currentLockKey = Include.NoRID;
            }
            else
            {
                MIDEnvironment.requestFailed = true;
                MIDEnvironment.Message = message;
            }

            return new ROBoolOut(eROReturnCode.Successful, null, _ROInstanceID, success);
        }

        /// <summary>
        /// Closes are model resources and removes from the cache
        /// </summary>
        /// <param name="key">The key of the model to be closed</param>
        private void CloseModel(int key)
        {
            ModelBase modelBase = null;
            if (_modelClasses.TryGetValue(key, out modelBase))
            {
                if (modelBase != null
                            && modelBase.CurrentModelProfile != null)
                {
                    if (modelBase.CurrentModelProfile.ModelLockStatus == eLockStatus.Locked)
                    {
                        modelBase.UnlockModel(
                            modelType: modelBase.ModelType,
                            key: modelBase.CurrentModelProfile.Key
                        );
                    }
                    modelBase.OnClosing();

                    _modelClasses.Remove(key: key);
                }
            }
        }

        private string CleanseModelName(string name)
        {
            string newName = name;
            int nameCntr = 0;
            while (true)
            {
                if (!_modelClass.ModelNameExists(newName))
                {
                    break;
                }
                else
                {
                    nameCntr++;
                    newName = Include.GetNewName(name: name, index: nameCntr);
                }
            }

            return newName;
        }

        private ModelBase GetModelClass(
            eModelType modelType,
            int key,
            bool performingSave = false,
            bool addToCache = true
            )
        {
            if (_modelClass != null
                && _modelClass.ModelType == modelType
                && _modelClass.CurrentModelProfile != null
                && (_modelClass.CurrentModelProfile.Key == key || (key == Include.NoRID && performingSave))
                )
            {
                return _modelClass;
            }

            if (!_modelTypes.TryGetValue(modelType, out _modelClasses))
            {
                _modelClasses = new Dictionary<int, ModelBase>();
                _modelTypes.Add(modelType, _modelClasses);
            }

            // will close and remove any previous models of the same type
            // remove this code if multiple models of the same type can be open
            if (_modelClasses.Count > 0
                && addToCache)
            {
                CloseModel(_modelClasses.Keys.First());
                _currentModelProfile = null;
            }

            ModelBase modelBase = null;

            if (!_modelClasses.TryGetValue(key, out modelBase))
            {
                switch (modelType)
                {
                    case eModelType.Eligibility:
                        modelBase = new ModelEligibility(SAB: _SAB, ROWebTools: _ROWebTools);
                        break;
                    case eModelType.StockModifier:
                        modelBase = new ModelStockModifier(SAB: _SAB, ROWebTools: _ROWebTools);
                        break;
                    case eModelType.SalesModifier:
                        modelBase = new ModelSalesModifier(SAB: _SAB, ROWebTools: _ROWebTools);
                        break;
                    case eModelType.OverrideLowLevel:
                        modelBase = new ModelOverrideLowLevel(SAB: _SAB, ROWebTools: _ROWebTools);
                        break;
                    case eModelType.FWOSModifier:
                        modelBase = new ModelFWOSModifier(SAB: _SAB, ROWebTools: _ROWebTools);
                        break;
                    case eModelType.FWOSMax:
                        modelBase = new ModelFWOSMax(SAB: _SAB, ROWebTools: _ROWebTools);
                        break;
                    case eModelType.SizeConstraints:
                        modelBase = new ModelSizeConstraint(SAB: _SAB, ROWebTools: _ROWebTools);
                        break;
                    case eModelType.SizeAlternates:
                        modelBase = new ModelSizeAlternate(SAB: _SAB, ROWebTools: _ROWebTools);
                        break;
                    case eModelType.SizeGroup:
                        modelBase = new ModelSizeGroup(SAB: _SAB, ROWebTools: _ROWebTools);
                        break;
                    case eModelType.SizeCurve:
                        modelBase = new ModelSizeCurveGroup(SAB: _SAB, ROWebTools: _ROWebTools);
                        break;
                }

                if (addToCache)
                {
                    _modelClasses.Add(key, modelBase);
                }
            }

            return modelBase;
        }

        private eProfileType GetProfileType(eModelType modelType)
        {
            switch (modelType)
            {
                case eModelType.Eligibility:
                    return eProfileType.EligibilityModel;
                case eModelType.StockModifier:
                    return eProfileType.StockModifierModel;
                case eModelType.SalesModifier:
                    return eProfileType.SalesModifierModel;
                case eModelType.OverrideLowLevel:
                    return eProfileType.OverrideLowLevelModel;
                case eModelType.FWOSModifier:
                    return eProfileType.FWOSModifierModel;
                case eModelType.FWOSMax:
                    return eProfileType.FWOSMaxModel;
                case eModelType.SizeConstraints:
                    return eProfileType.SizeConstraintModel;
                case eModelType.SizeAlternates:
                    return eProfileType.SizeAlternateModel;
                case eModelType.SizeGroup:
                    return eProfileType.SizeGroup;
                case eModelType.SizeCurve:
                    return eProfileType.SizeCurveGroup;
            }

            return eProfileType.None;
        }

        private eModelType GetModelType(eProfileType profileType)
        {
            switch (profileType)
            {
                case eProfileType.EligibilityModel:
                    return eModelType.Eligibility;
                case eProfileType.StockModifierModel:
                    return eModelType.StockModifier;
                case eProfileType.SalesModifierModel:
                    return eModelType.SalesModifier;
                case eProfileType.OverrideLowLevelModel:
                    return eModelType.OverrideLowLevel;
                case eProfileType.FWOSModifierModel:
                    return eModelType.FWOSModifier;
                case eProfileType.FWOSMaxModel:
                    return eModelType.FWOSMax;
                case eProfileType.SizeConstraintModel:
                    return eModelType.SizeConstraints;
                case eProfileType.SizeAlternateModel:
                    return eModelType.SizeAlternates;
                case eProfileType.SizeGroup:
                    return eModelType.SizeGroup;
                case eProfileType.SizeCurveGroup:
                    return eModelType.SizeCurve;
            }

            return eModelType.None;
        }

    }
}
