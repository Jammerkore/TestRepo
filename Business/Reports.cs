//using System;
//using System.Configuration;
//using System.Data;
//using System.IO;

////using CrystalDecisions.CrystalReports.Engine;
////using CrystalDecisions.Shared;

//using MIDRetail.DataCommon;

//namespace MIDRetail.Business
//{
//	public class Reports
//	{
//        //private bool isBusy;
//		public string file;
//		public string xmlFile;
//        //private CrystalHelper cryCrystalHelper;

//		public Reports(string file)
//		{
//			this.file = file;
////			this.xmlFile = ConfigurationSettings.AppSettings["InitialReportsDirectory"].TrimEnd('\\') + "\reports.xml";
//		}

//		public int GetParameterCount()
//		{
//            //ReportDocument crDocz;
//            //crDocz = null;
//            //crDocz = new ReportDocument();
//            //crDocz.Load(file.ToString());
//            //// Get parameter field definitions in collection of ParameterFieldDefinition objects
//            //ParameterFieldDefinitions crParamFieldDefinitions = crDocz.DataDefinition.ParameterFields;
//            //crDocz = null;
//            //return crParamFieldDefinitions.Count;
//            return 0;
//		}

//		#region '" Load Parameter(s) & Tables Read from .rpt Report File "' 


//		/// <summary>
//		/// Reads selected parameters saved to xml file into DataSet.
//		/// Reads DataSet into array that is passed into Crystal Report document.
//		/// Loads tables and establishes database connection string passed to Crystal.
//		/// </summary>
//		/// 
//		private String server=null;
//		private String database=null;
//		private String userid=null;
//		private String pass=null;

//		public ReportDocument LoadParameters() 
//		{ 
//			singleton oSingleton = singleton.GetCurrentSingleton;
//			//CrystalReportViewer1.ReportSource = null;
//            //isBusy = false;
//			string filepath = oSingleton.ReportFile;
//			if(!File.Exists(file))
//			{
//				// Reset GUI selection criteria if no valid file is already loaded
//				oSingleton.CrystalDoc = null;
//				oSingleton.LoadReport = false;
//				oSingleton.ParamLoaded = false;
//				return null;
//			}

//			// if no params, then go to the report page
//			int paramCount = GetParameterCount();

//			if(paramCount<1)
//				oSingleton.ParamLoaded = true;

//			ReportDocument crDoc;

//			if( paramCount<1 )
//			{
//				oSingleton.CrystalDoc = null;
//				crDoc = new ReportDocument();
//				crDoc.Load(file);
//			}
//			else
//			{
//				crDoc = (ReportDocument) oSingleton.CrystalDoc;
//			}

//			///////////////////////////////////////////////////////////////////////
//			//LOAD DATABASE INFORMATION: Read table info from .rpt report dynamically
//			CrystalDecisions.Shared.TableLogOnInfo tliCurrent;
//			//Loop through all tables in report and apply connection information for each table.
//            // Begin TT#1054 - JSmith - Relieve Intransit not working.
//            //String con=ConfigurationSettings.AppSettings["ConnectionString"];
//            String con = MIDConfigurationManager.AppSettings["ConnectionString"];
//            // End TT#1054
//			char[]qute=new char[]{';'};
//			String[] constr=new String[4];
//			int j=0;
//			foreach(String s in con.Split(qute))
//			{
//				int i=s.IndexOf("=");
//				if(i == 0)
//					continue;
//				String st=s.Substring(i+1);
//				if(st.Trim() !="")
//				{
//					constr[j]=st;
//					j++;

//				}

//			}

//			server=constr[0].ToString();
//			database=constr[1].ToString();
//			userid=constr[2].ToString();
//			pass=constr[3].ToString();
//			foreach(Table tbl in crDoc.Database.Tables)
//			{
//				tliCurrent = tbl.LogOnInfo;
//				//Change ONLY the connection string info, leave other table info alone!
//				tliCurrent.ConnectionInfo.ServerName = server;    //physical server name
//				tliCurrent.ConnectionInfo.DatabaseName = database;
//				tliCurrent.ConnectionInfo.UserID = userid;
//				tliCurrent.ConnectionInfo.Password = pass;
//				tbl.ApplyLogOnInfo(tliCurrent);
//			}
//			///////////////////////////////////////////////////////////////////////
			
//			//Set viewer to the report object to be previewed.
//			return crDoc;

//		}
//		#endregion 
//	}
//}
