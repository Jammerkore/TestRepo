using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace Logility.ROUI
{
	public class BasisLabelVariableEntry
    {
        //=======
        // FIELDS
        //=======

        private int _systemOptionRID;
        private int _labelType;
        private string _labelText;
        private int _labelSequence;

        //=============
        // CONSTRUCTORS
        //=============

        public BasisLabelVariableEntry(DataRow aDataRow)
        {
            try
            {
                LoadFromDataRow(aDataRow);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public BasisLabelVariableEntry(int aSystemOptionRID, int aLabelType, string aLabelText, int aLabelSequence)
        {
            try
            {
                _systemOptionRID = aSystemOptionRID;
                _labelType = aLabelType;
                _labelText = aLabelText;
                _labelSequence = aLabelSequence;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========

        public int SystemOptionRID
        {
            get
            {
                return _systemOptionRID;
            }
            set
            {
                _systemOptionRID = value;
            }
        }

        public int LabelType
        {
            get
            {
                return _labelType;
            }
            set
            {
                _labelType = value;
            }
        }

        public string LabelText
        {
            get
            {
                return _labelText;
            }
            set
            {
                _labelText = value;
            }
        }

        public int LabelSequence
        {
            get
            {
                return _labelSequence;
            }
            set
            {
                _labelSequence = value;
            }
        }

        //========
        // METHODS
        //========

        private void LoadFromDataRow(DataRow aRow)
        {
            try
            {
                _systemOptionRID = Convert.ToInt32(aRow["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture);
                _labelType = Convert.ToInt32(aRow["LABEL_TYPE"], CultureInfo.CurrentCulture);
                _labelText = "";
                _labelSequence = Convert.ToInt32(aRow["LABEL_SEQ"], CultureInfo.CurrentUICulture);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public DataRow UnloadToDataRow(DataRow aRow)
        {
            try
            {
                aRow["SYSTEM_OPTION_RID"] = _systemOptionRID;
                aRow["LABEL_TYPE"] = _labelType;
                aRow["LABEL_TEXT"] = _labelType;
                aRow["LABEL_SEQ"] = _labelSequence;

                return aRow;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

    public class BasisLabelTypeProfile : Profile
    {
        //=======
        // FIELDS
        //=======

        private int _key;
        private int _systemOptionRID;
        private string _name;
        private int _type;
        private int _sequence;

        //=============
        // CONSTRUCTORS
        //=============

        /// <summary>
        /// Creates a new instance of BasisLabelTypeProfile.
        /// </summary>
        /// <param name="aKey">
        /// The integer that identifies the logical RID of this coordinate.
        /// </param>

        /// <summary>
        /// Returns the eProfileType of this profile.
        /// </summary>

        public BasisLabelTypeProfile(int aKey)
            : base(aKey)
        {
            _key = aKey;
        }

        override public eProfileType ProfileType
        {
            get
            {
                return eProfileType.BasisLabelType;

            }
        }

        public int BasisLabelSystemOptionRID
        {
            get
            {
                return _systemOptionRID;
            }
            set
            {
                _systemOptionRID = value;
            }
        }

        public int BasisLabelType
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public string BasisLabelName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int BasisLabelSequence
        {
            get
            {
                return _sequence;
            }
            set
            {
                _sequence = value;
            }
        }
        // BEGIN TT#1966-MD - AGallagher - DC Fulfillment
        public int DCFulfillmentSystemOptionRID
        {
            get
            {
                return _systemOptionRID;
            }
            set
            {
                _systemOptionRID = value;
            }
        }
        // END TT#1966-MD - AGallagher - DC Fulfillment

        //===========
        // PROPERTIES
        //===========

        /// <summary>
        /// Gets the integer key of this coordinate.
        /// </summary>

        public int Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        //========
        // METHODS
        //========

        /// <summary>
        /// Creates a copy of this BasisLabelTypeProfile.
        /// </summary>
        /// <returns>
        /// A new instance of BasisLabelTypeProfile with a copy of this objects information.
        /// </returns>

        public BasisLabelTypeProfile Copy()
        {
            try
            {
                return new BasisLabelTypeProfile(_key);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

}

