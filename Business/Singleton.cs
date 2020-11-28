using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Runtime.CompilerServices; // for MethodImplAttribute



namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for CWSingleton
	/// Dim oSingleton As singleton = singleton.GetCurrentSingleton()
	/// singleton oSingleton = singleton.GetCurrentSingleton
	/// </summary>
	[Serializable]
	public sealed class singleton
	{

		//Variables to store data 
		string appDir = "";
		bool bIsSysop = false;   
		bool bIsAdmin = false;   
		string userName = "";
		string clientCode = "";
		string lastName = "";
		string firstName = "";
		bool bSortAscending = false;
		System.Data.DataSet dsProduct = null;

		// Crystal Report Variables
		bool bLoadReport = false;
		bool bParamLoaded = false;
		string reportFile = "";
		string reportFileName = "";
		CrystalDecisions.CrystalReports.Engine.ReportDocument crDoc = null;

		/// <summary>
		/// Directory where application is stored
		/// </summary>
		public string AppDir 
		{
			get { return appDir; }
			set { appDir = value; }
		}

		/// <summary>
		/// LoadReport
		/// </summary>
		public bool LoadReport 
		{
			get { return bLoadReport; }
			set { bLoadReport = value; }
		}

		/// <summary>
		/// ParamLoaded
		/// </summary>
		public bool ParamLoaded 
		{
			get { return bParamLoaded; }
			set { bParamLoaded = value; }
		}

		/// <summary>
		/// Name of the Crystal Report File without the .rpt extension
		/// </summary>
		public string ReportFile 
		{
			get { return reportFile; }
			set { reportFile = value; }
		}

		/// <summary>
		/// Full pathway of the .rpt Report File with .rpt extension
		/// </summary>
		public string ReportFileName 
		{
			get { return reportFileName; }
			set { reportFileName = value; }
		}

		/// <summary>
		/// Crystal Document Object that we can store a .rpt file inside of.
		/// </summary>
		public CrystalDecisions.CrystalReports.Engine.ReportDocument CrystalDoc 
		{
			get { return crDoc; }
			set { crDoc = value; }
		}


		public System.Data.DataSet Product
		{
			get { return dsProduct; }
			set { dsProduct = value; }
		}


		public bool IsSysop 
		{
			get { return bIsSysop; }
			set { bIsSysop = value; }
		}

		public bool IsAdmin 
		{
			get { return bIsAdmin; }
			set { bIsAdmin = value; }
		}

		public string UserName 
		{
			get { return userName; }
			set { userName = value; }
		}

		public string ClientCode 
		{
			get { return clientCode; }
			set { clientCode = value; }
		}

		public string LastName 
		{
			get { return lastName; }
			set	{ lastName = value; }
		}

		public string FirstName 
		{
			get	{ return firstName; }
			set	{ firstName = value; }
		}

		public bool SortAscending 
		{
			get { return bSortAscending; }
			set { bSortAscending = value; }
		}

		static readonly singleton oSingleton = new singleton();

		// Explicit static constructor to tell C# compiler
		// not to mark type as beforefieldinit
		static singleton()
		{
		}

		singleton()
		{
		}

		public static singleton GetCurrentSingleton
		{
			get
			{
				return oSingleton;
			}
		}
	}

}




