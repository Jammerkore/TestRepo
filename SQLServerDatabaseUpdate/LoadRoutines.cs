// Begin Track #4637 - JSmith - Split variables by type
// Too many lines changed to mark.  Use SCM Compare for details.
// End Track #4637
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.ForecastComputations;

namespace MIDRetail.DatabaseUpdate
{
	public class LoadRoutines
	{
		static ArrayList _keyColumns = new ArrayList();
        static ArrayList _extractKeyColumns = new ArrayList();  // TT#2131-MD - JSmith - Halo Integration
		//Begin TT#173 - JScott - Provide Container for large data Collections #6
		//const string cWildcard = "%";
		const string cWildcard = "_";
		//End TT#173 - JScott - Provide Container for large data Collections #6

		static LoadRoutines()
		{
			_keyColumns.Add("HN_MOD");
			_keyColumns.Add("HN_RID");
			_keyColumns.Add("FV_RID");
			_keyColumns.Add("TIME_ID");
			_keyColumns.Add("ST_RID");
            // Begin TT#2131-MD - JSmith - Halo Integration
            _extractKeyColumns.Add("Merchandise");
            _extractKeyColumns.Add("Version");
            _extractKeyColumns.Add("Attribute");
            _extractKeyColumns.Add("AttributeSet");
            _extractKeyColumns.Add("TimePeriod");
            _extractKeyColumns.Add("Filter");
            _extractKeyColumns.Add("Store");
            // End TT#2131-MD - JSmith - Halo Integration
        }

		static public bool LoadComputations(Queue aMessageQueue, string aConnString)
		{
			FileStream file;
			long fileSize;
			BinaryReader reader;
			byte[] dllObject;
			string version;
			ComputationsDLLData compDLLDL;
            // Begin TT#924 - JSmith - Files not found when creating new database using Installer
            string computationsAssemblyName;
            // End TT#924
			
			try
			{
                // Begin TT#924 - JSmith - Files not found when creating new database using Installer
                computationsAssemblyName = Path.GetDirectoryName(Application.ExecutablePath) + @"\" + Include.ComputationsAssemblyName;
                // End TT#924
				try
				{
                    // Begin TT#924 - JSmith - Files not found when creating new database using Installer
                    //file = File.Open(Include.ComputationsAssemblyName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    file = File.Open(computationsAssemblyName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    // End TT#924
					fileSize = file.Length;
					reader = new BinaryReader(file);
					dllObject = reader.ReadBytes((int)fileSize);
					reader.Close();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					aMessageQueue.Enqueue("Error encountered during open and read of Computations DLL." + message);
					throw;
				}

				try
				{
                    // Begin TT#924 - JSmith - Files not found when creating new database using Installer
                    //version = System.Diagnostics.FileVersionInfo.GetVersionInfo(Include.ComputationsAssemblyName).ProductVersion;
                    version = System.Diagnostics.FileVersionInfo.GetVersionInfo(computationsAssemblyName).ProductVersion;
                    // End TT#924
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					aMessageQueue.Enqueue("Error encountered during retrieval of version information from Computations DLL");
					throw;
				}


                //begin TT#1130 - Database Utility  does not always connect across the network
				//compDLLDL = new ComputationsDLLData("server=" + aServer + ";database=" + aDatabase + ";uid=" + aUser + ";pwd=" + aPassword + ";");
                compDLLDL = new ComputationsDLLData(aConnString);
                //end TT#1130 

				try
				{
					if (!compDLLDL.ComputationsDLL_Exists(version))
					{
						compDLLDL.OpenUpdateConnection();
						compDLLDL.ComputationsDLL_Insert(version, dllObject);
						compDLLDL.CommitData();
						compDLLDL.CloseUpdateConnection();

						aMessageQueue.Enqueue("Computations were successfully applied.");
						return true;
					}
					else
					{
						aMessageQueue.Enqueue("Computations were already applied.");  // TT#1468 - fixed verbiage here - GRT
						return true;
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					aMessageQueue.Enqueue("Error encountered during update of COMPUTATIONS_DLL table");
					throw;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        //begin TT#1130 - Database Utility  does not always connect across the network - apicchetti - 02/11/11
        static public bool UpdateTables(bool isROExtractDatabase, Queue aMessageQueue, string aConnString)
		//static public bool UpdateTables(Queue aMessageQueue, string aServer, string aDatabase, eDatabaseType aDatabaseType, string aUser, string aPassword)
        //end TT#1130 - Database Utility  does not always connect across the network - apicchetti - 02/11/11
		{
			ComputationsDLLData compDLLDL;
			string tableMask = null;
			
			try
			{
				compDLLDL = new ComputationsDLLData(aConnString);
				PlanComputationsCollection compCollections = new PlanComputationsCollection();
				IPlanComputationVariables variables = compCollections.GetDefaultComputations().PlanVariables;

				try
				{
                    // Begin TT#2131-MD - JSmith - Halo Integration
                    if (isROExtractDatabase)
                    {
                        IPlanComputationTimeTotalVariables totalVariables = compCollections.GetDefaultComputations().PlanTimeTotalVariables; 
						tableMask = Include.DBROExtractPlanningChainTable;
                        if (!GetExtractTables(false, compDLLDL, tableMask, variables.GetChainWeeklyVariableList(), aMessageQueue, false, eVariableCategory.Chain))
                        {
                            return false;
                        }

                        tableMask = Include.DBROExtractPlanningChainTotalTable;
                        if (!GetExtractTables(true, compDLLDL, tableMask, totalVariables.GetChainTimeTotalVariableList(), aMessageQueue, false, eVariableCategory.Chain))
                        {
                            return false;
                        }

                        tableMask = Include.DBROExtractPlanningStoresTable;
                        if (!GetExtractTables(false, compDLLDL, tableMask, variables.GetStoreWeeklyVariableList(), aMessageQueue, false, eVariableCategory.Store))
                        {
                            return false;
                        }

                        tableMask = Include.DBROExtractPlanningStoresTotalTable;
                        if (!GetExtractTables(true, compDLLDL, tableMask, totalVariables.GetStoreTimeTotalVariableList(), aMessageQueue, false, eVariableCategory.Store))
                        {
                            return false;
                        }

                        tableMask = Include.DBROExtractPlanningAttributesTable;
                        if (!GetExtractTables(false, compDLLDL, tableMask, variables.GetStoreWeeklyVariableList(), aMessageQueue, false, eVariableCategory.Store))
                        {
                            return false;
                        }

                        tableMask = Include.DBROExtractPlanningAttributesTotalTable;
                        if (!GetExtractTables(true, compDLLDL, tableMask, totalVariables.GetStoreTimeTotalVariableList(), aMessageQueue, false, eVariableCategory.Store))
                        {
                            return false;
                        }

                        return true;
                    }
                    // End TT#2131-MD - JSmith - Halo Integration

					tableMask = Include.DBChainWeeklyHistoryTable;
                    if (!GetTables(compDLLDL, tableMask, variables.GetChainWeeklyHistoryDatabaseVariableList(), aMessageQueue, false, eVariableCategory.Chain))
					{
						return false;
					}

					tableMask = Include.DBChainWeeklyForecastTable;
                    if (!GetTables(compDLLDL, tableMask, variables.GetChainWeeklyForecastDatabaseVariableList(), aMessageQueue, false, eVariableCategory.Chain))
					{
						return false;
					}

					tableMask = Include.DBChainWeeklyLockTable;
                    if (!GetTables(compDLLDL, tableMask, variables.GetChainWeeklyForecastDatabaseVariableList(), aMessageQueue, true, eVariableCategory.Chain))
					{
						return false;
					}

					//Begin TT#173 - JScott - Provide Container for large data Collections #6
					//tableMask = Include.DBStoreDailyHistoryTable.Replace(Include.DBTableCountReplaceString, cWildcard);
					//if (!GetTables(compDLLDL, tableMask, variables.GetStoreDailyHistoryDatabaseVariableList(), aMessageQueue, aServer, aDatabase, aDatabaseType, aUser, aPassword, false, eVariableCategory.Chain))
					tableMask = Include.DBStoreDailyHistoryTable;
					if (!GetTables(compDLLDL, tableMask, variables.GetStoreDailyHistoryDatabaseVariableList(), aMessageQueue, false, eVariableCategory.Chain, true))
					//End TT#173 - JScott - Provide Container for large data Collections #6
					{
						return false;
					}

					//Begin TT#173 - JScott - Provide Container for large data Collections #6
					//tableMask = Include.DBStoreWeeklyHistoryTable.Replace(Include.DBTableCountReplaceString, cWildcard);
					//if (!GetTables(compDLLDL, tableMask, variables.GetStoreWeeklyHistoryDatabaseVariableList(), aMessageQueue, aServer, aDatabase, aDatabaseType, aUser, aPassword, false, eVariableCategory.Chain))
					tableMask = Include.DBStoreWeeklyHistoryTable;
					if (!GetTables(compDLLDL, tableMask, variables.GetStoreWeeklyHistoryDatabaseVariableList(), aMessageQueue, false, eVariableCategory.Chain, true))
					//End TT#173 - JScott - Provide Container for large data Collections #6
					{
						return false;
					}

					//Begin TT#173 - JScott - Provide Container for large data Collections #6
					//tableMask = Include.DBStoreWeeklyForecastTable.Replace(Include.DBTableCountReplaceString, cWildcard);
					//if (!GetTables(compDLLDL, tableMask, variables.GetStoreWeeklyForecastDatabaseVariableList(), aMessageQueue, aServer, aDatabase, aDatabaseType, aUser, aPassword, false, eVariableCategory.Chain))
					tableMask = Include.DBStoreWeeklyForecastTable;
					if (!GetTables(compDLLDL, tableMask, variables.GetStoreWeeklyForecastDatabaseVariableList(), aMessageQueue, false, eVariableCategory.Chain, true))
					//End TT#173 - JScott - Provide Container for large data Collections #6
					{
						return false;
					}

					tableMask = Include.DBStoreWeeklyLockTable;
                    if (!GetTables(compDLLDL, tableMask, variables.GetStoreWeeklyForecastDatabaseVariableList(), aMessageQueue, true, eVariableCategory.Chain))
					{
						return false;
					}

					return true;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					aMessageQueue.Enqueue("Error encountered during update of COMPUTATIONS_DLL table");
					throw;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}



        static public bool GetTables(ComputationsDLLData aCompDLLDL, string aTableMask, ProfileList aVariables, Queue aMessageQueue, bool aLockTable, eVariableCategory aVariableCategory, bool aReplaceWildcards = false)
		//End TT#173 - JScott - Provide Container for large data Collections #6
		{
			DataTable dtTables;
			string SQLCommand = null;
			ArrayList alTables = new ArrayList(); 
			bool addTable;
			string dbTableName;
			//Begin TT#173 - JScott - Provide Container for large data Collections #6
			string likeStmt;
			//End TT#173 - JScott - Provide Container for large data Collections #6
			try
			{
				try
				{
					//Begin TT#173 - JScott - Provide Container for large data Collections #6
					//SQLCommand = "select table_name as Name "
					//        + " from  INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE'"
					//        + " and table_name like '" + aTableMask + "'";
					if (aReplaceWildcards)
					{
						likeStmt = "(table_name like '" + aTableMask.Replace(Include.DBTableCountReplaceString, cWildcard) + "' or " +
								   "table_name like '" + aTableMask.Replace(Include.DBTableCountReplaceString, cWildcard + cWildcard) + "')";
					}
					else
					{
						likeStmt = "table_name like '" + aTableMask + "'";
					}

					SQLCommand = "select table_name as Name "
							+ " from  INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE'"
							+ " and " + likeStmt;

					//End TT#173 - JScott - Provide Container for large data Collections #6
					dtTables = aCompDLLDL.ComputationsDLL_GetTables(SQLCommand);
					foreach (DataRow dr in dtTables.Rows)
					{
						addTable = false;
						dbTableName = Convert.ToString(dr["Name"], CultureInfo.CurrentCulture);
						if (dbTableName.IndexOf(Include.cLockExtension) > 0)
						{
							if (aLockTable)
							{
								addTable = true;
							}
						}
						else
						{
							if (!aLockTable)
							{
								addTable = true;
							}
						}
						if (addTable)
						{
							alTables.Add(dbTableName);
						}
					}
					
					if (alTables.Count > 0)
					{
						foreach (string tableName in alTables)
						{
							if (!UpdateTable(aCompDLLDL, tableName, aVariables, aMessageQueue, aLockTable, aVariableCategory))
							{
								return false;
							}
						}
					}

					return true;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					aMessageQueue.Enqueue("Error encountered reading table names from database");
					throw;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
		static public bool UpdateTable(ComputationsDLLData aCompDLLDL, string aTableName, ProfileList aVariables, Queue aMessageQueue, bool aLockTable, eVariableCategory aVariableCategory)
		{
			DataTable tableDefn;
			bool columnFound;
			
			try
			{
				try
				{
					tableDefn = aCompDLLDL.ComputationsDLL_ReadTableDefn(aTableName);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					aMessageQueue.Enqueue("Error encountered reading column names of table " + aTableName);
					throw;
				}

				if (tableDefn == null) 
				{
					aMessageQueue.Enqueue("Error encountered reading column names of table " + aTableName);
					return false;
				}
				else
				{
					try
					{
						aCompDLLDL.OpenUpdateConnection();
						// add new columns
						foreach (VariableProfile vp in aVariables)
						{
							if (aLockTable)
							{
								if (!tableDefn.Columns.Contains(vp.DatabaseColumnName + Include.cLockExtension))
								{
									AddLockColumn(aCompDLLDL, aTableName, vp);
									aMessageQueue.Enqueue("Column " + vp.DatabaseColumnName + Include.cLockExtension + " successfully added to table " + aTableName);
								}
							}
							else
							{
								if (!tableDefn.Columns.Contains(vp.DatabaseColumnName))
								{
									AddColumn(aCompDLLDL, aTableName, vp, aVariableCategory);
									aMessageQueue.Enqueue("Column " + vp.DatabaseColumnName + " successfully added to table " + aTableName);
								}
							}
						}

                        //bool stop = true;
						// remove old columns
						foreach (System.Data.DataColumn tableColumn in tableDefn.Columns)
						{
							if (aLockTable)
							{
								if (!_keyColumns.Contains(tableColumn.ColumnName))
								{
									columnFound = false;
									foreach (VariableProfile vp in aVariables)
									{
										if (vp.DatabaseColumnName + Include.cLockExtension == tableColumn.ColumnName)
										{
											columnFound = true;
											break;
										}
									}
									if (!columnFound)
									{
										AddDropColumn(aCompDLLDL, aTableName, tableColumn.ColumnName, aLockTable);
										aMessageQueue.Enqueue("Column " + tableColumn.ColumnName + " successfully dropped from table " + aTableName);
									}
								}
							}
							else
							{
								if (!_keyColumns.Contains(tableColumn.ColumnName))
								{
									columnFound = false;
									foreach (VariableProfile vp in aVariables)
									{
										if (vp.DatabaseColumnName == tableColumn.ColumnName)
										{
											columnFound = true;
											break;
										}
									}
									if (!columnFound)
									{
										AddDropColumn(aCompDLLDL, aTableName, tableColumn.ColumnName, aLockTable);
										aMessageQueue.Enqueue("Column " + tableColumn.ColumnName + " successfully dropped from table " + aTableName);
									}
								}
							}
						}

						aCompDLLDL.CommitData();
						return true;
					}
					catch
					{
						throw;
					}
					finally
					{
						if (aCompDLLDL.ConnectionIsOpen)
						{
							aCompDLLDL.CloseUpdateConnection();
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		static public bool AddColumn(ComputationsDLLData aCompDLLDL, string aTableName, VariableProfile aVariableProfile, eVariableCategory aVariableCategory)
		{
			try
			{
				string command = null;
				eVariableDatabaseType databaseVariableType;
				switch (aVariableCategory)
				{
					case eVariableCategory.Chain:
						databaseVariableType = aVariableProfile.ChainDatabaseVariableType;
						break;
					case eVariableCategory.Store:
						databaseVariableType = aVariableProfile.StoreDatabaseVariableType;
						break;
					default:
						databaseVariableType = eVariableDatabaseType.None;
						break;
				}
				switch (databaseVariableType)
				{
					case eVariableDatabaseType.Integer:
						command = "alter table " + aTableName + " add " + aVariableProfile.DatabaseColumnName + " INT NULL";
						break;
					case eVariableDatabaseType.Real:
						command = "alter table " + aTableName + " add " + aVariableProfile.DatabaseColumnName + " REAL NULL";
						break;
					case eVariableDatabaseType.DateTime:
						command = "alter table " + aTableName + " add " + aVariableProfile.DatabaseColumnName + " DateTime NULL";
						break;
					case eVariableDatabaseType.String:
						command = "alter table " + aTableName + " add " + aVariableProfile.DatabaseColumnName + " VARCHAR(100) NULL";
						break;
					case eVariableDatabaseType.Char:
						command = "alter table " + aTableName + " add " + aVariableProfile.DatabaseColumnName + " CHAR(1) NULL";
						break;
					case eVariableDatabaseType.Float:
						command = "alter table " + aTableName + " add " + aVariableProfile.DatabaseColumnName + " FLOAT NULL";
						break;
					case eVariableDatabaseType.BigInteger:
						command = "alter table " + aTableName + " add " + aVariableProfile.DatabaseColumnName + " BIGINT NULL";
						break;
				}
				if (command != null)
				{
					aCompDLLDL.ComputationsDLL_AddColumn(command);
					//Begin TT#889 - JScott - Database Upgrade hangs when loading Computations
					aCompDLLDL.CommitData();
					//End TT#889 - JScott - Database Upgrade hangs when loading Computations
				}
				return true;
			}
			catch
			{
				throw;
			}
			
		}

		static public bool AddLockColumn(ComputationsDLLData aCompDLLDL, string aTableName, VariableProfile aVariableProfile)
		{
			try
			{
				string command = "alter table " + aTableName + " add " + aVariableProfile.DatabaseColumnName + Include.cLockExtension + " char(1) default '0' null";
						
				aCompDLLDL.ComputationsDLL_AddColumn(command);
				//Begin TT#889 - JScott - Database Upgrade hangs when loading Computations
				aCompDLLDL.CommitData();
				//End TT#889 - JScott - Database Upgrade hangs when loading Computations
				return true;
			}
			catch
			{
				throw;
			}
			
		}

        // Begin TT#2131-MD - JSmith - Halo Integration
        static public bool GetExtractTables(bool isTimeTotalTable, ComputationsDLLData aCompDLLDL, string aTableMask, ProfileList aVariables, Queue aMessageQueue, bool aLockTable, eVariableCategory aVariableCategory, bool aReplaceWildcards = false)
        {
            DataTable dtTables;
            string SQLCommand = null;
            ArrayList alTables = new ArrayList();
            bool addTable;
            string dbTableName;
            string likeStmt;
            try
            {
                try
                {
                    if (aReplaceWildcards)
                    {
                        likeStmt = "(table_name like '" + aTableMask.Replace(Include.DBTableCountReplaceString, cWildcard) + "' or " +
                                   "table_name like '" + aTableMask.Replace(Include.DBTableCountReplaceString, cWildcard + cWildcard) + "')";
                    }
                    else
                    {
                        likeStmt = "table_name like '" + aTableMask + "'";
                    }

                    SQLCommand = "select table_name as Name "
                            + " from  INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE'"
                            + " and " + likeStmt;

                    dtTables = aCompDLLDL.ComputationsDLL_GetTables(SQLCommand);
                    foreach (DataRow dr in dtTables.Rows)
                    {
                        addTable = false;
                        dbTableName = Convert.ToString(dr["Name"], CultureInfo.CurrentCulture);
                        if (dbTableName.IndexOf(Include.cLockExtension) > 0)
                        {
                            if (aLockTable)
                            {
                                addTable = true;
                            }
                        }
                        else
                        {
                            if (!aLockTable)
                            {
                                addTable = true;
                            }
                        }
                        if (addTable)
                        {
                            alTables.Add(dbTableName);
                        }
                    }

                    if (alTables.Count > 0)
                    {
                        foreach (string tableName in alTables)
                        {
                            if (!UpdateExtractTable(isTimeTotalTable, aCompDLLDL, tableName, aVariables, aMessageQueue, aLockTable, aVariableCategory))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    aMessageQueue.Enqueue("Error encountered reading table names from database");
                    throw;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        static public bool UpdateExtractTable(bool isTimeTotalTable, ComputationsDLLData aCompDLLDL, string aTableName, ProfileList aVariables, Queue aMessageQueue, bool aLockTable, eVariableCategory aVariableCategory)
        {
            DataTable tableDefn;
            bool columnFound;

            try
            {
                try
                {
                    tableDefn = aCompDLLDL.ComputationsDLL_ReadTableDefn(aTableName);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    aMessageQueue.Enqueue("Error encountered reading column names of table " + aTableName);
                    throw;
                }

                if (tableDefn == null)
                {
                    aMessageQueue.Enqueue("Error encountered reading column names of table " + aTableName);
                    return false;
                }
                else
                {
                    try
                    {
                        aCompDLLDL.OpenUpdateConnection();
                        if (isTimeTotalTable)
                        {
                            // add new columns
                            foreach (TimeTotalVariableProfile vp in aVariables)
                            {
                                if (aLockTable)
                                {
                                    if (!tableDefn.Columns.Contains(vp.VariableName + Include.cLockExtension))
                                    {
                                        AddExtractLockColumn(aCompDLLDL, aTableName, vp);
                                        aMessageQueue.Enqueue("Column " + vp.VariableName + Include.cLockExtension + " successfully added to table " + aTableName);
                                    }
                                }
                                else
                                {
                                    if (!tableDefn.Columns.Contains(vp.VariableName))
                                    {
                                        AddExtractColumn(aCompDLLDL, aTableName, vp, aVariableCategory);
                                        aMessageQueue.Enqueue("Column " + vp.VariableName + " successfully added to table " + aTableName);
                                    }
                                }
                            }

                            //bool stop = true;
                            // remove old columns
                            foreach (System.Data.DataColumn tableColumn in tableDefn.Columns)
                            {
                                if (aLockTable)
                                {
                                    if (!_extractKeyColumns.Contains(tableColumn.ColumnName))
                                    {
                                        columnFound = false;
                                        foreach (TimeTotalVariableProfile vp in aVariables)
                                        {
                                            if (vp.VariableName + Include.cLockExtension == tableColumn.ColumnName)
                                            {
                                                columnFound = true;
                                                break;
                                            }
                                        }
                                        if (!columnFound)
                                        {
                                            AddDropColumn(aCompDLLDL, aTableName, tableColumn.ColumnName, aLockTable);
                                            aMessageQueue.Enqueue("Column " + tableColumn.ColumnName + " successfully dropped from table " + aTableName);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!_extractKeyColumns.Contains(tableColumn.ColumnName))
                                    {
                                        columnFound = false;
                                        foreach (TimeTotalVariableProfile vp in aVariables)
                                        {
                                            if (vp.VariableName == tableColumn.ColumnName)
                                            {
                                                columnFound = true;
                                                break;
                                            }
                                        }
                                        if (!columnFound)
                                        {
                                            AddDropColumn(aCompDLLDL, aTableName, tableColumn.ColumnName, aLockTable);
                                            aMessageQueue.Enqueue("Column " + tableColumn.ColumnName + " successfully dropped from table " + aTableName);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // add new columns
                            foreach (VariableProfile vp in aVariables)
                            {
                                if (aLockTable)
                                {
                                    if (!tableDefn.Columns.Contains(vp.VariableName + Include.cLockExtension))
                                    {
                                        AddExtractLockColumn(aCompDLLDL, aTableName, vp);
                                        aMessageQueue.Enqueue("Column " + vp.VariableName + Include.cLockExtension + " successfully added to table " + aTableName);
                                    }
                                }
                                else
                                {
                                    if (!tableDefn.Columns.Contains(vp.VariableName))
                                    {
                                        AddExtractColumn(aCompDLLDL, aTableName, vp, aVariableCategory);
                                        aMessageQueue.Enqueue("Column " + vp.VariableName + " successfully added to table " + aTableName);
                                    }
                                }
                            }

                            //bool stop = true;
                            // remove old columns
                            foreach (System.Data.DataColumn tableColumn in tableDefn.Columns)
                            {
                                if (aLockTable)
                                {
                                    if (!_extractKeyColumns.Contains(tableColumn.ColumnName))
                                    {
                                        columnFound = false;
                                        foreach (VariableProfile vp in aVariables)
                                        {
                                            if (vp.VariableName + Include.cLockExtension == tableColumn.ColumnName)
                                            {
                                                columnFound = true;
                                                break;
                                            }
                                        }
                                        if (!columnFound)
                                        {
                                            AddDropColumn(aCompDLLDL, aTableName, tableColumn.ColumnName, aLockTable);
                                            aMessageQueue.Enqueue("Column " + tableColumn.ColumnName + " successfully dropped from table " + aTableName);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!_extractKeyColumns.Contains(tableColumn.ColumnName))
                                    {
                                        columnFound = false;
                                        foreach (VariableProfile vp in aVariables)
                                        {
                                            if (vp.VariableName == tableColumn.ColumnName)
                                            {
                                                columnFound = true;
                                                break;
                                            }
                                        }
                                        if (!columnFound)
                                        {
                                            AddDropColumn(aCompDLLDL, aTableName, tableColumn.ColumnName, aLockTable);
                                            aMessageQueue.Enqueue("Column " + tableColumn.ColumnName + " successfully dropped from table " + aTableName);
                                        }
                                    }
                                }
                            }
                        }

                        aCompDLLDL.CommitData();
                        return true;
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        if (aCompDLLDL.ConnectionIsOpen)
                        {
                            aCompDLLDL.CloseUpdateConnection();
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        static public bool AddExtractColumn(ComputationsDLLData aCompDLLDL, string aTableName, VariableProfile aVariableProfile, eVariableCategory aVariableCategory)
        {
            try
            {
                string command = null;
                eVariableDatabaseType databaseVariableType;
                // Begin RO-4468/TT#5816 - Extract Database Upgrade Failure
                //switch (aVariableCategory)
                //{
                //    case eVariableCategory.Chain:
                //        databaseVariableType = aVariableProfile.ChainDatabaseVariableType;
                //        break;
                //    case eVariableCategory.Store:
                //        databaseVariableType = aVariableProfile.StoreDatabaseVariableType;
                //        break;
                //    default:
                //        databaseVariableType = eVariableDatabaseType.None;
                //        break;
                //}
                switch (aVariableProfile.FormatType)
                {
                    case eValueFormatType.GenericNumeric:
                        databaseVariableType = eVariableDatabaseType.Real;
                        break;
                    default:
                        databaseVariableType = eVariableDatabaseType.String;
                        break;
                }
                // End RO-4468/TT#5816
                switch (databaseVariableType)
                {
                    case eVariableDatabaseType.Integer:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] INT NULL";
                        break;
                    case eVariableDatabaseType.Real:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] REAL NULL";
                        break;
                    case eVariableDatabaseType.DateTime:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] DateTime NULL";
                        break;
                    case eVariableDatabaseType.String:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] VARCHAR(100) NULL";
                        break;
                    case eVariableDatabaseType.Char:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] CHAR(1) NULL";
                        break;
                    case eVariableDatabaseType.Float:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] FLOAT NULL";
                        break;
                    case eVariableDatabaseType.BigInteger:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] BIGINT NULL";
                        break;
                }
                if (command != null)
                {
                    aCompDLLDL.ComputationsDLL_AddColumn(command);
                    aCompDLLDL.CommitData();
                }
                return true;
            }
            catch
            {
                throw;
            }

        }

        static public bool AddExtractColumn(ComputationsDLLData aCompDLLDL, string aTableName, TimeTotalVariableProfile aVariableProfile, eVariableCategory aVariableCategory)
        {
            try
            {
                string command = null;
                eVariableDatabaseType databaseVariableType;
                switch (aVariableProfile.FormatType)
                {
                    case eValueFormatType.GenericNumeric:
                        databaseVariableType = eVariableDatabaseType.Real;
                        break;
                    default:
                        databaseVariableType = eVariableDatabaseType.String;
                        break;
                }
                switch (databaseVariableType)
                {
                    case eVariableDatabaseType.Integer:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] INT NULL";
                        break;
                    case eVariableDatabaseType.Real:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] REAL NULL";
                        break;
                    case eVariableDatabaseType.DateTime:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] DateTime NULL";
                        break;
                    case eVariableDatabaseType.String:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] VARCHAR(50) NULL";
                        break;
                    case eVariableDatabaseType.Char:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] CHAR(1) NULL";
                        break;
                    case eVariableDatabaseType.Float:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] FLOAT NULL";
                        break;
                    case eVariableDatabaseType.BigInteger:
                        command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + "] BIGINT NULL";
                        break;
                }
                if (command != null)
                {
                    aCompDLLDL.ComputationsDLL_AddColumn(command);
                    aCompDLLDL.CommitData();
                }
                return true;
            }
            catch
            {
                throw;
            }

        }

        static public bool AddExtractLockColumn(ComputationsDLLData aCompDLLDL, string aTableName, VariableProfile aVariableProfile)
        {
            try
            {
                string command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + Include.cLockExtension + "] char(1) default '0' null";

                aCompDLLDL.ComputationsDLL_AddColumn(command);
                aCompDLLDL.CommitData();
                return true;
            }
            catch
            {
                throw;
            }

        }

        static public bool AddExtractLockColumn(ComputationsDLLData aCompDLLDL, string aTableName, TimeTotalVariableProfile aVariableProfile)
        {
            try
            {
                string command = "alter table " + aTableName + " add [" + aVariableProfile.VariableName + Include.cLockExtension + "] char(1) default '0' null";

                aCompDLLDL.ComputationsDLL_AddColumn(command);
                aCompDLLDL.CommitData();
                return true;
            }
            catch
            {
                throw;
            }

        }
        // End TT#2131-MD - JSmith - Halo Integration

        static public bool AddDropColumn(ComputationsDLLData aCompDLLDL, string aTableName, string aColumnName, bool aLockTable)
		{
			string command;
			string defaultConstraintName;
			try
			{
				if (aLockTable)
				{
					// remove default constraint
					defaultConstraintName = aCompDLLDL.ComputationsDLL_GetDefaultConstraintName(aTableName, aColumnName);
					//Begin Fix - JScott - Errors in Upgrade process
					//command = "alter table " + aTableName + " drop constraint " + defaultConstraintName;
					//aCompDLLDL.ExecuteNonQuery(command);
					if (defaultConstraintName != null)
					{
						command = "alter table " + aTableName + " drop constraint " + defaultConstraintName;
						aCompDLLDL.ExecuteNonQuery(command);
					}
					//End Fix - JScott - Errors in Upgrade process
				}

				command = "alter table " + aTableName + " drop column [" + aColumnName + "]";
						
				aCompDLLDL.ComputationsDLL_AddColumn(command);
				//Begin Fix - JScott - Errors in Upgrade process
				aCompDLLDL.CommitData();
				//End Fix - JScott - Errors in Upgrade process
				return true;
			}
			catch
			{
				throw;
			}
			
		}
	}
}
