using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.DatabaseUpdate;

namespace MIDRetail.DatabaseUpdate
{
	public class LicenseKeyRoutines
	{
		static LicenseKeyRoutines()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		static public bool LoadLicenseKey(Queue aMessageQueue, string aConnString)
		{
			StreamReader reader = null;
			GlobalOptions globalOptions = null; 
			InfragisticsLayoutData infragistics = null;
			try
			{
                // Begin TT#924 - JSmith - Files not found when creating new database using Installer
                //string fileName = MIDConfigurationManager.AppSettings["KeyFile"];
                string fileName = Path.GetDirectoryName(Application.ExecutablePath) + @"\" + MIDConfigurationManager.AppSettings["KeyFile"];
                // End TT#924
				if (fileName == null)
				{
					aMessageQueue.Enqueue("LicenseKey parameter not found in configuration file");
					return false;
				}
				else if (!File.Exists(fileName))
				{
					aMessageQueue.Enqueue("License Key file not found");
					return false;
				}

				reader = new StreamReader(fileName);
				string licenseKey = reader.ReadLine();
                
                

                //begin TT#1130 - Database Utility  does not always connect across the network
                //globalOptions = new GlobalOptions("server=" + aServer + ";database=" + aDatabase + ";uid=" + aUser + ";pwd=" + aPassword + ";");
                globalOptions = new GlobalOptions(aConnString);

                //GlobalOptionsProfile globalOptionsProfile = new GlobalOptionsProfile(Include.NoRID);
                //globalOptionsProfile.LoadOptions();

                //end TT#1130

                int origHashCode = GetLicenseHashCode();	// TT#862 - MD - stodd - Assortment Upgrade Issues

				globalOptions.WriteLicenseKey(1, licenseKey);

                int newHashCode = GetLicenseHashCode();		// TT#862 - MD - stodd - Assortment Upgrade Issues
                //begin TT#1130 - Database Utility  does not always connect across the network

				// clear Infragistics menu layouts
                //infragistics = new InfragisticsLayoutData("server=" + aServer + ";database=" + aDatabase + ";uid=" + aUser + ";pwd=" + aPassword + ";");
                infragistics = new InfragisticsLayoutData(aConnString);

                //end TT#1130

				infragistics.InfragisticsLayout_Delete(eLayoutID.explorerToolbar);

				// Begin TT#862 - MD - stodd - Assortment Upgrade Issues
                if (origHashCode != newHashCode)
                {
                    infragistics.InfragisticsLayout_Delete(eLayoutID.explorerDock);
                    infragistics.InfragisticsLayout_Delete(eLayoutID.allocationWorkspaceGrid);
                }
				// End TT#862 - MD - stodd - Assortment Upgrade Issues

				aMessageQueue.Enqueue("License key successfully installed");
				return true;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				aMessageQueue.Enqueue("License key installed failed - " + message);
				return false;
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}
		}

		// Begin TT#862 - MD - stodd - Assortment Upgrade Issues
        static private int GetLicenseHashCode()
        {
            int hashCode = 0;
            GlobalOptionsProfile gop = new GlobalOptionsProfile(Include.NoRID);
            gop.LoadOptions();
            hashCode = gop.AppConfig.GetHashCode();
            return hashCode;
        }
		// End TT#862 - MD - stodd - Assortment Upgrade Issues
	}
}
