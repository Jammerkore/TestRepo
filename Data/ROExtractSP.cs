using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class ROExtractData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static RO_Database_Exists_def RO_Database_Exists = new RO_Database_Exists_def();
            public class RO_Database_Exists_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Database_Exists.SQL"

                public RO_Database_Exists_def()
                {
                    base.procedureName = "RO_Database_Exists";
                    base.procedureType = storedProcedureTypes.RecordCount;
                    base.tableNames.Add("RO_Database_Exists");
                }

                public int ReadRecordCount(DatabaseAccess _dba)
                {
                    lock (typeof(RO_Database_Exists_def))
                    {
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
                }
            }

            public static RO_Stores_Data_Write_def RO_Stores_Data_Write = new RO_Stores_Data_Write_def();
            public class RO_Stores_Data_Write_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Stores_Data_Write.SQL"

                private tableParameter dt;
			
			    public RO_Stores_Data_Write_def()
			    {
                    base.procedureName = "RO_Stores_Data_Write";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("RO_Stores_Data_Write");
			        dt = new tableParameter("@dt", "Stores_Data_Type", base.inputParameterList);
			    }

                [UnitTestMethodAttribute(BypassValidation = true)]
			    public int Insert(DatabaseAccess _dba, 
			                      DataTable dt
			                      )
			    {
                    lock (typeof(RO_Stores_Data_Write_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static RO_Stores_Data_Delete_def RO_Stores_Data_Delete = new RO_Stores_Data_Delete_def();
            public class RO_Stores_Data_Delete_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Stores_Data_Delete.SQL"

                private tableParameter dt;

                public RO_Stores_Data_Delete_def()
                {
                    base.procedureName = "RO_Stores_Data_Delete";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RO_Stores_Data_Delete");
                    dt = new tableParameter("@dt", "Stores_Data_Type", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Stores_Data_Delete_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static RO_Stores_Characteristics_Write_def RO_Stores_Characteristics_Write = new RO_Stores_Characteristics_Write_def();
            public class RO_Stores_Characteristics_Write_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Stores_Characteristics_Write.SQL"

                private tableParameter dt;

                public RO_Stores_Characteristics_Write_def()
                {
                    base.procedureName = "RO_Stores_Characteristics_Write";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RO_Stores_Characteristics_Write");
                    dt = new tableParameter("@dt", "Stores_Characteristics_Type", base.inputParameterList);
                }

                [UnitTestMethodAttribute(BypassValidation = true)]
                public int Insert(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Stores_Characteristics_Write_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static RO_Stores_Characteristics_Delete_def RO_Stores_Characteristics_Delete = new RO_Stores_Characteristics_Delete_def();
            public class RO_Stores_Characteristics_Delete_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Stores_Characteristics_Delete.SQL"

                private stringParameter Characteristic;

                public RO_Stores_Characteristics_Delete_def()
                {
                    base.procedureName = "RO_Stores_Characteristics_Delete";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RO_Stores_Characteristics_Delete");
                    Characteristic = new stringParameter("@Characteristic", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba,
                                  string Characteristic
                                  )
                {
                    lock (typeof(RO_Stores_Characteristics_Delete_def))
                    {
                        this.Characteristic.SetValue(Characteristic);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static RO_Hierarchy_Definition_Write_def RO_Hierarchy_Definition_Write = new RO_Hierarchy_Definition_Write_def();
            public class RO_Hierarchy_Definition_Write_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Hierarchy_Definition_Write.SQL"

                private tableParameter dt;

                public RO_Hierarchy_Definition_Write_def()
                {
                    base.procedureName = "RO_Hierarchy_Definition_Write";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RO_Hierarchy_Definition_Write");
                    dt = new tableParameter("@dt", "Hierarchy_Definition_Type", base.inputParameterList);
                }

                [UnitTestMethodAttribute(BypassValidation = true)]
                public int Insert(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Hierarchy_Definition_Write_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static RO_Hierarchy_Data_Write_def RO_Hierarchy_Data_Write = new RO_Hierarchy_Data_Write_def();
            public class RO_Hierarchy_Data_Write_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Hierarchy_Data_Write.SQL"

                private tableParameter dt;

                public RO_Hierarchy_Data_Write_def()
                {
                    base.procedureName = "RO_Hierarchy_Data_Write";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RO_Hierarchy_Data_Write");
                    dt = new tableParameter("@dt", "Hierarchy_Data_Type", base.inputParameterList);
                }

                [UnitTestMethodAttribute(BypassValidation = true)]
                public int Insert(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Hierarchy_Data_Write_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static RO_Hierarchy_Data_Delete_def RO_Hierarchy_Data_Delete = new RO_Hierarchy_Data_Delete_def();
            public class RO_Hierarchy_Data_Delete_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Hierarchy_Data_Delete.SQL"

                private tableParameter dt;

                public RO_Hierarchy_Data_Delete_def()
                {
                    base.procedureName = "RO_Hierarchy_Data_Delete";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("RO_Hierarchy_Data_Delete");
                    dt = new tableParameter("@dt", "Hierarchy_Data_Type", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Hierarchy_Data_Delete_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static RO_Planning_Chain_Write_def RO_Planning_Chain_Write = new RO_Planning_Chain_Write_def();
            public class RO_Planning_Chain_Write_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Planning_Chain_Write.SQL"

                private tableParameter dt;

                public RO_Planning_Chain_Write_def()
                {
                    base.procedureName = "RO_Planning_Chain_Write";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RO_Planning_Chain_Write");
                    dt = new tableParameter("@dt", "Planning_Chain_Type", base.inputParameterList);
                }

                [UnitTestMethodAttribute(BypassValidation = true)]
                public int Insert(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Planning_Chain_Write_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static RO_Planning_Chain_Total_Write_def RO_Planning_Chain_Total_Write = new RO_Planning_Chain_Total_Write_def();
            public class RO_Planning_Chain_Total_Write_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Planning_Chain_Total_Write.SQL"

                private tableParameter dt;

                public RO_Planning_Chain_Total_Write_def()
                {
                    base.procedureName = "RO_Planning_Chain_Total_Write";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RO_Planning_Chain_Total_Write");
                    dt = new tableParameter("@dt", "Planning_Chain_Total_Type", base.inputParameterList);
                }

                [UnitTestMethodAttribute(BypassValidation = true)]
                public int Insert(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Planning_Chain_Total_Write_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static RO_Planning_Stores_Write_def RO_Planning_Stores_Write = new RO_Planning_Stores_Write_def();
            public class RO_Planning_Stores_Write_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Planning_Stores_Write.SQL"

                private tableParameter dt;

                public RO_Planning_Stores_Write_def()
                {
                    base.procedureName = "RO_Planning_Stores_Write";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RO_Planning_Stores_Write");
                    dt = new tableParameter("@dt", "Planning_Stores_Type", base.inputParameterList);
                }

                [UnitTestMethodAttribute(BypassValidation = true)]
                public int Insert(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Planning_Stores_Write_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static RO_Planning_Stores_Total_Write_def RO_Planning_Stores_Total_Write = new RO_Planning_Stores_Total_Write_def();
            public class RO_Planning_Stores_Total_Write_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Planning_Stores_Total_Write.SQL"

                private tableParameter dt;

                public RO_Planning_Stores_Total_Write_def()
                {
                    base.procedureName = "RO_Planning_Stores_Total_Write";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RO_Planning_Stores_Total_Write");
                    dt = new tableParameter("@dt", "Planning_Stores_Total_Type", base.inputParameterList);
                }

                [UnitTestMethodAttribute(BypassValidation = true)]
                public int Insert(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Planning_Stores_Total_Write_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static RO_Planning_Attributes_Write_def RO_Planning_Attributes_Write = new RO_Planning_Attributes_Write_def();
            public class RO_Planning_Attributes_Write_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Planning_Attributes_Write.SQL"

                private tableParameter dt;

                public RO_Planning_Attributes_Write_def()
                {
                    base.procedureName = "RO_Planning_Attributes_Write";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RO_Planning_Attributes_Write");
                    dt = new tableParameter("@dt", "Planning_Attributes_Type", base.inputParameterList);
                }

                [UnitTestMethodAttribute(BypassValidation = true)]
                public int Insert(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Planning_Attributes_Write_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static RO_Planning_Attributes_Total_Write_def RO_Planning_Attributes_Total_Write = new RO_Planning_Attributes_Total_Write_def();
            public class RO_Planning_Attributes_Total_Write_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Planning_Attributes_Total_Write.SQL"

                private tableParameter dt;

                public RO_Planning_Attributes_Total_Write_def()
                {
                    base.procedureName = "RO_Planning_Attributes_Total_Write";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RO_Planning_Attributes_Total_Write");
                    dt = new tableParameter("@dt", "Planning_Attributes_Total_Type", base.inputParameterList);
                }

                [UnitTestMethodAttribute(BypassValidation = true)]
                public int Insert(DatabaseAccess _dba,
                                  DataTable dt
                                  )
                {
                    lock (typeof(RO_Planning_Attributes_Total_Write_def))
                    {
                        this.dt.SetValue(dt);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static RO_Extract_Session_Read_def RO_Extract_Session_Read = new RO_Extract_Session_Read_def();
            public class RO_Extract_Session_Read_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Extract_Session_Read.SQL"

                private longParameter SessionKey;

                public RO_Extract_Session_Read_def()
                {
                    base.procedureName = "RO_Extract_Session_Read";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("Extract_Session");
                    SessionKey = new longParameter("@SessionKey", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, long? SessionKey)
                {
                    lock (typeof(RO_Extract_Session_Read_def))
                    {
                        this.SessionKey.SetValue(SessionKey);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static RO_Extract_Session_Read_All_def RO_Extract_Session_Read_All = new RO_Extract_Session_Read_All_def();
            public class RO_Extract_Session_Read_All_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Extract_Session_Read_All.SQL"


                public RO_Extract_Session_Read_All_def()
                {
                    base.procedureName = "RO_Extract_Session_Read_All";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("Extract_Session");
                }

                public DataTable Read(DatabaseAccess _dba)
                {
                    lock (typeof(RO_Extract_Session_Read_All_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static RO_Extract_Session_Active_def RO_Extract_Session_Active = new RO_Extract_Session_Active_def();
            public class RO_Extract_Session_Active_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Extract_Session_Active.SQL"

                private intParameter ActiveSession; //Declare Output Parameter

                public RO_Extract_Session_Active_def()
                {
                    base.procedureName = "RO_Extract_Session_Active";
                    base.procedureType = storedProcedureTypes.OutputOnly;
                    ActiveSession = new intParameter("@ActiveSession", base.outputParameterList); //Add Output Parameter
                }

                public void GetOutput(DatabaseAccess _dba, ref int activeSession)
                {
                    lock (typeof(RO_Extract_Session_Active_def))
                    {
                        this.ActiveSession.SetValue(null); //Initialize Output Parameter
                        ExecuteStoredProcedureForRead(_dba);
                        activeSession = Convert.ToInt32(this.ActiveSession.Value);
                    }
                }
            }

            public static RO_Extract_Session_Write_def RO_Extract_Session_Write = new RO_Extract_Session_Write_def();
            public class RO_Extract_Session_Write_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Extract_Session_Write.SQL"

                private longParameter SessionKey;
                private charParameter SessionActiveInd;
                private datetimeParameter SessionStartDateTime;
                private datetimeParameter SessionEndDateTime;

                public RO_Extract_Session_Write_def()
                {
                    base.procedureName = "RO_Extract_Session_Write";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("RO_Extract_Session_Write");
                    SessionKey = new longParameter("@SessionKey", base.inputParameterList);
                    SessionActiveInd = new charParameter("@SessionActiveInd", base.inputParameterList);
                    SessionStartDateTime = new datetimeParameter("@SessionStartDateTime", base.inputParameterList);
                    SessionEndDateTime = new datetimeParameter("@SessionEndDateTime", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  long? SessionKey,
                                  char? SessionActiveInd,
                                  DateTime? SessionStartDateTime,
                                  DateTime? SessionEndDateTime
                                  )
                {
                    lock (typeof(RO_Extract_Session_Write_def))
                    {
                        this.SessionKey.SetValue(SessionKey);
                        this.SessionActiveInd.SetValue(SessionActiveInd);
                        this.SessionStartDateTime.SetValue(SessionStartDateTime);
                        this.SessionEndDateTime.SetValue(SessionEndDateTime);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

            public static RO_Extract_Session_Update_All_For_Close_def RO_Extract_Session_Update_All_For_Close = new RO_Extract_Session_Update_All_For_Close_def();
            public class RO_Extract_Session_Update_All_For_Close_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\RO_Extract_Session_Update_All_For_Close.SQL"

                private datetimeParameter SessionEndDateTime;

                public RO_Extract_Session_Update_All_For_Close_def()
                {
                    base.procedureName = "RO_Extract_Session_Update_All_For_Close";
                    base.procedureType = storedProcedureTypes.Update;
                    base.tableNames.Add("Extract_Session");
                    SessionEndDateTime = new datetimeParameter("@SessionEndDateTime", base.inputParameterList);
                }

                public int Update(DatabaseAccess _dba, DateTime? sessionEndDateTime)
                {
                    lock (typeof(RO_Extract_Session_Update_All_For_Close_def))
                    {
                        this.SessionEndDateTime.SetValue(sessionEndDateTime);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
                }
            }

            public static MID_TABLE_READ_SCHEMA_def MID_TABLE_READ_SCHEMA = new MID_TABLE_READ_SCHEMA_def();
            public class MID_TABLE_READ_SCHEMA_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TABLE_READ_SCHEMA.SQL"

                private stringParameter TABLE_NAME;

                public MID_TABLE_READ_SCHEMA_def()
                {
                    base.procedureName = "MID_TABLE_READ_SCHEMA";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("");
                    TABLE_NAME = new stringParameter("@TABLE_NAME", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, string TABLE_NAME)
                {
                    lock (typeof(MID_TABLE_READ_SCHEMA_def))
                    {
                        this.TABLE_NAME.SetValue(TABLE_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            //INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
