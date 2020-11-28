using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Data
{
    public partial class EnvironmentData : DataLayer
    {
        public EnvironmentData() : base()
		{

		}

        public bool ServiceStart_Add(int aServiceID, DateTime aStartDate, string aVersion)
        {
            try
            {
                ServiceStart_Delete(aServiceID);

                int rowsInserted = StoredProcedures.MID_APPLICATION_SERVICES_HISTORY_INSERT.Insert(_dba,
                                                                                                    SERVICE_EPROCESSES_ID: aServiceID,
                                                                                                    SERVICE_START_DATETIME: aStartDate,
                                                                                                    SERVICE_VERSION: aVersion
                                                                                                    );
                return (rowsInserted > 0);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public bool ServiceStart_Delete(int aServiceID)
        {
            try
            {
                int rowsDeleted = StoredProcedures.MID_APPLICATION_SERVICES_HISTORY_DELETE.Delete(_dba, SERVICE_EPROCESSES_ID: aServiceID);
                return (rowsDeleted > 0);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public ServicesProfileList GetServicesInformation()
        {
            ServicesProfileList pl = new ServicesProfileList(eProfileType.Service);
            ServiceProfile sp;

            try
            {
                DataTable dt = StoredProcedures.MID_APPLICATION_SERVICES_HISTORY_READ_ALL.Read(_dba);

                foreach (DataRow dr in dt.Rows)
                {
                    sp = new ServiceProfile(Convert.ToInt32(dr["SERVICE_EPROCESSES_ID"]));
                    sp.StartDateTime = Convert.ToDateTime(dr["SERVICE_START_DATETIME"]);
                    sp.Version = Convert.ToString(dr["SERVICE_VERSION"]);
                    pl.Add(sp);
                }

                return pl;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        public ServiceProfile GetServicesInformation(eProcesses aServiceID)
        {
            ServiceProfile sp = null;

            try
            {
                DataTable dt = StoredProcedures.MID_APPLICATION_SERVICES_HISTORY_READ_FROM_PROCESS.Read(_dba, SERVICE_EPROCESSES_ID: (int)aServiceID);

                if (dt.Rows.Count > 0)
                {
                    sp = new ServiceProfile(Convert.ToInt32(dt.Rows[0]["SERVICE_EPROCESSES_ID"]));
                    sp.StartDateTime = Convert.ToDateTime(dt.Rows[0]["SERVICE_START_DATETIME"]);
                    sp.Version = Convert.ToString(dt.Rows[0]["SERVICE_VERSION"]);
                }

                return sp;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }


        public UpgradeProfile GetUpgradeInformation()
        {

            try
            {
                UpgradeProfile up = new UpgradeProfile(Include.NoRID);
                DataTable dt = StoredProcedures.MID_APPLICATION_SERVICES_HISTORY_READ_LAST_UPGRADE.Read(_dba);

                if (dt.Rows.Count > 0)
                {
                    up.UpgradeVersion = Convert.ToString(dt.Rows[0]["UPGRADE_VERSION"]);
                    up.UpgradeDateTime = Convert.ToDateTime(dt.Rows[0]["UPGRADE_DATETIME"]);
                    up.UpgradeUser = Convert.ToString(dt.Rows[0]["UPGRADE_USER"]);
                    up.UpgradeMachine = Convert.ToString(dt.Rows[0]["UPGRADE_MACHINE"]);
                    up.UpgradeRemoteMachine = Convert.ToString(dt.Rows[0]["UPGRADE_REMOTE_MACHINE"]);
                    up.UpgradeConfiguration = Convert.ToString(dt.Rows[0]["UPGRADE_CONFIGURATION"]);
                }

                return up;

            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        //Begin TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions
        public DataTable ReadCustomConversionsForImmediateExecution()
        {

            try
            {
                return StoredProcedures.MID_APPLICATION_DB_CONVERSION_QUEUE_READ_IMMEDIATE.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public DataTable ReadCustomConversionsForDeferredExecution()
        {

            try
            {
                return StoredProcedures.MID_APPLICATION_DB_CONVERSION_QUEUE_READ_DEFERRED.Read(_dba);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        public void RemoveCustomConversionFromQueue(string functionName)
        {
            try
            {
                try
                {
                    _dba.OpenUpdateConnection();
                    int rowsDeleted = StoredProcedures.MID_APPLICATION_DB_CONVERSION_QUEUE_DELETE_FROM_FUNCTION.Delete(_dba, CC_FUNCTION_NAME: functionName);
                    _dba.CommitData();
                }
                finally
                {
                    _dba.CloseUpdateConnection();
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        //End TT#1356-MD -jsobek -SQL Upgrade - Custom Conversions
    }
}
