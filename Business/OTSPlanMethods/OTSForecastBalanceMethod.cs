using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business
{
	/// <summary>
	/// 
	/// </summary>
	public class OTSForecastBalanceMethod : OTSForecastBalanceBaseMethod
	{
		private OTSForecastBalanceMethodData	_ForecastBalanceData;
		private int								_filterRID;
		private int								_hnRID;
		private int								_highLevelVersionRID;
		private int								_cdrRID;
		private int								_lowLevelVersionRID;
		eLowLevelsType							_lowLevelsType;
		private int								_lowLevelsOffset;
		private int								_lowLevelsSequence;
		private bool							_similarStoresInd;
		private bool							_ineligibleStoresInd;
		private int								_variableNumber;
		private eIterationType					_iterationType;
		private int								_iterationsCount;
		private eBalanceMode					_balanceMode;
		private string							_computationMode;
		private int								_overrideLowLevelRid;
		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private eMatrixType                     _matrixType;
		private int                             _modelRID;
		private bool                            _useBasis;
		// END MID Track #5647
        // Begin TT#1630 - JSmith - Computation mode incorrect when using model
        private ForecastBalanceProfile          _forecastBalanceProfile;
        // End TT#1630 

		//private eChangeType			_ForecastBalance_Method_Change_Type;

		string _sourceModule = "OTSForecastBalanceMethod.cs";
		//private bool _firstReadForBasis;
		private SessionAddressBlock _SAB;
		private PlanCubeGroup _cubeGroup;
		private PlanOpenParms _openParms = null;
		private ApplicationSessionTransaction _applicationTransaction;

		private ProfileList _weeksToPlan;
		private ProfileList _lowLevelVersionOverrideProfileList = null;
		private BasisProfile _basisProfile = null;
		private ProfileList _versionProfList;


		private HierarchyNodeProfile _hnp = null;
		
		System.DateTime beginTime;

        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        bool _foundDuplicate = false;
        string _duplicateMessage = null;
        // End TT#2281

		#region Properties
		public ProfileList WeeksToPlan
		{
			get	{return _weeksToPlan;}
			set	{_weeksToPlan = value;	}
		}
		public BasisProfile BasisProfile
		{
			get
			{
				try
				{
					if (_basisProfile == null)
					{
						_basisProfile = new BasisProfile(1, "basis", null);
					}
					return _basisProfile;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public ProfileList LowLevelVersionOverrideProfileList
		{
			get
			{
				try
				{
					if (_lowLevelVersionOverrideProfileList == null)
					{
						_lowLevelVersionOverrideProfileList = new ProfileList(eProfileType.LowLevelVersionOverride);
					}
			
					return _lowLevelVersionOverrideProfileList;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public int FilterRID
		{
			get	{return _filterRID;}
			set	{_filterRID = value;}
		}

		public int HnRID
		{
			get	{return _hnRID;}
			set	{_hnRID = value;}
		}

		public int HighLevelVersionRID
		{
			get {return _highLevelVersionRID;}
			set {_highLevelVersionRID = value;}
		}

		public int CDR_RID
		{
			get	{return _cdrRID;}
			set	{_cdrRID = value;	}
		}

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		public eMatrixType Matrix_Type
		{
			get	{return _matrixType;}
			set	{_matrixType = value;	}
		}

		public int Model_RID
		{
			get	{return _modelRID;}
			set	{_modelRID = value;	}
		}
		// END MID Track #5647

		public int LowLevelVersionRID
		{
			get	{return _lowLevelVersionRID;}
			set	{_lowLevelVersionRID = value;	}
		}

		public eLowLevelsType LowLevelsType
		{
			get	{return _lowLevelsType;}
			set	{_lowLevelsType = value;	}
		}

		public int LowLevelsOffset
		{
			get	{return _lowLevelsOffset;}
			set	{_lowLevelsOffset = value;	}
		}

		public int LowLevelsSequence
		{
			get	{return _lowLevelsSequence;}
			set	{_lowLevelsSequence = value;	}
		}

		public bool SimilarStoresInd
		{
			get	{return _similarStoresInd;}
			set	{_similarStoresInd = value;	}
		}

		public bool IneligibleStoresInd
		{
			get	{return _ineligibleStoresInd;}
			set	{_ineligibleStoresInd = value;	}
		}

		public int VariableNumber
		{
			get	{return _variableNumber;}
			set	{_variableNumber = value;	}
		}

		public eIterationType IterationType
		{
			get	{return _iterationType;}
			set	{_iterationType = value;	}
		}

		public int IterationsCount
		{
			get	{return _iterationsCount;}
			set	{_iterationsCount = value;	}
		}

		public eBalanceMode BalanceMode
		{
			get	{return _balanceMode;}
			set	{_balanceMode = value;	}
		}

		public string ComputationMode
		{
			get	{return _computationMode;}
			set	{_computationMode = value;	}
		}

		public int OverrideLowLevelRid
		{
			get { return _overrideLowLevelRid; }
			set { _overrideLowLevelRid = value; }
		}

		public ApplicationSessionTransaction ApplicationTransaction 
		{
			get { return _applicationTransaction ; }
			set { _applicationTransaction = value; }
		}

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.MethodForecastBalance;
			}
		}

		private ProfileList VersionProfList
		{
			get
			{
				try
				{
					if (_versionProfList == null)
					{
						_versionProfList = _SAB.ClientServerSession.GetUserForecastVersions();
					}
			
					return _versionProfList;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private HierarchyNodeProfile HNP
		{
			get
			{
				try
				{
					if (_hnp == null)
					{
						_hnp = _SAB.HierarchyServerSession.GetNodeData(this.HnRID);
					}
			
					return _hnp;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        public bool FoundDuplicate
        {
            get
            {
                return _foundDuplicate;
            }
        }

        public string DuplicateMessage
        {
            get
            {
                return _duplicateMessage;
            }
        }
        // End TT#2281

		#endregion

		public OTSForecastBalanceMethod(SessionAddressBlock SAB, int aMethodRID): base (SAB,
			//Begin TT#523 - JScott - Duplicate folder when new folder added
			//aMethodRID, eMethodType.ForecastBalance)
			aMethodRID, eMethodType.ForecastBalance, eProfileType.MethodForecastBalance)
			//End TT#523 - JScott - Duplicate folder when new folder added
		{
			try
			{
				_SAB = SAB;
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				_useBasis = false;
				// END MID Track #5647

				if (base.Filled)
				{
					_ForecastBalanceData = new OTSForecastBalanceMethodData(aMethodRID, eChangeType.populate);
					_filterRID = _ForecastBalanceData.FilterRID;
					_hnRID = _ForecastBalanceData.HnRID;
					_highLevelVersionRID = _ForecastBalanceData.HighLevelVersionRID;
					_cdrRID = _ForecastBalanceData.CDR_RID;
					// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
					_modelRID = _ForecastBalanceData.Model_RID;
					_matrixType = _ForecastBalanceData.Matrix_Type;
					// END MID Track #5647
                    // Begin TT#1630 - JSmith - Computation mode incorrect when using model
                    _forecastBalanceProfile = new ForecastBalanceProfile(_modelRID);
                    _computationMode = _forecastBalanceProfile.ComputationMode;
                    // End TT#1630

					_lowLevelVersionRID = _ForecastBalanceData.LowLevelVersionRID;
					_lowLevelsType = _ForecastBalanceData.LowLevelsType;
					_lowLevelsOffset = _ForecastBalanceData.LowLevelOffset;
					_lowLevelsSequence = _ForecastBalanceData.LowLevelSequence;
					_similarStoresInd = _ForecastBalanceData.SimilarStoresInd;
					_ineligibleStoresInd = _ForecastBalanceData.IneligibleStoresInd;
					_variableNumber = _ForecastBalanceData.VariableNumber;
					_iterationType = _ForecastBalanceData.IterationType;
					_iterationsCount = _ForecastBalanceData.IterationsCount;
					_balanceMode = _ForecastBalanceData.BalanceMode;
                    // Begin TT#1630 - JSmith - Computation mode incorrect when using model
                    //_computationMode =  _ForecastBalanceData.ComputationMode;
                    // End TT#1630
					_overrideLowLevelRid = _ForecastBalanceData.OverrideLowLevelRid;

					LoadBasisList();
                    // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                    //LoadVersionOverrideList();
                    try
                    {
                        LoadVersionOverrideList();
                    }
                    catch (DuplicateOverrideListEntry ex)
                    {
                        _foundDuplicate = true;
                        _duplicateMessage = ex.Message;
                    }
                    // End TT#2281
				}
				else
					//Defaults
				{
					_filterRID = Include.NoRID;
					_hnRID = Include.NoRID;
					_highLevelVersionRID = Include.NoRID;
					_cdrRID = Include.NoRID;
					// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
					_modelRID = Include.NoRID;
					_matrixType = 0;
					// END MID Track #5647
					_lowLevelVersionRID = Include.NoRID;
					_lowLevelsType = eLowLevelsType.None;
					_lowLevelsOffset = 0;
					_lowLevelsSequence = 0;
					_similarStoresInd = false;
					_ineligibleStoresInd = false;
					_variableNumber = 0;
					_iterationType = eIterationType.UseBase;
					_iterationsCount = 3;
					_balanceMode = eBalanceMode.Store;
					// Begin Track #5949 stodd
					_computationMode = SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
					// End Track #5949 stodd
					_overrideLowLevelRid = Include.NoRID;
				}
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (IsFilterUser(_filterRID))
            {
                return true;
            }
            
            if (IsHierarchyNodeUser(_hnRID))
            {
                return true;
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		public override void ProcessMethod(
			ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
			// Begin MID Track #5210 - JSmith - Out of memory
			try
			{
                // Begin TT#381-MD - JSmith - OTS Forecast Workflow with a Filter. The filter criteria is not being honored for Global Unlock, Global Lock. and Matrix Balance.
                if (aStoreFilter != Include.UndefinedStoreFilter &&
                    aStoreFilter != Include.NoRID)
                {
                    FilterRID = aStoreFilter;
                }
                // End TT#381-MD - JSmith - OTS Forecast Workflow with a Filter. The filter criteria is not being honored for Global Unlock, Global Lock. and Matrix Balance.

				// End MID Track #5210
				ArrayList forecastingOverrideList = aApplicationTransaction.ForecastingOverrideList;
				if (forecastingOverrideList != null)
				{
					foreach (ForecastingOverride fo in forecastingOverrideList)
					{
						if (fo.VariableNumber > 0)
						{
							this.VariableNumber = fo.VariableNumber;
						}

                        //if (fo.ComputationMode != null &&
                        //    fo.ComputationMode != string.Empty)
                        //{
                        //    _computationMode = fo.ComputationMode;
                        //}
		
						this.ProcessAction(
							aApplicationTransaction.SAB,
							aApplicationTransaction,
							null,
							methodProfile,
							true,
//Begin Track #5188 - JScott - Method Filters not being over overriden from Workflow
//							Include.NoRID);
							aStoreFilter);
//End Track #5188 - JScott - Method Filters not being over overriden from Workflow
					}
				}
				else
				{
					this.ProcessAction(
						aApplicationTransaction.SAB,
						aApplicationTransaction,
						null,
						methodProfile,
						true,
						//Begin Track #5188 - JScott - Method Filters not being over overriden from Workflow
						//						Include.NoRID);
						aStoreFilter);
					//End Track #5188 - JScott - Method Filters not being over overriden from Workflow
				}
			// Begin MID Track #5210 - JSmith - Out of memory
			}
			catch
			{
				aApplicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
			}
			finally
			{
				_ForecastBalanceData = null;
				_cubeGroup = null;
				_openParms = null;
				_weeksToPlan = null;
				_lowLevelVersionOverrideProfileList = null;
				_basisProfile = null;
				_versionProfList = null;
				_hnp = null;
			}
			// End MID Track #5210
		}

		public override void ProcessAction(SessionAddressBlock aSAB, ApplicationSessionTransaction aApplicationTransaction, ApplicationWorkFlowStep aWorkFlowStep, Profile aProfile, bool WriteToDB, int aStoreFilterRID)
		{
			try
			{
				beginTime = System.DateTime.Now;
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				bool ignoreLocks = false;
				// END MID Track #5647
				
				_applicationTransaction = aApplicationTransaction;
				
				try
				{
					string infoMsg = "OTS Forecast Balance Started: " + this.Name;
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, infoMsg, _sourceModule);

					// Begin MID Track 4858 - JSmith - Security changes
					// Need to check here incase part of workflow
					if (!AuthorizedToUpdate(_SAB.ApplicationServerSession, _SAB.ClientServerSession.UserRID))
					{
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_NotAuthorizedForNode, this.ToString());
                        // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                        //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                        // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
						_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
						return;
					}
					// End MID Track 4858
				
					// get the plan info once for the data cube.
					FillOpenParmForPlan();
					_cubeGroup = _applicationTransaction.CreateStoreMultiLevelPlanMaintCubeGroup();
					_cubeGroup.OpenCubeGroup(_openParms);

					if (_openParms.FilterRID != Include.NoRID)
					{
						// BEGIN Issue 5727 stodd
                        if (!((PlanCubeGroup)_cubeGroup).SetStoreFilter(_openParms.FilterRID, _cubeGroup))
						{
							FilterData storeFilterData = new FilterData();
							string msg = MIDText.GetText(eMIDTextCode.msg_FilterInvalid);
							msg = msg.Replace("{0}",storeFilterData.FilterGetName(_openParms.FilterRID));
							string suffix = ". Method " + this.Name + ". ";
							string auditMsg = msg + suffix;
							_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, eMIDTextCode.msg_FilterInvalid, auditMsg, this.ToString());
							throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_FilterInvalid, msg);
						}
						// END issue 5727
					}
					// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
					if (Matrix_Type == eMatrixType.Balance)
					{
						//******************
						// process matrix balance
						//******************
						if (_cubeGroup.GetType() == typeof(StoreMultiLevelPlanMaintCubeGroup))  // yes, save it
						{
							((StoreMultiLevelPlanMaintCubeGroup)_cubeGroup).MatrixBalance(VariableNumber, BalanceMode, IterationsCount, ePlanBasisType.Basis, 0);
						}
					} 
					else
					{
						//******************
						// process matrix forcast
						//******************
					    ((StoreMultiLevelPlanMaintCubeGroup)_cubeGroup).SpreadHighToLowLevelStore(VariableNumber, _useBasis, ignoreLocks);
					}
					// END MID Track #5647
								
					//******************
					// Save data
					//******************
					if (_cubeGroup.GetType() == typeof(StoreMultiLevelPlanMaintCubeGroup))  // yes, save it
					{
						infoMsg = "Saving values...";
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, infoMsg, _sourceModule);
				
						PlanSaveParms planSaveParms = new PlanSaveParms();
						planSaveParms.SaveStoreLowLevel = true;
						_cubeGroup.SaveCubeGroup(planSaveParms);
				
						infoMsg = "Saving Values Completed";
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, infoMsg, _sourceModule);
						_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
					}
						
				}
				catch (PlanInUseException)
				{
					_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;

					throw;
				}
				catch (SpreadFailed err)
				{
					_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, err.Message, _sourceModule);

					throw;
				}
				catch ( Exception err )
				{
					_applicationTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
					// BEGIN Issue 5401 stodd
					string msg = MIDText.GetText(eMIDTextCode.msg_MethodException);
					msg = msg.Replace("{0}", this.Name);
					msg = msg.Replace("{1}", err.ToString());
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, _sourceModule);
					// END Issue 5401
                    // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                    //_SAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
                    // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
								
					throw;
				}
				finally
				{
					// Cleanup & dequeue
					if (_cubeGroup != null &&
						_cubeGroup.GetType() == typeof(StoreMultiLevelPlanMaintCubeGroup))  
					{
						((StoreMultiLevelPlanMaintCubeGroup)_cubeGroup).CloseCubeGroup();
						// Begin MID Track #5210 - JSmith - Out of memory
						_cubeGroup.Dispose();
						_cubeGroup = null;
						// End MID Track #5210
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void LoadBasisList()
		{
			try
			{
				DataTable dt = _ForecastBalanceData.GetForecastBalanceBasis(this.Key);
				BasisProfile.BasisDetailProfileList.Clear();
				BasisDetailProfile basisDetailProfile;
				int i = 0;
				foreach(DataRow dr in dt.Rows)
				{
					basisDetailProfile = new BasisDetailProfile(i + 1, null);
					//Begin Track #4457 - JSmith - Add forecast versions
					ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
					basisDetailProfile.VersionProfile = fvpb.Build(Convert.ToInt32(dr["FV_RID"]));
					//End Track #4457
					basisDetailProfile.HierarchyNodeProfile = new HierarchyNodeProfile(HnRID);
					basisDetailProfile.DateRangeProfile = new DateRangeProfile(Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture));
					basisDetailProfile.DateRangeProfile.DisplayDate = Convert.ToString(dr["CDR_RID"], CultureInfo.CurrentUICulture);
					basisDetailProfile.DateRangeProfile.Name = "Basis Total";
					if (Include.ConvertCharToBool(Convert.ToChar(dr["IS_INCLUDED_IND"], CultureInfo.CurrentUICulture)) == true)
					{
						basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
					}
					else
					{
						basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Exclude;
					}
					if (dr["WEIGHT"] == DBNull.Value)
					{
						basisDetailProfile.Weight = 1;
					}
					else
					{
						basisDetailProfile.Weight = Convert.ToSingle(dr["WEIGHT"], CultureInfo.CurrentUICulture);
					}
					BasisProfile.BasisDetailProfileList.Add(basisDetailProfile);					
					// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
					_useBasis = true;
					// END MID Track #5647
					i++;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
        public void CheckDescendantsForDuplicates()
        {
            try
            {
                LoadVersionOverrideList();
            }
            catch (DuplicateOverrideListEntry ex)
            {
                _foundDuplicate = true;
                _duplicateMessage = ex.Message;
            }
        }
        // End TT#2281

		private void LoadVersionOverrideList()
		{
			LowLevelVersionOverrideProfileList.Clear();
            try
            {
                // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                _foundDuplicate = false;
                _duplicateMessage = string.Empty;
                // End TT#2281

                // BEGIN Override Low Level Enhancement
                HierarchySessionTransaction hTran = new HierarchySessionTransaction(_SAB);
                // BEGIN Track #6107 – John Smith - Cannot view departments in multi-level
                //_lowLevelVersionOverrideProfileList = hTran.GetOverrideList(_overrideLowLevelRid, this.HnRID, this.LowLevelVersionRID,
                //                                           _lowLevelsOffset, Include.NoRID, true, false);
                if (_lowLevelsType == eLowLevelsType.HierarchyLevel)
                {
                    _lowLevelVersionOverrideProfileList = hTran.GetOverrideList(_overrideLowLevelRid, this.HnRID, this.LowLevelVersionRID,
                                                          eHierarchyDescendantType.levelType, _lowLevelsSequence, Include.NoRID, true, false);
                }
                else
                {
                    // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                    //_lowLevelVersionOverrideProfileList = hTran.GetOverrideList(_overrideLowLevelRid, this.HnRID, this.LowLevelVersionRID,
                    //                                           _lowLevelsOffset, Include.NoRID, true, false);
                    _lowLevelVersionOverrideProfileList = hTran.GetOverrideListWithIgnore(_overrideLowLevelRid, this.HnRID, this.LowLevelVersionRID,
                                                               _lowLevelsOffset, Include.NoRID, true, false, false);
                    // End TT#2281
                }
                // End Track #6107
				// END Override Low Level Enhancement
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		/// <summary>
		/// Fills in the basis part (Balance) of the CubeGroup open parms.
		/// </summary>
		private void FillOpenParmForBasisBalance()
		{
			BasisProfile basisProfile = new BasisProfile(1, "basis", _openParms);
			BasisDetailProfile basisDetailProfile;
			if (BasisProfile.BasisDetailProfileList.Count > 0)
			{
				foreach (BasisDetailProfile bdp in BasisProfile.BasisDetailProfileList)
				{
					basisDetailProfile = new BasisDetailProfile(bdp.Key, _openParms);
					basisDetailProfile.VersionProfile = bdp.VersionProfile;
					basisDetailProfile.HierarchyNodeProfile = bdp.HierarchyNodeProfile;
					basisDetailProfile.DateRangeProfile = _SAB.ApplicationServerSession.Calendar.GetDateRange(bdp.DateRangeProfile.Key);
					basisDetailProfile.IncludeExclude = bdp.IncludeExclude;
					basisDetailProfile.Weight = bdp.Weight;
					basisProfile.BasisDetailProfileList.Add(basisDetailProfile);
				}
				_openParms.BasisProfileList.Add(basisProfile);
			}
		}

		/// <summary>
		/// Fills in the basis part (Forecast) of the CubeGroup open parms.
		/// </summary>
		private void FillOpenParmForBasisForecast(int sglRid)
		{
			BasisProfile basisProfile;
			BasisProfile currentBasisProfile;
			BasisProfile staticBasisProfile = null;
			BasisDetailProfile basisDetailProfile;
			int bdpKey = 1;
			int staticBdpKey = 1;
			int bpKey = 1;
			HierarchyNodeProfile hnp = null;

			//=======================
			// Set up Basis Profile
			//=======================

			if (BasisProfile.BasisDetailProfileList.Count > 0)
			{
				foreach (BasisDetailProfile bdp in BasisProfile.BasisDetailProfileList)
				{
					int basisHierNodeRid = this._hnRID;
					hnp = _SAB.HierarchyServerSession.GetNodeData(basisHierNodeRid, true, true);
					int basisVersionRid = bdp.VersionProfile.Key;
					int basisDateRangeRid = bdp.DateRangeProfile.Key;
					DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(basisDateRangeRid);

					// This determines whether to make a new BasisProfile/BasisDetail set (done for each Dyn to Plan)
					// or add the row as a BasisDetail to the StaticBasisProfile (done for Static and Dyn to Current)
					if (drp.DateRangeType == eCalendarRangeType.Dynamic && drp.RelativeTo == eDateRangeRelativeTo.Plan)
					{
						basisProfile = new BasisProfile(bpKey++, null, _openParms);
						_openParms.BasisProfileList.Add(basisProfile);
						basisProfile.BasisType = eTyLyType.NonTyLy;
						bdpKey = 1;
						currentBasisProfile = basisProfile;
					}
					else
					{
						if (staticBasisProfile == null)
						{
							staticBasisProfile = new BasisProfile(bpKey++, null, _openParms);
							_openParms.BasisProfileList.Add(staticBasisProfile);
						}
						staticBasisProfile.BasisType = eTyLyType.NonTyLy;
						bdpKey = staticBdpKey++;
						currentBasisProfile = staticBasisProfile;
					}

					basisDetailProfile = new BasisDetailProfile(bdpKey, _openParms);
					//Begin Track #4457 - JSmith - Add forecast versions
					ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
					basisDetailProfile.VersionProfile = fvpb.Build(basisVersionRid);
					//End Track #4457
					basisDetailProfile.HierarchyNodeProfile = hnp;
					basisDetailProfile.DateRangeProfile = drp;
					DateRangeProfile planDrp = _SAB.ApplicationServerSession.Calendar.GetDateRange(this._cdrRID);
					ProfileList weekRange = _SAB.ApplicationServerSession.Calendar.GetWeekRange(planDrp, null);
					basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
					basisDetailProfile.ForecastingInfo = new BasisDetailForecastInfo();
					basisDetailProfile.ForecastingInfo.PlanWeek = (WeekProfile)weekRange[0]; //Issue 4025
					basisDetailProfile.ForecastingInfo.OrigWeekListCount = basisDetailProfile.GetWeekProfileList(_SAB.ApplicationServerSession).Count;
					basisDetailProfile.ForecastingInfo.BasisPeriodList = _SAB.ApplicationServerSession.Calendar.GetDateRangePeriods(drp, (WeekProfile)weekRange[0]); //Issue 4025
					//basisDetailProfile.IncludeExclude = (eBasisIncludeExclude)Convert.ToInt32(_dsForcastSpread.Tables[1].Rows[row]["INCLUDE_EXCLUDE"], CultureInfo.CurrentUICulture);
					basisDetailProfile.Weight = bdp.Weight;
					currentBasisProfile.BasisDetailProfileList.Add(basisDetailProfile);
				}
			}
		}
		// END MID Track #5647

		///// <summary>
		///// Gets the low level node list without using a Low Level Override Model.
		///// </summary>
		///// <param name="aMethodRID"></param>
		//public void LoadVersionOverrideListNoModel()
		//{
		//    try
		//    {
		//        HierarchyNodeList hnl = null;
		//        if (this.LowLevelsType == eLowLevelsType.LevelOffset)
		//        {
		//            hnl = _SAB.HierarchyServerSession.GetDescendantData(this.HnRID, this.LowLevelsOffset, true, eNodeSelectType.NoVirtual, true);
		//        }
		//        else
		//        {
		//            hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(this.HnRID, this.LowLevelsSequence, true, eNodeSelectType.NoVirtual, true);
		//        }

		//        foreach (HierarchyNodeProfile hnp in hnl)
		//        {
		//            LowLevelVersionOverrideProfile llvop = new LowLevelVersionOverrideProfile(hnp.Key, hnp, false,
		//                new VersionProfile(this.LowLevelVersionRID), false);
		//            LowLevelVersionOverrideProfileList.Add(llvop);
		//        }
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}

//        private void PopulateVersionOverrideList()
//        {
//            try
//            {
//                HierarchyNodeList hnl = null;
//                if (LowLevelsType == eLowLevelsType.LevelOffset)
//                {
////Begin Track #4037 - JSmith - Optionally include dummy color in child list
//                    hnl = _SAB.HierarchyServerSession.GetDescendantData(HnRID, LowLevelsOffset, false, eNodeSelectType.NoVirtual);
////End Track #4037
//                }
//                else
//                {
////Begin Track #4037 - JSmith - Optionally include dummy color in child list
//                    hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(HnRID, LowLevelsSequence, false, eNodeSelectType.NoVirtual);
////End Track #4037
//                }

//                foreach (HierarchyNodeProfile hnp in hnl)
//                {
//                    LowLevelVersionOverrideProfile lvop = new LowLevelVersionOverrideProfile(hnp.Key);
//                    lvop.VersionIsOverridden = false;
//                    lvop.VersionProfile = null;
//                    lvop.Exclude = false;
//                    lvop.NodeProfile = hnp;
		
//                    LowLevelVersionOverrideProfileList.Add(lvop);
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

		/// <summary>
		/// Fills in the plan part of the CubeGroup open parms
		/// </summary>
		private void FillOpenParmForPlan()
		{
			try
			{
				_openParms = new PlanOpenParms(ePlanSessionType.StoreMultiLevel, ComputationMode);
				// high level values will not be updated, so set to read only
				if (this.GlobalUserType == eGlobalUserType.User)
				{
					_openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsUserOTSBalance);
				}
				else
				{
					_openParms.FunctionSecurityProfile = new FunctionSecurityProfile((int)eSecurityFunctions.ForecastMethodsGlobalOTSBalance);
				}
				_openParms.FunctionSecurityProfile.SetAllowUpdate();

				HNP.ChainSecurityProfile = new HierarchyNodeSecurityProfile(this.HnRID);
				HNP.ChainSecurityProfile.SetReadOnly();
				// Begin TT#5541 - JSmith - Matrix Balance Method not balancing Store High Level to the Store Low Levels
				//HNP.StoreSecurityProfile = new HierarchyNodeSecurityProfile(this.HnRID);
				//HNP.StoreSecurityProfile.SetReadOnly();
                HNP.StoreSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(this.HnRID, (int)eSecurityTypes.Store);
				// End TT#5541 - JSmith - Matrix Balance Method not balancing Store High Level to the Store Low Levels
				//Begin Track #4457 - JSmith - Add forecast versions
				ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
				VersionProfile vp = fvpb.Build(LowLevelVersionRID);
				//End Track #4457
				// Begin TT#5541 - JSmith - Matrix Balance Method not balancing Store High Level to the Store Low Levels
				//vp.StoreSecurity = new VersionSecurityProfile(this.LowLevelVersionRID);
				//vp.StoreSecurity.SetReadOnly();
				vp.StoreSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(this.LowLevelVersionRID, (int)eSecurityTypes.Store);
				// End TT#5541 - JSmith - Matrix Balance Method not balancing Store High Level to the Store Low Levels
				vp.ChainSecurity = new VersionSecurityProfile(this.LowLevelVersionRID);
				vp.ChainSecurity.SetReadOnly();
			
				_openParms.StoreHLPlanProfile.VersionProfile = vp;
				_openParms.StoreHLPlanProfile.NodeProfile = HNP;
				_openParms.ChainHLPlanProfile.VersionProfile = vp;
				_openParms.ChainHLPlanProfile.NodeProfile = HNP;
//Begin Track #4018 - JScott - Calendar Serialization error in Forecast Balance Method
				_openParms.DateRangeProfile = _SAB.ApplicationServerSession.Calendar.GetDateRange(this.CDR_RID);
//End Track #4018 - JScott - Calendar Serialization error in Forecast Balance Method
				//Begin Track #4457 - JSmith - Add forecast versions
				_openParms.LowLevelVersionDefault = fvpb.Build(LowLevelVersionRID);
				//End Track #4457
				
				_openParms.StoreGroupRID = _SAB.ClientServerSession.GlobalOptions.OTSPlanStoreGroupRID;
				_openParms.FilterRID = this.FilterRID;
			
				_openParms.LowLevelsType = this.LowLevelsType;
				_openParms.LowLevelsOffset = this.LowLevelsOffset;
				_openParms.LowLevelsSequence = this.LowLevelsSequence;

				_openParms.IneligibleStores = this.IneligibleStoresInd; 
				_openParms.SimilarStores = this.SimilarStoresInd;

				if (_computationMode != null)
				{
					_openParms.ComputationsMode = _computationMode;
				}
				else
				{
					_openParms.ComputationsMode = _SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
				}

				PopulateOpenParmVersionOverrideList();

				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				if (Matrix_Type == eMatrixType.Balance)
				{
					FillOpenParmForBasisBalance();
				} 
				else
				{
					FillOpenParmForBasisForecast(Include.AllStoreGroupRID);
				}
				// END MID Track #5647
				
			}
			catch
			{
				throw;
			}
		}

		private void PopulateOpenParmVersionOverrideList()
		{
			try
			{
				// BEGIN Override Low Level Enhancement
//                HierarchyNodeList hnl = null;
//                if (LowLevelsType == eLowLevelsType.LevelOffset)
//                {
////Begin Track #4588 - John Smith - Matrix not working
////Begin Track #4037 - JSmith - Optionally include dummy color in child list
//                    hnl = _SAB.HierarchyServerSession.GetDescendantData(HnRID, LowLevelsOffset, true, eNodeSelectType.NoVirtual);
////End Track #4037
//                }
//                else
//                {
////Begin Track #4037 - JSmith - Optionally include dummy color in child list
//                    hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(HnRID, LowLevelsSequence, true, eNodeSelectType.NoVirtual);
////End Track #4037
////End Track #4588
//                }

				foreach (LowLevelVersionOverrideProfile lvop in LowLevelVersionOverrideProfileList.ArrayList)
				{
				// END Override Low Level Enhancement
					bool exclude = false;
					PlanProfile planProfile = new PlanProfile(lvop.NodeProfile.Key);
					if (lvop == null)
					{
						planProfile.NodeProfile = lvop.NodeProfile;
						planProfile.VersionProfile = (VersionProfile)VersionProfList.FindKey(_openParms.LowLevelVersionDefault.Key);
					}
					else
					{
						if (!lvop.Exclude)
						{
//Begin Track #4588 - John Smith - Matrix not working
							planProfile.NodeProfile = lvop.NodeProfile;
//End Track #4588
							if (lvop.VersionIsOverridden)
							{
								planProfile.VersionProfile = (VersionProfile)VersionProfList.FindKey(lvop.VersionProfile.Key);
							}
							else
							{
								planProfile.VersionProfile = (VersionProfile)VersionProfList.FindKey(_openParms.LowLevelVersionDefault.Key);
							}
						}
						else
						{
							exclude = true;
						}
					}
		
					if (!exclude)
					{
						planProfile.NodeProfile.ChainSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.NodeProfile.Key, (int)eSecurityTypes.Chain);
						planProfile.NodeProfile.StoreSecurityProfile = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.NodeProfile.Key, (int)eSecurityTypes.Store);

//Begin Track #3867 -- Low level not sorted on Store Multi view
						_openParms.AddLowLevelPlanProfile(planProfile);
//End Track #3867 -- Low level not sorted on Store Multi view
					}
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Updates the OTS Plan method
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		//		new public void Update(TransactionData td)
		override public void Update(TransactionData td)

		{
			int versionKey;
			int seq = 0;
			bool isIncluded = true;
			if (_ForecastBalanceData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_ForecastBalanceData = new OTSForecastBalanceMethodData();
			}
			
			_ForecastBalanceData.Method_RID = this.Key;
			_ForecastBalanceData.FilterRID = _filterRID;
			_ForecastBalanceData.HnRID= _hnRID;
			_ForecastBalanceData.HighLevelVersionRID = _highLevelVersionRID;
			_ForecastBalanceData.CDR_RID = _cdrRID;
			// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
			_ForecastBalanceData.Model_RID = _modelRID;
			_ForecastBalanceData.Matrix_Type = _matrixType;
			// END MID Track #5647
			_ForecastBalanceData.LowLevelVersionRID = _lowLevelVersionRID;
			_ForecastBalanceData.LowLevelsType = _lowLevelsType;
			_ForecastBalanceData.LowLevelOffset = _lowLevelsOffset;
			_ForecastBalanceData.LowLevelSequence = _lowLevelsSequence;
			_ForecastBalanceData.SimilarStoresInd = _similarStoresInd;
			_ForecastBalanceData.IneligibleStoresInd = _ineligibleStoresInd;
			_ForecastBalanceData.VariableNumber = _variableNumber;
			_ForecastBalanceData.IterationType = _iterationType;
			_ForecastBalanceData.IterationsCount = _iterationsCount;
			_ForecastBalanceData.BalanceMode = _balanceMode;
			_ForecastBalanceData.ComputationMode = _computationMode;
			_ForecastBalanceData.OverrideLowLevelRid = _overrideLowLevelRid;  // Override low level enhancement
			try
			{
				switch (this.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);	// creates rec on METHOD table
						_ForecastBalanceData.InsertForecastBalance(base.Key, td);  // adds matrix table

						seq = 1;
						isIncluded = true;
						foreach(BasisDetailProfile basisDetailProfile in _basisProfile.BasisDetailProfileList)
						{
							if (basisDetailProfile.IncludeExclude == eBasisIncludeExclude.Include)
							{
								isIncluded = true;
							}
							else
							{
								isIncluded = false;
							}
							_ForecastBalanceData.InsertForecastBalanceBasis(base.Key, td, seq, 
								basisDetailProfile.VersionProfile.Key,
								basisDetailProfile.DateRangeProfile.Key,
								basisDetailProfile.Weight,
								isIncluded);
							seq++;
						}
						// BEGIN Override low level enhanacement
//                        foreach(LowLevelVersionOverrideProfile lvop in LowLevelVersionOverrideProfileList)
//                        {
//                            if (lvop.VersionIsOverridden ||
//                                lvop.Exclude)
//                            {
//                                if (lvop.VersionProfile != null)
//                                {
//                                    versionKey = lvop.VersionProfile.Key;
//                                }
//                                else
//                                {
//                                    versionKey = _lowLevelVersionRID;
//                                }
//                                _ForecastBalanceData.InsertForecastBalanceVersionOverride(base.Key, td,  
//                                    lvop.NodeProfile.Key,
////									lvop.VersionProfile.Key,
//                                    versionKey,
//                                    lvop.Exclude);
//                            }
//                        }
						// END Override low level enhanacement

						break;


					case eChangeType.update:
						base.Update(td);	// updates rec on METHOD table
						_ForecastBalanceData.UpdateForecastBalance(this.Key, td);  // Updates matrix table
						_ForecastBalanceData.DeleteForecastBalanceBasis(this.Key, td);

						seq = 1;
						isIncluded = true;
						foreach(BasisDetailProfile basisDetailProfile in _basisProfile.BasisDetailProfileList)
						{
							if (basisDetailProfile.IncludeExclude == eBasisIncludeExclude.Include)
							{
								isIncluded = true;
							}
							else
							{
								isIncluded = false;
							}
							_ForecastBalanceData.InsertForecastBalanceBasis(base.Key, td, seq, 
								basisDetailProfile.VersionProfile.Key,
								basisDetailProfile.DateRangeProfile.Key,
								basisDetailProfile.Weight,
								isIncluded);
							seq++;
						}

						// BEGIN Override low level enhanacement
						//_ForecastBalanceData.DeleteForecastBalanceVersionOverride(this.Key, td);
						//foreach(LowLevelVersionOverrideProfile lvop in LowLevelVersionOverrideProfileList)
						//{
						//    if (lvop.VersionIsOverridden ||
						//        lvop.Exclude)
						//    {
						//        if (lvop.VersionProfile != null)
						//        {
						//            versionKey = lvop.VersionProfile.Key;
						//        }
						//        else
						//        {
						//            versionKey = _lowLevelVersionRID;
						//        }
						//        _ForecastBalanceData.InsertForecastBalanceVersionOverride(base.Key, td,  
						//            lvop.NodeProfile.Key,
						//            versionKey,
						//            lvop.Exclude);
						//    }
						//}
						// END Override low level enhanacement
						break;

					case eChangeType.delete:
						_ForecastBalanceData.DeleteForecastBalanceBasis(this.Key, td);
						// BEGIN Override low level enhanacement
						//_ForecastBalanceData.DeleteForecastBalanceVersionOverride(this.Key, td);
						// END Override low level enhanacement
						_ForecastBalanceData.DeleteForecastBalance(this.Key, td);
						base.Update(td);	

						break;
				}
			}
			catch (Exception e)
			{
				string message = e.ToString();
				throw;
			}
			finally
			{

				//TO DO:  whatever has to be done after an update or exception.
			}
		}

		public override bool WithinTolerance(double aTolerancePercent)
		{
			try
			{

				if (((StoreMultiLevelPlanMaintCubeGroup)_cubeGroup).GetTolerance(VariableNumber, BalanceMode) < aTolerancePercent)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				throw;
			}
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
			OTSForecastBalanceMethod newOTSForecastBalanceMethod = null;
            // Begin Track #5912 - JSmith - Save As needs to clone custom override models
            OverrideLowLevelProfile ollp;
            int customUserRID;
            // End Track #5912

			try
			{
				newOTSForecastBalanceMethod = (OTSForecastBalanceMethod)this.MemberwiseClone();
				newOTSForecastBalanceMethod.BalanceMode = BalanceMode;
				if (aCloneDateRanges &&
					CDR_RID != Include.UndefinedCalendarDateRange)
				{
					newOTSForecastBalanceMethod.CDR_RID = aSession.Calendar.GetDateRangeClone(CDR_RID).Key;
				}
				else
				{
					newOTSForecastBalanceMethod.CDR_RID = CDR_RID;
				}
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				newOTSForecastBalanceMethod.Model_RID = Model_RID;
				newOTSForecastBalanceMethod.Matrix_Type = Matrix_Type;
				// END MID Track #5647
				newOTSForecastBalanceMethod.ComputationMode = ComputationMode;
				newOTSForecastBalanceMethod.FilterRID = FilterRID;
				newOTSForecastBalanceMethod.HighLevelVersionRID = HighLevelVersionRID;
				newOTSForecastBalanceMethod.HnRID = HnRID;
				newOTSForecastBalanceMethod.IneligibleStoresInd = IneligibleStoresInd;
				newOTSForecastBalanceMethod.IterationsCount = IterationsCount;
				newOTSForecastBalanceMethod.IterationType = IterationType;
				newOTSForecastBalanceMethod.LowLevelsOffset = LowLevelsOffset;
				newOTSForecastBalanceMethod.LowLevelsSequence = LowLevelsSequence;
				newOTSForecastBalanceMethod.LowLevelsType = LowLevelsType;
				newOTSForecastBalanceMethod.LowLevelVersionRID = LowLevelVersionRID;
				newOTSForecastBalanceMethod.Method_Change_Type = eChangeType.none;
				newOTSForecastBalanceMethod.Method_Description = Method_Description;
				newOTSForecastBalanceMethod.MethodStatus = MethodStatus;
				newOTSForecastBalanceMethod.Name = Name;
				newOTSForecastBalanceMethod.SG_RID = SG_RID;
				newOTSForecastBalanceMethod.SimilarStoresInd = SimilarStoresInd;
				newOTSForecastBalanceMethod.User_RID = User_RID;
				newOTSForecastBalanceMethod.VariableNumber = VariableNumber;
				newOTSForecastBalanceMethod.Virtual_IND = Virtual_IND;
                newOTSForecastBalanceMethod.Template_IND = Template_IND;
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
                    newOTSForecastBalanceMethod.CustomOLL_RID = customUserRID;
                }
                // End Track #5912
				// BEGIN Override low level enhanacement
				newOTSForecastBalanceMethod.OverrideLowLevelRid = OverrideLowLevelRid;
				// END Override low level enhanacement

				newOTSForecastBalanceMethod._basisProfile = _basisProfile.Copy(aSession, aCloneDateRanges);

				// BEGIN Override low level enhanacement
				//CopyVersionOverrideList(newOTSForecastBalanceMethod);
				// END Override low level enhanacement

				return newOTSForecastBalanceMethod;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		// BEGIN Override low level enhancement
		//private void CopyVersionOverrideList(OTSForecastBalanceMethod OTSForecastBalanceMethod)
		//{
		//    try
		//    {
		//        OTSForecastBalanceMethod._lowLevelVersionOverrideProfileList = new ProfileList(eProfileType.LowLevelVersionOverride);
		//        foreach(LowLevelVersionOverrideProfile lvop in LowLevelVersionOverrideProfileList)
		//        {
		//            LowLevelVersionOverrideProfile newlvop = lvop.Copy();
		//            if (newlvop.VersionProfile == null)
		//            {
		//                newlvop.VersionProfile = (VersionProfile)VersionProfList.FindKey(_lowLevelVersionRID);
		//            }
		//            OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.Add(newlvop);
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}
		// END Override low level enhancement

		// Begin MID Track 4858 - JSmith - Security changes
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
				if (LowLevelVersionRID == Include.NoRID ||
					HnRID == Include.NoRID)
				{
					return false;
				}
				FillOpenParmForPlan();
				foreach (PlanProfile planProf in _openParms.LowLevelPlanProfileList)
				{
					if (planProf.VersionProfile == null ||
						!planProf.VersionProfile.StoreSecurity.AllowUpdate)
					{
						return false;
					}
					HierarchyNodeSecurityProfile hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(planProf.NodeProfile.Key, (int)eSecurityTypes.Store);
					if (!hierNodeSecurity.AllowUpdate)
					{
						return false;
					}
				}
				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
        // End MID Track 4858

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalOTSBalance);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserOTSBalance);
            }         
        }

        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            ROOverrideLowLevel overrideLowLevel = new ROOverrideLowLevel();
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

            // Begin RO-740 RDewey
            ROLevelInformation lowLevelInformation = new ROLevelInformation();
            eROLevelsType levelType;
            string strLevelType;

            lowLevelInformation.LevelType = (eROLevelsType)_lowLevelsType;
            lowLevelInformation.LevelSequence = _lowLevelsSequence;
            lowLevelInformation.LevelOffset = _lowLevelsOffset;


            ROMethodMatrixBalanceProperties method = new ROMethodMatrixBalanceProperties(
                method: GetName.GetMethod(method: this),
                description: Method_Description,
                userKey: User_RID,
                filter: GetName.GetFilterName(key: _filterRID),
                highLevelMerchandise: GetName.GetMerchandiseName(nodeRID: _hnRID, SAB: SAB),
                highLevelVersion: GetName.GetVersion(versionRID: _highLevelVersionRID, SAB: SAB),
                dateRange: GetName.GetCalendarDateRange(calendarDateRID: _cdrRID, SAB: SAB),
                lowLevelVersion: GetName.GetVersion(versionRID: _lowLevelVersionRID, SAB: SAB),
                lowLevel: lowLevelInformation,
                ineligibleStores: _ineligibleStoresInd,
                similarStores: _similarStoresInd,
                variable: GetName.GetVariable(variableKey: _variableNumber, SAB: SAB),
                iterationType: _iterationType,
                iterationsCount: _iterationsCount,
                balanceMode: _balanceMode,
                computationMode: _computationMode,
                overrideLowLevel: overrideLowLevel,
                matrixType: _matrixType,
                model: GetName.GetForecastBalanceModel(modelRID: _modelRID, SAB: SAB),
                matrixBasis: new System.Collections.Generic.List<ROBasisDetailProfile>(),
                isTemplate: Template_IND
                );

            lowLevelInformation.LevelValue = GetName.GetLevelName(lowLevelInformation.LevelType, lowLevelInformation.LevelSequence, lowLevelInformation.LevelOffset, SAB);

            foreach (BasisDetailProfile basisProfile in BasisProfile.BasisDetailProfileList)
            {
                ROBasisDetailProfile options = new ROBasisDetailProfile(
                    iBasisId: basisProfile.Key,
                    iMerchandiseId: method.HighLevelMerchandise.Key,
                    sMerchandise: method.HighLevelMerchandise.Value,
                    iVersionId: basisProfile.VersionProfile.Key,
                    sVersion: basisProfile.VersionProfile.Description,
                    iDaterangeId: basisProfile.DateRangeProfile.Key,
                    sDateRange: basisProfile.DateRangeProfile.DisplayDate,
                    sPicture: null,
                    fWeight: basisProfile.Weight,
                    bIsIncluded: false,
                    sIncludeButton: null
                    );

                if (basisProfile.IncludeExclude == eBasisIncludeExclude.Include)
                {
                    options.IsIncluded = true;
                }

                KeyValuePair<int, string> kvpVersion, kvpDateRange;
                kvpVersion = GetName.GetVersion(versionRID: options.VersionId, SAB: SAB);
                options.Version = kvpVersion.Value;
                kvpDateRange = GetName.GetCalendarDateRange(options.DateRangeId, SAB: SAB, anchorDateRID: _cdrRID);
                options.DateRange = kvpDateRange.Value;

                method.MatrixBasis.Add(options);
            }

            BuildFilterList(method: method);

            BuildVersionLists(method: method);

            if (_hnRID > 0)
            {
                //BuildLowLevelLists(method: method);
                eMerchandiseType lowLevelMerchandiseType = method.LowLevelsType;
                lowLevelInformation = method.LowLevel;

                // build the low level list based on the selected merchandise
                BuildLowLevelsList(
                    hierarchyNodeRID: _hnRID,
                    lowLevels: method.LowLevels,
                    lowLevelMerchandiseType: ref lowLevelMerchandiseType
                    );

                // adjust the from and to lists along with the to level based on the selected from level
                AdjustLevelList(
                    lowLevel: ref lowLevelInformation,
                    lowLevels: method.LowLevels,
                    lowLevelMerchandiseType: ref lowLevelMerchandiseType
                    );

                method.LowLevelsType = lowLevelMerchandiseType;
                method.LowLevel = lowLevelInformation;
                LowLevelsType = (eLowLevelsType)method.LowLevel.LevelType;
                LowLevelsOffset = method.LowLevel.LevelOffset;
                LowLevelsSequence = method.LowLevel.LevelSequence;
            }

            BuildVariablesList(method: method);

            BuildIterationsList(method: method);

            return method;
        }

        private void BuildFilterList(ROMethodMatrixBalanceProperties method)
        {
            FilterData storeFilterDataLayer = new FilterData();
            FunctionSecurityProfile filterUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
            FunctionSecurityProfile filterGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);

            ArrayList userRIDList = new ArrayList();

            userRIDList.Add(-1);

            if (filterUserSecurity.AllowUpdate || filterUserSecurity.AllowView)
            {
                userRIDList.Add(SAB.ClientServerSession.UserRID);
            }

            if (filterGlobalSecurity.AllowUpdate || filterGlobalSecurity.AllowView)
            {
                userRIDList.Add(Include.GlobalUserRID);
            }

            DataTable dtFilter = storeFilterDataLayer.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, userRIDList);

            foreach (DataRow row in dtFilter.Rows)
            {
                int filterKey = Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture);
                string filterName = Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture);
                method.Filters.Add(new KeyValuePair<int, string>(filterKey, filterName));
            }
        }

        private void BuildVersionLists(ROMethodMatrixBalanceProperties method)
        {
            ProfileList versionList;
            versionList = GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store, false, _highLevelVersionRID, true);

            foreach (VersionProfile versionProfile in versionList)
            {
                method.HighLevelVersions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }

            versionList = GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store, false, _lowLevelVersionRID, true);

            foreach (VersionProfile versionProfile in versionList)
            {
                method.LowLevelVersions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }

            versionList = GetForecastVersionList(
                eSecuritySelectType.View | eSecuritySelectType.Update, 
                eSecurityTypes.Store | eSecurityTypes.Chain
                );

            foreach (VersionProfile versionProfile in versionList)
            {
                method.BasisVersions.Add(new KeyValuePair<int, string>(versionProfile.Key, versionProfile.Description));
            }
        }

        private void BuildVariablesList(ROMethodMatrixBalanceProperties method)
        {
            foreach (VariableProfile variableProfile in SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList)
            {
                if (variableProfile.AllowForecastBalance)
                {
                    method.Variables.Add(new KeyValuePair<int, string>(variableProfile.Key, variableProfile.VariableName));
                }
            }
        }

        private void BuildIterationsList(ROMethodMatrixBalanceProperties method)
        {
            for (int i = 1; i < 10; i++)
            {
                method.Iterations.Add(i);
            }
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            ROMethodMatrixBalanceProperties roMethodMatrixBalanceProperties = (ROMethodMatrixBalanceProperties)methodProperties;
            try
            {
                Template_IND = methodProperties.IsTemplate;
                _highLevelVersionRID = roMethodMatrixBalanceProperties.HighLevelVersion.Key;
                _hnRID = roMethodMatrixBalanceProperties.HighLevelMerchandise.Key;
                _filterRID = roMethodMatrixBalanceProperties.Filter.Key;
                _cdrRID = roMethodMatrixBalanceProperties.DateRange.Key;
                _lowLevelVersionRID = roMethodMatrixBalanceProperties.LowLevelVersion.Key;
                _lowLevelsOffset = roMethodMatrixBalanceProperties.LowLevel.LevelOffset;
                _lowLevelsSequence = roMethodMatrixBalanceProperties.LowLevel.LevelSequence;
                _lowLevelsType = (eLowLevelsType)roMethodMatrixBalanceProperties.LowLevel.LevelType;
                _similarStoresInd = roMethodMatrixBalanceProperties.SimilarStores;
                _ineligibleStoresInd = roMethodMatrixBalanceProperties.IneligibleStores;
                _variableNumber = roMethodMatrixBalanceProperties.Variable.Key;
                _iterationsCount = roMethodMatrixBalanceProperties.IterationsCount;
                _iterationType = eIterationType.Custom;
                _balanceMode = roMethodMatrixBalanceProperties.BalanceMode;
                _computationMode = roMethodMatrixBalanceProperties.ComputationMode;
                _matrixType = roMethodMatrixBalanceProperties.MatrixType;
                _overrideLowLevelRid = roMethodMatrixBalanceProperties.OverrideLowLevel.OverrideLowLevelsModel.Key;
                _modelRID = roMethodMatrixBalanceProperties.Model.Key;

                // matrix type is forecast, override to required values
                if (_matrixType == eMatrixType.Forecast)
                {
                    _balanceMode = eBalanceMode.Store;
                    _iterationsCount = 1;
                }


                BasisProfile.BasisDetailProfileList.Clear();
                BasisDetailProfile basisDetailProfile;
                int i = 0;
                foreach (ROBasisDetailProfile basisDetail in roMethodMatrixBalanceProperties.MatrixBasis)
                {
                    basisDetailProfile = new BasisDetailProfile(i + 1, null);
                    //Begin Track #4457 - JSmith - Add forecast versions
                    ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                    basisDetailProfile.VersionProfile = fvpb.Build(Convert.ToInt32(basisDetail.VersionId));
                    //End Track #4457
                    basisDetailProfile.HierarchyNodeProfile = new HierarchyNodeProfile(basisDetail.MerchandiseId);
                    basisDetailProfile.DateRangeProfile = new DateRangeProfile(Convert.ToInt32(basisDetail.DateRangeId, CultureInfo.CurrentUICulture));
                    basisDetailProfile.DateRangeProfile.DisplayDate = Convert.ToString(basisDetail.DateRangeId, CultureInfo.CurrentUICulture);
                    basisDetailProfile.Weight = Convert.ToSingle(basisDetail.Weight, CultureInfo.CurrentUICulture);
                    basisDetailProfile.DateRangeProfile.Name = "Basis Total";
                    if (basisDetail.IsIncluded == true)
                    {
                        basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
                    }
                    else
                    {
                        basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Exclude;
                    }
                   
                    BasisProfile.BasisDetailProfileList.Add(basisDetailProfile);
                    // BEGIN MID Track #5647 - KJohnson - Matrix Forecast
                    _useBasis = true;
                    // END MID Track #5647
                    i++;
                }

                return true;
            }
            catch
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
}
