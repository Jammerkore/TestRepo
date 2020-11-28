using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.LicenseKeyGen
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class LicenseKeyGen
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
			StreamWriter writer = null;
			bool allocationInstalled = false;
			int allocationExpiration = 0;
			bool planningInstalled = false;
			int planningExpiration = 0;
			bool sizeInstalled = false;
			int sizeExpiration = 0;
			bool assortmentInstalled = false;
			int assortmentExpiration = 0;
            bool groupAllocationInstalled = false;	// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
            int groupAllocationExpiration = 0;		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
			bool masterInstalled = false;
			int masterExpiration = 0;

			bool installSuccessful = false;

			public int GenFile(string[] args)
			{
				try
				{
					string fileName = MIDConfigurationManager.AppSettings["KeyFile"];
					if (fileName == null)
					{
						Console.WriteLine("file name not found");
						return 1;
					}

					writer = new StreamWriter(@".\" + fileName);
					string keyValue = MIDConfigurationManager.AppSettings["Allocation"];
					if (keyValue != null)
					{
						try
						{
							allocationInstalled = Convert.ToBoolean(keyValue);
						}
						catch
						{
						}
					}
					keyValue = MIDConfigurationManager.AppSettings["AlExp"];
					if (keyValue != null)
					{
						try
						{
							allocationExpiration = Convert.ToInt32(keyValue);
						}
						catch
						{
						}
					}

					keyValue = MIDConfigurationManager.AppSettings["Planning"];
					if (keyValue != null)
					{
						try
						{
							planningInstalled = Convert.ToBoolean(keyValue);
						}
						catch
						{
						}
					}
					keyValue = MIDConfigurationManager.AppSettings["PlExp"];
					if (keyValue != null)
					{
						try
						{
							planningExpiration = Convert.ToInt32(keyValue);
						}
						catch
						{
						}
					}

					keyValue = MIDConfigurationManager.AppSettings["Size"];
					if (keyValue != null)
					{
						try
						{
							sizeInstalled = Convert.ToBoolean(keyValue);
						}
						catch
						{
						}
					}
					keyValue = MIDConfigurationManager.AppSettings["SzExp"];
					if (keyValue != null)
					{
						try
						{
							sizeExpiration = Convert.ToInt32(keyValue);
						}
						catch
						{
						}
					}

					keyValue = MIDConfigurationManager.AppSettings["Assortment"];
					if (keyValue != null)
					{
						try
						{
							assortmentInstalled = Convert.ToBoolean(keyValue);
						}
						catch
						{
						}
					}
					keyValue = MIDConfigurationManager.AppSettings["AsExp"];
					if (keyValue != null)
					{
						try
						{
							assortmentExpiration = Convert.ToInt32(keyValue);
						}
						catch
						{
						}
					}

					// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                    keyValue = MIDConfigurationManager.AppSettings["GroupAllocation"];
                    if (keyValue != null)
                    {
                        try
                        {
                            groupAllocationInstalled = Convert.ToBoolean(keyValue);
                        }
                        catch
                        {
                        }
                    }
                    keyValue = MIDConfigurationManager.AppSettings["GaExp"];
                    if (keyValue != null)
                    {
                        try
                        {
                            groupAllocationExpiration = Convert.ToInt32(keyValue);
                        }
                        catch
                        {
                        }
                    }
					// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

					keyValue = MIDConfigurationManager.AppSettings["Master"];
					if (keyValue != null)
					{
						try
						{
							masterInstalled = Convert.ToBoolean(keyValue);
						}
						catch
						{
						}
					}
					keyValue = MIDConfigurationManager.AppSettings["MasterExp"];
					if (keyValue != null)
					{
						try
						{
							masterExpiration = Convert.ToInt32(keyValue);
						}
						catch
						{
						}
					}

					AppConfig appConfig = new AppConfig();
					string key = appConfig.BuildLicenseKey(allocationInstalled, allocationExpiration,
						sizeInstalled, sizeExpiration,
						planningInstalled, planningExpiration,
						assortmentInstalled, assortmentExpiration,
                        groupAllocationInstalled, groupAllocationExpiration,	// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
						masterInstalled, masterExpiration);

					writer.WriteLine(key);
					installSuccessful = true;

					return 0;
				}
				catch (Exception ex)
				{
					Console.WriteLine("License key generate failed - " + ex.ToString());
					Console.Read();
					string message = ex.ToString();
					throw;
				}
				finally
				{
					if (writer != null)
					{
						writer.Close();
					}
					if (installSuccessful)
					{
						Console.WriteLine("License key generated successfully");
						Console.Read();
					}
				}
			}

		}
	}
}
