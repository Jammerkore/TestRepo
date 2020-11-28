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
    public class ModelSalesModifier : ModelBase
    {
        //=======
        // FIELDS
        //=======


        //=============
        // CONSTRUCTORS
        //=============
        public ModelSalesModifier(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.SalesModifier)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsSalesModifier);
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
            return ConvertProfileListToList(SAB.HierarchyServerSession.GetSlsModModels());
        }

        override public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false)
        {
            // populate modelProperties using Windows\SalesModifierModelMaint.cs as a reference

            ROCalendarDateInfo calendarDateInfo;
            SlsModModelProfile smmp = (SlsModModelProfile)modelProfile;

            KeyValuePair<int, string> model = new KeyValuePair<int, string>(key: smmp.Key, value: smmp.ModelID);
            ROModelSalesModifierProperties modelProperties = new ROModelSalesModifierProperties(model: model);


            modelProperties.ModelValuesProperties = new ROModelValuesProperties(smmp.SlsModModelDefault);
            foreach (SlsModModelEntry smme in smmp.ModelEntries)
            {
                calendarDateInfo = new ROCalendarDateInfo(smme.DateRange.Key, smme.DateRange.DisplayDate, smme.DateRange.DateRangeType);
                modelProperties.ModelValuesProperties.ModelValues.Add(new ROModelValue(modelValue: smme.SlsModModelEntryValue, calendarDateInfo: calendarDateInfo));
            }

            
            return modelProperties;
        }

        override public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            SlsModModelEntry modelEntry;
            int sequence;
            successful = true;

            ROModelSalesModifierProperties salesModifierProperties = (ROModelSalesModifierProperties)modelsProperties;

            SlsModModelProfile smmp = new SlsModModelProfile(modelsProperties.Model.Key);

            if (modelsProperties.Model.Key == Include.NoRID)
            {
                smmp.ModelChangeType = eChangeType.add;
            }
            else
            {
                smmp.ModelChangeType = eChangeType.update;
            }

            smmp.SlsModModelDefault = salesModifierProperties.ModelValuesProperties.DefaultValue;

            sequence = 0;
            foreach (ROModelValue modelValue in salesModifierProperties.ModelValuesProperties.ModelValues)
            {
                modelEntry = new SlsModModelEntry();
                modelEntry.SlsModModelEntryValue = modelValue.ModelValue;
                if (modelValue.CalendarDateInfo.Key != Include.NoRID)
                {
                    if (cloneDates)
                    {
                        modelEntry.DateRange = SAB.ClientServerSession.Calendar.GetDateRangeClone(modelValue.CalendarDateInfo.Key);
                    }
                    else
                    {
                        modelEntry.DateRange = SAB.ClientServerSession.Calendar.GetDateRange(modelValue.CalendarDateInfo.Key);
                    }
                }
                else
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                    continue;
                }
                modelEntry.ModelEntrySeq = sequence;
                modelEntry.ModelEntryChangeType = eChangeType.add;
                smmp.ModelEntries.Add(modelEntry);

                ++sequence;
            }


            smmp.ModelID = modelsProperties.Model.Value;
            smmp.Key = modelsProperties.Model.Key;

            if (!applyOnly)
            {
                smmp.Key = SAB.HierarchyServerSession.SlsModModelUpdate(smmp);
            }

            // set lock status to maintain lock.
            smmp.ModelLockStatus = eLockStatus.Locked;

            return smmp;
        }

        override public bool ModelDelete(int key, ref string message)
        {
            message = null;
            SlsModModelProfile smmp = new SlsModModelProfile(key);
            smmp.ModelChangeType = eChangeType.delete;
            smmp.Key = key ;
            SAB.HierarchyServerSession.SlsModModelUpdate(smmp);

            return true;
        }

        override public bool ModelNameExists(string name)
        {
            ModelProfile checkExists = SAB.HierarchyServerSession.GetModelData(aModelType: eModelType.SalesModifier, modelID: name);

            return checkExists.Key != Include.NoRID;
        }
    }
}
