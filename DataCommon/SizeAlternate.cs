using System;
using System.Collections;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Hold a single Size Alternate Primary.
	/// </summary>
	[Serializable]	// MID Track #5092 - Serialization error 
	public class SizeAlternatePrimary
	{
		private int _seq;
		private int _sizeRid;
		private int _dimensionRid;
		private ArrayList _alternateList; // list of SizeAlternate classes for the primary

		public int Sequence
		{
			get {return _seq;}
			set {_seq = value;}
		}
		public int SizeRid
		{
			get {return _sizeRid;}
			set {_sizeRid = value;}
		}
		public int DimensionRid
		{
			get {return _dimensionRid;}
			set {_dimensionRid = value;}
		}
		public ArrayList AlternateList
		{
			get {return _alternateList;}
			set {_alternateList = value;}
		}
		
		public SizeAlternatePrimary(int seq, int sizeRid, int dimRid)
		{
			_seq = seq;
			_sizeRid = sizeRid;
			_dimensionRid = dimRid;
			_alternateList = new ArrayList();
		}

		public SizeAlternatePrimary()
		{
			_seq = 0;
			_sizeRid = Include.NoRID;
			_dimensionRid = Include.NoRID;
			_alternateList = new ArrayList();
		}
	}



	/// <summary>
	/// Hold a single Size Alternate for the Size Alternate primary.
	/// </summary>
	[Serializable]	// MID Track #5092 - Serialization error 
	public class SizeAlternate
	{
		private int _seq;
		private int _sizeRid;
		private int _dimensionRid;

		public int Sequence
		{
			get {return _seq;}
			set {_seq = value;}
		}
		public int SizeRid
		{
			get {return _sizeRid;}
			set {_sizeRid = value;}
		}
		public int DimensionRid
		{
			get {return _dimensionRid;}
			set {_dimensionRid = value;}
		}
		
		public SizeAlternate(int seq, int sizeRid, int dimRid)
		{
			_seq = seq;
			_sizeRid = sizeRid;
			_dimensionRid = dimRid;
		}

		public SizeAlternate()
		{
			_seq = 0;
			_sizeRid = Include.NoRID;
			_dimensionRid = Include.NoRID;
		}
	}
}
