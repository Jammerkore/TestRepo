using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;


namespace MIDRetail.MIDRegExtract
{
    class MIDRegExtract
    {
        static int iExitCode = 0;
        static string keyName = null;
        static string fileName = null;
        static bool deleteKey = false;
        const string cRegistryRootKey = "MIDRetailInc";
        const string cRegistrySoftwareKey = @"Software\Wow6432Node";

        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                keyName = args[0];
                fileName = args[1];
                if (keyName == null ||
                    fileName == null)
                {
                    iExitCode = 1;
                }
                else
                {
                    if (args.Length > 2)
                    {
                        try
                        {
                            //EventLog.WriteEntry("MIDRetail", "deleteKey=" + args[2], EventLogEntryType.Error);
                            deleteKey = Convert.ToBoolean(args[2]);
                        }
                        catch
                        {
                        }
                    }
                    //EventLog.WriteEntry("MIDRetail", "Before ExtractRegistryEntries", EventLogEntryType.Error);
                    ExtractRegistryEntries(keyName, fileName);
                    //EventLog.WriteEntry("MIDRetail", "After ExtractRegistryEntries", EventLogEntryType.Error);
                    if (deleteKey)
                    {
                        RemoveRegisteredComponents();
                    }
                }
            }
            Environment.Exit(iExitCode);
        }

        private static void ExtractRegistryEntries(string aKeyName, string aFileName)
        {
            string hkValue;
            string[] hkValueNames;
            StreamWriter writer = null;
            RegistryKey hklm;
            RegistryKey hkSoftware;
            RegistryKey hkKey;
            RegistryKey hkSubKey;

            try
            {
                hklm = Registry.LocalMachine;
                hkSoftware = hklm.OpenSubKey(cRegistrySoftwareKey);
                hkKey = hkSoftware.OpenSubKey(aKeyName);

                if (hkKey == null)
                {
                    iExitCode = 1;
                    return;
                }

                writer = new StreamWriter(aFileName);

                string[] subkeynames = hkKey.GetSubKeyNames();

                foreach (string subkeyname in subkeynames)
                {
                    hkSubKey = hkKey.OpenSubKey(subkeyname);
                    hkValueNames = hkSubKey.GetValueNames();
                    foreach (string hkValueName in hkValueNames)
                    {
                        hkValue = hkSubKey.GetValue(hkValueName).ToString().Trim();
                        writer.WriteLine(subkeyname + "|" + hkValueName + "|" + hkValue);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDRetail", ex.ToString(), EventLogEntryType.Error);
                iExitCode = 1;
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
            
        }

        static private void RemoveRegisteredComponents()
        {
            try
            {
                //EventLog.WriteEntry("MIDRetail", "In RemoveRegisteredComponents" , EventLogEntryType.Error);
                //drill down to the install registry entries
                RegistryKey local_key = Registry.LocalMachine;
                RegistryKey soft_key = local_key.OpenSubKey(cRegistrySoftwareKey, true);
                RegistryKey mid_key = soft_key.OpenSubKey(cRegistryRootKey, true);

                //if the install registy entries exist
                if (mid_key != null)
                {
                    //if the install registry has subkeys
                    if (mid_key.SubKeyCount > 0)
                    {
                        //get a list of sub keys
                        string[] subKeyNames = mid_key.GetSubKeyNames();

                        //delete sub keys
                        foreach (string subKeyName in subKeyNames)
                        {
                            mid_key.DeleteSubKey(subKeyName);
                        }

                        //delete the mid install key
                        soft_key.DeleteSubKey(cRegistryRootKey);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDRetail", ex.ToString(), EventLogEntryType.Error);
                throw;
            }
        }
    }
}
