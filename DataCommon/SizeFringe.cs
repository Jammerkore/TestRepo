// begin MID Track 3619 Remove Fringe
//using System;
//using System.Collections;
//
//namespace MIDRetail.DataCommon
//{
//	#region SizeFringe
//	/// <summary>
//	/// Summary description for SizeFringe.
//	/// Used in the SizeFringeModelProfile.
//	/// </summary>
//	public class SizeFringe
//	{
//		private int _sizeTypeRid;
//		private bool _fringeInd;
//		private eEquateOverrideSizeType _sizeType;
//		private bool _used;
//
//		public int SizeTypeRid
//		{
//			get {return _sizeTypeRid;}
//			set {_sizeTypeRid = value;}
//		}
//		/// <summary>
//		/// True equals FRINGE, while false equals NEVER FRINGE.
//		/// </summary>
//		public bool FringeInd
//		{
//			get {return _fringeInd;}
//			set {_fringeInd = value;}
//		}
//		public eEquateOverrideSizeType SizeType
//		{
//			get {return _sizeType;}
//			set {_sizeType = value;}
//		}
//		public bool Used
//		{
//			get {return _used;}
//			set {_used = value;}
//		}
//
//		public SizeFringe()
//		{
//			_sizeTypeRid = Include.NoRID;
//			_fringeInd = false;
//			_sizeType = eEquateOverrideSizeType.DimensionSize;
//			_used = false;
//		}
//
//		public SizeFringe(int aSizeTypeRid, bool aFringeInd,  eEquateOverrideSizeType aSizeType)
//		{
//			_sizeTypeRid = aSizeTypeRid;
//			_fringeInd = aFringeInd;
//			_sizeType = aSizeType;
//			_used = false;
//		}
//	}
//	#endregion
//
//	#region SizeFringeFilter
//	/// <summary>
//	/// Summary description for SizeFringeFilter.
//	/// Used in the SizeFringeModelProfile.
//	/// </summary>
//	public class SizeFringeFilter
//	{
//		private eFringeOverrideUnitCriteria _fringeCriteria;
//		private eFringeOverrideCondition _fringeCondition;
//		private decimal _fringeValue;
//		private eFringeFilterValueType _valueType;
//
//		public eFringeOverrideUnitCriteria Criteria
//		{
//			get {return _fringeCriteria;}
//			set {_fringeCriteria = value;}
//		}
//		public eFringeOverrideCondition Condition
//		{
//			get {return _fringeCondition;}
//			set {_fringeCondition = value;}
//		}
//		public decimal Value
//		{
//			get {return _fringeValue;}
//			set {_fringeValue = value;}
//		}
//		public eFringeFilterValueType ValueType
//		{
//			get {return _valueType;}
//			set {_valueType = value;}
//		}
//
//		public SizeFringeFilter(eFringeOverrideUnitCriteria criteria, eFringeOverrideCondition condition,
//			decimal aValue, eFringeFilterValueType valueType)
//		{
//			_fringeCriteria = criteria;
//			_fringeCondition = condition;
//			_fringeValue = aValue;
//			_valueType = valueType;
//		}
//
//		public SizeFringeFilter()
//		{
//		}
//	}
//	#endregion
//}
// end MID Track 3619 Remove Fringe
