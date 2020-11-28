using System;
using System.Collections;
using MIDRetail.DataCommon;


namespace MIDRetail.Common
{
	/// <summary>
	/// Contains the information about the selected headers
	/// </summary>
	[Serializable()]
	public class SelectedHeaderProfile  : Profile
	{
		// Fields
		private string			_headerID;
		private eHeaderType		_headerType;     // Added as part of Assortment
        private int             _asrtRID;        // TT#2 - Assortment Planning 4.0
        private int             _styleHnRID;     // TT#2 - RMatelic - Assortment Planning 
        private bool            _bypassEnqueue;  // TT#1442 - RMatelic - Error processing General Allocation Method on Assortment>>> temporary

		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SelectedHeaderProfile(int aKey)
			: base(aKey)
		{
			_headerType = eHeaderType.Dummy;
            _bypassEnqueue = false;              // TT#1442 - RMatelic - Error processing General Allocation Method on Assortment>>> temporary
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SelectedHeader;
			}
		}

		/// <summary>
		/// Gets or sets the id of the header.
		/// </summary>
		public string HeaderID 
		{
			get { return _headerID ; }
			set { _headerID = value; }
		}
		/// <summary>
		/// Gets or sets the Header Type which is used to distinguish
		/// assortment headers from others.
		/// </summary>
		public eHeaderType HeaderType
		{
			get { return _headerType; }
			set { _headerType = value; }
		}

        /// <summary>
        /// Gets or sets the AsrtRID of the header.
        /// </summary>
        public int AsrtRID                          // Begin TT#2 - Assortment Planning 4.0
        {
            get { return _asrtRID; }
            set { _asrtRID = value; }               // End TT#2 - Assortment Planning 4.0
        }

        // Begin TT#2 - RMatelic - Assortment Planning 
        /// <summary>
        /// Gets or sets the StyleRID of the header.
        /// </summary>
        public int StyleHnRID                           
        {
            get { return _styleHnRID; }
            set { _styleHnRID = value; }                
        }
        // End TT#2

        // Begin TT#1442 - RMatelic - Error processing General Allocation Method on Assortment>>> temporary
         public bool BypassEnqueue                           
        {
            get { return _bypassEnqueue; }
            set { _bypassEnqueue = value; }                
        }
        // End TT#1442  
	}

	/// <summary>
	/// Used to retrieve a list of selected headers
	/// </summary>
	[Serializable()]
	public class SelectedHeaderList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SelectedHeaderList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}

	#region SelectedComponentProfile
	[Serializable()]
	public class SelectedComponentProfile  : Profile
	{
		// Fields
		
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SelectedComponentProfile(int aKey)
			: base(aKey)
		{
			
		}

		// Properties

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SelectedComponent;
			}
		}

//		/// <summary>
//		/// Gets or sets the id of the header.
//		/// </summary>
//		public string HeaderID 
//		{
//			get { return _headerID ; }
//			set { _headerID = value; }
//		}
	}

	/// <summary>
	/// Used to retrieve a list of selected headers
	/// </summary>
	[Serializable()]
	public class SelectedComponentList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public SelectedComponentList(eProfileType aProfileType)
			: base(aProfileType)
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
	#endregion

}
