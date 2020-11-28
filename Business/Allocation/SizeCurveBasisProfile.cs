using System;
using System.Collections.Generic;
using System.Text;

using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using System.Reflection.Emit;

namespace MIDRetail.Business.Allocation
{
    /// <summary>
    /// Used to hold the information for a size curve basis for a method.
    /// </summary>
    /// <remarks>
    /// Profile entry is keyed by the sequence of the entry.
    /// </remarks>
    [Serializable()]
    public class SizeCurveMerchBasisProfile : Profile
    {
        //BASIS_SEQ is key
        private int _basis_HN_RID;
		private int _basis_FV_RID;
		private int _basis_CDR_RID;
        private decimal _basis_Weight;
        private eMerchandiseType _merchType = eMerchandiseType.Node;
        private int _overrideLLRid;
		private int _customOverrideLLRid;

        public int Basis_HN_RID
        {
            get { return _basis_HN_RID; }
            set { _basis_HN_RID = value; }
        }
		public int Basis_FV_RID
		{
			get { return _basis_FV_RID; }
			set { _basis_FV_RID = value; }
		}
		public int Basis_CDR_RID
        {
            get { return _basis_CDR_RID; }
            set { _basis_CDR_RID = value; }
        }
        public decimal Basis_Weight
        {
            get { return _basis_Weight; }
            set { _basis_Weight = value; }
        }
        public eMerchandiseType MerchType
        {
            get { return _merchType; }
            set { _merchType = value; }
        }
		public int OverrideLowLevelRid
		{
			get { return _overrideLLRid; }
			set { _overrideLLRid = value; }
		}
		public int CustomOverrideLowLevelRid
		{
			get { return _customOverrideLLRid; }
			set { _customOverrideLLRid = value; }
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aKey">int</param>
		public SizeCurveMerchBasisProfile(int aKey)
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
                return eProfileType.SizeCurveMerchBasis;
            }
        }

		/// <summary>
        /// Returns a copy of this object.
        /// </summary>
        /// <returns>
        /// A copy of the object.
        /// </returns>
        public SizeCurveMerchBasisProfile Copy()
        {
            try
            {
				SizeCurveMerchBasisProfile scbp = (SizeCurveMerchBasisProfile)this.MemberwiseClone();
                scbp.Key = Key;
                scbp.Basis_HN_RID = Basis_HN_RID;
				scbp.Basis_FV_RID = Basis_FV_RID;
				scbp._basis_CDR_RID = Basis_CDR_RID;
                scbp._basis_Weight = Basis_Weight;
                scbp._merchType = MerchType;
				scbp._overrideLLRid = OverrideLowLevelRid;
				scbp._customOverrideLLRid = CustomOverrideLowLevelRid;
				//scbp._lowlevelOverrideList = (LowLevelVersionOverrideProfileList)LowlevelOverrideList.Clone();
				return scbp;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

	/// <summary>
	/// Used to hold the information for a size curve basis for a method.
	/// </summary>
	/// <remarks>
	/// Profile entry is keyed by the sequence of the entry.
	/// </remarks>
	[Serializable()]
	public class SizeCurveCurveBasisProfile : Profile
	{
		//BASIS_SEQ is key
		private int _basis_SizeCurveGroupRID;
		private decimal _basis_Weight;

		public int Basis_SizeCurveGroupRID
		{
			get { return _basis_SizeCurveGroupRID; }
			set { _basis_SizeCurveGroupRID = value; }
		}
		public decimal Basis_Weight
		{
			get { return _basis_Weight; }
			set { _basis_Weight = value; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="aKey">int</param>
		public SizeCurveCurveBasisProfile(int aKey)
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
				return eProfileType.SizeCurveCurveBasis;
			}
		}

		/// <summary>
		/// Returns a copy of this object.
		/// </summary>
		/// <returns>
		/// A copy of the object.
		/// </returns>
		public SizeCurveCurveBasisProfile Copy()
		{
			try
			{
				SizeCurveCurveBasisProfile scbp = (SizeCurveCurveBasisProfile)this.MemberwiseClone();
				scbp.Key = Key;
				scbp._basis_SizeCurveGroupRID = Basis_SizeCurveGroupRID;
				scbp._basis_Weight = Basis_Weight;
				return scbp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
    /// Used to retrieve a list of hierarchy profiles
    /// </summary>
    [Serializable()]
    public class SizeCurveBasisProfileList : ProfileList
    {
        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public SizeCurveBasisProfileList(eProfileType aProfileType)
            : base(aProfileType)
        {

        }
    }
}
