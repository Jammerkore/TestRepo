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
    public class ModelOverrideLowLevel : ModelBase
    {
        //=======
        // FIELDS
        //=======
        private OverrideLowLevelProfile _overrideLowLevelProfile = null;
        private bool _copiedToWorkTables = false;
        private int _last_custom_model_RID = Include.NoRID;
        private int _current_Model_Key = 0;
        private Dictionary<int, ROModelOverrideLowLevel> _lowLevelMerchandise;
        private ProfileList _versionProfileList;

        //=============
        // CONSTRUCTORS
        //=============
        public ModelOverrideLowLevel(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.OverrideLowLevel)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsOverrideLowLevels);
            _userSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsUserOverrideLowLevels);
            _globalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsGlobalOverrideLowLevels);
            _versionProfileList = SAB.ClientServerSession.GetUserForecastVersions();
        }

        //===========
        // PROPERTIES
        //===========



        //========
        // METHODS
        //========

        override public bool OnClosing()
        {
            if (_overrideLowLevelProfile != null)
            {
                _overrideLowLevelProfile.DeleteModelDetailsWork(_overrideLowLevelProfile.Key);
            }
            return true;
        }

        override public List<KeyValuePair<int, string>> ModelGetList()
        {
            List<KeyValuePair<int, string>> keyValueList = new List<KeyValuePair<int, string>>();

            ProfileList overrideProfiles = OverrideLowLevelProfile.LoadAllProfiles(
                _last_custom_model_RID,
                SAB.ClientServerSession.UserRID,
                GlobalSecurity.AllowView,
                UserSecurity.AllowView,
                _last_custom_model_RID
                );

            foreach (OverrideLowLevelProfile overrideLowLevelProfile in overrideProfiles)
            {
                string name = overrideLowLevelProfile.Name;
                switch (overrideLowLevelProfile.User_RID)
                {
                    case Include.GlobalUserRID:
                        break;
                    case Include.CustomUserRID:
                        name = name + " (Custom)";
                        break;
                    default:
                        name = name + " (" + UserNameStorage.GetUserName(overrideLowLevelProfile.User_RID) + ")";
                        break;
                }
                keyValueList.Add(new KeyValuePair<int, string>(overrideLowLevelProfile.Key, name));
            }

            return keyValueList;
        }

        override public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false)
        {
            // first call
            if (_current_Model_Key == 0)
            {
                _current_Model_Key = parms.Key;
            }
            // changed model
            else if(_overrideLowLevelProfile != null
                && _current_Model_Key != parms.Key)
            {
                _overrideLowLevelProfile.DeleteModelWork(_overrideLowLevelProfile.Key);
                _copiedToWorkTables = false;
                _current_Model_Key = parms.Key;
            }

            // populate modelProperties using Windows\OverrideLowLevelModel.cs as a reference
            _overrideLowLevelProfile = (OverrideLowLevelProfile)modelProfile;

            // update the custom model key if opening a custom model
            if (_overrideLowLevelProfile.User_RID == Include.CustomUserRID)
            {
                _last_custom_model_RID = _overrideLowLevelProfile.Key;
            }

            // Data must be copied to work tables before it can be properly accessed
            if (_overrideLowLevelProfile.Key != Include.NoRID
                && !_copiedToWorkTables)
            {
                _overrideLowLevelProfile.CopyToWorkTables(_overrideLowLevelProfile.Key);
                _copiedToWorkTables = true;
            }

            string name = _overrideLowLevelProfile.Name == null ? string.Empty : _overrideLowLevelProfile.Name;

            KeyValuePair<int, string> model = new KeyValuePair<int, string>(_current_Model_Key, name);
            ROModelOverrideLowLevelsProperties modelProperties = new ROModelOverrideLowLevelsProperties(model: model);

            FillOverrideModel(
                modelProperties: modelProperties,
                message: ref message
                );

            return modelProperties;
        }

        private void FillOverrideModel(
            ROModelOverrideLowLevelsProperties modelProperties, 
            ref string message
            )
        {
            int nodeKey;

            modelProperties.ActiveOnly = _overrideLowLevelProfile.ActiveOnlyInd;

            if (_overrideLowLevelProfile.HN_RID != Include.NoRID)
            {
                modelProperties.Merchandise = GetName.GetMerchandiseName(nodeRID: _overrideLowLevelProfile.HN_RID,
                    SAB: SAB);

                if (_overrideLowLevelProfile.HighLevelType == eHighLevelsType.None)
                {
                    HierarchyNodeProfile hierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(nodeRID: _overrideLowLevelProfile.HN_RID);
                    if (hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
                    {
                        _overrideLowLevelProfile.HighLevelType = eHighLevelsType.HierarchyLevel;
                        _overrideLowLevelProfile.HighLevelSeq = hierarchyNodeProfile.HomeHierarchyLevel;
                        _overrideLowLevelProfile.HighLevelOffset = 0;
                    }
                    else
                    {
                        _overrideLowLevelProfile.HighLevelType = eHighLevelsType.LevelOffset;
                        _overrideLowLevelProfile.HighLevelSeq = 0;
                        _overrideLowLevelProfile.HighLevelOffset = 0;
                    }
                }

                ROLevelInformation highLevel = new ROLevelInformation();
                highLevel.LevelType = (eROLevelsType)_overrideLowLevelProfile.HighLevelType;
                highLevel.LevelOffset = _overrideLowLevelProfile.HighLevelOffset;
                highLevel.LevelSequence = _overrideLowLevelProfile.HighLevelSeq;
                highLevel.LevelValue = GetName.GetLevelName(
                       levelType: (eROLevelsType)_overrideLowLevelProfile.HighLevelType,
                       levelSequence: _overrideLowLevelProfile.HighLevelSeq,
                       levelOffset: _overrideLowLevelProfile.HighLevelOffset,
                       SAB: SAB
                       );
                modelProperties.HighLevel = highLevel;

                if (_overrideLowLevelProfile.High_Level_HN_RID != Include.NoRID)
                {
                    modelProperties.HighLevelMerchandise = GetName.GetMerchandiseName(nodeRID: _overrideLowLevelProfile.High_Level_HN_RID,
                        SAB: SAB);
                }

                ROLevelInformation lowLevel = new ROLevelInformation();
                lowLevel.LevelType = (eROLevelsType)_overrideLowLevelProfile.LowLevelType;
                lowLevel.LevelOffset = _overrideLowLevelProfile.LowLevelOffset;
                lowLevel.LevelSequence = _overrideLowLevelProfile.LowLevelSeq;
                lowLevel.LevelValue = GetName.GetLevelName(
                       levelType: (eROLevelsType)_overrideLowLevelProfile.LowLevelType,
                       levelSequence: _overrideLowLevelProfile.LowLevelSeq,
                       levelOffset: _overrideLowLevelProfile.LowLevelOffset,
                       SAB: SAB
                       );
                modelProperties.LowLevel = lowLevel;

                eMerchandiseType highLevelType = modelProperties.HighLevelType;
                eMerchandiseType lowLevelType = modelProperties.LowLevelType;
                nodeKey = _overrideLowLevelProfile.HN_RID;

                // build the high and low level lists based on the selected merchandise
                HierarchyTools.BuildLowLevelLists(
                    sessionAddressBlock: SAB,
                    hierarchyNodeRID: nodeKey,
                    fromLevels: modelProperties.HighLevels,
                    fromMerchandiseType: ref highLevelType,
                    toLevels: modelProperties.LowLevels,
                    toMerchandiseType: ref lowLevelType
                    );

                // adjust the high and low level lists along with the to level based on the selected high level
                HierarchyTools.AdjustLevelLists(
                    sessionAddressBlock: SAB,
                    fromLevel: ref highLevel,
                    fromLevels: modelProperties.HighLevels,
                    fromMerchandiseType: ref highLevelType,
                    toLevel: ref lowLevel,
                    toLevels: modelProperties.LowLevels,
                    toMerchandiseType: ref lowLevelType
                    );

                // Verify highLevel is in list
                // If not, set to first level in list
                if (highLevelType == eMerchandiseType.HierarchyLevel)
                {
                    if (!modelProperties.HighLevels.Any(item => item.Key == modelProperties.HighLevel.LevelSequence))
                    {
                        modelProperties.HighLevel.LevelSequence = modelProperties.HighLevels[0].Key;
                        modelProperties.HighLevel.LevelType = eROLevelsType.HierarchyLevel;
                    }
                }
                else
                {
                    if (!modelProperties.HighLevels.Any(item => item.Key == modelProperties.HighLevel.LevelOffset))
                    {
                        modelProperties.HighLevel.LevelOffset = modelProperties.HighLevels[0].Key;
                        modelProperties.HighLevel.LevelType = eROLevelsType.LevelOffset;
                    }
                }

                modelProperties.HighLevelType = highLevelType;
                modelProperties.HighLevel = highLevel;
                _overrideLowLevelProfile.HighLevelType = HierarchyTools.ConvertToHighLevelsType(levelType: modelProperties.HighLevelType);
                _overrideLowLevelProfile.HighLevelOffset = modelProperties.HighLevel.LevelOffset;
                _overrideLowLevelProfile.HighLevelSeq = modelProperties.HighLevel.LevelSequence;

                // Verify lowLevel is in list
                // If not, set to first level in list
                if (lowLevelType == eMerchandiseType.HierarchyLevel)
                {
                    if (!modelProperties.LowLevels.Any(item => item.Key == modelProperties.LowLevel.LevelSequence))
                    {
                        modelProperties.LowLevel.LevelSequence = modelProperties.LowLevels[0].Key;
                        modelProperties.LowLevel.LevelType = eROLevelsType.HierarchyLevel;
                    }
                }
                else
                {
                    if (!modelProperties.LowLevels.Any(item => item.Key == modelProperties.LowLevel.LevelOffset))
                    {
                        modelProperties.LowLevel.LevelOffset = modelProperties.LowLevels[0].Key;
                        modelProperties.LowLevel.LevelType = eROLevelsType.LevelOffset;
                    }
                }

                modelProperties.LowLevelType = lowLevelType;
                modelProperties.LowLevel = lowLevel;
                _overrideLowLevelProfile.LowLevelType = HierarchyTools.ConvertToLowLevelsType(levelType: modelProperties.LowLevelType);
                _overrideLowLevelProfile.LowLevelOffset = modelProperties.LowLevel.LevelOffset;
                _overrideLowLevelProfile.LowLevelSeq = modelProperties.LowLevel.LevelSequence;

                LoadHighLevelMerchandise(
                    modelProperties: modelProperties,
                    message: ref message
                );

                LoadLowLevelMerchandise(
                    modelProperties: modelProperties,
                    message: ref message
                ); 
            }

            LoadVersionList(
                modelProperties: modelProperties,
                message: ref message
            );
        }

        private void LoadHighLevelMerchandise(
            ROModelOverrideLowLevelsProperties modelProperties, 
            ref string message
            )
        {
            LowLevelVersionOverrideProfileList overrideList = null;
            HierarchySessionTransaction hierarchyTransaction = new HierarchySessionTransaction(this.SAB);

            int offset = 0;  // this is the offset in the high level list

            foreach (KeyValuePair<int, string> highLevelMerchandise in modelProperties.HighLevels)
            {
                if (modelProperties.HighLevel.LevelType == eROLevelsType.HierarchyLevel)
                {
                    if (highLevelMerchandise.Key == modelProperties.HighLevel.LevelSequence)
                    {
                        break;
                    }
                }
                else if (modelProperties.HighLevel.LevelType == eROLevelsType.LevelOffset)
                {
                    if (highLevelMerchandise.Key == modelProperties.HighLevel.LevelOffset)
                    {
                        break;
                    }
                }

                ++offset;

            }

            if (offset > 0)
            {
                overrideList = hierarchyTransaction.GetOverrideList(modelProperties.Model.Key, _overrideLowLevelProfile.HN_RID, Include.NoRID,
                                                                               offset, _overrideLowLevelProfile.HN_RID, true, false, true);
            }

            if (overrideList != null &&
                overrideList.ArrayList.Count > 0)
            {
                foreach (LowLevelVersionOverrideProfile llvop in overrideList)
                {
                    if (!llvop.Exclude)
                    {
                        modelProperties.HighLevelMerchandiseList.Add(GetName.GetMerchandiseName(nodeRID: (int)llvop.NodeProfile.Key,
                                SAB: SAB));
                    }
                }
            }
            else
            {
                modelProperties.HighLevelMerchandiseList.Add(GetName.GetMerchandiseName(nodeRID: _overrideLowLevelProfile.HN_RID,
                    SAB: SAB));
            }
        }

        private void LoadLowLevelMerchandise(
            ROModelOverrideLowLevelsProperties modelProperties,
            ref string message
            )
        {
            LowLevelVersionOverrideProfileList overrideList = null;
            HierarchySessionTransaction hierarchyTransaction = new HierarchySessionTransaction(this.SAB);

            int highLevelOffset = 0;  // this is the offset in the high level list

            foreach (KeyValuePair<int, string> highLevelMerchandise in modelProperties.HighLevels)
            {
                if (modelProperties.HighLevel.LevelType == eROLevelsType.HierarchyLevel)
                {
                    if (highLevelMerchandise.Key == modelProperties.HighLevel.LevelSequence)
                    {
                        break;
                    }
                }
                else if (modelProperties.HighLevel.LevelType == eROLevelsType.LevelOffset)
                {
                    if (highLevelMerchandise.Key == modelProperties.HighLevel.LevelOffset)
                    {
                        break;
                    }
                }

                ++highLevelOffset;

            }

            int lowLevelOffset = 0;  // this is the offset in the high level list

            foreach (KeyValuePair<int, string> lowLevelMerchandise in modelProperties.LowLevels)
            {
                if (modelProperties.LowLevel.LevelType == eROLevelsType.HierarchyLevel)
                {
                    if (lowLevelMerchandise.Key == modelProperties.LowLevel.LevelSequence)
                    {
                        break;
                    }
                }
                else if (modelProperties.HighLevel.LevelType == eROLevelsType.LevelOffset)
                {
                    if (lowLevelMerchandise.Key == modelProperties.LowLevel.LevelOffset)
                    {
                        break;
                    }
                }

                ++lowLevelOffset;

            }

            overrideList = hierarchyTransaction.GetOverrideList(modelProperties.Model.Key,
                                                                  _overrideLowLevelProfile.HN_RID,
                                                                  Include.NoRID,
                                                                  lowLevelOffset + highLevelOffset + 1,
                                                                  _overrideLowLevelProfile.High_Level_HN_RID,
                                                                  true,
                                                                  false,
                                                                  true);


            if (overrideList.Count > 500)
            {
                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoItemsExceedsMaximum, overrideList.Count.ToString());
                return;
            }

            // keep low level setting to compare for changes
            _lowLevelMerchandise = new Dictionary<int, ROModelOverrideLowLevel>();

            foreach (LowLevelVersionOverrideProfile llvop in overrideList)
            {
                if (llvop.NodeProfile != null)
                {
                    HierarchyNodeProfile hnp = llvop.NodeProfile;

                    if ((hnp.LevelType == eHierarchyLevelType.Color) && (hnp.ColorOrSizeCodeRID == 0))
                    {
                        // Through Out "UNKNOWN Color" (don't add this record)
                    }
                    else
                    {
                        ROModelOverrideLowLevel lowLevel = new ROModelOverrideLowLevel(
                            merchandise: GetName.GetMerchandiseName(nodeRID: hnp.Key,
                                                                    SAB: SAB),
                            version: GetName.GetVersion(versionRID: llvop.VersionProfile.Key,
                                                                    SAB: SAB),
                            exclude: llvop.Exclude,
                            inactive: !hnp.Active);

                        modelProperties.LowLevelMerchandise.Add(lowLevel);
                        _lowLevelMerchandise.Add(hnp.Key, lowLevel);
                    }
                }
            }

            modelProperties.LowLevelMerchandise.Sort();
        }

        private void LoadVersionList(
            ROModelOverrideLowLevelsProperties modelProperties,
            ref string message
            )
        {
            modelProperties.Versions.Add(new KeyValuePair<int, string>(Include.NoRID, "Default Version"));
            foreach (VersionProfile versionProfile in _versionProfileList)
            {
                modelProperties.Versions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }
        }

        override public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;

            ROModelOverrideLowLevelsProperties lowLevelsProperties = (ROModelOverrideLowLevelsProperties)modelsProperties;
            int lowLevelsModelRid = modelsProperties.Model.Key;
            string saveName = modelsProperties.Model.Value.ToString();

            bool continueSave = false;

            try
            {
                //if either new model or copy as model
                if (lowLevelsModelRid == Include.NoRID)
                {
                    bool checkExists = GetLowLevelModel(saveName);
                    if (!checkExists) //new model name does not exist
                    {
                        _overrideLowLevelProfile.Key = lowLevelsModelRid;
                        _overrideLowLevelProfile.ModelID = saveName;
                        _overrideLowLevelProfile.Name = saveName;
                        _overrideLowLevelProfile.ModelChangeType = eChangeType.add;
                        continueSave = true;
                    }
                    else //new model name does exist
                    {
                        message = eMIDTextCode.msg_DuplicateName.ToString();
                        continueSave = false;
                        successful = false;
                    }
                }
                //model is update of existing 
                else
                {
                    //current model equals saveing model
                    if (_overrideLowLevelProfile.Key == lowLevelsModelRid)
                    {
                        _overrideLowLevelProfile.Key = lowLevelsModelRid;
                        _overrideLowLevelProfile.ModelID = saveName;
                        _overrideLowLevelProfile.Name = saveName;
                        _overrideLowLevelProfile.ModelChangeType = eChangeType.update;
                        continueSave = true;
                    }
                    else
                    {
                        message = eMIDTextCode.msg_SaveCanceled.ToString();
                        continueSave = false;
                        successful = false;
                    }
                }

                if (continueSave)
                {
                    //move changes from properties to model profile
                    SaveToProfile(lowLevelsProperties);

                    if (!string.IsNullOrEmpty(_overrideLowLevelProfile.Name))
                    {
                        // Save file low level override model data
                        if (!applyOnly)
                        {
                            _current_Model_Key = _overrideLowLevelProfile.CopyToPermanentTable(_overrideLowLevelProfile.Key);
                        }
                        if (successful)
                        {
                            message = eMIDTextCode.msg_ProcessCompletedSuccessfully.ToString();
                        }
                    }
                    else if (!applyOnly)
                    {
                        message = eMIDTextCode.msg_NameRequiredToSave.ToString();
                        successful = false;
                    }
                }
                else
                {
                    message = eMIDTextCode.msg_SaveCanceled.ToString();
                    successful = false;
                }
            }
            catch (Exception exception)
            {
                throw (exception);
            }
            finally
            {

            }

            _copiedToWorkTables = true;

            return _overrideLowLevelProfile;
        }

        private bool GetLowLevelModel(string modelName)
        {
            return _overrideLowLevelProfile.ModelNameExists(modelName, Include.GlobalUserRID);
        }

        private void SaveToProfile(ROModelOverrideLowLevelsProperties lowLevelsProperties)
        {
            // if merchandise is changed, remove rows from the work tables
            if (_overrideLowLevelProfile.HN_RID != Include.NoRID
                && _overrideLowLevelProfile.HN_RID != lowLevelsProperties.Merchandise.Key)
            {
                _overrideLowLevelProfile.DeleteModelWork(_overrideLowLevelProfile.Key);
                _overrideLowLevelProfile.HN_RID = lowLevelsProperties.Merchandise.Key;
                _overrideLowLevelProfile.High_Level_HN_RID = lowLevelsProperties.Merchandise.Key;
                // clear level information so the first entry will be used
                _overrideLowLevelProfile.HighLevelType = eHighLevelsType.None;
                _overrideLowLevelProfile.HighLevelSeq = 0;
                _overrideLowLevelProfile.HighLevelOffset = 0;
                _overrideLowLevelProfile.LowLevelType = eLowLevelsType.None;
                _overrideLowLevelProfile.LowLevelSeq = 0;
                _overrideLowLevelProfile.LowLevelOffset = 0;
                lowLevelsProperties.HighLevel = null;
                lowLevelsProperties.LowLevel = null;
            }
            else
            {
                if (_overrideLowLevelProfile.HN_RID != lowLevelsProperties.Merchandise.Key)
                {
                    lowLevelsProperties.HighLevel = null;
                    lowLevelsProperties.LowLevel = null;
                }
                _overrideLowLevelProfile.HN_RID = lowLevelsProperties.Merchandise.Key;
                if (lowLevelsProperties.HighLevelMerchandiseIsSet)
                {
                    _overrideLowLevelProfile.High_Level_HN_RID = lowLevelsProperties.HighLevelMerchandise.Key;
                }
                else
                {
                    _overrideLowLevelProfile.High_Level_HN_RID = lowLevelsProperties.Merchandise.Key;
                }
                _overrideLowLevelProfile.ActiveOnlyInd = lowLevelsProperties.ActiveOnly;
                if (_overrideLowLevelProfile.User_RID == Include.NoRID)
                {
                    _overrideLowLevelProfile.User_RID = lowLevelsProperties.UserKey;
                }
                if (lowLevelsProperties.HighLevel != null)
                {
                    _overrideLowLevelProfile.HighLevelSeq = lowLevelsProperties.HighLevel.LevelSequence;
                    _overrideLowLevelProfile.HighLevelOffset = lowLevelsProperties.HighLevel.LevelOffset;
                    _overrideLowLevelProfile.HighLevelType = (eHighLevelsType)lowLevelsProperties.HighLevel.LevelType;
                }
                else if (_overrideLowLevelProfile.HN_RID > 0)
                {
                    HierarchyNodeProfile hierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(nodeRID: _overrideLowLevelProfile.HN_RID);
                    if (hierarchyNodeProfile.HomeHierarchyType == eHierarchyType.organizational)
                    {
                        _overrideLowLevelProfile.HighLevelType = eHighLevelsType.HierarchyLevel;
                        _overrideLowLevelProfile.HighLevelSeq = hierarchyNodeProfile.HomeHierarchyLevel;
                        _overrideLowLevelProfile.HighLevelOffset = 0;
                    }
                    else
                    {
                        _overrideLowLevelProfile.HighLevelType = eHighLevelsType.LevelOffset;
                        _overrideLowLevelProfile.HighLevelSeq = 0;
                        _overrideLowLevelProfile.HighLevelOffset = 0;
                    }
                }
                if (lowLevelsProperties.LowLevel != null)
                {
                    _overrideLowLevelProfile.LowLevelSeq = lowLevelsProperties.LowLevel.LevelSequence;
                    _overrideLowLevelProfile.LowLevelOffset = lowLevelsProperties.LowLevel.LevelOffset;
                    _overrideLowLevelProfile.LowLevelType = (eLowLevelsType)lowLevelsProperties.LowLevel.LevelType;
                }

                ROModelOverrideLowLevel lowLevel;
                foreach (ROModelOverrideLowLevel lowLevelMerchandise in lowLevelsProperties.LowLevelMerchandise)
                {
                    lowLevel = _lowLevelMerchandise[lowLevelMerchandise.Merchandise.Key];
                    bool originalVersionChanged = false;
                    if (lowLevel != null
                        && lowLevelMerchandise.Version.Key != lowLevel.Version.Key)
                    {
                        originalVersionChanged = true;
                    }

                    bool originalExcludeChanged = false;
                    if (lowLevel != null
                        && lowLevelMerchandise.Exclude != lowLevel.Exclude)
                    {
                        originalExcludeChanged = true;
                    }

                    if (originalVersionChanged || originalExcludeChanged)
                    {
                        OverrideLowLevelDetailProfile overrideLowLevelDetailProfile = new OverrideLowLevelDetailProfile(Include.NoRID);
                        string productId = lowLevelMerchandise.Merchandise.Value;
                        overrideLowLevelDetailProfile.Key = lowLevelMerchandise.Merchandise.Key;
                        overrideLowLevelDetailProfile.Model_RID = lowLevelsProperties.Model.Key;
                        overrideLowLevelDetailProfile.Version_RID = lowLevelMerchandise.Version.Key;
                        overrideLowLevelDetailProfile.Exclude_Ind = lowLevelMerchandise.Exclude;

                        _overrideLowLevelProfile.DetailList.Add(overrideLowLevelDetailProfile);
                    }
                }

                _overrideLowLevelProfile.WriteProfileWork(ref _last_custom_model_RID, SAB.ClientServerSession.UserRID);
            }
        }

        override public bool ModelDelete(int key, ref string message)
        {
            try
            {
                _overrideLowLevelProfile.DeleteModel(key);
                _overrideLowLevelProfile.DeleteModelWork(key);
            }
            catch (DatabaseForeignKeyViolation)
            {
                message = eMIDTextCode.msg_DeleteFailedDataInUse.ToString();
                return false;

            }
            catch (Exception exception)
            {
                message = exception.Message.ToString();
                return false;
            }
            finally
            {
                message = eMIDTextCode.msg_DeleteSuccessfulWithValue.ToString();
            }

            return true;
        }

        override public bool ModelNameExists(string name)
        {
            ModelsData modelsData = new ModelsData();
            return modelsData.OverrideLowLevelsModelName_Exist(aModelName: name, aUserRID: Include.GlobalUserRID);
        }

        override public ModelProfile GetModelProfile(ROModelParms parms)
        {
            string message;
            // Override Low Level Model is different from other models in flow so will need to be handled differently
            ModelProfile modelProfile = null;

            if (parms.ReadOnly
                || !FunctionSecurity.AllowUpdate)
            {
                modelProfile = new OverrideLowLevelProfile(parms.Key);
            }
            else
            {
                modelProfile = new OverrideLowLevelProfile(parms.Key);
                modelProfile.ModelLockStatus = LockModel(modelType: parms.ModelType, key: parms.Key, name: modelProfile.ModelID, allowReadOnly: true, message: out message);
                if (modelProfile.ModelLockStatus == eLockStatus.ReadOnly)
                {
                    MIDEnvironment.isChangedToReadOnly = true;
                    MIDEnvironment.Message = message;
                }
            }

            return modelProfile;
        }

        override public ROModelParms GetModelParms(ROModelPropertiesParms parms, eModelType modelType, int key, bool readOnly = false)
        {
            ROModelParms modelParms = new ROModelParms(sROUserID: parms.ROUserID,
                sROSessionID: parms.ROSessionID,
                ROClass: parms.ROClass,
                RORequest: eRORequest.GetModel,
                ROInstanceID: parms.ROInstanceID,
                modelType: modelType,
                key: _current_Model_Key,
                readOnly: readOnly
                );

            return modelParms;
        }

    }
}
