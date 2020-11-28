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


        //=============
        // CONSTRUCTORS
        //=============
        public ModelOverrideLowLevel(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.OverrideLowLevel)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsOverrideLowLevels);
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

        override public List<KeyValuePair<int, string>> ModelGetList()
        {
            throw new NotImplementedException("ModelGetList is not implemented");
        }

        override public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false)
        {
            // dummy values for compile purpose only
            KeyValuePair<int, string> model = new KeyValuePair<int, string>(Include.NoRID, string.Empty);
            ROModelOverrideLowLevelsProperties modelProperties = new ROModelOverrideLowLevelsProperties(model: model);


            // populate modelProperties using Windows\OverrideLowLevelModel.cs as a reference

            return modelProperties;
        }

        override public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            successful = true;

            throw new NotImplementedException("ModelUpdateData is not implemented");
        }

        override public bool ModelDelete(int key, ref string message)
        {
            throw new NotImplementedException("ModelDelete is not implemented");
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

    }
}
