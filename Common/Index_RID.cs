using System;
using MIDRetail.Data;
using MIDRetail.DataCommon;


namespace MIDRetail.Common
{
	#region Index_RID structure
	/// <summary>
	/// Defines an index to RID key relationship.  Used to locate information contained in arrays where the key is an RID but the position in the array is a potentially different index value.
	/// </summary>
	public struct Index_RID
	{
		//=======
		// FIELDS
		//=======
		private int _index;
		private int _RID;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aIndex">Index associated with the RID.</param>
		/// <param name="aRID">RID associated with the Index.</param>
		public Index_RID(int aIndex, int aRID)
		{
			if (aIndex < 0)
			{
				throw new MIDException(eErrorLevel.severe,
					(int)(eMIDTextCode.msg_IndexOutOfRange),
					MIDText.GetText(eMIDTextCode.msg_IndexOutOfRange));
			}
			_index = aIndex;
			_RID = aRID;
		}
		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets the index value.
		/// </summary>
		public int Index
		{
			get
			{
				return _index;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_IndexOutOfRange),
						MIDText.GetText(eMIDTextCode.msg_IndexOutOfRange));
				}
				_index = value;
			}
		}

		/// <summary>
		/// Gets or sets the RID value.
		/// </summary>
		public int RID
		{
			get
			{
				return _RID;
			}
			set
			{
				_RID = value;
			}
		}
	}
	#endregion Index_RID
}
