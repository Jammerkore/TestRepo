using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the size curve criteria for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class SizeCurveCriteriaProfile : Profile
	{
		// Fields

		private eChangeType _criteriaChangeType;
		private bool _recordExists;

		private bool _criteriaIsInherited;
		private int _criteriaInheritedFromNodeRID;
		private int _criteriaSequence;
		private eLowLevelsType _criteriaLevelType;
		private int _criteriaLevelRID;
		private int _criteriaLevelSequence;
		private int _criteriaLevelOffset;
		private int _criteriaDateRID;
		private bool _criteriaApplyLostSalesInd;
		private int _criteriaOLLRID;
		private int _criteriaCustomOLLRID;
		private int _criteriaSizeGroupRID;
		private string _criteriaCurveName;
		//Begin TT#1076 - JScott - Size Curves by Set
		private int _criteriaSgRID;
		//End TT#1076 - JScott - Size Curves by Set

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCurveCriteriaProfile(int aKey)
			: base(aKey)
		{
			_criteriaChangeType = eChangeType.none;
			_recordExists = false;

			_criteriaIsInherited = false;
			_criteriaInheritedFromNodeRID = Include.NoRID;
			_criteriaSequence = Include.Undefined;
			_criteriaLevelType = eLowLevelsType.None;
			_criteriaLevelRID = Include.NoRID;
			_criteriaLevelSequence = Include.Undefined;
			_criteriaLevelOffset = Include.Undefined;
			_criteriaDateRID = Include.NoRID;
			_criteriaApplyLostSalesInd = true;
			_criteriaOLLRID = Include.NoRID;
			_criteriaCustomOLLRID = Include.NoRID;
			_criteriaSizeGroupRID = Include.NoRID;
			_criteriaCurveName = string.Empty;
			//Begin TT#1076 - JScott - Size Curves by Set
			_criteriaSgRID = Include.NoRID;
			//End TT#1076 - JScott - Size Curves by Set
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeCurveCriteria;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a size curve criteria.
		/// </summary>
		public eChangeType CriteriaChangeType
		{
			get { return _criteriaChangeType; }
			set { _criteriaChangeType = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a size curve criteria information is inherited.
		/// </summary>
		public bool RecordExists
		{
			get { return _recordExists; }
			set { _recordExists = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a size curve criteria information is inherited.
		/// </summary>
		public bool CriteriaIsInherited
		{
			get { return _criteriaIsInherited; }
			set { _criteriaIsInherited = value; }
		}
		/// <summary>
		/// The Node where the size curve information is inherited.
		/// </summary>
		public int CriteriaInheritedFromNodeRID
		{
			get { return _criteriaInheritedFromNodeRID; }
			set { _criteriaInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria sequence value.
		/// </summary>
		public int CriteriaSequence
		{
			get { return _criteriaSequence; }
			set { _criteriaSequence = value; }
		}
		/// <summary>
		/// Gets or sets the criteria level type value.
		/// </summary>
		public eLowLevelsType CriteriaLevelType
		{
			get { return _criteriaLevelType; }
			set { _criteriaLevelType = value; }
		}
		/// <summary>
		/// Gets or sets the criteria level value.
		/// </summary>
		public int CriteriaLevelRID
		{
			get { return _criteriaLevelRID; }
			set { _criteriaLevelRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria level sequence value.
		/// </summary>
		public int CriteriaLevelSequence
		{
			get { return _criteriaLevelSequence; }
			set { _criteriaLevelSequence = value; }
		}
		/// <summary>
		/// Gets or sets the criteria level offset value.
		/// </summary>
		public int CriteriaLevelOffset
		{
			get { return _criteriaLevelOffset; }
			set { _criteriaLevelOffset = value; }
		}
		/// <summary>
		/// Gets or sets the criteria date value.
		/// </summary>
		public int CriteriaDateRID
		{
			get { return _criteriaDateRID; }
			set { _criteriaDateRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria apply lost sales indicator.
		/// </summary>
		public bool CriteriaApplyLostSalesInd
		{
			get { return _criteriaApplyLostSalesInd; }
			set { _criteriaApplyLostSalesInd = value; }
		}
		/// <summary>
		/// Gets or sets the criteria override low-level value.
		/// </summary>
		public int CriteriaOLLRID
		{
			get { return _criteriaOLLRID; }
			set { _criteriaOLLRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria custom override low-level value.
		/// </summary>
		public int CriteriaCustomOLLRID
		{
			get { return _criteriaCustomOLLRID; }
			set { _criteriaCustomOLLRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria size group value.
		/// </summary>
		public int CriteriaSizeGroupRID
		{
			get { return _criteriaSizeGroupRID; }
			set { _criteriaSizeGroupRID = value; }
		}
		/// <summary>
		/// Gets or sets the criteria curve name value.
		/// </summary>
		public string CriteriaCurveName
		{
			get { return _criteriaCurveName; }
			set { _criteriaCurveName = value; }
		}
		//Begin TT#1076 - JScott - Size Curves by Set
		/// <summary>
		/// Gets or sets the criteria curve name value.
		/// </summary>
		public int CriteriaSgRID
		{
			get { return _criteriaSgRID; }
			set { _criteriaSgRID = value; }
		}
		//End TT#1076 - JScott - Size Curves by Set
		//Begin TT#417 - JScott - Size Curve - Generated a Size Curve the name is too long to read in Admini>Size Curve.  Need a Tool Tip so user can read the name of the Curve created.
		////Begin TT#438 - JScott - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)

		//public string GetCurveName(HierarchyNodeProfile aNodeProf)
		//{
		//    string name;

		//    try
		//    {
		//        name = aNodeProf.NodeID;

		//        if (_criteriaCurveName.Length > 0)
		//        {
		//            name += "-" + _criteriaCurveName;
		//        }

		//        return name;
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}
		////End TT#438 - JScott - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
		//End TT#417 - JScott - Size Curve - Generated a Size Curve the name is too long to read in Admini>Size Curve.  Need a Tool Tip so user can read the name of the Curve created.
	}

	/// <summary>
	/// Used to retrieve a list of eligibility information
	/// </summary>
	[Serializable()]
	public class SizeCurveCriteriaList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCurveCriteriaList()
			: base(eProfileType.SizeCurveCriteria)
		{
		}
	}

	/// <summary>
	/// Contains the information about the size curve criteria for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class SizeCurveDefaultCriteriaProfile : Profile
	{
		// Fields

		private eChangeType _defaultChangeType;
		private bool _recordExists;

		private bool _defaultRIDIsInherited;
		private int _defaultRIDIsInheritedFromRID;
		private int _defaultRID;

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCurveDefaultCriteriaProfile()
			: base(0)
		{
			_defaultChangeType = eChangeType.none;
			_recordExists = false;

			_defaultRIDIsInherited = false;
			_defaultRIDIsInheritedFromRID = Include.NoRID;
			_defaultRID = Include.NoRID;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeCurveDefaultCriteria;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a size curve criteria.
		/// </summary>
		public eChangeType DefaultChangeType
		{
			get { return _defaultChangeType; }
			set { _defaultChangeType = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a size curve criteria information is inherited.
		/// </summary>
		public bool DefaultRIDIsInherited
		{
			get { return _defaultRIDIsInherited; }
			set { _defaultRIDIsInherited = value; }
		}
		/// <summary>
		/// The Node where the size curve information is inherited.
		/// </summary>
		public int DefaultRIDIsInheritedFromRID
		{
			get { return _defaultRIDIsInheritedFromRID; }
			set { _defaultRIDIsInheritedFromRID = value; }
		}
		/// <summary>
		/// The Node where the size curve information is inherited.
		/// </summary>
		public int DefaultRID
		{
			get { return _defaultRID; }
			set { _defaultRID = value; }
		}
	}
}
