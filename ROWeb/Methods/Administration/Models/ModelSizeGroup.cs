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
    public class ModelSizeGroup : ModelBase
    {
        //=======
        // FIELDS
        //=======


        //=============
        // CONSTRUCTORS
        //=============
        public ModelSizeGroup(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.SizeGroup)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeGroups);
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
            ROModelSizeGroupProperties modelProperties = new ROModelSizeGroupProperties(model: model);

            // populate modelProperties using Windows\SizeGroup.cs as a reference

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
            ModelProfile checkExists = SAB.HierarchyServerSession.GetModelData(aModelType: eModelType.SizeGroup, modelID: name);

            return checkExists.Key != Include.NoRID;
        }
    }
}
