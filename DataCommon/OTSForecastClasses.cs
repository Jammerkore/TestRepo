using System;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Class that defines information for OTS Forecast variables.
	/// </summary>

	public class OTSForecastVariableProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private bool	_balance;
		private bool	_process;
		
		//=============
		// CONSTRUCTORS
		//=============

		public OTSForecastVariableProfile(int aKey)
			: base(aKey)
		{
			_balance = false;
			_process = false;
		}

		public OTSForecastVariableProfile(int aKey, bool aProcess, bool aBalance)
			: base(aKey)
		{
			_process = aProcess;
			_balance = aBalance;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Returns the eProfileType of this profile.
		/// </summary>

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.OTSForecastVariable;
			}
		}

		public bool Balance
		{
			get
			{
				return _balance;
			}
			set
			{
				_balance = value;
			}
		}

		public bool Process
		{
			get
			{
				return _process;
			}
			set
			{
				_process = value;
			}
		}

		//========
		// METHODS
		//========

	}

	/// <summary>
	/// Used to retrieve a list of OTSForecastVariable profiles
	/// </summary>
	[Serializable()]
	public class OTSForecastVariableProfileList : ProfileList
	{
		/// <summary>
		/// Used to construct an instance of the class.
		/// </summary>
		public OTSForecastVariableProfileList(eProfileType aProfileType)
			: base(aProfileType)
		{
			
		}
	}
}
