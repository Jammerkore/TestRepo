using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace MIDRetail.DataCommon
{
    #region
    public class PlaceholderComponentLinks
    {
        //=======
        // FIELDS
        //=======

        private int _asrtRID;
        private string _asrtID;
        private bool _hasHeaderColors;
        private ProfileList _placeHolderList;
       
        public int AsrtRid
        {
            get { return _asrtRID; }
            set { _asrtRID = value; }
        }

        public string AsrtID
        {
            get { return _asrtID; }
            set { _asrtID = value; }
        }

        public bool HasHeaderColors
        {
            get { return _hasHeaderColors; }
            set { _hasHeaderColors = value; }
        }

        public ProfileList PlaceHolderList
        {
            get { return _placeHolderList; }
            set { _placeHolderList = value; }
        }

        //=============
		// CONSTRUCTORS
		//=============

        public PlaceholderComponentLinks(int asrtRID)
		{
			_asrtRID = asrtRID;
            _hasHeaderColors = false;
            _placeHolderList = new ProfileList(eProfileType.PlaceholderComponentLink);
		}

        //============
		// Methods
		//============

        public void AddPlaceholder(int aPlaceholderRID, string aPhID)
        {
            try
            {
                if (!_placeHolderList.Contains(aPlaceholderRID))
                {
                    PlaceholderComponentLinkProfile pclp = new PlaceholderComponentLinkProfile(aPlaceholderRID, aPhID);
                    _placeHolderList.Add(pclp);
                }
            }
            catch
            {
                throw;
            }
        }

        public PlaceholderComponentLinkProfile GetPlaceholderComponentLinkProfile(int aPlaceholderRID)
        {
            try
            {
                return (PlaceholderComponentLinkProfile)_placeHolderList.FindKey(aPlaceholderRID);
            }
            catch
            {
                throw;
            }
        }

        public bool PlaceholderColorExists(int aPlaceholderRID, int aPhBCRID)
        {
            try
            {
                PlaceholderComponentLinkProfile pclp = GetPlaceholderComponentLinkProfile(aPlaceholderRID);
                return pclp.PlaceholderColorExists(aPhBCRID);
            }
            catch
            {
                throw;
            }
        }

        public void AddPlaceholderColor(int aPlaceholderRID, HeaderComponentProfile aHCP)
        {
            try
            {
                PlaceholderComponentLinkProfile pclp = GetPlaceholderComponentLinkProfile(aPlaceholderRID);
                pclp.AddPlaceholderColor(aHCP);
            }
            catch
            {
                throw;
            }
        }

        public bool HeaderColorExists(int aPlaceholderRID, int aHeaderRID, int aColorBCRID)
        {
            try
            {
                PlaceholderComponentLinkProfile pclp = GetPlaceholderComponentLinkProfile(aPlaceholderRID);
                return pclp.HeaderColorExists(aHeaderRID, aColorBCRID);
            }
            catch
            {
                throw;
            }
        }

        public void AddHeaderColor(int aPlaceholderRID, int aHeaderRID, HeaderComponentProfile aHCP)
        {
            try
            {
                PlaceholderComponentLinkProfile pclp = GetPlaceholderComponentLinkProfile(aPlaceholderRID);
                pclp.AddHeaderColor(aHeaderRID,aHCP);
                _hasHeaderColors = true;
            }
            catch
            {
                throw;
            }
        }

        public int PlaceholderColorCount(int aPlaceholderRID)
        {
            try
            {
                PlaceholderComponentLinkProfile pclp = GetPlaceholderComponentLinkProfile(aPlaceholderRID);
                return pclp.PlaceholderColorCount();
            }
            catch
            {
                throw;
            }
        }

        public int HeaderColorCount(int aPlaceholderRID, int aHeaderRID)
        {
            try
            {
                PlaceholderComponentLinkProfile pclp = GetPlaceholderComponentLinkProfile(aPlaceholderRID);
                return pclp.HeaderColorCount(aHeaderRID);
            }
            catch
            {
                throw;
            }
        }

        public bool AllHeaderColorsMatched(int aPlaceholderRID, int aHeaderRID)
        {
            try
            {
                PlaceholderComponentLinkProfile pclp = GetPlaceholderComponentLinkProfile(aPlaceholderRID);
                return pclp.AllHeaderColorsMatched(aHeaderRID);
            }
            catch
            {
                throw;
            }
        }    
    }
    #endregion

    #region PlaceholderComponentLinkProfile
    public class PlaceholderComponentLinkProfile : Profile
    {
        //=======
        // FIELDS
        //=======
        private int _placeholderRID;
        private string _placeholderID;
        private ProfileList _phColorList;
        private Hashtable _headerColorList;
     
        public int PlaceholderRID
        {
            get { return _placeholderRID; }
            set { _placeholderRID = value; }
        }
        public string PlaceholderID
        {
            get { return _placeholderID; }
            set { _placeholderID = value; }
        }
        public ProfileList PhColorList
        {
            get { return _phColorList; }
            set { _phColorList = value; }
        }
        public Hashtable HeaderColorList
        {
            get { return _headerColorList; }
            set { _headerColorList = value; }
        }
        /// <summary>
        /// Returns the eProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.PlaceholderComponentLink;
            }
        }
   
        //=============
        // CONSTRUCTORS
        //=============

        public PlaceholderComponentLinkProfile(int key, string aPlaceholdeID)
            : base(key)
        {
            _placeholderRID = key;
            _placeholderID = aPlaceholdeID;
            _phColorList = new ProfileList(eProfileType.PlaceholderComponentLink);
            _headerColorList = new Hashtable();
        }

        //============
        // Methods
        //============
        public bool PlaceholderColorExists(int aPhBCRID)
        {
            return (_phColorList.Contains(aPhBCRID) ? true : false); 
        }

        public void AddPlaceholderColor(HeaderComponentProfile aHCP)
        {
            _phColorList.Add(aHCP);
        }

        public bool HeaderColorExists(int aHeaderRID, int aColorBCRID)
        {
            try
            {
                if (_headerColorList.ContainsKey(aHeaderRID))
                {
                    ProfileList profileList = (ProfileList)_headerColorList[aHeaderRID];
                    return (profileList.Contains(aColorBCRID) ? true : false); 
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        public void AddHeaderColor(int aHeaderRID, HeaderComponentProfile aHCP)
        {
            try
            {
                ProfileList profileList;
                if (_headerColorList.ContainsKey(aHeaderRID))
                {
                    profileList = (ProfileList)_headerColorList[aHeaderRID];
                    profileList.Add(aHCP);
                }
                else
                {
                    profileList = new ProfileList(eProfileType.HeaderComponent);
                    profileList.Add(aHCP);
                    _headerColorList.Add(aHeaderRID, profileList);
                }
            }
            catch
            {
                throw;
            }
        }

        public int PlaceholderColorCount()
        {
            try
            {
                return _phColorList.Count;
            }
            catch
            {
                throw;
            }
        }

        public int HeaderColorCount(int aHeaderRID)
        {
            try
            {
                int hdrColorCount = _headerColorList.Count;

                if (hdrColorCount > 0)
                {
                    if (_headerColorList.ContainsKey(aHeaderRID))
                    {
                        ProfileList profileList = (ProfileList)_headerColorList[aHeaderRID];
                        hdrColorCount = profileList.Count;
                    }
                }
                return hdrColorCount;
            }
            catch
            {
                throw;
            }
        }

        public bool AllHeaderColorsMatched(int aHeaderRID)
        {
            try
            {
                bool allHeadersMatched = true;
                int hdrColorCount = _headerColorList.Count;

                if (_headerColorList.Count > 0)
                {
                    if (_headerColorList.ContainsKey(aHeaderRID))
                    {
                        ProfileList profileList = (ProfileList)_headerColorList[aHeaderRID];
                        foreach (HeaderComponentProfile hcp in profileList)
                        {
                            if (hcp.AsrtBCRID == Include.NoRID)
                            {
                                allHeadersMatched = false;
                                break;
                            }
                        }
                    }
                }
                return allHeadersMatched;
            }
            catch
            {
                throw;
            }
        }
    }   
    #endregion

    #region HeaderComponentProfile
    public class HeaderComponentProfile : Profile
    {
        //=======
        // FIELDS
        //=======
        private int _headerRID;
        private string _headerID;
        private eComponentType _componentType;

        private int _hdrBCRID;
        private int _colorCodeRID;
        private int _asrtBCRID;
        private int _sequence;
        private string _bulkColor;
        private string _description;
        private string _name;
        private bool _isVirtual;
        private Color _displayColor;

        public int HeaderRID
        {
            get { return _headerRID; }
            set { _headerRID = value; }
        }
        public string HeaderID
        {
            get { return _headerID; }
            set { _headerID = value; }
        }
        public eComponentType ComponentType
        {
            get { return _componentType; }
            set { _componentType = value; }
        }
        public int HdrBCRID
        {
            get { return _hdrBCRID; }
            set { _hdrBCRID = value; }
        }
        public int ColorCodeRID
        {
            get { return _colorCodeRID; }
            set { _colorCodeRID = value; }
        }
        public int AsrtBCRID
        {
            get { return _asrtBCRID; }
            set { _asrtBCRID = value; }
        }
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }
        public string BulkColor
        {
            get { return _bulkColor; }
            set { _bulkColor = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public bool IsVirtual
        {
            get { return _isVirtual; }
            set { _isVirtual = value; }
        }
        public Color DisplayColor
        {
            get { return _displayColor; }
            set { _displayColor = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Returns the eProfileType of this profile.
        /// </summary>
        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.HeaderComponent;
            }
        }
        //=============
        // CONSTRUCTORS
        //=============

        public HeaderComponentProfile(int key, eComponentType aComponentType)
            : base(key)
        {
            _hdrBCRID = key;
            _componentType = aComponentType;
        }



        
    }
    #endregion
}
