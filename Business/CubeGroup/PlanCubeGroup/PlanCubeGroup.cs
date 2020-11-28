using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Diagnostics;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// The PlanEnqueueInfo class is used to store information about a plan that is to be enqueued.
	/// </summary>

	public class PlanEnqueueInfo
	{
		//=======
		// FIELDS
		//=======

		private SessionAddressBlock _SAB;
		private PlanProfile _planProfile;
		private ePlanType _planType;
		private int _startWeekKey;
		private int _endWeekKey;
		private PlanEnqueue _planEnqueue;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanEnqueueInfo using the given PlanProfile and ePlanType.
		/// </summary>
		/// <param name="aPlanProfile">
		/// The PlanProfile of the plan to enqueue.
		/// </param>
		/// <param name="aPlanType">
		/// The ePlanType of the plan to enqueue.
		/// </param>
		/// <param name="aStartWeekKey">
		/// The starting week key of the plan to enqueue.
		/// </param>
		/// <param name="aEndWeekKey">
		/// The ending week key of the plan to enqueue.
		/// </param>

		public PlanEnqueueInfo(SessionAddressBlock aSAB, PlanProfile aPlanProfile, ePlanType aPlanType, int aStartWeekKey, int aEndWeekKey)
		{
			try
			{
				_SAB = aSAB;
				_planProfile = aPlanProfile;
				_planType = aPlanType;
				_startWeekKey = aStartWeekKey;
				_endWeekKey = aEndWeekKey;

				_planEnqueue = new PlanEnqueue(
					_planType,
					_planProfile.VersionProfile.Key,
					_planProfile.NodeProfile.Key,
					_startWeekKey,
					_endWeekKey,
					_SAB.ClientServerSession.UserRID,
					_SAB.ClientServerSession.ThreadID);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the PlanProfile for this PlanEnqueueInfo.
		/// </summary>
		
		public PlanProfile PlanProfile
		{
			get
			{
				return _planProfile;
			}
		}

		/// <summary>
		/// Gets the PlanType for this PlanEnqueueInfo.
		/// </summary>
		
		public ePlanType PlanType
		{
			get
			{
				return _planType;
			}
		}

		/// <summary>
		/// Gets the starting week key for this PlanEnqueueInfo.
		/// </summary>
		
		public int StartWeekKey
		{
			get
			{
				return _startWeekKey;
			}
		}

		/// <summary>
		/// Gets the ending week key for this PlanEnqueueInfo.
		/// </summary>
		
		public int EndWeekKey
		{
			get
			{
				return _endWeekKey;
			}
		}

		/// <summary>
		/// Gets the PlanEnqueue for this PlanEnqueueInfo.
		/// </summary>

		public PlanEnqueue PlanEnqueue
		{
			get
			{
				return _planEnqueue;
			}
		}

		//========
		// METHODS
		//========
	}

	/// <summary>
	/// The PlanEnqueueGroup class is used to store information about a group of plans that are to be enqueued.
	/// </summary>

	public class PlanEnqueueGroup
	{
		//=======
		// FIELDS
		//=======

		private ArrayList _enqueuedList;
		private ArrayList _conflictList;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of PlanEnqueueGroup using the given ArrayList of PlanEnqueueInfo objects.
		/// </summary>
		
		public PlanEnqueueGroup()
		{
			try
			{
				_enqueuedList = new ArrayList();
				_conflictList = new ArrayList();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets a boolean indicating if there are conflicts.
		/// </summary>

		public bool isConflict
		{
			get
			{
				return _conflictList.Count > 0;
			}
		}

		/// <summary>
		/// Gets the ArrayList of enqueued Plans.
		/// </summary>

		public ArrayList EnqueuedList
		{
			get
			{
				return _enqueuedList;
			}
		}

		/// <summary>
		/// Gets the ArrayList of conflicts.
		/// </summary>

		public ArrayList ConflictList
		{
			get
			{
				return _conflictList;
			}
		}

		//========
		// METHODS
		//========

		public void EnqueuePlan(SessionAddressBlock aSAB, PlanEnqueueInfo aPlanEnqueueInfo,
			bool aAllowReadOnlyOnConflict, bool aFormatErrorsForMessageBox, bool aUpdateAuditHeaderOnError)
		{
			ArrayList planEnqInfoLst;

			try
			{
				planEnqInfoLst = new ArrayList();
				planEnqInfoLst.Add(aPlanEnqueueInfo);
				EnqueuePlans(aSAB, planEnqInfoLst, aAllowReadOnlyOnConflict, aFormatErrorsForMessageBox, aUpdateAuditHeaderOnError);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that enqueues the plans in the group.
		/// </summary>

		public void EnqueuePlans(SessionAddressBlock aSAB, ArrayList aPlanEnqInfoList, bool aAllowReadOnlyOnConflict,
			bool aFormatErrorsForMessageBox, bool aUpdateAuditHeaderOnError)
		{
			bool allowUpdate;
			string errMsg;
			System.Windows.Forms.DialogResult diagResult;
			Hashtable forecastVersionHash = new Hashtable();
			string msg;

			try
			{
				foreach (PlanEnqueueInfo planEnqInfo in aPlanEnqInfoList)
				{
					allowUpdate = false;

					switch (planEnqInfo.PlanType)
					{
						case ePlanType.Chain :

							if (planEnqInfo.PlanProfile.NodeProfile.ChainSecurityProfile.AllowUpdate &&
								planEnqInfo.PlanProfile.VersionProfile.ChainSecurity.AllowUpdate)
							{
								allowUpdate = true;
							}

							break;

						case ePlanType.Store :

							if (planEnqInfo.PlanProfile.NodeProfile.StoreSecurityProfile.AllowUpdate &&
								planEnqInfo.PlanProfile.VersionProfile.StoreSecurity.AllowUpdate)
							{
								allowUpdate = true;
							}

							break;
					}

					// make blended version read only if version not same as forecast version
					if (allowUpdate &&
						planEnqInfo.PlanProfile.VersionProfile.IsBlendedVersion &&
						planEnqInfo.PlanProfile.VersionProfile.ForecastVersionRID != planEnqInfo.PlanProfile.VersionProfile.Key)
					{
						allowUpdate = false;

						if (!forecastVersionHash.ContainsKey(planEnqInfo.PlanProfile.VersionProfile.Key))
						{
							forecastVersionHash.Add(planEnqInfo.PlanProfile.VersionProfile.Key, planEnqInfo.PlanProfile.VersionProfile.Description);

							msg = MIDText.GetTextOnly(eMIDTextCode.msg_pl_ForecastVersionReadOnly);
							msg = msg.Replace("{0}", planEnqInfo.PlanProfile.VersionProfile.Description);

							diagResult = aSAB.MessageCallback.HandleMessage(
								msg,
								"Plan Will Open Read Only",
								System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information);

							if (diagResult != System.Windows.Forms.DialogResult.OK)
							{
								aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msg, this.ToString());
                                // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                                //aSAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", aSAB.GetHighestAuditMessageLevel());
                                // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
								throw new PlanInUseException();
							}
						}
					}

					if (allowUpdate)
					{
						try
						{
							planEnqInfo.PlanEnqueue.EnqueuePlan();
							_enqueuedList.Add(planEnqInfo);
						}
						catch (PlanConflictException)
						{
							_conflictList.Add(planEnqInfo);
						}
						catch (Exception exc)
						{
							string message = exc.ToString();
							throw;
						}
					}
				}

				if (isConflict)
				{
					errMsg = MIDText.GetTextOnly(eMIDTextCode.msg_pl_PlanInUseLine1);

					if (aFormatErrorsForMessageBox)
					{
						errMsg += System.Environment.NewLine;
					}

					errMsg += FormatConflictList(aFormatErrorsForMessageBox);

					if (aFormatErrorsForMessageBox)
					{
						errMsg += System.Environment.NewLine + System.Environment.NewLine;
					}

					if (aAllowReadOnlyOnConflict)
					{
                        if (MIDEnvironment.isWindows)
                        {
                            errMsg += MIDText.GetTextOnly(eMIDTextCode.msg_pl_PlanInUseContinue);

                            diagResult = aSAB.MessageCallback.HandleMessage(
                                errMsg,
                                "Plan Conflict",
                                System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Asterisk);
                        }
                        else
                        {
                            MIDEnvironment.isChangedToReadOnly = true;

                            errMsg += MIDText.GetTextOnly(eMIDTextCode.msg_ReadOnlyMode);

                            diagResult = aSAB.MessageCallback.HandleMessage(
                                errMsg,
                                "Plan Conflict",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);

                            MIDEnvironment.Message = errMsg;
                        }

						if (diagResult == System.Windows.Forms.DialogResult.OK)
						{
							foreach (PlanEnqueueInfo planEnqInfo in ConflictList)
							{
								switch (planEnqInfo.PlanType)
								{
									case ePlanType.Chain :
										planEnqInfo.PlanProfile.NodeProfile.ChainSecurityProfile.SetReadOnly();
										break;

									case ePlanType.Store :
										planEnqInfo.PlanProfile.NodeProfile.StoreSecurityProfile.SetReadOnly();
										break;
								}
							}
						}
						else
						{
							aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errMsg, this.ToString());

                            // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                            //if (aUpdateAuditHeaderOnError)
                            //{
                            //    aSAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", aSAB.GetHighestAuditMessageLevel());
                            //}
                            // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running

							throw new PlanInUseException(errMsg);
						}
					}
					else
					{
						if (aFormatErrorsForMessageBox)
						{
							errMsg += MIDText.GetTextOnly(eMIDTextCode.msg_pl_PlanInUseReselect);

							aSAB.MessageCallback.HandleMessage(
								errMsg,
								"Plan Conflict",
								System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

							aSAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errMsg, this.ToString());
						}
                        // Begin TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running
                        //if (aUpdateAuditHeaderOnError)
                        //{
                        //    aSAB.ApplicationServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", aSAB.GetHighestAuditMessageLevel());
                        //}
                        // End TT#504-MD - JSmith - Release Action causes the Audit to mark the Client as Stopped instead of Running

						throw new PlanInUseException();
					}
				}
			}
			catch (PlanInUseException)
			{
				DequeuePlans();
				throw;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        /// <summary>
        /// Method that formats the date display text.
        /// </summary>

        // begin MID Track 5116:  Display Text Problem
        private string FormatAnchorDateDisplay(Profile anchorDate)
        {
            string dateDisplay = "";

            switch (anchorDate.ProfileType)
            {
                case eProfileType.Day:
                    DayProfile anchorDay = (DayProfile)anchorDate;
                    dateDisplay = anchorDay.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture) + "/" +
                        anchorDay.DayInYear.ToString("000", CultureInfo.CurrentUICulture);
                    break;
                case eProfileType.Week:
                    WeekProfile anchorWeek = (WeekProfile)anchorDate;
                    dateDisplay = "Week " + anchorWeek.WeekInYear.ToString("00", CultureInfo.CurrentUICulture) + "/" +
                        anchorWeek.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);
                    break;
                case eProfileType.Period:
                    PeriodProfile anchorPeriod = (PeriodProfile)anchorDate;
                    dateDisplay = anchorPeriod.Abbreviation + " " +
                        anchorPeriod.FiscalYear.ToString("0000", CultureInfo.CurrentUICulture);
                    break;
            }

            return dateDisplay;
        }
        // end MID Track 5116:  Display Text Problem


		/// <summary>
		/// Method that dequeues the plans in the group.
		/// </summary>

		public void DequeuePlans()
		{
			try
			{
				foreach (PlanEnqueueInfo planEnqInfo in _enqueuedList)
				{
					planEnqInfo.PlanEnqueue.DequeuePlan();
				}

				_enqueuedList.Clear();
				_conflictList.Clear();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that formats the information of conflicted plans into a printable string.
		/// </summary>
		/// <returns>
		/// A string of the conflicted plans in printable format.
		/// </returns>

		public string FormatConflictList(bool aFormatErrorsForMessageBox)
		{
			string message = "";
            //SecurityAdmin secAdmin; //TT#827-MD -jsobek -Allocation Reviews Performance
            // begin MID Track 5116:  Display Text Problem
            MRSCalendar _calendar = new MRSCalendar();
            // end MID Track 5116:  Display Text Problem

			try
			{
                //secAdmin = new SecurityAdmin(); //TT#827-MD -jsobek -Allocation Reviews Performance

				foreach (PlanEnqueueInfo planEnqInfo in _conflictList)
				{
					foreach (PlanConflict planCon in planEnqInfo.PlanEnqueue.PlanConflictList)
					{
                        // begin MID Track 5116:  Display Text Problem
                        WeekProfile startWP = _calendar.GetWeek(planCon.StartWeek);
                        WeekProfile endWP = _calendar.GetWeek(planCon.EndWeek);
                        // end MID Track 5116:  Display Text Problem

						if (planCon.StartWeek == planCon.EndWeek)
						{
							if (aFormatErrorsForMessageBox)
							{
								message += System.Environment.NewLine;
							}

							message += 
								String.Format(
								MIDText.GetTextOnly(eMIDTextCode.msg_pl_PlanInUseOneWeek),
								(planEnqInfo.PlanType == ePlanType.Chain ? "Chain" : "Store"),
								planEnqInfo.PlanProfile.NodeProfile.Text,
								planEnqInfo.PlanProfile.VersionProfile.Description,
                                FormatAnchorDateDisplay(startWP),
								UserNameStorage.GetUserName(planCon.UserRID) //secAdmin.GetUserName(planCon.UserRID) //TT#827-MD -jsobek -Allocation Reviews Performance
                                );
						}
						else
						{
							if (aFormatErrorsForMessageBox)
							{
								message += System.Environment.NewLine;
							}

							message +=
								String.Format(
								MIDText.GetTextOnly(eMIDTextCode.msg_pl_PlanInUseMultipleWeeks),
								(planEnqInfo.PlanType == ePlanType.Chain ? "Chain" : "Store"),
								planEnqInfo.PlanProfile.NodeProfile.Text,
								planEnqInfo.PlanProfile.VersionProfile.Description,
                                FormatAnchorDateDisplay(startWP),
                                FormatAnchorDateDisplay(endWP),
                                UserNameStorage.GetUserName(planCon.UserRID) //secAdmin.GetUserName(planCon.UserRID) //TT#827-MD -jsobek -Allocation Reviews Performance
                                );
						}
					}
				}

				return message;
			}
			catch (Exception exc)
			{
				message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// The PlanCubeGroup class defines the CubeGroup for a set of Planning cubes.
	/// </summary>
	/// <remarks>
	/// PlanCubeGroup inherits from the base CubeGroup class.  PlanCubeGroup adds the additional functionality of a ComputationSchedule, plus
	/// the ability to retrieve wafers of cube data.
	/// </remarks>

	abstract public class PlanCubeGroup : ComputationCubeGroup
	{
		//=======
		// FIELDS
		//=======

#if (DEBUG)
		protected TimeSpan TotalPageBuildTime;
		protected TimeSpan TotalDBReadAndLoadTime;
		protected TimeSpan TotalValueInitTime;
		protected TimeSpan TotalComparativeInitTime;
		protected TimeSpan TotalValueInitCalcTime;
		protected TimeSpan TotalComparativeInitCalcTime;
#endif

		protected PlanOpenParms _openParms;
		protected VariablesData _varData;
		protected Hashtable _simStoreModelHash;
		private object _planComputationWorkArea;
		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		protected bool _forceCurrentInit;
        //End Enhancement - JScott - Add Balance Low Levels functionality
        //Begin Init Performance Benchmarking -- DO NOT REMOVE
        //public PerfInitHash PerfInitHash = new PerfInitHash();
        //Begin Init Performance Benchmarking -- DO NOT REMOVE
        // Begin TT#2131-MD - JSmith - Halo Integration
        protected ROExtractData _ROExtractData = null;
        protected IPlanComputationVariables _variables = null;
        protected IPlanComputationTimeTotalVariables _totalVariables = null;
        protected DataTable dtExtractControl = null;
        // End TT#2131-MD - JSmith - Halo Integration

        //=============
        // CONSTRUCTORS
        //=============

        /// <summary>
        /// Creates a new instance of PlanCubeGroup that are owned by the given SessionAddressBlock and Transaction.
        /// </summary>
        /// <param name="aSAB">
        /// A reference to the current SessionAddressBlock.
        /// </param>
        /// <param name="aTransaction">
        /// A reference to the current Transaction.
        /// </param>

        public PlanCubeGroup(SessionAddressBlock aSAB, Transaction aTransaction)
			: base(aSAB, aTransaction)
		{
			try
			{
                // Begin TT#5124 - JSmith - Performance
                //_varData = new VariablesData();
                _varData = new VariablesData(aSAB.ApplicationServerSession.GlobalOptions.NumberOfStoreDataTables);
                // End TT#5124 - JSmith - Performance
				_simStoreModelHash = new Hashtable();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the PlanOpenParms object that was used to open this PlanCubeGroup.
		/// </summary>

		public PlanOpenParms OpenParms
		{
			get
			{
				return _openParms;
			}
		}
			
		/// <summary>
		/// Gets the VariablesData object.
		/// </summary>

		public VariablesData VarData
		{
			get
			{
				return _varData;
			}
		}

        // Begin TT#2131-MD - JSmith - Halo Integration
         public bool ROExtractEnabled
        {
            get
            {
                return SAB.ROExtractEnabled;
            }
        }

        
        public ROExtractData ROExtractData
        {
            get
            {
                if (_ROExtractData == null)
                {
                    _ROExtractData = new ROExtractData(SAB.ROExtractConnectionString);
                }

                return _ROExtractData;
            }
        }

        public IPlanComputationVariables Variables
        {
            get
            {
                if (_variables == null)
                {
                    _variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables;
                }

                return _variables;
            }
        }

        public IPlanComputationTimeTotalVariables TotalVariables
        {
            get
            {
                if (_totalVariables == null)
                {
                    _totalVariables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanTimeTotalVariables;
                }

                return _totalVariables;
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration

        /// <summary>
        /// Gets the boolean indicating if the user has made a changed to a cell.
        /// </summary>

        public Hashtable SimilarStoreModelHash
		{
			get
			{
				return _simStoreModelHash;
			}
		}

		/// <summary>
		/// Gets the PlanComputationWorkArea
		/// </summary>

		public object PlanComputationWorkArea
		{
			get
			{
				if (_planComputationWorkArea == null)
				{
					_planComputationWorkArea = Transaction.PlanComputations.CreatePlanComputationWorkArea();
				}

				return _planComputationWorkArea;
			}
		}

		//Begin Enhancement - JScott - Add Balance Low Levels functionality
		/// <summary>
		/// Gets the ForceCurrentInit that indicates whether the Current Initialization should be forced to execute during a cell Init
		/// </summary>

		public bool ForceCurrentInit
		{
			get
			{
				return _forceCurrentInit;
			}
			set
			{
				_forceCurrentInit = value;
			}
		}

		//End Enhancement - JScott - Add Balance Low Levels functionality
		//========
		// METHODS
		//========

#if (DEBUG)
		public void ClearPageBuildTimer()
		{
			TotalPageBuildTime = TimeSpan.Zero;
			TotalValueInitTime = TimeSpan.Zero;
			TotalComparativeInitTime = TimeSpan.Zero;
			TotalValueInitCalcTime = TimeSpan.Zero;
			TotalComparativeInitCalcTime = TimeSpan.Zero;
		}

		public void ClearDBReadAndLoadTimer()
		{
			TotalDBReadAndLoadTime = TimeSpan.Zero;
		}

		public double GetPageBuildTime()
		{
			return TotalPageBuildTime.TotalMilliseconds;
		}

		public double GetDBReadAndLoadTime()
		{
			return TotalDBReadAndLoadTime.TotalMilliseconds;
		}

		public double GetValueInitTime()
		{
			return TotalValueInitTime.TotalMilliseconds;
		}

		public double GetComparativeInitTime()
		{
			return TotalComparativeInitTime.TotalMilliseconds;
		}

		public double GetValueInitCalcTime()
		{
			return TotalValueInitCalcTime.TotalMilliseconds;
		}

		public double GetComparativeInitCalcTime()
		{
			return TotalComparativeInitCalcTime.TotalMilliseconds;
		} 
#endif
        /// <summary>
        /// Creates a ComputationCubeGroupWaferInfo object from a CubeWaferCoordinateList object.
        /// </summary>
        /// <param name="aCoorList">
        /// The list of wafer coordinates to create a ComputationCubeGroupWaferInfo with.
        /// </param>
        /// <returns></returns>

        override protected ComputationCubeGroupWaferInfo CreateWaferInfo(CubeWaferCoordinateList aCoorList)
		{
			PlanCubeGroupWaferInfo waferInfo;

			try
			{
				waferInfo = new PlanCubeGroupWaferInfo();
				waferInfo.ProcessWaferCoordinates(aCoorList);
				return waferInfo;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Private method that determines the eCubeType that is specified by the given ComputationCubeGroupWaferInfo objects.
		/// </summary>
		/// <param name="aGlobalWaferInfo">
		/// The ComputationCubeGroupWaferInfo object that contains the global cube flags that are used to determine the eCubeType.
		/// </param>
		/// <param name="aRowWaferInfo">
		/// The ComputationCubeGroupWaferInfo object that contains the row cube flags that are used to determine the eCubeType.
		/// </param>
		/// <param name="aColWaferInfo">
		/// The ComputationCubeGroupWaferInfo object that contains the col cube flags that are used to determine the eCubeType.
		/// </param>
		/// <returns>
		/// The eCubeType of the cube that is described by the given ComputationCubeGroupWaferInfo objects.
		/// </returns>

		override protected eCubeType DetermineCubeType(ComputationCubeGroupWaferInfo aGlobalWaferInfo, ComputationCubeGroupWaferInfo aRowWaferInfo, ComputationCubeGroupWaferInfo aColWaferInfo)
		{
			try
			{
				PlanCubeGroupWaferCubeFlags cumulatedFlags;

				cumulatedFlags = new PlanCubeGroupWaferCubeFlags();
				cumulatedFlags.CubeFlags = (ushort)(aGlobalWaferInfo.CubeFlagValues | aRowWaferInfo.CubeFlagValues | aColWaferInfo.CubeFlagValues);

				if (cumulatedFlags.isChainPlan)
				{
					if (cumulatedFlags.isBasis)
					{
						if (cumulatedFlags.isLowLevelTotal)
						{
							if (cumulatedFlags.isDate)
							{
								return eCubeType.ChainBasisLowLevelTotalDateTotal;
							}
							else if (cumulatedFlags.isPeriod)
							{
								return eCubeType.ChainBasisLowLevelTotalPeriodDetail;
							}
							else
							{
								return eCubeType.ChainBasisLowLevelTotalWeekDetail;
							}
						}
						else
						{
							if (cumulatedFlags.isDate)
							{
								return eCubeType.ChainBasisDateTotal;
							}
							else if (cumulatedFlags.isPeriod)
							{
								return eCubeType.ChainBasisPeriodDetail;
							}
							else
							{
								return eCubeType.ChainBasisWeekDetail;
							}
						}
					}
					else
					{
						if (cumulatedFlags.isLowLevelTotal)
						{
							if (cumulatedFlags.isDate)
							{
								return eCubeType.ChainPlanLowLevelTotalDateTotal;
							}
							else if (cumulatedFlags.isPeriod)
							{
								return eCubeType.ChainPlanLowLevelTotalPeriodDetail;
							}
							else
							{
								return eCubeType.ChainPlanLowLevelTotalWeekDetail;
							}
						}
						else
						{
							if (cumulatedFlags.isDate)
							{
								return eCubeType.ChainPlanDateTotal;
							}
							else if (cumulatedFlags.isPeriod)
							{
								return eCubeType.ChainPlanPeriodDetail;
							}
							else
							{
								return eCubeType.ChainPlanWeekDetail;
							}
						}
					}
				}
				else if (cumulatedFlags.isStorePlan)
				{
					if (cumulatedFlags.isBasis)
					{
						if (cumulatedFlags.isStore)
						{
							if (cumulatedFlags.isLowLevelTotal)
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StoreBasisLowLevelTotalDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StoreBasisLowLevelTotalPeriodDetail;
								}
								else
								{
									return eCubeType.StoreBasisLowLevelTotalWeekDetail;
								}
							}
							else
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StoreBasisDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StoreBasisPeriodDetail;
								}
								else
								{
									return eCubeType.StoreBasisWeekDetail;
								}
							}
						}
						else if (cumulatedFlags.isStoreGroupLevel)
						{
							if (cumulatedFlags.isLowLevelTotal)
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StoreBasisLowLevelTotalGroupTotalDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StoreBasisLowLevelTotalGroupTotalPeriodDetail;
								}
								else
								{
									return eCubeType.StoreBasisLowLevelTotalGroupTotalWeekDetail;
								}
							}
							else
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StoreBasisGroupTotalDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StoreBasisGroupTotalPeriodDetail;
								}
								else
								{
									return eCubeType.StoreBasisGroupTotalWeekDetail;
								}
							}
						}
						else if (cumulatedFlags.isStoreTotal)
						{
							if (cumulatedFlags.isLowLevelTotal)
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StoreBasisLowLevelTotalStoreTotalDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StoreBasisLowLevelTotalStoreTotalPeriodDetail;
								}
								else
								{
									return eCubeType.StoreBasisLowLevelTotalStoreTotalWeekDetail;
								}
							}
							else
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StoreBasisStoreTotalDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StoreBasisStoreTotalPeriodDetail;
								}
								else
								{
									return eCubeType.StoreBasisStoreTotalWeekDetail;
								}
							}
						}
						else
						{
							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_pl_CubeTypeNotDetermined,
								MIDText.GetText(eMIDTextCode.msg_pl_CubeTypeNotDetermined));
						}
					}
					else
					{
						if (cumulatedFlags.isStore)
						{
							if (cumulatedFlags.isLowLevelTotal)
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StorePlanLowLevelTotalDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StorePlanLowLevelTotalPeriodDetail;
								}
								else
								{
									return eCubeType.StorePlanLowLevelTotalWeekDetail;
								}
							}
							else
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StorePlanDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StorePlanPeriodDetail;
								}
								else
								{
									return eCubeType.StorePlanWeekDetail;
								}
							}
						}
						else if (cumulatedFlags.isStoreGroupLevel)
						{
							if (cumulatedFlags.isLowLevelTotal)
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StorePlanLowLevelTotalGroupTotalDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StorePlanLowLevelTotalGroupTotalPeriodDetail;
								}
								else
								{
									return eCubeType.StorePlanLowLevelTotalGroupTotalWeekDetail;
								}
							}
							else
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StorePlanGroupTotalDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StorePlanGroupTotalPeriodDetail;
								}
								else
								{
									return eCubeType.StorePlanGroupTotalWeekDetail;
								}
							}
						}
						else if (cumulatedFlags.isStoreTotal)
						{
							if (cumulatedFlags.isLowLevelTotal)
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StorePlanLowLevelTotalStoreTotalDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StorePlanLowLevelTotalStoreTotalPeriodDetail;
								}
								else
								{
									return eCubeType.StorePlanLowLevelTotalStoreTotalWeekDetail;
								}
							}
							else
							{
								if (cumulatedFlags.isDate)
								{
									return eCubeType.StorePlanStoreTotalDateTotal;
								}
								else if (cumulatedFlags.isPeriod)
								{
									return eCubeType.StorePlanStoreTotalPeriodDetail;
								}
								else
								{
									return eCubeType.StorePlanStoreTotalWeekDetail;
								}
							}
						}
						else
						{
							throw new MIDException(eErrorLevel.severe,
								(int)eMIDTextCode.msg_pl_CubeTypeNotDetermined,
								MIDText.GetText(eMIDTextCode.msg_pl_CubeTypeNotDetermined));
						}
					}
				}
				else
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_CubeTypeNotDetermined,
						MIDText.GetText(eMIDTextCode.msg_pl_CubeTypeNotDetermined));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method converts a set of common, row, and column CubeWaferCoordinateList objects for a given eCubeType into the corresponding
		/// ComputationCellReference.
		/// </summary>
		/// <param name="aCubeType">
		/// The eCubeType that identifies the ComputationCube this Cell exists in.
		/// </param>
		/// <param name="aCommonWaferList">
		/// The CubeWaferCoordinateList that contains the common CubeWaferCoordinates.
		/// </param>
		/// <param name="aRowWaferList">
		/// The CubeWaferCoordinateList that contains the row CubeWaferCoordinates.
		/// </param>
		/// <param name="aColWaferList">
		/// The CubeWaferCoordinateList that contains the column CubeWaferCoordinates.
		/// </param>
		/// <returns>
		/// The ComputationCellReference identifying the Cell.
		/// </returns>

		override protected ComputationCellReference ConvertCubeWaferInfoToCellReference(
			eCubeType aCubeType,
			CubeWaferCoordinateList aCommonWaferList,
			CubeWaferCoordinateList aRowWaferList,
			CubeWaferCoordinateList aColWaferList,
			ComputationCubeGroupWaferInfo aGlobalWaferInfo,
			ComputationCubeGroupWaferInfo aRowWaferInfo,
			ComputationCubeGroupWaferInfo aColWaferInfo)
		{
			PlanCellReference planCellRef = null;
			int varRID;

			try
			{
				if (aCubeType != eCubeType.None)
				{
					planCellRef = (PlanCellReference)((PlanCube)GetCube(aCubeType)).CreateCellReference();

					varRID = ((PlanCubeGroupWaferInfo)aGlobalWaferInfo).VariableRID;

					if (varRID == Include.NoRID)
					{
						varRID = ((PlanCubeGroupWaferInfo)aRowWaferInfo).VariableRID;
					}

					if (varRID == Include.NoRID)
					{
						varRID = ((PlanCubeGroupWaferInfo)aColWaferInfo).VariableRID;
					}

					foreach (CubeWaferCoordinate waferCoordinate in aCommonWaferList)
					{
						intLoadWaferCoordinates(waferCoordinate, planCellRef, varRID);
					}

					foreach (CubeWaferCoordinate waferCoordinate in aRowWaferList)
					{
						intLoadWaferCoordinates(waferCoordinate, planCellRef, varRID);
					}

					foreach (CubeWaferCoordinate waferCoordinate in aColWaferList)
					{
						intLoadWaferCoordinates(waferCoordinate, planCellRef, varRID);
					}
				}

				return planCellRef;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that creates a new CustomStoreFilter object for the given Filter ID.
		/// </summary>
		/// <param name="aFilterID">
		/// The ID of the CustomStoreFilter to create.
		/// </param>
		/// <returns>
		/// A new CustomStoreFilter object.
		/// </returns>

        //protected override CustomStoreFilter CreateCustomStoreFilter(int aFilterID)
        //{
        //    return new CustomStoreFilter(_SAB, _transaction, _SAB.ApplicationServerSession, this, aFilterID);
        //}

        protected override filter CreateCustomStoreFilter(int filterRID)
        {
            filterDataHelper.SetVariableKeysFromTransaction(_transaction);

            filter f = filterDataHelper.LoadExistingFilter(filterRID);
            f.SetExtraInfoForCubes(_SAB, _transaction, this);
            //return new CustomStoreFilter(_SAB, _transaction, _SAB.ApplicationServerSession, this, aFilterID);
            return f;
        }

		/// <summary>
		/// Executed to open a PlanCubeGroup.
		/// </summary>
		/// <param name="aOpenParms">
		/// The PlanOpenParms object that contains information about the plan open.
		/// </param>

		virtual public void OpenCubeGroup(PlanOpenParms aOpenParms)
		{
			ProfileXRef profXRef;
            // Begin TT#1968 - JSmith - Second set of stores not forecasting
            //ComplexProfileXRef basisTimeTotalTotalXRef;
            //TriProfileXRef triProfXRef;
            // End TT#1968
			ProfileList detailList;

			try
			{
				_openParms = aOpenParms;

				//=============================
				// Set current Computation Mode
				//=============================

				Transaction.CurrentComputationMode = _openParms.ComputationsMode;

				//===========================
				// Build Week Time Total XRef
				//===========================

				profXRef = new ProfileXRef(eProfileType.TimeTotalVariable, eProfileType.Week);
				detailList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);
				intBuildTimeTotalPlanXRefEntries(profXRef, detailList, _SAB.ApplicationServerSession.Calendar.AddWeeks(detailList[detailList.Count - 1].Key, 1));
				SetProfileXRef(profXRef);

				//=============================
				// Build Period Time Total XRef
				//=============================

				if (_openParms.GetDisplayType(SAB.ApplicationServerSession) == ePlanDisplayType.Period)
				{
					profXRef = new ProfileXRef(eProfileType.TimeTotalVariable, eProfileType.Period);
					detailList = _openParms.GetPeriodProfileList(_SAB.ApplicationServerSession);
					intBuildTimeTotalPlanXRefEntries(profXRef, detailList, _SAB.ApplicationServerSession.Calendar.AddPeriods(detailList[detailList.Count - 1].Key, 1));
					SetProfileXRef(profXRef);
				}
            // Begin TT#1968 - JSmith - Second set of stores not forecasting
                BuildBasisXRefs(aOpenParms);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildBasisXRefs(PlanOpenParms aOpenParms)
        {
			ComplexProfileXRef basisTimeTotalTotalXRef;
			TriProfileXRef triProfXRef;
			ProfileList detailList;

			try
			{
            // End TT#1968
				
				if (_openParms.BasisProfileList.Count > 0)
				{
					//=================================
					// Build Basis Week Time Total XRef
					//=================================

					basisTimeTotalTotalXRef = new ComplexProfileXRef(eProfileType.TimeTotalVariable, eProfileType.Basis, eProfileType.Week);

					foreach (BasisProfile basisProf in _openParms.BasisProfileList)
					{
						detailList = basisProf.DisplayablePlanWeekProfileList(SAB.ApplicationServerSession);
						intBuiltTimeTotalBasisXRefEntries(basisTimeTotalTotalXRef, basisProf, detailList, _SAB.ApplicationServerSession.Calendar.AddWeeks(detailList[detailList.Count - 1].Key, 1));
					}

					SetProfileXRef(basisTimeTotalTotalXRef);

					//===================================
					// Build Basis Period Time Total XRef
					//===================================

					if (_openParms.GetDisplayType(SAB.ApplicationServerSession) == ePlanDisplayType.Period)
					{
						basisTimeTotalTotalXRef = new ComplexProfileXRef(eProfileType.TimeTotalVariable, eProfileType.Basis, eProfileType.Period);

						foreach (BasisProfile basisProf in _openParms.BasisProfileList)
						{
							detailList = basisProf.DisplayablePlanPeriodProfileList(SAB.ApplicationServerSession);
							intBuiltTimeTotalBasisXRefEntries(basisTimeTotalTotalXRef, basisProf, detailList, _SAB.ApplicationServerSession.Calendar.AddPeriods(detailList[detailList.Count - 1].Key, 1));
						}

						SetProfileXRef(basisTimeTotalTotalXRef);
					}
				}

				//========================
				// Build Basis Detail XRef
				//========================

				triProfXRef = new TriProfileXRef(eProfileType.HierarchyNode, eProfileType.Basis, 
					eProfileType.BasisDetail, eProfileType.BasisHierarchyNode, eProfileType.BasisVersion);
			
				foreach (BasisProfile basisProf in _openParms.BasisProfileList)
				{
					foreach (BasisDetailProfile basisDetProf in basisProf.BasisDetailProfileList)
					{
						if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel ||
							_openParms.PlanSessionType == ePlanSessionType.StoreMultiLevel)
						{
							triProfXRef.AddXRefIdEntry(_openParms.StoreHLPlanProfile.NodeProfile.Key,
								basisProf.Key, basisDetProf.Key,
								basisDetProf.HierarchyNodeProfile.Key, basisDetProf.VersionProfile.Key);
						}
						//Begin Track #5708 - JScott - I can not bring up the basis for the chain when in a store review
						//else

						if (_openParms.PlanSessionType == ePlanSessionType.ChainSingleLevel ||
							_openParms.PlanSessionType == ePlanSessionType.ChainMultiLevel ||
							_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel)
						//End Track #5708 - JScott - I can not bring up the basis for the chain when in a store review
						{
							triProfXRef.AddXRefIdEntry(_openParms.ChainHLPlanProfile.NodeProfile.Key,
								basisProf.Key, basisDetProf.Key,
								basisDetProf.HierarchyNodeProfile.Key, basisDetProf.VersionProfile.Key);
						}
					}
				}

				if (_openParms.LowLevelPlanProfileList != null &&
					_openParms.LowLevelPlanProfileList.Count > 0)
				{
					foreach (PlanProfile planProfile in _openParms.LowLevelPlanProfileList)
					{
						foreach (BasisProfile basisProf in _openParms.BasisProfileList)
						{
							foreach (BasisDetailProfile basisDetProf in basisProf.BasisDetailProfileList)
							{
								triProfXRef.AddXRefIdEntry(planProfile.NodeProfile.Key,
									basisProf.Key, basisDetProf.Key,
									planProfile.NodeProfile.Key, basisDetProf.VersionProfile.Key);
							}
						}
					}
				}

				SetProfileXRef(triProfXRef);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		/// <summary>
		/// Executed to save a PlanCubeGroup.
		/// </summary>
		/// <param name="aSaveParms">
		/// The PlanSaveParms object that contains information about the plan save.
		/// </param>

		abstract public void SaveCubeGroup(PlanSaveParms aSaveParms);

        // Begin TT#2131-MD - JSmith - Halo Integration
        /// <summary>
        /// Executed to extract a PlanCubeGroup.
        /// </summary>
        /// <param name="aSaveParms">
        /// The PlanSaveParms object that contains information about the plan save.
        /// </param>

        abstract public void ExtractCubeGroup(ExtractOptions aExtractOptions);
        // End TT#2131-MD - JSmith - Halo Integration

        /// <summary>
        /// Closes this PlanCubeGroup.
        /// </summary>

        //Begin Enhancement - JScott - Add Balance Low Levels functionality
        //virtual public void CloseCubeGroup()
        //{
        //}

        override public void CloseCubeGroup()
		{
			try
			{
				base.CloseCubeGroup();

				_simStoreModelHash.Clear();

				_openParms = null;
				_varData = null;
				_simStoreModelHash = null;
				_planComputationWorkArea = null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Method that allows PlanCubeGroups to copy High-level values to Low-level Total values.
		/// </summary>
		virtual public void CopyLowToHigh()
		{
			throw new Exception("Invalid call");
		}

		/// <summary>
		/// Method that allows PlanCubeGroups to copy High-level values to Low-level Total values.
		/// </summary>
		virtual public void BalanceLowLevels()
		{
			throw new Exception("Invalid call");
		}

		//End Enhancement - JScott - Add Balance Low Levels functionality

		/// <summary>
		/// Forecasting for stores may have different basis definitions for any particular Attribute Set.
		/// This refreshes the basis with the set's basis definition.
		/// Note: this was originally just in the ForecastCubeGroup, but now several forecasting methods
		/// need this functionality regardless of the cube group.
		/// </summary>
		/// <param name="aOpenParms"></param>
		public void RefreshStoreBasis(PlanOpenParms aOpenParms)
		{
			// Begin MID Issue 2476
			((StoreBasisWeekDetail)GetCube(eCubeType.StoreBasisWeekDetail)).Clear();
			// End MID Issue 2476
            // Begin TT#1968 - JSmith - Second set of stores not forecasting
            BuildBasisXRefs(aOpenParms);
            // End TT#1968
			((StoreBasisWeekDetail)GetCube(eCubeType.StoreBasisWeekDetail)).ReadAndLoadCube(_openParms.StoreHLPlanProfile, aOpenParms.SimilarStores, 0);
		}

		/// <summary>
		/// Gets a 2-dimensional array of values contained in the requested CubeWafer.
		/// </summary>
		/// <remarks>
		/// GetCubeCellValues allows the user to retrieve arrays of cell values.  This is a much more efficient means of retrieving values,
		/// as it reduces the number of remoted calls required to retrive large sets of data.
		/// </remarks>
		/// <param name="aCubeWafer">
		/// The CubeWafer indicating which cells are to be returned.
		/// </param>
		/// <returns>
		/// A double array of values contained in the Cells requested in the CubeWafer.
		/// </returns>

		//Begin Modification - JScott - Add Scaling Decimals
		//public PlanWaferCell[,] GetPlanWaferCellValues(CubeWafer aCubeWafer, int aUnitScaling, int aDollarScaling)
		// Begin track #6415 - stodd
		public PlanWaferFlagCell[,] GetPlanWaferCellValues(CubeWafer aCubeWafer, string aUnitScaling, string aDollarScaling)
		// End Track #6415
		//End Modification - JScott - Add Scaling Decimals
		{
#if (DEBUG)
			DateTime startTime;
#endif
			int maxRows, maxCols;
			// Begin Track #6415 - stodd
			PlanWaferFlagCell[,] planCellTable;
			// End Track #6415 - stodd
			ComputationCubeGroupWaferInfo[] rowWaferInfo;
			ComputationCubeGroupWaferInfo[] colWaferInfo;
			ComputationCubeGroupWaferInfo globalWaferInfo;
			int i, j;

			try
			{
#if (DEBUG)
				startTime = DateTime.Now;
#endif

				maxRows = System.Math.Max(aCubeWafer.RowWaferCoordinateListGroup.Count, 1);
				maxCols = System.Math.Max(aCubeWafer.ColWaferCoordinateListGroup.Count, 1);

				// Begin Track #6415 - stodd
				planCellTable = new PlanWaferFlagCell[maxRows, maxCols];
				// End Track #6415 - stodd
				if (aCubeWafer.CommonWaferCoordinateList != null)
				{
					rowWaferInfo = new PlanCubeGroupWaferInfo[maxRows];
					colWaferInfo = new PlanCubeGroupWaferInfo[maxCols];

					globalWaferInfo = CreateWaferInfo(aCubeWafer.CommonWaferCoordinateList);

					for (i = 0; i < maxRows; i++)
					{
						rowWaferInfo[i] = CreateWaferInfo(aCubeWafer.RowWaferCoordinateListGroup[i]);
					}

					for (i = 0; i < maxCols; i++)
					{
						colWaferInfo[i] = CreateWaferInfo(aCubeWafer.ColWaferCoordinateListGroup[i]);
					}

					for (i = 0; i < maxRows; i++)
					{
						if (i < aCubeWafer.RowWaferCoordinateListGroup.Count && aCubeWafer.RowWaferCoordinateListGroup[i] != null)
						{
							for (j = 0; j < maxCols; j++)
							{
								if (j < aCubeWafer.ColWaferCoordinateListGroup.Count && aCubeWafer.ColWaferCoordinateListGroup[j] != null)
								{
									planCellTable[i, j] = intGetCellValue(
										aCubeWafer.CommonWaferCoordinateList,
										aCubeWafer.RowWaferCoordinateListGroup[i],
										aCubeWafer.ColWaferCoordinateListGroup[j],
										(PlanCubeGroupWaferInfo)globalWaferInfo,
										(PlanCubeGroupWaferInfo)rowWaferInfo[i],
										(PlanCubeGroupWaferInfo)colWaferInfo[j],
										aUnitScaling,
										aDollarScaling);
								}
							}
						}
					}
				}

#if (DEBUG)
				TotalPageBuildTime = TotalPageBuildTime.Add(DateTime.Now.Subtract(startTime));
#endif

				return planCellTable;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Private method that retrieves the PlanWaferCell specified by the passed parameters.
		/// </summary>
		/// <param name="aGlobalCoordinates">
		/// The CubeWaferCoordinateList object that contains the global coordinate list for the cell.
		/// </param>
		/// <param name="aRowCoordinates">
		/// The CubeWaferCoordinateList object that contains the row coordinate list for the cell.
		/// </param>
		/// <param name="aColCoordinates">
		/// The CubeWaferCoordinateList object that contains the col coordinate list for the cell.
		/// </param>
		/// <param name="aGlobalWaferInfo">
		/// The PlanCubeGroupWaferInfo object that contains the global cube flags that are used to determine the eCubeType.
		/// </param>
		/// <param name="aRowWaferInfo">
		/// The PlanCubeGroupWaferInfo object that contains the row cube flags that are used to determine the eCubeType.
		/// </param>
		/// <param name="aColWaferInfo">
		/// The PlanCubeGroupWaferInfo object that contains the col cube flags that are used to determine the eCubeType.
		/// </param>
		/// <param name="aUnitScaling">
		/// The Unit Scaling factor.
		/// </param>
		/// <param name="aDollarScaling">
		/// The Dollar Scaling factor.
		/// </param>
		/// <returns>
		/// A new PlanWaferCell.
		/// </returns>
		// Begin Track #6415 - stodd
		private PlanWaferFlagCell intGetCellValue(
		// End Track #6415 - stodd
			CubeWaferCoordinateList aGlobalCoordinates,
			CubeWaferCoordinateList aRowCoordinates,
			CubeWaferCoordinateList aColCoordinates,
			PlanCubeGroupWaferInfo aGlobalWaferInfo,
			PlanCubeGroupWaferInfo aRowWaferInfo,
			PlanCubeGroupWaferInfo aColWaferInfo,
			//Begin Modification - JScott - Add Scaling Decimals
			//int aUnitScaling,
			//int aDollarScaling)
			string aUnitScaling,
			string aDollarScaling)
			//End Modification - JScott - Add Scaling Decimals
		{
			PlanWaferFlagCell waferCell;
			PlanCubeGroupWaferValueFlags cumulatedFlags;
			eCubeType cubeType;
			PlanCellReference planCellRef;

			try
			{
				waferCell = null;
				cumulatedFlags = new PlanCubeGroupWaferValueFlags();

				cubeType = DetermineCubeType(aGlobalWaferInfo, aRowWaferInfo, aColWaferInfo);

				planCellRef = (PlanCellReference)ConvertCubeWaferInfoToCellReference(
					cubeType,
					aGlobalCoordinates,
					aRowCoordinates,
					aColCoordinates,
					aGlobalWaferInfo,
					aRowWaferInfo,
					aColWaferInfo);

				if (planCellRef != null && planCellRef.isCellValid)
				{
					cumulatedFlags.ValueFlags = (ushort)(aGlobalWaferInfo.ValueFlagValues | aRowWaferInfo.ValueFlagValues | aColWaferInfo.ValueFlagValues);

					if (cumulatedFlags.isAdjusted)
					{
						if (planCellRef.isCellAdjusted)
						{
							waferCell = new PlanWaferFlagCell(planCellRef, planCellRef.CurrentCellValue, aUnitScaling, aDollarScaling);
						}
						else
						{
							waferCell = new PlanWaferFlagCell(planCellRef);
						}
					}
					else if (cumulatedFlags.isCurrent)
					{
						waferCell = new PlanWaferFlagCell(planCellRef, planCellRef.CurrentCellValue, aUnitScaling, aDollarScaling);
					}
					else if (cumulatedFlags.isPostInit)
					{
						waferCell = new PlanWaferFlagCell(planCellRef, planCellRef.PostInitCellValue, aUnitScaling, aDollarScaling);
					}
				}

				return waferCell;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This private method inspecta a given CubeWaferCoordinate and assigns it's value to the given PlanCellReference, if the PlanCellReference
		/// has a dimension of the type specified in the CubeWaferCoordinate object.
		/// </summary>
		/// <param name="aWaferCoordinate">
		/// The CubeWaferCoordinate object that is to be assigned to the PlanCellReference.
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that is to be modified.
		/// </param>

		private void intLoadWaferCoordinates(CubeWaferCoordinate aWaferCoordinate, PlanCellReference aPlanCellRef, int aVariableRID)
		{
			TimeTotalVariableProfile timeTotVar;

			try
			{
				switch (aWaferCoordinate.WaferCoordinateType)
				{
					case eProfileType.ChainTimeTotalIndex:
						
						if (aVariableRID != Include.NoRID)
						{
							timeTotVar = ((VariableProfile)this.Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableRID)).GetChainTimeTotalVariable((int)aWaferCoordinate.Key);

							if (timeTotVar != null)
							{
								intSetDimensionIdx(
									aPlanCellRef,
									eProfileType.TimeTotalVariable, 
									((TimeTotalVariableProfile)timeTotVar).Key);
							}
							else
							{
								intSetDimensionIdx(aPlanCellRef, eProfileType.TimeTotalVariable, Include.NoRID);
							}
						}
						else
						{
							intSetDimensionIdx(aPlanCellRef, eProfileType.TimeTotalVariable, Include.NoRID);
						}

						break;

					case eProfileType.StoreTimeTotalIndex:
						
						if (aVariableRID != Include.NoRID)
						{
							timeTotVar = ((VariableProfile)this.Transaction.PlanComputations.PlanVariables.VariableProfileList.FindKey(aVariableRID)).GetStoreTimeTotalVariable((int)aWaferCoordinate.Key);

							if (timeTotVar != null)
							{
								intSetDimensionIdx(
									aPlanCellRef,
									eProfileType.TimeTotalVariable, 
									((TimeTotalVariableProfile)timeTotVar).Key);
							}
							else
							{
								intSetDimensionIdx(aPlanCellRef, eProfileType.TimeTotalVariable, Include.NoRID);
							}
						}
						else
						{
							intSetDimensionIdx(aPlanCellRef, eProfileType.TimeTotalVariable, Include.NoRID);
						}

						break;

					case eProfileType.HierarchyNode:
						intSetDimensionIdx(aPlanCellRef, eProfileType.HierarchyNode, (int)aWaferCoordinate.Key);
						break;

					case eProfileType.Version:
						intSetDimensionIdx(aPlanCellRef, eProfileType.Version, (int)aWaferCoordinate.Key);
						break;

					case eProfileType.Basis:
						intSetDimensionIdx(aPlanCellRef, eProfileType.Basis, (int)aWaferCoordinate.Key);
						break;

					case eProfileType.StoreGroupLevel:
						intSetDimensionIdx(aPlanCellRef, eProfileType.StoreGroupLevel, (int)aWaferCoordinate.Key);
						break;

					case eProfileType.Period:
						intSetDimensionIdx(aPlanCellRef, eProfileType.Period, (int)aWaferCoordinate.Key);
						break;

					default:
						intSetDimensionIdx(aPlanCellRef, aWaferCoordinate.WaferCoordinateType, aWaferCoordinate.Key);
						break;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// This method substitutes the given key into the given PlanCellReference in the coordinate specified by the given eProfileType.  If the eProfileType
		/// is not found to be a coordinate in the PlanCellReference, the value is not moved.
		/// </summary>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that is to be modified.
		/// </param>
		/// <param name="aProfileType">
		/// The eProfileType that identifies the coordinate in the PlanCellReference to update.
		/// </param>
		/// <param name="aKey">
		/// The key to be substituted in the PlanCellReference.
		/// </param>

		private void intSetDimensionIdx(PlanCellReference aPlanCellRef, eProfileType aProfileType, int aKey)
		{
			int coordinateIdx;

			try
			{
				coordinateIdx = aPlanCellRef.GetDimensionProfileTypeIndex(aProfileType);
				if (coordinateIdx != -1)
				{
					aPlanCellRef[coordinateIdx] = aKey;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds a ProfileXRef for a Time Total to Week/Period relationship.
		/// </summary>
		/// <param name="aProfXRef">
		/// The ProfileXRef to build.
		/// </param>
		/// <param name="aDetailTimeList">
		/// The list of detail Weeks/Periods.
		/// </param>
		/// <param name="aNextProfileKey">
		/// The "next" Week/Period that follows the opened time line.
		/// </param>

		private void intBuildTimeTotalPlanXRefEntries(ProfileXRef aProfXRef, ProfileList aDetailTimeList, int aNextProfileKey)
		{
			int i;

			try
			{
				foreach (TimeTotalVariableProfile timeTotVarProf in Transaction.PlanComputations.PlanTimeTotalVariables.TimeTotalVariableProfileList)
				{
					switch (timeTotVarProf.VariableTimeTotalType)
					{
						case eVariableTimeTotalType.First :
						case eVariableTimeTotalType.All :
						case eVariableTimeTotalType.AllPlusNext :
						case eVariableTimeTotalType.FirstAndLast :
							aProfXRef.AddXRefIdEntry(timeTotVarProf.Key, aDetailTimeList[0].Key);
							break;
					}

					if (aDetailTimeList.Count > 1)
					{
						switch (timeTotVarProf.VariableTimeTotalType)
						{
							case eVariableTimeTotalType.All :
							case eVariableTimeTotalType.AllPlusNext :
								for (i = 1; i < aDetailTimeList.Count - 1; i++)
								{
									aProfXRef.AddXRefIdEntry(timeTotVarProf.Key, aDetailTimeList[i].Key);
								}
								break;
						}

						switch (timeTotVarProf.VariableTimeTotalType)
						{
							case eVariableTimeTotalType.All :
							case eVariableTimeTotalType.AllPlusNext :
							case eVariableTimeTotalType.Last :
							case eVariableTimeTotalType.FirstAndLast :
								aProfXRef.AddXRefIdEntry(timeTotVarProf.Key, aDetailTimeList[aDetailTimeList.Count - 1].Key);
								break;
						}
					}

					switch (timeTotVarProf.VariableTimeTotalType)
					{
						case eVariableTimeTotalType.AllPlusNext :
						case eVariableTimeTotalType.Next :
							aProfXRef.AddXRefIdEntry(timeTotVarProf.Key, aNextProfileKey);
							break;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Builds a ProfileXRef for a Time Total to Week/Period relationship for a given Basis.
		/// </summary>
		/// <param name="aProfXRef">
		/// The ProfileXRef to build.
		/// </param>
		/// <param name="aBasisProf">
		/// The BasisProfile of the basis to build.
		/// </param>
		/// <param name="aDetailTimeList">
		/// The list of detail Weeks/Periods.
		/// </param>
		/// <param name="aNextProfileKey">
		/// The "next" Week/Period that follows the opened time line.
		/// </param>

		private void intBuiltTimeTotalBasisXRefEntries(ComplexProfileXRef aProfXRef, BasisProfile aBasisProf, ProfileList aDetailTimeList, int aNextProfileKey)
		{
			int i;

			try
			{
				foreach (TimeTotalVariableProfile timeTotVarProf in Transaction.PlanComputations.PlanTimeTotalVariables.TimeTotalVariableProfileList)
				{
					switch (timeTotVarProf.VariableTimeTotalType)
					{
						case eVariableTimeTotalType.First :
						case eVariableTimeTotalType.All :
						case eVariableTimeTotalType.AllPlusNext :
						case eVariableTimeTotalType.FirstAndLast :
							aProfXRef.AddXRefIdEntry(timeTotVarProf.Key, aBasisProf.Key, aDetailTimeList[0].Key);
							break;
					}

					if (aDetailTimeList.Count > 1)
					{
						switch (timeTotVarProf.VariableTimeTotalType)
						{
							case eVariableTimeTotalType.All :
							case eVariableTimeTotalType.AllPlusNext :
								for (i = 1; i < aDetailTimeList.Count - 1; i++)
								{
									aProfXRef.AddXRefIdEntry(timeTotVarProf.Key, aBasisProf.Key, aDetailTimeList[i].Key);
								}
								break;
						}

						switch (timeTotVarProf.VariableTimeTotalType)
						{
							case eVariableTimeTotalType.All :
							case eVariableTimeTotalType.AllPlusNext :
							case eVariableTimeTotalType.Last :
							case eVariableTimeTotalType.FirstAndLast :
								aProfXRef.AddXRefIdEntry(timeTotVarProf.Key, aBasisProf.Key, aDetailTimeList[aDetailTimeList.Count - 1].Key);
								break;
						}
					}

					switch (timeTotVarProf.VariableTimeTotalType)
					{
						case eVariableTimeTotalType.AllPlusNext :
						case eVariableTimeTotalType.Next :
							aProfXRef.AddXRefIdEntry(timeTotVarProf.Key, aBasisProf.Key, aNextProfileKey);
							break;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a boolean value indicating if the variable is stored on the database.
		/// </summary>
		/// <param name="aVarProf">
		/// The VariableProfile containing the variable to inspect
		/// </param>
		/// <param name="aPlanCellRef">
		/// The PlanCellReference that points to the Cell to inspect.
		/// </param>
		/// <returns>
		/// A boolean indicating if the variable is stored on the database.
		/// </returns>

		abstract public bool isDatabaseVariable(VariableProfile aVarProf, PlanCellReference aPlanCellRef);

        // Begin TT#5276 - JSmith - Read Only Saves
        abstract public bool IsNodeEnqueued(int aNodeKey, bool aIsChain);
        // End TT#5276 - JSmith - Read Only Saves

        // Begin TT#2131-MD - JSmith - Halo Integration
        public List<DateProfile> BuildPeriods(
            bool includeYear = false,
            bool includeSeason = false,
            bool includeQuarter = false,
            bool includeMonth = false,
            bool includeWeeks = false,
            bool includeAllWeeks = true,
            bool isForExtract = true,
            int HN_RID = Include.NoRID,
            int FV_RID = Include.NoRID,
            ePlanType planType = ePlanType.None
            )
        {
            int i = 0;
            ProfileList periodProfileList = SAB.ApplicationServerSession.Calendar.GetPeriodProfileList(_openParms.DateRangeProfile.Key);
            ProfileList weeksProfileList = _openParms.GetWeekProfileList(SAB.ApplicationServerSession);
            List<DateProfile> periods = new List<DateProfile>();
            List<int> periodHeader = new List<int>();
            if (includeYear)
            {
                periodHeader.Add((int)eProfileType.Year);
            }
            if (includeSeason)
            {
                periodHeader.Add((int)eProfileType.Season);
            }
            if (includeQuarter)
            {
                periodHeader.Add((int)eProfileType.Quarter);
            }
            if (includeMonth)
            {
                periodHeader.Add((int)eProfileType.Month);
            }
            if (includeWeeks)
            {
                periodHeader.Add((int)eProfileType.Week);
            }

            if (ROExtractEnabled
                && isForExtract
                && !includeAllWeeks)
            {
                ProfileList changedWweeksProfileList = GetChangedWeeks(weeksProfileList, HN_RID, FV_RID, planType);
                // if any week changed, include all weeks otherwise set the week list to the empty changed week list
                if (changedWweeksProfileList.Count == 0)
                {
                    weeksProfileList = changedWweeksProfileList;
                    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "At least one week has changed, all weeks will be used.", this.GetType().Name);
                }
            }

            BuildPeriods(periodProfileList, weeksProfileList, ref i, periods, periodHeader, includeAllWeeks);

            return periods;
        }

        private ProfileList GetChangedWeeks (
            ProfileList weeksProfileList,
            int HN_RID,
            int FV_RID,
            ePlanType planType
            )
        {

            ProfileList changedWeeks = new ProfileList(eProfileType.Week);

            if (dtExtractControl == null)
            {
                dtExtractControl = VarData.EXTRACT_PLANNING_CONTROL_Read(HN_RID: HN_RID, FV_RID: FV_RID, PLAN_TYPE: planType);
            }

            bool includeWeek = true;
            DateTime extractDate, updateDate;
            foreach (WeekProfile weekProf in weeksProfileList)
            {
                includeWeek = true;
                DataRow[] rows = dtExtractControl.Select("TIME_ID = " + weekProf.Key.ToString(CultureInfo.CurrentUICulture));
                if (rows.Length > 0)
                {
                    if (rows[0]["EXTRACT_DATE"] != System.DBNull.Value)
                    {
                        extractDate = Convert.ToDateTime(rows[0]["EXTRACT_DATE"]);
                        if (rows[0]["UPDATE_DATE"] != System.DBNull.Value)
                        {
                            updateDate = Convert.ToDateTime(rows[0]["UPDATE_DATE"]);
                            if (extractDate > updateDate)
                            {
                                includeWeek = false;
                            }
                        }
                        else
                        {
                            includeWeek = false;
                        }
                    }
                }

                if (includeWeek)
                {
                    changedWeeks.Add(weekProf);
                }
            }

            return changedWeeks;
        }

        private void BuildPeriods(ProfileList aPeriodList, ProfileList aWeekList, ref int aSeq, List<DateProfile> periods, List<int> periodHeader, bool includeAllWeeks)
        {
            try
            {
                if (aPeriodList.ProfileType == eProfileType.Period)
                {
                    foreach (PeriodProfile perProf in aPeriodList)
                    {
                        if (periodHeader.Contains((int)perProf.PeriodProfileType))
                        {
                            // restrict periods based on weeks
                            bool includePeriod = true;
                            if (!includeAllWeeks)
                            {
                                includePeriod = false;
                                foreach (WeekProfile weekProf in aWeekList.ArrayList)
                                {
                                    if (perProf.Weeks.Contains(weekProf.Key))
                                    {
                                        includePeriod = true;
                                        break;
                                    }
                                }
                            }
                            if (includePeriod)
                            {
                                periods.Add(perProf);
                            }
                        }

                        if (perProf.ChildPeriodList.Count > 0)
                        {
                            BuildPeriods(perProf.ChildPeriodList, aWeekList, ref aSeq, periods, periodHeader, includeAllWeeks);
                        }
                        else
                        {
                            BuildPeriods(perProf.Weeks, aWeekList, ref aSeq, periods, periodHeader, includeAllWeeks);
                        }
                    }
                }
                else
                {
                    if (periodHeader.Contains((int)aPeriodList.ProfileType))
                    {
                        foreach (WeekProfile weekProf in aPeriodList)
                        {
                            if (aWeekList.Contains(weekProf.Key))
                            {
                                periods.Add(weekProf);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration
    }
}
