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
    public class ModelFWOSMax : ModelBase
    {
        //=======
        // FIELDS
        //=======


        //=============
        // CONSTRUCTORS
        //=============
        public ModelFWOSMax(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.FWOSMax)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminFWOSMax);
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
            return ConvertProfileListToList(SAB.HierarchyServerSession.GetFWOSMaxModels());
        }

        override public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false)
        {
            // populate modelProperties using Windows\FWOSMaxModelMaint.cs as a reference

            ROCalendarDateInfo calendarDateInfo;
            FWOSMaxModelProfile FWOSmmp = (FWOSMaxModelProfile)modelProfile;

            KeyValuePair<int, string> model = new KeyValuePair<int, string>(key: FWOSmmp.Key, value: FWOSmmp.ModelID);
            ROModelFWOSMaxProperties modelProperties = new ROModelFWOSMaxProperties(model: model);


            modelProperties.ModelValuesProperties = new ROModelValuesProperties(FWOSmmp.FWOSMaxModelDefault);
            foreach (FWOSMaxModelEntry smme in FWOSmmp.ModelEntries)
            {
                calendarDateInfo = new ROCalendarDateInfo(smme.DateRange.Key, smme.DateRange.DisplayDate, smme.DateRange.DateRangeType);
                modelProperties.ModelValuesProperties.ModelValues.Add(new ROModelValue(modelValue: smme.FWOSMaxModelEntryValue, calendarDateInfo: calendarDateInfo));
            }


            return modelProperties;
        }

        override public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            FWOSMaxModelEntry modelEntry;
            int sequence;
            successful = true;

            ROModelFWOSMaxProperties fwosMaxProperties = (ROModelFWOSMaxProperties)modelsProperties;

            FWOSMaxModelProfile FWOSmmp = new FWOSMaxModelProfile(modelsProperties.Model.Key);

            if (modelsProperties.Model.Key == Include.NoRID)
            {
                FWOSmmp.ModelChangeType = eChangeType.add;
            }
            else
            {
                FWOSmmp.ModelChangeType = eChangeType.update;
            }

            FWOSmmp.FWOSMaxModelDefault = fwosMaxProperties.ModelValuesProperties.DefaultValue;

            sequence = 0;
            foreach (ROModelValue modelValue in fwosMaxProperties.ModelValuesProperties.ModelValues)
            {
                modelEntry = new FWOSMaxModelEntry();
                modelEntry.FWOSMaxModelEntryValue = modelValue.ModelValue;
                if (modelValue.CalendarDateInfo.Key != Include.NoRID)
                {
                    if (cloneDates)
                    {
                        modelEntry.DateRange = SAB.ClientServerSession.Calendar.GetDateRangeClone(modelValue.CalendarDateInfo.Key);
                        modelEntry.DateRange.DisplayDate = modelValue.CalendarDateInfo.DisplayDate;
                    }
                    else
                    {
                        modelEntry.DateRange = SAB.ClientServerSession.Calendar.GetDateRange(modelValue.CalendarDateInfo.Key);
                        modelEntry.DateRange.DisplayDate = modelValue.CalendarDateInfo.DisplayDate;
                    }
                }
                else
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                    continue;
                }
                modelEntry.ModelEntrySeq = sequence;
                modelEntry.ModelEntryChangeType = eChangeType.add;
                FWOSmmp.ModelEntries.Add(modelEntry);

                ++sequence;
            }


            FWOSmmp.ModelID = modelsProperties.Model.Value;
            FWOSmmp.Key = modelsProperties.Model.Key;

            if (!applyOnly)
            {
                FWOSmmp.Key = SAB.HierarchyServerSession.FWOSMaxModelUpdate(FWOSmmp);
            }

            // set lock status to maintain lock.
            FWOSmmp.ModelLockStatus = eLockStatus.Locked;

            return FWOSmmp;
        }

        override public bool ModelDelete(int key, ref string message)
        {
            message = null;
            FWOSMaxModelProfile FWOSmmp = new FWOSMaxModelProfile(key);
            FWOSmmp.ModelChangeType = eChangeType.delete;
            FWOSmmp.Key = key;
            SAB.HierarchyServerSession.FWOSMaxModelUpdate(FWOSmmp);

            return true;
        }

        override public bool ModelNameExists(string name, int userKey)
        {
            ModelProfile checkExists = SAB.HierarchyServerSession.GetModelData(aModelType: eModelType.FWOSMax, modelID: name);

            return checkExists.Key != Include.NoRID;
        }
    }
}
