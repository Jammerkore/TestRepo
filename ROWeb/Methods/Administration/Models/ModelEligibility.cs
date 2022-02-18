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
    public class ModelEligibility : ModelBase
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============
        public ModelEligibility(SessionAddressBlock SAB, ROWebTools ROWebTools) :
            base (SAB: SAB, ROWebTools: ROWebTools, modelType: eModelType.Eligibility)
        {
            _functionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsEligibility);
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
            return ConvertProfileListToList(SAB.HierarchyServerSession.GetEligModels());
        }

        override public ROModelProperties ModelGetData(ROModelParms parms, ModelProfile modelProfile, ref string message, bool applyOnly = false)
        {
            // populate modelProperties using Windows\EligibilityModelMaint.cs as a reference
            ROCalendarDateInfo calendarDateInfo;
            EligModelProfile emp = (EligModelProfile)modelProfile;

            KeyValuePair<int, string> model = new KeyValuePair<int, string>(key: emp.Key, value: emp.ModelID);
            ROModelEligibilityProperties modelProperties = new ROModelEligibilityProperties(model: model);

            foreach (EligModelEntry modelEntry in emp.ModelEntries)
            {
                calendarDateInfo = new ROCalendarDateInfo(
                    key: modelEntry.DateRange.Key, 
                    displayDate: modelEntry.DateRange.DisplayDate,
                    dateRangeType: modelEntry.DateRange.DateRangeType
                    );
                modelProperties.ModelStockEligibilityEntry.Add(calendarDateInfo);
            }

            foreach (EligModelEntry modelEntry in emp.SalesEligibilityEntries)
            {
                calendarDateInfo = new ROCalendarDateInfo(
                    key: modelEntry.DateRange.Key,
                    displayDate: modelEntry.DateRange.DisplayDate,
                    dateRangeType: modelEntry.DateRange.DateRangeType
                    );
                modelProperties.ModelSalesEligibilityEntry.Add(calendarDateInfo);
            }

            foreach (EligModelEntry modelEntry in emp.PriorityShippingEntries)
            {
                calendarDateInfo = new ROCalendarDateInfo(
                    key: modelEntry.DateRange.Key,
                    displayDate: modelEntry.DateRange.DisplayDate,
                    dateRangeType: modelEntry.DateRange.DateRangeType
                    );
                modelProperties.ModelPriorityShippingEntry.Add(calendarDateInfo);
            }

            return modelProperties;
        }
    

        override public ModelProfile ModelUpdateData(ROModelProperties modelsProperties, bool cloneDates, ref string message, out bool successful, bool applyOnly = false)
        {
            EligModelEntry modelEntry;
            int sequence;
            successful = true;

            ROModelEligibilityProperties eligibilityProperties = (ROModelEligibilityProperties)modelsProperties;

            EligModelProfile emp = new EligModelProfile(modelsProperties.Model.Key);

            if (modelsProperties.Model.Key == Include.NoRID)
            {
                emp.ModelChangeType = eChangeType.add;
            }
            else
            {
                emp.ModelChangeType = eChangeType.update;
            }

            sequence = 0;
            foreach (ROCalendarDateInfo calendarDateInfo in eligibilityProperties.ModelStockEligibilityEntry)
            {
                modelEntry = new EligModelEntry();
                modelEntry.EligModelEntryType = eEligModelEntryType.StockEligibility;
                if (calendarDateInfo.Key != Include.NoRID)
                {
                    if (cloneDates)
                    {
                        modelEntry.DateRange = SAB.ClientServerSession.Calendar.GetDateRangeClone(calendarDateInfo.Key);
                    }
                    else
                    {
                        modelEntry.DateRange = SAB.ClientServerSession.Calendar.GetDateRange(calendarDateInfo.Key);
                    }
                }
                else
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                    continue;
                }
                modelEntry.ModelEntrySeq = sequence;
                modelEntry.ModelEntryChangeType = eChangeType.add;
                emp.ModelEntries.Add(modelEntry);

                ++sequence;
            }

            sequence = 0;
            foreach (ROCalendarDateInfo calendarDateInfo in eligibilityProperties.ModelSalesEligibilityEntry)
            {
                modelEntry = new EligModelEntry();
                modelEntry.EligModelEntryType = eEligModelEntryType.SalesEligibility;
                if (calendarDateInfo.Key != Include.NoRID)
                {
                    if (cloneDates)
                    {
                        modelEntry.DateRange = SAB.ClientServerSession.Calendar.GetDateRangeClone(calendarDateInfo.Key);
                    }
                    else
                    {
                        modelEntry.DateRange = SAB.ClientServerSession.Calendar.GetDateRange(calendarDateInfo.Key);
                    }
                }
                else
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                    continue;
                }
                modelEntry.ModelEntrySeq = sequence;
                modelEntry.ModelEntryChangeType = eChangeType.add;
                emp.SalesEligibilityEntries.Add(modelEntry);

                ++sequence;
            }

            sequence = 0;
            foreach (ROCalendarDateInfo calendarDateInfo in eligibilityProperties.ModelPriorityShippingEntry)
            {
                modelEntry = new EligModelEntry();
                modelEntry.EligModelEntryType = eEligModelEntryType.PriorityShipping;
                if (calendarDateInfo.Key != Include.NoRID)
                {
                    if (cloneDates)
                    {
                        modelEntry.DateRange = SAB.ClientServerSession.Calendar.GetDateRangeClone(calendarDateInfo.Key);
                    }
                    else
                    {
                        modelEntry.DateRange = SAB.ClientServerSession.Calendar.GetDateRange(calendarDateInfo.Key);
                    }
                }
                else
                {
                    message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                    continue;
                }
                modelEntry.ModelEntrySeq = sequence;
                modelEntry.ModelEntryChangeType = eChangeType.add;
                emp.PriorityShippingEntries.Add(modelEntry);

                ++sequence;
            }

            emp.ModelID = modelsProperties.Model.Value;
            emp.Key = modelsProperties.Model.Key;

            if (!applyOnly)
            {
                emp.Key = SAB.HierarchyServerSession.EligModelUpdate(emp);
            }

            // set lock status to maintain lock.
            emp.ModelLockStatus = eLockStatus.Locked;

            return emp;
        }

        override public bool ModelDelete(int key, ref string message)
        {
            message = null;

            EligModelProfile emp = new EligModelProfile(key);
            emp.ModelChangeType = eChangeType.delete;
            emp.Key = key;
            SAB.HierarchyServerSession.EligModelUpdate(emp);

            return true;
        }

        override public bool ModelNameExists(string name, int userKey)
        {
            ModelProfile checkExists = SAB.HierarchyServerSession.GetModelData(aModelType: eModelType.Eligibility, modelID: name);

            return checkExists.Key != Include.NoRID;
        }
    }
}
