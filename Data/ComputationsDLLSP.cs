using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class ComputationsDLLData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_COMPUTATIONS_DLL_READ_COUNT_def MID_COMPUTATIONS_DLL_READ_COUNT = new MID_COMPUTATIONS_DLL_READ_COUNT_def();
			public class MID_COMPUTATIONS_DLL_READ_COUNT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATIONS_DLL_READ_COUNT.SQL"

			
			    public MID_COMPUTATIONS_DLL_READ_COUNT_def()
			    {
			        base.procedureName = "MID_COMPUTATIONS_DLL_READ_COUNT";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("COMPUTATIONS_DLL");
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_COMPUTATIONS_DLL_READ_COUNT_def))
                    {
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_COMPUTATIONS_DLL_READ_COUNT_FROM_VERSION_def MID_COMPUTATIONS_DLL_READ_COUNT_FROM_VERSION = new MID_COMPUTATIONS_DLL_READ_COUNT_FROM_VERSION_def();
			public class MID_COMPUTATIONS_DLL_READ_COUNT_FROM_VERSION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATIONS_DLL_READ_COUNT_FROM_VERSION.SQL"

			    public stringParameter VERSION;
			
			    public MID_COMPUTATIONS_DLL_READ_COUNT_FROM_VERSION_def()
			    {
			        base.procedureName = "MID_COMPUTATIONS_DLL_READ_COUNT_FROM_VERSION";
			        base.procedureType = storedProcedureTypes.RecordCount;
			        base.tableNames.Add("COMPUTATIONS_DLL");
			        VERSION = new stringParameter("@VERSION", base.inputParameterList);
			    }
			
			    public int ReadRecordCount(DatabaseAccess _dba, string VERSION)
			    {
                    lock (typeof(MID_COMPUTATIONS_DLL_READ_COUNT_FROM_VERSION_def))
                    {
                        this.VERSION.SetValue(VERSION);
                        return ExecuteStoredProcedureForRecordCount(_dba);
                    }
			    }
			}

			public static MID_COMPUTATIONS_DLL_READ_LATEST_VERSION_def MID_COMPUTATIONS_DLL_READ_LATEST_VERSION = new MID_COMPUTATIONS_DLL_READ_LATEST_VERSION_def();
			public class MID_COMPUTATIONS_DLL_READ_LATEST_VERSION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATIONS_DLL_READ_LATEST_VERSION.SQL"

			
			    public MID_COMPUTATIONS_DLL_READ_LATEST_VERSION_def()
			    {
			        base.procedureName = "MID_COMPUTATIONS_DLL_READ_LATEST_VERSION";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("COMPUTATIONS_DLL");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_COMPUTATIONS_DLL_READ_LATEST_VERSION_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_COMPUTATIONS_DLL_READ_LATEST_IMAGE_def MID_COMPUTATIONS_DLL_READ_LATEST_IMAGE = new MID_COMPUTATIONS_DLL_READ_LATEST_IMAGE_def();
			public class MID_COMPUTATIONS_DLL_READ_LATEST_IMAGE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATIONS_DLL_READ_LATEST_IMAGE.SQL"

			
			    public MID_COMPUTATIONS_DLL_READ_LATEST_IMAGE_def()
			    {
			        base.procedureName = "MID_COMPUTATIONS_DLL_READ_LATEST_IMAGE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("COMPUTATIONS_DLL");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_COMPUTATIONS_DLL_READ_LATEST_IMAGE_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_COMPUTATIONS_DLL_READ_IMAGE_FROM_VERSION_def MID_COMPUTATIONS_DLL_READ_IMAGE_FROM_VERSION = new MID_COMPUTATIONS_DLL_READ_IMAGE_FROM_VERSION_def();
			public class MID_COMPUTATIONS_DLL_READ_IMAGE_FROM_VERSION_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATIONS_DLL_READ_IMAGE_FROM_VERSION.SQL"

			    public stringParameter VERSION;
			
			    public MID_COMPUTATIONS_DLL_READ_IMAGE_FROM_VERSION_def()
			    {
			        base.procedureName = "MID_COMPUTATIONS_DLL_READ_IMAGE_FROM_VERSION";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("COMPUTATIONS_DLL");
			        VERSION = new stringParameter("@VERSION", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string VERSION)
			    {
                    lock (typeof(MID_COMPUTATIONS_DLL_READ_IMAGE_FROM_VERSION_def))
                    {
                        this.VERSION.SetValue(VERSION);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_COMPUTATIONS_DLL_INSERT_def MID_COMPUTATIONS_DLL_INSERT = new MID_COMPUTATIONS_DLL_INSERT_def();
			public class MID_COMPUTATIONS_DLL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATIONS_DLL_INSERT.SQL"

			    private stringParameter VERSION;
			    private byteArrayParameter DLL_IMAGE;
			    private datetimeParameter CREATION_DATE;
			
			    public MID_COMPUTATIONS_DLL_INSERT_def()
			    {
			        base.procedureName = "MID_COMPUTATIONS_DLL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("COMPUTATIONS_DLL");
			        VERSION = new stringParameter("@VERSION", base.inputParameterList);
                    DLL_IMAGE = new byteArrayParameter("@DLL_IMAGE", base.inputParameterList);
			        CREATION_DATE = new datetimeParameter("@CREATION_DATE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      string VERSION,
			                      byte[] DLL_IMAGE,
			                      DateTime? CREATION_DATE
			                      )
			    {
                    lock (typeof(MID_COMPUTATIONS_DLL_INSERT_def))
                    {
                        this.VERSION.SetValue(VERSION);
                        this.DLL_IMAGE.SetValue(DLL_IMAGE);
                        this.CREATION_DATE.SetValue(CREATION_DATE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_COMPUTATIONS_DLL_READ_DEFAULT_CONSTRAINT_def MID_COMPUTATIONS_DLL_READ_DEFAULT_CONSTRAINT = new MID_COMPUTATIONS_DLL_READ_DEFAULT_CONSTRAINT_def();
			public class MID_COMPUTATIONS_DLL_READ_DEFAULT_CONSTRAINT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_COMPUTATIONS_DLL_READ_DEFAULT_CONSTRAINT.SQL"

			    private stringParameter TABLE_NAME;
			    private stringParameter COLUMN_NAME;
			
			    public MID_COMPUTATIONS_DLL_READ_DEFAULT_CONSTRAINT_def()
			    {
			        base.procedureName = "MID_COMPUTATIONS_DLL_READ_DEFAULT_CONSTRAINT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("COMPUTATIONS_DLL");
			        TABLE_NAME = new stringParameter("@TABLE_NAME", base.inputParameterList);
			        COLUMN_NAME = new stringParameter("@COLUMN_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          string TABLE_NAME,
			                          string COLUMN_NAME
			                          )
			    {
                    lock (typeof(MID_COMPUTATIONS_DLL_READ_DEFAULT_CONSTRAINT_def))
                    {
                        this.TABLE_NAME.SetValue(TABLE_NAME);
                        this.COLUMN_NAME.SetValue(COLUMN_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}
		
			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
