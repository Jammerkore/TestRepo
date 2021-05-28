using System;
using System.IO;
using System.Collections;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Reflection.Emit;

namespace MIDRetail.Business
{
	/// <summary>
	/// Used to hold the information for a single Method.
	/// </summary>\
	[Serializable()]
	public class GroupLevelFunctionProfile : Profile, IComparable , ICloneable
	{			
		//SGL_RID is key
		private bool _default_IND;
		private bool _plan_IND;
		private bool _use_default_IND;
		private bool _clear_IND;
		private bool _season_IND;
		private int	_season_HN_RID;
		private eGroupLevelFunctionType _glft_ID;
		private eGroupLevelSmoothBy _glsb_ID;
		private eChangeType _GLF_Change_Type;
		//private ProfileList _basis_Plan;
		//private ProfileList _basis_Range;
		private ProfileList _groupLevelBasis;
		private bool _filled;
		private bool _ly_alt_IND;
		private bool _trend_alt_IND;
		private bool _ty_Weight_Multiple_Basis_Ind; //Track #4817 - JBolles - Weighting Multiple Basis
		private bool _ly_Weight_Multiple_Basis_Ind;
		private bool _apply_Weight_Multiple_Basis_Ind;
		private ProfileList _trend_Caps;
		private Hashtable _group_Level_Nodes;
        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        private bool _Proj_Curr_Wk_Sales_Ind;
        //END TT#43 - MD - DOConnell - Projected Sales Enhancement

		public bool Default_IND 
		{
			get { return _default_IND ; }
			set { _default_IND = value; }
		}
		public bool Plan_IND 
		{
			get { return _plan_IND ; }
			set { _plan_IND = value; }
		}
		public bool Use_Default_IND 
		{
			get { return _use_default_IND ; }
			set { _use_default_IND = value; }
		}
		public bool Clear_IND
		{
			get { return _clear_IND ; }
			set { _clear_IND = value; }
		}
		public bool Season_IND 
		{
			get { return _season_IND ; }
			set { _season_IND = value; }
		}
		public int Season_HN_RID 
		{
			get { return _season_HN_RID ; }
			set { _season_HN_RID = value; }
		}
		public eGroupLevelFunctionType GLFT_ID 
		{
			get { return _glft_ID ; }
			set { _glft_ID = value; }
		}
		public eGroupLevelSmoothBy GLSB_ID 
		{
			get { return _glsb_ID ; }
			set { _glsb_ID = value; }
		}
		/// <summary>
		/// Gets or sets the type of change for the Method.
		/// </summary>
		public eChangeType GLF_Change_Type 
		{
			get { return _GLF_Change_Type ; }
			set { _GLF_Change_Type = value; }
		}

//		/// <summary>
//		/// Gets or sets the Basis Plan ProfileList for the Method.
//		/// </summary>
//		public ProfileList Basis_Plan 
//		{
//			get { return _basis_Plan ; }
//			set { _basis_Plan = value; }
//		}
//
//		/// <summary>
//		/// Gets or sets the Basis Range ProfileList for the Method.
//		/// </summary>
//		public ProfileList Basis_Range 
//		{
//		get { return _basis_Range ; }
//		set { _basis_Range = value; }
//		}

		public bool TY_Weight_Multiple_Basis_Ind
		{
			get {return _ty_Weight_Multiple_Basis_Ind;}
			set {_ty_Weight_Multiple_Basis_Ind = value;}
		}

		public bool LY_Weight_Multiple_Basis_Ind
		{
			get {return _ly_Weight_Multiple_Basis_Ind;}
			set {_ly_Weight_Multiple_Basis_Ind = value;}
		}

		public bool Apply_Weight_Multiple_Basis_Ind
		{
			get {return _apply_Weight_Multiple_Basis_Ind;}
			set {_apply_Weight_Multiple_Basis_Ind = value;}
		}

		/// <summary>
		/// Gets or sets the Basis Range ProfileList for the Method.
		/// </summary>
		public ProfileList GroupLevelBasis 
		{
			get { return _groupLevelBasis ; }
			set { _groupLevelBasis = value; }
		}

		/// <summary>
		/// Gets or sets the Trend Caps ProfileList for the Method.
		/// </summary>
		public ProfileList Trend_Caps 
		{
			get { return _trend_Caps ; }
			set { _trend_Caps = value; }
		}
		
		public Hashtable Group_Level_Nodes
		{
			get
			{ 
				if(_group_Level_Nodes == null)
					_group_Level_Nodes = new Hashtable();
				
				return _group_Level_Nodes;
			}
			set { _group_Level_Nodes = value; }
		}

		public bool Filled 
		{
			get { return _filled ; }
			set { _filled = value; }
		}

		public bool LY_Alt_IND
		{
			get { return _ly_alt_IND ; }
			set { _ly_alt_IND = value; }
		}

		public bool Trend_Alt_IND
		{
			get { return _trend_alt_IND ; }
			set { _trend_alt_IND = value; }
		}

        //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        public bool Proj_Curr_Wk_Sales_IND
        {
            get { return _Proj_Curr_Wk_Sales_Ind; }
            set { _Proj_Curr_Wk_Sales_Ind = value; }
        }
        //END TT#43 - MD - DOConnell - Projected Sales Enhancement

		public override int GetHashCode()
		{
			return this.Key;
		}

		/// <summary>
		/// overrided Equals
		/// </summary>
		/// <param name="obj">GroupLevelFunctionProfile</param>
		/// <returns>Bool</returns>
		public override bool Equals(Object obj) 
		{
			// Check for null values and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) 
				return false;

			return (this.Key == ((GroupLevelFunctionProfile)obj).Key);
		}

		public int CompareTo(object obj)
		{ 
			return Key - ((GroupLevelFunctionProfile)obj).Key; 
		} 
 
		public static bool operator<(GroupLevelFunctionProfile lhs, GroupLevelFunctionProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) < 0;
		}

		public static bool operator<=(GroupLevelFunctionProfile lhs, GroupLevelFunctionProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) <= 0;
		}

		public static bool operator>(GroupLevelFunctionProfile lhs, GroupLevelFunctionProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) > 0;
		}

		public static bool operator>=(GroupLevelFunctionProfile lhs, GroupLevelFunctionProfile rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) >= 0;
		}

        // Begin TT#2647 - JSmith - Delays in OTS Method
        /// <summary>
        /// Copies all values to the object provided
        /// </summary>
        /// <returns>
        /// The object with the values of this object.
        /// </returns>
        public GroupLevelFunctionProfile CopyTo(GroupLevelFunctionProfile glfp, Session aSession, bool blCopyKey, bool blCopyMinMax, 
            bool blCopyToNewMinMax = false)
        {
            try
            {
                int saveKey = glfp.Key;
                glfp.Default_IND = Default_IND;
                glfp.Plan_IND = Plan_IND;
                glfp.Use_Default_IND = Use_Default_IND;
                glfp.Clear_IND = Clear_IND;
                glfp.Season_IND = Season_IND;
                glfp.Season_HN_RID = Season_HN_RID;
                glfp.GLFT_ID = GLFT_ID;
                glfp.GLSB_ID = GLSB_ID;
                glfp.GLF_Change_Type = GLF_Change_Type;
                glfp.TY_Weight_Multiple_Basis_Ind = TY_Weight_Multiple_Basis_Ind;
                glfp.LY_Weight_Multiple_Basis_Ind = LY_Weight_Multiple_Basis_Ind;
                glfp.Apply_Weight_Multiple_Basis_Ind = Apply_Weight_Multiple_Basis_Ind;
                glfp.Proj_Curr_Wk_Sales_IND = Proj_Curr_Wk_Sales_IND;
                glfp.LY_Alt_IND = LY_Alt_IND;
                glfp.Trend_Alt_IND = Trend_Alt_IND;

                glfp.GroupLevelBasis = new ProfileList(eProfileType.GroupLevelBasis);
                foreach (GroupLevelBasisProfile glbp in GroupLevelBasis)
                {
                    glfp.GroupLevelBasis.Add(glbp.Copy());
                }

                glfp.Trend_Caps = new ProfileList(eProfileType.TrendCaps);
                foreach (TrendCapsProfile tcp in Trend_Caps)
                {
                    glfp.Trend_Caps.Add(tcp.Copy());
                }

                ArrayList alNodes = new ArrayList();

                foreach (GroupLevelNodeFunction glnf in glfp.Group_Level_Nodes.Values)
                {
                    if (glnf.MinMaxInheritType == eMinMaxInheritType.Default)
                    {
                        if (Group_Level_Nodes.ContainsKey(glnf.HN_RID))
                        {
                            GroupLevelNodeFunction default_glnf = ((GroupLevelNodeFunction)Group_Level_Nodes[glnf.HN_RID]).Copy();
                            CreateNewStockMinMaxProfiles(
                                    groupLevelNodeFunction: default_glnf,
                                    aSession: aSession
                                    );
                            default_glnf.MinMaxInheritType = eMinMaxInheritType.Default;
                            alNodes.Add(default_glnf);
                        }
                    }
                    else if (blCopyToNewMinMax)
                    {
                        GroupLevelNodeFunction default_glnf = glnf.Copy();
                        CreateNewStockMinMaxProfiles(
                                groupLevelNodeFunction: default_glnf,
                                aSession: aSession
                                );
                        alNodes.Add(default_glnf);
                    }
                    else
                    {
                        alNodes.Add(glnf);
                    }
                    
                }

                glfp.Group_Level_Nodes = new Hashtable();
                foreach (GroupLevelNodeFunction glnf in alNodes)
                {
                    glfp.Group_Level_Nodes.Add(glnf.HN_RID, glnf);
                }

                if (!blCopyKey)
                {
                    glfp.Key = saveKey;
                }

                return glfp;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#2647 - JSmith - Delays in OTS Method

        private void CreateNewStockMinMaxProfiles(
            GroupLevelNodeFunction groupLevelNodeFunction,
            Session aSession
            )
        {
            ArrayList stockMinMaxProfiles = new ArrayList();
            foreach (StockMinMaxProfile stockMinMaxProfile in groupLevelNodeFunction.Stock_MinMax)
            {
                stockMinMaxProfiles.Add(stockMinMaxProfile.Copy(aSession, true));
            }
            groupLevelNodeFunction.Stock_MinMax.Clear();
            foreach (StockMinMaxProfile stockMinMaxProfile in stockMinMaxProfiles)
            {
                groupLevelNodeFunction.Stock_MinMax.Add(stockMinMaxProfile);
            }
        }

		public object Clone()
		{
			return this;
		}  


		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="aKey">int</param>
		public GroupLevelFunctionProfile(int aKey)
			: base(aKey)
		{
			//Profile TrendCaps
			//ProfileList BasisPlan
			//ProfileList BasisRange
			//ProfileList StockMinMax
			//_basis_Plan = new ProfileList(eProfileType.BasisPlan);
			//_basis_Range = new ProfileList(eProfileType.BasisRange);
			_groupLevelBasis = new ProfileList(eProfileType.GroupLevelBasis);
			_trend_Caps = new ProfileList(eProfileType.TrendCaps);
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.GroupLevelFunction;
			}
		}

		/// <summary>
		/// Unloads MethodProfile in to field by field object array.
		/// </summary>
		/// <returns>Object array</returns>
		public object [] ItemArray()
		{
			object [] ar = new object[11];
			ar[0] = this.Key;
			ar[1] = Include.ConvertBoolToChar(this.Default_IND);
			ar[2] = Include.ConvertBoolToChar(this.Plan_IND);
			ar[3] = Include.ConvertBoolToChar(this.Use_Default_IND);
			ar[4] = Include.ConvertBoolToChar(this.Clear_IND);
			ar[5] = Include.ConvertBoolToChar(this.Season_IND);
			ar[6] = this.Season_HN_RID;
			ar[7] = Convert.ToInt32(this.GLFT_ID, CultureInfo.CurrentUICulture);
			ar[8] = Convert.ToInt32(this.GLSB_ID, CultureInfo.CurrentUICulture);
			ar[9] = Include.ConvertBoolToChar(this.LY_Alt_IND);
			ar[10] = Include.ConvertBoolToChar(this.Trend_Alt_IND);
			ar[11] = Include.ConvertBoolToChar(this.TY_Weight_Multiple_Basis_Ind);
			ar[12] = Include.ConvertBoolToChar(this.LY_Weight_Multiple_Basis_Ind);
			ar[13] = Include.ConvertBoolToChar(this.Apply_Weight_Multiple_Basis_Ind);
            //BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
            ar[14] = Include.ConvertBoolToChar(this.Proj_Curr_Wk_Sales_IND);
            //END TT#43 - MD - DOConnell - Projected Sales Enhancement
			return ar;
		}
	}

	/// <summary>
	/// Used to hold the information for a group level basis for a method.
	/// </summary>
	/// <remarks>
	/// Profile entry is keyed by the sequence of the entry.
	/// </remarks>
	[Serializable()]
	public class GroupLevelBasisProfile : Profile
	{			
		//BASIS_SEQ is key
		private int	_basis_HN_RID;
		private int	_basis_FV_RID;
		private int	_basis_CDR_RID;
		private double _basis_Weight;
		private bool _basisExcludeInd;
		private eTyLyType _basis_TyLyType;
		private eMerchandiseType _merchType = eMerchandiseType.Node;
		private int _merchPhRid;
		private int _merchPhlSequence;
		private int _merchOffset;
		
		public int Basis_HN_RID 
		{
			get { return _basis_HN_RID ; }
			set { _basis_HN_RID = value; }
		}
		public int Basis_FV_RID 
		{
			get { return _basis_FV_RID ; }
			set { _basis_FV_RID = value; }
		}
		public int Basis_CDR_RID 
		{
			get { return _basis_CDR_RID ; }
			set { _basis_CDR_RID = value; }
		}
		public double Basis_Weight 
		{
			get { return _basis_Weight ; }
			set { _basis_Weight = value; }
		}
		public bool Basis_ExcludeInd 
		{
			get { return _basisExcludeInd ; }
			set { _basisExcludeInd = value; }
		}
		public eTyLyType Basis_TyLyType
		{
			get { return _basis_TyLyType ; }
			set { _basis_TyLyType = value; }
		}
		public eMerchandiseType MerchType 
		{
			get { return _merchType ; }
			set { _merchType = value; }
		}
		public int MerchPhRid 
		{
			get { return _merchPhRid ; }
			set { _merchPhRid = value; }
		}
		public int MerchPhlSequence 
		{
			get { return _merchPhlSequence ; }
			set { _merchPhlSequence = value; }
		}
		public int MerchOffset 
		{
			get { return _merchOffset ; }
			set { _merchOffset = value; }
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="aKey">int</param>
		public GroupLevelBasisProfile(int aKey)
			: base(aKey)
		{
			
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.GroupLevelBasis;
			}
		}

		/// <summary>
		/// Returns a copy of this object.
		/// </summary>
		/// <returns>
		/// A copy of the object.
		/// </returns>
		public GroupLevelBasisProfile Copy()
		{
			try
			{
                // Begin TT#2647 - JSmith - Delays in OTS Method
                //GroupLevelBasisProfile glbp = (GroupLevelBasisProfile)this.MemberwiseClone();
                //glbp.Key = Key;
                GroupLevelBasisProfile glbp = new GroupLevelBasisProfile(Key);
                // End TT#2647 - JSmith - Delays in OTS Method
				glbp.Basis_HN_RID = Basis_HN_RID;
				glbp.Basis_FV_RID = Basis_FV_RID;
				glbp._basis_CDR_RID = Basis_CDR_RID;
				glbp._basis_Weight = Basis_Weight;
				glbp._basisExcludeInd = Basis_ExcludeInd;
				glbp._basis_TyLyType = Basis_TyLyType;
				glbp._merchType = MerchType;
				glbp._merchPhRid = MerchPhRid;
				glbp._merchPhlSequence = MerchPhlSequence;
				glbp._merchOffset = MerchOffset;
				return glbp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}


	//////////////////////////
	/// <summary>
	/// Used to hold the information for trend caps for a method.
	/// </summary>
	[Serializable()]
	public class TrendCapsProfile : Profile
	{			
		//SGL_RID is key
		private eTrendCapID	_trendCapID;
		private double _tolPct;
		private double _highLimit;
		private double _lowLimit;
		
		public eTrendCapID TrendCapID
		{
			get { return _trendCapID; }
			set { _trendCapID = value; }
		}
		public double TolPct 
		{
			get { return _tolPct ; }
			set { _tolPct = value; }
		}

		public double HighLimit
		{
			get { return _highLimit; }
			set { _highLimit = value; }
		} 
		public double LowLimit
		{
			get { return _lowLimit; }
			set { _lowLimit = value; }
		} 
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="aKey">int</param>
		public TrendCapsProfile(int aKey)
			: base(aKey)
		{
			
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.TrendCaps;
			}
		}

		/// <summary>
		/// Returns a copy of this object.
		/// </summary>
		/// <returns>
		/// A copy of the object.
		/// </returns>
		public TrendCapsProfile Copy()
		{
			try
			{
				TrendCapsProfile tcp = (TrendCapsProfile)this.MemberwiseClone();
				tcp.Key = Key;
				tcp.TrendCapID = TrendCapID;
				tcp.TolPct = TolPct;
				tcp.HighLimit = HighLimit;	
				tcp.LowLimit = LowLimit;	
				return tcp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	[Serializable()]
	public class StockMinMaxProfile : Profile
	{			
		// key is unique identifier
		private int	_methodRid;
		private int _sgl_rid;
		private int _boundary;
		private int _hn_rid;
		private int _cdr_rid;
		private int _minStock;
		private int _maxStock;
		
		public int MethodRid
		{
			get { return _methodRid; }
			set { _methodRid = value; }
		}
		public int StoreGroupLevelRid 
		{
			get { return _sgl_rid ; }
			set { _sgl_rid = value; }
		}
		public int Boundary 
		{
			get { return _boundary ; }
			set { _boundary = value; }
		}
		public int HN_RID
		{
			get { return _hn_rid; }
			set { _hn_rid = value; }
		}
		public int DateRangeRid
		{
			get { return _cdr_rid; }
			set { _cdr_rid = value; }
		} 
		public int MinimumStock
		{
			get { return _minStock; }
			set { _minStock = value; }
		} 
		public int MaximumStock
		{
			get { return _maxStock; }
			set { _maxStock = value; }
		} 
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="aKey">int</param>
		public StockMinMaxProfile(int aKey)
			: base(aKey)
		{
			
		}

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.StockMinMax;
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
		/// <returns>
		/// A copy of the object.
		/// </returns>
		public StockMinMaxProfile Copy(Session aSession, bool aCloneDateRanges)
		{
			try
			{
                // Begin TT#2647 - JSmith - Delays in OTS Method
                //StockMinMaxProfile smmp = (StockMinMaxProfile)this.MemberwiseClone();
                StockMinMaxProfile smmp = new StockMinMaxProfile(Key);
                smmp.DateRangeRid = DateRangeRid;
                // End TT#2647 - JSmith - Delays in OTS Method
				smmp.MethodRid = MethodRid;
				smmp.StoreGroupLevelRid = StoreGroupLevelRid;
				smmp.HN_RID = HN_RID;
				if (aCloneDateRanges &&
					smmp.DateRangeRid != Include.UndefinedCalendarDateRange)
				{
					smmp.DateRangeRid = aSession.Calendar.GetDateRangeClone(DateRangeRid).Key;;
				}
				else
				{
					smmp.DateRangeRid = DateRangeRid;
				}
				smmp.Boundary = Boundary;
				smmp.MinimumStock = MinimumStock;
				smmp.MaximumStock = MaximumStock;	
				return smmp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
