using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;
using System.Collections.Generic;

namespace MIDRetail.Business
{
    /// <summary>
    /// Summary description for OTSForecastExportMethod.
    /// </summary>

    public class OTSForecastExportMethod : OTSPlanBaseMethod
    {
        //==========
        // CONSTANTS
        //==========

        const string _sourceModule = "OTSForecastExport.cs";

        //=======
        // FIELDS
        //=======

        private ProfileList _masterVersionProfList;
        private ProfileList _varProfList;
        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        private Stack _extractDataStack;
        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        private OTSForecastExportMethodData _dlExportMethod;
        private bool _methodValid = true;
        private bool _forcingDefaultFormatSettings = false;
        private bool _forceDefaultFormatSettings = false;

        private int _merchandiseRID;
        private int _versionRID;
        private int _dateRangeRID;
        private int _filterRID;
        private ePlanType _planType;
        private bool _lowLevels;
        private bool _lowLevelsOnly;
        private eLowLevelsType _lowLevelsType;
        private int _lowLevelSequence;
        private int _lowLevelOffset;
        private bool _showIneligible;
        private bool _useDefaultSettings;
        private ExportFormatOptions _databaseFormatOptions;
        private ExportFormatOptions _defaultFormatOptions;

        private ArrayList _selectableVariableList;
        // BEGIN Override Low Level Enhancement
        //private ArrayList _versionOverrideList;
        // END Override Low Level Enhancement

        private int _nodeOverrideRid = Include.NoRID;
        private int _versionOverrideRid = Include.NoRID;
        // BEGIN Override Low Level Enhancements
        private int _overrideLowLevelRid = Include.NoRID;
        // END Override Low Level Enhancements

        //=============
        // CONSTRUCTORS
        //=============

        public OTSForecastExportMethod(SessionAddressBlock SAB, int aMethodRID)
            //Begin TT#523 - JScott - Duplicate folder when new folder added
            //: base(SAB, aMethodRID, eMethodType.Export)
            : base(SAB, aMethodRID, eMethodType.Export, eProfileType.MethodExport)
        //End TT#523 - JScott - Duplicate folder when new folder added
        {
            _masterVersionProfList = SAB.ClientServerSession.GetUserForecastVersions();
            _varProfList = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanVariables.VariableProfileList;
            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            _extractDataStack = new Stack();
            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            _databaseFormatOptions = new ExportFormatOptions();

            if (base.Filled)
            {
                _dlExportMethod = new OTSForecastExportMethodData(_varProfList, aMethodRID, eChangeType.populate);

                _merchandiseRID = _dlExportMethod.HierarchyRID;
                _versionRID = _dlExportMethod.VersionRID;
                _dateRangeRID = _dlExportMethod.DateRangeRID;
                _filterRID = _dlExportMethod.FilterRID;
                _planType = _dlExportMethod.PlanType;
                _lowLevels = _dlExportMethod.LowLevels;
                _lowLevelsOnly = _dlExportMethod.LowLevelsOnly;
                _lowLevelsType = _dlExportMethod.LowLevelsType;
                _lowLevelSequence = _dlExportMethod.LowLevelSequence;
                _lowLevelOffset = _dlExportMethod.LowLevelOffset;
                _showIneligible = _dlExportMethod.ShowIneligible;
                _useDefaultSettings = _dlExportMethod.UseDefaultSettings;
                // BEGIN Override Low Level Enhancements
                _overrideLowLevelRid = _dlExportMethod.OverrideLowLevelRid;
                // END Override Low Level Enhancements
                _databaseFormatOptions.ExportType = _dlExportMethod.ExportType;
                _databaseFormatOptions.Delimeter = _dlExportMethod.Delimeter;
                _databaseFormatOptions.CSVFileExtension = _dlExportMethod.CSVFileExtension;
                _databaseFormatOptions.DateType = _dlExportMethod.DateType;
                _databaseFormatOptions.PreinitValues = _dlExportMethod.PreinitValues;
                _databaseFormatOptions.ExcludeZeroValues = _dlExportMethod.ExcludeZeroValues;
                _databaseFormatOptions.FilePath = _dlExportMethod.FilePath;
                _databaseFormatOptions.AddDateStamp = _dlExportMethod.AddDateStamp;
                _databaseFormatOptions.AddTimeStamp = _dlExportMethod.AddTimeStamp;
                _databaseFormatOptions.SplitType = _dlExportMethod.SplitType;
                _databaseFormatOptions.SplitNumEntries = _dlExportMethod.SplitNumEntries;
                _databaseFormatOptions.ConcurrentProcesses = _dlExportMethod.ConcurrentProcesses;
                _databaseFormatOptions.CreateFlagFile = _dlExportMethod.CreateFlagFile;
                _databaseFormatOptions.FlagFileExtension = _dlExportMethod.FlagFileExtension;
                _databaseFormatOptions.CreateEndFile = _dlExportMethod.CreateEndFile;
                _databaseFormatOptions.EndFileExtension = _dlExportMethod.EndFileExtension;
                // BEGIN OVerride Low Level Enhancement
                //_versionOverrideList = (ArrayList)_dlExportMethod.VersionOverrideList.Clone();
                // END OVerride Low Level Enhancement
            }
            else
            {   //Defaults
                _dlExportMethod = new OTSForecastExportMethodData(_varProfList);

                _merchandiseRID = Include.NoRID;
                _versionRID = Include.NoRID;
                _dateRangeRID = Include.NoRID;
                _filterRID = Include.NoRID;
                _planType = ePlanType.Chain;
                _lowLevels = false;
                _lowLevelsOnly = false;
                _lowLevelsType = eLowLevelsType.None;
                _lowLevelSequence = -1;
                _lowLevelOffset = -1;
                _showIneligible = false;
                _useDefaultSettings = true;
                // BEGIN Override Low Level Enhancements
                _overrideLowLevelRid = Include.NoRID;
                // END Override Low Level Enhancements
                _databaseFormatOptions.ExportType = eExportType.XML;
                _databaseFormatOptions.Delimeter = string.Empty;
                _databaseFormatOptions.CSVFileExtension = string.Empty;
                _databaseFormatOptions.DateType = 0;
                _databaseFormatOptions.PreinitValues = false;
                _databaseFormatOptions.ExcludeZeroValues = false;
                _databaseFormatOptions.FilePath = string.Empty;
                _databaseFormatOptions.AddDateStamp = false;
                _databaseFormatOptions.AddTimeStamp = false;
                _databaseFormatOptions.SplitType = eExportSplitType.None;
                _databaseFormatOptions.SplitNumEntries = 0;
                _databaseFormatOptions.ConcurrentProcesses = 0;
                _databaseFormatOptions.CreateFlagFile = false;
                _databaseFormatOptions.FlagFileExtension = string.Empty;
                _databaseFormatOptions.CreateEndFile = false;
                _databaseFormatOptions.EndFileExtension = string.Empty;
                // BEGIN Override Low Level Enhancement
                //_versionOverrideList = new ArrayList();
                // END Override Low Level Enhancement
            }
        }

        //============
        // PROPERTIES
        //============

        /// <summary>
        /// Gets the ProfileType of this profile.
        /// </summary>

        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.MethodExport;
            }
        }

        public bool MethodValid
        {
            get
            {
                return _methodValid;
            }
        }

        public bool ForceDefaultFormatSettings
        {
            set
            {
                _forcingDefaultFormatSettings = true;
                _forceDefaultFormatSettings = value;
            }
        }

        public int HierarchyRID
        {
            get
            {
                return _merchandiseRID;
            }
            set
            {
                _merchandiseRID = value;
            }
        }

        public int VersionRID
        {
            get
            {
                return _versionRID;
            }
            set
            {
                _versionRID = value;
            }
        }

        public int DateRangeRID
        {
            get
            {
                return _dateRangeRID;
            }
            set
            {
                _dateRangeRID = value;
            }
        }

        public int FilterRID
        {
            get
            {
                return _filterRID;
            }
            set
            {
                _filterRID = value;
            }
        }

        public ePlanType PlanType
        {
            get
            {
                return _planType;
            }
            set
            {
                _planType = value;
            }
        }

        public bool LowLevels
        {
            get
            {
                return _lowLevels;
            }
            set
            {
                _lowLevels = value;
            }
        }

        public bool LowLevelsOnly
        {
            get
            {
                return _lowLevelsOnly;
            }
            set
            {
                _lowLevelsOnly = value;
            }
        }

        public eLowLevelsType LowLevelsType
        {
            get
            {
                return _lowLevelsType;
            }
            set
            {
                _lowLevelsType = value;
            }
        }

        public int LowLevelSequence
        {
            get
            {
                return _lowLevelSequence;
            }
            set
            {
                _lowLevelSequence = value;
            }
        }

        public int LowLevelOffset
        {
            get
            {
                return _lowLevelOffset;
            }
            set
            {
                _lowLevelOffset = value;
            }
        }

        public bool ShowIneligible
        {
            get
            {
                return _showIneligible;
            }
            set
            {
                _showIneligible = value;
            }
        }

        public bool UseDefaultSettings
        {
            get
            {
                return _useDefaultSettings;
            }
            set
            {
                _useDefaultSettings = value;
            }
        }

        // BEGIN Override Low Level Enhancement
        public int OverrideLowLevelRid
        {
            get
            {
                return _overrideLowLevelRid;
            }
            set
            {
                _overrideLowLevelRid = value;
            }
        }
        // END Override Low Level Enhancement

        public eExportType ExportType
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.ExportType;
                }
                else
                {
                    return _databaseFormatOptions.ExportType;
                }
            }
            set
            {
                _databaseFormatOptions.ExportType = value;
            }
        }

        public string Delimeter
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.Delimeter;
                }
                else
                {
                    return _databaseFormatOptions.Delimeter;
                }
            }
            set
            {
                _databaseFormatOptions.Delimeter = value;
            }
        }

        public string CSVFileExtension
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.CSVFileExtension;
                }
                else
                {
                    return _databaseFormatOptions.CSVFileExtension;
                }
            }
            set
            {
                _databaseFormatOptions.CSVFileExtension = value;
            }
        }

        public eExportDateType DateType
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.DateType;
                }
                else
                {
                    return _databaseFormatOptions.DateType;
                }
            }
            set
            {
                _databaseFormatOptions.DateType = value;
            }
        }

        public bool PreinitValues
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.PreinitValues;
                }
                else
                {
                    return _databaseFormatOptions.PreinitValues;
                }
            }
            set
            {
                _databaseFormatOptions.PreinitValues = value;
            }
        }

        public bool ExcludeZeroValues
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.ExcludeZeroValues;
                }
                else
                {
                    return _databaseFormatOptions.ExcludeZeroValues;
                }
            }
            set
            {
                _databaseFormatOptions.ExcludeZeroValues = value;
            }
        }

        public string FilePath
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.FilePath;
                }
                else
                {
                    return _databaseFormatOptions.FilePath;
                }
            }
            set
            {
                _databaseFormatOptions.FilePath = value;
            }
        }

        public bool AddDateStamp
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.AddDateStamp;
                }
                else
                {
                    return _databaseFormatOptions.AddDateStamp;
                }
            }
            set
            {
                _databaseFormatOptions.AddDateStamp = value;
            }
        }

        public bool AddTimeStamp
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.AddTimeStamp;
                }
                else
                {
                    return _databaseFormatOptions.AddTimeStamp;
                }
            }
            set
            {
                _databaseFormatOptions.AddTimeStamp = value;
            }
        }

        public eExportSplitType SplitType
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.SplitType;
                }
                else
                {
                    return _databaseFormatOptions.SplitType;
                }
            }
            set
            {
                _databaseFormatOptions.SplitType = value;
            }
        }

        public int SplitNumEntries
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.SplitNumEntries;
                }
                else
                {
                    return _databaseFormatOptions.SplitNumEntries;
                }
            }
            set
            {
                _databaseFormatOptions.SplitNumEntries = value;
            }
        }

        public int ConcurrentProcesses
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.ConcurrentProcesses;
                }
                else
                {
                    return _databaseFormatOptions.ConcurrentProcesses;
                }
            }
            set
            {
                _databaseFormatOptions.ConcurrentProcesses = value;
            }
        }

        public bool CreateFlagFile
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.CreateFlagFile;
                }
                else
                {
                    return _databaseFormatOptions.CreateFlagFile;
                }
            }
            set
            {
                _databaseFormatOptions.CreateFlagFile = value;
            }
        }

        public string FlagFileExtension
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.FlagFileExtension;
                }
                else
                {
                    return _databaseFormatOptions.FlagFileExtension;
                }
            }
            set
            {
                _databaseFormatOptions.FlagFileExtension = value;
            }
        }

        public bool CreateEndFile
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.CreateEndFile;
                }
                else
                {
                    return _databaseFormatOptions.CreateEndFile;
                }
            }
            set
            {
                _databaseFormatOptions.CreateEndFile = value;
            }
        }

        public string EndFileExtension
        {
            get
            {
                if ((!_forcingDefaultFormatSettings && _useDefaultSettings) ||
                    (_forcingDefaultFormatSettings && _forceDefaultFormatSettings))
                {
                    return DefaultFormatOptions.EndFileExtension;
                }
                else
                {
                    return _databaseFormatOptions.EndFileExtension;
                }
            }
            set
            {
                _databaseFormatOptions.EndFileExtension = value;
            }
        }

        // BEGIN Override Low Level Enhancement
        //public ArrayList VersionOverrideList
        //{
        //    get
        //    {
        //        return _versionOverrideList;
        //    }
        //    set
        //    {
        //        _versionOverrideList = value;
        //    }
        //}
        // END Override Low Level Enhancement

        public ArrayList SelectableVariableList
        {
            get
            {
                try
                {
                    if (_selectableVariableList == null)
                    {
                        LoadSelectableVariableList();
                    }

                    return _selectableVariableList;
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }
            set
            {
                _selectableVariableList = value;
            }
        }

        private ExportFormatOptions DefaultFormatOptions
        {
            get
            {
                try
                {
                    if (_defaultFormatOptions == null)
                    {
                        _defaultFormatOptions = new ExportFormatOptions();
                        _methodValid = _defaultFormatOptions.LoadDefaultSettings(SAB);
                    }

                    return _defaultFormatOptions;
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }
        }

        //========
        // METHODS
        //========

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (IsFilterUser(_filterRID))
            {
                return true;
            }

            if (IsStoreGroupUser(SG_RID))
            {
                return true;
            }

            if (IsHierarchyNodeUser(_merchandiseRID))
            {
                return true;
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)


        /// <summary>
        /// Updates the OTS Forecast Export method
        /// </summary>
        /// <param name="td">
        /// An instance of the TransactionData class which contains the database connection
        /// </param>

        override public void Update(TransactionData td)
        {
            ArrayList variableList;

            try
            {
                _dlExportMethod.HierarchyRID = _merchandiseRID;
                _dlExportMethod.VersionRID = _versionRID;
                _dlExportMethod.DateRangeRID = _dateRangeRID;
                _dlExportMethod.FilterRID = _filterRID;
                _dlExportMethod.PlanType = _planType;
                _dlExportMethod.LowLevels = _lowLevels;
                _dlExportMethod.LowLevelsOnly = _lowLevelsOnly;
                _dlExportMethod.LowLevelsType = _lowLevelsType;
                _dlExportMethod.LowLevelSequence = _lowLevelSequence;
                _dlExportMethod.LowLevelOffset = _lowLevelOffset;
                _dlExportMethod.ShowIneligible = _showIneligible;
                _dlExportMethod.UseDefaultSettings = _useDefaultSettings;
                // BEGIN Override Low Level Enhancement
                _dlExportMethod.OverrideLowLevelRid = _overrideLowLevelRid;
                // END Override Low Level Enhancement
                _dlExportMethod.ExportType = _databaseFormatOptions.ExportType;
                _dlExportMethod.Delimeter = _databaseFormatOptions.Delimeter;
                _dlExportMethod.CSVFileExtension = _databaseFormatOptions.CSVFileExtension;
                _dlExportMethod.DateType = _databaseFormatOptions.DateType;
                _dlExportMethod.PreinitValues = _databaseFormatOptions.PreinitValues;
                _dlExportMethod.ExcludeZeroValues = _databaseFormatOptions.ExcludeZeroValues;
                _dlExportMethod.FilePath = _databaseFormatOptions.FilePath;
                _dlExportMethod.AddDateStamp = _databaseFormatOptions.AddDateStamp;
                _dlExportMethod.AddTimeStamp = _databaseFormatOptions.AddTimeStamp;
                _dlExportMethod.SplitType = _databaseFormatOptions.SplitType;
                _dlExportMethod.SplitNumEntries = _databaseFormatOptions.SplitNumEntries;
                _dlExportMethod.ConcurrentProcesses = _databaseFormatOptions.ConcurrentProcesses;
                _dlExportMethod.CreateFlagFile = _databaseFormatOptions.CreateFlagFile;
                _dlExportMethod.FlagFileExtension = _databaseFormatOptions.FlagFileExtension;
                _dlExportMethod.CreateEndFile = _databaseFormatOptions.CreateEndFile;
                _dlExportMethod.EndFileExtension = _databaseFormatOptions.EndFileExtension;

                variableList = new ArrayList();

                foreach (RowColProfileHeader varEntry in SelectableVariableList)
                {
                    if (varEntry.IsDisplayed)
                    {
                        variableList.Add(new ForecastExportMethodVariableEntry(Key, varEntry.Profile.Key, varEntry.Sequence));
                    }
                }

                _dlExportMethod.VariableList = variableList;
                // BEGIN Override Low Level Enhancement
                //_dlExportMethod.VersionOverrideList = (ArrayList)_versionOverrideList.Clone();
                // END Override Low Level Enhancement

                switch (base.Method_Change_Type)
                {
                    case eChangeType.add:
                        base.Update(td);
                        _dlExportMethod.InsertMethod(base.Key, td);
                        break;
                    case eChangeType.update:
                        base.Update(td);
                        _dlExportMethod.UpdateMethod(base.Key, td);
                        break;
                    case eChangeType.delete:
                        _dlExportMethod.DeleteMethod(base.Key, td);
                        base.Update(td);
                        break;
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }



        override public void ProcessMethod(
            ApplicationSessionTransaction aApplicationTransaction,
            int aStoreFilter, Profile methodProfile)
        {
            ArrayList forecastingOverrideList;

            try
            {
                forecastingOverrideList = aApplicationTransaction.ForecastingOverrideList;

                if (forecastingOverrideList != null)
                {
                    foreach (ForecastingOverride fo in forecastingOverrideList)
                    {
                        if (fo.HierarchyNodeRid != Include.NoRID)
                        {
                            this._merchandiseRID = fo.HierarchyNodeRid;
                            this._nodeOverrideRid = fo.HierarchyNodeRid;
                        }

                        if (fo.ForecastVersionRid != Include.NoRID)
                        {
                            this._versionRID = fo.ForecastVersionRid;
                            this._versionOverrideRid = fo.ForecastVersionRid;
                        }

                        if (_merchandiseRID == Include.NoRID)
                        {
                            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_PlanHierarchyNodeMissing, this.ToString());
                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            //SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running

                            throw new MIDException(eErrorLevel.severe,
                                (int)eMIDTextCode.msg_pl_PlanHierarchyNodeMissing,
                                MIDText.GetText(eMIDTextCode.msg_pl_PlanHierarchyNodeMissing));
                        }

                        if (_versionRID == Include.NoRID)
                        {
                            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_PlanVersionMissing, this.ToString());
                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            //SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running

                            throw new MIDException(eErrorLevel.severe,
                                (int)eMIDTextCode.msg_pl_PlanVersionMissing,
                                MIDText.GetText(eMIDTextCode.msg_pl_PlanVersionMissing));
                        }

                        ProcessAction(
                            aApplicationTransaction.SAB,
                            aApplicationTransaction,
                            null,
                            methodProfile,
                            true,
                            aStoreFilter);
                    }
                }
                else
                {
                    ProcessAction(
                        aApplicationTransaction.SAB,
                        aApplicationTransaction,
                        null,
                        methodProfile,
                        true,
                        aStoreFilter);
                }
            }
            catch
            {
                aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
            }
            finally
            {
                _masterVersionProfList = null;
                _varProfList = null;
                _dlExportMethod = null;
                _databaseFormatOptions = null;
                _defaultFormatOptions = null;
                _selectableVariableList = null;
            }
        }

        override public void ProcessAction(
            SessionAddressBlock aSAB,
            ApplicationSessionTransaction aApplicationTransaction,
            ApplicationWorkFlowStep aWorkFlowStep,
            Profile aProfile,
            bool WriteToDB,
            int aStoreFilterRID)
        {
            MIDTimer aTimer;
            string infoMsg;
            ProfileList varProfList;
            Stack procStack;
            ProfileList nodeList;
            HierarchyNodeProfile nodeProf;
            ForecastVersionProfileBuilder verBuilder;
            VersionProfile verProf;
            ProfileList lowLevelList;
            PlanProfile HLPlanProf;
            PlanProfile planProf;
            DateTime currDate;
            eProcessCompletionStatus compStat;
            string endFileName = string.Empty;

            try
            {
                aTimer = new MIDTimer();

                infoMsg = "Starting OTS Forecast Export: " + this.Name;
                aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, infoMsg, this.ToString());
                aTimer.Start();

                aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.NoActionPerformed;

                if (aStoreFilterRID != Include.NoRID && aStoreFilterRID != Include.UndefinedStoreFilter)
                {
                    _filterRID = aStoreFilterRID;
                }


                CreateSortedProfileList(SelectableVariableList, out varProfList);

                nodeList = new ProfileList(eProfileType.Plan);
                procStack = new Stack();
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //nodeProf = aSAB.HierarchyServerSession.GetNodeData(_merchandiseRID);
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                nodeProf = aSAB.HierarchyServerSession.GetNodeData(_merchandiseRID);
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                nodeProf.QualifiedNodeID = SAB.HierarchyServerSession.GetQualifiedNodeID(nodeProf.Key);
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                nodeProf.ChainSecurityProfile = new HierarchyNodeSecurityProfile(_merchandiseRID);
                nodeProf.StoreSecurityProfile = new HierarchyNodeSecurityProfile(_merchandiseRID);

                verBuilder = new ForecastVersionProfileBuilder();
                verProf = verBuilder.Build(_versionRID);
                verProf.StoreSecurity = new VersionSecurityProfile(_versionRID);
                verProf.ChainSecurity = new VersionSecurityProfile(_versionRID);

                currDate = System.DateTime.Now;

                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                if (CreateEndFile)
                {
                    //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                    //					endFileName =
                    //						GenerateEndFileName(aSAB.HierarchyServerSession.GetNodeData(_merchandiseRID), currDate) +
                    //						((EndFileExtension[0] == '.') ? "" : ".") + EndFileExtension;
                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //endFileName = GenerateEndFileName(aSAB.HierarchyServerSession.GetNodeData(_merchandiseRID), currDate);
                    endFileName = GenerateEndFileName(nodeProf, currDate);
                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file

                    if (File.Exists(endFileName))
                    {
                        File.Delete(endFileName);
                    }
                }

                //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                HLPlanProf = new PlanProfile(0, nodeProf, verProf);

                //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                if (!LowLevels || !LowLevelsOnly)
                {
                    //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                    //					nodeList.Add(new PlanProfile(nodeList.Count, nodeProf, verProf));
                    nodeList.Add(HLPlanProf);
                    //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                }

                if (LowLevels)
                {
                    lowLevelList = PopulateVersionOverrideList(aSAB, _lowLevelsType, _lowLevelOffset, _lowLevelSequence, nodeProf.Key, verProf.Key, _overrideLowLevelRid);

                    foreach (LowLevelVersionOverrideProfile lvop in lowLevelList)
                    {
                        if (!lvop.Exclude)
                        {
                            planProf = new PlanProfile(nodeList.Count);
                            planProf.NodeProfile = lvop.NodeProfile;
                            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            planProf.NodeProfile.QualifiedNodeID = SAB.HierarchyServerSession.GetQualifiedNodeID(planProf.NodeProfile.Key);
                            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            planProf.NodeProfile.ChainSecurityProfile = aSAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.Key, (int)eSecurityTypes.Chain);
                            planProf.NodeProfile.StoreSecurityProfile = aSAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.Key, (int)eSecurityTypes.Store);

                            if (lvop.VersionIsOverridden)
                            {
                                planProf.VersionProfile = lvop.VersionProfile;
                            }
                            else
                            {
                                planProf.VersionProfile = verProf;
                            }

                            nodeList.Add(planProf);
                        }
                    }
                }

                if (SplitType == eExportSplitType.Merchandise)
                {
                    foreach (PlanProfile planProfile in nodeList)
                    {
                        //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                        //						procStack.Push(new ExtractProcessor(this, SAB, planProfile, varProfList, currDate));
                        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                        //procStack.Push(new ExtractProcessor(this, SAB, HLPlanProf, planProfile, varProfList, currDate));
                        _extractDataStack.Push(new ExtractData(this, HLPlanProf, planProfile, varProfList, currDate));
                        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                        //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                    }
                }
                else
                {
                    //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                    //					procStack.Push(new ExtractProcessor(this, SAB, nodeList, varProfList, currDate));
                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //procStack.Push(new ExtractProcessor(this, SAB, HLPlanProf, nodeList, varProfList, currDate));
                    _extractDataStack.Push(new ExtractData(this, HLPlanProf, nodeList, varProfList, currDate));
                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                }

                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //compStat = ProcessExtractList(aSAB, procStack);
                compStat = ProcessExtractList(aSAB);
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                if (CreateEndFile)
                {
                    System.IO.File.Create(endFileName).Close();
                }

                aTimer.Stop();
                infoMsg = "Completed OTS Forecast Export: " + this.Name + " " + "Elasped time: " + aTimer.ElaspedTimeString;
                aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, infoMsg, this.ToString());

                if (compStat == eProcessCompletionStatus.Successful)
                {
                    aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
                }
                else
                {
                    aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                }
            }
            catch (Exception err)
            {
                aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessAction: " + err.Message, _sourceModule);
                aSAB.ApplicationServerSession.Audit.Log_Exception(err);
                aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
            }
        }

        override public bool WithinTolerance(double aTolerancePercent)
        {
            return true;
        }

        /// <summary>
        /// Returns a copy of this object.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aCloneDateRanges">
        /// A flag identifying if date ranges are to be cloned or use the original</param>
        /// <param name="aCloneCustomOverrideModels">
        /// A flag identifying if custom override models are to be cloned or use the original
        /// </param>
        /// <returns>
        /// A copy of the object.
        /// </returns>

        // Begin Track #5912 - JSmith - Save As needs to clone custom override models
        //override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges)
        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
        // End Track #5912
        {
            OTSForecastExportMethod newOTSForecastExportMethod = null;
            // Begin Track #5912 - JSmith - Save As needs to clone custom override models
            OverrideLowLevelProfile ollp;
            int customUserRID;
            // End Track #5912

            try
            {
                newOTSForecastExportMethod = (OTSForecastExportMethod)this.MemberwiseClone();

                newOTSForecastExportMethod._merchandiseRID = _merchandiseRID;
                newOTSForecastExportMethod._versionRID = _versionRID;

                if (aCloneDateRanges &&
                    _dateRangeRID != Include.UndefinedCalendarDateRange)
                {
                    newOTSForecastExportMethod._dateRangeRID = aSession.Calendar.GetDateRangeClone(_dateRangeRID).Key;
                }
                else
                {
                    newOTSForecastExportMethod._dateRangeRID = _dateRangeRID;
                }

                newOTSForecastExportMethod._filterRID = _filterRID;
                newOTSForecastExportMethod._planType = _planType;
                newOTSForecastExportMethod._lowLevels = _lowLevels;
                newOTSForecastExportMethod._lowLevelsOnly = _lowLevelsOnly;
                newOTSForecastExportMethod._lowLevelsType = _lowLevelsType;
                newOTSForecastExportMethod._lowLevelSequence = _lowLevelSequence;
                newOTSForecastExportMethod._lowLevelOffset = _lowLevelOffset;
                newOTSForecastExportMethod._showIneligible = _showIneligible;
                newOTSForecastExportMethod._useDefaultSettings = _useDefaultSettings;
                newOTSForecastExportMethod._databaseFormatOptions = _databaseFormatOptions.Copy();

                if (_defaultFormatOptions != null)
                {
                    newOTSForecastExportMethod._defaultFormatOptions = _defaultFormatOptions.Copy();
                }

                newOTSForecastExportMethod._selectableVariableList = null;

                // BEGIN Override Low Level Enhancement
                //newOTSForecastExportMethod._versionOverrideList = new ArrayList();

                //foreach (ForecastExportMethodVersionOverrideEntry obj in _versionOverrideList)
                //{
                //    newOTSForecastExportMethod._versionOverrideList.Add(obj);
                //}
                // END Override Low Level Enhancement

                newOTSForecastExportMethod.Method_Change_Type = eChangeType.none;
                newOTSForecastExportMethod.Method_Description = Method_Description;
                newOTSForecastExportMethod.MethodStatus = MethodStatus;
                newOTSForecastExportMethod.Name = Name;
                newOTSForecastExportMethod.SG_RID = SG_RID;
                newOTSForecastExportMethod.User_RID = User_RID;
                newOTSForecastExportMethod.Virtual_IND = Virtual_IND;
                newOTSForecastExportMethod.Template_IND = Template_IND;
                // Begin Track #5912 - JSmith - Save As needs to clone custom override models
                if (aCloneCustomOverrideModels &&
                    CustomOLL_RID != Include.NoRID)
                {
                    ollp = new OverrideLowLevelProfile(CustomOLL_RID);
                    ollp.Key = Include.NoRID;
                    ollp.ModelChangeType = eChangeType.add;
                    customUserRID = Include.NoRID;
                    customUserRID = ollp.WriteProfile(ref customUserRID, SAB.ClientServerSession.UserRID);
                    if (CustomOLL_RID == OverrideLowLevelRid)
                    {
                        OverrideLowLevelRid = customUserRID;
                    }
                    newOTSForecastExportMethod.CustomOLL_RID = customUserRID;
                }
                // End Track #5912
                // BEGIN Override Low Level Enhancement
                newOTSForecastExportMethod.OverrideLowLevelRid = OverrideLowLevelRid;
                // END Override Low Level Enhancement

                return newOTSForecastExportMethod;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        private void LoadSelectableVariableList()
        {
            Hashtable varKeyHash;
            VariableProfile viewVarProf;
            ForecastExportMethodVariableEntry varEntry;

            try
            {
                varKeyHash = new Hashtable();

                foreach (ForecastExportMethodVariableEntry viewVarEntry in _dlExportMethod.VariableList)
                {
                    viewVarProf = (VariableProfile)_varProfList.FindKey(viewVarEntry.VariableRID);

                    if (viewVarProf != null)
                    {
                        varKeyHash.Add(viewVarProf.Key, viewVarEntry);
                    }
                }

                _selectableVariableList = new ArrayList();

                foreach (VariableProfile varProf in _varProfList)
                {
                    varEntry = (ForecastExportMethodVariableEntry)varKeyHash[varProf.Key];

                    if (varEntry != null)
                    {
                        _selectableVariableList.Add(new RowColProfileHeader(varProf.VariableName, true, Convert.ToInt32(varEntry.VariableSequence), varProf));
                    }
                    else
                    {
                        _selectableVariableList.Add(new RowColProfileHeader(varProf.VariableName, false, -1, varProf));
                    }
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public ProfileList PopulateVersionOverrideList(
            SessionAddressBlock aSAB,
            eLowLevelsType aLowLevelsType,
            int aLowLevelOffset,
            int aLowLevelSequence,
            int aNodeRID,
            int aCurrentVersionRID,
            int aOverrideLowLevelRid)
        {
            // BEGIN OVerride Low Level Enhancement
            LowLevelVersionOverrideProfileList overrideList = null;
            try
            {
                HierarchySessionTransaction hTran = new HierarchySessionTransaction(this.SAB);
                if (aLowLevelsType == eLowLevelsType.LevelOffset)
                {
                    overrideList = hTran.GetOverrideList(aOverrideLowLevelRid, aNodeRID, aCurrentVersionRID,
                                                                               aLowLevelOffset, Include.NoRID, true, false);
                }
                else if (_lowLevelsType == eLowLevelsType.HierarchyLevel)
                {
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(aNodeRID);

                    // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                    //int offset = aLowLevelSequence - hnp.NodeLevel;
                    //overrideList = hTran.GetOverrideList(aOverrideLowLevelRid, aNodeRID, aCurrentVersionRID,
                    //                                                           offset, Include.NoRID, true, false);
                    overrideList = hTran.GetOverrideList(aOverrideLowLevelRid, aNodeRID, aCurrentVersionRID,
                                                          eHierarchyDescendantType.levelType, aLowLevelSequence, Include.NoRID, true, false);
                    // End Track #6107
                }
                else
                {
                    overrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
                }

                return overrideList;


                //            HierarchyNodeList hnl;
                //            LowLevelVersionOverrideProfile lvop;

                //            try
                //            {
                //                aLowLevelList.Clear();

                //                if (aLowLevelsType == eLowLevelsType.LevelOffset)
                //                {
                //                    //Begin Track #5378 - color and size not qualified
                ////					hnl = aSAB.HierarchyServerSession.GetDescendantData(aNodeRID, aLowLevelOffset, true, eNodeSelectType.NoVirtual);
                //                    hnl = aSAB.HierarchyServerSession.GetDescendantData(aNodeRID, aLowLevelOffset, true, eNodeSelectType.NoVirtual, true);
                //                    //End Track #5378
                //                }
                //                else
                //                {
                //                    //Begin Track #5378 - color and size not qualified
                ////					hnl = aSAB.HierarchyServerSession.GetDescendantDataByLevel(aNodeRID, aLowLevelSequence, true, eNodeSelectType.NoVirtual);
                //                    hnl = aSAB.HierarchyServerSession.GetDescendantDataByLevel(aNodeRID, aLowLevelSequence, true, eNodeSelectType.NoVirtual, true);
                //                    //End Track #5378
                //                }

                //                foreach (HierarchyNodeProfile llhnp in hnl)
                //                {
                //                    lvop = new LowLevelVersionOverrideProfile(llhnp.Key);
                //                    lvop.NodeProfile = llhnp;
                //                    lvop.VersionIsOverridden = false;
                //                    lvop.VersionProfile = null;
                //                    lvop.Exclude = false;
                //                    aLowLevelList.Add(lvop);
                //                }

                //                foreach (ForecastExportMethodVersionOverrideEntry ovrrdEntry in VersionOverrideList)
                //                {
                //                    lvop = (LowLevelVersionOverrideProfile)aLowLevelList.FindKey(ovrrdEntry.MerchandiseRID);

                //                    if (lvop != null)
                //                    {
                //                        if (ovrrdEntry.VersionRID != aCurrentVersionRID)
                //                        {
                //                            lvop.VersionIsOverridden = true;
                //                            lvop.VersionProfile = (VersionProfile)_masterVersionProfList.FindKey(ovrrdEntry.VersionRID);
                //                        }

                //                        lvop.Exclude = ovrrdEntry.Exclude;
                //                    }
                //                }
                // END OVerride Low Level Enhancement
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Creates a ProfileList containing selected profile objects ordered by sequence from the Selectable ArrayList.
        /// </summary>
        /// <param name="aSelectableList">
        /// An ArrayList containing the selected RowColProfileHeader.
        /// </param>
        /// <param name="aProfileList">
        /// A ProfileList containing selected profile objects ordered by sequence.
        /// </param>

        private void CreateSortedProfileList(ArrayList aSelectableList, out ProfileList aProfileList)
        {
            SortedList sortList;
            IDictionaryEnumerator enumerator;
            eProfileType profType;

            try
            {
                sortList = new SortedList();
                profType = eProfileType.None;

                foreach (RowColProfileHeader rowColHeader in aSelectableList)
                {
                    profType = rowColHeader.Profile.ProfileType;

                    if (rowColHeader.IsDisplayed)
                    {
                        sortList.Add(rowColHeader.Sequence, rowColHeader.Profile);
                    }
                }

                enumerator = sortList.GetEnumerator();
                aProfileList = new ProfileList(profType);

                while (enumerator.MoveNext())
                {
                    aProfileList.Add((Profile)enumerator.Value);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        //private eProcessCompletionStatus ProcessExtractList(SessionAddressBlock aSAB, Stack aProcStack)
        private eProcessCompletionStatus ProcessExtractList(SessionAddressBlock aSAB)
        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        {
            ExtractProcessor[] extractProcArray = null;
            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            Thread[] threadArray;
            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            int concurrentExtracts;
            eProcessCompletionStatus maxCompStat;
            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //ExtractProcessor extractProc;
            ExtractData extractData;
            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            int i;

            try
            {
                if (SplitType == eExportSplitType.Merchandise)
                {
                    concurrentExtracts = ConcurrentProcesses;
                }
                else
                {
                    concurrentExtracts = 1;
                }

                extractProcArray = new ExtractProcessor[concurrentExtracts];
                maxCompStat = eProcessCompletionStatus.Successful;
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                //while (aProcStack.Count > 0)
                //{
                //    for (i = 0; i < concurrentExtracts && aProcStack.Count > 0; i++)
                //    {
                //        if (extractProcArray[i] == null || !extractProcArray[i].isRunning)
                //        {
                //            if (extractProcArray[i] != null)
                //            {
                //                maxCompStat = (eProcessCompletionStatus)Math.Max((int)maxCompStat, (int)extractProcArray[i].CompletionStatus);
                //                extractProcArray[i] = null;
                //            }

                //            extractProc = (ExtractProcessor)aProcStack.Pop();
                //            extractProcArray[i] = extractProc;
                //            extractProcArray[i].ExecuteExtractInThread();
                //        }
                //    }

                //    if (aProcStack.Count > 0)
                //    {
                //        System.Threading.Thread.Sleep(5000);
                //    }
                //}

                //for (i = 0; i < concurrentExtracts; i++)
                //{
                //    if (extractProcArray[i] != null)
                //    {
                //        extractProcArray[i].WaitForThreadExit();
                //        maxCompStat = (eProcessCompletionStatus)Math.Max((int)maxCompStat, (int)extractProcArray[i].CompletionStatus);
                //        extractProcArray[i] = null;
                //    }
                //}
                if (concurrentExtracts > 1)
                {
                    threadArray = new Thread[concurrentExtracts];

                    for (i = 0; i < concurrentExtracts; i++)
                    {
                        extractProcArray[i] = new ExtractProcessor(this);
                        maxCompStat = (eProcessCompletionStatus)Math.Max((int)maxCompStat, (int)extractProcArray[i].Initialize());
                    }

                    if (maxCompStat == eProcessCompletionStatus.Successful)
                    {
                        for (i = 0; i < concurrentExtracts; i++)
                        {
                            threadArray[i] = new Thread(new ThreadStart(extractProcArray[i].ProcessExtract));
                            threadArray[i].Start();
                        }

                        for (i = 0; i < concurrentExtracts; i++)
                        {
                            threadArray[i].Join();
                        }

                        for (i = 0; i < concurrentExtracts; i++)
                        {
                            maxCompStat = (eProcessCompletionStatus)Math.Max((int)maxCompStat, (int)extractProcArray[i].CompletionStatus);
                        }
                    }
                }
                else
                {
                    while (_extractDataStack.Count > 0)
                    {
                        extractData = (ExtractData)_extractDataStack.Pop();
                        extractData.ExecuteExtract(SAB);
                        maxCompStat = (eProcessCompletionStatus)Math.Max((int)maxCompStat, (int)extractData.CompletionStatus);
                    }
                }
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                return maxCompStat;
            }
            catch (Exception err)
            {
                aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessExtractList: " + err.Message, _sourceModule);
                aSAB.ApplicationServerSession.Audit.Log_Exception(err);
                throw;
            }
        }

        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        public ExtractData GetNextExtractData()
        {
            try
            {
                lock (_extractDataStack.SyncRoot)
                {
                    if (_extractDataStack.Count > 0)
                    {
                        return (ExtractData)_extractDataStack.Pop();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

        private string GenerateEndFileName(HierarchyNodeProfile aHLNodeProf, DateTime aCurrDate)
        {
            string fileName;

            try
            {
                fileName = FilePath + "/" + Name + "_" + SAB.HierarchyServerSession.GetQualifiedNodeID(aHLNodeProf.Key, "-");

                if (AddDateStamp)
                {
                    fileName += "_" + aCurrDate.ToString("MMddyy");
                }

                if (AddTimeStamp)
                {
                    fileName += "_" + aCurrDate.ToString("HHmmss");
                }

                if (ExportType == eExportType.XML)
                {
                    fileName += ".xml";
                }
                else
                {
                    if (CSVFileExtension != null &&
                        CSVFileExtension.Length > 0)
                    {
                        fileName += ((CSVFileExtension[0] == '.') ? "" : ".") + CSVFileExtension;
                    }
                    else
                    {
                        fileName += ".csv";
                    }
                }

                fileName += ((EndFileExtension[0] == '.') ? "" : ".") + EndFileExtension;

                return fileName;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        private class ExportFormatOptions
        {
            private eExportType _exportType;
            private string _delimeter;
            private string _csvFileExtension;
            private eExportDateType _dateType;
            private bool _preinitValues;
            private bool _excludeZeroValues;
            private string _filePath;
            private bool _addDateStamp;
            private bool _addTimeStamp;
            private eExportSplitType _splitType;
            private int _splitNumEntries;
            private int _concurrentProcesses;
            private bool _createFlagFile;
            private string _flagFileExtension;
            private bool _createEndFile;
            private string _endFileExtension;

            public ExportFormatOptions()
            {
            }

            public eExportType ExportType
            {
                get
                {
                    return _exportType;
                }
                set
                {
                    _exportType = value;
                }
            }

            public string Delimeter
            {
                get
                {
                    return _delimeter;
                }
                set
                {
                    _delimeter = value;
                }
            }

            public string CSVFileExtension
            {
                get
                {
                    return _csvFileExtension;
                }
                set
                {
                    _csvFileExtension = value;
                }
            }

            public eExportDateType DateType
            {
                get
                {
                    return _dateType;
                }
                set
                {
                    _dateType = value;
                }
            }

            public bool PreinitValues
            {
                get
                {
                    return _preinitValues;
                }
                set
                {
                    _preinitValues = value;
                }
            }

            public bool ExcludeZeroValues
            {
                get
                {
                    return _excludeZeroValues;
                }
                set
                {
                    _excludeZeroValues = value;
                }
            }

            public string FilePath
            {
                get
                {
                    return _filePath;
                }
                set
                {
                    _filePath = value;
                }
            }

            public bool AddDateStamp
            {
                get
                {
                    return _addDateStamp;
                }
                set
                {
                    _addDateStamp = value;
                }
            }

            public bool AddTimeStamp
            {
                get
                {
                    return _addTimeStamp;
                }
                set
                {
                    _addTimeStamp = value;
                }
            }

            public eExportSplitType SplitType
            {
                get
                {
                    return _splitType;
                }
                set
                {
                    _splitType = value;
                }
            }

            public int SplitNumEntries
            {
                get
                {
                    return _splitNumEntries;
                }
                set
                {
                    _splitNumEntries = value;
                }
            }

            public int ConcurrentProcesses
            {
                get
                {
                    return _concurrentProcesses;
                }
                set
                {
                    _concurrentProcesses = value;
                }
            }

            public bool CreateFlagFile
            {
                get
                {
                    return _createFlagFile;
                }
                set
                {
                    _createFlagFile = value;
                }
            }

            public string FlagFileExtension
            {
                get
                {
                    return _flagFileExtension;
                }
                set
                {
                    _flagFileExtension = value;
                }
            }

            public bool CreateEndFile
            {
                get
                {
                    return _createEndFile;
                }
                set
                {
                    _createEndFile = value;
                }
            }

            public string EndFileExtension
            {
                get
                {
                    return _endFileExtension;
                }
                set
                {
                    _endFileExtension = value;
                }
            }

            public bool LoadDefaultSettings(SessionAddressBlock aSAB)
            {
                string cfgVal;
                bool errFound;
                string errMessage;
                string tmpMessage;

                try
                {
                    errFound = false;
                    errMessage = string.Empty;

                    cfgVal = MIDConfigurationManager.AppSettings["Export_FileFormat"];

                    if (cfgVal == null)
                    {
                        errFound = true;
                        tmpMessage = MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired);
                        tmpMessage = tmpMessage.Replace("{0}", "Key 'Export_FileFormat' in config file");
                        errMessage += tmpMessage + System.Environment.NewLine;
                    }
                    else if (cfgVal.Trim().ToLower() == "csv")
                        if (cfgVal.Trim().ToLower() == "csv")
                        {
                            _exportType = eExportType.CSV;
                        }
                        else if (cfgVal.Trim().ToLower() == "xml")
                        {
                            _exportType = eExportType.XML;
                        }
                        else
                        {
                            errFound = true;
                            errMessage += MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDefaultFileFormat) + System.Environment.NewLine;
                        }

                    if (_exportType == eExportType.CSV)
                    {
                        cfgVal = MIDConfigurationManager.AppSettings["Export_CSVDelimeter"];

                        if (cfgVal == null)
                        {
                            errFound = true;
                            tmpMessage = MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired);
                            tmpMessage = tmpMessage.Replace("{0}", "Key 'Export_CSVDelimeter' in config file");
                            errMessage += tmpMessage + System.Environment.NewLine;
                        }
                        else if (cfgVal.Trim().Length == 1)
                        {
                            _delimeter = cfgVal.Trim();
                        }
                        else
                        {
                            errFound = true;
                            errMessage += MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDefaultDelimeter) + System.Environment.NewLine;
                        }

                        cfgVal = MIDConfigurationManager.AppSettings["Export_CSVFileSuffix"];

                        if (cfgVal == null)
                        {
                            errFound = true;
                            tmpMessage = MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired);
                            tmpMessage = tmpMessage.Replace("{0}", "Key 'Export_CSVFileSuffix' in config file");
                            errMessage += tmpMessage + System.Environment.NewLine;
                        }
                        else
                        {
                            _csvFileExtension = cfgVal.Trim();
                        }
                    }
                    else
                    {
                        _delimeter = string.Empty;
                    }

                    cfgVal = MIDConfigurationManager.AppSettings["Export_DateType"];

                    if (cfgVal == null)
                    {
                        _dateType = eExportDateType.Calendar;
                    }
                    else if (cfgVal.Trim().ToLower() == "calendar")
                    {
                        _dateType = eExportDateType.Calendar;
                    }
                    else if (cfgVal.Trim().ToLower() == "fiscal")
                    {
                        _dateType = eExportDateType.Fiscal;
                    }
                    else
                    {
                        errFound = true;
                        errMessage += MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDefaultDateType) + System.Environment.NewLine;
                    }

                    _preinitValues = ConvertBoolConfigValue(MIDConfigurationManager.AppSettings["Export_PreinitValues"]);
                    _excludeZeroValues = ConvertBoolConfigValue(MIDConfigurationManager.AppSettings["Export_ExcludeZeroValues"]);

                    cfgVal = MIDConfigurationManager.AppSettings["Export_FilePath"];

                    if (cfgVal == null)
                    {
                        errFound = true;
                        tmpMessage = MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired);
                        tmpMessage = tmpMessage.Replace("{0}", "Key 'Export_FilePath' in config file");
                        errMessage += tmpMessage + System.Environment.NewLine;
                    }
                    else if (cfgVal.Trim().Length > 0)
                    {
                        _filePath = cfgVal.Trim();
                    }
                    else
                    {
                        errFound = true;
                        errMessage += MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDefaultFilePath) + System.Environment.NewLine;
                    }

                    _addDateStamp = ConvertBoolConfigValue(MIDConfigurationManager.AppSettings["Export_AddDateStampToFileName"]);
                    _addTimeStamp = ConvertBoolConfigValue(MIDConfigurationManager.AppSettings["Export_AddTimeStampToFileName"]);

                    cfgVal = MIDConfigurationManager.AppSettings["Export_SplitFiles"];

                    if (cfgVal == null ||
                        cfgVal.Trim().ToLower() == "none")
                    {
                        _splitType = eExportSplitType.None;
                    }
                    else if (cfgVal.Trim().ToLower() == "merchandise")
                    {
                        _splitType = eExportSplitType.Merchandise;
                    }
                    else if (cfgVal.Trim().ToLower() == "numberofentries")
                    {
                        _splitType = eExportSplitType.NumEntries;
                    }
                    else
                    {
                        errFound = true;
                        errMessage += MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDefaultSplitType) + System.Environment.NewLine;
                    }

                    if (_splitType == eExportSplitType.NumEntries)
                    {
                        cfgVal = MIDConfigurationManager.AppSettings["Export_SplitNumberOfEntries"];

                        if (cfgVal == null)
                        {
                            errFound = true;
                            tmpMessage = MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired);
                            tmpMessage = tmpMessage.Replace("{0}", "Key 'Export_SplitNumberOfEntries' in config file");
                            errMessage += tmpMessage + System.Environment.NewLine;
                        }

                        _splitNumEntries = ConvertNumericConfigValue(cfgVal);

                        if (_splitNumEntries <= 0)
                        {
                            errFound = true;
                            errMessage += MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDefaultSplitNumEntries) + System.Environment.NewLine;
                        }
                    }
                    else
                    {
                        _splitNumEntries = 0;
                    }

                    if (_splitType == eExportSplitType.Merchandise)
                    {
                        cfgVal = MIDConfigurationManager.AppSettings["Export_ConcurrentProcesses"];

                        if (cfgVal == null)
                        {
                            errFound = true;
                            tmpMessage = MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired);
                            tmpMessage = tmpMessage.Replace("{0}", "Key 'Export_ConcurrentProcesses' in config file");
                            errMessage += tmpMessage + System.Environment.NewLine;
                        }

                        _concurrentProcesses = ConvertNumericConfigValue(cfgVal);

                        if (_concurrentProcesses <= 0)
                        {
                            errFound = true;
                            errMessage += MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDefaultConcurrentProcesses) + System.Environment.NewLine;
                        }
                    }
                    else
                    {
                        _concurrentProcesses = 0;
                    }

                    _createFlagFile = ConvertBoolConfigValue(MIDConfigurationManager.AppSettings["Export_CreateFlagFile"]);

                    if (_createFlagFile)
                    {
                        cfgVal = MIDConfigurationManager.AppSettings["Export_FlagFileSuffix"];

                        if (cfgVal == null)
                        {
                            errFound = true;
                            tmpMessage = MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired);
                            tmpMessage = tmpMessage.Replace("{0}", "Key 'Export_FlagFileSuffix' in config file");
                            errMessage += tmpMessage + System.Environment.NewLine;
                        }
                        else if (cfgVal.Trim().Length > 0)
                        {
                            _flagFileExtension = cfgVal.Trim();
                        }
                        else
                        {
                            errFound = true;
                            errMessage += MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDefaultFlagFileSuffix) + System.Environment.NewLine;
                        }
                    }
                    else
                    {
                        _flagFileExtension = string.Empty;
                    }

                    _createEndFile = ConvertBoolConfigValue(MIDConfigurationManager.AppSettings["Export_CreateEndFile"]);

                    if (_createEndFile)
                    {
                        cfgVal = MIDConfigurationManager.AppSettings["Export_EndFileSuffix"];

                        if (cfgVal == null)
                        {
                            errFound = true;
                            tmpMessage = MIDText.GetTextOnly(eMIDTextCode.msg_ValueRequired);
                            tmpMessage = tmpMessage.Replace("{0}", "Key 'Export_EndFileSuffix' in config file");
                            errMessage += tmpMessage + System.Environment.NewLine;
                        }
                        else if (cfgVal.Trim().Length > 0)
                        {
                            _endFileExtension = cfgVal.Trim();
                        }
                        else
                        {
                            errFound = true;
                            errMessage += MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDefaultEndFileSuffix) + System.Environment.NewLine;
                        }
                    }
                    else
                    {
                        _endFileExtension = string.Empty;
                    }

                    if (errFound)
                    {
                        aSAB.MessageCallback.HandleMessage(
                            "Errors exist in Default information for the Export Method in midsettings.config: " + System.Environment.NewLine + errMessage,
                            "Invalid Default Information",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }

                    return !errFound;
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }

            public ExportFormatOptions Copy()
            {
                ExportFormatOptions newExpFormOpt;

                try
                {
                    newExpFormOpt = new ExportFormatOptions();

                    newExpFormOpt._exportType = _exportType;
                    newExpFormOpt._delimeter = _delimeter;
                    //Begin Track #4942 - JScott - Correct problems in Export Method
                    newExpFormOpt._csvFileExtension = _csvFileExtension;
                    newExpFormOpt._dateType = _dateType;
                    //End Track #4942 - JScott - Correct problems in Export Method
                    newExpFormOpt._preinitValues = _preinitValues;
                    newExpFormOpt._filePath = _filePath;
                    newExpFormOpt._addDateStamp = _addDateStamp;
                    newExpFormOpt._addTimeStamp = _addTimeStamp;
                    newExpFormOpt._splitType = _splitType;
                    newExpFormOpt._splitNumEntries = _splitNumEntries;
                    newExpFormOpt._concurrentProcesses = _concurrentProcesses;
                    newExpFormOpt._createFlagFile = _createFlagFile;
                    newExpFormOpt._flagFileExtension = _flagFileExtension;
                    newExpFormOpt._createEndFile = _createEndFile;
                    newExpFormOpt._endFileExtension = _endFileExtension;

                    return newExpFormOpt;
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }

            private bool ConvertBoolConfigValue(string aBoolConfigValue)
            {
                try
                {
                    return Include.ConvertBoolConfigValue(aBoolConfigValue);
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }

            private int ConvertNumericConfigValue(string aNumericConfigValue)
            {
                try
                {
                    return Include.ConvertNumericConfigValue(aNumericConfigValue);
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }
        }

        public class ExtractProcessor
        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        {
            //=======
            // FIELDS
            //=======

            private OTSForecastExportMethod _exportMethod;
            private eProcessCompletionStatus _maxCompStat = eProcessCompletionStatus.None;
            private SessionAddressBlock SAB;

            //=============
            // CONSTRUCTORS
            //=============

            public ExtractProcessor(
                OTSForecastExportMethod aExportMethod)
            {
                _exportMethod = aExportMethod;
            }

            //===========
            // PROPERTIES
            //===========

            public eProcessCompletionStatus CompletionStatus
            {
                get
                {
                    return _maxCompStat;
                }
            }

            //========
            // METHODS
            //========

            public eProcessCompletionStatus Initialize()
            {
                SessionSponsor sponsor;
                IMessageCallback messageCallback;
                Exception innerE;
                string userId = null;
                string passWd = null;
                eSecurityAuthenticate authentication;

                try
                {
                    sponsor = new SessionSponsor();
                    messageCallback = new BatchMessageCallback();
                    SAB = new SessionAddressBlock(messageCallback, sponsor);

                    // ===============
                    // Create Sessions
                    // ===============

                    try
                    {
                        SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Application | (int)eServerType.Hierarchy | (int)eServerType.Store);
                    }
                    catch (Exception Ex)
                    {
                        innerE = Ex;

                        while (innerE.InnerException != null)
                        {
                            innerE = innerE.InnerException;
                        }

                        _exportMethod.SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Error,
                            "ExtractProcessor:Initialize():Error creating sessions - " + innerE.ToString(),
                            this.ToString());

                        _maxCompStat = eProcessCompletionStatus.Failed;
                        return eProcessCompletionStatus.Failed;
                    }

                    // =====
                    // Login
                    // =====

                    userId = MIDConfigurationManager.AppSettings["User"];
                    passWd = MIDConfigurationManager.AppSettings["Password"];

                    if ((userId == "" || userId == null) &&
                        (passWd == "" || passWd == null))
                    {
                        _exportMethod.SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Error,
                            "ExtractProcessor:Initialize():User and Password NOT specified",
                            this.ToString());

                        _maxCompStat = eProcessCompletionStatus.Failed;
                        return eProcessCompletionStatus.Failed;
                    }

                    authentication = SAB.ClientServerSession.UserLogin(userId, passWd, eProcesses.forecastExportThread);

                    if (authentication != eSecurityAuthenticate.UserAuthenticated)
                    {
                        _exportMethod.SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Error,
                            "ExtractProcessor:Initialize():Unable to log in with user: [" + userId + "] password: [" + passWd + "]",
                            this.ToString());

                        _maxCompStat = eProcessCompletionStatus.Failed;
                        return eProcessCompletionStatus.Failed;
                    }

                    // ===================
                    // Initialize Sessions
                    // ===================

                    SAB.ClientServerSession.Initialize();
                    SAB.ApplicationServerSession.Initialize();
                    //SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                    SAB.StoreServerSession.Initialize();
                    // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                    // StoreServerSession must be initialized before HierarchyServerSession 
                    SAB.HierarchyServerSession.Initialize();
                    // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.

                    return eProcessCompletionStatus.Successful;
                }
                catch (Exception Ex)
                {
                    _exportMethod.SAB.ApplicationServerSession.Audit.Add_Msg(
                        eMIDMessageLevel.Error,
                        "ExtractProcessor:Initialize():Error Encountered - " + Ex.ToString(),
                        this.ToString());

                    _maxCompStat = eProcessCompletionStatus.Failed;
                    return eProcessCompletionStatus.Failed;
                }
            }

            public void ProcessExtract()
            {
                ExtractData extractData;

                try
                {
                    extractData = _exportMethod.GetNextExtractData();

                    while (extractData != null)
                    {
                        extractData.ExecuteExtract(SAB);
                        _maxCompStat = (eProcessCompletionStatus)Math.Max((int)_maxCompStat, (int)extractData.CompletionStatus);

                        extractData = _exportMethod.GetNextExtractData();
                    }
                }
                catch (Exception Ex)
                {
                    _exportMethod.SAB.ApplicationServerSession.Audit.Add_Msg(
                        eMIDMessageLevel.Error,
                        "ExtractProcessor:ProcessExtract():Error Encountered - " + Ex.ToString(),
                        this.ToString());

                    _maxCompStat = eProcessCompletionStatus.Failed;
                }
                finally
                {
                    if (SAB.ClientServerSession != null && SAB.ClientServerSession.Audit != null)
                    {
                        if (_maxCompStat == eProcessCompletionStatus.Successful)
                        {
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", SAB.GetHighestAuditMessageLevel());
                        }
                        else
                        {
                            SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", SAB.GetHighestAuditMessageLevel());
                        }
                    }

                    SAB.CloseSessions();
                }
            }
        }

        public class ExtractData
        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        {
            //=======
            // FIELDS
            //=======

            private OTSForecastExportMethod _exportMethod;
            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //private SessionAddressBlock SAB;
            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
            private PlanProfile _HLPlanProf;
            //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
            private ProfileList _planProfList;
            private ProfileList _varProfList;
            private DateTime _currDate;
            private ApplicationSessionTransaction _transaction;
            private int _recordOutCount;
            private int _fileSeq;
            private string _dateStamp;
            private string _timeStamp;
            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //private bool _isRunning;
            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            private eProcessCompletionStatus _completionStatus;
            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //private DateTime _completionDateTime;
            //private Thread _thread;
            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            private string _currentOutName;
            private StreamWriter _currentOutStream;
            private int _lastNodeProfKey;
            private ArrayList _prodAmtList;
            private ArrayList _varAmtArray;
            private ProductAmountsProductAmount _prodAmount;
            private int _lastVerProfRID;
            private int _lastNodeProfRID;
            private int _lastStoreProfRID;
            private int _lastWeekProfRID;

            //=============
            // CONSTRUCTORS
            //=============

            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //public ExtractProcessor(
            public ExtractData(
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                OTSForecastExportMethod aExportMethod,
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //SessionAddressBlock aSAB,
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                PlanProfile aHLPlanProfile,
                //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                PlanProfile aPlanProfile,
                ProfileList aVarProfList,
                DateTime aCurrDate)
            {
                _exportMethod = aExportMethod;
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //SAB = aSAB;
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                _HLPlanProf = aHLPlanProfile;
                //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                _varProfList = aVarProfList;
                _currDate = aCurrDate;

                _planProfList = new ProfileList(eProfileType.Plan);
                _planProfList.Add(aPlanProfile);

                Initialize();
            }

            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //public ExtractProcessor(
            public ExtractData(
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                OTSForecastExportMethod aExportMethod,
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //SessionAddressBlock aSAB,
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                PlanProfile aHLPlanProfile,
                //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                ProfileList aPlanProfList,
                ProfileList aVarProfList,
                DateTime aCurrDate)
            {
                _exportMethod = aExportMethod;
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //SAB = aSAB;
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                _HLPlanProf = aHLPlanProfile;
                //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                _planProfList = aPlanProfList;
                _varProfList = aVarProfList;
                _currDate = aCurrDate;

                Initialize();
            }

            //===========
            // PROPERTIES
            //===========

            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //public bool isRunning
            //{
            //    get
            //    {
            //        return _isRunning;
            //    }
            //}

            //public DateTime CompletionDateTime
            //{
            //    get
            //    {
            //        return _completionDateTime;
            //    }
            //}

            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            public eProcessCompletionStatus CompletionStatus
            {
                get
                {
                    return _completionStatus;
                }
            }

            //========
            // METHODS
            //========

            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //public void WaitForThreadExit()
            //{
            //    try
            //    {
            //        if (_thread != null)
            //        {
            //            _thread.Join();
            //        }
            //    }
            //    catch (Exception err)
            //    {
            //        SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in WaitForThreadExit: " + err.Message, _sourceModule);
            //        SAB.ApplicationServerSession.Audit.Log_Exception(err);

            //        throw;
            //    }
            //}

            //public void ExecuteExtractInThread()
            //{
            //    try
            //    {
            //        _thread = new Thread(new ThreadStart(ExecuteExtract));
            //        _thread.Start();
            //    }
            //    catch (Exception err)
            //    {
            //        SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteExtractInThread: " + err.Message, _sourceModule);
            //        SAB.ApplicationServerSession.Audit.Log_Exception(err);

            //        throw;
            //    }
            //}

            //public void ExecuteExtract()
            public void ExecuteExtract(SessionAddressBlock aSAB)
            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            {
                PlanCubeGroup cubeGroup;
                PlanOpenParms openParms;
                PlanCellReference planCellRef;
                eProcessCompletionStatus extractStatus = eProcessCompletionStatus.None;

                try
                {
                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //_isRunning = true;

                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    _currentOutStream = null;
                    _recordOutCount = 0;
                    _lastNodeProfKey = Include.NoRID;
                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    _transaction = aSAB.ApplicationServerSession.CreateTransaction();
                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                    if (_exportMethod.PlanType == ePlanType.Chain)
                    {
                        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                        //SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Starting Chain Forecast Extract " + _exportMethod.Name + "...", _sourceModule);
                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Starting Chain Forecast Extract " + _exportMethod.Name + "...", _sourceModule);
                        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                        foreach (PlanProfile planProf in _planProfList)
                        {
                            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            //cubeGroup = new ChainPlanMaintCubeGroup(SAB, _transaction);
                            //openParms = FillOpenParmForPlan(planProf);
                            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            cubeGroup = new ChainPlanMaintCubeGroup(aSAB, _transaction);
                            openParms = FillOpenParmForPlan(aSAB, planProf);
                            cubeGroup.OpenCubeGroup(openParms);

                            planCellRef = (PlanCellReference)cubeGroup.GetCube(eCubeType.ChainPlanWeekDetail).CreateCellReference();

                            try
                            {
                                planCellRef[eProfileType.Version] = planProf.VersionProfile.Key;
                                planCellRef[eProfileType.HierarchyNode] = planProf.NodeProfile.Key;
                                planCellRef[eProfileType.QuantityVariable] = _transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

                                foreach (WeekProfile weekProf in openParms.WeekProfileList)
                                {
                                    planCellRef[eProfileType.Week] = weekProf.Key;

                                    foreach (VariableProfile varProf in _varProfList)
                                    {
                                        planCellRef[eProfileType.Variable] = varProf.Key;

                                        if (_exportMethod.PreinitValues)
                                        {
                                            //Begin Track #6151 - JScott - CRITICAL\URGENT - Net Rec & Net Rec Cost not exporting using export method
                                            //if (!_exportMethod.ExcludeZeroValues || (_exportMethod.ExcludeZeroValues && planCellRef.PreInitCellValue != 0))
                                            if (!_exportMethod.ExcludeZeroValues || planCellRef.PreInitCellValue != 0)
                                            //End Track #6151 -- CRITICAL\URGENT - Net Rec & Net Rec Cost not exporting using export method
                                            {
                                                //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                                                //											WriteOutputRecord(planProf.VersionProfile, openParms.ChainHLPlanProfile.NodeProfile, planProf.NodeProfile, null, weekProf, varProf, planCellRef.PreInitCellValue);
                                                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                                                //WriteOutputRecord(planProf.VersionProfile, _HLPlanProf.NodeProfile, planProf.NodeProfile, null, weekProf, varProf, planCellRef.PreInitCellValue);
                                                WriteOutputRecord(aSAB, planProf.VersionProfile, _HLPlanProf.NodeProfile, planProf.NodeProfile, null, weekProf, varProf, planCellRef, planCellRef.PreInitCellValue);
                                                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                                                //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                                            }
                                        }
                                        else
                                        {
                                            //Begin Track #6151 - JScott - CRITICAL\URGENT - Net Rec & Net Rec Cost not exporting using export method
                                            //if (!_exportMethod.ExcludeZeroValues || (_exportMethod.ExcludeZeroValues && planCellRef.PreInitCellValue != 0))
                                            if (!_exportMethod.ExcludeZeroValues || planCellRef.CurrentCellValue != 0)
                                            //End Track #6151 -- CRITICAL\URGENT - Net Rec & Net Rec Cost not exporting using export method
                                            {
                                                //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                                                //											WriteOutputRecord(planProf.VersionProfile, openParms.ChainHLPlanProfile.NodeProfile, planProf.NodeProfile, null, weekProf, varProf, planCellRef.CurrentCellValue);
                                                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                                                //WriteOutputRecord(planProf.VersionProfile, _HLPlanProf.NodeProfile, planProf.NodeProfile, null, weekProf, varProf, planCellRef.CurrentCellValue);
                                                WriteOutputRecord(aSAB, planProf.VersionProfile, _HLPlanProf.NodeProfile, planProf.NodeProfile, null, weekProf, varProf, planCellRef, planCellRef.CurrentCellValue);
                                                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                                                //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception err)
                            {
                                string message = err.ToString();
                                throw;
                            }
                            finally
                            {
                                cubeGroup.CloseCubeGroup();
                            }
                        }
                    }
                    else
                    {
                        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                        //SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Starting Store Forecast Extract " + _exportMethod.Name + "...", _sourceModule);
                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Starting Store Forecast Extract " + _exportMethod.Name + "...", _sourceModule);
                        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                        foreach (PlanProfile planProf in _planProfList)
                        {
                            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            //cubeGroup = new StorePlanMaintCubeGroup(SAB, _transaction);
                            //openParms = FillOpenParmForPlan(planProf);
                            cubeGroup = new StorePlanMaintCubeGroup(aSAB, _transaction);
                            openParms = FillOpenParmForPlan(aSAB, planProf);
                            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            cubeGroup.OpenCubeGroup(openParms);
                            // BEGIN Issue 5727 stodd
                            if (!cubeGroup.SetStoreFilter(openParms.FilterRID, cubeGroup))
                            {
                                FilterData storeFilterData = new FilterData();
                                string msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
                                msg = msg.Replace("{0}", storeFilterData.FilterGetName(openParms.FilterRID));
                                string suffix = ". Method " + this._exportMethod.Name + ". ";
                                string auditMsg = msg + suffix;
                                aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.ToString());
                                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
                            }
                            // END issue 5727

                            planCellRef = (PlanCellReference)cubeGroup.GetCube(eCubeType.StorePlanWeekDetail).CreateCellReference();

                            try
                            {
                                planCellRef[eProfileType.Version] = planProf.VersionProfile.Key;
                                planCellRef[eProfileType.HierarchyNode] = planProf.NodeProfile.Key;
                                planCellRef[eProfileType.QuantityVariable] = _transaction.PlanComputations.PlanQuantityVariables.ValueQuantity.Key;

                                foreach (WeekProfile weekProf in openParms.WeekProfileList)
                                {
                                    planCellRef[eProfileType.Week] = weekProf.Key;
                                    ProfileList strProfList = cubeGroup.GetFilteredProfileList(eProfileType.Store);

                                    foreach (StoreProfile storeProf in strProfList)
                                    {
                                        planCellRef[eProfileType.Store] = storeProf.Key;

                                        foreach (VariableProfile varProf in _varProfList)
                                        {
                                            planCellRef[eProfileType.Variable] = varProf.Key;

                                            if (_exportMethod.PreinitValues)
                                            {
                                                //Begin Track #6151 - JScott - CRITICAL\URGENT - Net Rec & Net Rec Cost not exporting using export method
                                                //if (!_exportMethod.ExcludeZeroValues || (_exportMethod.ExcludeZeroValues && planCellRef.PreInitCellValue != 0))
                                                if (!_exportMethod.ExcludeZeroValues || planCellRef.PreInitCellValue != 0)
                                                //End Track #6151 -- CRITICAL\URGENT - Net Rec & Net Rec Cost not exporting using export method
                                                {
                                                    //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                                                    //												WriteOutputRecord(planProf.VersionProfile, openParms.StoreHLPlanProfile.NodeProfile, planProf.NodeProfile, storeProf, weekProf, varProf, planCellRef.PreInitCellValue);
                                                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                                                    //WriteOutputRecord(planProf.VersionProfile, _HLPlanProf.NodeProfile, planProf.NodeProfile, storeProf, weekProf, varProf, planCellRef.PreInitCellValue);
                                                    WriteOutputRecord(aSAB, planProf.VersionProfile, _HLPlanProf.NodeProfile, planProf.NodeProfile, storeProf, weekProf, varProf, planCellRef, planCellRef.PreInitCellValue);
                                                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                                                    //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                                                }
                                            }
                                            else
                                            {
                                                //Begin Track #6151 - JScott - CRITICAL\URGENT - Net Rec & Net Rec Cost not exporting using export method
                                                //if (!_exportMethod.ExcludeZeroValues || (_exportMethod.ExcludeZeroValues && planCellRef.PreInitCellValue != 0))
                                                if (!_exportMethod.ExcludeZeroValues || planCellRef.CurrentCellValue != 0)
                                                //End Track #6151 -- CRITICAL\URGENT - Net Rec & Net Rec Cost not exporting using export method
                                                {
                                                    //Begin Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                                                    //												WriteOutputRecord(planProf.VersionProfile, openParms.StoreHLPlanProfile.NodeProfile, planProf.NodeProfile, storeProf, weekProf, varProf, planCellRef.CurrentCellValue);
                                                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                                                    //WriteOutputRecord(planProf.VersionProfile, _HLPlanProf.NodeProfile, planProf.NodeProfile, storeProf, weekProf, varProf, planCellRef.CurrentCellValue);
                                                    WriteOutputRecord(aSAB, planProf.VersionProfile, _HLPlanProf.NodeProfile, planProf.NodeProfile, storeProf, weekProf, varProf, planCellRef, planCellRef.CurrentCellValue);
                                                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                                                    //End Track #5175 - JScott - Suffixes for End file not consistent with Flag file
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception err)
                            {
                                string message = err.ToString();
                                throw;
                            }
                            finally
                            {
                                cubeGroup.CloseCubeGroup();
                            }
                        }
                    }

                    extractStatus = eProcessCompletionStatus.Successful;
                }
                catch (Exception err)
                {
                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteExtract: " + err.Message, _sourceModule);
                    //SAB.ApplicationServerSession.Audit.Log_Exception(err);
                    aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteExtract: " + err.Message, _sourceModule);
                    aSAB.ApplicationServerSession.Audit.Log_Exception(err);
                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                    extractStatus = eProcessCompletionStatus.Failed;
                }
                finally
                {
                    CloseOutputFile();

                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //_isRunning = false;
                    //_completionDateTime = DateTime.Now;
                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    _completionStatus = extractStatus;

                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //SAB.ApplicationServerSession.Audit.Add_Msg(
                    aSAB.ApplicationServerSession.Audit.Add_Msg(
                        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                        eMIDMessageLevel.Information,
                        "Completed Forecast Extract " + _exportMethod.Name + " with status " + MIDText.GetTextOnly((int)extractStatus),
                        _sourceModule);
                }
            }

            private void Initialize()
            {
                try
                {
                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //_transaction = SAB.ApplicationServerSession.CreateTransaction();
                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    _fileSeq = 0;
                    _dateStamp = _currDate.ToString("MMddyy");
                    _timeStamp = _currDate.ToString("HHmmss");
                    _lastVerProfRID = Include.NoRID;
                    _lastNodeProfRID = Include.NoRID;
                    _lastStoreProfRID = Include.NoRID;
                    _lastWeekProfRID = Include.NoRID;
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }

            private void WriteOutputRecord(
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                SessionAddressBlock aSAB,
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                VersionProfile aVerProf,
                HierarchyNodeProfile aHLNodeProf,
                HierarchyNodeProfile aCurrNodeProf,
                StoreProfile aStoreProf,
                WeekProfile aWeekProf,
                VariableProfile aVarProf,
                //Begin BonTon Calcs - JScott - Add Display Precision
                PlanCellReference aPlanCellRef,
                //End BonTon Calcs - JScott - Add Display Precision
                double aValue)
            {
                //Begin BonTon Calcs - JScott - Add Display Precision
                //string fmtBase = "##############################";
                //End BonTon Calcs - JScott - Add Display Precision
                string fmtDecimals = "000000000000";
                //End Track #5133
                string outStr;
                //Begin BonTon Calcs - JScott - Add Display Precision
                //string fmtStr;
                PlanWaferCell waferCell;
                //End BonTon Calcs - JScott - Add Display Precision
                string storeName;
                DayProfile firstDayProf;
                ProductAmountsProductAmountVariableAmount varAmt;

                try
                {
                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //OpenOutputFile(aHLNodeProf, aCurrNodeProf);
                    OpenOutputFile(aSAB, aHLNodeProf, aCurrNodeProf);
                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                    firstDayProf = (DayProfile)aWeekProf.Days[0];

                    if (_exportMethod.ExportType == eExportType.CSV)
                    {
                        if (aStoreProf != null)
                        {
                            storeName = aStoreProf.StoreId;
                        }
                        else
                        {
                            storeName = string.Empty;
                        }

                        //Begin BonTon Calcs - JScott - Add Display Precision
                        //if (aVarProf.NumDecimals > 0)
                        //{
                        //    //Begin Track #5133 - JSmith - value is blank in CSV format
                        //    //							fmtStr = fmtBase + '.' + fmtBase.Substring(0, aVarProf.NumDecimals);
                        //    fmtStr = fmtBase + "0." + fmtDecimals.Substring(0, aVarProf.NumDecimals);
                        //    //End Track #5133
                        //}
                        //else
                        //{
                        //    fmtStr = string.Empty;
                        //}

                        //End BonTon Calcs - JScott - Add Display Precision

                        //Begin Track #4942 - JScott - Correct problems in Export Method
                        //						outStr =
                        //							"W" + _exportMethod.Delimeter +
                        //							aVerProf.Description + _exportMethod.Delimeter +
                        //							aCurrNodeProf.NodeID + _exportMethod.Delimeter +
                        //							storeName + _exportMethod.Delimeter +
                        //							"C" + _exportMethod.Delimeter +
                        //							firstDayProf.Date.ToString("MM/dd/yyyy") + _exportMethod.Delimeter +
                        //							aVarProf.VariableName + _exportMethod.Delimeter +
                        //							((decimal)aValue).ToString(fmtStr);
                        outStr =
                            "W" + _exportMethod.Delimeter +
                            aVerProf.Description + _exportMethod.Delimeter +
                            //Begin Track #5085 - JScott - Color and Size IDs not unique
                            //							aCurrNodeProf.NodeID + _exportMethod.Delimeter +
                            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            //SAB.HierarchyServerSession.GetQualifiedNodeID(aCurrNodeProf.Key) + _exportMethod.Delimeter +
                            aCurrNodeProf.QualifiedNodeID + _exportMethod.Delimeter +
                            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            //End Track #5085 - JScott - Color and Size IDs not unique
                            storeName + _exportMethod.Delimeter;

                        if (_exportMethod.DateType == eExportDateType.Calendar)
                        {
                            outStr += "C" + _exportMethod.Delimeter +
                                firstDayProf.Date.ToString("MM/dd/yyyy") + _exportMethod.Delimeter;
                        }
                        else
                        {
                            outStr += "F" + _exportMethod.Delimeter +
                                aWeekProf.YearWeek.ToString() + _exportMethod.Delimeter;
                        }

                        //Begin BonTon Calcs - JScott - Add Display Precision
                        //outStr += aVarProf.VariableName + _exportMethod.Delimeter +
                        //    ((decimal)aValue).ToString(fmtStr);
                        //Begin Modification - JScott - Add Scaling Decimals
                        //waferCell = new PlanWaferCell(aPlanCellRef, aValue, 1, 1, false);
                        waferCell = new PlanWaferCell(aPlanCellRef, aValue, "1", "1", false);
                        //End Modification - JScott - Add Scaling Decimals

                        outStr += aVarProf.VariableName + _exportMethod.Delimeter + waferCell.ValueAsString;
                        //End BonTon Calcs - JScott - Add Display Precision

                        _currentOutStream.WriteLine(outStr);
                    }
                    else
                    {
                        if (aVerProf.Key != _lastVerProfRID || aCurrNodeProf.Key != _lastNodeProfRID ||
                            (aStoreProf != null && aStoreProf.Key != _lastStoreProfRID) || aWeekProf.Key != _lastWeekProfRID)
                        {
                            if (_prodAmount != null && _varAmtArray != null && _varAmtArray.Count > 0)
                            {
                                _prodAmount.VariableAmount = new ProductAmountsProductAmountVariableAmount[_varAmtArray.Count];
                                _varAmtArray.CopyTo(0, _prodAmount.VariableAmount, 0, _varAmtArray.Count);
                                _prodAmtList.Add(_prodAmount);
                            }

                            _varAmtArray = new ArrayList();

                            _prodAmount = new ProductAmountsProductAmount();
                            _prodAmount.Period = ProductAmountsProductAmountPeriod.Week;
                            _prodAmount.Version = aVerProf.Description;
                            //Begin Track #5085 - JScott - Color and Size IDs not unique
                            //							_prodAmount.Product = aCurrNodeProf.NodeID;
                            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            //_prodAmount.Product = SAB.HierarchyServerSession.GetQualifiedNodeID(aCurrNodeProf.Key);
                            _prodAmount.Product = aCurrNodeProf.QualifiedNodeID;
                            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            //End Track #5085 - JScott - Color and Size IDs not unique

                            if (aStoreProf != null)
                            {
                                _prodAmount.Store = aStoreProf.StoreId;
                            }

                            if (_exportMethod.DateType == eExportDateType.Calendar)
                            {
                                _prodAmount.DateType = ProductAmountsProductAmountDateType.Calendar;
                                _prodAmount.Date = firstDayProf.Date.ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                _prodAmount.DateType = ProductAmountsProductAmountDateType.Fiscal;
                                _prodAmount.Date = aWeekProf.YearWeek.ToString();
                            }

                            _lastVerProfRID = aVerProf.Key;
                            _lastNodeProfRID = aCurrNodeProf.Key;

                            if (aStoreProf != null)
                            {
                                _lastStoreProfRID = aStoreProf.Key;
                            }

                            _lastWeekProfRID = aWeekProf.Key;
                        }

                        varAmt = new ProductAmountsProductAmountVariableAmount();
                        varAmt.Variable = aVarProf.VariableName;
                        //Begin BonTon Calcs - JScott - Add Display Precision
                        //varAmt.Amount = (double)((decimal)aValue);
                        //Begin Modification - JScott - Add Scaling Decimals
                        //waferCell = new PlanWaferCell(aPlanCellRef, aValue, 1, 1, false);
                        waferCell = new PlanWaferCell(aPlanCellRef, aValue, "1", "1", false);
                        //End Modification - JScott - Add Scaling Decimals
                        varAmt.Amount = Convert.ToString(waferCell.ValueAsString);
                        //End BonTon Calcs - JScott - Add Display Precision
                        _varAmtArray.Add(varAmt);
                    }

                    _recordOutCount++;
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }

            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //private PlanOpenParms FillOpenParmForPlan(PlanProfile aPlanProf)
            private PlanOpenParms FillOpenParmForPlan(SessionAddressBlock aSAB, PlanProfile aPlanProf)
            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            {
                PlanOpenParms openParms;

                try
                {
                    if (_exportMethod.PlanType == ePlanType.Chain)
                    {
                        openParms = new PlanOpenParms(ePlanSessionType.ChainSingleLevel, "Default");
                    }
                    else
                    {
                        openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, "Default");
                    }

                    if (_exportMethod.GlobalUserType == eGlobalUserType.User)
                    {
                        openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsUserExport);
                    }
                    else
                    {
                        openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsGlobalExport);
                    }

                    openParms.FunctionSecurityProfile.SetAllowUpdate();

                    openParms.StoreHLPlanProfile.VersionProfile = aPlanProf.VersionProfile;
                    openParms.StoreHLPlanProfile.NodeProfile = aPlanProf.NodeProfile;
                    openParms.ChainHLPlanProfile.VersionProfile = aPlanProf.VersionProfile;
                    openParms.ChainHLPlanProfile.NodeProfile = aPlanProf.NodeProfile;

                    //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                    //openParms.DateRangeProfile = SAB.ApplicationServerSession.Calendar.GetDateRange(_exportMethod.DateRangeRID);
                    openParms.DateRangeProfile = aSAB.ApplicationServerSession.Calendar.GetDateRange(_exportMethod.DateRangeRID);
                    //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                    openParms.StoreGroupRID = Include.AllStoreGroupRID;
                    openParms.FilterRID = _exportMethod.FilterRID;

                    openParms.IneligibleStores = _exportMethod.ShowIneligible;
                    openParms.SimilarStores = false;

                    return openParms;
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }

            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //private void OpenOutputFile(HierarchyNodeProfile aHLNodeProf, HierarchyNodeProfile aCurrNodeProf)
            private void OpenOutputFile(SessionAddressBlock aSAB, HierarchyNodeProfile aHLNodeProf, HierarchyNodeProfile aCurrNodeProf)
            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            {
                try
                {
                    if (_exportMethod.SplitType == eExportSplitType.NumEntries && _recordOutCount >= _exportMethod.SplitNumEntries)
                    {
                        CloseOutputFile();
                        _recordOutCount = 0;
                    }
                    else if (_exportMethod.SplitType == eExportSplitType.Merchandise && aCurrNodeProf.Key != _lastNodeProfKey)
                    {
                        CloseOutputFile();
                        _lastNodeProfKey = aCurrNodeProf.Key;
                    }

                    if (_currentOutStream == null)
                    {
                        if (_exportMethod.ExportType == eExportType.XML)
                        {
                            _prodAmtList = new ArrayList();
                        }

                        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                        //_currentOutName = GenerateFileName(aHLNodeProf, aCurrNodeProf);
                        _currentOutName = GenerateFileName(aSAB, aHLNodeProf, aCurrNodeProf);
                        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.



                        _currentOutStream = new StreamWriter(_currentOutName, false);
                    }
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }

            private void CloseOutputFile()
            {
                ProductAmounts prodAmts;
                XmlSerializer serializer;

                try
                {
                    if (_currentOutStream != null)
                    {
                        if (_exportMethod.ExportType == eExportType.CSV)
                        {
                            _currentOutStream.Close();
                            _currentOutStream = null;
                        }
                        else
                        {
                            if (_prodAmount != null && _varAmtArray != null && _varAmtArray.Count > 0)
                            {
                                _prodAmount.VariableAmount = new ProductAmountsProductAmountVariableAmount[_varAmtArray.Count];
                                _varAmtArray.CopyTo(0, _prodAmount.VariableAmount, 0, _varAmtArray.Count);
                                _prodAmtList.Add(_prodAmount);
                            }

                            _varAmtArray = new ArrayList();

                            prodAmts = new ProductAmounts();
                            prodAmts.ProductAmount = new ProductAmountsProductAmount[_prodAmtList.Count];
                            _prodAmtList.CopyTo(0, prodAmts.ProductAmount, 0, _prodAmtList.Count);

                            serializer = new XmlSerializer(typeof(ProductAmounts));
                            serializer.Serialize(_currentOutStream, prodAmts);

                            _currentOutStream.Close();
                            _currentOutStream = null;
                        }
                    }

                    if (_exportMethod.CreateFlagFile)
                    {
                        System.IO.File.Create(_currentOutName + ((_exportMethod.FlagFileExtension[0] == '.') ? "" : ".") + _exportMethod.FlagFileExtension).Close();
                    }
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }

            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            //private string GenerateFileName(HierarchyNodeProfile aHLNodeProf, HierarchyNodeProfile aCurrNodeProf)
            private string GenerateFileName(SessionAddressBlock aSAB, HierarchyNodeProfile aHLNodeProf, HierarchyNodeProfile aCurrNodeProf)
            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            {
                string fileName;

                try
                {
                    _fileSeq++;

                    //Begin Test Track #695 - APicchetti - Check for and replace special characters in export file name
                    string strCurrentOutName = _exportMethod.Name;
                    for (int intOutNameChr = 0; intOutNameChr < strCurrentOutName.Length; intOutNameChr++)
                    {
                        string _charToReplace = strCurrentOutName.Substring(intOutNameChr, 1);
                        switch (_charToReplace)
                        {
                            case @"\":   //forward slash (\)
                            case "/":
                            case ":":
                            case "*":
                            case "?":
                            case "\"":  // double quote (")
                            case "<":
                            case ">":
                            case "|":


                                strCurrentOutName = strCurrentOutName.Replace(_charToReplace, "_");
                                break;
                        }
                    }

                    //fileName = _exportMethod.FilePath + "/" + _exportMethod.Name;
                    fileName = _exportMethod.FilePath + "/" + strCurrentOutName;

                    //End tt#695

                    switch (_exportMethod.SplitType)
                    {
                        case eExportSplitType.Merchandise:
                            //Begin Track #5085 - JScott - Color and Size IDs not unique
                            //							fileName += "_" + aCurrNodeProf.NodeID;
                            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            //fileName += "_" + SAB.HierarchyServerSession.GetQualifiedNodeID(aCurrNodeProf.Key, "-");
                            fileName += "_" + Include.FormatFileName(aCurrNodeProf.QualifiedNodeID);
                            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            //End Track #5085 - JScott - Color and Size IDs not unique
                            break;

                        case eExportSplitType.NumEntries:
                        case eExportSplitType.None:
                            //Begin Track #5085 - JScott - Color and Size IDs not unique
                            //							fileName += "_" + aHLNodeProf.NodeID;
                            //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            //fileName += "_" + SAB.HierarchyServerSession.GetQualifiedNodeID(aHLNodeProf.Key, "-");
                            fileName += "_" + Include.FormatFileName(aHLNodeProf.QualifiedNodeID);
                            //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                            //End Track #5085 - JScott - Color and Size IDs not unique
                            break;
                    }

                    if (_exportMethod.AddDateStamp)
                    {
                        fileName += "_" + _dateStamp;
                    }

                    if (_exportMethod.AddTimeStamp)
                    {
                        fileName += "_" + _timeStamp;
                    }

                    if (_exportMethod.SplitType == eExportSplitType.NumEntries)
                    {
                        fileName += "_" + _fileSeq.ToString("0000");
                    }

                    if (_exportMethod.ExportType == eExportType.XML)
                    {
                        fileName += ".xml";
                    }
                    else
                    {
                        if (_exportMethod.CSVFileExtension != null &&
                            _exportMethod.CSVFileExtension.Length > 0)
                        {
                            fileName += ((_exportMethod.CSVFileExtension[0] == '.') ? "" : ".") + _exportMethod.CSVFileExtension;
                        }
                        else
                        {
                            fileName += ".csv";
                        }
                    }

                    return fileName;
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag identifying if the user can update the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToUpdate(Session aSession, int aUserRID)
        {
            try
            {
                if (VersionRID == Include.NoRID ||
                    HierarchyRID == Include.NoRID)
                {
                    return false;
                }

                VersionSecurityProfile versionSecurity;
                HierarchyNodeSecurityProfile hierNodeSecurity;
                //Begin Track #5852 - stodd
                if (_planType == ePlanType.Chain)
                {
                    versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Chain);
                    hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierarchyRID, (int)eSecurityTypes.Chain);
                }
                else
                {
                    versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Store);
                    hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierarchyRID, (int)eSecurityTypes.Store);
                }
                //End Track #5852 - stodd

                //Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
                //if (!versionSecurity.AllowUpdate)
                if (!versionSecurity.AllowView)
                //End Track #5719 - JScott - Export Method will not allow me to export Act Version
                {
                    return false;
                }
                //Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
                //if (!hierNodeSecurity.AllowUpdate)
                if (!hierNodeSecurity.AllowView)
                //End Track #5719 - JScott - Export Method will not allow me to export Act Version
                {
                    return false;
                }
                return true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin Track #5852 stodd
        public bool StoreAuthorizedToView(Session aSession, int aUserRID)
        {
            try
            {
                if (VersionRID == Include.NoRID ||
                    HierarchyRID == Include.NoRID)
                {
                    return true;
                }

                VersionSecurityProfile versionSecurity;
                HierarchyNodeSecurityProfile hierNodeSecurity;

                versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Store);
                hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierarchyRID, (int)eSecurityTypes.Store);
                if (!versionSecurity.AllowView)
                {
                    return false;
                }
                if (!hierNodeSecurity.AllowView)
                {
                    return false;
                }
                return true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public bool ChainAuthorizedToView(Session aSession, int aUserRID)
        {
            try
            {
                if (VersionRID == Include.NoRID ||
                    HierarchyRID == Include.NoRID)
                {
                    return true;
                }

                VersionSecurityProfile versionSecurity;
                HierarchyNodeSecurityProfile hierNodeSecurity;

                versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Chain);
                hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierarchyRID, (int)eSecurityTypes.Chain);
                if (!versionSecurity.AllowView)
                {
                    return false;
                }
                if (!hierNodeSecurity.AllowView)
                {
                    return false;
                }
                return true;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End Track #5852 stodd

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalExport);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserExport);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            ROOverrideLowLevel overrideLowLevel = new ROOverrideLowLevel();
            overrideLowLevel.OverrideLowLevelsModel = GetName.GetOverrideLowLevelsModel(OverrideLowLevelRid, SAB); //CustomOLL_RID;
            overrideLowLevel.OverrideLowLevelsModelList = BuildOverrideLowLevelList(
                overrideLowLevelRid: OverrideLowLevelRid,
                customOverrideLowLevelRid: CustomOLL_RID
                );
            overrideLowLevel.AssociatedCustomModelId = CustomOLL_RID;

            if (CustomOLL_RID > Include.NoRID
                && CustomOLL_RID == OverrideLowLevelRid)
            {
                overrideLowLevel.IsCustomModel = true;
            }

            ROLevelInformation lowLevel = new ROLevelInformation();
            lowLevel.LevelType = (eROLevelsType)LowLevelsType;
            lowLevel.LevelOffset = LowLevelOffset;
            lowLevel.LevelSequence = LowLevelSequence;
            lowLevel.LevelValue = GetName.GetLevelName(
               levelType: (eROLevelsType)LowLevelsType,
               levelSequence: LowLevelSequence,
               levelOffset: LowLevelOffset,
               SAB: SAB
               );

            ApplicationSessionTransaction _applicationSessionTransaction = SAB.ApplicationServerSession.CreateTransaction();
            ArrayList al = _applicationSessionTransaction.PlanComputations.PlanVariables.GetVariableGroupings();
            ROVariableGroupings variableList = VariableGroupings.BuildVariableGroupings(ePlanType.Store, SelectableVariableList, al);

            ROPlanningForecastExportProperties method = new ROPlanningForecastExportProperties(
                kvpMethod: GetName.GetMethod(method: this),
                sDescription: Method_Description,
                iUserKey: User_RID,
                planType: PlanType,
                kvpMerchandise: GetName.GetMerchandiseName(HierarchyRID, SAB),
                kvpVersion: GetName.GetVersion(VersionRID, SAB),
                kvpTimePeriod: GetName.GetCalendarDateRange(DateRangeRID, SAB),
                kvpFilter: GetName.GetFilterName(FilterRID),
                bIsLowLevels: LowLevels,
                bIsLowLevelsOnly: LowLevelsOnly,
                lowLevel: lowLevel,
                overrideLowLevel: overrideLowLevel,
                bIsExtractIneligibleStores: ShowIneligible,
                alVariableList: variableList,//SelectableVariableList,
                bUseDefaultSettings: UseDefaultSettings,
                exportType: ExportType,
                sDelimiter: Delimeter,
                sCSVFileExtension: CSVFileExtension,
                exportDateType: DateType,
                bIsExtractPreInitValues: PreinitValues,
                bIsExcludeZeroValues: ExcludeZeroValues,
                sFilePath: FilePath,
                bIsDateStamp: AddDateStamp,
                bIsTimeStamp: AddTimeStamp,
                exportSplitType: SplitType,
                iSplitNumEntries: SplitNumEntries,
                iConcurrentProcesses: ConcurrentProcesses,
                bIsCreateFlagfile: CreateFlagFile,
                bIsCreateEndfile: CreateEndFile,
                sFlagFileExtension: FlagFileExtension,
                sEndFileExtension: EndFileExtension,
                isTemplate: Template_IND
              );
            return method;
        }

        override public ROOverrideLowLevel MethodGetOverrideModelList(
            ROOverrideLowLevel overrideLowLevel,
            out bool successful,
            ref string message
            )
        {
            successful = true;

            _overrideLowLevelRid = overrideLowLevel.OverrideLowLevelsModel.Key;
            if (overrideLowLevel.IsCustomModel)
            {
                CustomOLL_RID = overrideLowLevel.OverrideLowLevelsModel.Key;
            }
            else
            {
                CustomOLL_RID = Include.NoRID;
            }

            overrideLowLevel.OverrideLowLevelsModel = GetName.GetOverrideLowLevelsModel(OverrideLowLevelRid, SAB);
            overrideLowLevel.OverrideLowLevelsModelList = BuildOverrideLowLevelList(
                overrideLowLevelRid: OverrideLowLevelRid,
                customOverrideLowLevelRid: CustomOLL_RID
                );
            overrideLowLevel.AssociatedCustomModelId = CustomOLL_RID;

            if (CustomOLL_RID > Include.NoRID
                && CustomOLL_RID == OverrideLowLevelRid)
            {
                overrideLowLevel.IsCustomModel = true;
            }

            return overrideLowLevel;
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            ROPlanningForecastExportProperties roPlanningForecastExportProperties = (ROPlanningForecastExportProperties)methodProperties;
            try
            {
                Template_IND = methodProperties.IsTemplate;
                ArrayList alVariableList = new ArrayList();

                if (_varProfList == null)
                {
                    _varProfList = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanVariables.VariableProfileList;
                }

                int variableSequence = 0;
                int nextSequence = 0;
                foreach (ROVariableGrouping grouping in roPlanningForecastExportProperties.VariableList.VariableGrouping)
                {
                    foreach (ROVariable variable in grouping.Variables)
                    {
                        variableSequence = -1;
                        if (variable.IsSelectable)
                        {
                            variableSequence = nextSequence;
                            ++nextSequence;
                        }
                        alVariableList.Add(new RowColProfileHeader(variable.Name, true, variableSequence, (VariableProfile)_varProfList.FindKey(variable.Number)));
                    }
                }

                PlanType = roPlanningForecastExportProperties.PlanType;
                HierarchyRID = roPlanningForecastExportProperties.Merchandise.Key;
                VersionRID = roPlanningForecastExportProperties.Version.Key;
                DateRangeRID = roPlanningForecastExportProperties.TimePeriod.Key;
                FilterRID = roPlanningForecastExportProperties.Filter.Key;
                LowLevels = roPlanningForecastExportProperties.IsLowLevels;
                LowLevelsOnly = roPlanningForecastExportProperties.IsLowLevelsOnly;
                LowLevelsType = (eLowLevelsType)roPlanningForecastExportProperties.LowLevel.LevelType;
                LowLevelOffset = roPlanningForecastExportProperties.LowLevel.LevelOffset;
                LowLevelSequence = roPlanningForecastExportProperties.LowLevel.LevelSequence;
                OverrideLowLevelRid = roPlanningForecastExportProperties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                if (roPlanningForecastExportProperties.OverrideLowLevel.IsCustomModel)
                {
                    CustomOLL_RID = roPlanningForecastExportProperties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                }
                else
                {
                    CustomOLL_RID = Include.NoRID;
                }
                ShowIneligible = roPlanningForecastExportProperties.IsExtractIneligibleStores;
                SelectableVariableList = alVariableList;                //variable list to be added.
                UseDefaultSettings = roPlanningForecastExportProperties.UseDefaultSettings;
                ExportType = roPlanningForecastExportProperties.ExportType;
                Delimeter = roPlanningForecastExportProperties.Delimiter;
                CSVFileExtension = roPlanningForecastExportProperties.CSVFileExtension;
                DateType = roPlanningForecastExportProperties.ExportDateType;
                PreinitValues = roPlanningForecastExportProperties.IsExtractPreInitValues;
                ExcludeZeroValues = roPlanningForecastExportProperties.IsExcludeZeroValues;
                FilePath = roPlanningForecastExportProperties.FilePath;
                AddDateStamp = roPlanningForecastExportProperties.IsDateStamp;
                AddTimeStamp = roPlanningForecastExportProperties.IsTimeStamp;
                SplitType = roPlanningForecastExportProperties.SplitType;
                SplitNumEntries = roPlanningForecastExportProperties.SplitNumEntries;
                ConcurrentProcesses = roPlanningForecastExportProperties.ConcurrentProcesses;
                CreateFlagFile = roPlanningForecastExportProperties.IsCreateFlagfile;
                CreateEndFile = roPlanningForecastExportProperties.IsCreateEndfile;
                FlagFileExtension = roPlanningForecastExportProperties.FlagFileExtension;
                EndFileExtension = roPlanningForecastExportProperties.EndFileExtension;


                return true;
            }
            catch
            {
                return false;
            }
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }
}
