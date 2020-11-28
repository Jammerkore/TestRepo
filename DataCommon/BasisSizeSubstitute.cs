using System;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Summary description for BasisSizeSubstitute.
	/// </summary>
	public class BasisSizeSubstitute
	{
		private int _sizeTypeRid;
		private int _substituteRid;
		private eEquateOverrideSizeType _sizeType;

		public int SizeTypeRid
		{
			get {return _sizeTypeRid;}
			set {_sizeTypeRid = value;}
		}
		public int SubstituteRid
		{
			get {return _substituteRid;}
			set {_substituteRid = value;}
		}
		public eEquateOverrideSizeType SizeType
		{
			get {return _sizeType;}
			set {_sizeType = value;}
		}

		public BasisSizeSubstitute(int sizeTypeRid, int substituteRid, eEquateOverrideSizeType sizeType)
		{
			_sizeTypeRid = sizeTypeRid;
			_substituteRid = substituteRid;
			_sizeType = sizeType;
		}

		public BasisSizeSubstitute()
		{
			_sizeTypeRid = Include.NoRID;
			_substituteRid = Include.NoRID;
			_sizeType = eEquateOverrideSizeType.DimensionSize;
		}
	}
}
