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
    public class ModelStockModifier : ModelBase
    {
        //=======
        // FIELDS
        //=======


        //=============
        // CONSTRUCTORS
        //=============
        public ModelStockModifier(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.StockModifier)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsStockModifier);
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
            return ConvertProfileListToList(SAB.HierarchyServerSession.GetStkModModels());
        }

        override public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false)
        {
            // populate modelProperties using Windows\StockModifierModelMaint.cs as a reference

            ROCalendarDateInfo calendarDateInfo;
            StkModModelProfile smmp = (StkModModelProfile)modelProfile;

            KeyValuePair<int, string> model = new KeyValuePair<int, string>(key: smmp.Key, value: smmp.ModelID);
            ROModelStockModifierProperties modelProperties = new ROModelStockModifierProperties(model: model);


            modelProperties.ModelValuesProperties = new ROModelValuesProperties(smmp.StkModModelDefault);
            foreach (StkModModelEntry smme in smmp.ModelEntries)
            {
                calendarDateInfo = new ROCalendarDateInfo(smme.DateRange.Key, smme.DateRange.DisplayDate, smme.DateRange.DateRangeType);
                modelProperties.ModelValuesProperties.ModelValues.Add(new ROModelValue(modelValue: smme.StkModModelEntryValue, calendarDateInfo: calendarDateInfo));
            }

            return modelProperties;
        }

        override public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            StkModModelEntry modelEntry;
            int sequence;
            successful = true;

            ROModelStockModifierProperties stockModifierProperties = (ROModelStockModifierProperties)modelsProperties;

            StkModModelProfile smmp = new StkModModelProfile(modelsProperties.Model.Key);

            if (modelsProperties.Model.Key == Include.NoRID)
            {
                smmp.ModelChangeType = eChangeType.add;
            }
            else
            {
                smmp.ModelChangeType = eChangeType.update;
            }

            smmp.StkModModelDefault = stockModifierProperties.ModelValuesProperties.DefaultValue;

            sequence = 0;
            foreach (ROModelValue modelValue in stockModifierProperties.ModelValuesProperties.ModelValues)
            {
                modelEntry = new StkModModelEntry();
                modelEntry.StkModModelEntryValue = modelValue.ModelValue;
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
                smmp.ModelEntries.Add(modelEntry);

                ++sequence;
            }


            smmp.ModelID = modelsProperties.Model.Value;
            smmp.Key = modelsProperties.Model.Key;

            if (!applyOnly)
            {
                smmp.Key = SAB.HierarchyServerSession.StkModModelUpdate(smmp);
            }

            // set lock status to maintain lock.
            smmp.ModelLockStatus = eLockStatus.Locked;

            return smmp;
        }

        override public bool ModelDelete(int key, ref string message)
        {
            message = null;
            StkModModelProfile smmp = new StkModModelProfile(key);
            smmp.ModelChangeType = eChangeType.delete;
            smmp.Key = key;
            SAB.HierarchyServerSession.StkModModelUpdate(smmp);

            return true;
        }

        override public bool ModelNameExists(string name, int userKey)
        {
            ModelProfile checkExists = SAB.HierarchyServerSession.GetModelData(aModelType: eModelType.StockModifier, modelID: name);

            return checkExists.Key != Include.NoRID;
        }
    }
}
