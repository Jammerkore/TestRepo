using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class ForecastVersion : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_FORECAST_VERSION_DELETE_def MID_FORECAST_VERSION_DELETE = new MID_FORECAST_VERSION_DELETE_def();
			public class MID_FORECAST_VERSION_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_VERSION_DELETE.SQL"

			    private intParameter FV_RID;
			
			    public MID_FORECAST_VERSION_DELETE_def()
			    {
			        base.procedureName = "MID_FORECAST_VERSION_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("FORECAST_VERSION");
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? FV_RID)
			    {
                    lock (typeof(MID_FORECAST_VERSION_DELETE_def))
                    {
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_FORECAST_VERSION_UPDATE_def MID_FORECAST_VERSION_UPDATE = new MID_FORECAST_VERSION_UPDATE_def();
			public class MID_FORECAST_VERSION_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_VERSION_UPDATE.SQL"

			    private intParameter FV_RID;
			    private charParameter FV_ID;
			    private stringParameter DESCRIPTION;
			    private charParameter PROTECT_HISTORY_IND;
			    private charParameter ACTIVE_IND;
			    private intParameter BLEND_TYPE;
			    private intParameter ACTUAL_FV_RID;
			    private intParameter FORECAST_FV_RID;
			    private charParameter CURRENT_BLEND_IND;
			    private charParameter SIMILAR_STORE_IND;
			
			    public MID_FORECAST_VERSION_UPDATE_def()
			    {
			        base.procedureName = "MID_FORECAST_VERSION_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("FORECAST_VERSION");
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        FV_ID = new charParameter("@FV_ID", base.inputParameterList);
			        DESCRIPTION = new stringParameter("@DESCRIPTION", base.inputParameterList);
			        PROTECT_HISTORY_IND = new charParameter("@PROTECT_HISTORY_IND", base.inputParameterList);
			        ACTIVE_IND = new charParameter("@ACTIVE_IND", base.inputParameterList);
			        BLEND_TYPE = new intParameter("@BLEND_TYPE", base.inputParameterList);
			        ACTUAL_FV_RID = new intParameter("@ACTUAL_FV_RID", base.inputParameterList);
			        FORECAST_FV_RID = new intParameter("@FORECAST_FV_RID", base.inputParameterList);
			        CURRENT_BLEND_IND = new charParameter("@CURRENT_BLEND_IND", base.inputParameterList);
			        SIMILAR_STORE_IND = new charParameter("@SIMILAR_STORE_IND", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? FV_RID,
			                      char? FV_ID,
			                      string DESCRIPTION,
			                      char? PROTECT_HISTORY_IND,
			                      char? ACTIVE_IND,
			                      int? BLEND_TYPE,
			                      int? ACTUAL_FV_RID,
			                      int? FORECAST_FV_RID,
			                      char? CURRENT_BLEND_IND,
			                      char? SIMILAR_STORE_IND
			                      )
			    {
                    lock (typeof(MID_FORECAST_VERSION_UPDATE_def))
                    {
                        this.FV_RID.SetValue(FV_RID);
                        this.FV_ID.SetValue(FV_ID);
                        this.DESCRIPTION.SetValue(DESCRIPTION);
                        this.PROTECT_HISTORY_IND.SetValue(PROTECT_HISTORY_IND);
                        this.ACTIVE_IND.SetValue(ACTIVE_IND);
                        this.BLEND_TYPE.SetValue(BLEND_TYPE);
                        this.ACTUAL_FV_RID.SetValue(ACTUAL_FV_RID);
                        this.FORECAST_FV_RID.SetValue(FORECAST_FV_RID);
                        this.CURRENT_BLEND_IND.SetValue(CURRENT_BLEND_IND);
                        this.SIMILAR_STORE_IND.SetValue(SIMILAR_STORE_IND);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_FORECAST_VERSION_READ_def MID_FORECAST_VERSION_READ = new MID_FORECAST_VERSION_READ_def();
			public class MID_FORECAST_VERSION_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_VERSION_READ.SQL"

			    private intParameter FV_RID;
			
			    public MID_FORECAST_VERSION_READ_def()
			    {
			        base.procedureName = "MID_FORECAST_VERSION_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_VERSION");
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? FV_RID)
			    {
                    lock (typeof(MID_FORECAST_VERSION_READ_def))
                    {
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_FORECAST_VERSION_INSERT_def SP_MID_FORECAST_VERSION_INSERT = new SP_MID_FORECAST_VERSION_INSERT_def();
            public class SP_MID_FORECAST_VERSION_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_FORECAST_VERSION_INSERT.SQL"

                private charParameter FV_ID;
                private stringParameter DESCRIPTION;
                private charParameter PROTECT_HISTORY_IND;
                private charParameter ACTIVE_IND;
                private intParameter BLEND_TYPE;
                private intParameter ACTUAL_FV_RID;
                private intParameter FORECAST_FV_RID;
                private charParameter CURRENT_BLEND_IND;
                private charParameter SIMILAR_STORE_IND;
                private intParameter FV_RID; //Declare Output Parameter

                public SP_MID_FORECAST_VERSION_INSERT_def()
                {
                    base.procedureName = "SP_MID_FORECAST_VERSION_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("FORECAST_VERSION");
                    FV_ID = new charParameter("@FV_ID", base.inputParameterList);
                    DESCRIPTION = new stringParameter("@DESCRIPTION", base.inputParameterList);
                    PROTECT_HISTORY_IND = new charParameter("@PROTECT_HISTORY_IND", base.inputParameterList);
                    ACTIVE_IND = new charParameter("@ACTIVE_IND", base.inputParameterList);
                    BLEND_TYPE = new intParameter("@BLEND_TYPE", base.inputParameterList);
                    ACTUAL_FV_RID = new intParameter("@ACTUAL_FV_RID", base.inputParameterList);
                    FORECAST_FV_RID = new intParameter("@FORECAST_FV_RID", base.inputParameterList);
                    CURRENT_BLEND_IND = new charParameter("@CURRENT_BLEND_IND", base.inputParameterList);
                    SIMILAR_STORE_IND = new charParameter("@SIMILAR_STORE_IND", base.inputParameterList);
                    FV_RID = new intParameter("@FV_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              char? FV_ID,
                                              string DESCRIPTION,
                                              char? PROTECT_HISTORY_IND,
                                              char? ACTIVE_IND,
                                              int? BLEND_TYPE,
                                              int? ACTUAL_FV_RID,
                                              int? FORECAST_FV_RID,
                                              char? CURRENT_BLEND_IND,
                                              char? SIMILAR_STORE_IND
                                              )
                {
                    lock (typeof(SP_MID_FORECAST_VERSION_INSERT_def))
                    {
                        this.FV_ID.SetValue(FV_ID);
                        this.DESCRIPTION.SetValue(DESCRIPTION);
                        this.PROTECT_HISTORY_IND.SetValue(PROTECT_HISTORY_IND);
                        this.ACTIVE_IND.SetValue(ACTIVE_IND);
                        this.BLEND_TYPE.SetValue(BLEND_TYPE);
                        this.ACTUAL_FV_RID.SetValue(ACTUAL_FV_RID);
                        this.FORECAST_FV_RID.SetValue(FORECAST_FV_RID);
                        this.CURRENT_BLEND_IND.SetValue(CURRENT_BLEND_IND);
                        this.SIMILAR_STORE_IND.SetValue(SIMILAR_STORE_IND);
                        this.FV_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }

			public static MID_FORECAST_VERSION_READ_ALL_def MID_FORECAST_VERSION_READ_ALL = new MID_FORECAST_VERSION_READ_ALL_def();
			public class MID_FORECAST_VERSION_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_VERSION_READ_ALL.SQL"

			
			    public MID_FORECAST_VERSION_READ_ALL_def()
			    {
			        base.procedureName = "MID_FORECAST_VERSION_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_VERSION");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_FORECAST_VERSION_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_VERSION_READ_ALL_ACTIVE_def MID_FORECAST_VERSION_READ_ALL_ACTIVE = new MID_FORECAST_VERSION_READ_ALL_ACTIVE_def();
			public class MID_FORECAST_VERSION_READ_ALL_ACTIVE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_VERSION_READ_ALL_ACTIVE.SQL"

			
			    public MID_FORECAST_VERSION_READ_ALL_ACTIVE_def()
			    {
			        base.procedureName = "MID_FORECAST_VERSION_READ_ALL_ACTIVE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_VERSION");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_FORECAST_VERSION_READ_ALL_ACTIVE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_FORECAST_VERSION_READ_ALL_SORT_BY_RID_def MID_FORECAST_VERSION_READ_ALL_SORT_BY_RID = new MID_FORECAST_VERSION_READ_ALL_SORT_BY_RID_def();
			public class MID_FORECAST_VERSION_READ_ALL_SORT_BY_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_VERSION_READ_ALL_SORT_BY_RID.SQL"

			
			    public MID_FORECAST_VERSION_READ_ALL_SORT_BY_RID_def()
			    {
			        base.procedureName = "MID_FORECAST_VERSION_READ_ALL_SORT_BY_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("FORECAST_VERSION");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_FORECAST_VERSION_READ_ALL_SORT_BY_RID_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_FORECAST_VERSION_READ_DESCRIPTION_def MID_FORECAST_VERSION_READ_DESCRIPTION = new MID_FORECAST_VERSION_READ_DESCRIPTION_def();
            public class MID_FORECAST_VERSION_READ_DESCRIPTION_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_VERSION_READ_DESCRIPTION.SQL"

                private intParameter FV_RID;

                public MID_FORECAST_VERSION_READ_DESCRIPTION_def()
                {
                    base.procedureName = "MID_FORECAST_VERSION_READ_DESCRIPTION";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("FORECAST_VERSION");
                    FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? FV_RID)
                {
                    lock (typeof(MID_FORECAST_VERSION_READ_DESCRIPTION_def))
                    {
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

            public static MID_FORECAST_VERSION_READ_PROTECTED_def MID_FORECAST_VERSION_READ_PROTECTED = new MID_FORECAST_VERSION_READ_PROTECTED_def();
            public class MID_FORECAST_VERSION_READ_PROTECTED_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_FORECAST_VERSION_READ_PROTECTED.SQL"

                private intParameter FV_RID;

                public MID_FORECAST_VERSION_READ_PROTECTED_def()
                {
                    base.procedureName = "MID_FORECAST_VERSION_READ_PROTECTED";
                    base.procedureType = storedProcedureTypes.ScalarValue;
                    base.tableNames.Add("FORECAST_VERSION");
                    FV_RID = new intParameter("@FV_RID", base.inputParameterList);
                }

                public object ReadValue(DatabaseAccess _dba, int? FV_RID)
                {
                    lock (typeof(MID_FORECAST_VERSION_READ_PROTECTED_def))
                    {
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForScalarValue(_dba);
                    }
                }
            }

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
