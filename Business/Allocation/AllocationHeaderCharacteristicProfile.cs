using System;
using System.Data;
using System.Collections;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Used to retrieve and update information about a HeaderCharacteristicGroup.
	/// </summary>
	public class HeaderCharGroupProfile : Profile
	{
		// Fields
		private string			_ID;
		private eHeaderCharType	_type;
		private bool			_listInd;
		private bool			_protectInd;
		private Hashtable		_characteristics;
	
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderCharGroupProfile(int aKey)
			: base(aKey)
		{
			_characteristics = new Hashtable();
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HeaderCharGroup;
			}
		}

		/// <summary>
		/// Gets or sets the id of the header characteristic group.
		/// </summary>
		public string ID 
		{
			get { return _ID ; }
			set { _ID = value; }
		}
		/// <summary>
		/// Gets or sets the type of the header characteristic group.
		/// </summary>
		public eHeaderCharType Type 
		{
			get { return _type ; }
			set { _type = value; }
		}
		/// <summary>
		/// Gets or sets the flag identifying if the header characteristic group is to be listed.
		/// </summary>
		public bool ListInd 
		{
			get { return _listInd ; }
			set { _listInd = value; }
		}
		/// <summary>
		/// Gets or sets the flag identifying if the header characteristic group is to be protected.
		/// </summary>
		public bool ProtectInd 
		{
			get { return _protectInd ; }
			set { _protectInd = value; }
		}
		/// <summary>
		/// Gets or sets the list of header characteristics in the group.
		/// </summary>
		public Hashtable Characteristics 
		{
			get { return _characteristics ; }
			set { _characteristics = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of HeaderCharGroup profiles
	/// </summary>
	[Serializable()]
	public class HeaderCharGroupProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderCharGroupProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}

	/// <summary>
	/// Used to retrieve and update information about a HeaderCharProfile.
	/// </summary>
	public class HeaderCharProfile : Profile
	{
		// Fields
//		private int			_RID;
		//private int		 _groupRID;          // BEGIN MID Track #5488 - characteristic error
        private int          _charRID;           // END MID Track #5488
		private eHeaderCharType _headerCharType;
		private string		_textValue;
		private DateTime	_dateValue;
		private double		_numberValue;
		private double		_dollarValue;
		private string		_text;
	
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderCharProfile(int aKey)
			: base(aKey)
		{
//			_RID = Include.NoRID;
			_charRID = Include.NoRID;
			_text = null;
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.HeaderChar;
			}
		}
        // BEGIN MID Track #5488 - characteristic error
		/// <summary>
		/// Gets or sets the header characteristics group's record id.
		/// </summary>
        //public int GroupRID 
        //{
        //    get { return _groupRID; }
        //    set { _groupRID = value; }
        //}
        public int CharRID
        {
            get { return _charRID; }
            set { _charRID = value; }
        }
        // END MID Track #5488
		/// <summary>
		/// Gets or sets the header characteristic type for the group.
		/// </summary>
		public eHeaderCharType HeaderCharType
		{
			get { return _headerCharType; }
			set { _headerCharType = value; }
		}
		/// <summary>
		/// Gets or sets the text value for the header characteristic.
		/// </summary>
		public string TextValue 
		{
			get { return _textValue ; }
			set { _textValue = value; }
		}
		/// <summary>
		/// Gets or sets the date value for the header characteristic.
		/// </summary>
		public DateTime	DateValue 
		{
			get { return _dateValue ; }
			set { _dateValue = value; }
		}
		/// <summary>
		/// Gets or sets the number value for the header characteristic.
		/// </summary>
		public double NumberValue 
		{
			get { return _numberValue ; }
			set { _numberValue = value; }
		}
		/// <summary>
		/// Gets or sets the dollar value for the header characteristic.
		/// </summary>
		public double DollarValue 
		{
			get { return _dollarValue ; }
			set { _dollarValue = value; }
		}
		/// <summary>
		/// Gets or sets the text value for the header characteristic.
		/// </summary>
		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}
	}

	/// <summary>
	/// Used to retrieve a list of HeaderChar profiles
	/// </summary>
	[Serializable()]
	public class HeaderCharProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public HeaderCharProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}
}
