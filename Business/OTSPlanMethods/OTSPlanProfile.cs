using System;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for OTSPlanProfile.
	/// </summary>
	public class OTSPlanProfile:Profile
	{
//		string _sourceModule = "OTSPlanMethod.cs";
//		private SessionAddressBlock _SAB;
		private PlanCubeGroup _cubeGroup;
		//private PlanOpenParms _openParms;

		private Hashtable _storeGroupLevelHash;
		private ProfileList _allStoreList;
		private ProfileList _weeksToPlan;
		//private ProfileList _GLFunctions;
		//private ArrayList _storeBasisArray;

//		private DataTable _dtBasis;
		//private string _errorMsg;
		private ForecastMonitor _forecastMonitor;
		private bool _monitor;

		public ProfileList WeeksToPlan
		{
			get	{return _weeksToPlan;}
			set	{_weeksToPlan = value;	}
		}
		public ProfileList AllStoreList
		{
			get	{return _allStoreList;}
			set	{_allStoreList = value;	}
		}
		public Hashtable StoreGroupLevelHash
		{
			get	{return _storeGroupLevelHash;}
			set	{_storeGroupLevelHash = value;	}
		}
		public PlanCubeGroup CubeGroup
		{
			get	{return _cubeGroup;}
			set	{_cubeGroup = value;	}
		}
		public ForecastMonitor ForecastMonitor 
		{
			get { return _forecastMonitor ; }
			set { _forecastMonitor = value; }
		}
		public bool MONITOR 
		{
			get { return _monitor ; }
			set { _monitor = value; }
		}

		public OTSPlanProfile(SessionAddressBlock aSAB, int aKey)
			: base(aKey)
		{
		
		}

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.OTSPlan;
			}
		}
	}
}
