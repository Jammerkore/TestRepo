using System; 
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Summary description for SizeCurveMethod.
	/// </summary>  
	public class SizeCurveMethod : AllocationSizeBaseMethod
	{

		#region "Member Variables"
		private MethodSizeCurveData _methodData;
		private SizeCurve _sizeCurveData;
        //private StoreVariableHistoryBin _dlStoreVarHist; //TT#739-MD -jsobek -Delete Stores
		private SessionAddressBlock _aSAB;
        //private eSizeCurveGenerateType _sizeCurveGenType; //TT#739-MD -jsobek -Delete Stores
        //private HierarchyNodeProfile _nodeProfile; //TT#739-MD -jsobek -Delete Stores
		//Begin TT#1076 - JScott - Size Curves by Set
		private eSizeCurvesByType _sizeCurvesByType;
		private int _sizeCurvesBySGRID;
		//End TT#1076 - JScott - Size Curves by Set
		private SizeCurveSimilarStoreList _nodeSimStoreList;
		private bool _merchBasisEqualizeWeight;
		private bool _curveBasisEqualizeWeight;
		private bool _applyLostSales;
		private double _tolerMinAvgPerSize;
		private eLowLevelsType _tolerHighLvlLevelType;
		private int _tolerHighLvlHierarchyRID;
		private int _tolerHighLvlLevelRID;
		private int _tolerHighLvlOffset;
		private double _tolerSalesTolerance;
		private eNodeChainSalesType _tolerIndexUnitsType;
        private double _tolerMinTolerancePct;
        private double _tolerMaxTolerancePct;
		private DataTable _dtMerchBasisDetail;
		private DataTable _dtCurveBasisDetail;
		private SizeCurveBasisProfileList _merchBasisProfileList;
		private SizeCurveBasisProfileList _curveBasisProfileList;
		private SizeCurveSpread _spreader;
		private Hashtable _storeHash;
		private SizeGroupProfile _szGrpProf;
		//private bool _useStoredProcedure;  //TT#739-MD -jsobek -Delete Stores
		private string _lblSizeCurve;
		
		//Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        private bool _applyMinToZeroTolerance;
		//End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
		
		// counts, indices and other work fields used to allocate packs by need
        private ApplicationSessionTransaction _transaction;
        private bool _processCancelled = false;

		#endregion

		#region "Properties"

		/// <summary>
		/// Get Profile Type of this profile.
		/// </summary>
		public override eProfileType ProfileType
		{
			get	{return eProfileType.MethodSizeCurve;}
		}

		//Begin TT#1076 - JScott - Size Curves by Set
		public eSizeCurvesByType SizeCurvesByType
		{
			get { return _sizeCurvesByType; }
			set { _sizeCurvesByType = value; }
		}

		public int SizeCurvesBySGRID
		{
			get { return _sizeCurvesBySGRID; }
			set { _sizeCurvesBySGRID = value; }
		}

		//End TT#1076 - JScott - Size Curves by Set
		public bool MerchBasisEqualizeWeight
		{
			get { return _merchBasisEqualizeWeight; }
			set { _merchBasisEqualizeWeight = value; }
		}

		public bool CurveBasisEqualizeWeight
		{
			get { return _curveBasisEqualizeWeight; }
			set { _curveBasisEqualizeWeight = value; }
		}

		public bool ApplyLostSales
		{
			get { return _applyLostSales; }
			set { _applyLostSales = value; }
		}

		public double TolerMinAvgPerSize
		{
			get { return _tolerMinAvgPerSize; }
			set { _tolerMinAvgPerSize = value; }
		}

        public double TolerSalesTolerance
		{
			get { return _tolerSalesTolerance; }
			set { _tolerSalesTolerance = value; }
		}

		public eNodeChainSalesType TolerIndexUnitsType
		{
			get { return _tolerIndexUnitsType; }
			set { _tolerIndexUnitsType = value; }
		}

        public double TolerMinTolerancePct
		{
			get { return _tolerMinTolerancePct; }
			set { _tolerMinTolerancePct = value; }
		}

        public double TolerMaxTolerancePct
		{
			get { return _tolerMaxTolerancePct; }
			set { _tolerMaxTolerancePct = value; }
		}

		public DataTable DTMerchBasisDetail
		{
			get { return _dtMerchBasisDetail; }
			set { _dtMerchBasisDetail = value; }
		}

		public DataTable DTCurveBasisDetail
		{
			get { return _dtCurveBasisDetail; }
			set { _dtCurveBasisDetail = value; }
		}

        public SizeCurveBasisProfileList MerchBasisProfileList
        {
			get { return _merchBasisProfileList; }
			set { _merchBasisProfileList = value; }
        }

		public SizeCurveBasisProfileList CurveBasisProfileList
		{
			get { return _curveBasisProfileList; }
			set { _curveBasisProfileList = value; }
		}

		//Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        public bool ApplyMinToZeroTolerance
        {
            get { return _applyMinToZeroTolerance; }
            set { _applyMinToZeroTolerance = value; }
        }
		//End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
		
		#endregion

		#region "Constructors"
		/// <summary>
		/// Creates an instance of Size Need Method
		/// </summary>
		/// <param name="aSAB">Session Address Block</param>
		/// <param name="aMethodRID">RID for the Method.</param>
		public SizeCurveMethod(SessionAddressBlock aSAB, int aMethodRID): base (aSAB, aMethodRID, eMethodType.SizeCurve, eProfileType.MethodSizeCurve)
		{
			Load(aSAB, aMethodRID);
		}

		/// <summary>
		/// Creates an instance of Size Need Method for a specific allocation Profile.
		/// Used by allocation when requesting plan and need units for onHand and other calculations. 
		/// </summary>
		/// <param name="aSAB"></param>
		/// <param name="aMethodRID"></param>
		/// <param name="aProfile"></param>
		public SizeCurveMethod(SessionAddressBlock aSAB, int aMethodRID, AllocationProfile aProfile, ApplicationSessionTransaction aApplicationTransaction)
			: base(aSAB, aMethodRID, eMethodType.SizeCurve, eProfileType.MethodSizeCurve)
		{
			_transaction = aApplicationTransaction;
			Load(aSAB, aMethodRID);
		}

		/// <summary>
		/// Creates an instance of Size Need Method
		/// </summary>
		/// <param name="aSAB">Session Address Block</param>
		/// <param name="aName">The Name of the Size Curve Method.</param>
		/// <param name="aSizeGroupRID">The SizeGroupRID for the Size Curve Method.</param>
		//Begin TT#1076 - JScott - Size Curves by Set
		/// <param name="aSizeCurvesByType">The SizeCurvesByType for the Size Curve Method.</param>
		/// <param name="aSizeCurvesBySGRID">The SizeCurvesBySGRID for the Size Curve Method.</param>
		//End TT#1076 - JScott - Size Curves by Set
		/// <param name="aSizeCurveSimStoreList">The SizeCurveSimilarStoreList the Size Curve Method.</param>
		/// <param name="aApplyLostSales">A boolean indicating if Lost Sales should be applied to the Size Curve Method.</param>
		/// <param name="aTolerMinAvgPerSize">The TolerMinAvgPerSize for the Size Curve Method.</param>
		/// <param name="aTolerHighLvlLevelType">The TolerHighLvlLevelType for the Size Curve Method.</param>
		/// <param name="aTolerHighLvlHierarchyRID">The TolerHighLvlHierarchyRID for the Size Curve Method.</param>
		/// <param name="aTolerHighLvlLevelRID">The TolerHighLvlLevelRID for the Size Curve Method.</param>
		/// <param name="aTolerHighLvlOffset">The TolerHighLvlOffset for the Size Curve Method.</param>
		/// <param name="aTolerSalesTolerance">The TolerSalesTolerance for the Size Curve Method.</param>
		/// <param name="aTolerIndexUnitsType">The TolerIndexUnitsType for the Size Curve Method.</param>
		/// <param name="aTolerMinTolerancePct">The TolerMinTolerancePct for the Size Curve Method.</param>
		/// <param name="aTolerMaxTolerancePct">The TolerMaxTolerancePct for the Size Curve Method.</param>
		/// <param name="aDtMerchBasisDetail">The MerchBasisDetail datatable for the Size Curve Method.</param>
		/// <param name="aDtCurveBasisDetail">The CurveBasisDetail datatable for the Size Curve Method.</param>
		public SizeCurveMethod(
			SessionAddressBlock aSAB,
			string aName,
			int aSizeGroupRID,
			//Begin TT#1076 - JScott - Size Curves by Set
			eSizeCurvesByType aSizeCurvesByType,
			int aSizeCurvesBySGRID,
			//End TT#1076 - JScott - Size Curves by Set
			SizeCurveSimilarStoreList aSizeCurveSimStoreList,
			bool aApplyLostSales,
			double aTolerMinAvgPerSize,
			eLowLevelsType aTolerHighLvlLevelType,
			int aTolerHighLvlHierarchyRID,
			int aTolerHighLvlLevelRID,
			int aTolerHighLvlOffset,
			double aTolerSalesTolerance,
			eNodeChainSalesType aTolerIndexUnitsType,
			double aTolerMinTolerancePct,
            //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
            bool aApplyMinToZeroTolerance,
		    //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
			double aTolerMaxTolerancePct,
			DataTable aDtMerchBasisDetail,
			DataTable aDtCurveBasisDetail)
			: base(aSAB, Include.NoRID, eMethodType.SizeCurve, eProfileType.MethodSizeCurve)
		{
			_aSAB = aSAB;
            //_sizeCurveGenType = eSizeCurveGenerateType.Method; //TT#739-MD -jsobek -Delete Stores

			if (base.MethodType != eMethodType.SizeCurve)
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_NotSizeCurveMethod),
					MIDText.GetText(eMIDTextCode.msg_NotSizeCurveMethod));
			}

			_methodData = new MethodSizeCurveData();
			Name = aName;
			SizeGroupRid = aSizeGroupRID;
            //_nodeProfile = null; //TT#739-MD -jsobek -Delete Stores
			//Begin TT#1076 - JScott - Size Curves by Set
			_sizeCurvesByType = aSizeCurvesByType;
			_sizeCurvesBySGRID = aSizeCurvesBySGRID;
			//End TT#1076 - JScott - Size Curves by Set
			_nodeSimStoreList = aSizeCurveSimStoreList;
			_merchBasisEqualizeWeight = false;
			_curveBasisEqualizeWeight = false;
			_applyLostSales = aApplyLostSales;
			_tolerMinAvgPerSize = aTolerMinAvgPerSize;
			_tolerHighLvlLevelType = aTolerHighLvlLevelType;
			_tolerHighLvlHierarchyRID = aTolerHighLvlHierarchyRID;
			_tolerHighLvlLevelRID = aTolerHighLvlLevelRID;
			_tolerHighLvlOffset = aTolerHighLvlOffset;
			_tolerSalesTolerance = aTolerSalesTolerance;
			_tolerIndexUnitsType = aTolerIndexUnitsType;
			_tolerMinTolerancePct = aTolerMinTolerancePct;
            //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
            _applyMinToZeroTolerance = aApplyMinToZeroTolerance;
            //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
			_tolerMaxTolerancePct = aTolerMaxTolerancePct;
			_dtMerchBasisDetail = aDtMerchBasisDetail;
			_dtCurveBasisDetail = aDtCurveBasisDetail;


			Common_Load();
		}

		private void Load(SessionAddressBlock aSAB, int aMethodRID)
		{
			_aSAB = aSAB;
            //_sizeCurveGenType = eSizeCurveGenerateType.Method; //TT#739-MD -jsobek -Delete Stores

			if (base.MethodType != eMethodType.SizeCurve)
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_NotSizeCurveMethod),
                    MIDText.GetText(eMIDTextCode.msg_NotSizeCurveMethod));
			}

			if (base.Filled)
			{
				#region METHOD VALUES
				_methodData = new MethodSizeCurveData(base.Key,eChangeType.populate);
                SizeGroupRid = _methodData.SizeGroupRID;
                //_nodeProfile = null; //TT#739-MD -jsobek -Delete Stores
				//Begin TT#1076 - JScott - Size Curves by Set
				_sizeCurvesByType = _methodData.SizeCurvesByType;
				_sizeCurvesBySGRID = _methodData.SizeCurvesBySGRID;
				//End TT#1076 - JScott - Size Curves by Set
				_nodeSimStoreList = null;
				_merchBasisEqualizeWeight = _methodData.MerchBasisEqualizeWeight;
				_curveBasisEqualizeWeight = _methodData.CurveBasisEqualizeWeight;
                _applyLostSales = _methodData.ApplyLostSales;
				_tolerMinAvgPerSize = _methodData.TolerMinAvgPerSize;
				_tolerHighLvlLevelType = eLowLevelsType.None;
				_tolerHighLvlHierarchyRID = Include.NoRID;
				_tolerHighLvlLevelRID = Include.NoRID;
				_tolerHighLvlOffset = Include.Undefined;
				_tolerSalesTolerance = _methodData.TolerSalesTolerance;
                _tolerIndexUnitsType = _methodData.TolerIndexUnitsType;
                _tolerMinTolerancePct = _methodData.TolerMinTolerancePct;
                //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                _applyMinToZeroTolerance = _methodData.ApplyMinToZeroTolerance;
                //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                _tolerMaxTolerancePct = _methodData.TolerMaxTolerancePct;
				_dtMerchBasisDetail = _methodData.DTMerchBasisDetail;
				_dtCurveBasisDetail = _methodData.DTCurveBasisDetail;

                

				#endregion
			}
			else
			{
				#region DEFAULT VALUES
				_methodData = new MethodSizeCurveData();
				SizeGroupRid = Include.NoRID;
                //_nodeProfile = null; //TT#739-MD -jsobek -Delete Stores
				//Begin TT#1076 - JScott - Size Curves by Set
				_sizeCurvesByType = eSizeCurvesByType.Store;
				_sizeCurvesBySGRID = Include.NoRID;
				//End TT#1076 - JScott - Size Curves by Set
				_nodeSimStoreList = null;
				_merchBasisEqualizeWeight = false;
				_curveBasisEqualizeWeight = false;
				_applyLostSales = false;
				_tolerMinAvgPerSize = Include.Undefined;
				_tolerHighLvlLevelType = eLowLevelsType.None;
				_tolerHighLvlHierarchyRID = Include.NoRID;
				_tolerHighLvlLevelRID = Include.NoRID;
				_tolerHighLvlOffset = Include.Undefined;
				_tolerSalesTolerance = Include.Undefined;
				_tolerIndexUnitsType = eNodeChainSalesType.None;
				_tolerMinTolerancePct = Include.Undefined;
				_tolerMaxTolerancePct = Include.Undefined;
				_dtMerchBasisDetail = _methodData.GetMerchBasisData(Include.NoRID);
				_dtCurveBasisDetail = _methodData.GetCurveBasisData(Include.NoRID);
				#endregion
			}

			Common_Load();
		}

		private void Common_Load()
		{
			_sizeCurveData = new SizeCurve();

			SMBD = _methodData;

			SetMerchBasisProfileList();
			SetCurveBasisProfileList();

			_spreader = new SizeCurveSpread();
			_storeHash = new Hashtable();
			_lblSizeCurve = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeCurve);
		}

		private void SetMerchBasisProfileList()
        {
			_merchBasisProfileList = new SizeCurveBasisProfileList(eProfileType.SizeCurveMerchBasis);
			foreach (DataRow dr in _dtMerchBasisDetail.Rows)
            {
                int seq = Convert.ToInt32(dr["BASIS_SEQ"]);
                SizeCurveMerchBasisProfile scbp = new SizeCurveMerchBasisProfile(seq);

                if (dr["HN_RID"] == DBNull.Value)
                {
                    scbp.Basis_HN_RID = Include.NoRID;
                }
                else
                {
                    scbp.Basis_HN_RID = Convert.ToInt32(dr["HN_RID"]);
                }
				scbp.Basis_FV_RID = Convert.ToInt32(dr["FV_RID"]);
                scbp.Basis_CDR_RID = Convert.ToInt32(dr["CDR_RID"]);
				scbp.Basis_Weight = Convert.ToDecimal(dr["WEIGHT"]);
				scbp.MerchType = (eMerchandiseType)Convert.ToInt32(dr["MERCH_TYPE"]);
				scbp.OverrideLowLevelRid = Convert.ToInt32(dr["OLL_RID"]);
				scbp.CustomOverrideLowLevelRid = Convert.ToInt32(dr["CUSTOM_OLL_RID"]);

				_merchBasisProfileList.Add(scbp);
            }
        }

		private void SetCurveBasisProfileList()
		{
			_curveBasisProfileList = new SizeCurveBasisProfileList(eProfileType.SizeCurveCurveBasis);
			foreach (DataRow dr in _dtCurveBasisDetail.Rows)
			{
				int seq = Convert.ToInt32(dr["BASIS_SEQ"]);
				SizeCurveCurveBasisProfile scbp = new SizeCurveCurveBasisProfile(seq);

				scbp.Basis_SizeCurveGroupRID = Convert.ToInt32(dr["SIZE_CURVE_GROUP_RID"]);
				scbp.Basis_Weight = Convert.ToDecimal(dr["WEIGHT"]);

				_curveBasisProfileList.Add(scbp);
			}
		}

		#endregion

		#region "Override Methods"

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (CheckSizeMethodForUserData())
            {
                return true;
            }

            if (IsStoreGroupUser(_sizeCurvesBySGRID))
            {
                return true;
            }

            foreach (DataRow dr in _dtMerchBasisDetail.Rows)
            {
                if (dr["HN_RID"] != DBNull.Value
                    && IsHierarchyNodeUser(Convert.ToInt32(dr["HN_RID"])))
                {
                    return true;
                }
            }

            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		public override void ProcessMethod(
			ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile)
		{
			try
			{
				aApplicationTransaction.ResetAllocationActionStatus();

				this.ProcessAction(
					aApplicationTransaction.SAB,
					aApplicationTransaction,
					null,
					null,
					true,
					Include.NoRID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// processes a single action/work flow step
		/// </summary>
		/// <param name="aSAB"></param>
		/// <param name="aApplicationTransaction"></param>
		/// <param name="aApplicationWorkFlowStep"></param>
		/// <param name="aAllocationProfile"></param>
		/// <param name="WriteToDB"></param>
		/// <param name="aStoreFilterRID"></param>
		public override void ProcessAction(
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aApplicationWorkFlowStep, 
			Profile aAllocationProfile,
			bool WriteToDB,
			int aStoreFilterRID)
		{
			int apKey;
			string msg;
			bool actionPerformed;

			try
			{
				if (aAllocationProfile != null)
				{
					apKey = aAllocationProfile.Key;
				}
				else
				{
					apKey = 0;
				}

				try
				{
                    //Begin TT#739-MD -jsobek -Delete Stores
					//_useStoredProcedure = (Convert.ToString(MIDConfigurationManager.AppSettings["UseSizeStoreProcedureRead"]) == "True");

					//if (!_useStoredProcedure)
					//{
					//	_dlStoreVarHist = new StoreVariableHistoryBin();
					//}
                    //End TT#739-MD -jsobek -Delete Stores
                    // Begin TT#1966-MD - JSmith- DC Fulfillment
                    // Begin TT#5663 - JSmith - Null Reference Error
                    //if (((AllocationProfile)aAllocationProfile).IsMasterHeader && ((AllocationProfile)aAllocationProfile).DCFulfillmentProcessed)
                    if (aAllocationProfile != null && (((AllocationProfile)aAllocationProfile).IsMasterHeader && ((AllocationProfile)aAllocationProfile).DCFulfillmentProcessed))
                    // End TT#5663 - JSmith - Null Reference Error
                    {
                        string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ((AllocationProfile)aAllocationProfile).HeaderID);
                        SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Warning,
                            eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed,
                            this.Name + " " + errorMessage,
                            this.GetType().Name);
                        aApplicationTransaction.SetAllocationActionStatus(apKey, eAllocationActionStatus.NoActionPerformed);
                    }
                    // Begin TT#5663 - JSmith - Null Reference Error
                    //else if (((AllocationProfile)aAllocationProfile).IsSubordinateHeader && !((AllocationProfile)aAllocationProfile).DCFulfillmentProcessed)
                    else if (aAllocationProfile != null && (((AllocationProfile)aAllocationProfile).IsSubordinateHeader && !((AllocationProfile)aAllocationProfile).DCFulfillmentProcessed))
                    // End TT#5663 - JSmith - Null Reference Error
                    {
                        string errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ((AllocationProfile)aAllocationProfile).HeaderID);
                        SAB.ApplicationServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Warning,
                            eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed,
                            this.Name + " " + errorMessage,
                            this.GetType().Name);
                        aApplicationTransaction.SetAllocationActionStatus(apKey, eAllocationActionStatus.NoActionPerformed);
                    }
                    else
                    {
                        // End TT#1966-MD - JSmith- DC Fulfillment

                        actionPerformed = ProcessAction(aApplicationTransaction, apKey);

                        if (actionPerformed)
                        {
                            aApplicationTransaction.SetAllocationActionStatus(apKey, eAllocationActionStatus.ActionCompletedSuccessfully);
                        }
                        else
                        {
                            aApplicationTransaction.SetAllocationActionStatus(apKey, eAllocationActionStatus.NoActionPerformed);
                        }
                    }
				}
				catch (MIDException ex)
				{
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ErrorMessage, this.ToString());
					msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
					msg = msg.Replace("{0}", _lblSizeCurve);
					msg = msg.Replace("{1}", this.Name);
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.GetType().Name);
					aApplicationTransaction.SetAllocationActionStatus(apKey, eAllocationActionStatus.ActionFailed);
					throw;
				}
				catch (Exception ex)
				{
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, ex.ToString(), this.ToString());
					msg = MIDText.GetText(eMIDTextCode.msg_al_MethodFailed);
					msg = msg.Replace("{0}", _lblSizeCurve);
					msg = msg.Replace("{1}", this.Name);
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, msg, this.GetType().Name);
					aApplicationTransaction.SetAllocationActionStatus(apKey, eAllocationActionStatus.ActionFailed);
					throw;
				}
			}
			catch
			{
				throw;
			}
		}

		private bool ProcessAction(ApplicationSessionTransaction aAppTran, int aApKey)
		{
			int crvGrpKey;
			SizeCurveGroupProfile currSizeCurveGrpProf;
			string errMsg;
			NodeDescendantList descNodeList;
			HierarchyNodeProfile sizeNode;
			SizeCurve dataLock = null;
			SizeCurveGroupEnqueue scgEnqueue;
			DataTable dtLock;
			
			try
			{
				if (SizeGroupRid != Include.NoRID)
				{
					_szGrpProf = new SizeGroupProfile(SizeGroupRid);
				}
				else
				{
					_szGrpProf = new SizeGroupProfile();

					foreach (SizeCurveMerchBasisProfile scmbp in _merchBasisProfileList)
					{
                        // Begin TT#1952 - JSmith - Size Curve Failures - Override Low Level
                        //descNodeList = SAB.HierarchyServerSession.GetNodeDescendantList(scmbp.Basis_HN_RID, eHierarchyLevelType.Size, eNodeSelectType.All);

                        //foreach (NodeDescendantProfile descProf in descNodeList)
                        //{
                        //    sizeNode = SAB.HierarchyServerSession.GetNodeData(descProf.Key);

                        //    if (!_szGrpProf.SizeCodeList.Contains(sizeNode.ColorOrSizeCodeRID))
                        //    {
                        //        _szGrpProf.SizeCodeList.Add(new SizeCodeProfile(sizeNode.ColorOrSizeCodeRID));
                        //    }
                        //}
                        if (scmbp.OverrideLowLevelRid == Include.NoRID)
                        {
                            // Begin TT#2231 - JSmith - Size curve build failing
                            //descNodeList = SAB.HierarchyServerSession.GetNodeDescendantList(scmbp.Basis_HN_RID, eHierarchyLevelType.Size, eNodeSelectType.All);

                            //foreach (NodeDescendantProfile descProf in descNodeList)
                            //{
                            //    sizeNode = SAB.HierarchyServerSession.GetNodeData(descProf.Key);

                            //    if (!szGrpProf.SizeCodeList.Contains(sizeNode.ColorOrSizeCodeRID))
                            //    {
                            //        szGrpProf.SizeCodeList.Add(new SizeCodeProfile(sizeNode.ColorOrSizeCodeRID));
                            //    }
                            //}

                            descNodeList = SAB.HierarchyServerSession.GetNodeDescendantList(Include.NoRID, scmbp.Basis_HN_RID, eHierarchyDescendantType.masterType, 0, eHierarchyLevelMasterType.Size, eNodeSelectType.All);

                            foreach (NodeDescendantProfile descProf in descNodeList)
                            {
                                if (!_szGrpProf.SizeCodeList.Contains(descProf.ColorOrSizeCodeRID))
                                {
                                    _szGrpProf.SizeCodeList.Add(new SizeCodeProfile(descProf.ColorOrSizeCodeRID));
                                }
                            }
                            // End TT#2231
                        }
                        else
                        {
                            // Begin TT#2231 - JSmith - Size curve build failing
                            //int sizeLevelSequence = -1;
                            //HierarchyProfile hp = SAB.HierarchyServerSession.GetMainHierarchyData();
                            //foreach (DictionaryEntry lvl in hp.HierarchyLevels)
                            //{
                            //    int level = (int)lvl.Key;
                            //    HierarchyLevelProfile xhlp = (HierarchyLevelProfile)lvl.Value;
                            //    switch (xhlp.LevelType)
                            //    {
                            //        case eHierarchyLevelType.Size:
                            //            sizeLevelSequence = xhlp.Key;
                            //            break;
                            //    }
                            //}

                            //SizeGroupProfile szGrpProf = new SizeGroupProfile();
                            //HierarchySessionTransaction hierTran = new HierarchySessionTransaction(SAB);
                            //LowLevelVersionOverrideProfileList lowLevelList = hierTran.GetOverrideList(scmbp.OverrideLowLevelRid, scmbp.Basis_HN_RID, scmbp.Basis_FV_RID,
                            //                                                eHierarchyDescendantType.levelType, sizeLevelSequence, Include.NoRID, true, false);
                            //foreach (LowLevelVersionOverrideProfile lvop in lowLevelList.ArrayList)
                            //{
                            //    if (!szGrpProf.SizeCodeList.Contains(lvop.NodeProfile.ColorOrSizeCodeRID))
                            //    {
                            //        szGrpProf.SizeCodeList.Add(new SizeCodeProfile(lvop.NodeProfile.ColorOrSizeCodeRID));
                            //    }
                            //}

                            HierarchySessionTransaction hierTran = new HierarchySessionTransaction(SAB);
                            LowLevelVersionOverrideProfileList lowLevelList2 = hierTran.GetOverrideListOfSizes(scmbp.OverrideLowLevelRid, scmbp.Basis_HN_RID, true);

                            foreach (LowLevelVersionOverrideProfile lvop in lowLevelList2.ArrayList)
                            {
                                if (!_szGrpProf.SizeCodeList.Contains(lvop.NodeProfile.ColorOrSizeCodeRID))
                                {
                                    _szGrpProf.SizeCodeList.Add(new SizeCodeProfile(lvop.NodeProfile.ColorOrSizeCodeRID));
                                }
                            }

                            // End TT#2231
                        }
                        // End TT#1952
					}
				}

				currSizeCurveGrpProf = new SizeCurveGroupProfile(Include.NoRID);
				currSizeCurveGrpProf.SizeCurveGroupName = this.Name;
				currSizeCurveGrpProf.DefinedSizeGroupRID = SizeGroupRid;

				if (ProcessMerchandiseBasis(currSizeCurveGrpProf, aAppTran))
				{
					//FUTURE -- IMPLMEMENT CURVE BASIS IF REQUIRED
					//currSizeCurveGrpProf = ProcessCurveBasis(currSizeCurveGrpProf);

					try
					{
						dataLock = new SizeCurve();
						dataLock.OpenUpdateConnection(eLockType.SizeCurve, currSizeCurveGrpProf.SizeCurveGroupName);

						crvGrpKey = _sizeCurveData.GetSizeCurveGroupKey(this.Name);

						if (crvGrpKey != Include.NoRID)
						{
							// Size Curve Group Exists -- Update

							scgEnqueue = new SizeCurveGroupEnqueue(crvGrpKey, SAB.ClientServerSession.UserRID, SAB.ClientServerSession.ThreadID);

							try
							{
								// Enqueue Size Curve Group

								scgEnqueue.EnqueueSizeCurveGroup();

								// Write Size Curve Group

								currSizeCurveGrpProf.Key = crvGrpKey;
								currSizeCurveGrpProf.WriteSizeCurveGroup(SAB, false, true);

								// Dequeue Size Curve Group

                                //scgEnqueue.DequeueSizeCurveGroup();  // TT#4351 - JSmith - Size Curve Group is not unlocked if error occurs
							}
							catch (SizeCurveGroupConflictException)
							{
								// Size Curve Group in use

								errMsg = MIDText.GetText(eMIDTextCode.msg_CurveGroupInUse);
								errMsg = errMsg.Replace("{0}", Name);
								SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errMsg, this.GetType().Name);
							}
							catch
							{
								throw;
							}
                            // Begin TT#4351 - JSmith - Size Curve Group is not unlocked if error occurs
                            finally
                            {
                                scgEnqueue.DequeueSizeCurveGroup();
                            }
                            // End TT#4351 - JSmith - Size Curve Group is not unlocked if error occurs
						}
						else
						{
							// Size Curve Group Does Not Exist -- Insert

							currSizeCurveGrpProf.WriteSizeCurveGroup(SAB, false, true);
						}
					}
					catch
					{
						throw;
					}
					finally
					{
						if (dataLock != null && dataLock.ConnectionIsOpen)
						{
							dataLock.CloseUpdateConnection();
						}
					}

					return true;
				}
				else
				{
					errMsg = MIDText.GetText(eMIDTextCode.msg_NoCurveCreated);
					errMsg = errMsg.Replace("{0}", Name);
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, errMsg, this.GetType().Name);
					return false;
				}
			}
			catch
			{
				throw;
			}
		}

        //begin TT#1235-MD -jsobek -Size Curve Null Reference Exception (Object Reference) -Code is not dead, it is an override.
		override public void Update(TransactionData td)
		{
			if (_methodData == null || base.Key < 0 || base.Method_Change_Type == eChangeType.add)
			{
				_methodData = new MethodSizeCurveData(td);
			}

            _methodData.SizeGroupRID = SizeGroupRid;
			//Begin TT#1076 - JScott - Size Curves by Set
			_methodData.SizeCurvesByType = _sizeCurvesByType;

			if (_sizeCurvesByType == eSizeCurvesByType.AttributeSet)
			{
				_methodData.SizeCurvesBySGRID = _sizeCurvesBySGRID;
			}
			else
			{
				_methodData.SizeCurvesBySGRID = Include.NoRID;
			}

			//End TT#1076 - JScott - Size Curves by Set
			_methodData.MerchBasisEqualizeWeight = _merchBasisEqualizeWeight;
			_methodData.CurveBasisEqualizeWeight = _curveBasisEqualizeWeight;
			_methodData.ApplyLostSales = _applyLostSales;
			_methodData.TolerMinAvgPerSize = _tolerMinAvgPerSize;
			_methodData.TolerSalesTolerance = _tolerSalesTolerance;
			_methodData.TolerIndexUnitsType = _tolerIndexUnitsType;
			_methodData.TolerMinTolerancePct = _tolerMinTolerancePct;
            //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
            _methodData.ApplyMinToZeroTolerance = _applyMinToZeroTolerance;
		    //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
			_methodData.TolerMaxTolerancePct = _tolerMaxTolerancePct;
			_methodData.DTMerchBasisDetail = _dtMerchBasisDetail;
            _methodData.DTCurveBasisDetail = _dtCurveBasisDetail;

			try
			{
				switch (base.Method_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						_methodData.InsertMethod(base.Key, td);
						break;
					case eChangeType.update:
						base.Update(td);
						_methodData.UpdateMethod(base.Key, td);
						break;
					case eChangeType.delete:
						_methodData.DeleteMethod(base.Key, td);
						base.Update(td);
						break;
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				//TO DO:  whatever has to be done after an update or exception.
			}
		}
        //end TT#1235-MD -jsobek -Size Curve Null Reference Exception (Object Reference) -Code is not dead, it is an override.
		#endregion

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
			SizeCurveMethod newSizeCurveMethod = null;

			try
			{
				newSizeCurveMethod = (SizeCurveMethod)this.MemberwiseClone();
				

				return newSizeCurveMethod;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
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
			return true;
		}

        // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
        /// <summary>
        /// Returns a flag identifying if the user can view the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        override public bool AuthorizedToView(Session aSession, int aUserRID)
        {
            HierarchyNodeSecurityProfile hierNodeSecurity;

            foreach (SizeCurveMerchBasisProfile scbp in _merchBasisProfileList)
            {
                if (scbp.Basis_HN_RID != Include.NoRID)
                {
                    hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(scbp.Basis_HN_RID, (int)eSecurityTypes.Store);

                    if (!hierNodeSecurity.AllowView)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.

        // BEGIN RO-642 - RDewey
        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalSizeCurve);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserSizeCurve);
            }

        }
        override public ROMethodProperties MethodGetData(out bool successful, ref string message, bool processingApply = false)
        {
            successful = true;

            //RO4431 Data Transport for Size Curve Method
            //throw new NotImplementedException("MethodGetData is not implemented");
            ROMethodSizeCurveProperties method = new ROMethodSizeCurveProperties(
                method: GetName.GetMethod(method: this),
                description: _methodData.Method_Description,
                userKey: User_RID,
                sizeGroup: GetName.GetSizeGroup(_methodData.SizeGroupRID),
                attribute: GetName.GetAttributeName(_methodData.SizeCurvesBySGRID),
                sizeCurvesByType: EnumTools.VerifyEnumValue(_methodData.SizeCurvesByType),
                merchBasisEqualizeWeight: _methodData.MerchBasisEqualizeWeight,
                sizeCurveMerchBasisSet: SizeCurveMerchBasisSet.BuildSizeCurveMerchBasisSet(_methodData.Method_RID,
                            _methodData.Method_Type_ID, DTMerchBasisDetail, SAB),
                tolerMinAvgPerSize: _methodData.TolerMinAvgPerSize,
                tolerSalesTolerance: _methodData.TolerSalesTolerance,
                tolerIndexUnitsType: EnumTools.VerifyEnumValue(_methodData.TolerIndexUnitsType),
                tolerMinTolerancePct: _methodData.TolerMinTolerancePct,
                tolerMaxTolerancePct: _methodData.TolerMaxTolerancePct,
                applyMinToZeroTolerance: _methodData.ApplyMinToZeroTolerance,
                isTemplate: Template_IND
            );

            return method;
        }

        override public ROOverrideLowLevel MethodGetOverrideModelList(ROOverrideLowLevel overrideLowLevel, out bool successful, ref string message)
        {
            successful = true;

            throw new NotImplementedException("MethodGetOverrideModelList is not implemented");
        }

        override public bool MethodSetData(ROMethodProperties methodProperties, ref string message, bool processingApply)
        {
            //RO4431 Data Transport for Size Curve Method
            ROMethodSizeCurveProperties roMethodSizeCurveProperties = (ROMethodSizeCurveProperties)methodProperties;

            try
            {
                Template_IND = methodProperties.IsTemplate;
                Method_Description = roMethodSizeCurveProperties.Description;
                User_RID = roMethodSizeCurveProperties.UserKey;
                SizeGroupRid = roMethodSizeCurveProperties.SizeGroup.Key;
                SizeCurvesBySGRID = roMethodSizeCurveProperties.Attribute.Key;
                SizeCurvesByType = roMethodSizeCurveProperties.SizeCurvesByType;
                MerchBasisEqualizeWeight = roMethodSizeCurveProperties.MerchBasisEqualizeWeight;
                DTMerchBasisDetail = SizeCurveMerchBasisSet.BuilddtMerchBasisDetail( roMethodSizeCurveProperties.Method.Key, 
                            roMethodSizeCurveProperties.SizeCurveMerchBasisSet, DTMerchBasisDetail, SAB);
                TolerMinAvgPerSize = roMethodSizeCurveProperties.TolerMinAvgPerSize;
                TolerSalesTolerance = roMethodSizeCurveProperties.TolerSalesTolerance;
                TolerIndexUnitsType = roMethodSizeCurveProperties.TolerIndexUnitsType;
                TolerMinTolerancePct = roMethodSizeCurveProperties.TolerMinTolerancePct;
                TolerMaxTolerancePct = roMethodSizeCurveProperties.TolerMaxTolerancePct;
                ApplyMinToZeroTolerance = roMethodSizeCurveProperties.ApplyMinToZeroTolerance;
                return true;
            }
            catch (Exception e)
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
        private bool ProcessMerchandiseBasis(SizeCurveGroupProfile aSzCrvGrpProf, ApplicationSessionTransaction aAppTran)
		{
			MerchandiseBasisList mainBasisList;
			ArrayList storeList;
			StoreSizeValueGroup combinedStrSizeValGrp;
			IDictionaryEnumerator iEnum;
			StoreSizeValue strSzVal;
			Hashtable strValHash;
			//Begin TT#1076 - JScott - Size Curves by Set
			Hashtable storeHash;
			Hashtable storeToSetHash = null;
			StoreGroupProfile strGrpProf = null;
			//End TT#1076 - JScott - Size Curves by Set

			Hashtable useDefault;
			Hashtable parentBasisHash;
			//Begin TT#1076 - JScott - Size Curves by Set
			//decimal chnAvg;
			decimal chnAvg = 0;
			Hashtable setAvg = null;
			//End TT#1076 - JScott - Size Curves by Set
			decimal minAvg;
			bool tolerancePassed;
			int nodeRID;
			ProfileList nodeList;
			int i;
			MerchandiseBasisList newBasisList;
			SizeCurveBasisProfileList merchBasisProfileList;
			SizeCurveMerchBasisProfile scbp;
			SizeCurveSimilarStoreList simStoreList;
			decimal strSalesIdx;

			int szCrvRID;
			ArrayList balOutItemList;
			SizeCurveProfile szCrvProf;
			SizeCodeProfile szCodeProf;
			//Begin TT#1076 - JScott - Size Curves by Set
			//StoreProfile storeProf;
			//End TT#1076 - JScott - Size Curves by Set

			Hashtable chnMinToleranceHash;
			Hashtable chnMaxToleranceHash;
			Hashtable valueHash;
			ArrayList balInItemList;
			//Begin TT#1076 - JScott - Size Curves by Set
			//SizeCurveBalanceItem balInItem;
			//double minTolVal;
			//double maxTolVal;
			//int balIterations;
			//bool balStop;
			//int lockedCount;
			//int unlockedCount;
			int strRID;
			Hashtable storeCountHash;
			Hashtable setSzCrvProfHash = null;
			//End TT#1076 - JScott - Size Curves by Set

			//Begin TT#2168 - DOConnell - Size Curve Method 20% min 50% max Issue
            string balMessage = null;
			//End TT#2168 - DOConnell - Size Curve Method 20% min 50% max Issue
			try
			{
				// Read Merch Basis values

				mainBasisList = ReadMerchandiseBasis(aAppTran, _merchBasisProfileList, _szGrpProf, _nodeSimStoreList);

				// Combine basis values by store

				storeList = mainBasisList.GetStoreList();

				combinedStrSizeValGrp = new StoreSizeValueGroup(storeList);

				// Combine the Chain values

				foreach (MerchandiseBasisEntry entry in mainBasisList)
				{
					strValHash = entry.GetAllStoreSizeValues();

					if (strValHash != null)
					{
						iEnum = strValHash.GetEnumerator();

						while (iEnum.MoveNext())
						{
							strSzVal = (StoreSizeValue)iEnum.Value;
							combinedStrSizeValGrp.Add(strSzVal.StoreRID, strSzVal.SizeCodeRID, strSzVal.SalesValue * entry.Weight);
						}
					}
				}

				//Begin TT#1076 - JScott - Size Curves by Set
				// Apply StoreGroup to combined values

				useDefault = new Hashtable();

				if (_sizeCurvesByType == eSizeCurvesByType.AttributeSet)
				{
					storeHash = new Hashtable();
					storeToSetHash = new Hashtable();

					foreach (int storeRID in storeList)
					{
						storeHash[storeRID] = null;
					}

                    strGrpProf = StoreMgmt.StoreGroup_GetFilled(_sizeCurvesBySGRID); //SAB.StoreServerSession.GetStoreGroupFilled(_sizeCurvesBySGRID);

					foreach (StoreGroupLevelProfile strGrpLvlProf in strGrpProf.GroupLevels)
					{
						foreach (StoreProfile strProf in strGrpLvlProf.Stores)
						{
							storeToSetHash[strProf.Key] = strGrpLvlProf.Key;

							if (!storeHash.Contains(strProf.Key))
							{
								useDefault[strProf.Key] = true;
							}
						}
					}

					combinedStrSizeValGrp.SetStoreGroup(storeToSetHash);
				}

				//Begin TT#1076 - JScott - Size Curves by Set
				//Calculate the Chain/Set Average values to use in the Apply Chain/Set Sales tolerance calculation

				if (_sizeCurvesByType == eSizeCurvesByType.AttributeSet)
				{
					setAvg = new Hashtable();

					foreach (StoreGroupLevelProfile strGrpLvlProf in strGrpProf.GroupLevels)
					{
                        //Begin TT#1667 - JSmith - System.DivideByZeroException: Attempted to divide by zero.
                        //setAvg[strGrpLvlProf.Key] = combinedStrSizeValGrp.GetSetSizeTotalValue(strGrpLvlProf.Key) / combinedStrSizeValGrp.GetSetStoreList(strGrpLvlProf.Key).Count;
                        if (combinedStrSizeValGrp.GetSetStoreList(strGrpLvlProf.Key).Count > 0)
                        {
                            setAvg[strGrpLvlProf.Key] = combinedStrSizeValGrp.GetSetSizeTotalValue(strGrpLvlProf.Key) / combinedStrSizeValGrp.GetSetStoreList(strGrpLvlProf.Key).Count;
                        }
                        else
                        {
                            setAvg[strGrpLvlProf.Key] = 0;
                        }
                        //End TT#1667
					}
				}
				else
				{
                    //Begin TT#1667 - JSmith - System.DivideByZeroException: Attempted to divide by zero.
                    //chnAvg = combinedStrSizeValGrp.ChainSizeTotalValue / storeList.Count;
                    if (storeList.Count > 0)
                    {
                        chnAvg = combinedStrSizeValGrp.ChainSizeTotalValue / storeList.Count;
                    }
                    else
                    {
                        chnAvg = 0;
                    }
                    //End TT#1667
				}

				//End TT#1076 - JScott - Size Curves by Set
				//Apply Higher Level and Apply Chain/Set Sales Tolerance

				//Begin TT#1076 - JScott - Size Curves by Set
				//useDefault = new Hashtable();
				//End TT#1076 - JScott - Size Curves by Set
				parentBasisHash = new Hashtable();
				//Begin TT#1076 - JScott - Size Curves by Set
				//chnAvg = combinedStrSizeValGrp.ChainSizeTotalValue / storeList.Count;
				//End TT#1076 - JScott - Size Curves by Set

				if (_tolerMinAvgPerSize != Include.Undefined || _tolerSalesTolerance != Include.Undefined)
				{
					foreach (int storeRID in storeList)
					{
						tolerancePassed = false;

						strValHash = combinedStrSizeValGrp.GetStoreSizeValues(storeRID);

						if (strValHash != null && strValHash.Count > 0)
						{
							// Process Minimum Average per Size Tolerance

							if (_tolerMinAvgPerSize != Include.Undefined)
							{
								minAvg = (decimal)(Math.Max(strValHash.Count, _szGrpProf.SizeCodeList.Count) * _tolerMinAvgPerSize);

								if (combinedStrSizeValGrp.GetStoreSizeTotalValue(storeRID) >= minAvg)
								{
									tolerancePassed = true;
								}
								else
								{
									if (_tolerHighLvlLevelType != eLowLevelsType.None && _merchBasisProfileList.Count == 1)
									{
										nodeRID = ((SizeCurveMerchBasisProfile)_merchBasisProfileList[0]).Basis_HN_RID;
										nodeList = SAB.HierarchyServerSession.GetAncestorPath(nodeRID, _tolerHighLvlHierarchyRID, _tolerHighLvlLevelType, _tolerHighLvlLevelRID, _tolerHighLvlOffset);

										if (nodeList != null)
										{
											for (i = 1; i < nodeList.Count; i++)
											{
												newBasisList = (MerchandiseBasisList)parentBasisHash[nodeList[i].Key];

												if (newBasisList == null)
												{
													merchBasisProfileList = new SizeCurveBasisProfileList(eProfileType.SizeCurveMerchBasis);

													scbp = new SizeCurveMerchBasisProfile(0);

													scbp.Basis_HN_RID = nodeList[i].Key;
													scbp.Basis_FV_RID = Include.FV_ActualRID;
													scbp.Basis_CDR_RID = ((SizeCurveMerchBasisProfile)_merchBasisProfileList[0]).Basis_CDR_RID;
													scbp.Basis_Weight = 1;
													scbp.MerchType = eMerchandiseType.Node;
													scbp.OverrideLowLevelRid = ((SizeCurveMerchBasisProfile)_merchBasisProfileList[0]).OverrideLowLevelRid;
													scbp.CustomOverrideLowLevelRid = ((SizeCurveMerchBasisProfile)_merchBasisProfileList[0]).CustomOverrideLowLevelRid;

													merchBasisProfileList.Add(scbp);
													//Begin TT#1517-MD -jsobek -Store Service Optimization
                                                    ProfileList activeStoreList = StoreMgmt.StoreProfiles_GetActiveStoresList();
                                                    simStoreList = SAB.HierarchyServerSession.GetSizeCurveSimilarStoreList(activeStoreList, nodeList[i].Key, true, false); //SAB.HierarchyServerSession.GetSizeCurveSimilarStoreList(SAB.ApplicationServerSession.GetProfileList(eProfileType.Store), nodeList[i].Key, true, false);
													//End TT#1517-MD -jsobek -Store Service Optimization

													newBasisList = ReadMerchandiseBasis(aAppTran, merchBasisProfileList, _szGrpProf, simStoreList);
													parentBasisHash[nodeList[i].Key] = newBasisList;
												}

												if (((MerchandiseBasisEntry)newBasisList[0]).GetStoreSizeTotalValue(storeRID) >= minAvg)
												{
													combinedStrSizeValGrp.ReplaceStoreSizeValues(storeRID, ((MerchandiseBasisEntry)newBasisList[0]).GetStoreSizeValues(storeRID));
													tolerancePassed = true;
													break;
												}
											}
										}
									}
								}
							}

							// Process Apply Chain/Set Sales Tolerance

							if (!tolerancePassed && _tolerSalesTolerance != Include.Undefined)
							{
								if (_tolerIndexUnitsType == eNodeChainSalesType.IndexToAverage)
								{
									//Begin TT#1076 - JScott - Size Curves by Set
									//strSalesIdx = combinedStrSizeValGrp.GetStoreSizeTotalValue(storeRID) / chnAvg;
									if (_sizeCurvesByType == eSizeCurvesByType.AttributeSet)
									{
                                        //Begin TT#1667 - JSmith - System.DivideByZeroException: Attempted to divide by zero.
                                        //strSalesIdx = combinedStrSizeValGrp.GetStoreSizeTotalValue(storeRID) / (decimal)setAvg[storeToSetHash[storeRID]];
                                        if ((decimal)setAvg[storeToSetHash[storeRID]] > 0)
                                        {
                                            strSalesIdx = combinedStrSizeValGrp.GetStoreSizeTotalValue(storeRID) / (decimal)setAvg[storeToSetHash[storeRID]];
                                        }
                                        else
                                        {
                                            strSalesIdx = 0;
                                        }
                                        //End TT#1667
									}
									else
									{
                                        //Begin TT#1667 - JSmith - System.DivideByZeroException: Attempted to divide by zero.
                                        //strSalesIdx = combinedStrSizeValGrp.GetStoreSizeTotalValue(storeRID) / chnAvg;
                                        if (chnAvg > 0)
                                        {
                                            strSalesIdx = combinedStrSizeValGrp.GetStoreSizeTotalValue(storeRID) / chnAvg;
                                        }
                                        else
                                        {
                                            strSalesIdx = 0;
                                        }
                                        //End TT#1667
									}
									//End TT#1076 - JScott - Size Curves by Set

									if (strSalesIdx >= (decimal)(_tolerSalesTolerance / 100))
									{
										tolerancePassed = true;
									}
								}
								else
								{
									if (combinedStrSizeValGrp.GetStoreSizeTotalValue(storeRID) >= (decimal)_tolerSalesTolerance)
									{
										tolerancePassed = true;
									}
								}
							}
						}

						useDefault[storeRID] = !tolerancePassed;
					}
				}

				// Generate Size Curves

				szCrvRID = -1;
				balInItemList = new ArrayList();
				chnMinToleranceHash = new Hashtable();
				chnMaxToleranceHash = new Hashtable();

				aSzCrvGrpProf.StoreSizeCurveHash.Clear();
				aSzCrvGrpProf.SizeCurveList.Clear();

				// Balance the Chain (Default) curve

				valueHash = combinedStrSizeValGrp.GetChainSizeValues();

				if (valueHash.Count > 0)
				{
					aSzCrvGrpProf.DefaultSizeCurve = new SizeCurveProfile(szCrvRID);
					aSzCrvGrpProf.DefaultSizeCurve.SizeCurveName = this.Name + " - Default";

					//Begin TT#1076 - JScott - Size Curves by Set
					//balInItemList = new ArrayList();
					//iEnum = valueHash.GetEnumerator();

					//while (iEnum.MoveNext())
					//{
					//    strSzVal = (StoreSizeValue)iEnum.Value;
					//    balInItemList.Add(new SizeCurveBalanceItem(strSzVal.SizeCodeRID, (double)strSzVal.SalesValue, false));
					//}

					//balOutItemList = BalanceBasisValues(balInItemList);
					balOutItemList = BalanceSizeCurve(combinedStrSizeValGrp, valueHash);
					//End TT#1076 - JScott - Size Curves by Set

					foreach (SizeCurveBalanceItem balOutItem in balOutItemList)
					{
						szCodeProf = new SizeCodeProfile(balOutItem.SizeCode);
						szCodeProf.SizeCodePercent = Convert.ToSingle(balOutItem.Value);
						aSzCrvGrpProf.DefaultSizeCurve.SizeCodeList.Add(szCodeProf);
					}

					aSzCrvGrpProf.SizeCurveList.Add(aSzCrvGrpProf.DefaultSizeCurve);

					szCrvRID--;

					if (_tolerMinTolerancePct != Include.Undefined || _tolerMaxTolerancePct != Include.Undefined)
					{
						//Begin TT#1076 - JScott - Size Curves by Set
						//balInItemList = new ArrayList();
						//iEnum = valueHash.GetEnumerator();

						//while (iEnum.MoveNext())
						//{
						//    strSzVal = (StoreSizeValue)iEnum.Value;
						//    balInItemList.Add(new SizeCurveBalanceItem(strSzVal.SizeCodeRID, (double)strSzVal.SalesValue / combinedStrSizeValGrp.GetChainStoreCountBySize(strSzVal.SizeCodeRID), false));
						//}

						//balOutItemList = BalanceBasisValues(balInItemList);
						storeCountHash = new Hashtable();
						iEnum = valueHash.GetEnumerator();

						while (iEnum.MoveNext())
						{
							strSzVal = (StoreSizeValue)iEnum.Value;
							storeCountHash[strSzVal.SizeCodeRID] = combinedStrSizeValGrp.GetChainStoreCountBySize(strSzVal.SizeCodeRID);
						}
						
						//Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
						//balOutItemList = BalanceSizeCurveForAverage(combinedStrSizeValGrp, valueHash, storeCountHash);
						//End TT#1076 - JScott - Size Curves by Set
						
						foreach (SizeCurveBalanceItem balOutItem in balOutItemList)
						{
                            // Begin TT#2908 - JSmith - Size Tolerance for Size Curves-> min % calculation 
                            //chnMinToleranceHash[balOutItem.SizeCode] = balOutItem.Value * (_tolerMinTolerancePct / 100);
                            // Begin TT#2912 - JSmith - Size Curve Method -> Max tolerance % is not correct when processing max tolerance % only
                            //chnMinToleranceHash[balOutItem.SizeCode] = balOutItem.Value * (1 - (_tolerMinTolerancePct / 100));
                            if (_tolerMinTolerancePct != Include.Undefined)
                            {
                                chnMinToleranceHash[balOutItem.SizeCode] = balOutItem.Value * (1 - (_tolerMinTolerancePct / 100));
                            }
                            // Begin TT#2912 - JSmith - Size Curve Method -> Max tolerance % is not correct when processing max tolerance % only
                            // End TT#2908 - JSmith - Size Tolerance for Size Curves-> min % calculation 
                            // Begin TT#2912 - JSmith - Size Curve Method -> Max tolerance % is not correct when processing max tolerance % only
                            //chnMaxToleranceHash[balOutItem.SizeCode] = balOutItem.Value * (1 + (_tolerMaxTolerancePct / 100));
                            if (_tolerMaxTolerancePct != Include.Undefined)
                            {
                                chnMaxToleranceHash[balOutItem.SizeCode] = balOutItem.Value * (1 + (_tolerMaxTolerancePct / 100));
                            }
                            // End TT#2912 - JSmith - Size Curve Method -> Max tolerance % is not correct when processing max tolerance % only
						}
						//End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
					}
				}
				else
				{
					return false;
				}

				//Begin TT#1076 - JScott - Size Curves by Set
				//foreach (int storeRID in storeList)
				//{
				//    if ((useDefault[storeRID] == null || !(bool)useDefault[storeRID]))
				//    {
				//        valueHash = combinedStrSizeValGrp.GetStoreSizeValues(storeRID);

				//        if (valueHash != null && valueHash.Count > 0)
				//        {
				//            balInItemList = new ArrayList();
				//            iEnum = valueHash.GetEnumerator();

				//            while (iEnum.MoveNext())
				//            {
				//                strSzVal = (StoreSizeValue)iEnum.Value;
				//                balInItemList.Add(new SizeCurveBalanceItem(strSzVal.SizeCodeRID, (double)strSzVal.SalesValue, false));
				//            }

				//            balIterations = 0;
				//            balStop = false;

				//            while (!balStop)
				//            {
				//                balOutItemList = BalanceBasisValues(balInItemList);
				//                balIterations++;

				//                if (balIterations < 4 && (_tolerMinTolerancePct != Include.Undefined || _tolerMaxTolerancePct != Include.Undefined))
				//                {
				//                    balInItemList = new ArrayList();
				//                    lockedCount = 0;
				//                    unlockedCount = 0;

				//                    foreach (SizeCurveBalanceItem balOutItem in balOutItemList)
				//                    {
				//                        if (!balOutItem.Locked)
				//                        {
				//                            minTolVal = (double)chnMinToleranceHash[balOutItem.SizeCode];
				//                            maxTolVal = (double)chnMaxToleranceHash[balOutItem.SizeCode];

				//                            if (_tolerMinTolerancePct > 0 && balOutItem.Value < minTolVal)
				//                            {
				//                                balInItem = new SizeCurveBalanceItem(balOutItem.SizeCode, minTolVal, true);
				//                                lockedCount++;
				//                            }
				//                            else if (_tolerMaxTolerancePct > 0 && balOutItem.Value > maxTolVal)
				//                            {
				//                                balInItem = new SizeCurveBalanceItem(balOutItem.SizeCode, maxTolVal, true);
				//                                lockedCount++;
				//                            }
				//                            else
				//                            {
				//                                balInItem = new SizeCurveBalanceItem(balOutItem.SizeCode, balOutItem.Value, false);
				//                                unlockedCount++;
				//                            }
				//                        }
				//                        else
				//                        {
				//                            balInItem = new SizeCurveBalanceItem(balOutItem.SizeCode, balOutItem.Value, true);
				//                            lockedCount++;
				//                        }

				//                        balInItemList.Add(balInItem);
				//                    }

				//                    if (unlockedCount <= 1 || lockedCount == balInItemList.Count || lockedCount == 0)
				//                    {
				//                        balStop = true;
				//                    }
				//                }
				//                else
				//                {
				//                    balStop = true;
				//                }
				//            }

				//            szCrvProf = new SizeCurveProfile(szCrvRID);

				//            storeProf = GetStoreProfile(storeRID);
				//            szCrvProf.SizeCurveName = this.Name + " - Store " + storeProf.Text;

				//            aSzCrvGrpProf.StoreSizeCurveHash[storeRID] = szCrvProf;

				//            foreach (SizeCurveBalanceItem balOutItem in balOutItemList)
				//            {
				//                szCodeProf = new SizeCodeProfile(balOutItem.SizeCode);
				//                szCodeProf.SizeCodePercent = Convert.ToSingle(balOutItem.Value);
				//                szCrvProf.SizeCodeList.Add(szCodeProf);
				//            }

				//            aSzCrvGrpProf.SizeCurveList.Add(szCrvProf);

				//            szCrvRID--;
				//        }
				//    }
				//}
				// Balance the Store Group curves

				if (_sizeCurvesByType == eSizeCurvesByType.AttributeSet)
				{
					setSzCrvProfHash = new Hashtable();

					foreach (StoreGroupLevelProfile strGrpLvlProf in strGrpProf.GroupLevels)
					{
						valueHash = combinedStrSizeValGrp.GetSetSizeValues(strGrpLvlProf.Key);

						if (valueHash != null && valueHash.Count > 0)
						{
							balOutItemList = BalanceSizeCurve(combinedStrSizeValGrp, valueHash);

							szCrvProf = new SizeCurveProfile(szCrvRID);
							szCrvProf.SizeCurveName = this.Name + " - Set " + strGrpLvlProf.Name;

							setSzCrvProfHash[strGrpLvlProf.Key] = szCrvProf;

							foreach (SizeCurveBalanceItem balOutItem in balOutItemList)
							{
								szCodeProf = new SizeCodeProfile(balOutItem.SizeCode);
								szCodeProf.SizeCodePercent = Convert.ToSingle(balOutItem.Value);
								szCrvProf.SizeCodeList.Add(szCodeProf);
							}

							aSzCrvGrpProf.SizeCurveList.Add(szCrvProf);

							szCrvRID--;
						}
					}
				}

				// Balance the Store curves
				foreach (StoreProfile storeProf in aAppTran.GetProfileList(eProfileType.Store))
				{
					strRID = storeProf.Key;

					if ((useDefault[strRID] == null || !(bool)useDefault[strRID]))
					{
						valueHash = combinedStrSizeValGrp.GetStoreSizeValues(strRID);

                        if (valueHash != null && valueHash.Count > 0)
                        {
                            //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                            if (ApplyMinToZeroTolerance == true)
                            {
                                ArrayList newArray = new ArrayList();

                                foreach (SizeCodeProfile scp in aSzCrvGrpProf.SizeCodeList)
                                {
                                    bool found = false;
                                    foreach (StoreSizeValue ssv in valueHash.Keys)
                                    {
                                        if (ssv.SizeCodeRID == scp.Key)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }

                                    if (!found)
                                    {
                                        int sizeCodeRID = scp.Key;
                                        Decimal salesValue = 0;
                                        strSzVal = new StoreSizeValue(strRID, sizeCodeRID, salesValue);

                                        newArray.Add(strSzVal);
                                    }
                                }
                                foreach (StoreSizeValue ssv in newArray)
                                {
                                    valueHash.Add(ssv, ssv);
                                }
                            }
                            //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
							//Begin TT#2168 - DOConnell - Size Curve Method 20% min 50% max Issue
                            try
                            {
                                balOutItemList = BalanceSizeCurveWithMinMax(combinedStrSizeValGrp, valueHash, chnMinToleranceHash, chnMaxToleranceHash);
                            }
                            catch (NothingToSpreadException)
                            {
                                balMessage = MIDText.GetText(eMIDTextCode.msg_AllSizesLocked);
                                balMessage = balMessage.Replace("{0}", Convert.ToString(this.Name));
                                balMessage = balMessage.Replace("{1}", Convert.ToString(storeProf.Text));
                                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, balMessage, this.GetType().Name);
                                balOutItemList.Clear();
                            }
                            if (balOutItemList.Count != 0)
                            {
                                szCrvProf = new SizeCurveProfile(szCrvRID);
                                szCrvProf.SizeCurveName = this.Name + " - Store " + storeProf.Text;
                                aSzCrvGrpProf.StoreSizeCurveHash[strRID] = szCrvProf;

                                foreach (SizeCurveBalanceItem balOutItem in balOutItemList)
                                {
                                    szCodeProf = new SizeCodeProfile(balOutItem.SizeCode);
                                    szCodeProf.SizeCodePercent = Convert.ToSingle(balOutItem.Value);
                                    szCrvProf.SizeCodeList.Add(szCodeProf);
                                }

                                aSzCrvGrpProf.SizeCurveList.Add(szCrvProf);

                                szCrvRID--;
                            }
							//End TT#2168 - DOConnell - Size Curve Method 20% min 50% max Issue
                        }
					}
					else
					{
						if (_sizeCurvesByType == eSizeCurvesByType.AttributeSet)
						{
							szCrvProf = (SizeCurveProfile)setSzCrvProfHash[storeToSetHash[strRID]];

							if (szCrvProf != null)
							{
								aSzCrvGrpProf.SetStoreSizeCurveProfile(strRID, szCrvProf.Key);
							}
						}
					}
				}
				//End TT#1076 - JScott - Size Curves by Set

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private MerchandiseBasisList ReadMerchandiseBasis(ApplicationSessionTransaction aAppTran, SizeCurveBasisProfileList aMerchBasisProfList, SizeGroupProfile aSzGrpProf, SizeCurveSimilarStoreList aSimStoreList)
		{
			MerchandiseBasisList basisList;
			HierarchyNodeProfile nodeProf;
			DateRangeProfile drp;
			ProfileList weekList;
			MerchandiseBasisEntry merchBasisEntry;
			decimal totalSizeValue;

			try
			{
				basisList = new MerchandiseBasisList();

				foreach (SizeCurveMerchBasisProfile scmbp in aMerchBasisProfList)
				{
					nodeProf = SAB.HierarchyServerSession.GetNodeData(scmbp.Basis_HN_RID);
					drp = SAB.ApplicationServerSession.Calendar.GetDateRange(scmbp.Basis_CDR_RID);
					drp.InternalAnchorDate = SAB.ApplicationServerSession.Calendar.CurrentWeek;
					weekList = SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(drp, drp.InternalAnchorDate);

					merchBasisEntry = ReadSizeValues(aAppTran, nodeProf, weekList, scmbp, aSzGrpProf);

					ProcessNodeSimilarStore(aAppTran, merchBasisEntry, nodeProf, weekList, aSzGrpProf, aSimStoreList);

					basisList.Add(merchBasisEntry);
				}

				if (MerchBasisEqualizeWeight)
				{
					totalSizeValue = 0;

					foreach (MerchandiseBasisEntry basisEntry in basisList)
					{
						totalSizeValue += basisEntry.ChainSizeTotalValue;
					}

					foreach (MerchandiseBasisEntry basisEntry in basisList)
					{
						if (basisEntry.ChainSizeTotalValue > 0)
						{
							basisEntry.Weight = (totalSizeValue * basisEntry.Weight) / basisEntry.ChainSizeTotalValue;
						}
					}
				}

				return basisList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private MerchandiseBasisEntry ReadSizeValues(ApplicationSessionTransaction aAppTran, HierarchyNodeProfile aNodeProf, ProfileList aWeekList, SizeCurveMerchBasisProfile aSzCrvMerchBasisProf, SizeGroupProfile aSzGrpProf)
		{
            //Begin TT#739-MD -jsobek -Delete Stores
			//MIDTimer ollTimer;
			//MIDTimer readTimer;
			MIDTimer totalTimer;
			DataTable dtSizeValues;
			MerchandiseBasisEntry merchBasisEntry;
			//ArrayList varKeyList;
			//ArrayList basisKeyList;
			//ArrayList timeKeyList;
			//List<StoreSizeValue> sizeValueList;

            
			
            //if (_useStoredProcedure)
            //{
				//======================
				// CALL STORED PROCEDURE
				//======================

				totalTimer = new MIDTimer();
				totalTimer.Start();

				dtSizeValues = _methodData.ReadSizeData(aNodeProf, aWeekList, aSzCrvMerchBasisProf.OverrideLowLevelRid);

				totalTimer.Stop();
				SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Size Curve Method Read (Stored Procedure), Node" + aNodeProf.Text + ": Total time: " + totalTimer.ElaspedTimeString, this.ToString());

				merchBasisEntry = new MerchandiseBasisEntry(aAppTran, aNodeProf, aWeekList, aSzGrpProf, aSzCrvMerchBasisProf, dtSizeValues);
            //}
            //else
            //{
            //    //==================================
            //    // CALL StoreVariableHistory ROUTINE
            //    //==================================

            //    ollTimer = new MIDTimer();
            //    ollTimer.Start();
            //    totalTimer = new MIDTimer();
            //    totalTimer.Start();

            //    // Build Variable list to retrieve

            //    varKeyList = new ArrayList();

            //    if (aAppTran.GlobalOptions.GenerateSizeCurveUsing == eGenerateSizeCurveUsing.InStockSales)
            //    {
            //        if (aNodeProf.OTSPlanLevelType == eOTSPlanLevelType.Total)
            //        {
            //            varKeyList.Add(MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSales));
            //        }
            //        else
            //        {
            //            varKeyList.Add(MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSalesReg));
            //            varKeyList.Add(MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSalesPromo));
            //        }
            //    }
            //    else
            //    {
            //        if (aNodeProf.OTSPlanLevelType == eOTSPlanLevelType.Total)
            //        {
            //            varKeyList.Add(SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanVariables.SalesTotalUnitsVariable.DatabaseColumnName);
            //        }
            //        else
            //        {
            //            varKeyList.Add(SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanVariables.SalesRegularUnitsVariable.DatabaseColumnName);
            //            varKeyList.Add(SAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().PlanVariables.SalesPromoUnitsVariable.DatabaseColumnName);
            //        }
            //    }

            //    // Get Hierarchy Node list to retrieve

            //    // Begin TT#1952 - JSmith - Size Curve Failures - Override Low Level
            //    //basisKeyList = SAB.HierarchyServerSession.GetAggregateSizeNodeList(aNodeProf, aSzCrvMerchBasisProf.Basis_FV_RID, aSzCrvMerchBasisProf.OverrideLowLevelRid);
            //    basisKeyList = SAB.HierarchyServerSession.GetAggregateSizeNodeList(aNodeProf, aSzCrvMerchBasisProf.Basis_FV_RID, aSzCrvMerchBasisProf.OverrideLowLevelRid, aSzGrpProf.SizeCodeList.ArrayList);
            //    // End TT#1952

            //    // Build Time list to retrieve

            //    timeKeyList = new ArrayList();

            //    foreach (WeekProfile weekProf in aWeekList)
            //    {
            //        timeKeyList.Add(new SQL_TimeID(eSQLTimeIdType.TimeIdIsWeekly, weekProf.Key));
            //    }

            //    ollTimer.Stop();

            //    // Read Values

            //    readTimer = new MIDTimer();
            //    readTimer.Start();

            //    sizeValueList = _dlStoreVarHist.GetStoreSizeAggregateValues(
            //        (string[])varKeyList.ToArray(typeof(string)),
            //        (SizeAggregateBasisKey[])basisKeyList.ToArray(typeof(SizeAggregateBasisKey)),
            //        (SQL_TimeID[])timeKeyList.ToArray(typeof(SQL_TimeID)));

            //    readTimer.Stop();
            //    totalTimer.Stop();
            //    SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, "Size Curve Method Read (Size Reader), Node" + aNodeProf.Text + ": OLL time: " + ollTimer.ElaspedTimeString + ", Read time: " + readTimer.ElaspedTimeString + ", Total time: " + totalTimer.ElaspedTimeString, this.ToString());

            //    merchBasisEntry = new MerchandiseBasisEntry(aAppTran, aNodeProf, aWeekList, aSzGrpProf, aSzCrvMerchBasisProf, sizeValueList);
            //}

			return merchBasisEntry;
			
            //End TT#739-MD -jsobek -Delete Stores
		}

		private void ProcessNodeSimilarStore(
			ApplicationSessionTransaction aAppTran,
			MerchandiseBasisEntry aMerchBasisEntry,
			HierarchyNodeProfile aNodeProf,
			ProfileList aWeekList,
			SizeGroupProfile aSzGrpProf,
			SizeCurveSimilarStoreList aSimStoreList)
		{
			Hashtable simStoreHash;
			Hashtable dateHash;
			Hashtable valueHash;
			DateRangeProfile drp;
			StoreProfile storeProf;
			ProfileList weekList;
			WeekProfile firstWeek;
			WeekProfile lastWeek;
			IDictionaryEnumerator iEnum;
			MerchandiseBasisEntry merchBasisEntry;
			HashKeyObject dateHashKey;

			try
			{
				if (aSimStoreList != null && aSimStoreList.Count > 0)
				{
					simStoreHash = new Hashtable();
					dateHash = new Hashtable();
					valueHash = new Hashtable();

					foreach (SizeCurveSimilarStoreProfile scssp in aSimStoreList)
					{
						drp = SAB.ApplicationServerSession.Calendar.GetDateRange(scssp.SimStoreUntilDateRangeRID);

						storeProf = GetStoreProfile(scssp.Key);

						if (storeProf.SellingOpenDt != Include.UndefinedDate)
						{
							drp.InternalAnchorDate = SAB.ApplicationServerSession.Calendar.GetWeek(storeProf.SellingOpenDt);
						}

						weekList = SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(drp, drp.InternalAnchorDate);

						if (((WeekProfile)weekList[weekList.Count - 1]).Key > ((WeekProfile)aWeekList[aWeekList.Count - 1]).Key)
						{
							lastWeek = (WeekProfile)aWeekList[aWeekList.Count - 1];
						}
						else
						{
							lastWeek = (WeekProfile)weekList[weekList.Count - 1];
						}

						if (weekList.Count > 1)
						{
							if (((WeekProfile)weekList[0]).Key < ((WeekProfile)aWeekList[0]).Key)
							{
								firstWeek = (WeekProfile)aWeekList[0];
							}
							else
							{
								firstWeek = (WeekProfile)weekList[0];
							}
						}
						else
						{
							firstWeek = (WeekProfile)aWeekList[0];
						}

						dateHashKey = new HashKeyObject(firstWeek.Key, lastWeek.Key);
						simStoreHash[scssp.Key] = dateHashKey;
						dateHash[dateHashKey] = SAB.ApplicationServerSession.Calendar.GetWeekRange(firstWeek, lastWeek);
					}

					iEnum = dateHash.GetEnumerator();

					while (iEnum.MoveNext())
					{
						valueHash[iEnum.Key] = ReadSizeValues(aAppTran, aNodeProf, (ProfileList)iEnum.Value, aMerchBasisEntry.SizeCurveMerchBasisProf, aSzGrpProf);
					}

					foreach (SizeCurveSimilarStoreProfile scssp in aSimStoreList)
					{
						dateHashKey = (HashKeyObject)simStoreHash[scssp.Key];
						merchBasisEntry = (MerchandiseBasisEntry)valueHash[dateHashKey];
						aMerchBasisEntry.AdjustSimilarStoreSizeValue(scssp.Key, merchBasisEntry.GetStoreSizeValues(scssp.Key), merchBasisEntry.GetStoreSizeValues(scssp.SimStoreRID));
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//FUTURE -- IMPLMEMENT CURVE BASIS IF REQUIRED
		//private SizeCurveGroupProfile ProcessCurveBasis(SizeCurveGroupProfile aSzCrvGrpProf)
		//{
		//    CurveBasisList basisList;
		//    int szCrvRID;
		//    ArrayList balOutItemList;
		//    SizeCurveGroupProfile szCrvGrpProf;
		//    SizeCurveProfile szCrvProf;
		//    SizeCodeProfile szCodeProf;
		//    StoreProfile storeProf;
		//    SizeCurveProfile defaultSzCrv;

		//    try
		//    {
		//        basisList = new CurveBasisList();

		//        foreach (SizeCurveCurveBasisProfile sccbp in _curveBasisProfileList)
		//        {
		//            basisList.Add(new CurveBasisEntry(sccbp, new SizeCurveGroupProfile(sccbp.Basis_SizeCurveGroupRID)));
		//        }

		//        if (basisList.Count == 0)
		//        {
		//            return aSzCrvGrpProf;
		//        }

		//        foreach (CurveBasisEntry basisEntry in basisList)
		//        {
		//            basisEntry.Weight = basisEntry.SizeCurveCurveBasisProf.Basis_Weight;
		//        }

		//        if (CurveBasisEqualizeWeight)
		//        {
		//            //FUTURE -- Add Curve Basis weighting here if feature is ever implemented
		//        }

		//        szCrvRID = -1;
		//        szCrvGrpProf = new SizeCurveGroupProfile(Include.NoRID);

		//        // Process the Default Curve

		//        balOutItemList = BalanceCurveBasisValues(aSzCrvGrpProf, basisList, Include.NoRID);

		//        szCrvProf = new SizeCurveProfile(szCrvRID);
		//        szCrvProf.SizeCurveName = this.Name + " - Default";
		//        defaultSzCrv = szCrvProf;

		//        foreach (SizeCurveBalanceItem sizeVal in balOutItemList)
		//        {
		//            szCodeProf = new SizeCodeProfile(sizeVal.SizeCode);
		//            szCodeProf.SizeCodePercent = Convert.ToSingle(sizeVal.Value);
		//            szCrvProf.SizeCodeList.Add(szCodeProf);
		//        }

		//        szCrvGrpProf.SizeCurveList.Add(szCrvProf);

		//        szCrvRID--;

		//        // Process the Store Curves

		//        foreach (int storeRID in basisList.GetStoreList(aSzCrvGrpProf))
		//        {
		//            balOutItemList = BalanceCurveBasisValues(aSzCrvGrpProf, basisList, storeRID);

		//            szCrvProf = new SizeCurveProfile(szCrvRID);

		//            storeProf = GetStoreProfile(storeRID);
		//            szCrvProf.SizeCurveName = this.Name + " - " + storeProf.StoreId;
		//            szCrvGrpProf.StoreSizeCurveHash[storeRID] = szCrvProf;

		//            foreach (SizeCurveBalanceItem sizeVal in balOutItemList)
		//            {
		//                szCodeProf = new SizeCodeProfile(sizeVal.SizeCode);
		//                szCodeProf.SizeCodePercent = Convert.ToSingle(sizeVal.Value);
		//                szCrvProf.SizeCodeList.Add(szCodeProf);
		//            }

		//            szCrvGrpProf.SizeCurveList.Add(szCrvProf);

		//            szCrvRID--;
		//        }

		//        szCrvGrpProf.SizeCurveGroupName = this.Name;
		//        szCrvGrpProf.DefaultSizeCurve = defaultSzCrv;
		//        szCrvGrpProf.DefinedSizeGroupRID = SizeGroupRid;

		//        return szCrvGrpProf;
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//FUTURE -- IMPLMEMENT CURVE BASIS IF REQUIRED
		//private void UpdateValueHash(Hashtable aValueHash, int aSzCodeRID, float aSalesValue, decimal aWeight)
		//{
		//    try
		//    {
		//        if (!aValueHash.Contains(aSzCodeRID))
		//        {
		//            aValueHash[aSzCodeRID] = 0;
		//        }

		//        aValueHash[aSzCodeRID] = (double)(Convert.ToInt32(aValueHash[aSzCodeRID]) + (aSalesValue * (float)aWeight));
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//FUTURE -- IMPLMEMENT CURVE BASIS IF REQUIRED
		//private ArrayList BalanceCurveBasisValues(SizeCurveGroupProfile aSzCrvGrpProf, CurveBasisList aBasisList, int aStoreRID)
		//{
		//    Hashtable valueHash;
		//    SizeCurveProfile szCrvProf;

		//    try
		//    {
		//        valueHash = new Hashtable();

		//        if (aStoreRID != Include.NoRID)
		//        {
		//            szCrvProf = (SizeCurveProfile)aSzCrvGrpProf.StoreSizeCurveHash[aStoreRID];

		//            if (szCrvProf != null)
		//            {
		//                foreach (SizeCodeProfile szCodeProf in szCrvProf.SizeCodeList)
		//                {
		//                    UpdateValueHash(valueHash, szCodeProf.Key, szCodeProf.SizeCodePercent, 1);
		//                }
		//            }

		//            foreach (CurveBasisEntry entry in aBasisList)
		//            {
		//                szCrvProf = (SizeCurveProfile)entry.SizeCurveGroupProfile.StoreSizeCurveHash[aStoreRID];

		//                if (szCrvProf != null)
		//                {
		//                    foreach (SizeCodeProfile szCodeProf in szCrvProf.SizeCodeList)
		//                    {
		//                        UpdateValueHash(valueHash, szCodeProf.Key, szCodeProf.SizeCodePercent, entry.SizeCurveCurveBasisProf.Basis_Weight);
		//                    }
		//                }
		//            }
		//        }
		//        else
		//        {
		//            foreach (SizeCurveProfile szCrvPrf in aSzCrvGrpProf.SizeCurveList)
		//            {
		//                foreach (SizeCodeProfile szCodeProf in szCrvPrf.SizeCodeList)
		//                {
		//                    UpdateValueHash(valueHash, szCodeProf.Key, szCodeProf.SizeCodePercent, 1);
		//                }
		//            }

		//            foreach (CurveBasisEntry entry in aBasisList)
		//            {
		//                foreach (SizeCurveProfile szCrvPrf in entry.SizeCurveGroupProfile.SizeCurveList)
		//                {
		//                    foreach (SizeCodeProfile szCodeProf in szCrvPrf.SizeCodeList)
		//                    {
		//                        UpdateValueHash(valueHash, szCodeProf.Key, szCodeProf.SizeCodePercent, entry.SizeCurveCurveBasisProf.Basis_Weight);
		//                    }
		//                }
		//            }
		//        }

		//        return BalanceBasisValues(valueHash);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//Begin TT#1076 - JScott - Size Curves by Set
		//private ArrayList BalanceBasisValues(ArrayList aBalItemList)
		private ArrayList BalanceSizeCurve(StoreSizeValueGroup aStrValGrp, Hashtable aValueHash)
		{
			try
			{
				return BalanceSizeCurve(aStrValGrp, aValueHash, null, null, null);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private ArrayList BalanceSizeCurveForAverage(StoreSizeValueGroup aStrValGrp, Hashtable aValueHash, Hashtable aStoreCountHash)
		{
			try
			{
				return BalanceSizeCurve(aStrValGrp, aValueHash, aStoreCountHash, null, null);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private ArrayList BalanceSizeCurveWithMinMax(StoreSizeValueGroup aStrValGrp, Hashtable aValueHash, Hashtable aChnMinToleranceHash, Hashtable aChnMaxToleranceHash)
		{
			try
			{
				return BalanceSizeCurve(aStrValGrp, aValueHash, null, aChnMinToleranceHash, aChnMaxToleranceHash);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		
		private ArrayList BalanceSizeCurve(StoreSizeValueGroup aStrValGrp, Hashtable aValueHash, Hashtable aStoreCountHash, Hashtable aChnMinToleranceHash, Hashtable aChnMaxToleranceHash)
		{
			ArrayList balInItemList;
			IDictionaryEnumerator iEnum;
			StoreSizeValue strSzVal;
			int balIterations;
			bool balStop;
			ArrayList balOutItemList = null;
			int lockedCount;
			int unlockedCount;
			double minTolVal;
			double maxTolVal;
			SizeCurveBalanceItem balInItem;

			try
			{
				balInItemList = new ArrayList();
				iEnum = aValueHash.GetEnumerator();

				while (iEnum.MoveNext())
				{
					strSzVal = (StoreSizeValue)iEnum.Value;

					if (aStoreCountHash == null)
					{
						balInItemList.Add(new SizeCurveBalanceItem(strSzVal.SizeCodeRID, (double)strSzVal.SalesValue, false));
					}
					else
					{
						balInItemList.Add(new SizeCurveBalanceItem(strSzVal.SizeCodeRID, (double)strSzVal.SalesValue / (int)aStoreCountHash[strSzVal.SizeCodeRID], false));
					}
				}

				balIterations = 0;
				balStop = false;

				while (!balStop)
				{
                    balOutItemList = BalanceSizeCurveValues(balInItemList);
                    balIterations++;

                    if (aChnMinToleranceHash != null &&
                        aChnMaxToleranceHash != null &&
                        balIterations < 4 &&
                        (_tolerMinTolerancePct != Include.Undefined || _tolerMaxTolerancePct != Include.Undefined))
                    {
                        balInItemList = new ArrayList();
                        lockedCount = 0;
                        unlockedCount = 0;

                        foreach (SizeCurveBalanceItem balOutItem in balOutItemList)
                        {
                            if (!balOutItem.Locked)
                            {
                                //Begin TT#1952 - DOConnell - Size Curve Failures - Override Low Level
                                //minTolVal = (double)(decimal)Math.Round((double)aChnMinToleranceHash[balOutItem.SizeCode], 3);
                                //maxTolVal = (double)(decimal)Math.Round((double)aChnMaxToleranceHash[balOutItem.SizeCode], 3);
                                if (aChnMinToleranceHash.Contains(balOutItem.SizeCode))
                                {
                                    minTolVal = (double)(decimal)Math.Round((double)aChnMinToleranceHash[balOutItem.SizeCode], 3);
                                }
                                else
                                {
                                    minTolVal = 0;
                                }

                                if (aChnMaxToleranceHash.Contains(balOutItem.SizeCode))
                                {
                                    maxTolVal = (double)(decimal)Math.Round((double)aChnMaxToleranceHash[balOutItem.SizeCode], 3);
                                }
                                else
                                {
                                    // Begin TT#2912 - JSmith - Size Curve Method -> Max tolerance % is not correct when processing max tolerance % only
                                    //maxTolVal = 0;
                                    maxTolVal = double.MaxValue;
                                    // End TT#2912 - JSmith - Size Curve Method -> Max tolerance % is not correct when processing max tolerance % only
                                }
                                //End TT#1952 - DOConnell - Size Curve Failures - Override Low Level
                                if (_tolerMinTolerancePct > 0 && balOutItem.Value < minTolVal)
                                {
                                    balInItem = new SizeCurveBalanceItem(balOutItem.SizeCode, minTolVal, true);
                                    lockedCount++;
                                }
                                else if (_tolerMaxTolerancePct > 0 && balOutItem.Value > maxTolVal)
                                {
                                    balInItem = new SizeCurveBalanceItem(balOutItem.SizeCode, maxTolVal, true);
                                    lockedCount++;
                                }
                                else
                                {
                                    balInItem = new SizeCurveBalanceItem(balOutItem.SizeCode, balOutItem.Value, false);
                                    unlockedCount++;
                                }
                            }
                            else
                            {
                                balInItem = new SizeCurveBalanceItem(balOutItem.SizeCode, balOutItem.Value, true);
                                lockedCount++;
                            }

                            balInItemList.Add(balInItem);
                        }
                        //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                        if (!ApplyMinToZeroTolerance)
                        {
                            // Begin TT#2912 - JSmith - Size Curve Method -> Max tolerance % is not correct when processing max tolerance % only
                            //if (unlockedCount <= 1 || lockedCount == balInItemList.Count || lockedCount == 0)
                            if (lockedCount == balInItemList.Count || lockedCount == 0)
                            // End TT#2912 - JSmith - Size Curve Method -> Max tolerance % is not correct when processing max tolerance % only
                            {
                                balStop = true;
                            }
                        }
                        //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                    }
                    else
                    {
                        balStop = true;
                    }

				}

				return balOutItemList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private ArrayList BalanceSizeCurveValues(ArrayList aBalItemList)
		//End TT#1076 - JScott - Size Curves by Set
		{
			ArrayList outValueList = null;
			ArrayList outRefList = null;
			SortedList sortList;
			ArrayList sortOutList;
			int i;

			try
			{
				outValueList = new ArrayList();
				outRefList = new ArrayList();

				if (aBalItemList.Count > 0)
				{
					sortList = new SortedList();

					foreach (SizeCurveBalanceItem balItem in aBalItemList)
					{
						sortList.Add(balItem, balItem);
					}

					sortOutList = new ArrayList();

					for (i = 0; i < sortList.Count; i++)
					{
						sortOutList.Add(sortList.GetByIndex(i));
					}

					_spreader.ExecuteSimpleSpread(100, sortOutList, null, 3, out outValueList, out outRefList);

					for (i = 0; i < outRefList.Count; i++)
					{
						((SizeCurveBalanceItem)outRefList[i]).Value = (double)outValueList[i];
					}
				}

				return outRefList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private StoreProfile GetStoreProfile(int aStoreRID)
		{
			StoreProfile storeProf;

			try
			{
				storeProf = (StoreProfile)_storeHash[aStoreRID];

				if (storeProf == null)
				{
                    storeProf = StoreMgmt.StoreProfile_Get(aStoreRID); // SAB.StoreServerSession.GetStoreProfile(aStoreRID);
					_storeHash[aStoreRID] = storeProf;
				}

				return storeProf;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private class MerchandiseBasisList : ArrayList
		{
			public ArrayList GetStoreList()
			{
				Hashtable storeHash;
				ArrayList storeList;

				try
				{
					storeHash = new Hashtable();

					foreach (MerchandiseBasisEntry entry in this)
					{
						storeList = entry.GetStoreList();

						foreach (int storeRID in storeList)
						{
							storeHash[storeRID] = null;
						}
					}

					return new ArrayList(storeHash.Keys);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class MerchandiseBasisEntry
		{
			private StoreSizeValueGroup _sizeValGroup;
			private SizeCurveMerchBasisProfile _szCrvMerchBasisProf;
			private decimal _weight;

			public SizeCurveMerchBasisProfile SizeCurveMerchBasisProf
			{
				get
				{
					return _szCrvMerchBasisProf;
				}
			}

			public decimal Weight
			{
				get
				{
					return _weight;
				}
				set
				{
					_weight = value;
				}
			}

			public decimal ChainSizeTotalValue
			{
				get
				{
					return _sizeValGroup.ChainSizeTotalValue;
				}
			}

			public MerchandiseBasisEntry(ApplicationSessionTransaction aAppTran, HierarchyNodeProfile aHierNodeProf, ProfileList aWeekList, SizeGroupProfile aSzGrpProf, SizeCurveMerchBasisProfile aSzCrvMerchBasisProf, DataTable aDtSizeValues)
			{
				int storeRID;
				int sizeCode;
				int sales;

				try
				{
					Common_Load(aAppTran, aHierNodeProf, aWeekList, aSzCrvMerchBasisProf);

					foreach (DataRow row in aDtSizeValues.Rows)
					{
						storeRID = Convert.ToInt32(row["ST_RID"]);
						sizeCode = Convert.ToInt32(row["SIZE_CODE_RID"]);
						sales = Convert.ToInt32(row["SALES"]);

						if (aSzGrpProf.SizeCodeList.Contains(sizeCode))
						{
							if (storeRID != Include.NoRID)
							{
								_sizeValGroup.Add(storeRID, sizeCode, sales);
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

			public MerchandiseBasisEntry(ApplicationSessionTransaction aAppTran, HierarchyNodeProfile aHierNodeProf, ProfileList aWeekList, SizeGroupProfile aSzGrpProf, SizeCurveMerchBasisProfile aSzCrvMerchBasisProf, List<StoreSizeValue> aSizeValueList)
			{
				int i;

				try
				{
					Common_Load(aAppTran, aHierNodeProf, aWeekList, aSzCrvMerchBasisProf);

					for (i = 0; i < aSizeValueList.Count; i++)
					{
						if (aSizeValueList[i].SalesValue != 0 && aSzGrpProf.SizeCodeList.Contains(aSizeValueList[i].SizeCodeRID))
						{
							if (aSizeValueList[i].StoreRID != Include.NoRID)
							{
								_sizeValGroup.Add(aSizeValueList[i].StoreRID, aSizeValueList[i].SizeCodeRID, aSizeValueList[i].SalesValue);
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

			private void Common_Load(ApplicationSessionTransaction aAppTran, HierarchyNodeProfile aHierNodeProf, ProfileList aWeekList, SizeCurveMerchBasisProfile aSzCrvMerchBasisProf)
			{
				ArrayList validStoreList;

				try
				{
					_szCrvMerchBasisProf = aSzCrvMerchBasisProf;
					_weight = aSzCrvMerchBasisProf.Basis_Weight;
					validStoreList = new ArrayList();

					foreach (StoreProfile storeProf in aAppTran.GetProfileList(eProfileType.Store))
					{
						foreach (WeekProfile weekProf in aWeekList)
						{
							if (aAppTran.GetStoreEligibilityForSales(
                                eRequestingApplication.Allocation, 
                                aHierNodeProf.Key, 
                                storeProf.Key, 
                                weekProf.Key
                                ))
							{
								validStoreList.Add(storeProf.Key);
								break;
							}
						}
					}

					_sizeValGroup = new StoreSizeValueGroup(validStoreList);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			public ArrayList GetStoreList()
			{
				try
				{
					return _sizeValGroup.GetStoreList();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			public Hashtable GetChainSizeValues()
			{
				try
				{
					return _sizeValGroup.GetChainSizeValues();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			public Hashtable GetAllStoreSizeValues()
			{
				try
				{
					return _sizeValGroup.GetAllStoreSizeValues();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			public Hashtable GetStoreSizeValues(int aStoreRID)
			{
				try
				{
					return _sizeValGroup.GetStoreSizeValues(aStoreRID);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			public decimal GetStoreSizeTotalValue(int aStoreRID)
			{
				try
				{
					return _sizeValGroup.GetStoreSizeTotalValue(aStoreRID);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			public void ReplaceStoreSizeValues(int aStoreRID, Hashtable aValueHash)
			{
				try
				{
					_sizeValGroup.ReplaceStoreSizeValues(aStoreRID, aValueHash);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}

			public void AdjustSimilarStoreSizeValue(int aStoreRID, Hashtable aOldStrSzVals, Hashtable aNewStrSzVals)
			{
				IDictionaryEnumerator iEnum;
				StoreSizeValue strSzVal;

				try
				{
					if (aOldStrSzVals != null)
					{
						iEnum = aOldStrSzVals.GetEnumerator();

						while (iEnum.MoveNext())
						{
							strSzVal = (StoreSizeValue)iEnum.Value;
							_sizeValGroup.Add(strSzVal.StoreRID, strSzVal.SizeCodeRID, strSzVal.SalesValue * -1);
						}
					}

					if (aNewStrSzVals != null)
					{
						iEnum = aNewStrSzVals.GetEnumerator();

						while (iEnum.MoveNext())
						{
							strSzVal = (StoreSizeValue)iEnum.Value;
							_sizeValGroup.Add(aStoreRID, strSzVal.SizeCodeRID, strSzVal.SalesValue);
						}
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class CurveBasisList : ArrayList
		{
			public ArrayList GetStoreList(SizeCurveGroupProfile aSzCrvGrpProf)
			{
				Hashtable storeHash;
				IDictionaryEnumerator iEnum;

				try
				{
					storeHash = new Hashtable();

					iEnum = aSzCrvGrpProf.StoreSizeCurveHash.GetEnumerator();

					while (iEnum.MoveNext())
					{
						storeHash[Convert.ToInt32(iEnum.Key)] = null;
					}

					foreach (CurveBasisEntry entry in this)
					{
						iEnum = entry.SizeCurveGroupProfile.StoreSizeCurveHash.GetEnumerator();

						while (iEnum.MoveNext())
						{
							storeHash[Convert.ToInt32(iEnum.Key)] = null;
						}
					}

					return new ArrayList(storeHash.Keys);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		private class CurveBasisEntry
		{
			SizeCurveCurveBasisProfile _szCrvCurveBasisProf;
			SizeCurveGroupProfile _szCrvGrpProf;
			decimal _weight;

			public SizeCurveCurveBasisProfile SizeCurveCurveBasisProf
			{
				get
				{
					return _szCrvCurveBasisProf;
				}
			}

			public SizeCurveGroupProfile SizeCurveGroupProfile
			{
				get
				{
					return _szCrvGrpProf;
				}
			}

			public decimal Weight
			{
				get
				{
					return _weight;
				}
				set
				{
					_weight = value;
				}
			}

			public CurveBasisEntry(SizeCurveCurveBasisProfile aSzCrvCurveBasisProf, SizeCurveGroupProfile aSzCrvGrpProf)
			{

				try
				{
					_szCrvCurveBasisProf = aSzCrvCurveBasisProf;
					_szCrvGrpProf = aSzCrvGrpProf;
					_weight = 1;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	}
}
