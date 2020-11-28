using System;
using System.Globalization;
using System.Text;

// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
using MIDRetail.DataCommon;
// (CSMITH) - END MID Track #3369

namespace MIDRetail.Common
{
	/// <summary>
	/// Summary description for AppConfig.
	/// </summary>
// Begin Track #3455 - Serialization error
	[Serializable]
// End Track #3455 - Serialization error
	public class AppConfig
	{
		//=======
		// FIELDS
		//=======

        //================================================================================================
        // NOTE: If new bools are added to class. Update the GetHashCode() method at the end of the class.
        //================================================================================================

		private string		_licenseKey;
		private string		_encryptedLicenseKey;
		private bool		_allocationInstalled = false;
		private bool		_allocationTempLicense = false;
		private int			_allocationExpirationDays = int.MaxValue;
		private bool		_sizeInstalled = false;
		private bool		_sizeTempLicense = false;
		private int			_sizeExpirationDays = int.MaxValue;
		private bool		_planningInstalled = false;
		private bool		_planningTempLicense = false;
		private int			_planningExpirationDays = int.MaxValue;
		private bool		_assortmentInstalled = false;
		private bool		_assortmentTempLicense = false;
		private int			_assortmentExpirationDays = int.MaxValue;
		// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        private bool        _groupAllocationInstalled = false;
        private bool        _groupAllocationTempLicense = false;
        private int         _groupAllocationExpirationDays = int.MaxValue;
		// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -
		private bool		_masterAllocationInstalled = false;
		private bool		_masterAllocationTempLicense = false;
		private int			_masterAllocationExpirationDays = int.MaxValue;
        // Begin TT#2131-MD - JSmith - Halo Integration
        private bool _analyticsInstalled = false;
        private bool _analyticsTempLicense = false;
        private int _analyticsExpirationDays = int.MaxValue;
        // End TT#2131-MD - JSmith - Halo Integration

		private int			_pos = 0;
// Begin Track #3455 - Serialization error
//		private Encryption  _encryption;
// End Track #3455 - Serialization error

		//=============
		// CONSTRUCTORS
		//=============

		public AppConfig()
		{
			try
			{
// Begin Track #3455 - Serialization error
//				_encryption = new Encryption('0');
// End Track #3455 - Serialization error
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public bool AllocationInstalled
		{
			get{return _allocationInstalled;}
		}
		public bool AllocationTempLicense
		{
			get{return _allocationTempLicense;}
		}
		public int AllocationExpirationDays
		{
			get{return _allocationExpirationDays;}
		}

		public bool SizeInstalled
		{
			get{return _sizeInstalled;}
		}
		public bool SizeTempLicense
		{
			get{return _sizeTempLicense;}
		}
		public int SizeExpirationDays
		{
			get{return _sizeExpirationDays;}
		}

		public bool PlanningInstalled
		{
			get{return _planningInstalled;}
		}
		public bool PlanningTempLicense
		{
			get{return _planningTempLicense;}
		}
		public int PlanningExpirationDays
		{
			get{return _planningExpirationDays;}
		}

		public bool AssortmentInstalled
		{
			get{return _assortmentInstalled;}
		}
		public bool AssortmentTempLicense
		{
			get{return _assortmentTempLicense;}
		}
		public int AssortmentExpirationDays
		{
			get{return _assortmentExpirationDays;}
		}

		// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        public bool GroupAllocationInstalled
        {
            get { return _groupAllocationInstalled; }
        }
        public bool GroupAllocationTempLicense
        {
            get { return _groupAllocationTempLicense; }
        }
        public int GroupAllocationExpirationDays
        {
            get { return _groupAllocationExpirationDays; }
        }
		// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

		public bool MasterAllocationInstalled
		{
            //get{return _masterAllocationInstalled;}
            get { return true; }
		}
		public bool MasterAllocationTempLicense
		{
			get{return _masterAllocationTempLicense;}
		}
		public int MasterAllocationExpirationDays
		{
			get{return _masterAllocationExpirationDays;}
		}

        // Begin TT#2131-MD - JSmith - Halo Integration
        public bool AnalyticsInstalled
        {
            get { return _analyticsInstalled; }
        }
        public bool AnalyticsTempLicense
        {
            get { return _analyticsTempLicense; }
        }
        public int AnalyticsExpirationDays
        {
            get { return _analyticsExpirationDays; }
        }
        // End TT#2131-MD - JSmith - Halo Integration

		//========
		// METHODS
		//========

		public void SetLicenseKey (string aLicenseKey)
		{
// Begin Track #3455 - Serialization error
			Encryption encryption = new Encryption('0');

// End Track #3455 - Serialization error
			try
			{
				// temp code until configuration manager is activated
				if (aLicenseKey.Length == 0)
				{
					_allocationInstalled = true;
					_allocationTempLicense = false;
					_allocationExpirationDays = int.MaxValue;
					_sizeInstalled = true;
					_sizeTempLicense = false;
					_sizeExpirationDays = int.MaxValue;
					_planningInstalled = true;
					_planningTempLicense = false;
					_planningExpirationDays = int.MaxValue;
					_assortmentInstalled = false;
					_assortmentTempLicense = false;
					_assortmentExpirationDays = int.MaxValue;
					// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                    _groupAllocationInstalled = false;
                    _groupAllocationTempLicense = false;
                    _groupAllocationExpirationDays = int.MaxValue;
					// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -
					_masterAllocationInstalled = false;
					_masterAllocationTempLicense = false;
					_masterAllocationExpirationDays = int.MaxValue;
                    // Begin TT#2131-MD - JSmith - Halo Integration
                    _analyticsInstalled = false;
                    _analyticsTempLicense = false;
                    _analyticsExpirationDays = int.MaxValue;
                    // End TT#2131-MD - JSmith - Halo Integration
					return;
				}

//				if (aLicenseKey.Length != 28)
//				{
//					throw new Exception("Invalid license Key");
//				}
				_encryptedLicenseKey = aLicenseKey;
				
				// decode license key
//				_licenseKey = aLicenseKey;
// Begin Track #3455 - Serialization error
//				_licenseKey = _encryption.Decrypt(aLicenseKey);
				_licenseKey = encryption.Decrypt(aLicenseKey);
// End Track #3455 - Serialization error

				SetApplication(ref _allocationInstalled, ref _allocationTempLicense, ref _allocationExpirationDays);
				SetApplication(ref _sizeInstalled, ref _sizeTempLicense, ref _sizeExpirationDays);
				SetApplication(ref _planningInstalled, ref _planningTempLicense, ref _planningExpirationDays);
                // Begin TT#236 MD - JSmith - Create License Key option for Assortment
                //// BEGIN TT#2 - stodd - assortment will now be part of the base, but still wanted it to show in the add-ons
                ////SetApplication(ref _assortmentInstalled, ref _assortmentTempLicense, ref _assortmentExpirationDays);
                //_assortmentInstalled = true;
                //_assortmentTempLicense = false;
                //_assortmentExpirationDays = int.MaxValue;
                //// END TT#2 - stodd 
                SetApplication(ref _assortmentInstalled, ref _assortmentTempLicense, ref _assortmentExpirationDays);
                //SetApplication(ref _groupAllocationInstalled, ref _groupAllocationTempLicense, ref _groupAllocationExpirationDays);		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                // Begin TT#236 MD
				SetApplication(ref _masterAllocationInstalled, ref _masterAllocationTempLicense, ref _masterAllocationExpirationDays);
                SetApplication(ref _groupAllocationInstalled, ref _groupAllocationTempLicense, ref _groupAllocationExpirationDays);		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                SetApplication(ref _analyticsInstalled, ref _analyticsTempLicense, ref _analyticsExpirationDays);  // TT#2131-MD - JSmith - Halo Integration
			
			}
			catch
			{
				throw;
			}
		}

		private void SetApplication(ref bool aInstalled, ref bool aTempLicense, ref int aExpirationDays)
		{
			try
			{
                // Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                if (_pos > _licenseKey.Length - 1)
                {
                    aInstalled = false;
                    aTempLicense = false;
                    aExpirationDays = int.MaxValue;
                    return;
                }
                // End TT#1247-MD - stodd - Add Group Allocation as a License Key option -
				if (_licenseKey.Substring(_pos, 1) == "1")
				{
					aInstalled = true;
				}
				else
				{
					aInstalled = false;
				}
				string strExpDate = _licenseKey.Substring(_pos+1, 6);
				int expDate, year, month, day;
				try
				{
					expDate = Convert.ToInt32(strExpDate);
					if (expDate > 0)
					{
						year = Convert.ToInt32("20" + strExpDate.Substring(0,2));
						month = Convert.ToInt32(strExpDate.Substring(2,2));
						day = Convert.ToInt32(strExpDate.Substring(4,2));
						aTempLicense = true;
						DateTime dt = new DateTime(year, month, day);
						System.TimeSpan diff = dt.Subtract(DateTime.Now);
						aExpirationDays = diff.Days + 1;
						// temporary license has expired
						if (aExpirationDays <= 0)
						{
							aInstalled = false;
						}
					}
					else
					{
						aTempLicense = false;
						aExpirationDays = int.MaxValue;
					}
				}
				catch
				{
					throw;
				}

				_pos += 7;
			}
			catch
			{
				throw;
			}
		}

		public string BuildLicenseKey(bool aAllocationInstalled, int aAllocationExpiration, 
			bool aSizeInstalled, int aSizeExpiration, 
			bool aPlanningInstalled, int aPlanningExpiration, 
			bool aAssortmentInstalled, int aAssortmentExpiration,
            bool aGroupAllocationInstalled, int aGroupAllocationExpiration,		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
			bool aMasterInstalled, int aMasterExpiration,
            bool aAnalyticsInstalled, int aAnalyticsExpiration)  // TT#2131-MD - JSmith - Halo Integration
		{
// Begin Track #3455 - Serialization error
			Encryption encryption = new Encryption('0');

// End Track #3455 - Serialization error
			try
			{
                //**********************************************************************************
                //
                //     IMPORTANT
                // 
				// Do not change the order of these calls.  It will impact how the key is processed. 
                // Add new add-ons to the end of the key.
                //**********************************************************************************
				StringBuilder key = new StringBuilder();
				AddApplication(key, aAllocationInstalled, aAllocationExpiration);
				AddApplication(key, aSizeInstalled, aSizeExpiration);
				AddApplication(key, aPlanningInstalled, aPlanningExpiration);
				AddApplication(key, aAssortmentInstalled, aAssortmentExpiration);
                //AddApplication(key, aGroupAllocationInstalled, aGroupAllocationExpiration);		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
				AddApplication(key, aMasterInstalled, aMasterExpiration);
                AddApplication(key, aGroupAllocationInstalled, aGroupAllocationExpiration);		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                AddApplication(key, aAnalyticsInstalled, aAnalyticsExpiration);  // TT#2131-MD - JSmith - Halo Integration

				// add encode here
//				string encodedKey = key.ToString();
// Begin Track #3455 - Serialization error
//				string encodedKey = _encryption.Encrypt(key.ToString());
				string encodedKey = encryption.Encrypt(key.ToString());
// End Track #3455 - Serialization error

				return encodedKey;
			}
			catch
			{
				throw;
			}
		}

		private void AddApplication(StringBuilder aKey, bool aApplicationInstalled, int aApplicationExpiration)
		{
			try
			{
				if (aApplicationInstalled)
				{
					aKey.Append("1");
					if (aApplicationExpiration > 0)
					{
						DateTime dt = DateTime.Now.AddDays(Convert.ToDouble(aApplicationExpiration));
						aKey.Append(dt.ToString("yyMMdd"));
					}
					else
					{
						aKey.Append("000000");
					}
				}
				else
				{
					aKey.Append("0000000");
				}
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#862 - MD - stodd - Assortment Upgrade Issues
        public override int GetHashCode()
        {
            // NOTE: If new bools are added to class, include them here 
            //       so hash code is properly calculated.
            bool[] bools = new bool[]
                {
                    _allocationInstalled, 
                    _sizeInstalled, 
                    _planningInstalled, 
                    _assortmentInstalled,
					_masterAllocationInstalled, 
                    _groupAllocationInstalled		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                };

            // FYI: 17 and 23 are just randomly chosen prime numbers.
            int hash = 17;
            for (int index = 0; index < bools.Length; index++)
            {
                hash = hash * 23 + bools[index].GetHashCode();
            }
            return hash;
            
        }
        // End TT#862 - MD - stodd - Assortment Upgrade Issues
	}
}
