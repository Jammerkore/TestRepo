using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class MessagingData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_MESSAGE_QUEUE_DELETE_def MID_MESSAGE_QUEUE_DELETE = new MID_MESSAGE_QUEUE_DELETE_def();
			public class MID_MESSAGE_QUEUE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MESSAGE_QUEUE_DELETE.SQL"

			    private intParameter MESSAGE_RID;
			
			    public MID_MESSAGE_QUEUE_DELETE_def()
			    {
			        base.procedureName = "MID_MESSAGE_QUEUE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("MESSAGE_QUEUE");
			        MESSAGE_RID = new intParameter("@MESSAGE_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? MESSAGE_RID)
			    {
                    lock (typeof(MID_MESSAGE_QUEUE_DELETE_def))
                    {
                        this.MESSAGE_RID.SetValue(MESSAGE_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

            public static SP_MID_MESSAGE_QUEUE_INSERT_def SP_MID_MESSAGE_QUEUE_INSERT = new SP_MID_MESSAGE_QUEUE_INSERT_def();
            public class SP_MID_MESSAGE_QUEUE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_MESSAGE_QUEUE_INSERT.SQL"

                private intParameter MESSAGE_TO;
                private intParameter MESSAGE_FROM;
                private intParameter MESSAGE_CODE;
                private intParameter MESSAGE_PROCESSING_PRIORITY;
                private stringParameter MESSAGE_DETAILS;
                private intParameter MESSAGE_RID; //Declare Output Parameter

                public SP_MID_MESSAGE_QUEUE_INSERT_def()
                {
                    base.procedureName = "SP_MID_MESSAGE_QUEUE_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("MESSAGE_QUEUE");
                    MESSAGE_TO = new intParameter("@MESSAGE_TO", base.inputParameterList);
                    MESSAGE_FROM = new intParameter("@MESSAGE_FROM", base.inputParameterList);
                    MESSAGE_CODE = new intParameter("@MESSAGE_CODE", base.inputParameterList);
                    MESSAGE_PROCESSING_PRIORITY = new intParameter("@MESSAGE_PROCESSING_PRIORITY", base.inputParameterList);
                    MESSAGE_DETAILS = new stringParameter("@MESSAGE_DETAILS", base.inputParameterList);
                    MESSAGE_RID = new intParameter("@MESSAGE_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              int? MESSAGE_TO,
                                              int? MESSAGE_FROM,
                                              int? MESSAGE_CODE,
                                              int? MESSAGE_PROCESSING_PRIORITY,
                                              string MESSAGE_DETAILS
                                              )
                {
                    lock (typeof(SP_MID_MESSAGE_QUEUE_INSERT_def))
                    {
                        this.MESSAGE_TO.SetValue(MESSAGE_TO);
                        this.MESSAGE_FROM.SetValue(MESSAGE_FROM);
                        this.MESSAGE_CODE.SetValue(MESSAGE_CODE);
                        this.MESSAGE_PROCESSING_PRIORITY.SetValue(MESSAGE_PROCESSING_PRIORITY);
                        this.MESSAGE_DETAILS.SetValue(MESSAGE_DETAILS);
                        this.MESSAGE_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }


			public static MID_MESSAGE_QUEUE_READ_def MID_MESSAGE_QUEUE_READ = new MID_MESSAGE_QUEUE_READ_def();
			public class MID_MESSAGE_QUEUE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_MESSAGE_QUEUE_READ.SQL"

			    private intParameter MESSAGE_TO;
			
			    public MID_MESSAGE_QUEUE_READ_def()
			    {
			        base.procedureName = "MID_MESSAGE_QUEUE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("MESSAGE_QUEUE");
			        MESSAGE_TO = new intParameter("@MESSAGE_TO", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? MESSAGE_TO)
			    {
                    lock (typeof(MID_MESSAGE_QUEUE_READ_def))
                    {
                        this.MESSAGE_TO.SetValue(MESSAGE_TO);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
