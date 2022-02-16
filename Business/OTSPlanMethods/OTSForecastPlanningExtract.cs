using System;
using System.Collections;
using System.Collections.Generic;
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

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for OTSForecastPlanningExtractMethod.
	/// </summary>

	public class OTSForecastPlanningExtractMethod : OTSPlanBaseMethod
	{
		//==========
		// CONSTANTS
		//==========

		const string _sourceModule = "OTSForecastPlanningExtract.cs";

		//=======
		// FIELDS
		//=======

		private ProfileList _masterVersionProfList;
		private ProfileList _varProfList;
        private ProfileList _totalVarProfList;
        private Stack _extractDataStack;
		private OTSForecastPlanningExtractMethodData _dlPlanningExtractMethod;
		private bool _methodValid = true;

		private int _merchandiseRID;
		private int _versionRID;
		private int _dateRangeRID;
		private int _filterRID;
		private bool _chain;
        private bool _store;
        private bool _attributeSet;
        private int _attributeRID;
        private bool _lowLevels;
		private bool _lowLevelsOnly;
		private eLowLevelsType _lowLevelsType;
		private int _lowLevelSequence;
		private int _lowLevelOffset;
		private bool _showIneligible;
        private bool _excludeZeroValues;
        private int _concurrentProcesses;

        private ArrayList _selectableVariableList;
        private ArrayList _selectableTimetimeTotalVariableList;

        private int _nodeOverrideRid = Include.NoRID;
		private int _versionOverrideRid = Include.NoRID;
		private int _overrideLowLevelRid = Include.NoRID;
        private DateTime _updateDate;
        private DateTime _extractDate;
		
		//=============
		// CONSTRUCTORS
		//=============

		public OTSForecastPlanningExtractMethod(SessionAddressBlock SAB, int aMethodRID)
			: base(SAB, aMethodRID, eMethodType.PlanningExtract, eProfileType.MethodPlanningExtract)
		{
			_masterVersionProfList = SAB.ClientServerSession.GetUserForecastVersions();
			_varProfList = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanVariables.VariableProfileList;
            _totalVarProfList = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanTimeTotalVariables.TimeTotalVariableProfileList;
            _extractDataStack = new Stack();

			if (base.Filled)
			{
				_dlPlanningExtractMethod = new OTSForecastPlanningExtractMethodData(_varProfList, aMethodRID, eChangeType.populate);

				_merchandiseRID = _dlPlanningExtractMethod.HierarchyRID;
				_versionRID = _dlPlanningExtractMethod.VersionRID;
				_dateRangeRID = _dlPlanningExtractMethod.DateRangeRID;
				_filterRID = _dlPlanningExtractMethod.FilterRID;
				_chain = _dlPlanningExtractMethod.Chain;
                _store = _dlPlanningExtractMethod.Store;
                _attributeSet = _dlPlanningExtractMethod.AttributeSet;
                _attributeRID = _dlPlanningExtractMethod.AttributeRID;
                _chain = _dlPlanningExtractMethod.Chain;
                _lowLevels = _dlPlanningExtractMethod.LowLevels;
				_lowLevelsOnly = _dlPlanningExtractMethod.LowLevelsOnly;
				_lowLevelsType = _dlPlanningExtractMethod.LowLevelsType;
				_lowLevelSequence = _dlPlanningExtractMethod.LowLevelSequence;
				_lowLevelOffset = _dlPlanningExtractMethod.LowLevelOffset;
				_showIneligible = _dlPlanningExtractMethod.ShowIneligible;
				_overrideLowLevelRid = _dlPlanningExtractMethod.OverrideLowLevelRid;
                _excludeZeroValues = _dlPlanningExtractMethod.ExcludeZeroValues;
				_concurrentProcesses = _dlPlanningExtractMethod.ConcurrentProcesses;
                _updateDate = _dlPlanningExtractMethod.UpdateDate;
                _extractDate = _dlPlanningExtractMethod.ExtractDate;
			}
			else
			{	//Defaults
				_dlPlanningExtractMethod = new OTSForecastPlanningExtractMethodData(_varProfList);

				_merchandiseRID = Include.NoRID;
				_versionRID = Include.NoRID;
				_dateRangeRID = Include.NoRID;
				_filterRID = Include.NoRID;
				_chain = false;
                _store = false;
                _attributeSet = false;
                _attributeRID = Include.NoRID;
                _lowLevels = false;
				_lowLevelsOnly = false;
				_lowLevelsType = eLowLevelsType.None;
				_lowLevelSequence = -1;
				_lowLevelOffset = -1;
				_showIneligible = false;
				_overrideLowLevelRid = Include.NoRID;
				_excludeZeroValues = false;
				_concurrentProcesses = 1;
                _updateDate = DateTime.MinValue;
                _extractDate = DateTime.MinValue;
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
				return eProfileType.MethodPlanningExtract;
			}
		}
		
		public bool MethodValid
		{
			get
			{
				return _methodValid;
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

		public bool Chain
		{
			get
			{
				return _chain;
			}
			set
			{
                _chain = value;
			}
		}

        public bool Store
        {
            get
            {
                return _store;
            }
            set
            {
                _store = value;
            }
        }

        public bool AttributeSet
        {
            get
            {
                return _attributeSet;
            }
            set
            {
                _attributeSet = value;
            }
        }

        public int AttributeRID
        {
            get
            {
                return _attributeRID;
            }
            set
            {
                _attributeRID = value;
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

        public DateTime UpdateDate
        {
            get
            {
                return _updateDate;
            }
            set
            {
                _updateDate = value;
            }
        }

        public DateTime ExtractDate
        {
            get
            {
                return _extractDate;
            }
            set
            {
                _extractDate = value;
            }
        }

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

        public ArrayList SelectableTimetimeTotalVariableList
        {
            get
            {
                try
                {
                    if (_selectableTimetimeTotalVariableList == null)
                    {
                        LoadSelectableTimetimeTotalVariableList();
                    }

                    return _selectableTimetimeTotalVariableList;
                }
                catch (Exception err)
                {
                    string message = err.ToString();
                    throw;
                }
            }
            set
            {
                _selectableTimetimeTotalVariableList = value;
            }
        }

		//========
		// METHODS
		//========

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


		/// <summary>
		/// Updates the OTS Forecast Planning Extract method
		/// </summary>
		/// <param name="td">
		/// An instance of the TransactionData class which contains the database connection
		/// </param>

		override public void Update(TransactionData td)
		{
			ArrayList variableList;
            ArrayList timeTotalVariableList;

            try
			{
				_dlPlanningExtractMethod.HierarchyRID = _merchandiseRID;
				_dlPlanningExtractMethod.VersionRID = _versionRID;
				_dlPlanningExtractMethod.DateRangeRID = _dateRangeRID;
				_dlPlanningExtractMethod.FilterRID = _filterRID;
				_dlPlanningExtractMethod.Chain = _chain;
                _dlPlanningExtractMethod.Store = _store;
                _dlPlanningExtractMethod.AttributeSet = _attributeSet;
                _dlPlanningExtractMethod.AttributeRID = _attributeRID;
                _dlPlanningExtractMethod.LowLevels = _lowLevels;
				_dlPlanningExtractMethod.LowLevelsOnly = _lowLevelsOnly;
				_dlPlanningExtractMethod.LowLevelsType = _lowLevelsType;
				_dlPlanningExtractMethod.LowLevelSequence = _lowLevelSequence;
				_dlPlanningExtractMethod.LowLevelOffset = _lowLevelOffset;
				_dlPlanningExtractMethod.ShowIneligible = _showIneligible;
				_dlPlanningExtractMethod.OverrideLowLevelRid = _overrideLowLevelRid;
				_dlPlanningExtractMethod.ExcludeZeroValues = _excludeZeroValues;
				_dlPlanningExtractMethod.ConcurrentProcesses = _concurrentProcesses;
                _dlPlanningExtractMethod.UpdateDate = UpdateDate;
				
				variableList = new ArrayList();
                timeTotalVariableList = new ArrayList();

                // Weekly variables
                foreach (RowColProfileHeader varEntry in SelectableVariableList)
				{
					if (varEntry.IsDisplayed)
					{
						variableList.Add(new ForecastPlanningExtractMethodVariableEntry(Key, varEntry.Profile.Key, (int)eVariableTimeType.Weekly, varEntry.Sequence));
					}
				}

                // Time Total variables
                foreach (RowColProfileHeader varEntry in SelectableTimetimeTotalVariableList)
                {
                    if (varEntry.IsDisplayed)
                    {
                        timeTotalVariableList.Add(new ForecastPlanningExtractMethodVariableEntry(Key, varEntry.Profile.Key, (int)eVariableTimeType.TimeTotal, varEntry.Sequence));
                    }
                }

                _dlPlanningExtractMethod.VariableList = variableList;
                _dlPlanningExtractMethod.TimeTotalVariableList = timeTotalVariableList;


                switch (base.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						_dlPlanningExtractMethod.InsertMethod(base.Key, td);
						break;
					case eChangeType.update:
						base.Update(td);
						_dlPlanningExtractMethod.UpdateMethod(base.Key, td);
						break;
					case eChangeType.delete:
						_dlPlanningExtractMethod.DeleteMethod(base.Key, td);
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

							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_pl_PlanHierarchyNodeMissing,
								MIDText.GetText(eMIDTextCode.msg_pl_PlanHierarchyNodeMissing));
						}

						if (_versionRID == Include.NoRID)
						{
							SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_pl_PlanVersionMissing, this.ToString());

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
                _totalVarProfList = null;
                _dlPlanningExtractMethod = null;
				_selectableVariableList = null;
                _selectableTimetimeTotalVariableList = null;
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
            ProfileList timeTotalVarProfList;
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
            long processId = Include.Undefined;

            try
			{
                if (!aSAB.ROExtractEnabled)
                {
                    aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, "OTS Forecast Planning Extract database is not set up.  " + this.Name + " will not be executed.", this.ToString());
                    aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                    return;
                }

				aTimer = new MIDTimer();

				infoMsg = "Starting OTS Forecast Planning Extract: " + this.Name;
				aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, infoMsg, this.ToString());
				aTimer.Start();

                processId = aSAB.ControlServerSession.OpenExtractSession();

                aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.NoActionPerformed;

				if (aStoreFilterRID != Include.NoRID && aStoreFilterRID != Include.UndefinedStoreFilter)
				{
					_filterRID = aStoreFilterRID;
				}


				CreateSortedProfileList(SelectableVariableList, out varProfList);
                CreateSortedProfileList(SelectableTimetimeTotalVariableList, out timeTotalVarProfList);

                nodeList = new ProfileList(eProfileType.Plan);
				procStack = new Stack();

                nodeProf = aSAB.HierarchyServerSession.GetNodeData(_merchandiseRID);
				nodeProf.QualifiedNodeID = SAB.HierarchyServerSession.GetQualifiedNodeID(nodeProf.Key);
				nodeProf.ChainSecurityProfile = new HierarchyNodeSecurityProfile(_merchandiseRID);
				nodeProf.StoreSecurityProfile = new HierarchyNodeSecurityProfile(_merchandiseRID);

				verBuilder = new ForecastVersionProfileBuilder();
				verProf = verBuilder.Build(_versionRID);
				verProf.StoreSecurity = new VersionSecurityProfile(_versionRID);
				verProf.ChainSecurity = new VersionSecurityProfile(_versionRID);
			
				currDate = System.DateTime.Now;

				HLPlanProf = new PlanProfile(0, nodeProf, verProf);

				if (!LowLevels || !LowLevelsOnly)
				{
					nodeList.Add(HLPlanProf);
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
							planProf.NodeProfile.QualifiedNodeID = SAB.HierarchyServerSession.GetQualifiedNodeID(planProf.NodeProfile.Key);
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

                foreach (PlanProfile planProfile in nodeList)
                {
                    _extractDataStack.Push(new ExtractData(this, HLPlanProf, planProfile, varProfList, timeTotalVarProfList, currDate));
                }


				compStat = ProcessExtractList(aSAB);

				aTimer.Stop();
				infoMsg = "Completed OTS Forecast Planning Extract: " + this.Name + " " + "Elasped time: " + aTimer.ElaspedTimeString;
				aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, infoMsg, this.ToString());

				if (compStat == eProcessCompletionStatus.Successful)
				{
					aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
                    bool openedConnedtion = false;
                    try
                    {
                        _extractDate = DateTime.Now;
                        if (_dlPlanningExtractMethod == null)
                        {
                            _dlPlanningExtractMethod = new OTSForecastPlanningExtractMethodData();
                        }
                        if (!_dlPlanningExtractMethod.ConnectionIsOpen)
                        {
                            _dlPlanningExtractMethod.OpenUpdateConnection();
                            openedConnedtion = true;
                        }
                        _dlPlanningExtractMethod.ExtractDate = _extractDate;
                        _dlPlanningExtractMethod.ExtractDateUpdateMethod(Key);
                        _dlPlanningExtractMethod.CommitData();
                    }
                    catch (Exception err)
                    {
                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessAction: " + err.Message, _sourceModule);
                        aSAB.ApplicationServerSession.Audit.Log_Exception(err);
                        aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
                    }
                    finally
                    {
                        if (openedConnedtion
                            && _dlPlanningExtractMethod.ConnectionIsOpen)
                        {
                            _dlPlanningExtractMethod.CloseUpdateConnection();
                        }
                    }
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
            finally
            {
                aSAB.ControlServerSession.CloseExtractSession(processId);
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

        override public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels)
		{
			OTSForecastPlanningExtractMethod newOTSForecastPlanningExtractMethod = null;
            OverrideLowLevelProfile ollp;
            int customUserRID;

			try
			{
				newOTSForecastPlanningExtractMethod = (OTSForecastPlanningExtractMethod)this.MemberwiseClone();

				newOTSForecastPlanningExtractMethod._merchandiseRID = _merchandiseRID;
				newOTSForecastPlanningExtractMethod._versionRID = _versionRID;

				if (aCloneDateRanges &&
					_dateRangeRID != Include.UndefinedCalendarDateRange)
				{
					newOTSForecastPlanningExtractMethod._dateRangeRID = aSession.Calendar.GetDateRangeClone(_dateRangeRID).Key;
				}
				else
				{
					newOTSForecastPlanningExtractMethod._dateRangeRID = _dateRangeRID;
				}

				newOTSForecastPlanningExtractMethod._filterRID = _filterRID;
				newOTSForecastPlanningExtractMethod._chain = _chain;
                newOTSForecastPlanningExtractMethod._store = _store;
                newOTSForecastPlanningExtractMethod._attributeSet = _attributeSet;
                newOTSForecastPlanningExtractMethod._attributeRID = _attributeRID;
                newOTSForecastPlanningExtractMethod._lowLevels = _lowLevels;
				newOTSForecastPlanningExtractMethod._lowLevelsOnly = _lowLevelsOnly;
				newOTSForecastPlanningExtractMethod._lowLevelsType = _lowLevelsType;
				newOTSForecastPlanningExtractMethod._lowLevelSequence = _lowLevelSequence;
				newOTSForecastPlanningExtractMethod._lowLevelOffset = _lowLevelOffset;
				newOTSForecastPlanningExtractMethod._showIneligible = _showIneligible;
				newOTSForecastPlanningExtractMethod._excludeZeroValues = _excludeZeroValues;
				newOTSForecastPlanningExtractMethod._concurrentProcesses = _concurrentProcesses;

				newOTSForecastPlanningExtractMethod._selectableVariableList = null;
                newOTSForecastPlanningExtractMethod._selectableTimetimeTotalVariableList = null;

                newOTSForecastPlanningExtractMethod.Method_Change_Type = eChangeType.none;
				newOTSForecastPlanningExtractMethod.Method_Description = Method_Description;
				newOTSForecastPlanningExtractMethod.MethodStatus = MethodStatus;
				newOTSForecastPlanningExtractMethod.Name = Name;
				newOTSForecastPlanningExtractMethod.SG_RID = SG_RID;
				newOTSForecastPlanningExtractMethod.User_RID = User_RID;
				newOTSForecastPlanningExtractMethod.Virtual_IND = Virtual_IND;
                newOTSForecastPlanningExtractMethod.Template_IND = Template_IND;
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
                    newOTSForecastPlanningExtractMethod.CustomOLL_RID = customUserRID;
                }
                newOTSForecastPlanningExtractMethod.OverrideLowLevelRid = OverrideLowLevelRid;

				return newOTSForecastPlanningExtractMethod;
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
			ForecastPlanningExtractMethodVariableEntry varEntry;

			try
			{
				varKeyHash = new Hashtable();

				foreach (ForecastPlanningExtractMethodVariableEntry viewVarEntry in _dlPlanningExtractMethod.VariableList)
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
					varEntry = (ForecastPlanningExtractMethodVariableEntry)varKeyHash[varProf.Key];

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

        private void LoadSelectableTimetimeTotalVariableList()
        {
            Hashtable varKeyHash;
            TimeTotalVariableProfile viewVarProf;
            ForecastPlanningExtractMethodVariableEntry varEntry;

            try
            {
                varKeyHash = new Hashtable();

                foreach (ForecastPlanningExtractMethodVariableEntry viewVarEntry in _dlPlanningExtractMethod.TimeTotalVariableList)
                {
                    viewVarProf = (TimeTotalVariableProfile)_totalVarProfList.FindKey(viewVarEntry.VariableRID);

                    if (viewVarProf != null)
                    {
                        varKeyHash.Add(viewVarProf.Key, viewVarEntry);
                    }
                }

                _selectableTimetimeTotalVariableList = new ArrayList();

                foreach (TimeTotalVariableProfile varProf in _totalVarProfList)
                {
                    varEntry = (ForecastPlanningExtractMethodVariableEntry)varKeyHash[varProf.Key];

                    if (varEntry != null)
                    {
                        _selectableTimetimeTotalVariableList.Add(new RowColProfileHeader(varProf.VariableName, true, Convert.ToInt32(varEntry.VariableSequence), varProf));
                    }
                    else
                    {
                        _selectableTimetimeTotalVariableList.Add(new RowColProfileHeader(varProf.VariableName, false, -1, varProf));
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

                    overrideList = hTran.GetOverrideList(aOverrideLowLevelRid, aNodeRID, aCurrentVersionRID,
                                                          eHierarchyDescendantType.levelType, aLowLevelSequence, Include.NoRID, true, false);
				}
				else
				{
					overrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
				}

				return overrideList;
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

		private eProcessCompletionStatus ProcessExtractList(SessionAddressBlock aSAB)
		{
			ExtractProcessor[] extractProcArray = null;
			Thread[] threadArray;
			int concurrentExtracts;
			eProcessCompletionStatus maxCompStat;
			ExtractData extractData;
			int i;

			try
			{
				concurrentExtracts = ConcurrentProcesses;

                extractProcArray = new ExtractProcessor[concurrentExtracts];
				maxCompStat = eProcessCompletionStatus.Successful;
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

				return maxCompStat;
			}
			catch (Exception err)
			{
				aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessExtractList: " + err.Message, _sourceModule);
				aSAB.ApplicationServerSession.Audit.Log_Exception(err);
				throw;
			}
		}

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

		public class ExtractProcessor
		{
			//=======
			// FIELDS
			//=======

			private OTSForecastPlanningExtractMethod _planningExtractMethod;
			private eProcessCompletionStatus _maxCompStat = eProcessCompletionStatus.None;
			private SessionAddressBlock SAB;

			//=============
			// CONSTRUCTORS
			//=============

			public ExtractProcessor(
				OTSForecastPlanningExtractMethod aExportMethod)
			{
				_planningExtractMethod = aExportMethod;
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

						_planningExtractMethod.SAB.ApplicationServerSession.Audit.Add_Msg(
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
						_planningExtractMethod.SAB.ApplicationServerSession.Audit.Add_Msg(
							eMIDMessageLevel.Error,
							"ExtractProcessor:Initialize():User and Password NOT specified",
							this.ToString());

						_maxCompStat = eProcessCompletionStatus.Failed;
						return eProcessCompletionStatus.Failed;
					}

					authentication = SAB.ClientServerSession.UserLogin(userId, passWd, eProcesses.planningExtractThread);

					if (authentication != eSecurityAuthenticate.UserAuthenticated)
					{
						_planningExtractMethod.SAB.ApplicationServerSession.Audit.Add_Msg(
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
                    SAB.HierarchyServerSession.Initialize();

					return eProcessCompletionStatus.Successful;
				}
				catch (Exception Ex)
				{
					_planningExtractMethod.SAB.ApplicationServerSession.Audit.Add_Msg(
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
					extractData = _planningExtractMethod.GetNextExtractData();

					while (extractData != null)
					{
						extractData.ExecuteExtract(SAB);
						_maxCompStat = (eProcessCompletionStatus)Math.Max((int)_maxCompStat, (int)extractData.CompletionStatus);

						extractData = _planningExtractMethod.GetNextExtractData();
					}
				}
				catch (Exception Ex)
				{
					_planningExtractMethod.SAB.ApplicationServerSession.Audit.Add_Msg(
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
		{
			//=======
			// FIELDS
			//=======

			private OTSForecastPlanningExtractMethod _planningExtractMethod;
			private PlanProfile _HLPlanProf;
			private ProfileList _planProfList;
			private ProfileList _varProfList;
            private ProfileList _totalVarProfList;
            private DateTime _currDate;
			private ApplicationSessionTransaction _transaction;
			private eProcessCompletionStatus _completionStatus;


			//=============
			// CONSTRUCTORS
			//=============

			public ExtractData(
				OTSForecastPlanningExtractMethod aExportMethod,
				PlanProfile aHLPlanProfile,
				PlanProfile aPlanProfile,
				ProfileList aVarProfList,
                ProfileList aTotalVarProfList,
                DateTime aCurrDate)
			{
				_planningExtractMethod = aExportMethod;
				_HLPlanProf = aHLPlanProfile;
				_varProfList = aVarProfList;
                _totalVarProfList = aTotalVarProfList;
                _currDate = aCurrDate;

				_planProfList = new ProfileList(eProfileType.Plan);
				_planProfList.Add(aPlanProfile);

				Initialize();
			}

			public ExtractData(
				OTSForecastPlanningExtractMethod aExportMethod,
				PlanProfile aHLPlanProfile,
				ProfileList aPlanProfList,
				ProfileList aVarProfList,
                ProfileList aTotalVarProfList,
                DateTime aCurrDate)
			{
				_planningExtractMethod = aExportMethod;
				_HLPlanProf = aHLPlanProfile;
				_planProfList = aPlanProfList;
				_varProfList = aVarProfList;
                _totalVarProfList = aTotalVarProfList;
                _currDate = aCurrDate;

				Initialize();
			}

			//===========
			// PROPERTIES
			//===========

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

			public void ExecuteExtract(SessionAddressBlock aSAB)
			{
				PlanCubeGroup cubeGroup;
				PlanOpenParms openParms;
				eProcessCompletionStatus extractStatus = eProcessCompletionStatus.None;
                bool includeAllWeeks = false;


                ExtractOptions extractOptions;
                string filterName = string.Empty;
                DateTime filterUpdateDate = DateTime.MinValue;
                DateTime attributeUpdateDate = DateTime.MinValue;
				FilterData fd = new FilterData();
				StoreGroupProfile sgp = null;

                try
				{
                    _transaction = aSAB.ApplicationServerSession.CreateTransaction();

                    if (_planningExtractMethod.FilterRID != Include.NoRID)
                    {
                        filterName = fd.FilterGetName(_planningExtractMethod.FilterRID);
                        filterUpdateDate = fd.FilterGetUpdateDate(_planningExtractMethod.FilterRID);
                    }

                    string attribute = string.Empty;
                    Dictionary<int, string> storeAttributeSets = null;
                    if (_planningExtractMethod.AttributeSet)
                    {
                        storeAttributeSets = new Dictionary<int, string>();
                        ProfileList sgl = StoreMgmt.StoreGroup_GetLevelListFilled(_planningExtractMethod.AttributeRID);
                        foreach (StoreGroupLevelProfile sglp in sgl.ArrayList)
                        {
                            foreach (StoreProfile sp in sglp.Stores.ArrayList)
                            {
                                storeAttributeSets.Add(sp.Key, sglp.Name);
                            }
                        }
                        sgp = StoreMgmt.StoreGroup_Get(_planningExtractMethod.AttributeRID);
                        attribute = sgp.Name;
                        attributeUpdateDate =  fd.FilterGetUpdateDate(sgp.FilterRID);
                    }

                    if (_planningExtractMethod.Store
                        || _planningExtractMethod.AttributeSet)
					{
                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Starting Store Planning Extract " + _planningExtractMethod.Name + "...", _sourceModule);
                        ProfileList storeList = StoreMgmt.StoreProfiles_GetActiveStoresList();

                        foreach (PlanProfile planProf in _planProfList)
                        {
                            includeAllWeeks = false;
                            cubeGroup = new StorePlanMaintCubeGroup(aSAB, _transaction);
                            openParms = FillOpenParmForPlan(aSAB, planProf);
                            cubeGroup.OpenCubeGroup(openParms);
                            if (!cubeGroup.SetStoreFilter(openParms.FilterRID, cubeGroup))
                            {
                                FilterData storeFilterData = new FilterData();
                                string msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
                                msg = msg.Replace("{0}", storeFilterData.FilterGetName(openParms.FilterRID));
                                string suffix = ". Method " + this._planningExtractMethod.Name + ". ";
                                string auditMsg = msg + suffix;
                                aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.ToString());
                                throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
                            }

                            try
                            {
                                // check if need to override to all weeks
                                if (_planningExtractMethod.UpdateDate > _planningExtractMethod.ExtractDate
                                    || filterUpdateDate > _planningExtractMethod.ExtractDate
                                    || attributeUpdateDate > _planningExtractMethod.ExtractDate
                                    )
                                {
                                    includeAllWeeks = true;
                                    if (_planningExtractMethod.UpdateDate > _planningExtractMethod.ExtractDate
                                    )
                                    {
                                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "All weeks will be included because of a change to the method.", _sourceModule);
                                    }
                                    else if (filterUpdateDate > _planningExtractMethod.ExtractDate
                                    )
                                    {
                                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "All weeks will be included because of a change to the filter.", _sourceModule);
                                    }
                                    else if (attributeUpdateDate > _planningExtractMethod.ExtractDate
                                    )
                                    {
                                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "All weeks will be included because of a change to the attribute.", _sourceModule);
                                    }
                                }

                                // include all weeks if custom calcs and upgraded since last extract
                                if (!includeAllWeeks)
                                {
                                    EnvironmentData ed = new EnvironmentData();
                                    UpgradeProfile up = ed.GetUpgradeInformation();
                                    if (up.UpgradeConfiguration.ToLower() != "base" 
                                        && up.UpgradeDateTime > _planningExtractMethod.ExtractDate
                                    )
                                    {
                                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "All weeks will be included because of an upgrade to the application.", _sourceModule);
                                        includeAllWeeks = true;
                                    }
                                }

                                if (!includeAllWeeks)
                                {
                                    StoreEligibilityList storeEligList = aSAB.HierarchyServerSession.GetStoreEligibilityList(storeList, planProf.NodeProfile.Key, true, false);
                                    foreach (StoreEligibilityProfile sep in storeEligList)
                                    {
                                        if (sep.UpdateDate > _planningExtractMethod.ExtractDate)
                                        {
                                            aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "All weeks will be included because of a change to store eligibility.", _sourceModule);
                                            includeAllWeeks = true;
                                            break;
                                        }
                                    }
                                }

                                extractOptions = new ExtractOptions(
                                aChain: _planningExtractMethod.Chain,
                                aStore: _planningExtractMethod.Store,
                                aAttributeSet: _planningExtractMethod.AttributeSet,
                                aExcludeZeroValues: _planningExtractMethod.ExcludeZeroValues,
                                aIncludeAllWeeks: includeAllWeeks,
                                aVarProfList: _varProfList,
                                aTimeTotalVarProfList: _totalVarProfList,
                                aAttributeRID: _planningExtractMethod.AttributeRID,
                                aAttribute: attribute,
                                storeAttributeSets: storeAttributeSets,
                                aFilterName: filterName
                                );

                                cubeGroup.ExtractCubeGroup(extractOptions);
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
					else if (_planningExtractMethod.Chain)
					{
                        aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Starting Chain Planning Extract " + _planningExtractMethod.Name + "...", _sourceModule);

                        foreach (PlanProfile planProf in _planProfList)
                        {
                            includeAllWeeks = false;
                            cubeGroup = new ChainPlanMaintCubeGroup(aSAB, _transaction);
                            openParms = FillOpenParmForPlan(aSAB, planProf);
                            cubeGroup.OpenCubeGroup(openParms);

                            try
                            {
                                // check if need to override to all weeks
                                if (_planningExtractMethod.UpdateDate > _planningExtractMethod.ExtractDate
                                    //|| filterUpdateDate > _planningExtractMethod.ExtractDate
                                    //|| attributeUpdateDate > _planningExtractMethod.ExtractDate
                                    )
                                {
                                    includeAllWeeks = true;
                                }

                                extractOptions = new ExtractOptions(
                                aChain: _planningExtractMethod.Chain,
                                aStore: _planningExtractMethod.Store,
                                aAttributeSet: _planningExtractMethod.AttributeSet,
                                aExcludeZeroValues: _planningExtractMethod.ExcludeZeroValues,
                                aIncludeAllWeeks: includeAllWeeks,
                                aVarProfList: _varProfList,
                                aTimeTotalVarProfList: _totalVarProfList,
                                aAttributeRID: _planningExtractMethod.AttributeRID,
                                aAttribute: attribute,
                                storeAttributeSets: storeAttributeSets,
                                aFilterName: filterName
                                );

                                cubeGroup.ExtractCubeGroup(extractOptions);
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
					aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteExtract: " + err.Message, _sourceModule);
					aSAB.ApplicationServerSession.Audit.Log_Exception(err);
					
					extractStatus = eProcessCompletionStatus.Failed;
				}
				finally
				{
					_completionStatus = extractStatus;

					aSAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Information,
						"Completed Forecast Extract " + _planningExtractMethod.Name + " with status " + MIDText.GetTextOnly((int)extractStatus),
						_sourceModule);
				}
			}

			private void Initialize()
			{
				try
				{

				}
				catch (Exception err)
				{
					string message = err.ToString();
					throw;
				}
			}

			private PlanOpenParms FillOpenParmForPlan(SessionAddressBlock aSAB, PlanProfile aPlanProf)
			{
				PlanOpenParms openParms;

				try
				{
					if (_planningExtractMethod.Store
                        || _planningExtractMethod.AttributeSet)
					{
						openParms = new PlanOpenParms(ePlanSessionType.StoreSingleLevel, "Default");
					}
					else
					{
						openParms = new PlanOpenParms(ePlanSessionType.ChainSingleLevel, "Default");
					}

					if (_planningExtractMethod.GlobalUserType == eGlobalUserType.User)
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

					openParms.DateRangeProfile = aSAB.ApplicationServerSession.Calendar.GetDateRange(_planningExtractMethod.DateRangeRID);

                    if (_planningExtractMethod.AttributeRID != Include.NoRID
                        && _planningExtractMethod.AttributeSet)
                    {
                        openParms.StoreGroupRID = _planningExtractMethod.AttributeRID;
                    }
                    else
                    {
                        openParms.StoreGroupRID = Include.AllStoreGroupRID;
                    }
					openParms.FilterRID = _planningExtractMethod.FilterRID;
			
					openParms.IneligibleStores = _planningExtractMethod.ShowIneligible; 
					openParms.SimilarStores = false;

					return openParms;
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
				if (Chain)
				{
					versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Chain);
					hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierarchyRID, (int)eSecurityTypes.Chain);
				}
				else
				{
					versionSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(VersionRID, (int)eSecurityTypes.Store);
					hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(HierarchyRID, (int)eSecurityTypes.Store);
				}

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

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalPlanningExtract);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserPlanningExtract);
            }
        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            ROOverrideLowLevel overrideLowLevel = new ROOverrideLowLevel();
            overrideLowLevel.OverrideLowLevelsModel = GetName.GetOverrideLowLevelsModel(_dlPlanningExtractMethod.OverrideLowLevelRid, SAB); //CustomOLL_RID;
            overrideLowLevel.OverrideLowLevelsModelList = BuildOverrideLowLevelList(
                overrideLowLevelRid: OverrideLowLevelRid,
                customOverrideLowLevelRid: CustomOLL_RID
                );

            if (CustomOLL_RID > Include.NoRID
                && CustomOLL_RID == OverrideLowLevelRid)
            {
                overrideLowLevel.IsCustomModel = true;
            }

            ROLevelInformation lowLevel = new ROLevelInformation();
            lowLevel.LevelType = (eROLevelsType)_dlPlanningExtractMethod.LowLevelsType;
            lowLevel.LevelOffset = _dlPlanningExtractMethod.LowLevelOffset;
            lowLevel.LevelSequence = _dlPlanningExtractMethod.LowLevelSequence;
            lowLevel.LevelValue = GetName.GetLevelName(
               levelType: (eROLevelsType)_dlPlanningExtractMethod.LowLevelsType,
               levelSequence: _dlPlanningExtractMethod.LowLevelSequence,
               levelOffset: _dlPlanningExtractMethod.LowLevelOffset,
               SAB: SAB
               );

            ROVariableGroupings variableList, totalVariableList;
            variableList = null;
            totalVariableList = null;

            if (_varProfList == null)
            {
                _varProfList = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanVariables.VariableProfileList;
            }

            ApplicationSessionTransaction _applicationSessionTransaction = SAB.ApplicationServerSession.CreateTransaction();
            ArrayList al = _applicationSessionTransaction.PlanComputations.PlanVariables.GetVariableGroupings();
            variableList = VariableGroupings.BuildVariableGroupings(ePlanType.Store, SelectableVariableList, al);
            totalVariableList = VariableGroupings.BuildVariableGroupings(ePlanType.Store, SelectableTimetimeTotalVariableList, al);

            // Go through the variable lists and put in the sort sequence value; use -1 for unselected variables
            foreach (ROVariableGrouping variableGrouping in variableList.VariableGrouping)
            {
                foreach (ROVariable variable in variableGrouping.Variables)
                {
                    variable.Sequence = -1;
                    foreach (MIDRetail.Data.ForecastPlanningExtractMethodVariableEntry variableEntry in _dlPlanningExtractMethod.VariableList)
                    {
                        if (variableEntry.VariableRID == variable.Number)
                        {
                            variable.Sequence = variableEntry.VariableSequence;
                        }
                    }
                }
            }

            foreach (ROVariableGrouping variableGrouping in totalVariableList.VariableGrouping)
            {
                foreach (ROVariable variable in variableGrouping.Variables)
                {
                    variable.Sequence = -1;
                    foreach (MIDRetail.Data.ForecastPlanningExtractMethodVariableEntry variableEntry in _dlPlanningExtractMethod.TimeTotalVariableList)
                    {
                        if (variableEntry.VariableRID == variable.Number)
                        {
                            variable.Sequence = variableEntry.VariableSequence;
                        }
                    }
                }
            }

            ROMethodPlanningExtractProperties method = new ROMethodPlanningExtractProperties(
                kvpMethod: GetName.GetMethod(method: this),
                sDescription: Method_Description,
                iUserKey: User_RID,
                merchandise: GetName.GetMerchandiseName(_dlPlanningExtractMethod.HierarchyRID, SAB),
                versionFilter: GetName.GetVersion(_dlPlanningExtractMethod.VersionRID, SAB),
                timePeriod_CDR: GetName.GetCalendarDateRange(_dlPlanningExtractMethod.DateRangeRID, SAB),
                storeFilter: GetName.GetFilterName(_dlPlanningExtractMethod.FilterRID),
                chainIndicator: _dlPlanningExtractMethod.Chain,
                storeIndicator: _dlPlanningExtractMethod.Store,
                attributeSetIndicator: _dlPlanningExtractMethod.AttributeSet,
                attribute: GetName.GetAttributeName(_dlPlanningExtractMethod.AttributeRID),
                lowLevelsIndicator: _dlPlanningExtractMethod.LowLevels,
                lowLevelsOnlyIndicator: _dlPlanningExtractMethod.LowLevelsOnly,
                lowLevel: lowLevel,
                extractIneligibleStoresIndicator: _dlPlanningExtractMethod.ShowIneligible,
                excludeZeroValuesIndicator: _dlPlanningExtractMethod.ExcludeZeroValues,
                numberOfConcurrentProcesses: _dlPlanningExtractMethod.ConcurrentProcesses,
                overrideLowLevel: overrideLowLevel,
                variableList: variableList,
                totalVariableList: totalVariableList,
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

            if (CustomOLL_RID > Include.NoRID
                && CustomOLL_RID == OverrideLowLevelRid)
            {
                overrideLowLevel.IsCustomModel = true;
            }

            return overrideLowLevel;
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            ROMethodPlanningExtractProperties roMethodPlanningExtractProperties = (ROMethodPlanningExtractProperties)methodProperties;
            try
            {
                Template_IND = methodProperties.IsTemplate;
                ArrayList alVariableList = new ArrayList();
                ArrayList alTotalVariableList = new ArrayList();

                if (_varProfList == null)
                {
                    _varProfList = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanVariables.VariableProfileList;
                }
               
                int nextSequence = 0;
                foreach (ROVariableGrouping grouping in roMethodPlanningExtractProperties.VariableList.VariableGrouping)
                {
                    foreach (ROVariable variable in grouping.Variables)
                    {
                        if (variable.Sequence < 0)
                        {
                            variable.IsDisplayed = false;
                        }
                        else
                        {
                            variable.IsDisplayed = true;
                        }
                        alVariableList.Add(new RowColProfileHeader(variable.Name, variable.IsDisplayed, variable.Sequence, (VariableProfile)_varProfList.FindKey(variable.Number)));
                    }
                }

                if (_totalVarProfList == null)
                {
                    _totalVarProfList = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanTimeTotalVariables.TimeTotalVariableProfileList;
                }
                foreach (ROVariableGrouping grouping in roMethodPlanningExtractProperties.TotalVariableList.VariableGrouping)
                {
                    foreach (ROVariable variable in grouping.Variables)
                    {
                        if (variable.Sequence < 0)
                        {
                            variable.IsDisplayed = false;
                        }
                        else
                        {
                            variable.IsDisplayed = true;
                        }
                        alTotalVariableList.Add(new RowColProfileHeader(variable.Name, variable.IsDisplayed, variable.Sequence, (TimeTotalVariableProfile)_totalVarProfList.FindKey(variable.Number)));
                    }
                }

                _merchandiseRID = roMethodPlanningExtractProperties.Merchandise.Key;
                _versionRID = roMethodPlanningExtractProperties.VersionFilter.Key;
                _dateRangeRID = roMethodPlanningExtractProperties.TimePeriod_CDR.Key;
                _filterRID = roMethodPlanningExtractProperties.StoreFilter.Key;
                _lowLevels = roMethodPlanningExtractProperties.LowLevelsIndicator;
                _lowLevelsOnly = roMethodPlanningExtractProperties.LowLevelsOnlyIndicator;
                _lowLevelsType = (eLowLevelsType)roMethodPlanningExtractProperties.LowLevel.LevelType;
                _lowLevelOffset = roMethodPlanningExtractProperties.LowLevel.LevelOffset;
                _lowLevelSequence = roMethodPlanningExtractProperties.LowLevel.LevelSequence;
                _dlPlanningExtractMethod.OverrideLowLevelRid = roMethodPlanningExtractProperties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                if (roMethodPlanningExtractProperties.OverrideLowLevel.IsCustomModel)
                {
                    CustomOLL_RID = roMethodPlanningExtractProperties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                }
                else
                {
                    CustomOLL_RID = Include.NoRID;
                }
                _chain = roMethodPlanningExtractProperties.ChainIndicator;
                _store = roMethodPlanningExtractProperties.StoreIndicator;
                _attributeSet = roMethodPlanningExtractProperties.AttributeSetIndicator;
                _attributeRID = roMethodPlanningExtractProperties.Attribute.Key;
                _showIneligible = roMethodPlanningExtractProperties.ExtractIneligibleStoresIndicator;
                _excludeZeroValues = roMethodPlanningExtractProperties.ExcludeZeroValuesIndicator;
                _concurrentProcesses = roMethodPlanningExtractProperties.NumberOfConcurrentProcesses;
                _overrideLowLevelRid = roMethodPlanningExtractProperties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                if (roMethodPlanningExtractProperties.OverrideLowLevel.IsCustomModel)
                {
                    CustomOLL_RID = roMethodPlanningExtractProperties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                }
                else
                {
                    CustomOLL_RID = Include.NoRID;
                }
                _selectableVariableList = alVariableList;
                _selectableTimetimeTotalVariableList = alTotalVariableList;
                //_dlPlanningExtractMethod.VariableList = roMethodPlanningExtractProperties.VariableList;
                //_dlPlanningExtractMethod.TimeTotalVariableList = roMethodPlanningExtractProperties.TotalVariableList;
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
            //throw new NotImplementedException("MethodSaveData is not implemented");
        }

        override public ROMethodProperties MethodCopyData()
        {
            throw new NotImplementedException("MethodCopyData is not implemented");
        }
        // END RO-642 - RDewey
    }

    public class ExtractOptions
    {
        //=======
        // FIELDS
        //=======

        private bool _chain;
        private bool _store;
        private bool _attributeSet;
        private bool _excludeZeroValues;
        private bool _includeAllWeeks;
        private ProfileList _varProfList;
        private ProfileList _timeTotalVarProfList;
        private string _filterName;
        private int _attributeRID;
        private string _attribute;
        private Dictionary<int, string> _storeAttributeSets;

        //=============
        // CONSTRUCTORS
        //=============

        public ExtractOptions(
            bool aChain,
            bool aStore,
            bool aAttributeSet,
            bool aExcludeZeroValues,
            bool aIncludeAllWeeks,
            ProfileList aVarProfList,
            ProfileList aTimeTotalVarProfList,
            int aAttributeRID,
            string aAttribute,
            Dictionary<int, string> storeAttributeSets,
            string aFilterName)
        {
            _chain = aChain;
            _store = aStore;
            _attributeSet = aAttributeSet;
            _excludeZeroValues = aExcludeZeroValues;
            _includeAllWeeks = aIncludeAllWeeks;
            _varProfList = aVarProfList;
            _timeTotalVarProfList = aTimeTotalVarProfList;
            _attributeRID = aAttributeRID;
            _attribute = aAttribute;
            _storeAttributeSets = storeAttributeSets;
            _filterName = aFilterName;
        }

        //===========
        // PROPERTIES
        //===========

        public bool Chain
        {
            get { return _chain; }
        }

        public bool Store
        {
            get { return _store; }
        }

        public bool AttributeSet
        {
            get { return _attributeSet; }
        }

        public bool ExcludeZeroValues
        {
            get { return _excludeZeroValues; }
        }

        public bool IncludeAllWeeks
        {
            get { return _includeAllWeeks; }
        }

        public ProfileList VarProfList
        {
            get { return _varProfList; }
        }

        public ProfileList TimeTotalVarProfList
        {
            get { return _timeTotalVarProfList; }
        }

        public int AttributeRID
        {
            get { return _attributeRID; }
        }

        public string Attribute
        {
            get { return _attribute; }
        }

        public Dictionary<int, string> StoreAttributeSets
        {
            get { return _storeAttributeSets; }
        }

        public string FilterName
        {
            get { return _filterName; }
        }
    }
 }
