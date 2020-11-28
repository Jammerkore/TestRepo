using System;
using System.Configuration;
using System.IO;

using MIDRetail.Data;
using MIDRetail.DataCommon;


namespace MIDRetail.LicenseKeyInstaller
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class LicenseKeyInstaller
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			try
			{
				GenWorker worker = new GenWorker();
				return worker.GenFile(args);
			}
			catch
			{
				return 1;
			}
		}

		public class GenWorker
		{
			StreamReader reader;
			GlobalOptions globalOptions = null; 
			InfragisticsLayoutData infragistics = null;
			public int GenFile(string[] args)
			{
				try
				{
					string fileName = MIDConfigurationManager.AppSettings["KeyFile"];
					if (fileName == null)
					{
						Console.WriteLine("LicenseKey not found");
						return 1;
					}

					reader = new StreamReader(@".\" + fileName);
					string licenseKey = reader.ReadLine();

					globalOptions = new GlobalOptions(MIDConfigurationManager.AppSettings["ConnectionString"]);
					globalOptions.WriteLicenseKey(1, licenseKey);

					// clear Infragistics menu layouts
					infragistics = new InfragisticsLayoutData(MIDConfigurationManager.AppSettings["ConnectionString"]);
					infragistics.InfragisticsLayout_Delete(eLayoutID.explorerToolbar);

					Console.WriteLine("License key successfully installed");
					Console.Read();
					return 0;
				}
				catch (Exception ex)
				{
					string message = ex.ToString();
					Console.WriteLine("License key installed failed - " + message);
					Console.Read();
					throw;
				}
				finally
				{
					if (reader != null)
					{
						reader.Close();
					}
				}
			}
		}
	}
}
