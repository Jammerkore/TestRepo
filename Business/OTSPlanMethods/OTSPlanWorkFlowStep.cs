using System;
using System.Collections;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;


namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for OTSPlanWorkFlowStep.
	/// </summary>
	public class OTSPlanWorkFlowStep:ApplicationWorkFlowStep
	{
		//=======
		// FIELDS
		//=======
		private bool					_review;
		private bool					_usedSystemTolerancePercent;
		private double					_tolerancePercent;
		private int						_variableNumber;
		private string					_computationMode;
		private bool					_balance;

		public OTSPlanWorkFlowStep(
			ApplicationBaseAction aMethod,
			bool aReviewFlag, 
			bool aUsedSystemTolerancePercent,
			double aTolerancePercent, 
			int aStoreFilter, int aWorkFlowStepKey, int aVariableNumber, string aComputationMode, bool aBalance)
			: base(aMethod, aStoreFilter, aWorkFlowStepKey)
		{
			_review = aReviewFlag;
			if (aTolerancePercent < 0)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_TolerancePctCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_TolerancePctCannotBeNeg));
			}
			_usedSystemTolerancePercent = aUsedSystemTolerancePercent;
			_tolerancePercent = aTolerancePercent;
			_variableNumber = aVariableNumber;
			_computationMode = aComputationMode;
			_balance = aBalance;
		}
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets the ProfileType of the Profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.OTSPlanWorkFlowStep;
			}
		}

		/// <summary>
		/// Gets or sets the Review Flag value.
		/// </summary>
		/// <remarks>
		/// True indicates to stop processing when processing of this method is complete.
		/// False indicates to continue processing when processing of this method is complete
		/// </remarks>
		public bool Review
		{
			get 
			{
				return _review;
			}
			set
			{
				_review = value;
			}
		}

		/// <summary>
		/// Gets or sets the Used System Tolerance Percent Flag value.
		/// </summary>
		/// <remarks>
		/// True indicates the value came from systems options.
		/// False indicates the value was defined to the step.
		/// </remarks>
		public bool UsedSystemTolerancePercent
		{
			get 
			{
				return _usedSystemTolerancePercent;
			}
			set
			{
				_usedSystemTolerancePercent = value;
			}
		}

		/// <summary>
		/// Gets or sets Tolerance Percent for balancing purposes.
		/// </summary>
		public double TolerancePercent
		{
			get
			{
				return _tolerancePercent;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_TolerancePctCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_TolerancePctCannotBeNeg));
				}
				_tolerancePercent = value;
			}
		}

		/// <summary>
		/// Gets or sets the variable number to be used.
		/// </summary>
		public int VariableNumber
		{
			get 
			{
				return _variableNumber;
			}
			set
			{
				_variableNumber = value;
			}
		}

		/// <summary>
		/// Gets or sets the computation mode value.
		/// </summary>
		public string ComputationMode
		{
			get 
			{
				return _computationMode;
			}
			set
			{
				_computationMode = value;
			}
		}

		/// <summary>
		/// Gets or sets the Balance Flag value.
		/// </summary>
		public bool Balance
		{
			get 
			{
				return _balance;
			}
			set
			{
				_balance = value;
			}
		}
		
		/// <summary>
		/// Verifies that the specified method is a valid method for this workflow.
		/// </summary>
		/// <param name="aMethod">Method to be checked</param>
		override public void CheckMethod(ApplicationBaseAction aMethod)
		{
			if (!Enum.IsDefined(typeof(eForecastMethodType), (eForecastMethodType)aMethod.MethodType))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_UnknownMethodInWorkFlow,
					MIDText.GetText(eMIDTextCode.msg_UnknownMethodInWorkFlow));
			}
		}

		public OTSPlanWorkFlowStep Copy()
		{
			try
			{
				OTSPlanWorkFlowStep ows = (OTSPlanWorkFlowStep)this.MemberwiseClone();
				ows.Key = Key;
				ows.Method = Method;
				ows.Review = Review;
				ows.UsedSystemTolerancePercent = UsedSystemTolerancePercent;
				ows.TolerancePercent = TolerancePercent;
				ows.StoreFilterRID = StoreFilterRID;
				ows.VariableNumber = VariableNumber;
				ows.ComputationMode = ComputationMode;
				ows.Balance = Balance;
				return ows;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
