using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class HierarchyReclassData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_HIERARCHY_RECLASS_TRANS_DELETE_def MID_HIERARCHY_RECLASS_TRANS_DELETE = new MID_HIERARCHY_RECLASS_TRANS_DELETE_def();
			public class MID_HIERARCHY_RECLASS_TRANS_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_RECLASS_TRANS_DELETE.SQL"

			    private intParameter TRANS_PROCESS_ID;
			
			    public MID_HIERARCHY_RECLASS_TRANS_DELETE_def()
			    {
			        base.procedureName = "MID_HIERARCHY_RECLASS_TRANS_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("HIERARCHY_RECLASS_TRANS");
			        TRANS_PROCESS_ID = new intParameter("@TRANS_PROCESS_ID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TRANS_PROCESS_ID)
			    {
                    lock (typeof(MID_HIERARCHY_RECLASS_TRANS_DELETE_def))
                    {
                        this.TRANS_PROCESS_ID.SetValue(TRANS_PROCESS_ID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_HIERARCHY_RECLASS_TRANS_INSERT_def MID_HIERARCHY_RECLASS_TRANS_INSERT = new MID_HIERARCHY_RECLASS_TRANS_INSERT_def();
			public class MID_HIERARCHY_RECLASS_TRANS_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_HIERARCHY_RECLASS_TRANS_INSERT.SQL"

			    private intParameter TRANS_PROCESS_ID;
			    private intParameter TRANS_SEQUENCE;
			    private stringParameter TRANS_ORIGINAL;
			    private stringParameter TRANS_HIERARCHY_ID;
			    private stringParameter TRANS_PARENT_ID;
			    private stringParameter TRANS_PRODUCT_ID;
			    private stringParameter TRANS_PRODUCT_NAME;
			    private stringParameter TRANS_PRODUCT_DESC;
			
			    public MID_HIERARCHY_RECLASS_TRANS_INSERT_def()
			    {
			        base.procedureName = "MID_HIERARCHY_RECLASS_TRANS_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("HIERARCHY_RECLASS_TRANS");
			        TRANS_PROCESS_ID = new intParameter("@TRANS_PROCESS_ID", base.inputParameterList);
			        TRANS_SEQUENCE = new intParameter("@TRANS_SEQUENCE", base.inputParameterList);
			        TRANS_ORIGINAL = new stringParameter("@TRANS_ORIGINAL", base.inputParameterList);
			        TRANS_HIERARCHY_ID = new stringParameter("@TRANS_HIERARCHY_ID", base.inputParameterList);
			        TRANS_PARENT_ID = new stringParameter("@TRANS_PARENT_ID", base.inputParameterList);
			        TRANS_PRODUCT_ID = new stringParameter("@TRANS_PRODUCT_ID", base.inputParameterList);
			        TRANS_PRODUCT_NAME = new stringParameter("@TRANS_PRODUCT_NAME", base.inputParameterList);
			        TRANS_PRODUCT_DESC = new stringParameter("@TRANS_PRODUCT_DESC", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TRANS_PROCESS_ID,
			                      int? TRANS_SEQUENCE,
			                      string TRANS_ORIGINAL,
			                      string TRANS_HIERARCHY_ID,
			                      string TRANS_PARENT_ID,
			                      string TRANS_PRODUCT_ID,
			                      string TRANS_PRODUCT_NAME,
			                      string TRANS_PRODUCT_DESC
			                      )
			    {
                    lock (typeof(MID_HIERARCHY_RECLASS_TRANS_INSERT_def))
                    {
                        this.TRANS_PROCESS_ID.SetValue(TRANS_PROCESS_ID);
                        this.TRANS_SEQUENCE.SetValue(TRANS_SEQUENCE);
                        this.TRANS_ORIGINAL.SetValue(TRANS_ORIGINAL);
                        this.TRANS_HIERARCHY_ID.SetValue(TRANS_HIERARCHY_ID);
                        this.TRANS_PARENT_ID.SetValue(TRANS_PARENT_ID);
                        this.TRANS_PRODUCT_ID.SetValue(TRANS_PRODUCT_ID);
                        this.TRANS_PRODUCT_NAME.SetValue(TRANS_PRODUCT_NAME);
                        this.TRANS_PRODUCT_DESC.SetValue(TRANS_PRODUCT_DESC);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            public static SP_MID_HIER_RECLASS_PROCESS_def SP_MID_HIER_RECLASS_PROCESS = new SP_MID_HIER_RECLASS_PROCESS_def();
			public class SP_MID_HIER_RECLASS_PROCESS_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_HIER_RECLASS_PROCESS.SQL"

                private intParameter ProcessId;
                private stringParameter HierarchyId;

                public SP_MID_HIER_RECLASS_PROCESS_def()
			    {
			        base.procedureName = "SP_MID_HIER_RECLASS_PROCESS";
			        base.procedureType = storedProcedureTypes.ReadAsDataset;
                    base.tableNames.Add("HIERARCHY_RECLASS_TRANS");
                    ProcessId = new intParameter("@ProcessId", base.inputParameterList);
                    HierarchyId = new stringParameter("@HierarchyId", base.inputParameterList);
			    }
			
			    public DataSet ReadAsDataSet(DatabaseAccess _dba,
                                             int? ProcessId,
                                             string HierarchyId
			                                 )
			    {
                    lock (typeof(SP_MID_HIER_RECLASS_PROCESS_def))
                    {
                        this.ProcessId.SetValue(ProcessId);
                        this.HierarchyId.SetValue(HierarchyId);
                        DataSet ds = ExecuteStoredProcedureForReadAsDataSet(_dba);
                        ds.DataSetName = "OutTrans";
                        return ds;
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
