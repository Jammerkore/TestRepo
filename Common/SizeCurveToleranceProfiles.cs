using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the size curve tolerance for a store for a node in a hierarchy
	/// </summary>
	[Serializable()]
	public class SizeCurveToleranceProfile : Profile
	{
		// Fields

		private eChangeType _toleranceChangeType;
		private bool _recordExists;

		private bool _toleranceMinAvgIsInherited;
		private int _toleranceMinAvgInheritedFromNodeRID;
		private double _toleranceMinAvg;
		private bool _toleranceLevelIsInherited;
		private int _toleranceLevelInheritedFromNodeRID;
		private eLowLevelsType _toleranceLevelType;
		private int _toleranceLevelRID;
		private int _toleranceLevelSeq;
		private int _toleranceLevelOffset;
		private bool _toleranceSalesTolerIsInherited;
		private int _toleranceSalesTolerInheritedFromNodeRID;
		private double _toleranceSalesToler;
		private bool _toleranceIdxUnitsIndIsInherited;
		private int _toleranceIdxUnitsIndInheritedFromNodeRID;
		private eNodeChainSalesType _toleranceIdxUnitsInd;
		private bool _toleranceMinTolerIsInherited;
		private int _toleranceMinTolerInheritedFromNodeRID;
		private double _toleranceMinToler;
		private bool _toleranceMaxTolerIsInherited;
		private int _toleranceMaxTolerInheritedFromNodeRID;
		private double _toleranceMaxToler;
        //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        private bool _applyMinToZeroTolerance;
        //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SizeCurveToleranceProfile()
			: base(0)
		{
			_toleranceChangeType = eChangeType.none;
			_recordExists = false;

			_toleranceMinAvgIsInherited = false;
			_toleranceMinAvgInheritedFromNodeRID = Include.NoRID;
			_toleranceMinAvg = Include.Undefined;
			_toleranceLevelIsInherited = false;
			_toleranceLevelInheritedFromNodeRID = Include.NoRID;
			_toleranceLevelType = eLowLevelsType.None;
			_toleranceLevelRID = Include.NoRID;
			_toleranceLevelSeq = Include.Undefined;
			_toleranceLevelOffset = Include.Undefined;
			_toleranceSalesTolerIsInherited = false;
			_toleranceSalesTolerInheritedFromNodeRID = Include.NoRID;
			_toleranceSalesToler = Include.Undefined;
			_toleranceIdxUnitsIndIsInherited = false;
			_toleranceIdxUnitsIndInheritedFromNodeRID = Include.NoRID;
			_toleranceIdxUnitsInd = eNodeChainSalesType.None;
			_toleranceMinTolerIsInherited = false;
			_toleranceMinTolerInheritedFromNodeRID = Include.NoRID;
			_toleranceMinToler = Include.Undefined;
			_toleranceMaxTolerIsInherited = false;
			_toleranceMaxTolerInheritedFromNodeRID = Include.NoRID;
			_toleranceMaxToler = Include.Undefined;
            //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
            _applyMinToZeroTolerance = false;
		    //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SizeCurveTolerance;
			}
		}

		/// <summary>
		/// Gets or sets the change type of a size curve tolerance.
		/// </summary>
		public eChangeType ToleranceChangeType
		{
			get { return _toleranceChangeType; }
			set { _toleranceChangeType = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if a size curve tolerance information is inherited.
		/// </summary>
		public bool RecordExists
		{
			get { return _recordExists; }
			set { _recordExists = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if the minimum average is inherited.
		/// </summary>
		public bool ToleranceMinAvgIsInherited
		{
			get { return _toleranceMinAvgIsInherited; }
			set { _toleranceMinAvgIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the Node where the minimum average is inherited.
		/// </summary>
		public int ToleranceMinAvgInheritedFromNodeRID
		{
			get { return _toleranceMinAvgInheritedFromNodeRID; }
			set { _toleranceMinAvgInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the minimum average value.
		/// </summary>
		public double ToleranceMinAvg
		{
			get { return _toleranceMinAvg; }
			set { _toleranceMinAvg = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if the level type is inherited.
		/// </summary>
		public bool ToleranceLevelIsInherited
		{
			get { return _toleranceLevelIsInherited; }
			set { _toleranceLevelIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the Node where the level type is inherited.
		/// </summary>
		public int ToleranceLevelInheritedFromNodeRID
		{
			get { return _toleranceLevelInheritedFromNodeRID; }
			set { _toleranceLevelInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the level type value.
		/// </summary>
		public eLowLevelsType ToleranceLevelType
		{
			get { return _toleranceLevelType; }
			set { _toleranceLevelType = value; }
		}
		/// <summary>
		/// Gets or sets the level RID value.
		/// </summary>
		public int ToleranceLevelRID
		{
			get { return _toleranceLevelRID; }
			set { _toleranceLevelRID = value; }
		}
		/// <summary>
		/// Gets or sets the level sequence value.
		/// </summary>
		public int ToleranceLevelSeq
		{
			get { return _toleranceLevelSeq; }
			set { _toleranceLevelSeq = value; }
		}
		/// <summary>
		/// Gets or sets the level offset value.
		/// </summary>
		public int ToleranceLevelOffset
		{
			get { return _toleranceLevelOffset; }
			set { _toleranceLevelOffset = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if the sales tolerance is inherited.
		/// </summary>
		public bool ToleranceSalesToleranceIsInherited
		{
			get { return _toleranceSalesTolerIsInherited; }
			set { _toleranceSalesTolerIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the Node where the sales tolerance is inherited.
		/// </summary>
		public int ToleranceSalesToleranceInheritedFromNodeRID
		{
			get { return _toleranceSalesTolerInheritedFromNodeRID; }
			set { _toleranceSalesTolerInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the sales tolerance value.
		/// </summary>
		public double ToleranceSalesTolerance
		{
			get { return _toleranceSalesToler; }
			set { _toleranceSalesToler = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if the index units indicator is inherited.
		/// </summary>
		public bool ToleranceIdxUnitsIndIsInherited
		{
			get { return _toleranceIdxUnitsIndIsInherited; }
			set { _toleranceIdxUnitsIndIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the Node where the index units indicator is inherited.
		/// </summary>
		public int ToleranceIdxUnitsIndInheritedFromNodeRID
		{
			get { return _toleranceIdxUnitsIndInheritedFromNodeRID; }
			set { _toleranceIdxUnitsIndInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the index units indicator value.
		/// </summary>
		public eNodeChainSalesType ToleranceIdxUnitsInd
		{
			get { return _toleranceIdxUnitsInd; }
			set { _toleranceIdxUnitsInd = value; }
		}
		/// <summary>
		/// Gets or sets a flag identifying if the minimum tolerance is inherited.
		/// </summary>
		public bool ToleranceMinToleranceIsInherited
		{
			get { return _toleranceMinTolerIsInherited; }
			set { _toleranceMinTolerIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the Node where the minimum tolerance is inherited.
		/// </summary>
		public int ToleranceMinToleranceInheritedFromNodeRID
		{
			get { return _toleranceMinTolerInheritedFromNodeRID; }
			set { _toleranceMinTolerInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the minimum tolerance value.
		/// </summary>
		public double ToleranceMinTolerance
		{
			get { return _toleranceMinToler; }
			set { _toleranceMinToler = value; }
		}
        //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
        public bool ApplyMinToZeroTolerance
        {
            get { return _applyMinToZeroTolerance; }
            set { _applyMinToZeroTolerance = value; }
        }
        //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
		/// <summary>
		/// Gets or sets a flag identifying if the maximum tolerance is inherited.
		/// </summary>
		public bool ToleranceMaxToleranceIsInherited
		{
			get { return _toleranceMaxTolerIsInherited; }
			set { _toleranceMaxTolerIsInherited = value; }
		}
		/// <summary>
		/// Gets or sets the Node where the maximum tolerance is inherited.
		/// </summary>
		public int ToleranceMaxToleranceInheritedFromNodeRID
		{
			get { return _toleranceMaxTolerInheritedFromNodeRID; }
			set { _toleranceMaxTolerInheritedFromNodeRID = value; }
		}
		/// <summary>
		/// Gets or sets the maximum tolerance value.
		/// </summary>
		public double ToleranceMaxTolerance
		{
			get { return _toleranceMaxToler; }
			set { _toleranceMaxToler = value; }
		}
	}
}
